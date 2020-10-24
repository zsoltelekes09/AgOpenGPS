namespace AgOpenGPS
{
    partial class FormSettings
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
            this.rbtnHarvester = new System.Windows.Forms.RadioButton();
            this.rbtn4WD = new System.Windows.Forms.RadioButton();
            this.btnChangeAttachment = new System.Windows.Forms.Button();
            this.rbtnTractor = new System.Windows.Forms.RadioButton();
            this.tabSettings = new System.Windows.Forms.TabPage();
            this.TboxWheelbase = new System.Windows.Forms.TextBox();
            this.TboxAntennaOffset = new System.Windows.Forms.TextBox();
            this.TboxAntennaHeight = new System.Windows.Forms.TextBox();
            this.TboxAntennaPivot = new System.Windows.Forms.TextBox();
            this.btnNext = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.tabVehicle = new System.Windows.Forms.TabPage();
            this.TboxMinTurnRadius = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.TboxHydLiftSecs = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabGuidance = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.TboxLineWidth = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TboxLightbarCmPerPixel = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.TboxSnapDistance = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.lblInchesCm = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.bntOK = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabConfig.SuspendLayout();
            this.gboxAttachment.SuspendLayout();
            this.tabSettings.SuspendLayout();
            this.tabVehicle.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tabGuidance.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabConfig);
            this.tabControl1.Controls.Add(this.tabSettings);
            this.tabControl1.Controls.Add(this.tabVehicle);
            this.tabControl1.Controls.Add(this.tabGuidance);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(200, 69);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(978, 618);
            this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
            this.tabControl1.TabIndex = 2;
            // 
            // tabConfig
            // 
            this.tabConfig.BackColor = System.Drawing.SystemColors.Control;
            this.tabConfig.Controls.Add(this.gboxAttachment);
            this.tabConfig.Location = new System.Drawing.Point(4, 73);
            this.tabConfig.Name = "tabConfig";
            this.tabConfig.Padding = new System.Windows.Forms.Padding(3);
            this.tabConfig.Size = new System.Drawing.Size(970, 541);
            this.tabConfig.TabIndex = 14;
            this.tabConfig.Text = "Type";
            // 
            // gboxAttachment
            // 
            this.gboxAttachment.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.gboxAttachment.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.gboxAttachment.Controls.Add(this.rbtnHarvester);
            this.gboxAttachment.Controls.Add(this.rbtn4WD);
            this.gboxAttachment.Controls.Add(this.btnChangeAttachment);
            this.gboxAttachment.Controls.Add(this.rbtnTractor);
            this.gboxAttachment.Location = new System.Drawing.Point(35, 38);
            this.gboxAttachment.Name = "gboxAttachment";
            this.gboxAttachment.Size = new System.Drawing.Size(871, 483);
            this.gboxAttachment.TabIndex = 111;
            this.gboxAttachment.TabStop = false;
            this.gboxAttachment.Text = "Vehicle Type";
            // 
            // rbtnHarvester
            // 
            this.rbtnHarvester.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnHarvester.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnHarvester.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbtnHarvester.FlatAppearance.BorderSize = 0;
            this.rbtnHarvester.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rbtnHarvester.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnHarvester.Image = global::AgOpenGPS.Properties.Resources.vehiclePageHarvester;
            this.rbtnHarvester.Location = new System.Drawing.Point(49, 278);
            this.rbtnHarvester.Name = "rbtnHarvester";
            this.rbtnHarvester.Size = new System.Drawing.Size(321, 183);
            this.rbtnHarvester.TabIndex = 253;
            this.rbtnHarvester.TabStop = true;
            this.rbtnHarvester.UseVisualStyleBackColor = true;
            this.rbtnHarvester.CheckedChanged += new System.EventHandler(this.Rbtn4WD_CheckedChanged);
            // 
            // rbtn4WD
            // 
            this.rbtn4WD.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtn4WD.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtn4WD.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbtn4WD.FlatAppearance.BorderSize = 0;
            this.rbtn4WD.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rbtn4WD.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtn4WD.Image = global::AgOpenGPS.Properties.Resources.vehiclePage4WD;
            this.rbtn4WD.Location = new System.Drawing.Point(49, 53);
            this.rbtn4WD.Name = "rbtn4WD";
            this.rbtn4WD.Size = new System.Drawing.Size(321, 183);
            this.rbtn4WD.TabIndex = 252;
            this.rbtn4WD.TabStop = true;
            this.rbtn4WD.UseVisualStyleBackColor = true;
            this.rbtn4WD.CheckedChanged += new System.EventHandler(this.Rbtn4WD_CheckedChanged);
            // 
            // btnChangeAttachment
            // 
            this.btnChangeAttachment.BackColor = System.Drawing.Color.Transparent;
            this.btnChangeAttachment.Enabled = false;
            this.btnChangeAttachment.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChangeAttachment.Image = global::AgOpenGPS.Properties.Resources.ToolAcceptChange;
            this.btnChangeAttachment.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnChangeAttachment.Location = new System.Drawing.Point(611, 316);
            this.btnChangeAttachment.Name = "btnChangeAttachment";
            this.btnChangeAttachment.Size = new System.Drawing.Size(170, 106);
            this.btnChangeAttachment.TabIndex = 251;
            this.btnChangeAttachment.UseVisualStyleBackColor = false;
            this.btnChangeAttachment.Click += new System.EventHandler(this.BtnChangeAttachment_Click);
            // 
            // rbtnTractor
            // 
            this.rbtnTractor.Appearance = System.Windows.Forms.Appearance.Button;
            this.rbtnTractor.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.rbtnTractor.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.rbtnTractor.FlatAppearance.BorderSize = 0;
            this.rbtnTractor.FlatAppearance.CheckedBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.rbtnTractor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rbtnTractor.Image = global::AgOpenGPS.Properties.Resources.vehiclePageTractor;
            this.rbtnTractor.Location = new System.Drawing.Point(460, 53);
            this.rbtnTractor.Name = "rbtnTractor";
            this.rbtnTractor.Size = new System.Drawing.Size(321, 183);
            this.rbtnTractor.TabIndex = 112;
            this.rbtnTractor.TabStop = true;
            this.rbtnTractor.UseVisualStyleBackColor = true;
            this.rbtnTractor.CheckedChanged += new System.EventHandler(this.Rbtn4WD_CheckedChanged);
            // 
            // tabSettings
            // 
            this.tabSettings.BackColor = System.Drawing.SystemColors.Window;
            this.tabSettings.BackgroundImage = global::AgOpenGPS.Properties.Resources.VehicleSettingsTractor;
            this.tabSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabSettings.Controls.Add(this.TboxWheelbase);
            this.tabSettings.Controls.Add(this.TboxAntennaOffset);
            this.tabSettings.Controls.Add(this.TboxAntennaHeight);
            this.tabSettings.Controls.Add(this.TboxAntennaPivot);
            this.tabSettings.Controls.Add(this.btnNext);
            this.tabSettings.Controls.Add(this.label9);
            this.tabSettings.Controls.Add(this.label26);
            this.tabSettings.Controls.Add(this.label15);
            this.tabSettings.Controls.Add(this.label7);
            this.tabSettings.Controls.Add(this.label18);
            this.tabSettings.Location = new System.Drawing.Point(4, 73);
            this.tabSettings.Name = "tabSettings";
            this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
            this.tabSettings.Size = new System.Drawing.Size(970, 541);
            this.tabSettings.TabIndex = 11;
            this.tabSettings.Text = "Settings";
            // 
            // TboxWheelbase
            // 
            this.TboxWheelbase.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxWheelbase.BackColor = System.Drawing.SystemColors.Control;
            this.TboxWheelbase.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxWheelbase.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxWheelbase.Location = new System.Drawing.Point(266, 312);
            this.TboxWheelbase.Name = "TboxWheelbase";
            this.TboxWheelbase.ShortcutsEnabled = false;
            this.TboxWheelbase.Size = new System.Drawing.Size(150, 70);
            this.TboxWheelbase.TabIndex = 474;
            this.TboxWheelbase.Text = "501";
            this.TboxWheelbase.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxWheelbase.WordWrap = false;
            this.TboxWheelbase.Enter += new System.EventHandler(this.TboxWheelbase_Enter);
            // 
            // TboxAntennaOffset
            // 
            this.TboxAntennaOffset.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxAntennaOffset.BackColor = System.Drawing.SystemColors.Control;
            this.TboxAntennaOffset.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxAntennaOffset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxAntennaOffset.Location = new System.Drawing.Point(710, 319);
            this.TboxAntennaOffset.Name = "TboxAntennaOffset";
            this.TboxAntennaOffset.ShortcutsEnabled = false;
            this.TboxAntennaOffset.Size = new System.Drawing.Size(150, 70);
            this.TboxAntennaOffset.TabIndex = 473;
            this.TboxAntennaOffset.Text = "0";
            this.TboxAntennaOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxAntennaOffset.WordWrap = false;
            this.TboxAntennaOffset.Enter += new System.EventHandler(this.TboxAntennaOffset_Enter);
            // 
            // TboxAntennaHeight
            // 
            this.TboxAntennaHeight.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxAntennaHeight.BackColor = System.Drawing.SystemColors.Control;
            this.TboxAntennaHeight.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxAntennaHeight.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxAntennaHeight.Location = new System.Drawing.Point(710, 180);
            this.TboxAntennaHeight.Name = "TboxAntennaHeight";
            this.TboxAntennaHeight.ShortcutsEnabled = false;
            this.TboxAntennaHeight.Size = new System.Drawing.Size(150, 70);
            this.TboxAntennaHeight.TabIndex = 472;
            this.TboxAntennaHeight.Text = "300";
            this.TboxAntennaHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxAntennaHeight.WordWrap = false;
            this.TboxAntennaHeight.Enter += new System.EventHandler(this.TboxAntennaHeight_Enter);
            // 
            // TboxAntennaPivot
            // 
            this.TboxAntennaPivot.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxAntennaPivot.BackColor = System.Drawing.SystemColors.Control;
            this.TboxAntennaPivot.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxAntennaPivot.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxAntennaPivot.Location = new System.Drawing.Point(292, 22);
            this.TboxAntennaPivot.Name = "TboxAntennaPivot";
            this.TboxAntennaPivot.ShortcutsEnabled = false;
            this.TboxAntennaPivot.Size = new System.Drawing.Size(150, 70);
            this.TboxAntennaPivot.TabIndex = 471;
            this.TboxAntennaPivot.Text = "5.0";
            this.TboxAntennaPivot.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxAntennaPivot.WordWrap = false;
            this.TboxAntennaPivot.Enter += new System.EventHandler(this.TboxAntennaPivot_Enter);
            // 
            // btnNext
            // 
            this.btnNext.BackColor = System.Drawing.Color.Transparent;
            this.btnNext.FlatAppearance.BorderSize = 0;
            this.btnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNext.Location = new System.Drawing.Point(863, 5);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(104, 49);
            this.btnNext.TabIndex = 0;
            this.btnNext.UseVisualStyleBackColor = false;
            // 
            // label9
            // 
            this.label9.BackColor = System.Drawing.SystemColors.Window;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.label9.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label9.Location = new System.Drawing.Point(670, 392);
            this.label9.Name = "label9";
            this.label9.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.label9.Size = new System.Drawing.Size(216, 23);
            this.label9.TabIndex = 28;
            this.label9.Text = "*Left is negative";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label26
            // 
            this.label26.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label26.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label26.Location = new System.Drawing.Point(266, 258);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(134, 51);
            this.label26.TabIndex = 13;
            this.label26.Text = "Wheelbase";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.SystemColors.Window;
            this.label15.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(670, 154);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(216, 23);
            this.label15.TabIndex = 21;
            this.label15.Text = "Height";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.BackColor = System.Drawing.SystemColors.Window;
            this.label7.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(670, 293);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(216, 23);
            this.label7.TabIndex = 27;
            this.label7.Text = "Offset";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label18
            // 
            this.label18.BackColor = System.Drawing.SystemColors.Window;
            this.label18.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label18.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label18.Location = new System.Drawing.Point(442, 31);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(149, 56);
            this.label18.TabIndex = 8;
            this.label18.Text = "Distance";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabVehicle
            // 
            this.tabVehicle.BackColor = System.Drawing.SystemColors.Window;
            this.tabVehicle.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.tabVehicle.Controls.Add(this.TboxMinTurnRadius);
            this.tabVehicle.Controls.Add(this.groupBox3);
            this.tabVehicle.Controls.Add(this.pictureBox1);
            this.tabVehicle.Controls.Add(this.label6);
            this.tabVehicle.Location = new System.Drawing.Point(4, 73);
            this.tabVehicle.Margin = new System.Windows.Forms.Padding(4);
            this.tabVehicle.Name = "tabVehicle";
            this.tabVehicle.Padding = new System.Windows.Forms.Padding(4);
            this.tabVehicle.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.tabVehicle.Size = new System.Drawing.Size(970, 541);
            this.tabVehicle.TabIndex = 1;
            this.tabVehicle.Text = "  Vehicle ";
            // 
            // TboxMinTurnRadius
            // 
            this.TboxMinTurnRadius.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxMinTurnRadius.BackColor = System.Drawing.SystemColors.Control;
            this.TboxMinTurnRadius.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxMinTurnRadius.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxMinTurnRadius.Location = new System.Drawing.Point(96, 255);
            this.TboxMinTurnRadius.Name = "TboxMinTurnRadius";
            this.TboxMinTurnRadius.ShortcutsEnabled = false;
            this.TboxMinTurnRadius.Size = new System.Drawing.Size(150, 70);
            this.TboxMinTurnRadius.TabIndex = 471;
            this.TboxMinTurnRadius.Text = "301";
            this.TboxMinTurnRadius.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxMinTurnRadius.WordWrap = false;
            this.TboxMinTurnRadius.Enter += new System.EventHandler(this.TboxMinTurnRadius_Enter);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.TboxHydLiftSecs);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(393, 36);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(289, 172);
            this.groupBox3.TabIndex = 125;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Hydraulic Lift Look Ahead (secs)";
            // 
            // TboxHydLiftSecs
            // 
            this.TboxHydLiftSecs.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxHydLiftSecs.BackColor = System.Drawing.SystemColors.Control;
            this.TboxHydLiftSecs.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxHydLiftSecs.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxHydLiftSecs.Location = new System.Drawing.Point(50, 63);
            this.TboxHydLiftSecs.Name = "TboxHydLiftSecs";
            this.TboxHydLiftSecs.ShortcutsEnabled = false;
            this.TboxHydLiftSecs.Size = new System.Drawing.Size(150, 70);
            this.TboxHydLiftSecs.TabIndex = 470;
            this.TboxHydLiftSecs.Text = "5.0";
            this.TboxHydLiftSecs.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxHydLiftSecs.WordWrap = false;
            this.TboxHydLiftSecs.Enter += new System.EventHandler(this.TboxHydLiftSecs_Enter);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AgOpenGPS.Properties.Resources.tire;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(64, 36);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(220, 164);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(96, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(155, 47);
            this.label6.TabIndex = 25;
            this.label6.Text = "Turn Radius";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tabGuidance
            // 
            this.tabGuidance.BackColor = System.Drawing.SystemColors.Window;
            this.tabGuidance.Controls.Add(this.groupBox4);
            this.tabGuidance.Controls.Add(this.groupBox2);
            this.tabGuidance.Controls.Add(this.groupBox1);
            this.tabGuidance.Location = new System.Drawing.Point(4, 73);
            this.tabGuidance.Name = "tabGuidance";
            this.tabGuidance.Padding = new System.Windows.Forms.Padding(3);
            this.tabGuidance.Size = new System.Drawing.Size(970, 541);
            this.tabGuidance.TabIndex = 13;
            this.tabGuidance.Text = " Guidance ";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.TboxLineWidth);
            this.groupBox4.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox4.Location = new System.Drawing.Point(714, 59);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(232, 172);
            this.groupBox4.TabIndex = 124;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Guidance Line Width";
            // 
            // TboxLineWidth
            // 
            this.TboxLineWidth.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxLineWidth.BackColor = System.Drawing.SystemColors.Control;
            this.TboxLineWidth.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxLineWidth.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxLineWidth.Location = new System.Drawing.Point(44, 76);
            this.TboxLineWidth.Name = "TboxLineWidth";
            this.TboxLineWidth.ShortcutsEnabled = false;
            this.TboxLineWidth.Size = new System.Drawing.Size(150, 70);
            this.TboxLineWidth.TabIndex = 470;
            this.TboxLineWidth.Text = "1";
            this.TboxLineWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxLineWidth.WordWrap = false;
            this.TboxLineWidth.Enter += new System.EventHandler(this.TboxLineWidth_Enter);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TboxLightbarCmPerPixel);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(394, 59);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 172);
            this.groupBox2.TabIndex = 123;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "cm / Lightbar Pixel";
            // 
            // TboxLightbarCmPerPixel
            // 
            this.TboxLightbarCmPerPixel.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxLightbarCmPerPixel.BackColor = System.Drawing.SystemColors.Control;
            this.TboxLightbarCmPerPixel.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxLightbarCmPerPixel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxLightbarCmPerPixel.Location = new System.Drawing.Point(26, 73);
            this.TboxLightbarCmPerPixel.Name = "TboxLightbarCmPerPixel";
            this.TboxLightbarCmPerPixel.ShortcutsEnabled = false;
            this.TboxLightbarCmPerPixel.Size = new System.Drawing.Size(150, 70);
            this.TboxLightbarCmPerPixel.TabIndex = 469;
            this.TboxLightbarCmPerPixel.Text = "5";
            this.TboxLightbarCmPerPixel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxLightbarCmPerPixel.WordWrap = false;
            this.TboxLightbarCmPerPixel.Enter += new System.EventHandler(this.TboxLightbarCmPerPixel_Enter);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.TboxSnapDistance);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.groupBox1.Location = new System.Drawing.Point(34, 63);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(279, 168);
            this.groupBox1.TabIndex = 90;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "<< >> Snap Distance";
            // 
            // TboxSnapDistance
            // 
            this.TboxSnapDistance.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxSnapDistance.BackColor = System.Drawing.SystemColors.Control;
            this.TboxSnapDistance.Font = new System.Drawing.Font("Tahoma", 39F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxSnapDistance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxSnapDistance.Location = new System.Drawing.Point(23, 73);
            this.TboxSnapDistance.Name = "TboxSnapDistance";
            this.TboxSnapDistance.ShortcutsEnabled = false;
            this.TboxSnapDistance.Size = new System.Drawing.Size(150, 70);
            this.TboxSnapDistance.TabIndex = 470;
            this.TboxSnapDistance.Text = "499";
            this.TboxSnapDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxSnapDistance.WordWrap = false;
            this.TboxSnapDistance.Enter += new System.EventHandler(this.TboxSnapDistance_Enter);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(23, 622);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(153, 23);
            this.label17.TabIndex = 89;
            this.label17.Text = "Measurements in";
            // 
            // lblInchesCm
            // 
            this.lblInchesCm.AutoSize = true;
            this.lblInchesCm.Font = new System.Drawing.Font("Tahoma", 24F);
            this.lblInchesCm.ForeColor = System.Drawing.Color.Red;
            this.lblInchesCm.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblInchesCm.Location = new System.Drawing.Point(65, 653);
            this.lblInchesCm.Name = "lblInchesCm";
            this.lblInchesCm.Size = new System.Drawing.Size(111, 39);
            this.lblInchesCm.TabIndex = 109;
            this.lblInchesCm.Text = "Inches";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel.Location = new System.Drawing.Point(687, 626);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 68);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // bntOK
            // 
            this.bntOK.BackColor = System.Drawing.Color.Transparent;
            this.bntOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bntOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntOK.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.bntOK.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.bntOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bntOK.Location = new System.Drawing.Point(820, 626);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(156, 68);
            this.bntOK.TabIndex = 0;
            this.bntOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bntOK.UseVisualStyleBackColor = false;
            this.bntOK.Click += new System.EventHandler(this.BtnOK_Click);
            // 
            // FormSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(987, 700);
            this.ControlBox = false;
            this.Controls.Add(this.lblInchesCm);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.bntOK);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSettings";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.FormSettings_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabConfig.ResumeLayout(false);
            this.gboxAttachment.ResumeLayout(false);
            this.tabSettings.ResumeLayout(false);
            this.tabSettings.PerformLayout();
            this.tabVehicle.ResumeLayout(false);
            this.tabVehicle.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tabGuidance.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabVehicle;
        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label lblInchesCm;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TabPage tabSettings;
        private System.Windows.Forms.TabPage tabGuidance;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabPage tabConfig;
        private System.Windows.Forms.GroupBox gboxAttachment;
        private System.Windows.Forms.Button btnChangeAttachment;
        private System.Windows.Forms.RadioButton rbtnTractor;
        private System.Windows.Forms.RadioButton rbtnHarvester;
        private System.Windows.Forms.RadioButton rbtn4WD;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox TboxLightbarCmPerPixel;
        private System.Windows.Forms.TextBox TboxLineWidth;
        private System.Windows.Forms.TextBox TboxSnapDistance;
        private System.Windows.Forms.TextBox TboxHydLiftSecs;
        private System.Windows.Forms.TextBox TboxMinTurnRadius;
        private System.Windows.Forms.TextBox TboxAntennaPivot;
        private System.Windows.Forms.TextBox TboxAntennaHeight;
        private System.Windows.Forms.TextBox TboxAntennaOffset;
        private System.Windows.Forms.TextBox TboxWheelbase;
    }
}