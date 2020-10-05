using System;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Text.RegularExpressions;

namespace AgOpenGPS
{
    public partial class FormABCurve : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;
        readonly bool CurveMode;
        private double upDnHeading = 0;

        public FormABCurve(Form _mf, bool mode)
        {
            Owner = mf = _mf as FormGPS;
            InitializeComponent();

            btnPausePlay.Text = gStr.gsPause;

            CurveMode = mode;
            Text = CurveMode ? gStr.gsABCurve : gStr.gsABline;
            tboxHeading.Text = "0";
        }

        private void FormABCurve_Load(object sender, EventArgs e)
        {
            lvLines.Clear();
            ListViewItem itm;

            if (CurveMode)
            {
                mf.CurveLines.isOkToAddPoints = false;
                foreach (var item in mf.CurveLines.Lines)
                {
                    itm = new ListViewItem(item.Name);
                    lvLines.Items.Add(itm);
                }
            }
            else
            {
                foreach (var item in mf.ABLines.ABLines)
                {
                    itm = new ListViewItem(item.Name);
                    lvLines.Items.Add(itm);
                }
            }
            if (lvLines.Items.Count > 0) lvLines.Items[lvLines.Items.Count - 1].EnsureVisible();

            ShowSavedPanel(true);
        }

        private void BtnAddToFile_Click(object sender, EventArgs e)
        {
            if (NameBox.Text.Length > 0)
            {
                if (CurveMode)
                {
                    if (mf.CurveLines.CurrentEditLine < mf.CurveLines.Lines.Count && mf.CurveLines.CurrentEditLine > -1)
                    {
                        mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].Name = NameBox.Text.Trim();
                        mf.FileSaveCurveLines();
                    }
                }
                else
                {
                    if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
                    {
                        mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Name = NameBox.Text.Trim();
                        mf.FileSaveABLines();
                    }
                }

                Button sa = sender as Button;
                if (sa.Name == "btnAddAndGo")
                {
                    if (CurveMode)
                    {
                        mf.CurveLines.CurrentLine = mf.CurveLines.CurrentEditLine;
                        mf.CurveLines.CurrentEditLine = -1;
                        mf.CurveLines.tryoutcurve = -1;
                    }
                    else
                    {

                        mf.ABLines.CurrentLine = mf.ABLines.CurrentEditLine;
                        mf.ABLines.CurrentEditLine = -1;
                        mf.ABLines.tryoutcurve = -1;
                    }
                    Close();
                }
                else
                {
                    //update the listbox
                    if (CurveMode) if (mf.CurveLines.CurrentEditLine < mf.CurveLines.Lines.Count && mf.CurveLines.CurrentEditLine > -1) lvLines.Items.Add(new ListViewItem(mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].Name));
                    else if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1) lvLines.Items.Add(new ListViewItem(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Name));

                    // go to bottom of list - if there is a bottom
                    if (lvLines.Items.Count > 0) lvLines.Items[lvLines.Items.Count - 1].EnsureVisible();

