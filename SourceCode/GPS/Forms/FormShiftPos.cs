using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormShiftPos : Form
    {
        //class variables
        private readonly FormGPS mf;
        private readonly Timer Timer = new Timer();
        private byte TimerMode = 0;

        public FormShiftPos(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();
            Timer.Tick += new EventHandler(TimerRepeat_Tick);

            label27.Text = String.Get("gsNorth");
            label2.Text = String.Get("gsWest");
            label3.Text = String.Get("gsEast");
            label4.Text = String.Get("gsSouth");
            this.Text = String.Get("gsShiftGPSPosition");
        }

        private void FormShiftPos_Load(object sender, EventArgs e)
        {
            nudEast.Value = (decimal)mf.pn.fixOffset.Easting * 100;
            nudNorth.Value = (decimal)mf.pn.fixOffset.Northing * 100;
        }

        private void BtnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            TimerMode = 0;
            Timer.Enabled = false;
            TimerRepeat_Tick(null, EventArgs.Empty);
        }

        private void BtnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            TimerMode = 1;
            Timer.Enabled = false;
            TimerRepeat_Tick(null, EventArgs.Empty);
        }

        private void BtnWest_MouseDown(object sender, MouseEventArgs e)
        {
            TimerMode = 2;
            Timer.Enabled = false;
            TimerRepeat_Tick(null, EventArgs.Empty);
        }

        private void BtnEast_MouseDown(object sender, MouseEventArgs e)
        {
            TimerMode = 3;
            Timer.Enabled = false;
            TimerRepeat_Tick(null, EventArgs.Empty);
        }

        private void Btn_MouseUp(object sender, MouseEventArgs e)
        {
            Timer.Enabled = false;
        }

        private void TimerRepeat_Tick(object sender, EventArgs e)
        {
            if (Timer.Enabled)
            {
                if (Timer.Interval > 50) Timer.Interval -= 50;
            }
            else
                Timer.Interval = 500;

            Timer.Enabled = true;

            if (TimerMode == 0)
            {
                nudNorth.UpButton();
                mf.pn.fixOffset.Northing = (double)nudNorth.Value / 100;
            }
            else if (TimerMode == 1)
            {
                nudNorth.DownButton();
                mf.pn.fixOffset.Northing = (double)nudNorth.Value / 100;
            }
            else if (TimerMode == 2)
            {
                nudEast.DownButton();
                mf.pn.fixOffset.Easting = (double)nudEast.Value / 100;
            }
            else if (TimerMode == 3)
            {
                nudEast.UpButton();
                mf.pn.fixOffset.Easting = (double)nudEast.Value / 100;
            }
        }

        private void NudNorth_ValueChanged(object sender, EventArgs e)
        {
            mf.pn.fixOffset.Northing = (double)nudNorth.Value / 100;
        }

        private void NudEast_ValueChanged(object sender, EventArgs e)
        {
            mf.pn.fixOffset.Easting = (double)nudEast.Value / 100;
        }

        private void BtnZero_Click(object sender, EventArgs e)
        {
            nudEast.Value = 0;
            nudNorth.Value = 0;
            mf.pn.fixOffset.Easting = 0;
            mf.pn.fixOffset.Northing = 0;
        }

        private void BntOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}