using SharpKml.Base;
using System;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using Trip_Simulator.NMEA;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Globalization;
using System.Collections.Specialized;
using System.Threading;
using Newtonsoft.Json;


namespace Trip_Simulator
{
    public partial class MainForm : Form
    {
        public const string CONNECT_TEXT = "Connect/Start Timer";
        public const string DISCONNECT_TEXT = "Disconnect";

        private const string ZOOM_250_FEET_TEXT = "250 feet";
        private const string ZOOM_2500_FEET_TEXT = "2500 feet";
        private const string ZOOM_1_MILE_TEXT = "1 mile";
        private const string ZOOM_2_MILES_TEXT = "2 miles";
        private const string ZOOM_5_MILES_TEXT = "5 miles";

        private const int INITIAL_RPM = 600;

        // NAV data and ports
        static SerialPort NAVPort;
        List<string> theList = new List<string> { };
        double lat = 0;
        double lon = 0;
        BinaryWriter Writing = null;
        BinaryReader Reading = null;
        FileStream PBF = null;
        SerialPort VehiclePort = null;
        MainMap TheMap;
        MainVehicle TheVehicle = new MainVehicle();
        MainFunctions TheFunctions = new MainFunctions();
        bool J1708PlaybackRunning = false;
        String LastGPSDateTime = DateTime.UtcNow.ToString("s");
        bool HACCEnabled = false;
        bool StartPlaybackDone = false;

        private bool mNmeaTimerStarted = false;
        private bool mJ1708TimerStarted = false;

        public double ZoomLevel { get; private set; }

