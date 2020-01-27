using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTimedMessage : Form
    {
        public void SetTimedMessage(int timeInMsec, string str, string str2, Form callingForm)
        {
            timer1.Stop();
            lblMessage.Text = str;
            lblMessage2.Text = str2;
            timer1.Interval = timeInMsec;
            Width = str2.Length * 15 + 120;
            this.Show();
            this.Focus();
            this.Left = callingForm.Left + callingForm.Width / 2 - this.Width/2;
            this.Top = callingForm.Top + callingForm.Height / 2 - this.Height/2;
            timer1.Start();
        }

        public FormTimedMessage()
        {
            InitializeComponent();
            lblMessage.Text = "";
            lblMessage2.Text = "";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Stop();
            this.Hide();
        }
    }
}