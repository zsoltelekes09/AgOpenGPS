using System;
using System.Collections.Generic;
using System.Threading;

namespace AgOpenGPS
{
    public partial class CBoundaryLines
    {
        //list of coordinates of boundary line
        public List<Vec3> turnLine = new List<Vec3>();

        //the list of constants and multiples of the boundary
        public List<Vec2> calcList = new List<Vec2>();

        public void BuildTurnLine(List<Vec3> bndLine, double triggerDistanceOffset, CancellationToken ct, out List<Vec3> turnLine2, out List<Vec2> calcList2)
        {
            turnLine2 = new List<Vec3>();
            calcList2 = new List<Vec2>();

            //to fill the list of line points
            Vec3 point = new Vec3();
            for (int i = 0; i < bndLine.Count; i++)
            {
                if (ct.IsCancellationRequested) break;
                point.Northing = bndLine[i].Northing + Math.Sin(bndLine[i].Heading) * -triggerDistanceOffset;
                point.Easting = bndLine[i].Easting + Math.Cos(bndLine[i].Heading) * triggerDistanceOffset;
                point.Heading = bndLine[i].Heading;
                turnLine2.Add(point);
            }

            FixTurnLine(ref turnLine2, Math.Abs(triggerDistanceOffset), ref bndLine, ct);
            PreCalcTurnLines(ref turnLine2, ref calcList2, ct);
        }

        public void FixTurnLine(ref List<Vec3> turnLine2, double totalHeadWidth, ref List<Vec3> curBnd, CancellationToken ct)
        {
            double distance;

            turnLine2.PolygonArea(ct, true);
            List<List<Vec3>> tt = turnLine2.ClipPolyLine(curBnd, true, totalHeadWidth, ct);
            if (tt.Count > 0) turnLine2 = tt[0];

            for (int i = 0; i < turnLine2.Count; i++)
            {
                if (ct.IsCancellationRequested) break;
                int j = (i == turnLine2.Count - 1) ? 0 : i + 1;
                //make sure distance isn't too small between points on turnLine
                distance = Glm.Distance(turnLine2[i], turnLine2[j]);
                if (distance < 2)
                {
                    turnLine2.RemoveAt(j);
                    i--;
                }
            }
            turnLine2.CalculateHeading(true, ct);
        }

        public void PreCalcTurnLines(ref List<Vec3> turnLine2, ref List<Vec2> calcList2, CancellationToken ct)
        {
            if (turnLine2.Count > 3)
            {
                int j = turnLine2.Count - 1;
                //clear the list, constant is easting, multiple is northing
                Vec2 constantMultiple = new Vec2(0, 0);

                for (int i = 0; i < turnLine2.Count; j = i++)
                {
                    if (ct.IsCancellationRequested) break;
                    //check for divide by zero
                    if (Math.Abs(turnLine2[i].Northing - turnLine2[j].Northing) < double.Epsilon)
                    {
                        constantMultiple.Easting = turnLine2[i].Easting;
                        constantMultiple.Northing = 0;
                        calcList2.Add(constantMultiple);
                    }
                    else
                    {
                        //determine constant and multiple and add to list
                        constantMultiple.Easting = turnLine2[i].Easting - ((turnLine2[i].Northing * turnLine2[j].Easting)
                                        / (turnLine2[j].Northing - turnLine2[i].Northing)) + ((turnLine2[i].Northing * turnLine2[i].Easting)
                                            / (turnLine2[j].Northing - turnLine2[i].Northing));
                        constantMultiple.Northing = (turnLine2[j].Easting - turnLine2[i].Easting) / (turnLine2[j].Northing - turnLine2[i].Northing);
                        calcList2.Add(constantMultiple);
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
    }
}