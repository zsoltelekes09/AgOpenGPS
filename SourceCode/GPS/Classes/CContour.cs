using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Threading;

namespace AgOpenGPS
{
    public class CContour
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        public bool isContourOn, isContourBtnOn, isRightPriority = true;
        public bool OldisSameWay, isSameWay;
        public double HowManyPathsAway, OldHowManyPathsAway;
        public bool ResetLine = false;
        public int Oldstrip, strip = 0, pt = 0;

        //angle to path line closest point and fix
        public double refHeading;

        // for closest line point to current fix
        public double Distance, refEast, refNorth;

        //generated reference line
        public double refLineSide = 1.0;

        public Vec2 refPoint1 = new Vec2(1, 1), refPoint2 = new Vec2(2, 2);

        public double distanceFromRefLine;
        public double distanceFromCurrentLine;

        public double abFixHeadingDelta, abHeading;

        public bool isABSameAsVehicleHeading = true;

        //pure pursuit values
        public bool isValid;

        public Vec2 goalPointCT = new Vec2(0, 0);
        public Vec2 radiusPointCT = new Vec2(0, 0);
        public double steerAngleCT;
        public double rEastCT, rNorthCT;
        public double ppRadiusCT;

        //list of the list of individual Lines for entire field
        public List<List<Vec3>> stripList = new List<List<Vec3>>();
        readonly List<List<Vec3>> CurrentBuilds = new List<List<Vec3>>();

        //list of points for the new contour line
        public List<Vec3> ctList = new List<Vec3>();

        //list of points to determine position ofnew contour line
        public List<CVec> conList = new List<CVec>();

        //constructor
        public CContour(FormGPS _f)
        {
            mf = _f;
        }

        //Add current position to stripList
        public void AddPoint(Vec3 pivot, bool start)
        {
            if (!start)
            {
                isContourOn = true;
                stripList.Add(new List<Vec3>());
            }
            Vec3 point = new Vec3(pivot.Northing, pivot.Easting, pivot.Heading);
            stripList[stripList.Count - 1].Add(point);
        }

        //End the strip
        public void StopContourLine(Vec3 pivot)
        {
            if (stripList.Count > 0)
            {
                //make sure its long enough to bother
                if (stripList[stripList.Count - 1].Count > 10)
                {
                    Vec3 point = new Vec3(pivot.Northing, pivot.Easting, mf.fixHeading);
                    stripList[stripList.Count - 1].Add(point);

                    //add the point list to the save list for appending to contour file
                    mf.ContourSaveList.Add(stripList[stripList.Count - 1]);
                }
                else
                {
                    int ra = stripList.Count - 1;
                    if (ra > 0) stripList.RemoveAt(ra);
                }
            }

            //turn it off
            isContourOn = false;
        }

        //build contours for boundaries
        public void BuildBoundaryContours(double pass, double spacingInt)
        {
            Vec3 point = new Vec3();
            double totalHeadWidth = (mf.Guidance.WidthMinusOverlap * (pass - 0.5)) + spacingInt;

            for (int j = 0; j < mf.bnd.bndArr.Count; j++)
            {
                int ChangeDirection = j == 0 ? 1 : -1;

                //count the points from the boundary
                int ptCount = mf.bnd.bndArr[j].bndLine.Count;

                stripList.Add(new List<Vec3>());

                for (int i = ptCount - 1; i >= 0; i--)
                {
                    //calculate the point inside the boundary
                    point.Northing = mf.bnd.bndArr[j].bndLine[i].Northing + (Math.Sin(mf.bnd.bndArr[j].bndLine[i].Heading) * -totalHeadWidth * ChangeDirection);
                    point.Easting = mf.bnd.bndArr[j].bndLine[i].Easting + (Math.Cos(mf.bnd.bndArr[j].bndLine[i].Heading) * totalHeadWidth * ChangeDirection);
                    point.Heading = mf.bnd.bndArr[j].bndLine[i].Heading;

                    //only add if inside actual field boundary
                    stripList[stripList.Count - 1].Add(point);
                }

                FixContourLine(totalHeadWidth, ref mf.bnd.bndArr[j].bndLine);
            }
        }

