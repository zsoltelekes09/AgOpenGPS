using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using System.Threading;
using System.Threading.Tasks;

namespace AgOpenGPS
{
    public partial class FormABDraw : Form
    {
        //access to the main GPS form and all its variables
        private readonly FormGPS mf;

        private double fieldCenterX, fieldCenterY, maxFieldDistance, Offset;
        private bool isA = true, isSet = false, ResetHeadLine = false, isDrawSections = false, Changed = false, NeedFixHeading = false;
        private readonly bool HeadLand;
        private int Start = -1, End = -1, Boundary = -1, TemplateIndex;


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
            Start = End = -1;
            if (Start == -1) lblStart.Text = "--";
            else lblStart.Text = Start.ToString();
            if (End == -1) lblEnd.Text = "--";
            else lblEnd.Text = End.ToString();

            btnCancelTouch.Enabled = false;
            oglSelf.Refresh();
        }

        private void BtnDeleteCurve_Click(object sender, EventArgs e)
        {
            if (mf.CurveLines.CurrentEditLine < mf.CurveLines.Lines.Count && mf.CurveLines.CurrentEditLine > -1)
            {
                mf.CurveLines.Lines.RemoveAt(mf.CurveLines.CurrentEditLine);
                if (mf.CurveLines.CurrentEditLine < mf.CurveLines.CurrentLine)
                {
                    mf.CurveLines.ResetABLine = true;
                    mf.CurveLines.CurrentLine--;
                    mf.CurveLines.GuidanceLines.Clear();
                }
                if (mf.CurveLines.CurrentEditLine == mf.CurveLines.CurrentLine)
                {
                    mf.CurveLines.ResetABLine = true;
                    mf.CurveLines.CurrentLine = -1;
                    mf.CurveLines.GuidanceLines.Clear();
                }
                mf.CurveLines.CurrentEditLine--;

                Properties.Settings.Default.LastCurveLine = mf.CurveLines.CurrentLine;
                Properties.Settings.Default.Save();
            }

            FixLabelsCurve();
            oglSelf.Refresh();
        }

        private void BtnDeleteABLine_Click(object sender, EventArgs e)
        {
            if (mf.ABLines.CurrentEditLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentEditLine > -1)
            {
                mf.ABLines.ABLines.RemoveAt(mf.ABLines.CurrentEditLine);
                if (mf.ABLines.CurrentEditLine < mf.ABLines.CurrentLine)
                {
                    mf.ABLines.ResetABLine = true;
                    mf.ABLines.CurrentLine--;
                }
                if (mf.ABLines.CurrentEditLine == mf.ABLines.CurrentLine)
                {
                    mf.ABLines.ResetABLine = true;
                    mf.ABLines.CurrentLine = -1;
                }
                

                mf.ABLines.CurrentEditLine--;

                Properties.Settings.Default.LastABLine = mf.ABLines.CurrentLine;
                Properties.Settings.Default.Save();
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
            if (mf.bnd.bndArr.Count > Boundary && Boundary > -1)
            {
                mf.bnd.bndArr[Boundary].Template.Clear();
                if (!Reset && mf.bnd.bndArr[Boundary].HeadLine.Count > 0)
                {
                    mf.bnd.bndArr[Boundary].Template.AddRange(mf.bnd.bndArr[Boundary].HeadLine);
                    ResetHeadLine = false;
                }
                else
                {
                    mf.bnd.bndArr[Boundary].Template.Add(new List<Vec3>(mf.bnd.bndArr[Boundary].bndLine));
                    ResetHeadLine = true;
                }

                Changed = !Reset;
                NeedFixHeading = !Reset;

                UpdateBoundary();
            }
            Start = End = -1;
            if (Start == -1) lblStart.Text = "--";
            else lblStart.Text = Start.ToString();
            if (End == -1) lblEnd.Text = "--";
            else lblEnd.Text = End.ToString();
            isA = true;
            isSet = false;
            TemplateIndex = 0;
        }

        private void BtnDeletePoints_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            Changed = true;
            NeedFixHeading = true;
            if (TemplateIndex <= mf.bnd.bndArr[Boundary].Template.Count && Start >= 0 && End >= 0)
            {
                int start2 = Start;
                int end2 = End;

                if (((mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count - end2 + start2) % mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count) < ((mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count - start2 + end2) % mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count)) { int index = start2; start2 = end2; end2 = index; }

                if (start2 > end2)
                {
                    mf.bnd.bndArr[Boundary].Template[TemplateIndex].RemoveRange(start2, mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count - start2);
                    mf.bnd.bndArr[Boundary].Template[TemplateIndex].RemoveRange(0, end2);
                }
                else
                {
                    mf.bnd.bndArr[Boundary].Template[TemplateIndex].RemoveRange(start2, end2 - start2);
                }
            }

            Start = End = -1;
            if (Start == -1) lblStart.Text = "--";
            else lblStart.Text = Start.ToString();
            if (End == -1) lblEnd.Text = "--";
            else lblEnd.Text = End.ToString();
            isA = true;
            isSet = false;
            UpdateBoundary();
        }

