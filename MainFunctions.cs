using Microsoft.Maps.MapControl.WPF;
using SharpKml.Base;
using SharpKml.Dom;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Media;
using Trip_Simulator.NMEA;

namespace Trip_Simulator
{
    public class MainFunctions
    {
        private const string TIME_FORMAT = "HH:mm:ss";
        public const string SPEED_FORMAT = "0.0";

        NumericUpDown SpeedUpDown;
        NumericUpDown numRPM;
        NumericUpDown numOdometer;
        NumericUpDown numLoad;
        NumericUpDown numPTO;
        NumericUpDown numRate;
        NumericUpDown numFuel;
        CheckBox chkBrk;
        RadioButton mRbtnBrake2Mph;
        RadioButton mRbtnBrake5Mph;
        CheckBox chkPTO;
        CheckBox chkVehicle;
        CheckBox chkLoopKml;
        TrackBar tbThrottle;
        ToolStripProgressBar tsProgress;
        Button ConnectButton;
        Button CameraLockButton;
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
        OpenFileDialog TheFileName;
        Label lblTimeAtSpeed;
        SaveFileDialog saveFileDialog1;
        Label CurrentTimeLabel;
        RadioButton rStart;
        CheckBox chkStop;
        DateTimePicker StartTime;
        DateTimePicker StopTime;
        Label theCountDown;
        RadioButton SetPlaybackTime;
        RadioButton None;
        DateTimePicker TempTime1 = new DateTimePicker();
        DateTimePicker TempTime2 = new DateTimePicker();
        DateTimePicker InitTime = new DateTimePicker();
        CheckBox chkNoRPM;
        CheckBox GPSTimeOnly;
        CheckBox chkNoSpeed;
        Label vehicleSpeed;
        CheckBox chkNoOdo;
        CheckBox chkNoTEH;
        NumericUpDown numEngineHours;
        Label FileLoaded;
        ComboBox VehicleComboBox;
        ComboBox NAVComboBox;
        CheckBox chkLoadFileResetECM;
        Label TimeStarted;
        ToolStripMenuItem mMapOnStartup;
        CheckBox chkSetRPM;
        CheckBox chkSetFuelRate;
        CheckBox chkSetEngineLoad;
        Button mapBtn;

        // NAV data and ports
        DATFileReader openDATFile;
        Route route;                // The route from reading a KML file.
        Route WPRoute;              // The route for plotting the highlighted route when reading a KML file.
        bool weAreReadingADATFile = false;
        int lineCount = 0;
        int NMEALineCount = 0;
        int numLine = 0;
        int index = 0;
        bool numCheck = false;
        string[] globalSentence = null;
        List<string> theList = new List<string> { };
        bool weAreProcessingAnNMEAFile = false;
        Stream stream = null;
        TextReader reader = null;
        double lat = 0;
        double lon = 0;
        bool startPos = true;
        bool done = false;
        bool HACCEnabled = false;
        decimal HACCValue = 0;
        GPRMC rmc;
        GPGGA gga;
        PUBX00 pub;
        double lastLat = 0;
        double lastLon = 0;
        Label currentCoordinates;
        string PinRMCSentence;
        string PinGGASentence;
        string PinPUBSentence;
        bool PinSet = false;
        Microsoft.Maps.MapControl.WPF.Location tempLoc;

        // Vehicle data
        static decimal HR_PER_1S = 0.200M / 60.0M / 60.0M;          // hr
        decimal speedMargin = (decimal).1;
        DateTime elapsed = new DateTime();
        decimal speed = 0;
        decimal tempSpeed = 0;
        BinaryReader J1708Playback = null;
        BinaryWriter Writing = null;
        BinaryReader Reading = null;
        FileStream PBF = null;

        // DAT vehicle data
        decimal TotFuel = 0;
        decimal PTOHours = 0;
        decimal TotEngineHours = 0;
        decimal PTOCheckBox = 0;
        List<uint> STD1List = new List<uint> { };
        List<uint> STD2List = new List<uint> { };
        uint[] globalSTD1Data = null;
        uint[] globalSTD2Data = null;
        int STD2FiveCount = 4;          // The count to print STD2 messages every 5 seconds
        int STD1Index = 0;
        int STD2Index = 0;
        int STD1Line = 0;
        int STD2Line = 0;
        int STD1Count = 0;
        int STD2Count = 0;
        int DATLines = 0;

        // Ports
        SerialPort NAVPort;
        SerialPort VehiclePort;

        // Objects/Misc
        MainForm TheMain;
        Map map = null;
        MainMap TheMap;
        MainVehicle TheVehicle = new MainVehicle();
        bool ReadReady = true;
        bool StartFlag = false;
        bool StartDone = false;
        bool InitialCurrentTime = true;
        bool initialStartTime = true;
        bool mapCreated = false;

        //*****************************************************************************************
        // Name: SetMain()
        // Description: Sets TheMain so that it doesn't create an infinite call between MainForm
        //              and MainFunctions when initializing TheMain.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void SetMain(MainForm main)
        {
            TheMain = main;
        }

        //*****************************************************************************************
        // Name: SetValues()
        // Description: Sets the values of all the necessary variables that are used in the
        //              MainFunctions.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2016-11-30 SC Yuen   Setting the new parameters of GPSTimeOnly and chkNoSpeed
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen   Added new parameters Label VehicleSpeed, CheckBox NoOdo,
        //                      CheckBox NoTEH, NumericUpDown TheEngineHour, Label TheFileLoaded,
        //                      ComboBox VehicleBox, ComboBox NAVBox
        //-----------------------------------------------------------------------------------------
        // 2017-05-10 SC Yuen   Added new parameters CheckBox ResetECM, Label TheTimeStarted
        //-----------------------------------------------------------------------------------------
        // 2017-06-20 SC Yuen   Added new parameters ToolStripMenuItem DisableMap,
        //                      ToolStripMenuItem EnableMap, CheckBox SetRPM, CheckBox SetRate,
        //                      CheckBox SetLoad, and Button MapBtn
        //-----------------------------------------------------------------------------------------
        // 2018-03-22 JC Anderson   Added option to loop over kml file coordinates
        //-----------------------------------------------------------------------------------------
        // 2020-06-10 B Bassing   Added the braking rate
        //*****************************************************************************************
        public void SetValues(NumericUpDown TheSpeed, NumericUpDown TheRPM, NumericUpDown TheOdometer,
            NumericUpDown TheLoad, NumericUpDown ThePTO, NumericUpDown TheRate, NumericUpDown TheFuel,
            CheckBox TheBrake, RadioButton rbtnBrake2Mph, RadioButton rbtnBrake5Mph, CheckBox CheckPTO,
            CheckBox CheckVehicle, TrackBar TheThrottle, ToolStripProgressBar TheProgress, Button TheButton,
            Button CamLockButton, ListBox TheOutput, SaveFileDialog TheRecord, OpenFileDialog ThePlayback,
            StatusStrip TheStatus, GroupBox TheGrpSpeed, GroupBox TheGrpFuel, GroupBox TheGrpPedals,
            ToolStripMenuItem TheStopRecording, ToolStripMenuItem TheRecordTo, ToolStripMenuItem ThePlaybackFrom,
            ToolStripMenuItem TheStopPlayback, SerialPort NPort, SerialPort VPort, OpenFileDialog FileName,
            Label LabelSpeed, SaveFileDialog TheSave, BinaryWriter Write, BinaryReader Read, FileStream StreamF,
            bool HACCChecked, decimal HACCNum, Label CurrentTime, RadioButton RadioStart, CheckBox CheckStop,
            DateTimePicker SetStartTime, DateTimePicker SetStopTime, Label countDown, bool StartPlaybackDone,
            RadioButton RadioSetPlaybackTime, RadioButton NoneButton, CheckBox NoRPM, CheckBox OnlyGPSTime,
            CheckBox NoSpeed, Label VehicleSpeed, CheckBox NoOdo, CheckBox NoTEH, NumericUpDown TheEngineHour,
            Label TheFileLoaded, ComboBox VehicleBox, ComboBox NAVBox, Label CurrentCoordinates, CheckBox ResetECM,
            Label TheTimeStarted, ToolStripMenuItem MapOnStartup, CheckBox SetRPM, CheckBox SetRate,
            CheckBox SetLoad, Button MapBtn, CheckBox chkLoopKml)
        {
            SpeedUpDown = TheSpeed;
            numRPM = TheRPM;
            numOdometer = TheOdometer;
            numLoad = TheLoad;
            numPTO = ThePTO;
            numRate = TheRate;
            numFuel = TheFuel;
            chkBrk = TheBrake;
            mRbtnBrake2Mph = rbtnBrake2Mph;
            mRbtnBrake5Mph = rbtnBrake5Mph;
            this.chkLoopKml = chkLoopKml;
            chkPTO = CheckPTO;
            chkVehicle = CheckVehicle;
            tbThrottle = TheThrottle;
            tsProgress = TheProgress;
            ConnectButton = TheButton;
            CameraLockButton = CamLockButton;
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
            NAVPort = NPort;
            VehiclePort = VPort;
            TheFileName = FileName;
            lblTimeAtSpeed = LabelSpeed;
            saveFileDialog1 = TheSave;
            Writing = Write;
            Reading = Read;
            PBF = StreamF;
            HACCEnabled = HACCChecked;
            HACCValue = HACCNum;
            CurrentTimeLabel = CurrentTime;
            rStart = RadioStart;
            chkStop = CheckStop;
            StartTime = SetStartTime;
            StopTime = SetStopTime;
            theCountDown = countDown;
            StartDone = StartPlaybackDone;
            SetPlaybackTime = RadioSetPlaybackTime;
            None = NoneButton;
            chkNoRPM = NoRPM;
            GPSTimeOnly = OnlyGPSTime;
            chkNoSpeed = NoSpeed;
            vehicleSpeed = VehicleSpeed;
            chkNoOdo = NoOdo;
            chkNoTEH = NoTEH;
            numEngineHours = TheEngineHour;
            FileLoaded = TheFileLoaded;
            VehicleComboBox = VehicleBox;
            NAVComboBox = NAVBox;
            currentCoordinates = CurrentCoordinates;
            chkLoadFileResetECM = ResetECM;
            TimeStarted = TheTimeStarted;
            mMapOnStartup = MapOnStartup;
            chkSetRPM = SetRPM;
            chkSetFuelRate = SetRate;
            chkSetEngineLoad = SetLoad;
            mapBtn = MapBtn;
        }

