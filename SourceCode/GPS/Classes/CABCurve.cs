using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AgOpenGPS
{
    public class CABCurve
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        //flag for starting stop adding points
        public bool BtnCurveLineOn, isOkToAddPoints;
        public double distanceFromRefLine;

        public bool ResetABLine = false;
        public int HowManyPathsAway, OldHowManyPathsAway;
        public bool isSmoothWindowOpen, isSameWay, OldisSameWay;

        public int A, B, CurrentLine = -1, CurrentEditLine = -1, tryoutcurve = -1;

        //the list of points of curve to drive on
        public List<Vec3> curList = new List<Vec3>();
        public List<Vec3> smooList = new List<Vec3>();
        public List<CCurveLines> Lines = new List<CCurveLines>();

        public bool isEditing;
        public List<List<List<Vec3>>> GuidanceLines = new List<List<List<Vec3>>>();

        public CABCurve(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public void DrawCurve()
        {
            int ptCount;
            if (tryoutcurve > -1 && tryoutcurve < Lines.Count)
            {
                GL.LineWidth(mf.ABLines.lineWidth * 2);
                GL.Color3(1.0f, 0.0f, 0.0f);
                if (Lines[tryoutcurve].BoundaryMode) GL.Begin(PrimitiveType.LineLoop);
                else GL.Begin(PrimitiveType.LineStrip);
                for (int h = 0; h < Lines[tryoutcurve].curvePts.Count; h++) GL.Vertex3(Lines[tryoutcurve].curvePts[h].Easting, Lines[tryoutcurve].curvePts[h].Northing, 0);
                GL.End();
                return;
            }
            else if (isOkToAddPoints && CurrentEditLine < Lines.Count && CurrentEditLine > -1)//draw the last line to tractor
            {
                ptCount = Lines[CurrentEditLine].curvePts.Count;
                if (ptCount > 0)
                {
                    GL.Color3(0.930f, 0.0692f, 0.260f);
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < ptCount; h++) GL.Vertex3(Lines[CurrentEditLine].curvePts[h].Easting, Lines[CurrentEditLine].curvePts[h].Northing, 0);

                    Vec3 pivot = mf.pivotAxlePos;
                    GL.Vertex3(Lines[CurrentEditLine].curvePts[ptCount - 1].Easting, Lines[CurrentEditLine].curvePts[ptCount - 1].Northing, 0);
                    GL.Vertex3(pivot.Easting, pivot.Northing, 0);
                    GL.End();
                }
            }
            else if (isSmoothWindowOpen)
            {
                ptCount = smooList.Count;
                if (smooList.Count == 0) return;

                GL.LineWidth(mf.ABLines.lineWidth);
                GL.Color3(0.930f, 0.92f, 0.260f);
                GL.Begin(PrimitiveType.Lines);
                for (int h = 0; h < ptCount; h++) GL.Vertex3(smooList[h].Easting, smooList[h].Northing, 0);
                GL.End();
            }
            else
            {
                if (CurrentLine < Lines.Count && CurrentLine > -1)
                {
                    ptCount = Lines[CurrentLine].curvePts.Count;
                    if (ptCount < 1) return;

                    GL.Color3(0.96, 0.2f, 0.2f);

                    //original line

                    if (Lines[CurrentLine].BoundaryMode) GL.Begin(PrimitiveType.LineLoop);
                    else GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < ptCount; h++) GL.Vertex3(Lines[CurrentLine].curvePts[h].Easting, Lines[CurrentLine].curvePts[h].Northing, 0);
                    GL.End();


                    if (isEditing)
                    {
                        ptCount = Lines[CurrentLine].curvePts.Count;
                        GL.Color3(0.8f, 0.3f, 0.2f);
                        GL.PointSize(2);
                        GL.Begin(PrimitiveType.Points);

                        for (int i = 0; i <= 6; i++)
                        {
                            for (int h = 0; h < ptCount; h++)
                                GL.Vertex3((Math.Cos(-Lines[CurrentLine].curvePts[h].Heading) * mf.Guidance.WidthMinusOverlap * i) + Lines[CurrentLine].curvePts[h].Easting,
                                              (Math.Sin(-Lines[CurrentLine].curvePts[h].Heading) * mf.Guidance.WidthMinusOverlap * i) + Lines[CurrentLine].curvePts[h].Northing, 0);
                        }

                        GL.End();
                        return;
                    }

                    if (mf.isSideGuideLines)
                    {
                        GL.Color3(0.56f, 0.650f, 0.650f);
                        GL.Enable(EnableCap.LineStipple);
                        GL.LineStipple(1, 0x0101);

                        GL.LineWidth(mf.ABLines.lineWidth);

                        for (int i = 0; i < GuidanceLines.Count; i++)
                        {
                            for (int j = 0; j < GuidanceLines[i].Count; j++)
                            {
                                if (GuidanceLines[i][j].Count > 0)
                                {
                                    if (Lines[CurrentLine].BoundaryMode) GL.Begin(PrimitiveType.LineLoop);
                                    else GL.Begin(PrimitiveType.LineStrip);
                                    for (int h = 0; h < GuidanceLines[i][j].Count; h++) GL.Vertex3(GuidanceLines[i][j][h].Easting, GuidanceLines[i][j][h].Northing, 0);
                                    GL.End();
                                }
                            }
                        }
                        GL.Disable(EnableCap.LineStipple);
                    }

                    if (mf.font.isFontOn && ptCount > 410)
                    {
                        GL.Color3(0.40f, 0.90f, 0.95f);
                        mf.font.DrawText3D(Lines[CurrentLine].curvePts[201].Easting, Lines[CurrentLine].curvePts[201].Northing, "&A");
                        mf.font.DrawText3D(Lines[CurrentLine].curvePts[Lines[CurrentLine].curvePts.Count - 200].Easting, Lines[CurrentLine].curvePts[Lines[CurrentLine].curvePts.Count - 200].Northing, "&B");
                    }

                    ptCount = curList.Count;
                    if (ptCount > 1)
                    {
                        //OnDrawGizmos();
                        GL.LineWidth(mf.ABLines.lineWidth);
                        GL.Color3(0.95f, 0.2f, 0.95f);
                        if (Lines[CurrentLine].BoundaryMode) GL.Begin(PrimitiveType.LineLoop);
                        else GL.Begin(PrimitiveType.LineStrip);
                        for (int h = 0; h < ptCount; h++) GL.Vertex3(curList[h].Easting, curList[h].Northing, 0);
                        GL.End();
                    }

                    if (mf.isPureDisplayOn && !mf.isStanleyUsed)
                    {
                        if (mf.ppRadius < 100 && mf.ppRadius > -100)
                        {
                            const int numSegments = 100;
                            double theta = Glm.twoPI / numSegments;
                            double c = Math.Cos(theta);//precalculate the sine and cosine
                            double s = Math.Sin(theta);
                            double x = mf.ppRadius;//we start at angle = 0
                            double y = 0;

                            GL.LineWidth(1);
                            GL.Color3(0.95f, 0.30f, 0.950f);
                            GL.Begin(PrimitiveType.LineLoop);
                            for (int ii = 0; ii < numSegments; ii++)
                            {
                                //glVertex2f(x + cx, y + cy);//output vertex
                                Vec2 Point2 = mf.radiusPoint;
                                GL.Vertex3(x + Point2.Easting, y + Point2.Northing, 0);//output vertex
                                double t = x;//apply the rotation matrix
                                x = (c * x) - (s * y);
                                y = (s * t) + (c * y);
                            }
                            GL.End();
                        }

                        //Draw lookahead Point
                        GL.PointSize(4.0f);
                        GL.Begin(PrimitiveType.Points);
                        GL.Color3(1.0f, 0.5f, 0.95f);
                        Vec2 Point = mf.GoalPoint;
                        GL.Vertex3(Point.Easting, Point.Northing, 0.0);
                        GL.Color3(1.0f, 1.5f, 0.95f);
                        GL.Vertex3(mf.rEast, mf.rNorth, 0.0);
                        GL.End();
                    }

                    mf.yt.DrawYouTurn();

                    if (mf.yt.isYouTurnTriggered)
                    {
                        GL.Color3(0.95f, 0.95f, 0.25f);
                        GL.LineWidth(mf.ABLines.lineWidth);
                        ptCount = mf.yt.ytList.Count;
                        if (ptCount > 0)
                        {
                            GL.Begin(PrimitiveType.Points);
                            for (int i = 0; i < ptCount; i++)
                            {
                                GL.Vertex3(mf.yt.ytList[i].Easting, mf.yt.ytList[i].Northing, 0);
                            }
                            GL.End();
                        }
                        GL.Color3(0.95f, 0.05f, 0.05f);
                    }

                    GL.Color4(0.8630f, 0.73692f, 0.60f, 0.25);
                    mf.tram.DrawTram(false);
                }
            }
            GL.PointSize(1.0f);
        }

        public void BuildTram()
        {
            mf.tram.TramList.Clear();

            if (mf.bnd.bndArr.Count > 0) mf.tram.CreateBndTramRef();

            if (CurrentLine > -1 && CurrentLine < Lines.Count)
            {
                Vec3 Point;
                Vec3 Point2;

                List<List<Vec3>> BuildLeft = new List<List<Vec3>>();
                List<List<Vec3>> BuildRight = new List<List<Vec3>>();
                for (double i = 0.5; i < mf.tram.passes; i++)
                {
                    List<Vec3> Build = new List<Vec3>();
                    List<Vec3> Build2 = new List<Vec3>();
                    double Offset = (mf.tram.tramWidth * i) - mf.Guidance.WidthMinusOverlap / 2 - mf.tram.abOffset - mf.tram.halfWheelTrack;
                    double Offset2 = (mf.tram.tramWidth * i) - mf.Guidance.WidthMinusOverlap / 2 - mf.tram.abOffset + mf.tram.halfWheelTrack;
                    for (int j = 0; j < Lines[CurrentLine].curvePts.Count; j++)
                    {
                        double CosHeading = Math.Cos(Lines[CurrentLine].curvePts[j].Heading);
                        double SinHeading = Math.Sin(Lines[CurrentLine].curvePts[j].Heading);

                        Point = Lines[CurrentLine].curvePts[j];
                        Point.Northing += SinHeading * -Offset;
                        Point.Easting += CosHeading * Offset;

                        Point2 = Lines[CurrentLine].curvePts[j];
                        Point2.Northing += SinHeading * -Offset2;
                        Point2.Easting += CosHeading * Offset2;

                        if (Build.Count > 0)
                        {
                            double dist = ((Point.Easting - Build[Build.Count - 1].Easting) * (Point.Easting - Build[Build.Count - 1].Easting)) + ((Point.Northing - Build[Build.Count - 1].Northing) * (Point.Northing - Build[Build.Count - 1].Northing));
                            if (dist > 1) Build.Add(Point);
                        }
                        else Build.Add(Point);

                        if (Build2.Count > 0)
                        {
                            double dist = ((Point2.Easting - Build2[Build2.Count - 1].Easting) * (Point2.Easting - Build2[Build2.Count - 1].Easting)) + ((Point2.Northing - Build2[Build2.Count - 1].Northing) * (Point2.Northing - Build2[Build2.Count - 1].Northing));
                            if (dist > 1) Build2.Add(Point2);
                        }
                        else Build2.Add(Point2);
                    }
                    BuildLeft.AddRange(Build.ClipPolyLine(mf.bnd.bndArr[0].bndLine, Lines[CurrentLine].BoundaryMode, Offset, CancellationToken.None));
                    BuildRight.AddRange(Build2.ClipPolyLine(mf.bnd.bndArr[0].bndLine, Lines[CurrentLine].BoundaryMode, Offset, CancellationToken.None));
                }

                for (int k = 0; k < BuildLeft.Count; k++)
                {
                    BuildLeft[k].CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentLine].BoundaryMode, 0.0436332, CancellationToken.None, true, true, mf.tram.halfWheelTrack);

                    if (Lines[CurrentLine].BoundaryMode) BuildLeft[k].Add(BuildLeft[k][0]);

                    mf.tram.TramList.Add(new Trams());
                    int tramidx = mf.tram.TramList.Count - 1;

                    for (int l = 0; l < BuildLeft[k].Count; l += 2)
                    {
                        Point = BuildLeft[k][l];

                        double CosHeading = Math.Cos(BuildLeft[k][l].Heading);
                        double SinHeading = Math.Sin(BuildLeft[k][l].Heading);

                        Point.Northing += SinHeading * (mf.tram.WheelWidth / 2);
                        Point.Easting += CosHeading * (-mf.tram.WheelWidth / 2);
                        mf.tram.TramList[tramidx].Left.Add(new Vec2(Point.Northing, Point.Easting));
                        Point.Northing += SinHeading * -mf.tram.WheelWidth;
                        Point.Easting += CosHeading * mf.tram.WheelWidth;
                        mf.tram.TramList[tramidx].Left.Add(new Vec2(Point.Northing, Point.Easting));
                    }
                }
                for (int k = 0; k < BuildRight.Count; k++)
                {
                    BuildRight[k].CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentLine].BoundaryMode, 0.0436332, CancellationToken.None, true, false, mf.tram.halfWheelTrack);

                    if (Lines[CurrentLine].BoundaryMode) BuildRight[k].Add(BuildRight[k][0]);

                    mf.tram.TramList.Add(new Trams());
                    int tramidx = mf.tram.TramList.Count - 1;

                    for (int l = 0; l < BuildRight[k].Count; l += 2)
                    {
                        Point = BuildRight[k][l];

                        double CosHeading = Math.Cos(BuildRight[k][l].Heading);
                        double SinHeading = Math.Sin(BuildRight[k][l].Heading);

                        Point.Northing += SinHeading * (mf.tram.WheelWidth / 2);
                        Point.Easting += CosHeading * (-mf.tram.WheelWidth / 2);
                        mf.tram.TramList[tramidx].Right.Add(new Vec2(Point.Northing, Point.Easting));
                        Point.Northing += SinHeading * -mf.tram.WheelWidth;
                        Point.Easting += CosHeading * mf.tram.WheelWidth;
                        mf.tram.TramList[tramidx].Right.Add(new Vec2(Point.Northing, Point.Easting));
                    }
                }
            }
        }

        //for calculating for display the averaged new line
        public void SmoothAB(int smPts)
        {
            if (CurrentEditLine < Lines.Count && CurrentEditLine > -1)
            {
                //count the reference list of original curve
                int cnt = Lines[CurrentEditLine].curvePts.Count;

                //just go back if not very long
                if (cnt < 10) return;

                //the temp array
                Vec3[] arr = new Vec3[cnt];

                int counter = 0;
                //average them - center weighted average

                for (int i = 0; i < cnt; i++)
                {
                    if (!Lines[CurrentEditLine].BoundaryMode && (i < smPts / 2 || i > cnt - (smPts / 2)))
                    {
                        arr[i].Easting = Lines[CurrentEditLine].curvePts[i].Easting;
                        arr[i].Northing = Lines[CurrentEditLine].curvePts[i].Northing;
                        arr[i].Heading = Lines[CurrentEditLine].curvePts[i].Heading;
                        continue;
                    }

                    for (int j = -smPts / 2; j < smPts / 2; j++)
                    {
                        counter++;
                        int test = (j + i).Clamp(cnt);

                        arr[i].Easting += Lines[CurrentEditLine].curvePts[test].Easting;
                        arr[i].Northing += Lines[CurrentEditLine].curvePts[test].Northing;
                    }
                    arr[i].Easting /= smPts;
                    arr[i].Northing /= smPts;
                    arr[i].Heading = Lines[CurrentEditLine].curvePts[i].Heading;
                }

                //make a list to draw
                smooList.Clear();
                smooList.AddRange(arr);
            }
        }

        public void SwapAB()
        {
            int cnt = Lines[CurrentLine].curvePts.Count;
            if (cnt > 0)
            {
                Lines[CurrentLine].curvePts.Reverse();
                Lines[CurrentLine].Heading += Math.PI;
                Lines[CurrentLine].Heading %= Glm.twoPI;

                Lines[CurrentEditLine].curvePts.CalculateHeading(Lines[CurrentEditLine].BoundaryMode, CancellationToken.None);
            }
        }

        //turning the visual line into the real reference line to use
        public void SaveSmoothList()
        {
            if (CurrentEditLine < Lines.Count && CurrentEditLine > -1)
            {
                int cnt = smooList.Count;
                if (cnt < 3) return;

                smooList.CalculateHeading(Lines[CurrentEditLine].BoundaryMode, CancellationToken.None);
                Lines[CurrentEditLine].curvePts.Clear();
                Lines[CurrentEditLine].curvePts.AddRange(smooList);
                ResetABLine = true;
            }
        }

        public void CalculateExtraGuides(int Idx, double PathsAway)
        {
            double Offset2 = mf.Guidance.WidthMinusOverlap * PathsAway;

            for (int j = 0; j < Lines[CurrentLine].curvePts.Count; j++)
            {
                var point2 = new Vec3(
                    Lines[CurrentLine].curvePts[j].Northing + (Math.Sin(Lines[CurrentLine].curvePts[j].Heading) * -Offset2),
                    Lines[CurrentLine].curvePts[j].Easting + (Math.Cos(Lines[CurrentLine].curvePts[j].Heading) * Offset2),
                    Lines[CurrentLine].curvePts[j].Heading);

                bool Add = true;
                for (int t = 0; t < Lines[CurrentLine].curvePts.Count; t++)
                {
                    double dist = ((point2.Easting - Lines[CurrentLine].curvePts[t].Easting) * (point2.Easting - Lines[CurrentLine].curvePts[t].Easting)) + ((point2.Northing - Lines[CurrentLine].curvePts[t].Northing) * (point2.Northing - Lines[CurrentLine].curvePts[t].Northing));
                    if (dist < (Offset2 * Offset2) - 0.001)
                    {
                        Add = false;
                        break;
                    }
                }
                if (Add)
                {
                    if (GuidanceLines[Idx][0].Count > 0)
                    {
                        double dist = ((point2.Easting - GuidanceLines[Idx][0][GuidanceLines[Idx][0].Count - 1].Easting) * (point2.Easting - GuidanceLines[Idx][0][GuidanceLines[Idx][0].Count - 1].Easting)) + ((point2.Northing - GuidanceLines[Idx][0][GuidanceLines[Idx][0].Count - 1].Northing) * (point2.Northing - GuidanceLines[Idx][0][GuidanceLines[Idx][0].Count - 1].Northing));
                        if (dist > 1) GuidanceLines[Idx][0].Add(point2);
                    }
                    else GuidanceLines[Idx][0].Add(point2);
                }
            }

            if (Lines[CurrentLine].BoundaryMode)
            {
                bool first = true;
                int testa = -1;
                int l = 0;
                int m = 1;

                for (int k = 1; k + 1 < GuidanceLines[Idx][0].Count; l = k++)
                {
                    double Nort = GuidanceLines[Idx][0][k].Northing - GuidanceLines[Idx][0][l].Northing;
                    double East = GuidanceLines[Idx][0][k].Easting - GuidanceLines[Idx][0][l].Easting;
                    double dist = (East * East) + (Nort * Nort);
                    if (dist > Offset2 * Offset2 + Offset2 * Offset2)
                    {
                        if (first)
                        {
                            first = false;
                            testa = k;
                        }
                        else
                        {
                            GuidanceLines[Idx].Add(new List<Vec3>());

                            GuidanceLines[Idx][m].AddRange(GuidanceLines[Idx][0].GetRange(testa, k - testa));
                            GuidanceLines[Idx][0].RemoveRange(testa, k - testa);
                            first = true;
                            m++;
                        }
                    }
                }
            }
        }

        public void GetCurrentCurveLine(Vec3 pivot, Vec3 steer)
        {
            double minDistance;

            double boundaryTriggerDistance = 1;
            if (CurrentLine < Lines.Count && CurrentLine > -1)
            {
                bool useSteer = mf.isStanleyUsed;

                if (!mf.vehicle.isSteerAxleAhead) useSteer = !useSteer;
                bool isreversedriving = mf.pn.speed < -0.09;
                if (isreversedriving) useSteer = !useSteer;

                Vec3 point = useSteer ? steer : pivot;

                if (Lines[CurrentLine].SpiralMode == true)
                {
                    minDistance = Glm.Distance(pivot, Lines[CurrentLine].curvePts[0]);

                    double RefDist = minDistance / mf.Guidance.WidthMinusOverlap;
                    if (RefDist < 0) HowManyPathsAway = (int)(RefDist - 0.5);
                    else HowManyPathsAway = (int)(RefDist + 0.5);

                    if (OldHowManyPathsAway != HowManyPathsAway || ResetABLine)
                    {
                        ResetABLine = false;
                        OldHowManyPathsAway = HowManyPathsAway;
                        if (HowManyPathsAway < 2) HowManyPathsAway = 2;

                        double s = mf.Guidance.WidthMinusOverlap / 2;

                        curList.Clear();
                        //double circumference = (glm.twoPI * s) / (boundaryTriggerDistance * 0.1);
                        double circumference;

                        for (double round = Glm.twoPI * (HowManyPathsAway - 2); round <= (Glm.twoPI * (HowManyPathsAway + 2) + 0.00001); round += (Glm.twoPI / circumference))
                        {
                            double x = s * (Math.Cos(round) + (round / Math.PI) * Math.Sin(round));
                            double y = s * (Math.Sin(round) - (round / Math.PI) * Math.Cos(round));

                            Vec3 pt = new Vec3(Lines[CurrentLine].curvePts[0].Northing + y, Lines[CurrentLine].curvePts[0].Easting + x, 0);
                            curList.Add(pt);

                            double radius = Math.Sqrt(x * x + y * y);
                            circumference = (Glm.twoPI * radius) / (boundaryTriggerDistance);
                        }

                        if (curList.Count > 2) curList.CalculateHeading(true, CancellationToken.None);
                    }
                }
                else if (Lines[CurrentLine].CircleMode == true)
                {
                    minDistance = Glm.Distance(pivot, Lines[CurrentLine].curvePts[0]);

                    double RefDist = minDistance / mf.Guidance.WidthMinusOverlap;
                    if (RefDist < 0) HowManyPathsAway = (int)(RefDist - 0.5);
                    else HowManyPathsAway = (int)(RefDist + 0.5);

                    if (OldHowManyPathsAway != HowManyPathsAway && HowManyPathsAway == 0)
                    {
                        OldHowManyPathsAway = HowManyPathsAway;
                        curList.Clear();
                    }
                    else if (OldHowManyPathsAway != HowManyPathsAway)
                    {
                        if (HowManyPathsAway > 100) return;
                        OldHowManyPathsAway = HowManyPathsAway;

                        curList.Clear();

                        int aa = (int)((Glm.twoPI * mf.Guidance.WidthMinusOverlap * HowManyPathsAway) / boundaryTriggerDistance);

                        for (double round = 0; round <= Glm.twoPI + 0.00001; round += (Glm.twoPI) / aa)
                        {
                            Vec3 pt = new Vec3(Lines[CurrentLine].curvePts[0].Northing + (Math.Cos(round) * mf.Guidance.WidthMinusOverlap * HowManyPathsAway), Lines[CurrentLine].curvePts[0].Easting + (Math.Sin(round) * mf.Guidance.WidthMinusOverlap * HowManyPathsAway), 0);
                            curList.Add(pt);
                        }

                        if (curList.Count > 2) curList.CalculateHeading(true, CancellationToken.None);
                    }
                }
                else
                {
                    if (Lines[CurrentLine].curvePts.Count < 2) return;
                    double minDistA = double.PositiveInfinity, minDistB = double.PositiveInfinity;

                    if (!mf.isAutoSteerBtnOn || ResetABLine)
                    {
                        //find the closest 2 points to current fix
                        for (int t = 0; t < Lines[CurrentLine].curvePts.Count; t++)
                        {
                            double dist = ((pivot.Easting - Lines[CurrentLine].curvePts[t].Easting) * (pivot.Easting - Lines[CurrentLine].curvePts[t].Easting))
                                            + ((pivot.Northing - Lines[CurrentLine].curvePts[t].Northing) * (pivot.Northing - Lines[CurrentLine].curvePts[t].Northing));
                            if (dist < minDistA)
                            {
                                minDistB = minDistA;
                                B = A;
                                minDistA = dist;
                                A = t;
                            }
                            else if (dist < minDistB)
                            {
                                minDistB = dist;
                                B = t;
                            }
                        }

                        if (A > B) { int C = A; A = B; B = C; }

                        if (double.IsInfinity(minDistA) || double.IsInfinity(minDistB)) return;

                        double dx = Lines[CurrentLine].curvePts[B].Easting - Lines[CurrentLine].curvePts[A].Easting;
                        double dy = Lines[CurrentLine].curvePts[B].Northing - Lines[CurrentLine].curvePts[A].Northing;

                        if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

                        //are we going same direction as stripList was created?
                        isSameWay = Math.PI - Math.Abs(Math.Abs(point.Heading - Lines[CurrentLine].curvePts[A].Heading) - Math.PI) < Glm.PIBy2;

                        distanceFromRefLine = ((dy * point.Easting) - (dx * point.Northing) + (Lines[CurrentLine].curvePts[B].Easting * Lines[CurrentLine].curvePts[A].Northing) - (Lines[CurrentLine].curvePts[B].Northing * Lines[CurrentLine].curvePts[A].Easting)) / Math.Sqrt((dy * dy) + (dx * dx));

                        if (isSameWay) distanceFromRefLine += mf.Guidance.GuidanceOffset;
                        else distanceFromRefLine -= mf.Guidance.GuidanceOffset;

                        double RefDist = distanceFromRefLine / mf.Guidance.WidthMinusOverlap;
                        if (RefDist < 0) HowManyPathsAway = (int)(RefDist - 0.5);
                        else HowManyPathsAway = (int)(RefDist + 0.5);

                    }
                    else if (A < Lines[CurrentLine].curvePts.Count && B < Lines[CurrentLine].curvePts.Count)
                    {
                        double dx = Lines[CurrentLine].curvePts[B].Easting - Lines[CurrentLine].curvePts[A].Easting;
                        double dy = Lines[CurrentLine].curvePts[B].Northing - Lines[CurrentLine].curvePts[A].Northing;

                        if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

                        distanceFromRefLine = ((dy * point.Easting) - (dx * point.Northing) + (Lines[CurrentLine].curvePts[B].Easting * Lines[CurrentLine].curvePts[A].Northing) - (Lines[CurrentLine].curvePts[B].Northing * Lines[CurrentLine].curvePts[A].Easting)) / Math.Sqrt((dy * dy) + (dx * dx));
                    }

                    if (OldisSameWay != isSameWay || HowManyPathsAway != OldHowManyPathsAway || ResetABLine)
                    {
                        ResetABLine = false;
                        if (mf.isSideGuideLines)
                        {
                            if (OldHowManyPathsAway != HowManyPathsAway)
                            {
                                int Gcnt;

                                int Up = HowManyPathsAway - OldHowManyPathsAway;

                                if (Up < -5 || Up > 5) GuidanceLines.Clear();

                                int Count = GuidanceLines.Count;

                                if (Count < 6 || Up == 0)
                                {
                                    if (Count > 0) GuidanceLines.Clear();
                                    for (double i = -2.5; i < 3; i++)
                                    {
                                        GuidanceLines.Add(new List<List<Vec3>>());
                                        Gcnt = GuidanceLines.Count - 1;
                                        GuidanceLines[Gcnt].Add(new List<Vec3>());
                                        CalculateExtraGuides(Gcnt, HowManyPathsAway + i);
                                    }
                                }
                                else if (Up < 0)
                                {
                                    for (double i = -3.5; i >= Up - 2.5; i--)
                                    {
                                        GuidanceLines.RemoveAt(5);
                                        GuidanceLines.Insert(0, new List<List<Vec3>>());
                                        Gcnt = 0;
                                        GuidanceLines[Gcnt].Add(new List<Vec3>());
                                        CalculateExtraGuides(Gcnt, OldHowManyPathsAway + i);
                                    }
                                }
                                else
                                {
                                    for (double i = 3.5; i <= Up + 2.5; i++)
                                    {
                                        GuidanceLines.RemoveAt(0);
                                        GuidanceLines.Insert(5, new List<List<Vec3>>());
                                        Gcnt = 5;
                                        GuidanceLines[Gcnt].Add(new List<Vec3>());
                                        CalculateExtraGuides(Gcnt, OldHowManyPathsAway + i);
                                    }
                                }
                            }
                        }
                        else GuidanceLines.Clear();

                        OldisSameWay = isSameWay;
                        OldHowManyPathsAway = HowManyPathsAway;

                        curList.Clear();

                        double Offset = mf.Guidance.WidthMinusOverlap * HowManyPathsAway;

                        if (isSameWay) Offset -= mf.Guidance.GuidanceOffset;
                        else Offset += mf.Guidance.GuidanceOffset;

                        for (int i = 0; i < Lines[CurrentLine].curvePts.Count; i++)
                        {
                            var point2 = new Vec3(Lines[CurrentLine].curvePts[i].Northing + (Math.Sin(Lines[CurrentLine].curvePts[i].Heading) * -Offset), Lines[CurrentLine].curvePts[i].Easting + (Math.Cos(Lines[CurrentLine].curvePts[i].Heading) * Offset), Lines[CurrentLine].curvePts[i].Heading);
                            bool Add = true;

                            for (int t = 0; t < Lines[CurrentLine].curvePts.Count; t++)
                            {
                                double dist = ((point2.Easting - Lines[CurrentLine].curvePts[t].Easting) * (point2.Easting - Lines[CurrentLine].curvePts[t].Easting)) + ((point2.Northing - Lines[CurrentLine].curvePts[t].Northing) * (point2.Northing - Lines[CurrentLine].curvePts[t].Northing));
                                if (dist < (Offset * Offset) - 0.003)
                                {
                                    Add = false;
                                    break;
                                }
                            }
                            if (Add)
                            {
                                if (curList.Count > 0)
                                {
                                    double dist = ((point2.Easting - curList[curList.Count - 1].Easting) * (point2.Easting - curList[curList.Count - 1].Easting)) + ((point2.Northing - curList[curList.Count - 1].Northing) * (point2.Northing - curList[curList.Count - 1].Northing));
                                    if (dist > 1) curList.Add(point2);
                                }
                                else curList.Add(point2);
                            }
                        }
                        curList.CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentLine].BoundaryMode, 0.0436332, CancellationToken.None);
                    }
                }
                mf.CalculateSteerAngle(ref curList, isSameWay, Lines[CurrentLine].BoundaryMode);
            }
            else
            {
                //invalid distance so tell AS module
                mf.distanceFromCurrentLine = 32000;
                mf.guidanceLineDistanceOff = 32000;
            }
        }

        public void MoveLine(double dist)
        {
            int test = -1;
            if (CurrentLine < Lines.Count && CurrentLine > -1) test = CurrentLine;
            if (CurrentEditLine < Lines.Count && CurrentEditLine > -1) test = CurrentEditLine;

            if (test >= 0)
            {
                int cnt = Lines[test].curvePts.Count;

                Vec3 point;
                for (int i = 0; i < cnt; i++)
                {
                    point.Northing = Lines[test].curvePts[i].Northing + Math.Sin(Lines[test].curvePts[i].Heading) * -dist;
                    point.Easting = Lines[test].curvePts[i].Easting + Math.Cos(Lines[test].curvePts[i].Heading) * dist;
                    point.Heading = Lines[test].curvePts[i].Heading;
                    Lines[test].curvePts[i] = point;
                }
            }
            ResetABLine = true;
        }

        public bool PointOnLine(Vec3 pt1, Vec3 pt2, Vec3 pt)
        {
            var r = new Vec2(0, 0);
            if (pt1.Northing == pt2.Northing && pt1.Easting == pt2.Easting) { pt1.Northing -= 0.00001; }

            var U = ((pt.Northing - pt1.Northing) * (pt2.Northing - pt1.Northing)) + ((pt.Easting - pt1.Easting) * (pt2.Easting - pt1.Easting));

            var Udenom = Math.Pow(pt2.Northing - pt1.Northing, 2) + Math.Pow(pt2.Easting - pt1.Easting, 2);

            U /= Udenom;

            r.Northing = pt1.Northing + (U * (pt2.Northing - pt1.Northing));
            r.Easting = pt1.Easting + (U * (pt2.Easting - pt1.Easting));

            double minx, maxx, miny, maxy;

            minx = Math.Min(pt1.Northing, pt2.Northing);
            maxx = Math.Max(pt1.Northing, pt2.Northing);

            miny = Math.Min(pt1.Easting, pt2.Easting);
            maxy = Math.Max(pt1.Easting, pt2.Easting);
            return _ = r.Northing >= minx && r.Northing <= maxx && (r.Easting >= miny && r.Easting <= maxy);
        }

        public void AddFirstLastPoints()
        {
            Lines[CurrentEditLine].curvePts.RemoveAt(0);
            int ptCnt = Lines[CurrentEditLine].curvePts.Count - 1;
            Lines[CurrentEditLine].curvePts.RemoveAt(ptCnt);
            double x = 0, y = 0;
            for (int i = ptCnt - 4; i < ptCnt; i++)
            {
                x += Math.Cos(Lines[CurrentEditLine].curvePts[i].Heading);
                y += Math.Sin(Lines[CurrentEditLine].curvePts[i].Heading);
            }
            x /= 5;
            y /= 5;
            double EndHeading = Math.Atan2(y, x);
            Vec3 EndPoint = Lines[CurrentEditLine].curvePts[ptCnt-1];
            EndPoint.Heading = EndHeading;
            for (int i = 2; i < 25; i++)
            {
                EndPoint.Easting += Math.Sin(EndHeading) * i * 0.5;
                EndPoint.Northing += Math.Cos(EndHeading) * i * 0.5;
                Lines[CurrentEditLine].curvePts.Add(EndPoint);
            }

            x = 0;
            y = 0;
            for (int i = 0; i < 5; i++)
            {
                x += Math.Cos(Lines[CurrentEditLine].curvePts[i].Heading);
                y += Math.Sin(Lines[CurrentEditLine].curvePts[i].Heading);
            }
            x /= 5;
            y /= 5;
            double StartHeading = Math.Atan2(y, x);

            //and the beginning
            Vec3 StartPoint = Lines[CurrentEditLine].curvePts[1];
            StartPoint.Heading = StartHeading;
            for (int i = 2; i < 25; i++)
            {
                StartPoint.Easting -= Math.Sin(StartHeading) * i * 0.5;
                StartPoint.Northing -= Math.Cos(StartHeading) * i * 0.5;
                Lines[CurrentEditLine].curvePts.Insert(0, StartPoint);
            }
        }
    }

    public class CCurveLines
    {
        public List<Vec3> curvePts = new List<Vec3>();
        public double Heading = 3;
        public string Name = "aa";
        public bool SpiralMode = false;
        public bool CircleMode = false;
        public bool BoundaryMode = false;
    }
}