        // invoke deep magic to stop screen flicker
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams handleParam = base.CreateParams;
                // set WS_EX_COMPOSITED flag, enabling double buffering for entire form
                handleParam.ExStyle |= 0x02000000;
                return handleParam;
            }
        }

        // Constructor
        public MainForm()
        {
            NAVPort = new SerialPort();
            NAVPort.BaudRate = 9600;
            VehiclePort = new SerialPort();
            VehiclePort.BaudRate = 9600;
            InitializeComponent();
            
            tsProgress.Width = statusStrip1.Width - 20;

            NAVComboBox.Items.AddRange(SerialPort.GetPortNames()
                .OrderBy(port => Convert.ToInt32(port.Replace("COM", string.Empty)))    // sort by port number
                .ToArray());
            VehicleComboBox.Items.AddRange(SerialPort.GetPortNames()
                .OrderBy(port => Convert.ToInt32(port.Replace("COM", string.Empty)))    // sort by port number
                .ToArray());

            if (Properties.Settings.Default.NAVPort == null)
            {
                Properties.Settings.Default.NAVPort = new StringCollection();
                Properties.Settings.Default.NAVPort.Add("");
            }

            if (Properties.Settings.Default.VehiclePort == null)
            {
                Properties.Settings.Default.VehiclePort = new StringCollection();
                Properties.Settings.Default.VehiclePort.Add("");
            }
            System.Console.WriteLine("Hola");
            // Loading the ports that were used before
            NAVComboBox.Text = Properties.Settings.Default.NAVPort[0];
            VehicleComboBox.Text = Properties.Settings.Default.VehiclePort[0];

            // Restoring ECM data
            numOdometer.Value = Properties.Settings.Default.Odometer;
            numFuel.Value = Properties.Settings.Default.TotalFuel;
            numPTO.Value = Properties.Settings.Default.TotalPTO;
            numEngineHours.Value = Properties.Settings.Default.TotalEngineHours;

            // restore map creation options
            mapOnStartupMenuItem.Checked = Properties.Settings.Default.EnableMap;
            setInitialZoomLevel();

            // restore engine on at startup option and related settings
            engineOnStartupMenuItem.Checked = Properties.Settings.Default.EngineOnAtStartup;
            chkVehicle.Checked = engineOnStartupMenuItem.Checked;
            chkVehicle_Click(this, null);

            // restore the braking rate
            if (Properties.Settings.Default.BrakeAt2Mph)
            {
                rbtnBrake2Mph.Checked = true;
            }
            else
            {
                rbtnBrake5Mph.Checked = true;
            }

            Version v = Assembly.GetExecutingAssembly().GetName().Version;

            // prints out the version number of the Trip Simulator.
            this.Text += string.Format(CultureInfo.InvariantCulture, @" - Version {0}.{1}.{2} (r{3})", v.Major, v.Minor, v.Build, v.Revision);

            // Copying this main object into the main object in TheFunctions.
            TheFunctions.SetMain(this);

            TheFunctions.SetValues(SpeedUpDown, numRPM, numOdometer, numLoad, numPTO, numRate,
                numFuel, chkBrk, rbtnBrake2Mph, rbtnBrake5Mph, chkPTO, chkVehicle, tbThrottle,
                tsProgress, ConnectButton, CameraLockButton, lstOutput, dlgRecord, dlgPlayback,
                statusStrip1, grpSpdDist, grpFuel, grpPedals, mnuStopRecording, mnuRecordTo,
                mnuPlaybackFrom, mnuStopPlayback, NAVPort, VehiclePort, TheFileName, lblTimeAtSpeed,
                saveFileDialog1, Writing, Reading, PBF, HACCEnabled, HACCNum.Value, CurrentTime,
                radioStart, chkStop, StartDate, StopDate, countDown, StartPlaybackDone, radioSetPlaybackTime,
                radioNone, chkNoRPM, GPSTimeOnly, chkNoSpeed, VehicleSpeed, chkNoOdo, chkNoTEH,
                numEngineHours, FileLoaded, VehicleComboBox, NAVComboBox, CurrentCoordinates,
                chkLoadFileResetECM, TimeStarted, mapOnStartupMenuItem, chkSetRPM, chkSetFuelRate,
                chkSetEngineLoad, MapBtn, chkLoopKmlFile);

            // restore reload last file at startup options
            reloadFileAtStartupMenuItem.Checked = Properties.Settings.Default.ReloadFileAtStartup;
            /*Sistema de lectura de json
             * Esquema:
             * { 
             *      "file":
             *      "speed":
             * }
             * 
             * 
             * 
             */
            string json = File.ReadAllText("C:/Users/Jose.Valadez/Desktop/Selenium/mobile-automation/TripSim.json");
            trip datos =    JsonConvert.DeserializeObject<trip>(json);
            string route = datos.file;
            int speed= datos.speed;


            string fileName = route;
            bool fileOpened = TheFunctions.OpenFile(fileName);

            // if (we successfully opened and processed the file) then
            if (fileOpened)
            {
                // start processing
                ConnectButton_Click(this, null);
                chkVehicle.Checked = true;
                chkVehicle_Click(this, null);
                SpeedUpDown.Value = speed;
            }
            
            // if (we should reload the last file used AND there is a file name to use) then


        }



        //*****************************************************************************************
        // Name: GetVehicle()
        // Description: Gets the TheVehicle object and returns it.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: TheVehicle
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public MainVehicle GetVehicle()
        {
            return TheVehicle;
        }

        //*****************************************************************************************
        // Name: GetFunctions()
        // Description: Gets the TheFunctions object and returns it.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: TheFunctions
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public MainFunctions GetFunctions()
        {
            return TheFunctions;
        }

        //*****************************************************************************************
        // Name: GetMap()
        // Description: Gets the TheMap object and returns it.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: TheMap
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public MainMap GetMap()
        {
            return TheMap;
        }

        //*****************************************************************************************
        // Name: ConnectButton_Click(object, EventArgs)
        // Description: When user clicks on the button while it has "Connect" on it, by default it
        //              will attempt to connect to the NAV port that the user has selected. If none
        //              were selected, it will prompt the user to select one. If user has checked
        //              the "Vehicle Data" checkbox and hasn't selected a vehicle port when
        //              clicking on the "Connect" button, it will prompt the user to select a one.
        //              If user has selected all the necessary port(s), when clicking on the
        //              "Connect" button the Trip Sim will connect to the ports that were selected.
        //              The "Connect" button will also turn into a "Disconnect" button.
        //              When user clicks on the button while it has "Disconnect" on it, it will
        //              disconnect from the connected ports the user has selected before. It will
        //              also go from "Disconnect" back to "Connect".
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2016-01-22 SC Yuen   Edits to code to not have NAV port as a constraint
        //-----------------------------------------------------------------------------------------
        // 2016-02-03 SC Yuen   Added codes for the start and stop timer for the playback
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen   Saving port settings
        //-----------------------------------------------------------------------------------------
        // 2017-06-21 SC Yuen   Setting the OnOffBtn according to the state of the ConnectButton
        //                      when it got clicked on
        //*****************************************************************************************
        public void ConnectButton_Click(object sender, EventArgs e)
        {
            // Connects the port and starting the correct timers.
            if (ConnectButton.Text == CONNECT_TEXT && radioNone.Checked)
            {
                // Checking to make sure at least NAVComboBox has been selected, or/and if the chkVehicle button has been checked and the VehicleComboBox has been selected too.
                if ((NAVComboBox.Text != "" && VehicleComboBox.Text != "" && chkVehicle.Checked) ||
                    (!chkVehicle.Checked && NAVComboBox.Text != "") ||
                    (VehicleComboBox.Text != "" && chkVehicle.Checked))
                {
                    if (NAVComboBox.Text != "")
                    {
                        NAVPort.PortName = NAVComboBox.Text;
                        Properties.Settings.Default.NAVPort.Clear();
                        Properties.Settings.Default.NAVPort.Add(NAVComboBox.Text);
                        Properties.Settings.Default.Save();
                    }

                    if (VehicleComboBox.Text != "" && chkVehicle.Checked)
                    {
                        VehiclePort.PortName = VehicleComboBox.Text;
                        Properties.Settings.Default.VehiclePort.Clear();
                        Properties.Settings.Default.VehiclePort.Add(VehicleComboBox.Text);
                        Properties.Settings.Default.Save();
                    }

                    // Making sure that the NAVComboBox and the VehicleComboBox don't have the same port selected.
                    if (NAVComboBox.Text != VehicleComboBox.Text)
                    {
                        try
                        {
                            // Start the timer specifically for doing a J1708 playback.
                            if (J1708PlaybackRunning)
                            {
                                J1708Timer.Start();
                            }

                            // If not doing a playback of a J1708 file restart the NmeaTimer and open the NAVPort.
                            else if (NAVComboBox.Text != "")
                            {
                                NAVPort.Open();
                                LeapTimer.Stop();
                                NmeaTimer.Start();
                            }

                            // Starting the NmeaTimer and opening the VehiclePort if the VehicleComboBox isn't empty and the chkVehicle box has been checked.
                            if (VehicleComboBox.Text != "" && chkVehicle.Checked)
                            {
                                VehiclePort.Open();
                                NmeaTimer.Start();
                            }

                            NAVComboBox.Enabled = false;
                            VehicleComboBox.Enabled = false;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Unable to access port.\nPlease select another port.");
                            return;
                        }
                    }

                    // else VehicleComboBox.Text == NAVComboBox.Text
                    else
                    {
                        MessageBox.Show("Nav port and Vehicle port cannot be the same.\nPlease select different ports.");
                        return;
                    }
                }
                else if (!chkVehicle.Checked && NAVComboBox.Text == "")
                {
                    MessageBox.Show("No NAV port found!\nPlease select a NAV port.");
                    return;
                }
                else if (chkVehicle.Checked && VehicleComboBox.Text == "")
                {
                    MessageBox.Show("No Vehicle port found!\nPlease select a Vehicle port.");
                    return;
                }
            }

            // Check to see if chkStart has been checked.
            else if (ConnectButton.Text == CONNECT_TEXT && radioStart.Checked)
            {
                Playback_Starter();
                return;
            }

            // Check to see if radioSetPlaybackTime has been checked.
            else if (ConnectButton.Text == CONNECT_TEXT && radioSetPlaybackTime.Checked)
            {
                SetTimeForPlayback();
                return;
            }

            // Disconnect from the port and stopping the timers.
            else
            {
                NmeaTimer.Stop();
                mNmeaTimerStarted = false;
                StopJ1708Timer(false);
                LeapTimer.Stop();
                NAVPort.Close();
                VehiclePort.Close();

                NAVComboBox.Enabled = true;
                VehicleComboBox.Enabled = true;
            }

            // Changing the text on the button.
            if (ConnectButton.Text == CONNECT_TEXT)
            {
                ConnectButton.Text = DISCONNECT_TEXT;
                StartPlaybackDone = true;
                OnOffBtn.Text = "RUNNING";
                OnOffBtn.BackColor = System.Drawing.Color.Green;
            }
            else
            {
                ConnectButton.Text = CONNECT_TEXT;
                StartPlaybackDone = false;
                OnOffBtn.Text = "OFF";
                OnOffBtn.BackColor = System.Drawing.Color.Red;
            }
        }

        //*****************************************************************************************
        // Name: TripFinished()
        // Description: Sets the Trip Sim indicator to OFF when a trip finishes.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-22 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void TripFinished()
        {
            ConnectButton.Text = CONNECT_TEXT;
            StartPlaybackDone = false;
            OnOffBtn.Text = "OFF";
            OnOffBtn.BackColor = System.Drawing.Color.Red;
            VehicleSpeed.Text = "0.0";
            tbThrottle.Value = 0;
            SpeedUpDown.Value = 0;
            numRPM.Value = INITIAL_RPM;
            Application.Exit();
        }

        //*****************************************************************************************
        // Name: Playback_Starter()
        // Description: Check and make sure that the scheduled start time is not before the current
        //              time or before the scheduled stop time. Also, make sure most of the needed
        //              setups are done before starting the NmeaTimer.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2016-02-03 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void Playback_Starter()
        {
            // Check to see the scheduled start time is valid.
            if (StartDate.Value <= DateTime.Now)
            {
                MessageBox.Show("Please choose a later start date/time.");
                return;
            }

            CheckPorts();
            NmeaTimer.Start();
        }

        //*****************************************************************************************
        // Name: SetTimeForPlayback()
        // Description: When user wants to select their own custom date/time for playback.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2016-02-05 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void SetTimeForPlayback()
        {
            // Check to see if the scheduled stop time is valid or not if the chkStop is checked.
            if (chkStop.Checked && StopDate.Value <= StartDate.Value)
            {
                MessageBox.Show("Please choose a later stop date/time.");
                return;
            }

            ConnectButton.Text = DISCONNECT_TEXT;
            CheckPorts();
            NmeaTimer.Start();
        }

        //*****************************************************************************************
        // Name: CheckPorts()
        // Description: Checking to see if the ports are valid or not when user either wants to set
        //              a time to start a playback, or set their own specific date/time for
        //              playback.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2016-02-05 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void CheckPorts()
        {
            // Checking to make sure at least NAVComboBox has been selected, or/and if the chkVehicle button has been checked and the VehicleComboBox has been selected too.
            if ((NAVComboBox.Text != "" && VehicleComboBox.Text != "" && chkVehicle.Checked) ||
                (!chkVehicle.Checked && NAVComboBox.Text != "") ||
                (VehicleComboBox.Text != "" && chkVehicle.Checked))
            {
                if (NAVComboBox.Text != "")
                {
                    NAVPort.PortName = NAVComboBox.Text;
                }

                if (VehicleComboBox.Text != "" && chkVehicle.Checked)
                {
                    VehiclePort.PortName = VehicleComboBox.Text;
                }

                StartPlaybackDone = false;        
            }

            // Check to see if both ports are empty when the chkVehicle is not checked.
            else if (!chkVehicle.Checked && NAVComboBox.Text == "")
            {
                MessageBox.Show("No NAV port found!\nPlease select a NAV port.");
                return;
            }

            // Check to see if the vehicle port is empty or not when chkVehicle is checked.
            else if (chkVehicle.Checked && VehicleComboBox.Text == "")
            {
                MessageBox.Show("No Vehicle port found!\nPlease select a Vehicle port.");
                return;
            }
        }

        //*****************************************************************************************
        // Name: CameraLockButton_Click(object, EventArgs)
        // Description: When user clicks on the button while it has "Camera Lock Off", it will
        //              cause the camera to lock-on the latest position of the blue dots on the
        //              map, which represents the current position of the simulated vehicle. The
        //              button will turn into "Camera Lock On" too. When user clicks on the button
        //              while it has "Camera Lock On", it will cause it to stop locking onto the
        //              latest position of the blue dots and change to "Camera Lock Off". The
        //              actual code that does all these are in the MainFunctions' TheCamLock
        //              method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void CameraLockButton_Click(object sender, EventArgs e)
        {
            TheFunctions.TheCamLock();
        }

        //*****************************************************************************************
        // Name: GetUpdateCoordinates()
        // Description: Gets the updated coordinates by calling the MainMap's UpdateCoordinates
        //              method and then after receiving the updated coordinates, it returns it as a
        //              Microsoft.Maps.MapControl.WPF.Location type.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: loc
        //*****************************************************************************************
        // 2015-10-27 SC Yuen   initial version
        //-----------------------------------------------------------------------------------------
        // 2017-06-21 SC Yuen   Setting the default coordinates
        //*****************************************************************************************
        public Microsoft.Maps.MapControl.WPF.Location GetUpdateCoordinates()
        {
            if (lat == 0 && lon == 0)
            {
                lat = 45;
                lon = -95;
            }
            TheMap = new MainMap(lat, lon);
            Microsoft.Maps.MapControl.WPF.Location loc = TheMap.UpdateCoordinates(lat, lon);
            return loc;
        }

        //*****************************************************************************************
        // Name: Leaper_Tick(object, EventArgs)
        // Description: Activates the GPRMC(), GPGGA(), and PUBX00() with the input of "true". The
        //              "true" will cause those functions to use time of 23:59:60. This is in order
        //              to make sure that the Relay can handle a value of 60 seconds, which is
        //              present during leap seconds.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void Leaper_Tick(object sender, EventArgs e)
        {
            Vector position = new Vector();
            DateTime time = new DateTime(2015, 06, 30);

            try
            {
                GPRMC rmc = new GPRMC(Convert.ToDecimal(position.Latitude),
                    Convert.ToDecimal(position.Longitude), time, 0, true, null);
                NAVPort.WriteLine(rmc.ToString());
                Console.WriteLine(rmc.ToString());
                GPGGA gga = new GPGGA(Convert.ToDecimal(position.Latitude),
                    Convert.ToDecimal(position.Longitude), time, true, null);
                NAVPort.WriteLine(gga.ToString());
                Console.WriteLine(gga.ToString());
                PUBX00 pubx = new PUBX00(Convert.ToDecimal(position.Latitude),
                    Convert.ToDecimal(position.Longitude), time, true, null);
                NAVPort.WriteLine(pubx.ToString());
                Console.WriteLine(pubx.ToString());

                NmeaTimer.Stop();
                mNmeaTimerStarted = false;
                StopJ1708Timer(false);
                LeapTimer.Start();
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("No port connected!\nPlease select a port and connect it.");
            }
        }

        //*****************************************************************************************
        // Name: StopLeaper_Tick(object, EventArgs)
        // Description: Stops the LeaperTimer and if J1708 Recording/Playback isn't running it will
        //              start the NmeaTimer back up.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void StopLeaper_Tick(object sender, EventArgs e)
        {
            LeapTimer.Stop();
            if (J1708PlaybackRunning) J1708Timer.Start();
            else NmeaTimer.Start();
        }

        //*****************************************************************************************
        // Name: NmeaTimer_Tick(object, EventArgs)
        // Description: Prints out the RMC, GGA, and PUBX00 sentences each second. Also activates
        //              Pin() when startPos is "false". The startPos is "false" represents that it
        //              is in the starting position. Pin() will place a pin at that coordinate.
        //              After that it will activate PlaceDot() to place blue dots at current
        //              coordinates. When chkBrk has been checked, it will subtract 5mph every tick
        //              in order to simulate a truck/vehicle braking. It will set the truck/vehicle
        //              data if chkVehicle has been checked. The actual code that does all these
        //              are in the MainFunctions' TheNMEATimer method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen       initial version
        //-----------------------------------------------------------------------------------------
        // 2016-01-19 JJ Hietpas    Added codes to fix the timing issue
        //-----------------------------------------------------------------------------------------
        // 2016-11-30 SC Yuen       Added new parameters of chkNoSpeed and GPSTimeOnly for
        //                          TheFunctions.SetValues()
        //-----------------------------------------------------------------------------------------
        // 2017-04-21 SC Yuen       Updated the arguments needed for TheFunctions.SetValues(),
        //                          added VehicleSpeed, chkNoOdo, chkNoTEH, numEngineHours,
        //                          FileLoaded, VehicleComboBox, NAVComboBox
        //-----------------------------------------------------------------------------------------
        // 2017-05-10 SC Yuen       Updated the arguments needed for TheFunctions.SetValues(),
        //                          added chkLoadFileResetECM, TimeStarted
        //-----------------------------------------------------------------------------------------
        // 2017-05-25 SC Yuen       Updated the arguments needed for TheVehicle.SetValues()
        //-----------------------------------------------------------------------------------------
        // 2017-06-21 SC Yuen       Updated the argurments needed for TheFunctions.SetValues() and
        //                          TheVehicle.SetValues()
        //-----------------------------------------------------------------------------------------
        // 2018-03-22 JC Anderson   adding option to loop over kml file
        //*****************************************************************************************
        public void NmeaTimer_Tick(object sender, EventArgs e)
        {
            mNmeaTimerStarted = true;

            if (LastGPSDateTime == DateTime.UtcNow.ToString("s"))
                return;
            LastGPSDateTime = DateTime.UtcNow.ToString("s");

            if (ChkBoxHACC.Checked) HACCEnabled = true;
            else HACCEnabled = false;
            TheVehicle.SetMain(this);
            TheFunctions.SetValues(SpeedUpDown, numRPM, numOdometer, numLoad, numPTO, numRate, numFuel,
                chkBrk, rbtnBrake2Mph, rbtnBrake5Mph, chkPTO, chkVehicle, tbThrottle, tsProgress,
                ConnectButton, CameraLockButton, lstOutput, dlgRecord, dlgPlayback, statusStrip1,
                grpSpdDist, grpFuel, grpPedals, mnuStopRecording, mnuRecordTo, mnuPlaybackFrom,
                mnuStopPlayback, NAVPort, VehiclePort, TheFileName, lblTimeAtSpeed, saveFileDialog1,
                Writing, Reading, PBF, HACCEnabled, HACCNum.Value, CurrentTime, radioStart, chkStop,
                StartDate, StopDate, countDown, StartPlaybackDone, radioSetPlaybackTime, radioNone,
                chkNoRPM, GPSTimeOnly, chkNoSpeed, VehicleSpeed, chkNoOdo, chkNoTEH, numEngineHours,
                FileLoaded, VehicleComboBox, NAVComboBox, CurrentCoordinates, chkLoadFileResetECM,
                TimeStarted, mapOnStartupMenuItem, chkSetRPM, chkSetFuelRate, chkSetEngineLoad,
                MapBtn, chkLoopKmlFile);

            TheVehicle.SetValues(SpeedUpDown, numRPM, numOdometer, numLoad, numPTO, numRate, numFuel,
                chkBrk, chkPTO, chkVehicle, tbThrottle, tsProgress, ConnectButton, lstOutput,
                numEngineHours, dlgRecord, dlgPlayback, statusStrip1, grpSpdDist, grpFuel, grpPedals,
                mnuStopRecording, mnuRecordTo, mnuPlaybackFrom, mnuStopPlayback, VehiclePort, Writing,
                Reading, PBF, chkSetRPM, chkSetFuelRate, chkSetEngineLoad);
            TheFunctions.TheNMEATimer();            
        }

        //*****************************************************************************************
        // Name: J1708Timer_Tick(object, EventArgs)
        // Description: The timer for when doing a playback of a J1708 file. This timer is needed
        //              because we can't use the NmeaTimer. Using the NmeaTimer will cause overlaps
        //              in the J1708 data from reading the J1708 file, and running and reading a 
        //              KML, NMEA, or DAT file at the same time.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void J1708Timer_Tick(object sender, EventArgs e)
        {
            mJ1708TimerStarted = true;

            // If playback is enabled do the playback process.
            if (J1708PlaybackRunning)
            {
                TheVehicle.PlaybackTruck();
                TheVehicle.SetNMEATimerStatus(true);

                if (chkVehicle.Checked && VehiclePort.IsOpen)
                {
                    TheVehicle.UpdateTruck();
                }

                TheVehicle.Update_J1708();
            }

            // Else just update the vehicle data.
            else
            {
                TheVehicle.UpdateTruck();
                TheVehicle.SetNMEATimerStatus(true);
                TheVehicle.Update_J1708();
            }
        }

        //*****************************************************************************************
        // Name: SetJ1708Timer()
        // Description: Starts and stops the J1708Timer. If NmeaTimer is enabled when this
        //              function gets called, it will stop the NmeaTimer.
        //-----------------------------------------------------------------------------------------
        // Inputs: bool playback
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-11-05 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void SetJ1708Timer(bool playback)
        {
            // If the NmeaTimer is running and playback is enabled, it will stop the NmeaTimer and start the J1708Timer.
            if (NmeaTimer.Enabled && playback)
            {
                J1708Timer.Start();
                J1708PlaybackRunning = true;
                NmeaTimer.Stop();
                mNmeaTimerStarted = false;
            }

            // If the NmeaTimer is running and playback is not enabled, just start the J1708Timer.
            else if (NmeaTimer.Enabled && !playback)
            {
                J1708Timer.Start();
            }

            // If none of the above then, stop the J1708Timer.
            else
            {
                StopJ1708Timer(true);
            }
        }

        //*****************************************************************************************
        // Name: StopJ1708Timer()
        // Description: Stops the J1708Timer.
        //-----------------------------------------------------------------------------------------
        // Inputs: bool clearPlaybackFlag
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-04-21 SC Yuen   Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void StopJ1708Timer(bool clearPlaybackFlag)
        {
            J1708Timer.Stop();
            mJ1708TimerStarted = false;

            if (clearPlaybackFlag)
            {
                J1708PlaybackRunning = false;
            }

            // clear the J1708 output window to show that no vehicle data is being generated
            lstOutput.Items.Clear();
        }

        //*****************************************************************************************
        // Name: openToolStripMenuItem_Click(object, EventArgs)
        // Description: Lets user search for all file types, but can only open kml, nmea, and dat
        //              file types.  There's also a filter that only search for one of the openable
        //              file types at a time too. The actual code that does all these are in the
        //              MainFunctions' OpeningTheFile method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TheFunctions.OpeningTheFile();            
        }

        //*****************************************************************************************
        // Name: mnuRecordTo_Click(object, EventArgs)
        // Description: Sets the recording of the truck/vehicle data to be true to start recording.
        //              Also, writes the truck/vehicle data into a .J1708 file.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void mnuRecordTo_Click(object sender, EventArgs e)
        {
            try
            {
                Writing = TheVehicle.RecordItTo();
            }
            catch
            {
                MessageBox.Show("No file is open!\nPlease open a file and run it first before trying to record it.");
            }
        }

        //*****************************************************************************************
        // Name: stopRecordingToolStripMenuItem_Click(object, EventArgs)
        // Description: Stops the recording of the truck/vehicle data.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void stopRecordingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ConnectButton_Click(null, null);
            Writing = TheVehicle.StopTheRecording();
        }

        //*****************************************************************************************
        // Name: mnuPlaybackFrom_Click(object, EventArgs)
        // Description: Sets the playback of the truck/vehicle data to be true. Also, reads the
        //              data from the .J1708 file that was selected for playback.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void mnuPlaybackFrom_Click(object sender, EventArgs e)
        {
            try
            {
                if(VehiclePort.IsOpen)
                {
                    Reading = TheVehicle.DoThePlaybackFrom();
                    PBF = TheVehicle.GetPlaybackF();
                }
                else MessageBox.Show("Vehicle Port not selected!\nPlease select a Vehicle Port.");
            }
            catch
            {
                MessageBox.Show("Ports aren't connected!\nPlease connect either the NAV Port and/or the Vehicle Port.");
            }
        }

        //*****************************************************************************************
        // Name: mnuStopPlayback_Click(object, EventArgs)
        // Description: Stops the playback of the truck/vehicle data.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void mnuStopPlayback_Click(object sender, EventArgs e)
        {
            ConnectButton_Click(null, null);
            TheVehicle.StopThePlayback();
            Reading = null;
        }

        //*****************************************************************************************
        // Name: helpToolStripMenuItem_Click(object, EventArgs)
        // Description: Gives instructions to the user as to how to start using this simulator with
        //              the Relay.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Connect GPS NMEA COM port on laptop to the SERIAL port of Relay breakout box and use Configuration Constant dbggps=1.\n\n"
                + "To set dbggps=1, first connect breakout box DEBUG port to the COM port on laptop."
                + "\nNext open a command terminal and login into the Relay, login: root, password: x@t@1"
                + "\nType in 'cd /xataturnpike/bin' without the quotations. Type in 'ls' after that and the terminal shall show you the contents in"
                + " this directory.\nLocate XRSConstants to confirm that it's in this directory. Once you have confirm that XRSConstants is there,"
                + " type in 'echo dbggps=1 >>/xataturnpike/bin/XRSConstants' to set dbggps to 1.\nType in 'cat XRSConstants' to confirm that dbggps is indeed 1."
                + "\nNow, type in 'reboot' and wait until the Relay has finished rebooting."
                + "\n\nAfter the Relay has finished rebooting, you can connect GPS NMEA COM port on laptop to the SERIAL port of Relay breakout box and start your simulation.");
        }

        //*****************************************************************************************
        // Name: saveAsToolStripMenuItem_Click(object, EventArgs)
        // Description: Saves the results into a nmea file. Actual code is in TheFunctions'
        //              SavingTheFile method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-09-29 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TheFunctions.SavingTheFile();
        }

        //*****************************************************************************************
        // Name: tbTrhottle_Scroll(object, EventArgs)
        // Description: Sets the minimum and maximum value for the throttle, and in turn it will
        //              set the value of the SpeedUpDown to the value of tbThrottle.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void tbThrottle_Scroll(object sender, EventArgs e)
        {
            tbThrottle.Minimum = 0;
            tbThrottle.Maximum = MainVehicle.MAX_SPEED;
            SpeedUpDown.Value = tbThrottle.Value;
        }

        //*****************************************************************************************
        // Name: ECMClearButton_Click(object, EventArgs)
        // Description: Sets the ECM values to be their default values
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-05-10       SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void ECMClearButton_Click(object sender, EventArgs e)
        {
            tbThrottle.Value = 0;
            SpeedUpDown.Value = 0;
            VehicleSpeed.Text = "0.0";
            numOdometer.Value = 10000;
            numFuel.Value = 2000;
            numPTO.Value = 600;
            numEngineHours.Value = 500;
        }

        //*****************************************************************************************
        // Name: MapBtn_Click(object, EventArgs)
        // Description: Clicking this button will open a new Bing map. Initial start for new
        //              feature #3.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-05-25       SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void MapBtn_Click(object sender, EventArgs e)
        {
            TheFunctions.MapButton();
        }

        //*****************************************************************************************
        // Name: chkSetRPM_CheckedChanged(object, EventArgs)
        // Description: Checking this will enable the user to set their desired engine speed.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-21      SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void chkSetRPM_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSetRPM.Checked)
            {
                numRPM.Enabled = true;
            }
            else
            {
                numRPM.Enabled = false;
            }
        }

        //*****************************************************************************************
        // Name: ChkBoxHACC_CheckedChanged(object, EventArgs)
        // Description: Checking this will let the user to set their desired horizontal accuracy.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-21      SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void ChkBoxHACC_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkBoxHACC.Checked)
            {
                HACCNum.Enabled = true;
            }
            else
            {
                HACCNum.Enabled = false;
            }
        }

        //*****************************************************************************************
        // Name: chkSetFuelRate_CheckedChanged(object, EventArgs)
        // Description: Checking this will enable the user to set their desired fuel rate.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-21      SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void chkSetFuelRate_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSetFuelRate.Checked)
            {
                numRate.Enabled = true;
            }
            else
            {
                numRate.Enabled = false;
            }
        }

        //*****************************************************************************************
        // Name: chkSetEngineLoad_CheckedChanged(object, EventArgs)
        // Description: Checking this will enable the user to set their desired engine load.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2017-06-21      SC Yuen     Initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void chkSetEngineLoad_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSetEngineLoad.Checked)
            {
                numLoad.Enabled = true;
            }
            else
            {
                numLoad.Enabled = false;
            }
        }

        /// <summary>
        /// This will stop the NMEA timer and reset the NMEA timer started flag.
        /// </summary>
        public void StopNmeaTimer()
        {
            NmeaTimer.Stop();
            mNmeaTimerStarted = false;
        }

        /// <summary>
        /// This will stop the NMEA timer but does not reset the NMEA timer started flag.
        /// </summary>
        public void PauseNmeaTimer()
        {
            NmeaTimer.Stop();
        }

        /// <summary>
        /// This will start the NMEA timer.
        /// </summary>
        public void StartNmeaTimer()
        {
            NmeaTimer.Start();
        }

        /// <summary>
        /// This will stop the J1708 timer but does not reset the J1708 playback flags.
        /// </summary>
        public void PauseJ1708Timer()
        {
            J1708Timer.Stop();
        }

        /// <summary>
        /// This will start the J1708 timer.
        /// </summary>
        public void StartJ1708Timer()
        {
            J1708Timer.Start();
        }

        /// <summary>
        /// This will get the NMEA timer status and the J1708 timer status.
        /// </summary>
        /// <returns>
        /// A Tuple indicating the the NMEA timer status and the J1708 timer status.
        /// The first value is the NMEA timer started indicator.
        /// The second value is the J1708 timer started indicator.
        /// </returns>
        public Tuple<bool, bool> GetTimerStatus()
        {
            return new Tuple<bool, bool>(mNmeaTimerStarted, mJ1708TimerStarted);
        }

        /// <summary>
        /// This gets invoked when the Display Map at Startup menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mapOnStartupMenuItem_Click(object sender, EventArgs e)
        {
            mapOnStartupMenuItem.Checked = !mapOnStartupMenuItem.Checked;
            Properties.Settings.Default.EnableMap = mapOnStartupMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// This gets invoked when the Initial Zoom Level menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void zoomMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem menuItem = (ToolStripMenuItem)sender;
            string zoomLevel = menuItem.Text;

            switch (zoomLevel)
            {
                case ZOOM_2500_FEET_TEXT:
                    ZoomLevel = Map.ZOOM_2500_FEET;
                    zoom250FeetMenuItem.Checked = false;
                    zoom2500FeetMenuItem.Checked = true;
                    zoom1MileMenuItem.Checked = false;
                    zoom2MilesMenuItem.Checked = false;
                    zoom5MilesMenuItem.Checked = false;
                    break;

                case ZOOM_1_MILE_TEXT:
                    ZoomLevel = Map.ZOOM_1_MILE;
                    zoom250FeetMenuItem.Checked = false;
                    zoom2500FeetMenuItem.Checked = false;
                    zoom1MileMenuItem.Checked = true;
                    zoom2MilesMenuItem.Checked = false;
                    zoom5MilesMenuItem.Checked = false;
                    break;

                case ZOOM_2_MILES_TEXT:
                    ZoomLevel = Map.ZOOM_2_MILES;
                    zoom250FeetMenuItem.Checked = false;
                    zoom2500FeetMenuItem.Checked = false;
                    zoom1MileMenuItem.Checked = false;
                    zoom2MilesMenuItem.Checked = true;
                    zoom5MilesMenuItem.Checked = false;
                    break;

                case ZOOM_5_MILES_TEXT:
                    ZoomLevel = Map.ZOOM_5_MILES;
                    zoom250FeetMenuItem.Checked = false;
                    zoom2500FeetMenuItem.Checked = false;
                    zoom1MileMenuItem.Checked = false;
                    zoom2MilesMenuItem.Checked = false;
                    zoom5MilesMenuItem.Checked = true;
                    break;

                case ZOOM_250_FEET_TEXT:
                default:
                    ZoomLevel = Map.ZOOM_250_FEET;
                    zoom250FeetMenuItem.Checked = true;
                    zoom2500FeetMenuItem.Checked = false;
                    zoom1MileMenuItem.Checked = false;
                    zoom2MilesMenuItem.Checked = false;
                    zoom5MilesMenuItem.Checked = false;
                    break;
            }

            Properties.Settings.Default.ZoomLevel = zoomLevel;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// This will set the Zoom Level menu items based on the saved setting.
        /// </summary>
        private void setInitialZoomLevel()
        {
            string zoomLevel = Properties.Settings.Default.ZoomLevel;

            switch (zoomLevel)
            {
                case ZOOM_2500_FEET_TEXT:
                    ZoomLevel = Map.ZOOM_2500_FEET;
                    zoom250FeetMenuItem.Checked = false;
                    zoom2500FeetMenuItem.Checked = true;
                    zoom1MileMenuItem.Checked = false;
                    zoom2MilesMenuItem.Checked = false;
                    zoom5MilesMenuItem.Checked = false;
                    break;

                case ZOOM_1_MILE_TEXT:
                    ZoomLevel = Map.ZOOM_1_MILE;
                    zoom250FeetMenuItem.Checked = false;
                    zoom2500FeetMenuItem.Checked = false;
                    zoom1MileMenuItem.Checked = true;
                    zoom2MilesMenuItem.Checked = false;
                    zoom5MilesMenuItem.Checked = false;
                    break;

                case ZOOM_2_MILES_TEXT:
                    ZoomLevel = Map.ZOOM_2_MILES;
                    zoom250FeetMenuItem.Checked = false;
                    zoom2500FeetMenuItem.Checked = false;
                    zoom1MileMenuItem.Checked = false;
                    zoom2MilesMenuItem.Checked = true;
                    zoom5MilesMenuItem.Checked = false;
                    break;

                case ZOOM_5_MILES_TEXT:
                    ZoomLevel = Map.ZOOM_5_MILES;
                    zoom250FeetMenuItem.Checked = false;
                    zoom2500FeetMenuItem.Checked = false;
                    zoom1MileMenuItem.Checked = false;
                    zoom2MilesMenuItem.Checked = false;
                    zoom5MilesMenuItem.Checked = true;
                    break;

                case ZOOM_250_FEET_TEXT:
                default:
                    ZoomLevel = Map.ZOOM_250_FEET;
                    zoom250FeetMenuItem.Checked = true;
                    zoom2500FeetMenuItem.Checked = false;
                    zoom1MileMenuItem.Checked = false;
                    zoom2MilesMenuItem.Checked = false;
                    zoom5MilesMenuItem.Checked = false;
                    break;
            }
        }

        /// <summary>
        /// This gets invoked when the Engine On at Startup menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void engineOnStartupMenuItem_Click(object sender, EventArgs e)
        {
            // toggle the Engine On at Startup setting and save the selection
            engineOnStartupMenuItem.Checked = !engineOnStartupMenuItem.Checked;
            Properties.Settings.Default.EngineOnAtStartup = engineOnStartupMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// This gets invoked when the Engine On checkbox is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void chkVehicle_Click(object sender, EventArgs e)
        {
            // set other controls accordingly
            bool enabled = chkVehicle.Checked;
            VehicleSpeed.Text = "0.0";
            SpeedUpDown.Value = 0;
            SpeedUpDown.Enabled = enabled;
            tbThrottle.Value = 0;
            tbThrottle.Enabled = enabled;
            chkPTO.Enabled = enabled;

            if (enabled)
            {
                // if (we are currently connected) then
                if (ConnectButton.Text == DISCONNECT_TEXT)
                {
                    // disconnect then reconnect in order to get the data flowing correctly
                    ConnectButton_Click(this, null);
                    ConnectButton_Click(this, null);
                }

                numRPM.Value = INITIAL_RPM;
                numRPM.Enabled = true;
                chkSetRPM.Enabled = true;
            }
            else
            {
                numRPM.Value = 0;
                numRPM.Enabled = false;
                chkSetRPM.Enabled = false;
            }
        }

        /// <summary>
        /// This gets invoked when the Exit menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exitMenuItem_Click(object sender, EventArgs e)
        {
            // close the application
            Close();
        }

        /// <summary>
        /// This gets invoked when the form is closing.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Close the COM ports

            if (NAVPort != null)
            {
                NAVPort.Close();
            }

            if (VehiclePort != null)
            {
                VehiclePort.Close();
            }
        }

        /// <summary>
        /// This gets invoked when the form has changed size.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_SizeChanged(object sender, EventArgs e)
        {
            // resize the progress bar accordingly
            tsProgress.Width = statusStrip1.Width - 20;
        }

        /// <summary>
        /// This gets invoked when the Reload File at Startup menu item is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void reloadFileAtStartupMenuItem_Click(object sender, EventArgs e)
        {
            // toggle the Reload File at Startup setting and save the selection
            reloadFileAtStartupMenuItem.Checked = !reloadFileAtStartupMenuItem.Checked;
            Properties.Settings.Default.ReloadFileAtStartup = reloadFileAtStartupMenuItem.Checked;
            Properties.Settings.Default.Save();
        }

        /// <summary>
        /// This gets invoked when the 2 mph braking rate radio button changes state.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnBrake2Mph_CheckedChanged(object sender, EventArgs e)
        {
            // save the selection
            Properties.Settings.Default.BrakeAt2Mph = rbtnBrake2Mph.Checked;
            Properties.Settings.Default.Save();
        }
    }
}