using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormBoundaryPlayer : Form
    {
        //properties
        private readonly FormGPS mf;

        //constructor
        public FormBoundaryPlayer(FormGPS AgOpenGPS, Form callingForm)
        {
            Owner = callingForm;
            mf = AgOpenGPS;

            InitializeComponent();

            //btnStop.Text = gStr.gsDone;
            btnPausePlay.Text = gStr.gsRecord;
            label1.Text = gStr.gsArea + ":";
            this.Text = gStr.gsStopRecordPauseBoundary;
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

                mf.bnd.bndArr[mf.bnd.bndArr.Count - 1].FixBoundaryLine(mf.bnd.bndArr.Count - 1, mf.Guidance.GuidanceWidth);
                mf.bnd.bndArr[mf.bnd.bndArr.Count - 1].PreCalcBoundaryLines();
                mf.bnd.bndArr[mf.bnd.bndArr.Count - 1].CalculateBoundaryArea();
                mf.bnd.bndArr[mf.bnd.bndArr.Count - 1].CalculateBoundaryWinding();


                mf.turn.BuildTurnLines(mf.bnd.bndArr.Count - 1);
                mf.gf.BuildGeoFenceLines(mf.bnd.bndArr.Count - 1);

                mf.fd.UpdateFieldBoundaryGUIAreas();
                mf.mazeGrid.BuildMazeGridArray();

                mf.FileSaveBoundary();
            }

            //stop it all for adding
            mf.bnd.isOkToAddPoints = false;
            mf.bnd.isBndBeingMade = false;


            mf.bnd.bndBeingMadePts.Clear();
            Hide();
            Owner.Show();
            Close();
        }

        //actually the record button
        private void BtnPausePlay_Click(object sender, EventArgs e)
        {
            if (mf.bnd.isOkToAddPoints)
            {
                mf.bnd.isOkToAddPoints = false;
                btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                btnPausePlay.Text = gStr.gsRecord;
                btnAddPoint.Enabled = true;
                btnDeleteLast.Enabled = true;
            }
            else
            {
                mf.bnd.isOkToAddPoints = true;
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                btnPausePlay.Text = gStr.gsPause;
                btnAddPoint.Enabled = false;
                btnDeleteLast.Enabled = false;
            }
            mf.Focus();
        }

        private void FormBoundaryPlayer_Load(object sender, EventArgs e)
        {
            //mf.bnd.isOkToAddPoints = false;
            btnPausePlay.Image = Properties.Resources.BoundaryRecord;
            nudOffset.Value = (decimal)mf.bnd.createBndOffset;
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

        private void Timer1_Tick(object sender, EventArgs e)
        {
            int ptCount = mf.bnd.bndBeingMadePts.Count;
            double area = 0;

            if (ptCount > 0)
            {
                int j = ptCount - 1;  // The last vertex is the 'previous' one to the first

                for (int i = 0; i < ptCount; j = i++)
                {
                    area += (mf.bnd.bndBeingMadePts[j].easting + mf.bnd.bndBeingMadePts[i].easting) * (mf.bnd.bndBeingMadePts[j].northing - mf.bnd.bndBeingMadePts[i].northing);
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
            DialogResult result3 = MessageBox.Show(gStr.gsCompletelyDeleteBoundary,
                                    gStr.gsDeleteForSure,
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Question,
                                    MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes)
            {
                mf.bnd.bndBeingMadePts?.Clear();
                lblPoints.Text = mf.bnd.bndBeingMadePts.Count.ToString();
            }
            mf.Focus();
        }

        private void NudOffset_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender, this);
            btnPausePlay.Focus();
            mf.bnd.createBndOffset = (double)nudOffset.Value;
        }
    }
}