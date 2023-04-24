using System;
using System.IO;
using System.IO.Ports;
using System.Text;
using System.Windows.Forms;

namespace Trip_Simulator
{
    public class MainVehicle
    {
        public const int MAX_SPEED = 125;

        SerialPort VehiclePort = null;
        bool NmeaTimerRunning = false;
        MainForm TheMain;

        // Vehicle data
        static decimal MPH_TO_MIin1S = 1.0M / 60.0M / 60.0M;        // mi
        static decimal HR_PER_1S = 0.200M / 60.0M / 60.0M;          // hr
        decimal GAL_PER_HOUR_PER1S = 1.0M / 60.0M / 60.0M;          // gal
        static decimal GAL_PER_HOUR = 0.89M;                        // gal
        decimal Odometer;
        DateTime elapsed = new DateTime();
        decimal Loads;
        decimal Fuel;
        decimal Rate;
        decimal EngineHours;
        decimal speed = 0;

        NumericUpDown SpeedUpDown;
        NumericUpDown numRPM;
        NumericUpDown numOdometer;
        NumericUpDown numLoad;
        NumericUpDown numPTO;
        NumericUpDown numRate;
        NumericUpDown numFuel;
        NumericUpDown numEngineHours;
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
        CheckBox chkSetRPM;
        CheckBox chkSetFuelRate;
        CheckBox chkSetEngineLoad;

        // J1708 values to send (these are global - set by UpdateTruck, sent by Update_J1708)
        byte j_mph = 0;
        byte j_load = 0;
        byte j_pedal = 0;
        UInt16 j_rpm = 0;
        UInt32 j_tvd = 0;
        UInt16 j_rate = 0;
        UInt32 j_fuel = 0;
        UInt32 j_pto = 0;
        UInt32 j_teh = 0;
        byte j_ptos = 0;
        byte j_ccs = 0;
        const int SpaceMsec = 20;

        private decimal? speedSpike = null;
        private int spikeDelay = 0;
        string j_display = "";
        BinaryWriter J1708Recording = null;
        FileStream J1708PlaybackF = null;
        public BinaryReader J1708Playback = null;

                                    //     MID  RS     PD     EL     ES         TVD                 CHKM
        byte[] J1708_motion = new byte[] { 128, 84, 0, 91, 0, 92, 0, 190, 0, 0, 245, 4, 0, 0, 0, 0, 0 };
                                    //   MID  IFR        TF                  CHKF
        byte[] J1708_fuel = new byte[] { 128, 183, 0, 0, 250, 4, 0, 0, 0, 0, 0 };
                                    //   MID  CCS    PTOS   PTO                 TEH                 CHKO
        byte[] J1708_more = new byte[] { 128, 85, 0, 89, 0, 248, 4, 0, 0, 0, 0, 247, 4, 0, 0, 0, 0, 0 };

        const int RS = 2;
        const int PD = 4;
        const int EL = 6;
        const int ES = 8;
        const int TVD = 12;
        const int CHKM = 16;
        const int CHKMO = 10;

        const int IFR = 2;
        const int TF = 6;
        const int TFSIZE = 5;
        const int CHKF = 10;
        const int CHKFO = 4;

        const int CCS = 2;
        const int PTOS = 4;
        const int PTO = 7;
        const int TEH = 13;
        const int CHKO = 17;
        const int CHKOV = 25;

        bool playback = false;

        //*****************************************************************************************
        // Name: SetNMEATimerStatus(bool)
        // Description: Sets the status of the NMEATimer.
        //-----------------------------------------------------------------------------------------
        // Inputs: Status
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void SetNMEATimerStatus(bool Status)
        {
            NmeaTimerRunning = Status;          // For the use of Update_J1708().
        }

