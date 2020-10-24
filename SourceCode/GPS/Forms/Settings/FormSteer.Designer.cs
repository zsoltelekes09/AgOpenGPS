namespace AgOpenGPS
{
    partial class FormSteer
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabGain = new System.Windows.Forms.TabPage();
            this.label27 = new System.Windows.Forms.Label();
            this.lblPWMDisplay = new System.Windows.Forms.Label();
            this.hsbarHighSteerPWM = new System.Windows.Forms.HScrollBar();
            this.hsbarMinPWM = new System.Windows.Forms.HScrollBar();
            this.hsbarProportionalGain = new System.Windows.Forms.HScrollBar();
            this.label45 = new System.Windows.Forms.Label();
            this.label41 = new System.Windows.Forms.Label();
            this.hsbarLowSteerPWM = new System.Windows.Forms.HScrollBar();
            this.label7 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.lblMinPWM = new System.Windows.Forms.Label();
            this.lblHighSteerPWM = new System.Windows.Forms.Label();
            this.lblLowSteerPWM = new System.Windows.Forms.Label();
            this.lblProportionalGain = new System.Windows.Forms.Label();
            this.tabSteer = new System.Windows.Forms.TabPage();
            this.hsbarSidehillDraftGain = new System.Windows.Forms.HScrollBar();
            this.label25 = new System.Windows.Forms.Label();
            this.hsbarCountsPerDegree = new System.Windows.Forms.HScrollBar();
            this.label29 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.hsbarMaxSteerAngle = new System.Windows.Forms.HScrollBar();
            this.label10 = new System.Windows.Forms.Label();
            this.hsbarSteerAngleSensorZero = new System.Windows.Forms.HScrollBar();
            this.lblCountsPerDegree = new System.Windows.Forms.Label();
            this.lblMaxSteerAngle = new System.Windows.Forms.Label();
            this.lblSidehillDraftGain = new System.Windows.Forms.Label();
            this.lblSteerAngleSensorZero = new System.Windows.Forms.Label();
            this.tabLook = new System.Windows.Forms.TabPage();
            this.hsbarLookAheadUturnMult = new System.Windows.Forms.HScrollBar();
            this.label2 = new System.Windows.Forms.Label();
            this.hsbarLookAheadMin = new System.Windows.Forms.HScrollBar();
            this.hsbarDistanceFromLine = new System.Windows.Forms.HScrollBar();
            this.hsbarLookAhead = new System.Windows.Forms.HScrollBar();
            this.label37 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.lblLookAheadUturnMult = new System.Windows.Forms.Label();
            this.lblLookAhead = new System.Windows.Forms.Label();
            this.lblLookAheadMinimum = new System.Windows.Forms.Label();
            this.lblDistanceFromLine = new System.Windows.Forms.Label();
            this.tabStan = new System.Windows.Forms.TabPage();
            this.btnStanley = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.hsbarHeadingErrorGain = new System.Windows.Forms.HScrollBar();
            this.btnChart = new System.Windows.Forms.Button();
            this.lblStanleyGain = new System.Windows.Forms.Label();
            this.hsbarStanleyGain = new System.Windows.Forms.HScrollBar();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblHeadingErrorGain = new System.Windows.Forms.Label();
            this.tabInt = new System.Windows.Forms.TabPage();
            this.TboxIntHeading = new System.Windows.Forms.TextBox();
            this.TboxIntDistance = new System.Windows.Forms.TextBox();
            this.label24 = new System.Windows.Forms.Label();
            this.lblDistanceAway = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label20 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.lblAvgXTE = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.hsbarAvgXTE = new System.Windows.Forms.HScrollBar();
            this.label17 = new System.Windows.Forms.Label();
            this.lblStanleyIntegralGain = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.hsbarIntegralGain = new System.Windows.Forms.HScrollBar();
            this.label21 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.pbErrNeg = new System.Windows.Forms.ProgressBar();
            this.BtnFreeDrive = new System.Windows.Forms.Button();
            this.BtnPlus20 = new System.Windows.Forms.Button();
            this.pbErrPos = new System.Windows.Forms.ProgressBar();
            this.pbSetNeg = new System.Windows.Forms.ProgressBar();
            this.pbActNeg = new System.Windows.Forms.ProgressBar();
            this.pbActPos = new System.Windows.Forms.ProgressBar();
            this.pbSetPos = new System.Windows.Forms.ProgressBar();
            this.BtnMinus20 = new System.Windows.Forms.Button();
            this.BtnSteerAngleDown = new ProXoft.WinForms.RepeatButton();
            this.BtnSteerAngleUp = new ProXoft.WinForms.RepeatButton();
            this.BtnFreeDriveZero = new System.Windows.Forms.Button();
            this.lblError = new System.Windows.Forms.Label();
            this.lblSteerAngle = new System.Windows.Forms.Label();
            this.lblSteerAngleActual = new System.Windows.Forms.Label();
            this.lblSent = new System.Windows.Forms.Label();
            this.lblRecd = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabGain.SuspendLayout();
            this.tabSteer.SuspendLayout();
            this.tabLook.SuspendLayout();
            this.tabStan.SuspendLayout();
            this.tabInt.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Appearance = System.Windows.Forms.TabAppearance.Buttons;
            this.tabControl1.Controls.Add(this.tabGain);
            this.tabControl1.Controls.Add(this.tabSteer);
            this.tabControl1.Controls.Add(this.tabLook);
            this.tabControl1.Controls.Add(this.tabStan);
            this.tabControl1.Controls.Add(this.tabInt);
            this.tabControl1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControl1.ItemSize = new System.Drawing.Size(92, 44);
            this.tabControl1.Location = new System.Drawing.Point(2, 4);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(526, 319);
            this.tabControl1.TabIndex = 232;
            // 
            // tabGain
            // 
            this.tabGain.AutoScroll = true;
            this.tabGain.BackColor = System.Drawing.Color.PowderBlue;
            this.tabGain.Controls.Add(this.label27);
            this.tabGain.Controls.Add(this.lblPWMDisplay);
            this.tabGain.Controls.Add(this.hsbarHighSteerPWM);
            this.tabGain.Controls.Add(this.hsbarMinPWM);
            this.tabGain.Controls.Add(this.hsbarProportionalGain);
            this.tabGain.Controls.Add(this.label45);
            this.tabGain.Controls.Add(this.label41);
            this.tabGain.Controls.Add(this.hsbarLowSteerPWM);
            this.tabGain.Controls.Add(this.label7);
            this.tabGain.Controls.Add(this.label33);
            this.tabGain.Controls.Add(this.lblMinPWM);
            this.tabGain.Controls.Add(this.lblHighSteerPWM);
            this.tabGain.Controls.Add(this.lblLowSteerPWM);
            this.tabGain.Controls.Add(this.lblProportionalGain);
            this.tabGain.Location = new System.Drawing.Point(4, 48);
            this.tabGain.Name = "tabGain";
            this.tabGain.Size = new System.Drawing.Size(518, 267);
            this.tabGain.TabIndex = 13;
            this.tabGain.Text = "Gain";
            // 
            // label27
            // 
            this.label27.BackColor = System.Drawing.Color.Transparent;
            this.label27.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label27.Location = new System.Drawing.Point(331, 196);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(64, 23);
            this.label27.TabIndex = 320;
            this.label27.Text = "PWM:";
            this.label27.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblPWMDisplay
            // 
            this.lblPWMDisplay.BackColor = System.Drawing.Color.Transparent;
            this.lblPWMDisplay.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPWMDisplay.Location = new System.Drawing.Point(396, 190);
            this.lblPWMDisplay.Name = "lblPWMDisplay";
            this.lblPWMDisplay.Size = new System.Drawing.Size(89, 32);
            this.lblPWMDisplay.TabIndex = 319;
            this.lblPWMDisplay.Text = "255";
            this.lblPWMDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // hsbarHighSteerPWM
            // 
            this.hsbarHighSteerPWM.LargeChange = 2;
            this.hsbarHighSteerPWM.Location = new System.Drawing.Point(12, 163);
            this.hsbarHighSteerPWM.Maximum = 255;
            this.hsbarHighSteerPWM.Minimum = 1;
            this.hsbarHighSteerPWM.Name = "hsbarHighSteerPWM";
            this.hsbarHighSteerPWM.Size = new System.Drawing.Size(260, 30);
            this.hsbarHighSteerPWM.TabIndex = 274;
            this.hsbarHighSteerPWM.Value = 50;
            this.hsbarHighSteerPWM.ValueChanged += new System.EventHandler(this.HsbarHighSteerPWM_ValueChanged);
            // 
            // hsbarMinPWM
            // 
            this.hsbarMinPWM.LargeChange = 1;
            this.hsbarMinPWM.Location = new System.Drawing.Point(14, 97);
            this.hsbarMinPWM.Name = "hsbarMinPWM";
            this.hsbarMinPWM.Size = new System.Drawing.Size(260, 30);
            this.hsbarMinPWM.TabIndex = 284;
            this.hsbarMinPWM.Value = 10;
            this.hsbarMinPWM.ValueChanged += new System.EventHandler(this.HsbarMinPWM_ValueChanged);
            // 
            // hsbarProportionalGain
            // 
            this.hsbarProportionalGain.LargeChange = 1;
            this.hsbarProportionalGain.Location = new System.Drawing.Point(12, 32);
            this.hsbarProportionalGain.Maximum = 200;
            this.hsbarProportionalGain.Name = "hsbarProportionalGain";
            this.hsbarProportionalGain.Size = new System.Drawing.Size(260, 30);
            this.hsbarProportionalGain.TabIndex = 254;
            this.hsbarProportionalGain.Value = 4;
            this.hsbarProportionalGain.ValueChanged += new System.EventHandler(this.HsbarProportionalGain_ValueChanged);
            // 
            // label45
            // 
            this.label45.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label45.Location = new System.Drawing.Point(13, 139);
            this.label45.Name = "label45";
            this.label45.Size = new System.Drawing.Size(251, 23);
            this.label45.TabIndex = 275;
            this.label45.Text = "High Max PWM";
            this.label45.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label41
            // 
            this.label41.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label41.Location = new System.Drawing.Point(17, 71);
            this.label41.Name = "label41";
            this.label41.Size = new System.Drawing.Size(247, 23);
            this.label41.TabIndex = 285;
            this.label41.Text = "Minimum PWM Drive";
            this.label41.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarLowSteerPWM
            // 
            this.hsbarLowSteerPWM.LargeChange = 1;
            this.hsbarLowSteerPWM.Location = new System.Drawing.Point(14, 229);
            this.hsbarLowSteerPWM.Maximum = 255;
            this.hsbarLowSteerPWM.Minimum = 1;
            this.hsbarLowSteerPWM.Name = "hsbarLowSteerPWM";
            this.hsbarLowSteerPWM.Size = new System.Drawing.Size(260, 30);
            this.hsbarLowSteerPWM.TabIndex = 269;
            this.hsbarLowSteerPWM.Value = 1;
            this.hsbarLowSteerPWM.ValueChanged += new System.EventHandler(this.HsbarLowSteerPWM_ValueChanged);
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(14, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(249, 23);
            this.label7.TabIndex = 255;
            this.label7.Text = "Proportional Gain";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            this.label33.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.Location = new System.Drawing.Point(12, 205);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(254, 23);
            this.label33.TabIndex = 270;
            this.label33.Text = "Low Max PWM";
            this.label33.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblMinPWM
            // 
            this.lblMinPWM.AutoSize = true;
            this.lblMinPWM.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinPWM.Location = new System.Drawing.Point(279, 91);
            this.lblMinPWM.Name = "lblMinPWM";
            this.lblMinPWM.Size = new System.Drawing.Size(91, 39);
            this.lblMinPWM.TabIndex = 288;
            this.lblMinPWM.Text = "-888";
            // 
            // lblHighSteerPWM
            // 
            this.lblHighSteerPWM.AutoSize = true;
            this.lblHighSteerPWM.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHighSteerPWM.Location = new System.Drawing.Point(279, 157);
            this.lblHighSteerPWM.Name = "lblHighSteerPWM";
            this.lblHighSteerPWM.Size = new System.Drawing.Size(91, 39);
            this.lblHighSteerPWM.TabIndex = 278;
            this.lblHighSteerPWM.Text = "-888";
            // 
            // lblLowSteerPWM
            // 
            this.lblLowSteerPWM.AutoSize = true;
            this.lblLowSteerPWM.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLowSteerPWM.Location = new System.Drawing.Point(279, 223);
            this.lblLowSteerPWM.Name = "lblLowSteerPWM";
            this.lblLowSteerPWM.Size = new System.Drawing.Size(91, 39);
            this.lblLowSteerPWM.TabIndex = 273;
            this.lblLowSteerPWM.Text = "-888";
            // 
            // lblProportionalGain
            // 
            this.lblProportionalGain.AutoSize = true;
            this.lblProportionalGain.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProportionalGain.Location = new System.Drawing.Point(279, 25);
            this.lblProportionalGain.Name = "lblProportionalGain";
            this.lblProportionalGain.Size = new System.Drawing.Size(91, 39);
            this.lblProportionalGain.TabIndex = 258;
            this.lblProportionalGain.Text = "-888";
            // 
            // tabSteer
            // 
            this.tabSteer.AutoScroll = true;
            this.tabSteer.BackColor = System.Drawing.Color.PowderBlue;
            this.tabSteer.Controls.Add(this.hsbarSidehillDraftGain);
            this.tabSteer.Controls.Add(this.label25);
            this.tabSteer.Controls.Add(this.hsbarCountsPerDegree);
            this.tabSteer.Controls.Add(this.label29);
            this.tabSteer.Controls.Add(this.label19);
            this.tabSteer.Controls.Add(this.hsbarMaxSteerAngle);
            this.tabSteer.Controls.Add(this.label10);
            this.tabSteer.Controls.Add(this.hsbarSteerAngleSensorZero);
            this.tabSteer.Controls.Add(this.lblCountsPerDegree);
            this.tabSteer.Controls.Add(this.lblMaxSteerAngle);
            this.tabSteer.Controls.Add(this.lblSidehillDraftGain);
            this.tabSteer.Controls.Add(this.lblSteerAngleSensorZero);
            this.tabSteer.Location = new System.Drawing.Point(4, 48);
            this.tabSteer.Name = "tabSteer";
            this.tabSteer.Size = new System.Drawing.Size(518, 267);
            this.tabSteer.TabIndex = 5;
            this.tabSteer.Text = "Steer";
            // 
            // hsbarSidehillDraftGain
            // 
            this.hsbarSidehillDraftGain.LargeChange = 1;
            this.hsbarSidehillDraftGain.Location = new System.Drawing.Point(10, 231);
            this.hsbarSidehillDraftGain.Maximum = 24;
            this.hsbarSidehillDraftGain.Name = "hsbarSidehillDraftGain";
            this.hsbarSidehillDraftGain.Size = new System.Drawing.Size(260, 30);
            this.hsbarSidehillDraftGain.TabIndex = 264;
            this.hsbarSidehillDraftGain.Value = 2;
            this.hsbarSidehillDraftGain.ValueChanged += new System.EventHandler(this.HsbarSidehillDraftGain_ValueChanged);
            // 
            // label25
            // 
            this.label25.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label25.Location = new System.Drawing.Point(10, 71);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(253, 23);
            this.label25.TabIndex = 305;
            this.label25.Text = "Counts per Degree";
            this.label25.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarCountsPerDegree
            // 
            this.hsbarCountsPerDegree.LargeChange = 1;
            this.hsbarCountsPerDegree.Location = new System.Drawing.Point(10, 96);
            this.hsbarCountsPerDegree.Maximum = 255;
            this.hsbarCountsPerDegree.Minimum = 1;
            this.hsbarCountsPerDegree.Name = "hsbarCountsPerDegree";
            this.hsbarCountsPerDegree.Size = new System.Drawing.Size(260, 30);
            this.hsbarCountsPerDegree.TabIndex = 304;
            this.hsbarCountsPerDegree.Value = 20;
            this.hsbarCountsPerDegree.ValueChanged += new System.EventHandler(this.HsbarCountsPerDegree_ValueChanged);
            // 
            // label29
            // 
            this.label29.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label29.Location = new System.Drawing.Point(12, 206);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(253, 23);
            this.label29.TabIndex = 265;
            this.label29.Text = "Sidehill Draft Gain";
            this.label29.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label19
            // 
            this.label19.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label19.Location = new System.Drawing.Point(10, 139);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(253, 23);
            this.label19.TabIndex = 300;
            this.label19.Text = "Max Steer Angle in Degrees";
            this.label19.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarMaxSteerAngle
            // 
            this.hsbarMaxSteerAngle.LargeChange = 1;
            this.hsbarMaxSteerAngle.Location = new System.Drawing.Point(10, 164);
            this.hsbarMaxSteerAngle.Maximum = 80;
            this.hsbarMaxSteerAngle.Minimum = 10;
            this.hsbarMaxSteerAngle.Name = "hsbarMaxSteerAngle";
            this.hsbarMaxSteerAngle.Size = new System.Drawing.Size(260, 30);
            this.hsbarMaxSteerAngle.TabIndex = 299;
            this.hsbarMaxSteerAngle.Value = 10;
            this.hsbarMaxSteerAngle.ValueChanged += new System.EventHandler(this.HsbarMaxSteerAngle_ValueChanged);
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(10, 5);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(253, 23);
            this.label10.TabIndex = 295;
            this.label10.Text = "Wheel Angle Sensor Zero";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarSteerAngleSensorZero
            // 
            this.hsbarSteerAngleSensorZero.LargeChange = 1;
            this.hsbarSteerAngleSensorZero.Location = new System.Drawing.Point(10, 30);
            this.hsbarSteerAngleSensorZero.Maximum = 127;
            this.hsbarSteerAngleSensorZero.Minimum = -127;
            this.hsbarSteerAngleSensorZero.Name = "hsbarSteerAngleSensorZero";
            this.hsbarSteerAngleSensorZero.Size = new System.Drawing.Size(260, 30);
            this.hsbarSteerAngleSensorZero.TabIndex = 294;
            this.hsbarSteerAngleSensorZero.ValueChanged += new System.EventHandler(this.HsbarSteerAngleSensorZero_ValueChanged);
            // 
            // lblCountsPerDegree
            // 
            this.lblCountsPerDegree.AutoSize = true;
            this.lblCountsPerDegree.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountsPerDegree.Location = new System.Drawing.Point(277, 90);
            this.lblCountsPerDegree.Name = "lblCountsPerDegree";
            this.lblCountsPerDegree.Size = new System.Drawing.Size(91, 39);
            this.lblCountsPerDegree.TabIndex = 308;
            this.lblCountsPerDegree.Text = "-888";
            // 
            // lblMaxSteerAngle
            // 
            this.lblMaxSteerAngle.AutoSize = true;
            this.lblMaxSteerAngle.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMaxSteerAngle.Location = new System.Drawing.Point(277, 157);
            this.lblMaxSteerAngle.Name = "lblMaxSteerAngle";
            this.lblMaxSteerAngle.Size = new System.Drawing.Size(91, 39);
            this.lblMaxSteerAngle.TabIndex = 303;
            this.lblMaxSteerAngle.Text = "-888";
            // 
            // lblSidehillDraftGain
            // 
            this.lblSidehillDraftGain.AutoSize = true;
            this.lblSidehillDraftGain.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSidehillDraftGain.Location = new System.Drawing.Point(279, 224);
            this.lblSidehillDraftGain.Name = "lblSidehillDraftGain";
            this.lblSidehillDraftGain.Size = new System.Drawing.Size(91, 39);
            this.lblSidehillDraftGain.TabIndex = 268;
            this.lblSidehillDraftGain.Text = "-888";
            // 
            // lblSteerAngleSensorZero
            // 
            this.lblSteerAngleSensorZero.AutoSize = true;
            this.lblSteerAngleSensorZero.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngleSensorZero.Location = new System.Drawing.Point(277, 23);
            this.lblSteerAngleSensorZero.Name = "lblSteerAngleSensorZero";
            this.lblSteerAngleSensorZero.Size = new System.Drawing.Size(91, 39);
            this.lblSteerAngleSensorZero.TabIndex = 298;
            this.lblSteerAngleSensorZero.Text = "-888";
            // 
            // tabLook
            // 
            this.tabLook.BackColor = System.Drawing.Color.PowderBlue;
            this.tabLook.Controls.Add(this.hsbarLookAheadUturnMult);
            this.tabLook.Controls.Add(this.label2);
            this.tabLook.Controls.Add(this.hsbarLookAheadMin);
            this.tabLook.Controls.Add(this.hsbarDistanceFromLine);
            this.tabLook.Controls.Add(this.hsbarLookAhead);
            this.tabLook.Controls.Add(this.label37);
            this.tabLook.Controls.Add(this.label4);
            this.tabLook.Controls.Add(this.label6);
            this.tabLook.Controls.Add(this.lblLookAheadUturnMult);
            this.tabLook.Controls.Add(this.lblLookAhead);
            this.tabLook.Controls.Add(this.lblLookAheadMinimum);
            this.tabLook.Controls.Add(this.lblDistanceFromLine);
            this.tabLook.Location = new System.Drawing.Point(4, 48);
            this.tabLook.Name = "tabLook";
            this.tabLook.Size = new System.Drawing.Size(518, 267);
            this.tabLook.TabIndex = 14;
            this.tabLook.Text = "Pure P";
            // 
            // hsbarLookAheadUturnMult
            // 
            this.hsbarLookAheadUturnMult.LargeChange = 1;
            this.hsbarLookAheadUturnMult.Location = new System.Drawing.Point(11, 231);
            this.hsbarLookAheadUturnMult.Maximum = 10;
            this.hsbarLookAheadUturnMult.Minimum = 1;
            this.hsbarLookAheadUturnMult.Name = "hsbarLookAheadUturnMult";
            this.hsbarLookAheadUturnMult.Size = new System.Drawing.Size(260, 30);
            this.hsbarLookAheadUturnMult.TabIndex = 298;
            this.hsbarLookAheadUturnMult.Value = 4;
            this.hsbarLookAheadUturnMult.ValueChanged += new System.EventHandler(this.HsbarLookAheadUturnMult_ValueChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(18, 202);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(257, 25);
            this.label2.TabIndex = 297;
            this.label2.Text = "UTurn Look Ahead (Multiplier)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarLookAheadMin
            // 
            this.hsbarLookAheadMin.LargeChange = 1;
            this.hsbarLookAheadMin.Location = new System.Drawing.Point(11, 164);
            this.hsbarLookAheadMin.Maximum = 50;
            this.hsbarLookAheadMin.Minimum = 2;
            this.hsbarLookAheadMin.Name = "hsbarLookAheadMin";
            this.hsbarLookAheadMin.Size = new System.Drawing.Size(260, 30);
            this.hsbarLookAheadMin.TabIndex = 272;
            this.hsbarLookAheadMin.Value = 10;
            this.hsbarLookAheadMin.ValueChanged += new System.EventHandler(this.HsbarLookAheadMin_ValueChanged);
            // 
            // hsbarDistanceFromLine
            // 
            this.hsbarDistanceFromLine.LargeChange = 1;
            this.hsbarDistanceFromLine.Location = new System.Drawing.Point(11, 98);
            this.hsbarDistanceFromLine.Maximum = 30;
            this.hsbarDistanceFromLine.Name = "hsbarDistanceFromLine";
            this.hsbarDistanceFromLine.Size = new System.Drawing.Size(260, 30);
            this.hsbarDistanceFromLine.TabIndex = 269;
            this.hsbarDistanceFromLine.Value = 10;
            this.hsbarDistanceFromLine.ValueChanged += new System.EventHandler(this.HsbarDistanceFromLine_ValueChanged);
            // 
            // hsbarLookAhead
            // 
            this.hsbarLookAhead.LargeChange = 1;
            this.hsbarLookAhead.Location = new System.Drawing.Point(11, 33);
            this.hsbarLookAhead.Maximum = 60;
            this.hsbarLookAhead.Minimum = 5;
            this.hsbarLookAhead.Name = "hsbarLookAhead";
            this.hsbarLookAhead.Size = new System.Drawing.Size(260, 30);
            this.hsbarLookAhead.TabIndex = 289;
            this.hsbarLookAhead.Value = 25;
            this.hsbarLookAhead.ValueChanged += new System.EventHandler(this.HsbarLookAhead_ValueChanged);
            // 
            // label37
            // 
            this.label37.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label37.Location = new System.Drawing.Point(18, 5);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(257, 25);
            this.label37.TabIndex = 290;
            this.label37.Text = "Look Ahead (Seconds)";
            this.label37.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(18, 71);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(257, 25);
            this.label4.TabIndex = 273;
            this.label4.Text = "Look Ahead Offline Multiplier";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(18, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(257, 25);
            this.label6.TabIndex = 270;
            this.label6.Text = "Min Look Ahead (Meters)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLookAheadUturnMult
            // 
            this.lblLookAheadUturnMult.AutoSize = true;
            this.lblLookAheadUturnMult.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLookAheadUturnMult.Location = new System.Drawing.Point(277, 223);
            this.lblLookAheadUturnMult.Name = "lblLookAheadUturnMult";
            this.lblLookAheadUturnMult.Size = new System.Drawing.Size(91, 39);
            this.lblLookAheadUturnMult.TabIndex = 299;
            this.lblLookAheadUturnMult.Text = "-888";
            // 
            // lblLookAhead
            // 
            this.lblLookAhead.AutoSize = true;
            this.lblLookAhead.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLookAhead.Location = new System.Drawing.Point(277, 25);
            this.lblLookAhead.Name = "lblLookAhead";
            this.lblLookAhead.Size = new System.Drawing.Size(91, 39);
            this.lblLookAhead.TabIndex = 293;
            this.lblLookAhead.Text = "-888";
            // 
            // lblLookAheadMinimum
            // 
            this.lblLookAheadMinimum.AutoSize = true;
            this.lblLookAheadMinimum.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLookAheadMinimum.Location = new System.Drawing.Point(277, 157);
            this.lblLookAheadMinimum.Name = "lblLookAheadMinimum";
            this.lblLookAheadMinimum.Size = new System.Drawing.Size(91, 39);
            this.lblLookAheadMinimum.TabIndex = 274;
            this.lblLookAheadMinimum.Text = "-888";
            // 
            // lblDistanceFromLine
            // 
            this.lblDistanceFromLine.AutoSize = true;
            this.lblDistanceFromLine.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistanceFromLine.Location = new System.Drawing.Point(277, 91);
            this.lblDistanceFromLine.Name = "lblDistanceFromLine";
            this.lblDistanceFromLine.Size = new System.Drawing.Size(91, 39);
            this.lblDistanceFromLine.TabIndex = 271;
            this.lblDistanceFromLine.Text = "-888";
            // 
            // tabStan
            // 
            this.tabStan.BackColor = System.Drawing.Color.PowderBlue;
            this.tabStan.Controls.Add(this.btnStanley);
            this.tabStan.Controls.Add(this.label1);
            this.tabStan.Controls.Add(this.hsbarHeadingErrorGain);
            this.tabStan.Controls.Add(this.btnChart);
            this.tabStan.Controls.Add(this.lblStanleyGain);
            this.tabStan.Controls.Add(this.hsbarStanleyGain);
            this.tabStan.Controls.Add(this.label3);
            this.tabStan.Controls.Add(this.label5);
            this.tabStan.Controls.Add(this.lblHeadingErrorGain);
            this.tabStan.Location = new System.Drawing.Point(4, 48);
            this.tabStan.Name = "tabStan";
            this.tabStan.Size = new System.Drawing.Size(518, 267);
            this.tabStan.TabIndex = 15;
            this.tabStan.Text = "Stanley";
            // 
            // btnStanley
            // 
            this.btnStanley.BackColor = System.Drawing.Color.Transparent;
            this.btnStanley.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnStanley.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnStanley.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStanley.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStanley.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnStanley.Location = new System.Drawing.Point(218, 188);
            this.btnStanley.Name = "btnStanley";
            this.btnStanley.Size = new System.Drawing.Size(129, 69);
            this.btnStanley.TabIndex = 302;
            this.btnStanley.Text = "Stanley";
            this.btnStanley.UseVisualStyleBackColor = false;
            this.btnStanley.Click += new System.EventHandler(this.BtnStanley_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(183, 158);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 27);
            this.label1.TabIndex = 300;
            this.label1.Text = "Choose Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // hsbarHeadingErrorGain
            // 
            this.hsbarHeadingErrorGain.LargeChange = 1;
            this.hsbarHeadingErrorGain.Location = new System.Drawing.Point(6, 114);
            this.hsbarHeadingErrorGain.Minimum = 1;
            this.hsbarHeadingErrorGain.Name = "hsbarHeadingErrorGain";
            this.hsbarHeadingErrorGain.Size = new System.Drawing.Size(260, 30);
            this.hsbarHeadingErrorGain.TabIndex = 294;
            this.hsbarHeadingErrorGain.Value = 50;
            this.hsbarHeadingErrorGain.ValueChanged += new System.EventHandler(this.HsbarHeadingErrorGain_ValueChanged);
            // 
            // btnChart
            // 
            this.btnChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnChart.BackColor = System.Drawing.Color.Transparent;
            this.btnChart.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnChart.FlatAppearance.BorderColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.btnChart.FlatAppearance.CheckedBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.btnChart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnChart.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChart.ForeColor = System.Drawing.Color.Black;
            this.btnChart.Location = new System.Drawing.Point(17, 188);
            this.btnChart.Name = "btnChart";
            this.btnChart.Size = new System.Drawing.Size(125, 69);
            this.btnChart.TabIndex = 234;
            this.btnChart.Text = "Steer Chart";
            this.btnChart.UseVisualStyleBackColor = true;
            this.btnChart.Click += new System.EventHandler(this.BtnChart_Click);
            // 
            // lblStanleyGain
            // 
            this.lblStanleyGain.AutoSize = true;
            this.lblStanleyGain.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStanleyGain.Location = new System.Drawing.Point(280, 28);
            this.lblStanleyGain.Name = "lblStanleyGain";
            this.lblStanleyGain.Size = new System.Drawing.Size(91, 39);
            this.lblStanleyGain.TabIndex = 299;
            this.lblStanleyGain.Text = "-888";
            // 
            // hsbarStanleyGain
            // 
            this.hsbarStanleyGain.LargeChange = 1;
            this.hsbarStanleyGain.Location = new System.Drawing.Point(6, 35);
            this.hsbarStanleyGain.Maximum = 300;
            this.hsbarStanleyGain.Minimum = 1;
            this.hsbarStanleyGain.Name = "hsbarStanleyGain";
            this.hsbarStanleyGain.Size = new System.Drawing.Size(260, 30);
            this.hsbarStanleyGain.TabIndex = 297;
            this.hsbarStanleyGain.Value = 100;
            this.hsbarStanleyGain.ValueChanged += new System.EventHandler(this.HsbarStanleyGain_ValueChanged);
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(13, 7);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(241, 23);
            this.label3.TabIndex = 298;
            this.label3.Text = "Agressiveness";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(13, 86);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(241, 23);
            this.label5.TabIndex = 296;
            this.label5.Text = "Overshoot Reduction";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblHeadingErrorGain
            // 
            this.lblHeadingErrorGain.AutoSize = true;
            this.lblHeadingErrorGain.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeadingErrorGain.Location = new System.Drawing.Point(280, 107);
            this.lblHeadingErrorGain.Name = "lblHeadingErrorGain";
            this.lblHeadingErrorGain.Size = new System.Drawing.Size(91, 39);
            this.lblHeadingErrorGain.TabIndex = 295;
            this.lblHeadingErrorGain.Text = "-888";
            // 
            // tabInt
            // 
            this.tabInt.BackColor = System.Drawing.Color.PowderBlue;
            this.tabInt.Controls.Add(this.TboxIntHeading);
            this.tabInt.Controls.Add(this.TboxIntDistance);
            this.tabInt.Controls.Add(this.label24);
            this.tabInt.Controls.Add(this.lblDistanceAway);
            this.tabInt.Controls.Add(this.label15);
            this.tabInt.Controls.Add(this.label20);
            this.tabInt.Controls.Add(this.label26);
            this.tabInt.Controls.Add(this.lblAvgXTE);
            this.tabInt.Controls.Add(this.label22);
            this.tabInt.Controls.Add(this.hsbarAvgXTE);
            this.tabInt.Controls.Add(this.label17);
            this.tabInt.Controls.Add(this.lblStanleyIntegralGain);
            this.tabInt.Controls.Add(this.label18);
            this.tabInt.Controls.Add(this.hsbarIntegralGain);
            this.tabInt.Controls.Add(this.label21);
            this.tabInt.Controls.Add(this.label23);
            this.tabInt.Location = new System.Drawing.Point(4, 48);
            this.tabInt.Name = "tabInt";
            this.tabInt.Padding = new System.Windows.Forms.Padding(3);
            this.tabInt.Size = new System.Drawing.Size(518, 267);
            this.tabInt.TabIndex = 16;
            this.tabInt.Text = "Integral";
            // 
            // TboxIntHeading
            // 
            this.TboxIntHeading.BackColor = System.Drawing.SystemColors.Control;
            this.TboxIntHeading.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxIntHeading.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxIntHeading.Location = new System.Drawing.Point(100, 40);
            this.TboxIntHeading.Name = "TboxIntHeading";
            this.TboxIntHeading.Size = new System.Drawing.Size(85, 40);
            this.TboxIntHeading.TabIndex = 355;
            this.TboxIntHeading.Text = "5";
            this.TboxIntHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxIntHeading.Enter += new System.EventHandler(this.TboxIntHeading_Enter);
            // 
            // TboxIntDistance
            // 
            this.TboxIntDistance.BackColor = System.Drawing.SystemColors.Control;
            this.TboxIntDistance.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxIntDistance.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxIntDistance.Location = new System.Drawing.Point(240, 40);
            this.TboxIntDistance.Name = "TboxIntDistance";
            this.TboxIntDistance.Size = new System.Drawing.Size(85, 40);
            this.TboxIntDistance.TabIndex = 353;
            this.TboxIntDistance.Text = "15";
            this.TboxIntDistance.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxIntDistance.Enter += new System.EventHandler(this.TboxIntDistance_Enter);
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.BackColor = System.Drawing.Color.Transparent;
            this.label24.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.Location = new System.Drawing.Point(70, 106);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(45, 23);
            this.label24.TabIndex = 352;
            this.label24.Text = "Less";
            this.label24.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblDistanceAway
            // 
            this.lblDistanceAway.BackColor = System.Drawing.Color.Transparent;
            this.lblDistanceAway.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDistanceAway.Location = new System.Drawing.Point(218, 14);
            this.lblDistanceAway.Name = "lblDistanceAway";
            this.lblDistanceAway.Size = new System.Drawing.Size(134, 23);
            this.lblDistanceAway.TabIndex = 344;
            this.lblDistanceAway.Text = "Distance (cm)";
            this.lblDistanceAway.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label15
            // 
            this.label15.BackColor = System.Drawing.Color.Transparent;
            this.label15.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(71, 14);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(141, 23);
            this.label15.TabIndex = 339;
            this.label15.Text = "Heading (Deg)";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.BackColor = System.Drawing.Color.Transparent;
            this.label20.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label20.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label20.Location = new System.Drawing.Point(362, 200);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(22, 16);
            this.label20.TabIndex = 346;
            this.label20.Text = "%";
            this.label20.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.BackColor = System.Drawing.Color.Transparent;
            this.label26.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label26.Location = new System.Drawing.Point(302, 106);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(52, 23);
            this.label26.TabIndex = 351;
            this.label26.Text = "More";
            this.label26.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblAvgXTE
            // 
            this.lblAvgXTE.AutoSize = true;
            this.lblAvgXTE.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAvgXTE.Location = new System.Drawing.Point(358, 214);
            this.lblAvgXTE.Name = "lblAvgXTE";
            this.lblAvgXTE.Size = new System.Drawing.Size(91, 39);
            this.lblAvgXTE.TabIndex = 343;
            this.lblAvgXTE.Text = "-888";
            // 
            // label22
            // 
            this.label22.BackColor = System.Drawing.Color.Transparent;
            this.label22.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label22.Location = new System.Drawing.Point(137, 174);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(129, 46);
            this.label22.TabIndex = 345;
            this.label22.Text = "Cross Track Smoothing";
            this.label22.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarAvgXTE
            // 
            this.hsbarAvgXTE.LargeChange = 1;
            this.hsbarAvgXTE.Location = new System.Drawing.Point(70, 222);
            this.hsbarAvgXTE.Maximum = 99;
            this.hsbarAvgXTE.Minimum = 1;
            this.hsbarAvgXTE.Name = "hsbarAvgXTE";
            this.hsbarAvgXTE.Size = new System.Drawing.Size(285, 30);
            this.hsbarAvgXTE.TabIndex = 342;
            this.hsbarAvgXTE.Value = 5;
            this.hsbarAvgXTE.ValueChanged += new System.EventHandler(this.hsbarAvgXTE_ValueChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.BackColor = System.Drawing.Color.Transparent;
            this.label17.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label17.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.label17.Location = new System.Drawing.Point(361, 109);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(35, 16);
            this.label17.TabIndex = 341;
            this.label17.Text = "Gain";
            this.label17.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblStanleyIntegralGain
            // 
            this.lblStanleyIntegralGain.AutoSize = true;
            this.lblStanleyIntegralGain.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStanleyIntegralGain.Location = new System.Drawing.Point(357, 124);
            this.lblStanleyIntegralGain.Name = "lblStanleyIntegralGain";
            this.lblStanleyIntegralGain.Size = new System.Drawing.Size(91, 39);
            this.lblStanleyIntegralGain.TabIndex = 338;
            this.lblStanleyIntegralGain.Text = "-888";
            // 
            // label18
            // 
            this.label18.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label18.Location = new System.Drawing.Point(135, 106);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(161, 23);
            this.label18.TabIndex = 340;
            this.label18.Text = "Integral";
            this.label18.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // hsbarIntegralGain
            // 
            this.hsbarIntegralGain.LargeChange = 1;
            this.hsbarIntegralGain.Location = new System.Drawing.Point(69, 132);
            this.hsbarIntegralGain.Name = "hsbarIntegralGain";
            this.hsbarIntegralGain.Size = new System.Drawing.Size(285, 30);
            this.hsbarIntegralGain.TabIndex = 337;
            this.hsbarIntegralGain.Value = 5;
            this.hsbarIntegralGain.ValueChanged += new System.EventHandler(this.hsbarIntegralGain_ValueChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.BackColor = System.Drawing.Color.Transparent;
            this.label21.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.Location = new System.Drawing.Point(303, 194);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(52, 23);
            this.label21.TabIndex = 347;
            this.label21.Text = "More";
            this.label21.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.BackColor = System.Drawing.Color.Transparent;
            this.label23.Font = new System.Drawing.Font("Tahoma", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label23.Location = new System.Drawing.Point(71, 194);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(45, 23);
            this.label23.TabIndex = 349;
            this.label23.Text = "Less";
            this.label23.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 250;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.BackColor = System.Drawing.Color.Transparent;
            this.label8.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(404, 338);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(24, 16);
            this.label8.TabIndex = 359;
            this.label8.Text = "10";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.BackColor = System.Drawing.Color.Transparent;
            this.label9.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(404, 364);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 16);
            this.label9.TabIndex = 358;
            this.label9.Text = "10";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(407, 394);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(16, 16);
            this.label11.TabIndex = 357;
            this.label11.Text = "5";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // pbErrNeg
            // 
            this.pbErrNeg.Location = new System.Drawing.Point(147, 393);
            this.pbErrNeg.Maximum = 50;
            this.pbErrNeg.Name = "pbErrNeg";
            this.pbErrNeg.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pbErrNeg.RightToLeftLayout = true;
            this.pbErrNeg.Size = new System.Drawing.Size(126, 18);
            this.pbErrNeg.Step = 1;
            this.pbErrNeg.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.pbErrNeg.TabIndex = 356;
            // 
            // BtnFreeDrive
            // 
            this.BtnFreeDrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFreeDrive.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnFreeDrive.Location = new System.Drawing.Point(298, 422);
            this.BtnFreeDrive.Name = "BtnFreeDrive";
            this.BtnFreeDrive.Size = new System.Drawing.Size(125, 29);
            this.BtnFreeDrive.TabIndex = 336;
            this.BtnFreeDrive.Text = "Drive";
            this.BtnFreeDrive.UseVisualStyleBackColor = true;
            this.BtnFreeDrive.Click += new System.EventHandler(this.BtnFreeDrive_Click);
            // 
            // BtnPlus20
            // 
            this.BtnPlus20.Enabled = false;
            this.BtnPlus20.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPlus20.Location = new System.Drawing.Point(340, 462);
            this.BtnPlus20.Name = "BtnPlus20";
            this.BtnPlus20.Size = new System.Drawing.Size(83, 45);
            this.BtnPlus20.TabIndex = 350;
            this.BtnPlus20.Text = "+20";
            this.BtnPlus20.UseVisualStyleBackColor = true;
            this.BtnPlus20.Click += new System.EventHandler(this.BtnPlus20_Click);
            // 
            // pbErrPos
            // 
            this.pbErrPos.Location = new System.Drawing.Point(275, 393);
            this.pbErrPos.Maximum = 50;
            this.pbErrPos.Name = "pbErrPos";
            this.pbErrPos.Size = new System.Drawing.Size(126, 18);
            this.pbErrPos.Step = 1;
            this.pbErrPos.TabIndex = 355;
            // 
            // pbSetNeg
            // 
            this.pbSetNeg.Location = new System.Drawing.Point(147, 336);
            this.pbSetNeg.Name = "pbSetNeg";
            this.pbSetNeg.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pbSetNeg.RightToLeftLayout = true;
            this.pbSetNeg.Size = new System.Drawing.Size(126, 18);
            this.pbSetNeg.Step = 1;
            this.pbSetNeg.TabIndex = 354;
            // 
            // pbActNeg
            // 
            this.pbActNeg.Location = new System.Drawing.Point(147, 363);
            this.pbActNeg.Name = "pbActNeg";
            this.pbActNeg.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.pbActNeg.RightToLeftLayout = true;
            this.pbActNeg.Size = new System.Drawing.Size(126, 18);
            this.pbActNeg.Step = 1;
            this.pbActNeg.TabIndex = 352;
            // 
            // pbActPos
            // 
            this.pbActPos.Location = new System.Drawing.Point(275, 363);
            this.pbActPos.Name = "pbActPos";
            this.pbActPos.Size = new System.Drawing.Size(126, 18);
            this.pbActPos.Step = 1;
            this.pbActPos.TabIndex = 351;
            // 
            // pbSetPos
            // 
            this.pbSetPos.Location = new System.Drawing.Point(275, 336);
            this.pbSetPos.Name = "pbSetPos";
            this.pbSetPos.Size = new System.Drawing.Size(126, 18);
            this.pbSetPos.Step = 1;
            this.pbSetPos.TabIndex = 353;
            // 
            // BtnMinus20
            // 
            this.BtnMinus20.Enabled = false;
            this.BtnMinus20.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnMinus20.Location = new System.Drawing.Point(36, 462);
            this.BtnMinus20.Name = "BtnMinus20";
            this.BtnMinus20.Size = new System.Drawing.Size(83, 45);
            this.BtnMinus20.TabIndex = 349;
            this.BtnMinus20.Text = "-20";
            this.BtnMinus20.UseVisualStyleBackColor = true;
            this.BtnMinus20.Click += new System.EventHandler(this.BtnMinus20_Click);
            // 
            // BtnSteerAngleDown
            // 
            this.BtnSteerAngleDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnSteerAngleDown.Enabled = false;
            this.BtnSteerAngleDown.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSteerAngleDown.Image = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.BtnSteerAngleDown.Location = new System.Drawing.Point(137, 462);
            this.BtnSteerAngleDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnSteerAngleDown.Name = "BtnSteerAngleDown";
            this.BtnSteerAngleDown.Size = new System.Drawing.Size(83, 45);
            this.BtnSteerAngleDown.TabIndex = 343;
            this.BtnSteerAngleDown.UseVisualStyleBackColor = true;
            this.BtnSteerAngleDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnSteerAngleDown_MouseDown);
            // 
            // BtnSteerAngleUp
            // 
            this.BtnSteerAngleUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnSteerAngleUp.Enabled = false;
            this.BtnSteerAngleUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnSteerAngleUp.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.BtnSteerAngleUp.Location = new System.Drawing.Point(240, 462);
            this.BtnSteerAngleUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.BtnSteerAngleUp.Name = "BtnSteerAngleUp";
            this.BtnSteerAngleUp.Size = new System.Drawing.Size(83, 45);
            this.BtnSteerAngleUp.TabIndex = 344;
            this.BtnSteerAngleUp.UseVisualStyleBackColor = true;
            this.BtnSteerAngleUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnSteerAngleUp_MouseDown);
            // 
            // BtnFreeDriveZero
            // 
            this.BtnFreeDriveZero.Enabled = false;
            this.BtnFreeDriveZero.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnFreeDriveZero.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnFreeDriveZero.Location = new System.Drawing.Point(164, 422);
            this.BtnFreeDriveZero.Name = "BtnFreeDriveZero";
            this.BtnFreeDriveZero.Size = new System.Drawing.Size(118, 28);
            this.BtnFreeDriveZero.TabIndex = 342;
            this.BtnFreeDriveZero.Text = ">0<   or   +5";
            this.BtnFreeDriveZero.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnFreeDriveZero.UseVisualStyleBackColor = true;
            this.BtnFreeDriveZero.Click += new System.EventHandler(this.BtnFreeDriveZero_Click);
            // 
            // lblError
            // 
            this.lblError.BackColor = System.Drawing.Color.Transparent;
            this.lblError.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblError.Location = new System.Drawing.Point(75, 391);
            this.lblError.Name = "lblError";
            this.lblError.Size = new System.Drawing.Size(70, 23);
            this.lblError.TabIndex = 341;
            this.lblError.Text = "-30.0";
            this.lblError.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSteerAngle
            // 
            this.lblSteerAngle.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngle.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngle.ForeColor = System.Drawing.Color.DarkViolet;
            this.lblSteerAngle.Location = new System.Drawing.Point(75, 334);
            this.lblSteerAngle.Name = "lblSteerAngle";
            this.lblSteerAngle.Size = new System.Drawing.Size(79, 23);
            this.lblSteerAngle.TabIndex = 337;
            this.lblSteerAngle.Text = "-55.5";
            this.lblSteerAngle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSteerAngleActual
            // 
            this.lblSteerAngleActual.BackColor = System.Drawing.Color.Transparent;
            this.lblSteerAngleActual.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSteerAngleActual.ForeColor = System.Drawing.Color.DarkCyan;
            this.lblSteerAngleActual.Location = new System.Drawing.Point(75, 360);
            this.lblSteerAngleActual.Name = "lblSteerAngleActual";
            this.lblSteerAngleActual.Size = new System.Drawing.Size(70, 23);
            this.lblSteerAngleActual.TabIndex = 340;
            this.lblSteerAngleActual.Text = "-55.5";
            this.lblSteerAngleActual.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSent
            // 
            this.lblSent.AutoSize = true;
            this.lblSent.BackColor = System.Drawing.Color.Transparent;
            this.lblSent.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSent.Location = new System.Drawing.Point(71, 429);
            this.lblSent.Name = "lblSent";
            this.lblSent.Size = new System.Drawing.Size(32, 16);
            this.lblSent.TabIndex = 339;
            this.lblSent.Text = "255";
            this.lblSent.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblRecd
            // 
            this.lblRecd.AutoSize = true;
            this.lblRecd.BackColor = System.Drawing.Color.Transparent;
            this.lblRecd.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRecd.Location = new System.Drawing.Point(103, 429);
            this.lblRecd.Name = "lblRecd";
            this.lblRecd.Size = new System.Drawing.Size(32, 16);
            this.lblRecd.TabIndex = 338;
            this.lblRecd.Text = "255";
            this.lblRecd.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label12
            // 
            this.label12.BackColor = System.Drawing.Color.Transparent;
            this.label12.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.Location = new System.Drawing.Point(34, 336);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(45, 23);
            this.label12.TabIndex = 347;
            this.label12.Text = "Set:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label13
            // 
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(32, 361);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(45, 23);
            this.label13.TabIndex = 346;
            this.label13.Text = "Act:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label14
            // 
            this.label14.BackColor = System.Drawing.Color.Transparent;
            this.label14.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.Location = new System.Drawing.Point(33, 392);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(45, 23);
            this.label14.TabIndex = 348;
            this.label14.Text = "Err:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.BackColor = System.Drawing.Color.Transparent;
            this.label16.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label16.Location = new System.Drawing.Point(41, 429);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(36, 16);
            this.label16.TabIndex = 345;
            this.label16.Text = "Chk:";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // FormSteer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(659, 538);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.pbErrNeg);
            this.Controls.Add(this.BtnFreeDrive);
            this.Controls.Add(this.BtnPlus20);
            this.Controls.Add(this.pbErrPos);
            this.Controls.Add(this.pbSetNeg);
            this.Controls.Add(this.pbActNeg);
            this.Controls.Add(this.pbActPos);
            this.Controls.Add(this.pbSetPos);
            this.Controls.Add(this.BtnMinus20);
            this.Controls.Add(this.BtnSteerAngleDown);
            this.Controls.Add(this.BtnSteerAngleUp);
            this.Controls.Add(this.BtnFreeDriveZero);
            this.Controls.Add(this.lblError);
            this.Controls.Add(this.lblSteerAngle);
            this.Controls.Add(this.lblSteerAngleActual);
            this.Controls.Add(this.lblSent);
            this.Controls.Add(this.lblRecd);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSteer";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Auto Steer Configuration";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FormSteer_FormClosing);
            this.Load += new System.EventHandler(this.FormSteer_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabGain.ResumeLayout(false);
            this.tabGain.PerformLayout();
            this.tabSteer.ResumeLayout(false);
            this.tabSteer.PerformLayout();
            this.tabLook.ResumeLayout(false);
            this.tabLook.PerformLayout();
            this.tabStan.ResumeLayout(false);
            this.tabStan.PerformLayout();
            this.tabInt.ResumeLayout(false);
            this.tabInt.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabSteer;
        private System.Windows.Forms.TabPage tabGain;
        private System.Windows.Forms.Label lblHighSteerPWM;
        private System.Windows.Forms.Label label45;
        private System.Windows.Forms.HScrollBar hsbarHighSteerPWM;
        private System.Windows.Forms.Label lblLowSteerPWM;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.HScrollBar hsbarLowSteerPWM;
        private System.Windows.Forms.Label lblSidehillDraftGain;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.HScrollBar hsbarSidehillDraftGain;
        private System.Windows.Forms.Label lblProportionalGain;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.HScrollBar hsbarProportionalGain;
        private System.Windows.Forms.Label lblLookAhead;
        private System.Windows.Forms.HScrollBar hsbarLookAhead;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label lblMinPWM;
        private System.Windows.Forms.HScrollBar hsbarMinPWM;
        private System.Windows.Forms.Label label41;
        private System.Windows.Forms.Label lblCountsPerDegree;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.HScrollBar hsbarCountsPerDegree;
        private System.Windows.Forms.Label lblMaxSteerAngle;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.HScrollBar hsbarMaxSteerAngle;
        private System.Windows.Forms.Label lblSteerAngleSensorZero;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.HScrollBar hsbarSteerAngleSensorZero;
        private System.Windows.Forms.TabPage tabLook;
        private System.Windows.Forms.HScrollBar hsbarLookAheadMin;
        private System.Windows.Forms.HScrollBar hsbarDistanceFromLine;
        private System.Windows.Forms.Label lblLookAheadMinimum;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label lblDistanceFromLine;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnChart;
        private System.Windows.Forms.HScrollBar hsbarLookAheadUturnMult;
        private System.Windows.Forms.Label lblLookAheadUturnMult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TabPage tabStan;
        private System.Windows.Forms.HScrollBar hsbarHeadingErrorGain;
        private System.Windows.Forms.Label lblStanleyGain;
        private System.Windows.Forms.HScrollBar hsbarStanleyGain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblHeadingErrorGain;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnStanley;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabPage tabInt;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.Label lblDistanceAway;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.Label lblAvgXTE;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.HScrollBar hsbarAvgXTE;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label lblStanleyIntegralGain;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.HScrollBar hsbarIntegralGain;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox TboxIntDistance;
        private System.Windows.Forms.TextBox TboxIntHeading;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ProgressBar pbErrNeg;
        private System.Windows.Forms.Button BtnFreeDrive;
        private System.Windows.Forms.Button BtnPlus20;
        private System.Windows.Forms.ProgressBar pbErrPos;
        private System.Windows.Forms.ProgressBar pbSetNeg;
        private System.Windows.Forms.ProgressBar pbActNeg;
        private System.Windows.Forms.ProgressBar pbActPos;
        private System.Windows.Forms.ProgressBar pbSetPos;
        private System.Windows.Forms.Button BtnMinus20;
        private ProXoft.WinForms.RepeatButton BtnSteerAngleDown;
        private ProXoft.WinForms.RepeatButton BtnSteerAngleUp;
        private System.Windows.Forms.Button BtnFreeDriveZero;
        private System.Windows.Forms.Label lblError;
        private System.Windows.Forms.Label lblSteerAngle;
        private System.Windows.Forms.Label lblSteerAngleActual;
        private System.Windows.Forms.Label lblSent;
        private System.Windows.Forms.Label lblRecd;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.Label lblPWMDisplay;
    }
}