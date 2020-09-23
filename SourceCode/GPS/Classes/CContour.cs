using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CContour
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        public bool isContourOn, isContourBtnOn, isRightPriority = true;

        //used to determine if section was off and now is on or vice versa
        public bool wasSectionOn;

        //generated box for finding closest point
        public Vec2 boxA = new Vec2(0, 0), boxB = new Vec2(2, 0);
        public Vec2 boxC = new Vec2(1, 1), boxD = new Vec2(3, 2);
        public Vec2 boxE = new Vec2(4, 3), boxF = new Vec2(5, 4);

        //current contour patch and point closest to current fix
        public int closestRefPatch, closestRefPoint;

        //angle to path line closest point and fix
        public double refHeading, ref2;

        // for closest line point to current fix
        public double minDistance = 99999.0, refX, refZ;

        //generated reference line
        public double refLineSide = 1.0;

        public Vec2 refPoint1 = new Vec2(1, 1), refPoint2 = new Vec2(2, 2);

        public double distanceFromRefLine;
        public double distanceFromCurrentLine;

        private int A, B, C;
        public double abFixHeadingDelta, abHeading;

        public bool isABSameAsVehicleHeading = true;
        public bool isOnRightSideCurrentLine = true;

        //pure pursuit values
        public bool isValid;

        public Vec2 goalPointCT = new Vec2(0, 0);
        public Vec2 radiusPointCT = new Vec2(0, 0);
        public double steerAngleCT;
        public double rEastCT, rNorthCT;
        public double ppRadiusCT;

        //list of the list of individual Lines for entire field
        public List<List<Vec3>> stripList = new List<List<Vec3>>();

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
            }
        }

        public void IsInsideBox(Vec2 LeftDown, Vec2 RightDown, Vec2 RightUp, Vec2 LeftUp)
        {
            CVec pointC = new CVec();
            int stripCount = stripList.Count;
            for (int s = 0; s < stripCount; s++)
            {
                int ptCount = stripList[s].Count;
                for (int p = 0; p < ptCount; p++)
                {

                    if ((((RightDown.Easting - LeftDown.Easting) * (stripList[s][p].Northing - LeftDown.Northing))
                            - ((RightDown.Northing - LeftDown.Northing) * (stripList[s][p].Easting - LeftDown.Easting))) < 0) { continue; }

                    if ((((LeftUp.Easting - RightUp.Easting) * (stripList[s][p].Northing - RightUp.Northing))
                            - ((LeftUp.Northing - RightUp.Northing) * (stripList[s][p].Easting - RightUp.Easting))) < 0) { continue; }

                    if ((((RightUp.Easting - RightDown.Easting) * (stripList[s][p].Northing - RightDown.Northing))
                            - ((RightUp.Northing - RightDown.Northing) * (stripList[s][p].Easting - RightDown.Easting))) < 0) { continue; }

                    if ((((LeftDown.Easting - LeftUp.Easting) * (stripList[s][p].Northing - LeftUp.Northing))
                            - ((LeftDown.Northing - LeftUp.Northing) * (stripList[s][p].Easting - LeftUp.Easting))) < 0) { continue; }

                    //in the box so is it parallelish or perpedicularish to current heading
                    ref2 = Math.PI - Math.Abs(Math.Abs(mf.fixHeading - stripList[s][p].Heading) - Math.PI);
                    if (ref2 < 1.2 || ref2 > 1.9)
                    {
                        //it's in the box and parallelish so add to list
                        pointC.x = stripList[s][p].Easting;
                        pointC.z = stripList[s][p].Northing;
                        pointC.h = stripList[s][p].Heading;
                        pointC.strip = s;
                        pointC.pt = p;
                        conList.Add(pointC);
                    }
                }
            }
        }


        //determine closest point on left side
        public void BuildContourGuidanceLine(Vec3 pivot)
        {
            double toolWid = mf.Guidance.GuidanceWidth;
            double SinHeading = Math.Sin(pivot.Heading);
            double CosHeading = Math.Cos(pivot.Heading);



            //build a frustum box ahead of fix to find adjacent paths and points
            //Center
            boxA.Northing = pivot.Northing + CosHeading * -2;
            boxA.Easting = pivot.Easting + SinHeading * -2; //center down
            boxB.Northing = boxA.Northing + CosHeading * 10;
            boxB.Easting = boxA.Easting + SinHeading * 10; //center up

            //Left
            double tools = toolWid + mf.Guidance.GuidanceOffset + 10;
            boxC.Northing = boxB.Northing + SinHeading * -tools + CosHeading * 10;
            boxC.Easting = boxB.Easting + CosHeading * tools + SinHeading * 10; //Right up
            boxD.Northing = boxA.Northing + SinHeading * -tools + CosHeading * -10;
            boxD.Easting = boxA.Easting + CosHeading * tools + SinHeading * -10; // right down

            //Right
            tools = toolWid - mf.Guidance.GuidanceOffset + 10;
            boxE.Northing = boxB.Northing + SinHeading * tools + CosHeading * 10;
            boxE.Easting = boxB.Easting + CosHeading * -tools + SinHeading * 10; //left up
            boxF.Northing = boxA.Northing + SinHeading * tools + CosHeading * -10;
            boxF.Easting = boxA.Easting + CosHeading * -tools + SinHeading * -10;//left down

            conList.Clear();
            ctList.Clear();
            int ptCount;

            //check if no strips yet, return
            int stripCount = stripList.Count;
            if (stripCount == 0) return;

            if (isRightPriority)
            {
                IsInsideBox(boxA, boxD, boxC, boxB);

                if (conList.Count == 0)
                {
                    IsInsideBox(boxF, boxA, boxB, boxE);
                }
            }
            else
            {
                IsInsideBox(boxF, boxA, boxB, boxE);

                if (conList.Count == 0)
                {
                    IsInsideBox(boxA, boxD, boxC, boxB);
                }
            }

            //no points in the box, exit
            ptCount = conList.Count;
            if (ptCount == 0)
            {
                distanceFromCurrentLine = 32000;
                mf.guidanceLineDistanceOff = 32000;
                return;
            }

            //determine closest point
            minDistance = 99999;
            for (int i = 0; i < ptCount; i++)
            {
                double dist = ((pivot.Easting - conList[i].x) * (pivot.Easting - conList[i].x))
                                + ((pivot.Northing - conList[i].z) * (pivot.Northing - conList[i].z));
                if (dist < minDistance)
                {
                    minDistance = dist;
                    closestRefPoint = i;
                }
            }

            //now we have closest point, the distance squared from it, and which patch and point its from
            int strip = conList[closestRefPoint].strip;
            int pt = conList[closestRefPoint].pt;
            refX = stripList[strip][pt].Easting;
            refZ = stripList[strip][pt].Northing;
            refHeading = stripList[strip][pt].Heading;

            //which side of the patch are we on is next
            //calculate endpoints of reference line based on closest point
            refPoint1.Easting = refX - (Math.Sin(refHeading) * 50.0);
            refPoint1.Northing = refZ - (Math.Cos(refHeading) * 50.0);

            refPoint2.Easting = refX + (Math.Sin(refHeading) * 50.0);
            refPoint2.Northing = refZ + (Math.Cos(refHeading) * 50.0);

            //x2-x1
            double dx = refPoint2.Easting - refPoint1.Easting;
            //z2-z1
            double dz = refPoint2.Northing - refPoint1.Northing;

            //how far are we away from the reference line at 90 degrees - 2D cross product and distance
            distanceFromRefLine = ((dz * mf.pn.fix.Easting) - (dx * mf.pn.fix.Northing) + (refPoint2.Easting
                                    * refPoint1.Northing) - (refPoint2.Northing * refPoint1.Easting))
                                        / Math.Sqrt((dz * dz) + (dx * dx));

            bool isSameWay = Math.PI - Math.Abs(Math.Abs(mf.pivotAxlePos.Heading - refHeading) - Math.PI) < Glm.PIBy2;

            if (isSameWay) distanceFromRefLine += mf.Guidance.GuidanceOffset;
            else distanceFromRefLine -= mf.Guidance.GuidanceOffset;


            double HowManyPathsAway = Math.Round(distanceFromRefLine / mf.Guidance.WidthMinusOverlap, 0, MidpointRounding.AwayFromZero);

            //absolute the distance
            distanceFromRefLine = Math.Abs(distanceFromRefLine);

            //make the new guidance line list called guideList
            ptCount = stripList[strip].Count - 1;
            int start, stop;

            start = pt - 35; if (start < 0) start = 0;
            stop = pt + 35; if (stop > ptCount) stop = ptCount + 1;

            bool fail = false;

            double Offset = mf.Guidance.WidthMinusOverlap * HowManyPathsAway;
            double distSq = Offset * Offset - 10;



            for (int i = start; i < stop; i++)
            {
                var point = new Vec3(
                stripList[strip][i].Northing + (Math.Sin(stripList[strip][i].Heading) * -Offset),
                    stripList[strip][i].Easting + (Math.Cos(stripList[strip][i].Heading) * Offset),
                    stripList[strip][i].Heading);

                //make sure its not closer then 1 eq width
                for (int j = start; j < stop; j++)
                {
                    double check = Glm.DistanceSquared(point.Northing, point.Easting, stripList[strip][j].Northing, stripList[strip][j].Easting);
                    if (check < distSq)
                    {
                        fail = true;
                        break;
                    }
                }

                if (!fail) ctList.Add(point);
                fail = false;
            }

            int ctCount = ctList.Count;
            if (ctCount < 6) return;

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
            ctList.CalculateHeadings(true);
        }

        //determine distance from contour guidance line
        public void DistanceFromContourLine(Vec3 pivot, Vec3 steer)
        {
            isValid = false;
            int ptCount = ctList.Count;
            //distanceFromCurrentLine = 9999;
            if (ptCount > 8)
            {
                Vec3 point = mf.isStanleyUsed ? steer : pivot;

                double minDistA = double.PositiveInfinity;
                double minDistB = double.PositiveInfinity;

                //find the closest 2 points to current fix
                for (int t = 0; t < ptCount; t++)
                {
                    double dist = ((point.Easting - ctList[t].Easting) * (point.Easting - ctList[t].Easting))
                                    + ((point.Northing - ctList[t].Northing) * (point.Northing - ctList[t].Northing));
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

                //just need to make sure the points continue ascending or heading switches all over the place
                if (A > B) { C = A; A = B; B = C; }

                //get the distance from currently active AB line
                //x2-x1
                double dx = ctList[B].Easting - ctList[A].Easting;
                //z2-z1
                double dy = ctList[B].Northing - ctList[A].Northing;

                if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dy) < Double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLine = ((dy * point.Easting) - (dx * point.Northing) + (ctList[B].Easting
                            * ctList[A].Northing) - (ctList[B].Northing * ctList[A].Easting))
                                / Math.Sqrt((dy * dy) + (dx * dx));

                //are we on the right side or not
                isOnRightSideCurrentLine = distanceFromCurrentLine > 0;


                // calc point on ABLine closest to current position
                double U = (((point.Easting - ctList[A].Easting) * dx) + ((point.Northing - ctList[A].Northing) * dy))
                            / ((dx * dx) + (dy * dy));

                rEastCT = ctList[A].Easting + (U * dx);
                rNorthCT = ctList[A].Northing + (U * dy);


                if (mf.isStanleyUsed)
                {
                    abHeading = Math.Atan2(dx, dy);
                    if (abHeading < 0) abHeading += Glm.twoPI;
                    //if (abHeading > Math.PI) abHeading -= glm.twoPI;

                    //Subtract the two headings, if > 1.57 its going the opposite heading as refAB
                    abFixHeadingDelta = (Math.Abs(mf.fixHeading - abHeading));
                    if (abFixHeadingDelta >= Math.PI) abFixHeadingDelta = Math.Abs(abFixHeadingDelta - Glm.twoPI);

                    isABSameAsVehicleHeading = abFixHeadingDelta < Glm.PIBy2;

                    //distance is negative if on left, positive if on right
                    abFixHeadingDelta = (steer.Heading - abHeading);
                    if (!isABSameAsVehicleHeading)
                    {
                        distanceFromCurrentLine *= -1.0;
                        abFixHeadingDelta += Math.PI;
                    }

                    //Fix the circular error
                    if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < -Math.PI) abFixHeadingDelta += Math.PI;

                    if (abFixHeadingDelta > Glm.PIBy2) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < -Glm.PIBy2) abFixHeadingDelta += Math.PI;

                    abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
                    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
                    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

                    steerAngleCT = Math.Atan((distanceFromCurrentLine * mf.vehicle.stanleyGain) 
                        / ((Math.Abs(mf.pn.speed) * 0.277777) + 1));

                    if (steerAngleCT > 0.74) steerAngleCT = 0.74;
                    if (steerAngleCT < -0.74) steerAngleCT = -0.74;

                    if (mf.pn.speed > -0.1)
                        steerAngleCT = Glm.ToDegrees((steerAngleCT + abFixHeadingDelta) * -1.0);
                    else
                        steerAngleCT = Glm.ToDegrees((steerAngleCT - abFixHeadingDelta) * -1.0);


                    if (steerAngleCT < -mf.vehicle.maxSteerAngle) steerAngleCT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCT > mf.vehicle.maxSteerAngle) steerAngleCT = mf.vehicle.maxSteerAngle;

                    //Convert to millimeters
                    distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    //abHeading = Math.Atan2(dz, dx);
                    abHeading = ctList[A].Heading;

                    //Subtract the two headings, if > 1.57 its going the opposite heading as refAB
                    abFixHeadingDelta = (Math.Abs(mf.fixHeading - abHeading));
                    if (abFixHeadingDelta >= Math.PI) abFixHeadingDelta = Math.Abs(abFixHeadingDelta - Glm.twoPI);

                    //used for accumulating distance to find goal point
                    double distSoFar;

                    //update base on autosteer settings and distance from line
                    double goalPointDistance = mf.vehicle.UpdateGoalPointDistance(distanceFromCurrentLine);
                    mf.lookaheadActual = goalPointDistance;

                    // used for calculating the length squared of next segment.
                    double tempDist = 0.0;

                    if (abFixHeadingDelta >= Glm.PIBy2)
                    {
                        //counting down
                        isABSameAsVehicleHeading = false;
                        distSoFar = Glm.Distance(ctList[A], rEastCT, rNorthCT);
                        //Is this segment long enough to contain the full lookahead distance?
                        if (distSoFar > goalPointDistance)
                        {
                            //treat current segment like an AB Line
                            goalPointCT.Easting = rEastCT - (Math.Sin(ctList[A].Heading) * goalPointDistance);
                            goalPointCT.Northing = rNorthCT - (Math.Cos(ctList[A].Heading) * goalPointDistance);
                        }

                        //multiple segments required
                        else
                        {
                            //cycle thru segments and keep adding lengths. check if start and break if so.
                            while (A > 0)
                            {
                                B--; A--;
                                tempDist = Glm.Distance(ctList[B], ctList[A]);

                                //will we go too far?
                                if ((tempDist + distSoFar) > goalPointDistance)
                                {
                                    //A++; B++;
                                    break; //tempDist contains the full length of next segment
                                }
                                else
                                {
                                    distSoFar += tempDist;
                                }
                            }

                            double t = (goalPointDistance - distSoFar); // the remainder to yet travel
                            t /= tempDist;

                            goalPointCT.Easting = (((1 - t) * ctList[B].Easting) + (t * ctList[A].Easting));
                            goalPointCT.Northing = (((1 - t) * ctList[B].Northing) + (t * ctList[A].Northing));
                        }
                    }
                    else
                    {
                        //counting up
                        isABSameAsVehicleHeading = true;
                        distSoFar = Glm.Distance(ctList[B], rEastCT, rNorthCT);

                        //Is this segment long enough to contain the full lookahead distance?
                        if (distSoFar > goalPointDistance)
                        {
                            //treat current segment like an AB Line
                            goalPointCT.Easting = rEastCT + (Math.Sin(ctList[A].Heading) * goalPointDistance);
                            goalPointCT.Northing = rNorthCT + (Math.Cos(ctList[A].Heading) * goalPointDistance);
                        }

                        //multiple segments required
                        else
                        {
                            //cycle thru segments and keep adding lengths. check if end and break if so.
                            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                            while (B < ptCount - 1)
                            {
                                B++; A++;
                                tempDist = Glm.Distance(ctList[B], ctList[A]);

                                //will we go too far?
                                if ((tempDist + distSoFar) > goalPointDistance)
                                {
                                    //A--; B--;
                                    break; //tempDist contains the full length of next segment
                                }

                                distSoFar += tempDist;
                            }

                            //xt = (((1 - t) * x0 + t * x1)
                            //yt = ((1 - t) * y0 + t * y1))

                            double t = (goalPointDistance - distSoFar); // the remainder to yet travel
                            t /= tempDist;

                            goalPointCT.Easting = (((1 - t) * ctList[A].Easting) + (t * ctList[B].Easting));
                            goalPointCT.Northing = (((1 - t) * ctList[A].Northing) + (t * ctList[B].Northing));
                        }
                    }

                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = Glm.DistanceSquared(goalPointCT.Northing, goalPointCT.Easting, pivot.Northing, pivot.Easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading = Glm.twoPI - mf.fixHeading;
                    ppRadiusCT = goalPointDistanceSquared / (2 * (((goalPointCT.Easting - pivot.Easting) * Math.Cos(localHeading)) + ((goalPointCT.Northing - pivot.Northing) * Math.Sin(localHeading))));

                    steerAngleCT = Glm.ToDegrees(Math.Atan(2 * (((goalPointCT.Easting - pivot.Easting) * Math.Cos(localHeading))
                        + ((goalPointCT.Northing - pivot.Northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

                    if (steerAngleCT < -mf.vehicle.maxSteerAngle) steerAngleCT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCT > mf.vehicle.maxSteerAngle) steerAngleCT = mf.vehicle.maxSteerAngle;

                    if (ppRadiusCT < -500) ppRadiusCT = -500;
                    if (ppRadiusCT > 500) ppRadiusCT = 500;

                    radiusPointCT.Easting = pivot.Easting + (ppRadiusCT * Math.Cos(localHeading));
                    radiusPointCT.Northing = pivot.Northing + (ppRadiusCT * Math.Sin(localHeading));

                    //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
                    double angVel = Glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(Glm.ToRadians(steerAngleCT))) / mf.vehicle.wheelbase;

                    //clamp the steering angle to not exceed safe angular velocity
                    if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
                    {
                        steerAngleCT = Glm.ToDegrees(steerAngleCT > 0 ?
                                (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (Glm.twoPI * mf.pn.speed * 0.277777)))
                            : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (Glm.twoPI * mf.pn.speed * 0.277777))));
                    }
                    //Convert to centimeters
                    distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);

                    //distance is negative if on left, positive if on right
                    //if you're going the opposite direction left is right and right is left
                    //double temp;
                    if (!isABSameAsVehicleHeading) distanceFromCurrentLine *= -1.0;
                }

                //fill in the autosteer variables
                mf.guidanceLineDistanceOff = mf.distanceFromCurrentLine = mf.distanceDisplay = (Int16)distanceFromCurrentLine;
                mf.guidanceLineSteerAngle = (Int16)(steerAngleCT * 100);
            }
            else
            {
                //invalid distance so tell AS module
                distanceFromCurrentLine = 32000;
                mf.guidanceLineDistanceOff = 32000;
            }
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

            /*
            GL.Begin(PrimitiveType.LineStrip);
            GL.Vertex3(boxA.Easting, boxA.Northing, 0);
            GL.Vertex3(boxB.Easting, boxB.Northing, 0);
            GL.Vertex3(boxC.Easting, boxC.Northing, 0);
            GL.Vertex3(boxD.Easting, boxD.Northing, 0);
            GL.Vertex3(boxA.Easting, boxA.Northing, 0);
            GL.Vertex3(boxF.Easting, boxF.Northing, 0);
            GL.Vertex3(boxE.Easting, boxE.Northing, 0);
            GL.Vertex3(boxB.Easting, boxB.Northing, 0);
            GL.End();
            */

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