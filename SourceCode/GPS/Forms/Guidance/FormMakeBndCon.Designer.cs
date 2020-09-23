namespace AgOpenGPS
{
    partial class FormMakeBndCon
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
            this.lblHz = new System.Windows.Forms.Label();
            this.btnOk = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.TboxPasses = new System.Windows.Forms.TextBox();
            this.TboxSpacing = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // lblHz
            // 
            this.lblHz.AutoSize = true;
            this.lblHz.BackColor = System.Drawing.Color.Transparent;
            this.lblHz.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.lblHz.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.lblHz.Location = new System.Drawing.Point(34, 38);
            this.lblHz.Name = "lblHz";
            this.lblHz.Size = new System.Drawing.Size(84, 25);
            this.lblHz.TabIndex = 250;
            this.lblHz.Text = "Pass #";
            // 
            // btnOk
            // 
            this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOk.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold);
            this.btnOk.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnOk.Location = new System.Drawing.Point(341, 361);
            this.btnOk.Margin = new System.Windows.Forms.Padding(5, 4, 5, 4);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(129, 80);
            this.btnOk.TabIndex = 1;
            this.btnOk.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Tahoma", 15.75F, System.Drawing.FontStyle.Bold);
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(34, 213);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(151, 25);
            this.label1.TabIndex = 253;
            this.label1.Text = "Spacing (cm)";
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnCancel.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.btnCancel.Location = new System.Drawing.Point(225, 360);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 3, 2, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(91, 81);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnCancel.Click += new System.EventHandler(this.BtnCancel_Click);
            // 
            // TboxPasses
            // 
            this.TboxPasses.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TboxPasses.BackColor = System.Drawing.SystemColors.Control;
            this.TboxPasses.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxPasses.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxPasses.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxPasses.Location = new System.Drawing.Point(37, 69);
            this.TboxPasses.MaxLength = 10;
            this.TboxPasses.Name = "TboxPasses";
            this.TboxPasses.Size = new System.Drawing.Size(120, 85);
            this.TboxPasses.TabIndex = 474;
            this.TboxPasses.Text = "88";
            this.TboxPasses.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxPasses.Enter += new System.EventHandler(this.TboxPasses_Enter);
            // 
            // TboxSpacing
            // 
            this.TboxSpacing.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TboxSpacing.BackColor = System.Drawing.SystemColors.Control;
            this.TboxSpacing.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.TboxSpacing.Font = new System.Drawing.Font("Tahoma", 48F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxSpacing.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxSpacing.Location = new System.Drawing.Point(30, 246);
            this.TboxSpacing.MaxLength = 10;
            this.TboxSpacing.Name = "TboxSpacing";
            this.TboxSpacing.Size = new System.Drawing.Size(120, 85);
            this.TboxSpacing.TabIndex = 475;
            this.TboxSpacing.Text = "88";
            this.TboxSpacing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.TboxSpacing.Enter += new System.EventHandler(this.TboxSpacing_Enter);
            // 
            // FormMakeBndCon
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::AgOpenGPS.Properties.Resources.MakeBoundaryContour;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(484, 461);
            this.ControlBox = false;
            this.Controls.Add(this.TboxSpacing);
            this.Controls.Add(this.TboxPasses);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.lblHz);
            this.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMakeBndCon";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Make Boundary Contour";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblHz;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox TboxPasses;
        private System.Windows.Forms.TextBox TboxSpacing;
    }
}