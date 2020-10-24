using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{

    public class CABCurve
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        //flag for starting stop adding points
        public bool BtnCurveLineOn, isOkToAddPoints;
        public double distanceFromRefLine;

        public double HowManyPathsAway, OldHowManyPathsAway;
        public bool isSmoothWindowOpen, isSameWay, OldisSameWay;

        public int A, B, CurrentLine = -1, CurrentEditLine = -1;
        public int tryoutcurve = -1;

        //the list of points of curve to drive on
        public List<Vec3> curList = new List<Vec3>();
        public List<Vec3> smooList = new List<Vec3>();
        public List<CCurveLines> Lines = new List<CCurveLines>();

        public bool isEditing;
        public List<List<Vec3>> GuidanceLines = new List<List<Vec3>>();

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
                    if (ptCount < 2) return;

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
                            if (GuidanceLines[i].Count > 0)
                            {
                                if (Lines[CurrentLine].BoundaryMode) GL.Begin(PrimitiveType.LineLoop);
                                else GL.Begin(PrimitiveType.LineStrip);
                                for (int h = 0; h < GuidanceLines[i].Count; h++) GL.Vertex3(GuidanceLines[i][h].Easting, GuidanceLines[i][h].Northing, 0);
                                GL.End();
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
                    BuildLeft.AddRange(Build.ClipPolyLine(mf.bnd.bndArr[0].bndLine, Lines[CurrentLine].BoundaryMode, Offset));
                    BuildRight.AddRange(Build2.ClipPolyLine(mf.bnd.bndArr[0].bndLine, Lines[CurrentLine].BoundaryMode, Offset));
                }

                for (int k = 0; k < BuildLeft.Count; k++)
                {
                    BuildLeft[k].CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentLine].BoundaryMode, 0.0436332, 5, true, false, true, mf.tram.halfWheelTrack);

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
                    BuildRight[k].CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentLine].BoundaryMode, 0.0436332, 5, true, false, false, mf.tram.halfWheelTrack);

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

                //read the points before and after the setpoint
                for (int s = 0; s < smPts / 2; s++)
                {
                    arr[s].Easting = Lines[CurrentEditLine].curvePts[s].Easting;
                    arr[s].Northing = Lines[CurrentEditLine].curvePts[s].Northing;
                    arr[s].Heading = Lines[CurrentEditLine].curvePts[s].Heading;
                }

                for (int s = cnt - (smPts / 2); s < cnt; s++)
                {
                    arr[s].Easting = Lines[CurrentEditLine].curvePts[s].Easting;
                    arr[s].Northing = Lines[CurrentEditLine].curvePts[s].Northing;
                    arr[s].Heading = Lines[CurrentEditLine].curvePts[s].Heading;
                }

                //average them - center weighted average
                for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
                {
                    for (int j = -smPts / 2; j < smPts / 2; j++)
                    {
                        arr[i].Easting += Lines[CurrentEditLine].curvePts[j + i].Easting;
                        arr[i].Northing += Lines[CurrentEditLine].curvePts[j + i].Northing;
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

                Lines[CurrentEditLine].curvePts.CalculateHeading(Lines[CurrentEditLine].BoundaryMode);
            }
        }

        //turning the visual line into the real reference line to use
        public void SaveSmoothList()
        {
            if (CurrentEditLine < Lines.Count && CurrentEditLine > -1)
            {
                int cnt = smooList.Count;
                if (cnt < 3) return;

                smooList.CalculateHeading(Lines[CurrentEditLine].BoundaryMode);
                Lines[CurrentEditLine].curvePts = smooList;
                OldHowManyPathsAway = double.NegativeInfinity;
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
                    double dist = ((pivot.Easting - Lines[CurrentLine].curvePts[0].Easting) * (pivot.Easting - Lines[CurrentLine].curvePts[0].Easting)) + ((pivot.Northing - Lines[CurrentLine].curvePts[0].Northing) * (pivot.Northing - Lines[CurrentLine].curvePts[0].Northing));

                    minDistance = Math.Sqrt(dist);

                    HowManyPathsAway = Math.Round(minDistance / mf.Guidance.WidthMinusOverlap, 0, MidpointRounding.AwayFromZero);
                    if (OldHowManyPathsAway != HowManyPathsAway)
                    {
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

                        if (curList.Count > 2) curList.CalculateHeading(true);
                    }
                }
                else if (Lines[CurrentLine].CircleMode == true)
                {
                    double dist = ((pivot.Easting - Lines[CurrentLine].curvePts[0].Easting) * (pivot.Easting - Lines[CurrentLine].curvePts[0].Easting)) + ((pivot.Northing - Lines[CurrentLine].curvePts[0].Northing) * (pivot.Northing - Lines[CurrentLine].curvePts[0].Northing));

                    minDistance = Math.Sqrt(dist);

                    HowManyPathsAway = Math.Round(minDistance / mf.Guidance.WidthMinusOverlap, 0, MidpointRounding.AwayFromZero);

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

                        int aa = (int)((Glm.twoPI * mf.Guidance.WidthMinusOverlap * HowManyPathsAway) / (boundaryTriggerDistance));

                        for (double round = 0; round <= Glm.twoPI + 0.00001; round += (Glm.twoPI) / aa)
                        {
                            Vec3 pt = new Vec3(Lines[CurrentLine].curvePts[0].Northing + (Math.Cos(round) * mf.Guidance.WidthMinusOverlap * HowManyPathsAway), Lines[CurrentLine].curvePts[0].Easting + (Math.Sin(round) * mf.Guidance.WidthMinusOverlap * HowManyPathsAway), 0);
                            curList.Add(pt);
                        }

                        if (curList.Count > 2) curList.CalculateHeading(true);
                    }
                }
                else
                {
                    if (Lines[CurrentLine].curvePts.Count < 2) return;
                    double minDistA = double.PositiveInfinity, minDistB = double.PositiveInfinity;

                    if (!mf.isAutoSteerBtnOn || double.IsNegativeInfinity(OldHowManyPathsAway))
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

                        HowManyPathsAway = Math.Round(distanceFromRefLine / mf.Guidance.WidthMinusOverlap, 0, MidpointRounding.AwayFromZero);
                    }
                    else if (A < Lines[CurrentLine].curvePts.Count && B < Lines[CurrentLine].curvePts.Count)
                    {
                        double dx = Lines[CurrentLine].curvePts[B].Easting - Lines[CurrentLine].curvePts[A].Easting;
                        double dy = Lines[CurrentLine].curvePts[B].Northing - Lines[CurrentLine].curvePts[A].Northing;

                        if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

                        distanceFromRefLine = ((dy * point.Easting) - (dx * point.Northing) + (Lines[CurrentLine].curvePts[B].Easting * Lines[CurrentLine].curvePts[A].Northing) - (Lines[CurrentLine].curvePts[B].Northing * Lines[CurrentLine].curvePts[A].Easting)) / Math.Sqrt((dy * dy) + (dx * dx));
                    }

                    if (OldisSameWay != isSameWay || HowManyPathsAway != OldHowManyPathsAway)
                    {
                        OldisSameWay = isSameWay;
                        OldHowManyPathsAway = HowManyPathsAway;

                        if (mf.isSideGuideLines)
                        {
                            GuidanceLines.Clear();
                            for (double i = -2.5; i < 3.5; i++)
                            {
                                GuidanceLines.Add(new List<Vec3>());
                                int Gcnt = GuidanceLines.Count - 1;
                                double Offset2 = mf.Guidance.WidthMinusOverlap * (i + HowManyPathsAway);

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
                                        if (GuidanceLines[Gcnt].Count > 0)
                                        {
                                            double dist = ((point2.Easting - GuidanceLines[Gcnt][GuidanceLines[Gcnt].Count - 1].Easting) * (point2.Easting - GuidanceLines[Gcnt][GuidanceLines[Gcnt].Count - 1].Easting)) + ((point2.Northing - GuidanceLines[Gcnt][GuidanceLines[Gcnt].Count - 1].Northing) * (point2.Northing - GuidanceLines[Gcnt][GuidanceLines[Gcnt].Count - 1].Northing));
                                            if (dist > 1) GuidanceLines[Gcnt].Add(point2);
                                        }
                                        else GuidanceLines[Gcnt].Add(point2);
                                    }
                                }

                                if (Lines[CurrentLine].BoundaryMode)
                                {

                                    bool first = true;
                                    int testa = -1;
                                    int m = 1;
                                    int l = 0;
                                    for (int k = 1; k + 1 < GuidanceLines[Gcnt].Count; l = k++)
                                    {
                                        double dist = ((GuidanceLines[Gcnt][k].Easting - GuidanceLines[Gcnt][l].Easting) * (GuidanceLines[Gcnt][k].Easting - GuidanceLines[Gcnt][l].Easting)) + ((GuidanceLines[Gcnt][k].Northing - GuidanceLines[Gcnt][l].Northing) * (GuidanceLines[Gcnt][k].Northing - GuidanceLines[Gcnt][l].Northing));
                                        if (dist > Offset2 * Offset2 + Offset2 * Offset2)
                                        {
                                            if (first)
                                            {
                                                first = false;
                                                testa = k;
                                            }
                                            else
                                            {
                                                GuidanceLines.Add(new List<Vec3>());
                                                GuidanceLines[Gcnt + m].AddRange(GuidanceLines[Gcnt].GetRange(testa, k - testa));
                                                GuidanceLines[Gcnt].RemoveRange(testa, k - testa);
                                                first = true;
                                                m++;
                                            }
                                        }
                                    }
                                }
                            }
                        }

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
                                if (dist < (Offset * Offset) - 0.001)
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

                        curList.CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentLine].BoundaryMode, 0.0436332, Offset);

                        int cnt = curList.Count;
                        if (cnt < -10)
                        {
                            for (int i = 1; i < (curList.Count - 1); i++)
                            {
                                curList[i] = new Vec3(curList[i].Northing, curList[i].Easting, Math.Atan2(curList[i + 1].Easting - curList[i - 1].Easting, curList[i + 1].Northing - curList[i - 1].Northing));
                            }

                            Vec3[] arr = new Vec3[cnt];
                            curList.CopyTo(arr);
                            curList.Clear();
                            bool Next = false;
                            for (int i = 0; i < arr.Length - 1; i++)
                            {
                                if (arr[i].Heading - arr[i + 1].Heading > 0.174533)
                                {
                                    Next = true;
                                    curList.Add(new Vec3(arr[i].Northing * 0.75 + arr[i + 1].Northing * 0.25, arr[i].Easting * 0.75 + arr[i + 1].Easting * 0.25, 0));
                                    curList.Add(new Vec3(arr[i].Northing * 0.25 + arr[i + 1].Northing * 0.75, arr[i].Easting * 0.25 + arr[i + 1].Easting * 0.75, 0));
                                }
                                else if (Next)
                                {
                                    Next = false;
                                    curList.Add(new Vec3(arr[i].Northing * 0.75 + arr[i + 1].Northing * 0.25, arr[i].Easting * 0.75 + arr[i + 1].Easting * 0.25, 0));
                                    curList.Add(new Vec3(arr[i].Northing * 0.25 + arr[i + 1].Northing * 0.75, arr[i].Easting * 0.25 + arr[i + 1].Easting * 0.75, 0));
                                }
                                else
                                {
                                    curList.Add(new Vec3(arr[i].Northing * 0.5 + arr[i + 1].Northing * 0.5, arr[i].Easting * 0.5 + arr[i + 1].Easting * 0.5, 0));
                                }
                            }


                            for (int i = 1; i < curList.Count - 1; i++)
                            {
                                curList[i] = new Vec3(curList[i].Northing, curList[i].Easting, Math.Atan2(curList[i + 1].Easting - curList[i - 1].Easting, curList[i + 1].Northing - curList[i - 1].Northing));
                            }



                            if (mf.Tools[0].isToolTrailing)
                            {
                                double head = 0;
                                if (isSameWay) head = Math.PI;

                                if (mf.Tools[0].isToolTBT && mf.Tools[0].toolTankTrailingHitchLength < 0)
                                {
                                    arr = new Vec3[curList.Count];
                                    curList.CopyTo(arr);
                                    curList.Clear();

                                    for (int i = 0; i < arr.Length; i++)
                                    {
                                        arr[i].Easting += Math.Sin(arr[i].Heading + head) * mf.Tools[0].toolTankTrailingHitchLength;
                                        arr[i].Northing += Math.Cos(arr[i].Heading + head) * mf.Tools[0].toolTankTrailingHitchLength;
                                    }

                                    for (int i = 1; i < (arr.Length - 1); i++)
                                    {
                                        arr[i].Heading = Math.Atan2(arr[i + 1].Easting - arr[i - 1].Easting, arr[i + 1].Northing - arr[i - 1].Northing);
                                    }

                                    Next = false;
                                    for (int i = 0; i < arr.Length - 1; i++)
                                    {
                                        if (arr[i].Heading - arr[i + 1].Heading > 0.174533)
                                        {
                                            Next = true;
                                            curList.Add(new Vec3(arr[i].Northing * 0.75 + arr[i + 1].Northing * 0.25, arr[i].Easting * 0.75 + arr[i + 1].Easting * 0.25, 0));
                                            curList.Add(new Vec3(arr[i].Northing * 0.25 + arr[i + 1].Northing * 0.75, arr[i].Easting * 0.25 + arr[i + 1].Easting * 0.75, 0));
                                        }
                                        else if (Next)
                                        {
                                            Next = false;
                                            curList.Add(new Vec3(arr[i].Northing * 0.75 + arr[i + 1].Northing * 0.25, arr[i].Easting * 0.75 + arr[i + 1].Easting * 0.25, 0));
                                            curList.Add(new Vec3(arr[i].Northing * 0.25 + arr[i + 1].Northing * 0.75, arr[i].Easting * 0.25 + arr[i + 1].Easting * 0.75, 0));
                                        }
                                        else
                                        {
                                            curList.Add(new Vec3(arr[i].Northing * 0.5 + arr[i + 1].Northing * 0.5, arr[i].Easting * 0.5 + arr[i + 1].Easting * 0.5, 0));
                                        }
                                    }
                                    for (int i = 1; i < (curList.Count - 1); i++)
                                    {
                                        curList[i] = new Vec3(curList[i].Northing, curList[i].Easting, Math.Atan2(curList[i + 1].Easting - curList[i - 1].Easting, curList[i + 1].Northing - curList[i - 1].Northing));
                                    }
                                }

                                arr = new Vec3[curList.Count];
                                curList.CopyTo(arr);
                                curList.Clear();


                                for (int i = 0; i < arr.Length; i++)
                                {
                                    arr[i].Easting += Math.Sin(arr[i].Heading + head) * mf.Tools[0].toolTrailingHitchLength;
                                    arr[i].Northing += Math.Cos(arr[i].Heading + head) * mf.Tools[0].toolTrailingHitchLength;
                                }
                                for (int i = 1; i < (arr.Length - 1); i++)
                                {
                                    arr[i].Heading = Math.Atan2(arr[i + 1].Easting - arr[i - 1].Easting, arr[i + 1].Northing - arr[i - 1].Northing);
                                }

                                Next = false;
                                for (int i = 0; i < arr.Length - 1; i++)
                                {
                                    if (arr[i].Heading - arr[i + 1].Heading > 0.174533)
                                    {
                                        Next = true;
                                        curList.Add(new Vec3(arr[i].Northing * 0.75 + arr[i + 1].Northing * 0.25, arr[i].Easting * 0.75 + arr[i + 1].Easting * 0.25, 0));
                                        curList.Add(new Vec3(arr[i].Northing * 0.25 + arr[i + 1].Northing * 0.75, arr[i].Easting * 0.25 + arr[i + 1].Easting * 0.75, 0));
                                    }
                                    else if (Next)
                                    {
                                        Next = false;
                                        curList.Add(new Vec3(arr[i].Northing * 0.75 + arr[i + 1].Northing * 0.25, arr[i].Easting * 0.75 + arr[i + 1].Easting * 0.25, 0));
                                        curList.Add(new Vec3(arr[i].Northing * 0.25 + arr[i + 1].Northing * 0.75, arr[i].Easting * 0.25 + arr[i + 1].Easting * 0.75, 0));
                                    }
                                    else
                                    {
                                        curList.Add(new Vec3(arr[i].Northing * 0.5 + arr[i + 1].Northing * 0.5, arr[i].Easting * 0.5 + arr[i + 1].Easting * 0.5, 0));
                                    }
                                }
                                for (int i = 1; i < (curList.Count - 1); i++)
                                {
                                    curList[i] = new Vec3(curList[i].Northing, curList[i].Easting, Math.Atan2(curList[i + 1].Easting - curList[i - 1].Easting, curList[i + 1].Northing - curList[i - 1].Northing));
                                }
                            }
                        }
                    }
                }
                mf.CalculateSteerAngle(ref curList);
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
            OldHowManyPathsAway = double.NegativeInfinity;
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