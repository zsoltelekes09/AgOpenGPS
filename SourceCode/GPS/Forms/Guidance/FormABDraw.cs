using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Linq;

namespace AgOpenGPS
{
    public partial class FormABDraw : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        private double fieldCenterX, fieldCenterY, maxFieldDistance, Offset;
        private bool isA = true, isSet = false, ResetHeadLine = false, isDrawSections = false;
        private readonly bool HeadLand;
        private int start = 99999, end = 99999, Boundary = -1, TemplateIndex;

        //list of coordinates of boundary line
        public List<List<Vec3>> Template = new List<List<Vec3>>();

        public FormABDraw(Form callingForm, bool HeadDraw)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;

            InitializeComponent();
            //lblPick.Text = gStr.gsSelectALine;
            label3.Text = String.Get("gsCreate");
            label4.Text = String.Get("gsSelect");

            Offset1.Text = Offset2.Text = String.Get("gsOffset");



            Boundary = 0;

            Offset = Math.Round(mf.Guidance.GuidanceWidth * mf.m2MetImp, mf.decimals);

            Size = new Size(1006, 759);
            if (HeadLand = HeadDraw)
            {
                HeadLandBox.Visible = true;
                ABDrawBox.Visible = false;
                Text = String.Get("gsHeadlandForm");
                RebuildHeadLineTemplate(false);
                HeadLandBox.Location = new Point(710, 85);
            }
            else
            {
                Offset /= 2;
                HeadLandBox.Visible = false;
                ABDrawBox.Visible = true;
                Text = String.Get("gsClick2Pointsontheboundary");
                UpdateBoundary();
            }
            TboxOffset1.Text = TboxOffset2.Text = Offset.ToString();
        }

        private void FixLabelsCurve()
        {
            label1.Text = (mf.CurveLines.CurrentEditLine + 1).ToString() + " of " + mf.CurveLines.Lines.Count.ToString();
            if (mf.CurveLines.CurrentEditLine > -1 && mf.CurveLines.CurrentEditLine < mf.CurveLines.Lines.Count)
            {
                btnDeleteCurve.Enabled = true;
                lblCurveName.Text = mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].Name;
            }
            else
            {
                btnDeleteCurve.Enabled = false;
                lblCurveName.Text = "***";
            }
        }

        private void FixLabelsABLine()
        {
            label2.Text = (mf.ABLines.CurrentEditLine + 1).ToString() + " of " + mf.ABLines.ABLines.Count.ToString();
            if (mf.ABLines.CurrentEditLine > -1 && mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count)
            {
                btnDeleteABLine.Enabled = true;
                lblABLineName.Text = mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Name;
            }
            else
            {
                btnDeleteABLine.Enabled = false;
                lblABLineName.Text = "***";
            }
        }

        private void BtnSelectCurve_Click(object sender, EventArgs e)
        {
            mf.CurveLines.CurrentEditLine = (mf.CurveLines.Lines.Count > 0) ? (mf.CurveLines.CurrentEditLine + 1) % mf.CurveLines.Lines.Count : -1;

            FixLabelsCurve();
            oglSelf.Refresh();
        }

        private void BtnSelectABLine_Click(object sender, EventArgs e)
        {
            mf.ABLines.CurrentEditLine = (mf.ABLines.ABLines.Count > 0) ? (mf.ABLines.CurrentEditLine + 1) % mf.ABLines.ABLines.Count : -1;

            FixLabelsABLine();
            oglSelf.Refresh();
        }

        private void BtnCancelTouch_Click(object sender, EventArgs e)
        {
            btnSelectABLine.Focus();
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;

            isA = true;
            start = end = 99999;
            if (start == 99999) lblStart.Text = "--";
            else lblStart.Text = start.ToString();
            if (end == 99999) lblEnd.Text = "--";
            else lblEnd.Text = end.ToString();

            btnCancelTouch.Enabled = false;
            oglSelf.Refresh();
        }

        private void BtnDeleteCurve_Click(object sender, EventArgs e)
        {
            if (mf.CurveLines.CurrentEditLine < mf.CurveLines.Lines.Count && mf.CurveLines.CurrentEditLine > -1)
            {
                mf.CurveLines.Lines.RemoveAt(mf.CurveLines.CurrentEditLine);
                if (mf.CurveLines.CurrentEditLine < mf.CurveLines.CurrentLine) mf.CurveLines.CurrentLine--;
                if (mf.CurveLines.CurrentEditLine == mf.CurveLines.CurrentLine) mf.CurveLines.CurrentLine = -1;
                mf.CurveLines.CurrentEditLine--;
            }

            FixLabelsCurve();
            oglSelf.Refresh();
        }

        private void BtnDeleteABLine_Click(object sender, EventArgs e)
        {
            if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
            {
                mf.ABLines.ABLines.RemoveAt(mf.ABLines.CurrentEditLine);
                if (mf.ABLines.CurrentEditLine < mf.ABLines.CurrentLine) mf.ABLines.CurrentLine--;
                if (mf.ABLines.CurrentEditLine == mf.ABLines.CurrentLine) mf.ABLines.CurrentLine = -1;
                mf.ABLines.CurrentEditLine--;
            }

            FixLabelsABLine();
            oglSelf.Refresh();
        }

        private void BtnDrawSections_Click(object sender, EventArgs e)
        {
            isDrawSections = !isDrawSections;
            if (isDrawSections) btnDrawSections.Text = "On";
            else btnDrawSections.Text = "Off";
            oglSelf.Refresh();
        }

        private void UpdateBoundary()
        {
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                CalculateMinMax();
                oglSelf.Refresh();
            }

            FixLabelsABLine();
            FixLabelsCurve();

            if (HeadLand)
            {
                if (isSet)
                {
                    btnMoveLeft.Enabled = true;
                    btnMoveRight.Enabled = true;
                    btnMoveUp.Enabled = true;
                    btnMoveDown.Enabled = true;
                    btnDoneManualMove.Enabled = true;
                    btnDeletePoints.Enabled = true;
                    btnStartUp.Enabled = true;
                    btnStartDown.Enabled = true;
                    btnEndDown.Enabled = true;
                    btnEndUp.Enabled = true;
                }
                else
                {
                    btnMoveLeft.Enabled = false;
                    btnMoveRight.Enabled = false;
                    btnMoveUp.Enabled = false;
                    btnMoveDown.Enabled = false;
                    btnDoneManualMove.Enabled = false;
                    btnDeletePoints.Enabled = false;
                    btnStartUp.Enabled = false;
                    btnStartDown.Enabled = false;
                    btnEndDown.Enabled = false;
                    btnEndUp.Enabled = false;
                }
            }
        }

        private void RebuildHeadLineTemplate(bool Reset)
        {
            if (mf.hd.headArr.Count > Boundary && Boundary > -1)
            {
                Template.Clear();
                if (!Reset && mf.hd.headArr[Boundary].HeadLine.Count > 0)
                {
                    Template.AddRange(mf.hd.headArr[Boundary].HeadLine);

                    ResetHeadLine = false;
                }
                else
                {
                    Template.Add(new List<Vec3>(mf.bnd.bndArr[Boundary].bndLine));
                    ResetHeadLine = true;
                }
                UpdateBoundary();
            }
            start = end = 99999;
            if (start == 99999) lblStart.Text = "--";
            else lblStart.Text = start.ToString();
            if (end == 99999) lblEnd.Text = "--";
            else lblEnd.Text = end.ToString();
            isA = true;
            isSet = false;
            TemplateIndex = 0;
        }

        private void BtnDeletePoints_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;

            if (TemplateIndex <= Template.Count)
            {
                int start2 = start;
                int end2 = end;

               if (((Template[TemplateIndex].Count - end2 + start2) % Template[TemplateIndex].Count) < ((Template[TemplateIndex].Count - start2 + end2) % Template[TemplateIndex].Count)) { int index = start2; start2 = end2; end2 = index; }

                if (start2 > end2)
                {
                    Template[TemplateIndex].RemoveRange(start2, Template[TemplateIndex].Count - start2);
                    Template[TemplateIndex].RemoveRange(0, end2);
                }
                else
                {
                    Template[TemplateIndex].RemoveRange(start2, end2 - start2);
                }
            }

            start = end = 99999;
            if (start == 99999) lblStart.Text = "--";
            else lblStart.Text = start.ToString();
            if (end == 99999) lblEnd.Text = "--";
            else lblEnd.Text = end.ToString();
            isA = true;
            isSet = false;
            UpdateBoundary();
        }

        private void BtnDoneManualMove_Click(object sender, EventArgs e)
        {
            start = end = 99999;
            if (start == 99999) lblStart.Text = "--";
            else lblStart.Text = start.ToString();
            if (end == 99999) lblEnd.Text = "--";
            else lblEnd.Text = end.ToString();
            isA = true;
            isSet = false;
            UpdateBoundary();
        }

        private void BtnTurnOffHeadland_Click(object sender, EventArgs e)
        {
            if (ResetHeadLine && mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                mf.hd.headArr[Boundary].Indexer.Clear();
                mf.hd.headArr[Boundary].HeadLine.Clear();
            }
            ResetHeadLine = false;

            mf.FileSaveHeadland();
            Close();
        }

        private void BtnExit2_Click(object sender, EventArgs e)
        {
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                mf.hd.headArr[Boundary].HeadLine.Clear();
                if (!ResetHeadLine)
                    mf.hd.headArr[Boundary].HeadLine.AddRange(Template);

                mf.hd.headArr[Boundary].PreCalcHeadArea();
                ResetHeadLine = false;
            }
            mf.FileSaveHeadland();
            Close();
        }

        private void BtnReset_Click(object sender, EventArgs e)
        {
            ResetHeadLine = true;
            RebuildHeadLineTemplate(true);
        }

        private void BtnMoveUp_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            Vec3 Point;

            if (TemplateIndex <= Template.Count)
            {
                int start2 = start;
                int end2 = end;
                if (((Template[TemplateIndex].Count - end2 + start2) % Template[TemplateIndex].Count) < ((Template[TemplateIndex].Count - start2 + end2) % Template[TemplateIndex].Count)) { int index = start2; start2 = end2; end2 = index; }
                bool Loop = start2 > end2;

                for (int i = start2; i <= end2 || Loop; i++)
                {
                    if (i > Template[TemplateIndex].Count)
                    {
                        i = 0;
                        Loop = false;
                    }
                    if (i < Template[TemplateIndex].Count)
                    {
                        Point = Template[TemplateIndex][i];
                        Point.Northing++;
                        Template[TemplateIndex][i] = Point;
                    }
                }
            }
            UpdateBoundary();
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            Vec3 Point;

            if (TemplateIndex <= Template.Count)
            {
                int start2 = start;
                int end2 = end;
                if (((Template[TemplateIndex].Count - end2 + start2) % Template[TemplateIndex].Count) < ((Template[TemplateIndex].Count - start2 + end2) % Template[TemplateIndex].Count)) { int index = start2; start2 = end2; end2 = index; }
                bool Loop = start2 > end2;

                for (int i = start2; i <= end2 || Loop; i++)
                {
                    if (i > Template[TemplateIndex].Count)
                    {
                        i = 0;
                        Loop = false;
                    }
                    if (i < Template[TemplateIndex].Count)
                    {
                        Point = Template[TemplateIndex][i];
                        Point.Northing--;
                        Template[TemplateIndex][i] = Point;
                    }
                }
            }
            UpdateBoundary();
        }

        private void BtnMoveLeft_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            Vec3 Point;

            if (TemplateIndex <= Template.Count)
            {
                int start2 = start;
                int end2 = end;
                if (((Template[TemplateIndex].Count - end2 + start2) % Template[TemplateIndex].Count) < ((Template[TemplateIndex].Count - start2 + end2) % Template[TemplateIndex].Count)) { int index = start2; start2 = end2; end2 = index; }
                bool Loop = start2 > end2;
                for (int i = start2; i <= end2 || Loop; i++)
                {
                    if (i > Template[TemplateIndex].Count)
                    {
                        i = 0;
                        Loop = false;
                    }
                    if (i < Template[TemplateIndex].Count)
                    {
                        Point = Template[TemplateIndex][i];
                        Point.Easting--;
                        Template[TemplateIndex][i] = Point;
                    }
                }
            }
            UpdateBoundary();
        }

        private void BtnMoveRight_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            Vec3 Point;
            if (TemplateIndex <= Template.Count)
            {
                int start2 = start;
                int end2 = end;
                if (((Template[TemplateIndex].Count - end2 + start2) % Template[TemplateIndex].Count) < ((Template[TemplateIndex].Count - start2 + end2) % Template[TemplateIndex].Count)) { int index = start2; start2 = end2; end2 = index; }
                bool Loop = start2 > end2;
                for (int i = start2; i <= end2 || Loop; i++)
                {
                    if (i > Template[TemplateIndex].Count)
                    {
                        i = 0;
                        Loop = false;
                    }
                    if (i < Template[TemplateIndex].Count)
                    {
                        Point = Template[TemplateIndex][i];
                        Point.Easting++;
                        Template[TemplateIndex][i] = Point;
                    }
                }
            }
            UpdateBoundary();
        }

        private void BtnMakeFixedHeadland_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {

                if (Template.Count > 0 && Template.Count > TemplateIndex && Template[TemplateIndex].Count > 0)
                {
                    double offset = Math.Round(Offset * mf.metImp2m, 2) * (Boundary == 0 ? 1 : -1);
                    Vec3 Point;

                    int Start2 = start;
                    int End2 = end;

                    if (End2 > Template[TemplateIndex].Count) End2 = 0;
                    if (((Template[TemplateIndex].Count - End2 + Start2) % Template[TemplateIndex].Count) < ((Template[TemplateIndex].Count - Start2 + End2) % Template[TemplateIndex].Count)) { int index = Start2; Start2 = End2; End2 = index; }

                    int Index = (start == 99999 || end == 99999) ? -1 : TemplateIndex;

                    int test = Template.Count();

                    for (int i = 0; i < test; i++)
                    {
                        bool Loop = Start2 > End2;

                        List<Vec3> Template2 = new List<Vec3>();

                        for (int j = 0; j < Template[i].Count; j++)
                        {
                            Point = Template[i][j];
                            if (Index == -1 || (Index == i && (Loop && (j < End2 || j > Start2)) || (!Loop && j > Start2 && j < End2)))
                            {
                                double CosHeading = Math.Cos(Template[i][j].Heading);
                                double SinHeading = Math.Sin(Template[i][j].Heading);
                                Point.Northing += SinHeading * -offset;
                                Point.Easting += CosHeading * offset;
                            }
                            Template2.Add(Point);
                        }

                        List<List<Vec3>> finalPoly = Template2.ClipPolyLine(null, true, offset);

                        for (int j = 0; j < finalPoly.Count; j++)
                        {
                            double Area = finalPoly[j].PolygonArea();
                            if (Area > -25)
                            {
                                finalPoly.RemoveAt(j);
                                j--;
                            }
                            else
                                finalPoly[j].CalculateRoundedCorner(0.25,true, 0.04, 5);
                        }
                        Template.AddRange(finalPoly);
                    }
                    Template.RemoveRange(0, test);
                    
                    isSet = false;
                    isA = true;
                    start = end = 99999;
                }
                if (start == 99999) lblStart.Text = "--";
                else lblStart.Text = start.ToString();
                if (end == 99999) lblEnd.Text = "--";
                else lblEnd.Text = end.ToString();
                UpdateBoundary();
            }
        }

        private void BtnEndUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateIndex <= Template.Count)
            {
                if (end != 99999)
                {
                    end = (end + 1).Clamp(Template[TemplateIndex].Count);
                }
                if (end == 99999) lblEnd.Text = "--";
                else lblEnd.Text = end.ToString();
            }

            UpdateBoundary();
        }

        private void BtnEndDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateIndex <= Template.Count)
            {
                if (end != 99999)
                {
                    end = (end - 1).Clamp(Template[TemplateIndex].Count);
                }
                if (end == 99999) lblEnd.Text = "--";
                else lblEnd.Text = end.ToString();
            }
            UpdateBoundary();
        }

        private void BtnStartUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateIndex <= Template.Count)
            {
                if (start != 99999)
                {
                    start = (start + 1).Clamp(Template[TemplateIndex].Count);
                }
                if (start == 99999) lblStart.Text = "--";
                else lblStart.Text = start.ToString();
            }
            UpdateBoundary();
        }

        private void BtnStartDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateIndex <= Template.Count)
            {
                if (start != 99999)
                {
                    start = (start - 1).Clamp(Template[TemplateIndex].Count);
                }

                if (start == 99999) lblStart.Text = "--";
                else lblStart.Text = start.ToString();
            }
            UpdateBoundary();
        }

        private void TboxOffset_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(100 * mf.m2MetImp, mf.decimals), Offset, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxOffset1.Text = TboxOffset2.Text = (Offset = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnExit2.Focus();
            btnExit.Focus();
        }

        private void OglSelf_MouseDown(object sender, MouseEventArgs e)
        {
            btnCancelTouch.Enabled = true;
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;
            if (isSet) return;
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                Point pt = oglSelf.PointToClient(Cursor.Position);

                //Convert to Origin in the center of window, 800 pixels
                Vec2 pint;

                pint.Easting = (pt.X - 350) * maxFieldDistance / 632.0 + fieldCenterX;
                pint.Northing = (700 - pt.Y - 350) * maxFieldDistance / 632.0 + fieldCenterY;

                double minDist = double.PositiveInfinity;

                int Closest = 0;

                if (HeadLand)
                {
                    if (Template.Count > 0)
                    {
                        if (isA)
                        {
                            for (int i = 0; i < Template.Count; i++)
                            {
                                for (int j = 0; j < Template[i].Count; j++)
                                {
                                    double dist = ((pint.Easting - Template[i][j].Easting) * (pint.Easting - Template[i][j].Easting))
                                                    + ((pint.Northing - Template[i][j].Northing) * (pint.Northing - Template[i][j].Northing));
                                    if (dist < minDist)
                                    {
                                        TemplateIndex = i;
                                        minDist = dist;
                                        Closest = j;
                                    }
                                }
                            }
                        }
                        else
                        {
                            for (int j = 0; j < Template[TemplateIndex].Count; j++)
                            {
                                double dist = ((pint.Easting - Template[TemplateIndex][j].Easting) * (pint.Easting - Template[TemplateIndex][j].Easting))
                                                + ((pint.Northing - Template[TemplateIndex][j].Northing) * (pint.Northing - Template[TemplateIndex][j].Northing));
                                if (dist < minDist)
                                {
                                    minDist = dist;
                                    Closest = j;
                                }
                            }
                        }
                    }
                }
                else
                {
                    int ptCount = mf.bnd.bndArr[Boundary].bndLine.Count;
                    if (ptCount > 1)
                    {
                        for (int t = 0; t < ptCount; t++)
                        {
                            double dist = ((pint.Easting - mf.bnd.bndArr[Boundary].bndLine[t].Easting) * (pint.Easting - mf.bnd.bndArr[Boundary].bndLine[t].Easting))
                                            + ((pint.Northing - mf.bnd.bndArr[Boundary].bndLine[t].Northing) * (pint.Northing - mf.bnd.bndArr[Boundary].bndLine[t].Northing));
                            if (dist < minDist)
                            {
                                minDist = dist;
                                Closest = t;
                            }
                        }
                    }
                }

                if (isA && (minDist != double.PositiveInfinity))
                {
                    start = Closest;
                    end = 99999;
                    isA = false;
                }
                else if (minDist != double.PositiveInfinity)
                {
                    end = Closest;
                    isA = true;
                    if (HeadLand) isSet = true;
                    btnMakeABLine.Enabled = true;
                    btnMakeCurve.Enabled = true;

                    UpdateBoundary();
                }
                else
                {
                    start = end = 99999;
                    isA = true;
                    isSet = false;
                }

                if (start == 99999) lblStart.Text = "--";
                else lblStart.Text = start.ToString();
                if (end == 99999) lblEnd.Text = "--";
                else lblEnd.Text = end.ToString();
            }
            oglSelf.Refresh();
        }

        private void Next_Click(object sender, EventArgs e)
        {
            if (mf.bnd.bndArr.Count > 0)
            {
                if (HeadLand)
                {
                    if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
                    {
                        mf.hd.headArr[Boundary].HeadLine.Clear();
                        if (!ResetHeadLine)
                            mf.hd.headArr[Boundary].HeadLine.AddRange(Template);

                        mf.hd.headArr[Boundary].PreCalcHeadArea();
                        ResetHeadLine = false;
                    }

                    Boundary++;
                    if (Boundary > mf.bnd.bndArr.Count - 1) Boundary = 0;

                    RebuildHeadLineTemplate(false);
                }
                else
                {
                    btnMakeABLine.Enabled = btnMakeCurve.Enabled = btnCancelTouch.Enabled = false;

                    isA = true;
                    start = end = 99999;
                    if (start == 99999) lblStart.Text = "--";
                    else lblStart.Text = start.ToString();
                    if (end == 99999) lblEnd.Text = "--";
                    else lblEnd.Text = end.ToString();
                    Boundary++;
                    if (Boundary > mf.bnd.bndArr.Count - 1) Boundary = 0;

                    UpdateBoundary();
                }
            }
        }

        private void Previous_Click(object sender, EventArgs e)
        {
            if (mf.bnd.bndArr.Count > 0)
            {
                if (HeadLand)
                {
                    if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
                    {
                        mf.hd.headArr[Boundary].HeadLine.Clear();
                        if (!ResetHeadLine)
                            mf.hd.headArr[Boundary].HeadLine.AddRange(Template);
                    
                        mf.hd.headArr[Boundary].PreCalcHeadArea();
                        ResetHeadLine = false;
                    }

                    Boundary--;
                    if (Boundary < 0 || Boundary > mf.bnd.bndArr.Count - 1) Boundary = mf.bnd.bndArr.Count - 1;

                    RebuildHeadLineTemplate(false);
                }
                else
                {
                    btnMakeABLine.Enabled = btnMakeCurve.Enabled = btnCancelTouch.Enabled = false;
                    isA = true;
                    start = end = 99999;
                    if (start == 99999) lblStart.Text = "--";
                    else lblStart.Text = start.ToString();
                    if (end == 99999) lblEnd.Text = "--";
                    else lblEnd.Text = end.ToString();

                    Boundary--;
                    if (Boundary < 0 || Boundary > mf.bnd.bndArr.Count - 1) Boundary = mf.bnd.bndArr.Count - 1;
                    UpdateBoundary();
                }
            }
        }

        private void Label2_Click(object sender, EventArgs e)
        {
            mf.ABLines.CurrentEditLine = -1;
        }

        private void Label1_Click(object sender, EventArgs e)
        {
            mf.CurveLines.CurrentEditLine = -1;
        }

        private void BtnMakeCurve_Click(object sender, EventArgs e)
        {
            btnCancelTouch.Enabled = false;

            Button b = (Button)sender;
            bool test = b.Name == "btnMakeCurve";

            int ptCount = mf.bnd.bndArr[Boundary].bndLine.Count;
            if (test)
            {
                if (((ptCount - end + start) % ptCount) < ((ptCount - start + end) % ptCount)) { int index = start; start = end; end = index; }
                if (((ptCount - start + end) % ptCount) < 7) return;
            }
            else
            {
                start = 0;
                end = mf.bnd.bndArr[Boundary].bndLine.Count - 1;
                if (ptCount < 7) return;
            }

            mf.CurveLines.Lines.Add(new CCurveLines());
            int idx = mf.CurveLines.Lines.Count - 1;
            mf.CurveLines.CurrentEditLine = idx;

            mf.CurveLines.Lines[idx].SpiralMode = false;
            mf.CurveLines.Lines[idx].CircleMode = false;
            if (!test) mf.CurveLines.Lines[idx].BoundaryMode = true;
            else mf.CurveLines.Lines[idx].BoundaryMode = false;

            double offset = Math.Round(Offset * mf.metImp2m, 2);
            Vec3 point;
            bool Loop = start > end;
            for (int i = start; i <= end || Loop; i++)
            {
                if (i >= mf.bnd.bndArr[Boundary].bndLine.Count)
                {
                    i = 0;
                    Loop = false;
                }

                point.Northing = mf.bnd.bndArr[Boundary].bndLine[i].Northing + (Math.Sin(mf.bnd.bndArr[Boundary].bndLine[i].Heading) * -offset);
                point.Easting = mf.bnd.bndArr[Boundary].bndLine[i].Easting + (Math.Cos(mf.bnd.bndArr[Boundary].bndLine[i].Heading) * offset);
                point.Heading = mf.bnd.bndArr[Boundary].bndLine[i].Heading;

                bool Add = true;


                for (int t = 0; t < mf.bnd.bndArr[Boundary].bndLine.Count; t++)
                {
                    double dist = ((point.Easting - mf.bnd.bndArr[Boundary].bndLine[t].Easting) * (point.Easting - mf.bnd.bndArr[Boundary].bndLine[t].Easting)) + ((point.Northing - mf.bnd.bndArr[Boundary].bndLine[t].Northing) * (point.Northing - mf.bnd.bndArr[Boundary].bndLine[t].Northing));
                    if (dist < (offset * offset) - 0.01)
                    {
                        Add = false;
                        break;
                    }
                }

                if (Add) mf.CurveLines.Lines[idx].curvePts.Add(point);
            }

            //who knows which way it actually goes
            mf.CurveLines.Lines[idx].curvePts.CalculateRoundedCorner(0.5, mf.CurveLines.Lines[idx].BoundaryMode, 0.0436332, 5);

            //calculate average heading of line
            double x = 0, y = 0;

            foreach (var pt in mf.CurveLines.Lines[idx].curvePts)
            {
                x += Math.Cos(pt.Heading);
                y += Math.Sin(pt.Heading);
            }
            x /= mf.CurveLines.Lines[idx].curvePts.Count;
            y /= mf.CurveLines.Lines[idx].curvePts.Count;
            mf.CurveLines.Lines[idx].Heading = Math.Atan2(y, x);
            if (mf.CurveLines.Lines[idx].Heading < 0) mf.CurveLines.Lines[idx].Heading += Glm.twoPI;

            //build the tail extensions
            if (test) mf.CurveLines.AddFirstLastPoints();
            //mf.curve.SmoothAB(4);
            //mf.curve.CalculateTurnHeadings();
            //create a name
            mf.CurveLines.Lines[idx].Name = (Math.Round(Glm.ToDegrees(mf.CurveLines.Lines[idx].Heading), 1)).ToString(CultureInfo.InvariantCulture)
                 + "\u00B0" + mf.FindDirection(mf.CurveLines.Lines[idx].Heading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);


            mf.FileSaveCurveLines();

            //update the arrays
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;
            start = 99999; end = 99999;

            if (start == 99999) lblStart.Text = "--";
            else lblStart.Text = start.ToString();
            if (end == 99999) lblEnd.Text = "--";
            else lblEnd.Text = end.ToString();

            FixLabelsCurve();
            oglSelf.Refresh();
        }

        private void BtnMakeABLine_Click(object sender, EventArgs e)
        {
            btnCancelTouch.Enabled = false;

            //calculate the AB Heading
            double abHead = Math.Atan2(mf.bnd.bndArr[Boundary].bndLine[end].Easting - mf.bnd.bndArr[Boundary].bndLine[start].Easting, mf.bnd.bndArr[Boundary].bndLine[end].Northing - mf.bnd.bndArr[Boundary].bndLine[start].Northing);
            if (abHead < 0) abHead += Glm.twoPI;

            mf.ABLines.ABLines.Add(new CABLines());

            double offset = Math.Round(Offset * mf.metImp2m, 2);
            int idx = mf.ABLines.ABLines.Count - 1;
            mf.ABLines.ABLines[idx].Heading = abHead;
            mf.ABLines.ABLines[idx].ref1.Northing = (Math.Sin(abHead) * -offset) + mf.bnd.bndArr[Boundary].bndLine[start].Northing;
            mf.ABLines.ABLines[idx].ref1.Easting = (Math.Cos(abHead) * offset) + mf.bnd.bndArr[Boundary].bndLine[start].Easting;
            mf.ABLines.ABLines[idx].ref2.Northing = (Math.Sin(abHead) * -offset) + mf.bnd.bndArr[Boundary].bndLine[end].Northing;
            mf.ABLines.ABLines[idx].ref2.Easting = (Math.Cos(abHead) * offset) + mf.bnd.bndArr[Boundary].bndLine[end].Easting;
            mf.ABLines.ABLines[idx].UsePoint = true;

            //create a name
            mf.ABLines.ABLines[idx].Name = (Math.Round(Glm.ToDegrees(mf.ABLines.ABLines[idx].Heading), 1)).ToString(CultureInfo.InvariantCulture)
                 + "\u00B0" + mf.FindDirection(mf.ABLines.ABLines[idx].Heading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;
            start = 99999; end = 99999;
            if (start == 99999) lblStart.Text = "--";
            else lblStart.Text = start.ToString();
            if (end == 99999) lblEnd.Text = "--";
            else lblEnd.Text = end.ToString();

            FixLabelsABLine();
            oglSelf.Refresh();
        }

        private void OglSelf_Paint(object sender, PaintEventArgs e)
        {
            oglSelf.MakeCurrent();

            GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.ColorBufferBit);
            GL.LoadIdentity();                  // Reset The View

            //back the camera up
            GL.Translate(0, 0, -maxFieldDistance);

            //translate to that spot in the world
            GL.Translate(-fieldCenterX, -fieldCenterY, 0);

            GL.Color3(1, 1, 1);

            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                if (Boundary == i) GL.Color3(1.0f, 0.0f, 0.0f);
                else GL.Color3(0.95f, 0.5f, 0.250f);
                mf.bnd.bndArr[i].DrawBoundaryLine();
            }

            if (isDrawSections)
            {
                mf.CalcFrustum();
                mf.DrawPatchList(8);
                mf.DrawSectionsPatchList(true);
            }

            //the vehicle
            GL.PointSize(8.0f);
            GL.Begin(PrimitiveType.Points);
            GL.Color3(0.95f, 0.90f, 0.0f);
            Vec3 pivot = mf.pivotAxlePos;
            GL.Vertex3(pivot.Easting, pivot.Northing, 0.0);
            GL.End();

            if (HeadLand)
            {
                if (Template.Count > 0)
                {
                    GL.LineWidth(1);
                    GL.Color3(0.20f, 0.96232f, 0.30f);
                    GL.PointSize(2);
                    for (int h = 0; h < Template.Count; h++)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        for (int i = 0; i < Template[h].Count; i++) GL.Vertex3(Template[h][i].Easting, Template[h][i].Northing, 0);
                        GL.End();
                    }

                    GL.PointSize(6);
                    if (TemplateIndex < Template.Count)
                    {
                        GL.Begin(PrimitiveType.Points);
                        GL.Color3(0.990, 0.00, 0.250);
                        if (start != 99999 && start < Template[TemplateIndex].Count) GL.Vertex3(Template[TemplateIndex][start].Easting, Template[TemplateIndex][start].Northing, 0);
                        GL.Color3(0.990, 0.960, 0.250);
                        if (end != 99999 && end < Template[TemplateIndex].Count) GL.Vertex3(Template[TemplateIndex][end].Easting, Template[TemplateIndex][end].Northing, 0);
                        GL.End();

                        if (start != 99999 && end != 99999)
                        {
                            GL.Color3(0.965, 0.250, 0.950);
                            GL.LineWidth(2.0f);
                            int ptCount = Template[TemplateIndex].Count;
                            if (ptCount < 1) return;

                            int start2 = Math.Min(start, Template[TemplateIndex].Count - 1);
                            int end2 = Math.Min(end, Template[TemplateIndex].Count);
                            if (((Template[TemplateIndex].Count - end2 + start2) % Template[TemplateIndex].Count) < ((Template[TemplateIndex].Count - start2 + end2) % Template[TemplateIndex].Count)) { int index = start2; start2 = end2; end2 = index; }
                            bool Loop = start2 > end2;


                            GL.Begin(PrimitiveType.LineStrip);
                            for (int i = start2; i <= end2 || Loop; i++)
                            {
                                if (i > Template[TemplateIndex].Count)
                                {
                                    i = 0;
                                    Loop = false;
                                }
                                if (i < Template[TemplateIndex].Count) GL.Vertex3(Template[TemplateIndex][i].Easting, Template[TemplateIndex][i].Northing, 0);
                            }
                            GL.End();
                        }
                    }
                }
            }
            else
            {
                GL.PointSize(8);
                GL.Begin(PrimitiveType.Points);

                GL.Color3(0.95, 0.950, 0.0);
                if (start != 99999) GL.Vertex3(mf.bnd.bndArr[Boundary].bndLine[start].Easting, mf.bnd.bndArr[Boundary].bndLine[start].Northing, 0);

                GL.Color3(0.950, 096.0, 0.0);
                if (end != 99999) GL.Vertex3(mf.bnd.bndArr[Boundary].bndLine[end].Easting, mf.bnd.bndArr[Boundary].bndLine[end].Northing, 0);
                GL.End();

                //draw the actual built lines
                if (start == 99999 && end == 99999)
                {
                    GL.LineWidth(2);
                    GL.Enable(EnableCap.LineStipple);
                    GL.LineStipple(1, 0x0707);

                    int numLines = mf.ABLines.ABLines.Count;
                    if (numLines > 0)
                    {
                        GL.Color3(1.0f, 0.0f, 0.0f);
                        for (int i = 0; i < numLines; i++)
                        {
                            if (mf.ABLines.CurrentEditLine != i)
                            {
                                GL.Begin(PrimitiveType.Lines);

                                GL.Vertex2(mf.ABLines.ABLines[i].ref1.Easting + Math.Sin(mf.ABLines.ABLines[i].Heading) * 4000,
                                    mf.ABLines.ABLines[i].ref1.Northing + Math.Cos(mf.ABLines.ABLines[i].Heading) * 4000);
                                if (mf.ABLines.ABLines[i].UsePoint) GL.Vertex2(mf.ABLines.ABLines[i].ref2.Easting + Math.Sin(mf.ABLines.ABLines[i].Heading) * -4000,
                                    mf.ABLines.ABLines[i].ref2.Northing + Math.Cos(mf.ABLines.ABLines[i].Heading) * -4000);
                                else GL.Vertex2(mf.ABLines.ABLines[i].ref1.Easting + Math.Sin(mf.ABLines.ABLines[i].Heading) * -4000,
                                    mf.ABLines.ABLines[i].ref1.Northing + Math.Cos(mf.ABLines.ABLines[i].Heading) * -4000);
                                GL.End();
                            }
                        }
                    }

                    int numCurves = mf.CurveLines.Lines.Count;
                    if (numCurves > 0)
                    {
                        GL.Color3(0.0f, 1.0f, 0.0f);
                        for (int i = 0; i < numCurves; i++)
                        {
                            if (mf.CurveLines.CurrentEditLine != i)
                            {
                                GL.Begin(PrimitiveType.LineStrip);
                                foreach (Vec3 item in mf.CurveLines.Lines[i].curvePts)
                                {
                                    GL.Vertex3(item.Easting, item.Northing, 0);
                                }
                                GL.End();
                            }
                        }
                    }
                    GL.Disable(EnableCap.LineStipple);

                    GL.LineWidth(4);
                    if (mf.ABLines.CurrentEditLine > -1)
                    {
                        GL.Color3(1.0f, 0.0f, 0.0f);
                        GL.Begin(PrimitiveType.Lines);
                        GL.Vertex2(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Easting + Math.Sin(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * 4000,
                            mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Northing + Math.Cos(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * 4000);
                        if (mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].UsePoint) GL.Vertex2(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref2.Easting + Math.Sin(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * -4000,
                            mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref2.Northing + Math.Cos(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * -4000);
                        else GL.Vertex2(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Easting + Math.Sin(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * -4000,
                            mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Northing + Math.Cos(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * -4000);
                        GL.End();
                    }

                    if (mf.CurveLines.CurrentEditLine > -1)
                    {
                        GL.Color3(0.0f, 1.0f, 0.0f);
                        GL.Begin(PrimitiveType.LineStrip);
                        foreach (Vec3 item in mf.CurveLines.Lines[mf.CurveLines.CurrentEditLine].curvePts)
                        {
                            GL.Vertex3(item.Easting, item.Northing, 0);
                        }
                        GL.End();
                    }
                }
            }

            GL.Flush();
            oglSelf.SwapBuffers();
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            mf.FileSaveABLines();
            mf.FileSaveCurveLines();
            Close();
        }

        private void OglSelf_Resize(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();

            //58 degrees view
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, 1.0f, 20000);
            GL.LoadMatrix(ref mat);

            GL.MatrixMode(MatrixMode.Modelview);
        }

        private void OglSelf_Load(object sender, EventArgs e)
        {
            oglSelf.MakeCurrent();
            GL.Enable(EnableCap.CullFace);
            GL.CullFace(CullFaceMode.Back);
            GL.ClearColor(0.23122f, 0.2318f, 0.2315f, 1.0f);
        }

        //determine mins maxs of patches and whole field.
        private void CalculateMinMax()
        {
            if (mf.bnd.bndArr.Count > 0)
            {
                //the largest distancew across field
                double dist = Math.Abs(mf.bnd.bndArr[0].Eastingmin - mf.bnd.bndArr[0].Eastingmax);
                double dist2 = Math.Abs(mf.bnd.bndArr[0].Northingmin - mf.bnd.bndArr[0].Northingmax);

                if (dist > dist2) maxFieldDistance = dist;
                else maxFieldDistance = dist2;

                if (maxFieldDistance < 100) maxFieldDistance = 100;
                if (maxFieldDistance > 19900) maxFieldDistance = 19900;
                //lblMax.Text = ((int)maxFieldDistance).ToString();

                fieldCenterX = (mf.bnd.bndArr[0].Eastingmax + mf.bnd.bndArr[0].Eastingmin) / 2.0;
                fieldCenterY = (mf.bnd.bndArr[0].Northingmax + mf.bnd.bndArr[0].Northingmin) / 2.0;
            }
        }
    }
}