        //*****************************************************************************************
        // Name: TheCamLock()
        // Description: The actual code where the camera lock and unlock happens. Gets called from
        //              MainForm's CameraLockButton_Click method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void TheCamLock()
        {
            try
            {
                if (CameraLockButton.Text == "Camera Lock On" && map != null)
                {
                    CameraLockButton.Text = "Camera Lock Off";
                    map.CamLock(true);
                }
                else if (map != null)
                {
                    CameraLockButton.Text = "Camera Lock On";
                    map.CamLock(false);
                }
            }
            catch { }
        }

        //*****************************************************************************************
        // Name: SetPinCoordinates()
        // Description: Sends the coordinates of the pin.
        //-----------------------------------------------------------------------------------------
        // Inputs: Microsoft.Maps.MapControl.WPF.Location loc
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void SetPinCoordinates(Microsoft.Maps.MapControl.WPF.Location loc)
        {
            tempLoc = loc;
            PinSet = true;
            DateTime TimeNow = DateTime.Now.ToUniversalTime();
            StringBuilder sb = new StringBuilder();
            char NorthSouth;
            char EastWest;
            double locLat = loc.Latitude;
            double locLon = loc.Longitude;

            // used for converting lat and long
            decimal LatIntegral;
            decimal LatFraction;
            decimal LonIntegral;
            decimal LonFraction;

            if (locLat < 0)
            {
                NorthSouth = 'S';
                locLat *= -1;
            }
            else NorthSouth = 'N';

            LatIntegral = decimal.Truncate((decimal)locLat);
            LatFraction = (decimal)locLat - LatIntegral;

            if (locLon < 0)
            {
                EastWest = 'W';
                locLon *= -1;
            }
            else EastWest = 'E';

            LonIntegral = decimal.Truncate((decimal)locLon);
            LonFraction = (decimal)locLon - LonIntegral;

            sb.Append("$GPRMC,");
            sb.Append(TimeNow.Hour.ToString("D2"));
            sb.Append(TimeNow.Minute.ToString("D2"));
            sb.Append(TimeNow.Second.ToString("D2"));
            sb.Append(".");
            sb.Append(TimeNow.Millisecond.ToString());
            sb.Append(",A,");
            sb.Append(LatIntegral.ToString("00"));
            sb.Append((LatFraction * 60).ToString("00.00000"));
            sb.Append(",");
            sb.Append(NorthSouth);
            sb.Append(",");
            sb.Append(LonIntegral.ToString("000"));
            sb.Append((LonFraction * 60).ToString("00.00000"));
            sb.Append(",");
            sb.Append(EastWest);
            sb.Append(",");
            sb.Append(((double)SpeedUpDown.Value * 0.86897699264).ToString("0.000"));
            sb.Append(",,");
            sb.Append(TimeNow.ToString("ddMMyy"));
            sb.Append(",,,S,");
            byte checksum = 0;
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum ^= c;
            }
            sb.Append('*');
            sb.Append(checksum.ToString("X2"));

            PinRMCSentence = sb.ToString();
            if (NAVPort.IsOpen) NAVPort.WriteLine(PinRMCSentence);
            Console.WriteLine(PinRMCSentence);

            sb.Clear();
            sb.Append("$GPGGA,");
            sb.Append(TimeNow.Hour.ToString("D2"));
            sb.Append(TimeNow.Minute.ToString("D2"));
            sb.Append(TimeNow.Second.ToString("D2"));
            sb.Append(".");
            sb.Append(TimeNow.Millisecond.ToString());
            sb.Append(",");
            sb.Append(LatIntegral.ToString("00"));
            sb.Append((LatFraction * 60).ToString("00.00000"));
            sb.Append(",");
            sb.Append(NorthSouth);
            sb.Append(",");
            sb.Append(LonIntegral.ToString("000"));
            sb.Append((LonFraction * 60).ToString("00.00000"));
            sb.Append(",");
            sb.Append(EastWest);
            sb.Append(",8,08,0.9,,,,,,");
            byte checksum2 = 0;
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum2 ^= c;
            }
            sb.Append('*');
            sb.Append(checksum2.ToString("X2"));

            PinGGASentence = sb.ToString();
            if (NAVPort.IsOpen) NAVPort.WriteLine(PinGGASentence);
            Console.WriteLine(PinGGASentence);

