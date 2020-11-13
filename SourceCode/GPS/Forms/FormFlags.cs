using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormFlags : Form
    {
        //class variables
        private readonly FormGPS mf;

        public FormFlags(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //this.bntOK.Text = gStr.gsForNow;
            //this.btnSave.Text = gStr.gsToFile;

            //this.Text = gStr.gsSmoothABCurve;
        }

        private void UpdateLabels()
        {
            lblLatStart.Text = mf.flagPts[mf.flagNumberPicked - 1].Latitude.ToString();
            lblLonStart.Text = mf.flagPts[mf.flagNumberPicked - 1].Longitude.ToString();
            lblEasting.Text = mf.flagPts[mf.flagNumberPicked - 1].Easting.ToString("N2");
            lblNorthing.Text = mf.flagPts[mf.flagNumberPicked - 1].Northing.ToString("N2");
            lblHeading.Text = Glm.ToDegrees(mf.flagPts[mf.flagNumberPicked - 1].Heading).ToString("N2");
            lblFlagSelected.Text = mf.flagPts[mf.flagNumberPicked - 1].ID.ToString();
            tboxFlagNotes.Text = mf.flagPts[mf.flagNumberPicked - 1].notes;
        }
        private void FormFlags_Load(object sender, EventArgs e)
        {
            UpdateLabels();
        }

        private void BtnNorth_MouseDown(object sender, MouseEventArgs e)
        {
            mf.flagNumberPicked++;
            if (mf.flagNumberPicked > mf.flagPts.Count) mf.flagNumberPicked = 1;
            UpdateLabels();
        }

        private void BtnSouth_MouseDown(object sender, MouseEventArgs e)
        {
            mf.flagNumberPicked--;
            if (mf.flagNumberPicked < 1) mf.flagNumberPicked = mf.flagPts.Count;
            UpdateLabels();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            mf.flagNumberPicked = 0;
            mf.FileSaveFlags();
            mf.flagDubinsList.Clear();
            Close();
        }

        private void BtnDeleteFlag_Click(object sender, EventArgs e)
        {
            int flag = mf.flagNumberPicked;
            if (mf.flagPts.Count > 0) mf.DeleteSelectedFlag();
            if (mf.flagPts.Count == 0)
            {
                mf.FileSaveFlags();
                Close();
                return;
            }
            if (flag > mf.flagPts.Count) mf.flagNumberPicked = mf.flagPts.Count;
            else mf.flagNumberPicked = flag;
            UpdateLabels();
        }

        private void TboxFlagNotes_Leave(object sender, EventArgs e)
        {
            mf.flagPts[mf.flagNumberPicked - 1].notes = tboxFlagNotes.Text;
        }

        private void TboxFlagNotes_TextChanged(object sender, EventArgs e)
        {

            //mf.flagPts[mf.flagNumberPicked - 1].notes = tboxFlagNotes.Text;
        }

        private void TboxFlagNotes_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r') e.Handled = true;
        }

        private void BtnDriveToFlag_Click(object sender, EventArgs e)
        {
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //MakeDubinsLineFromPivotToFlag();
            Vec3 steerAxlePosRP = mf.pivotAxlePos;
            if (mf.isMetric)
                lblDistanceToFlag.Text = Glm.Distance(steerAxlePosRP,
                    mf.flagPts[mf.flagNumberPicked - 1].Easting, mf.flagPts[mf.flagNumberPicked - 1].Northing).ToString("N2") + " m";
            else lblDistanceToFlag.Text = (Glm.Distance(steerAxlePosRP,
                mf.flagPts[mf.flagNumberPicked - 1].Easting, mf.flagPts[mf.flagNumberPicked - 1].Northing) * Glm.m2ft).ToString("N2") + " m";
        
        }

        private void MakeDubinsLineFromPivotToFlag()
        {
            //if (mf.ABLine.isBtnABLineOn)
            //{
            //    mf.ABLine.isBtnABLineOn = false;
            //    mf.btnABLine.Image = Properties.Resources.ABLineOff;
            //}

            CDubins.turningRadius = mf.vehicle.minTurningRadius * 3.0;
            CDubins dubPath = new CDubins();

            // current psition
            Vec3 steerAxlePosRP = mf.pivotAxlePos;

            //bump it back so you can line up to point
            Vec3 goal = new Vec3
            {
                Easting = mf.flagPts[mf.flagNumberPicked - 1].Easting - (Math.Sin(mf.flagPts[mf.flagNumberPicked - 1].Heading) * 6),
                Northing = mf.flagPts[mf.flagNumberPicked - 1].Northing - (Math.Cos(mf.flagPts[mf.flagNumberPicked - 1].Heading) * 6),
                Heading = mf.flagPts[mf.flagNumberPicked - 1].Heading
            };

            //bump it forward
            Vec3 pt2 = new Vec3
            {
                Easting = steerAxlePosRP.Easting + (Math.Sin(steerAxlePosRP.Heading) * 6),
                Northing = steerAxlePosRP.Northing + (Math.Cos(steerAxlePosRP.Heading) * 6),
                Heading = steerAxlePosRP.Heading
            };

            //get the dubins path vec3 point coordinates of turn
            mf.flagDubinsList.Clear();

            mf.flagDubinsList = dubPath.GenerateDubins(pt2, goal, mf.gf);

        }

        private void TboxFlagNotes_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnExit.Focus();
            }

        }
    }
}