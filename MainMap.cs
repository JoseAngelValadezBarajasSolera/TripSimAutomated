using System;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;

namespace Trip_Simulator
{
    public class MainMap
    {
        MainForm TheMain;
        MainVehicle TheVehicle;
        DateTime elapsed;        
        double lat;
        double lon;
        decimal DATTime;
        decimal DATDate;

        NumericUpDown numRPM;
        NumericUpDown numOdometer;
        NumericUpDown numLoad;
        NumericUpDown numPTO;
        NumericUpDown numRate;
        NumericUpDown numFuel;
        CheckBox chkBrk;
        CheckBox chkPTO;
        CheckBox chkVehicle;
        TrackBar tbThrottle;
        ToolStripProgressBar tsProgress;
        Button ConnectButton;
        ListBox lstOutput;
        SaveFileDialog dlgRecord;
        OpenFileDialog dlgPlayback;
        StatusStrip statusStrip1;
        GroupBox grpSpdDist;
        GroupBox grpFuel;
        GroupBox grpPedals;
        ToolStripMenuItem mnuStopRecording;
        ToolStripMenuItem mnuRecordTo;
        ToolStripMenuItem mnuPlaybackFrom;
        ToolStripMenuItem mnuStopPlayback;
        BinaryWriter Writing = null;
        BinaryReader Reading = null;
        FileStream PBF = null;

        // Ports
        SerialPort NAVPort;
        SerialPort VehiclePort;

        // Constructor
        public MainMap(double TheLat, double TheLon)
        {
            lat = TheLat;
            lon = TheLon;
        }

        public void GetMain(MainForm main)
        {
            TheMain = main;
        }

        //*****************************************************************************************
        // Name: SetValues()
        // Description: Sets the values of all the necessary variables that are used in the
        //              MainMap.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void SetValues(NumericUpDown TheRPM, NumericUpDown TheOdometer, NumericUpDown TheLoad, NumericUpDown ThePTO,
            NumericUpDown TheRate, NumericUpDown TheFuel, CheckBox TheBrake, CheckBox CheckPTO, CheckBox CheckVehicle, TrackBar TheThrottle, ToolStripProgressBar TheProgress,
            Button TheButton, ListBox TheOutput, SaveFileDialog TheRecord, OpenFileDialog ThePlayback, StatusStrip TheStatus, GroupBox TheGrpSpeed,
            GroupBox TheGrpFuel, GroupBox TheGrpPedals, ToolStripMenuItem TheStopRecording, ToolStripMenuItem TheRecordTo, ToolStripMenuItem ThePlaybackFrom,
            ToolStripMenuItem TheStopPlayback, SerialPort VPort, BinaryWriter Write, BinaryReader Read, FileStream StreamF)
        {
            numRPM = TheRPM;
            numOdometer = TheOdometer;
            numLoad = TheLoad;
            numPTO = ThePTO;
            numRate = TheRate;
            numFuel = TheFuel;
            chkBrk = TheBrake;
            chkPTO = CheckPTO;
            chkVehicle = CheckVehicle;
            tbThrottle = TheThrottle;
            tsProgress = TheProgress;
            ConnectButton = TheButton;
            lstOutput = TheOutput;

            dlgRecord = TheRecord;
            dlgPlayback = ThePlayback;
            statusStrip1 = TheStatus;
            grpSpdDist = TheGrpSpeed;
            grpFuel = TheGrpFuel;
            grpPedals = TheGrpPedals;
            mnuStopRecording = TheStopRecording;
            mnuRecordTo = TheRecordTo;
            mnuPlaybackFrom = ThePlaybackFrom;
            mnuStopPlayback = TheStopPlayback;
            VehiclePort = VPort;
            Writing = Write;
            Reading = Read;
            PBF = StreamF;
        }

        //*****************************************************************************************
        // Name: UpdateCoordinates(double, double)
        // Description: Updates the coordinates by combining latitude and longitude together to
        //              make a single Location type. Then return the Location type.
        //-----------------------------------------------------------------------------------------
        // Inputs: lat, lon
        // Outputs: none
        // Returns: loc
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public Microsoft.Maps.MapControl.WPF.Location UpdateCoordinates(double lat, double lon)
        {
            Microsoft.Maps.MapControl.WPF.Location loc = new Microsoft.Maps.MapControl.WPF.Location(lat, lon);
            return loc;
        }

