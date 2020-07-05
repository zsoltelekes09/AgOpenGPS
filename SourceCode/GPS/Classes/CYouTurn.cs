using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace AgOpenGPS
{
    public class CYouTurn
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        /// <summary>/// triggered right after youTurnTriggerPoint is set /// </summary>
        public bool isYouTurnTriggered;

        /// <summary>  /// turning right or left?/// </summary>
        public bool isYouTurnRight, isLastToggle;

        /// <summary> /// What was the last successful you turn direction? /// </summary>
        public bool isLastYouTurnRight;

        //public bool isEnteringDriveThru = false, isExitingDriveThru = false;

        //if not in workArea but in bounds, then we are on headland
        public bool isInWorkArea, isInBoundz;

        //controlled by user in GUI to en/dis able
        public bool isRecordingCustomYouTurn;

        /// <summary> /// Is the youturn button enabled? /// </summary>
        public bool isYouTurnBtnOn;

        //Patterns or Dubins
        public bool isUsingDubinsTurn;

        public double boundaryAngleOffPerpendicular;
        public double distanceTurnBeforeLine = 0, tangencyAngle;

        public int rowSkipsWidth = 1;

        /// <summary>  /// distance from headland as offset where to start turn shape /// </summary>
        public int youTurnStartOffset;

        //guidance values
        public double distanceFromCurrentLine, triggerDistanceOffset, geoFenceDistance, dxAB, dyAB;

        private int A, B, C;
        private bool isABSameAsFixHeading = true, isOnRightSideCurrentLine = true;
        public bool isTurnCreationTooClose = false, isTurnCreationNotCrossingError = false;

        //pure pursuit values
        public Vec3 pivot = new Vec3(0, 0, 0);

        public Vec2 goalPointYT = new Vec2(0, 0);
        public Vec2 radiusPointYT = new Vec2(0, 0);
        public double steerAngleYT, rEastYT, rNorthYT, ppRadiusYT;
        private int numShapePoints;

        //list of points for scaled and rotated YouTurn line, used for pattern, dubins, abcurve, abline
        public List<Vec3> ytList = new List<Vec3>();

        //list of points read from file, this is the actual pattern from a bunch of sources possible
        public List<Vec2> youFileList = new List<Vec2>();

        //to try and pull a UTurn back in bounds
        public double turnDistanceAdjuster;

        //is UTurn pattern in or out of bounds
        public bool isOutOfBounds = false;

        //sequence of operations of finding the next turn 0 to 3
        public int youTurnPhase, curListCount;

        public Vec4 crossingCurvePoint = new Vec4();
        public Vec4 crossingTurnLinePoint = new Vec4();

        //constructor
        public CYouTurn(FormGPS _f)
        {
            mf = _f;

            triggerDistanceOffset = Properties.Vehicle.Default.set_youTriggerDistance;
            geoFenceDistance = Properties.Vehicle.Default.set_geoFenceDistance;

            //how far before or after boundary line should turn happen
            youTurnStartOffset = Properties.Vehicle.Default.set_youTurnDistance;

            //the youturn shape scaling.
            //rowSkipsHeight = Properties.Vehicle.Default.set_youSkipHeight;
            rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;

            isUsingDubinsTurn = Properties.Vehicle.Default.set_youUseDubins;
        }

        //Finds the point where an AB Curve crosses the turn line
        public bool FindCurveTurnPoints()
        {
            crossingCurvePoint.easting = -20000;
            crossingTurnLinePoint.easting = -20000;

            //find closet AB Curve point that will cross and go out of bounds
            curListCount = mf.curve.curList.Count;

            //otherwise we count down
            bool isCountingUp = mf.curve.isABSameAsVehicleHeading;

            bool abc = true;
            //check if outside a border
            if (isCountingUp)
            {
                crossingTurnLinePoint.index = 99;

                for (int j = mf.curve.currentLocationIndex; j < mf.curve.currentLocationIndex || abc; j++)
                {
                    if (!mf.turn.turnArr[0].IsPointInTurnWorkArea(mf.curve.curList[j]))
                    {
                        if (j == curListCount)
                        {
                            j = 0;
                            abc = false;
                        }
                        //it passed outer turnLine
                        crossingCurvePoint.easting = mf.curve.curList[j].easting;
                        crossingCurvePoint.northing = mf.curve.curList[j].northing;
                        crossingCurvePoint.heading = mf.curve.curList[j].heading;
                        crossingCurvePoint.index = j;
                        crossingTurnLinePoint.index = 0;
                        goto CrossingFound;
                    }

                    for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                    {
                        //make sure not inside a non drivethru boundary
                        if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                        if (mf.turn.turnArr[i].IsPointInTurnWorkArea(mf.curve.curList[j]))
                        {
                            crossingCurvePoint.easting = mf.curve.curList[j - 1].easting;
                            crossingCurvePoint.northing = mf.curve.curList[j - 1].northing;
                            crossingCurvePoint.heading = mf.curve.curList[j - 1].heading;
                            crossingCurvePoint.index = j - 1;
                            crossingTurnLinePoint.index = i;
                            goto CrossingFound;
                        }
                    }
                }

                //escape for multiple for's
                CrossingFound:;
            }
            else //counting down, going opposite way mf.curve was created.
            {
                crossingTurnLinePoint.index = 99;

                for (int j = mf.curve.currentLocationIndex; j > mf.curve.currentLocationIndex || abc; j--)
                {
                    if (j == -1)
                    {
                        j = curListCount - 1;
                        abc = false;
                    }
                    if (!mf.turn.turnArr[0].IsPointInTurnWorkArea(mf.curve.curList[j]))
                    {                                        //it passed outer turnLine
                        crossingCurvePoint.easting = mf.curve.curList[j + 1].easting;
                        crossingCurvePoint.northing = mf.curve.curList[j + 1].northing;
                        crossingCurvePoint.heading = mf.curve.curList[j + 1].heading;
                        crossingCurvePoint.index = j + 1;
                        crossingTurnLinePoint.index = 0;
                        goto CrossingFound;
                    }

                    for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                    {
                        //make sure not inside a non drivethru boundary
                        if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                        if (mf.turn.turnArr[i].IsPointInTurnWorkArea(mf.curve.curList[j]))
                        {
                            crossingCurvePoint.easting = mf.curve.curList[j].easting;
                            crossingCurvePoint.northing = mf.curve.curList[j].northing;
                            crossingCurvePoint.heading = mf.curve.curList[j].heading;
                            crossingCurvePoint.index = j;
                            crossingTurnLinePoint.index = i;
                            goto CrossingFound;
                        }
                    }
                }

                //escape for multiple for's, point and turnLine index are found
                CrossingFound:;
            }

            int turnNum = crossingTurnLinePoint.index;
            int turnNum2 = crossingTurnLinePoint.index;

            if (turnNum == 99)
            {
                isTurnCreationNotCrossingError = true;
                return false;
            }

            int curTurnLineCount = mf.turn.turnArr[turnNum].turnLine.Count;

            //possible points close to AB Curve point
            List<int> turnLineCloseList = new List<int>();

            for (int j = 0; j < curTurnLineCount; j++)
            {
                if ((mf.turn.turnArr[turnNum].turnLine[j].easting - crossingCurvePoint.easting) < 2
                    && (mf.turn.turnArr[turnNum].turnLine[j].easting - crossingCurvePoint.easting) > -2
                    && (mf.turn.turnArr[turnNum].turnLine[j].northing - crossingCurvePoint.northing) < 2
                    && (mf.turn.turnArr[turnNum].turnLine[j].northing - crossingCurvePoint.northing) > -2)
                {
                    turnLineCloseList.Add(j);
                }
            }

            double dist1, dist2 = 99;
            curTurnLineCount = turnLineCloseList.Count;
            for (int i = 0; i < curTurnLineCount; i++)
            {
                dist1 = Glm.Distance(mf.turn.turnArr[turnNum].turnLine[turnLineCloseList[i]].easting,
                                        mf.turn.turnArr[turnNum].turnLine[turnLineCloseList[i]].northing,
                                            crossingCurvePoint.easting, crossingCurvePoint.northing);
                if (dist1 < dist2)
                {
                    turnNum2 = turnLineCloseList[i];
                    dist2 = dist1;
                }
            }

            //fill up the coords
            crossingTurnLinePoint.easting = mf.turn.turnArr[turnNum].turnLine[turnNum2].easting;
            crossingTurnLinePoint.northing = mf.turn.turnArr[turnNum].turnLine[turnNum2].northing;
            crossingTurnLinePoint.heading = mf.turn.turnArr[turnNum].turnLine[turnNum2].heading;

            return crossingCurvePoint.easting != -20000 && crossingCurvePoint.easting != -20000;
        }

        public void AddSequenceLines(double head)
        {
            Vec3 pt;
            for (int a = 0; a < youTurnStartOffset*2; a++)
            {
                pt.easting = ytList[0].easting + (Math.Sin(head)*0.5);
                pt.northing = ytList[0].northing + (Math.Cos(head) * 0.5);
                pt.heading = ytList[0].heading;
                ytList.Insert(0, pt);
            }

            int count = ytList.Count;

            for (int i = 1; i <= youTurnStartOffset*2; i++)
            {
                pt.easting = ytList[count - 1].easting + (Math.Sin(head) * i * 0.5);
                pt.northing = ytList[count - 1].northing + (Math.Cos(head) * i * 0.5);
                pt.heading = head;
                ytList.Add(pt);
            }

            double distancePivotToTurnLine;
            count = ytList.Count;
            for (int i = 0; i < count; i += 2)
            {
                distancePivotToTurnLine = Glm.Distance(ytList[i], mf.pivotAxlePos);
                if (distancePivotToTurnLine > 3)
                {
                    isTurnCreationTooClose = false;
                }
                else
                {
                    isTurnCreationTooClose = true;
                    //set the flag to Critical stop machine
                    if (isTurnCreationTooClose) mf.mc.isOutOfBounds = true;
                    break;
                }
            }
        }

        //list of points of collision path avoidance
        public List<Vec3> mazeList = new List<Vec3>();

        public bool BuildDriveAround()
        {
            double headAB = mf.ABLine.abHeading;
            if (!mf.ABLine.isABSameAsVehicleHeading) headAB += Math.PI;

            double cosHead = Math.Cos(headAB);
            double sinHead = Math.Sin(headAB);

            Vec3 start = new Vec3();
            Vec3 stop = new Vec3();
            Vec3 pt2 = new Vec3();

            //grab the pure pursuit point right on ABLine
            Vec3 onPurePoint = new Vec3(mf.ABLine.rEastAB, mf.ABLine.rNorthAB, 0);

            //how far are we from any geoFence
            mf.gf.FindPointsDriveAround(onPurePoint, headAB, ref start, ref stop);

            //not an inside border
            if (start.easting == 88888) return false;

            //get the dubins path vec3 point coordinates of path
            ytList?.Clear();

            //find a path from start to goal - diagnostic, but also used later
            mazeList = mf.mazeGrid.SearchForPath(start, stop);

            //you can't get anywhere!
            if (mazeList == null) return false;

            //not really changing direction so need to fake a turn twice.
            mf.SwapDirection();

            //Dubins at the start and stop of mazePath
            CDubins.turningRadius = mf.vehicle.minTurningRadius * 1.0;
            CDubins dubPath = new CDubins();

            //start is navigateable - maybe
            int cnt = mazeList.Count;
            int cut = 8;
            if (cnt < 18) cut = 3;

            if (cnt > 0)
            {
                pt2.easting = start.easting - (sinHead * mf.vehicle.minTurningRadius * 1.5);
                pt2.northing = start.northing - (cosHead * mf.vehicle.minTurningRadius * 1.5);
                pt2.heading = headAB;

                List<Vec3> shortestDubinsList = dubPath.GenerateDubins(pt2, mazeList[cut - 1], mf.gf);
                for (int i = 1; i < shortestDubinsList.Count; i++)
                {
                    Vec3 pt = new Vec3(shortestDubinsList[i].easting, shortestDubinsList[i].northing, shortestDubinsList[i].heading);
                    ytList.Add(pt);
                }

                for (int i = cut; i < mazeList.Count - cut; i++)
                {
                    Vec3 pt = new Vec3(mazeList[i].easting, mazeList[i].northing, mazeList[i].heading);
                    ytList.Add(pt);
                }

                pt2.easting = stop.easting + (sinHead * mf.vehicle.minTurningRadius * 1.5);
                pt2.northing = stop.northing + (cosHead * mf.vehicle.minTurningRadius * 1.5);
                pt2.heading = headAB;

                shortestDubinsList = dubPath.GenerateDubins(mazeList[cnt - cut], pt2, mf.gf);

                for (int i = 1; i < shortestDubinsList.Count; i++)
                {
                    Vec3 pt = new Vec3(shortestDubinsList[i].easting, shortestDubinsList[i].northing, shortestDubinsList[i].heading);
                    ytList.Add(pt);
                }
            }

            if (ytList.Count > 10) youTurnPhase = 3;

            Vec3 pt3 = new Vec3();

            for (int a = 0; a < youTurnStartOffset; a++)
            {
                pt3.easting = ytList[0].easting - sinHead;
                pt3.northing = ytList[0].northing - cosHead;
                pt3.heading = headAB;
                ytList.Insert(0, pt3);
            }

            int count = ytList.Count;

            for (int i = 1; i <= youTurnStartOffset; i++)
            {
                pt3.easting = ytList[count - 1].easting + (sinHead * i);
                pt3.northing = ytList[count - 1].northing + (cosHead * i);
                pt3.heading = headAB;
                ytList.Add(pt3);
            }

            return true;
        }

        public bool BuildABLineDubinsYouTurn(bool isTurnRight)
        {
            double head = mf.ABLine.abHeading;
            if (!mf.ABLine.isABSameAsVehicleHeading) head += Math.PI;

            if (youTurnPhase == 0)
            {
                if (BuildDriveAround()) return true;

                //grab the pure pursuit point right on ABLine
                Vec3 onPurePoint = new Vec3(mf.ABLine.rEastAB, mf.ABLine.rNorthAB, 0);

                //how far are we from any turn boundary
                mf.turn.FindClosestTurnPoint(isYouTurnRight, onPurePoint, head);

                //or did we lose the turnLine - we are on the highway cuz we left the outer/inner turn boundary
                if ((int)mf.turn.closestTurnPt.easting != -20000)
                {
                    mf.distancePivotToTurnLine = Glm.Distance(mf.pivotAxlePos, mf.turn.closestTurnPt);
                }
                else
                {
                    //Full emergency stop code goes here, it thinks its auto turn, but its not!
                    mf.distancePivotToTurnLine = -3333;
                }


                boundaryAngleOffPerpendicular = Glm.PIBy2 + (mf.turn.closestTurnPt.heading - head);

                if (boundaryAngleOffPerpendicular > Math.PI) boundaryAngleOffPerpendicular -= Glm.twoPI;
                if (boundaryAngleOffPerpendicular < -Math.PI) boundaryAngleOffPerpendicular += Glm.twoPI;

                CDubins dubYouTurnPath = new CDubins();
                CDubins.turningRadius = mf.vehicle.minTurningRadius;

                //grab the vehicle widths and offsets
                double toolOffset = mf.Tools[0].ToolOffset * 2.0;
                double turnOffset;

                //calculate the true width
                if (!isTurnRight) turnOffset = ((mf.Tools[0].WidthMinusOverlap * rowSkipsWidth) + toolOffset);
                else turnOffset = ((mf.Tools[0].WidthMinusOverlap * rowSkipsWidth) - toolOffset);


                double turnRadius = turnOffset * Math.Tan(boundaryAngleOffPerpendicular);

                //start point of Dubins
                rEastYT = mf.ABLine.rEastAB + (Math.Sin(head) * mf.distancePivotToTurnLine);
                rNorthYT = mf.ABLine.rNorthAB + (Math.Cos(head) * mf.distancePivotToTurnLine);
                var start = new Vec3(rEastYT, rNorthYT, head);

                var goal = new Vec3();

                if (!isTurnRight)//this is actualy right
                {
                    goal.northing = (rNorthYT - (Math.Cos(head - Glm.PIBy2) * turnOffset)) - Math.Cos(head) * turnRadius;
                    goal.easting = (rEastYT - (Math.Sin(head - Glm.PIBy2) * turnOffset)) - Math.Sin(head) * turnRadius;
                    goal.heading = head - Math.PI;
                }
                else
                {
                    goal.northing = (rNorthYT - (Math.Cos(head + Glm.PIBy2) * turnOffset)) + Math.Cos(head) * turnRadius;
                    goal.easting = (rEastYT - (Math.Sin(head + Glm.PIBy2) * turnOffset)) + Math.Sin(head) * turnRadius;
                    goal.heading = head + Math.PI;
                }

                //now we go the other way to turn round
                head -= Math.PI;
                if (head < 0) head += Glm.twoPI;

                //generate the turn points
                ytList = dubYouTurnPath.GenerateDubins(start, goal);
                int count = ytList.Count;
                if (count == 0) return false;
                else youTurnPhase = 1;

                AddSequenceLines(head);

                if (ytList.Count == 0) return false;
                else youTurnPhase = 1;
            }

            if (youTurnPhase == 3) return true;

            // Phase 0 - back up the turn till it is out of bounds.
            // Phase 1 - move it forward till out of bounds.
            // Phase 2 - move forward couple meters away from turn line.
            // Phase 3 - ytList is made, waiting to get close enough to it

            isOutOfBounds = false;
            switch (youTurnPhase)
            {
                case 1:
                    //the temp array
                    mf.distancePivotToTurnLine = Glm.Distance(ytList[0], mf.pivotAxlePos);
                    double cosHead = Math.Cos(head);
                    double sinHead = Math.Sin(head);

                    int cnt = ytList.Count;
                    Vec3[] arr2 = new Vec3[cnt];

                    ytList.CopyTo(arr2);
                    ytList.Clear();

                    for (int i = 0; i < cnt; i++)
                    {
                        arr2[i].easting -= (sinHead);
                        arr2[i].northing -= (cosHead);
                        ytList.Add(arr2[i]);
                    }

                    for (int j = 0; j < cnt; j += 2)
                    {
                        if (!mf.turn.turnArr[0].IsPointInTurnWorkArea(ytList[j])) isOutOfBounds = true;
                        if (isOutOfBounds) break;

                        for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                        {
                            //make sure not inside a non drivethru boundary
                            if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                            if (mf.turn.turnArr[i].IsPointInTurnWorkArea(ytList[j]))
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                        if (isOutOfBounds) break;
                    }

                    if (!isOutOfBounds)
                    {
                        youTurnPhase = 3;
                    }
                    else
                    {
                        //turn keeps approaching vehicle and running out of space - end of field?
                        if (isOutOfBounds && mf.distancePivotToTurnLine > 3)
                        {
                            isTurnCreationTooClose = false;
                        }
                        else
                        {
                            isTurnCreationTooClose = true;

                            //set the flag to Critical stop machine
                            mf.mc.isOutOfBounds = true;
                        }
                    }
                    break;
            }
            return true;
        }

        public bool BuildABLinePatternYouTurn(bool isTurnRight)
        {
            double headAB = mf.ABLine.abHeading;
            if (!mf.ABLine.isABSameAsVehicleHeading) headAB += Math.PI;

            //grab the pure pursuit point right on ABLine
            Vec3 onPurePoint = new Vec3(mf.ABLine.rEastAB, mf.ABLine.rNorthAB, 0);

            //how far are we from any turn boundary
            mf.turn.FindClosestTurnPoint(isYouTurnRight, onPurePoint, headAB);

            //or did we lose the turnLine - we are on the highway cuz we left the outer/inner turn boundary
            if ((int)mf.turn.closestTurnPt.easting != -20000)
            {
                mf.distancePivotToTurnLine = Glm.Distance(mf.pivotAxlePos, mf.turn.closestTurnPt);
            }
            else
            {
                //Full emergency stop code goes here, it thinks its auto turn, but its not!
                mf.distancePivotToTurnLine = -3333;
            }

            distanceTurnBeforeLine = turnDistanceAdjuster;

            ytList.Clear();

            //point on AB line closest to pivot axle point from ABLine PurePursuit
            rEastYT = mf.ABLine.rEastAB;
            rNorthYT = mf.ABLine.rNorthAB;
            isABSameAsFixHeading = mf.ABLine.isABSameAsVehicleHeading;
            double head = mf.ABLine.abHeading;

            //grab the vehicle widths and offsets
            double toolOffset = mf.Tools[0].ToolOffset * 2.0;
            double turnOffset;

            //turning right
            if (isTurnRight) turnOffset = (mf.Tools[0].WidthMinusOverlap - toolOffset);
            else turnOffset = (mf.Tools[0].WidthMinusOverlap + toolOffset);

            //Pattern Turn
            numShapePoints = youFileList.Count;
            Vec3[] pt = new Vec3[numShapePoints];

            //Now put the shape into an array since lists are immutable
            for (int i = 0; i < numShapePoints; i++)
            {
                pt[i].easting = youFileList[i].easting;
                pt[i].northing = youFileList[i].northing;
            }

            //start of path on the origin. Mirror the shape if left turn
            if (isTurnRight)
            {
                for (int i = 0; i < pt.Length; i++) pt[i].easting *= -1;
            }

            //scaling - Drawing is 10m wide so find ratio of tool width
            double scale = turnOffset * 0.1;
            for (int i = 0; i < pt.Length; i++)
            {
                pt[i].easting *= scale * rowSkipsWidth;
                pt[i].northing *= scale * rowSkipsWidth;
            }

            if (!isABSameAsFixHeading) head += Math.PI;

            double _turnDiagDistance = mf.distancePivotToTurnLine - distanceTurnBeforeLine;

            //move the start forward
            if (youTurnPhase < 2)
            {
                rEastYT += (Math.Sin(head) * (_turnDiagDistance - turnOffset));
                rNorthYT += (Math.Cos(head) * (_turnDiagDistance - turnOffset));
            }
            else
            {
                _turnDiagDistance -= 2;
                turnDistanceAdjuster += 5;
                rEastYT += (Math.Sin(head) * (_turnDiagDistance - turnOffset));
                rNorthYT += (Math.Cos(head) * (_turnDiagDistance - turnOffset));
                youTurnPhase = 3;
            }

            //rotate pattern to match AB Line heading
            double xr, yr, xr2, yr2;
            for (int i = 0; i < pt.Length - 1; i++)
            {
                xr = (Math.Cos(-head) * pt[i].easting) - (Math.Sin(-head) * pt[i].northing) + rEastYT;
                yr = (Math.Sin(-head) * pt[i].easting) + (Math.Cos(-head) * pt[i].northing) + rNorthYT;

                xr2 = (Math.Cos(-head) * pt[i + 1].easting) - (Math.Sin(-head) * pt[i + 1].northing) + rEastYT;
                yr2 = (Math.Sin(-head) * pt[i + 1].easting) + (Math.Cos(-head) * pt[i + 1].northing) + rNorthYT;

                pt[i].easting = xr;
                pt[i].northing = yr;
                pt[i].heading = Math.Atan2(xr2 - xr, yr2 - yr);
                if (pt[i].heading < 0) pt[i].heading += Glm.twoPI;
                ytList.Add(pt[i]);
            }
            xr = (Math.Cos(-head) * pt[pt.Length - 1].easting) - (Math.Sin(-head) * pt[pt.Length - 1].northing) + rEastYT;
            yr = (Math.Sin(-head) * pt[pt.Length - 1].easting) + (Math.Cos(-head) * pt[pt.Length - 1].northing) + rNorthYT;

            pt[pt.Length - 1].easting = xr;
            pt[pt.Length - 1].northing = yr;
            pt[pt.Length - 1].heading = pt[pt.Length - 2].heading;
            ytList.Add(pt[pt.Length - 1]);

            //pattern all made now is it outside a boundary
            //now check to make sure we are not in an inner turn boundary - drive thru is ok
            int count = ytList.Count;
            if (count == 0) return false;
            isOutOfBounds = false;

            head += Math.PI;

            Vec3 ptt;
            for (int a = 0; a < youTurnStartOffset; a++)
            {
                ptt.easting = ytList[0].easting + (Math.Sin(head));
                ptt.northing = ytList[0].northing + (Math.Cos(head));
                ptt.heading = ytList[0].heading;
                ytList.Insert(0, ptt);
            }

            count = ytList.Count;

            for (int i = 1; i <= youTurnStartOffset; i++)
            {
                ptt.easting = ytList[count - 1].easting + (Math.Sin(head) * i);
                ptt.northing = ytList[count - 1].northing + (Math.Cos(head) * i);
                ptt.heading = ytList[count - 1].heading;
                ytList.Add(ptt);
            }

            double distancePivotToTurnLine;
            count = ytList.Count;
            for (int i = 0; i < count; i += 2)
            {
                distancePivotToTurnLine = Glm.Distance(ytList[i], mf.pivotAxlePos);
                if (distancePivotToTurnLine > 3)
                {
                    isTurnCreationTooClose = false;
                }
                else
                {
                    isTurnCreationTooClose = true;
                    //set the flag to Critical stop machine
                    if (isTurnCreationTooClose) mf.mc.isOutOfBounds = true;
                    break;
                }
            }

            // Phase 0 - back up the turn till it is out of bounds.
            // Phase 1 - move it forward till out of bounds.
            // Phase 2 - move forward couple meters away from turn line.

            switch (youTurnPhase)
            {
                case 0:
                    //if (turnDiagnosticAdjuster == 0) turnDiagnosticAdjuster = turnRadius;
                    turnDistanceAdjuster -= 2;
                    for (int j = 0; j < count; j += 2)
                    {
                        if (!mf.turn.turnArr[0].IsPointInTurnWorkArea(ytList[j])) isOutOfBounds = true;
                        if (isOutOfBounds) break;

                        for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                        {
                            //make sure not inside a non drivethru boundary
                            if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                            if (mf.turn.turnArr[i].IsPointInTurnWorkArea(ytList[j]))
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                        if (isOutOfBounds) break;
                    }

                    if (isOutOfBounds) youTurnPhase = 1;
                    break;

                case 1:
                    for (int j = 0; j < count; j += 2)
                    {
                        if (!mf.turn.turnArr[0].IsPointInTurnWorkArea(ytList[j])) isOutOfBounds = true;
                        if (isOutOfBounds) break;

                        for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                        {
                            //make sure not inside a non drivethru boundary
                            if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                            if (mf.turn.turnArr[i].IsPointInTurnWorkArea(ytList[j]))
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                        if (isOutOfBounds) break;
                    }

                    if (!isOutOfBounds)
                    {
                        youTurnPhase = 2;
                    }
                    else
                    {
                        //turn keeps approaching vehicle and running out of space - end of field?
                        if (isOutOfBounds && _turnDiagDistance > 3)
                        {
                            turnDistanceAdjuster += 2;
                            isTurnCreationTooClose = false;
                        }
                        else
                        {
                            isTurnCreationTooClose = true;

                            //set the flag to Critical stop machine
                            if (isTurnCreationTooClose) mf.mc.isOutOfBounds = true;
                            break;
                        }
                    }
                    break;
            }

            return isOutOfBounds;
        }

        public bool BuildCurvePatternYouTurn(bool isTurnRight, Vec3 pivotPos)
        {
            if (youTurnPhase > 0)
            {
                ytList.Clear();

                double head = crossingCurvePoint.heading;
                if (!mf.curve.isABSameAsVehicleHeading) head += Math.PI;

                //are we going same way as creation of curve
                //bool isCountingUp = mf.curve.isABSameAsVehicleHeading;

                double toolOffset = mf.Tools[0].ToolOffset * 2.0;
                double turnOffset;

                //turning right
                if (isTurnRight) turnOffset = (mf.Tools[0].WidthMinusOverlap - toolOffset);
                else turnOffset = (mf.Tools[0].WidthMinusOverlap + toolOffset);

                //Pattern Turn
                numShapePoints = youFileList.Count;
                Vec3[] pt = new Vec3[numShapePoints];

                //Now put the shape into an array since lists are immutable
                for (int i = 0; i < numShapePoints; i++)
                {
                    pt[i].easting = youFileList[i].easting;
                    pt[i].northing = youFileList[i].northing;
                }

                //start of path on the origin. Mirror the shape if left turn
                if (isTurnRight)
                {
                    for (int i = 0; i < pt.Length; i++) pt[i].easting *= -1;
                }

                //scaling - Drawing is 10m wide so find ratio of tool width
                double scale = turnOffset * 0.1;
                for (int i = 0; i < pt.Length; i++)
                {
                    pt[i].easting *= scale * rowSkipsWidth;
                    pt[i].northing *= scale * rowSkipsWidth;
                }

                //rotate pattern to match AB Line heading
                double xr, yr, xr2, yr2;
                for (int i = 0; i < pt.Length - 1; i++)
                {
                    xr = (Math.Cos(-head) * pt[i].easting) - (Math.Sin(-head) * pt[i].northing) + crossingCurvePoint.easting;
                    yr = (Math.Sin(-head) * pt[i].easting) + (Math.Cos(-head) * pt[i].northing) + crossingCurvePoint.northing;

                    xr2 = (Math.Cos(-head) * pt[i + 1].easting) - (Math.Sin(-head) * pt[i + 1].northing) + crossingCurvePoint.easting;
                    yr2 = (Math.Sin(-head) * pt[i + 1].easting) + (Math.Cos(-head) * pt[i + 1].northing) + crossingCurvePoint.northing;

                    pt[i].easting = xr;
                    pt[i].northing = yr;

                    pt[i].heading = Math.Atan2(xr2 - xr, yr2 - yr);
                    if (pt[i].heading < 0) pt[i].heading += Glm.twoPI;
                    ytList.Add(pt[i]);
                }
                xr = (Math.Cos(-head) * pt[pt.Length - 1].easting) - (Math.Sin(-head) * pt[pt.Length - 1].northing) + crossingCurvePoint.easting;
                yr = (Math.Sin(-head) * pt[pt.Length - 1].easting) + (Math.Cos(-head) * pt[pt.Length - 1].northing) + crossingCurvePoint.northing;

                pt[pt.Length - 1].easting = xr;
                pt[pt.Length - 1].northing = yr;
                pt[pt.Length - 1].heading = pt[pt.Length - 2].heading;
                ytList.Add(pt[pt.Length - 1]);

                //pattern all made now is it outside a boundary
                head -= Math.PI;

                Vec3 ptt;
                for (int a = 0; a < youTurnStartOffset; a++)
                {
                    ptt.easting = ytList[0].easting + (Math.Sin(head));
                    ptt.northing = ytList[0].northing + (Math.Cos(head));
                    ptt.heading = ytList[0].heading;
                    ytList.Insert(0, ptt);
                }

                int count = ytList.Count;

                for (int i = 1; i <= youTurnStartOffset; i++)
                {
                    ptt.easting = ytList[count - 1].easting + (Math.Sin(head) * i);
                    ptt.northing = ytList[count - 1].northing + (Math.Cos(head) * i);
                    ptt.heading = ytList[count - 1].heading;
                    ytList.Add(ptt);
                }

                double distancePivotToTurnLine;
                count = ytList.Count;
                for (int i = 0; i < count; i += 2)
                {
                    distancePivotToTurnLine = Glm.Distance(ytList[i], mf.pivotAxlePos);
                    if (distancePivotToTurnLine > 3)
                    {
                        isTurnCreationTooClose = false;
                    }
                    else
                    {
                        isTurnCreationTooClose = true;
                        //set the flag to Critical stop machine
                        if (isTurnCreationTooClose) mf.mc.isOutOfBounds = true;
                        break;
                    }
                }
            }

            switch (youTurnPhase)
            {
                case 0: //find the crossing points
                    if (FindCurveTurnPoints()) youTurnPhase = 1;
                    else
                    {
                        if (mf.curve.SpiralMode == true || mf.curve.CircleMode == true)
                        {
                            isTurnCreationNotCrossingError = false;
                            mf.mc.isOutOfBounds = false;
                        }
                        else
                        {
                            mf.mc.isOutOfBounds = true;
                        }
                    }
                    ytList?.Clear();
                    break;

                case 1:
                    //now check to make sure turn is not in an inner turn boundary - drive thru is ok
                    int count = ytList.Count;
                    if (count == 0) return false;
                    isOutOfBounds = false;

                    //Out of bounds?
                    for (int j = 0; j < count; j += 2)
                    {
                        if (!mf.turn.turnArr[0].IsPointInTurnWorkArea(ytList[j])) isOutOfBounds = true;
                        if (isOutOfBounds) break;

                        for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                        {
                            //make sure not inside a non drivethru boundary
                            if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                            if (mf.turn.turnArr[i].IsPointInTurnWorkArea(ytList[j]))
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                        if (isOutOfBounds) break;
                    }

                    //first check if not out of bounds, add a bit more to clear turn line, set to phase 2
                    if (!isOutOfBounds)
                    {
                        youTurnPhase = 2;
                        //if (mf.curve.isABSameAsVehicleHeading)
                        //{
                        //    crossingCurvePoint.index -= 2;
                        //    if (crossingCurvePoint.index < 0) crossingCurvePoint.index = 0;
                        //}
                        //else
                        //{
                        //    crossingCurvePoint.index += 2;
                        //    if (crossingCurvePoint.index >= curListCount)
                        //        crossingCurvePoint.index = curListCount - 1;
                        //}

                        //crossingCurvePoint.easting = mf.curve.curList[crossingCurvePoint.index].easting;
                        //crossingCurvePoint.northing = mf.curve.curList[crossingCurvePoint.index].northing;
                        //crossingCurvePoint.heading = mf.curve.curList[crossingCurvePoint.index].heading;
                        return true;
                    }

                    //keep moving infield till pattern is all inside
                    if (mf.curve.isABSameAsVehicleHeading)
                    {
                        crossingCurvePoint.index--;
                        if (crossingCurvePoint.index < 0) crossingCurvePoint.index = 0;
                    }
                    else
                    {
                        crossingCurvePoint.index++;
                        if (crossingCurvePoint.index >= curListCount)
                            crossingCurvePoint.index = curListCount - 1;
                    }

                    crossingCurvePoint.easting = mf.curve.curList[crossingCurvePoint.index].easting;
                    crossingCurvePoint.northing = mf.curve.curList[crossingCurvePoint.index].northing;
                    crossingCurvePoint.heading = mf.curve.curList[crossingCurvePoint.index].heading;

                    double tooClose = Glm.Distance(ytList[0], pivotPos);
                    isTurnCreationTooClose = tooClose < 3;

                    //set the flag to Critical stop machine
                    if (isTurnCreationTooClose) mf.mc.isOutOfBounds = true;
                    break;

                case 2:
                    youTurnPhase = 3;
                    break;
            }
            return true;
        }

        public bool BuildCurveDubinsYouTurn(bool isTurnRight, Vec3 pivotPos)
        {
            if (youTurnPhase > 0)
            {
                isABSameAsFixHeading = mf.curve.isSameWay;

                double head = crossingCurvePoint.heading;
                if (!isABSameAsFixHeading) head += Math.PI;

                boundaryAngleOffPerpendicular = Glm.PIBy2 + (crossingTurnLinePoint.heading - head);
                if (boundaryAngleOffPerpendicular > Math.PI) boundaryAngleOffPerpendicular -= Glm.twoPI;
                if (boundaryAngleOffPerpendicular < -Math.PI) boundaryAngleOffPerpendicular += Glm.twoPI;

                CDubins dubYouTurnPath = new CDubins();
                CDubins.turningRadius = mf.vehicle.minTurningRadius;

                double toolOffset = mf.Tools[0].ToolOffset * 2.0;
                double turnOffset;

                //calculate the true width
                if (isTurnRight) turnOffset = (mf.Tools[0].WidthMinusOverlap * rowSkipsWidth) - toolOffset;
                else turnOffset = (mf.Tools[0].WidthMinusOverlap * rowSkipsWidth) + toolOffset;

                //diagonally across
                double turnRadius = turnOffset * Math.Tan(boundaryAngleOffPerpendicular);
                var start = new Vec3(crossingCurvePoint.easting, crossingCurvePoint.northing, head);

                var goal = new Vec3();
                if (!isTurnRight)//this is actualy right
                {



                    goal.northing = (crossingCurvePoint.northing - (Math.Cos(head - Glm.PIBy2) * turnOffset)) - Math.Cos(head) * turnRadius;
                    goal.easting = (crossingCurvePoint.easting - (Math.Sin(head - Glm.PIBy2) * turnOffset)) - Math.Sin(head) * turnRadius;
                    goal.heading = head - Math.PI;
                }
                else
                {
                    goal.northing = (crossingCurvePoint.northing - (Math.Cos(head + Glm.PIBy2) * turnOffset)) + Math.Cos(head) * turnRadius;
                    goal.easting = (crossingCurvePoint.easting - (Math.Sin(head + Glm.PIBy2) * turnOffset)) + Math.Sin(head) * turnRadius;
                    goal.heading = head + Math.PI;
                }

                //now we go the other way to turn round
                head -= Math.PI;
                if (head < 0) head += Glm.twoPI;

                //generate the turn points
                ytList = dubYouTurnPath.GenerateDubins(start, goal);
                int count = ytList.Count;
                if (count == 0) return false;

                //these are the lead in lead out lines that add to the turn
                AddSequenceLines(head);
            }

            switch (youTurnPhase)
            {
                case 0: //find the crossing points
                    if (FindCurveTurnPoints()) youTurnPhase = 1;
                    ytList?.Clear();
                    break;

                case 1:
                    //now check to make sure we are not in an inner turn boundary - drive thru is ok
                    int count = ytList.Count;
                    if (count == 0) return false;

                    //Are we out of bounds?
                    isOutOfBounds = false;
                    for (int j = 0; j < count; j += 2)
                    {
                        if (!mf.turn.turnArr[0].IsPointInTurnWorkArea(ytList[j]))
                        {
                            isOutOfBounds = true;
                            break;
                        }

                        for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                        {
                            //make sure not inside a non drivethru boundary
                            if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                            if (mf.turn.turnArr[i].IsPointInTurnWorkArea(ytList[j]))
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                        if (isOutOfBounds) break;
                    }

                    //first check if not out of bounds, add a bit more to clear turn line, set to phase 2
                    if (!isOutOfBounds)
                    {
                        youTurnPhase = 3;
                        //if (mf.curve.isABSameAsVehicleHeading)
                        //{
                        //    crossingCurvePoint.index -= 2;
                        //    if (crossingCurvePoint.index < 0) crossingCurvePoint.index = 0;
                        //}
                        //else
                        //{
                        //    crossingCurvePoint.index += 2;
                        //    if (crossingCurvePoint.index >= curListCount)
                        //        crossingCurvePoint.index = curListCount - 1;
                        //}
                        //crossingCurvePoint.easting = mf.curve.curList[crossingCurvePoint.index].easting;
                        //crossingCurvePoint.northing = mf.curve.curList[crossingCurvePoint.index].northing;
                        //crossingCurvePoint.heading = mf.curve.curList[crossingCurvePoint.index].heading;
                        return true;
                    }

                    //keep moving infield till pattern is all inside
                    if (mf.curve.isABSameAsVehicleHeading)
                    {
                        crossingCurvePoint.index--;
                        if (crossingCurvePoint.index < 0) crossingCurvePoint.index = 0;
                    }
                    else
                    {
                        crossingCurvePoint.index++;
                        if (crossingCurvePoint.index >= curListCount)
                            crossingCurvePoint.index = curListCount - 1;
                    }
                    crossingCurvePoint.easting = mf.curve.curList[crossingCurvePoint.index].easting;
                    crossingCurvePoint.northing = mf.curve.curList[crossingCurvePoint.index].northing;
                    crossingCurvePoint.heading = mf.curve.curList[crossingCurvePoint.index].heading;

                    double tooClose = Glm.Distance(ytList[0], pivotPos);
                    isTurnCreationTooClose = tooClose < 3;

                    //set the flag to Critical stop machine
                    if (isTurnCreationTooClose) mf.mc.isOutOfBounds = true;
                    break;

                case 2:
                    youTurnPhase = 3;
                    break;
            }
            return true;
        }

        //called to initiate turn
        public void YouTurnTrigger()
        {
            //trigger pulled
            isYouTurnTriggered = true;
            mf.seq.isSequenceTriggered = true;

            //just do the opposite of last turn
            isYouTurnRight = !isLastYouTurnRight;
            isLastYouTurnRight = !isLastYouTurnRight;

            if (mf.curve.isSameWay)
            {
                if (isYouTurnRight)
                {
                    mf.curve.curveNumber -= rowSkipsWidth;
                    mf.curve.howManyPathsAway = Math.Abs(mf.curve.curveNumber);
                }
                else
                {
                    mf.curve.curveNumber += rowSkipsWidth;
                    mf.curve.howManyPathsAway = Math.Abs(mf.curve.curveNumber);
                }
            }
            else
            {
                if (isYouTurnRight)
                {
                    mf.curve.curveNumber += rowSkipsWidth;
                    mf.curve.howManyPathsAway = Math.Abs(mf.curve.curveNumber);
                }
                else
                {
                    mf.curve.curveNumber -= rowSkipsWidth;
                    mf.curve.howManyPathsAway = Math.Abs(mf.curve.curveNumber);
                }
            }
            mf.curve.isSameWay = !mf.curve.isSameWay;
        }

        //Normal copmpletion of youturn
        public void CompleteYouTurn()
        {
            isYouTurnTriggered = false;
            ResetCreatedYouTurn();
            mf.seq.ResetSequenceEventTriggers();
            mf.seq.isSequenceTriggered = false;
            mf.isBoundAlarming = false;
            //mf.curve.isSameWay = !mf.curve.isSameWay;
        }

        //something went seriously wrong so reset everything
        public void ResetYouTurn()
        {
            //fix you turn
            isYouTurnTriggered = false;
            ytList?.Clear();
            ResetCreatedYouTurn();
            turnDistanceAdjuster = 0;
            mf.isBoundAlarming = false;
            isTurnCreationTooClose = false;
            isTurnCreationNotCrossingError = false;

            //reset sequence
            mf.seq.ResetSequenceEventTriggers();
            mf.seq.isSequenceTriggered = false;
        }

        public void ResetCreatedYouTurn()
        {
            turnDistanceAdjuster = 0;
            youTurnPhase = 0;
            ytList?.Clear();
        }

        //get list of points from txt shape file
        public void LoadYouTurnShapeFromFile(string filename)
        {
            //if there is existing shape, delete it
            if (youFileList.Count > 0) youFileList.Clear();

            if (!File.Exists(filename))
            {
                mf.TimedMessageBox(2000, "Missing Youturn File", "Fix the thing!");
            }
            else
            {
                using (StreamReader reader = new StreamReader(filename))
                {
                    try
                    {
                        string line = reader.ReadLine();
                        int points = int.Parse(line);

                        if (points > 0)
                        {
                            Vec2 coords = new Vec2();
                            for (int v = 0; v < points; v++)
                            {
                                line = reader.ReadLine();
                                string[] words = line.Split(',');

                                coords.easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                                coords.northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                                youFileList.Add(coords);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        mf.TimedMessageBox(2000, "YouTurn File is Corrupt", "But Field is Loaded");
                        mf.WriteErrorLog("FieldOpen, Loading Flags, Corrupt Flag File" + e);
                    }
                }
            }
        }

        //Resets the drawn YOuTurn and set diagPhase to 0

        //build the points and path of youturn to be scaled and transformed
        public void BuildManualYouTurn(bool isTurnRight, bool isTurnButtonTriggered)
        {
            isYouTurnTriggered = true;

            double head;
            //point on AB line closest to pivot axle point from ABLine PurePursuit
            if (mf.ABLine.isABLineSet)
            {
                rEastYT = mf.ABLine.rEastAB;
                rNorthYT = mf.ABLine.rNorthAB;
                isABSameAsFixHeading = mf.ABLine.isABSameAsVehicleHeading;
                head = mf.ABLine.abHeading;
            }
            else
            {
                if (mf.curve.isSameWay)
                {
                    if (isTurnRight)
                    {
                        mf.curve.curveNumber -= rowSkipsWidth;
                        mf.curve.howManyPathsAway = Math.Abs(mf.curve.curveNumber);
                    }
                    else
                    {
                        mf.curve.curveNumber += rowSkipsWidth;
                        mf.curve.howManyPathsAway = Math.Abs(mf.curve.curveNumber);
                    }
                }
                else
                {
                    if (isTurnRight)
                    {
                        mf.curve.curveNumber += rowSkipsWidth;
                        mf.curve.howManyPathsAway = Math.Abs(mf.curve.curveNumber);
                    }
                    else
                    {
                        mf.curve.curveNumber -= rowSkipsWidth;
                        mf.curve.howManyPathsAway = Math.Abs(mf.curve.curveNumber);
                    }
                }
                mf.curve.isSameWay = !mf.curve.isSameWay;



                rEastYT = mf.curve.rEastCu;
                rNorthYT = mf.curve.rNorthCu;
                isABSameAsFixHeading = !mf.curve.isSameWay;
                head = mf.curve.curList[mf.curve.currentLocationIndex].heading;//set to curent line heading ;)
            }

            double toolOffset = mf.Tools[0].ToolOffset * 2.0;
            double turnOffset;

            //turning right
            if (isTurnRight) turnOffset = (mf.Tools[0].WidthMinusOverlap * rowSkipsWidth + toolOffset);
            else turnOffset = (mf.Tools[0].WidthMinusOverlap * rowSkipsWidth - toolOffset);

            CDubins dubYouTurnPath = new CDubins();
            CDubins.turningRadius = mf.vehicle.minTurningRadius;

            //if its straight across it makes 2 loops instead so goal is a little lower then start
            if (!isABSameAsFixHeading) head += 3.14;
            else head -= 0.01;

            //move the start forward 2 meters, this point is critical to formation of uturn
            rEastYT += (Math.Sin(head) * 2);
            rNorthYT += (Math.Cos(head) * 2);

            //now we have our start point
            var start = new Vec3(rEastYT, rNorthYT, head);
            var goal = new Vec3();


            //now we go the other way to turn round
            head -= Math.PI;
            if (head < 0) head += Glm.twoPI;

            //set up the goal point for Dubins
            goal.heading = head;
            if (isTurnButtonTriggered)
            {
                if (isTurnRight)
                {
                    goal.easting = rEastYT - (Math.Cos(-head) * turnOffset);
                    goal.northing = rNorthYT - (Math.Sin(-head) * turnOffset);
                }
                else
                {
                    goal.easting = rEastYT + (Math.Cos(-head) * turnOffset);
                    goal.northing = rNorthYT + (Math.Sin(-head) * turnOffset);
                }
            }

            //generate the turn points
            ytList = dubYouTurnPath.GenerateDubins(start, goal);

            Vec3 pt;
            for (int a = 0; a < 3; a++)
            {
                pt.easting = ytList[0].easting + (Math.Sin(head));
                pt.northing = ytList[0].northing + (Math.Cos(head));
                pt.heading = ytList[0].heading;
                ytList.Insert(0, pt);
            }

            int count = ytList.Count;

            for (int i = 1; i <= 7; i++)
            {
                pt.easting = ytList[count - 1].easting + (Math.Sin(head) * i);
                pt.northing = ytList[count - 1].northing + (Math.Cos(head) * i);
                pt.heading = head;
                ytList.Add(pt);
            }
        }

        public int onA;

        //determine distance from youTurn guidance line
        public void DistanceFromYouTurnLine()
        {
            //grab a copy from main - the steer position
            double minDistA = 1000000, minDistB = 1000000;
            int ptCount = ytList.Count;

            if (ptCount > 0)
            {
                if (mf.isStanleyUsed)
                {
                    pivot = mf.steerAxlePos;

                    //find the closest 2 points to current fix
                    for (int t = 0; t < ptCount; t++)
                    {
                        double dist = ((pivot.easting - ytList[t].easting) * (pivot.easting - ytList[t].easting))
                                        + ((pivot.northing - ytList[t].northing) * (pivot.northing - ytList[t].northing));
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

                    minDistA = 100;
                    int closestPt = 0;
                    for (int i = 0; i < ptCount; i++)
                    {
                        double distancePiv = Glm.Distance(ytList[i], pivot);
                        if (distancePiv < minDistA)
                        {
                            minDistA = distancePiv;
                            closestPt = i;
                        }
                    }

                    //used for sequencing to find entry, exit positioning
                    onA = ptCount / 2;
                    if (closestPt < onA) onA = -closestPt;
                    else onA = ptCount - closestPt;

                    //return and reset if too far away or end of the line
                    if (B >= ptCount - 1)
                    {
                        CompleteYouTurn();
                        return;
                    }

                    //feed forward to turn faster
                    A++;
                    B++;

                    //get the distance from currently active AB line, precalc the norm of line
                    double dx = ytList[B].easting - ytList[A].easting;
                    double dz = ytList[B].northing - ytList[A].northing;
                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                    double abHeading = ytList[A].heading;

                    //how far from current AB Line is steer point 90 degrees from steer position
                    distanceFromCurrentLine = ((dz * pivot.easting) - (dx * pivot.northing) + (ytList[B].easting
                                * ytList[A].northing) - (ytList[B].northing * ytList[A].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                    //are we on the right side or not, the sign from above determines that
                    isOnRightSideCurrentLine = distanceFromCurrentLine > 0;

                    //Calc point on ABLine closest to current position and 90 degrees to segment heading
                    double U = (((pivot.easting - ytList[A].easting) * dx)
                                + ((pivot.northing - ytList[A].northing) * dz))
                                / ((dx * dx) + (dz * dz));

                    //critical point used as start for the uturn path - critical
                    rEastYT = ytList[A].easting + (U * dx);
                    rNorthYT = ytList[A].northing + (U * dz);

                    //the first part of stanley is to extract heading error
                    double abFixHeadingDelta = (pivot.heading - abHeading);

                    //Fix the circular error - get it from -Pi/2 to Pi/2
                    if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < Math.PI) abFixHeadingDelta += Math.PI;
                    if (abFixHeadingDelta > Glm.PIBy2) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < -Glm.PIBy2) abFixHeadingDelta += Math.PI;

                    //normally set to 1, less then unity gives less heading error.
                    abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
                    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
                    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

                    //the non linear distance error part of stanley
                    steerAngleYT = Math.Atan((distanceFromCurrentLine * mf.vehicle.stanleyGain) / ((mf.pn.speed * 0.277777) + 1));

                    //clamp it to max 42 degrees
                    if (steerAngleYT > 0.74) steerAngleYT = 0.74;
                    if (steerAngleYT < -0.74) steerAngleYT = -0.74;

                    //add them up and clamp to max in vehicle settings
                    steerAngleYT = Glm.ToDegrees((steerAngleYT + abFixHeadingDelta) * -1.0);
                    if (steerAngleYT < -mf.vehicle.maxSteerAngle) steerAngleYT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleYT > mf.vehicle.maxSteerAngle) steerAngleYT = mf.vehicle.maxSteerAngle;

                    //Convert to millimeters and round properly to above/below .5
                    distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);

                    //every guidance method dumps into these that are used and sent everywhere, last one wins
                    mf.guidanceLineDistanceOff = mf.distanceDisplay = (Int16)distanceFromCurrentLine;
                    mf.guidanceLineSteerAngle = (Int16)(steerAngleYT * 100);
                }
                else
                {
                    pivot = mf.steerAxlePos;

                    //find the closest 2 points to current fix
                    for (int t = 0; t < ptCount; t++)
                    {
                        double dist = ((pivot.easting - ytList[t].easting) * (pivot.easting - ytList[t].easting))
                                        + ((pivot.northing - ytList[t].northing) * (pivot.northing - ytList[t].northing));
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

                    minDistA = 100;
                    int closestPt = 0;
                    for (int i = 0; i < ptCount; i++)
                    {
                        double distancePiv = Glm.Distance(ytList[i], mf.pivotAxlePos);
                        if (distancePiv < minDistA)
                        {
                            minDistA = distancePiv;
                            closestPt = i;
                        }
                    }

                    onA = ptCount / 2;
                    if (closestPt < onA)
                    {
                        onA = -closestPt;
                    }
                    else
                    {
                        onA = ptCount - closestPt;
                    }

                    //return and reset if too far away or end of the line
                    if (B >= ptCount - 1)
                    {
                        CompleteYouTurn();
                        return;
                    }

                    //get the distance from currently active AB line
                    double dx = ytList[B].easting - ytList[A].easting;
                    double dz = ytList[B].northing - ytList[A].northing;
                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                    //how far from current AB Line is fix
                    distanceFromCurrentLine = ((dz * pivot.easting) - (dx * pivot.northing) + (ytList[B].easting
                                * ytList[A].northing) - (ytList[B].northing * ytList[A].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                    //are we on the right side or not
                    isOnRightSideCurrentLine = distanceFromCurrentLine > 0;

                    //absolute the distance
                    distanceFromCurrentLine = Math.Abs(distanceFromCurrentLine);

                    // ** Pure pursuit ** - calc point on ABLine closest to current position
                    double U = (((pivot.easting - ytList[A].easting) * dx)
                                + ((pivot.northing - ytList[A].northing) * dz))
                                / ((dx * dx) + (dz * dz));

                    rEastYT = ytList[A].easting + (U * dx);
                    rNorthYT = ytList[A].northing + (U * dz);

                    //update base on autosteer settings and distance from line
                    double goalPointDistance = mf.vehicle.UpdateGoalPointDistance(distanceFromCurrentLine);

                    //sharp turns on you turn.
                    goalPointDistance = mf.vehicle.goalPointLookAheadUturnMult * goalPointDistance;
                    mf.lookaheadActual = goalPointDistance;

                    //used for accumulating distance to find goal point
                    double distSoFar;

                    isABSameAsFixHeading = true;
                    distSoFar = Glm.Distance(ytList[B], rEastYT, rNorthYT);

                    // used for calculating the length squared of next segment.
                    double tempDist = 0.0;

                    //Is this segment long enough to contain the full lookahead distance?
                    if (distSoFar > goalPointDistance)
                    {
                        //treat current segment like an AB Line
                        goalPointYT.easting = rEastYT + (Math.Sin(ytList[A].heading) * goalPointDistance);
                        goalPointYT.northing = rNorthYT + (Math.Cos(ytList[A].heading) * goalPointDistance);
                    }

                    //multiple segments required
                    else
                    {
                        //cycle thru segments and keep adding lengths. check if end and break if so.
                        while (B < ptCount - 1)
                        {
                            B++; A++;
                            tempDist = Glm.Distance(ytList[B], ytList[A]);
                            if ((tempDist + distSoFar) > goalPointDistance) break; //will we go too far?
                            distSoFar += tempDist;
                        }

                        double t = (goalPointDistance - distSoFar); // the remainder to yet travel
                        t /= tempDist;
                        goalPointYT.easting = (((1 - t) * ytList[A].easting) + (t * ytList[B].easting));
                        goalPointYT.northing = (((1 - t) * ytList[A].northing) + (t * ytList[B].northing));
                    }

                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = Glm.DistanceSquared(goalPointYT.northing, goalPointYT.easting, pivot.northing, pivot.easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading = Glm.twoPI - mf.fixHeading;
                    ppRadiusYT = goalPointDistanceSquared / (2 * (((goalPointYT.easting - pivot.easting) * Math.Cos(localHeading)) + ((goalPointYT.northing - pivot.northing) * Math.Sin(localHeading))));

                    steerAngleYT = Glm.ToDegrees(Math.Atan(2 * (((goalPointYT.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPointYT.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

                    if (steerAngleYT < -mf.vehicle.maxSteerAngle) steerAngleYT = -mf.vehicle.maxSteerAngle;
                    if (steerAngleYT > mf.vehicle.maxSteerAngle) steerAngleYT = mf.vehicle.maxSteerAngle;

                    if (ppRadiusYT < -500) ppRadiusYT = -500;
                    if (ppRadiusYT > 500) ppRadiusYT = 500;

                    radiusPointYT.easting = pivot.easting + (ppRadiusYT * Math.Cos(localHeading));
                    radiusPointYT.northing = pivot.northing + (ppRadiusYT * Math.Sin(localHeading));

                    //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
                    double angVel = Glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(Glm.ToRadians(steerAngleYT))) / mf.vehicle.wheelbase;

                    //clamp the steering angle to not exceed safe angular velocity
                    if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
                    {
                        steerAngleYT = Glm.ToDegrees(steerAngleYT > 0 ?
                                (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (Glm.twoPI * mf.pn.speed * 0.277777)))
                            : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (Glm.twoPI * mf.pn.speed * 0.277777))));
                    }
                    //Convert to centimeters
                    distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);

                    //distance is negative if on left, positive if on right
                    //if you're going the opposite direction left is right and right is left
                    if (isABSameAsFixHeading)
                    {
                        if (!isOnRightSideCurrentLine) distanceFromCurrentLine *= -1.0;
                    }

                    //opposite way so right is left
                    else
                    {
                        if (isOnRightSideCurrentLine) distanceFromCurrentLine *= -1.0;
                    }

                    mf.guidanceLineDistanceOff = mf.distanceDisplay = (Int16)distanceFromCurrentLine;
                    mf.guidanceLineSteerAngle = (Int16)(steerAngleYT * 100);
                }
            }
            else
            {
                CompleteYouTurn();
            }
        }

        //Duh.... What does this do....
        public void DrawYouTurn()
        {
            {
                //GL.PointSize(8);
                //GL.Begin(PrimitiveType.Points);
                //{
                //    GL.Color3(0.95f, 0.05f, 0.05f);
                //    GL.Vertex3(crossingCurvePoint.easting, crossingCurvePoint.northing, 0);
                //    GL.Color3(0.05f, 9, 0.05f);
                //    GL.Vertex3(crossingTurnLinePoint.easting, crossingTurnLinePoint.northing, 0);
                //}
                //GL.End();

                int ptCount = ytList.Count;
                if (ptCount < 3) return;
                GL.PointSize(mf.ABLine.lineWidth);

                if (isYouTurnTriggered)
                {
                    GL.Color3(0.95f, 0.95f, 0.25f);
                    GL.Begin(PrimitiveType.Points);
                    for (int i = 0; i < ptCount; i++)
                    {
                        GL.Vertex3(ytList[i].easting, ytList[i].northing, 0);
                    }
                    GL.End();
                }
                else
                {
                    if (!isOutOfBounds)
                        GL.Color3(0.395f, 0.925f, 0.30f);
                    else
                        GL.Color3(0.9495f, 0.395f, 0.325f);
                    {
                        GL.Begin(PrimitiveType.Points);
                        for (int i = 0; i < ptCount; i++)
                        {
                            GL.Vertex3(ytList[i].easting, ytList[i].northing, 0);
                        }
                        GL.End();
                    }
                }
            }
        }
    }
}