        //*****************************************************************************************
        // Name: SetValues()
        // Description: Sets the values of all the necessary variables that are used in the
        //              MainVehicle.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-25   SC Yuen     Added SetRPM into the parameter and set chkSetRPM with it
        //-----------------------------------------------------------------------------------------
        // 2017-06-21   SC Yuen     Added new parameters CheckBox SetFuelRate and
        //                          CheckBox SetEngineLoad
        //*****************************************************************************************
        public void SetValues(NumericUpDown TheSpeed, NumericUpDown TheRPM, NumericUpDown TheOdometer, NumericUpDown TheLoad, NumericUpDown ThePTO,
            NumericUpDown TheRate, NumericUpDown TheFuel ,CheckBox TheBrake, CheckBox CheckPTO, CheckBox CheckVehicle, TrackBar TheThrottle, ToolStripProgressBar TheProgress,
            Button TheButton, ListBox TheOutput, NumericUpDown TheEngineHours, SaveFileDialog TheRecord, OpenFileDialog ThePlayback, StatusStrip TheStatus, GroupBox TheGrpSpeed,
            GroupBox TheGrpFuel, GroupBox TheGrpPedals, ToolStripMenuItem TheStopRecording, ToolStripMenuItem TheRecordTo, ToolStripMenuItem ThePlaybackFrom,
            ToolStripMenuItem TheStopPlayback, SerialPort VPort, BinaryWriter Write, BinaryReader Read, FileStream PBF, CheckBox SetRPM, CheckBox SetFuelRate, CheckBox SetEngineLoad)
        {
            SpeedUpDown = TheSpeed;
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
            numEngineHours = TheEngineHours;

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
            chkVehicle = CheckVehicle;
            VehiclePort = VPort;
            J1708Recording = Write;
            J1708Playback = Read;
            J1708PlaybackF = PBF;
            chkSetRPM = SetRPM;
            chkSetFuelRate = SetFuelRate;
            chkSetEngineLoad = SetEngineLoad;

            if (chkPTO.Checked) numPTO.Value += HR_PER_1S;
        }

        //*****************************************************************************************
        // Name: SetValues(DateTime, NumericUpDown, bool, bool)
        // Description: Sets the values of the truck/vehicle data and returns the value of elapsed.
        //-----------------------------------------------------------------------------------------
        // Inputs: TheElapsed, SpeedUpDown, NoRPM, NoSpeed
        // Outputs: none
        // Returns: elapsed
        //*****************************************************************************************
        // 2015-10-27   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        // 2017-04-12   SC Yuen     Deleted SpeedUpDown parameter and added in NoSpeed parameter
        //                          Added logic when NoSpeed is true and when it's false
        //-----------------------------------------------------------------------------------------
        // 2017-04-21   SC Yuen     Added new parameters NoOdo and NoTEH and added new logics to
        //                          set numOdometer and/or numEngineHours to NA if NoOdo and/or
        //                          NoTEH is respectively true
        //-----------------------------------------------------------------------------------------
        // 2017-05-25   SC Yuen     Added code to check if chkSetRPM has been checked, if it's
        //                          checked don't set its value based on vehicle speed
        //-----------------------------------------------------------------------------------------
        // 2017-06-21   SC Yuen     Added code to check if chkSetFuelRate has been checked, if it's
        //                          checked don't set its value based on the calculated value Rate
        //                          Added code to check if chkSetEngineLoad has been checked, if
        //                          it's checked don't set its value based on the calculated value
        //                          Loads
        //*****************************************************************************************
        public DateTime SetValues(DateTime TheElapsed, bool NoRPM, bool NoSpeed, bool NoOdo, bool NoTEH)
        {
            elapsed = TheElapsed.AddSeconds(1);

            if (SpeedUpDown.Value <= MAX_SPEED)
            {
                speed = SpeedUpDown.Value;
            }
            
            decimal RPM = 0;

            if (NoSpeed)
            {
                SpeedUpDown.Value = 0xFFFF;
            }
            else
            {
                SpeedUpDown.Value = speed;
            }

            if (!NoRPM)
            {
                RPM = (600.0M + (speed / 50.0M * 800.0M));               
            }
            else
            {
                RPM = 0xFFFF;
            }

            if (!chkSetRPM.Checked)
            {
                numRPM.Value = RPM;
            }
            
            if (!chkSetFuelRate.Checked)
            {
                Rate = GAL_PER_HOUR + (speed / 50.0M * 4.77M);
                numRate.Value = Rate;
            }
            
            if (!chkSetEngineLoad.Checked)
            {
                Loads = RPM / (numRPM.Maximum - 600.0M) * 120.0M;
                numLoad.Value = Loads;
            }

            if (numOdometer.Value != 0xFFFFFFFF)
            {
                Odometer = numOdometer.Value;
            }

            Odometer = Odometer + speed * MPH_TO_MIin1S;
            
            if (Odometer > 0xFFFFFFFF)
            {
                Odometer = 0xFFFFFFFF;
            }         

            if (NoOdo)
            {
                numOdometer.Value = 0xFFFFFFFF;
            }
            else
            {
                numOdometer.Value = Odometer;
            }

            Fuel = numFuel.Value + numRate.Value * GAL_PER_HOUR_PER1S;

            if (Fuel > 0xFFFFFFFF)
            {
                Fuel = 0xFFFFFFFF;
            }

            numFuel.Value = Fuel;     
            
            if (numEngineHours.Value != 0xFFFFFFFF)
            {
                EngineHours = numEngineHours.Value;
            }
                  
            EngineHours = EngineHours + (1.0M / 3600.0M);

            if (NoTEH)
            {
                numEngineHours.Value = 0xFFFFFFFF;
            }
            else
            {
                numEngineHours.Value = EngineHours;
            }

            return elapsed;
        }

