namespace Trip_Simulator
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.grpNAV = new System.Windows.Forms.GroupBox();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.CameraLockButton = new System.Windows.Forms.Button();
            this.NAVComboBox = new System.Windows.Forms.ComboBox();
            this.grpVehicle = new System.Windows.Forms.GroupBox();
            this.VehicleComboBox = new System.Windows.Forms.ComboBox();
            this.chkVehicle = new System.Windows.Forms.CheckBox();
            this.lblVehicleData = new System.Windows.Forms.Label();
            this.LeapTimer = new System.Windows.Forms.Timer(this.components);
            this.NmeaTimer = new System.Windows.Forms.Timer(this.components);
            this.J1708Timer = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reloadFileAtStartupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRecord = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuRecordTo = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStopRecording = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.mnuPlaybackFrom = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuStopPlayback = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.leapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopLeapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mapOnStartupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.initialZoomLevelMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom250FeetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom2500FeetMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom1MileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom2MilesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.zoom5MilesMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.engineOnStartupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TheFileName = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.grpSpdDist = new System.Windows.Forms.GroupBox();
            this.chkSetEngineLoad = new System.Windows.Forms.CheckBox();
            this.chkSetRPM = new System.Windows.Forms.CheckBox();
            this.chkLoadFileResetECM = new System.Windows.Forms.CheckBox();
            this.ECMClearButton = new System.Windows.Forms.Button();
            this.chkNoTEH = new System.Windows.Forms.CheckBox();
            this.chkNoOdo = new System.Windows.Forms.CheckBox();
            this.VehicleSpeed = new System.Windows.Forms.Label();
            this.lblNoSpeed = new System.Windows.Forms.Label();
            this.chkNoSpeed = new System.Windows.Forms.CheckBox();
            this.numEngineHours = new System.Windows.Forms.NumericUpDown();
            this.EngineHour = new System.Windows.Forms.Label();
            this.lblNoRPM = new System.Windows.Forms.Label();
            this.chkNoRPM = new System.Windows.Forms.CheckBox();
            this.SpeedUpDown = new System.Windows.Forms.NumericUpDown();
            this.numRPM = new System.Windows.Forms.NumericUpDown();
            this.numOdometer = new System.Windows.Forms.NumericUpDown();
            this.lblTimeAtSpeed = new System.Windows.Forms.Label();
            this.numLoad = new System.Windows.Forms.NumericUpDown();
            this.numPTO = new System.Windows.Forms.NumericUpDown();
            this.lblSpeed = new System.Windows.Forms.Label();
            this.lblRPM = new System.Windows.Forms.Label();
            this.lblOdo = new System.Windows.Forms.Label();
            this.lblTimeAtSpd = new System.Windows.Forms.Label();
            this.lblEngineLoad = new System.Windows.Forms.Label();
            this.lblTotalPTO = new System.Windows.Forms.Label();
            this.MapBtn = new System.Windows.Forms.Button();
            this.grpFuel = new System.Windows.Forms.GroupBox();
            this.chkSetFuelRate = new System.Windows.Forms.CheckBox();
            this.numRate = new System.Windows.Forms.NumericUpDown();
            this.numFuel = new System.Windows.Forms.NumericUpDown();
            this.lblFuelRate = new System.Windows.Forms.Label();
            this.lblTotalFuel = new System.Windows.Forms.Label();
            this.grpPedals = new System.Windows.Forms.GroupBox();
            this.pnlBrakingRate = new System.Windows.Forms.Panel();
            this.rbtnBrake5Mph = new System.Windows.Forms.RadioButton();
            this.rbtnBrake2Mph = new System.Windows.Forms.RadioButton();
            this.chkBrk = new System.Windows.Forms.CheckBox();
            this.chkPTO = new System.Windows.Forms.CheckBox();
            this.tbThrottle = new System.Windows.Forms.TrackBar();
            this.lblBrake = new System.Windows.Forms.Label();
            this.lblThrottle = new System.Windows.Forms.Label();
            this.lblPTO = new System.Windows.Forms.Label();
            this.grpJ1708 = new System.Windows.Forms.GroupBox();
            this.lstOutput = new System.Windows.Forms.ListBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.dlgRecord = new System.Windows.Forms.SaveFileDialog();
            this.dlgPlayback = new System.Windows.Forms.OpenFileDialog();
            this.HACCNum = new System.Windows.Forms.NumericUpDown();
            this.lblHAC = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.ChkBoxHACC = new System.Windows.Forms.CheckBox();
            this.PlaybackStarter = new System.Windows.Forms.GroupBox();
            this.GPSTimeOnlyLabel = new System.Windows.Forms.Label();
            this.GPSTimeOnly = new System.Windows.Forms.CheckBox();
            this.lblNone = new System.Windows.Forms.Label();
            this.radioNone = new System.Windows.Forms.RadioButton();
            this.radioSetPlaybackTime = new System.Windows.Forms.RadioButton();
            this.lblPastPlayback = new System.Windows.Forms.Label();
            this.radioStart = new System.Windows.Forms.RadioButton();
            this.lblTimeDef = new System.Windows.Forms.Label();
            this.lblTimeLeft = new System.Windows.Forms.Label();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblStop = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.StopDate = new System.Windows.Forms.DateTimePicker();
            this.countDown = new System.Windows.Forms.Label();
            this.chkStop = new System.Windows.Forms.CheckBox();
            this.StartDate = new System.Windows.Forms.DateTimePicker();
            this.CurrentTime = new System.Windows.Forms.Label();
            this.lblFileLoaded = new System.Windows.Forms.Label();
            this.FileLoaded = new System.Windows.Forms.Label();
            this.lblCurrentCoordinates = new System.Windows.Forms.Label();
            this.CurrentCoordinates = new System.Windows.Forms.Label();
            this.lblTimeStarted = new System.Windows.Forms.Label();
            this.TimeStarted = new System.Windows.Forms.Label();
            this.OnOffBtn = new System.Windows.Forms.Button();
            this.chkLoopKmlFile = new System.Windows.Forms.CheckBox();
            this.grpNAV.SuspendLayout();
            this.grpVehicle.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.grpSpdDist.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEngineHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRPM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOdometer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoad)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPTO)).BeginInit();
            this.grpFuel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFuel)).BeginInit();
            this.grpPedals.SuspendLayout();
            this.pnlBrakingRate.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbThrottle)).BeginInit();
            this.grpJ1708.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HACCNum)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.PlaybackStarter.SuspendLayout();
            this.SuspendLayout();
            // 
            // grpNAV
            // 
            this.grpNAV.Controls.Add(this.ConnectButton);
            this.grpNAV.Controls.Add(this.CameraLockButton);
            this.grpNAV.Controls.Add(this.NAVComboBox);
            this.grpNAV.Location = new System.Drawing.Point(10, 108);
            this.grpNAV.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.grpNAV.Name = "grpNAV";
            this.grpNAV.Size = new System.Drawing.Size(383, 47);
            this.grpNAV.TabIndex = 1;
            this.grpNAV.TabStop = false;
            this.grpNAV.Text = "NAV Port";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(135, 16);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(2);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(122, 23);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect/Start Timer";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectButton_Click);
            // 
            // CameraLockButton
            // 
            this.CameraLockButton.Location = new System.Drawing.Point(263, 16);
            this.CameraLockButton.Margin = new System.Windows.Forms.Padding(2);
            this.CameraLockButton.Name = "CameraLockButton";
            this.CameraLockButton.Size = new System.Drawing.Size(114, 23);
            this.CameraLockButton.TabIndex = 0;
            this.CameraLockButton.Text = "Camera Lock Off";
            this.CameraLockButton.UseVisualStyleBackColor = true;
            this.CameraLockButton.Click += new System.EventHandler(this.CameraLockButton_Click);
            // 
            // NAVComboBox
            // 
            this.NAVComboBox.FormattingEnabled = true;
            this.NAVComboBox.Location = new System.Drawing.Point(9, 18);
            this.NAVComboBox.Name = "NAVComboBox";
            this.NAVComboBox.Size = new System.Drawing.Size(121, 21);
            this.NAVComboBox.TabIndex = 1;
            // 
            // grpVehicle
            // 
            this.grpVehicle.Controls.Add(this.VehicleComboBox);
            this.grpVehicle.Controls.Add(this.chkVehicle);
            this.grpVehicle.Controls.Add(this.lblVehicleData);
            this.grpVehicle.Location = new System.Drawing.Point(9, 155);
            this.grpVehicle.Name = "grpVehicle";
            this.grpVehicle.Size = new System.Drawing.Size(228, 47);
            this.grpVehicle.TabIndex = 1;
            this.grpVehicle.TabStop = false;
            this.grpVehicle.Text = "Vehicle Port";
            // 
            // VehicleComboBox
            // 
            this.VehicleComboBox.FormattingEnabled = true;
            this.VehicleComboBox.Location = new System.Drawing.Point(10, 20);
            this.VehicleComboBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.VehicleComboBox.Name = "VehicleComboBox";
            this.VehicleComboBox.Size = new System.Drawing.Size(120, 21);
            this.VehicleComboBox.TabIndex = 0;
            // 
            // chkVehicle
            // 
            this.chkVehicle.AutoSize = true;
            this.chkVehicle.Location = new System.Drawing.Point(135, 23);
            this.chkVehicle.Name = "chkVehicle";
            this.chkVehicle.Size = new System.Drawing.Size(15, 14);
            this.chkVehicle.TabIndex = 17;
            this.chkVehicle.UseVisualStyleBackColor = true;
            this.chkVehicle.Click += new System.EventHandler(this.chkVehicle_Click);
            // 
            // lblVehicleData
            // 
            this.lblVehicleData.AutoSize = true;
            this.lblVehicleData.Location = new System.Drawing.Point(155, 23);
            this.lblVehicleData.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblVehicleData.Name = "lblVehicleData";
            this.lblVehicleData.Size = new System.Drawing.Size(57, 13);
            this.lblVehicleData.TabIndex = 4;
            this.lblVehicleData.Text = "Engine On";
            // 
            // LeapTimer
            // 
            this.LeapTimer.Interval = 1000;
            this.LeapTimer.Tick += new System.EventHandler(this.Leaper_Tick);
            // 
            // NmeaTimer
            // 
            this.NmeaTimer.Interval = 50;
            this.NmeaTimer.Tick += new System.EventHandler(this.NmeaTimer_Tick);
            // 
            // J1708Timer
            // 
            this.J1708Timer.Tick += new System.EventHandler(this.J1708Timer_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.mnuRecord,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 1, 0, 1);
            this.menuStrip1.Size = new System.Drawing.Size(833, 24);
            this.menuStrip1.TabIndex = 2;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.reloadFileAtStartupMenuItem,
            this.exitMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.A)));
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 22);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.B)));
            this.openToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.openToolStripMenuItem.Text = "Open File...";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.saveAsToolStripMenuItem.Text = "Save As...";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // reloadFileAtStartupMenuItem
            // 
            this.reloadFileAtStartupMenuItem.Name = "reloadFileAtStartupMenuItem";
            this.reloadFileAtStartupMenuItem.Size = new System.Drawing.Size(209, 22);
            this.reloadFileAtStartupMenuItem.Text = "Reload Last File at Startup";
            this.reloadFileAtStartupMenuItem.Click += new System.EventHandler(this.reloadFileAtStartupMenuItem_Click);
            // 
            // exitMenuItem
            // 
            this.exitMenuItem.Name = "exitMenuItem";
            this.exitMenuItem.Size = new System.Drawing.Size(209, 22);
            this.exitMenuItem.Text = "Exit";
            this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
            // 
            // mnuRecord
            // 
            this.mnuRecord.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuRecordTo,
            this.mnuStopRecording,
            this.toolStripSeparator1,
            this.mnuPlaybackFrom,
            this.mnuStopPlayback});
            this.mnuRecord.Name = "mnuRecord";
            this.mnuRecord.Size = new System.Drawing.Size(56, 22);
            this.mnuRecord.Text = "Record";
            // 
            // mnuRecordTo
            // 
            this.mnuRecordTo.Name = "mnuRecordTo";
            this.mnuRecordTo.Size = new System.Drawing.Size(159, 22);
            this.mnuRecordTo.Text = "Record to...";
            this.mnuRecordTo.Click += new System.EventHandler(this.mnuRecordTo_Click);
            // 
            // mnuStopRecording
            // 
            this.mnuStopRecording.Enabled = false;
            this.mnuStopRecording.Name = "mnuStopRecording";
            this.mnuStopRecording.Size = new System.Drawing.Size(159, 22);
            this.mnuStopRecording.Text = "Stop Recording";
            this.mnuStopRecording.Click += new System.EventHandler(this.stopRecordingToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // mnuPlaybackFrom
            // 
            this.mnuPlaybackFrom.Name = "mnuPlaybackFrom";
            this.mnuPlaybackFrom.Size = new System.Drawing.Size(159, 22);
            this.mnuPlaybackFrom.Text = "Playback from...";
            this.mnuPlaybackFrom.Click += new System.EventHandler(this.mnuPlaybackFrom_Click);
            // 
            // mnuStopPlayback
            // 
            this.mnuStopPlayback.Enabled = false;
            this.mnuStopPlayback.Name = "mnuStopPlayback";
            this.mnuStopPlayback.Size = new System.Drawing.Size(159, 22);
            this.mnuStopPlayback.Text = "Stop Playback";
            this.mnuStopPlayback.Click += new System.EventHandler(this.mnuStopPlayback_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.leapToolStripMenuItem,
            this.stopLeapToolStripMenuItem,
            this.mapOnStartupMenuItem,
            this.initialZoomLevelMenuItem,
            this.engineOnStartupMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 22);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // leapToolStripMenuItem
            // 
            this.leapToolStripMenuItem.Name = "leapToolStripMenuItem";
            this.leapToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.leapToolStripMenuItem.Text = "Leap";
            this.leapToolStripMenuItem.Click += new System.EventHandler(this.Leaper_Tick);
            // 
            // stopLeapToolStripMenuItem
            // 
            this.stopLeapToolStripMenuItem.Name = "stopLeapToolStripMenuItem";
            this.stopLeapToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.stopLeapToolStripMenuItem.Text = "Stop Leap";
            this.stopLeapToolStripMenuItem.Click += new System.EventHandler(this.StopLeaper_Tick);
            // 
            // mapOnStartupMenuItem
            // 
            this.mapOnStartupMenuItem.Name = "mapOnStartupMenuItem";
            this.mapOnStartupMenuItem.Size = new System.Drawing.Size(193, 22);
            this.mapOnStartupMenuItem.Text = "Display Map at Startup";
            this.mapOnStartupMenuItem.Click += new System.EventHandler(this.mapOnStartupMenuItem_Click);
            // 
            // initialZoomLevelMenuItem
            // 
            this.initialZoomLevelMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.zoom250FeetMenuItem,
            this.zoom2500FeetMenuItem,
            this.zoom1MileMenuItem,
            this.zoom2MilesMenuItem,
            this.zoom5MilesMenuItem});
            this.initialZoomLevelMenuItem.Name = "initialZoomLevelMenuItem";
            this.initialZoomLevelMenuItem.Size = new System.Drawing.Size(193, 22);
            this.initialZoomLevelMenuItem.Text = "Initial Zoom Level";
            // 
            // zoom250FeetMenuItem
            // 
            this.zoom250FeetMenuItem.Name = "zoom250FeetMenuItem";
            this.zoom250FeetMenuItem.Size = new System.Drawing.Size(121, 22);
            this.zoom250FeetMenuItem.Text = "250 feet";
            this.zoom250FeetMenuItem.Click += new System.EventHandler(this.zoomMenuItem_Click);
            // 
            // zoom2500FeetMenuItem
            // 
            this.zoom2500FeetMenuItem.Name = "zoom2500FeetMenuItem";
            this.zoom2500FeetMenuItem.Size = new System.Drawing.Size(121, 22);
            this.zoom2500FeetMenuItem.Text = "2500 feet";
            this.zoom2500FeetMenuItem.Click += new System.EventHandler(this.zoomMenuItem_Click);
            // 
            // zoom1MileMenuItem
            // 
            this.zoom1MileMenuItem.Name = "zoom1MileMenuItem";
            this.zoom1MileMenuItem.Size = new System.Drawing.Size(121, 22);
            this.zoom1MileMenuItem.Text = "1 mile";
            this.zoom1MileMenuItem.Click += new System.EventHandler(this.zoomMenuItem_Click);
            // 
            // zoom2MilesMenuItem
            // 
            this.zoom2MilesMenuItem.Name = "zoom2MilesMenuItem";
            this.zoom2MilesMenuItem.Size = new System.Drawing.Size(121, 22);
            this.zoom2MilesMenuItem.Text = "2 miles";
            this.zoom2MilesMenuItem.Click += new System.EventHandler(this.zoomMenuItem_Click);
            // 
            // zoom5MilesMenuItem
            // 
            this.zoom5MilesMenuItem.Name = "zoom5MilesMenuItem";
            this.zoom5MilesMenuItem.Size = new System.Drawing.Size(121, 22);
            this.zoom5MilesMenuItem.Text = "5 miles";
            this.zoom5MilesMenuItem.Click += new System.EventHandler(this.zoomMenuItem_Click);
            // 
            // engineOnStartupMenuItem
            // 
            this.engineOnStartupMenuItem.Name = "engineOnStartupMenuItem";
            this.engineOnStartupMenuItem.Size = new System.Drawing.Size(193, 22);
            this.engineOnStartupMenuItem.Text = "Engine On at Startup";
            this.engineOnStartupMenuItem.Click += new System.EventHandler(this.engineOnStartupMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 22);
            this.helpToolStripMenuItem.Text = "Help";
            this.helpToolStripMenuItem.Click += new System.EventHandler(this.helpToolStripMenuItem_Click);
            // 
            // grpSpdDist
            // 
            this.grpSpdDist.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSpdDist.Controls.Add(this.chkSetEngineLoad);
            this.grpSpdDist.Controls.Add(this.chkSetRPM);
            this.grpSpdDist.Controls.Add(this.chkLoadFileResetECM);
            this.grpSpdDist.Controls.Add(this.ECMClearButton);
            this.grpSpdDist.Controls.Add(this.chkNoTEH);
            this.grpSpdDist.Controls.Add(this.chkNoOdo);
            this.grpSpdDist.Controls.Add(this.VehicleSpeed);
            this.grpSpdDist.Controls.Add(this.lblNoSpeed);
            this.grpSpdDist.Controls.Add(this.chkNoSpeed);
            this.grpSpdDist.Controls.Add(this.numEngineHours);
            this.grpSpdDist.Controls.Add(this.EngineHour);
            this.grpSpdDist.Controls.Add(this.lblNoRPM);
            this.grpSpdDist.Controls.Add(this.chkNoRPM);
            this.grpSpdDist.Controls.Add(this.SpeedUpDown);
            this.grpSpdDist.Controls.Add(this.numRPM);
            this.grpSpdDist.Controls.Add(this.numOdometer);
            this.grpSpdDist.Controls.Add(this.lblTimeAtSpeed);
            this.grpSpdDist.Controls.Add(this.numLoad);
            this.grpSpdDist.Controls.Add(this.numPTO);
            this.grpSpdDist.Controls.Add(this.lblSpeed);
            this.grpSpdDist.Controls.Add(this.lblRPM);
            this.grpSpdDist.Controls.Add(this.lblOdo);
            this.grpSpdDist.Controls.Add(this.lblTimeAtSpd);
            this.grpSpdDist.Controls.Add(this.lblEngineLoad);
            this.grpSpdDist.Controls.Add(this.lblTotalPTO);
            this.grpSpdDist.Location = new System.Drawing.Point(200, 209);
            this.grpSpdDist.Name = "grpSpdDist";
            this.grpSpdDist.Size = new System.Drawing.Size(624, 205);
            this.grpSpdDist.TabIndex = 3;
            this.grpSpdDist.TabStop = false;
            this.grpSpdDist.Text = "Engine";
            // 
            // chkSetEngineLoad
            // 
            this.chkSetEngineLoad.AutoSize = true;
            this.chkSetEngineLoad.Location = new System.Drawing.Point(197, 107);
            this.chkSetEngineLoad.Margin = new System.Windows.Forms.Padding(2);
            this.chkSetEngineLoad.Name = "chkSetEngineLoad";
            this.chkSetEngineLoad.Size = new System.Drawing.Size(69, 17);
            this.chkSetEngineLoad.TabIndex = 34;
            this.chkSetEngineLoad.Text = "Set Load";
            this.chkSetEngineLoad.UseVisualStyleBackColor = true;
            this.chkSetEngineLoad.CheckedChanged += new System.EventHandler(this.chkSetEngineLoad_CheckedChanged);
            // 
            // chkSetRPM
            // 
            this.chkSetRPM.AutoSize = true;
            this.chkSetRPM.Location = new System.Drawing.Point(197, 41);
            this.chkSetRPM.Margin = new System.Windows.Forms.Padding(2);
            this.chkSetRPM.Name = "chkSetRPM";
            this.chkSetRPM.Size = new System.Drawing.Size(69, 17);
            this.chkSetRPM.TabIndex = 32;
            this.chkSetRPM.Text = "Set RPM";
            this.chkSetRPM.UseVisualStyleBackColor = true;
            this.chkSetRPM.CheckedChanged += new System.EventHandler(this.chkSetRPM_CheckedChanged);
            // 
            // chkLoadFileResetECM
            // 
            this.chkLoadFileResetECM.AutoSize = true;
            this.chkLoadFileResetECM.Location = new System.Drawing.Point(455, 61);
            this.chkLoadFileResetECM.Margin = new System.Windows.Forms.Padding(2);
            this.chkLoadFileResetECM.Name = "chkLoadFileResetECM";
            this.chkLoadFileResetECM.Size = new System.Drawing.Size(166, 17);
            this.chkLoadFileResetECM.TabIndex = 31;
            this.chkLoadFileResetECM.Text = "Load File Resets ECM Values";
            this.chkLoadFileResetECM.UseVisualStyleBackColor = true;
            // 
            // ECMClearButton
            // 
            this.ECMClearButton.Location = new System.Drawing.Point(491, 16);
            this.ECMClearButton.Margin = new System.Windows.Forms.Padding(2);
            this.ECMClearButton.Name = "ECMClearButton";
            this.ECMClearButton.Size = new System.Drawing.Size(128, 28);
            this.ECMClearButton.TabIndex = 30;
            this.ECMClearButton.Text = "ECM Value Reset";
            this.ECMClearButton.UseVisualStyleBackColor = true;
            this.ECMClearButton.Click += new System.EventHandler(this.ECMClearButton_Click);
            // 
            // chkNoTEH
            // 
            this.chkNoTEH.AutoSize = true;
            this.chkNoTEH.Location = new System.Drawing.Point(132, 187);
            this.chkNoTEH.Margin = new System.Windows.Forms.Padding(2);
            this.chkNoTEH.Name = "chkNoTEH";
            this.chkNoTEH.Size = new System.Drawing.Size(135, 17);
            this.chkNoTEH.TabIndex = 29;
            this.chkNoTEH.Text = "NA Total Engine Hours";
            this.chkNoTEH.UseVisualStyleBackColor = true;
            // 
            // chkNoOdo
            // 
            this.chkNoOdo.AutoSize = true;
            this.chkNoOdo.Location = new System.Drawing.Point(132, 168);
            this.chkNoOdo.Margin = new System.Windows.Forms.Padding(2);
            this.chkNoOdo.Name = "chkNoOdo";
            this.chkNoOdo.Size = new System.Drawing.Size(90, 17);
            this.chkNoOdo.TabIndex = 28;
            this.chkNoOdo.Text = "NA Odometer";
            this.chkNoOdo.UseVisualStyleBackColor = true;
            // 
            // VehicleSpeed
            // 
            this.VehicleSpeed.AutoSize = true;
            this.VehicleSpeed.Location = new System.Drawing.Point(90, 21);
            this.VehicleSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.VehicleSpeed.Name = "VehicleSpeed";
            this.VehicleSpeed.Size = new System.Drawing.Size(22, 13);
            this.VehicleSpeed.TabIndex = 27;
            this.VehicleSpeed.Text = "0.0";
            // 
            // lblNoSpeed
            // 
            this.lblNoSpeed.AutoSize = true;
            this.lblNoSpeed.Location = new System.Drawing.Point(28, 168);
            this.lblNoSpeed.Name = "lblNoSpeed";
            this.lblNoSpeed.Size = new System.Drawing.Size(94, 13);
            this.lblNoSpeed.TabIndex = 26;
            this.lblNoSpeed.Text = "NA Vehicle Speed";
            // 
            // chkNoSpeed
            // 
            this.chkNoSpeed.AutoSize = true;
            this.chkNoSpeed.Location = new System.Drawing.Point(9, 168);
            this.chkNoSpeed.Name = "chkNoSpeed";
            this.chkNoSpeed.Size = new System.Drawing.Size(15, 14);
            this.chkNoSpeed.TabIndex = 25;
            this.chkNoSpeed.UseVisualStyleBackColor = true;
            // 
            // numEngineHours
            // 
            this.numEngineHours.DecimalPlaces = 3;
            this.numEngineHours.Location = new System.Drawing.Point(5, 145);
            this.numEngineHours.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numEngineHours.Name = "numEngineHours";
            this.numEngineHours.Size = new System.Drawing.Size(80, 20);
            this.numEngineHours.TabIndex = 25;
            this.numEngineHours.Value = new decimal(new int[] {
            500000,
            0,
            0,
            196608});
            // 
            // EngineHour
            // 
            this.EngineHour.AutoSize = true;
            this.EngineHour.Location = new System.Drawing.Point(90, 147);
            this.EngineHour.Name = "EngineHour";
            this.EngineHour.Size = new System.Drawing.Size(85, 13);
            this.EngineHour.TabIndex = 26;
            this.EngineHour.Text = "Total Engine (hr)";
            // 
            // lblNoRPM
            // 
            this.lblNoRPM.AutoSize = true;
            this.lblNoRPM.Location = new System.Drawing.Point(28, 187);
            this.lblNoRPM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNoRPM.Name = "lblNoRPM";
            this.lblNoRPM.Size = new System.Drawing.Size(92, 13);
            this.lblNoRPM.TabIndex = 24;
            this.lblNoRPM.Text = "NA Engine Speed";
            // 
            // chkNoRPM
            // 
            this.chkNoRPM.AutoSize = true;
            this.chkNoRPM.Location = new System.Drawing.Point(9, 186);
            this.chkNoRPM.Margin = new System.Windows.Forms.Padding(2);
            this.chkNoRPM.Name = "chkNoRPM";
            this.chkNoRPM.Size = new System.Drawing.Size(15, 14);
            this.chkNoRPM.TabIndex = 23;
            this.chkNoRPM.UseVisualStyleBackColor = true;
            // 
            // SpeedUpDown
            // 
            this.SpeedUpDown.DecimalPlaces = 1;
            this.SpeedUpDown.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.SpeedUpDown.Location = new System.Drawing.Point(5, 18);
            this.SpeedUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.SpeedUpDown.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.SpeedUpDown.Name = "SpeedUpDown";
            this.SpeedUpDown.Size = new System.Drawing.Size(80, 20);
            this.SpeedUpDown.TabIndex = 3;
            // 
            // numRPM
            // 
            this.numRPM.Enabled = false;
            this.numRPM.Location = new System.Drawing.Point(5, 39);
            this.numRPM.Maximum = new decimal(new int[] {
            65536,
            0,
            0,
            0});
            this.numRPM.Name = "numRPM";
            this.numRPM.Size = new System.Drawing.Size(80, 20);
            this.numRPM.TabIndex = 4;
            this.numRPM.Value = new decimal(new int[] {
            600,
            0,
            0,
            0});
            // 
            // numOdometer
            // 
            this.numOdometer.DecimalPlaces = 1;
            this.numOdometer.Location = new System.Drawing.Point(5, 62);
            this.numOdometer.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numOdometer.Name = "numOdometer";
            this.numOdometer.Size = new System.Drawing.Size(80, 20);
            this.numOdometer.TabIndex = 8;
            this.numOdometer.Value = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            // 
            // lblTimeAtSpeed
            // 
            this.lblTimeAtSpeed.AutoSize = true;
            this.lblTimeAtSpeed.Location = new System.Drawing.Point(7, 87);
            this.lblTimeAtSpeed.Name = "lblTimeAtSpeed";
            this.lblTimeAtSpeed.Size = new System.Drawing.Size(49, 13);
            this.lblTimeAtSpeed.TabIndex = 9;
            this.lblTimeAtSpeed.Text = "00:00:00";
            // 
            // numLoad
            // 
            this.numLoad.DecimalPlaces = 1;
            this.numLoad.Enabled = false;
            this.numLoad.Location = new System.Drawing.Point(5, 105);
            this.numLoad.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numLoad.Name = "numLoad";
            this.numLoad.Size = new System.Drawing.Size(80, 20);
            this.numLoad.TabIndex = 17;
            // 
            // numPTO
            // 
            this.numPTO.DecimalPlaces = 3;
            this.numPTO.Location = new System.Drawing.Point(5, 126);
            this.numPTO.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numPTO.Name = "numPTO";
            this.numPTO.Size = new System.Drawing.Size(80, 20);
            this.numPTO.TabIndex = 21;
            this.numPTO.Value = new decimal(new int[] {
            600000,
            0,
            0,
            196608});
            // 
            // lblSpeed
            // 
            this.lblSpeed.AutoSize = true;
            this.lblSpeed.Location = new System.Drawing.Point(120, 21);
            this.lblSpeed.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblSpeed.Name = "lblSpeed";
            this.lblSpeed.Size = new System.Drawing.Size(67, 13);
            this.lblSpeed.TabIndex = 4;
            this.lblSpeed.Text = "Speed (mph)";
            // 
            // lblRPM
            // 
            this.lblRPM.AutoSize = true;
            this.lblRPM.Location = new System.Drawing.Point(90, 43);
            this.lblRPM.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblRPM.Name = "lblRPM";
            this.lblRPM.Size = new System.Drawing.Size(57, 13);
            this.lblRPM.TabIndex = 4;
            this.lblRPM.Text = "RPM (rpm)";
            // 
            // lblOdo
            // 
            this.lblOdo.AutoSize = true;
            this.lblOdo.Location = new System.Drawing.Point(90, 64);
            this.lblOdo.Name = "lblOdo";
            this.lblOdo.Size = new System.Drawing.Size(72, 13);
            this.lblOdo.TabIndex = 4;
            this.lblOdo.Text = "Odometer (mi)";
            // 
            // lblTimeAtSpd
            // 
            this.lblTimeAtSpd.AutoSize = true;
            this.lblTimeAtSpd.Location = new System.Drawing.Point(90, 86);
            this.lblTimeAtSpd.Name = "lblTimeAtSpd";
            this.lblTimeAtSpd.Size = new System.Drawing.Size(93, 13);
            this.lblTimeAtSpd.TabIndex = 4;
            this.lblTimeAtSpd.Text = "Time at this speed";
            // 
            // lblEngineLoad
            // 
            this.lblEngineLoad.AutoSize = true;
            this.lblEngineLoad.Location = new System.Drawing.Point(90, 107);
            this.lblEngineLoad.Name = "lblEngineLoad";
            this.lblEngineLoad.Size = new System.Drawing.Size(48, 13);
            this.lblEngineLoad.TabIndex = 18;
            this.lblEngineLoad.Text = "Load (%)";
            // 
            // lblTotalPTO
            // 
            this.lblTotalPTO.AutoSize = true;
            this.lblTotalPTO.Location = new System.Drawing.Point(90, 127);
            this.lblTotalPTO.Name = "lblTotalPTO";
            this.lblTotalPTO.Size = new System.Drawing.Size(74, 13);
            this.lblTotalPTO.TabIndex = 22;
            this.lblTotalPTO.Text = "Total PTO (hr)";
            // 
            // MapBtn
            // 
            this.MapBtn.Location = new System.Drawing.Point(741, 68);
            this.MapBtn.Margin = new System.Windows.Forms.Padding(2);
            this.MapBtn.Name = "MapBtn";
            this.MapBtn.Size = new System.Drawing.Size(81, 28);
            this.MapBtn.TabIndex = 33;
            this.MapBtn.Text = "Map";
            this.MapBtn.UseVisualStyleBackColor = true;
            this.MapBtn.Click += new System.EventHandler(this.MapBtn_Click);
            // 
            // grpFuel
            // 
            this.grpFuel.Controls.Add(this.chkSetFuelRate);
            this.grpFuel.Controls.Add(this.numRate);
            this.grpFuel.Controls.Add(this.numFuel);
            this.grpFuel.Controls.Add(this.lblFuelRate);
            this.grpFuel.Controls.Add(this.lblTotalFuel);
            this.grpFuel.Location = new System.Drawing.Point(9, 333);
            this.grpFuel.Name = "grpFuel";
            this.grpFuel.Size = new System.Drawing.Size(185, 81);
            this.grpFuel.TabIndex = 4;
            this.grpFuel.TabStop = false;
            this.grpFuel.Text = "Fuel";
            // 
            // chkSetFuelRate
            // 
            this.chkSetFuelRate.AutoSize = true;
            this.chkSetFuelRate.Location = new System.Drawing.Point(5, 40);
            this.chkSetFuelRate.Margin = new System.Windows.Forms.Padding(2);
            this.chkSetFuelRate.Name = "chkSetFuelRate";
            this.chkSetFuelRate.Size = new System.Drawing.Size(91, 17);
            this.chkSetFuelRate.TabIndex = 17;
            this.chkSetFuelRate.Text = "Set Fuel Rate";
            this.chkSetFuelRate.UseVisualStyleBackColor = true;
            this.chkSetFuelRate.CheckedChanged += new System.EventHandler(this.chkSetFuelRate_CheckedChanged);
            // 
            // numRate
            // 
            this.numRate.DecimalPlaces = 3;
            this.numRate.Enabled = false;
            this.numRate.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numRate.Location = new System.Drawing.Point(5, 19);
            this.numRate.Name = "numRate";
            this.numRate.Size = new System.Drawing.Size(85, 20);
            this.numRate.TabIndex = 14;
            this.numRate.Value = new decimal(new int[] {
            89,
            0,
            0,
            131072});
            // 
            // numFuel
            // 
            this.numFuel.DecimalPlaces = 3;
            this.numFuel.Increment = new decimal(new int[] {
            125,
            0,
            0,
            196608});
            this.numFuel.Location = new System.Drawing.Point(5, 58);
            this.numFuel.Maximum = new decimal(new int[] {
            -1,
            0,
            0,
            0});
            this.numFuel.Name = "numFuel";
            this.numFuel.Size = new System.Drawing.Size(85, 20);
            this.numFuel.TabIndex = 15;
            this.numFuel.Value = new decimal(new int[] {
            20000,
            0,
            0,
            65536});
            // 
            // lblFuelRate
            // 
            this.lblFuelRate.AutoSize = true;
            this.lblFuelRate.Location = new System.Drawing.Point(95, 21);
            this.lblFuelRate.Name = "lblFuelRate";
            this.lblFuelRate.Size = new System.Drawing.Size(87, 13);
            this.lblFuelRate.TabIndex = 13;
            this.lblFuelRate.Text = "Fuel Rate (gal/h)";
            // 
            // lblTotalFuel
            // 
            this.lblTotalFuel.AutoSize = true;
            this.lblTotalFuel.Location = new System.Drawing.Point(96, 58);
            this.lblTotalFuel.Name = "lblTotalFuel";
            this.lblTotalFuel.Size = new System.Drawing.Size(77, 13);
            this.lblTotalFuel.TabIndex = 16;
            this.lblTotalFuel.Text = "Total Fuel (gal)";
            // 
            // grpPedals
            // 
            this.grpPedals.Controls.Add(this.pnlBrakingRate);
            this.grpPedals.Controls.Add(this.chkBrk);
            this.grpPedals.Controls.Add(this.chkPTO);
            this.grpPedals.Controls.Add(this.tbThrottle);
            this.grpPedals.Controls.Add(this.lblBrake);
            this.grpPedals.Controls.Add(this.lblThrottle);
            this.grpPedals.Controls.Add(this.lblPTO);
            this.grpPedals.Location = new System.Drawing.Point(9, 209);
            this.grpPedals.Name = "grpPedals";
            this.grpPedals.Size = new System.Drawing.Size(185, 118);
            this.grpPedals.TabIndex = 11;
            this.grpPedals.TabStop = false;
            this.grpPedals.Text = "Pedals/Switches";
            // 
            // pnlBrakingRate
            // 
            this.pnlBrakingRate.Controls.Add(this.rbtnBrake5Mph);
            this.pnlBrakingRate.Controls.Add(this.rbtnBrake2Mph);
            this.pnlBrakingRate.Location = new System.Drawing.Point(10, 36);
            this.pnlBrakingRate.Name = "pnlBrakingRate";
            this.pnlBrakingRate.Size = new System.Drawing.Size(80, 44);
            this.pnlBrakingRate.TabIndex = 21;
            // 
            // rbtnBrake5Mph
            // 
            this.rbtnBrake5Mph.AutoSize = true;
            this.rbtnBrake5Mph.Location = new System.Drawing.Point(19, 21);
            this.rbtnBrake5Mph.Name = "rbtnBrake5Mph";
            this.rbtnBrake5Mph.Size = new System.Drawing.Size(54, 17);
            this.rbtnBrake5Mph.TabIndex = 1;
            this.rbtnBrake5Mph.TabStop = true;
            this.rbtnBrake5Mph.Text = "5 mph";
            this.rbtnBrake5Mph.UseVisualStyleBackColor = true;
            // 
            // rbtnBrake2Mph
            // 
            this.rbtnBrake2Mph.AutoSize = true;
            this.rbtnBrake2Mph.Location = new System.Drawing.Point(19, 4);
            this.rbtnBrake2Mph.Name = "rbtnBrake2Mph";
            this.rbtnBrake2Mph.Size = new System.Drawing.Size(54, 17);
            this.rbtnBrake2Mph.TabIndex = 0;
            this.rbtnBrake2Mph.TabStop = true;
            this.rbtnBrake2Mph.Text = "2 mph";
            this.rbtnBrake2Mph.UseVisualStyleBackColor = true;
            this.rbtnBrake2Mph.CheckedChanged += new System.EventHandler(this.rbtnBrake2Mph_CheckedChanged);
            // 
            // chkBrk
            // 
            this.chkBrk.AutoSize = true;
            this.chkBrk.Location = new System.Drawing.Point(13, 22);
            this.chkBrk.Name = "chkBrk";
            this.chkBrk.Size = new System.Drawing.Size(15, 14);
            this.chkBrk.TabIndex = 17;
            this.chkBrk.UseVisualStyleBackColor = true;
            // 
            // chkPTO
            // 
            this.chkPTO.AutoSize = true;
            this.chkPTO.Location = new System.Drawing.Point(13, 94);
            this.chkPTO.Name = "chkPTO";
            this.chkPTO.Size = new System.Drawing.Size(15, 14);
            this.chkPTO.TabIndex = 20;
            this.chkPTO.UseVisualStyleBackColor = true;
            // 
            // tbThrottle
            // 
            this.tbThrottle.Location = new System.Drawing.Point(125, 15);
            this.tbThrottle.Maximum = 1200;
            this.tbThrottle.Name = "tbThrottle";
            this.tbThrottle.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.tbThrottle.Size = new System.Drawing.Size(45, 97);
            this.tbThrottle.TabIndex = 12;
            this.tbThrottle.Scroll += new System.EventHandler(this.tbThrottle_Scroll);
            // 
            // lblBrake
            // 
            this.lblBrake.AutoSize = true;
            this.lblBrake.Location = new System.Drawing.Point(33, 22);
            this.lblBrake.Name = "lblBrake";
            this.lblBrake.Size = new System.Drawing.Size(49, 13);
            this.lblBrake.TabIndex = 16;
            this.lblBrake.Text = "Brake @";
            // 
            // lblThrottle
            // 
            this.lblThrottle.AutoSize = true;
            this.lblThrottle.Location = new System.Drawing.Point(77, 88);
            this.lblThrottle.Name = "lblThrottle";
            this.lblThrottle.Size = new System.Drawing.Size(43, 13);
            this.lblThrottle.TabIndex = 16;
            this.lblThrottle.Text = "Throttle";
            // 
            // lblPTO
            // 
            this.lblPTO.AutoSize = true;
            this.lblPTO.Location = new System.Drawing.Point(33, 93);
            this.lblPTO.Name = "lblPTO";
            this.lblPTO.Size = new System.Drawing.Size(29, 13);
            this.lblPTO.TabIndex = 16;
            this.lblPTO.Text = "PTO";
            // 
            // grpJ1708
            // 
            this.grpJ1708.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpJ1708.Controls.Add(this.lstOutput);
            this.grpJ1708.Controls.Add(this.statusStrip1);
            this.grpJ1708.Location = new System.Drawing.Point(10, 416);
            this.grpJ1708.Name = "grpJ1708";
            this.grpJ1708.Size = new System.Drawing.Size(815, 194);
            this.grpJ1708.TabIndex = 8;
            this.grpJ1708.TabStop = false;
            this.grpJ1708.Text = "J1708";
            // 
            // lstOutput
            // 
            this.lstOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstOutput.FormattingEnabled = true;
            this.lstOutput.Location = new System.Drawing.Point(5, 19);
            this.lstOutput.Name = "lstOutput";
            this.lstOutput.Size = new System.Drawing.Size(805, 147);
            this.lstOutput.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgress});
            this.statusStrip1.Location = new System.Drawing.Point(3, 167);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(809, 24);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsProgress
            // 
            this.tsProgress.AutoSize = false;
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(150, 18);
            this.tsProgress.Step = 1;
            this.tsProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.tsProgress.ToolTipText = "Route progress";
            // 
            // dlgRecord
            // 
            this.dlgRecord.DefaultExt = "J1708";
            this.dlgRecord.FileName = "*.J1708";
            this.dlgRecord.Filter = "J1708 recording (*.J1708)|*.J1708";
            this.dlgRecord.Title = "Record J1708";
            // 
            // dlgPlayback
            // 
            this.dlgPlayback.DefaultExt = "J1708";
            this.dlgPlayback.FileName = "*.J1708";
            this.dlgPlayback.Filter = "J1708 recording (*.J1708)|*.J1708";
            this.dlgPlayback.Title = "Playback J1708 Recording";
            // 
            // HACCNum
            // 
            this.HACCNum.DecimalPlaces = 1;
            this.HACCNum.Enabled = false;
            this.HACCNum.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.HACCNum.Location = new System.Drawing.Point(4, 18);
            this.HACCNum.Margin = new System.Windows.Forms.Padding(2);
            this.HACCNum.Name = "HACCNum";
            this.HACCNum.Size = new System.Drawing.Size(80, 20);
            this.HACCNum.TabIndex = 12;
            // 
            // lblHAC
            // 
            this.lblHAC.AutoSize = true;
            this.lblHAC.Location = new System.Drawing.Point(108, 19);
            this.lblHAC.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblHAC.Name = "lblHAC";
            this.lblHAC.Size = new System.Drawing.Size(36, 13);
            this.lblHAC.TabIndex = 13;
            this.lblHAC.Text = "HACC";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.ChkBoxHACC);
            this.groupBox1.Controls.Add(this.lblHAC);
            this.groupBox1.Controls.Add(this.HACCNum);
            this.groupBox1.Location = new System.Drawing.Point(243, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(151, 47);
            this.groupBox1.TabIndex = 14;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "NMEA HACC Mod";
            // 
            // ChkBoxHACC
            // 
            this.ChkBoxHACC.AutoSize = true;
            this.ChkBoxHACC.Location = new System.Drawing.Point(89, 20);
            this.ChkBoxHACC.Name = "ChkBoxHACC";
            this.ChkBoxHACC.Size = new System.Drawing.Size(15, 14);
            this.ChkBoxHACC.TabIndex = 18;
            this.ChkBoxHACC.UseVisualStyleBackColor = true;
            this.ChkBoxHACC.CheckedChanged += new System.EventHandler(this.ChkBoxHACC_CheckedChanged);
            // 
            // PlaybackStarter
            // 
            this.PlaybackStarter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PlaybackStarter.Controls.Add(this.GPSTimeOnlyLabel);
            this.PlaybackStarter.Controls.Add(this.GPSTimeOnly);
            this.PlaybackStarter.Controls.Add(this.lblNone);
            this.PlaybackStarter.Controls.Add(this.radioNone);
            this.PlaybackStarter.Controls.Add(this.radioSetPlaybackTime);
            this.PlaybackStarter.Controls.Add(this.lblPastPlayback);
            this.PlaybackStarter.Controls.Add(this.radioStart);
            this.PlaybackStarter.Controls.Add(this.lblTimeDef);
            this.PlaybackStarter.Controls.Add(this.lblTimeLeft);
            this.PlaybackStarter.Controls.Add(this.lblCurrentTime);
            this.PlaybackStarter.Controls.Add(this.lblStop);
            this.PlaybackStarter.Controls.Add(this.lblStart);
            this.PlaybackStarter.Controls.Add(this.StopDate);
            this.PlaybackStarter.Controls.Add(this.countDown);
            this.PlaybackStarter.Controls.Add(this.chkStop);
            this.PlaybackStarter.Controls.Add(this.StartDate);
            this.PlaybackStarter.Controls.Add(this.CurrentTime);
            this.PlaybackStarter.Location = new System.Drawing.Point(399, 108);
            this.PlaybackStarter.Margin = new System.Windows.Forms.Padding(2);
            this.PlaybackStarter.Name = "PlaybackStarter";
            this.PlaybackStarter.Padding = new System.Windows.Forms.Padding(2);
            this.PlaybackStarter.Size = new System.Drawing.Size(426, 95);
            this.PlaybackStarter.TabIndex = 15;
            this.PlaybackStarter.TabStop = false;
            this.PlaybackStarter.Text = "Playback Starter/Stopper";
            // 
            // GPSTimeOnlyLabel
            // 
            this.GPSTimeOnlyLabel.AutoSize = true;
            this.GPSTimeOnlyLabel.Location = new System.Drawing.Point(175, 40);
            this.GPSTimeOnlyLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.GPSTimeOnlyLabel.Name = "GPSTimeOnlyLabel";
            this.GPSTimeOnlyLabel.Size = new System.Drawing.Size(79, 13);
            this.GPSTimeOnlyLabel.TabIndex = 21;
            this.GPSTimeOnlyLabel.Text = "GPS Time Only";
            // 
            // GPSTimeOnly
            // 
            this.GPSTimeOnly.AutoSize = true;
            this.GPSTimeOnly.Location = new System.Drawing.Point(157, 40);
            this.GPSTimeOnly.Name = "GPSTimeOnly";
            this.GPSTimeOnly.Size = new System.Drawing.Size(15, 14);
            this.GPSTimeOnly.TabIndex = 20;
            this.GPSTimeOnly.UseVisualStyleBackColor = true;
            // 
            // lblNone
            // 
            this.lblNone.AutoSize = true;
            this.lblNone.Location = new System.Drawing.Point(367, 22);
            this.lblNone.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblNone.Name = "lblNone";
            this.lblNone.Size = new System.Drawing.Size(33, 13);
            this.lblNone.TabIndex = 19;
            this.lblNone.Text = "None";
            // 
            // radioNone
            // 
            this.radioNone.AutoSize = true;
            this.radioNone.Checked = true;
            this.radioNone.Location = new System.Drawing.Point(349, 22);
            this.radioNone.Margin = new System.Windows.Forms.Padding(2);
            this.radioNone.Name = "radioNone";
            this.radioNone.Size = new System.Drawing.Size(14, 13);
            this.radioNone.TabIndex = 18;
            this.radioNone.TabStop = true;
            this.radioNone.UseVisualStyleBackColor = true;
            // 
            // radioSetPlaybackTime
            // 
            this.radioSetPlaybackTime.AutoSize = true;
            this.radioSetPlaybackTime.Location = new System.Drawing.Point(217, 21);
            this.radioSetPlaybackTime.Margin = new System.Windows.Forms.Padding(2);
            this.radioSetPlaybackTime.Name = "radioSetPlaybackTime";
            this.radioSetPlaybackTime.Size = new System.Drawing.Size(14, 13);
            this.radioSetPlaybackTime.TabIndex = 17;
            this.radioSetPlaybackTime.TabStop = true;
            this.radioSetPlaybackTime.UseVisualStyleBackColor = true;
            // 
            // lblPastPlayback
            // 
            this.lblPastPlayback.AutoSize = true;
            this.lblPastPlayback.Location = new System.Drawing.Point(235, 21);
            this.lblPastPlayback.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblPastPlayback.Name = "lblPastPlayback";
            this.lblPastPlayback.Size = new System.Drawing.Size(96, 13);
            this.lblPastPlayback.TabIndex = 14;
            this.lblPastPlayback.Text = "Set Playback Time";
            // 
            // radioStart
            // 
            this.radioStart.AutoSize = true;
            this.radioStart.Location = new System.Drawing.Point(157, 21);
            this.radioStart.Margin = new System.Windows.Forms.Padding(2);
            this.radioStart.Name = "radioStart";
            this.radioStart.Size = new System.Drawing.Size(14, 13);
            this.radioStart.TabIndex = 16;
            this.radioStart.TabStop = true;
            this.radioStart.UseVisualStyleBackColor = true;
            // 
            // lblTimeDef
            // 
            this.lblTimeDef.AutoSize = true;
            this.lblTimeDef.Location = new System.Drawing.Point(211, 73);
            this.lblTimeDef.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTimeDef.Name = "lblTimeDef";
            this.lblTimeDef.Size = new System.Drawing.Size(149, 13);
            this.lblTimeDef.TabIndex = 12;
            this.lblTimeDef.Text = "Days/Hours:Minutes:Seconds";
            // 
            // lblTimeLeft
            // 
            this.lblTimeLeft.AutoSize = true;
            this.lblTimeLeft.Location = new System.Drawing.Point(346, 58);
            this.lblTimeLeft.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTimeLeft.Name = "lblTimeLeft";
            this.lblTimeLeft.Size = new System.Drawing.Size(51, 13);
            this.lblTimeLeft.TabIndex = 11;
            this.lblTimeLeft.Text = "Time Left";
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Location = new System.Drawing.Point(345, 40);
            this.lblCurrentTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(67, 13);
            this.lblCurrentTime.TabIndex = 10;
            this.lblCurrentTime.Text = "Current Time";
            // 
            // lblStop
            // 
            this.lblStop.AutoSize = true;
            this.lblStop.Location = new System.Drawing.Point(176, 58);
            this.lblStop.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStop.Name = "lblStop";
            this.lblStop.Size = new System.Drawing.Size(29, 13);
            this.lblStop.TabIndex = 9;
            this.lblStop.Text = "Stop";
            // 
            // lblStart
            // 
            this.lblStart.AutoSize = true;
            this.lblStart.Location = new System.Drawing.Point(175, 20);
            this.lblStart.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(29, 13);
            this.lblStart.TabIndex = 8;
            this.lblStart.Text = "Start";
            // 
            // StopDate
            // 
            this.StopDate.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.StopDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StopDate.Location = new System.Drawing.Point(5, 57);
            this.StopDate.Margin = new System.Windows.Forms.Padding(2);
            this.StopDate.Name = "StopDate";
            this.StopDate.Size = new System.Drawing.Size(148, 20);
            this.StopDate.TabIndex = 7;
            // 
            // countDown
            // 
            this.countDown.AutoSize = true;
            this.countDown.Location = new System.Drawing.Point(257, 58);
            this.countDown.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.countDown.Name = "countDown";
            this.countDown.Size = new System.Drawing.Size(66, 13);
            this.countDown.TabIndex = 6;
            this.countDown.Text = "00/00:00:00";
            // 
            // chkStop
            // 
            this.chkStop.AutoSize = true;
            this.chkStop.Location = new System.Drawing.Point(157, 58);
            this.chkStop.Margin = new System.Windows.Forms.Padding(2);
            this.chkStop.Name = "chkStop";
            this.chkStop.Size = new System.Drawing.Size(15, 14);
            this.chkStop.TabIndex = 5;
            this.chkStop.UseVisualStyleBackColor = true;
            // 
            // StartDate
            // 
            this.StartDate.CustomFormat = "MM/dd/yyyy HH:mm:ss";
            this.StartDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.StartDate.Location = new System.Drawing.Point(4, 18);
            this.StartDate.Margin = new System.Windows.Forms.Padding(2);
            this.StartDate.Name = "StartDate";
            this.StartDate.Size = new System.Drawing.Size(149, 20);
            this.StartDate.TabIndex = 2;
            // 
            // CurrentTime
            // 
            this.CurrentTime.AutoSize = true;
            this.CurrentTime.Location = new System.Drawing.Point(257, 40);
            this.CurrentTime.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentTime.Name = "CurrentTime";
            this.CurrentTime.Size = new System.Drawing.Size(49, 13);
            this.CurrentTime.TabIndex = 1;
            this.CurrentTime.Text = "00:00:00";
            // 
            // lblFileLoaded
            // 
            this.lblFileLoaded.AutoSize = true;
            this.lblFileLoaded.Location = new System.Drawing.Point(8, 29);
            this.lblFileLoaded.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblFileLoaded.Name = "lblFileLoaded";
            this.lblFileLoaded.Size = new System.Drawing.Size(65, 13);
            this.lblFileLoaded.TabIndex = 16;
            this.lblFileLoaded.Text = "File Loaded:";
            // 
            // FileLoaded
            // 
            this.FileLoaded.AutoSize = true;
            this.FileLoaded.Location = new System.Drawing.Point(76, 29);
            this.FileLoaded.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.FileLoaded.Name = "FileLoaded";
            this.FileLoaded.Size = new System.Drawing.Size(0, 13);
            this.FileLoaded.TabIndex = 17;
            // 
            // lblCurrentCoordinates
            // 
            this.lblCurrentCoordinates.AutoSize = true;
            this.lblCurrentCoordinates.Location = new System.Drawing.Point(8, 48);
            this.lblCurrentCoordinates.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblCurrentCoordinates.Name = "lblCurrentCoordinates";
            this.lblCurrentCoordinates.Size = new System.Drawing.Size(103, 13);
            this.lblCurrentCoordinates.TabIndex = 18;
            this.lblCurrentCoordinates.Text = "Current Coordinates:";
            // 
            // CurrentCoordinates
            // 
            this.CurrentCoordinates.AutoSize = true;
            this.CurrentCoordinates.Location = new System.Drawing.Point(112, 48);
            this.CurrentCoordinates.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CurrentCoordinates.Name = "CurrentCoordinates";
            this.CurrentCoordinates.Size = new System.Drawing.Size(0, 13);
            this.CurrentCoordinates.TabIndex = 19;
            // 
            // lblTimeStarted
            // 
            this.lblTimeStarted.AutoSize = true;
            this.lblTimeStarted.Location = new System.Drawing.Point(630, 48);
            this.lblTimeStarted.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblTimeStarted.Name = "lblTimeStarted";
            this.lblTimeStarted.Size = new System.Drawing.Size(70, 13);
            this.lblTimeStarted.TabIndex = 20;
            this.lblTimeStarted.Text = "Time Started:";
            // 
            // TimeStarted
            // 
            this.TimeStarted.AutoSize = true;
            this.TimeStarted.Location = new System.Drawing.Point(704, 48);
            this.TimeStarted.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.TimeStarted.Name = "TimeStarted";
            this.TimeStarted.Size = new System.Drawing.Size(112, 13);
            this.TimeStarted.TabIndex = 21;
            this.TimeStarted.Text = "mm/dd/yyyy hh:mm:ss";
            // 
            // OnOffBtn
            // 
            this.OnOffBtn.BackColor = System.Drawing.Color.Red;
            this.OnOffBtn.Enabled = false;
            this.OnOffBtn.Location = new System.Drawing.Point(9, 63);
            this.OnOffBtn.Margin = new System.Windows.Forms.Padding(2);
            this.OnOffBtn.Name = "OnOffBtn";
            this.OnOffBtn.Size = new System.Drawing.Size(149, 38);
            this.OnOffBtn.TabIndex = 22;
            this.OnOffBtn.Text = "OFF";
            this.OnOffBtn.UseVisualStyleBackColor = false;
            // 
            // chkLoopKmlFile
            // 
            this.chkLoopKmlFile.AutoSize = true;
            this.chkLoopKmlFile.Location = new System.Drawing.Point(163, 84);
            this.chkLoopKmlFile.Name = "chkLoopKmlFile";
            this.chkLoopKmlFile.Size = new System.Drawing.Size(89, 17);
            this.chkLoopKmlFile.TabIndex = 34;
            this.chkLoopKmlFile.Text = "Loop Kml File";
            this.chkLoopKmlFile.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 616);
            this.Controls.Add(this.chkLoopKmlFile);
            this.Controls.Add(this.OnOffBtn);
            this.Controls.Add(this.MapBtn);
            this.Controls.Add(this.TimeStarted);
            this.Controls.Add(this.lblTimeStarted);
            this.Controls.Add(this.CurrentCoordinates);
            this.Controls.Add(this.lblCurrentCoordinates);
            this.Controls.Add(this.FileLoaded);
            this.Controls.Add(this.lblFileLoaded);
            this.Controls.Add(this.PlaybackStarter);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpSpdDist);
            this.Controls.Add(this.grpFuel);
            this.Controls.Add(this.grpPedals);
            this.Controls.Add(this.grpJ1708);
            this.Controls.Add(this.grpNAV);
            this.Controls.Add(this.grpVehicle);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "MainForm";
            this.Text = "Trip Simulator";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.SizeChanged += new System.EventHandler(this.MainForm_SizeChanged);
            this.grpNAV.ResumeLayout(false);
            this.grpVehicle.ResumeLayout(false);
            this.grpVehicle.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpSpdDist.ResumeLayout(false);
            this.grpSpdDist.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numEngineHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.SpeedUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numRPM)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numOdometer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numLoad)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPTO)).EndInit();
            this.grpFuel.ResumeLayout(false);
            this.grpFuel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numRate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numFuel)).EndInit();
            this.grpPedals.ResumeLayout(false);
            this.grpPedals.PerformLayout();
            this.pnlBrakingRate.ResumeLayout(false);
            this.pnlBrakingRate.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.tbThrottle)).EndInit();
            this.grpJ1708.ResumeLayout(false);
            this.grpJ1708.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.HACCNum)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.PlaybackStarter.ResumeLayout(false);
            this.PlaybackStarter.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox grpNAV;
        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button CameraLockButton;
        private System.Windows.Forms.ComboBox NAVComboBox;
        private System.Windows.Forms.GroupBox grpVehicle;
        private System.Windows.Forms.ComboBox VehicleComboBox;
        private System.Windows.Forms.CheckBox chkVehicle;
        private System.Windows.Forms.Label lblVehicleData;
        private System.Windows.Forms.Timer NmeaTimer;
        private System.Windows.Forms.Timer J1708Timer;
        private System.Windows.Forms.Timer LeapTimer;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog TheFileName;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem leapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopLeapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.GroupBox grpSpdDist;
        private System.Windows.Forms.NumericUpDown SpeedUpDown;
        private System.Windows.Forms.NumericUpDown numRPM;
        private System.Windows.Forms.NumericUpDown numOdometer;
        private System.Windows.Forms.Label lblTimeAtSpeed;
        private System.Windows.Forms.NumericUpDown numLoad;
        private System.Windows.Forms.NumericUpDown numPTO;
        private System.Windows.Forms.Label lblSpeed;
        private System.Windows.Forms.Label lblRPM;
        private System.Windows.Forms.Label lblOdo;
        private System.Windows.Forms.Label lblTimeAtSpd;
        private System.Windows.Forms.Label lblEngineLoad;
        private System.Windows.Forms.Label lblTotalPTO;
        private System.Windows.Forms.GroupBox grpFuel;
        private System.Windows.Forms.NumericUpDown numRate;
        private System.Windows.Forms.NumericUpDown numFuel;
        private System.Windows.Forms.Label lblFuelRate;
        private System.Windows.Forms.Label lblTotalFuel;
        private System.Windows.Forms.GroupBox grpPedals;
        private System.Windows.Forms.CheckBox chkBrk;
        private System.Windows.Forms.CheckBox chkPTO;
        private System.Windows.Forms.TrackBar tbThrottle;
        private System.Windows.Forms.Label lblBrake;
        private System.Windows.Forms.Label lblThrottle;
        private System.Windows.Forms.Label lblPTO;
        private System.Windows.Forms.GroupBox grpJ1708;
        private System.Windows.Forms.ListBox lstOutput;
        private System.Windows.Forms.ToolStripMenuItem mnuRecord;
        private System.Windows.Forms.ToolStripMenuItem mnuRecordTo;
        private System.Windows.Forms.ToolStripMenuItem mnuStopRecording;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem mnuPlaybackFrom;
        private System.Windows.Forms.ToolStripMenuItem mnuStopPlayback;
        private System.Windows.Forms.SaveFileDialog dlgRecord;
        private System.Windows.Forms.OpenFileDialog dlgPlayback;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar tsProgress;
        private System.Windows.Forms.NumericUpDown HACCNum;
        private System.Windows.Forms.Label lblHAC;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox ChkBoxHACC;
        private System.Windows.Forms.GroupBox PlaybackStarter;
        private System.Windows.Forms.DateTimePicker StartDate;
        private System.Windows.Forms.Label CurrentTime;
        private System.Windows.Forms.DateTimePicker StopDate;
        private System.Windows.Forms.Label countDown;
        private System.Windows.Forms.CheckBox chkStop;
        private System.Windows.Forms.Label lblStop;
        private System.Windows.Forms.Label lblStart;
        private System.Windows.Forms.Label lblCurrentTime;
        private System.Windows.Forms.Label lblTimeDef;
        private System.Windows.Forms.Label lblTimeLeft;
        private System.Windows.Forms.Label lblPastPlayback;
        private System.Windows.Forms.RadioButton radioStart;
        private System.Windows.Forms.RadioButton radioSetPlaybackTime;
        private System.Windows.Forms.RadioButton radioNone;
        private System.Windows.Forms.Label lblNone;
        private System.Windows.Forms.Label lblNoRPM;
        private System.Windows.Forms.CheckBox chkNoRPM;
        private System.Windows.Forms.Label lblNoSpeed;
        private System.Windows.Forms.CheckBox chkNoSpeed;
        private System.Windows.Forms.CheckBox GPSTimeOnly;
        private System.Windows.Forms.Label GPSTimeOnlyLabel;
        private System.Windows.Forms.NumericUpDown numEngineHours;
        private System.Windows.Forms.Label EngineHour;
        private System.Windows.Forms.Label VehicleSpeed;
        private System.Windows.Forms.CheckBox chkNoOdo;
        private System.Windows.Forms.CheckBox chkNoTEH;
        private System.Windows.Forms.Label lblFileLoaded;
        private System.Windows.Forms.Label FileLoaded;
        private System.Windows.Forms.Label lblCurrentCoordinates;
        private System.Windows.Forms.Label CurrentCoordinates;
        private System.Windows.Forms.Button ECMClearButton;
        private System.Windows.Forms.CheckBox chkLoadFileResetECM;
        private System.Windows.Forms.Label lblTimeStarted;
        private System.Windows.Forms.Label TimeStarted;
        private System.Windows.Forms.CheckBox chkSetRPM;
        private System.Windows.Forms.Button MapBtn;
        private System.Windows.Forms.CheckBox chkSetFuelRate;
        private System.Windows.Forms.CheckBox chkSetEngineLoad;
        private System.Windows.Forms.Button OnOffBtn;
        private System.Windows.Forms.ToolStripMenuItem mapOnStartupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem initialZoomLevelMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom250FeetMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom2500FeetMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom1MileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom2MilesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem zoom5MilesMenuItem;
        private System.Windows.Forms.ToolStripMenuItem engineOnStartupMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
        private System.Windows.Forms.CheckBox chkLoopKmlFile;
        private System.Windows.Forms.ToolStripMenuItem reloadFileAtStartupMenuItem;
        private System.Windows.Forms.Panel pnlBrakingRate;
        private System.Windows.Forms.RadioButton rbtnBrake5Mph;
        private System.Windows.Forms.RadioButton rbtnBrake2Mph;
    }
}

