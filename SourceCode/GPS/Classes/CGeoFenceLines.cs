using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CGeoFenceLines
    {
        //list of coordinates of boundary line
        public List<Vec2> geoFenceLine = new List<Vec2>();

        //the list of constants and multiples of the boundary
        public List<Vec2> calcList = new List<Vec2>();

        public double Northingmin, Northingmax, Eastingmin, Eastingmax;

        public bool IsPointInGeoFenceArea(Vec3 TestPoint)
        {
            if (calcList.Count < 3) return false;
            int j = geoFenceLine.Count - 1;
            bool oddNodes = false;

            if (TestPoint.northing > Northingmin || TestPoint.northing < Northingmax || TestPoint.easting > Eastingmin || TestPoint.easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < geoFenceLine.Count; j = i++)
                {
                    if ((geoFenceLine[i].northing < TestPoint.northing && geoFenceLine[j].northing >= TestPoint.northing)
                    || (geoFenceLine[j].northing < TestPoint.northing && geoFenceLine[i].northing >= TestPoint.northing))
                    {
                        oddNodes ^= ((TestPoint.northing * calcList[i].northing) + calcList[i].easting < TestPoint.easting);
                    }
                }
            }
            return oddNodes; //true means inside.
        }

        public bool IsPointInGeoFenceArea(Vec2 TestPoint)
        {
            if (calcList.Count < 3) return false;
            int j = geoFenceLine.Count - 1;
            bool oddNodes = false;

            if (TestPoint.northing > Northingmin || TestPoint.northing < Northingmax || TestPoint.easting > Eastingmin || TestPoint.easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < geoFenceLine.Count; j = i++)
                {
                    if ((geoFenceLine[i].northing < TestPoint.northing && geoFenceLine[j].northing >= TestPoint.northing)
                    || (geoFenceLine[j].northing < TestPoint.northing && geoFenceLine[i].northing >= TestPoint.northing))
                    {
                        oddNodes ^= ((TestPoint.northing * calcList[i].northing) + calcList[i].easting < TestPoint.easting);
                    }
                }
            }
            return oddNodes; //true means inside.
        }

        public void DrawGeoFenceLine()
        {
            ////draw the turn line oject
            if (geoFenceLine.Count < 1) return;
            int ptCount = geoFenceLine.Count;
            GL.LineWidth(3);
            GL.Color3(0.96555f, 0.1232f, 0.50f);
            //GL.PointSize(4);

            GL.Begin(PrimitiveType.LineLoop);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(geoFenceLine[h].easting, geoFenceLine[h].northing, 0);
            GL.End();
        }

        public void FixGeoFenceLine(double totalHeadWidth, List<Vec3> curBnd, double spacing)
        {
            //count the points from the boundary
            int lineCount = geoFenceLine.Count;

            //int headCount = mf.bndArr[inTurnNum].bndLine.Count;
            int bndCount = curBnd.Count;

            double distance;
            //remove the points too close to boundary
            for (int i = 0; i < bndCount; i++)
            {
                for (int j = 0; j < lineCount; j++)
                {
                    //make sure distance between headland and boundary is not less then width
                    distance = Glm.Distance(curBnd[i], geoFenceLine[j]);
                    if (distance < (totalHeadWidth * 0.96))
                    {
                        geoFenceLine.RemoveAt(j);
                        lineCount = geoFenceLine.Count;
                        j = -1;
                    }
                }
            }

            //make sure distance isn't too small between points on turnLine
            bndCount = geoFenceLine.Count;

            //double spacing = mf.Tools[0].toolWidth * 0.25;
            for (int i = 0; i < bndCount - 1; i++)
            {
                distance = Glm.Distance(geoFenceLine[i], geoFenceLine[i + 1]);
                if (distance < spacing)
                {
                    geoFenceLine.RemoveAt(i + 1);
                    bndCount = geoFenceLine.Count;
                    i--;
                }
            }

            //make sure distance isn't too big between points on Turn
            bndCount = geoFenceLine.Count;
            for (int i = 0; i < bndCount; i++)
            {
                int j = i + 1;
                if (j == bndCount) j = 0;
                distance = Glm.Distance(geoFenceLine[i], geoFenceLine[j]);
                if (distance > (spacing * 1.25))
                {
                    Vec2 pointB = new Vec2((geoFenceLine[i].easting + geoFenceLine[j].easting) / 2.0, (geoFenceLine[i].northing + geoFenceLine[j].northing) / 2.0);

                    geoFenceLine.Insert(j, pointB);
                    bndCount = geoFenceLine.Count;
                    i--;
                }
            }

            //make sure headings are correct for calculated points
            //CalculateTurnHeadings();
        }

        public void PreCalcTurnLines()
        {
            if (geoFenceLine.Count > 3)
            {
                int j = geoFenceLine.Count - 1;
                //clear the list, constant is easting, multiple is northing
                calcList.Clear();
                Vec2 constantMultiple = new Vec2(0, 0);

                Northingmin = Northingmax = geoFenceLine[0].northing;
                Eastingmin = Eastingmax = geoFenceLine[0].easting;


                for (int i = 0; i < geoFenceLine.Count; j = i++)
                {
                    if (Northingmin > geoFenceLine[i].northing) Northingmin = geoFenceLine[i].northing;
                    if (Northingmax < geoFenceLine[i].northing) Northingmax = geoFenceLine[i].northing;
                    if (Eastingmin > geoFenceLine[i].easting) Eastingmin = geoFenceLine[i].easting;
                    if (Eastingmax < geoFenceLine[i].easting) Eastingmax = geoFenceLine[i].easting;

                    //check for divide by zero
                    if (Math.Abs(geoFenceLine[i].northing - geoFenceLine[j].northing) < double.Epsilon)
                    {
                        constantMultiple.easting = geoFenceLine[i].easting;
                        constantMultiple.northing = 0;
                        calcList.Add(constantMultiple);
                    }
                    else
                    {
                        //determine constant and multiple and add to list
                        constantMultiple.easting = geoFenceLine[i].easting - ((geoFenceLine[i].northing * geoFenceLine[j].easting)
                                        / (geoFenceLine[j].northing - geoFenceLine[i].northing)) + ((geoFenceLine[i].northing * geoFenceLine[i].easting)
                                            / (geoFenceLine[j].northing - geoFenceLine[i].northing));
                        constantMultiple.northing = (geoFenceLine[j].easting - geoFenceLine[i].easting) / (geoFenceLine[j].northing - geoFenceLine[i].northing);
                        calcList.Add(constantMultiple);
                    }
                }
            }
        }
    }
}