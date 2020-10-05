using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS
{
    public class Trams
    {
        public List<Vec2> Left = new List<Vec2>();
        public List<Vec2> Right = new List<Vec2>();
    }

    public class CTram
    {
        private readonly FormGPS mf;

        public List<Trams> TramList = new List<Trams>();

        public List<List<Vec2>> calcList = new List<List<Vec2>>();
        List<double[]> MaxBound = new List<double[]>();

        //tram settings
        public double wheelTrack, WheelWidth;
        public double tramWidth, abOffset;
        public double halfWheelTrack;
        public int passes;

        // 0 off, 1 All, 2, Lines, 3 Outer
        public int displayMode;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;
            abOffset = Properties.Settings.Default.setTram_offset;
            tramWidth = Properties.Settings.Default.setTram_eqWidth;
            wheelTrack = Properties.Settings.Default.setTram_wheelSpacing;
            WheelWidth = Properties.Settings.Default.Tram_wheelWidth;
            halfWheelTrack = wheelTrack * 0.5;

            passes = Properties.Settings.Default.setTram_passes;
            displayMode = 0;
        }

        public void DrawTram(bool Force)
        {
            if (Force || displayMode > 0)
            {
                int end = (displayMode == 3 && !Force) ? 1 : TramList.Count;
                for (int a = (displayMode == 2 && !Force) ? 1 : 0; a < end; a++)
                {
                    GL.Begin(PrimitiveType.TriangleStrip);
                    for (int b = 0; b < TramList[a].Left.Count; b++)
                    {
                        GL.Vertex3(TramList[a].Left[b].Easting, TramList[a].Left[b].Northing, 0);
                    }
                    GL.End();
                    GL.Begin(PrimitiveType.TriangleStrip);
                    for (int b = 0; b < TramList[a].Right.Count; b++)
                    {
                        GL.Vertex3(TramList[a].Right[b].Easting, TramList[a].Right[b].Northing, 0);
                    }
                    GL.End();
                }

                //draw tram numbers at end and beggining of line
                if (!Force && mf.font.isFontOn && displayMode != 3)
                {
                    for (int i = 1; i < TramList.Count; i++)
                    {
                        GL.Color4(0.8630f, 0.93692f, 0.8260f, 0.752);
                        if (TramList[i].Left.Count > 1)
                        {
                            int End = TramList[i].Left.Count - 2;
                            mf.font.DrawText3D(TramList[i].Left[End].Easting, TramList[i].Left[End].Northing, i.ToString());
                            mf.font.DrawText3D(TramList[i].Left[0].Easting, TramList[i].Left[0].Northing, i.ToString());
                        }
                    }
                }
            }
        }

        public void CreateBndTramRef()
        {
            //make the boundary tram outer array
            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                int ChangeDirection = i == 0 ? 1 : -1;

                //count the points from the boundary
                int ptCount = mf.bnd.bndArr[i].bndLine.Count;

                double Offset = tramWidth * 0.5 - halfWheelTrack - abOffset;
                double Offset2 = tramWidth * 0.5 + halfWheelTrack - abOffset;
                Vec3 Point;

                List<Vec3> BuildLeft = new List<Vec3>();
                List<Vec3> BuildRight = new List<Vec3>();

                //make the boundary tram outer array
                for (int j = 0; j < ptCount; j++)
                {
                    double CosHeading = Math.Cos(mf.bnd.bndArr[i].bndLine[j].Heading);
                    double SinHeading = Math.Sin(mf.bnd.bndArr[i].bndLine[j].Heading);


                    Point = mf.bnd.bndArr[i].bndLine[j];

                    //calculate the point inside the boundary
                    Point.Northing += SinHeading * -Offset * ChangeDirection;
                    Point.Easting += CosHeading * Offset * ChangeDirection;

                    bool fail = false;
                    for (int k = 0; k < ptCount; k++)
                    {
                        double dist = Glm.DistanceSquared(Point.Northing, Point.Easting, mf.bnd.bndArr[i].bndLine[k].Northing, mf.bnd.bndArr[i].bndLine[k].Easting);
                        if (dist < (Offset * Offset) - 0.001)
                        {
                            fail = true;
                            break;
                        }
                    }
                    if (!fail)
                    {
                        BuildLeft.Add(Point);

                        Point = mf.bnd.bndArr[i].bndLine[j];
                        Point.Northing += SinHeading * -Offset2 * ChangeDirection;
                        Point.Easting += CosHeading * Offset2 * ChangeDirection;

                        fail = false;
                        for (int k = 0; k < ptCount; k++)
                        {
                            double dist = Glm.DistanceSquared(Point.Northing, Point.Easting, mf.bnd.bndArr[i].bndLine[k].Northing, mf.bnd.bndArr[i].bndLine[k].Easting);
                            if (dist < (Offset2 * Offset2) - 0.001)
                            {
                                fail = true;
                                break;
                            }
                        }
                        if (!fail)
                        {
                            BuildRight.Add(Point);
                        }
                    }
                }

                if (BuildLeft.Count > 1 && BuildRight.Count > 1)
                {

                    TramList.Add(new Trams());
                    BuildLeft.CalculateRoundedCorner(mf.vehicle.minTurningRadius, true, 0.0436332, true, false, true, halfWheelTrack);

                    BuildLeft.Add(BuildLeft[0]);

                    for (int k = 0; k < BuildLeft.Count; k += 2)
                    {
                        Point = BuildLeft[k];

                        double CosHeading = Math.Cos(BuildLeft[k].Heading);
                        double SinHeading = Math.Sin(BuildLeft[k].Heading);

                        Point.Northing += SinHeading * (WheelWidth / 2);
                        Point.Easting += CosHeading * (-WheelWidth / 2);
                        TramList[i].Left.Add(new Vec2(Point.Northing, Point.Easting));
                        Point.Northing += SinHeading * -WheelWidth;
                        Point.Easting += CosHeading * WheelWidth;
                        TramList[i].Left.Add(new Vec2(Point.Northing, Point.Easting));
                    }

                    BuildRight.Add(BuildRight[0]);

                    BuildRight.CalculateRoundedCorner(mf.vehicle.minTurningRadius, true, 0.0436332, true, false, false, halfWheelTrack);

                    BuildRight.Add(BuildRight[0]);

                    for (int k = 0; k < BuildRight.Count; k += 2)
                    {
                        Point = BuildRight[k];

                        double CosHeading = Math.Cos(BuildRight[k].Heading);
                        double SinHeading = Math.Sin(BuildRight[k].Heading);

                        Point.Northing += SinHeading * (WheelWidth / 2);
                        Point.Easting += CosHeading * (-WheelWidth / 2);
                        TramList[i].Right.Add(new Vec2(Point.Northing, Point.Easting));
                        Point.Northing += SinHeading * -WheelWidth;
                        Point.Easting += CosHeading * WheelWidth;
                        TramList[i].Right.Add(new Vec2(Point.Northing, Point.Easting));
                    }
                }
            }

            //PreCalcTurnLines();
        }

        public void PreCalcTurnLines()
        {
            calcList.Clear();
            MaxBound.Clear();
            for (int i = 0; i < TramList.Count; i++)
            {
                calcList.Add(new List<Vec2>());

                if (TramList[i].Left.Count > 3)
                {
                    int j = TramList[i].Left.Count - 2;

                    Vec2 constantMultiple = new Vec2(0, 0);

                    MaxBound.Add(new double[4] { TramList[i].Left[0].Northing, TramList[i].Left[0].Northing, TramList[i].Left[0].Easting, TramList[i].Left[0].Easting });

                    for (int k = -2; k + 2 < TramList[i].Left.Count; j = k)
                    {
                        k += 2;
                        if (MaxBound[i][0] < TramList[i].Left[k].Northing) MaxBound[i][0] = TramList[i].Left[k].Northing;
                        if (MaxBound[i][1] > TramList[i].Left[k].Northing) MaxBound[i][1] = TramList[i].Left[k].Northing;
                        if (MaxBound[i][2] < TramList[i].Left[k].Easting) MaxBound[i][2] = TramList[i].Left[k].Easting;
                        if (MaxBound[i][3] > TramList[i].Left[k].Easting) MaxBound[i][3] = TramList[i].Left[k].Easting;

                        //check for divide by zero
                        if (Math.Abs(TramList[i].Left[k].Northing - TramList[i].Left[j].Northing) < double.Epsilon)
                        {
                            constantMultiple.Easting = TramList[i].Left[k].Easting;
                            constantMultiple.Northing = 0;
                            calcList[i].Add(constantMultiple);
                        }
                        else
                        {
                            //determine constant and multiple and add to list
                            constantMultiple.Easting = TramList[i].Left[k].Easting - ((TramList[i].Left[k].Northing * TramList[i].Left[j].Easting)
                                            / (TramList[i].Left[j].Northing - TramList[i].Left[k].Northing)) + ((TramList[i].Left[k].Northing * TramList[i].Left[k].Easting)
                                                / (TramList[i].Left[j].Northing - TramList[i].Left[k].Northing));
                            constantMultiple.Northing = (TramList[i].Left[j].Easting - TramList[i].Left[k].Easting) / (TramList[i].Left[j].Northing - TramList[i].Left[k].Northing);
                            calcList[i].Add(constantMultiple);
                        }
                    }
                }
            }
        }

        public bool IsPointInsideTrams(Vec3 pt)
        {
            //if inside outer boundary, then potentially add
            if (calcList.Count > 0 && IsPointInGeoFenceArea(0 , pt))
            {
                for (int b = 1; b < calcList.Count; b++)
                {
                    if (IsPointInGeoFenceArea(b, pt))
                    {
                        //point is in an inner turn area but inside outer
                        return false;
                    }
                }
                return true;
            }
            else return false;
        }

        public bool IsPointInGeoFenceArea(int idx, Vec3 TestPoint)
        {
            if (calcList[idx].Count < 3) return false;
            int j = TramList[idx].Left.Count - 2;
            bool oddNodes = false;

            if (TestPoint.Northing > MaxBound[idx][1] || TestPoint.Northing < MaxBound[idx][0] || TestPoint.Easting > MaxBound[idx][3] || TestPoint.Easting < MaxBound[idx][2])
            {
                //test against the constant and multiples list the test point
                for (int k = -2; k + 2 < TramList[idx].Left.Count; j = k)
                {
                    k += 2;
                    if ((TramList[idx].Left[k].Northing < TestPoint.Northing && TramList[idx].Left[j].Northing >= TestPoint.Northing)
                    || (TramList[idx].Left[j].Northing < TestPoint.Northing && TramList[idx].Left[k].Northing >= TestPoint.Northing))
                    {
                        oddNodes ^= ((TestPoint.Northing * calcList[idx][k/2].Northing) + calcList[idx][k/2].Easting < TestPoint.Easting);
                    }
                }
            }
            return oddNodes; //true means inside.
        }
    }
}
