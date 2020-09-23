namespace AgOpenGPS
{
    partial class FormABDraw
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
            this.oglSelf = new OpenTK.GLControl();
            this.btnMakeABLine = new System.Windows.Forms.Button();
            this.btnMakeCurve = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.Next = new System.Windows.Forms.Button();
            this.Previous = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.ABDrawBox = new System.Windows.Forms.GroupBox();
            this.TboxOffset1 = new System.Windows.Forms.TextBox();
            this.btnDrawSections = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.lblABLineName = new System.Windows.Forms.Label();
            this.btnDeleteABLine = new System.Windows.Forms.Button();
            this.btnSelectABLine = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCurveName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnDeleteCurve = new System.Windows.Forms.Button();
            this.btnSelectCurve = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.Offset1 = new System.Windows.Forms.Label();
            this.btnCancelTouch = new System.Windows.Forms.Button();
            this.HeadLandBox = new System.Windows.Forms.GroupBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.btnDoneManualMove = new System.Windows.Forms.Button();
            this.btnMoveRight = new ProXoft.WinForms.RepeatButton();
            this.btnMoveLeft = new ProXoft.WinForms.RepeatButton();
            this.btnMoveUp = new ProXoft.WinForms.RepeatButton();
            this.btnMoveDown = new ProXoft.WinForms.RepeatButton();
            this.lblEnd = new System.Windows.Forms.Label();
            this.lblStart = new System.Windows.Forms.Label();
            this.btnEndUp = new ProXoft.WinForms.RepeatButton();
            this.btnEndDown = new ProXoft.WinForms.RepeatButton();
            this.btnStartUp = new ProXoft.WinForms.RepeatButton();
            this.btnStartDown = new ProXoft.WinForms.RepeatButton();
            this.btnReset = new System.Windows.Forms.Button();
            this.btnDeletePoints = new System.Windows.Forms.Button();
            this.btnMakeFixedHeadland = new System.Windows.Forms.Button();
            this.Offset2 = new System.Windows.Forms.Label();
            this.TboxOffset2 = new System.Windows.Forms.TextBox();
            this.btnExit2 = new System.Windows.Forms.Button();
            this.btnTurnOffHeadland = new System.Windows.Forms.Button();
            this.ABDrawBox.SuspendLayout();
            this.HeadLandBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // oglSelf
            // 
            this.oglSelf.BackColor = System.Drawing.Color.Black;
            this.oglSelf.Cursor = System.Windows.Forms.Cursors.Cross;
            this.oglSelf.Location = new System.Drawing.Point(10, 10);
            this.oglSelf.Margin = new System.Windows.Forms.Padding(0);
            this.oglSelf.Name = "oglSelf";
            this.oglSelf.Size = new System.Drawing.Size(700, 700);
            this.oglSelf.TabIndex = 183;
            this.oglSelf.VSync = false;
            this.oglSelf.Load += new System.EventHandler(this.OglSelf_Load);
            this.oglSelf.Paint += new System.Windows.Forms.PaintEventHandler(this.OglSelf_Paint);
            this.oglSelf.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OglSelf_MouseDown);
            this.oglSelf.Resize += new System.EventHandler(this.OglSelf_Resize);
            // 
            // btnMakeABLine
            // 
            this.btnMakeABLine.BackColor = System.Drawing.SystemColors.Control;
            this.btnMakeABLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMakeABLine.Enabled = false;
            this.btnMakeABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeABLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineOn;
            this.btnMakeABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeABLine.Location = new System.Drawing.Point(190, 40);
            this.btnMakeABLine.Name = "btnMakeABLine";
            this.btnMakeABLine.Size = new System.Drawing.Size(80, 80);
            this.btnMakeABLine.TabIndex = 311;
            this.btnMakeABLine.UseVisualStyleBackColor = false;
            this.btnMakeABLine.Click += new System.EventHandler(this.BtnMakeABLine_Click);
            // 
            // btnMakeCurve
            // 
            this.btnMakeCurve.BackColor = System.Drawing.SystemColors.Control;
            this.btnMakeCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMakeCurve.Enabled = false;
            this.btnMakeCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeCurve.Image = global::AgOpenGPS.Properties.Resources.CurveOn;
            this.btnMakeCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeCurve.Location = new System.Drawing.Point(100, 40);
            this.btnMakeCurve.Name = "btnMakeCurve";
            this.btnMakeCurve.Size = new System.Drawing.Size(80, 80);
            this.btnMakeCurve.TabIndex = 313;
            this.btnMakeCurve.UseVisualStyleBackColor = false;
            this.btnMakeCurve.Click += new System.EventHandler(this.BtnMakeCurve_Click);
            // 
            // label3
            // 
            this.label3.Enabled = false;
            this.label3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(10, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(260, 30);
            this.label3.TabIndex = 336;
            this.label3.Text = "gsCreate";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // Next
            // 
            this.Next.BackColor = System.Drawing.SystemColors.Control;
            this.Next.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.Next.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Next.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Next.Location = new System.Drawing.Point(855, 10);
            this.Next.Name = "Next";
            this.Next.Size = new System.Drawing.Size(125, 50);
            this.Next.TabIndex = 342;
            this.Next.Text = "Next";
            this.Next.UseVisualStyleBackColor = false;
            this.Next.Click += new System.EventHandler(this.Next_Click);
            // 
            // Previous
            // 
            this.Previous.BackColor = System.Drawing.SystemColors.Control;
            this.Previous.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.Previous.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Previous.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Previous.Location = new System.Drawing.Point(720, 10);
            this.Previous.Name = "Previous";
            this.Previous.Size = new System.Drawing.Size(125, 50);
            this.Previous.TabIndex = 343;
            this.Previous.Text = "Previous";
            this.Previous.UseVisualStyleBackColor = false;
            this.Previous.Click += new System.EventHandler(this.Previous_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.Control;
            this.button1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.button1.Image = global::AgOpenGPS.Properties.Resources.ContourOn;
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(10, 40);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(80, 80);
            this.button1.TabIndex = 344;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.BtnMakeCurve_Click);
            // 
            // label4
            // 
            this.label4.Enabled = false;
            this.label4.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(10, 145);
            this.label4.Margin = new System.Windows.Forms.Padding(0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(260, 29);
            this.label4.TabIndex = 337;
            this.label4.Text = "gsSelect";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel2.Location = new System.Drawing.Point(720, 70);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(260, 5);
            this.panel2.TabIndex = 324;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel3.Location = new System.Drawing.Point(10, 130);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(260, 5);
            this.panel3.TabIndex = 325;
            // 
            // ABDrawBox
            // 
            this.ABDrawBox.BackColor = System.Drawing.Color.Transparent;
            this.ABDrawBox.Controls.Add(this.TboxOffset1);
            this.ABDrawBox.Controls.Add(this.btnDrawSections);
            this.ABDrawBox.Controls.Add(this.btnExit);
            this.ABDrawBox.Controls.Add(this.lblABLineName);
            this.ABDrawBox.Controls.Add(this.btnDeleteABLine);
            this.ABDrawBox.Controls.Add(this.btnSelectABLine);
            this.ABDrawBox.Controls.Add(this.label2);
            this.ABDrawBox.Controls.Add(this.lblCurveName);
            this.ABDrawBox.Controls.Add(this.label1);
            this.ABDrawBox.Controls.Add(this.btnDeleteCurve);
            this.ABDrawBox.Controls.Add(this.btnSelectCurve);
            this.ABDrawBox.Controls.Add(this.panel1);
            this.ABDrawBox.Controls.Add(this.Offset1);
            this.ABDrawBox.Controls.Add(this.btnCancelTouch);
            this.ABDrawBox.Controls.Add(this.label3);
            this.ABDrawBox.Controls.Add(this.panel3);
            this.ABDrawBox.Controls.Add(this.btnMakeCurve);
            this.ABDrawBox.Controls.Add(this.btnMakeABLine);
            this.ABDrawBox.Controls.Add(this.button1);
            this.ABDrawBox.Controls.Add(this.label4);
            this.ABDrawBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ABDrawBox.Location = new System.Drawing.Point(710, 85);
            this.ABDrawBox.Name = "ABDrawBox";
            this.ABDrawBox.Size = new System.Drawing.Size(280, 626);
            this.ABDrawBox.TabIndex = 346;
            this.ABDrawBox.TabStop = false;
            // 
            // TboxOffset1
            // 
            this.TboxOffset1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TboxOffset1.BackColor = System.Drawing.SystemColors.Control;
            this.TboxOffset1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxOffset1.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxOffset1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxOffset1.Location = new System.Drawing.Point(10, 214);
            this.TboxOffset1.MaxLength = 10;
            this.TboxOffset1.Name = "TboxOffset1";
            this.TboxOffset1.Size = new System.Drawing.Size(170, 50);
            this.TboxOffset1.TabIndex = 489;
            this.TboxOffset1.Text = "0.0";
            this.TboxOffset1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxOffset1.Enter += new System.EventHandler(this.TboxOffset_Enter);
            // 
            // btnDrawSections
            // 
            this.btnDrawSections.BackColor = System.Drawing.SystemColors.Control;
            this.btnDrawSections.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDrawSections.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDrawSections.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDrawSections.Location = new System.Drawing.Point(10, 545);
            this.btnDrawSections.Name = "btnDrawSections";
            this.btnDrawSections.Size = new System.Drawing.Size(80, 80);
            this.btnDrawSections.TabIndex = 0;
            this.btnDrawSections.Text = "Off";
            this.btnDrawSections.UseVisualStyleBackColor = false;
            this.btnDrawSections.Click += new System.EventHandler(this.BtnDrawSections_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnExit.Location = new System.Drawing.Point(190, 545);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(80, 80);
            this.btnExit.TabIndex = 357;
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.BtnExit_Click);
            // 
            // lblABLineName
            // 
            this.lblABLineName.Enabled = false;
            this.lblABLineName.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblABLineName.Location = new System.Drawing.Point(10, 507);
            this.lblABLineName.Margin = new System.Windows.Forms.Padding(0);
            this.lblABLineName.Name = "lblABLineName";
            this.lblABLineName.Size = new System.Drawing.Size(260, 28);
            this.lblABLineName.TabIndex = 358;
            this.lblABLineName.Text = "Name";
            this.lblABLineName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDeleteABLine
            // 
            this.btnDeleteABLine.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteABLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteABLine.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteABLine.Image = global::AgOpenGPS.Properties.Resources.FileDelete;
            this.btnDeleteABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteABLine.Location = new System.Drawing.Point(190, 417);
            this.btnDeleteABLine.Name = "btnDeleteABLine";
            this.btnDeleteABLine.Size = new System.Drawing.Size(80, 80);
            this.btnDeleteABLine.TabIndex = 354;
            this.btnDeleteABLine.UseVisualStyleBackColor = false;
            this.btnDeleteABLine.Click += new System.EventHandler(this.BtnDeleteABLine_Click);
            // 
            // btnSelectABLine
            // 
            this.btnSelectABLine.BackColor = System.Drawing.SystemColors.Control;
            this.btnSelectABLine.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSelectABLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectABLine.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSelectABLine.Image = global::AgOpenGPS.Properties.Resources.ABLineOn;
            this.btnSelectABLine.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSelectABLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectABLine.Location = new System.Drawing.Point(10, 417);
            this.btnSelectABLine.Name = "btnSelectABLine";
            this.btnSelectABLine.Size = new System.Drawing.Size(80, 80);
            this.btnSelectABLine.TabIndex = 353;
            this.btnSelectABLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSelectABLine.UseVisualStyleBackColor = false;
            this.btnSelectABLine.Click += new System.EventHandler(this.BtnSelectABLine_Click);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(100, 417);
            this.label2.Margin = new System.Windows.Forms.Padding(0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 80);
            this.label2.TabIndex = 355;
            this.label2.Text = "9 of 99";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label2.Click += new System.EventHandler(this.Label2_Click);
            // 
            // lblCurveName
            // 
            this.lblCurveName.Enabled = false;
            this.lblCurveName.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurveName.Location = new System.Drawing.Point(10, 379);
            this.lblCurveName.Margin = new System.Windows.Forms.Padding(0);
            this.lblCurveName.Name = "lblCurveName";
            this.lblCurveName.Size = new System.Drawing.Size(260, 28);
            this.lblCurveName.TabIndex = 352;
            this.lblCurveName.Text = "Name";
            this.lblCurveName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(100, 289);
            this.label1.Margin = new System.Windows.Forms.Padding(0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 80);
            this.label1.TabIndex = 351;
            this.label1.Text = "9 of 99";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label1.Click += new System.EventHandler(this.Label1_Click);
            // 
            // btnDeleteCurve
            // 
            this.btnDeleteCurve.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeleteCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnDeleteCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeleteCurve.Image = global::AgOpenGPS.Properties.Resources.FileDelete;
            this.btnDeleteCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeleteCurve.Location = new System.Drawing.Point(190, 289);
            this.btnDeleteCurve.Name = "btnDeleteCurve";
            this.btnDeleteCurve.Size = new System.Drawing.Size(80, 80);
            this.btnDeleteCurve.TabIndex = 350;
            this.btnDeleteCurve.UseVisualStyleBackColor = false;
            this.btnDeleteCurve.Click += new System.EventHandler(this.BtnDeleteCurve_Click);
            // 
            // btnSelectCurve
            // 
            this.btnSelectCurve.BackColor = System.Drawing.SystemColors.Control;
            this.btnSelectCurve.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSelectCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSelectCurve.Image = global::AgOpenGPS.Properties.Resources.CurveOn;
            this.btnSelectCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSelectCurve.Location = new System.Drawing.Point(10, 289);
            this.btnSelectCurve.Name = "btnSelectCurve";
            this.btnSelectCurve.Size = new System.Drawing.Size(80, 80);
            this.btnSelectCurve.TabIndex = 349;
            this.btnSelectCurve.UseVisualStyleBackColor = false;
            this.btnSelectCurve.Click += new System.EventHandler(this.BtnSelectCurve_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel1.Location = new System.Drawing.Point(10, 274);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 5);
            this.panel1.TabIndex = 348;
            // 
            // Offset1
            // 
            this.Offset1.Enabled = false;
            this.Offset1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Offset1.Location = new System.Drawing.Point(10, 184);
            this.Offset1.Margin = new System.Windows.Forms.Padding(0);
            this.Offset1.Name = "Offset1";
            this.Offset1.Size = new System.Drawing.Size(170, 30);
            this.Offset1.TabIndex = 347;
            this.Offset1.Text = "Tool Width / 2 (cm)";
            this.Offset1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnCancelTouch
            // 
            this.btnCancelTouch.BackColor = System.Drawing.SystemColors.Control;
            this.btnCancelTouch.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnCancelTouch.Enabled = false;
            this.btnCancelTouch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelTouch.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancelTouch.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancelTouch.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelTouch.Location = new System.Drawing.Point(190, 184);
            this.btnCancelTouch.Name = "btnCancelTouch";
            this.btnCancelTouch.Size = new System.Drawing.Size(80, 80);
            this.btnCancelTouch.TabIndex = 345;
            this.btnCancelTouch.UseVisualStyleBackColor = false;
            this.btnCancelTouch.Click += new System.EventHandler(this.BtnCancelTouch_Click);
            // 
            // HeadLandBox
            // 
            this.HeadLandBox.BackColor = System.Drawing.Color.Transparent;
            this.HeadLandBox.Controls.Add(this.panel6);
            this.HeadLandBox.Controls.Add(this.panel5);
            this.HeadLandBox.Controls.Add(this.panel4);
            this.HeadLandBox.Controls.Add(this.btnDoneManualMove);
            this.HeadLandBox.Controls.Add(this.btnMoveRight);
            this.HeadLandBox.Controls.Add(this.btnMoveLeft);
            this.HeadLandBox.Controls.Add(this.btnMoveUp);
            this.HeadLandBox.Controls.Add(this.btnMoveDown);
            this.HeadLandBox.Controls.Add(this.lblEnd);
            this.HeadLandBox.Controls.Add(this.lblStart);
            this.HeadLandBox.Controls.Add(this.btnEndUp);
            this.HeadLandBox.Controls.Add(this.btnEndDown);
            this.HeadLandBox.Controls.Add(this.btnStartUp);
            this.HeadLandBox.Controls.Add(this.btnStartDown);
            this.HeadLandBox.Controls.Add(this.btnReset);
            this.HeadLandBox.Controls.Add(this.btnDeletePoints);
            this.HeadLandBox.Controls.Add(this.btnMakeFixedHeadland);
            this.HeadLandBox.Controls.Add(this.Offset2);
            this.HeadLandBox.Controls.Add(this.TboxOffset2);
            this.HeadLandBox.Controls.Add(this.btnExit2);
            this.HeadLandBox.Controls.Add(this.btnTurnOffHeadland);
            this.HeadLandBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.HeadLandBox.Location = new System.Drawing.Point(990, 85);
            this.HeadLandBox.Name = "HeadLandBox";
            this.HeadLandBox.Size = new System.Drawing.Size(280, 626);
            this.HeadLandBox.TabIndex = 347;
            this.HeadLandBox.TabStop = false;
            // 
            // panel6
            // 
            this.panel6.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel6.Location = new System.Drawing.Point(10, 530);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(260, 5);
            this.panel6.TabIndex = 350;
            // 
            // panel5
            // 
            this.panel5.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel5.Location = new System.Drawing.Point(14, 295);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(260, 5);
            this.panel5.TabIndex = 349;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel4.Location = new System.Drawing.Point(10, 90);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(260, 5);
            this.panel4.TabIndex = 326;
            // 
            // btnDoneManualMove
            // 
            this.btnDoneManualMove.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnDoneManualMove.BackColor = System.Drawing.SystemColors.Control;
            this.btnDoneManualMove.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDoneManualMove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDoneManualMove.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDoneManualMove.Image = global::AgOpenGPS.Properties.Resources.HeadlandTouchSave;
            this.btnDoneManualMove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDoneManualMove.Location = new System.Drawing.Point(100, 310);
            this.btnDoneManualMove.Name = "btnDoneManualMove";
            this.btnDoneManualMove.Size = new System.Drawing.Size(80, 80);
            this.btnDoneManualMove.TabIndex = 488;
            this.btnDoneManualMove.UseVisualStyleBackColor = false;
            this.btnDoneManualMove.Click += new System.EventHandler(this.BtnDoneManualMove_Click);
            // 
            // btnMoveRight
            // 
            this.btnMoveRight.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveRight.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveRight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMoveRight.FlatAppearance.BorderSize = 0;
            this.btnMoveRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveRight.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveRight.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnMoveRight.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMoveRight.Location = new System.Drawing.Point(180, 155);
            this.btnMoveRight.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnMoveRight.Name = "btnMoveRight";
            this.btnMoveRight.Size = new System.Drawing.Size(80, 80);
            this.btnMoveRight.TabIndex = 486;
            this.btnMoveRight.UseVisualStyleBackColor = false;
            this.btnMoveRight.Click += new System.EventHandler(this.BtnMoveRight_Click);
            // 
            // btnMoveLeft
            // 
            this.btnMoveLeft.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveLeft.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMoveLeft.FlatAppearance.BorderSize = 0;
            this.btnMoveLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveLeft.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveLeft.Image = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.btnMoveLeft.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnMoveLeft.Location = new System.Drawing.Point(20, 155);
            this.btnMoveLeft.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnMoveLeft.Name = "btnMoveLeft";
            this.btnMoveLeft.Size = new System.Drawing.Size(80, 80);
            this.btnMoveLeft.TabIndex = 487;
            this.btnMoveLeft.UseVisualStyleBackColor = false;
            this.btnMoveLeft.Click += new System.EventHandler(this.BtnMoveLeft_Click);
            // 
            // btnMoveUp
            // 
            this.btnMoveUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveUp.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMoveUp.FlatAppearance.BorderSize = 0;
            this.btnMoveUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveUp.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnMoveUp.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMoveUp.Location = new System.Drawing.Point(100, 105);
            this.btnMoveUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnMoveUp.Name = "btnMoveUp";
            this.btnMoveUp.Size = new System.Drawing.Size(80, 80);
            this.btnMoveUp.TabIndex = 485;
            this.btnMoveUp.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnMoveUp.UseVisualStyleBackColor = false;
            this.btnMoveUp.Click += new System.EventHandler(this.BtnMoveUp_Click);
            // 
            // btnMoveDown
            // 
            this.btnMoveDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnMoveDown.BackColor = System.Drawing.Color.Transparent;
            this.btnMoveDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMoveDown.FlatAppearance.BorderSize = 0;
            this.btnMoveDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMoveDown.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMoveDown.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnMoveDown.ImageAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnMoveDown.Location = new System.Drawing.Point(100, 205);
            this.btnMoveDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnMoveDown.Name = "btnMoveDown";
            this.btnMoveDown.Size = new System.Drawing.Size(80, 80);
            this.btnMoveDown.TabIndex = 484;
            this.btnMoveDown.UseVisualStyleBackColor = false;
            this.btnMoveDown.Click += new System.EventHandler(this.BtnMoveDown_Click);
            // 
            // lblEnd
            // 
            this.lblEnd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblEnd.BackColor = System.Drawing.Color.Khaki;
            this.lblEnd.Enabled = false;
            this.lblEnd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblEnd.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEnd.Location = new System.Drawing.Point(190, 400);
            this.lblEnd.Margin = new System.Windows.Forms.Padding(0);
            this.lblEnd.Name = "lblEnd";
            this.lblEnd.Size = new System.Drawing.Size(80, 30);
            this.lblEnd.TabIndex = 479;
            this.lblEnd.Text = "99999";
            this.lblEnd.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblStart
            // 
            this.lblStart.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblStart.BackColor = System.Drawing.Color.LightSalmon;
            this.lblStart.Enabled = false;
            this.lblStart.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStart.Location = new System.Drawing.Point(10, 400);
            this.lblStart.Margin = new System.Windows.Forms.Padding(0);
            this.lblStart.Name = "lblStart";
            this.lblStart.Size = new System.Drawing.Size(80, 30);
            this.lblStart.TabIndex = 478;
            this.lblStart.Text = "99999";
            this.lblStart.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnEndUp
            // 
            this.btnEndUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEndUp.BackColor = System.Drawing.Color.Transparent;
            this.btnEndUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEndUp.FlatAppearance.BorderSize = 0;
            this.btnEndUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEndUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEndUp.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnEndUp.Location = new System.Drawing.Point(190, 310);
            this.btnEndUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnEndUp.Name = "btnEndUp";
            this.btnEndUp.Size = new System.Drawing.Size(80, 80);
            this.btnEndUp.TabIndex = 483;
            this.btnEndUp.UseVisualStyleBackColor = false;
            this.btnEndUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnEndUp_MouseDown);
            // 
            // btnEndDown
            // 
            this.btnEndDown.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnEndDown.BackColor = System.Drawing.Color.Transparent;
            this.btnEndDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnEndDown.FlatAppearance.BorderSize = 0;
            this.btnEndDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEndDown.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnEndDown.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnEndDown.Location = new System.Drawing.Point(190, 440);
            this.btnEndDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnEndDown.Name = "btnEndDown";
            this.btnEndDown.Size = new System.Drawing.Size(80, 80);
            this.btnEndDown.TabIndex = 482;
            this.btnEndDown.UseVisualStyleBackColor = false;
            this.btnEndDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnEndDown_MouseDown);
            // 
            // btnStartUp
            // 
            this.btnStartUp.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnStartUp.BackColor = System.Drawing.Color.Transparent;
            this.btnStartUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStartUp.FlatAppearance.BorderSize = 0;
            this.btnStartUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartUp.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartUp.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.btnStartUp.Location = new System.Drawing.Point(10, 310);
            this.btnStartUp.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnStartUp.Name = "btnStartUp";
            this.btnStartUp.Size = new System.Drawing.Size(80, 80);
            this.btnStartUp.TabIndex = 481;
            this.btnStartUp.UseVisualStyleBackColor = false;
            this.btnStartUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnStartUp_MouseDown);
            // 
            // btnStartDown
            // 
            this.btnStartDown.BackColor = System.Drawing.Color.Transparent;
            this.btnStartDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnStartDown.FlatAppearance.BorderSize = 0;
            this.btnStartDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartDown.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStartDown.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.btnStartDown.Location = new System.Drawing.Point(10, 440);
            this.btnStartDown.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnStartDown.Name = "btnStartDown";
            this.btnStartDown.Size = new System.Drawing.Size(80, 80);
            this.btnStartDown.TabIndex = 480;
            this.btnStartDown.UseVisualStyleBackColor = false;
            this.btnStartDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnStartDown_MouseDown);
            // 
            // btnReset
            // 
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BackColor = System.Drawing.SystemColors.Control;
            this.btnReset.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnReset.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnReset.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnReset.Image = global::AgOpenGPS.Properties.Resources.back_button;
            this.btnReset.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnReset.Location = new System.Drawing.Point(100, 545);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(80, 80);
            this.btnReset.TabIndex = 477;
            this.btnReset.Text = "Reset";
            this.btnReset.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.BtnReset_Click);
            // 
            // btnDeletePoints
            // 
            this.btnDeletePoints.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDeletePoints.BackColor = System.Drawing.SystemColors.Control;
            this.btnDeletePoints.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnDeletePoints.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeletePoints.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnDeletePoints.Image = global::AgOpenGPS.Properties.Resources.HeadlandDeletePoints;
            this.btnDeletePoints.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnDeletePoints.Location = new System.Drawing.Point(100, 440);
            this.btnDeletePoints.Name = "btnDeletePoints";
            this.btnDeletePoints.Size = new System.Drawing.Size(80, 80);
            this.btnDeletePoints.TabIndex = 476;
            this.btnDeletePoints.UseVisualStyleBackColor = false;
            this.btnDeletePoints.Click += new System.EventHandler(this.BtnDeletePoints_Click);
            // 
            // btnMakeFixedHeadland
            // 
            this.btnMakeFixedHeadland.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnMakeFixedHeadland.BackColor = System.Drawing.SystemColors.Control;
            this.btnMakeFixedHeadland.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnMakeFixedHeadland.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMakeFixedHeadland.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnMakeFixedHeadland.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnMakeFixedHeadland.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnMakeFixedHeadland.Location = new System.Drawing.Point(190, 0);
            this.btnMakeFixedHeadland.Name = "btnMakeFixedHeadland";
            this.btnMakeFixedHeadland.Size = new System.Drawing.Size(80, 80);
            this.btnMakeFixedHeadland.TabIndex = 474;
            this.btnMakeFixedHeadland.UseVisualStyleBackColor = false;
            this.btnMakeFixedHeadland.Click += new System.EventHandler(this.BtnMakeFixedHeadland_Click);
            // 
            // Offset2
            // 
            this.Offset2.BackColor = System.Drawing.Color.Transparent;
            this.Offset2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Offset2.Location = new System.Drawing.Point(10, 0);
            this.Offset2.Name = "Offset2";
            this.Offset2.Size = new System.Drawing.Size(170, 30);
            this.Offset2.TabIndex = 473;
            this.Offset2.Text = "Tool Width / 2 (cm)";
            this.Offset2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TboxOffset2
            // 
            this.TboxOffset2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TboxOffset2.BackColor = System.Drawing.SystemColors.Control;
            this.TboxOffset2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxOffset2.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxOffset2.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxOffset2.Location = new System.Drawing.Point(10, 30);
            this.TboxOffset2.MaxLength = 10;
            this.TboxOffset2.Name = "TboxOffset2";
            this.TboxOffset2.ReadOnly = true;
            this.TboxOffset2.ShortcutsEnabled = false;
            this.TboxOffset2.Size = new System.Drawing.Size(170, 50);
            this.TboxOffset2.TabIndex = 0;
            this.TboxOffset2.TabStop = false;
            this.TboxOffset2.Text = "0.0";
            this.TboxOffset2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxOffset2.Enter += new System.EventHandler(this.TboxOffset_Enter);
            // 
            // btnExit2
            // 
            this.btnExit2.BackColor = System.Drawing.SystemColors.Control;
            this.btnExit2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExit2.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnExit2.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnExit2.Location = new System.Drawing.Point(190, 545);
            this.btnExit2.Name = "btnExit2";
            this.btnExit2.Size = new System.Drawing.Size(80, 80);
            this.btnExit2.TabIndex = 462;
            this.btnExit2.UseVisualStyleBackColor = false;
            this.btnExit2.Click += new System.EventHandler(this.BtnExit2_Click);
            // 
            // btnTurnOffHeadland
            // 
            this.btnTurnOffHeadland.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnTurnOffHeadland.BackColor = System.Drawing.SystemColors.Control;
            this.btnTurnOffHeadland.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.btnTurnOffHeadland.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnOffHeadland.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnTurnOffHeadland.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnTurnOffHeadland.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTurnOffHeadland.Location = new System.Drawing.Point(10, 545);
            this.btnTurnOffHeadland.Name = "btnTurnOffHeadland";
            this.btnTurnOffHeadland.Size = new System.Drawing.Size(80, 80);
            this.btnTurnOffHeadland.TabIndex = 461;
            this.btnTurnOffHeadland.UseVisualStyleBackColor = false;
            this.btnTurnOffHeadland.Click += new System.EventHandler(this.BtnTurnOffHeadland_Click);
            // 
            // FormABDraw
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(1270, 720);
            this.ControlBox = false;
            this.Controls.Add(this.HeadLandBox);
            this.Controls.Add(this.ABDrawBox);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.Previous);
            this.Controls.Add(this.Next);
            this.Controls.Add(this.oglSelf);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormABDraw";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Click 2 points on the Boundary to Begin";
            this.ABDrawBox.ResumeLayout(false);
            this.ABDrawBox.PerformLayout();
            this.HeadLandBox.ResumeLayout(false);
            this.HeadLandBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl oglSelf;
        private System.Windows.Forms.Button btnMakeABLine;
        private System.Windows.Forms.Button btnMakeCurve;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button Next;
        private System.Windows.Forms.Button Previous;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.GroupBox ABDrawBox;
        private System.Windows.Forms.Button btnDeleteABLine;
        private System.Windows.Forms.Button btnSelectABLine;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCurveName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnDeleteCurve;
        private System.Windows.Forms.Button btnSelectCurve;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label Offset1;
        private System.Windows.Forms.Button btnCancelTouch;
        private System.Windows.Forms.Button btnDrawSections;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label lblABLineName;
        private System.Windows.Forms.GroupBox HeadLandBox;
        private System.Windows.Forms.Button btnTurnOffHeadland;
        private System.Windows.Forms.Button btnExit2;
        private System.Windows.Forms.Label Offset2;
        private System.Windows.Forms.TextBox TboxOffset2;
        private System.Windows.Forms.Button btnMakeFixedHeadland;
        private System.Windows.Forms.Button btnReset;
        private System.Windows.Forms.Button btnDeletePoints;
        private System.Windows.Forms.Button btnDoneManualMove;
        private ProXoft.WinForms.RepeatButton btnMoveRight;
        private ProXoft.WinForms.RepeatButton btnMoveLeft;
        private ProXoft.WinForms.RepeatButton btnMoveUp;
        private ProXoft.WinForms.RepeatButton btnMoveDown;
        private System.Windows.Forms.Label lblEnd;
        private System.Windows.Forms.Label lblStart;
        private ProXoft.WinForms.RepeatButton btnEndUp;
        private ProXoft.WinForms.RepeatButton btnEndDown;
        private ProXoft.WinForms.RepeatButton btnStartUp;
        private ProXoft.WinForms.RepeatButton btnStartDown;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.TextBox TboxOffset1;
    }
}