using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormEditAB : Form
    {
        private readonly FormGPS mf;
        readonly bool CurveMode;
        private double snapAdj;

        public FormEditAB(Form callingForm, bool mode)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;

            InitializeComponent();

            this.Text = String.Get("gsEditABLine");
            label2.Text = String.Get("gsABHeading");
            label5.Text = String.Get("gsOffset");

            snapAdj = Math.Round(mf.Guidance.WidthMinusOverlap / 2 * mf.m2MetImp, mf.decimals);

            TboxSnapAdj.Text = Math.Round(snapAdj, mf.decimals).ToString();

            btnCancel.Focus();
            CurveMode = mode;
            if (CurveMode)
            {
                btnBPoint.Visible = false;
                label2.Visible = false;
                mf.CurveLines.isEditing = true;
                mf.CurveLines.CurrentEditLine = mf.CurveLines.CurrentLine;
            }
            else
            {
                tboxHeading.Visible = true;
                btnBPoint.Visible = true;
                label2.Visible = true;
                mf.ABLines.isEditing = true;
                mf.ABLines.CurrentEditLine = mf.ABLines.CurrentLine;
                if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1) tboxHeading.Text = Glm.ToDegrees(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading).ToString("0.00##");
            }
        }

        private void BntOk_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.isEditing = false;
                mf.CurveLines.CurrentEditLine = -1;
                mf.FileSaveCurveLines();
            }
            else
            {
                mf.ABLines.isEditing = false;
                mf.ABLines.CurrentEditLine = -1;
                mf.FileSaveABLines();
            }
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                int last = mf.CurveLines.CurrentLine;
                mf.FileLoadCurveLines();
                mf.CurveLines.ResetABLine = true;
                mf.CurveLines.isEditing = false;
                mf.CurveLines.CurrentLine = last;
                mf.CurveLines.GuidanceLines.Clear();
                Properties.Settings.Default.LastCurveLine = last;
                Properties.Settings.Default.Save();
                mf.CurveLines.CurrentEditLine = -1;
            }
            else
            {
                int last = mf.ABLines.CurrentLine;
                mf.FileLoadABLines();
                mf.ABLines.ResetABLine = true;
                mf.ABLines.isEditing = false;
                Properties.Settings.Default.LastABLine = last;
                Properties.Settings.Default.Save();
                mf.ABLines.CurrentLine = last;
                mf.ABLines.CurrentEditLine = -1;
            }
            Close();
        }

        private void BtnSwapAB_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.SwapAB();
                if (mf.tram.displayMode > 0) mf.CurveLines.BuildTram();
            }
            else
            {
                mf.ABLines.SwapAB();
                if (mf.tram.displayMode > 0) mf.ABLines.BuildTram();
                tboxHeading.Text = Glm.ToDegrees(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading).ToString("0.00##");
                if (mf.tram.displayMode > 0) mf.ABLines.BuildTram();
            }

        }

        private void BtnBPoint_Click(object sender, EventArgs e)
        {
            if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
            {
                mf.ABLines.SetABLineBPoint(true);
                tboxHeading.Text = Glm.ToDegrees(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading).ToString("0.00##");
            }
        }

        private void TboxHeading_Enter(object sender, EventArgs e)
        {
            if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
            {
                using (var form = new FormNumeric(0, 360, Math.Round(Glm.ToDegrees(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading), 4), this, false,6))
                {
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK)
                    {
                        tboxHeading.Text = form.ReturnValue.ToString("0.00##");
                        mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading = Math.Round(Glm.ToRadians(form.ReturnValue),6);
                        mf.ABLines.SetABLineBPoint(false);
                    }
                }
            }
            btnCancel.Focus();
        }

        private void TboxSnapAdj_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(0.1 * mf.m2MetImp, mf.decimals), Math.Round(50 * mf.m2MetImp,mf.decimals), snapAdj, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxSnapAdj.Text = (snapAdj = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void BtnContourPriority_Click(object sender, EventArgs e)
        {
            if (CurveMode) mf.CurveLines.MoveLine(mf.CurveLines.distanceFromRefLine);
            else mf.ABLines.MoveLine(mf.ABLines.distanceFromRefLine);
        }

        private void BtnRightHalfWidth_Click(object sender, EventArgs e)
        {
            if (CurveMode) mf.CurveLines.MoveLine(Math.Round(mf.CurveLines.isSameWay ? snapAdj * mf.metImp2m : -snapAdj * mf.metImp2m, 2));
            else mf.ABLines.MoveLine(Math.Round(mf.ABLines.isSameWay ? snapAdj * mf.metImp2m : -snapAdj * mf.metImp2m, 2));
        }

        private void BtnLeftHalfWidth_Click(object sender, EventArgs e)
        {
            if (CurveMode) mf.CurveLines.MoveLine(Math.Round(mf.CurveLines.isSameWay ? -snapAdj * mf.metImp2m : snapAdj * mf.metImp2m, 2));
            else mf.ABLines.MoveLine(Math.Round(mf.ABLines.isSameWay ? -snapAdj * mf.metImp2m : snapAdj * mf.metImp2m, 2));
        }

        private void BtnAdjRight_Click(object sender, EventArgs e)
        {
            if (CurveMode) mf.CurveLines.MoveLine(mf.CurveLines.isSameWay ? 0.1 : -0.11);
            else mf.ABLines.MoveLine(mf.ABLines.isSameWay ? 0.1 : -0.1);
        }

        private void BtnAdjLeft_Click(object sender, EventArgs e)
        {
            if (CurveMode) mf.CurveLines.MoveLine(mf.CurveLines.isSameWay ? -0.1 : 0.1);
            else mf.ABLines.MoveLine(mf.ABLines.isSameWay ? -0.1 : 0.1);
        }
    }
}
