namespace AgOpenGPS
{
    partial class FormPicker
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
            this.CboxTool = new System.Windows.Forms.ComboBox();
            this.lblLast = new System.Windows.Forms.Label();
            this.TboxName = new System.Windows.Forms.TextBox();
            this.BtnOk = new System.Windows.Forms.Button();
            this.TextName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CboxTool
            // 
            this.CboxTool.BackColor = System.Drawing.SystemColors.ControlLight;
            this.CboxTool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CboxTool.Font = new System.Drawing.Font("Tahoma", 21.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CboxTool.ForeColor = System.Drawing.SystemColors.ControlText;
            this.CboxTool.FormattingEnabled = true;
            this.CboxTool.ItemHeight = 35;
            this.CboxTool.Location = new System.Drawing.Point(10, 80);
            this.CboxTool.Name = "CboxTool";
            this.CboxTool.Size = new System.Drawing.Size(330, 43);
            this.CboxTool.TabIndex = 212;
            this.CboxTool.SelectedIndexChanged += new System.EventHandler(this.CboxVeh_SelectedIndexChanged);
            // 
            // lblLast
            // 
            this.lblLast.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLast.Location = new System.Drawing.Point(10, 10);
            this.lblLast.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.lblLast.Name = "lblLast";
            this.lblLast.Size = new System.Drawing.Size(330, 30);
            this.lblLast.TabIndex = 213;
            this.lblLast.Text = "Tool";
            this.lblLast.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // TboxName
            // 
            this.TboxName.BackColor = System.Drawing.SystemColors.ControlLight;
            this.TboxName.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TboxName.ForeColor = System.Drawing.SystemColors.ControlText;
            this.TboxName.Location = new System.Drawing.Point(10, 170);
            this.TboxName.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.TboxName.Name = "TboxName";
            this.TboxName.Size = new System.Drawing.Size(330, 36);
            this.TboxName.TabIndex = 214;
            this.TboxName.Click += new System.EventHandler(this.TboxName_Click);
            this.TboxName.TextChanged += new System.EventHandler(this.TboxName_TextChanged);
            // 
            // BtnOk
            // 
            this.BtnOk.BackColor = System.Drawing.SystemColors.ControlLight;
            this.BtnOk.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.BtnOk.Image = global::AgOpenGPS.Properties.Resources.FileSave;
            this.BtnOk.Location = new System.Drawing.Point(180, 230);
            this.BtnOk.Name = "BtnOk";
            this.BtnOk.Size = new System.Drawing.Size(160, 80);
            this.BtnOk.TabIndex = 215;
            this.BtnOk.UseVisualStyleBackColor = false;
            this.BtnOk.Click += new System.EventHandler(this.BtnOk_Click);
            // 
            // TextName
            // 
            this.TextName.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TextName.Location = new System.Drawing.Point(10, 140);
            this.TextName.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.TextName.Name = "TextName";
            this.TextName.Size = new System.Drawing.Size(330, 30);
            this.TextName.TabIndex = 216;
            this.TextName.Text = "Enter New Name:";
            this.TextName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(10, 50);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(330, 30);
            this.label2.TabIndex = 217;
            this.label2.Text = "Save As:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = new System.Drawing.Font("Tahoma", 15.75F);
            this.button1.Image = global::AgOpenGPS.Properties.Resources.Cancel64;
            this.button1.Location = new System.Drawing.Point(10, 230);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(160, 80);
            this.button1.TabIndex = 219;
            this.button1.UseVisualStyleBackColor = false;
            // 
            // FormPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(350, 320);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.TextName);
            this.Controls.Add(this.BtnOk);
            this.Controls.Add(this.TboxName);
            this.Controls.Add(this.lblLast);
            this.Controls.Add(this.CboxTool);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormPicker";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Load Tool";
            this.Load += new System.EventHandler(this.FormToolSaver_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox CboxTool;
        private System.Windows.Forms.Label lblLast;
        private System.Windows.Forms.TextBox TboxName;
        private System.Windows.Forms.Button BtnOk;
        private System.Windows.Forms.Label TextName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}