        public void FixContourLine(double totalHeadWidth, ref List<Vec3> curBnd)
        {
            double distance;
            for (int i = 0; i < stripList[stripList.Count - 1].Count; i++)
            {
                for (int k = 0; k < curBnd.Count; k++)
                {
                    //remove the points too close to boundary
                    distance = Glm.Distance(curBnd[k], stripList[stripList.Count - 1][i]);
                    if (distance < totalHeadWidth - 0.001)
                    {
                        stripList[stripList.Count - 1].RemoveAt(i);
                        i--;
                        break;
                    }
                }
            }

            for (int i = 0; i < stripList[stripList.Count - 1].Count; i++)
            {
                int j = (i == stripList[stripList.Count - 1].Count - 1) ? 0 : i + 1;
                //make sure distance isn't too small between points on turnLine
                distance = Glm.Distance(stripList[stripList.Count - 1][i], stripList[stripList.Count - 1][j]);
                if (distance < 2)
                {
                    stripList[stripList.Count - 1].RemoveAt(j);
                    i--;
                }
                else if (distance > 4)//make sure distance isn't too big between points on turnLine
                {
                    double northing = stripList[stripList.Count - 1][i].Northing / 2 + stripList[stripList.Count - 1][j].Northing / 2;
                    double easting = stripList[stripList.Count - 1][i].Easting / 2 + stripList[stripList.Count - 1][j].Easting / 2;
                    double heading = stripList[stripList.Count - 1][i].Heading / 2 + stripList[stripList.Count - 1][j].Heading / 2;
                    if (j == 0) stripList[stripList.Count - 1].Add(new Vec3(northing, easting, heading));
                    stripList[stripList.Count - 1].Insert(j, new Vec3(northing, easting, heading));
                    i--;
                }
            }

            stripList[stripList.Count - 1].CalculateRoundedCorner(0.5, true, 0.0436332, CancellationToken.None);
        }

        //determine distance from contour guidance line
        public void DistanceFromContourLine(Vec3 PivotAxlePos, Vec3 SteerAxlePos)
        {
            bool UseSteer = mf.isStanleyUsed;

            if (!mf.vehicle.isSteerAxleAhead) UseSteer = !UseSteer;
            bool isreversedriving = mf.pn.speed < -0.09;
            if (isreversedriving) UseSteer = !UseSteer;

            Vec3 Point = (UseSteer) ? SteerAxlePos : PivotAxlePos;

            if (!mf.isAutoSteerBtnOn || ctList.Count < 5 || ResetLine)
            {
                int closestRefPointRight = 0, ClosestStripRight = 0;
                int closestRefPoint = 0;

                double dx;
                //z2-z1
                double dy;

                Distance = double.PositiveInfinity;

                int stripCount = stripList.Count;
                for (int s = 0; s < stripCount; s++)
                {
                    double MinStripDistance = double.PositiveInfinity;


                    int StripCount = stripList[s].Count;
                    for (int p = 0; p < StripCount; p +=4)
                    {
                        dx = Point.Northing - stripList[s][p].Northing;
                        dy = Point.Easting - stripList[s][p].Easting;
                        double dist = (dx * dx) + (dy * dy);

                        if (dist < Distance)
                        {
                            if (dist < MinStripDistance)
                            {
                                MinStripDistance = dist;
                                closestRefPoint = p;
                            }

                            double angle = (mf.fixHeading - stripList[s][p].Heading);

                            if (angle < 0) angle += Glm.twoPI;
                            if (angle > Glm.twoPI) angle -= Glm.twoPI;
                            double diff = 0.436332;//25deg

                            if ((angle < diff || angle > Glm.twoPI - diff) || (angle > Math.PI - diff && angle < Math.PI + diff))
                            {
                                if (Math.Cos(Point.Heading) * (stripList[s][p].Easting - Point.Easting) < Math.Sin(Point.Heading) * (stripList[s][p].Northing - Point.Northing))
                                {
                                    if (isRightPriority) dist *= 10000.0;
                                }
                                else if (!isRightPriority) dist *= 10000.0;

                                if (dist < Distance)
                                {
                                    Distance = dist;
                                    ClosestStripRight = s;
                                    closestRefPointRight = p;
                                }
                            }
                        }
                    }

                    if (!double.IsInfinity(MinStripDistance))
                    {
                        if (ClosestStripRight == s) closestRefPointRight = closestRefPoint;
                    }
                }

                if (double.IsInfinity(Distance)) return;

                strip = ClosestStripRight;
                pt = closestRefPointRight;

                //now we have closest point, the distance squared from it, and which patch and point its from

                refEast = stripList[strip][pt].Easting;
                refNorth = stripList[strip][pt].Northing;
                refHeading = stripList[strip][pt].Heading;

                //which side of the patch are we on is next
                //calculate endpoints of reference line based on closest point
                refPoint1.Northing = refNorth - (Math.Cos(refHeading) * 50.0);
                refPoint1.Easting = refEast - (Math.Sin(refHeading) * 50.0);

                refPoint2.Northing = refNorth + (Math.Cos(refHeading) * 50.0);
                refPoint2.Easting = refEast + (Math.Sin(refHeading) * 50.0);


                //x2-x1
                dx = refPoint2.Easting - refPoint1.Easting;
                //z2-z1
                dy = refPoint2.Northing - refPoint1.Northing;

                if (Math.Abs(dx) < double.Epsilon && Math.Abs(dy) < double.Epsilon) return;

                //how far are we away from the reference line at 90 degrees - 2D cross product and distance
                distanceFromRefLine = ((dy * mf.pn.fix.Easting) - (dx * mf.pn.fix.Northing) + (refPoint2.Easting
                                        * refPoint1.Northing) - (refPoint2.Northing * refPoint1.Easting))
                                            / Math.Sqrt((dy * dy) + (dx * dx));

                isSameWay = Math.PI - Math.Abs(Math.Abs(Point.Heading - refHeading) - Math.PI) < Glm.PIBy2;

                if (isSameWay) distanceFromRefLine += mf.Guidance.GuidanceOffset;
                else distanceFromRefLine -= mf.Guidance.GuidanceOffset;



                HowManyPathsAway = Math.Round(distanceFromRefLine / mf.Guidance.WidthMinusOverlap, 0, MidpointRounding.AwayFromZero);

                //absolute the distance
                distanceFromRefLine = Math.Abs(distanceFromRefLine);
            }

            if (OldisSameWay != isSameWay || HowManyPathsAway != OldHowManyPathsAway || strip != Oldstrip || ResetLine)
            {
                ResetLine = false;
                OldisSameWay = isSameWay;
                OldHowManyPathsAway = HowManyPathsAway;
                Oldstrip = strip;

                bool fail = false;

                double Offset = mf.Guidance.WidthMinusOverlap * HowManyPathsAway;
                double distSq = Offset * Offset - 0.001;

                List<Vec3> CurrentBuild = new List<Vec3>();

                for (int i = 0; i < stripList[strip].Count; i++)
                {
                    stripList[strip].PolygonArea(CancellationToken.None, true);

                    var point = new Vec3(
                    stripList[strip][i].Northing + (Math.Sin(stripList[strip][i].Heading) * -Offset),
                        stripList[strip][i].Easting + (Math.Cos(stripList[strip][i].Heading) * Offset),
                        stripList[strip][i].Heading);

                    if (!fail) CurrentBuild.Add(point);
                    fail = false;
                }

                int ctCount = CurrentBuild.Count;
                if (ctCount < 6) return;


                CurrentBuilds.Clear();
                CurrentBuilds.AddRange(CurrentBuild.ClipPolyLine(null, true, distSq, CancellationToken.None));

                if (CurrentBuilds.Count < 1) return;

                double dx;
                //z2-z1
                double dy;

                int ClosestStripRight = 0;
                Distance = double.PositiveInfinity;
                double MinStripDistance = double.PositiveInfinity;

                for (int s = 0; s < CurrentBuilds.Count; s++)
                {
                    int StripCount = CurrentBuilds[s].Count;
                    for (int p = 0; p < StripCount; p += 4)
                    {
                        dx = Point.Northing - CurrentBuilds[s][p].Northing;
                        dy = Point.Easting - CurrentBuilds[s][p].Easting;
                        double dist = (dx * dx) + (dy * dy);

                        if (dist < Distance)
                        {
                            if (dist < MinStripDistance)
                            {
                                MinStripDistance = dist;
                                ClosestStripRight = s;
                            }
                        }
                    }
                }

                ctList.Clear();
                ctList.AddRange(CurrentBuilds[ClosestStripRight]);

                ctCount = ctList.Count;

                const double spacing = 1.0;
                double distance;
                for (int i = 0; i < ctCount - 1; i++)
                {
                    distance = Glm.Distance(ctList[i], ctList[i + 1]);
                    if (distance < spacing)
                    {
                        ctList.RemoveAt(i + 1);
                        ctCount = ctList.Count;
                        i--;
                    }
                }

                if (ctCount < 4) return;

                distance = Glm.Distance(ctList[0], ctList[ctList.Count - 1]);
                bool loop = distance < 5;
                ctList.CalculateRoundedCorner(mf.vehicle.minTurningRadius, loop, 0.0436332, CancellationToken.None);
            }

            mf.CalculateSteerAngle(ref ctList, !isSameWay);
        }

