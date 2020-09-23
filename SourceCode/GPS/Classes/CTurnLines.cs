using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CTurnLines
    {
        //list of coordinates of boundary line
        public List<Vec3> turnLine = new List<Vec3>();

        //the list of constants and multiples of the boundary
        public List<Vec2> calcList = new List<Vec2>();
        public double Northingmin, Northingmax, Eastingmin, Eastingmax;

        public void FixTurnLine(double totalHeadWidth, ref List<Vec3> curBnd)
        {
            double distance;
            for (int i = 0; i < turnLine.Count; i++)
            {
                for (int k = 0; k < curBnd.Count; k++)
                {
                    //remove the points too close to boundary
                    distance = Glm.Distance(curBnd[k], turnLine[i]);
                    if (distance < totalHeadWidth - 0.001)
                    {
                        turnLine.RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            for (int i = 0; i < turnLine.Count; i++)
            {
                int j = (i == turnLine.Count - 1) ? 0 : i + 1;
                //make sure distance isn't too small between points on turnLine
                distance = Glm.Distance(turnLine[i], turnLine[j]);
                if (distance < 2)
                {
                    turnLine.RemoveAt(j);
                    i--;
                }
                else if (distance > 4)//make sure distance isn't too big between points on turnLine
                {
                    double northing = turnLine[i].Northing / 2 + turnLine[j].Northing / 2;
                    double easting = turnLine[i].Easting / 2 + turnLine[j].Easting / 2;
                    double heading = turnLine[i].Heading / 2 + turnLine[j].Heading / 2;
                    if (j == 0) turnLine.Add(new Vec3(northing, easting, heading));
                    turnLine.Insert(j, new Vec3(northing, easting, heading));
                    i--;
                }
            }
            turnLine.CalculateHeadings(true);
        }
        public void PreCalcTurnLines()
        {
            if (turnLine.Count > 3)
            {
                int j = turnLine.Count - 1;
                //clear the list, constant is easting, multiple is northing
                calcList.Clear();
                Vec2 constantMultiple = new Vec2(0, 0);

                Northingmin = Northingmax = turnLine[0].Northing;
                Eastingmin = Eastingmax = turnLine[0].Easting;

                for (int i = 0; i < turnLine.Count; j = i++)
                {
                    if (Northingmin > turnLine[i].Northing) Northingmin = turnLine[i].Northing;
                    if (Northingmax < turnLine[i].Northing) Northingmax = turnLine[i].Northing;
                    if (Eastingmin > turnLine[i].Easting) Eastingmin = turnLine[i].Easting;
                    if (Eastingmax < turnLine[i].Easting) Eastingmax = turnLine[i].Easting;
                    //check for divide by zero
                    if (Math.Abs(turnLine[i].Northing - turnLine[j].Northing) < double.Epsilon)
                    {
                        constantMultiple.Easting = turnLine[i].Easting;
                        constantMultiple.Northing = 0;
                        calcList.Add(constantMultiple);
                    }
                    else
                    {
                        //determine constant and multiple and add to list
                        constantMultiple.Easting = turnLine[i].Easting - ((turnLine[i].Northing * turnLine[j].Easting)
                                        / (turnLine[j].Northing - turnLine[i].Northing)) + ((turnLine[i].Northing * turnLine[i].Easting)
                                            / (turnLine[j].Northing - turnLine[i].Northing));
                        constantMultiple.Northing = (turnLine[j].Easting - turnLine[i].Easting) / (turnLine[j].Northing - turnLine[i].Northing);
                        calcList.Add(constantMultiple);
                    }
                }
            }
        }

        public bool IsPointInTurnWorkArea(Vec3 TestPoint)
        {
            if (calcList.Count < 3) return false;
            int j = turnLine.Count - 1;
            bool oddNodes = false;

            if (TestPoint.Northing > Northingmin || TestPoint.Northing < Northingmax || TestPoint.Easting > Eastingmin || TestPoint.Easting < Eastingmax)
            {
                //test against the constant and multiples list the test point
                for (int i = 0; i < turnLine.Count; j = i++)
                {
                    if ((turnLine[i].Northing < TestPoint.Northing && turnLine[j].Northing >= TestPoint.Northing)
                    || (turnLine[j].Northing < TestPoint.Northing && turnLine[i].Northing >= TestPoint.Northing))
                    {
                        oddNodes ^= ((TestPoint.Northing * calcList[i].Northing) + calcList[i].Easting < TestPoint.Easting);
                    }
                }
            }
            return oddNodes; //true means inside.
        }

        public void DrawTurnLine()
        {
            ////draw the turn line oject
            int ptCount = turnLine.Count;
            if (ptCount < 1) return;
            GL.Begin(PrimitiveType.LineLoop);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(turnLine[h].Easting, turnLine[h].Northing, 0);
            GL.End();
        }
    }
}