                    lvLines.Enabled = true;
                    NameBox.BackColor = SystemColors.ControlLight;
                    NameBox.Clear();
                    NameBox.Enabled = false;
                    btnAddAndGo.Enabled = false;
                    btnAddToFile.Enabled = false;
                    btnAPoint.Enabled = false;
                    btnTurnOff.Enabled = true;
                    lvLines.SelectedItems.Clear();
                    btnNewLine.Enabled = true;
                }
            }
            else mf.TimedMessageBox(2000, gStr.gsNoNameEntered, gStr.gsEnterLineName);
        }

        private void BtnNewLine_Click(object sender, EventArgs e)
        {
            if (CurveMode) mf.CurveLines.tryoutcurve = -1;
            else mf.ABLines.tryoutcurve = -1;
            lvLines.SelectedItems.Clear();
            ShowSavedPanel(false);
        }

        private void BtnAPoint_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.Lines.Add(new CCurveLines());
                mf.CurveLines.CurrentEditLine = mf.CurveLines.Lines.Count - 1;
                mf.CurveLines.isOkToAddPoints = true;
                lblCurveExists.Text = gStr.gsDriving;
                btnBPoint.Enabled = true;
            }
            else
            {
                mf.ABLines.ABLines.Add(new CABLines());
                mf.ABLines.CurrentEditLine = mf.ABLines.ABLines.Count - 1;
                tboxHeading.Text = Math.Round(Glm.ToDegrees(mf.fixHeading), 6).ToString();

                mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Easting = mf.pivotAxlePos.Easting;
                mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Northing = mf.pivotAxlePos.Northing;
                mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading = mf.pivotAxlePos.Heading;
                timer1.Enabled = true;
            }
            btnPausePlay.Enabled = true;
            tboxHeading.Enabled = true;
            btnAPoint.Enabled = false;
        }

        private void BtnBPoint_Click(object sender, EventArgs e)
        {
            btnNewLine.Enabled = false;
            btnBPoint.Enabled = false;

            if (CurveMode)
            {
                mf.CurveLines.isOkToAddPoints = false;
                if (mf.CurveLines.CurrentEditLine < mf.CurveLines.Lines.Count && mf.CurveLines.CurrentEditLine > -1)
                {
                    int cnt = mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts.Count;

                    if (mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].SpiralMode || mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].CircleMode)
                    {
                        if (cnt > 0)
                        {
                            double easting = 0;
                            double northing = 0;

                            for (int i = 0; i < cnt; i++)
                            {
                                easting += mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[i].Easting;
                                northing += mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[i].Northing;
                            }
                            easting /= cnt;
                            northing /= cnt;

                            mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts.Clear();
                            mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts.Add(new Vec3(northing, easting, 0));

                        }
                        else
                        {
                            mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts.Add(new Vec3(mf.pivotAxlePos.Northing, mf.pivotAxlePos.Easting, 0));
                        }
                        mf.YouTurnButtons(true);
                        //mf.FileSaveCurveLine();
                        lblCurveExists.Text = gStr.gsCurveSet;


                        btnAddAndGo.Enabled = true;
                        btnAddToFile.Enabled = true;

                        NameBox.BackColor = Color.LightGreen;
                        NameBox.Enabled = true;

                        NameBox.Text = (mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].SpiralMode ? "spiral " : "circle ") + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                    }
                    else if (cnt > 5)
                    {
                        //make sure distance isn't too big between points on Turn
                        for (int i = 0; i < cnt - 1; i++)
                        {
                            int j = i + 1;
                            //if (j == cnt) j = 0;
                            double distance = Glm.Distance(mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[i], mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[j]);
                            if (distance > 1.2)
                            {
                                Vec3 pointB = new Vec3((mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[i].Northing + mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[j].Northing) / 2.0,
                                    (mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[i].Easting + mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[j].Easting) / 2.0,
                                    mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[i].Heading);

                                mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts.Insert(j, pointB);
                                cnt++;
                                i = -1;
                            }
                        }

                        //build the tail extensions
                        mf.CurveLines.AddFirstLastPoints();
                        
                        mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts.CalculateRoundedCorner(0.5, mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].BoundaryMode, 0.0436332);

                        //calculate average heading of line
                        double x = 0, y = 0;
                        for (int i = 20; i < cnt - 20; i++)
                        {
                            x += Math.Cos(mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[i].Heading);
                            y += Math.Sin(mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts[i].Heading);
                        }
                        x /= (mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts.Count - 40);
                        y /= (mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts.Count - 40);
                        mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].Heading = Math.Atan2(y, x);
                        if (mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].Heading < 0) mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].Heading += Glm.twoPI;

                        btnAddAndGo.Enabled = true;
                        btnAddToFile.Enabled = true;

                        NameBox.BackColor = Color.LightGreen;
                        NameBox.Enabled = true;
                        NameBox.Text = (Math.Round(Glm.ToDegrees(mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].Heading), 1)).ToString(CultureInfo.InvariantCulture)
                            + "\u00B0" + mf.FindDirection(mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].Heading)
                            + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        mf.CurveLines.Lines.RemoveAt(mf.CurveLines.CurrentEditLine);
                        mf.CurveLines.CurrentEditLine = -1;
                        lblCurveExists.Text = " > Off <";
                        btnNewLine.Enabled = true;
                    }
                }
                else
                {
                    mf.CurveLines.CurrentEditLine = -1;
                    lblCurveExists.Text = " > Off <";
                    btnNewLine.Enabled = true;
                }
            }
            else
            {
                if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
                {
                    mf.ABLines.SetABLineBPoint(true);
                    NameBox.Text = (Math.Round(Glm.ToDegrees(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading), 1)).ToString(CultureInfo.InvariantCulture)
                        + "\u00B0" +
                        mf.FindDirection(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

                    NameBox.Enabled = true;
                    btnAddAndGo.Enabled = true;
                    btnAddToFile.Enabled = true;
                }
            }
            ShowSavedPanel(true);
        }

        private void TextBox1_Enter(object sender, EventArgs e)
        {
            NameBox.Text = "";
        }

        private void BtnTurnOff_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                mf.CurveLines.OldHowManyPathsAway = double.NegativeInfinity;
                mf.CurveLines.isOkToAddPoints = false;
                mf.CurveLines.tryoutcurve = -1;
            }
            else mf.ABLines.tryoutcurve = -1;


            mf.CurveLines.BtnCurveLineOn = false;
            mf.btnCurve.Image = Properties.Resources.CurveOff;

            mf.ABLines.BtnABLineOn = false;
            mf.btnABLine.Image = Properties.Resources.ABLineOff;


            mf.YouTurnButtons(false);

            if (mf.isAutoSteerBtnOn)
            {
                mf.isAutoSteerBtnOn = false;
                mf.btnAutoSteer.Image = Properties.Resources.AutoSteerOff;
                if (mf.yt.isYouTurnBtnOn) mf.btnAutoYouTurn.PerformClick();
            }
            Close();
        }

        private void BtnListDelete_Click(object sender, EventArgs e)
        {
            if (lvLines.SelectedItems.Count > 0)
            {
                int num = lvLines.SelectedIndices[0];
                lvLines.SelectedItems[0].Remove();

                if (CurveMode)
                {
                    mf.CurveLines.CurrentEditLine--;

                    if (mf.CurveLines.CurrentLine == num) mf.CurveLines.CurrentLine = -1;
                    else if (mf.CurveLines.CurrentLine > num) mf.CurveLines.CurrentLine--;
                    mf.CurveLines.Lines.RemoveAt(num);
                    mf.FileSaveCurveLines();
                }
                else
                {
                    mf.ABLines.CurrentEditLine--;

                    if (mf.ABLines.CurrentLine == num) mf.ABLines.CurrentLine = -1;
                    else if (mf.ABLines.CurrentLine > num) mf.ABLines.CurrentLine--;
                    mf.ABLines.ABLines.RemoveAt(num);
                    mf.FileSaveABLines();
                }
            }
        }

        private void BtnListUse_Click(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;

            if (count > 0)
            {
                int idx = lvLines.SelectedIndices[0];

                if (CurveMode)
                {
                    mf.CurveLines.OldHowManyPathsAway = double.NegativeInfinity;
                    mf.CurveLines.CurrentLine = idx;
                    mf.CurveLines.tryoutcurve = -1;
                }
                else
                {
                    mf.ABLines.CurrentLine = idx;
                    mf.ABLines.tryoutcurve = -1;
                }

                mf.YouTurnButtons(true);
                Close();
            }
        }

        private void ShowSavedPanel(bool showPanel)
        {
            timer1.Enabled = false;
            if (showPanel)
            {
                Size = new Size(426, 409);
                ABCurveBox.Visible = false;
                SelectBox.Visible = true;
            }
            else
            {
                ABCurveBox.Left = 0;

                Size = new Size(256, 409);
                SelectBox.Visible = false;
                ABCurveBox.Visible = true;

                btnAPoint.Enabled = true;
                btnBPoint.Enabled = false;

                Status.Visible = CurveMode;
                lblCurveExists.Visible = CurveMode;
                btnPausePlay.Visible = CurveMode;

                lblFixHeading.Visible = !CurveMode;
                lblKeepGoing.Visible = !CurveMode;
                tboxHeading.Visible = !CurveMode;
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            if (!CurveMode)
            {
                lblFixHeading.Text = Convert.ToString(Math.Round(Glm.ToDegrees(mf.fixHeading), 1)) + "°";
                lblKeepGoing.Text = "";

                if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
                {
                    //make sure we go at least 3 or so meters before allowing B reference point
                    if (!btnAPoint.Enabled && !btnBPoint.Enabled)
                    {
                        double pointAToFixDistance = Glm.Distance(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref2, mf.pn.fix);
                        if (pointAToFixDistance > 100) btnBPoint.Enabled = true;
                        else lblKeepGoing.Text = Convert.ToInt16(100 - pointAToFixDistance).ToString();
                    }
                }
            }
        }

        private void BtnCancelMain_Click(object sender, EventArgs e)
        {
            if (CurveMode)
            {
                if (btnBPoint.Enabled)
                {
                    if (mf.CurveLines.CurrentEditLine < mf.CurveLines.Lines.Count && mf.CurveLines.CurrentEditLine > -1)
                    {
                        if (mf.CurveLines.CurrentLine == mf.CurveLines.CurrentEditLine) mf.CurveLines.CurrentLine = -1;
                        else if (mf.CurveLines.CurrentLine > mf.CurveLines.CurrentEditLine) mf.CurveLines.CurrentLine--;
                        mf.CurveLines.Lines.RemoveAt(mf.CurveLines.CurrentEditLine);
                    }
                }
                mf.FileSaveCurveLines();

                mf.CurveLines.tryoutcurve = -1;
                mf.CurveLines.CurrentEditLine = -1;
                mf.CurveLines.isOkToAddPoints = false;
            }
            else
            {
                if (btnBPoint.Enabled)
                {
                    if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
                    {
                        if (mf.ABLines.CurrentLine == mf.ABLines.CurrentEditLine) mf.ABLines.CurrentLine = -1;
                        else if (mf.ABLines.CurrentLine > mf.ABLines.CurrentEditLine) mf.ABLines.CurrentLine--;
                        mf.ABLines.ABLines.RemoveAt(mf.ABLines.CurrentEditLine);
                    }
                }

                mf.FileSaveABLines();
                mf.ABLines.CurrentEditLine = -1;
                mf.ABLines.tryoutcurve = -1;
            }
            Close();
        }

        private void LvLines_SelectedIndexChanged(object sender, EventArgs e)
        {
            int count = lvLines.SelectedItems.Count;
            if (count > 0)
            {
                if (CurveMode) mf.CurveLines.tryoutcurve = lvLines.SelectedIndices[0];
                else mf.ABLines.tryoutcurve = lvLines.SelectedIndices[0];
                btnListDelete.Enabled = true;
                btnListUse.Enabled = true;
            }
            else
            {
                if (CurveMode) mf.CurveLines.tryoutcurve = -1;
                else mf.ABLines.tryoutcurve = -1;
                btnListDelete.Enabled = false;
                btnListUse.Enabled = false;
            }
        }

        private void BtnPausePlay_Click(object sender, EventArgs e)
        {
            if (mf.CurveLines.isOkToAddPoints = !mf.CurveLines.isOkToAddPoints)
            {
                btnPausePlay.Image = Properties.Resources.BoundaryRecord;
                btnPausePlay.Text = gStr.gsRecord;
            }
            else
            {
                btnPausePlay.Image = Properties.Resources.boundaryPause;
                btnPausePlay.Text = gStr.gsPause;
            }
        }

        private void TboxHeading_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 360, upDnHeading, this, false, 6))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    tboxHeading.Text = (upDnHeading = Math.Round(form.ReturnValue, 6)).ToString();

                    if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
                    {
                        mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading = Math.Round(Glm.ToRadians(upDnHeading), 6);
                        mf.ABLines.SetABLineBPoint(false);

                        NameBox.Text = (Math.Round(Glm.ToDegrees(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading), 1)).ToString(CultureInfo.InvariantCulture)
                            + "\u00B0" +
                            mf.FindDirection(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);
                    }

                    NameBox.BackColor = Color.LightGreen;

                    NameBox.Enabled = true;
                    btnAddAndGo.Enabled = true;
                    btnAddToFile.Enabled = true;
                    btnNewLine.Enabled = false;
                    ShowSavedPanel(true);
                }
            }
            btnCancel2.Focus();
        }
    }
}