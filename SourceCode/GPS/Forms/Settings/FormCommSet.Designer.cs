namespace AgOpenGPS
{
    partial class FormCommSet
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
            this.btnRescan = new System.Windows.Forms.Button();
            this.btnSerialOK = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cboxPort = new System.Windows.Forms.ComboBox();
            this.cboxBaud = new System.Windows.Forms.ComboBox();
            this.lblCurrentPort = new System.Windows.Forms.Label();
            this.lblCurrentBaud = new System.Windows.Forms.Label();
            this.btnCloseSerial = new System.Windows.Forms.Button();
            this.textBoxRcv = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnOpenSerial = new System.Windows.Forms.Button();
            this.btnCloseSerialArduino = new System.Windows.Forms.Button();
            this.btnOpenSerialArduino = new System.Windows.Forms.Button();
            this.lblCurrentArduinoPort = new System.Windows.Forms.Label();
            this.cboxArdPort = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cboxASPort = new System.Windows.Forms.ComboBox();
            this.lblCurrentAutoSteerPort = new System.Windows.Forms.Label();
            this.btnOpenSerialAutoSteer = new System.Windows.Forms.Button();
            this.btnCloseSerialAutoSteer = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // btnRescan
            // 
            this.btnRescan.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnRescan.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnRescan.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnRescan.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnRescan.Location = new System.Drawing.Point(780, 230);
            this.btnRescan.Name = "btnRescan";
            this.btnRescan.Size = new System.Drawing.Size(150, 80);
            this.btnRescan.TabIndex = 58;
            this.btnRescan.Text = "Rescan Ports";
            this.btnRescan.UseVisualStyleBackColor = false;
            this.btnRescan.Click += new System.EventHandler(this.BtnRescan_Click);
            // 
            // btnSerialOK
            // 
            this.btnSerialOK.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnSerialOK.Font = new System.Drawing.Font("Tahoma", 12F);
            this.btnSerialOK.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnSerialOK.Image = global::AgOpenGPS.Properties.Resources.OK64;
            this.btnSerialOK.Location = new System.Drawing.Point(780, 330);
            this.btnSerialOK.Name = "btnSerialOK";
            this.btnSerialOK.Size = new System.Drawing.Size(150, 80);
            this.btnSerialOK.TabIndex = 59;
            this.btnSerialOK.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnSerialOK.UseVisualStyleBackColor = true;
            this.btnSerialOK.Click += new System.EventHandler(this.BtnSerialOK_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.cboxPort);
            this.groupBox1.Controls.Add(this.cboxBaud);
            this.groupBox1.Controls.Add(this.lblCurrentPort);
            this.groupBox1.Controls.Add(this.lblCurrentBaud);
            this.groupBox1.Controls.Add(this.btnCloseSerial);
            this.groupBox1.Controls.Add(this.textBoxRcv);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.btnOpenSerial);
            this.groupBox1.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox1.Location = new System.Drawing.Point(10, 10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(760, 200);
            this.groupBox1.TabIndex = 64;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "GPS Port";
            // 
            // cboxPort
            // 
            this.cboxPort.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxPort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxPort.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxPort.FormattingEnabled = true;
            this.cboxPort.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboxPort.Location = new System.Drawing.Point(10, 37);
            this.cboxPort.Name = "cboxPort";
            this.cboxPort.Size = new System.Drawing.Size(169, 37);
            this.cboxPort.TabIndex = 50;
            this.cboxPort.SelectedIndexChanged += new System.EventHandler(this.CboxPort_SelectedIndexChanged_1);
            // 
            // cboxBaud
            // 
            this.cboxBaud.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.cboxBaud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxBaud.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxBaud.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.cboxBaud.FormattingEnabled = true;
            this.cboxBaud.Items.AddRange(new object[] {
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.cboxBaud.Location = new System.Drawing.Point(198, 37);
            this.cboxBaud.Name = "cboxBaud";
            this.cboxBaud.Size = new System.Drawing.Size(127, 37);
            this.cboxBaud.TabIndex = 49;
            this.cboxBaud.SelectedIndexChanged += new System.EventHandler(this.CboxBaud_SelectedIndexChanged_1);
            // 
            // lblCurrentPort
            // 
            this.lblCurrentPort.AutoSize = true;
            this.lblCurrentPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentPort.Location = new System.Drawing.Point(71, 78);
            this.lblCurrentPort.Name = "lblCurrentPort";
            this.lblCurrentPort.Size = new System.Drawing.Size(40, 18);
            this.lblCurrentPort.TabIndex = 47;
            this.lblCurrentPort.Text = "Port";
            // 
            // lblCurrentBaud
            // 
            this.lblCurrentBaud.AutoSize = true;
            this.lblCurrentBaud.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentBaud.Location = new System.Drawing.Point(241, 78);
            this.lblCurrentBaud.Name = "lblCurrentBaud";
            this.lblCurrentBaud.Size = new System.Drawing.Size(45, 18);
            this.lblCurrentBaud.TabIndex = 46;
            this.lblCurrentBaud.Text = "Baud";
            // 
            // btnCloseSerial
            // 
            this.btnCloseSerial.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCloseSerial.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCloseSerial.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerial.Location = new System.Drawing.Point(608, 35);
            this.btnCloseSerial.Name = "btnCloseSerial";
            this.btnCloseSerial.Size = new System.Drawing.Size(138, 40);
            this.btnCloseSerial.TabIndex = 44;
            this.btnCloseSerial.Text = "Disconnect";
            this.btnCloseSerial.UseVisualStyleBackColor = false;
            this.btnCloseSerial.Click += new System.EventHandler(this.BtnCloseSerial_Click);
            // 
            // textBoxRcv
            // 
            this.textBoxRcv.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxRcv.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxRcv.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.textBoxRcv.Location = new System.Drawing.Point(3, 117);
            this.textBoxRcv.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxRcv.Multiline = true;
            this.textBoxRcv.Name = "textBoxRcv";
            this.textBoxRcv.ReadOnly = true;
            this.textBoxRcv.Size = new System.Drawing.Size(736, 75);
            this.textBoxRcv.TabIndex = 40;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(450, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(290, 19);
            this.label1.TabIndex = 41;
            this.label1.Text = "NMEA String";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // btnOpenSerial
            // 
            this.btnOpenSerial.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnOpenSerial.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpenSerial.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerial.Location = new System.Drawing.Point(458, 35);
            this.btnOpenSerial.Name = "btnOpenSerial";
            this.btnOpenSerial.Size = new System.Drawing.Size(138, 40);
            this.btnOpenSerial.TabIndex = 45;
            this.btnOpenSerial.Text = "Connect";
            this.btnOpenSerial.UseVisualStyleBackColor = false;
            this.btnOpenSerial.Click += new System.EventHandler(this.BtnOpenSerial_Click);
            // 
            // btnCloseSerialArduino
            // 
            this.btnCloseSerialArduino.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCloseSerialArduino.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCloseSerialArduino.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerialArduino.Location = new System.Drawing.Point(568, 24);
            this.btnCloseSerialArduino.Name = "btnCloseSerialArduino";
            this.btnCloseSerialArduino.Size = new System.Drawing.Size(138, 40);
            this.btnCloseSerialArduino.TabIndex = 52;
            this.btnCloseSerialArduino.Text = "Disconnect";
            this.btnCloseSerialArduino.UseVisualStyleBackColor = false;
            this.btnCloseSerialArduino.Click += new System.EventHandler(this.BtnCloseSerialArduino_Click);
            // 
            // btnOpenSerialArduino
            // 
            this.btnOpenSerialArduino.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnOpenSerialArduino.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpenSerialArduino.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerialArduino.Location = new System.Drawing.Point(372, 24);
            this.btnOpenSerialArduino.Name = "btnOpenSerialArduino";
            this.btnOpenSerialArduino.Size = new System.Drawing.Size(138, 40);
            this.btnOpenSerialArduino.TabIndex = 53;
            this.btnOpenSerialArduino.Text = "Connect";
            this.btnOpenSerialArduino.UseVisualStyleBackColor = false;
            this.btnOpenSerialArduino.Click += new System.EventHandler(this.BtnOpenSerialArduino_Click);
            // 
            // lblCurrentArduinoPort
            // 
            this.lblCurrentArduinoPort.AutoSize = true;
            this.lblCurrentArduinoPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentArduinoPort.Location = new System.Drawing.Point(156, 40);
            this.lblCurrentArduinoPort.Name = "lblCurrentArduinoPort";
            this.lblCurrentArduinoPort.Size = new System.Drawing.Size(40, 18);
            this.lblCurrentArduinoPort.TabIndex = 59;
            this.lblCurrentArduinoPort.Text = "Port";
            // 
            // cboxArdPort
            // 
            this.cboxArdPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxArdPort.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Bold);
            this.cboxArdPort.FormattingEnabled = true;
            this.cboxArdPort.Location = new System.Drawing.Point(25, 31);
            this.cboxArdPort.Name = "cboxArdPort";
            this.cboxArdPort.Size = new System.Drawing.Size(121, 37);
            this.cboxArdPort.TabIndex = 64;
            this.cboxArdPort.SelectedIndexChanged += new System.EventHandler(this.CboxArdPort_SelectedIndexChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox2.Controls.Add(this.cboxArdPort);
            this.groupBox2.Controls.Add(this.lblCurrentArduinoPort);
            this.groupBox2.Controls.Add(this.btnOpenSerialArduino);
            this.groupBox2.Controls.Add(this.btnCloseSerialArduino);
            this.groupBox2.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox2.Location = new System.Drawing.Point(10, 320);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(760, 90);
            this.groupBox2.TabIndex = 65;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Machine Port";
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox3.Controls.Add(this.cboxASPort);
            this.groupBox3.Controls.Add(this.lblCurrentAutoSteerPort);
            this.groupBox3.Controls.Add(this.btnOpenSerialAutoSteer);
            this.groupBox3.Controls.Add(this.btnCloseSerialAutoSteer);
            this.groupBox3.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.groupBox3.Location = new System.Drawing.Point(10, 220);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(760, 90);
            this.groupBox3.TabIndex = 66;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "AutoSteer Port";
            // 
            // cboxASPort
            // 
            this.cboxASPort.Cursor = System.Windows.Forms.Cursors.Default;
            this.cboxASPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboxASPort.Font = new System.Drawing.Font("Tahoma", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboxASPort.FormattingEnabled = true;
            this.cboxASPort.Location = new System.Drawing.Point(25, 31);
            this.cboxASPort.Name = "cboxASPort";
            this.cboxASPort.Size = new System.Drawing.Size(121, 41);
            this.cboxASPort.TabIndex = 64;
            this.cboxASPort.SelectedIndexChanged += new System.EventHandler(this.CboxASPort_SelectedIndexChanged);
            // 
            // lblCurrentAutoSteerPort
            // 
            this.lblCurrentAutoSteerPort.AutoSize = true;
            this.lblCurrentAutoSteerPort.Font = new System.Drawing.Font("Tahoma", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCurrentAutoSteerPort.Location = new System.Drawing.Point(156, 42);
            this.lblCurrentAutoSteerPort.Name = "lblCurrentAutoSteerPort";
            this.lblCurrentAutoSteerPort.Size = new System.Drawing.Size(40, 18);
            this.lblCurrentAutoSteerPort.TabIndex = 59;
            this.lblCurrentAutoSteerPort.Text = "Port";
            // 
            // btnOpenSerialAutoSteer
            // 
            this.btnOpenSerialAutoSteer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnOpenSerialAutoSteer.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnOpenSerialAutoSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOpenSerialAutoSteer.Location = new System.Drawing.Point(372, 24);
            this.btnOpenSerialAutoSteer.Name = "btnOpenSerialAutoSteer";
            this.btnOpenSerialAutoSteer.Size = new System.Drawing.Size(138, 40);
            this.btnOpenSerialAutoSteer.TabIndex = 53;
            this.btnOpenSerialAutoSteer.Text = "Connect";
            this.btnOpenSerialAutoSteer.UseVisualStyleBackColor = false;
            this.btnOpenSerialAutoSteer.Click += new System.EventHandler(this.BtnOpenSerialAutoSteer_Click);
            // 
            // btnCloseSerialAutoSteer
            // 
            this.btnCloseSerialAutoSteer.BackColor = System.Drawing.SystemColors.ControlLight;
            this.btnCloseSerialAutoSteer.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.btnCloseSerialAutoSteer.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCloseSerialAutoSteer.Location = new System.Drawing.Point(560, 24);
            this.btnCloseSerialAutoSteer.Name = "btnCloseSerialAutoSteer";
            this.btnCloseSerialAutoSteer.Size = new System.Drawing.Size(138, 40);
            this.btnCloseSerialAutoSteer.TabIndex = 52;
            this.btnCloseSerialAutoSteer.Text = "Disconnect";
            this.btnCloseSerialAutoSteer.UseVisualStyleBackColor = false;
            this.btnCloseSerialAutoSteer.Click += new System.EventHandler(this.BtnCloseSerialAutoSteer_Click);
            // 
            // FormCommSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(940, 418);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnRescan);
            this.Controls.Add(this.btnSerialOK);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FormCommSet";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Communication Settings";
            this.Load += new System.EventHandler(this.FormCommSet_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Button btnRescan;
        private System.Windows.Forms.Button btnSerialOK;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label lblCurrentPort;
        private System.Windows.Forms.Label lblCurrentBaud;
        private System.Windows.Forms.Button btnCloseSerial;
        private System.Windows.Forms.TextBox textBoxRcv;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnOpenSerial;
        private System.Windows.Forms.ComboBox cboxBaud;
        private System.Windows.Forms.ComboBox cboxPort;
        private System.Windows.Forms.Button btnCloseSerialArduino;
        private System.Windows.Forms.Button btnOpenSerialArduino;
        private System.Windows.Forms.Label lblCurrentArduinoPort;
        private System.Windows.Forms.ComboBox cboxArdPort;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.ComboBox cboxASPort;
        private System.Windows.Forms.Label lblCurrentAutoSteerPort;
        private System.Windows.Forms.Button btnOpenSerialAutoSteer;
        private System.Windows.Forms.Button btnCloseSerialAutoSteer;
    }
}