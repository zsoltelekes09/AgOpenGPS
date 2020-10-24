namespace AgOpenGPS
{
    partial class FormColor
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
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.bntOK = new System.Windows.Forms.Button();
            this.btnFrameDay = new System.Windows.Forms.Button();
            this.btnFrameNight = new System.Windows.Forms.Button();
            this.btnFieldNight = new System.Windows.Forms.Button();
            this.btnFieldDay = new System.Windows.Forms.Button();
            this.btnSwap = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 500;
            // 
            // bntOK
            // 
            this.bntOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bntOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.bntOK.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.bntOK.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.bntOK.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.bntOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.bntOK.Location = new System.Drawing.Point(327, 319);
            this.bntOK.Name = "bntOK";
            this.bntOK.Size = new System.Drawing.Size(161, 67);
            this.bntOK.TabIndex = 0;
            this.bntOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.bntOK.UseVisualStyleBackColor = true;
            this.bntOK.Click += new System.EventHandler(this.BntOK_Click);
            // 
            // btnFrameDay
            // 
            this.btnFrameDay.BackColor = System.Drawing.Color.Transparent;
            this.btnFrameDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFrameDay.FlatAppearance.BorderSize = 2;
            this.btnFrameDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFrameDay.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFrameDay.Image = global::AgOpenGPS.Properties.Resources.WindowDayMode;
            this.btnFrameDay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFrameDay.Location = new System.Drawing.Point(420, 63);
            this.btnFrameDay.Name = "btnFrameDay";
            this.btnFrameDay.Size = new System.Drawing.Size(68, 57);
            this.btnFrameDay.TabIndex = 4;
            this.btnFrameDay.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnFrameDay.UseVisualStyleBackColor = false;
            this.btnFrameDay.Click += new System.EventHandler(this.BtnFrameDay_Click);
            // 
            // btnFrameNight
            // 
            this.btnFrameNight.BackColor = System.Drawing.Color.Transparent;
            this.btnFrameNight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFrameNight.FlatAppearance.BorderSize = 2;
            this.btnFrameNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFrameNight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFrameNight.Image = global::AgOpenGPS.Properties.Resources.WindowNightMode;
            this.btnFrameNight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFrameNight.Location = new System.Drawing.Point(420, 182);
            this.btnFrameNight.Name = "btnFrameNight";
            this.btnFrameNight.Size = new System.Drawing.Size(68, 57);
            this.btnFrameNight.TabIndex = 5;
            this.btnFrameNight.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnFrameNight.UseVisualStyleBackColor = false;
            this.btnFrameNight.Click += new System.EventHandler(this.BtnFrameNight_Click);
            // 
            // btnFieldNight
            // 
            this.btnFieldNight.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldNight.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFieldNight.FlatAppearance.BorderSize = 2;
            this.btnFieldNight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldNight.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFieldNight.Image = global::AgOpenGPS.Properties.Resources.WindowNightMode;
            this.btnFieldNight.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFieldNight.Location = new System.Drawing.Point(222, 191);
            this.btnFieldNight.Name = "btnFieldNight";
            this.btnFieldNight.Size = new System.Drawing.Size(68, 57);
            this.btnFieldNight.TabIndex = 7;
            this.btnFieldNight.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnFieldNight.UseVisualStyleBackColor = false;
            this.btnFieldNight.Click += new System.EventHandler(this.BtnFieldNight_Click);
            // 
            // btnFieldDay
            // 
            this.btnFieldDay.BackColor = System.Drawing.Color.Transparent;
            this.btnFieldDay.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnFieldDay.FlatAppearance.BorderSize = 2;
            this.btnFieldDay.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFieldDay.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnFieldDay.Image = global::AgOpenGPS.Properties.Resources.WindowDayMode;
            this.btnFieldDay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnFieldDay.Location = new System.Drawing.Point(293, 110);
            this.btnFieldDay.Name = "btnFieldDay";
            this.btnFieldDay.Size = new System.Drawing.Size(68, 57);
            this.btnFieldDay.TabIndex = 6;
            this.btnFieldDay.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnFieldDay.UseVisualStyleBackColor = false;
            this.btnFieldDay.Click += new System.EventHandler(this.BtnFieldDay_Click);
            // 
            // btnSwap
            // 
            this.btnSwap.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSwap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSwap.Font = new System.Drawing.Font("Tahoma", 14.25F);
            this.btnSwap.Image = global::AgOpenGPS.Properties.Resources.Reload;
            this.btnSwap.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.btnSwap.Location = new System.Drawing.Point(1, 319);
            this.btnSwap.Name = "btnSwap";
            this.btnSwap.Size = new System.Drawing.Size(161, 67);
            this.btnSwap.TabIndex = 10;
            this.btnSwap.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSwap.UseVisualStyleBackColor = true;
            this.btnSwap.Click += new System.EventHandler(this.BtnSwap_Click);
            // 
            // FormColor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.BackgroundImage = global::AgOpenGPS.Properties.Resources.ColorBackGnd;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(492, 387);
            this.ControlBox = false;
            this.Controls.Add(this.btnSwap);
            this.Controls.Add(this.btnFieldNight);
            this.Controls.Add(this.btnFieldDay);
            this.Controls.Add(this.btnFrameNight);
            this.Controls.Add(this.btnFrameDay);
            this.Controls.Add(this.bntOK);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormColor";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Module Configure";
            this.Load += new System.EventHandler(this.FormDisplaySettings_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button bntOK;
        private System.Windows.Forms.Button btnFrameDay;
        private System.Windows.Forms.Button btnFrameNight;
        private System.Windows.Forms.Button btnFieldNight;
        private System.Windows.Forms.Button btnFieldDay;
        private System.Windows.Forms.Button btnSwap;
    }
}