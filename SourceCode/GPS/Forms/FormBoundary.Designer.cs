namespace AgOpenGPS
{
    partial class FormBoundary
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
            this.BtnLeftRight = new System.Windows.Forms.Button();
            this.BtnLoadMultiBoundaryFromGE = new System.Windows.Forms.Button();
            this.BtnLoadBoundaryFromGE = new System.Windows.Forms.Button();
            this.ShowPanel = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.BoundaryPanel = new System.Windows.Forms.TableLayoutPanel();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.Boundary = new System.Windows.Forms.Label();
            this.BtnClose = new System.Windows.Forms.Button();
            this.Down_Scroll = new System.Windows.Forms.Button();
            this.BtnDelete = new System.Windows.Forms.Button();
            this.Area = new System.Windows.Forms.Label();
            this.BtnDeleteAll = new System.Windows.Forms.Button();
            this.Slider_Scroll = new System.Windows.Forms.Button();
            this.BtnOpenGoogleEarth = new System.Windows.Forms.Button();
            this.Up_Scroll = new System.Windows.Forms.Button();
            this.Around = new System.Windows.Forms.Label();
            this.Thru = new System.Windows.Forms.Label();
            this.ChoosePanel = new System.Windows.Forms.Panel();
            this.BtnBack = new System.Windows.Forms.Button();
            this.BtnDrive = new System.Windows.Forms.Button();
            this.lblOffset = new System.Windows.Forms.Label();
            this.TboxBndOffset = new System.Windows.Forms.TextBox();
            this.BtnRestart = new System.Windows.Forms.Button();
            this.lblPoints = new System.Windows.Forms.Label();
            this.BtnDeleteLast = new System.Windows.Forms.Button();
            this.BtnAddPoint = new System.Windows.Forms.Button();
            this.lblArea = new System.Windows.Forms.Label();
            this.BtnStop = new System.Windows.Forms.Button();
            this.BtnPausePlay = new System.Windows.Forms.Button();
            this.DrivePanel = new System.Windows.Forms.Panel();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.ShowPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.ChoosePanel.SuspendLayout();
            this.DrivePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // BtnLeftRight
            // 
            this.BtnLeftRight.BackColor = System.Drawing.SystemColors.Control;
            this.BtnLeftRight.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(192)))));
            this.BtnLeftRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLeftRight.Image = global::AgOpenGPS.Properties.Resources.BoundaryLeft;
            this.BtnLeftRight.Location = new System.Drawing.Point(190, 10);
            this.BtnLeftRight.Name = "BtnLeftRight";
            this.BtnLeftRight.Size = new System.Drawing.Size(80, 80);
            this.BtnLeftRight.TabIndex = 67;
            this.BtnLeftRight.UseVisualStyleBackColor = false;
            this.BtnLeftRight.Click += new System.EventHandler(this.BtnLeftRight_Click);
            // 
            // BtnLoadMultiBoundaryFromGE
            // 
            this.BtnLoadMultiBoundaryFromGE.BackColor = System.Drawing.SystemColors.Control;
            this.BtnLoadMultiBoundaryFromGE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLoadMultiBoundaryFromGE.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLoadMultiBoundaryFromGE.Image = global::AgOpenGPS.Properties.Resources.BoundaryLoadMultiFromGE;
            this.BtnLoadMultiBoundaryFromGE.Location = new System.Drawing.Point(90, 34);
            this.BtnLoadMultiBoundaryFromGE.Name = "BtnLoadMultiBoundaryFromGE";
            this.BtnLoadMultiBoundaryFromGE.Size = new System.Drawing.Size(100, 100);
            this.BtnLoadMultiBoundaryFromGE.TabIndex = 211;
            this.BtnLoadMultiBoundaryFromGE.UseVisualStyleBackColor = false;
            this.BtnLoadMultiBoundaryFromGE.Click += new System.EventHandler(this.BtnLoadBoundaryFromGE_Click);
            // 
            // BtnLoadBoundaryFromGE
            // 
            this.BtnLoadBoundaryFromGE.BackColor = System.Drawing.SystemColors.Control;
            this.BtnLoadBoundaryFromGE.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnLoadBoundaryFromGE.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnLoadBoundaryFromGE.Image = global::AgOpenGPS.Properties.Resources.BoundaryLoadFromGE;
            this.BtnLoadBoundaryFromGE.Location = new System.Drawing.Point(90, 166);
            this.BtnLoadBoundaryFromGE.Name = "BtnLoadBoundaryFromGE";
            this.BtnLoadBoundaryFromGE.Size = new System.Drawing.Size(100, 100);
            this.BtnLoadBoundaryFromGE.TabIndex = 210;
            this.BtnLoadBoundaryFromGE.UseVisualStyleBackColor = false;
            this.BtnLoadBoundaryFromGE.Click += new System.EventHandler(this.BtnLoadBoundaryFromGE_Click);
            // 
            // ShowPanel
            // 
            this.ShowPanel.Controls.Add(this.panel1);
            this.ShowPanel.Controls.Add(this.BtnAdd);
            this.ShowPanel.Controls.Add(this.Boundary);
            this.ShowPanel.Controls.Add(this.BtnClose);
            this.ShowPanel.Controls.Add(this.BtnDelete);
            this.ShowPanel.Controls.Add(this.Area);
            this.ShowPanel.Controls.Add(this.BtnDeleteAll);
            this.ShowPanel.Controls.Add(this.BtnOpenGoogleEarth);
            this.ShowPanel.Controls.Add(this.Around);
            this.ShowPanel.Controls.Add(this.Thru);
            this.ShowPanel.Location = new System.Drawing.Point(0, 0);
            this.ShowPanel.Name = "ShowPanel";
            this.ShowPanel.Size = new System.Drawing.Size(550, 520);
            this.ShowPanel.TabIndex = 477;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.BoundaryPanel);
            this.panel1.Controls.Add(this.Up_Scroll);
            this.panel1.Controls.Add(this.Slider_Scroll);
            this.panel1.Controls.Add(this.Down_Scroll);
            this.panel1.Location = new System.Drawing.Point(10, 70);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(530, 350);
            this.panel1.TabIndex = 0;
            // 
            // BoundaryPanel
            // 
            this.BoundaryPanel.BackColor = System.Drawing.Color.Transparent;
            this.BoundaryPanel.ColumnCount = 4;
            this.BoundaryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34.04255F));
            this.BoundaryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.40425F));
            this.BoundaryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.2766F));
            this.BoundaryPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.2766F));
            this.BoundaryPanel.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BoundaryPanel.Location = new System.Drawing.Point(0, 0);
            this.BoundaryPanel.Margin = new System.Windows.Forms.Padding(0);
            this.BoundaryPanel.Name = "BoundaryPanel";
            this.BoundaryPanel.RowCount = 7;
            this.BoundaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.BoundaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.BoundaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.BoundaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.BoundaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.BoundaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.BoundaryPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 50F));
            this.BoundaryPanel.Size = new System.Drawing.Size(480, 350);
            this.BoundaryPanel.TabIndex = 205;
            // 
            // BtnAdd
            // 
            this.BtnAdd.BackColor = System.Drawing.SystemColors.Control;
            this.BtnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAdd.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnAdd.Image = global::AgOpenGPS.Properties.Resources.AddNew;
            this.BtnAdd.Location = new System.Drawing.Point(340, 430);
            this.BtnAdd.Margin = new System.Windows.Forms.Padding(5);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(80, 80);
            this.BtnAdd.TabIndex = 214;
            this.BtnAdd.UseVisualStyleBackColor = false;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // Boundary
            // 
            this.Boundary.BackColor = System.Drawing.Color.Transparent;
            this.Boundary.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Boundary.Location = new System.Drawing.Point(10, 10);
            this.Boundary.Name = "Boundary";
            this.Boundary.Size = new System.Drawing.Size(157, 50);
            this.Boundary.TabIndex = 203;
            this.Boundary.Text = "Bounds";
            this.Boundary.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnClose
            // 
            this.BtnClose.BackColor = System.Drawing.SystemColors.Control;
            this.BtnClose.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnClose.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnClose.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.BtnClose.Location = new System.Drawing.Point(460, 430);
            this.BtnClose.Margin = new System.Windows.Forms.Padding(5);
            this.BtnClose.Name = "BtnClose";
            this.BtnClose.Size = new System.Drawing.Size(80, 80);
            this.BtnClose.TabIndex = 64;
            this.BtnClose.UseVisualStyleBackColor = false;
            this.BtnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // Down_Scroll
            // 
            this.Down_Scroll.Font = new System.Drawing.Font("Tahoma", 24F);
            this.Down_Scroll.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.Down_Scroll.Location = new System.Drawing.Point(480, 300);
            this.Down_Scroll.Margin = new System.Windows.Forms.Padding(0);
            this.Down_Scroll.Name = "Down_Scroll";
            this.Down_Scroll.Size = new System.Drawing.Size(50, 50);
            this.Down_Scroll.TabIndex = 418;
            this.Down_Scroll.Text = "▼";
            this.Down_Scroll.UseVisualStyleBackColor = true;
            this.Down_Scroll.Click += new System.EventHandler(this.Down_Scroll_Click);
            // 
            // BtnDelete
            // 
            this.BtnDelete.BackColor = System.Drawing.SystemColors.Control;
            this.BtnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDelete.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDelete.Image = global::AgOpenGPS.Properties.Resources.BoundaryDelete;
            this.BtnDelete.Location = new System.Drawing.Point(220, 430);
            this.BtnDelete.Name = "BtnDelete";
            this.BtnDelete.Size = new System.Drawing.Size(80, 80);
            this.BtnDelete.TabIndex = 65;
            this.BtnDelete.UseVisualStyleBackColor = false;
            this.BtnDelete.Click += new System.EventHandler(this.BtnDelete_Click);
            // 
            // Area
            // 
            this.Area.BackColor = System.Drawing.Color.Transparent;
            this.Area.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Area.Location = new System.Drawing.Point(167, 10);
            this.Area.Name = "Area";
            this.Area.Size = new System.Drawing.Size(107, 50);
            this.Area.TabIndex = 201;
            this.Area.Text = "Area";
            this.Area.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnDeleteAll
            // 
            this.BtnDeleteAll.BackColor = System.Drawing.SystemColors.Control;
            this.BtnDeleteAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDeleteAll.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDeleteAll.Image = global::AgOpenGPS.Properties.Resources.BoundaryDeleteAll;
            this.BtnDeleteAll.Location = new System.Drawing.Point(130, 430);
            this.BtnDeleteAll.Name = "BtnDeleteAll";
            this.BtnDeleteAll.Size = new System.Drawing.Size(80, 80);
            this.BtnDeleteAll.TabIndex = 100;
            this.BtnDeleteAll.UseVisualStyleBackColor = false;
            this.BtnDeleteAll.Click += new System.EventHandler(this.BtnDeleteAll_Click);
            // 
            // Slider_Scroll
            // 
            this.Slider_Scroll.CausesValidation = false;
            this.Slider_Scroll.Location = new System.Drawing.Point(480, 50);
            this.Slider_Scroll.Margin = new System.Windows.Forms.Padding(0);
            this.Slider_Scroll.Name = "Slider_Scroll";
            this.Slider_Scroll.Size = new System.Drawing.Size(50, 250);
            this.Slider_Scroll.TabIndex = 419;
            this.Slider_Scroll.TabStop = false;
            this.Slider_Scroll.UseCompatibleTextRendering = true;
            this.Slider_Scroll.UseVisualStyleBackColor = true;
            this.Slider_Scroll.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mouse_Down);
            this.Slider_Scroll.MouseLeave += new System.EventHandler(this.Mouse_Leave);
            this.Slider_Scroll.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Mouse_Move);
            this.Slider_Scroll.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mouse_Up);
            // 
            // BtnOpenGoogleEarth
            // 
            this.BtnOpenGoogleEarth.BackColor = System.Drawing.SystemColors.Control;
            this.BtnOpenGoogleEarth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnOpenGoogleEarth.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnOpenGoogleEarth.Image = global::AgOpenGPS.Properties.Resources.GoogleEarth;
            this.BtnOpenGoogleEarth.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnOpenGoogleEarth.Location = new System.Drawing.Point(10, 430);
            this.BtnOpenGoogleEarth.Name = "BtnOpenGoogleEarth";
            this.BtnOpenGoogleEarth.Size = new System.Drawing.Size(80, 80);
            this.BtnOpenGoogleEarth.TabIndex = 213;
            this.BtnOpenGoogleEarth.Text = "Google Earth";
            this.BtnOpenGoogleEarth.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnOpenGoogleEarth.UseVisualStyleBackColor = false;
            this.BtnOpenGoogleEarth.Click += new System.EventHandler(this.BtnOpenGoogleEarth_Click);
            // 
            // Up_Scroll
            // 
            this.Up_Scroll.Font = new System.Drawing.Font("Tahoma", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Up_Scroll.Location = new System.Drawing.Point(480, 0);
            this.Up_Scroll.Margin = new System.Windows.Forms.Padding(0);
            this.Up_Scroll.Name = "Up_Scroll";
            this.Up_Scroll.Size = new System.Drawing.Size(50, 50);
            this.Up_Scroll.TabIndex = 417;
            this.Up_Scroll.Text = "▲";
            this.Up_Scroll.UseVisualStyleBackColor = true;
            this.Up_Scroll.Click += new System.EventHandler(this.Up_Scroll_Click);
            // 
            // Around
            // 
            this.Around.BackColor = System.Drawing.Color.Transparent;
            this.Around.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Around.Location = new System.Drawing.Point(389, 10);
            this.Around.Margin = new System.Windows.Forms.Padding(0);
            this.Around.Name = "Around";
            this.Around.Size = new System.Drawing.Size(97, 50);
            this.Around.TabIndex = 204;
            this.Around.Text = "Around?";
            this.Around.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Thru
            // 
            this.Thru.BackColor = System.Drawing.Color.Transparent;
            this.Thru.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Thru.Location = new System.Drawing.Point(289, 10);
            this.Thru.Name = "Thru";
            this.Thru.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.Thru.Size = new System.Drawing.Size(97, 50);
            this.Thru.TabIndex = 202;
            this.Thru.Text = "Thru?";
            this.Thru.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ChoosePanel
            // 
            this.ChoosePanel.Controls.Add(this.BtnBack);
            this.ChoosePanel.Controls.Add(this.BtnDrive);
            this.ChoosePanel.Controls.Add(this.BtnLoadBoundaryFromGE);
            this.ChoosePanel.Controls.Add(this.BtnLoadMultiBoundaryFromGE);
            this.ChoosePanel.Location = new System.Drawing.Point(610, 0);
            this.ChoosePanel.Name = "ChoosePanel";
            this.ChoosePanel.Size = new System.Drawing.Size(280, 520);
            this.ChoosePanel.TabIndex = 478;
            this.ChoosePanel.Visible = false;
            // 
            // BtnBack
            // 
            this.BtnBack.BackColor = System.Drawing.SystemColors.Control;
            this.BtnBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnBack.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBack.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnBack.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.BtnBack.Location = new System.Drawing.Point(10, 430);
            this.BtnBack.Margin = new System.Windows.Forms.Padding(5);
            this.BtnBack.Name = "BtnBack";
            this.BtnBack.Size = new System.Drawing.Size(80, 80);
            this.BtnBack.TabIndex = 213;
            this.BtnBack.UseVisualStyleBackColor = false;
            this.BtnBack.Click += new System.EventHandler(this.BtnBack_Click);
            // 
            // BtnDrive
            // 
            this.BtnDrive.BackColor = System.Drawing.SystemColors.Control;
            this.BtnDrive.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDrive.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnDrive.Image = global::AgOpenGPS.Properties.Resources.AutoSteerOff;
            this.BtnDrive.Location = new System.Drawing.Point(90, 298);
            this.BtnDrive.Name = "BtnDrive";
            this.BtnDrive.Size = new System.Drawing.Size(100, 100);
            this.BtnDrive.TabIndex = 212;
            this.BtnDrive.UseVisualStyleBackColor = false;
            this.BtnDrive.Click += new System.EventHandler(this.BtnGo_Click);
            // 
            // lblOffset
            // 
            this.lblOffset.BackColor = System.Drawing.Color.Transparent;
            this.lblOffset.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOffset.Location = new System.Drawing.Point(10, 10);
            this.lblOffset.Name = "lblOffset";
            this.lblOffset.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.lblOffset.Size = new System.Drawing.Size(170, 30);
            this.lblOffset.TabIndex = 488;
            this.lblOffset.Text = "gsOffset";
            this.lblOffset.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TboxBndOffset
            // 
            this.TboxBndOffset.BackColor = System.Drawing.SystemColors.Control;
            this.TboxBndOffset.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxBndOffset.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxBndOffset.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxBndOffset.Location = new System.Drawing.Point(10, 40);
            this.TboxBndOffset.MaxLength = 10;
            this.TboxBndOffset.Name = "TboxBndOffset";
            this.TboxBndOffset.Size = new System.Drawing.Size(170, 50);
            this.TboxBndOffset.TabIndex = 487;
            this.TboxBndOffset.Text = "49.99";
            this.TboxBndOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxBndOffset.Enter += new System.EventHandler(this.TboxBndOffset_Enter);
            // 
            // BtnRestart
            // 
            this.BtnRestart.BackColor = System.Drawing.SystemColors.Control;
            this.BtnRestart.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnRestart.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.BtnRestart.Image = global::AgOpenGPS.Properties.Resources.BoundaryDelete;
            this.BtnRestart.Location = new System.Drawing.Point(10, 220);
            this.BtnRestart.Name = "BtnRestart";
            this.BtnRestart.Size = new System.Drawing.Size(80, 80);
            this.BtnRestart.TabIndex = 485;
            this.BtnRestart.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnRestart.UseVisualStyleBackColor = false;
            this.BtnRestart.Click += new System.EventHandler(this.BtnRestart_Click);
            // 
            // lblPoints
            // 
            this.lblPoints.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPoints.Location = new System.Drawing.Point(10, 375);
            this.lblPoints.Name = "lblPoints";
            this.lblPoints.Size = new System.Drawing.Size(260, 30);
            this.lblPoints.TabIndex = 484;
            this.lblPoints.Text = "Points: 999";
            this.lblPoints.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnDeleteLast
            // 
            this.BtnDeleteLast.BackColor = System.Drawing.SystemColors.Control;
            this.BtnDeleteLast.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDeleteLast.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.BtnDeleteLast.Image = global::AgOpenGPS.Properties.Resources.PointDelete;
            this.BtnDeleteLast.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnDeleteLast.Location = new System.Drawing.Point(10, 115);
            this.BtnDeleteLast.Name = "BtnDeleteLast";
            this.BtnDeleteLast.Size = new System.Drawing.Size(80, 80);
            this.BtnDeleteLast.TabIndex = 483;
            this.BtnDeleteLast.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnDeleteLast.UseVisualStyleBackColor = false;
            this.BtnDeleteLast.Click += new System.EventHandler(this.BtnDeleteLast_Click);
            // 
            // BtnAddPoint
            // 
            this.BtnAddPoint.BackColor = System.Drawing.SystemColors.Control;
            this.BtnAddPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAddPoint.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.BtnAddPoint.Image = global::AgOpenGPS.Properties.Resources.PointAdd;
            this.BtnAddPoint.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnAddPoint.Location = new System.Drawing.Point(190, 115);
            this.BtnAddPoint.Name = "BtnAddPoint";
            this.BtnAddPoint.Size = new System.Drawing.Size(80, 80);
            this.BtnAddPoint.TabIndex = 482;
            this.BtnAddPoint.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnAddPoint.UseVisualStyleBackColor = false;
            this.BtnAddPoint.Click += new System.EventHandler(this.BtnAddPoint_Click);
            // 
            // lblArea
            // 
            this.lblArea.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblArea.Location = new System.Drawing.Point(10, 325);
            this.lblArea.Name = "lblArea";
            this.lblArea.Size = new System.Drawing.Size(260, 30);
            this.lblArea.TabIndex = 481;
            this.lblArea.Text = "Area: 9999";
            this.lblArea.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnStop
            // 
            this.BtnStop.BackColor = System.Drawing.SystemColors.Control;
            this.BtnStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnStop.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.BtnStop.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.BtnStop.Location = new System.Drawing.Point(190, 430);
            this.BtnStop.Name = "BtnStop";
            this.BtnStop.Size = new System.Drawing.Size(80, 80);
            this.BtnStop.TabIndex = 479;
            this.BtnStop.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnStop.UseVisualStyleBackColor = false;
            this.BtnStop.Click += new System.EventHandler(this.BtnStop_Click);
            // 
            // BtnPausePlay
            // 
            this.BtnPausePlay.BackColor = System.Drawing.SystemColors.Control;
            this.BtnPausePlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnPausePlay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnPausePlay.Image = global::AgOpenGPS.Properties.Resources.BoundaryRecord;
            this.BtnPausePlay.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnPausePlay.Location = new System.Drawing.Point(190, 220);
            this.BtnPausePlay.Name = "BtnPausePlay";
            this.BtnPausePlay.Size = new System.Drawing.Size(80, 80);
            this.BtnPausePlay.TabIndex = 478;
            this.BtnPausePlay.Text = "Record";
            this.BtnPausePlay.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnPausePlay.UseVisualStyleBackColor = false;
            this.BtnPausePlay.Click += new System.EventHandler(this.BtnPausePlay_Click);
            // 
            // DrivePanel
            // 
            this.DrivePanel.Controls.Add(this.lblOffset);
            this.DrivePanel.Controls.Add(this.BtnRestart);
            this.DrivePanel.Controls.Add(this.lblPoints);
            this.DrivePanel.Controls.Add(this.TboxBndOffset);
            this.DrivePanel.Controls.Add(this.BtnDeleteLast);
            this.DrivePanel.Controls.Add(this.BtnCancel);
            this.DrivePanel.Controls.Add(this.BtnAddPoint);
            this.DrivePanel.Controls.Add(this.BtnLeftRight);
            this.DrivePanel.Controls.Add(this.lblArea);
            this.DrivePanel.Controls.Add(this.BtnStop);
            this.DrivePanel.Controls.Add(this.BtnPausePlay);
            this.DrivePanel.Location = new System.Drawing.Point(890, 0);
            this.DrivePanel.Name = "DrivePanel";
            this.DrivePanel.Size = new System.Drawing.Size(280, 520);
            this.DrivePanel.TabIndex = 479;
            this.DrivePanel.Visible = false;
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.BtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BtnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.BtnCancel.Location = new System.Drawing.Point(10, 430);
            this.BtnCancel.Margin = new System.Windows.Forms.Padding(5);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(80, 80);
            this.BtnCancel.TabIndex = 64;
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FormBoundary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1170, 520);
            this.ControlBox = false;
            this.Controls.Add(this.DrivePanel);
            this.Controls.Add(this.ChoosePanel);
            this.Controls.Add(this.ShowPanel);
            this.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "FormBoundary";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.FormBoundary_Load);
            this.MouseWheel += new System.Windows.Forms.MouseEventHandler(this.MouseWheel_Scroll);
            this.ShowPanel.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.ChoosePanel.ResumeLayout(false);
            this.DrivePanel.ResumeLayout(false);
            this.DrivePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button BtnLeftRight;
        private System.Windows.Forms.Button BtnLoadMultiBoundaryFromGE;
        private System.Windows.Forms.Button BtnLoadBoundaryFromGE;
        private System.Windows.Forms.Panel ShowPanel;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.TableLayoutPanel BoundaryPanel;
        private System.Windows.Forms.Label Boundary;
        private System.Windows.Forms.Button BtnClose;
        private System.Windows.Forms.Button Down_Scroll;
        private System.Windows.Forms.Button BtnDelete;
        private System.Windows.Forms.Label Area;
        private System.Windows.Forms.Button BtnDeleteAll;
        private System.Windows.Forms.Button Slider_Scroll;
        private System.Windows.Forms.Button BtnOpenGoogleEarth;
        private System.Windows.Forms.Button Up_Scroll;
        private System.Windows.Forms.Label Around;
        private System.Windows.Forms.Label Thru;
        private System.Windows.Forms.Panel ChoosePanel;
        private System.Windows.Forms.Button BtnDrive;
        private System.Windows.Forms.Label lblOffset;
        private System.Windows.Forms.TextBox TboxBndOffset;
        private System.Windows.Forms.Button BtnRestart;
        private System.Windows.Forms.Label lblPoints;
        private System.Windows.Forms.Button BtnDeleteLast;
        private System.Windows.Forms.Button BtnAddPoint;
        private System.Windows.Forms.Label lblArea;
        private System.Windows.Forms.Button BtnStop;
        private System.Windows.Forms.Button BtnPausePlay;
        private System.Windows.Forms.Panel DrivePanel;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnBack;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel1;
    }
}