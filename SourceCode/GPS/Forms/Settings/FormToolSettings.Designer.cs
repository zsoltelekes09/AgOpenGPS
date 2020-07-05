namespace AgOpenGPS
{
    partial class FormToolSettings
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabConfig = new System.Windows.Forms.TabPage();
            this.gboxAttachment = new System.Windows.Forms.GroupBox();
            this.btnChangeAttachment = new System.Windows.Forms.Button();
            this.rbtnTBT = new System.Windows.Forms.RadioButton();
            this.rbtnFixedRear = new System.Windows.Forms.RadioButton();
            this.rbtnFront = new System.Windows.Forms.RadioButton();
            this.rbtnTrailing = new System.Windows.Forms.RadioButton();
            this.tabHitch = new System.Windows.Forms.TabPage();
            this.btnNext = new System.Windows.Forms.Button();
            this.nudForeAft = new System.Windows.Forms.NumericUpDown();
            this.nudHitchLength = new System.Windows.Forms.NumericUpDown();
            this.nudTankHitch = new System.Windows.Forms.NumericUpDown();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.nudMappingOffDelay = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nudMappingOnDelay = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.nudLookAheadOff = new System.Windows.Forms.NumericUpDown();
            this.nudCutoffSpeed = new System.Windows.Forms.NumericUpDown();
            this.label30 = new System.Windows.Forms.Label();
            this.lblTurnOffBelowUnits = new System.Windows.Forms.Label();
            this.nudOffset = new System.Windows.Forms.NumericUpDown();
            this.label23 = new System.Windows.Forms.Label();
            this.nudTurnOffDelay = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudLookAhead = new System.Windows.Forms.NumericUpDown();
            this.nudOverlap = new System.Windows.Forms.NumericUpDown();
            this.tabSections = new System.Windows.Forms.TabPage();
            this.SectionPanel = new System.Windows.Forms.Panel();
            this.NumSections = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudDefaultSectionWidth = new System.Windows.Forms.NumericUpDown();
            this.lblVehicleToolWidth = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.nudMinApplied = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tabWorkSwitch = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkWorkSwitchManual = new System.Windows.Forms.CheckBox();
            this.chkWorkSwActiveLow = new System.Windows.Forms.CheckBox();
            this.chkEnableWorkSwitch = new System.Windows.Forms.CheckBox();
            this.lblDoNotExceed = new System.Windows.Forms.Label();
            this.lblSecTotalWidthInches = new System.Windows.Forms.Label();
            this.lblSecTotalWidthFeet = new System.Windows.Forms.Label();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.lblSecTotalWidthMeters = new System.Windows.Forms.Label();
            this.lblInchesCm = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bntOK = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.gboxAttachment.SuspendLayout();
            this.tabHitch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudForeAft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHitchLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTankHitch)).BeginInit();
            this.tabSettings.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMappingOffDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMappingOnDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLookAheadOff)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCutoffSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTurnOffDelay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLookAhead)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverlap)).BeginInit();
            this.tabSections.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumSections)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultSectionWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinApplied)).BeginInit();
            this.tabWorkSwitch.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabConfig);
            this.tabControl1.Controls.Add(this.tabHitch);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Controls.Add(this.tabSections);
            this.tabControl1.Controls.Add(this.tabWorkSwitch);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(180, 69);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(980, 600);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 0;
            // 
            // tabConfig
            // 
            this.tabConfig.BackColor = System.Drawing.SystemColors.Window;
            this.tabConfig.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabConfig.Controls.Add(this.gboxAttachment);
            this.tabConfig.Location = new System.Drawing.Point(4, 73);
            this.tabConfig.Margin = new System.Windows.Forms.Padding(4);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(4);
            this.tabConfig.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabConfig.Size = new System.Drawing.Size(972, 523);
            this.tabConfig.TabIndex = 1;
            this.tabConfig.Text = "Configuration";
            // 
            // gboxAttachment
            // 
            this.gboxAttachment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gboxAttachment.Controls.Add(this.btnChangeAttachment);
            this.gboxAttachment.Controls.Add(this.rbtnTBT);
            this.gboxAttachment.Controls.Add(this.rbtnFixedRear);
            this.gboxAttachment.Controls.Add(this.rbtnFront);
            this.gboxAttachment.Controls.Add(this.rbtnTrailing);
            this.gboxAttachment.Location = new System.Drawing.Point(28, 80);
            this.gboxAttachment.Name = "gboxAttachment";
            this.gboxAttachment.Size = new System.Drawing.Size(888, 356);
            this.gboxAttachment.TabIndex = 110;
            this.gboxAttachment.TabStop = false;
            this.gboxAttachment.Text = "Attachment Style";
            // 
            // btnChangeAttachment
            // 
            this.btnChangeAttachment.BackColor = System.Drawing.Color.Transparent;
            this.btnChangeAttachment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeAttachment.Image = global::AgOpenGPS.Properties.Resources.ToolAcceptChange;
            this.btnChangeAttachment.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChangeAttachment.Location = new System.Drawing.Point(707, 141);
            this.btnChangeAttachment.Name = "btnChangeAttachment";
            this.btnChangeAttachment.Size = new System.Drawing.Size(133, 67);
            this.btnChangeAttachment.TabIndex = 251;
            this.btnChangeAttachment.UseVisualStyleBackColor = false;
            this.btnChangeAttachment.Click += new System.EventHandler(this.BtnChangeAttachment_Click);
            // 
            // rbtnTBT
            // 
            this.rbtnTBT.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnTBT.BackColor = System.Drawing.Color.Transparent;
            this.rbtnTBT.BackgroundImage = global::AgOpenGPS.Properties.Resources.ToolChkTBT;
            this.rbtnTBT.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnTBT.FlatAppearance.BorderSize = 0;
            this.rbtnTBT.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rbtnTBT.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnTBT.Location = new System.Drawing.Point(348, 48);
            this.rbtnTBT.Name = "rbtnTBT";
            this.rbtnTBT.Size = new System.Drawing.Size(241, 100);
            this.rbtnTBT.TabIndex = 112;
            this.rbtnTBT.TabStop = true;
            this.rbtnTBT.UseVisualStyleBackColor = false;
            this.rbtnTBT.CheckedChanged += new System.EventHandler(this.RbtnFront_CheckedChanged);
            // 
            // rbtnFixedRear
            // 
            this.rbtnFixedRear.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnFixedRear.BackColor = System.Drawing.Color.Transparent;
            this.rbtnFixedRear.BackgroundImage = global::AgOpenGPS.Properties.Resources.ToolChkRear;
            this.rbtnFixedRear.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnFixedRear.FlatAppearance.BorderSize = 0;
            this.rbtnFixedRear.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rbtnFixedRear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnFixedRear.Location = new System.Drawing.Point(16, 48);
            this.rbtnFixedRear.Name = "rbtnFixedRear";
            this.rbtnFixedRear.Size = new System.Drawing.Size(201, 100);
            this.rbtnFixedRear.TabIndex = 111;
            this.rbtnFixedRear.TabStop = true;
            this.rbtnFixedRear.UseVisualStyleBackColor = false;
            this.rbtnFixedRear.CheckedChanged += new System.EventHandler(this.RbtnFront_CheckedChanged);
            // 
            // rbtnFront
            // 
            this.rbtnFront.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnFront.BackColor = System.Drawing.Color.Transparent;
            this.rbtnFront.BackgroundImage = global::AgOpenGPS.Properties.Resources.ToolChkFront;
            this.rbtnFront.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnFront.FlatAppearance.BorderSize = 0;
            this.rbtnFront.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rbtnFront.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnFront.Location = new System.Drawing.Point(16, 212);
            this.rbtnFront.Name = "rbtnFront";
            this.rbtnFront.Size = new System.Drawing.Size(201, 100);
            this.rbtnFront.TabIndex = 110;
            this.rbtnFront.TabStop = true;
            this.rbtnFront.UseVisualStyleBackColor = false;
            this.rbtnFront.CheckedChanged += new System.EventHandler(this.RbtnFront_CheckedChanged);
            // 
            // rbtnTrailing
            // 
            this.rbtnTrailing.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnTrailing.BackColor = System.Drawing.Color.Transparent;
            this.rbtnTrailing.BackgroundImage = global::AgOpenGPS.Properties.Resources.ToolChkTrailing;
            this.rbtnTrailing.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnTrailing.FlatAppearance.BorderSize = 0;
            this.rbtnTrailing.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rbtnTrailing.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnTrailing.Location = new System.Drawing.Point(348, 212);
            this.rbtnTrailing.Name = "rbtnTrailing";
            this.rbtnTrailing.Size = new System.Drawing.Size(241, 100);
            this.rbtnTrailing.TabIndex = 109;
            this.rbtnTrailing.TabStop = true;
            this.rbtnTrailing.UseVisualStyleBackColor = false;
            this.rbtnTrailing.CheckedChanged += new System.EventHandler(this.RbtnFront_CheckedChanged);
            // 
            // tabHitch
            // 
            this.tabHitch.BackgroundImage = global::AgOpenGPS.Properties.Resources.ToolHitchPageTBT;
            this.tabHitch.Controls.Add(this.btnNext);
            this.tabHitch.Controls.Add(this.nudForeAft);
            this.tabHitch.Controls.Add(this.nudHitchLength);
            this.tabHitch.Controls.Add(this.nudTankHitch);
            this.tabHitch.Location = new System.Drawing.Point(4, 73);
            this.tabHitch.Name = "tabHitch";
            this.tabHitch.Size = new System.Drawing.Size(972, 523);
            this.tabHitch.TabIndex = 11;
            this.tabHitch.Text = "Hitch";
            this.tabHitch.UseVisualStyleBackColor = true;
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Location = new System.Drawing.Point(12, 19);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(104, 49);
            this.btnNext.TabIndex = 2;
            this.btnNext.UseVisualStyleBackColor = false;
            // 
            // nudForeAft
            // 
            this.nudForeAft.BackColor = System.Drawing.Color.AliceBlue;
            this.nudForeAft.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudForeAft.InterceptArrowKeys = false;
            this.nudForeAft.Location = new System.Drawing.Point(700, 112);
            this.nudForeAft.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudForeAft.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.nudForeAft.Name = "nudForeAft";
            this.nudForeAft.Size = new System.Drawing.Size(150, 65);
            this.nudForeAft.TabIndex = 102;
            this.nudForeAft.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudForeAft.Value = new decimal(new int[] {
            302,
            0,
            0,
            0});
            this.nudForeAft.ValueChanged += new System.EventHandler(this.NudForeAft_ValueChanged);
            this.nudForeAft.Enter += new System.EventHandler(this.NudForeAft_Enter);
            // 
            // nudHitchLength
            // 
            this.nudHitchLength.BackColor = System.Drawing.Color.AliceBlue;
            this.nudHitchLength.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudHitchLength.InterceptArrowKeys = false;
            this.nudHitchLength.Location = new System.Drawing.Point(283, 112);
            this.nudHitchLength.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.nudHitchLength.Name = "nudHitchLength";
            this.nudHitchLength.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.nudHitchLength.Size = new System.Drawing.Size(150, 65);
            this.nudHitchLength.TabIndex = 3;
            this.nudHitchLength.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudHitchLength.Value = new decimal(new int[] {
            51,
            0,
            0,
            0});
            this.nudHitchLength.ValueChanged += new System.EventHandler(this.NudHitchLength_ValueChanged);
            this.nudHitchLength.Enter += new System.EventHandler(this.NudHitchLength_Enter);
            // 
            // nudTankHitch
            // 
            this.nudTankHitch.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTankHitch.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTankHitch.InterceptArrowKeys = false;
            this.nudTankHitch.Location = new System.Drawing.Point(486, 112);
            this.nudTankHitch.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.nudTankHitch.Name = "nudTankHitch";
            this.nudTankHitch.Size = new System.Drawing.Size(150, 65);
            this.nudTankHitch.TabIndex = 114;
            this.nudTankHitch.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudTankHitch.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            this.nudTankHitch.ValueChanged += new System.EventHandler(this.NudTankHitch_ValueChanged);
            this.nudTankHitch.Enter += new System.EventHandler(this.NudTankHitch_Enter);
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.SystemColors.Window;
            this.tabSettings.BackgroundImage = global::AgOpenGPS.Properties.Resources.ImplementSettings;
            this.tabSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.tabSettings.Controls.Add(this.nudMappingOffDelay);
            this.tabSettings.Controls.Add(this.label12);
            this.tabSettings.Controls.Add(this.label9);
            this.tabSettings.Controls.Add(this.nudMappingOnDelay);
            this.tabSettings.Controls.Add(this.label8);
            this.tabSettings.Controls.Add(this.nudLookAheadOff);
            this.tabSettings.Controls.Add(this.nudCutoffSpeed);
            this.tabSettings.Controls.Add(this.label30);
            this.tabSettings.Controls.Add(this.lblTurnOffBelowUnits);
            this.tabSettings.Controls.Add(this.nudOffset);
            this.tabSettings.Controls.Add(this.label23);
            this.tabSettings.Controls.Add(this.nudTurnOffDelay);
            this.tabSettings.Controls.Add(this.label3);
            this.tabSettings.Controls.Add(this.nudLookAhead);
            this.tabSettings.Controls.Add(this.nudOverlap);
            this.tabSettings.Location = new System.Drawing.Point(4, 73);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Size = new System.Drawing.Size(972, 523);
            this.tabSettings.TabIndex = 9;
            this.tabSettings.Text = "Settings";
            // 
            // nudMappingOffDelay
            // 
            this.nudMappingOffDelay.BackColor = System.Drawing.Color.AliceBlue;
            this.nudMappingOffDelay.DecimalPlaces = 1;
            this.nudMappingOffDelay.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMappingOffDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudMappingOffDelay.InterceptArrowKeys = false;
            this.nudMappingOffDelay.Location = new System.Drawing.Point(70, 366);
            this.nudMappingOffDelay.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudMappingOffDelay.Name = "nudMappingOffDelay";
            this.nudMappingOffDelay.Size = new System.Drawing.Size(150, 65);
            this.nudMappingOffDelay.TabIndex = 119;
            this.nudMappingOffDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMappingOffDelay.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudMappingOffDelay.ValueChanged += new System.EventHandler(this.MappingOffDelay_ValueChanged);
            this.nudMappingOffDelay.Enter += new System.EventHandler(this.MappingOffDelay_Enter);
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(5, 328);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(280, 35);
            this.label12.TabIndex = 118;
            this.label12.Text = "Stop Mapping Delay (Secs)";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(5, 116);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(280, 35);
            this.label9.TabIndex = 116;
            this.label9.Text = "Start Mapping Delay (Secs)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudMappingOnDelay
            // 
            this.nudMappingOnDelay.BackColor = System.Drawing.Color.AliceBlue;
            this.nudMappingOnDelay.DecimalPlaces = 1;
            this.nudMappingOnDelay.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMappingOnDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudMappingOnDelay.InterceptArrowKeys = false;
            this.nudMappingOnDelay.Location = new System.Drawing.Point(70, 154);
            this.nudMappingOnDelay.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudMappingOnDelay.Name = "nudMappingOnDelay";
            this.nudMappingOnDelay.Size = new System.Drawing.Size(150, 65);
            this.nudMappingOnDelay.TabIndex = 115;
            this.nudMappingOnDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMappingOnDelay.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudMappingOnDelay.ValueChanged += new System.EventHandler(this.MappingOnDelay_ValueChanged);
            this.nudMappingOnDelay.Enter += new System.EventHandler(this.MappingOnDelay_Enter);
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(5, 222);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(280, 35);
            this.label8.TabIndex = 114;
            this.label8.Text = "Turn Off Ahead (Secs)";
            this.label8.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudLookAheadOff
            // 
            this.nudLookAheadOff.BackColor = System.Drawing.Color.AliceBlue;
            this.nudLookAheadOff.DecimalPlaces = 1;
            this.nudLookAheadOff.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLookAheadOff.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudLookAheadOff.InterceptArrowKeys = false;
            this.nudLookAheadOff.Location = new System.Drawing.Point(70, 260);
            this.nudLookAheadOff.Maximum = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.nudLookAheadOff.Name = "nudLookAheadOff";
            this.nudLookAheadOff.Size = new System.Drawing.Size(150, 65);
            this.nudLookAheadOff.TabIndex = 113;
            this.nudLookAheadOff.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLookAheadOff.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.nudLookAheadOff.ValueChanged += new System.EventHandler(this.NudLookAheadOff_ValueChanged);
            this.nudLookAheadOff.Enter += new System.EventHandler(this.NudLookAheadOff_Enter);
            // 
            // nudCutoffSpeed
            // 
            this.nudCutoffSpeed.BackColor = System.Drawing.Color.AliceBlue;
            this.nudCutoffSpeed.DecimalPlaces = 1;
            this.nudCutoffSpeed.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudCutoffSpeed.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudCutoffSpeed.InterceptArrowKeys = false;
            this.nudCutoffSpeed.Location = new System.Drawing.Point(725, 421);
            this.nudCutoffSpeed.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.nudCutoffSpeed.Name = "nudCutoffSpeed";
            this.nudCutoffSpeed.Size = new System.Drawing.Size(152, 52);
            this.nudCutoffSpeed.TabIndex = 110;
            this.nudCutoffSpeed.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudCutoffSpeed.Value = new decimal(new int[] {
            11,
            0,
            0,
            65536});
            this.nudCutoffSpeed.ValueChanged += new System.EventHandler(this.NudCutoffSpeed_ValueChanged);
            this.nudCutoffSpeed.Enter += new System.EventHandler(this.NudCutoffSpeed_Enter);
            // 
            // label30
            // 
            this.label30.BackColor = System.Drawing.SystemColors.Window;
            this.label30.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label30.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label30.Location = new System.Drawing.Point(728, 357);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(171, 61);
            this.label30.TabIndex = 111;
            this.label30.Text = "Off below";
            this.label30.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // lblTurnOffBelowUnits
            // 
            this.lblTurnOffBelowUnits.AutoSize = true;
            this.lblTurnOffBelowUnits.BackColor = System.Drawing.SystemColors.Window;
            this.lblTurnOffBelowUnits.Font = new System.Drawing.Font("Tahoma", 20.25F);
            this.lblTurnOffBelowUnits.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblTurnOffBelowUnits.Location = new System.Drawing.Point(883, 433);
            this.lblTurnOffBelowUnits.Name = "lblTurnOffBelowUnits";
            this.lblTurnOffBelowUnits.Size = new System.Drawing.Size(82, 33);
            this.lblTurnOffBelowUnits.TabIndex = 112;
            this.lblTurnOffBelowUnits.Text = "Km/H";
            // 
            // nudOffset
            // 
            this.nudOffset.BackColor = System.Drawing.Color.AliceBlue;
            this.nudOffset.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudOffset.InterceptArrowKeys = false;
            this.nudOffset.Location = new System.Drawing.Point(500, 409);
            this.nudOffset.Maximum = new decimal(new int[] {
            2500,
            0,
            0,
            0});
            this.nudOffset.Minimum = new decimal(new int[] {
            2500,
            0,
            0,
            -2147483648});
            this.nudOffset.Name = "nudOffset";
            this.nudOffset.Size = new System.Drawing.Size(150, 65);
            this.nudOffset.TabIndex = 99;
            this.nudOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudOffset.Value = new decimal(new int[] {
            8,
            0,
            0,
            -2147483648});
            this.nudOffset.ValueChanged += new System.EventHandler(this.NudOffset_ValueChanged);
            this.nudOffset.Enter += new System.EventHandler(this.NudOffset_Enter);
            // 
            // label23
            // 
            this.label23.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label23.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label23.Location = new System.Drawing.Point(5, 434);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(280, 35);
            this.label23.TabIndex = 109;
            this.label23.Text = "Turn Off Delay (Secs)";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudTurnOffDelay
            // 
            this.nudTurnOffDelay.BackColor = System.Drawing.Color.AliceBlue;
            this.nudTurnOffDelay.DecimalPlaces = 1;
            this.nudTurnOffDelay.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudTurnOffDelay.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudTurnOffDelay.InterceptArrowKeys = false;
            this.nudTurnOffDelay.Location = new System.Drawing.Point(70, 472);
            this.nudTurnOffDelay.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudTurnOffDelay.Name = "nudTurnOffDelay";
            this.nudTurnOffDelay.Size = new System.Drawing.Size(150, 65);
            this.nudTurnOffDelay.TabIndex = 108;
            this.nudTurnOffDelay.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudTurnOffDelay.ValueChanged += new System.EventHandler(this.NudTurnOffDelay_ValueChanged);
            this.nudTurnOffDelay.Enter += new System.EventHandler(this.NudTurnOffDelay_Enter);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(5, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(280, 35);
            this.label3.TabIndex = 107;
            this.label3.Text = "Turn On Ahead (Secs)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudLookAhead
            // 
            this.nudLookAhead.BackColor = System.Drawing.Color.AliceBlue;
            this.nudLookAhead.DecimalPlaces = 1;
            this.nudLookAhead.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudLookAhead.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudLookAhead.InterceptArrowKeys = false;
            this.nudLookAhead.Location = new System.Drawing.Point(70, 48);
            this.nudLookAhead.Maximum = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudLookAhead.Minimum = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            this.nudLookAhead.Name = "nudLookAhead";
            this.nudLookAhead.Size = new System.Drawing.Size(150, 65);
            this.nudLookAhead.TabIndex = 106;
            this.nudLookAhead.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudLookAhead.Value = new decimal(new int[] {
            10,
            0,
            0,
            65536});
            this.nudLookAhead.ValueChanged += new System.EventHandler(this.NudLookAhead_ValueChanged);
            this.nudLookAhead.Enter += new System.EventHandler(this.NudLookAhead_Enter);
            // 
            // nudOverlap
            // 
            this.nudOverlap.BackColor = System.Drawing.Color.AliceBlue;
            this.nudOverlap.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudOverlap.InterceptArrowKeys = false;
            this.nudOverlap.Location = new System.Drawing.Point(735, 225);
            this.nudOverlap.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.nudOverlap.Minimum = new decimal(new int[] {
            300,
            0,
            0,
            -2147483648});
            this.nudOverlap.Name = "nudOverlap";
            this.nudOverlap.Size = new System.Drawing.Size(150, 65);
            this.nudOverlap.TabIndex = 101;
            this.nudOverlap.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudOverlap.Value = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.nudOverlap.ValueChanged += new System.EventHandler(this.NudOverlap_ValueChanged);
            this.nudOverlap.Enter += new System.EventHandler(this.NudOverlap_Enter);
            // 
            // tabSections
            // 
            this.tabSections.AutoScroll = true;
            this.tabSections.BackColor = System.Drawing.SystemColors.Window;
            this.tabSections.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabSections.Controls.Add(this.label2);
            this.tabSections.Controls.Add(this.SectionPanel);
            this.tabSections.Controls.Add(this.NumSections);
            this.tabSections.Controls.Add(this.label4);
            this.tabSections.Controls.Add(this.nudDefaultSectionWidth);
            this.tabSections.Controls.Add(this.lblVehicleToolWidth);
            this.tabSections.Controls.Add(this.label41);
            this.tabSections.Controls.Add(this.nudMinApplied);
            this.tabSections.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabSections.Location = new System.Drawing.Point(4, 73);
            this.tabSections.Name = "tabSections";
            this.tabSections.Size = new System.Drawing.Size(972, 523);
            this.tabSections.TabIndex = 2;
            this.tabSections.Text = " Sections ";
            // 
            // SectionPanel
            // 
            this.SectionPanel.AutoScroll = true;
            this.SectionPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.SectionPanel.Location = new System.Drawing.Point(0, 0);
            this.SectionPanel.Name = "SectionPanel";
            this.SectionPanel.Size = new System.Drawing.Size(970, 416);
            this.SectionPanel.TabIndex = 260;
            // 
            // NumSections
            // 
            this.NumSections.BackColor = System.Drawing.Color.AliceBlue;
            this.NumSections.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NumSections.InterceptArrowKeys = false;
            this.NumSections.Location = new System.Drawing.Point(51, 460);
            this.NumSections.Name = "NumSections";
            this.NumSections.Size = new System.Drawing.Size(120, 52);
            this.NumSections.TabIndex = 259;
            this.NumSections.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NumSections.Value = new decimal(new int[] {
            92,
            0,
            0,
            0});
            this.NumSections.ValueChanged += new System.EventHandler(this.NumSections_SelectedIndexChanged);
            this.NumSections.Enter += new System.EventHandler(this.NudSections_Enter);
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.SystemColors.Window;
            this.label4.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(300, 419);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(221, 34);
            this.label4.TabIndex = 250;
            this.label4.Text = "Section Width";
            this.label4.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudDefaultSectionWidth
            // 
            this.nudDefaultSectionWidth.BackColor = System.Drawing.Color.AliceBlue;
            this.nudDefaultSectionWidth.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudDefaultSectionWidth.InterceptArrowKeys = false;
            this.nudDefaultSectionWidth.Location = new System.Drawing.Point(348, 460);
            this.nudDefaultSectionWidth.Maximum = new decimal(new int[] {
            7500,
            0,
            0,
            0});
            this.nudDefaultSectionWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudDefaultSectionWidth.Name = "nudDefaultSectionWidth";
            this.nudDefaultSectionWidth.Size = new System.Drawing.Size(120, 52);
            this.nudDefaultSectionWidth.TabIndex = 249;
            this.nudDefaultSectionWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudDefaultSectionWidth.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.nudDefaultSectionWidth.Enter += new System.EventHandler(this.NudDefaultSectionWidth_Enter);
            // 
            // lblVehicleToolWidth
            // 
            this.lblVehicleToolWidth.AutoSize = true;
            this.lblVehicleToolWidth.Font = new System.Drawing.Font("Tahoma", 24F);
            this.lblVehicleToolWidth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblVehicleToolWidth.Location = new System.Drawing.Point(490, 467);
            this.lblVehicleToolWidth.Name = "lblVehicleToolWidth";
            this.lblVehicleToolWidth.Size = new System.Drawing.Size(41, 39);
            this.lblVehicleToolWidth.TabIndex = 49;
            this.lblVehicleToolWidth.Text = "II";
            // 
            // label41
            // 
            this.label41.BackColor = System.Drawing.SystemColors.Window;
            this.label41.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label41.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label41.Location = new System.Drawing.Point(634, 419);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(244, 34);
            this.label41.TabIndex = 48;
            this.label41.Text = "Min UnApplied";
            this.label41.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // nudMinApplied
            // 
            this.nudMinApplied.BackColor = System.Drawing.Color.AliceBlue;
            this.nudMinApplied.Font = new System.Drawing.Font("Tahoma", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.nudMinApplied.InterceptArrowKeys = false;
            this.nudMinApplied.Location = new System.Drawing.Point(694, 460);
            this.nudMinApplied.Name = "nudMinApplied";
            this.nudMinApplied.Size = new System.Drawing.Size(125, 52);
            this.nudMinApplied.TabIndex = 47;
            this.nudMinApplied.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudMinApplied.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.nudMinApplied.ValueChanged += new System.EventHandler(this.NudMinApplied_ValueChanged);
            this.nudMinApplied.Enter += new System.EventHandler(this.NudMinApplied_Enter);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.SystemColors.Window;
            this.label2.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(3, 419);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(220, 34);
            this.label2.TabIndex = 1;
            this.label2.Text = "# Of Sections";
            this.label2.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // tabWorkSwitch
            // 
            this.tabWorkSwitch.BackColor = System.Drawing.SystemColors.Window;
            this.tabWorkSwitch.BackgroundImage = global::AgOpenGPS.Properties.Resources.WorkSwitch;
            this.tabWorkSwitch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabWorkSwitch.Controls.Add(this.groupBox3);
            this.tabWorkSwitch.Location = new System.Drawing.Point(4, 73);
            this.tabWorkSwitch.Name = "tabWorkSwitch";
            this.tabWorkSwitch.Size = new System.Drawing.Size(972, 523);
            this.tabWorkSwitch.TabIndex = 10;
            this.tabWorkSwitch.Text = " Switches ";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Window;
            this.groupBox3.Controls.Add(this.checkWorkSwitchManual);
            this.groupBox3.Controls.Add(this.chkWorkSwActiveLow);
            this.groupBox3.Controls.Add(this.chkEnableWorkSwitch);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(140, 33);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(636, 261);
            this.groupBox3.TabIndex = 66;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Work Switch";
            // 
            // checkWorkSwitchManual
            // 
            this.checkWorkSwitchManual.AutoSize = true;
            this.checkWorkSwitchManual.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkWorkSwitchManual.Location = new System.Drawing.Point(46, 199);
            this.checkWorkSwitchManual.Name = "checkWorkSwitchManual";
            this.checkWorkSwitchManual.Size = new System.Drawing.Size(380, 37);
            this.checkWorkSwitchManual.TabIndex = 0;
            this.checkWorkSwitchManual.Text = "Work Switch Controls Manual";
            this.checkWorkSwitchManual.UseVisualStyleBackColor = true;
            this.checkWorkSwitchManual.CheckedChanged += new System.EventHandler(this.CheckWorkSwitchManual_CheckedChanged);
            // 
            // chkWorkSwActiveLow
            // 
            this.chkWorkSwActiveLow.AutoSize = true;
            this.chkWorkSwActiveLow.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkWorkSwActiveLow.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkWorkSwActiveLow.Location = new System.Drawing.Point(46, 124);
            this.chkWorkSwActiveLow.Name = "chkWorkSwActiveLow";
            this.chkWorkSwActiveLow.Size = new System.Drawing.Size(160, 37);
            this.chkWorkSwActiveLow.TabIndex = 0;
            this.chkWorkSwActiveLow.Text = "Active Low";
            this.chkWorkSwActiveLow.UseVisualStyleBackColor = true;
            this.chkWorkSwActiveLow.CheckedChanged += new System.EventHandler(this.ChkWorkSwActiveLow_CheckedChanged);
            // 
            // chkEnableWorkSwitch
            // 
            this.chkEnableWorkSwitch.AutoSize = true;
            this.chkEnableWorkSwitch.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableWorkSwitch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.chkEnableWorkSwitch.Location = new System.Drawing.Point(46, 49);
            this.chkEnableWorkSwitch.Name = "chkEnableWorkSwitch";
            this.chkEnableWorkSwitch.Size = new System.Drawing.Size(268, 37);
            this.chkEnableWorkSwitch.TabIndex = 1;
            this.chkEnableWorkSwitch.Text = "Enable Work Switch";
            this.chkEnableWorkSwitch.UseVisualStyleBackColor = true;
            this.chkEnableWorkSwitch.CheckedChanged += new System.EventHandler(this.ChkEnableWorkSwitch_CheckedChanged);
            // 
            // lblDoNotExceed
            // 
            this.lblDoNotExceed.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.lblDoNotExceed.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblDoNotExceed.Location = new System.Drawing.Point(252, 623);
            this.lblDoNotExceed.Name = "lblDoNotExceed";
            this.lblDoNotExceed.Size = new System.Drawing.Size(391, 25);
            this.lblDoNotExceed.TabIndex = 24;
            this.lblDoNotExceed.Text = "* Do not exceed 1570 *";
            this.lblDoNotExceed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSecTotalWidthInches
            // 
            this.lblSecTotalWidthInches.AutoSize = true;
            this.lblSecTotalWidthInches.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold);
            this.lblSecTotalWidthInches.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSecTotalWidthInches.Location = new System.Drawing.Point(537, 648);
            this.lblSecTotalWidthInches.Name = "lblSecTotalWidthInches";
            this.lblSecTotalWidthInches.Size = new System.Drawing.Size(43, 35);
            this.lblSecTotalWidthInches.TabIndex = 25;
            this.lblSecTotalWidthInches.Text = "II";
            // 
            // lblSecTotalWidthFeet
            // 
            this.lblSecTotalWidthFeet.AutoSize = true;
            this.lblSecTotalWidthFeet.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold);
            this.lblSecTotalWidthFeet.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSecTotalWidthFeet.Location = new System.Drawing.Point(456, 648);
            this.lblSecTotalWidthFeet.Name = "lblSecTotalWidthFeet";
            this.lblSecTotalWidthFeet.Size = new System.Drawing.Size(49, 35);
            this.lblSecTotalWidthFeet.TabIndex = 24;
            this.lblSecTotalWidthFeet.Text = "FF";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(12, 625);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(153, 23);
            this.label17.TabIndex = 89;
            this.label17.Text = "Measurements in";
            // 
            // label16
            // 
            this.label16.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(272, 653);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(180, 25);
            this.label16.TabIndex = 107;
            this.label16.Text = "Tool Width:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSecTotalWidthMeters
            // 
            this.lblSecTotalWidthMeters.AutoSize = true;
            this.lblSecTotalWidthMeters.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold);
            this.lblSecTotalWidthMeters.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSecTotalWidthMeters.Location = new System.Drawing.Point(495, 648);
            this.lblSecTotalWidthMeters.Name = "lblSecTotalWidthMeters";
            this.lblSecTotalWidthMeters.Size = new System.Drawing.Size(43, 35);
            this.lblSecTotalWidthMeters.TabIndex = 108;
            this.lblSecTotalWidthMeters.Text = "II";
            // 
            // lblInchesCm
            // 
            this.lblInchesCm.AutoSize = true;
            this.lblInchesCm.Font = new System.Drawing.Font("Tahoma", 24F);
            this.lblInchesCm.ForeColor = System.Drawing.Color.Red;
            this.lblInchesCm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblInchesCm.Location = new System.Drawing.Point(25, 653);
            this.lblInchesCm.Name = "lblInchesCm";
            this.lblInchesCm.Size = new System.Drawing.Size(111, 39);
            this.lblInchesCm.TabIndex = 109;
            this.lblInchesCm.Text = "Inches";
            // 
            // btnCancel
            // 
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(685, 624);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 68);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // bntOK
            // 
            this.bntOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bntOK.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.bntOK.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.bntOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bntOK.Location = new System.Drawing.Point(818, 624);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(156, 68);
            this.bntOK.TabIndex = 0;
            this.bntOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // FormToolSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(980, 700);
            this.ControlBox = false;
            this.Controls.Add(this.lblInchesCm);
            this.Controls.Add(this.lblSecTotalWidthMeters);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.bntOK);
            this.Controls.Add(this.lblSecTotalWidthFeet);
            this.Controls.Add(this.lblSecTotalWidthInches);
            this.Controls.Add(this.lblDoNotExceed);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormToolSettings";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FormToolSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabConfig.ResumeLayout(false);
            this.gboxAttachment.ResumeLayout(false);
            this.tabHitch.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudForeAft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudHitchLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTankHitch)).EndInit();
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudMappingOffDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMappingOnDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLookAheadOff)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudCutoffSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOffset)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudTurnOffDelay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudLookAhead)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudOverlap)).EndInit();
            this.tabSections.ResumeLayout(false);
            this.tabSections.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NumSections)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDefaultSectionWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudMinApplied)).EndInit();
            this.tabWorkSwitch.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.TabPage tabSections;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblSecTotalWidthInches;
        private System.Windows.Forms.Label lblSecTotalWidthFeet;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.NumericUpDown nudForeAft;
        private System.Windows.Forms.NumericUpDown nudOverlap;
        private System.Windows.Forms.NumericUpDown nudOffset;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.NumericUpDown nudTurnOffDelay;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudLookAhead;
        private System.Windows.Forms.NumericUpDown nudHitchLength;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label lblSecTotalWidthMeters;
        private System.Windows.Forms.TabPage tabWorkSwitch;
        private System.Windows.Forms.CheckBox chkEnableWorkSwitch;
        private System.Windows.Forms.CheckBox chkWorkSwActiveLow;
        private System.Windows.Forms.CheckBox checkWorkSwitchManual;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label lblDoNotExceed;
        private System.Windows.Forms.Label lblInchesCm;
        private System.Windows.Forms.NumericUpDown nudTankHitch;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.NumericUpDown nudMinApplied;
        private System.Windows.Forms.Label lblVehicleToolWidth;
        private System.Windows.Forms.GroupBox gboxAttachment;
        private System.Windows.Forms.RadioButton rbtnFixedRear;
        private System.Windows.Forms.RadioButton rbtnFront;
        private System.Windows.Forms.RadioButton rbtnTrailing;
        private System.Windows.Forms.RadioButton rbtnTBT;
        private System.Windows.Forms.Button btnChangeAttachment;
        private System.Windows.Forms.TabPage tabHitch;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudDefaultSectionWidth;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.NumericUpDown nudCutoffSpeed;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label lblTurnOffBelowUnits;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown nudLookAheadOff;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown nudMappingOnDelay;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown nudMappingOffDelay;
        private System.Windows.Forms.NumericUpDown NumSections;
        private System.Windows.Forms.Panel SectionPanel;
    }
}