        //*****************************************************************************************
        // Name: GetElapsed()
        // Description: Gets elapsed.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: elapsed
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public DateTime GetElapsed()
        {
            return elapsed;
        }

        public void SetMain(MainForm main)
        {
            TheMain = main;
        }

        public void SetPlayback(bool PB)
        {
            playback = PB;
        }

        //*****************************************************************************************
        // Name: PlaybackTruck()
        // Description: Reads the value from the .J1708 file and set it for the Trip Sim.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-21 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public bool PlaybackTruck()
        {
            try
            {
                // Engine
                SpeedUpDown.Value = J1708Playback.ReadDecimal();
                numRPM.Value = J1708Playback.ReadDecimal();
                numOdometer.Value = J1708Playback.ReadDecimal();
                numLoad.Value = J1708Playback.ReadDecimal();
                numPTO.Value = J1708Playback.ReadDecimal();

                // Fuel
                numRate.Value = J1708Playback.ReadDecimal();
                numFuel.Value = J1708Playback.ReadDecimal();

                // Pedal/switches
                chkBrk.Checked = J1708Playback.ReadBoolean();
                J1708Playback.ReadBoolean();
                J1708Playback.ReadBoolean();
                chkPTO.Checked = J1708Playback.ReadBoolean();
                tbThrottle.Value = J1708Playback.ReadInt32();

                tsProgress.Value = (int)J1708PlaybackF.Position;
                return false;
            }
            catch
            {
                if (ConnectButton.Text != "Connect")
                {
                    TheMain.mnuStopPlayback_Click(null, null);
                    TheMain.SetJ1708Timer(playback);                 
                }
                return true;
            }
        }

