namespace AgOpenGPS
{
    partial class FormABCurve
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
            this.lblCurveExists = new System.Windows.Forms.Label();
            this.lvLines = new System.Windows.Forms.ListView();
            this.chField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameBox = new System.Windows.Forms.TextBox();
            this.Status = new System.Windows.Forms.Label();
            this.btnPausePlay = new System.Windows.Forms.Button();
            this.btnTurnOff = new System.Windows.Forms.Button();
            this.btnBPoint = new System.Windows.Forms.Button();
            this.btnAPoint = new System.Windows.Forms.Button();
            this.btnListUse = new System.Windows.Forms.Button();
            this.btnAddToFile = new System.Windows.Forms.Button();
            this.btnListDelete = new System.Windows.Forms.Button();
            this.btnAddAndGo = new System.Windows.Forms.Button();
            this.btnCancelMain = new System.Windows.Forms.Button();
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.SelectBox = new System.Windows.Forms.GroupBox();
            this.btnNewLine = new System.Windows.Forms.Button();
            this.ABCurveBox = new System.Windows.Forms.GroupBox();
            this.lblKeepGoing = new System.Windows.Forms.Label();
            this.lblFixHeading = new System.Windows.Forms.Label();
            this.tboxHeading = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SelectBox.SuspendLayout();
            this.ABCurveBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblCurveExists
            // 
            this.lblCurveExists.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.lblCurveExists.BackColor = System.Drawing.Color.Transparent;
            this.lblCurveExists.Cursor = System.Windows.Forms.Cursors.No;
            this.lblCurveExists.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurveExists.Location = new System.Drawing.Point(10, 45);
            this.lblCurveExists.Name = "lblCurveExists";
            this.lblCurveExists.Size = new System.Drawing.Size(220, 25);
            this.lblCurveExists.TabIndex = 87;
            this.lblCurveExists.Text = "> OFF <";
            this.lblCurveExists.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lvLines
            // 
            this.lvLines.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chField});
            this.lvLines.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLines.FullRowSelect = true;
            this.lvLines.GridLines = true;
            this.lvLines.HideSelection = false;
            this.lvLines.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvLines.LabelWrap = false;
            this.lvLines.Location = new System.Drawing.Point(10, 50);
            this.lvLines.Margin = new System.Windows.Forms.Padding(0);
            this.lvLines.MultiSelect = false;
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(300, 220);
            this.lvLines.TabIndex = 141;
            this.lvLines.TileSize = new System.Drawing.Size(240, 35);
            this.lvLines.UseCompatibleStateImageBehavior = false;
            this.lvLines.View = System.Windows.Forms.View.Tile;
            this.lvLines.SelectedIndexChanged += new System.EventHandler(this.LvLines_SelectedIndexChanged);
            // 
            // chField
            // 
            this.chField.Text = "CurveLines";
            this.chField.Width = 239;
            // 
            // NameBox
            // 
            this.NameBox.BackColor = System.Drawing.SystemColors.Control;
            this.NameBox.CausesValidation = false;
            this.NameBox.Enabled = false;
            this.NameBox.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.NameBox.Location = new System.Drawing.Point(10, 10);
            this.NameBox.Margin = new System.Windows.Forms.Padding(0);
            this.NameBox.MaxLength = 100;
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(300, 30);
            this.NameBox.TabIndex = 145;
            this.NameBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NameBox.Enter += new System.EventHandler(this.TextBox1_Enter);
            // 
            // Status
            // 
            this.Status.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Status.BackColor = System.Drawing.Color.Transparent;
            this.Status.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Status.Location = new System.Drawing.Point(10, 10);
            this.Status.Name = "Status";
            this.Status.Size = new System.Drawing.Size(220, 25);
            this.Status.TabIndex = 148;
            this.Status.Text = "Status: ";
            this.Status.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnPausePlay
            // 
            this.btnPausePlay.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPausePlay.BackColor = System.Drawing.SystemColors.Control;
            this.btnPausePlay.Enabled = false;
            this.btnPausePlay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPausePlay.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPausePlay.Image = global::AgOpenGPS.Properties.Resources.boundaryPause;
            this.btnPausePlay.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnPausePlay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnPausePlay.Location = new System.Drawing.Point(145, 280);
            this.btnPausePlay.Name = "btnPausePlay";
            this.btnPausePlay.Size = new System.Drawing.Size(80, 80);
            this.btnPausePlay.TabIndex = 140;
            this.btnPausePlay.Text = "Pause";
            this.btnPausePlay.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPausePlay.UseVisualStyleBackColor = false;
            this.btnPausePlay.Click += new System.EventHandler(this.BtnPausePlay_Click);
            // 
            // btnTurnOff
            // 
            this.btnTurnOff.BackColor = System.Drawing.Color.Transparent;
            this.btnTurnOff.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnTurnOff.FlatAppearance.BorderSize = 0;
            this.btnTurnOff.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnTurnOff.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnTurnOff.Image = global::AgOpenGPS.Properties.Resources.SwitchOff;
            this.btnTurnOff.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnTurnOff.Location = new System.Drawing.Point(10, 280);
            this.btnTurnOff.Margin = new System.Windows.Forms.Padding(0);
            this.btnTurnOff.Name = "btnTurnOff";
            this.btnTurnOff.Size = new System.Drawing.Size(80, 80);
            this.btnTurnOff.TabIndex = 86;
            this.btnTurnOff.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnTurnOff.UseVisualStyleBackColor = false;
            this.btnTurnOff.Click += new System.EventHandler(this.BtnTurnOff_Click);
            // 
            // btnBPoint
            // 
            this.btnBPoint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnBPoint.BackColor = System.Drawing.SystemColors.Control;
            this.btnBPoint.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnBPoint.Enabled = false;
            this.btnBPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBPoint.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBPoint.Image = global::AgOpenGPS.Properties.Resources.LetterBBlue;
            this.btnBPoint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnBPoint.Location = new System.Drawing.Point(140, 80);
            this.btnBPoint.Name = "btnBPoint";
            this.btnBPoint.Size = new System.Drawing.Size(90, 90);
            this.btnBPoint.TabIndex = 64;
            this.btnBPoint.UseVisualStyleBackColor = false;
            this.btnBPoint.Click += new System.EventHandler(this.BtnBPoint_Click);
            // 
            // btnAPoint
            // 
            this.btnAPoint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnAPoint.BackColor = System.Drawing.SystemColors.Control;
            this.btnAPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAPoint.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAPoint.Image = global::AgOpenGPS.Properties.Resources.LetterABlue;
            this.btnAPoint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAPoint.Location = new System.Drawing.Point(10, 80);
            this.btnAPoint.Name = "btnAPoint";
            this.btnAPoint.Size = new System.Drawing.Size(90, 90);
            this.btnAPoint.TabIndex = 63;
            this.btnAPoint.UseVisualStyleBackColor = false;
            this.btnAPoint.Click += new System.EventHandler(this.BtnAPoint_Click);
            // 
            // btnListUse
            // 
            this.btnListUse.BackColor = System.Drawing.SystemColors.Control;
            this.btnListUse.Enabled = false;
            this.btnListUse.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnListUse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnListUse.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnListUse.Image = global::AgOpenGPS.Properties.Resources.FileUse;
            this.btnListUse.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnListUse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnListUse.Location = new System.Drawing.Point(320, 280);
            this.btnListUse.Margin = new System.Windows.Forms.Padding(0);
            this.btnListUse.Name = "btnListUse";
            this.btnListUse.Size = new System.Drawing.Size(80, 80);
            this.btnListUse.TabIndex = 144;
            this.btnListUse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnListUse.UseVisualStyleBackColor = false;
            this.btnListUse.Click += new System.EventHandler(this.BtnListUse_Click);
            // 
            // btnAddToFile
            // 
            this.btnAddToFile.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddToFile.Enabled = false;
            this.btnAddToFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddToFile.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnAddToFile.Image = global::AgOpenGPS.Properties.Resources.FileNew;
            this.btnAddToFile.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddToFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddToFile.Location = new System.Drawing.Point(320, 100);
            this.btnAddToFile.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddToFile.Name = "btnAddToFile";
            this.btnAddToFile.Size = new System.Drawing.Size(80, 80);
            this.btnAddToFile.TabIndex = 143;
            this.btnAddToFile.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddToFile.UseVisualStyleBackColor = false;
            this.btnAddToFile.Click += new System.EventHandler(this.BtnAddToFile_Click);
            // 
            // btnListDelete
            // 
            this.btnListDelete.BackColor = System.Drawing.SystemColors.Control;
            this.btnListDelete.Enabled = false;
            this.btnListDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnListDelete.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnListDelete.Image = global::AgOpenGPS.Properties.Resources.FileDelete;
            this.btnListDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnListDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnListDelete.Location = new System.Drawing.Point(320, 190);
            this.btnListDelete.Margin = new System.Windows.Forms.Padding(0);
            this.btnListDelete.Name = "btnListDelete";
            this.btnListDelete.Size = new System.Drawing.Size(80, 80);
            this.btnListDelete.TabIndex = 142;
            this.btnListDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnListDelete.UseVisualStyleBackColor = false;
            this.btnListDelete.Click += new System.EventHandler(this.BtnListDelete_Click);
            // 
            // btnAddAndGo
            // 
            this.btnAddAndGo.BackColor = System.Drawing.SystemColors.Control;
            this.btnAddAndGo.Enabled = false;
            this.btnAddAndGo.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddAndGo.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.btnAddAndGo.Image = global::AgOpenGPS.Properties.Resources.FileNewAndGo;
            this.btnAddAndGo.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnAddAndGo.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAddAndGo.Location = new System.Drawing.Point(320, 10);
            this.btnAddAndGo.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddAndGo.Name = "btnAddAndGo";
            this.btnAddAndGo.Size = new System.Drawing.Size(80, 80);
            this.btnAddAndGo.TabIndex = 149;
            this.btnAddAndGo.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnAddAndGo.UseVisualStyleBackColor = false;
            this.btnAddAndGo.Click += new System.EventHandler(this.BtnAddToFile_Click);
            // 
            // btnCancelMain
            // 
            this.btnCancelMain.BackColor = System.Drawing.Color.Transparent;
            this.btnCancelMain.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancelMain.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancelMain.FlatAppearance.BorderSize = 0;
            this.btnCancelMain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancelMain.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancelMain.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancelMain.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancelMain.Location = new System.Drawing.Point(120, 280);
            this.btnCancelMain.Name = "btnCancelMain";
            this.btnCancelMain.Size = new System.Drawing.Size(80, 80);
            this.btnCancelMain.TabIndex = 422;
            this.btnCancelMain.UseVisualStyleBackColor = false;
            this.btnCancelMain.Click += new System.EventHandler(this.BtnCancelMain_Click);
            // 
            // btnCancel2
            // 
            this.btnCancel2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnCancel2.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnCancel2.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel2.FlatAppearance.BorderSize = 0;
            this.btnCancel2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel2.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnCancel2.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnCancel2.Location = new System.Drawing.Point(15, 280);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(80, 80);
            this.btnCancel2.TabIndex = 423;
            this.btnCancel2.UseVisualStyleBackColor = false;
            this.btnCancel2.Click += new System.EventHandler(this.BtnCancelMain_Click);
            // 
            // SelectBox
            // 
            this.SelectBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SelectBox.Controls.Add(this.NameBox);
            this.SelectBox.Controls.Add(this.lvLines);
            this.SelectBox.Controls.Add(this.btnAddAndGo);
            this.SelectBox.Controls.Add(this.btnNewLine);
            this.SelectBox.Controls.Add(this.btnCancelMain);
            this.SelectBox.Controls.Add(this.btnTurnOff);
            this.SelectBox.Controls.Add(this.btnAddToFile);
            this.SelectBox.Controls.Add(this.btnListUse);
            this.SelectBox.Controls.Add(this.btnListDelete);
            this.SelectBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SelectBox.Location = new System.Drawing.Point(0, 0);
            this.SelectBox.Margin = new System.Windows.Forms.Padding(0);
            this.SelectBox.Name = "SelectBox";
            this.SelectBox.Padding = new System.Windows.Forms.Padding(0);
            this.SelectBox.Size = new System.Drawing.Size(410, 370);
            this.SelectBox.TabIndex = 424;
            this.SelectBox.TabStop = false;
            // 
            // btnNewLine
            // 
            this.btnNewLine.BackColor = System.Drawing.Color.Transparent;
            this.btnNewLine.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.btnNewLine.FlatAppearance.BorderSize = 0;
            this.btnNewLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnNewLine.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnNewLine.Image = global::AgOpenGPS.Properties.Resources.AddNew;
            this.btnNewLine.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnNewLine.Location = new System.Drawing.Point(230, 280);
            this.btnNewLine.Name = "btnNewLine";
            this.btnNewLine.Size = new System.Drawing.Size(80, 80);
            this.btnNewLine.TabIndex = 150;
            this.btnNewLine.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnNewLine.UseVisualStyleBackColor = false;
            this.btnNewLine.Click += new System.EventHandler(this.BtnNewLine_Click);
            // 
            // ABCurveBox
            // 
            this.ABCurveBox.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ABCurveBox.Controls.Add(this.lblKeepGoing);
            this.ABCurveBox.Controls.Add(this.lblFixHeading);
            this.ABCurveBox.Controls.Add(this.Status);
            this.ABCurveBox.Controls.Add(this.lblCurveExists);
            this.ABCurveBox.Controls.Add(this.tboxHeading);
            this.ABCurveBox.Controls.Add(this.btnCancel2);
            this.ABCurveBox.Controls.Add(this.btnAPoint);
            this.ABCurveBox.Controls.Add(this.btnPausePlay);
            this.ABCurveBox.Controls.Add(this.btnBPoint);
            this.ABCurveBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.ABCurveBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ABCurveBox.Location = new System.Drawing.Point(410, 0);
            this.ABCurveBox.Name = "ABCurveBox";
            this.ABCurveBox.Size = new System.Drawing.Size(240, 370);
            this.ABCurveBox.TabIndex = 425;
            this.ABCurveBox.TabStop = false;
            // 
            // lblKeepGoing
            // 
            this.lblKeepGoing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblKeepGoing.AutoSize = true;
            this.lblKeepGoing.BackColor = System.Drawing.Color.Transparent;
            this.lblKeepGoing.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.lblKeepGoing.Location = new System.Drawing.Point(190, 15);
            this.lblKeepGoing.Name = "lblKeepGoing";
            this.lblKeepGoing.Size = new System.Drawing.Size(22, 25);
            this.lblKeepGoing.TabIndex = 426;
            this.lblKeepGoing.Text = "?";
            // 
            // lblFixHeading
            // 
            this.lblFixHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFixHeading.AutoSize = true;
            this.lblFixHeading.BackColor = System.Drawing.Color.Transparent;
            this.lblFixHeading.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold);
            this.lblFixHeading.Location = new System.Drawing.Point(15, 15);
            this.lblFixHeading.Name = "lblFixHeading";
            this.lblFixHeading.Size = new System.Drawing.Size(32, 33);
            this.lblFixHeading.TabIndex = 425;
            this.lblFixHeading.Text = "0";
            // 
            // tboxHeading
            // 
            this.tboxHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tboxHeading.BackColor = System.Drawing.SystemColors.Control;
            this.tboxHeading.Enabled = false;
            this.tboxHeading.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxHeading.Location = new System.Drawing.Point(10, 190);
            this.tboxHeading.MaxLength = 10;
            this.tboxHeading.Name = "tboxHeading";
            this.tboxHeading.Size = new System.Drawing.Size(220, 50);
            this.tboxHeading.TabIndex = 424;
            this.tboxHeading.Text = "359.123456";
            this.tboxHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tboxHeading.Enter += new System.EventHandler(this.TboxHeading_Enter);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FormABCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(650, 370);
            this.ControlBox = false;
            this.Controls.Add(this.ABCurveBox);
            this.Controls.Add(this.SelectBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormABCurve";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AB Curve";
            this.Load += new System.EventHandler(this.FormABCurve_Load);
            this.SelectBox.ResumeLayout(false);
            this.SelectBox.PerformLayout();
            this.ABCurveBox.ResumeLayout(false);
            this.ABCurveBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnBPoint;
        private System.Windows.Forms.Button btnAPoint;
        private System.Windows.Forms.Button btnTurnOff;
        private System.Windows.Forms.Label lblCurveExists;
        private System.Windows.Forms.Button btnPausePlay;
        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ColumnHeader chField;
        private System.Windows.Forms.Button btnListDelete;
        private System.Windows.Forms.Button btnAddToFile;
        private System.Windows.Forms.Button btnListUse;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Label Status;
        private System.Windows.Forms.Button btnAddAndGo;
        private System.Windows.Forms.Button btnCancelMain;
        private System.Windows.Forms.Button btnCancel2;
        private System.Windows.Forms.GroupBox SelectBox;
        private System.Windows.Forms.GroupBox ABCurveBox;
        private System.Windows.Forms.TextBox tboxHeading;
        private System.Windows.Forms.Label lblFixHeading;
        private System.Windows.Forms.Label lblKeepGoing;
        private System.Windows.Forms.Button btnNewLine;
        private System.Windows.Forms.Timer timer1;
    }
}
