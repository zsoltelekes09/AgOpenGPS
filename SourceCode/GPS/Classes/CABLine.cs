using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CABLine
    {
        public double distanceFromRefLine;

        public double HowManyPathsAway;
        public bool isEditing, BtnABLineOn;

        public double snapDistance;
        public int CurrentLine = -1, CurrentEditLine = -1, lineWidth, tryoutcurve = -1;


        //List of all available ABLines
        public List<CABLines> ABLines = new List<CABLines>();

        //pointers to mainform controls
        private readonly FormGPS mf;

        public CABLine(FormGPS _f)
        {
            mf = _f;
            lineWidth = Properties.Settings.Default.setDisplay_lineWidth;
        }

        public void DrawABLines()
        {
            if (CurrentLine < ABLines.Count && CurrentLine > -1)
            {
                GL.LineWidth(lineWidth);

                double cosHeading = Math.Cos(-ABLines[CurrentLine].Heading);
                double sinHeading = Math.Sin(-ABLines[CurrentLine].Heading);

                if (mf.isSideGuideLines || isEditing)
                {
                    double start;
                    double end;
                    if (isEditing)
                    {
                        GL.Color3(0.9630f, 0.2f, 0.2f);
                        GL.LineStipple(1, 0x0707);
                        start = 1;
                        end = 7;
                    }
                    else
                    {
                        GL.Color4(0.56f, 0.650f, 0.650f, 0.5f);
                        GL.LineStipple(1, 0x0101);
                        start = -2.5;
                        end = 3.5;
                    }

                    GL.Enable(EnableCap.LineStipple);
                    GL.Begin(PrimitiveType.Lines);



                    for (; start < end; start++)
                    {
                        double Offset = mf.Guidance.WidthMinusOverlap * ((isEditing ? 0 : HowManyPathsAway) + start);

                        GL.Vertex2(ABLines[CurrentLine].ref1.Easting + cosHeading * Offset - sinHeading * 4000, ABLines[CurrentLine].ref1.Northing + sinHeading * Offset + cosHeading * 4000);

                        if (ABLines[CurrentLine].UsePoint) GL.Vertex2(ABLines[CurrentLine].ref2.Easting + cosHeading * Offset + sinHeading * 4000, ABLines[CurrentLine].ref2.Northing + sinHeading * Offset - cosHeading * 4000);
                        else GL.Vertex2(ABLines[CurrentLine].ref1.Easting + cosHeading * Offset + sinHeading * 4000, ABLines[CurrentLine].ref1.Northing + sinHeading * Offset - cosHeading * 4000);
                    }
                    GL.End();
                    GL.Disable(EnableCap.LineStipple);
                }

                //Draw reference AB line
                GL.Enable(EnableCap.LineStipple);
                GL.LineStipple(1, 0x0F00);
                GL.Begin(PrimitiveType.Lines);
                GL.Color3(0.930f, 0.2f, 0.2f);
                GL.Vertex3(ABLines[CurrentLine].ref1.Easting + sinHeading * 4000, ABLines[CurrentLine].ref1.Northing - cosHeading * 4000, 0);
                if (ABLines[CurrentLine].UsePoint) GL.Vertex3(ABLines[CurrentLine].ref2.Easting - sinHeading * 4000, ABLines[CurrentLine].ref2.Northing + cosHeading * 4000, 0);
                else GL.Vertex3(ABLines[CurrentLine].ref1.Easting - sinHeading * 4000, ABLines[CurrentLine].ref1.Northing + cosHeading * 4000, 0);
                GL.End();
                GL.Disable(EnableCap.LineStipple);

                if (!isEditing)
                {

                    //Draw AB Points
                    GL.PointSize(8.0f);
                    GL.Begin(PrimitiveType.Points);
                    GL.Color3(0.95f, 0.0f, 0.0f);
                    GL.Vertex3(ABLines[CurrentLine].ref1.Easting, ABLines[CurrentLine].ref1.Northing, 0.0);
                    GL.Color3(0.0f, 0.90f, 0.95f);
                    if (ABLines[CurrentLine].UsePoint) GL.Vertex3(ABLines[CurrentLine].ref2.Easting, ABLines[CurrentLine].ref2.Northing, 0.0);
                    GL.End();

                    //draw current AB Line
                    GL.Begin(PrimitiveType.Lines);
                    GL.Color3(0.95f, 0.0f, 0.950f);
                    double Offset = mf.Guidance.WidthMinusOverlap * HowManyPathsAway;

                    if (mf.isABSameAsVehicleHeading) Offset -= mf.Guidance.GuidanceOffset;
                    else Offset += mf.Guidance.GuidanceOffset;

                    GL.Vertex2(ABLines[CurrentLine].ref1.Easting + cosHeading * Offset + sinHeading * 4000, ABLines[CurrentLine].ref1.Northing + sinHeading * Offset - cosHeading * 4000);
                    if (ABLines[CurrentLine].UsePoint) GL.Vertex2(ABLines[CurrentLine].ref2.Easting + cosHeading * Offset - sinHeading * 4000, ABLines[CurrentLine].ref2.Northing + sinHeading * Offset + cosHeading * 4000);
                    else GL.Vertex2(ABLines[CurrentLine].ref1.Easting + cosHeading * Offset - sinHeading * 4000, ABLines[CurrentLine].ref1.Northing + sinHeading * Offset + cosHeading * 4000);
                    GL.End();

                    if (mf.font.isFontOn)
                    {
                        mf.font.DrawText3D(ABLines[CurrentLine].ref1.Easting, ABLines[CurrentLine].ref1.Northing, "&A");
                        if (ABLines[CurrentLine].UsePoint) mf.font.DrawText3D(ABLines[CurrentLine].ref2.Easting, ABLines[CurrentLine].ref2.Northing, "&B");
                    }

                    GL.PointSize(1.0f);


                    if (mf.isPureDisplayOn && !mf.isStanleyUsed)
                    {
                        //Draw lookahead Point
                        GL.PointSize(8.0f);
                        GL.Begin(PrimitiveType.Points);
                        GL.Color3(1.0f, 1.0f, 0.0f);
                        Vec2 Point = mf.GoalPoint;
                        GL.Vertex3(Point.Easting, Point.Northing, 0.0);
                        //GL.Vertex3(rEastAB, rNorthAB, 0.0);
                        GL.Color3(1.0f, 1.5f, 0.95f);
                        GL.Vertex3(mf.rEast, mf.rNorth, 0.0);
                        GL.End();
                    }

                    mf.yt.DrawYouTurn();

                    if (mf.yt.isRecordingCustomYouTurn)
                    {
                        GL.Color3(0.05f, 0.05f, 0.95f);
                        GL.PointSize(2.0f);
                        int ptCount = mf.yt.youFileList.Count;
                        if (ptCount > 1)
                        {
                            GL.Begin(PrimitiveType.Points);
                            for (int i = 1; i < ptCount; i++)
                            {
                                GL.Vertex3(mf.yt.youFileList[i].Easting + mf.yt.youFileList[0].Easting, mf.yt.youFileList[i].Northing + mf.yt.youFileList[0].Northing, 0);
                            }
                            GL.End();
                        }
                    }

                    GL.PointSize(1.0f);
                    GL.LineWidth(1);
                    GL.Color4(0.8630f, 0.73692f, 0.60f, 0.25);
                    mf.tram.DrawTram(false);
                }
            }
        }

        public void BuildTram()
        {
            mf.tram.TramList.Clear();

            if (mf.bnd.bndArr.Count > 0) mf.tram.CreateBndTramRef();

            if (CurrentLine < ABLines.Count && CurrentLine > -1)
            {
                double hsin = Math.Sin(ABLines[CurrentLine].Heading);
                double hcos = Math.Cos(ABLines[CurrentLine].Heading);
                
                int tramcount = mf.tram.TramList.Count;

                for (double i = 0.5; i < mf.tram.passes; i++)
                {
                    double Offset = (mf.tram.tramWidth * i) - mf.Guidance.WidthMinusOverlap / 2 - mf.tram.halfWheelTrack - mf.tram.abOffset;

                    Vec2 pos1A = ABLines[CurrentLine].ref1;
                    pos1A.Northing += hcos * 4000 + hsin * -Offset;
                    pos1A.Easting += hsin * 4000 + hcos * Offset;

                    Vec2 pos1B = ABLines[CurrentLine].UsePoint ? ABLines[CurrentLine].ref2 : ABLines[CurrentLine].ref1;
                    pos1B.Northing += hcos * -4000 + hsin * -Offset;
                    pos1B.Easting += hsin * -4000 + hcos * Offset;

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
                            Crossings1.FindCrossingPoints(ref mf.tram.TramList[m].Left, pos1A, pos1B, 0);
                        }

                        if (Crossings1.Count > 1)
                        {
                            List<Vec4> Crossings2 = new List<Vec4>();
                            for (int m = 0; m < tramcount; m++)
                            {
                                Crossings2.FindCrossingPoints(ref mf.tram.TramList[m].Left, pos2A, pos2B, 0);
                            }

                            if (Crossings2.Count > 1)
                            {
                                Crossings1.Sort((x, y) => x.Heading.CompareTo(y.Heading));
                                Crossings2.Sort((x, y) => x.Heading.CompareTo(y.Heading));

                                if (Crossings1.Count - 1 > Crossings2.Count)
                                {
                                    for (int j = 0; j + 1 < Crossings2.Count; j += 2)
                                    {
                                        for (int l = j + 1; l + 1 < Crossings1.Count; l += 2)
                                        {
                                            if (Crossings2[j].Heading < Crossings1[l].Heading && Crossings1[l + 1].Heading < Crossings2[j + 1].Heading)
                                            {
                                                crossing = new Vec2(Crossings1[l + 1].Northing, Crossings1[l + 1].Easting);
                                                crossing.Northing += hsin * mf.tram.wheelTrack;
                                                crossing.Easting += hcos * mf.tram.wheelTrack;
                                                Crossings2.Insert(j + 1, new Vec4(crossing.Northing, crossing.Easting, Crossings1[l].Heading, 0));
                                                crossing = new Vec2(Crossings1[l].Northing, Crossings1[l].Easting);
                                                crossing.Northing += hsin * mf.tram.wheelTrack;
                                                crossing.Easting += hcos * mf.tram.wheelTrack;
                                                Crossings2.Insert(j + 1, new Vec4(crossing.Northing, crossing.Easting, Crossings1[l].Heading, 0));
                                            }
                                            else if (Crossings2[j + 1].Heading < Crossings1[j].Heading && Crossings2[j + 1].Heading < Crossings1[j + 1].Heading)
                                            {
                                                Crossings1.RemoveAt(j);
                                                Crossings1.RemoveAt(j);
                                            }
                                            else if (Crossings2[j + 1].Heading > Crossings1[j].Heading && Crossings2[j + 1].Heading > Crossings1[j + 1].Heading)
                                            {
                                                Crossings1.RemoveAt(j);
                                                Crossings1.RemoveAt(j);
                                            }

                                        }
                                    }
                                    Crossings2.Sort((x, y) => x.Heading.CompareTo(y.Heading));
                                }
                                else if (Crossings2.Count - 1 > Crossings1.Count)
                                {
                                    for (int j = 0; j + 1 < Crossings1.Count; j += 2)
                                    {
                                        for (int l = j + 1; l + 1 < Crossings2.Count; l += 2)
                                        {
                                            if (Crossings1[j].Heading < Crossings2[l].Heading && Crossings2[l + 1].Heading < Crossings1[j + 1].Heading)
                                            {
                                                crossing = new Vec2(Crossings2[l + 1].Northing, Crossings2[l + 1].Easting);
                                                crossing.Northing += hsin * mf.tram.wheelTrack;
                                                crossing.Easting += hcos * -mf.tram.wheelTrack;
                                                Crossings1.Insert(j + 1, new Vec4(crossing.Northing, crossing.Easting, Crossings2[l].Heading, 0));
                                                crossing = new Vec2(Crossings2[l].Northing, Crossings2[l].Easting);
                                                crossing.Northing += hsin * mf.tram.wheelTrack;
                                                crossing.Easting += hcos * -mf.tram.wheelTrack;
                                                Crossings1.Insert(j + 1, new Vec4(crossing.Northing, crossing.Easting, Crossings2[l].Heading, 0));

                                            }
                                            else if (Crossings1[j + 1].Heading < Crossings2[j].Heading && Crossings1[j + 1].Heading < Crossings2[j + 1].Heading)
                                            {
                                                Crossings2.RemoveAt(j);
                                                Crossings2.RemoveAt(j);
                                            }
                                            else if (Crossings1[j + 1].Heading > Crossings2[j].Heading && Crossings1[j + 1].Heading > Crossings2[j + 1].Heading)
                                            {
                                                Crossings2.RemoveAt(j);
                                                Crossings2.RemoveAt(j);
                                            }
                                        }
                                    }
                                    Crossings1.Sort((x, y) => x.Heading.CompareTo(y.Heading));
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
        }

        public void GetCurrentABLine()
        {
            if (CurrentLine < ABLines.Count && CurrentLine > -1)
            {
                double cosHeading = Math.Cos(ABLines[CurrentLine].Heading);
                double sinHeading = Math.Sin(ABLines[CurrentLine].Heading);

                List<Vec3> Points = new List<Vec3>
                {
                    new Vec3(ABLines[CurrentLine].ref1.Northing, ABLines[CurrentLine].ref1.Easting, ABLines[CurrentLine].Heading),
                    new Vec3(ABLines[CurrentLine].ref1.Northing, ABLines[CurrentLine].ref1.Easting, ABLines[CurrentLine].Heading)
                };

                if (ABLines[CurrentLine].UsePoint)
                    Points[1] = new Vec3(ABLines[CurrentLine].ref2.Northing, ABLines[CurrentLine].ref2.Easting, ABLines[CurrentLine].Heading);
                else
                    Points[1] = new Vec3(Points[1].Northing + cosHeading * 4000, Points[1].Easting + sinHeading * 4000, ABLines[CurrentLine].Heading);

                mf.CalculateSteerAngle(ref Points, true);
            }
        }

        public void MoveLine(double dist)
        {
            int test = -1;
            if (CurrentLine < ABLines.Count && CurrentLine > -1) test = CurrentLine;
            if (CurrentEditLine < ABLines.Count && CurrentEditLine > -1) test = CurrentEditLine;

            if (test >= 0)
            {
                ABLines[test].ref1.Northing += Math.Sin(ABLines[test].Heading) * -dist;
                ABLines[test].ref1.Easting += Math.Cos(ABLines[test].Heading) * dist;
                if (ABLines[test].UsePoint)
                {
                    ABLines[test].ref2.Northing += Math.Sin(ABLines[test].Heading) * -dist;
                    ABLines[test].ref2.Easting += Math.Cos(ABLines[test].Heading) * dist;
                }
            }
        }

        public void SwapAB()//used by Dynamic Tram Mode!!
        {
            if (CurrentEditLine < ABLines.Count && CurrentEditLine > -1)
            {
                ABLines[CurrentEditLine].Heading += Math.PI;
                ABLines[CurrentEditLine].Heading %= Glm.twoPI;
                if (mf.ABLines.ABLines[CurrentEditLine].UsePoint)
                {
                    Vec2 aa = ABLines[CurrentEditLine].ref1;

                    ABLines[CurrentEditLine].ref1 = ABLines[CurrentEditLine].ref2;
                    ABLines[CurrentEditLine].ref2 = aa;
                }
            }
        }

        public void SetABLineBPoint(bool byPoint)
        {
            if (CurrentEditLine < ABLines.Count && CurrentEditLine > -1)
            {
                if (byPoint)
                {
                    ABLines[CurrentEditLine].UsePoint = true;
                    ABLines[CurrentEditLine].ref2.Easting = mf.pn.fix.Easting;
                    ABLines[CurrentEditLine].ref2.Northing = mf.pn.fix.Northing;

                    //calculate the AB Heading
                    ABLines[CurrentEditLine].Heading = Math.Atan2(ABLines[CurrentEditLine].ref2.Easting - ABLines[CurrentEditLine].ref1.Easting, ABLines[CurrentEditLine].ref2.Northing - ABLines[CurrentEditLine].ref1.Northing);
                    if (ABLines[CurrentEditLine].Heading < 0) ABLines[CurrentEditLine].Heading += Glm.twoPI;
                }
                else ABLines[CurrentEditLine].UsePoint = false;
            }
        }
    }

    public class CABLines
    {
        public Vec2 ref1 = new Vec2();
        public Vec2 ref2 = new Vec2();
        public double Heading = 0;
        public string Name = "";
        public bool UsePoint = false;
    }

}