        //*****************************************************************************************
        // Name: UpdateTruck()
        // Description: Updates the truck/vehicle data.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-21   SC Yuen     initial version
        //-----------------------------------------------------------------------------------------
        // 2017-04-12   SC Yuen     Added logic when SpeedUpDown is greater than 100, then send in
        //                          NA for j_mph
        //-----------------------------------------------------------------------------------------
        // 2017-04-21   SC Yuen     Added logic to set j_tvd and/or j_teh to NA if numOdometer
        //                          and/or numEngineHours were set to NA respectively
        //*****************************************************************************************
        public void UpdateTruck()
        {
            if (J1708Recording != null)
                RecordTruck();

            // switches
            byte brake = 0x20;
            byte pto = 0x01;
            byte ptomode = 0x80;

            try
            {
                if (SpeedUpDown.Value > MAX_SPEED)
                {
                    j_mph = 0xFF;
                }
                else
                {
                    j_mph = (byte)(SpeedUpDown.Value * 2.0M);                               // Gets the value of the speed as byte
                }

                j_load = (byte)(numLoad.Value * 2.0M);                                  // Gets the value of the load as byte
                j_pedal = (byte)(tbThrottle.Value * 100 / tbThrottle.Maximum * 2.5M);   // Gets the percentage of the throttle as byte
                if (numRPM.Value == 0xFFFF)
                {
                    j_rpm = 0xFFFF;
                }
                else
                {
                    j_rpm = (UInt16)(numRPM.Value * 4.0M);                              // Gets the value of the rpm as byte
                }

                if (numOdometer.Value == 0xFFFFFFFF)
                {
                    j_tvd = 0xFFFFFFFF;
                }
                else
                {
                    j_tvd = (UInt32)(numOdometer.Value * 10.0M);                            // Gets the value of the odometer as byte
                }
                
                j_rate = (UInt16)(numRate.Value * 64.0M);                               // Gets the value of the rate of fuel used as byte
                j_fuel = (UInt32)(numFuel.Value * 8.0M);                                // Gets the value of total fuel used as byte
                j_ccs = (byte)((chkBrk.Checked ? brake : 0));                           // Gets the byte of whether the Brake checkbox has been checked or not
                j_pto = (UInt32)(numPTO.Value * 20.0M);                                 // Gets the value of the total PTO hours as byte
                j_ptos = (byte)(chkPTO.Checked ? ptomode | pto : 0);                    // Gets the byte of wheter the PTO checkbox has been checked or not

                if (numEngineHours.Value == 0xFFFFFFFF)
                {
                    j_teh = 0xFFFFFFFF;
                }
                else
                {
                    j_teh = (UInt32)(numEngineHours.Value * 20.0M);
                }
            }
            catch { }
        }

        //*****************************************************************************************
        // Name: Show_J1708()
        // Description: Prints out the truck/vehicle data into the lstOutput.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-21 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void Show_J1708()
        {
            lstOutput.Items.Insert(0, j_display);
            while (lstOutput.Items.Count > 10)
                lstOutput.Items.RemoveAt(10);
        }

        //*****************************************************************************************
        // Name: RecordTruck()
        // Description: Records the truck/vehicle data into a .J1708 file.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-21 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        private void RecordTruck()
        {
            // Engine 
            J1708Recording.Write(SpeedUpDown.Value);
            J1708Recording.Write(numRPM.Value);
            J1708Recording.Write(numOdometer.Value);

            J1708Recording.Write(numLoad.Value);
            J1708Recording.Write(numPTO.Value);

            // Fuel
            J1708Recording.Write(numRate.Value);
            J1708Recording.Write(numFuel.Value);

            // Pedal/switches
            J1708Recording.Write(chkBrk.Checked);
            J1708Recording.Write(false);
            J1708Recording.Write(false);
            J1708Recording.Write(chkPTO.Checked);
            J1708Recording.Write(tbThrottle.Value);

            mnuRecordTo.Enabled = false;
            mnuPlaybackFrom.Enabled = false;
        }

