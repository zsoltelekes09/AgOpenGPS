namespace AgOpenGPS
{
    partial class FormModules
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
            this.Recieved = new System.Windows.Forms.GroupBox();
            this.tBoxRecieved = new System.Windows.Forms.TextBox();
            this.Send = new System.Windows.Forms.GroupBox();
            this.tBoxSend = new System.Windows.Forms.TextBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.Recieved.SuspendLayout();
            this.Send.SuspendLayout();
            this.SuspendLayout();
            // 
            // Recieved
            // 
            this.Recieved.BackColor = System.Drawing.SystemColors.Control;
            this.Recieved.Controls.Add(this.tBoxRecieved);
            this.Recieved.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.Recieved.Location = new System.Drawing.Point(10, 375);
            this.Recieved.Name = "Recieved";
            this.Recieved.Size = new System.Drawing.Size(700, 355);
            this.Recieved.TabIndex = 70;
            this.Recieved.TabStop = false;
            this.Recieved.Text = "Recieved";
            // 
            // tBoxRecieved
            // 
            this.tBoxRecieved.BackColor = System.Drawing.SystemColors.Window;
            this.tBoxRecieved.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tBoxRecieved.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tBoxRecieved.Location = new System.Drawing.Point(3, 26);
            this.tBoxRecieved.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBoxRecieved.Multiline = true;
            this.tBoxRecieved.Name = "tBoxRecieved";
            this.tBoxRecieved.ReadOnly = true;
            this.tBoxRecieved.Size = new System.Drawing.Size(694, 326);
            this.tBoxRecieved.TabIndex = 42;
            // 
            // Send
            // 
            this.Send.BackColor = System.Drawing.SystemColors.Control;
            this.Send.Controls.Add(this.tBoxSend);
            this.Send.Font = new System.Drawing.Font("Tahoma", 14.25F, System.Drawing.FontStyle.Bold);
            this.Send.Location = new System.Drawing.Point(10, 10);
            this.Send.Name = "Send";
            this.Send.Size = new System.Drawing.Size(700, 355);
            this.Send.TabIndex = 69;
            this.Send.TabStop = false;
            this.Send.Text = "Send";
            // 
            // tBoxSend
            // 
            this.tBoxSend.BackColor = System.Drawing.Color.White;
            this.tBoxSend.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tBoxSend.Font = new System.Drawing.Font("Tahoma", 9.75F);
            this.tBoxSend.Location = new System.Drawing.Point(3, 26);
            this.tBoxSend.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tBoxSend.Multiline = true;
            this.tBoxSend.Name = "tBoxSend";
            this.tBoxSend.ReadOnly = true;
            this.tBoxSend.Size = new System.Drawing.Size(694, 326);
            this.tBoxSend.TabIndex = 41;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.Timer1_Tick);
            // 
            // FormModules
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(720, 740);
            this.Controls.Add(this.Recieved);
            this.Controls.Add(this.Send);
            this.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormModules";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Module Communication";
            this.Recieved.ResumeLayout(false);
            this.Recieved.PerformLayout();
            this.Send.ResumeLayout(false);
            this.Send.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox Recieved;
        public System.Windows.Forms.TextBox tBoxRecieved;
        private System.Windows.Forms.GroupBox Send;
        public System.Windows.Forms.TextBox tBoxSend;
        private System.Windows.Forms.Timer timer1;
    }
}