        //*****************************************************************************************
        // Name: NMEASetCoordinates(string)
        // Description: Takes in the NMEA sentence and set the coordinates so that it's in the
        //              format that Bing map uses.
        //-----------------------------------------------------------------------------------------
        // Inputs: sentence
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void NMEASetCoordinates(string sentence)
        {
            decimal Latitude = 0;
            decimal Longitude = 0;

            string[] fields = sentence.Split(',');

            if (fields[3].Length > 3)                           // Latitude of fix
            {
                // 4717.11399 = 47 + (17.11399 / 60) = 47.28523316666667
                decimal degrees;
                decimal.TryParse(fields[3].Substring(0, 2), out degrees);
                decimal minutes;
                decimal.TryParse(fields[3].Substring(2), out minutes);
                Latitude = degrees + (minutes / 60);
            };

            if (Latitude == 0) fields[3] = "0.00000";
            else fields[3] = Latitude.ToString("0000.#####");

            if (fields[5].Length > 4)                           // Longitude of fix
            {
                decimal degrees;
                decimal.TryParse(fields[5].Substring(0, 3), out degrees);
                decimal minutes;
                decimal.TryParse(fields[5].Substring(3), out minutes);
                Longitude = degrees + (minutes / 60);
            };

            if (Longitude == 0) fields[5] = "0.00000";
            else fields[5] = Longitude.ToString("00000.#####");

            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(",", fields));

