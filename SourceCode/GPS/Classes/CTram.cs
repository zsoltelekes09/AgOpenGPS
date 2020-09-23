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

        //tram settings
        public double wheelTrack, WheelWidth;
        public double tramWidth, abOffset;
        public double halfWheelTrack;
        public int passes;
        public double Northingmin, Northingmax, Eastingmin, Eastingmax;

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
            //count the points from the boundary
            int ptCount = mf.bnd.bndArr[0].bndLine.Count;

            TramList.Add(new Trams());

            //outside point
            Vec2 Point = new Vec2();

            double Offset = tramWidth * 0.5 - halfWheelTrack - abOffset;
            double Offset2 = tramWidth * 0.5 + halfWheelTrack - abOffset;

            //make the boundary tram outer array
            for (int i = 0; i < ptCount; i++)
            {
                double CosHeading = Math.Cos(mf.bnd.bndArr[0].bndLine[i].Heading);
                double SinHeading = Math.Sin(mf.bnd.bndArr[0].bndLine[i].Heading);

                //calculate the point inside the boundary
                Point.Northing = mf.bnd.bndArr[0].bndLine[i].Northing + SinHeading * -Offset;
                Point.Easting = mf.bnd.bndArr[0].bndLine[i].Easting + CosHeading * Offset;

                bool fail = false;
                for (int j = 0; j < ptCount; j++)
                {
                    double dist = Glm.DistanceSquared(Point.Northing, Point.Easting, mf.bnd.bndArr[0].bndLine[j].Northing, mf.bnd.bndArr[0].bndLine[j].Easting);
                    if (dist < (Offset * Offset) - 1)
                    {
                        fail = true;
                        break;
                    }
                }
                if (!fail)
                {

                    Point.Northing += SinHeading * (WheelWidth / 2);
                    Point.Easting += CosHeading * (-WheelWidth / 2);
                    TramList[0].Left.Add(Point);
                    Point.Northing += SinHeading * -WheelWidth;
                    Point.Easting += CosHeading * WheelWidth;
                    TramList[0].Left.Add(Point);

                    Point.Northing = mf.bnd.bndArr[0].bndLine[i].Northing + SinHeading * -Offset2;
                    Point.Easting = mf.bnd.bndArr[0].bndLine[i].Easting + CosHeading * Offset2;

                    fail = false;
                    for (int j = 0; j < ptCount; j++)
                    {
                        double dist = Glm.DistanceSquared(Point.Northing, Point.Easting, mf.bnd.bndArr[0].bndLine[j].Northing, mf.bnd.bndArr[0].bndLine[j].Easting);
                        if (dist < (Offset * Offset) - 1)
                        {
                            fail = true;
                            break;
                        }
                    }
                    if (!fail)
                    {
                        Point.Northing += SinHeading * (WheelWidth / 2);
                        Point.Easting += CosHeading * (-WheelWidth / 2);
                        TramList[0].Right.Add(Point);
                        Point.Northing += SinHeading * -WheelWidth;
                        Point.Easting += CosHeading * WheelWidth;
                        TramList[0].Right.Add(Point);
                    }
                }
            }

            if (TramList[0].Left.Count > 1)
            {
                TramList[0].Left.Add(TramList[0].Left[0]);
                TramList[0].Left.Add(TramList[0].Left[1]);
            }
            if (TramList[0].Right.Count > 1)
            {
                TramList[0].Right.Add(TramList[0].Right[0]);
                TramList[0].Right.Add(TramList[0].Right[1]);
            }
        }
    }
}
