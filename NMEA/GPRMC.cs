using System;
using System.Text;
using System.Windows.Forms;

namespace Trip_Simulator.NMEA
{
    // GPRMC: Recommended Minimum Specific GPS/TRANSIT Data

    public class GPRMC
    {
        public static string Name = "$GPRMC";
        public char NorthSouth;
        public char EastWest;
        public string[] fields;
        public DateTime Date;
        public decimal Longitude;
        public decimal Latitude;
        public decimal Speed;
        decimal lat = 0;
        decimal lon = 0;
        decimal South = 1;
        decimal West = 1;
        bool DAT = false;

        //*****************************************************************************************
        // Name: GPRMC(string)
        // Description: The constructor for when there is only a single string input. It splits the
        //              input and set the value and constraints to them. i.e. Setting the date and
        //              time formats, and setting constraints of how many decimals the coordinates
        //              should have, and etc.
        //-----------------------------------------------------------------------------------------
        // Inputs: line
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public GPRMC(string line, DateTimePicker SetTime)
        {
            string checksum;
            bool isValid;

            string[] message = line.Split('*');                     // Split message into message[0] and checksum into message[1]

            if (message.Length > 1)                                 // When a NMEA file appears
            {
                checksum = message[1];
                fields = message[0].Split(',');

                byte calcChecksum = 0;

                foreach (byte c in line.Substring(1))               // skip over leading '$' character
                {
                    if (c == '*') break;
                    calcChecksum ^= c;
                }

                isValid = (checksum == calcChecksum.ToString("X2"));
                if (!isValid) return;
                if (fields[0] != Name) return;                      // wrong message type, should be $GPRMC

                StringBuilder sb = new StringBuilder();

                if (SetTime == null)
                {
                    if (fields[1].Length > 0)                           // UTC time of fix
                    {
                        sb.Append(fields[1].Substring(0, 2));
                        sb.Append(fields[1].Substring(2, 2));
                        sb.Append(fields[1].Substring(4, 2));
                        sb.Append(".");
                        sb.Append(fields[1].Substring(7, 2));

                        fields[1] = sb.ToString();                      // Fix time
                        sb.Clear();
                    }
                }
                else
                {
                    sb.Append(SetTime.Value.Hour.ToString("D2"));
                    sb.Append(SetTime.Value.Minute.ToString("D2"));
                    sb.Append(SetTime.Value.Second.ToString("D2"));
                    sb.Append(".");
                    sb.Append(SetTime.Value.Millisecond.ToString());
                    fields[1] = sb.ToString();
                    sb.Clear();
                }

                decimal.TryParse(fields[3], out lat);
                if (fields[4].Contains("S")) South = -1;
                decimal.TryParse(fields[5], out lon);
                if (fields[6].Contains("W")) West = -1;

                decimal.TryParse(fields[7], out Speed);

                // Checking to see if we're using default current date/time.
                if (SetTime == null)
                {
                    if (fields[9].Length == 6)                      // UTC date of fix
                    {
                        int day, month, year;

                        int.TryParse(fields[9].Substring(0, 2), out day);
                        int.TryParse(fields[9].Substring(2, 2), out month);
                        int.TryParse(fields[9].Substring(4, 2), out year);
                        Date = new DateTime(2000 + year, month, day);

                        fields[9] = Date.ToString("ddMMyy");
                    }
                }
                // Using user selected date/time.
                else
                {
                    fields[9] = SetTime.Value.Date.ToString("ddMMyy");
                }
            }
            else                                                // When a DAT file appears
            {
                fields = message[0].Split(',');
                decimal.TryParse(fields[3], out lat);
                if (fields[4].Contains("S")) South = -1;
                decimal.TryParse(fields[5], out lon);
                if (fields[6].Contains("W")) West = -1;
                DAT = true;
            }
        }

