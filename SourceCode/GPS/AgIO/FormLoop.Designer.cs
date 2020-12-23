
namespace AgOpenGPS
{
    partial class FormLoop
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
            this.lblToAOG = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.lblFromAOG = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label3 = new System.Windows.Forms.Label();
            this.lblFromGPS = new System.Windows.Forms.Label();
            this.lblToGPS = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.lblFromUDP = new System.Windows.Forms.Label();
            this.lblToUDP = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.lblFromSteer = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblToSteer = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.lblFromMachine = new System.Windows.Forms.Label();
            this.lblToMachine = new System.Windows.Forms.Label();
            this.cboxIsTrafficOn = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblWatch = new System.Windows.Forms.Label();
            this.btnStartStopNtrip = new System.Windows.Forms.Button();
            this.lblNTRIPBytes = new System.Windows.Forms.Label();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.stripUDPConfig = new System.Windows.Forms.ToolStripDropDownButton();
            this.stripSerialPortsConfig = new System.Windows.Forms.ToolStripDropDownButton();
            this.stripRunDrive = new System.Windows.Forms.ToolStripDropDownButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.saveToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.loadToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.wizardToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.nTRIPToolStrip = new System.Windows.Forms.ToolStripMenuItem();
            this.lblBytes = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.lblCurentLon = new System.Windows.Forms.Label();
            this.lblCurrentLat = new System.Windows.Forms.Label();
            this.pcCPU = new System.Diagnostics.PerformanceCounter();
            this.pcRAM = new System.Diagnostics.PerformanceCounter();
            this.pbarCPU = new System.Windows.Forms.ProgressBar();
            this.pbarRAM = new System.Windows.Forms.ProgressBar();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.lblCPU = new System.Windows.Forms.Label();
            this.lblRAM = new System.Windows.Forms.Label();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pcCPU)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pcRAM)).BeginInit();
            this.SuspendLayout();
            // 
            // lblToAOG
            // 
            this.lblToAOG.BackColor = System.Drawing.Color.Gainsboro;
            this.lblToAOG.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToAOG.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblToAOG.Location = new System.Drawing.Point(398, 33);
            this.lblToAOG.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblToAOG.Name = "lblToAOG";
            this.lblToAOG.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblToAOG.Size = new System.Drawing.Size(70, 23);
            this.lblToAOG.TabIndex = 123;
            this.lblToAOG.Text = "---";
            this.lblToAOG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Gainsboro;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label1.Location = new System.Drawing.Point(272, 10);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 23);
            this.label1.TabIndex = 125;
            this.label1.Text = "From AOG";
            // 
            // lblFromAOG
            // 
            this.lblFromAOG.BackColor = System.Drawing.Color.Gainsboro;
            this.lblFromAOG.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromAOG.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblFromAOG.Location = new System.Drawing.Point(286, 33);
            this.lblFromAOG.Name = "lblFromAOG";
            this.lblFromAOG.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lblFromAOG.Size = new System.Drawing.Size(70, 23);
            this.lblFromAOG.TabIndex = 126;
            this.lblFromAOG.Text = "0000";
            this.lblFromAOG.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Gainsboro;
            this.label2.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label2.Location = new System.Drawing.Point(321, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 23);
            this.label2.TabIndex = 127;
            this.label2.Text = "Out";
            // 
            // timer1
            // 
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Gainsboro;
            this.label3.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label3.Location = new System.Drawing.Point(256, 96);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(45, 23);
            this.label3.TabIndex = 131;
            this.label3.Text = "UDP";
            // 
            // lblFromGPS
            // 
            this.lblFromGPS.BackColor = System.Drawing.Color.Gainsboro;
            this.lblFromGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromGPS.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblFromGPS.Location = new System.Drawing.Point(399, 130);
            this.lblFromGPS.Name = "lblFromGPS";
            this.lblFromGPS.Size = new System.Drawing.Size(70, 23);
            this.lblFromGPS.TabIndex = 130;
            this.lblFromGPS.Text = "---";
            this.lblFromGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToGPS
            // 
            this.lblToGPS.BackColor = System.Drawing.Color.Gainsboro;
            this.lblToGPS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToGPS.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblToGPS.Location = new System.Drawing.Point(306, 130);
            this.lblToGPS.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblToGPS.Name = "lblToGPS";
            this.lblToGPS.Size = new System.Drawing.Size(70, 23);
            this.lblToGPS.TabIndex = 128;
            this.lblToGPS.Text = "---";
            this.lblToGPS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.BackColor = System.Drawing.Color.Gainsboro;
            this.label7.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label7.Location = new System.Drawing.Point(257, 130);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 23);
            this.label7.TabIndex = 135;
            this.label7.Text = "GPS";
            // 
            // lblFromUDP
            // 
            this.lblFromUDP.BackColor = System.Drawing.Color.Gainsboro;
            this.lblFromUDP.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromUDP.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblFromUDP.Location = new System.Drawing.Point(399, 96);
            this.lblFromUDP.Name = "lblFromUDP";
            this.lblFromUDP.Size = new System.Drawing.Size(70, 23);
            this.lblFromUDP.TabIndex = 134;
            this.lblFromUDP.Text = "---";
            this.lblFromUDP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToUDP
            // 
            this.lblToUDP.BackColor = System.Drawing.Color.Gainsboro;
            this.lblToUDP.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToUDP.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblToUDP.Location = new System.Drawing.Point(306, 96);
            this.lblToUDP.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblToUDP.Name = "lblToUDP";
            this.lblToUDP.Size = new System.Drawing.Size(70, 23);
            this.lblToUDP.TabIndex = 132;
            this.lblToUDP.Text = "---";
            this.lblToUDP.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Gainsboro;
            this.label11.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label11.Location = new System.Drawing.Point(247, 164);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 23);
            this.label11.TabIndex = 139;
            this.label11.Text = "Steer";
            // 
            // lblFromSteer
            // 
            this.lblFromSteer.BackColor = System.Drawing.Color.Gainsboro;
            this.lblFromSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromSteer.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblFromSteer.Location = new System.Drawing.Point(399, 164);
            this.lblFromSteer.Name = "lblFromSteer";
            this.lblFromSteer.Size = new System.Drawing.Size(70, 23);
            this.lblFromSteer.TabIndex = 138;
            this.lblFromSteer.Text = "---";
            this.lblFromSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Gainsboro;
            this.label13.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label13.Location = new System.Drawing.Point(221, 198);
            this.label13.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(80, 23);
            this.label13.TabIndex = 137;
            this.label13.Text = "Machine";
            // 
            // lblToSteer
            // 
            this.lblToSteer.BackColor = System.Drawing.Color.Gainsboro;
            this.lblToSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToSteer.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblToSteer.Location = new System.Drawing.Point(306, 164);
            this.lblToSteer.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblToSteer.Name = "lblToSteer";
            this.lblToSteer.Size = new System.Drawing.Size(70, 23);
            this.lblToSteer.TabIndex = 136;
            this.lblToSteer.Text = "---";
            this.lblToSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.BackColor = System.Drawing.Color.Gainsboro;
            this.label15.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label15.Location = new System.Drawing.Point(418, 64);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 23);
            this.label15.TabIndex = 140;
            this.label15.Text = "In";
            // 
            // lblFromMachine
            // 
            this.lblFromMachine.BackColor = System.Drawing.Color.Gainsboro;
            this.lblFromMachine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFromMachine.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblFromMachine.Location = new System.Drawing.Point(399, 198);
            this.lblFromMachine.Name = "lblFromMachine";
            this.lblFromMachine.Size = new System.Drawing.Size(70, 23);
            this.lblFromMachine.TabIndex = 142;
            this.lblFromMachine.Text = "---";
            this.lblFromMachine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToMachine
            // 
            this.lblToMachine.BackColor = System.Drawing.Color.Gainsboro;
            this.lblToMachine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblToMachine.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblToMachine.Location = new System.Drawing.Point(306, 198);
            this.lblToMachine.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblToMachine.Name = "lblToMachine";
            this.lblToMachine.Size = new System.Drawing.Size(70, 23);
            this.lblToMachine.TabIndex = 141;
            this.lblToMachine.Text = "---";
            this.lblToMachine.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cboxIsTrafficOn
            // 
            this.cboxIsTrafficOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsTrafficOn.AutoSize = true;
            this.cboxIsTrafficOn.BackColor = System.Drawing.Color.Gainsboro;
            this.cboxIsTrafficOn.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cboxIsTrafficOn.Checked = true;
            this.cboxIsTrafficOn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cboxIsTrafficOn.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxIsTrafficOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsTrafficOn.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsTrafficOn.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxIsTrafficOn.Location = new System.Drawing.Point(161, 12);
            this.cboxIsTrafficOn.Name = "cboxIsTrafficOn";
            this.cboxIsTrafficOn.Size = new System.Drawing.Size(99, 33);
            this.cboxIsTrafficOn.TabIndex = 143;
            this.cboxIsTrafficOn.Text = "Bits/sec";
            this.cboxIsTrafficOn.UseVisualStyleBackColor = false;
            this.cboxIsTrafficOn.CheckedChanged += new System.EventHandler(this.CboxIsTrafficOn_CheckedChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.BackColor = System.Drawing.Color.Gainsboro;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label4.Location = new System.Drawing.Point(394, 10);
            this.label4.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 23);
            this.label4.TabIndex = 144;
            this.label4.Text = "To AOG";
            // 
            // lblWatch
            // 
            this.lblWatch.AutoSize = true;
            this.lblWatch.BackColor = System.Drawing.Color.Gainsboro;
            this.lblWatch.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblWatch.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblWatch.Location = new System.Drawing.Point(14, 47);
            this.lblWatch.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblWatch.Name = "lblWatch";
            this.lblWatch.Size = new System.Drawing.Size(50, 16);
            this.lblWatch.TabIndex = 146;
            this.lblWatch.Text = "Watch";
            // 
            // btnStartStopNtrip
            // 
            this.btnStartStopNtrip.BackColor = System.Drawing.SystemColors.ControlDark;
            this.btnStartStopNtrip.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnStartStopNtrip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartStopNtrip.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartStopNtrip.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnStartStopNtrip.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnStartStopNtrip.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStartStopNtrip.Location = new System.Drawing.Point(17, 10);
            this.btnStartStopNtrip.Margin = new System.Windows.Forms.Padding(5);
            this.btnStartStopNtrip.Name = "btnStartStopNtrip";
            this.btnStartStopNtrip.Size = new System.Drawing.Size(87, 33);
            this.btnStartStopNtrip.TabIndex = 147;
            this.btnStartStopNtrip.Text = "StartStop";
            this.btnStartStopNtrip.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnStartStopNtrip.UseVisualStyleBackColor = false;
            this.btnStartStopNtrip.Click += new System.EventHandler(this.BtnStartStopNtrip_Click);
            // 
            // lblNTRIPBytes
            // 
            this.lblNTRIPBytes.AutoSize = true;
            this.lblNTRIPBytes.BackColor = System.Drawing.Color.Gainsboro;
            this.lblNTRIPBytes.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblNTRIPBytes.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblNTRIPBytes.Location = new System.Drawing.Point(55, 70);
            this.lblNTRIPBytes.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblNTRIPBytes.Name = "lblNTRIPBytes";
            this.lblNTRIPBytes.Size = new System.Drawing.Size(45, 16);
            this.lblNTRIPBytes.TabIndex = 148;
            this.lblNTRIPBytes.Text = "Bytes";
            // 
            // statusStrip1
            // 
            this.statusStrip1.AutoSize = false;
            this.statusStrip1.BackColor = System.Drawing.Color.Gainsboro;
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(64, 64);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stripUDPConfig,
            this.stripSerialPortsConfig,
            this.stripRunDrive,
            this.toolStripDropDownButton1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 247);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(498, 84);
            this.statusStrip1.TabIndex = 149;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // stripUDPConfig
            // 
            this.stripUDPConfig.AutoSize = false;
            this.stripUDPConfig.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stripUDPConfig.Image = global::AgOpenGPS.Properties.Resources.UDPConfig;
            this.stripUDPConfig.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.stripUDPConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripUDPConfig.Name = "stripUDPConfig";
            this.stripUDPConfig.ShowDropDownArrow = false;
            this.stripUDPConfig.Size = new System.Drawing.Size(120, 82);
            this.stripUDPConfig.Text = "UDP";
            this.stripUDPConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.stripUDPConfig.Click += new System.EventHandler(this.StripUDPConfig_Click);
            // 
            // stripSerialPortsConfig
            // 
            this.stripSerialPortsConfig.AutoSize = false;
            this.stripSerialPortsConfig.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stripSerialPortsConfig.Image = global::AgOpenGPS.Properties.Resources.ComPorts;
            this.stripSerialPortsConfig.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.stripSerialPortsConfig.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripSerialPortsConfig.Name = "stripSerialPortsConfig";
            this.stripSerialPortsConfig.ShowDropDownArrow = false;
            this.stripSerialPortsConfig.Size = new System.Drawing.Size(120, 82);
            this.stripSerialPortsConfig.Text = "Serial";
            this.stripSerialPortsConfig.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.stripSerialPortsConfig.Click += new System.EventHandler(this.StripSerialPortsConfig_Click);
            // 
            // stripRunDrive
            // 
            this.stripRunDrive.AutoSize = false;
            this.stripRunDrive.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.stripRunDrive.Image = global::AgOpenGPS.Properties.Resources.displayMenu;
            this.stripRunDrive.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.stripRunDrive.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stripRunDrive.Name = "stripRunDrive";
            this.stripRunDrive.ShowDropDownArrow = false;
            this.stripRunDrive.Size = new System.Drawing.Size(120, 82);
            this.stripRunDrive.Text = "Drive";
            this.stripRunDrive.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.stripRunDrive.Click += new System.EventHandler(this.StripRunDrive_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.AutoSize = false;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStrip,
            this.loadToolStrip,
            this.wizardToolStrip,
            this.nTRIPToolStrip});
            this.toolStripDropDownButton1.Image = global::AgOpenGPS.Properties.Resources.Settings64;
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(100, 82);
            this.toolStripDropDownButton1.Text = "toolStripDropDownButton1";
            // 
            // saveToolStrip
            // 
            this.saveToolStrip.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveToolStrip.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.saveToolStrip.Name = "saveToolStrip";
            this.saveToolStrip.Size = new System.Drawing.Size(212, 70);
            this.saveToolStrip.Text = "Save";
            this.saveToolStrip.Click += new System.EventHandler(this.SaveToolStrip_Click);
            // 
            // loadToolStrip
            // 
            this.loadToolStrip.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.loadToolStrip.Image = global::AgOpenGPS.Properties.Resources.FileLoad;
            this.loadToolStrip.Name = "loadToolStrip";
            this.loadToolStrip.Size = new System.Drawing.Size(212, 70);
            this.loadToolStrip.Text = "Load";
            this.loadToolStrip.Click += new System.EventHandler(this.LoadToolStrip_Click);
            // 
            // wizardToolStrip
            // 
            this.wizardToolStrip.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.wizardToolStrip.Image = global::AgOpenGPS.Properties.Resources.SpecialFunctions;
            this.wizardToolStrip.Name = "wizardToolStrip";
            this.wizardToolStrip.Size = new System.Drawing.Size(212, 70);
            this.wizardToolStrip.Text = "Wizard";
            this.wizardToolStrip.Click += new System.EventHandler(this.WizardToolStrip_Click);
            // 
            // nTRIPToolStrip
            // 
            this.nTRIPToolStrip.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nTRIPToolStrip.Image = global::AgOpenGPS.Properties.Resources.NtripSettings;
            this.nTRIPToolStrip.Name = "nTRIPToolStrip";
            this.nTRIPToolStrip.Size = new System.Drawing.Size(212, 70);
            this.nTRIPToolStrip.Text = "NTRIP";
            this.nTRIPToolStrip.Click += new System.EventHandler(this.NTRIPToolStrip_Click);
            // 
            // lblBytes
            // 
            this.lblBytes.AutoSize = true;
            this.lblBytes.BackColor = System.Drawing.Color.Gainsboro;
            this.lblBytes.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBytes.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblBytes.Location = new System.Drawing.Point(14, 70);
            this.lblBytes.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblBytes.Name = "lblBytes";
            this.lblBytes.Size = new System.Drawing.Size(41, 14);
            this.lblBytes.TabIndex = 150;
            this.lblBytes.Text = "Bytes";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.BackColor = System.Drawing.Color.Gainsboro;
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label6.Location = new System.Drawing.Point(13, 172);
            this.label6.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(35, 23);
            this.label6.TabIndex = 151;
            this.label6.Text = "Lat";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Gainsboro;
            this.label8.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.label8.Location = new System.Drawing.Point(10, 198);
            this.label8.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 23);
            this.label8.TabIndex = 152;
            this.label8.Text = "Lon";
            // 
            // lblCurentLon
            // 
            this.lblCurentLon.AutoSize = true;
            this.lblCurentLon.BackColor = System.Drawing.Color.Gainsboro;
            this.lblCurentLon.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurentLon.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCurentLon.Location = new System.Drawing.Point(49, 198);
            this.lblCurentLon.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCurentLon.Name = "lblCurentLon";
            this.lblCurentLon.Size = new System.Drawing.Size(54, 23);
            this.lblCurentLon.TabIndex = 154;
            this.lblCurentLon.Text = "-111";
            // 
            // lblCurrentLat
            // 
            this.lblCurrentLat.AutoSize = true;
            this.lblCurrentLat.BackColor = System.Drawing.Color.Gainsboro;
            this.lblCurrentLat.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentLat.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCurrentLat.Location = new System.Drawing.Point(49, 172);
            this.lblCurrentLat.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCurrentLat.Name = "lblCurrentLat";
            this.lblCurrentLat.Size = new System.Drawing.Size(34, 23);
            this.lblCurrentLat.TabIndex = 153;
            this.lblCurrentLat.Text = "53";
            // 
            // pcCPU
            // 
            this.pcCPU.CategoryName = "Processor";
            this.pcCPU.CounterName = "% Processor Time";
            this.pcCPU.InstanceName = "_Total";
            // 
            // pcRAM
            // 
            this.pcRAM.CategoryName = "Memory";
            this.pcRAM.CounterName = "% Committed Bytes In Use";
            // 
            // pbarCPU
            // 
            this.pbarCPU.Location = new System.Drawing.Point(57, 106);
            this.pbarCPU.Name = "pbarCPU";
            this.pbarCPU.Size = new System.Drawing.Size(100, 8);
            this.pbarCPU.TabIndex = 156;
            // 
            // pbarRAM
            // 
            this.pbarRAM.Location = new System.Drawing.Point(57, 127);
            this.pbarRAM.Name = "pbarRAM";
            this.pbarRAM.Size = new System.Drawing.Size(100, 8);
            this.pbarRAM.TabIndex = 157;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Gainsboro;
            this.label5.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label5.Location = new System.Drawing.Point(5, 123);
            this.label5.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 14);
            this.label5.TabIndex = 158;
            this.label5.Text = "RAM%";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Gainsboro;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.label9.Location = new System.Drawing.Point(5, 102);
            this.label9.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(45, 14);
            this.label9.TabIndex = 159;
            this.label9.Text = "CPU%";
            // 
            // lblCPU
            // 
            this.lblCPU.AutoSize = true;
            this.lblCPU.BackColor = System.Drawing.Color.Gainsboro;
            this.lblCPU.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCPU.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblCPU.Location = new System.Drawing.Point(165, 103);
            this.lblCPU.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblCPU.Name = "lblCPU";
            this.lblCPU.Size = new System.Drawing.Size(23, 14);
            this.lblCPU.TabIndex = 160;
            this.lblCPU.Text = "33";
            // 
            // lblRAM
            // 
            this.lblRAM.AutoSize = true;
            this.lblRAM.BackColor = System.Drawing.Color.Gainsboro;
            this.lblRAM.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRAM.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.lblRAM.Location = new System.Drawing.Point(165, 124);
            this.lblRAM.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblRAM.Name = "lblRAM";
            this.lblRAM.Size = new System.Drawing.Size(23, 14);
            this.lblRAM.TabIndex = 161;
            this.lblRAM.Text = "33";
            // 
            // FormLoop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(10F, 23F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(498, 331);
            this.Controls.Add(this.lblRAM);
            this.Controls.Add(this.lblCPU);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.pbarRAM);
            this.Controls.Add(this.pbarCPU);
            this.Controls.Add(this.btnStartStopNtrip);
            this.Controls.Add(this.lblWatch);
            this.Controls.Add(this.lblNTRIPBytes);
            this.Controls.Add(this.lblCurentLon);
            this.Controls.Add(this.lblBytes);
            this.Controls.Add(this.lblCurrentLat);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboxIsTrafficOn);
            this.Controls.Add(this.lblFromMachine);
            this.Controls.Add(this.lblToMachine);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.lblFromSteer);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.lblToSteer);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.lblFromUDP);
            this.Controls.Add(this.lblToUDP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblFromGPS);
            this.Controls.Add(this.lblToGPS);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lblFromAOG);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblToAOG);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.DarkSlateGray;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(514, 370);
            this.Name = "FormLoop";
            this.Text = "Looper";
            this.Load += new System.EventHandler(this.FormLoop_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            //((System.ComponentModel.ISupportInitialize)(this.pcCPU)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.pcRAM)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblToAOG;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblFromAOG;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblFromGPS;
        private System.Windows.Forms.Label lblToGPS;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label lblFromUDP;
        private System.Windows.Forms.Label lblToUDP;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblFromSteer;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblToSteer;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblFromMachine;
        private System.Windows.Forms.Label lblToMachine;
        private System.Windows.Forms.CheckBox cboxIsTrafficOn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblWatch;
        private System.Windows.Forms.Button btnStartStopNtrip;
        private System.Windows.Forms.Label lblNTRIPBytes;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripDropDownButton stripUDPConfig;
        private System.Windows.Forms.ToolStripDropDownButton stripSerialPortsConfig;
        private System.Windows.Forms.Label lblBytes;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label lblCurentLon;
        private System.Windows.Forms.Label lblCurrentLat;
        private System.Windows.Forms.ToolStripDropDownButton stripRunDrive;
        private System.Diagnostics.PerformanceCounter pcCPU;
        private System.Diagnostics.PerformanceCounter pcRAM;
        private System.Windows.Forms.ProgressBar pbarCPU;
        private System.Windows.Forms.ProgressBar pbarRAM;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label lblCPU;
        private System.Windows.Forms.Label lblRAM;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStrip;
        private System.Windows.Forms.ToolStripMenuItem loadToolStrip;
        private System.Windows.Forms.ToolStripMenuItem wizardToolStrip;
        private System.Windows.Forms.ToolStripMenuItem nTRIPToolStrip;
    }
}