            SetLatLon(sb.ToString());
        }

        //*****************************************************************************************
        // Name: SetSpeed(string)
        // Description: Takes in the GPRMC sentence from the DAT file and divide the value of its
        //              speed by 10. We need to do this since the speed provided by the DAT file
        //              are greater by a factor of times 10. It also sets the coordinates so it's
        //              in the correct NMEA sentence format.
        //-----------------------------------------------------------------------------------------
        // Inputs: sentence
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-05 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-04-12 SC Yuen   Added parameter NoSpeed and updated the parameters when calling
        //                      TheVehicle.SetValues()
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen   Added new parameters NoOdo and NoTEH and replaced gpsSpeed
        //                      with SpeedUpDown and got rid of lblTimeAtSpeed
        //-----------------------------------------------------------------------------------------
        // 2017-05-11 SC Yuen   Added code to set the DATTime and DATDate
        //*****************************************************************************************
        public decimal SetSpeed(string sentence, SerialPort _serialPort, decimal speed, decimal SpeedUpDown,
            CheckBox chkVehicle, DateTimePicker SetTime, bool NoRPM, bool NoSpeed, bool NoOdo, bool NoTEH)
        {
            NAVPort = _serialPort;

            StringBuilder sb = new StringBuilder();
            decimal Latitude = 0;
            decimal Longitude = 0;

            // used for converting lat and long
            decimal integral;
            decimal fraction;

            string[] fields = sentence.Split(',');

            decimal.TryParse(fields[1], out DATTime);

            // Using user selected date/time.
            if (SetTime != null)
            {
                sb.Append(SetTime.Value.Hour.ToString("D2"));
                sb.Append(SetTime.Value.Minute.ToString("D2"));
                sb.Append(SetTime.Value.Second.ToString("D2"));
                sb.Append(".");
                sb.Append(SetTime.Value.Millisecond.ToString());
                fields[1] = sb.ToString();
                sb.Clear();
            }

            if (fields[3].Length > 3)                           // Latitude of fix
            {
                // 4717.11399 = 47 + (17.11399 / 60) = 47.28523316666667
                decimal.TryParse(fields[3], out Latitude);

                integral = decimal.Truncate(Latitude);
                fraction = Latitude - integral;
                sb.Append(integral.ToString("00")); // Degree
                sb.Append(fraction * 60);           // Minute

                if (Latitude == 0) fields[3] = "0.00000";
                else fields[3] = sb.ToString();
                sb.Clear();
            };

            if (fields[5].Length > 4)                           // Longitude of fix
            {
                decimal.TryParse(fields[5], out Longitude);

                integral = decimal.Truncate(Longitude);
                fraction = Longitude - integral;
                sb.Append(integral.ToString("000"));    // Degree
                sb.Append(fraction * 60);               // Minute

                if (Longitude == 0) fields[5] = "0.00000";
                else fields[5] = sb.ToString();
                sb.Clear();
            };

            decimal truckSpeed;
            decimal.TryParse(fields[7], out truckSpeed);
            truckSpeed = truckSpeed / 10;

            if (truckSpeed < 0 || truckSpeed > MainVehicle.MAX_SPEED)
            {
                truckSpeed = 0xFFFF;
            }

            SpeedUpDown = truckSpeed;

            if (Math.Abs(SpeedUpDown - speed) >= (decimal).1)
            {
                elapsed = new DateTime();
                speed = SpeedUpDown;
            }
            
            TheVehicle = TheMain.GetVehicle();

            if (chkVehicle.Checked) elapsed = TheVehicle.SetValues(elapsed, NoRPM, NoSpeed, NoOdo, NoTEH);

            if (truckSpeed != 0xFFFF)
            {
                truckSpeed = truckSpeed * (decimal)0.8689769926;
            }     

            fields[7] = truckSpeed.ToString("#.###");

            decimal.TryParse(fields[9], out DATDate);

            if (SetTime != null)
            {
                fields[9] = SetTime.Value.Date.ToString("ddMMyy");
            }

            sb.Append(string.Join(",", fields));

            TheSentence(sb.ToString(), NAVPort);

            if (truckSpeed != 0xFFFF)
            {
                truckSpeed = truckSpeed / (decimal)0.8689769926;
            }

            return truckSpeed;
        }

        //*****************************************************************************************
        // Name: GetDATTime()
        // Description: Gets DATTime.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: DATTime
        //*****************************************************************************************
        // 2017-05-11   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public decimal GetDATTime()
        {
            return DATTime;
        }

        //*****************************************************************************************
        // Name: GetDATDate()
        // Description: Gets DATDate.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: DATDate
        //*****************************************************************************************
        // 2017-05-11   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public decimal GetDATDate()
        {
            return DATDate;
        }

        //*****************************************************************************************
        // Name: GetElapsed()
        // Description: Gets elapsed.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: elapsed
        //*****************************************************************************************
        // 2015-11-13 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public DateTime GetElapsed()
        {
            return elapsed;
        }

        //*****************************************************************************************
        // Name: GGASetCoordinates(string)
        // Description: Takes in the GPGGA sentence and set its coordinates so that it's in the
        //              correct format of the NMEA sentences.
        //-----------------------------------------------------------------------------------------
        // Inputs: sentence
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-25   SC Yuen     Added new argumen GPSTimeOnly to not send in the coordinates
        //                          when GPSTimeOnly is true
        //*****************************************************************************************
        public void GGASetCoordinates(string sentence, DateTimePicker SetTime, bool GPSTimeOnly)
        {
            StringBuilder sb = new StringBuilder();
            decimal Latitude = 0;
            decimal Longitude = 0;

            // used for converting lat and long
            decimal integral;
            decimal fraction;

            string[] fields = sentence.Split(',');

            // Using user selected date/time.
            if (SetTime != null)
            {
                sb.Append(SetTime.Value.Hour.ToString("D2"));
                sb.Append(SetTime.Value.Minute.ToString("D2"));
                sb.Append(SetTime.Value.Second.ToString("D2"));
                sb.Append(".");
                sb.Append(SetTime.Value.Millisecond.ToString());
                fields[1] = sb.ToString();
                sb.Clear();
            }

            if (!GPSTimeOnly && fields[2].Length > 0)                           // Latitude of fix
            {
                // 4717.11399 = 47 + (17.11399 / 60) = 47.28523316666667
                decimal.TryParse(fields[2], out Latitude);

                integral = decimal.Truncate(Latitude);
                fraction = Latitude - integral;
                sb.Append(integral.ToString("00"));
                sb.Append(fraction * 60);
                if (Latitude == 0) fields[2] = "0.00000";
                else fields[2] = sb.ToString();
                sb.Clear();
            };

            if (!GPSTimeOnly && fields[4].Length > 0)                           // Longitude of fix
            {
                decimal.TryParse(fields[4], out Longitude);

                integral = decimal.Truncate(Longitude);
                fraction = Longitude - integral;
                sb.Append(integral.ToString("000"));
                sb.Append(fraction * 60);
                if (Longitude == 0) fields[4] = "0.00000";
                else fields[4] = sb.ToString();
                sb.Clear();
            };

            sb.Append(string.Join(",", fields));

            TheSentence(sb.ToString(), NAVPort);
        }

        //*****************************************************************************************
        // Name: PUBSetCoordinates(string)
        // Description: Takes in the PUBX00 sentence and set its coordinates so that it's in the
        //              correct format of the NMEA sentences.
        //-----------------------------------------------------------------------------------------
        // Inputs: sentence
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-05   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-25   SC Yuen     Added new argumen GPSTimeOnly to not send in the coordinates
        //                          when GPSTimeOnly is true
        //*****************************************************************************************
        public void PUBSetCoordinates(string sentence, DateTimePicker SetTime, bool GPSTimeOnly)
        {
            StringBuilder sb = new StringBuilder();
            decimal Latitude = 0;
            decimal Longitude = 0;

            // used for converting lat and long
            decimal integral;
            decimal fraction;

            string[] fields = sentence.Split(',');

            // Using user selected date/time.
            if (SetTime != null)
            {
                sb.Append(SetTime.Value.Hour.ToString("D2"));
                sb.Append(SetTime.Value.Minute.ToString("D2"));
                sb.Append(SetTime.Value.Second.ToString("D2"));
                sb.Append(".");
                sb.Append(SetTime.Value.Millisecond.ToString());
                fields[2] = sb.ToString();
                sb.Clear();
            }

            if (!GPSTimeOnly && fields[3].Length > 3)                           // Latitude of fix
            {
                // 4717.11399 = 47 + (17.11399 / 60) = 47.28523316666667
                decimal.TryParse(fields[3], out Latitude);

                integral = decimal.Truncate(Latitude);
                fraction = Latitude - integral;
                sb.Append(integral.ToString("00"));     // Degree
                sb.Append(fraction * 60);               // Minute

                if (Latitude == 0) fields[3] = "0.00000";
                else fields[3] = sb.ToString();
                sb.Clear();
            };

            if (!GPSTimeOnly && fields[5].Length > 4)                           // Longitude of fix
            {
                decimal.TryParse(fields[5], out Longitude);

                integral = decimal.Truncate(Longitude);
                fraction = Longitude - integral;
                sb.Append(integral.ToString("000"));    // Degree
                sb.Append(fraction * 60);               // Minute

                if (Longitude == 0) fields[5] = "0.00000";
                else fields[5] = sb.ToString();
                sb.Clear();
            };

            sb.Append(string.Join(",", fields));

            TheSentence(sb.ToString(), NAVPort);
        }

        //*****************************************************************************************
        // Name: SetLatLon(string)
        // Description: Takes in the NMEA sentences from NMEA or DAT files to get their latitudes
        //              and longitudes, and set lat and lon equal to them respectively. We use the
        //              lat and lon for marking the simulated vehicle's current location on the
        //              Bing map.
        //-----------------------------------------------------------------------------------------
        // Inputs: string sentence
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-01 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void SetLatLon(string sentence)
        {
            for (int i = 0; i < 2; i++)
            {
                int index = sentence.IndexOf(",", 0);
                string temp1 = sentence.Remove(0, index + 4);   // Getting rid of $PUBX,00,
                index = temp1.IndexOf(",", 0);
                string temp2 = temp1.Remove(0, index + 1);      // Getting rid of the UTC time.
                index = temp2.IndexOf(",", 0);
                if (i == 0)
                {
                    bool South = false;
                    if (temp2.Contains("S")) South = true;
                    temp1 = temp2.Remove(index);                // Getting rid of everything after the latitude.

                    // If South, then latitude is negative.
                    if (South)
                    {
                        double latSouth = double.Parse(temp1);
                        lat = latSouth * -1;
                    }
                    else lat = double.Parse(temp1);
                }
                else if (i == 1)
                {
                    bool West = false;
                    temp1 = temp2.Remove(0, index + 3);         // Getting rid of latitude,N/S,
                    index = temp1.IndexOf(",", 0);
                    if (temp1.Contains("W")) West = true;
                    temp2 = temp1.Remove(index);                // Getting rid of everything after longitude.

                    // If West, then longitude is negative.
                    if (West)
                    {
                        double lonWest = double.Parse(temp2);
                        lon = lonWest * -1;
                    }
                    else lon = double.Parse(temp2);
                }
            }
        }

        //*****************************************************************************************
        // Name: TheSentence(string)
        // Description: Prints each line of sentences out of the serial port.
        //-----------------------------------------------------------------------------------------
        // Inputs: string sentence
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2016-01-22 SC Yuen   Edits to code to remove constraints of needing NAV port selected
        //*****************************************************************************************
        public void TheSentence(string sentence, SerialPort _serialPort)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(sentence);

            // calculate checksum
            byte checksum = 0;
            // skip over leading '$' character
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum ^= c;
            }
            sb.Append('*');
            sb.Append(checksum.ToString("X2"));

            if (NAVPort.IsOpen) _serialPort.WriteLine(sb.ToString());
        }

        //*****************************************************************************************
        // Name: GetLat()
        // Description: Gets lat.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: lat
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public double GetLat()
        {
            return lat;
        }

        //*****************************************************************************************
        // Name: GetLon()
        // Description: Gets lon.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: lon
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public double GetLon()
        {
            return lon;
        }
    }
}