        //*****************************************************************************************
        // Name: Update_J1708()
        // Description: Updates the truck/vehicle data and format them into a string and put them
        //              into j_display. Show_1708 will then print j_display into lstOutput.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-21 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void Update_J1708()
        {
            // Runs continuously while timer is enabled
            while (NmeaTimerRunning && VehiclePort.IsOpen)
            {
                // Motion data set
                if (speedSpike != null && spikeDelay == 0)
                {
                    J1708_motion[RS] = (byte)speedSpike;
                }
                else
                {
                    J1708_motion[RS] = j_mph;                           //  84 Road Speed units 1/2 mph
                }
                J1708_motion[PD] = j_pedal;                             //  91 Pedal Position  0.4%
                J1708_motion[EL] = j_load;                              //  92 Engine load  0.5%
                BitConverter.GetBytes(j_rpm).CopyTo(J1708_motion, ES);  // 190 Engine speed units 1/4 rpm
                BitConverter.GetBytes(j_tvd).CopyTo(J1708_motion, TVD); // 245 Total vehicle Distance 1/10 mile

                // Calc checksum
                uint checksum = 0;
                int lastLength = 0;
                StringBuilder sb = new StringBuilder("Mot: ");

                J1708_motion[CHKM] = 0;
                foreach (byte b in J1708_motion)
                {
                    lastLength = sb.Length;
                    checksum += (uint)b;
                    sb.AppendFormat("{0},", b);
                }

                // Set checksum
                J1708_motion[CHKM] = (byte)(256 - (checksum & 0xFF));
                VehiclePort.Write(J1708_motion, 0, J1708_motion.Length);

                System.Threading.Thread.Sleep(SpaceMsec);
                sb.Remove(lastLength, sb.Length - lastLength);
                sb.Append(J1708_motion[CHKM].ToString());

                // Fuel data set
                BitConverter.GetBytes(j_rate).CopyTo(J1708_fuel, IFR);  // 183 Instantaneous Fuel Rate 1/64 gph
                BitConverter.GetBytes(j_fuel).CopyTo(J1708_fuel, TF);   // 250 Total Fuel Used 1/8 gal
                J1708_fuel[TFSIZE] = 4;

                // Calc checksum
                checksum = 0;
                sb.Append("\t Fuel: ");

                // Total fuel NOT omitted
                J1708_fuel[CHKF] = 0;
                foreach (byte b in J1708_fuel)
                {
                    lastLength = sb.Length;
                    checksum += (uint)b;
                    sb.AppendFormat("{0},", b);
                }

                // Set checksum
                J1708_fuel[CHKF] = (byte)(256 - (checksum & 0xFF));

                if (VehiclePort.IsOpen) VehiclePort.Write(J1708_fuel, 0, J1708_fuel.Length);

                System.Threading.Thread.Sleep(SpaceMsec);
                sb.Remove(lastLength, sb.Length - lastLength);
                sb.Append(J1708_fuel[CHKF].ToString());

                // More data set
                J1708_more[CCS] = j_ccs;                                // The byte of whether the Brake checkbox has been checked or not.
                J1708_more[PTOS] = j_ptos;                              // The byte of whether the PTO checkbox has been checked or not.
                BitConverter.GetBytes(j_pto).CopyTo(J1708_more, PTO);   // 248 PTO hours 0.05 hour
                BitConverter.GetBytes(j_teh).CopyTo(J1708_more, TEH);   // 247 Total Engine hours 0.05 hour
                J1708_more[CHKO] = 0;

                // Calc checksum
                checksum = 0;
                sb.Append("\t More: ");
                foreach (byte b in J1708_more)
                {
                    lastLength = sb.Length;
                    checksum += (uint)b;
                    sb.AppendFormat("{0},", b);
                }

                // Set checksum
                J1708_more[CHKO] = (byte)(256 - (checksum & 0xFF));
                if (VehiclePort.IsOpen) VehiclePort.Write(J1708_more, 0, J1708_more.Length);
                System.Threading.Thread.Sleep(SpaceMsec);

                sb.Remove(lastLength, sb.Length - lastLength);
                sb.Append(J1708_more[CHKO].ToString());

                // Set display data
                j_display = sb.ToString();

                Show_J1708();
                break;
            }
        }

