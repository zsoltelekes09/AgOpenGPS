using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormBoundaryPlayer : Form
    {
        //properties
        private readonly FormGPS mf;

        //constructor
        public FormBoundaryPlayer(FormGPS AgOpenGPS, FormBoundary callingForm)
        {
            Owner = callingForm;
            mf = AgOpenGPS;

            InitializeComponent();

            //btnStop.Text = gStr.gsDone;
            btnPausePlay.Text = String.Get("gsRecord");
            label1.Text = String.Get("gsArea") + ":";
            this.Text = String.Get("gsStopRecordPauseBoundary");
            lblOffset.Text = String.Get("gsOffset");

            btnPausePlay.Image = Properties.Resources.BoundaryRecord;
            TboxBndOffset.Text = mf.bnd.createBndOffset.ToString("N2");
            mf.Focus();

            if (mf.isMetric)
            {
                lblArea.Text = Math.Round(0.0, 2) + " Ha";
            }
            else
            {
                lblArea.Text = Math.Round(0.0, 2) + " Acre";
            }
            lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();

        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            if (mf.bnd.bndBeingMadePts.Count > 2)
            {
                mf.bnd.bndArr.Add(new CBoundaryLines());
                mf.turn.turnArr.Add(new CTurnLines());
                mf.gf.geoFenceArr.Add(new CGeoFenceLines());
                mf.hd.headArr.Add(new CHeadLines());
                mf.bnd.bndArr[mf.bnd.bndArr.Count - 1].bndLine.AddRange(mf.bnd.bndBeingMadePts);

                mf.bnd.bndArr[mf.bnd.bndArr.Count - 1].FixBoundaryLine();
                mf.bnd.bndArr[mf.bnd.bndArr.Count - 1].CalculateBoundaryArea();


                mf.turn.BuildTurnLines(mf.bnd.bndArr.Count - 1);
                mf.gf.BuildGeoFenceLines(mf.bnd.bndArr.Count - 1);

                mf.fd.UpdateFieldBoundaryGUIAreas();

                mf.FileSaveBoundary();
            }

            //stop it all for adding
            mf.bnd.isOkToAddPoints = false;
            mf.bnd.isBndBeingMade = false;


            mf.bnd.bndBeingMadePts.Clear();
            Hide();

            FormBoundary Boundary = (FormBoundary)Owner;
            Boundary.UpdateChart();
            Boundary.UpdateScroll(-1);

            Owner.Show();

            Close();
        }

        //actually the record button
        private void BtnPausePlay_Click(object sender, EventArgs e)
        {
            mf.bnd.isOkToAddPoints = !mf.bnd.isOkToAddPoints;
            btnAddPoint.Enabled = !mf.bnd.isOkToAddPoints;
            btnDeleteLast.Enabled = !mf.bnd.isOkToAddPoints;

            btnPausePlay.Image = mf.bnd.isOkToAddPoints ? Properties.Resources.boundaryPause : Properties.Resources.BoundaryRecord;
            btnPausePlay.Text = mf.bnd.isOkToAddPoints ? String.Get("gsPause") : String.Get("gsRecord");

            mf.Focus();
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.bndBeingMadePts.Count;
            double area = 0;

            if (ptCount > 0)
            {
                int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                for (int i = 0; i < ptCount; j = i++)
                {
                    area += (mf.bnd.bndBeingMadePts[j].Easting + mf.bnd.bndBeingMadePts[i].Easting) * (mf.bnd.bndBeingMadePts[j].Northing - mf.bnd.bndBeingMadePts[i].Northing);
                }
                area = Math.Abs(area / 2);
            }
            if (mf.isMetric)
            {
                lblArea.Text = Math.Round(area * 0.0001, 2) + " Ha";
            }
            else
            {
                lblArea.Text = Math.Round(area * 0.000247105, 2) + " Acre";
            }
            lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();

        }

        private void BtnAddPoint_Click(object sender, EventArgs e)
        {
            mf.bnd.isOkToAddPoints = true;
            mf.AddBoundaryPoint();
            mf.bnd.isOkToAddPoints = false;
            lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();

            mf.Focus();
        }

        private void BtnDeleteLast_Click(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.bndBeingMadePts.Count;
            if (ptCount > 0)
                mf.bnd.bndBeingMadePts.RemoveAt(ptCount - 1);
            lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();
            mf.Focus();
        }

        private void BtnRestart_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(String.Get("gsCompletelyDeleteBoundary"),
                                    String.Get("gsDeleteForSure"),
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                mf.bnd.bndBeingMadePts.Clear();
                lblPoints.Text = "0";
            }
            mf.Focus();
        }

        private void TboxBndOffset_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 50, mf.bnd.createBndOffset, this, false, 2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxBndOffset.Text = (mf.bnd.createBndOffset = form.ReturnValue).ToString("N2");
                }
            }
            btnStop.Focus();
        }
    }
}