        //draw the red follow me line
        public void DrawContourLine()
        {
            GL.Color3(1.0, 1.0, 1.0);
            for (int i = 0; i < stripList.Count; i++)
            {
                GL.Begin(PrimitiveType.LineStrip);

                for (int j = 0; j < stripList[i].Count; j++)
                {
                    GL.Vertex3(stripList[i][j].Easting, stripList[i][j].Northing, 0);
                }
                GL.End();
            }

            ////draw the guidance line
            int ptCount = ctList.Count;
            if (ptCount < 2) return;
            GL.LineWidth(mf.ABLines.lineWidth);
            GL.Color3(0.98f, 0.2f, 0.980f);
            GL.Begin(PrimitiveType.LineStrip);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(ctList[h].Easting, ctList[h].Northing, 0);
            GL.End();

            //draw points
            GL.PointSize(mf.ABLines.lineWidth);
            GL.Color3(0.87f, 08.7f, 0.25f);
            GL.Begin(PrimitiveType.Points);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(ctList[h].Easting, ctList[h].Northing, 0);
            GL.End();

            if (mf.isPureDisplayOn && distanceFromCurrentLine != 32000 && !mf.isStanleyUsed)
            {
                //Draw lookahead Point
                GL.PointSize(6.0f);
                GL.Begin(PrimitiveType.Points);

                GL.Color3(1.0f, 0.95f, 0.095f);
                GL.Vertex3(goalPointCT.Easting, goalPointCT.Northing, 0.0);
                GL.End();
                GL.PointSize(1.0f);
            }
        }

        //Reset the contour to zip
        public void ResetContour()
        {
            stripList.Clear();
            ctList.Clear();
        }
    }//class
}//namespace