        //*****************************************************************************************
        // Name: RecordItTo(object, EventArgs)
        // Description: Sets the recording of the truck/vehicle data to be true to start recording.
        //              Also, writes the truck/vehicle data into a .J1708 file. Called by
        //              MainForm's mnuRecordTo_Click method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public BinaryWriter RecordItTo()
        {
            if (dlgRecord.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                J1708Recording = new BinaryWriter(File.Open(dlgRecord.FileName, FileMode.Create));
                mnuStopRecording.Enabled = true;
                mnuRecordTo.Enabled = false;
                mnuPlaybackFrom.Enabled = false;
            }
            return J1708Recording;
        }

        //*****************************************************************************************
        // Name: StopTheRecording(object, EventArgs)
        // Description: Stops the recording of the truck/vehicle data. Called by MainForm's
        //              stopRecordingToolStripMenuItem_Click method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public BinaryWriter StopTheRecording()
        {
            J1708Recording.Close();
            J1708Recording = null;
            MessageBox.Show("Recording completed.");

            mnuRecordTo.Enabled = true;
            mnuStopRecording.Enabled = false;
            mnuPlaybackFrom.Enabled = true;

            return J1708Recording;
        }

        //*****************************************************************************************
        // Name: GetPlaybackStatus()
        // Description: Gets the J1708Playback and return it.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: BinaryReader J1708Playback
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public BinaryReader GetPlaybackStatus()
        {
            return J1708Playback;
        }

        //*****************************************************************************************
        // Name: Set1708Playback()
        // Description: Sets the J1708Playback to null.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-27 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public void SetJ1708Playback()
        {
            J1708Playback = null;
        }

        //*****************************************************************************************
        // Name: DoThePlaybackFrom(object, EventArgs)
        // Description: Sets the playback of the truck/vehicle data to be true. Also, reads the
        //              data from the .J1708 file that was selected for playback. Gets called by
        //              the MainForm's mnuPlaybackFrom_Click method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22   SC Yuen       initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-10   SC Yuen       Got rid of setting the statusStrip1 to be visible
        //*****************************************************************************************
        public BinaryReader DoThePlaybackFrom()
        {
            if (dlgPlayback.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                J1708PlaybackF = File.Open(dlgPlayback.FileName, FileMode.Open);
                J1708Playback = new BinaryReader(J1708PlaybackF);

                tsProgress.Width = statusStrip1.Width - 10;
                tsProgress.Value = 0;
                tsProgress.Maximum = (int)J1708PlaybackF.Length;

                mnuStopPlayback.Enabled = true;
                mnuRecordTo.Enabled = false;
                mnuPlaybackFrom.Enabled = false;

                grpSpdDist.Enabled = false;
                grpFuel.Enabled = false;
                grpPedals.Enabled = false;
            }
            return J1708Playback;
        }

        //*****************************************************************************************
        // Name: StopThePlayback(object, EventArgs)
        // Description: Stops the playback of the truck/vehicle data. Gets called by the MainForm's
        //              mnuStopPlayback_Click method.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: none
        //*****************************************************************************************
        // 2015-10-22 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        // 2017-05-10   SC Yuen       Got rid of setting the statusStrip1 to not be visible
        //*****************************************************************************************
        public void StopThePlayback()
        {
            SetPlayback(false);

            J1708Playback.Close();
            J1708PlaybackF.Close();

            J1708Playback = null;
            J1708PlaybackF = null;

            mnuStopPlayback.Enabled = false;
            mnuRecordTo.Enabled = true;
            mnuPlaybackFrom.Enabled = true;

            grpSpdDist.Enabled = true;
            grpFuel.Enabled = true;
            grpPedals.Enabled = true;
        }

        //*****************************************************************************************
        // Name: FileStream()
        // Description: Gets the J1708PlaybackF.
        //-----------------------------------------------------------------------------------------
        // Inputs: none
        // Outputs: none
        // Returns: J1708PlaybackF
        //*****************************************************************************************
        // 2015-11-03 SC Yuen initial version
        //-----------------------------------------------------------------------------------------
        //*****************************************************************************************
        public FileStream GetPlaybackF()
        {
            return J1708PlaybackF;
        }
    }
}
