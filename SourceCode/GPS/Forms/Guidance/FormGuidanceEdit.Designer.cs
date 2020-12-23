namespace AgOpenGPS
{
    partial class FormEditAB
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
            this.label2 = new System.Windows.Forms.Label();
            this.tboxHeading = new System.Windows.Forms.TextBox();
            this.btnSwapAB = new System.Windows.Forms.Button();
            this.btnBPoint = new System.Windows.Forms.Button();
            this.BtnCancel = new System.Windows.Forms.Button();
            this.BtnSave = new System.Windows.Forms.Button();
            this.btnAdjLeft = new System.Windows.Forms.Button();
            this.btnAdjRight = new System.Windows.Forms.Button();
            this.btnContourPriority = new System.Windows.Forms.Button();
            this.btnLeftHalfWidth = new System.Windows.Forms.Button();
            this.btnRightHalfWidth = new System.Windows.Forms.Button();
            this.TboxSnapAdj = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblSmooth = new System.Windows.Forms.Label();
            this.BtnSaveAs = new System.Windows.Forms.Button();
            this.BtnDown = new System.Windows.Forms.Button();
            this.BtnUp = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.NamePanel = new System.Windows.Forms.Panel();
            this.BtnAdd = new System.Windows.Forms.Button();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.EditPanel = new System.Windows.Forms.Panel();
            this.NamePanel.SuspendLayout();
            this.EditPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 150);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 30);
            this.label2.TabIndex = 442;
            this.label2.Text = "Heading";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tboxHeading
            // 
            this.tboxHeading.BackColor = System.Drawing.SystemColors.Control;
            this.tboxHeading.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tboxHeading.ForeColor = System.Drawing.SystemColors.ControlText;
            this.tboxHeading.Location = new System.Drawing.Point(10, 190);
            this.tboxHeading.MaxLength = 10;
            this.tboxHeading.Name = "tboxHeading";
            this.tboxHeading.Size = new System.Drawing.Size(120, 40);
            this.tboxHeading.TabIndex = 4;
            this.tboxHeading.Text = "359.123456";
            this.tboxHeading.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tboxHeading.Enter += new System.EventHandler(this.TboxHeading_Enter);
            // 
            // btnSwapAB
            // 
            this.btnSwapAB.BackColor = System.Drawing.Color.Transparent;
            this.btnSwapAB.FlatAppearance.BorderSize = 0;
            this.btnSwapAB.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwapAB.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSwapAB.Image = global::AgOpenGPS.Properties.Resources.ABSwapPoints;
            this.btnSwapAB.Location = new System.Drawing.Point(320, 150);
            this.btnSwapAB.Name = "btnSwapAB";
            this.btnSwapAB.Size = new System.Drawing.Size(80, 80);
            this.btnSwapAB.TabIndex = 5;
            this.btnSwapAB.UseVisualStyleBackColor = false;
            this.btnSwapAB.Click += new System.EventHandler(this.BtnSwapAB_Click);
            // 
            // btnBPoint
            // 
            this.btnBPoint.BackColor = System.Drawing.SystemColors.Control;
            this.btnBPoint.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnBPoint.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBPoint.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnBPoint.Image = global::AgOpenGPS.Properties.Resources.LetterBBlue;
            this.btnBPoint.Location = new System.Drawing.Point(140, 150);
            this.btnBPoint.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnBPoint.Name = "btnBPoint";
            this.btnBPoint.Size = new System.Drawing.Size(80, 80);
            this.btnBPoint.TabIndex = 2;
            this.btnBPoint.UseVisualStyleBackColor = false;
            this.btnBPoint.Click += new System.EventHandler(this.BtnBPoint_Click);
            // 
            // BtnCancel
            // 
            this.BtnCancel.BackColor = System.Drawing.SystemColors.Control;
            this.BtnCancel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.BtnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnCancel.Image = global::AgOpenGPS.Properties.Resources.FileDontSave;
            this.BtnCancel.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnCancel.Location = new System.Drawing.Point(40, 425);
            this.BtnCancel.Name = "BtnCancel";
            this.BtnCancel.Size = new System.Drawing.Size(80, 80);
            this.BtnCancel.TabIndex = 0;
            this.BtnCancel.UseVisualStyleBackColor = false;
            this.BtnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // BtnSave
            // 
            this.BtnSave.BackColor = System.Drawing.SystemColors.Control;
            this.BtnSave.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnSave.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.BtnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSave.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnSave.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.BtnSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnSave.Location = new System.Drawing.Point(290, 425);
            this.BtnSave.Name = "BtnSave";
            this.BtnSave.Size = new System.Drawing.Size(80, 80);
            this.BtnSave.TabIndex = 1;
            this.BtnSave.UseVisualStyleBackColor = false;
            this.BtnSave.Click += new System.EventHandler(this.BntOk_Click);
            // 
            // btnAdjLeft
            // 
            this.btnAdjLeft.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdjLeft.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjLeft.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjLeft.Image = global::AgOpenGPS.Properties.Resources.SnapLeft;
            this.btnAdjLeft.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjLeft.Location = new System.Drawing.Point(10, 100);
            this.btnAdjLeft.Name = "btnAdjLeft";
            this.btnAdjLeft.Size = new System.Drawing.Size(80, 40);
            this.btnAdjLeft.TabIndex = 6;
            this.btnAdjLeft.UseVisualStyleBackColor = false;
            this.btnAdjLeft.Click += new System.EventHandler(this.BtnAdjLeft_Click);
            // 
            // btnAdjRight
            // 
            this.btnAdjRight.BackColor = System.Drawing.SystemColors.Control;
            this.btnAdjRight.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnAdjRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdjRight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnAdjRight.Image = global::AgOpenGPS.Properties.Resources.SnapRight;
            this.btnAdjRight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnAdjRight.Location = new System.Drawing.Point(320, 100);
            this.btnAdjRight.Name = "btnAdjRight";
            this.btnAdjRight.Size = new System.Drawing.Size(80, 40);
            this.btnAdjRight.TabIndex = 7;
            this.btnAdjRight.UseVisualStyleBackColor = false;
            this.btnAdjRight.Click += new System.EventHandler(this.BtnAdjRight_Click);
            // 
            // btnContourPriority
            // 
            this.btnContourPriority.BackColor = System.Drawing.Color.Transparent;
            this.btnContourPriority.FlatAppearance.BorderColor = System.Drawing.SystemColors.HotTrack;
            this.btnContourPriority.FlatAppearance.BorderSize = 0;
            this.btnContourPriority.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnContourPriority.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnContourPriority.Image = global::AgOpenGPS.Properties.Resources.Snap2;
            this.btnContourPriority.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnContourPriority.Location = new System.Drawing.Point(230, 150);
            this.btnContourPriority.Name = "btnContourPriority";
            this.btnContourPriority.Size = new System.Drawing.Size(80, 80);
            this.btnContourPriority.TabIndex = 3;
            this.btnContourPriority.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnContourPriority.UseVisualStyleBackColor = false;
            this.btnContourPriority.Click += new System.EventHandler(this.BtnContourPriority_Click);
            // 
            // btnLeftHalfWidth
            // 
            this.btnLeftHalfWidth.BackColor = System.Drawing.SystemColors.Control;
            this.btnLeftHalfWidth.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnLeftHalfWidth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeftHalfWidth.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnLeftHalfWidth.Image = global::AgOpenGPS.Properties.Resources.ArrowLeft;
            this.btnLeftHalfWidth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnLeftHalfWidth.Location = new System.Drawing.Point(10, 10);
            this.btnLeftHalfWidth.Name = "btnLeftHalfWidth";
            this.btnLeftHalfWidth.Size = new System.Drawing.Size(80, 80);
            this.btnLeftHalfWidth.TabIndex = 8;
            this.btnLeftHalfWidth.UseVisualStyleBackColor = false;
            this.btnLeftHalfWidth.Click += new System.EventHandler(this.BtnLeftHalfWidth_Click);
            // 
            // btnRightHalfWidth
            // 
            this.btnRightHalfWidth.BackColor = System.Drawing.SystemColors.Control;
            this.btnRightHalfWidth.FlatAppearance.BorderColor = System.Drawing.SystemColors.ControlText;
            this.btnRightHalfWidth.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRightHalfWidth.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnRightHalfWidth.Image = global::AgOpenGPS.Properties.Resources.ArrowRight;
            this.btnRightHalfWidth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnRightHalfWidth.Location = new System.Drawing.Point(320, 10);
            this.btnRightHalfWidth.Name = "btnRightHalfWidth";
            this.btnRightHalfWidth.Size = new System.Drawing.Size(80, 80);
            this.btnRightHalfWidth.TabIndex = 9;
            this.btnRightHalfWidth.UseVisualStyleBackColor = false;
            this.btnRightHalfWidth.Click += new System.EventHandler(this.BtnRightHalfWidth_Click);
            // 
            // TboxSnapAdj
            // 
            this.TboxSnapAdj.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.TboxSnapAdj.BackColor = System.Drawing.SystemColors.Control;
            this.TboxSnapAdj.Font = new System.Drawing.Font("Tahoma", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxSnapAdj.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxSnapAdj.Location = new System.Drawing.Point(100, 39);
            this.TboxSnapAdj.Name = "TboxSnapAdj";
            this.TboxSnapAdj.ShortcutsEnabled = false;
            this.TboxSnapAdj.Size = new System.Drawing.Size(210, 50);
            this.TboxSnapAdj.TabIndex = 464;
            this.TboxSnapAdj.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxSnapAdj.WordWrap = false;
            this.TboxSnapAdj.Enter += new System.EventHandler(this.TboxSnapAdj_Enter);
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(100, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(210, 28);
            this.label5.TabIndex = 465;
            this.label5.Text = "1/2 W (m)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(100, 100);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(210, 40);
            this.label1.TabIndex = 466;
            this.label1.Text = "10 (Cm)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel3.Location = new System.Drawing.Point(0, 410);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(410, 5);
            this.panel3.TabIndex = 467;
            // 
            // lblSmooth
            // 
            this.lblSmooth.BackColor = System.Drawing.Color.Transparent;
            this.lblSmooth.Font = new System.Drawing.Font("Tahoma", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSmooth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblSmooth.Location = new System.Drawing.Point(18, 352);
            this.lblSmooth.Name = "lblSmooth";
            this.lblSmooth.Size = new System.Drawing.Size(158, 58);
            this.lblSmooth.TabIndex = 473;
            this.lblSmooth.Text = "99";
            this.lblSmooth.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // BtnSaveAs
            // 
            this.BtnSaveAs.BackColor = System.Drawing.SystemColors.Control;
            this.BtnSaveAs.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnSaveAs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnSaveAs.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnSaveAs.Image = global::AgOpenGPS.Properties.Resources.FileSaveAs;
            this.BtnSaveAs.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnSaveAs.Location = new System.Drawing.Point(290, 277);
            this.BtnSaveAs.Name = "BtnSaveAs";
            this.BtnSaveAs.Size = new System.Drawing.Size(80, 80);
            this.BtnSaveAs.TabIndex = 472;
            this.BtnSaveAs.UseVisualStyleBackColor = false;
            this.BtnSaveAs.Click += new System.EventHandler(this.BtnSaveAs_Click);
            // 
            // BtnDown
            // 
            this.BtnDown.BackColor = System.Drawing.SystemColors.Control;
            this.BtnDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnDown.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnDown.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.BtnDown.Image = global::AgOpenGPS.Properties.Resources.DnArrow64;
            this.BtnDown.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnDown.Location = new System.Drawing.Point(18, 277);
            this.BtnDown.Name = "BtnDown";
            this.BtnDown.Size = new System.Drawing.Size(72, 72);
            this.BtnDown.TabIndex = 469;
            this.BtnDown.UseVisualStyleBackColor = false;
            this.BtnDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnDown_MouseDown);
            this.BtnDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseUp);
            // 
            // BtnUp
            // 
            this.BtnUp.BackColor = System.Drawing.SystemColors.Control;
            this.BtnUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnUp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnUp.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.BtnUp.Image = global::AgOpenGPS.Properties.Resources.UpArrow64;
            this.BtnUp.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnUp.Location = new System.Drawing.Point(104, 277);
            this.BtnUp.Name = "BtnUp";
            this.BtnUp.Size = new System.Drawing.Size(72, 72);
            this.BtnUp.TabIndex = 470;
            this.BtnUp.UseVisualStyleBackColor = false;
            this.BtnUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.BtnUp_MouseDown);
            this.BtnUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Btn_MouseUp);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.HotTrack;
            this.panel1.Location = new System.Drawing.Point(0, 255);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(410, 5);
            this.panel1.TabIndex = 474;
            // 
            // NamePanel
            // 
            this.NamePanel.Controls.Add(this.BtnAdd);
            this.NamePanel.Controls.Add(this.NameBox);
            this.NamePanel.Location = new System.Drawing.Point(410, 0);
            this.NamePanel.Name = "NamePanel";
            this.NamePanel.Size = new System.Drawing.Size(240, 359);
            this.NamePanel.TabIndex = 427;
            // 
            // BtnAdd
            // 
            this.BtnAdd.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.BtnAdd.BackColor = System.Drawing.Color.Transparent;
            this.BtnAdd.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.BtnAdd.FlatAppearance.BorderSize = 0;
            this.BtnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BtnAdd.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.BtnAdd.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.BtnAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.BtnAdd.Location = new System.Drawing.Point(150, 274);
            this.BtnAdd.Name = "BtnAdd";
            this.BtnAdd.Size = new System.Drawing.Size(80, 80);
            this.BtnAdd.TabIndex = 425;
            this.BtnAdd.UseVisualStyleBackColor = false;
            this.BtnAdd.Click += new System.EventHandler(this.BtnAdd_Click);
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
            this.NameBox.Enter += new System.EventHandler(this.NameBox_Enter);
            // 
            // EditPanel
            // 
            this.EditPanel.Controls.Add(this.btnLeftHalfWidth);
            this.EditPanel.Controls.Add(this.btnAdjRight);
            this.EditPanel.Controls.Add(this.panel1);
            this.EditPanel.Controls.Add(this.btnAdjLeft);
            this.EditPanel.Controls.Add(this.lblSmooth);
            this.EditPanel.Controls.Add(this.BtnSave);
            this.EditPanel.Controls.Add(this.BtnSaveAs);
            this.EditPanel.Controls.Add(this.BtnCancel);
            this.EditPanel.Controls.Add(this.BtnDown);
            this.EditPanel.Controls.Add(this.tboxHeading);
            this.EditPanel.Controls.Add(this.BtnUp);
            this.EditPanel.Controls.Add(this.btnBPoint);
            this.EditPanel.Controls.Add(this.panel3);
            this.EditPanel.Controls.Add(this.label2);
            this.EditPanel.Controls.Add(this.label1);
            this.EditPanel.Controls.Add(this.btnSwapAB);
            this.EditPanel.Controls.Add(this.label5);
            this.EditPanel.Controls.Add(this.btnContourPriority);
            this.EditPanel.Controls.Add(this.TboxSnapAdj);
            this.EditPanel.Controls.Add(this.btnRightHalfWidth);
            this.EditPanel.Location = new System.Drawing.Point(0, 0);
            this.EditPanel.Name = "EditPanel";
            this.EditPanel.Size = new System.Drawing.Size(410, 515);
            this.EditPanel.TabIndex = 475;
            // 
            // FormEditAB
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLight;
            this.ClientSize = new System.Drawing.Size(650, 515);
            this.ControlBox = false;
            this.Controls.Add(this.EditPanel);
            this.Controls.Add(this.NamePanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormEditAB";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Edit AB Line";
            this.NamePanel.ResumeLayout(false);
            this.NamePanel.PerformLayout();
            this.EditPanel.ResumeLayout(false);
            this.EditPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAdjRight;
        private System.Windows.Forms.Button btnAdjLeft;
        private System.Windows.Forms.Button BtnCancel;
        private System.Windows.Forms.Button BtnSave;
        private System.Windows.Forms.Button btnSwapAB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnBPoint;
        private System.Windows.Forms.TextBox tboxHeading;
        public System.Windows.Forms.Button btnContourPriority;
        private System.Windows.Forms.Button btnLeftHalfWidth;
        private System.Windows.Forms.Button btnRightHalfWidth;
        private System.Windows.Forms.TextBox TboxSnapAdj;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblSmooth;
        private System.Windows.Forms.Button BtnSaveAs;
        private System.Windows.Forms.Button BtnDown;
        private System.Windows.Forms.Button BtnUp;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel NamePanel;
        private System.Windows.Forms.Button BtnAdd;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.Panel EditPanel;
    }
}