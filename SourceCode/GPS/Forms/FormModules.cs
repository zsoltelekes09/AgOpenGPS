using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormModules : Form
    {
        private readonly FormGPS mf = null;

        public FormModules(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();
            timer1.Start();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            tBoxSend.Lines = mf.DataSend;
            tBoxRecieved.Lines = mf.DataRecieved;
        }
    }
}