        private void BtnDoneManualMove_Click(object sender, EventArgs e)
        {
            if (NeedFixHeading)
            {
                NeedFixHeading = false;
                StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.FixHeading, Offset, TemplateIndex, Boundary, Start, End);
            }

            Start = End = -1;
            if (Start == -1) lblStart.Text = "--";
            else lblStart.Text = Start.ToString();
            if (End == -1) lblEnd.Text = "--";
            else lblEnd.Text = End.ToString();
            isA = true;
            isSet = false;
            UpdateBoundary();
        }

        private void BtnTurnOffHeadland_Click(object sender, EventArgs e)
        {
            mf.bnd.bndArr[Boundary].Template.Clear();
            Close();
        }

        private void BtnExit2_Click(object sender, EventArgs e)
        {
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                if (NeedFixHeading)
                {
                    NeedFixHeading = false;
                    StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.FixHeading, Offset, TemplateIndex, Boundary, Start, End);
                }

                if (Changed || ResetHeadLine)
                {
                    StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.Save, 0, ResetHeadLine ? 1 : 0, 0, 0, 0);
                }
                ResetHeadLine = false;
            }
            DialogResult = DialogResult.Yes;
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
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                NeedFixHeading = true;
                StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.Up, Offset, TemplateIndex, Boundary, Start, End);
                Changed = true;
            }
        }

        private void BtnMoveDown_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                NeedFixHeading = true;
                StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.Down, Offset, TemplateIndex, Boundary, Start, End);
                Changed = true;
            }
        }

        private void BtnMoveLeft_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                NeedFixHeading = true;
                StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.Left, Offset, TemplateIndex, Boundary, Start, End);
                Changed = true;
            }
        }

        private void BtnMoveRight_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                NeedFixHeading = true;
                StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.Right, Offset, TemplateIndex, Boundary, Start, End);
                Changed = true;
            }
        }

        private void BtnMakeFixedHeadland_Click(object sender, EventArgs e)
        {
            ResetHeadLine = false;
            if (mf.bnd.bndArr.Count > Boundary && Boundary >= 0)
            {
                StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.Offset, Offset, TemplateIndex, Boundary, Start, End);
                Changed = true;
            }

        }

        private void StartTask_FixHead(CBoundaryLines BoundaryLine, HeadLandTaskName HeadLandAction, double Offset, int Idx, int Boundary, int start, int end)
        {
            List<Task> tasks = new List<Task>();
            CancellationTokenSource newtoken = new CancellationTokenSource();
            for (int j = 0; j < mf.TaskList.Count; j++)
            {
                if (mf.TaskList[j].Task.IsCompleted)
                {
                    mf.TaskList.RemoveAt(j);
                    j--;
                }
                //{ OpenJob, CloseJob, Save, Delete     FixBnd, FixHead,    HeadLand     Boundary,     TurnLine, GeoFence, Triangulate, MinMax }
                else if (mf.TaskList[j].TaskName == TaskName.OpenJob || mf.TaskList[j].TaskName == TaskName.CloseJob || mf.TaskList[j].TaskName == TaskName.Save)
                {
                    tasks.Add(mf.TaskList[j].Task);
                }
                else if (mf.TaskList[j].Idx == BoundaryLine)
                {
                    if (mf.TaskList[j].TaskName == TaskName.Delete)
                    {
                        return;
                    }
                    else if (mf.TaskList[j].TaskName == TaskName.FixHead || mf.TaskList[j].TaskName == TaskName.FixBnd)
                    {
                        tasks.Add(mf.TaskList[j].Task);
                    }
                    else if (mf.TaskList[j].TaskName == TaskName.HeadLand)
                    {
                        tasks.Add(mf.TaskList[j].Task);
                        mf.TaskList[j].Token.Cancel();
                    }
                }
            }

            Task NewTask = mf.Task_FixHeadLand(BoundaryLine, HeadLandAction, Offset, Idx, Boundary, start, end, tasks, newtoken.Token);
            mf.TaskList.Add(new TaskClass(NewTask, BoundaryLine, TaskName.FixHead, newtoken));

            Awaittask(NewTask);
        }

        public async void Awaittask(Task task)
        {
            await Task.WhenAll(task);
            UpdateBoundary();
        }


        private void BtnEndUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateIndex <= mf.bnd.bndArr[Boundary].Template.Count)
            {
                if (End != -1)
                {
                    End = (End + 1).Clamp(mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count);
                }
                if (End == -1) lblEnd.Text = "--";
                else lblEnd.Text = End.ToString();
            }

            UpdateBoundary();
        }

        private void BtnEndDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateIndex <= mf.bnd.bndArr[Boundary].Template.Count)
            {
                if (End != -1)
                {
                    End = (End - 1).Clamp(mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count);
                }
                if (End == -1) lblEnd.Text = "--";
                else lblEnd.Text = End.ToString();
            }
            UpdateBoundary();
        }

        private void BtnStartUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateIndex <= mf.bnd.bndArr[Boundary].Template.Count)
            {
                if (Start != -1)
                {
                    Start = (Start + 1).Clamp(mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count);
                }
                if (Start == -1) lblStart.Text = "--";
                else lblStart.Text = Start.ToString();
            }
            UpdateBoundary();
        }

        private void BtnStartDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (TemplateIndex <= mf.bnd.bndArr[Boundary].Template.Count)
            {
                if (Start != -1)
                {
                    Start = (Start - 1).Clamp(mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count);
                }

                if (Start == -1) lblStart.Text = "--";
                else lblStart.Text = Start.ToString();
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
                    if (mf.bnd.bndArr[Boundary].Template.Count > 0)
                    {
                        if (isA)
                        {
                            for (int i = 0; i < mf.bnd.bndArr[Boundary].Template.Count; i++)
                            {
                                for (int j = 0; j < mf.bnd.bndArr[Boundary].Template[i].Count; j++)
                                {
                                    double dist = ((pint.Easting - mf.bnd.bndArr[Boundary].Template[i][j].Easting) * (pint.Easting - mf.bnd.bndArr[Boundary].Template[i][j].Easting))
                                                    + ((pint.Northing - mf.bnd.bndArr[Boundary].Template[i][j].Northing) * (pint.Northing - mf.bnd.bndArr[Boundary].Template[i][j].Northing));
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
                            for (int j = 0; j < mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count; j++)
                            {
                                double dist = ((pint.Easting - mf.bnd.bndArr[Boundary].Template[TemplateIndex][j].Easting) * (pint.Easting - mf.bnd.bndArr[Boundary].Template[TemplateIndex][j].Easting))
                                                + ((pint.Northing - mf.bnd.bndArr[Boundary].Template[TemplateIndex][j].Northing) * (pint.Northing - mf.bnd.bndArr[Boundary].Template[TemplateIndex][j].Northing));
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
                    Start = Closest;
                    End = -1;
                    isA = false;
                }
                else if (minDist != double.PositiveInfinity)
                {
                    End = Closest;
                    isA = true;
                    if (HeadLand) isSet = true;
                    btnMakeABLine.Enabled = true;
                    btnMakeCurve.Enabled = true;

                    UpdateBoundary();
                }
                else
                {
                    Start = End = -1;
                    isA = true;
                    isSet = false;
                }

                if (Start == -1) lblStart.Text = "--";
                else lblStart.Text = Start.ToString();
                if (End == -1) lblEnd.Text = "--";
                else lblEnd.Text = End.ToString();
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
                        if (NeedFixHeading)
                        {
                            NeedFixHeading = false;
                            StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.FixHeading, Offset, TemplateIndex, Boundary, Start, End);
                        }

                        if (Changed || ResetHeadLine)
                        {
                            StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.Save, 0, ResetHeadLine ? 1 : 0, 0, 0, 0);

                            ResetHeadLine = false;
                        }
                    }

                    Boundary++;
                    if (Boundary > mf.bnd.bndArr.Count - 1) Boundary = 0;

                    RebuildHeadLineTemplate(false);
                }
                else
                {
                    btnMakeABLine.Enabled = btnMakeCurve.Enabled = btnCancelTouch.Enabled = false;

                    isA = true;
                    Start = End = -1;
                    if (Start == -1) lblStart.Text = "--";
                    else lblStart.Text = Start.ToString();
                    if (End == -1) lblEnd.Text = "--";
                    else lblEnd.Text = End.ToString();
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
                        if (NeedFixHeading)
                        {
                            NeedFixHeading = false;
                            StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.FixHeading, Offset, TemplateIndex, Boundary, Start, End);
                        }

                        if (Changed || ResetHeadLine)
                        {
                            StartTask_FixHead(mf.bnd.bndArr[Boundary], HeadLandTaskName.Save, 0, ResetHeadLine ? 1 : 0, 0, 0, 0);
                            ResetHeadLine = false;
                        }
                    }

                    Boundary--;
                    if (Boundary < 0 || Boundary > mf.bnd.bndArr.Count - 1) Boundary = mf.bnd.bndArr.Count - 1;

                    RebuildHeadLineTemplate(false);
                }
                else
                {
                    btnMakeABLine.Enabled = btnMakeCurve.Enabled = btnCancelTouch.Enabled = false;
                    isA = true;
                    Start = End = -1;
                    if (Start == -1) lblStart.Text = "--";
                    else lblStart.Text = Start.ToString();
                    if (End == -1) lblEnd.Text = "--";
                    else lblEnd.Text = End.ToString();

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
                if (((ptCount - End + Start) % ptCount) < ((ptCount - Start + End) % ptCount)) { int index = Start; Start = End; End = index; }
                if (((ptCount - Start + End) % ptCount) < 7) return;
            }
            else
            {
                Start = 0;
                End = mf.bnd.bndArr[Boundary].bndLine.Count - 1;
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
            bool Loop = Start > End;
            for (int i = Start; i <= End || Loop; i++)
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
            mf.CurveLines.Lines[idx].curvePts.CalculateRoundedCorner(0.5, mf.CurveLines.Lines[idx].BoundaryMode, 0.0436332, CancellationToken.None);

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
            Start = -1; End = -1;

            if (Start == -1) lblStart.Text = "--";
            else lblStart.Text = Start.ToString();
            if (End == -1) lblEnd.Text = "--";
            else lblEnd.Text = End.ToString();

            FixLabelsCurve();
            oglSelf.Refresh();
        }

        private void BtnMakeABLine_Click(object sender, EventArgs e)
        {
            btnCancelTouch.Enabled = false;

            //calculate the AB Heading
            double abHead = Math.Atan2(mf.bnd.bndArr[Boundary].bndLine[End].Easting - mf.bnd.bndArr[Boundary].bndLine[Start].Easting, mf.bnd.bndArr[Boundary].bndLine[End].Northing - mf.bnd.bndArr[Boundary].bndLine[Start].Northing);
            if (abHead < 0) abHead += Glm.twoPI;

            mf.ABLines.ABLines.Add(new CABLines());

            double offset = Math.Round(Offset * mf.metImp2m, 2);
            int idx = mf.ABLines.ABLines.Count - 1;
            mf.ABLines.ABLines[idx].Heading = abHead;
            mf.ABLines.ABLines[idx].ref1.Northing = (Math.Sin(abHead) * -offset) + mf.bnd.bndArr[Boundary].bndLine[Start].Northing;
            mf.ABLines.ABLines[idx].ref1.Easting = (Math.Cos(abHead) * offset) + mf.bnd.bndArr[Boundary].bndLine[Start].Easting;
            mf.ABLines.ABLines[idx].ref2.Northing = (Math.Sin(abHead) * -offset) + mf.bnd.bndArr[Boundary].bndLine[End].Northing;
            mf.ABLines.ABLines[idx].ref2.Easting = (Math.Cos(abHead) * offset) + mf.bnd.bndArr[Boundary].bndLine[End].Easting;
            mf.ABLines.ABLines[idx].UsePoint = true;

            //create a name
            mf.ABLines.ABLines[idx].Name = (Math.Round(Glm.ToDegrees(mf.ABLines.ABLines[idx].Heading), 1)).ToString(CultureInfo.InvariantCulture)
                 + "\u00B0" + mf.FindDirection(mf.ABLines.ABLines[idx].Heading) + DateTime.Now.ToString("hh:mm:ss", CultureInfo.InvariantCulture);

            //clean up gui
            btnMakeABLine.Enabled = false;
            btnMakeCurve.Enabled = false;
            Start = -1; End = -1;
            if (Start == -1) lblStart.Text = "--";
            else lblStart.Text = Start.ToString();
            if (End == -1) lblEnd.Text = "--";
            else lblEnd.Text = End.ToString();

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
                if (mf.bnd.bndArr[Boundary].Template.Count > 0)
                {
                    GL.LineWidth(1);
                    GL.Color3(0.20f, 0.96232f, 0.30f);
                    GL.PointSize(2);
                    for (int h = 0; h < mf.bnd.bndArr[Boundary].Template.Count; h++)
                    {
                        GL.Begin(PrimitiveType.LineLoop);
                        for (int i = 0; i < mf.bnd.bndArr[Boundary].Template[h].Count; i++) GL.Vertex3(mf.bnd.bndArr[Boundary].Template[h][i].Easting, mf.bnd.bndArr[Boundary].Template[h][i].Northing, 0);
                        GL.End();
                    }

                    GL.PointSize(6);
                    if (TemplateIndex < mf.bnd.bndArr[Boundary].Template.Count)
                    {
                        GL.Begin(PrimitiveType.Points);
                        GL.Color3(0.990, 0.00, 0.250);
                        if (Start != -1 && Start < mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count) GL.Vertex3(mf.bnd.bndArr[Boundary].Template[TemplateIndex][Start].Easting, mf.bnd.bndArr[Boundary].Template[TemplateIndex][Start].Northing, 0);
                        GL.Color3(0.990, 0.960, 0.250);
                        if (End != -1 && End < mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count) GL.Vertex3(mf.bnd.bndArr[Boundary].Template[TemplateIndex][End].Easting, mf.bnd.bndArr[Boundary].Template[TemplateIndex][End].Northing, 0);
                        GL.End();

                        if (Start != -1 && End != -1)
                        {
                            GL.Color3(0.965, 0.250, 0.950);
                            GL.LineWidth(2.0f);
                            int ptCount = mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count;
                            if (ptCount < 1) return;

                            int start2 = Math.Min(Start, mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count - 1);
                            int end2 = Math.Min(End, mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count);
                            if (((mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count - end2 + start2) % mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count) < ((mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count - start2 + end2) % mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count)) { int index = start2; start2 = end2; end2 = index; }
                            bool Loop = start2 > end2;


                            GL.Begin(PrimitiveType.LineStrip);
                            for (int i = start2; i <= end2 || Loop; i++)
                            {
                                if (i > mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count)
                                {
                                    i = 0;
                                    Loop = false;
                                }
                                if (i < mf.bnd.bndArr[Boundary].Template[TemplateIndex].Count) GL.Vertex3(mf.bnd.bndArr[Boundary].Template[TemplateIndex][i].Easting, mf.bnd.bndArr[Boundary].Template[TemplateIndex][i].Northing, 0);
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
                if (Start != -1) GL.Vertex3(mf.bnd.bndArr[Boundary].bndLine[Start].Easting, mf.bnd.bndArr[Boundary].bndLine[Start].Northing, 0);

                GL.Color3(0.950, 096.0, 0.0);
                if (End != -1) GL.Vertex3(mf.bnd.bndArr[Boundary].bndLine[End].Easting, mf.bnd.bndArr[Boundary].bndLine[End].Northing, 0);
                GL.End();

                //draw the actual built lines
                if (Start == -1 && End == -1)
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

                                GL.Vertex2(mf.ABLines.ABLines[i].ref1.Easting + Math.Sin(mf.ABLines.ABLines[i].Heading) * mf.maxCrossFieldLength,
                                    mf.ABLines.ABLines[i].ref1.Northing + Math.Cos(mf.ABLines.ABLines[i].Heading) * mf.maxCrossFieldLength);
                                if (mf.ABLines.ABLines[i].UsePoint) GL.Vertex2(mf.ABLines.ABLines[i].ref2.Easting + Math.Sin(mf.ABLines.ABLines[i].Heading) * -mf.maxCrossFieldLength,
                                    mf.ABLines.ABLines[i].ref2.Northing + Math.Cos(mf.ABLines.ABLines[i].Heading) * -mf.maxCrossFieldLength);
                                else GL.Vertex2(mf.ABLines.ABLines[i].ref1.Easting + Math.Sin(mf.ABLines.ABLines[i].Heading) * -mf.maxCrossFieldLength,
                                    mf.ABLines.ABLines[i].ref1.Northing + Math.Cos(mf.ABLines.ABLines[i].Heading) * -mf.maxCrossFieldLength);
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
                        GL.Vertex2(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Easting + Math.Sin(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * mf.maxCrossFieldLength,
                            mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Northing + Math.Cos(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * mf.maxCrossFieldLength);
                        if (mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].UsePoint) GL.Vertex2(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref2.Easting + Math.Sin(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * -mf.maxCrossFieldLength,
                            mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref2.Northing + Math.Cos(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * -mf.maxCrossFieldLength);
                        else GL.Vertex2(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Easting + Math.Sin(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * -mf.maxCrossFieldLength,
                            mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].ref1.Northing + Math.Cos(mf.ABLines.ABLines[mf.ABLines.CurrentEditLine].Heading) * -mf.maxCrossFieldLength);
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
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView(1.01f, 1.0f, (float)(maxFieldDistance - 10), (float)(maxFieldDistance + 10));
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
                //if (maxFieldDistance > 19900) maxFieldDistance = 19900;
                //lblMax.Text = ((int)maxFieldDistance).ToString();

                fieldCenterX = (mf.bnd.bndArr[0].Eastingmax + mf.bnd.bndArr[0].Eastingmin) / 2.0;
                fieldCenterY = (mf.bnd.bndArr[0].Northingmax + mf.bnd.bndArr[0].Northingmin) / 2.0;
            }
        }
    }
}