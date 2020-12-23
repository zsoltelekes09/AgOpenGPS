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
            this.lvLines = new System.Windows.Forms.ListView();
            this.chField = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.NameBox = new System.Windows.Forms.TextBox();
            this.btnPausePlay = new System.Windows.Forms.Button();
            this.btnTurnOff = new System.Windows.Forms.Button();
            this.BtnBPoint = new System.Windows.Forms.Button();
            this.BtnAPoint = new System.Windows.Forms.Button();
            this.BtnListUse = new System.Windows.Forms.Button();
            this.BtnListDelete = new System.Windows.Forms.Button();
            this.btnCancelMain = new System.Windows.Forms.Button();
            this.btnCancel2 = new System.Windows.Forms.Button();
            this.SelectPanel = new System.Windows.Forms.Panel();
            this.BtnBoundary = new System.Windows.Forms.Button();
            this.BtnCurve = new System.Windows.Forms.Button();
            this.BtnAB = new System.Windows.Forms.Button();
            this.DrivePanel = new System.Windows.Forms.Panel();
            this.TboxOffset = new System.Windows.Forms.TextBox();
            this.BtnNext = new System.Windows.Forms.Button();
            this.TboxHeading = new System.Windows.Forms.TextBox();
            this.NamePanel = new System.Windows.Forms.Panel();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.SelectPanel.SuspendLayout();
            this.DrivePanel.SuspendLayout();
            this.NamePanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvLines
            // 
            this.lvLines.BackColor = System.Drawing.SystemColors.Control;
            this.lvLines.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lvLines.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chField});
            this.lvLines.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lvLines.ForeColor = System.Drawing.SystemColors.ControlText;
            this.lvLines.FullRowSelect = true;
            this.lvLines.GridLines = true;
            this.lvLines.HideSelection = false;
            this.lvLines.ImeMode = System.Windows.Forms.ImeMode.Off;
            this.lvLines.LabelWrap = false;
            this.lvLines.Location = new System.Drawing.Point(10, 10);
            this.lvLines.Margin = new System.Windows.Forms.Padding(0);
            this.lvLines.MultiSelect = false;
            this.lvLines.Name = "lvLines";
            this.lvLines.Size = new System.Drawing.Size(300, 260);
            this.lvLines.TabIndex = 141;
            this.lvLines.TabStop = false;
            this.lvLines.TileSize = new System.Drawing.Size(300, 35);
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
            this.NameBox.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NameBox.ForeColor = System.Drawing.SystemColors.ControlText;
            this.NameBox.Location = new System.Drawing.Point(10, 100);
            this.NameBox.Margin = new System.Windows.Forms.Padding(0);
            this.NameBox.MaxLength = 100;
            this.NameBox.Multiline = true;
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(220, 170);
            this.NameBox.TabIndex = 145;
            this.NameBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.NameBox.Enter += new System.EventHandler(this.TextBox1_Enter);
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
            this.btnPausePlay.Location = new System.Drawing.Point(150, 190);
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
            // BtnBPoint
            // 
            this.BtnBPoint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnBPoint.BackColor = System.Drawing.SystemColors.Control;
            this.BtnBPoint.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.BtnBPoint.Enabled = false;
            this.BtnBPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBPoint.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnBPoint.Image = global::AgOpenGPS.Properties.Resources.LetterBBlue;
            this.BtnBPoint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnBPoint.Location = new System.Drawing.Point(150, 100);
            this.BtnBPoint.Name = "BtnBPoint";
            this.BtnBPoint.Size = new System.Drawing.Size(80, 80);
            this.BtnBPoint.TabIndex = 64;
            this.BtnBPoint.UseVisualStyleBackColor = false;
            this.BtnBPoint.Click += new System.EventHandler(this.BtnBPoint_Click);
            // 
            // BtnAPoint
            // 
            this.BtnAPoint.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnAPoint.BackColor = System.Drawing.SystemColors.Control;
            this.BtnAPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAPoint.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnAPoint.Image = global::AgOpenGPS.Properties.Resources.LetterABlue;
            this.BtnAPoint.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnAPoint.Location = new System.Drawing.Point(10, 100);
            this.BtnAPoint.Name = "BtnAPoint";
            this.BtnAPoint.Size = new System.Drawing.Size(80, 80);
            this.BtnAPoint.TabIndex = 63;
            this.BtnAPoint.UseVisualStyleBackColor = false;
            this.BtnAPoint.Click += new System.EventHandler(this.BtnAPoint_Click);
            // 
            // BtnListUse
            // 
            this.BtnListUse.BackColor = System.Drawing.SystemColors.Control;
            this.BtnListUse.Enabled = false;
            this.BtnListUse.FlatAppearance.BorderColor = System.Drawing.Color.Black;
            this.BtnListUse.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnListUse.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.BtnListUse.Image = global::AgOpenGPS.Properties.Resources.FileLoad;
            this.BtnListUse.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnListUse.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnListUse.Location = new System.Drawing.Point(320, 280);
            this.BtnListUse.Margin = new System.Windows.Forms.Padding(0);
            this.BtnListUse.Name = "BtnListUse";
            this.BtnListUse.Size = new System.Drawing.Size(80, 80);
            this.BtnListUse.TabIndex = 144;
            this.BtnListUse.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnListUse.UseVisualStyleBackColor = false;
            this.BtnListUse.Click += new System.EventHandler(this.BtnListUse_Click);
            // 
            // BtnListDelete
            // 
            this.BtnListDelete.BackColor = System.Drawing.SystemColors.Control;
            this.BtnListDelete.Enabled = false;
            this.BtnListDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnListDelete.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold);
            this.BtnListDelete.Image = global::AgOpenGPS.Properties.Resources.FileDelete;
            this.BtnListDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.BtnListDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnListDelete.Location = new System.Drawing.Point(230, 280);
            this.BtnListDelete.Margin = new System.Windows.Forms.Padding(0);
            this.BtnListDelete.Name = "BtnListDelete";
            this.BtnListDelete.Size = new System.Drawing.Size(80, 80);
            this.BtnListDelete.TabIndex = 142;
            this.BtnListDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.BtnListDelete.UseVisualStyleBackColor = false;
            this.BtnListDelete.Click += new System.EventHandler(this.BtnListDelete_Click);
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
            this.btnCancelMain.Location = new System.Drawing.Point(100, 280);
            this.btnCancelMain.Name = "btnCancelMain";
            this.btnCancelMain.Size = new System.Drawing.Size(80, 80);
            this.btnCancelMain.TabIndex = 422;
            this.btnCancelMain.UseVisualStyleBackColor = false;
            this.btnCancelMain.Click += new System.EventHandler(this.BtnCancelMain_Click_1);
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
            this.btnCancel2.Location = new System.Drawing.Point(10, 280);
            this.btnCancel2.Name = "btnCancel2";
            this.btnCancel2.Size = new System.Drawing.Size(80, 80);
            this.btnCancel2.TabIndex = 423;
            this.btnCancel2.UseVisualStyleBackColor = false;
            this.btnCancel2.Click += new System.EventHandler(this.BtnCancelMain_Click);
            // 
            // SelectPanel
            // 
            this.SelectPanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.SelectPanel.Controls.Add(this.BtnBoundary);
            this.SelectPanel.Controls.Add(this.BtnCurve);
            this.SelectPanel.Controls.Add(this.BtnAB);
            this.SelectPanel.Controls.Add(this.lvLines);
            this.SelectPanel.Controls.Add(this.btnCancelMain);
            this.SelectPanel.Controls.Add(this.btnTurnOff);
            this.SelectPanel.Controls.Add(this.BtnListUse);
            this.SelectPanel.Controls.Add(this.BtnListDelete);
            this.SelectPanel.Location = new System.Drawing.Point(0, 0);
            this.SelectPanel.Margin = new System.Windows.Forms.Padding(0);
            this.SelectPanel.Name = "SelectPanel";
            this.SelectPanel.Size = new System.Drawing.Size(410, 370);
            this.SelectPanel.TabIndex = 424;
            // 
            // BtnBoundary
            // 
            this.BtnBoundary.BackColor = System.Drawing.SystemColors.Control;
            this.BtnBoundary.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnBoundary.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnBoundary.Image = global::AgOpenGPS.Properties.Resources.GuidanceBoundary;
            this.BtnBoundary.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnBoundary.Location = new System.Drawing.Point(320, 190);
            this.BtnBoundary.Name = "BtnBoundary";
            this.BtnBoundary.Size = new System.Drawing.Size(80, 80);
            this.BtnBoundary.TabIndex = 424;
            this.BtnBoundary.UseVisualStyleBackColor = false;
            this.BtnBoundary.Click += new System.EventHandler(this.BtnBoundary_Click);
            // 
            // BtnCurve
            // 
            this.BtnCurve.BackColor = System.Drawing.SystemColors.Control;
            this.BtnCurve.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCurve.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnCurve.Image = global::AgOpenGPS.Properties.Resources.GuidanceCurve;
            this.BtnCurve.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnCurve.Location = new System.Drawing.Point(320, 100);
            this.BtnCurve.Name = "BtnCurve";
            this.BtnCurve.Size = new System.Drawing.Size(80, 80);
            this.BtnCurve.TabIndex = 0;
            this.BtnCurve.UseVisualStyleBackColor = false;
            this.BtnCurve.Click += new System.EventHandler(this.BtnCurve_Click);
            // 
            // BtnAB
            // 
            this.BtnAB.BackColor = System.Drawing.SystemColors.Control;
            this.BtnAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAB.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnAB.Image = global::AgOpenGPS.Properties.Resources.GuidanceAB;
            this.BtnAB.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnAB.Location = new System.Drawing.Point(320, 10);
            this.BtnAB.Name = "BtnAB";
            this.BtnAB.Size = new System.Drawing.Size(80, 80);
            this.BtnAB.TabIndex = 423;
            this.BtnAB.UseVisualStyleBackColor = false;
            this.BtnAB.Click += new System.EventHandler(this.BtnAB_Click);
            // 
            // DrivePanel
            // 
            this.DrivePanel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.DrivePanel.Controls.Add(this.TboxOffset);
            this.DrivePanel.Controls.Add(this.BtnNext);
            this.DrivePanel.Controls.Add(this.TboxHeading);
            this.DrivePanel.Controls.Add(this.btnCancel2);
            this.DrivePanel.Controls.Add(this.BtnAPoint);
            this.DrivePanel.Controls.Add(this.btnPausePlay);
            this.DrivePanel.Controls.Add(this.BtnBPoint);
            this.DrivePanel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.DrivePanel.Location = new System.Drawing.Point(410, 0);
            this.DrivePanel.Name = "DrivePanel";
            this.DrivePanel.Size = new System.Drawing.Size(240, 370);
            this.DrivePanel.TabIndex = 425;
            // 
            // TboxOffset
            // 
            this.TboxOffset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TboxOffset.BackColor = System.Drawing.SystemColors.Control;
            this.TboxOffset.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxOffset.Location = new System.Drawing.Point(10, 190);
            this.TboxOffset.MaxLength = 10;
            this.TboxOffset.Name = "TboxOffset";
            this.TboxOffset.Size = new System.Drawing.Size(220, 50);
            this.TboxOffset.TabIndex = 427;
            this.TboxOffset.Text = "359.123456";
            this.TboxOffset.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxOffset.Enter += new System.EventHandler(this.TboxOffset_Enter);
            // 
            // BtnNext
            // 
            this.BtnNext.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnNext.BackColor = System.Drawing.Color.Transparent;
            this.BtnNext.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnNext.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnNext.FlatAppearance.BorderSize = 0;
            this.BtnNext.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnNext.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnNext.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.BtnNext.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnNext.Location = new System.Drawing.Point(150, 280);
            this.BtnNext.Name = "BtnNext";
            this.BtnNext.Size = new System.Drawing.Size(80, 80);
            this.BtnNext.TabIndex = 426;
            this.BtnNext.UseVisualStyleBackColor = false;
            this.BtnNext.Click += new System.EventHandler(this.BtnNext_Click);
            // 
            // TboxHeading
            // 
            this.TboxHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TboxHeading.BackColor = System.Drawing.SystemColors.Control;
            this.TboxHeading.Enabled = false;
            this.TboxHeading.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxHeading.Location = new System.Drawing.Point(10, 190);
            this.TboxHeading.MaxLength = 10;
            this.TboxHeading.Name = "TboxHeading";
            this.TboxHeading.Size = new System.Drawing.Size(220, 50);
            this.TboxHeading.TabIndex = 424;
            this.TboxHeading.Text = "359.123456";
            this.TboxHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxHeading.Enter += new System.EventHandler(this.TboxHeading_Enter);
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.BtnAdd);
            this.NamePanel.Controls.Add(this.NameBox);
            this.NamePanel.Location = new System.Drawing.Point(650, 0);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(240, 370);
            this.NamePanel.TabIndex = 426;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnAdd.BackColor = System.Drawing.Color.Transparent;
            this.BtnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnAdd.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnAdd.FlatAppearance.BorderSize = 0;
            this.BtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAdd.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnAdd.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.BtnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnAdd.Location = new System.Drawing.Point(150, 280);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(80, 80);
            this.BtnAdd.TabIndex = 425;
            this.BtnAdd.UseVisualStyleBackColor = false;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
            // 
            // FormABCurve
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(890, 370);
            this.ControlBox = false;
            this.Controls.Add(this.NamePanel);
            this.Controls.Add(this.DrivePanel);
            this.Controls.Add(this.SelectPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormABCurve";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "AB Curve";
            this.Load += new System.EventHandler(this.FormABCurve_Load);
            this.SelectPanel.ResumeLayout(false);
            this.DrivePanel.ResumeLayout(false);
            this.DrivePanel.PerformLayout();
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BtnBPoint;
        private System.Windows.Forms.Button BtnAPoint;
        private System.Windows.Forms.Button btnTurnOff;
        private System.Windows.Forms.Button btnPausePlay;
        private System.Windows.Forms.ListView lvLines;
        private System.Windows.Forms.ColumnHeader chField;
        private System.Windows.Forms.Button BtnListDelete;
        private System.Windows.Forms.Button BtnListUse;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Button btnCancelMain;
        private System.Windows.Forms.Button btnCancel2;
        private System.Windows.Forms.Panel SelectPanel;
        private System.Windows.Forms.Panel DrivePanel;
        private System.Windows.Forms.TextBox TboxHeading;
        private System.Windows.Forms.Button BtnCurve;
        private System.Windows.Forms.Button BtnAB;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.Button BtnNext;
        private System.Windows.Forms.Button BtnBoundary;
        private System.Windows.Forms.TextBox TboxOffset;
    }
}
