using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTimedMessage : Form
    {
        public int timer = 0;

        public void SetTimedMessage(int timeInMsec, string str, string str2, Form callingForm)
        {
            timer1.Stop();
            lblMessage.Text = str;
            lblMessage2.Text = str2;
            timer1.Interval = 100;

            Point position = Screen.FromControl(callingForm).WorkingArea.Location;
            position.X += Screen.FromControl(callingForm).Bounds.Width / 2 - Width / 2;
            position.Y += Screen.FromControl(callingForm).Bounds.Height / 2 - 75;

            Location = position;
            Width = Math.Max(str2.Length, str.Length) * 15 + 120;
            Height = 151;
            timer1.Start();
            Show();
            timer = timeInMsec;

        }

        public FormTimedMessage()
        {
            InitializeComponent();
            lblMessage.Text = "";
            lblMessage2.Text = "";
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer -= timer1.Interval;
            if (timer <= 0)
            {
                timer1.Stop();
                Hide();
            }
            else
            {
                panel1.BackColor = SystemColors.Control;
                BackColor = (BackColor == Color.Red) ? SystemColors.Control : Color.Red;
            }
        }
    }
}