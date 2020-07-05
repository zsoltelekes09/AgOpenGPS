using System;
using System.Collections.Generic;
using OpenTK.Graphics.OpenGL;

namespace AgOpenGPS
{

    public class CTram
    {
        private readonly FormGPS mf;

        //the list of constants and multiples of the boundary
        public List<Vec2> calcList = new List<Vec2>();

        //the outer ring of boundary tram - also used for clipping
        public List<Vec3> outArr = new List<Vec3>();

        //the triangle strip of the outer tram highlight
        public List<Vec2> tramBndArr = new List<Vec2>();

        //tram settings
        public double wheelTrack;
        public double tramWidth, abOffset;
        public double halfWheelTrack;
        public int passes;

        // 0 off, 1 All, 2, Lines, 3 Outer
        public int displayMode;

        public CTram(FormGPS _f)
        {
            //constructor
            mf = _f;

            tramWidth = Properties.Settings.Default.setTram_eqWidth;
            wheelTrack = Properties.Settings.Default.setTram_wheelSpacing;
            halfWheelTrack = wheelTrack * 0.5;

            passes = Properties.Settings.Default.setTram_passes;
            abOffset = (Math.Round((mf.Tools[0].ToolWidth - mf.Tools[0].ToolOverlap) / 2.0, 3));
            displayMode = 0;
        }

        public void DrawTramBnd()
        {
            if (tramBndArr.Count > 0)
            {
                GL.Color4(0.8630f, 0.73692f, 0.60f, 0.25);
                GL.Begin(PrimitiveType.TriangleStrip);
                for (int h = 0; h < tramBndArr.Count; h++) GL.Vertex3(tramBndArr[h].easting, tramBndArr[h].northing, 0);
                GL.End();
            }
        }

        public void BuildTramBnd()
        {
            if (mf.bnd.bndArr.Count > 0)
            {
                CreateBndTramRef();
                CreateOuterTram();
                PreCalcTurnLines();
            }
            else
            {
                outArr?.Clear();
                tramBndArr?.Clear();
            }
        }

        public void CreateBndTramRef()
        {
            //count the points from the boundary
            int ptCount = mf.bnd.bndArr[0].bndLine.Count;
            outArr?.Clear();

            //outside point
            Vec3 pt3 = new Vec3();

            double distSq = ((tramWidth * 0.5) - halfWheelTrack) * ((tramWidth * 0.5) - halfWheelTrack) * 0.97;
            bool fail = false;
            
            //make the boundary tram outer array
            for (int i = 0; i < ptCount; i++)
            {
                //calculate the point inside the boundary
                pt3.easting = mf.bnd.bndArr[0].bndLine[i].easting -
                    (Math.Sin(Glm.PIBy2 + mf.bnd.bndArr[0].bndLine[i].heading) * (tramWidth * 0.5 - halfWheelTrack));

                pt3.northing = mf.bnd.bndArr[0].bndLine[i].northing -
                    (Math.Cos(Glm.PIBy2 + mf.bnd.bndArr[0].bndLine[i].heading) * (tramWidth * 0.5 - halfWheelTrack));

                for (int j = 0; j < ptCount; j++)
                {
                    double check = Glm.DistanceSquared(pt3.northing, pt3.easting,
                                        mf.bnd.bndArr[0].bndLine[j].northing, mf.bnd.bndArr[0].bndLine[j].easting);
                    if (check < distSq)
                    {
                        fail = true;
                        break;
                    }
                }

                if (!fail)
                {
                    pt3.heading = mf.bnd.bndArr[0].bndLine[i].heading;
                    outArr.Add(pt3);
                }
                fail = false;
            }

            int cnt = outArr.Count;
            if (cnt < 6) return;

            const double spacing = 2.0;
            double distance;
            for (int i = 0; i < cnt - 1; i++)
            {
                distance = Glm.Distance(outArr[i], outArr[i + 1]);
                if (distance < spacing)
                {
                    outArr.RemoveAt(i + 1);
                    cnt = outArr.Count;
                    i--;
                }
            }
        }

        public void CreateOuterTram()
        {
            //build the outer boundary
            tramBndArr?.Clear();

            int cnt = mf.tram.outArr.Count;

            if (cnt > 0)
            {
                Vec2 pt = new Vec2();
                Vec2 pt2 = new Vec2();

                for (int i = 0; i < cnt; i++)
                {
                    pt.easting = mf.tram.outArr[i].easting;
                    pt.northing = mf.tram.outArr[i].northing;
                    tramBndArr.Add(pt);

                    pt2.easting = mf.tram.outArr[i].easting -
                        (Math.Sin(Glm.PIBy2 + mf.tram.outArr[i].heading) * mf.tram.wheelTrack);

                    pt2.northing = mf.tram.outArr[i].northing -
                        (Math.Cos(Glm.PIBy2 + mf.tram.outArr[i].heading) * mf.tram.wheelTrack);
                    tramBndArr.Add(pt2);
                }
            }
        }

        public bool IsPointInTramBndArea(Vec2 TestPoint)
        {
            if (calcList.Count < 3) return false;
            int j = outArr.Count - 1;
            bool oddNodes = false;

            //test against the constant and multiples list the test point
            for (int i = 0; i < outArr.Count; j = i++)
            {
                if ((outArr[i].northing < TestPoint.northing && outArr[j].northing >= TestPoint.northing)
                ||  (outArr[j].northing < TestPoint.northing && outArr[i].northing >= TestPoint.northing))
                {
                    oddNodes ^= ((TestPoint.northing * calcList[i].northing) + calcList[i].easting < TestPoint.easting);
                }
            }
            return oddNodes; //true means inside.
        }

        public void PreCalcTurnLines()
        {
            int j = outArr.Count - 1;
            //clear the list, constant is easting, multiple is northing
            calcList.Clear();
            Vec2 constantMultiple = new Vec2(0, 0);

            for (int i = 0; i < outArr.Count; j = i++)
            {
                //check for divide by zero
                if (Math.Abs(outArr[i].northing - outArr[j].northing) < double.Epsilon)
                {
                    constantMultiple.easting = outArr[i].easting;
                    constantMultiple.northing = 0;
                    calcList.Add(constantMultiple);
                }
                else
                {
                    //determine constant and multiple and add to list
                    constantMultiple.easting = outArr[i].easting - ((outArr[i].northing * outArr[j].easting)
                                    / (outArr[j].northing - outArr[i].northing)) + ((outArr[i].northing * outArr[i].easting)
                                        / (outArr[j].northing - outArr[i].northing));
                    constantMultiple.northing = (outArr[j].easting - outArr[i].easting) / (outArr[j].northing - outArr[i].northing);
                    calcList.Add(constantMultiple);
                }
            }
        }
    }
}
