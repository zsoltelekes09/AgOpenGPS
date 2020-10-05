namespace AgOpenGPS
{
    partial class FormArduinoSettings
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
            this.btnChangeAttachment = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabcArduino = new System.Windows.Forms.TabControl();
            this.tabAutoSteer = new System.Windows.Forms.TabPage();
            this.chkRemoteAutoSteerButton = new System.Windows.Forms.CheckBox();
            this.chkWorkSwitchManual = new System.Windows.Forms.CheckBox();
            this.chkWorkSwActiveLow = new System.Windows.Forms.CheckBox();
            this.chkEnableWorkSwitch = new System.Windows.Forms.CheckBox();
            this.TboxMaxSensorCounts = new System.Windows.Forms.TextBox();
            this.TboxAckerman = new System.Windows.Forms.TextBox();
            this.TboxMaxSpeed = new System.Windows.Forms.TextBox();
            this.TboxMinSpeed = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cboxSteerInvertRelays = new System.Windows.Forms.CheckBox();
            this.lblSent = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.lblRecd = new System.Windows.Forms.Label();
            this.cboxMotorDrive = new System.Windows.Forms.ComboBox();
            this.tboxSerialFromAutoSteer = new System.Windows.Forms.TextBox();
            this.cboxSteerEnable = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboxMMAAxis = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cboxInclinometer = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.chkInvertRoll = new System.Windows.Forms.CheckBox();
            this.cboxConv = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkInvertSteer = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cboxEncoder = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chkInvertWAS = new System.Windows.Forms.CheckBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkBNOInstalled = new System.Windows.Forms.CheckBox();
            this.tabMachine = new System.Windows.Forms.TabPage();
            this.cboxMachInvertRelays = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.cboxIsSendMachineControlToAutoSteer = new System.Windows.Forms.CheckBox();
            this.tboxSerialFromMachine = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TboxLowerTime = new System.Windows.Forms.TextBox();
            this.TboxRaiseTime = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cboxIsHydOn = new System.Windows.Forms.CheckBox();
            this.label11 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bntOK = new System.Windows.Forms.Button();
            this.tabcArduino.SuspendLayout();
            this.tabAutoSteer.SuspendLayout();
            this.tabMachine.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnChangeAttachment
            // 
            this.btnChangeAttachment.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnChangeAttachment.BackColor = System.Drawing.Color.Transparent;
            this.btnChangeAttachment.FlatAppearance.BorderSize = 2;
            this.btnChangeAttachment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeAttachment.Image = global::AgOpenGPS.Properties.Resources.ToolAcceptChange;
            this.btnChangeAttachment.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChangeAttachment.Location = new System.Drawing.Point(773, 628);
            this.btnChangeAttachment.Name = "btnChangeAttachment";
            this.btnChangeAttachment.Size = new System.Drawing.Size(133, 62);
            this.btnChangeAttachment.TabIndex = 251;
            this.btnChangeAttachment.UseVisualStyleBackColor = false;
            this.btnChangeAttachment.Click += new System.EventHandler(this.BtnSendToSteerArduino_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(595, 628);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(172, 62);
            this.label9.TabIndex = 275;
            this.label9.Text = "Send ";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 300;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // tabcArduino
            // 
            this.tabcArduino.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabcArduino.Controls.Add(this.tabAutoSteer);
            this.tabcArduino.Controls.Add(this.tabMachine);
            this.tabcArduino.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabcArduino.ItemSize = new System.Drawing.Size(260, 50);
            this.tabcArduino.Location = new System.Drawing.Point(0, 0);
            this.tabcArduino.Name = "tabcArduino";
            this.tabcArduino.SelectedIndex = 0;
            this.tabcArduino.Size = new System.Drawing.Size(900, 610);
            this.tabcArduino.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabcArduino.TabIndex = 315;
            // 
            // tabAutoSteer
            // 
            this.tabAutoSteer.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tabAutoSteer.Controls.Add(this.chkRemoteAutoSteerButton);
            this.tabAutoSteer.Controls.Add(this.chkWorkSwitchManual);
            this.tabAutoSteer.Controls.Add(this.chkWorkSwActiveLow);
            this.tabAutoSteer.Controls.Add(this.chkEnableWorkSwitch);
            this.tabAutoSteer.Controls.Add(this.TboxMaxSensorCounts);
            this.tabAutoSteer.Controls.Add(this.TboxAckerman);
            this.tabAutoSteer.Controls.Add(this.TboxMaxSpeed);
            this.tabAutoSteer.Controls.Add(this.TboxMinSpeed);
            this.tabAutoSteer.Controls.Add(this.label12);
            this.tabAutoSteer.Controls.Add(this.cboxSteerInvertRelays);
            this.tabAutoSteer.Controls.Add(this.lblSent);
            this.tabAutoSteer.Controls.Add(this.label13);
            this.tabAutoSteer.Controls.Add(this.lblRecd);
            this.tabAutoSteer.Controls.Add(this.cboxMotorDrive);
            this.tabAutoSteer.Controls.Add(this.tboxSerialFromAutoSteer);
            this.tabAutoSteer.Controls.Add(this.cboxSteerEnable);
            this.tabAutoSteer.Controls.Add(this.label5);
            this.tabAutoSteer.Controls.Add(this.cboxMMAAxis);
            this.tabAutoSteer.Controls.Add(this.label7);
            this.tabAutoSteer.Controls.Add(this.cboxInclinometer);
            this.tabAutoSteer.Controls.Add(this.label6);
            this.tabAutoSteer.Controls.Add(this.chkInvertRoll);
            this.tabAutoSteer.Controls.Add(this.cboxConv);
            this.tabAutoSteer.Controls.Add(this.label8);
            this.tabAutoSteer.Controls.Add(this.chkInvertSteer);
            this.tabAutoSteer.Controls.Add(this.label1);
            this.tabAutoSteer.Controls.Add(this.cboxEncoder);
            this.tabAutoSteer.Controls.Add(this.label4);
            this.tabAutoSteer.Controls.Add(this.label3);
            this.tabAutoSteer.Controls.Add(this.chkInvertWAS);
            this.tabAutoSteer.Controls.Add(this.label2);
            this.tabAutoSteer.Controls.Add(this.chkBNOInstalled);
            this.tabAutoSteer.Location = new System.Drawing.Point(4, 54);
            this.tabAutoSteer.Name = "tabAutoSteer";
            this.tabAutoSteer.Padding = new System.Windows.Forms.Padding(3);
            this.tabAutoSteer.Size = new System.Drawing.Size(892, 552);
            this.tabAutoSteer.TabIndex = 0;
            this.tabAutoSteer.Text = "Auto Steer";
            // 
            // chkRemoteAutoSteerButton
            // 
            this.chkRemoteAutoSteerButton.AccessibleRole = System.Windows.Forms.AccessibleRole.WhiteSpace;
            this.chkRemoteAutoSteerButton.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkRemoteAutoSteerButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkRemoteAutoSteerButton.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkRemoteAutoSteerButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkRemoteAutoSteerButton.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRemoteAutoSteerButton.Location = new System.Drawing.Point(676, 250);
            this.chkRemoteAutoSteerButton.Name = "chkRemoteAutoSteerButton";
            this.chkRemoteAutoSteerButton.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkRemoteAutoSteerButton.Size = new System.Drawing.Size(210, 70);
            this.chkRemoteAutoSteerButton.TabIndex = 480;
            this.chkRemoteAutoSteerButton.Text = "Enable Remote AutoSteer Button";
            this.chkRemoteAutoSteerButton.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkRemoteAutoSteerButton.UseVisualStyleBackColor = true;
            // 
            // chkWorkSwitchManual
            // 
            this.chkWorkSwitchManual.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWorkSwitchManual.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkWorkSwitchManual.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkWorkSwitchManual.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWorkSwitchManual.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWorkSwitchManual.Location = new System.Drawing.Point(676, 170);
            this.chkWorkSwitchManual.Name = "chkWorkSwitchManual";
            this.chkWorkSwitchManual.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkWorkSwitchManual.Size = new System.Drawing.Size(210, 70);
            this.chkWorkSwitchManual.TabIndex = 479;
            this.chkWorkSwitchManual.Text = "Work Switch Controls Manual";
            this.chkWorkSwitchManual.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkWorkSwitchManual.UseVisualStyleBackColor = true;
            // 
            // chkWorkSwActiveLow
            // 
            this.chkWorkSwActiveLow.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkWorkSwActiveLow.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkWorkSwActiveLow.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkWorkSwActiveLow.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkWorkSwActiveLow.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWorkSwActiveLow.Location = new System.Drawing.Point(676, 90);
            this.chkWorkSwActiveLow.Name = "chkWorkSwActiveLow";
            this.chkWorkSwActiveLow.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkWorkSwActiveLow.Size = new System.Drawing.Size(210, 70);
            this.chkWorkSwActiveLow.TabIndex = 478;
            this.chkWorkSwActiveLow.Text = "Work Switch Active Low";
            this.chkWorkSwActiveLow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkWorkSwActiveLow.UseVisualStyleBackColor = true;
            // 
            // chkEnableWorkSwitch
            // 
            this.chkEnableWorkSwitch.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkEnableWorkSwitch.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkEnableWorkSwitch.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkEnableWorkSwitch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkEnableWorkSwitch.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableWorkSwitch.Location = new System.Drawing.Point(676, 10);
            this.chkEnableWorkSwitch.Name = "chkEnableWorkSwitch";
            this.chkEnableWorkSwitch.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkEnableWorkSwitch.Size = new System.Drawing.Size(210, 70);
            this.chkEnableWorkSwitch.TabIndex = 477;
            this.chkEnableWorkSwitch.Text = "Enable\r\nWork Switch";
            this.chkEnableWorkSwitch.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkEnableWorkSwitch.UseVisualStyleBackColor = true;
            // 
            // TboxMaxSensorCounts
            // 
            this.TboxMaxSensorCounts.AcceptsReturn = true;
            this.TboxMaxSensorCounts.BackColor = System.Drawing.SystemColors.Control;
            this.TboxMaxSensorCounts.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxMaxSensorCounts.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxMaxSensorCounts.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxMaxSensorCounts.Location = new System.Drawing.Point(277, 412);
            this.TboxMaxSensorCounts.MaxLength = 10;
            this.TboxMaxSensorCounts.Name = "TboxMaxSensorCounts";
            this.TboxMaxSensorCounts.Size = new System.Drawing.Size(165, 50);
            this.TboxMaxSensorCounts.TabIndex = 476;
            this.TboxMaxSensorCounts.Text = "15";
            this.TboxMaxSensorCounts.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxMaxSensorCounts.Enter += new System.EventHandler(this.TboxMaxSensorCounts_Enter);
            // 
            // TboxAckerman
            // 
            this.TboxAckerman.AcceptsReturn = true;
            this.TboxAckerman.BackColor = System.Drawing.SystemColors.Control;
            this.TboxAckerman.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxAckerman.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxAckerman.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxAckerman.Location = new System.Drawing.Point(277, 297);
            this.TboxAckerman.MaxLength = 10;
            this.TboxAckerman.Name = "TboxAckerman";
            this.TboxAckerman.Size = new System.Drawing.Size(165, 50);
            this.TboxAckerman.TabIndex = 475;
            this.TboxAckerman.Text = "100";
            this.TboxAckerman.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxAckerman.Enter += new System.EventHandler(this.TboxAckerman_Enter);
            // 
            // TboxMaxSpeed
            // 
            this.TboxMaxSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.TboxMaxSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxMaxSpeed.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxMaxSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxMaxSpeed.Location = new System.Drawing.Point(277, 159);
            this.TboxMaxSpeed.MaxLength = 10;
            this.TboxMaxSpeed.Name = "TboxMaxSpeed";
            this.TboxMaxSpeed.Size = new System.Drawing.Size(165, 50);
            this.TboxMaxSpeed.TabIndex = 474;
            this.TboxMaxSpeed.Text = "25";
            this.TboxMaxSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxMaxSpeed.Enter += new System.EventHandler(this.TboxMaxSpeed_Enter);
            // 
            // TboxMinSpeed
            // 
            this.TboxMinSpeed.BackColor = System.Drawing.SystemColors.Control;
            this.TboxMinSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxMinSpeed.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxMinSpeed.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxMinSpeed.Location = new System.Drawing.Point(245, 40);
            this.TboxMinSpeed.MaxLength = 10;
            this.TboxMinSpeed.Name = "TboxMinSpeed";
            this.TboxMinSpeed.Size = new System.Drawing.Size(165, 50);
            this.TboxMinSpeed.TabIndex = 473;
            this.TboxMinSpeed.Text = "0.0";
            this.TboxMinSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxMinSpeed.Enter += new System.EventHandler(this.TboxMinSpeed_Enter);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(415, 517);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(67, 23);
            this.label12.TabIndex = 320;
            this.label12.Text = "Check:";
            // 
            // cboxSteerInvertRelays
            // 
            this.cboxSteerInvertRelays.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxSteerInvertRelays.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxSteerInvertRelays.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxSteerInvertRelays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxSteerInvertRelays.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxSteerInvertRelays.Location = new System.Drawing.Point(460, 410);
            this.cboxSteerInvertRelays.Name = "cboxSteerInvertRelays";
            this.cboxSteerInvertRelays.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxSteerInvertRelays.Size = new System.Drawing.Size(210, 70);
            this.cboxSteerInvertRelays.TabIndex = 317;
            this.cboxSteerInvertRelays.Text = "Invert Relays";
            this.cboxSteerInvertRelays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxSteerInvertRelays.UseVisualStyleBackColor = true;
            // 
            // lblSent
            // 
            this.lblSent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblSent.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSent.Location = new System.Drawing.Point(485, 517);
            this.lblSent.Name = "lblSent";
            this.lblSent.Size = new System.Drawing.Size(68, 26);
            this.lblSent.TabIndex = 319;
            this.lblSent.Text = "Sent";
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(277, 267);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(165, 30);
            this.label13.TabIndex = 316;
            this.label13.Text = "Ackerman %";
            this.label13.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblRecd
            // 
            this.lblRecd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblRecd.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecd.Location = new System.Drawing.Point(565, 517);
            this.lblRecd.Name = "lblRecd";
            this.lblRecd.Size = new System.Drawing.Size(68, 26);
            this.lblRecd.TabIndex = 318;
            this.lblRecd.Text = "Recd";
            // 
            // cboxMotorDrive
            // 
            this.cboxMotorDrive.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxMotorDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMotorDrive.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxMotorDrive.Font = new System.Drawing.Font("Tahoma", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxMotorDrive.Items.AddRange(new object[] {
            "Cytron",
            "IBT2"});
            this.cboxMotorDrive.Location = new System.Drawing.Point(10, 42);
            this.cboxMotorDrive.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxMotorDrive.Name = "cboxMotorDrive";
            this.cboxMotorDrive.Size = new System.Drawing.Size(220, 38);
            this.cboxMotorDrive.TabIndex = 252;
            // 
            // tboxSerialFromAutoSteer
            // 
            this.tboxSerialFromAutoSteer.BackColor = System.Drawing.SystemColors.Control;
            this.tboxSerialFromAutoSteer.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxSerialFromAutoSteer.Location = new System.Drawing.Point(7, 516);
            this.tboxSerialFromAutoSteer.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tboxSerialFromAutoSteer.Name = "tboxSerialFromAutoSteer";
            this.tboxSerialFromAutoSteer.ReadOnly = true;
            this.tboxSerialFromAutoSteer.Size = new System.Drawing.Size(372, 27);
            this.tboxSerialFromAutoSteer.TabIndex = 314;
            // 
            // cboxSteerEnable
            // 
            this.cboxSteerEnable.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxSteerEnable.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxSteerEnable.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxSteerEnable.Font = new System.Drawing.Font("Tahoma", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxSteerEnable.FormattingEnabled = true;
            this.cboxSteerEnable.Items.AddRange(new object[] {
            "Button",
            "Switch"});
            this.cboxSteerEnable.Location = new System.Drawing.Point(10, 362);
            this.cboxSteerEnable.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxSteerEnable.Name = "cboxSteerEnable";
            this.cboxSteerEnable.Size = new System.Drawing.Size(220, 38);
            this.cboxSteerEnable.TabIndex = 259;
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(10, 330);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(220, 32);
            this.label5.TabIndex = 260;
            this.label5.Text = "Steer Enable";
            this.label5.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cboxMMAAxis
            // 
            this.cboxMMAAxis.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxMMAAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxMMAAxis.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxMMAAxis.Font = new System.Drawing.Font("Tahoma", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxMMAAxis.FormattingEnabled = true;
            this.cboxMMAAxis.Items.AddRange(new object[] {
            "X Axis",
            "Y Axis"});
            this.cboxMMAAxis.Location = new System.Drawing.Point(10, 282);
            this.cboxMMAAxis.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxMMAAxis.Name = "cboxMMAAxis";
            this.cboxMMAAxis.Size = new System.Drawing.Size(220, 38);
            this.cboxMMAAxis.TabIndex = 261;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.Color.Transparent;
            this.label7.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(277, 380);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(165, 30);
            this.label7.TabIndex = 274;
            this.label7.Text = "Turn Sensor";
            this.label7.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cboxInclinometer
            // 
            this.cboxInclinometer.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxInclinometer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxInclinometer.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxInclinometer.Font = new System.Drawing.Font("Tahoma", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxInclinometer.FormattingEnabled = true;
            this.cboxInclinometer.Items.AddRange(new object[] {
            "None",
            "DOGS2",
            "MMA (1C)",
            "MMA (1D)"});
            this.cboxInclinometer.Location = new System.Drawing.Point(10, 202);
            this.cboxInclinometer.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxInclinometer.Name = "cboxInclinometer";
            this.cboxInclinometer.Size = new System.Drawing.Size(220, 38);
            this.cboxInclinometer.TabIndex = 271;
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.Transparent;
            this.label6.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(10, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(220, 32);
            this.label6.TabIndex = 262;
            this.label6.Text = "MMA Axis";
            this.label6.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // chkInvertRoll
            // 
            this.chkInvertRoll.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInvertRoll.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkInvertRoll.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkInvertRoll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInvertRoll.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInvertRoll.Location = new System.Drawing.Point(460, 170);
            this.chkInvertRoll.Name = "chkInvertRoll";
            this.chkInvertRoll.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInvertRoll.Size = new System.Drawing.Size(210, 70);
            this.chkInvertRoll.TabIndex = 263;
            this.chkInvertRoll.Text = "Invert Roll";
            this.chkInvertRoll.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkInvertRoll.UseVisualStyleBackColor = true;
            // 
            // cboxConv
            // 
            this.cboxConv.BackColor = System.Drawing.Color.AliceBlue;
            this.cboxConv.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxConv.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.cboxConv.Font = new System.Drawing.Font("Tahoma", 18.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxConv.FormattingEnabled = true;
            this.cboxConv.Items.AddRange(new object[] {
            "Single",
            "Differential"});
            this.cboxConv.Location = new System.Drawing.Point(10, 122);
            this.cboxConv.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.cboxConv.Name = "cboxConv";
            this.cboxConv.Size = new System.Drawing.Size(220, 38);
            this.cboxConv.TabIndex = 270;
            // 
            // label8
            // 
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(240, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(165, 30);
            this.label8.TabIndex = 268;
            this.label8.Text = "Min Speed";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // chkInvertSteer
            // 
            this.chkInvertSteer.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInvertSteer.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkInvertSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkInvertSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInvertSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInvertSteer.Location = new System.Drawing.Point(460, 90);
            this.chkInvertSteer.Name = "chkInvertSteer";
            this.chkInvertSteer.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInvertSteer.Size = new System.Drawing.Size(210, 70);
            this.chkInvertSteer.TabIndex = 257;
            this.chkInvertSteer.Text = "Invert Steer Motor";
            this.chkInvertSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkInvertSteer.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 32);
            this.label1.TabIndex = 253;
            this.label1.Text = "Motor Driver";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cboxEncoder
            // 
            this.cboxEncoder.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxEncoder.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.cboxEncoder.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxEncoder.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxEncoder.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxEncoder.Location = new System.Drawing.Point(460, 330);
            this.cboxEncoder.Name = "cboxEncoder";
            this.cboxEncoder.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxEncoder.Size = new System.Drawing.Size(210, 70);
            this.cboxEncoder.TabIndex = 269;
            this.cboxEncoder.Text = "Turn Sensor";
            this.cboxEncoder.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxEncoder.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Transparent;
            this.label4.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(277, 129);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(165, 30);
            this.label4.TabIndex = 250;
            this.label4.Text = "Max Speed";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(10, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(220, 32);
            this.label3.TabIndex = 255;
            this.label3.Text = "A2D Convertor";
            this.label3.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // chkInvertWAS
            // 
            this.chkInvertWAS.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkInvertWAS.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkInvertWAS.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkInvertWAS.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkInvertWAS.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInvertWAS.Location = new System.Drawing.Point(460, 10);
            this.chkInvertWAS.Name = "chkInvertWAS";
            this.chkInvertWAS.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkInvertWAS.Size = new System.Drawing.Size(210, 70);
            this.chkInvertWAS.TabIndex = 256;
            this.chkInvertWAS.Text = "Invert WAS\r\n";
            this.chkInvertWAS.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkInvertWAS.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(10, 170);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 32);
            this.label2.TabIndex = 272;
            this.label2.Text = "Inclinometer";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // chkBNOInstalled
            // 
            this.chkBNOInstalled.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBNOInstalled.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.chkBNOInstalled.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.chkBNOInstalled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBNOInstalled.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBNOInstalled.Location = new System.Drawing.Point(460, 250);
            this.chkBNOInstalled.Name = "chkBNOInstalled";
            this.chkBNOInstalled.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.chkBNOInstalled.Size = new System.Drawing.Size(210, 70);
            this.chkBNOInstalled.TabIndex = 258;
            this.chkBNOInstalled.Text = "BNO Installed";
            this.chkBNOInstalled.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkBNOInstalled.UseVisualStyleBackColor = true;
            // 
            // tabMachine
            // 
            this.tabMachine.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.tabMachine.Controls.Add(this.cboxMachInvertRelays);
            this.tabMachine.Controls.Add(this.groupBox5);
            this.tabMachine.Controls.Add(this.tboxSerialFromMachine);
            this.tabMachine.Controls.Add(this.groupBox1);
            this.tabMachine.Location = new System.Drawing.Point(4, 54);
            this.tabMachine.Name = "tabMachine";
            this.tabMachine.Padding = new System.Windows.Forms.Padding(3);
            this.tabMachine.Size = new System.Drawing.Size(892, 552);
            this.tabMachine.TabIndex = 1;
            this.tabMachine.Text = "Machine";
            // 
            // cboxMachInvertRelays
            // 
            this.cboxMachInvertRelays.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxMachInvertRelays.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxMachInvertRelays.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxMachInvertRelays.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxMachInvertRelays.Location = new System.Drawing.Point(447, 261);
            this.cboxMachInvertRelays.Name = "cboxMachInvertRelays";
            this.cboxMachInvertRelays.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxMachInvertRelays.Size = new System.Drawing.Size(192, 70);
            this.cboxMachInvertRelays.TabIndex = 318;
            this.cboxMachInvertRelays.Text = "Invert Relays";
            this.cboxMachInvertRelays.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxMachInvertRelays.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.cboxIsSendMachineControlToAutoSteer);
            this.groupBox5.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox5.Location = new System.Drawing.Point(396, 46);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(279, 168);
            this.groupBox5.TabIndex = 316;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "To AutoSteer Port";
            // 
            // cboxIsSendMachineControlToAutoSteer
            // 
            this.cboxIsSendMachineControlToAutoSteer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cboxIsSendMachineControlToAutoSteer.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsSendMachineControlToAutoSteer.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsSendMachineControlToAutoSteer.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cboxIsSendMachineControlToAutoSteer.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxIsSendMachineControlToAutoSteer.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsSendMachineControlToAutoSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsSendMachineControlToAutoSteer.Location = new System.Drawing.Point(26, 66);
            this.cboxIsSendMachineControlToAutoSteer.Name = "cboxIsSendMachineControlToAutoSteer";
            this.cboxIsSendMachineControlToAutoSteer.Size = new System.Drawing.Size(230, 61);
            this.cboxIsSendMachineControlToAutoSteer.TabIndex = 312;
            this.cboxIsSendMachineControlToAutoSteer.Text = "Machine Control \r\nPGN";
            this.cboxIsSendMachineControlToAutoSteer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsSendMachineControlToAutoSteer.UseVisualStyleBackColor = false;
            // 
            // tboxSerialFromMachine
            // 
            this.tboxSerialFromMachine.BackColor = System.Drawing.SystemColors.Control;
            this.tboxSerialFromMachine.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxSerialFromMachine.Location = new System.Drawing.Point(7, 434);
            this.tboxSerialFromMachine.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.tboxSerialFromMachine.Name = "tboxSerialFromMachine";
            this.tboxSerialFromMachine.ReadOnly = true;
            this.tboxSerialFromMachine.Size = new System.Drawing.Size(559, 40);
            this.tboxSerialFromMachine.TabIndex = 315;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TboxLowerTime);
            this.groupBox1.Controls.Add(this.TboxRaiseTime);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cboxIsHydOn);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(18, 46);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(321, 360);
            this.groupBox1.TabIndex = 278;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Hydraulic Tool Lift";
            // 
            // TboxLowerTime
            // 
            this.TboxLowerTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TboxLowerTime.BackColor = System.Drawing.SystemColors.Control;
            this.TboxLowerTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxLowerTime.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxLowerTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxLowerTime.Location = new System.Drawing.Point(55, 288);
            this.TboxLowerTime.MaxLength = 10;
            this.TboxLowerTime.Name = "TboxLowerTime";
            this.TboxLowerTime.Size = new System.Drawing.Size(165, 50);
            this.TboxLowerTime.TabIndex = 475;
            this.TboxLowerTime.Text = "1";
            this.TboxLowerTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxLowerTime.Enter += new System.EventHandler(this.TboxLowerTime_Enter);
            // 
            // TboxRaiseTime
            // 
            this.TboxRaiseTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TboxRaiseTime.BackColor = System.Drawing.SystemColors.Control;
            this.TboxRaiseTime.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxRaiseTime.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxRaiseTime.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxRaiseTime.Location = new System.Drawing.Point(55, 165);
            this.TboxRaiseTime.MaxLength = 10;
            this.TboxRaiseTime.Name = "TboxRaiseTime";
            this.TboxRaiseTime.Size = new System.Drawing.Size(165, 50);
            this.TboxRaiseTime.TabIndex = 474;
            this.TboxRaiseTime.Text = "1";
            this.TboxRaiseTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxRaiseTime.Enter += new System.EventHandler(this.TboxRaiseTime_Enter);
            // 
            // label10
            // 
            this.label10.BackColor = System.Drawing.Color.Transparent;
            this.label10.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(13, 106);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(235, 54);
            this.label10.TabIndex = 270;
            this.label10.Text = "Raise Time (secs)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // cboxIsHydOn
            // 
            this.cboxIsHydOn.Appearance = System.Windows.Forms.Appearance.Button;
            this.cboxIsHydOn.BackColor = System.Drawing.Color.Transparent;
            this.cboxIsHydOn.FlatAppearance.CheckedBackColor = System.Drawing.Color.PaleGreen;
            this.cboxIsHydOn.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cboxIsHydOn.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxIsHydOn.Location = new System.Drawing.Point(55, 50);
            this.cboxIsHydOn.Name = "cboxIsHydOn";
            this.cboxIsHydOn.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboxIsHydOn.Size = new System.Drawing.Size(165, 52);
            this.cboxIsHydOn.TabIndex = 273;
            this.cboxIsHydOn.Text = "Enable";
            this.cboxIsHydOn.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.cboxIsHydOn.UseVisualStyleBackColor = false;
            // 
            // label11
            // 
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(18, 231);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(235, 54);
            this.label11.TabIndex = 272;
            this.label11.Text = "Lower Time (secs)";
            this.label11.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(61, 628);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(90, 67);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // bntOK
            // 
            this.bntOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bntOK.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.bntOK.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.bntOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bntOK.Location = new System.Drawing.Point(183, 628);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(161, 67);
            this.bntOK.TabIndex = 0;
            this.bntOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // FormArduinoSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(918, 697);
            this.ControlBox = false;
            this.Controls.Add(this.tabcArduino);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.bntOK);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnChangeAttachment);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormArduinoSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Module Configure";
            this.Load += new System.EventHandler(this.FormToolSettings_Load);
            this.tabcArduino.ResumeLayout(false);
            this.tabAutoSteer.ResumeLayout(false);
            this.tabAutoSteer.PerformLayout();
            this.tabMachine.ResumeLayout(false);
            this.tabMachine.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnChangeAttachment;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabcArduino;
        private System.Windows.Forms.TabPage tabMachine;
        private System.Windows.Forms.CheckBox cboxIsHydOn;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox tboxSerialFromMachine;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.CheckBox cboxIsSendMachineControlToAutoSteer;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.CheckBox cboxMachInvertRelays;
        private System.Windows.Forms.TextBox TboxRaiseTime;
        private System.Windows.Forms.TextBox TboxLowerTime;
        private System.Windows.Forms.TabPage tabAutoSteer;
        private System.Windows.Forms.CheckBox chkWorkSwitchManual;
        private System.Windows.Forms.CheckBox chkWorkSwActiveLow;
        private System.Windows.Forms.CheckBox chkEnableWorkSwitch;
        private System.Windows.Forms.TextBox TboxMaxSensorCounts;
        private System.Windows.Forms.TextBox TboxAckerman;
        private System.Windows.Forms.TextBox TboxMaxSpeed;
        private System.Windows.Forms.TextBox TboxMinSpeed;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox cboxSteerInvertRelays;
        private System.Windows.Forms.Label lblSent;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblRecd;
        private System.Windows.Forms.ComboBox cboxMotorDrive;
        private System.Windows.Forms.TextBox tboxSerialFromAutoSteer;
        private System.Windows.Forms.ComboBox cboxSteerEnable;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboxMMAAxis;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cboxInclinometer;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkInvertRoll;
        private System.Windows.Forms.ComboBox cboxConv;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkInvertSteer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cboxEncoder;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox chkInvertWAS;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkBNOInstalled;
        private System.Windows.Forms.CheckBox chkRemoteAutoSteerButton;
    }
}