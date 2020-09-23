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

            if (TestPoint.Northing > Northingmin || TestPoint.Northing < Northingmax || TestPoint.Easting > Eastingmin || TestPoint.Easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < geoFenceLine.Count; j = i++)
                {
                    if ((geoFenceLine[i].Northing < TestPoint.Northing && geoFenceLine[j].Northing >= TestPoint.Northing)
                    || (geoFenceLine[j].Northing < TestPoint.Northing && geoFenceLine[i].Northing >= TestPoint.Northing))
                    {
                        oddNodes ^= ((TestPoint.Northing * calcList[i].Northing) + calcList[i].Easting < TestPoint.Easting);
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

            if (TestPoint.Northing > Northingmin || TestPoint.Northing < Northingmax || TestPoint.Easting > Eastingmin || TestPoint.Easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < geoFenceLine.Count; j = i++)
                {
                    if ((geoFenceLine[i].Northing < TestPoint.Northing && geoFenceLine[j].Northing >= TestPoint.Northing)
                    || (geoFenceLine[j].Northing < TestPoint.Northing && geoFenceLine[i].Northing >= TestPoint.Northing))
                    {
                        oddNodes ^= ((TestPoint.Northing * calcList[i].Northing) + calcList[i].Easting < TestPoint.Easting);
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
            for (int h = 0; h < ptCount; h++) GL.Vertex3(geoFenceLine[h].Easting, geoFenceLine[h].Northing, 0);
            GL.End();
        }

        public void FixGeoFenceLine(double totalHeadWidth, in List<Vec3> curBnd)
        {
            double distance;
            for (int i = 0; i < geoFenceLine.Count; i++)
            {
                for (int k = 0; k < curBnd.Count; k++)
                {
                    //remove the points too close to boundary
                    distance = Glm.Distance(curBnd[k], geoFenceLine[i]);
                    if (distance < totalHeadWidth - 0.001)
                    {
                        geoFenceLine.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            for (int i = 0; i < geoFenceLine.Count; i++)
            {
                if (i > 0)
                {
                    int j = (i == geoFenceLine.Count - 1) ? 0 : i + 1;
                    //make sure distance isn't too small between points on turnLine
                    distance = Glm.Distance(geoFenceLine[i], geoFenceLine[j]);
                    if (distance < 2)
                    {
                        geoFenceLine.RemoveAt(j);
                        i--;
                    }
                    else if (distance > 4)//make sure distance isn't too big between points on turnLine
                    {
                        if (j == 0 ) geoFenceLine.Add(new Vec2((geoFenceLine[i].Northing + geoFenceLine[j].Northing) / 2, (geoFenceLine[i].Easting + geoFenceLine[j].Easting) / 2));
                        geoFenceLine.Insert(j, new Vec2((geoFenceLine[i].Northing + geoFenceLine[j].Northing) / 2, (geoFenceLine[i].Easting + geoFenceLine[j].Easting) / 2));
                        i--;
                    }
                }
            }
        }

        public void PreCalcTurnLines()
        {
            if (geoFenceLine.Count > 3)
            {
                int j = geoFenceLine.Count - 1;
                //clear the list, constant is easting, multiple is northing
                calcList.Clear();
                Vec2 constantMultiple = new Vec2(0, 0);

                Northingmin = Northingmax = geoFenceLine[0].Northing;
                Eastingmin = Eastingmax = geoFenceLine[0].Easting;


                for (int i = 0; i < geoFenceLine.Count; j = i++)
                {
                    if (Northingmin > geoFenceLine[i].Northing) Northingmin = geoFenceLine[i].Northing;
                    if (Northingmax < geoFenceLine[i].Northing) Northingmax = geoFenceLine[i].Northing;
                    if (Eastingmin > geoFenceLine[i].Easting) Eastingmin = geoFenceLine[i].Easting;
                    if (Eastingmax < geoFenceLine[i].Easting) Eastingmax = geoFenceLine[i].Easting;

                    //check for divide by zero
                    if (Math.Abs(geoFenceLine[i].Northing - geoFenceLine[j].Northing) < double.Epsilon)
                    {
                        constantMultiple.Easting = geoFenceLine[i].Easting;
                        constantMultiple.Northing = 0;
                        calcList.Add(constantMultiple);
                    }
                    else
                    {
                        //determine constant and multiple and add to list
                        constantMultiple.Easting = geoFenceLine[i].Easting - ((geoFenceLine[i].Northing * geoFenceLine[j].Easting)
                                        / (geoFenceLine[j].Northing - geoFenceLine[i].Northing)) + ((geoFenceLine[i].Northing * geoFenceLine[i].Easting)
                                            / (geoFenceLine[j].Northing - geoFenceLine[i].Northing));
                        constantMultiple.Northing = (geoFenceLine[j].Easting - geoFenceLine[i].Easting) / (geoFenceLine[j].Northing - geoFenceLine[i].Northing);
                        calcList.Add(constantMultiple);
                    }
                }
            }
        }
    }
}