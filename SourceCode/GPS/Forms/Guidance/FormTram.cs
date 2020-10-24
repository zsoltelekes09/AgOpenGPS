using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormTram : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;
        readonly dynamic CurveMode;
        private double snapAdj = 0;

        public FormTram(Form callingForm, bool mode)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = String.Get("gsTramLines");
            lblSmallSnapRight.Text = String.Get("gsWidth");
            label1.Text = String.Get("gsTrack");
            label3.Text = String.Get("gsPasses");

            CurveMode = mode;

            if (CurveMode) mf.CurveLines.CurrentEditLine = mf.CurveLines.CurrentLine;
            else mf.ABLines.CurrentEditLine = mf.ABLines.CurrentLine;
        }

        private void FormTram_Load(object sender, EventArgs e)
        {
            //snapAdj = Properties.Settings.Default.setTram_eqWidth/2;
            snapAdj = Math.Round(mf.Guidance.WidthMinusOverlap / 2, mf.decimals);
            TboxSnapAdj.Text = Math.Round(snapAdj * mf.m2MetImp, mf.decimals).ToString();

            TboxWheelTrack.Text = Math.Round(mf.tram.wheelTrack * mf.m2MetImp, mf.decimals).ToString();
            TboxWheelWidth.Text = Math.Round(mf.tram.WheelWidth * mf.m2MetImp, mf.decimals).ToString();
            TboxOffset.Text = Math.Round(mf.tram.abOffset * mf.m2MetImp, mf.decimals).ToString();
            TboxTramWidth.Text = Math.Round(mf.tram.tramWidth * mf.m2MetImp, mf.decimals).ToString();
            TboxPasses.Text = mf.tram.passes.ToString();
            label2.Text = Math.Round(0.1 * mf.m2MetImp, mf.decimals).ToString() + (mf.isMetric ? " (Cm)" : " Inch?");
            //if off, turn it on because they obviously want a tram.
            if (mf.tram.displayMode == 0)
            {
                mf.tram.displayMode = 1;
                if (CurveMode) mf.CurveLines.BuildTram();
                else mf.ABLines.BuildTram();
            }

            switch (mf.tram.displayMode)
            {
                case 0:
                    btnMode.Image = Properties.Resources.TramOff;
                    break;
                case 1:
                    btnMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.FileSaveCurveLines();
                mf.CurveLines.isEditing = false;
                mf.CurveLines.CurrentEditLine = -1;
            }
            else
            {
                mf.FileSaveABLines();
                mf.ABLines.isEditing = false;
                mf.ABLines.CurrentEditLine = -1;
            }

            Close();
        }

        private void BtnLeft_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.MoveLine(-0.1);
                mf.CurveLines.BuildTram();
            }
            else
            {
                mf.ABLines.MoveLine(-0.1);
                mf.ABLines.BuildTram();
            }
        }

        private void BtnRight_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.MoveLine(0.1);
                mf.CurveLines.BuildTram();
            }
            else
            {
                mf.ABLines.MoveLine(0.1);
                mf.ABLines.BuildTram();
            }
        }

        private void BtnAdjLeft_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.MoveLine(-snapAdj);
                mf.CurveLines.BuildTram();
            }
            else
            {
                mf.ABLines.MoveLine(-snapAdj);
                mf.ABLines.BuildTram();
            }
        }

        private void BtnAdjRight_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.MoveLine(snapAdj);
                mf.CurveLines.BuildTram();
            }
            else
            {
                mf.ABLines.MoveLine(snapAdj);
                mf.ABLines.BuildTram();
            }
        }

        private void BtnSwapAB_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.SwapAB();
                mf.CurveLines.BuildTram();
            }
            else
            {
                mf.ABLines.SwapAB();
                mf.ABLines.BuildTram();
            }
        }

        private void BtnTriggerDistanceUp_MouseDown(object sender, MouseEventArgs e)
        {
            mf.tram.passes++;
            TboxPasses.Text = mf.tram.passes.ToString();
            Properties.Settings.Default.setTram_passes = mf.tram.passes;
            Properties.Settings.Default.Save();

            if (CurveMode) mf.CurveLines.BuildTram();
            else mf.ABLines.BuildTram();
        }

        private void BtnTriggerDistanceDn_MouseDown(object sender, MouseEventArgs e)
        {
            mf.tram.passes--;
            TboxPasses.Text = mf.tram.passes.ToString();
            Properties.Settings.Default.setTram_passes = mf.tram.passes;
            Properties.Settings.Default.Save();
            if (CurveMode) mf.CurveLines.BuildTram();
            else mf.ABLines.BuildTram();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            mf.tram.TramList.Clear();
            mf.tram.displayMode = 0;

            if (CurveMode)
            {
                mf.FileLoadCurveLines();
                mf.CurveLines.isEditing = false;
                mf.CurveLines.CurrentEditLine = -1;
            }
            else
            {
                mf.FileLoadABLines();
                mf.ABLines.isEditing = false;
                mf.ABLines.CurrentEditLine = -1;
            }

            Close();
        }

        private void BtnMode_Click(object sender, EventArgs e)
        {
            mf.tram.displayMode++;
            if (mf.tram.displayMode > 3) mf.tram.displayMode = 0;
            
            switch (mf.tram.displayMode)
            {
                case 0:
                    btnMode.Image = Properties.Resources.TramOff;
                    break;
                case 1:
                    btnMode.Image = Properties.Resources.TramAll;
                    break;
                case 2:
                    btnMode.Image = Properties.Resources.TramLines;
                    break;
                case 3:
                    btnMode.Image = Properties.Resources.TramOuter;
                    break;

                default:
                    break;
            }
        }


        #region NummericUpDown

        private void TboxSnapAdj_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(0.01 * mf.m2MetImp, mf.decimals), Math.Round(50 * mf.m2MetImp, mf.decimals), Math.Round(snapAdj * mf.m2MetImp, mf.decimals), this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxSnapAdj.Text = Math.Round(form.ReturnValue, mf.decimals).ToString();
                    Properties.Settings.Default.setTram_snapAdj = snapAdj = Math.Round(form.ReturnValue * mf.metImp2m, 2);
                    Properties.Settings.Default.Save();
                }
            }
            btnCancel.Focus();
        }

        private void TboxWheelWidth_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(0.05 * mf.m2MetImp, mf.decimals), Math.Round(5 * mf.m2MetImp, mf.decimals), Math.Round(mf.tram.WheelWidth * mf.m2MetImp, mf.decimals), this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxWheelWidth.Text = Math.Round(form.ReturnValue, mf.decimals).ToString();
                    Properties.Settings.Default.Tram_wheelWidth = mf.tram.WheelWidth = Math.Round(form.ReturnValue * mf.metImp2m, 2);
                    Properties.Settings.Default.Save();
                    if (CurveMode) mf.CurveLines.BuildTram();
                    else mf.ABLines.BuildTram();
                }
            }
            btnCancel.Focus();
        }

        private void TboxWheelTrack_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(0.1 * mf.m2MetImp, mf.decimals), Math.Round(10 * mf.m2MetImp, mf.decimals), Math.Round(mf.tram.wheelTrack * mf.m2MetImp, mf.decimals), this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxWheelTrack.Text = Math.Round(form.ReturnValue, mf.decimals).ToString();
                    Properties.Settings.Default.setTram_wheelSpacing = mf.tram.wheelTrack = Math.Round(form.ReturnValue * mf.metImp2m, 2);
                    mf.tram.halfWheelTrack = mf.tram.wheelTrack * 0.5;
                    Properties.Settings.Default.Save();
                    if (CurveMode) mf.CurveLines.BuildTram();
                    else mf.ABLines.BuildTram();
                }
            }
            btnCancel.Focus();
        }

        private void TboxOffset_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(-100 * mf.m2MetImp, mf.decimals), Math.Round(100 * mf.m2MetImp, mf.decimals), Math.Round(mf.tram.abOffset * mf.m2MetImp, mf.decimals), this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxOffset.Text = Math.Round(form.ReturnValue, mf.decimals).ToString();

                    Properties.Settings.Default.setTram_offset = mf.tram.abOffset = Math.Round(form.ReturnValue * mf.metImp2m, 2);
                    Properties.Settings.Default.Save();
                    if (CurveMode) mf.CurveLines.BuildTram();
                    else mf.ABLines.BuildTram();
                }
            }
            btnCancel.Focus();
        }

        private void TboxTramWidth_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(0.2 * mf.m2MetImp, mf.decimals), Math.Round(100 * mf.m2MetImp, mf.decimals), Math.Round(mf.tram.tramWidth * mf.m2MetImp, mf.decimals), this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxTramWidth.Text = Math.Round(form.ReturnValue, mf.decimals).ToString();
                    Properties.Settings.Default.setTram_eqWidth = mf.tram.tramWidth = Math.Round(form.ReturnValue * mf.metImp2m, 2);
                    Properties.Settings.Default.Save();
                    if (CurveMode) mf.CurveLines.BuildTram();
                    else mf.ABLines.BuildTram();
                }
            }
            btnCancel.Focus();
        }

        private void TboxPasses_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 999, mf.tram.passes, this, true,0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    mf.tram.passes = (int)form.ReturnValue;
                    TboxPasses.Text = mf.tram.passes.ToString();
                    Properties.Settings.Default.setTram_passes = mf.tram.passes;
                    Properties.Settings.Default.Save();
                    if (CurveMode) mf.CurveLines.BuildTram();
                    else mf.ABLines.BuildTram();
                }
            }
            btnCancel.Focus();
        }

        #endregion NummericUpDown

    }
}