            sb.Clear();
            sb.Append("$PUBX,00,");
            sb.Append(TimeNow.Hour.ToString("D2"));
            sb.Append(TimeNow.Minute.ToString("D2"));
            sb.Append(TimeNow.Second.ToString("D2"));
            sb.Append(".");
            sb.Append(TimeNow.Millisecond.ToString());
            sb.Append(",");
            sb.Append(LatIntegral.ToString("00"));
            sb.Append((LatFraction * 60).ToString("00.00000"));
            sb.Append(",");
            sb.Append(NorthSouth);
            sb.Append(",");
            sb.Append(LonIntegral.ToString("000"));
            sb.Append((LonFraction * 60).ToString("00.00000"));
            sb.Append(",");
            sb.Append(EastWest);
            sb.Append(",,,2.1,,,,,,");
            byte checksum3 = 0;
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum3 ^= c;
            }
            sb.Append('*');
            sb.Append(checksum3.ToString("X2"));

            PinPUBSentence = sb.ToString();
            if (NAVPort.IsOpen) NAVPort.WriteLine(PinPUBSentence);
            Console.WriteLine(PinPUBSentence);
            sb.Clear();

            if (loc.Latitude == 0xFFFFFFFF && loc.Longitude == 0xFFFFFFFF)
            {
                currentCoordinates.Text = "";
            }
            else
            {
                currentCoordinates.Text = loc.Latitude.ToString("0.000000") + ", " + loc.Longitude.ToString("0.000000");
            }
        }

        //*****************************************************************************************
        // Name: GPSTimeOnlyEngaged()
        // Description: Sending in GPS date/time only and don't care about the rest of the GPS data
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-19   SC Yuen     initial version
        //*****************************************************************************************
        public void GPSTimeOnlyEngaged()
        {
            DateTime TimeNow = DateTime.Now.ToUniversalTime();
            StringBuilder sb = new StringBuilder();
            sb.Append("$GPRMC,");
            sb.Append(TimeNow.Hour.ToString("D2"));
            sb.Append(TimeNow.Minute.ToString("D2"));
            sb.Append(TimeNow.Second.ToString("D2"));
            sb.Append(".");
            sb.Append(TimeNow.Millisecond.ToString());
            sb.Append(",V,,,,,,,");
            sb.Append(TimeNow.ToString("ddMMyy"));
            sb.Append(",,,S,");
            byte checksum = 0;
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum ^= c;
            }
            sb.Append('*');
            sb.Append(checksum.ToString("X2"));

            string RMCSentence = sb.ToString();
            if (NAVPort.IsOpen) NAVPort.WriteLine(RMCSentence);
            Console.WriteLine(RMCSentence);

            sb.Clear();
            sb.Append("$GPGGA,");
            sb.Append(TimeNow.Hour.ToString("D2"));
            sb.Append(TimeNow.Minute.ToString("D2"));
            sb.Append(TimeNow.Second.ToString("D2"));
            sb.Append(".");
            sb.Append(TimeNow.Millisecond.ToString());
            sb.Append(",,,,,8,,0.9,,,,,,");
            byte checksum2 = 0;
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum2 ^= c;
            }
            sb.Append('*');
            sb.Append(checksum2.ToString("X2"));

            string GGASentence = sb.ToString();
            if (NAVPort.IsOpen) NAVPort.WriteLine(GGASentence);
            Console.WriteLine(GGASentence);

            sb.Clear();
            sb.Append("$PUBX,00,");
            sb.Append(TimeNow.Hour.ToString("D2"));
            sb.Append(TimeNow.Minute.ToString("D2"));
            sb.Append(TimeNow.Second.ToString("D2"));
            sb.Append(".");
            sb.Append(TimeNow.Millisecond.ToString());
            sb.Append(",,,,,,,2.1,,,,,,");
            byte checksum3 = 0;
            foreach (byte c in sb.ToString().Substring(1))
            {
                checksum3 ^= c;
            }
            sb.Append('*');
            sb.Append(checksum3.ToString("X2"));

            string PUBSentence = sb.ToString();
            if (NAVPort.IsOpen) NAVPort.WriteLine(PUBSentence);
            Console.WriteLine(PUBSentence);
            sb.Clear();
        }

        //*****************************************************************************************
        // Name: DateTimeCheck()
        // Description: Setting/calculating date/time
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-19   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-11   SC Yuen     Added code to set the time when the loaded file first started
        //-----------------------------------------------------------------------------------------
        // 2017-06-21   SC Yuen     If PinSet is true, then set the coordinates to be where the pin
        //                          is at
        //*****************************************************************************************
        public void DateTimeCheck()
        {
            // If user want to use current time.
            if (!SetPlaybackTime.Checked)
            {
                CurrentTimeLabel.Text = DateTime.Now.ToString(TIME_FORMAT);      // Sets the current time.

                // If user just want to send GPS time.
                if (GPSTimeOnly.Checked)
                {
                    GPSTimeOnlyEngaged();

                    if (!chkVehicle.Checked)
                    {
                        return;
                    }
                }
                else if (PinSet)
                {
                    SetPinCoordinates(tempLoc);
                }
            }
            // Need to make sure that a file has been loaded before it could proceed when the user has selected to set the date/time.
            else if (SetPlaybackTime.Checked && TheFileName.FileName == "" && !GPSTimeOnly.Checked)
            {
                TheMain.StopNmeaTimer();
                NAVPort.Close();
                VehiclePort.Close();
                ConnectButton.Text = MainForm.CONNECT_TEXT;
                MessageBox.Show("Please select a file first.");
                ReadReady = true;
                return;
            }
            // Checks to see if the user has added a new time for the playback.
            else if (InitialCurrentTime)
            {
                TempTime1.Value = StartTime.Value;
                InitTime.Value = StartTime.Value;
                CurrentTimeLabel.Text = TempTime1.Value.ToString(TIME_FORMAT);             // Sets the current time to what the user has set.
                InitialCurrentTime = false;
            }
            // If no new time was added, then increment the time by 1 second.
            else
            {
                // Check to see if a new time has been set for playback.
                if (InitTime.Value != StartTime.Value)
                {
                    InitialCurrentTime = true;
                    CurrentTimeLabel.Text = StartTime.Value.ToString(TIME_FORMAT);       // Sets the current time to be the new value added.
                }

                // Printing out the current time when incremented by 1 second.
                else
                {
                    TempTime2.Value = TempTime1.Value.AddSeconds(1);        // Storing the incremented value.
                    CurrentTimeLabel.Text = TempTime2.Value.ToString(TIME_FORMAT);   // Sets the updated current time.
                    TempTime1 = TempTime2;                                  // Just to store the incremented value to be used on the next cycle.
                }
            }

            // Setting the time when the loaded file first started
            if (done && initialStartTime)
            {
                // When user set their own time
                if (SetPlaybackTime.Checked)
                {
                    TimeStarted.Text = StartTime.Value.ToString("MM/dd/yyyy HH:mm:ss");
                    initialStartTime = false;
                }
                // When using current time for KML or NMEA file
                else if (!SetPlaybackTime.Checked && !weAreReadingADATFile)
                {
                    TimeStarted.Text = DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss");
                    initialStartTime = false;
                }
                // When using the given time for DAT file
                else
                {
                    SetDATTimeStarted();
                }
            }

            if (!NAVPort.IsOpen)
            {
                try
                {
                    // Connecting the ports that aren't empty.
                    if (NAVPort.PortName != "" &&
                        NAVComboBox.Text != "")
                    {
                        NAVPort.Open();
                    }
                }
                catch { }
            }
            else if (NAVComboBox.Text == "")
            {
                NAVPort.Close();
            }

            if (chkVehicle.Checked)
            {
                if (!VehiclePort.IsOpen)
                {
                    try
                    {
                        // Connecting the ports that aren't empty.
                        if (VehiclePort.PortName != "" &&
                            VehicleComboBox.Text != "")
                        {
                            VehiclePort.Open();
                        }
                    }
                    catch { }
                }
                else if (VehiclePort.IsOpen && VehicleComboBox.Text == "")
                {
                    VehiclePort.Close();
                }
            }
        }

        //*****************************************************************************************
        // Name: SetDATTimeStarted()
        // Description: Setting the time started for the DAT file that was loaded
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-05-11   SC Yuen     initial version
        //*****************************************************************************************
        public void SetDATTimeStarted()
        {
            string tempDate = TheMap.GetDATDate().ToString("000000");       // ddmmyy

            if (tempDate != "000000")
            {
                string tempDay = tempDate[0].ToString() + tempDate[1].ToString();           // dd
                string tempMonth = tempDate[2].ToString() + tempDate[3].ToString();         // mm
                string tempYear = "20" + tempDate[4].ToString() + tempDate[5].ToString();   // 20 + yy = 20yy
                tempDate = tempMonth + "/" + tempDay + "/" + tempYear + " ";                // mm/dd/20yy
            }

            string tempTime = TheMap.GetDATTime().ToString("000000");       // hhmmss.ss

            if (tempTime != "000000")
            {
                string tempHour = tempTime[0].ToString() + tempTime[1].ToString();          // hh
                string tempMinute = tempTime[2].ToString() + tempTime[3].ToString();        // mm
                string tempSecond = tempTime[4].ToString() + tempTime[5].ToString();        // ss
                tempTime = tempHour + ":" + tempMinute + ":" + tempSecond;                  // hh:mm:ss
            }

            if (tempDate != "000000" &&
                tempTime != "000000")
            {
                TimeStarted.Text = tempDate + tempTime;     // mm/dd/20yy hh:mm:ss
                initialStartTime = false;
            }
        }

        //*****************************************************************************************
        // Name: StopTimeChecked()
        // Description: Functionality when "Stop" has been checked
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-21 SC Yuen   initial version
        //*****************************************************************************************
        public void StopTimeChecked()
        {
            // Check to see if the scheduled stop playback has been enabled and if the scheduled stop has been reached.
            if ((chkStop.Checked && StopTime.Value.ToString() == DateTime.Now.ToString()) || (chkStop.Checked && SetPlaybackTime.Checked &&
                StopTime.Value.ToString() == TempTime1.Value.ToString()))
            {
                TheMain.StopNmeaTimer();
                theCountDown.Text = "00/00:00:00";          // Set the timer back to default state.
                ConnectButton.Text = MainForm.CONNECT_TEXT;
                NAVPort.Close();
                VehiclePort.Close();
                StartFlag = false;
                StartDone = true;
                MessageBox.Show("Time reached, playback paused.");
                return;
            }

            // Check to see if chkStop is checked and if the scheduled stop time is valid when the SetPlaybackTime isnt' checked.
            else if (chkStop.Checked && StopTime.Value > DateTime.Now && !SetPlaybackTime.Checked)
            {
                DateTime tempTime = new DateTime();
                tempTime = StopTime.Value;
                System.TimeSpan ts = tempTime - DateTime.Now;

                // Prints the amount of time before the test stops.
                theCountDown.Text = ts.Days.ToString("00") + "/" + ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }

            // Check to see if chkStop is checked and if the scheduled stop time is valid when the SetPlaybackTime is checked.
            else if (chkStop.Checked && SetPlaybackTime.Checked && StopTime.Value > TempTime1.Value)
            {
                DateTime tempTime = new DateTime();
                tempTime = StopTime.Value;
                System.TimeSpan ts = tempTime - TempTime1.Value;

                // Prints the amount of time before the test stops.
                theCountDown.Text = ts.Days.ToString("00") + "/" + ts.Hours.ToString("00") + ":" + ts.Minutes.ToString("00") + ":" + ts.Seconds.ToString("00");
            }

            // Check to see if the chkStop is checked and if the scheduled stop time is valid.
            else if ((chkStop.Checked && StopTime.Value <= DateTime.Now) || (chkStop.Checked && SetPlaybackTime.Checked && StopTime.Value <= TempTime1.Value))
            {
                TheMain.StopNmeaTimer();
                NAVPort.Close();
                VehiclePort.Close();
                MessageBox.Show("Please choose a later stop date/time.");
                ConnectButton.Text = MainForm.CONNECT_TEXT;
                return;
            }
        }

        //*****************************************************************************************
        // Name: StartTimeChecked()
        // Description: Functionality when "Start" has been checked
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-21 SC Yuen   initial version
        //*****************************************************************************************
        public void StartTimeChecked()
        {
            // If chkStart has been checked and the scheduled start time is >= current time, then set the text on the ConnectButton.
            if (rStart.Checked && StartTime.Value >= DateTime.Now && ConnectButton.Text == MainForm.CONNECT_TEXT)
            {
                StartFlag = true;       // Set to true to indicate that the rStart has been initiated.
                ConnectButton.Text = "Timer Started";
            }

            // Making sure that if the rStart has been checked, but then unchecked it will stop the NmeaTimer.
            if (!rStart.Checked && StartFlag && StartTime.Value >= DateTime.Now)
            {
                TheMain.StopNmeaTimer();
                StartFlag = false;
            }

            // Check to see if the chkStart is checked and if a file has been selected.
            if (rStart.Checked && TheFileName.FileName != "")
            {
                if (!NAVPort.IsOpen && !VehiclePort.IsOpen) ReadReady = false;      // Sets ReadReady to be false to not run the loaded file.

                // Check to see if the current time has reached the scheduled start time.
                if (StartTime.Value.ToString() == DateTime.Now.ToString())
                {
                    ConnectButton.Text = MainForm.DISCONNECT_TEXT;
                    None.Checked = true;

                    // Connecting the ports that aren't empty.
                    if (NAVPort.PortName != "") NAVPort.Open();
                    if (VehiclePort.PortName != "" && chkVehicle.Checked) VehiclePort.Open();

                    StartFlag = false;
                    StartDone = true;
                    ReadReady = true;       // Files ready to be read.
                }
            }

            // Check to see if the chkStart has been checked and if a file has been selected or not.
            else if (rStart.Checked && TheFileName.FileName == "")
            {
                TheMain.StopNmeaTimer();
                ConnectButton.Text = MainForm.CONNECT_TEXT;
                MessageBox.Show("Please select a file first.");
                ReadReady = true;
                return;
            }
        }

        //*****************************************************************************************
        // Name: ProcessFile()
        // Description: Process the files if they were selected, if no file were selected then
        //              process the vehicle speed if "Vehicle Data" has been checked
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-21   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-25   SC Yuen     Removed the check for GPSTimeOnly for KML and DAT files
        //-----------------------------------------------------------------------------------------
        // 2017-06-20   SC Yuen     Setting the ECM data to be modifiable when the trip finishes
        //*****************************************************************************************
        public void ProcessFile()
        {
            // Printing NMEA sentences from KML files.
            if ((route != null) && !weAreReadingADATFile && !weAreProcessingAnNMEAFile)
            {
                ReadingKMLFile();
            }

            // Prints the NMEA sentences from the NMEA file.
            else if (weAreProcessingAnNMEAFile && !GPSTimeOnly.Checked)
            {
                ReadingNMEAFile();
            }

            // Load list to local array, then load local array onto global array. Doing this in order to create a dynamic array.
            // The numCheck goes to true once DATVehicleData() has been called so that the method would only gets called once.
            else if (weAreReadingADATFile && numCheck == false)
            {
                DATVehicleData();
            }

            // Printing NMEA sentences from DAT files.
            else if (weAreReadingADATFile && DATLines < (STD1Line - 4))
            {
                ReadingDATFile();
            }
            // Checking to see if we have reached the end of file.
            else if (weAreReadingADATFile && DATLines == STD1Line)
            {
                TheMain.StopNmeaTimer();
                NAVPort.Close();
                VehiclePort.Close();
                SpeedUpDown.Enabled = true;
                tbThrottle.Enabled = true;
                numFuel.Enabled = true;
                numEngineHours.Enabled = true;
                numPTO.Enabled = true;
                numOdometer.Enabled = true;
                numRate.Enabled = true;
                numLoad.Enabled = true;
                numRPM.Enabled = true;
                mapBtn.Enabled = true;
                weAreReadingADATFile = false;
                chkSetRPM.Enabled = true;
                chkSetFuelRate.Enabled = true;
                chkSetEngineLoad.Enabled = true;
                TheMain.TripFinished();
                ConnectButton.Text = MainForm.CONNECT_TEXT;
                MessageBox.Show("Trip has been finished.");
            }

            // If no files are loaded and just want to print out vehicle data.
            else
            {
                // Check to see if there was any difference in speed in the span of one second.
                // If no difference, elapsed will increase by one second.
                // If there is a difference, elapsed will reset back to zero and will set the new speed.
                if (Math.Abs(tempSpeed - speed) >= speedMargin)
                {
                    elapsed = new DateTime();
                    speed = tempSpeed;
                }

                // If the vehicle data checkbox has been checked, the elapsed will increase by one second to indicate how much time the vehicle has stayed
                // at the same speed.
                if (chkVehicle.Checked)
                {
                    elapsed = TheVehicle.SetValues(elapsed, chkNoRPM.Checked, chkNoSpeed.Checked, chkNoOdo.Checked, chkNoTEH.Checked);
                    lblTimeAtSpeed.Text = elapsed.ToString(TIME_FORMAT);
                    vehicleSpeed.Text = tempSpeed.ToString(SPEED_FORMAT);
                }
            }
        }

        //*****************************************************************************************
        // Name: TheNMEATimer()
        // Description: Check what type of file has been loaded (i.e. KML, NMEA, or DAT) and calls
        //              the appropriate functions to process them. Check the scheduled start time
        //              to see if it's good, or if it has been reached it will connect the port(s)
        //              that has been selected and start the playback. It also checks to see if the
        //              stop time is good, or if it has been reached. If so, it will disconnect the
        //              port(s) and pause the playback.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2016-01-22 SC Yuen   Edits to code to remove constraints on needing NAV port selected
        //-----------------------------------------------------------------------------------------
        // 2016-01-26 SC Yuen   Edits to downsize this function
        //-----------------------------------------------------------------------------------------
        // 2016-02-03 SC Yuen   Added codes for the start and stop timer for the playback
        //-----------------------------------------------------------------------------------------
        // 2016-11-30 SC Yuen   Added codes for handling situations where GPSTimeOnly was checked
        //-----------------------------------------------------------------------------------------
        // 2017-04-12 SC Yuen   Added logic when chkNoSpeed is checked and updated the parameters
        //                      when calling TheVehicle.SetValues() and TheMap.SetSpeed()
        //-----------------------------------------------------------------------------------------
        // 2017-04-19 SC Yuen   Refactored code
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen   Fixed issue with chkBrk not working
        //                      When the user delete the value in the SpeedUpDown and it doesn't
        //                      set the value to 0
        //                      Refactored code
        //-----------------------------------------------------------------------------------------
        // 2017-05-10 SC Yuen   Saving ECM values to be restored when opening Trip Sim next time
        //-----------------------------------------------------------------------------------------
        // 2017-05-12 SC Yuen   When chkBrk gets checked and the vehicle speed hits 0, the chkBrk
        //                      will get unchecked
        //-----------------------------------------------------------------------------------------
        // 2017-06-20 SC Yuen   Set TheMap only if mapNotCreated is false
        //-----------------------------------------------------------------------------------------
        // 2020-06-10 B Bassing   Added the braking rate
        //*****************************************************************************************
        public void TheNMEATimer()
        {
            Properties.Settings.Default.Odometer = numOdometer.Value;
            Properties.Settings.Default.TotalFuel = numFuel.Value;
            Properties.Settings.Default.TotalPTO = numPTO.Value;
            Properties.Settings.Default.TotalEngineHours = numEngineHours.Value;
            Properties.Settings.Default.Save();

            // Redefine line terminator. Without this, the NMEA sentences lack the carriage return.
            NAVPort.NewLine = "\r\n";
            DateTimeCheck();

            // Check to see if chkStop has been checked, if not we can skip going through these codes.
            if (chkStop.Checked)
            {
                StopTimeChecked();
            }

            // If StartDone is true or SetPlaybackTime is checked, then that means we don't have to go through the code for the start playback.
            if (!StartDone && !SetPlaybackTime.Checked)
            {
                StartTimeChecked();
            }          

            // Check to see if the file loaded is ready to be read or not.
            if (ReadReady)
            {
                TheVehicle = TheMain.GetVehicle();

                if (mapCreated)
                {
                    TheMap = TheMain.GetMap();
                }

                // if (the engine is on) then
                if (chkVehicle.Checked)
                {
                    if (SpeedUpDown.Text == "")
                    {
                        SpeedUpDown.Value = 0;
                    }
                    else if (SpeedUpDown.Value >= tbThrottle.Minimum &&
                    SpeedUpDown.Value <= tbThrottle.Maximum)
                    {
                        tempSpeed = SpeedUpDown.Value;
                    }
                }
                else
                {
                    // the engine is off, speed has already been taken off (elsewhere)
                    tempSpeed = 0;
                }

                if (chkBrk.Checked)
                {
                    if (mRbtnBrake2Mph.Checked && SpeedUpDown.Value >= 2)
                    {
                        // brake at 2 mph
                        SpeedUpDown.Value = SpeedUpDown.Value - 2.0M;
                    }
                    else if (mRbtnBrake5Mph.Checked && SpeedUpDown.Value >= 5)
                    {
                        // brake at 5 mph
                        SpeedUpDown.Value = SpeedUpDown.Value - 5.0M;
                    }
                    else
                    {
                        SpeedUpDown.Value = 0;
                        chkBrk.Checked = false;
                    }
                }

                if (!chkNoSpeed.Checked && tempSpeed <= tbThrottle.Maximum)
                {
                    tbThrottle.Value = (int)tempSpeed;
                }              

                J1708Playback = TheVehicle.GetPlaybackStatus();

                // Checking if playback has been enabled, and if it is enabled it will start the playback process.
                if (J1708Playback != null)
                {
                    TheVehicle.SetPlayback(true);
                    TheMain.SetJ1708Timer(true);
                    return;
                }

                // Check to see if the PTO checkbox has been checked and if it is, it will increase accordingly.
                if (chkPTO.Checked)
                {
                    numPTO.Value += HR_PER_1S;
                }

                // Check to see if the vehicle port is open and if the vehicle checkbox has been checked.
                // If both are true start the reports on the vehicle data process.
                if (chkVehicle.Checked && VehiclePort.IsOpen)
                {
                    TheMain.SetJ1708Timer(false);
                }
                else
                {
                    TheMain.StopJ1708Timer(true);
                }

                if (TheFileName.FileName != "" && weAreProcessingAnNMEAFile)
                {
                    stream = new FileStream(TheFileName.FileName, FileMode.Open, FileAccess.Read);
                    reader = new StreamReader(stream);
                    stream = null;
                }

                ProcessFile();
            }
        }

        //*****************************************************************************************
        // Name: ReadingKMLFile()
        // Description: Reads the KML file that has been loaded, prints out the data out into the
        //              serial port, and placing points on the Bing map to mark current and past
        //              locations. It also highlights the entire route of the trip loaded and place
        //              a marker at the destination.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2016-01-26   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        // 2017-04-21   SC Yuen     Replaced all the SpeedUpDown with tempSpeed
        //-----------------------------------------------------------------------------------------
        // 2017-05-11   SC Yuen     Added tsProgress.Value = route.getCoordinateIndex() to set the
        //                          progress bar and setting the current coordinates for the user
        //                          to view
        //-----------------------------------------------------------------------------------------
        // 2017-05-25   SC Yuen     Added if (!GPSTimeOnly.Checked) to keep the coordinates updated
        //                          but doesn't send it out if GPSTimeOnly is checked
        //-----------------------------------------------------------------------------------------
        // 2017-06-20   SC Yuen     If mapNotCreated is false, we can highlight, place pins, and
        //                          place dots on the Bing map
        //-----------------------------------------------------------------------------------------
        // 2018-03-22   JC Anderson Adding option to loop over kml coordinates
        //*****************************************************************************************
        public void ReadingKMLFile()
        {
            if (mapCreated)
            {
                if (!done)
                {
                    Highlight();
                }
            }
            else
            {
                if (!done)
                {
                    long countElements = route.GetListSize();
                    CoordinateCollection lastCoord = route.GetLastCoordinates();
                    Vector lastWaypoint = lastCoord.ElementAt((int)countElements);
                    lastLat = lastWaypoint.Latitude;
                    lastLon = lastWaypoint.Longitude;

                    while (lat != lastLat && lon != lastLon)
                    {
                        Vector pos = WPRoute.getCurrentPosition((decimal).00833);
                        lat = pos.Latitude;
                        lon = pos.Longitude;
                    }

                    tsProgress.Maximum = WPRoute.getCoordinateIndex();
                    done = true;
                } 
            }

            Vector position = route.getCurrentPosition(tempSpeed / 60 / 60);        // Spin box value divided by 3600, so it's miles per second
            tsProgress.Value = route.getCoordinateIndex();

            if (position != null)
            {
                // Check to see if there was any difference in speed in the span of one second.
                // If no difference, elapsed will increase by one second.
                // If there is a difference, elapsed will reset back to zero and will set the new speed.
                if (Math.Abs(tempSpeed - speed) >= speedMargin)
                {
                    elapsed = new DateTime();
                    speed = tempSpeed;
                }

                // If the vehicle data checkbox has been checked, the elapsed will increase by one second to indicate how much time the vehicle has stayed
                // at the same speed.
                if (chkVehicle.Checked)
                {
                    elapsed = TheVehicle.SetValues(elapsed, chkNoRPM.Checked, chkNoSpeed.Checked, chkNoOdo.Checked, chkNoTEH.Checked);
                    lblTimeAtSpeed.Text = elapsed.ToString(TIME_FORMAT);
                    vehicleSpeed.Text = tempSpeed.ToString(SPEED_FORMAT);
                }

                tbThrottle.Value = (int)tempSpeed;

                if (!GPSTimeOnly.Checked)
                {
                    // Checking to see if we're using default current date/time.
                    if (None.Checked)
                    {
                        rmc = new GPRMC(Convert.ToDecimal(position.Latitude),
                            Convert.ToDecimal(position.Longitude), DateTime.Now.ToUniversalTime(), Convert.ToDouble(tempSpeed), false, null);
                        if (NAVPort.IsOpen) NAVPort.WriteLine(rmc.ToString());
                        Console.WriteLine(rmc.ToString());
                        gga = new GPGGA(Convert.ToDecimal(position.Latitude),
                            Convert.ToDecimal(position.Longitude), DateTime.Now.ToUniversalTime(), false, null);
                        if (NAVPort.IsOpen) NAVPort.WriteLine(gga.ToString());
                        Console.WriteLine(gga.ToString());
                        pub = new PUBX00(Convert.ToDecimal(position.Latitude),
                            Convert.ToDecimal(position.Longitude), DateTime.Now.ToUniversalTime(), false, null);
                        if (NAVPort.IsOpen) NAVPort.WriteLine(pub.ToString());
                        Console.WriteLine(pub.ToString());
                    }
                    // Using user selected date/time.
                    else
                    {
                        rmc = new GPRMC(Convert.ToDecimal(position.Latitude),
                            Convert.ToDecimal(position.Longitude), TempTime1.Value, Convert.ToDouble(tempSpeed), false, TempTime1);
                        if (NAVPort.IsOpen) NAVPort.WriteLine(rmc.ToString());
                        Console.WriteLine(rmc.ToString());
                        gga = new GPGGA(Convert.ToDecimal(position.Latitude),
                            Convert.ToDecimal(position.Longitude), TempTime1.Value, false, TempTime1);
                        if (NAVPort.IsOpen) NAVPort.WriteLine(gga.ToString());
                        Console.WriteLine(gga.ToString());
                        pub = new PUBX00(Convert.ToDecimal(position.Latitude),
                            Convert.ToDecimal(position.Longitude), TempTime1.Value, false, TempTime1);
                        if (NAVPort.IsOpen) NAVPort.WriteLine(pub.ToString());
                        Console.WriteLine(pub.ToString());
                    }
                }
               
                lat = position.Latitude;
                lon = position.Longitude;

                if (mapCreated)
                {
                    // if (we should pin the starting location) then
                    if (startPos)
                    {
                        Vector startingPosition = WPRoute.GetLastCoordinates().ElementAt(0);
                        double startingLat = startingPosition.Latitude;
                        double startingLon = startingPosition.Longitude;

                        // Prevents putting pin on null/default coordinates.
                        if ((startingLat != 0 && startingLon != 0) &&
                            (startingLat != 1E-06 && startingLon != 1E-06) &&
                            (startingLat != -1E-06 && startingLon != -1E-06))
                        {
                            map.Pin(startingLat, startingLon);
                            startPos = false;
                        }
                    }

                    map.PlaceDot(lat, lon, Colors.Blue, tempSpeed);
                }


                if ((lat == lastLat) && (lon == lastLon) && (route.getCoordinateIndex() >= route.GetListSize()))
                {
                    if (chkLoopKml.Checked)
                    {
                        route.resetCoordinateIndex();
                        WPRoute.resetCoordinateIndex();
                    }
                    else
                    {
                        TheMain.StopNmeaTimer();
                        NAVPort.Close();
                        VehiclePort.Close();
                        mapBtn.Enabled = true;
                        route = null;
                        TheMain.TripFinished();
                        ConnectButton.Text = MainForm.CONNECT_TEXT;
                        MessageBox.Show("Trip has been finished.");
                    }
                }

                currentCoordinates.Text = lat.ToString("0.000000") + ", " + lon.ToString("0.000000");
            }
        }

        //*****************************************************************************************
        // Name: ProcessNMEAFile()
        // Description: Process NMEA file that was loaded
        //-----------------------------------------------------------------------------------------
        // Inputs: string[] allLines
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-21 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-10 SC Yuen   Added tsProgress.Value = NMEALineCount at the end to set the
        //                      progress bar and setting the current coordinates for the user to
        //                      view
        //-----------------------------------------------------------------------------------------
        // 2017-06-20 SC Yuen   If mapNotCreated is false, we can place pins and place dots on the
        //                      Bing map
        //*****************************************************************************************
        public void ProcessNMEAFile(string[] allLines)
        {
            bool incorrectSentence = true;

            while (incorrectSentence)
            {
                if (allLines[NMEALineCount].Contains("$GPRMC"))
                {
                    // Checking to see if we're using default current date/time.
                    if (None.Checked)
                    {
                        rmc = new GPRMC(allLines[NMEALineCount], null);
                    }
                    // Using user selected date/time.
                    else
                    {
                        rmc = new GPRMC(allLines[NMEALineCount], TempTime1);
                    }

                    if (NAVPort.IsOpen) NAVPort.WriteLine(rmc.ToString());
                    Console.WriteLine(rmc.ToString());
                    decimal currentSpeed = rmc.GetCurrentSpeed();
                    tempSpeed = Math.Round(currentSpeed, 1);
                    if (Math.Abs(tempSpeed - speed) >= speedMargin)
                    {
                        elapsed = new DateTime();
                        speed = tempSpeed;
                    }

                    if (chkVehicle.Checked)
                    {
                        elapsed = TheVehicle.SetValues(elapsed, chkNoRPM.Checked, chkNoSpeed.Checked, chkNoOdo.Checked, chkNoTEH.Checked);
                        lblTimeAtSpeed.Text = elapsed.ToString(TIME_FORMAT);
                        vehicleSpeed.Text = tempSpeed.ToString(SPEED_FORMAT);
                    }

                    tbThrottle.Value = (int)tempSpeed;

                    NMEALineCount++;
                }
                else if (allLines[NMEALineCount].Contains("$GPGGA"))
                {
                    // Checking to see if we're using default current date/time.
                    if (None.Checked)
                    {
                        gga = new GPGGA(allLines[NMEALineCount], null);
                    }
                    // Using user selected date/time.
                    else
                    {
                        gga = new GPGGA(allLines[NMEALineCount], TempTime1);
                    }

                    if (NAVPort.IsOpen) NAVPort.WriteLine(gga.ToString());
                    Console.WriteLine(gga.ToString());
                    NMEALineCount++;
                }
                else if (allLines[NMEALineCount].Contains("$PUBX"))
                {
                    // Checking to see if we're using default current date/time.
                    if (None.Checked)
                    {
                        pub = new PUBX00(allLines[NMEALineCount], HACCEnabled, HACCValue, null);
                    }
                    // Using user selected date/time.
                    else
                    {
                        pub = new PUBX00(allLines[NMEALineCount], HACCEnabled, HACCValue, TempTime1);
                    }

                    if (NAVPort.IsOpen) NAVPort.WriteLine(pub.ToString());
                    Console.WriteLine(pub.ToString());

                    if (mapCreated)
                    {
                        TheMap.GetMain(TheMain);
                        TheMap.NMEASetCoordinates(pub.ToString());                                 // Set the coordinates so it will work with Bing map.
                        lat = TheMap.GetLat();
                        lon = TheMap.GetLon();

                        // Pinning the starting location.
                        if (startPos)
                        {
                            if ((lat != 0 && lon != 0) && (lat != 1E-06 && lon != 1E-06)    // Prevents putting pin on null/default coordinates.
                                && (lat != -1E-06 && lon != -1E-06))
                            {
                                map.Pin(lat, lon);
                                startPos = false;
                            }
                        }
                        map.PlaceDot(lat, lon, Colors.Blue, tempSpeed);
                    }

                    incorrectSentence = false;
                    currentCoordinates.Text = lat.ToString("0.000000") + ", " + lon.ToString("0.000000");

                    NMEALineCount++;
                }
                else NMEALineCount++;

                tsProgress.Value = NMEALineCount;
            }
        }

        //*****************************************************************************************
        // Name: ReadingNMEAFile()
        // Description: Reads the NMEA file that has been loaded, prints out the data out into the
        //              serial port, and placing points on the Bing map to mark current and past
        //              locations. It also highlights the entire route of the trip loaded and place
        //              a marker at the destination.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2016-01-26 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen   Replaced all the SpeedUpDown with tempSpeed and refactored code
        //-----------------------------------------------------------------------------------------
        // 2017-06-20 SC Yuen   If mapNotCreated is false, we can highlight the Bing map
        //*****************************************************************************************
        public void ReadingNMEAFile()
        {
            if (mapCreated)
            {
                if (!done)
                {
                    Highlight();
                }
            }

            try
            {
                using (StreamReader r = new StreamReader(TheFileName.FileName))
                {
                    int count = 0;

                    // Read through each line of the NMEA file and increment "count" in order to get the total
                    // number of lines from the file.
                    while (r.ReadLine() != null) count++;

                    tsProgress.Maximum = count;

                    // Set the size of the "allLines" array.
                    string[] allLines = new string[count];

                    // Place each line from the NMEA file into the "allLines" array.
                    for (int i = 0; i < count; i++)
                    {
                        allLines[i] = reader.ReadLine();
                    }

                    // Go through the whole "allLines" array until it reaches "$PUBX", in order
                    // to get through all three NMEA sentences and print them out. After going
                    // through all the sentences, it will wait for the NMEATimer_Tick to go
                    // through here again in one second and go through the process again.
                    try
                    {
                        ProcessNMEAFile(allLines);
                    }
                    catch (Exception)
                    {
                        TheMain.StopNmeaTimer();
                        NAVPort.Close();
                        VehiclePort.Close();
                        SpeedUpDown.Enabled = true;
                        tbThrottle.Enabled = true;
                        mapBtn.Enabled = true;
                        weAreProcessingAnNMEAFile = false;
                        TheMain.TripFinished();
                        ConnectButton.Text = MainForm.CONNECT_TEXT;
                        MessageBox.Show("Trip has been finished.");
                    }
                }
            }
            finally
            {
                if (stream != null) stream.Dispose();
            }
        }

        //*****************************************************************************************
        // Name: ProcessDATFileECMData()
        // Description: Process the ECM data from the DAT file that was loaded
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-21 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-06-20 SC Yuen   Fixed the vehicle speed issue.  Had to convert the vehicle speed
        //                      from 1.256 kph to 1 mph
        //*****************************************************************************************
        public void ProcessDATFileECMData()
        {
            elapsed = TheVehicle.SetValues(elapsed, chkNoRPM.Checked, chkNoSpeed.Checked, chkNoOdo.Checked, chkNoTEH.Checked);
            lblTimeAtSpeed.Text = elapsed.ToString(TIME_FORMAT);
            vehicleSpeed.Text = tempSpeed.ToString(SPEED_FORMAT);

            // STD1 = 1 per second
            // STD2 = 1 per 5 seconds

            decimal odo = (decimal)globalSTD1Data[STD1Index];           // In STD1 with a Res of .125 km
            odo /= (decimal)(8 / .621371);                              // Conversion from km to mi     

            // Setting it to N/A when > 10000000 or when chkNoOdo has been checked     
            if (chkNoOdo.Checked || odo > 10000000)
            {
                odo = 0xFFFFFFFF;
            }

            STD1Index++;
            DATLines++;
            SpeedUpDown.Value = (decimal)globalSTD1Data[STD1Index];     // In STD1 with a Res of 1.256 km/hr
            SpeedUpDown.Value *= (decimal)(.00390625 * .621371);        // Conversion from km to mi

            // If chkNoSpeed has been checked or SpeedUpDown is NA, set it to N/A
            if (chkNoSpeed.Checked || SpeedUpDown.Value > MainVehicle.MAX_SPEED)
            {
                SpeedUpDown.Value = 0xFFFF;
            }

            STD1Index++;
            DATLines++;
            decimal VehicleRPM = (decimal)globalSTD1Data[STD1Index];    // In STD1 with a Res of .125 RPM
            VehicleRPM /= 8;

            // If chkNoRPM has been checked or VehicleRPM is NA, set it to N/A
            if (chkNoRPM.Checked || VehicleRPM > 10000)
            {
                VehicleRPM = 0xFFFF;
            }

            STD1Index++;
            DATLines++;
            decimal FRate = (decimal)globalSTD1Data[STD1Index];         // In STD1 with a Res of .05 L/hr
            FRate *= (decimal)(.05 * .264172);                          // Conversion from L/hr to Gal/hr
            STD1Index++;
            DATLines++;
            decimal VehicleLoad = (decimal)globalSTD1Data[STD1Index];   // In STD1
            STD1Index++;
            DATLines++;
            STD2FiveCount++;

            // Start of STD2 that prints out every 5 seconds.
            if (STD2FiveCount == 5 && STD2Line > STD2Index)
            {
                ProcessDATFileSTD2Messages();
            }

            // Setting the values to the Trip Sim MainForm.
            numOdometer.Value = odo;
            numRPM.Value = VehicleRPM;
            numPTO.Value = PTOHours;
            numFuel.Value = TotFuel;
            numRate.Value = FRate;
            numLoad.Value = VehicleLoad;
            numEngineHours.Value = TotEngineHours;

            // Checking to see if PTO is on or not and setting the PTO check box accordingly.
            if (PTOCheckBox == 1)   // 01 is on.
            {
                chkPTO.Checked = true;
            }
            else                    // Everything else we can ignore and set PTO status to off.
            {
                chkPTO.Checked = false;
            }
        }

        //*****************************************************************************************
        // Name: ProcessDATFileSTD2Messages()
        // Description: Process the STD2 data from the DAT file that was loaded
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-21 SC Yuen   initial version
        //*****************************************************************************************
        public void ProcessDATFileSTD2Messages()
        {
            TotFuel = (decimal)globalSTD2Data[STD2Index];           // Res of .05 L
            TotFuel *= (decimal).05;                                // Conversion from L to Gal

            // Set it to N/A if TotFuel is over 1000000
            if (TotFuel > 1000000)
            {
                TotFuel = 0xFFFFFFFF;
            }

            STD2Index++;
            PTOHours = (decimal)globalSTD2Data[STD2Index];          // Res of .05 hr
            PTOHours *= (decimal).05;

            // Set it to N/A if PTOHours is over 1000000
            if (PTOHours > 1000000)
            {
                PTOHours = 0xFFFFFFFF;
            }

            STD2Index++;
            TotEngineHours = (decimal)globalSTD2Data[STD2Index];

            // If chkNoTEH has been checked or TotEngineHours is NA, set it to NA
            if (chkNoTEH.Checked || TotEngineHours == 0xFFFFFFFF)
            {
                TotEngineHours = 0xFFFFFFFF;
            }
            else
            {
                TotEngineHours *= (decimal).05;
            }

            STD2Index++;
            PTOCheckBox = (decimal)(globalSTD2Data[STD2Index] >> 14);   // 2 set of 8 bits. First 2 MSB is what determines the PTO status.
                                                                        // Shift it 14 times to the right to only check those two bits.
                                                                        // 00 - Off
                                                                        // 01 - On
                                                                        // 10 - Error
                                                                        // 11 - NA
            STD2Index++;
            STD2FiveCount = 0;
        }

        //*****************************************************************************************
        // Name: ProcessDATFileGPRMC()
        // Description: Process the GPRMC data from the DAT file that was loaded
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-21 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-06-20 SC Yuen   If mapNotCreated is false, we can set the gpsSpeed.
        //*****************************************************************************************
        public void ProcessDATFileGPRMC()
        {
            decimal gpsSpeed = 0;

            if (mapCreated)
            {
                TheMap.GetMain(TheMain);

                // Checking to see if we're using default current date/time.
                if (None.Checked)
                {
                    gpsSpeed = TheMap.SetSpeed(globalSentence[index], NAVPort, speed, gpsSpeed, chkVehicle, null,
                        chkNoRPM.Checked, chkNoSpeed.Checked, chkNoOdo.Checked, chkNoTEH.Checked);       // Setting the speed in correct format.
                }
                // Using user selected date/time.
                else
                {
                    gpsSpeed = TheMap.SetSpeed(globalSentence[index], NAVPort, speed, gpsSpeed, chkVehicle,
                        TempTime1, chkNoRPM.Checked, chkNoSpeed.Checked, chkNoOdo.Checked, chkNoTEH.Checked);       // Setting the speed in correct format.
                }
            }

            // Adds a second to the elapsed time.
            if (chkVehicle.Checked)
            {
                ProcessDATFileECMData();
            }
            else
            {
                tempSpeed = gpsSpeed;
            }

            // Check to see if the speed changed since a second ago. If it did change, reset the elapsed time and set the speed.
            if (Math.Abs(SpeedUpDown.Value - speed) >= speedMargin)
            {
                elapsed = new DateTime();
                speed = SpeedUpDown.Value;
            }

            if (SpeedUpDown.Value <= tbThrottle.Maximum)
            {
                tbThrottle.Value = (int)SpeedUpDown.Value;
            }
        }

        //*****************************************************************************************
        // Name: ReadingDATFile()
        // Description: Reads the DAT file that has been loaded, prints out the data out into the
        //              serial port, and placing points on the Bing map to mark current and past
        //              locations. It also highlights the entire route of the trip loaded and place
        //              a marker at the destination.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2016-01-26   SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-04-18   SC Yuen   Fixing issues with reading DAT file that contains NA vehicle
        //                        speed
        //-----------------------------------------------------------------------------------------
        // 2017-04-21   SC Yuen   Replaced all the SpeedUpDown with tempSpeed, reading TEH from
        //                        DAT file, and now NA values from DAT file will be NA instead of
        //                        setting them to 0
        //-----------------------------------------------------------------------------------------
        // 2017-04-28   SC Yuen   Refactored code
        //-----------------------------------------------------------------------------------------
        // 2017-05-10   SC Yuen   Added tsProgress.Value = index at the end to set the progress bar
        //                        and setting the current coordinates for the user to view
        //-----------------------------------------------------------------------------------------
        // 2017-05-25   SC Yuen   Updated the arguments needed for some of the function calls
        //-----------------------------------------------------------------------------------------
        // 2017-06-20   SC Yuen   If mapNotCreated is false, we can set the coordinates on the Bing
        //                        map and place pin and dots to it
        //*****************************************************************************************
        public void ReadingDATFile()
        {
            if (index != lineCount)
            {
                for (int i = 0; i < 3; i++)
                {
                    if (globalSentence[index].Contains("GPRMC"))
                    {
                        ProcessDATFileGPRMC();
                    }
                    else if (globalSentence[index].Contains("GPGGA"))
                    {
                        if (mapCreated)
                        {
                            // Checking to see if we're using default current date/time.
                            if (None.Checked)
                            {
                                TheMap.GGASetCoordinates(globalSentence[index], null, GPSTimeOnly.Checked);
                            }
                            // Using user selected date/time.
                            else
                            {
                                TheMap.GGASetCoordinates(globalSentence[index], TempTime1, GPSTimeOnly.Checked);
                            }
                        }
                    }
                    else
                    {
                        if (mapCreated)
                        {
                            // Checking to see if we're using default current date/time.
                            if (None.Checked)
                            {
                                TheMap.PUBSetCoordinates(globalSentence[index], null, GPSTimeOnly.Checked);
                            }
                            // Using user selected date/time.
                            else
                            {
                                TheMap.PUBSetCoordinates(globalSentence[index], TempTime1, GPSTimeOnly.Checked);
                            }
                        }
                    }

                    tempSpeed = Math.Round(tempSpeed, 1);

                    if (mapCreated)
                    {
                        if (i == 2)
                        {
                            TheMap.SetLatLon(globalSentence[index]);
                            lat = TheMap.GetLat();
                            lon = TheMap.GetLon();

                            // Pinning the starting location.
                            if (startPos)
                            {
                                if ((lat != 0 && lon != 0) && (lat != 1E-06 && lon != 1E-06)            // Prevents putting pin on null/default coordinates.
                                    && (lat != -1E-06 && lon != -1E-06))
                                {
                                    map.Pin(lat, lon);
                                    startPos = false;
                                }
                            }
                            map.PlaceDot(lat, lon, Colors.Blue, tempSpeed);
                            currentCoordinates.Text = lat.ToString("0.000000") + ", " + lon.ToString("0.000000");
                        }
                    }

                    index++;
                    tsProgress.PerformStep();
                }
            }
        }

        //*****************************************************************************************
        // Name: DATVehicleData()
        // Description: Loads the lists into the global array.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2016-01-26 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-06-20 SC Yuen   If mapNotCreated is false, we can highlight the Bing map
        //*****************************************************************************************
        public void DATVehicleData()
        {
            if (mapCreated)
            {
                if (!done)
                {
                    Highlight();
                }
            }

            string[] theSentences = new string[lineCount];
            uint[] STD1Data = new uint[STD1Count];
            uint[] STD2Data = new uint[STD2Count];

            // Loading the list into an array.
            foreach (string parts in theList)
            {
                theSentences[numLine] = parts;
                numLine++;
            }

            // Loading STD1 list into an array.
            foreach (uint data1 in STD1List)
            {
                STD1Data[STD1Line] = data1;
                STD1Line++;
            }

            // Loading STD2 list into an array.
            foreach (uint data2 in STD2List)
            {
                STD2Data[STD2Line] = data2;
                STD2Line++;
            }

            // Loading local array onto global array since I couldn't initialize the global array directly and placing data into it.
            globalSentence = theSentences;
            globalSTD1Data = STD1Data;
            globalSTD2Data = STD2Data;
            numCheck = true;                // Set to true so we won't set the arrays again.
        }

        //*****************************************************************************************
        // Name: ProcessSelectedFile()
        // Description: Process the file that was selected
        //-----------------------------------------------------------------------------------------
        // Inputs: string fileName
        // Outputs: none
        // Returns: bool
        //*****************************************************************************************
        // 2017-04-21   SC Yuen         Initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-10   SC Yuen         Added progress bar when a file has been loaded. Resets the
        //                              ECM values when chkLoadFileResetECM is checked and setting
        //                              initialStartTime to true once a new file has been opened
        //-----------------------------------------------------------------------------------------
        // 2017-06-20   SC Yuen         Initialize the ECM data to be modifiable. If the map has
        //                              been disabled, no map will be generated once the new file
        //                              has been loaded. Setting the ability to modify the ECM data
        //                              according to which file has been loaded.
        //*****************************************************************************************
        public bool ProcessSelectedFile(string fileName)
        {
            bool validFileOpened = false;
            PinSet = false;
            mapCreated = false;
            numFuel.Enabled = true;
            numEngineHours.Enabled = true;
            numPTO.Enabled = true;
            numOdometer.Enabled = true;
            chkSetRPM.Checked = false;
            chkSetFuelRate.Checked = false;
            chkSetEngineLoad.Checked = false;
            chkSetFuelRate.Enabled = true;
            chkSetEngineLoad.Enabled = true;

            if (chkSetRPM.Checked)
            {
                numRPM.Enabled = true;
            }

            if (chkSetFuelRate.Checked)
            {
                numRate.Enabled = true;
            }

            if (chkSetEngineLoad.Checked)
            {
                numLoad.Enabled = true;
            }

            chkSetFuelRate.Enabled = true;
            chkSetEngineLoad.Enabled = true;

            if (fileName.Contains(".kml", StringComparison.OrdinalIgnoreCase))
            {
                // Reads KML files.
                KmlFile file = KmlFile.Load(File.OpenRead(fileName));
                Kml kmlTree = file.Root as Kml;

                if (kmlTree != null)
                {
                    route = new Route(fileName);
                    WPRoute = new Route(fileName);
                    startPos = true;

                    if (mMapOnStartup.Checked)
                    {
                        // if (there was a map already created) then
                        if (map != null)
                        {
                            // close it
                            map.Close();
                        }

                        // create a new map
                        map = new Map(TheMain);
                        mapCreated = true;
                    }

                    weAreProcessingAnNMEAFile = false;
                    weAreReadingADATFile = false;
                    validFileOpened = true;

                    tsProgress.Value = 0;

                    if (chkLoadFileResetECM.Checked)
                    {
                        numOdometer.Value = 10000;
                        numPTO.Value = 600;
                        numFuel.Value = 2000;
                        numEngineHours.Value = 500;
                    }

                    // Done is true when the Bing map has highlighted the entire route of the trip.
                    done = false;
                }
                else
                {
                    MessageBox.Show("Invalid KML file!\nPlease choose another file.");
                }
            }
            // Giving the Okay to read the NMEA file.
            else if (fileName.Contains(".nmea", StringComparison.OrdinalIgnoreCase))
            {
                SpeedUpDown.Enabled = false;
                tbThrottle.Enabled = false;
                weAreProcessingAnNMEAFile = true;
                weAreReadingADATFile = false;
                validFileOpened = true;
                startPos = true;
                NMEALineCount = 0;

                tsProgress.Value = 0;

                if (chkLoadFileResetECM.Checked)
                {
                    numOdometer.Value = 10000;
                    numPTO.Value = 600;
                    numFuel.Value = 2000;
                    numEngineHours.Value = 500;
                }

                // Done is true when the Bing map has highlighted the entire route of the trip.
                done = false;

                if (mMapOnStartup.Checked)
                {
                    // if (there was a map already created) then
                    if (map != null)
                    {
                        // close it
                        map.Close();
                    }

                    // create a new map
                    map = new Map(TheMain);
                    mapCreated = true;
                }
            }
            // Reads the DAT file by calling the DATReader function from the DATFileReader source file.
            else if (fileName.Contains(".dat", StringComparison.OrdinalIgnoreCase))
            {
                SpeedUpDown.Enabled = false;
                tbThrottle.Enabled = false;
                numFuel.Enabled = false;
                numEngineHours.Enabled = false;
                numPTO.Enabled = false;
                numOdometer.Enabled = false;
                numRate.Enabled = false;
                numLoad.Enabled = false;
                numRPM.Enabled = false;
                chkSetRPM.Enabled = false;
                chkSetFuelRate.Enabled = false;
                chkSetEngineLoad.Enabled = false;
                openDATFile = new DATFileReader();

                // Gets the number of lines from the file in order to get the right size for the array.
                lineCount = openDATFile.DATReader(NAVPort, fileName);

                STD1Count = openDATFile.GetSTD1Count();
                STD2Count = openDATFile.GetSTD2Count();
                DATLines = 0;

                // Gets the entire list of the sentences from the file.
                theList = openDATFile.TheListOfSentences();

                STD1List = openDATFile.GetSTD1List();
                STD2List = openDATFile.GetSTD2List();

                tsProgress.Value = 0;
                tsProgress.Maximum = lineCount;

                // If the counting isn't zero, that means the array contains something and we can start
                // printing the sentences.
                if (lineCount != 0)
                {
                    startPos = true;

                    if (mMapOnStartup.Checked)
                    {
                        // if (there was a map already created) then
                        if (map != null)
                        {
                            // close it
                            map.Close();
                        }

                        // create a new map
                        map = new Map(TheMain);
                        mapCreated = true;
                    }

                    weAreReadingADATFile = true;
                    weAreProcessingAnNMEAFile = false;
                    numCheck = false;   // numCheck is true when DATVehicleData() has been called.
                    numLine = 0;        // Index for the theSentences local array.
                    STD1Line = 0;       // Index for the STD1Data local array.
                    STD2Line = 0;       // Index for the STD2Data local array.
                    STD1Index = 0;      // Index for the globalSTD1Data global array.
                    STD2Index = 0;      // Index for the globalSTD2Data global array.
                    index = 0;          // Index for the globalSentence global array.
                    done = false;       // Done is true when the Bing map has highlighted the entire route of the trip.                       
                }

                validFileOpened = true;
            }

            initialStartTime = true;

            // Prevents the creation of another map when a file is running
            if (validFileOpened && mapCreated)
            {
                mapBtn.Enabled = false;
            }

            return validFileOpened;
        }

        //*****************************************************************************************
        // Name: GetMapBtnEnable()
        // Description: Returns modifiability of mapBtn.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-21 SC Yuen   initial version
        //*****************************************************************************************
        public bool GetMapBtnEnable()
        {
            return mapBtn.Enabled;
        }

        public void SetMapBtnEnabled()
        {
            mapBtn.Enabled = true;
            mapCreated = false;
        }

        //*****************************************************************************************
        // Name: OpeningTheFile()
        // Description: The actual bulk of the code that opens the file selected. Gets called from 
        //              openToolStripMenuItem_Click back in MainForm.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2016-01-25 SC Yuen   Edit to codes to use the vehicle values from the DAT file.
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen   Added code to print out the file that is currently loaded and
        //                      refactored code
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void OpeningTheFile()
        {
            bool validFileOpened = false;

            // This while loop will keep looping until the user has selected a valid kml/nmea/dat file or until they
            // decided to exit the file browser.
            while (validFileOpened == false)
            {
                TheFileName.Filter = "kml files (*.kml)|*.kml|" + "nmea files (*.nmea)|*.nmea|" +
                    "dat files (*.dat)|*.dat|" + "All files (*.*)|*.*";
                DialogResult result = TheFileName.ShowDialog();
                try
                {
                    // Check if user pressed the "OK" button.
                    if (result == DialogResult.OK)
                    {
                        validFileOpened = OpenFile(TheFileName.FileName);
                    }
                    else
                    {
                        // the user canceled - stop looping
                        validFileOpened = true;
                    }
                }
                catch (Exception)
                {
                    // there was an exception - stop looping
                    validFileOpened = true;
                }
            }
        }

        /// <summary>
        /// This will open and process the given file name.
        /// </summary>
        /// <param name="fileName">The file name to open.</param>
        /// <returns>True if the file was opened and processed successfully; false otherwise.</returns>
        public bool OpenFile(string fileName)
        {
            bool fileOpened = ProcessSelectedFile(fileName);

            // if (the file was opened and processed successfully) then
            if (fileOpened)
            {
                // keep track of the file name
                TheFileName.FileName = fileName;

                // display the file name
                FileLoaded.Text = fileName;

                // save this as the last file used
                Properties.Settings.Default.LastFileLoaded = fileName;
                Properties.Settings.Default.Save();

                // If the user has deselected the camera lock, it will re-enable it once a new file has been opened.
                if (CameraLockButton.Text == "Camera Lock On")
                {
                    TheCamLock();
                }
            }

            return fileOpened;
        }

        //*****************************************************************************************
        // Name: SavingTheFile()
        // Description: The actual bulk of the code that saves a kml into a nmea file. Called by
        //              saveAsToolStripMenuItem_Click method in MainForm.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-28 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen   Replaced all the SpeedUpDown with tempSpeed
        //*****************************************************************************************
        public void SavingTheFile()
        {
            if (!weAreProcessingAnNMEAFile && !weAreReadingADATFile && TheFileName.FileName != "")
            {
                saveFileDialog1.Filter = "nmea files (*.nmea)|*.nmea|All files (*.*)|*.*";
                DialogResult result = saveFileDialog1.ShowDialog();
                DateTime saveDate = DateTime.Now;
                if (result == DialogResult.OK)
                {
                    using (StreamWriter file = new StreamWriter(saveFileDialog1.FileName))
                    {
                        foreach (Vector position in route.Coordinates)
                        {
                            // Checking to see if we're using default current date/time.
                            if (None.Checked)
                            {
                                rmc = new GPRMC(Convert.ToDecimal(position.Latitude),
                                    Convert.ToDecimal(position.Longitude), saveDate, Convert.ToDouble(tempSpeed), false, null);
                                gga = new GPGGA(Convert.ToDecimal(position.Latitude),
                                    Convert.ToDecimal(position.Longitude), saveDate, false, null);
                                pub = new PUBX00(Convert.ToDecimal(position.Latitude),
                                    Convert.ToDecimal(position.Longitude), saveDate, false, null);
                            }
                            // Using user selected date/time.
                            else
                            {
                                rmc = new GPRMC(Convert.ToDecimal(position.Latitude),
                                    Convert.ToDecimal(position.Longitude), saveDate, Convert.ToDouble(tempSpeed), false, TempTime1);
                                gga = new GPGGA(Convert.ToDecimal(position.Latitude),
                                    Convert.ToDecimal(position.Longitude), saveDate, false, TempTime1);
                                pub = new PUBX00(Convert.ToDecimal(position.Latitude),
                                    Convert.ToDecimal(position.Longitude), saveDate, false, TempTime1);
                            }

                            saveDate = saveDate.AddSeconds(1);
                            file.WriteLine(rmc.ToString());
                            file.WriteLine(gga.ToString());
                            file.WriteLine(pub.ToString());
                        }
                    }
                    MessageBox.Show("NMEA file has been saved.");
                }
            }
            else if (weAreProcessingAnNMEAFile) MessageBox.Show("The file you loaded is already an .nmea file!");
            else if (weAreReadingADATFile) MessageBox.Show("Saving a .dat file into an .nmea file will be an upcoming feature!");
            else MessageBox.Show("There aren't any files loaded!");
        }

        //*****************************************************************************************
        // Name: Highlight()
        // Description: The function that grabs all the coordinates from the selected file and
        //              places them into a collection. The collection is then sent to another
        //              function called HighlightRoute in order to place the highlight on the map.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-11   SC Yuen     Setting the progress bar max value when a KML file has been
        //                          loaded
        //*****************************************************************************************
        public void Highlight()
        {
            MapPolyline polyline = new MapPolyline();
            polyline.Stroke = new System.Windows.Media.SolidColorBrush(Colors.Yellow);
            polyline.StrokeThickness = 6;
            polyline.Opacity = 0.7;
            LocationCollection locCollection = new LocationCollection();
            Microsoft.Maps.MapControl.WPF.Location theLoc;
            done = false;

            // When reading a KML file.
            if (TheFileName.FileName.Contains(".kml", StringComparison.OrdinalIgnoreCase))
            {
                lat = 0;
                lon = 0;
                WPRoute.resetCoordinateIndex();
                startPos = true;

                long countElements = route.GetListSize();
                CoordinateCollection lastCoord = route.GetLastCoordinates();
                Vector lastWaypoint = lastCoord.ElementAt((int)countElements);
                lastLat = lastWaypoint.Latitude;
                lastLon = lastWaypoint.Longitude;

                while (lat != lastLat && lon != lastLon)
                {
                    Vector position = WPRoute.getCurrentPosition((decimal).00833);
                    lat = position.Latitude;
                    lon = position.Longitude;
                    theLoc = new Microsoft.Maps.MapControl.WPF.Location(lat, lon);
                    locCollection.Add(theLoc);
                }

                tsProgress.Maximum = WPRoute.getCoordinateIndex();
                done = true;
            }
            // When reading a NMEA file.
            else if (TheFileName.FileName.Contains(".nmea", StringComparison.OrdinalIgnoreCase))
            {
                using (StreamReader read = new StreamReader(TheFileName.FileName))
                {
                    string r;

                    do
                    {
                        r = read.ReadLine();

                        if (r != null)
                        {
                            if (r.Contains("GPRMC"))
                            {
                                GPRMC rmc = new GPRMC(r, null);
                                lat = rmc.GetLat();
                                lon = rmc.GetLon();
                                theLoc = new Microsoft.Maps.MapControl.WPF.Location(lat, lon);
                                locCollection.Add(theLoc);
                            }
                        }
                    } while (r != null);

                    done = true;
                }
            }
            // When reading a DAT file.
            else if (TheFileName.FileName.Contains(".dat", StringComparison.OrdinalIgnoreCase))
            {
                // Going through each line of sentences to get the latitude and longitude of each point from the file.
                // theList contains all the sentences from the file.
                // theList was gotten from the OpeningTheFile function.
                foreach (string r in theList)
                {
                    if (r != null)
                    {
                        if (r.Contains("GPRMC"))
                        {
                            GPRMC rmc = new GPRMC(r, null);
                            lat = rmc.GetLat();
                            lon = rmc.GetLon();
                            theLoc = new Microsoft.Maps.MapControl.WPF.Location(lat, lon);
                            locCollection.Add(theLoc);
                        }
                    }
                }
                done = true;
            }

            map.FinishPin(lat, lon);
            polyline.Locations = locCollection;

            // HighlightRoute is the actual function that places the highlight on the map.
            map.HighlightRoute(polyline);
        }

        //*****************************************************************************************
        // Name: MapButton()
        // Description: If no data file is currently opened, this will generate a new Bing map that
        //              zooms out to view North America. This allows the user to set their own
        //              desired locations.  If a data file is currently opened, a new map is
        //              created using the data file as the source of location information.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-20   SC Yuen     Initial version
        //*****************************************************************************************
        public void MapButton()
        {
            if (string.IsNullOrEmpty(TheFileName.FileName))
            {
                // if (there was a map already created) then
                if (map != null)
                {
                    // close it
                    map.Close();
                }

                // create a new map
                map = new Map(TheMain, true);
                mapCreated = true;
            }
            else
            {
                // get the status of the timers
                Tuple<bool, bool> timerStatus = TheMain.GetTimerStatus();

                if (timerStatus.Item1)
                {
                    TheMain.PauseNmeaTimer();
                }

                if (timerStatus.Item1)
                {
                    TheMain.PauseJ1708Timer();
                }

                // if (there was a map already created) then
                if (map != null)
                {
                    // close it
                    map.Close();
                }

                // create a new map
                map = new Map(TheMain);
                mapCreated = true;
                done = false;

                if (timerStatus.Item1)
                {
                    TheMain.StartNmeaTimer();
                }

                if (timerStatus.Item1)
                {
                    TheMain.StartJ1708Timer();
                }
            }
        }

        // Work in progress.
        //public void Record()
        //{
        //    BinaryWriter Recording = new BinaryWriter(File.Open(dlgRecord.FileName, FileMode.Create));
        //}
    }

    /// <summary>
    /// This is an extension to the string class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// This is an extension of the string.Contains method. This allows for a
        /// StringComparison type when performing the string search.
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="substring">The string to search for.</param>
        /// <param name="comp">One of the StringComparison types.</param>
        /// <returns>True if the substring was found; false otherwise.</returns>
        public static bool Contains(this String str, String substring, StringComparison comp)
        {
            if (substring == null)
            {
                throw new ArgumentNullException("substring", "substring cannot be null.");
            }
            else if (!Enum.IsDefined(typeof(StringComparison), comp))
            {
                throw new ArgumentException("comp is not a member of StringComparison", "comp");
            }

            return str.IndexOf(substring, comp) >= 0;
        }
    }
}
