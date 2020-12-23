using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AgOpenGPS
{
    public class CGuidance
    {
        public double GuidanceWidth, GuidanceOverlap, GuidanceOffset, WidthMinusOverlap;


        //pointers to mainform controls
        private readonly FormGPS mf;

        //flag for starting stop adding points
        public bool BtnGuidanceOn, isOkToAddPoints;
        public double distanceFromRefLine;

        public bool ResetABLine = false;
        public int HowManyPathsAway, OldHowManyPathsAway, SmoothCount;
        public bool isSmoothWindowOpen, isSameWay, OldisSameWay;

        public int A, B, CurrentLine = -1, CurrentEditLine = -1, tryoutcurve = -1, CurrentTramLine = -1;

        //the list of points of curve to drive on
        public List<Vec3> curList = new List<Vec3>();
        public List<Vec3> smooList = new List<Vec3>();
        public List<CGuidanceLine> Lines = new List<CGuidanceLine>();
        public bool isEditing;
        public List<List<List<Vec3>>> GuidanceLines = new List<List<List<Vec3>>>();

        public CGuidance(FormGPS _f)
        {
            mf = _f;
            GuidanceWidth = Properties.Vehicle.Default.GuidanceWidth;
            GuidanceOverlap = Properties.Vehicle.Default.GuidanceOverlap;
            WidthMinusOverlap = GuidanceWidth - GuidanceOverlap;
            GuidanceOffset = Properties.Vehicle.Default.GuidanceOffset;
        }

        public void DrawLine()
        {
            int ptCount;
            if (tryoutcurve > -1 && tryoutcurve < Lines.Count)
            {
                GL.LineWidth(mf.lineWidth * 2);
                GL.Color3(1.0f, 0.0f, 0.0f);
                if (Lines[tryoutcurve].Mode == Gmode.Boundary) GL.Begin(PrimitiveType.LineLoop);
                else GL.Begin(PrimitiveType.LineStrip);

                if (Lines[tryoutcurve].Mode == Gmode.AB || Lines[tryoutcurve].Mode == Gmode.Heading)
                {
                    double cosHeading = Math.Cos(-Lines[tryoutcurve].Heading);
                    double sinHeading = Math.Sin(-Lines[tryoutcurve].Heading); 

                    GL.Vertex3(Lines[tryoutcurve].Segments[0].Easting + sinHeading * mf.maxCrossFieldLength, Lines[tryoutcurve].Segments[0].Northing - cosHeading * mf.maxCrossFieldLength, 0);
                    GL.Vertex3(Lines[tryoutcurve].Segments[1].Easting - sinHeading * mf.maxCrossFieldLength, Lines[tryoutcurve].Segments[1].Northing + cosHeading * mf.maxCrossFieldLength, 0);
                }
                else
                {
                    for (int h = 0; h < Lines[tryoutcurve].Segments.Count; h++)
                    {
                        GL.Vertex3(Lines[tryoutcurve].Segments[h].Easting, Lines[tryoutcurve].Segments[h].Northing, 0);
                    }
                }
                GL.End();
                return;
            }
            else if (isSmoothWindowOpen)
            {
                ptCount = smooList.Count;
                if (smooList.Count == 0) return;

                GL.LineWidth(mf.lineWidth);
                GL.Color3(0.930f, 0.92f, 0.260f);
                GL.Begin(PrimitiveType.Lines);
                for (int h = 0; h < ptCount; h++) GL.Vertex3(smooList[h].Easting, smooList[h].Northing, 0);
                GL.End();
            }
            else if (CurrentEditLine < Lines.Count && CurrentEditLine > -1)//draw the last line to tractor
            {
                ptCount = Lines[CurrentEditLine].Segments.Count;
                if (ptCount > 0)
                {
                    GL.Color3(0.930f, 0.0692f, 0.260f);
                    GL.Begin(PrimitiveType.LineStrip);

                    if (Lines[CurrentEditLine].Mode == Gmode.AB || Lines[CurrentEditLine].Mode == Gmode.Heading)
                    {
                        double cosHeading = Math.Cos(-Lines[CurrentEditLine].Heading);
                        double sinHeading = Math.Sin(-Lines[CurrentEditLine].Heading);

                        GL.Vertex3(Lines[CurrentEditLine].Segments[0].Easting + sinHeading * mf.maxCrossFieldLength, Lines[CurrentEditLine].Segments[0].Northing - cosHeading * mf.maxCrossFieldLength, 0);
                        GL.Vertex3(Lines[CurrentEditLine].Segments[1].Easting - sinHeading * mf.maxCrossFieldLength, Lines[CurrentEditLine].Segments[1].Northing + cosHeading * mf.maxCrossFieldLength, 0);
                    }
                    else
                    {
                        for (int h = 0; h < ptCount; h++) GL.Vertex3(Lines[CurrentEditLine].Segments[h].Easting, Lines[CurrentEditLine].Segments[h].Northing, 0);

                        if (isEditing && ptCount > 0)
                        {
                            Vec3 pivot = mf.pivotAxlePos;
                            GL.Vertex3(Lines[CurrentEditLine].Segments[ptCount - 1].Easting, Lines[CurrentEditLine].Segments[ptCount - 1].Northing, 0);
                            GL.Vertex3(pivot.Easting, pivot.Northing, 0);
                        }
                    }
                    GL.End();
                }
            }
            else
            {
                if (CurrentLine < Lines.Count && CurrentLine > -1)
                {
                    ptCount = Lines[CurrentLine].Segments.Count;
                    if (ptCount < 1) return;

                    GL.Color3(0.96, 0.2f, 0.2f);

                    //original line

                    if (Lines[CurrentLine].Mode == Gmode.Boundary) GL.Begin(PrimitiveType.LineLoop);
                    else GL.Begin(PrimitiveType.LineStrip);

                    if (Lines[CurrentLine].Mode == Gmode.AB || Lines[CurrentLine].Mode == Gmode.Heading)
                    {
                        double cosHeading = Math.Cos(-Lines[CurrentLine].Heading);
                        double sinHeading = Math.Sin(-Lines[CurrentLine].Heading);

                        GL.Vertex3(Lines[CurrentLine].Segments[0].Easting + sinHeading * mf.maxCrossFieldLength, Lines[CurrentLine].Segments[0].Northing - cosHeading * mf.maxCrossFieldLength, 0);
                        GL.Vertex3(Lines[CurrentLine].Segments[1].Easting - sinHeading * mf.maxCrossFieldLength, Lines[CurrentLine].Segments[1].Northing + cosHeading * mf.maxCrossFieldLength, 0);
                    }
                    else
                    {
                        for (int h = 0; h < ptCount; h++)
                        {
                            GL.Vertex3(Lines[CurrentLine].Segments[h].Easting, Lines[CurrentLine].Segments[h].Northing, 0);
                        }
                    }
                    GL.End();

                    if (mf.isSideGuideLines)
                    {
                        GL.Color3(0.56f, 0.650f, 0.650f);
                        GL.Enable(EnableCap.LineStipple);
                        GL.LineStipple(1, 0x0101);

                        GL.LineWidth(mf.lineWidth);

                        for (int i = 0; i < GuidanceLines.Count; i++)
                        {
                            for (int j = 0; j < GuidanceLines[i].Count; j++)
                            {
                                if (GuidanceLines[i][j].Count > 0)
                                {
                                    if (Lines[CurrentLine].Mode == Gmode.Boundary) GL.Begin(PrimitiveType.LineLoop);
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
                        mf.font.DrawText3D(Lines[CurrentLine].Segments[201].Easting, Lines[CurrentLine].Segments[201].Northing, "&A");
                        mf.font.DrawText3D(Lines[CurrentLine].Segments[Lines[CurrentLine].Segments.Count - 200].Easting, Lines[CurrentLine].Segments[Lines[CurrentLine].Segments.Count - 200].Northing, "&B");
                    }

                    ptCount = curList.Count;
                    if (ptCount > 1)
                    {
                        //OnDrawGizmos();
                        GL.LineWidth(mf.lineWidth);
                        GL.Color3(0.95f, 0.2f, 0.95f);
                        if (Lines[CurrentLine].Mode == Gmode.Boundary) GL.Begin(PrimitiveType.LineLoop);
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
                }
            }
            GL.PointSize(1.0f);
        }

        public void BuildTram()
        {
            mf.tram.TramList.Clear();

            if (mf.bnd.bndArr.Count > 0) mf.tram.CreateBndTramRef();

            if (CurrentTramLine > -1 && CurrentTramLine < Lines.Count)
            {
                if (Lines[CurrentTramLine].Mode == Gmode.AB || Lines[CurrentTramLine].Mode == Gmode.Heading)
                    BuildTram2();
                else
                {

                    Vec3 Point;
                    Vec3 Point2;

                    List<List<Vec3>> BuildLeft = new List<List<Vec3>>();
                    List<List<Vec3>> BuildRight = new List<List<Vec3>>();
                    for (double i = 0.5; i < mf.tram.passes; i++)
                    {
                        List<Vec3> Build = new List<Vec3>();
                        List<Vec3> Build2 = new List<Vec3>();
                        double Offset = (mf.tram.tramWidth * i) - WidthMinusOverlap / 2 + mf.tram.abOffset - mf.tram.halfWheelTrack;
                        double Offset2 = (mf.tram.tramWidth * i) - WidthMinusOverlap / 2 + mf.tram.abOffset + mf.tram.halfWheelTrack;
                        for (int j = 0; j < Lines[CurrentTramLine].Segments.Count; j++)
                        {
                            double CosHeading = Math.Cos(Lines[CurrentTramLine].Segments[j].Heading);
                            double SinHeading = Math.Sin(Lines[CurrentTramLine].Segments[j].Heading);

                            Point = Lines[CurrentTramLine].Segments[j];
                            Point.Northing += SinHeading * -Offset;
                            Point.Easting += CosHeading * Offset;

                            Point2 = Lines[CurrentTramLine].Segments[j];
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
                        BuildLeft.AddRange(Build.ClipPolyLine(mf.bnd.bndArr[0].bndLine, Lines[CurrentTramLine].Mode == Gmode.Boundary, Offset, CancellationToken.None));
                        BuildRight.AddRange(Build2.ClipPolyLine(mf.bnd.bndArr[0].bndLine, Lines[CurrentTramLine].Mode == Gmode.Boundary, Offset, CancellationToken.None));
                    }

                    for (int k = 0; k < BuildLeft.Count; k++)
                    {
                        BuildLeft[k].CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentTramLine].Mode == Gmode.Boundary, 0.0436332, CancellationToken.None, true, true, mf.tram.halfWheelTrack);

                        if (Lines[CurrentTramLine].Mode == Gmode.Boundary) BuildLeft[k].Add(BuildLeft[k][0]);

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
                        BuildRight[k].CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentTramLine].Mode == Gmode.Boundary, 0.0436332, CancellationToken.None, true, false, mf.tram.halfWheelTrack);

                        if (Lines[CurrentTramLine].Mode == Gmode.Boundary) BuildRight[k].Add(BuildRight[k][0]);

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
        }


        public void BuildTram2()
        {
            double hsin = Math.Sin(Lines[CurrentTramLine].Heading);
            double hcos = Math.Cos(Lines[CurrentTramLine].Heading);

            int tramcount = mf.tram.TramList.Count;

            for (double i = 0.5; i < mf.tram.passes; i++)
            {
                double Offset = (mf.tram.tramWidth * i) - WidthMinusOverlap / 2 - mf.tram.halfWheelTrack + mf.tram.abOffset;

                Vec2 pos1A = new Vec2(Lines[CurrentTramLine].Segments[0]);
                pos1A.Northing += hcos * mf.maxCrossFieldLength + hsin * -Offset;
                pos1A.Easting += hsin * mf.maxCrossFieldLength + hcos * Offset;

                Vec2 pos1B = new Vec2(Lines[CurrentTramLine].Segments[1]);
                pos1B.Northing += hcos * -mf.maxCrossFieldLength + hsin * -Offset;
                pos1B.Easting += hsin * -mf.maxCrossFieldLength + hcos * Offset;

                Vec2 pos2A = pos1A;
                pos2A.Northing += hsin * -mf.tram.wheelTrack;
                pos2A.Easting += hcos * mf.tram.wheelTrack;

                Vec2 pos2B = pos1B;
                pos2B.Northing += hsin * -mf.tram.wheelTrack;
                pos2B.Easting += hcos * mf.tram.wheelTrack;


                if (mf.bnd.bndArr.Count > 0 && mf.tram.TramList[0].Left.Count > 2)
                {
                    List<Vec4> Crossings1 = new List<Vec4>();
                    Vec2 crossing = new Vec2();

                    for (int m = 0; m < tramcount; m++)
                    {
                        Crossings1.FindCrossingPoints(mf.tram.TramList[m].Left, pos1A, pos1B, 0);
                    }

                    if (Crossings1.Count > 1)
                    {
                        List<Vec4> Crossings2 = new List<Vec4>();
                        for (int m = 0; m < tramcount; m++)
                        {
                            Crossings2.FindCrossingPoints(mf.tram.TramList[m].Left, pos2A, pos2B, 0);
                        }

                        if (Crossings2.Count > 1)
                        {
                            Crossings1.Sort((x, y) => x.Time.CompareTo(y.Time));
                            Crossings2.Sort((x, y) => x.Time.CompareTo(y.Time));

                            if (Crossings1.Count - 1 > Crossings2.Count)
                            {
                                for (int j = 0; j + 1 < Crossings2.Count; j += 2)
                                {
                                    for (int l = j + 1; l + 1 < Crossings1.Count; l += 2)
                                    {
                                        if (Crossings2[j].Time < Crossings1[l].Time && Crossings1[l + 1].Time < Crossings2[j + 1].Time)
                                        {
                                            crossing = new Vec2(Crossings1[l + 1].Northing, Crossings1[l + 1].Easting);
                                            crossing.Northing += hsin * mf.tram.wheelTrack;
                                            crossing.Easting += hcos * mf.tram.wheelTrack;
                                            Crossings2.Insert(j + 1, new Vec4(crossing.Northing, crossing.Easting, 0, Crossings1[l].Time, 0));
                                            crossing = new Vec2(Crossings1[l].Northing, Crossings1[l].Easting);
                                            crossing.Northing += hsin * mf.tram.wheelTrack;
                                            crossing.Easting += hcos * mf.tram.wheelTrack;
                                            Crossings2.Insert(j + 1, new Vec4(crossing.Northing, crossing.Easting, 0, Crossings1[l].Time, 0));
                                        }
                                        else if (Crossings2[j + 1].Time < Crossings1[j].Time && Crossings2[j + 1].Time < Crossings1[j + 1].Time)
                                        {
                                            Crossings1.RemoveAt(j);
                                            Crossings1.RemoveAt(j);
                                        }
                                        else if (Crossings2[j + 1].Time > Crossings1[j].Time && Crossings2[j + 1].Time > Crossings1[j + 1].Time)
                                        {
                                            Crossings1.RemoveAt(j);
                                            Crossings1.RemoveAt(j);
                                        }

                                    }
                                }
                                Crossings2.Sort((x, y) => x.Time.CompareTo(y.Time));
                            }
                            else if (Crossings2.Count - 1 > Crossings1.Count)
                            {
                                for (int j = 0; j + 1 < Crossings1.Count; j += 2)
                                {
                                    for (int l = j + 1; l + 1 < Crossings2.Count; l += 2)
                                    {
                                        if (Crossings1[j].Time < Crossings2[l].Time && Crossings2[l + 1].Time < Crossings1[j + 1].Time)
                                        {
                                            crossing = new Vec2(Crossings2[l + 1].Northing, Crossings2[l + 1].Easting);
                                            crossing.Northing += hsin * mf.tram.wheelTrack;
                                            crossing.Easting += hcos * -mf.tram.wheelTrack;
                                            Crossings1.Insert(j + 1, new Vec4(crossing.Northing, crossing.Easting, 0, Crossings2[l].Time, 0));
                                            crossing = new Vec2(Crossings2[l].Northing, Crossings2[l].Easting);
                                            crossing.Northing += hsin * mf.tram.wheelTrack;
                                            crossing.Easting += hcos * -mf.tram.wheelTrack;
                                            Crossings1.Insert(j + 1, new Vec4(crossing.Northing, crossing.Easting, 0, Crossings2[l].Time, 0));

                                        }
                                        else if (Crossings1[j + 1].Time < Crossings2[j].Time && Crossings1[j + 1].Time < Crossings2[j + 1].Time)
                                        {
                                            Crossings2.RemoveAt(j);
                                            Crossings2.RemoveAt(j);
                                        }
                                        else if (Crossings1[j + 1].Time > Crossings2[j].Time && Crossings1[j + 1].Time > Crossings2[j + 1].Time)
                                        {
                                            Crossings2.RemoveAt(j);
                                            Crossings2.RemoveAt(j);
                                        }
                                    }
                                }
                                Crossings1.Sort((x, y) => x.Time.CompareTo(y.Time));
                            }
                            for (int j = 0; j + 1 < Crossings1.Count; j += 2)
                            {
                                if (j + 1 < Crossings2.Count)
                                {
                                    mf.tram.TramList.Add(new Trams());

                                    //left of left tram
                                    crossing = new Vec2(Crossings1[j].Northing, Crossings1[j].Easting);
                                    crossing.Northing += hcos * -mf.tram.halfWheelTrack + hsin * -(mf.tram.WheelWidth / 2);
                                    crossing.Easting += hsin * -mf.tram.halfWheelTrack + hcos * (mf.tram.WheelWidth / 2);
                                    mf.tram.TramList[mf.tram.TramList.Count - 1].Left.Add(crossing);

                                    //right of left tram
                                    crossing.Northing += hsin * mf.tram.WheelWidth;
                                    crossing.Easting += hcos * -mf.tram.WheelWidth;
                                    mf.tram.TramList[mf.tram.TramList.Count - 1].Left.Add(crossing);

                                    //left of left tram
                                    crossing = new Vec2(Crossings1[j + 1].Northing, Crossings1[j + 1].Easting);
                                    crossing.Northing += hcos * mf.tram.halfWheelTrack + hsin * -(mf.tram.WheelWidth / 2);
                                    crossing.Easting += hsin * mf.tram.halfWheelTrack + hcos * (mf.tram.WheelWidth / 2);
                                    mf.tram.TramList[mf.tram.TramList.Count - 1].Left.Add(crossing);
                                    //right of left tram
                                    crossing.Northing += hsin * mf.tram.WheelWidth;
                                    crossing.Easting += hcos * -mf.tram.WheelWidth;
                                    mf.tram.TramList[mf.tram.TramList.Count - 1].Left.Add(crossing);


                                    //left of right tram
                                    crossing = new Vec2(Crossings2[j].Northing, Crossings2[j].Easting);
                                    crossing.Northing += hcos * -mf.tram.halfWheelTrack + hsin * -(mf.tram.WheelWidth / 2);
                                    crossing.Easting += hsin * -mf.tram.halfWheelTrack + hcos * (mf.tram.WheelWidth / 2);
                                    mf.tram.TramList[mf.tram.TramList.Count - 1].Right.Add(crossing);
                                    //right of right tram
                                    crossing.Northing += hsin * mf.tram.WheelWidth;
                                    crossing.Easting += hcos * -mf.tram.WheelWidth;
                                    mf.tram.TramList[mf.tram.TramList.Count - 1].Right.Add(crossing);



                                    //left of right tram
                                    crossing = new Vec2(Crossings2[j + 1].Northing, Crossings2[j + 1].Easting);
                                    crossing.Northing += hcos * mf.tram.halfWheelTrack + hsin * -(mf.tram.WheelWidth / 2);
                                    crossing.Easting += hsin * mf.tram.halfWheelTrack + hcos * (mf.tram.WheelWidth / 2);
                                    mf.tram.TramList[mf.tram.TramList.Count - 1].Right.Add(crossing);
                                    //right of right tram
                                    crossing.Northing += hsin * mf.tram.WheelWidth;
                                    crossing.Easting += hcos * -mf.tram.WheelWidth;
                                    mf.tram.TramList[mf.tram.TramList.Count - 1].Right.Add(crossing);
                                }
                            }
                        }
                    }
                }
                else
                {
                    mf.tram.TramList.Add(new Trams());

                    //left of left tram
                    pos1A.Northing += hcos * -mf.tram.halfWheelTrack + hsin * (mf.tram.WheelWidth / 2);
                    pos1A.Easting += hsin * -mf.tram.halfWheelTrack + hcos * -(mf.tram.WheelWidth / 2);
                    mf.tram.TramList[mf.tram.TramList.Count - 1].Left.Add(pos1A);
                    //right of left tram
                    pos1A.Northing += hsin * -mf.tram.WheelWidth;
                    pos1A.Easting += hcos * mf.tram.WheelWidth;
                    mf.tram.TramList[mf.tram.TramList.Count - 1].Left.Add(pos1A);

                    //left of left tram
                    pos1B.Northing += hcos * mf.tram.halfWheelTrack + hsin * (mf.tram.WheelWidth / 2);
                    pos1B.Easting += hsin * mf.tram.halfWheelTrack + hcos * -(mf.tram.WheelWidth / 2);
                    mf.tram.TramList[mf.tram.TramList.Count - 1].Left.Add(pos1B);
                    //right of left tram
                    pos1B.Northing += hsin * -mf.tram.WheelWidth;
                    pos1B.Easting += hcos * mf.tram.WheelWidth;
                    mf.tram.TramList[mf.tram.TramList.Count - 1].Left.Add(pos1B);


                    //right of right tram
                    pos2A.Northing += hcos * -mf.tram.halfWheelTrack + hsin * -(mf.tram.WheelWidth / 2);
                    pos2A.Easting += hsin * -mf.tram.halfWheelTrack + hcos * (mf.tram.WheelWidth / 2);
                    mf.tram.TramList[mf.tram.TramList.Count - 1].Right.Add(pos2A);
                    //left of right tram
                    pos2A.Northing += hsin * mf.tram.WheelWidth;
                    pos2A.Easting += hcos * -mf.tram.WheelWidth;
                    mf.tram.TramList[mf.tram.TramList.Count - 1].Right.Add(pos2A);

                    //right of right tram
                    pos2B.Northing += hcos * mf.tram.halfWheelTrack + hsin * -(mf.tram.WheelWidth / 2);
                    pos2B.Easting += hsin * mf.tram.halfWheelTrack + hcos * (mf.tram.WheelWidth / 2);
                    mf.tram.TramList[mf.tram.TramList.Count - 1].Right.Add(pos2B);
                    //left of right tram
                    pos2B.Northing += hsin * mf.tram.WheelWidth;
                    pos2B.Easting += hcos * -mf.tram.WheelWidth;
                    mf.tram.TramList[mf.tram.TramList.Count - 1].Right.Add(pos2B);
                }
            }

        }

        //for calculating for display the averaged new line
        public void SmoothAB(int smPts)
        {
            if (CurrentEditLine < Lines.Count && CurrentEditLine > -1)
            {
                //count the reference list of original curve
                int cnt = Lines[CurrentEditLine].Segments.Count;

                //just go back if not very long
                if (cnt < 10) return;

                //the temp array
                Vec3[] arr = new Vec3[cnt];

                //average them - center weighted average

                bool tt = Lines[CurrentEditLine].Mode != Gmode.Boundary;
                for (int i = 0; i < cnt; i++)
                {
                    int Start = (tt && i < smPts / 2) ? -i : -smPts / 2;
                    int Stop = tt && i > cnt - (smPts / 2) ? cnt - i: smPts / 2;
                    int cntd = Stop - Start;
                    for (int j = Start; j < Stop; j++)
                    {
                        int Idx = (j + i).Clamp(cnt);

                        arr[i].Easting += Lines[CurrentEditLine].Segments[Idx].Easting;
                        arr[i].Northing += Lines[CurrentEditLine].Segments[Idx].Northing;
                    }
                    arr[i].Easting /= cntd;
                    arr[i].Northing /= cntd;
                    arr[i].Heading = Lines[CurrentEditLine].Segments[i].Heading;
                }

                //make a list to draw
                smooList.Clear();
                smooList.AddRange(arr);
            }
        }

        public void SwapHeading(int Idx)
        {
            if (Idx < Lines.Count && Idx > -1)
            {
                int cnt = Lines[Idx].Segments.Count;
                if (cnt > 0)
                {
                    Lines[Idx].Segments.Reverse();
                    Lines[Idx].Heading += Math.PI;
                    Lines[Idx].Heading %= Glm.twoPI;
                    if (Lines[Idx].Mode != Gmode.Heading)
                        Lines[Idx].Segments.CalculateHeading(Lines[Idx].Mode == Gmode.Boundary, CancellationToken.None);
                }
                if (Idx == CurrentTramLine && mf.tram.displayMode > 0) BuildTram();
                if (isSmoothWindowOpen) SmoothAB(SmoothCount * 2);
            }
        }

        //turning the visual line into the real reference line to use
        public void SaveSmoothList()
        {
            if (CurrentEditLine < Lines.Count && CurrentEditLine > -1)
            {
                int cnt = smooList.Count;
                if (cnt < 3) return;

                smooList.CalculateHeading(Lines[CurrentEditLine].Mode == Gmode.Boundary, CancellationToken.None);
                Lines[CurrentEditLine].Segments.Clear();
                Lines[CurrentEditLine].Segments.AddRange(smooList);
                ResetABLine = true;
            }
        }

        public void CalculateExtraGuides(int Idx, double PathsAway)
        {
            double Offset = WidthMinusOverlap * PathsAway;

            if (Lines[CurrentLine].Mode == Gmode.AB || Lines[CurrentLine].Mode == Gmode.Heading)
            {
                double cosHeading = Math.Cos(Lines[CurrentLine].Heading);
                double sinHeading = Math.Sin(Lines[CurrentLine].Heading);

                GuidanceLines[Idx][0].Add(new Vec3(Lines[CurrentLine].Segments[0].Northing + cosHeading * -mf.maxCrossFieldLength + sinHeading * -Offset, Lines[CurrentLine].Segments[0].Easting + sinHeading * -mf.maxCrossFieldLength + cosHeading * Offset, Lines[CurrentLine].Heading));
                GuidanceLines[Idx][0].Add(new Vec3(Lines[CurrentLine].Segments[1].Northing + cosHeading * mf.maxCrossFieldLength + sinHeading * -Offset, Lines[CurrentLine].Segments[1].Easting + sinHeading * mf.maxCrossFieldLength + cosHeading * Offset, Lines[CurrentLine].Heading));
            }
            else
            {
                for (int j = 0; j < Lines[CurrentLine].Segments.Count; j++)
                {
                    var point2 = new Vec3(
                        Lines[CurrentLine].Segments[j].Northing + (Math.Sin(Lines[CurrentLine].Segments[j].Heading) * -Offset),
                        Lines[CurrentLine].Segments[j].Easting + (Math.Cos(Lines[CurrentLine].Segments[j].Heading) * Offset),
                        Lines[CurrentLine].Segments[j].Heading);

                    bool Add = true;
                    for (int t = 0; t < Lines[CurrentLine].Segments.Count; t++)
                    {
                        double dist = ((point2.Easting - Lines[CurrentLine].Segments[t].Easting) * (point2.Easting - Lines[CurrentLine].Segments[t].Easting)) + ((point2.Northing - Lines[CurrentLine].Segments[t].Northing) * (point2.Northing - Lines[CurrentLine].Segments[t].Northing));
                        if (dist < (Offset * Offset) - 0.001)
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

                if (Lines[CurrentLine].Mode == Gmode.Boundary)
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
                        if (dist > Offset * Offset + Offset * Offset)
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
        }

        public void GetCurrentCurveLine(Vec3 pivot, Vec3 steer)
        {
            double minDistance;

            double boundaryTriggerDistance = 1;
            if (CurrentLine < Lines.Count && CurrentLine > -1)
            {
                bool useSteer = mf.isStanleyUsed;

                if (!mf.vehicle.isSteerAxleAhead) useSteer = !useSteer;
                if (mf.vehicle.isReverse) useSteer = !useSteer;

                Vec3 point = useSteer ? steer : pivot;

                if (Lines[CurrentLine].Mode == Gmode.Spiral)
                {
                    minDistance = Glm.Distance(point, Lines[CurrentLine].Segments[0]);

                    double RefDist = minDistance / WidthMinusOverlap;
                    if (RefDist < 0) HowManyPathsAway = (int)(RefDist - 0.5);
                    else HowManyPathsAway = (int)(RefDist + 0.5);

                    if (OldHowManyPathsAway != HowManyPathsAway || ResetABLine)
                    {
                        ResetABLine = false;
                        OldHowManyPathsAway = HowManyPathsAway;
                        if (HowManyPathsAway < 2) HowManyPathsAway = 2;

                        double s = WidthMinusOverlap / 2;

                        curList.Clear();
                        //double circumference = (glm.twoPI * s) / (boundaryTriggerDistance * 0.1);
                        double circumference;

                        for (double round = Glm.twoPI * (HowManyPathsAway - 2); round <= (Glm.twoPI * (HowManyPathsAway + 2) + 0.00001); round += (Glm.twoPI / circumference))
                        {
                            double x = s * (Math.Cos(round) + (round / Math.PI) * Math.Sin(round));
                            double y = s * (Math.Sin(round) - (round / Math.PI) * Math.Cos(round));

                            Vec3 pt = new Vec3(Lines[CurrentLine].Segments[0].Northing + y, Lines[CurrentLine].Segments[0].Easting + x, 0);
                            curList.Add(pt);

                            double radius = Math.Sqrt(x * x + y * y);
                            circumference = (Glm.twoPI * radius) / (boundaryTriggerDistance);
                        }

                        if (curList.Count > 2) curList.CalculateHeading(true, CancellationToken.None);
                    }
                }
                else if (Lines[CurrentLine].Mode == Gmode.Circle)
                {
                    minDistance = Glm.Distance(point, Lines[CurrentLine].Segments[0]);

                    double RefDist = minDistance / WidthMinusOverlap;
                    if (RefDist < 0) HowManyPathsAway = (int)(RefDist - 0.5);
                    else HowManyPathsAway = (int)(RefDist + 0.5);

                    if (OldHowManyPathsAway != HowManyPathsAway && HowManyPathsAway == 0)
                    {
                        OldHowManyPathsAway = HowManyPathsAway;
                        curList.Clear();
                    }
                    else if (OldHowManyPathsAway != HowManyPathsAway || ResetABLine)
                    {
                        ResetABLine = false;
                        if (HowManyPathsAway > 100) return;
                        OldHowManyPathsAway = HowManyPathsAway;

                        curList.Clear();

                        int aa = (int)((Glm.twoPI * WidthMinusOverlap * HowManyPathsAway) / boundaryTriggerDistance);

                        for (double round = 0; round <= Glm.twoPI + 0.00001; round += (Glm.twoPI) / aa)
                        {
                            Vec3 pt = new Vec3(Lines[CurrentLine].Segments[0].Northing + (Math.Cos(round) * WidthMinusOverlap * HowManyPathsAway), Lines[CurrentLine].Segments[0].Easting + (Math.Sin(round) * WidthMinusOverlap * HowManyPathsAway), 0);
                            curList.Add(pt);
                        }

                        if (curList.Count > 2) curList.CalculateHeading(true, CancellationToken.None);
                    }
                }
                else
                {
                    if (Lines[CurrentLine].Segments.Count < 2) return;
                    double minDistA = double.PositiveInfinity, minDistB = double.PositiveInfinity;

                    if (!mf.isAutoSteerBtnOn || ResetABLine)
                    {
                        //find the closest 2 points to current fix
                        for (int t = 0; t < Lines[CurrentLine].Segments.Count; t++)
                        {
                            double dist = ((pivot.Easting - Lines[CurrentLine].Segments[t].Easting) * (pivot.Easting - Lines[CurrentLine].Segments[t].Easting))
                                            + ((pivot.Northing - Lines[CurrentLine].Segments[t].Northing) * (pivot.Northing - Lines[CurrentLine].Segments[t].Northing));
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

                        //are we going same direction as stripList was created?
                        isSameWay = Math.PI - Math.Abs(Math.Abs(point.Heading - Lines[CurrentLine].Segments[A].Heading) - Math.PI) < Glm.PIBy2;
                    }

                    double Dy, Dx;

                    if (Lines[CurrentLine].Mode == Gmode.Heading)
                    {
                        double cosHeading = Math.Cos(Lines[CurrentLine].Heading);
                        double sinHeading = Math.Sin(Lines[CurrentLine].Heading);

                        Dx = 2 * cosHeading * mf.maxCrossFieldLength;
                        Dy = 2 * sinHeading * mf.maxCrossFieldLength;

                        Vec3 Start = new Vec3(Lines[CurrentLine].Segments[0].Northing + cosHeading * -mf.maxCrossFieldLength, Lines[CurrentLine].Segments[0].Easting + sinHeading * -mf.maxCrossFieldLength, 0);
                        Vec3 Stop = new Vec3(Lines[CurrentLine].Segments[1].Northing + cosHeading * mf.maxCrossFieldLength, Lines[CurrentLine].Segments[1].Easting + sinHeading * mf.maxCrossFieldLength, 0);

                        distanceFromRefLine = ((Dx * point.Easting) - (Dy * point.Northing) + (Stop.Easting * Start.Northing) - (Stop.Northing * Start.Easting)) / Math.Sqrt((Dx * Dx) + (Dy * Dy));

                    }
                    else if (A < Lines[CurrentLine].Segments.Count && B < Lines[CurrentLine].Segments.Count)
                    {
                        Dx = Lines[CurrentLine].Segments[B].Northing - Lines[CurrentLine].Segments[A].Northing;
                        Dy = Lines[CurrentLine].Segments[B].Easting - Lines[CurrentLine].Segments[A].Easting;
                        if (Math.Abs(Dy) < double.Epsilon && Math.Abs(Dx) < double.Epsilon) return;
                        distanceFromRefLine = ((Dx * point.Easting) - (Dy * point.Northing) + (Lines[CurrentLine].Segments[B].Easting * Lines[CurrentLine].Segments[A].Northing) - (Lines[CurrentLine].Segments[B].Northing * Lines[CurrentLine].Segments[A].Easting)) / Math.Sqrt((Dx * Dx) + (Dy * Dy));
                    }

                    if (!mf.isAutoSteerBtnOn || ResetABLine)
                    {
                        double RefDist = (distanceFromRefLine + (isSameWay ? GuidanceOffset : -GuidanceOffset)) / WidthMinusOverlap;
                        if (RefDist < 0) HowManyPathsAway = (int)(RefDist - 0.5);
                        else HowManyPathsAway = (int)(RefDist + 0.5);
                    }

                    if ((GuidanceOffset != 0 && OldisSameWay != isSameWay) || HowManyPathsAway != OldHowManyPathsAway || ResetABLine)
                    {
                        if (mf.isSideGuideLines)
                        {
                            if (OldHowManyPathsAway != HowManyPathsAway || ResetABLine)
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

                        ResetABLine = false;
                        OldisSameWay = isSameWay;
                        OldHowManyPathsAway = HowManyPathsAway;

                        CalculateCurList(out curList, WidthMinusOverlap * HowManyPathsAway);
                    }
                }
                mf.CalculateSteerAngle(ref curList, isSameWay, Lines[CurrentLine].Mode == Gmode.Boundary);
            }
            else
            {
                //invalid distance so tell AS module
                mf.distanceFromCurrentLine = 32000;
                mf.guidanceLineDistanceOff = 32000;
            }
        }

        public void CalculateCurList(out List<Vec3> OffsetList, double Offset)
        {
            OffsetList = new List<Vec3>();

            if (isSameWay) Offset -= GuidanceOffset;
            else Offset += GuidanceOffset;

            if (Lines[CurrentLine].Mode == Gmode.AB || Lines[CurrentLine].Mode == Gmode.Heading)
            {
                double cosHeading = Math.Cos(Lines[CurrentLine].Heading);
                double sinHeading = Math.Sin(Lines[CurrentLine].Heading);

                OffsetList.Add(new Vec3(Lines[CurrentLine].Segments[0].Northing + cosHeading * -mf.maxCrossFieldLength + sinHeading * -Offset, Lines[CurrentLine].Segments[0].Easting + sinHeading * -mf.maxCrossFieldLength + cosHeading * Offset, Lines[CurrentLine].Heading));
                OffsetList.Add(new Vec3(Lines[CurrentLine].Segments[1].Northing + cosHeading * mf.maxCrossFieldLength + sinHeading * -Offset, Lines[CurrentLine].Segments[1].Easting + sinHeading * mf.maxCrossFieldLength + cosHeading * Offset, Lines[CurrentLine].Heading));
            }
            else
            {
                if (Lines[CurrentLine].Segments.Count > 1)
                {
                    Vec3 LastVec = Lines[CurrentLine].Segments[0];

                    for (int i = 1; i < Lines[CurrentLine].Segments.Count; i++)
                    {
                        double dx1 = Lines[CurrentLine].Segments[i].Northing - LastVec.Northing;
                        double dy1 = Lines[CurrentLine].Segments[i].Easting - LastVec.Easting;
                        
                        double dist = (dy1 * dy1) + (dx1 * dx1);
                        if (dist > 0.0001)
                        {
                            double Heading = Math.Atan2(dy1, dx1);

                            var point2 = new Vec3(LastVec.Northing + (Math.Sin(Heading) * -Offset), LastVec.Easting + (Math.Cos(Heading) * Offset), Heading);
                            bool Add = true;

                            for (int t = 0; t < Lines[CurrentLine].Segments.Count; t++)
                            {
                                double dist2 = ((point2.Easting - Lines[CurrentLine].Segments[t].Easting) * (point2.Easting - Lines[CurrentLine].Segments[t].Easting)) + ((point2.Northing - Lines[CurrentLine].Segments[t].Northing) * (point2.Northing - Lines[CurrentLine].Segments[t].Northing));
                                if (dist2 < (Offset * Offset) - 0.003)
                                {
                                    Add = false;
                                    break;
                                }
                            }
                            if (Add)
                            {
                                if (OffsetList.Count > 0)
                                {
                                    double dist2 = ((point2.Easting - OffsetList[OffsetList.Count - 1].Easting) * (point2.Easting - OffsetList[OffsetList.Count - 1].Easting)) + ((point2.Northing - OffsetList[OffsetList.Count - 1].Northing) * (point2.Northing - OffsetList[OffsetList.Count - 1].Northing));
                                    if (dist2 > 1) OffsetList.Add(point2);
                                }
                                else OffsetList.Add(point2);
                            }
                            LastVec = Lines[CurrentLine].Segments[i];
                        }
                    }
                    OffsetList.CalculateRoundedCorner(mf.vehicle.minTurningRadius, Lines[CurrentLine].Mode == Gmode.Boundary, 0.0436332, CancellationToken.None);

                    if (OffsetList.Count > 1 && Lines[CurrentLine].Mode == Gmode.Curve)
                    {
                        double cosHeading = Math.Cos(OffsetList[0].Heading);
                        double sinHeading = Math.Sin(OffsetList[0].Heading);

                        OffsetList.Insert(0, new Vec3(OffsetList[0].Northing + cosHeading * -mf.maxCrossFieldLength, OffsetList[0].Easting + sinHeading * -mf.maxCrossFieldLength, OffsetList[0].Heading));

                        cosHeading = Math.Cos(OffsetList[OffsetList.Count - 1].Heading);
                        sinHeading = Math.Sin(OffsetList[OffsetList.Count - 1].Heading);

                        OffsetList.Add(new Vec3(OffsetList[OffsetList.Count - 1].Northing + cosHeading * mf.maxCrossFieldLength, OffsetList[OffsetList.Count - 1].Easting + sinHeading * mf.maxCrossFieldLength, OffsetList[OffsetList.Count - 1].Heading));
                    }
                }
            }
        }

        public void MoveLine(int Idx, double dist)
        {
            if (Idx < Lines.Count && Idx > -1)
            {
                int cnt = Lines[Idx].Segments.Count;

                Vec3 point;
                for (int i = 0; i < cnt; i++)
                {
                    point.Northing = Lines[Idx].Segments[i].Northing + Math.Sin(Lines[Idx].Segments[i].Heading) * -dist;
                    point.Easting = Lines[Idx].Segments[i].Easting + Math.Cos(Lines[Idx].Segments[i].Heading) * dist;
                    point.Heading = Lines[Idx].Segments[i].Heading;
                    Lines[Idx].Segments[i] = point;
                }

                if (Idx == CurrentTramLine && mf.tram.displayMode > 0) BuildTram();

                if (isSmoothWindowOpen) SmoothAB(SmoothCount * 2);
            }
            ResetABLine = true;
        }

        public void AddFirstLastPoints()
        {
            int ptCnt = Lines[CurrentEditLine].Segments.Count;

            if (ptCnt > 4)
            {
                double x = 0, y = 0;
                for (int i = ptCnt - 5; i < ptCnt; i++)
                {
                    x += Math.Cos(Lines[CurrentEditLine].Segments[i].Heading);
                    y += Math.Sin(Lines[CurrentEditLine].Segments[i].Heading);
                }
                x /= 5;
                y /= 5;
                double EndHeading = Math.Atan2(y, x);

                Vec3 EndPoint = Lines[CurrentEditLine].Segments[ptCnt - 1];
                EndPoint.Heading = EndHeading;

                EndPoint.Easting += Math.Sin(EndHeading);
                EndPoint.Northing += Math.Cos(EndHeading);
                Lines[CurrentEditLine].Segments.Add(EndPoint);



                x = 0;
                y = 0;
                for (int i = 0; i < 5; i++)
                {
                    x += Math.Cos(Lines[CurrentEditLine].Segments[i].Heading);
                    y += Math.Sin(Lines[CurrentEditLine].Segments[i].Heading);
                }
                x /= 5;
                y /= 5;
                double StartHeading = Math.Atan2(y, x);

                //and the beginning
                Vec3 StartPoint = Lines[CurrentEditLine].Segments[1];
                StartPoint.Heading = StartHeading;

                StartPoint.Easting -= Math.Sin(StartHeading);
                StartPoint.Northing -= Math.Cos(StartHeading);
                Lines[CurrentEditLine].Segments.Insert(0, StartPoint);
            }
        }
    }




    public enum Gmode { Spiral, Circle, AB, Heading, Curve, Boundary };

    public class CGuidanceLine
    {
        public List<Vec3> Segments = new List<Vec3>();
        public double Heading = 3;
        public string Name = "aa";
        public Gmode Mode;
    }
}