        //*****************************************************************************************
        // Name: GPRMC(decimal, decimal, DateTime, double, bool)
        // Description: The constructor for when there are multiple inputs from the KML files the
        //              Trip Simulator reads. This constructor sets the format and constraints on
        //              the inputs it was given.
        //-----------------------------------------------------------------------------------------
        // Inputs: latitude, longitude, dateTime, mph, leap
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public GPRMC(decimal latitude, decimal longitude, DateTime dateTime, double mph, bool leap, DateTimePicker SetTime)
        {
            fields = new string[14];
            fields[0] = Name;

            // used for converting lat and long
            decimal integral;
            decimal fraction;

            // sample: $GPRMC,091010.00,A,4331.17768,N,07940.68471,W,0.000,,191213,,,A,*6E
            StringBuilder sb = new StringBuilder();

            // Checking to see if the leap timer has been activated.
            if (leap == true)
            {
                sb.Append(23);
                sb.Append(59);
                sb.Append(60);
                sb.Append(".");
                sb.Append(0);

                fields[1] = sb.ToString();     // Fix time
                sb.Clear();
            }
            // Checking to see if we're using default current date/time.
            else if (SetTime == null)
            {
                sb.Append(dateTime.Hour.ToString("D2"));
                sb.Append(dateTime.Minute.ToString("D2"));
                sb.Append(dateTime.Second.ToString("D2"));
                sb.Append(".");
                sb.Append(dateTime.Millisecond.ToString());

                fields[1] = sb.ToString();     // Fix time
                sb.Clear();
            }
            // Using user selected date/time.
            else
            {
                sb.Append(SetTime.Value.Hour.ToString("D2"));
                sb.Append(SetTime.Value.Minute.ToString("D2"));
                sb.Append(SetTime.Value.Second.ToString("D2"));
                sb.Append(".");
                sb.Append(SetTime.Value.Millisecond.ToString());
                fields[1] = sb.ToString();
                sb.Clear();
            }

            fields[2] = "A";               // Status A=active or V=Void

            if (latitude < 0)
            {
                NorthSouth = 'S';
                latitude *= -1;
            }
            else NorthSouth = 'N';

            integral = decimal.Truncate(latitude);
            fraction = latitude - integral;
            sb.Append(integral.ToString("00")); // Degree
            sb.Append((fraction * 60).ToString("00.00000"));           // Minute

            fields[3] = sb.ToString();
            sb.Clear();

            fields[4] = NorthSouth.ToString();

            if (longitude < 0)
            {
                EastWest = 'W';
                longitude *= -1;
            }
            else EastWest = 'E';

            integral = decimal.Truncate(longitude);
            fraction = longitude - integral;
            sb.Append(integral.ToString("000"));    // Degree
            sb.Append((fraction * 60).ToString("00.00000"));               // Minute

            fields[5] = sb.ToString();
            sb.Clear();

            fields[6] = EastWest.ToString();

            // convert to knots
            fields[7] = (mph * 0.86897699264).ToString("0.000");

            if (SetTime == null)
            {
                fields[9] = dateTime.ToString("ddMMyy");
            }
            else
            {
                fields[9] = SetTime.Value.Date.ToString("ddMMyy");
            }

            fields[12] = "S";              // signal integrity information: A=autonomous, D=differential, E=Estimated, N=not valid, S=Simulator.

        }

        //*****************************************************************************************
        // Name: toString()
        // Description: Overrides the system's ToString() and makes it so that when it's called, it
        //              will separate each of the strings appended to 'sb' with a ",". This also sets
        //              the value of the checksum too.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Join(",", fields));

            // calculate checksum
            byte checksum = 0;
            // skip over leading '$' character
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum ^= c;
            }
            sb.Append('*');
            sb.Append(checksum.ToString("X2"));
            return sb.ToString();
        }

        //*****************************************************************************************
        // Name: GetCurrentSpeed()
        // Description: Gets the current speed from the file its reading.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: Speed
        //*****************************************************************************************
        // 2015-10-19 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public decimal GetCurrentSpeed()
        {
            Speed = Speed / (decimal)0.8689769926;
            return Speed;
        }

        //*****************************************************************************************
        // Name: GetLat()
        // Description: Gets the current latitude and returns it. Used for the Highlight
        //              function to get the latitude from the file it's reading.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: lat
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public double GetLat()
        {
            if (!DAT)
            {
                decimal degrees;
                decimal.TryParse(fields[3].Substring(0, 2), out degrees);
                decimal minutes;
                decimal.TryParse(fields[3].Substring(2), out minutes);
                lat = degrees + (minutes / 60);
            }
                lat = lat * South;
            return (double)lat;
        }

        //*****************************************************************************************
        // Name: GetLon()
        // Description: Gets the current longitude and returns it. Used for the Highlight
        //              function to get the longitude from the file it's reading.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: lon
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public double GetLon()
        {
            if (!DAT)
            {
                decimal degrees;
                decimal.TryParse(fields[5].Substring(0, 3), out degrees);
                decimal minutes;
                decimal.TryParse(fields[5].Substring(3), out minutes);
                lon = degrees + (minutes / 60);
            }
                lon = lon * West;
            return (double)lon;
        }
    }
}