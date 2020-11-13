﻿using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;

namespace AgOpenGPS
{
    public class CYouTurn
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        /// <summary>/// triggered right after youTurnTriggerPoint is set /// </summary>
        public bool isYouTurnTriggered;

        /// <summary>  /// turning right or left?/// </summary>
        public bool isYouTurnRight;
        public bool SwapYouTurn = true;
        //controlled by user in GUI to en/dis able
        public bool isRecordingCustomYouTurn;

        /// <summary> /// Is the youturn button enabled? /// </summary>
        public bool isYouTurnBtnOn;

        //Patterns or Dubins
        public byte YouTurnType;

        public double boundaryAngleOffPerpendicular;
        public double distanceTurnBeforeLine = 0;
        public double ytLength = 0;

        public int rowSkipsWidth = 1;

        /// <summary>  /// distance from headland as offset where to start turn shape /// </summary>
        public int youTurnStartOffset;

        //guidance values
        public double distanceFromCurrentLine, triggerDistanceOffset, geoFenceDistance, dxAB, dyAB;

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
        public double onA;

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

            YouTurnType = Properties.Vehicle.Default.Youturn_Type;
        }

        //Finds the point where an AB Curve crosses the turn line
        public bool FindCurveTurnPoints()
        {
            crossingCurvePoint.Easting = -20000;
            crossingTurnLinePoint.Easting = -20000;
            crossingTurnLinePoint.Index = 99;

            //find closet AB Curve point that will cross and go out of bounds
            curListCount = mf.CurveLines.curList.Count;
            if (curListCount < 3) return false;
            //otherwise we count down
            bool isCountingUp = mf.CurveLines.isSameWay;

            bool Loop = true;
            //check if outside a border
            if (isCountingUp)
            {
                for (int j = mf.currentLocationIndex; j < mf.currentLocationIndex || Loop; j++)
                {
                    if (j == curListCount)
                    {
                        if (!mf.CurveLines.Lines[mf.CurveLines.CurrentLine].CircleMode && !mf.CurveLines.Lines[mf.CurveLines.CurrentLine].SpiralMode && !mf.CurveLines.Lines[mf.CurveLines.CurrentLine].BoundaryMode)
                            break;
                        j = 0;
                        Loop = false;
                    }

                    if (!mf.bnd.bndArr[0].IsPointInTurnWorkArea(mf.CurveLines.curList[j]))
                    {
                        //it passed outer turnLine
                        crossingCurvePoint.Easting = mf.CurveLines.curList[j].Easting;
                        crossingCurvePoint.Northing = mf.CurveLines.curList[j].Northing;
                        crossingCurvePoint.Heading = mf.CurveLines.curList[j].Heading;
                        crossingCurvePoint.Index = j;
                        crossingTurnLinePoint.Index = 0;
                        goto CrossingFound;
                    }

                    for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                    {
                        //make sure not inside a non drivethru boundary
                        if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                        if (mf.bnd.bndArr[0].IsPointInTurnWorkArea(mf.CurveLines.curList[j]))
                        {
                            crossingCurvePoint.Easting = mf.CurveLines.curList[j].Easting;
                            crossingCurvePoint.Northing = mf.CurveLines.curList[j].Northing;
                            crossingCurvePoint.Heading = mf.CurveLines.curList[j].Heading;
                            crossingCurvePoint.Index = j - 1;
                            crossingTurnLinePoint.Index = i;
                            goto CrossingFound;
                        }
                    }
                }

                //escape for multiple for's
                CrossingFound:;
            }
            else //counting down, going opposite way mf.curve was created.
            {
                for (int j = mf.currentLocationIndex; j > mf.currentLocationIndex || Loop; j--)
                {
                    if (j == -1)
                    {
                        if (!mf.CurveLines.Lines[mf.CurveLines.CurrentLine].CircleMode && !mf.CurveLines.Lines[mf.CurveLines.CurrentLine].SpiralMode && !mf.CurveLines.Lines[mf.CurveLines.CurrentLine].BoundaryMode) break;
                        j = curListCount - 1;
                        Loop = false;
                    }

                    if (!mf.bnd.bndArr[0].IsPointInTurnWorkArea(mf.CurveLines.curList[j]))
                    {                                        //it passed outer turnLine
                        crossingCurvePoint.Easting = mf.CurveLines.curList[j].Easting;
                        crossingCurvePoint.Northing = mf.CurveLines.curList[j].Northing;
                        crossingCurvePoint.Heading = mf.CurveLines.curList[j].Heading;
                        crossingCurvePoint.Index = j;
                        crossingTurnLinePoint.Index = 0;
                        goto CrossingFound;
                    }

                    for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                    {
                        //make sure not inside a non drivethru boundary
                        if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                        if (mf.bnd.bndArr[i].IsPointInTurnWorkArea(mf.CurveLines.curList[j]))
                        {
                            crossingCurvePoint.Easting = mf.CurveLines.curList[j].Easting;
                            crossingCurvePoint.Northing = mf.CurveLines.curList[j].Northing;
                            crossingCurvePoint.Heading = mf.CurveLines.curList[j].Heading;
                            crossingCurvePoint.Index = j;
                            crossingTurnLinePoint.Index = i;
                            goto CrossingFound;
                        }
                    }
                }

                //escape for multiple for's, point and turnLine index are found
                CrossingFound:;
            }

            int turnNum = crossingTurnLinePoint.Index;
            int turnNum2 = crossingTurnLinePoint.Index;

            if (turnNum == 99)
            {
                if (!mf.CurveLines.Lines[mf.CurveLines.CurrentLine].CircleMode && !mf.CurveLines.Lines[mf.CurveLines.CurrentLine].SpiralMode && !mf.CurveLines.Lines[mf.CurveLines.CurrentLine].BoundaryMode)
                    isTurnCreationNotCrossingError = true;
                else
                {
                    youTurnPhase = 3;
                }
                return false;
            }

            int curTurnLineCount = mf.bnd.bndArr[turnNum].turnLine.Count;

            //possible points close to AB Curve point
            List<int> turnLineCloseList = new List<int>();

            for (int j = 0; j < curTurnLineCount; j++)
            {
                if ((mf.bnd.bndArr[turnNum].turnLine[j].Easting - crossingCurvePoint.Easting) < 2
                    && (mf.bnd.bndArr[turnNum].turnLine[j].Easting - crossingCurvePoint.Easting) > -2
                    && (mf.bnd.bndArr[turnNum].turnLine[j].Northing - crossingCurvePoint.Northing) < 2
                    && (mf.bnd.bndArr[turnNum].turnLine[j].Northing - crossingCurvePoint.Northing) > -2)
                {
                    turnLineCloseList.Add(j);
                }
            }

            double dist1, dist2 = 99;
            curTurnLineCount = turnLineCloseList.Count;
            for (int i = 0; i < curTurnLineCount; i++)
            {
                dist1 = Glm.Distance(mf.bnd.bndArr[turnNum].turnLine[turnLineCloseList[i]].Easting,
                                        mf.bnd.bndArr[turnNum].turnLine[turnLineCloseList[i]].Northing,
                                            crossingCurvePoint.Easting, crossingCurvePoint.Northing);
                if (dist1 < dist2)
                {
                    turnNum2 = turnLineCloseList[i];
                    dist2 = dist1;
                }
            }

            //fill up the coords
            crossingTurnLinePoint.Easting = mf.bnd.bndArr[turnNum].turnLine[turnNum2].Easting;
            crossingTurnLinePoint.Northing = mf.bnd.bndArr[turnNum].turnLine[turnNum2].Northing;
            crossingTurnLinePoint.Heading = mf.bnd.bndArr[turnNum].turnLine[turnNum2].Heading;

            return crossingCurvePoint.Easting != -20000 && crossingCurvePoint.Easting != -20000;
        }

        public void AddSequenceLines(double head, bool Straight = false)
        {
            Vec3 pt;
            double Hcos = Math.Cos(head);
            double Hsin = Math.Sin(head);

            double distancePivotToTurnLine;
            for (int i = 0; i < youTurnStartOffset; i++)
            {
                pt.Easting = ytList[0].Easting - Hsin;
                pt.Northing = ytList[0].Northing - Hcos;
                pt.Heading = ytList[0].Heading;
                ytList.Insert(0, pt);

                distancePivotToTurnLine = Glm.Distance(ytList[0], mf.pivotAxlePos);
                if (distancePivotToTurnLine > 3)
                {
                    isTurnCreationTooClose = false;
                }
                else
                {
                    isTurnCreationTooClose = true;
                    break;
                }

                if (Straight)
                {
                    pt.Easting = ytList[ytList.Count - 1].Easting + Hsin;
                    pt.Northing = ytList[ytList.Count - 1].Northing + Hcos;
                    pt.Heading = head;
                }
                else
                {
                    pt.Easting = ytList[ytList.Count - 1].Easting - Hsin;
                    pt.Northing = ytList[ytList.Count - 1].Northing - Hcos;
                    pt.Heading = head - Math.PI;
                }
                ytList.Add(pt);
            }
            ytLength = 0;
            for (int i = 0; i +1 < ytList.Count; i++)
            {
                ytLength += Glm.Distance(ytList[i], ytList[i+1]);
            }
        }

        //list of points of collision path avoidance
        public List<Vec3> mazeList = new List<Vec3>();

        public bool BuildDriveAround()
        {
            if (mf.ABLines.CurrentLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentLine > -1)
            {
                double head = mf.ABLines.ABLines[mf.ABLines.CurrentLine].Heading;
                if (!mf.ABLines.isSameWay) head += Math.PI;

                double cosHead = Math.Cos(head);
                double sinHead = Math.Sin(head);

                Vec3 start = new Vec3();
                Vec3 stop = new Vec3();
                Vec3 pt2 = new Vec3();

                //grab the pure pursuit point right on ABLine
                Vec3 onPurePoint = new Vec3(mf.rNorth, mf.rEast, 0);

                //how far are we from any geoFence
                mf.gf.FindPointsDriveAround(onPurePoint, head, ref start, ref stop);

                //not an inside border
                if (start.Easting == 88888) return false;

                //get the dubins path vec3 point coordinates of path
                ytList.Clear();

                //find a path from start to goal - diagnostic, but also used later
                mazeList = mf.mazeGrid.SearchForPath(start, stop);

                //you can't get anywhere!
                if (mazeList == null) return false;

                //not really changing direction so need to fake a turn twice.
                SwapYouTurn = false;

                //Dubins at the start and stop of mazePath
                CDubins.turningRadius = mf.vehicle.minTurningRadius * 1.0;
                CDubins dubPath = new CDubins();

                //start is navigateable - maybe
                int cnt = mazeList.Count;
                int cut = 8;
                if (cnt < 18) cut = 3;

                if (cnt > 0)
                {
                    pt2.Easting = start.Easting - (sinHead * mf.vehicle.minTurningRadius * 1.5);
                    pt2.Northing = start.Northing - (cosHead * mf.vehicle.minTurningRadius * 1.5);
                    pt2.Heading = head;

                    List<Vec3> shortestDubinsList = dubPath.GenerateDubins(pt2, mazeList[cut - 1], mf.gf);
                    for (int i = 1; i < shortestDubinsList.Count; i++)
                    {
                        Vec3 pt = new Vec3(shortestDubinsList[i].Northing, shortestDubinsList[i].Easting, shortestDubinsList[i].Heading);
                        ytList.Add(pt);
                    }

                    for (int i = cut; i < mazeList.Count - cut; i++)
                    {
                        Vec3 pt = new Vec3(mazeList[i].Northing, mazeList[i].Easting, mazeList[i].Heading);
                        ytList.Add(pt);
                    }

                    pt2.Easting = stop.Easting + (sinHead * mf.vehicle.minTurningRadius * 1.5);
                    pt2.Northing = stop.Northing + (cosHead * mf.vehicle.minTurningRadius * 1.5);
                    pt2.Heading = head;

                    shortestDubinsList = dubPath.GenerateDubins(mazeList[cnt - cut], pt2, mf.gf);

                    for (int i = 1; i < shortestDubinsList.Count; i++)
                    {
                        Vec3 pt = new Vec3(shortestDubinsList[i].Northing, shortestDubinsList[i].Easting, shortestDubinsList[i].Heading);
                        ytList.Add(pt);
                    }
                }

                if (ytList.Count > 10) youTurnPhase = 3;

                AddSequenceLines(head, true);

                return true;
            }

            return false;
        }

        public bool BuildABLineCurveYouTurn(bool isTurnRight)
        {
            if (mf.ABLines.CurrentLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentLine > -1)
            {
                double head = mf.ABLines.ABLines[mf.ABLines.CurrentLine].Heading;
                if (!mf.ABLines.isSameWay) head += Math.PI;

                double Hcos = Math.Cos(head);
                double Hsin = Math.Sin(head);
                List<Vec4> Crossings = new List<Vec4>();

                Vec3 Start = new Vec3(mf.rNorth, mf.rEast,0);
                Vec3 End = Start;

                End.Northing += Hcos * mf.maxCrossFieldLength;
                End.Easting += Hsin * mf.maxCrossFieldLength;

                double turnOffset = (mf.Guidance.WidthMinusOverlap * rowSkipsWidth) + (isTurnRight ? mf.Guidance.GuidanceOffset * 2.0 : -mf.Guidance.GuidanceOffset * 2.0);

                if (Math.Abs(turnOffset) > mf.vehicle.minTurningRadius * 2)
                {
                    if (turnOffset < 0) isTurnRight = !isTurnRight;

                    for (int i = 0; i < mf.bnd.bndArr.Count; i++)
                    {
                        if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                        if (mf.bnd.bndArr.Count > i)
                        {
                            Crossings.FindCrossingPoints(mf.bnd.bndArr[i].turnLine, Start, End, i);
                        }
                    }
                    ytList.Clear();

                    if (Crossings.Count > 0)
                    {
                        Crossings.Sort((x, y) => x.Time.CompareTo(y.Time));

                        int tt = (int)(Crossings[0].Heading + (isTurnRight ? 0.5 : -0.5));
                        int StartInt = tt;
                        int count = isTurnRight ? 1 : -1;
                        if (Crossings[0].Index != 0) count *= -1;

                        tt = (tt + count).Clamp(mf.bnd.bndArr[Crossings[0].Index].turnLine.Count);


                        if (turnOffset < 0) turnOffset *= -1;
                        if (!isTurnRight) turnOffset *= -1;
                        
                        Vec2 Start2;
                        Start2.Northing = Start.Northing + Hsin * -turnOffset - Hcos * mf.maxCrossFieldLength;
                        Start2.Easting = Start.Easting + Hcos * turnOffset - Hsin * mf.maxCrossFieldLength;

                        Vec2 End2;
                        End2.Northing = End.Northing + Hsin * -turnOffset;
                        End2.Easting = End.Easting + Hcos * turnOffset;

                        Vec2 StartEnd2 = Start2 - End2;
                        Vec3 StartEnd = Start - End;

                        ytList.Add(Start);
                        ytList.Add(new Vec3(Crossings[0].Northing, Crossings[0].Easting, 0));

                        int CDLine = 0;
                        int ABLine = 0;



                        while (isTurnRight ? CDLine <= 0 && ABLine >= 0 : CDLine >= 0 && ABLine <= 0)
                        {
                            ytList.Add(mf.bnd.bndArr[Crossings[0].Index].turnLine[tt]);

                            ABLine = Math.Sign((StartEnd.Easting * (mf.bnd.bndArr[Crossings[0].Index].turnLine[tt].Northing - End.Northing))
                               - (StartEnd.Northing * (mf.bnd.bndArr[Crossings[0].Index].turnLine[tt].Easting - End.Easting)));

                            //offset
                            CDLine = Math.Sign((StartEnd2.Easting * (mf.bnd.bndArr[Crossings[0].Index].turnLine[tt].Northing - End2.Northing))
                                - (StartEnd2.Northing * (mf.bnd.bndArr[Crossings[0].Index].turnLine[tt].Easting - End2.Easting)));

                            tt += count;
                            tt = tt.Clamp(mf.bnd.bndArr[Crossings[0].Index].turnLine.Count);


                            if (tt == StartInt)
                            {
                                ytList.Clear();
                                return YtListClear();
                            }
                        }

                        if (isTurnRight ? ABLine < 0 : ABLine > 0)
                        {
                            if (Crossings.Count > 1 && Crossings[0].Index == Crossings[1].Index)
                            {
                                if (DanielP.GetLineIntersection(new Vec3(Start.Northing, Start.Easting, 0), new Vec3(End.Northing, End.Easting, 0), ytList[ytList.Count - 2], ytList[ytList.Count - 1], out Vec3 Crossing2, out _))
                                {
                                    ytList.RemoveAt(ytList.Count - 1);
                                    ytList.Add(Crossing2);

                                    Crossing2.Northing += (Math.Cos(head) * mf.Guidance.WidthMinusOverlap * 15);
                                    Crossing2.Easting += (Math.Sin(head) * mf.Guidance.WidthMinusOverlap * 15);
                                    ytList.Add(Crossing2);
                                    SwapYouTurn = false;
                                }
                                else return YtListClear();
                            }
                            else return YtListClear();
                        }
                        else
                        {
                            if (DanielP.GetLineIntersection(new Vec3(Start2.Northing, Start2.Easting, 0), new Vec3(End2.Northing, End2.Easting, 0), ytList[ytList.Count - 2], ytList[ytList.Count - 1], out Vec3 Crossing, out _))
                            {
                                ytList.RemoveAt(ytList.Count - 1);

                                ytList.Add(Crossing);

                                Crossing.Northing -= (Math.Cos(head) * mf.Guidance.WidthMinusOverlap * 15);
                                Crossing.Easting -= (Math.Sin(head) * mf.Guidance.WidthMinusOverlap * 15);
                                ytList.Add(Crossing);
                                SwapYouTurn = true;
                            }
                            else return YtListClear();
                        }

                        ytList.CalculateRoundedCorner(mf.vehicle.minTurningRadius, false, 0.0836332, CancellationToken.None);
                        ytList.RemoveAt(0);
                        ytList.RemoveAt(ytList.Count - 1);

                        count = ytList.Count;
                        if (count == 0) return YtListClear();

                        AddSequenceLines(head, !SwapYouTurn);

                        if (ytList.Count == 0) return YtListClear();
                        else youTurnPhase = 3;
                        return true;
                    }
                    return YtListClear();
                }
                else
                {
                    return BuildABLineDubinsYouTurn(isTurnRight);
                }
            }
            return YtListClear();
        }

        public bool YtListClear()
        {
            ytList.Clear();
            return false;
        }

        public bool BuildABLineDubinsYouTurn(bool isTurnRight)
        {
            if (mf.ABLines.CurrentLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentLine > -1)
            {
                double head = mf.ABLines.ABLines[mf.ABLines.CurrentLine].Heading;
                if (!mf.ABLines.isSameWay) head += Math.PI;

                double Hcos = Math.Cos(head);
                double Hsin = Math.Sin(head);

                if (youTurnPhase == 0)
                {
                    if (BuildDriveAround()) return true;

                    //grab the pure pursuit point right on ABLine
                    Vec3 onPurePoint = new Vec3(mf.rNorth, mf.rEast, 0);

                    //how far are we from any turn boundary
                    mf.turn.FindClosestTurnPoint(isYouTurnRight, onPurePoint, head);

                    //or did we lose the turnLine - we are on the highway cuz we left the outer/inner turn boundary
                    if ((int)mf.turn.closestTurnPt.Easting != -20000)
                    {
                        mf.distancePivotToTurnLine = Glm.Distance(mf.pivotAxlePos, mf.turn.closestTurnPt);
                    }
                    else
                    {
                        //Full emergency stop code goes here, it thinks its auto turn, but its not!
                        mf.distancePivotToTurnLine = -3333;
                    }

                    boundaryAngleOffPerpendicular = (mf.turn.closestTurnPt.Heading - head) - Glm.PIBy2;

                    if (boundaryAngleOffPerpendicular > Math.PI) boundaryAngleOffPerpendicular -= Glm.twoPI;
                    if (boundaryAngleOffPerpendicular < -Math.PI) boundaryAngleOffPerpendicular += Glm.twoPI;

                    CDubins dubYouTurnPath = new CDubins();
                    CDubins.turningRadius = mf.vehicle.minTurningRadius;

                    double turnOffset = (mf.Guidance.WidthMinusOverlap * rowSkipsWidth) + (isTurnRight ? mf.Guidance.GuidanceOffset * 2.0 : -mf.Guidance.GuidanceOffset * 2.0);


                    double turnRadius = turnOffset * Math.Tan(boundaryAngleOffPerpendicular);

                    if (turnOffset < mf.vehicle.minTurningRadius * 2) turnRadius = 0;

                    //start point of Dubins
                    rEastYT = mf.rEast + Hsin * mf.distancePivotToTurnLine;
                    rNorthYT = mf.rNorth + Hcos * mf.distancePivotToTurnLine;
                    Vec3 Start = new Vec3(rNorthYT, rEastYT, head);
                    Vec3 Goal = new Vec3();

                    if (isTurnRight)
                    {
                        Goal.Northing = rNorthYT + Hsin * -turnOffset + Hcos * -turnRadius;
                        Goal.Easting = rEastYT + Hcos * turnOffset + Hsin * -turnRadius;
                        Goal.Heading = head - Math.PI;
                    }
                    else
                    {
                        Goal.Northing = rNorthYT + Hsin * turnOffset + Hcos * turnRadius;
                        Goal.Easting = rEastYT + Hcos * -turnOffset + Hsin * turnRadius;
                        Goal.Heading = head - Math.PI;
                    }

                    //generate the turn points
                    ytList = dubYouTurnPath.GenerateDubins(Start, Goal);
                    int count = ytList.Count;
                    if (count == 0) return false;

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
                int cnt = ytList.Count;
                Vec3 arr2;

                if (youTurnPhase < 2)
                {
                    //the temp array
                    mf.distancePivotToTurnLine = Glm.Distance(ytList[0], mf.pivotAxlePos);

                    for (int i = 0; i < cnt; i++)
                    {
                        arr2 = ytList[i];
                        arr2.Northing -= Hcos;
                        arr2.Easting -= Hsin;
                        ytList[i] = arr2;
                    }

                }

                if (youTurnPhase > 0)
                {
                    for (int j = 0; j < cnt; j += 2)
                    {
                        if (!mf.bnd.bndArr[0].IsPointInTurnWorkArea(ytList[j])) isOutOfBounds = true;
                        if (isOutOfBounds) break;

                        for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                        {
                            //make sure not inside a non drivethru boundary
                            if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                            if (mf.bnd.bndArr[i].IsPointInTurnWorkArea(ytList[j]))
                            {
                                isOutOfBounds = true;
                                break;
                            }
                        }
                        if (isOutOfBounds) break;
                    }

                    if (!isOutOfBounds)
                    {
                        if (mf.Guidance.GuidanceOffset != 0)
                        {
                            if (isTurnRight)
                            {
                                for (int i = 0; i < cnt; i++)
                                {
                                    arr2 = ytList[i];
                                    arr2.Northing += Hcos * -mf.Guidance.GuidanceOffset;
                                    arr2.Easting += Hsin * -mf.Guidance.GuidanceOffset;
                                    ytList[i] = arr2;
                                }
                            }
                        }

                        youTurnPhase = 3;
                    }
                    else
                    {
                        youTurnPhase = 1;
                        //turn keeps approaching vehicle and running out of space - end of field?
                        if (isOutOfBounds && mf.distancePivotToTurnLine > 3)
                        {
                            isTurnCreationTooClose = false;
                        }
                        else
                        {
                            isTurnCreationTooClose = true;
                        }
                    }
                }
                return true;
            }
            return false;
        }

        public bool BuildABLinePatternYouTurn(bool isTurnRight)
        {
            if (mf.ABLines.CurrentLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentLine > -1)
            {
                double headAB = mf.ABLines.ABLines[mf.ABLines.CurrentLine].Heading;
                if (!mf.ABLines.isSameWay) headAB += Math.PI;

                //grab the pure pursuit point right on ABLine
                Vec3 onPurePoint = new Vec3(mf.rNorth, mf.rEast, 0);

                //how far are we from any turn boundary
                mf.turn.FindClosestTurnPoint(isYouTurnRight, onPurePoint, headAB);

                //or did we lose the turnLine - we are on the highway cuz we left the outer/inner turn boundary
                if ((int)mf.turn.closestTurnPt.Easting != -20000)
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
                rEastYT = mf.rEast;
                rNorthYT = mf.rNorth;

                //grab the vehicle widths and offsets
                double toolOffset = mf.Guidance.GuidanceOffset * 2.0;

                double turnOffset = ((mf.Guidance.WidthMinusOverlap * rowSkipsWidth) + (isTurnRight ? toolOffset: -toolOffset));

                //Pattern Turn
                numShapePoints = youFileList.Count;

                if (numShapePoints < 2) return false;

                Vec3[] pt = new Vec3[numShapePoints];

                //Now put the shape into an array since lists are immutable
                for (int i = 0; i < numShapePoints; i++)
                {
                    pt[i].Easting = youFileList[i].Easting;
                    pt[i].Northing = youFileList[i].Northing;
                }

                //start of path on the origin. Mirror the shape if left turn
                if (!isTurnRight)
                {
                    for (int i = 0; i < pt.Length; i++) pt[i].Easting *= -1;
                }

                //scaling - Drawing is 10m wide so find ratio of tool width
                double scale = turnOffset * 0.1;
                for (int i = 0; i < pt.Length; i++)
                {
                    pt[i].Easting *= scale;
                    pt[i].Northing *= scale;
                }

                double _turnDiagDistance = mf.distancePivotToTurnLine - distanceTurnBeforeLine;

                //move the start forward
                if (youTurnPhase < 2)
                {
                    rEastYT += (Math.Sin(headAB) * (_turnDiagDistance - turnOffset));
                    rNorthYT += (Math.Cos(headAB) * (_turnDiagDistance - turnOffset));
                }
                else
                {
                    _turnDiagDistance -= 2;
                    turnDistanceAdjuster += 5;
                    rEastYT += (Math.Sin(headAB) * (_turnDiagDistance - turnOffset));
                    rNorthYT += (Math.Cos(headAB) * (_turnDiagDistance - turnOffset));
                    youTurnPhase = 3;
                }

                //rotate pattern to match AB Line heading
                double xr, yr, xr2, yr2;
                for (int i = 0; i < pt.Length - 1; i++)
                {
                    xr = (Math.Cos(-headAB) * pt[i].Easting) - (Math.Sin(-headAB) * pt[i].Northing) + rEastYT;
                    yr = (Math.Sin(-headAB) * pt[i].Easting) + (Math.Cos(-headAB) * pt[i].Northing) + rNorthYT;

                    xr2 = (Math.Cos(-headAB) * pt[i + 1].Easting) - (Math.Sin(-headAB) * pt[i + 1].Northing) + rEastYT;
                    yr2 = (Math.Sin(-headAB) * pt[i + 1].Easting) + (Math.Cos(-headAB) * pt[i + 1].Northing) + rNorthYT;

                    pt[i].Easting = xr;
                    pt[i].Northing = yr;
                    pt[i].Heading = Math.Atan2(xr2 - xr, yr2 - yr);
                    if (pt[i].Heading < 0) pt[i].Heading += Glm.twoPI;
                    ytList.Add(pt[i]);
                }
                xr = (Math.Cos(-headAB) * pt[pt.Length - 1].Easting) - (Math.Sin(-headAB) * pt[pt.Length - 1].Northing) + rEastYT;
                yr = (Math.Sin(-headAB) * pt[pt.Length - 1].Easting) + (Math.Cos(-headAB) * pt[pt.Length - 1].Northing) + rNorthYT;

                pt[pt.Length - 1].Easting = xr;
                pt[pt.Length - 1].Northing = yr;
                pt[pt.Length - 1].Heading = pt[pt.Length - 2].Heading;
                ytList.Add(pt[pt.Length - 1]);

                //pattern all made now is it outside a boundary
                //now check to make sure we are not in an inner turn boundary - drive thru is ok
                int count = ytList.Count;
                if (count == 0) return false;
                isOutOfBounds = false;

                headAB += Math.PI;

                Vec3 ptt;
                for (int a = 0; a < youTurnStartOffset; a++)
                {
                    ptt.Easting = ytList[0].Easting + (Math.Sin(headAB));
                    ptt.Northing = ytList[0].Northing + (Math.Cos(headAB));
                    ptt.Heading = ytList[0].Heading;
                    ytList.Insert(0, ptt);
                }

                count = ytList.Count;

                for (int i = 1; i <= youTurnStartOffset; i++)
                {
                    ptt.Easting = ytList[count - 1].Easting + (Math.Sin(headAB) * i);
                    ptt.Northing = ytList[count - 1].Northing + (Math.Cos(headAB) * i);
                    ptt.Heading = ytList[count - 1].Heading;
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
                        break;
                    }
                }

                // Phase 0 - back up the turn till it is out of bounds.
                // Phase 1 - move it forward till out of bounds.
                // Phase 2 - move forward couple meters away from turn line.

                for (int j = 0; j < count; j += 2)
                {
                    if (!mf.bnd.bndArr[0].IsPointInTurnWorkArea(ytList[j])) isOutOfBounds = true;
                    if (isOutOfBounds) break;

                    for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                    {
                        //make sure not inside a non drivethru boundary
                        if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                        if (mf.bnd.bndArr[i].IsPointInTurnWorkArea(ytList[j]))
                        {
                            isOutOfBounds = true;
                            break;
                        }
                    }
                    if (isOutOfBounds) break;
                }

                if (youTurnPhase == 0)
                {
                    turnDistanceAdjuster -= 2;
                    if (isOutOfBounds) youTurnPhase = 1;
                }
                else
                {
                    if (!isOutOfBounds)
                    {
                        youTurnPhase = 3;
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
                        }
                    }
                }
                return isOutOfBounds;
            }
            return false;
        }

        public bool BuildCurvePatternYouTurn(bool isTurnRight, Vec3 pivotPos)
        {
            if (youTurnPhase > 0)
            {
                ytList.Clear();

                double head = crossingCurvePoint.Heading;
                if (!mf.CurveLines.isSameWay) head += Math.PI;

                //are we going same way as creation of curve
                //bool isCountingUp = mf.curve.isABSameAsVehicleHeading;

                double toolOffset = mf.Guidance.GuidanceOffset * 2.0;
                double turnOffset;

                //turning right
                if (isTurnRight) turnOffset = (mf.Guidance.WidthMinusOverlap + toolOffset);
                else turnOffset = (mf.Guidance.WidthMinusOverlap - toolOffset);

                //Pattern Turn
                numShapePoints = youFileList.Count;
                Vec3[] pt = new Vec3[numShapePoints];

                //Now put the shape into an array since lists are immutable
                for (int i = 0; i < numShapePoints; i++)
                {
                    pt[i].Easting = youFileList[i].Easting;
                    pt[i].Northing = youFileList[i].Northing;
                }

                //start of path on the origin. Mirror the shape if left turn
                if (!isTurnRight)
                {
                    for (int i = 0; i < pt.Length; i++) pt[i].Easting *= -1;
                }

                //scaling - Drawing is 10m wide so find ratio of tool width
                double scale = turnOffset * 0.1;
                for (int i = 0; i < pt.Length; i++)
                {
                    pt[i].Easting *= scale * rowSkipsWidth;
                    pt[i].Northing *= scale * rowSkipsWidth;
                }

                //rotate pattern to match AB Line heading
                double xr, yr, xr2, yr2;
                for (int i = 0; i < pt.Length - 1; i++)
                {
                    xr = (Math.Cos(-head) * pt[i].Easting) - (Math.Sin(-head) * pt[i].Northing) + crossingCurvePoint.Easting;
                    yr = (Math.Sin(-head) * pt[i].Easting) + (Math.Cos(-head) * pt[i].Northing) + crossingCurvePoint.Northing;

                    xr2 = (Math.Cos(-head) * pt[i + 1].Easting) - (Math.Sin(-head) * pt[i + 1].Northing) + crossingCurvePoint.Easting;
                    yr2 = (Math.Sin(-head) * pt[i + 1].Easting) + (Math.Cos(-head) * pt[i + 1].Northing) + crossingCurvePoint.Northing;

                    pt[i].Easting = xr;
                    pt[i].Northing = yr;

                    pt[i].Heading = Math.Atan2(xr2 - xr, yr2 - yr);
                    if (pt[i].Heading < 0) pt[i].Heading += Glm.twoPI;
                    ytList.Add(pt[i]);
                }
                xr = (Math.Cos(-head) * pt[pt.Length - 1].Easting) - (Math.Sin(-head) * pt[pt.Length - 1].Northing) + crossingCurvePoint.Easting;
                yr = (Math.Sin(-head) * pt[pt.Length - 1].Easting) + (Math.Cos(-head) * pt[pt.Length - 1].Northing) + crossingCurvePoint.Northing;

                pt[pt.Length - 1].Easting = xr;
                pt[pt.Length - 1].Northing = yr;
                pt[pt.Length - 1].Heading = pt[pt.Length - 2].Heading;
                ytList.Add(pt[pt.Length - 1]);

                //pattern all made now is it outside a boundary
                head -= Math.PI;

                Vec3 ptt;
                for (int a = 0; a < youTurnStartOffset; a++)
                {
                    ptt.Easting = ytList[0].Easting + (Math.Sin(head));
                    ptt.Northing = ytList[0].Northing + (Math.Cos(head));
                    ptt.Heading = ytList[0].Heading;
                    ytList.Insert(0, ptt);
                }

                int count = ytList.Count;

                for (int i = 1; i <= youTurnStartOffset; i++)
                {
                    ptt.Easting = ytList[count - 1].Easting + (Math.Sin(head) * i);
                    ptt.Northing = ytList[count - 1].Northing + (Math.Cos(head) * i);
                    ptt.Heading = ytList[count - 1].Heading;
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
                        if (mf.CurveLines.Lines[mf.CurveLines.CurrentLine].SpiralMode || mf.CurveLines.Lines[mf.CurveLines.CurrentLine].CircleMode || mf.CurveLines.Lines[mf.CurveLines.CurrentLine].BoundaryMode)
                        {
                            isTurnCreationNotCrossingError = false;
                        }
                        else isTurnCreationNotCrossingError = true;
                    }
                    ytList.Clear();
                    break;

                case 1:
                    //now check to make sure turn is not in an inner turn boundary - drive thru is ok
                    int count = ytList.Count;
                    if (count == 0) return false;
                    isOutOfBounds = false;

                    //Out of bounds?
                    for (int j = 0; j < count; j += 2)
                    {
                        if (!mf.bnd.bndArr[0].IsPointInTurnWorkArea(ytList[j])) isOutOfBounds = true;
                        if (isOutOfBounds) break;

                        for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                        {
                            //make sure not inside a non drivethru boundary
                            if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                            if (mf.bnd.bndArr[i].IsPointInTurnWorkArea(ytList[j]))
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
                        return true;
                    }

                    //keep moving infield till pattern is all inside
                    if (mf.CurveLines.isSameWay)
                    {
                        crossingCurvePoint.Index--;
                        if (crossingCurvePoint.Index < 0) crossingCurvePoint.Index = 0;
                    }
                    else
                    {
                        crossingCurvePoint.Index++;
                        if (crossingCurvePoint.Index >= curListCount)
                            crossingCurvePoint.Index = curListCount - 1;
                    }

                    crossingCurvePoint.Easting = mf.CurveLines.curList[crossingCurvePoint.Index].Easting;
                    crossingCurvePoint.Northing = mf.CurveLines.curList[crossingCurvePoint.Index].Northing;
                    crossingCurvePoint.Heading = mf.CurveLines.curList[crossingCurvePoint.Index].Heading;

                    double tooClose = Glm.Distance(ytList[0], pivotPos);
                    isTurnCreationTooClose = tooClose < 3;
                    break;
            }
            return true;
        }

        public bool BuildCurveDubinsYouTurn(bool isTurnRight, Vec3 pivotPos)
        {
            double head = crossingCurvePoint.Heading;
            if (!mf.CurveLines.isSameWay) head += Math.PI;

            if (youTurnPhase == 1)
            {
                boundaryAngleOffPerpendicular =  (crossingTurnLinePoint.Heading - head) - Glm.PIBy2;
                if (boundaryAngleOffPerpendicular > Math.PI) boundaryAngleOffPerpendicular -= Glm.twoPI;
                if (boundaryAngleOffPerpendicular < -Math.PI) boundaryAngleOffPerpendicular += Glm.twoPI;

                CDubins dubYouTurnPath = new CDubins();
                CDubins.turningRadius = mf.vehicle.minTurningRadius;

                double turnOffset = (mf.Guidance.WidthMinusOverlap * rowSkipsWidth) - (isTurnRight ? -mf.Guidance.GuidanceOffset * 2.0 : mf.Guidance.GuidanceOffset * 2.0);


                double turnRadius = turnOffset * Math.Tan(boundaryAngleOffPerpendicular);
                if (Math.Abs(turnRadius) > 100) turnRadius = 0;
                if (turnOffset < mf.vehicle.minTurningRadius * 2) turnRadius = 0;

                var start = new Vec3(crossingCurvePoint.Northing, crossingCurvePoint.Easting, head);

                var goal = new Vec3();
                if (isTurnRight)
                {
                    goal.Northing = crossingCurvePoint.Northing + Math.Sin(head) * -turnOffset + Math.Cos(head) * -turnRadius;
                    goal.Easting = crossingCurvePoint.Easting + Math.Cos(head) * turnOffset + Math.Sin(head) * -turnRadius;
                    goal.Heading = head - Math.PI;
                }
                else
                {
                    goal.Northing = crossingCurvePoint.Northing + Math.Sin(head) * turnOffset + Math.Cos(head) * turnRadius;
                    goal.Easting = crossingCurvePoint.Easting + Math.Cos(head) * -turnOffset + Math.Sin(head) * turnRadius;
                    goal.Heading = head + Math.PI;
                }

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
                    ytList.Clear();
                    break;

                case 1:
                    //now check to make sure we are not in an inner turn boundary - drive thru is ok
                    int count = ytList.Count;
                    if (count == 0) return false;

                    //Are we out of bounds?
                    isOutOfBounds = false;
                    for (int j = 0; j < count; j += 2)
                    {
                        if (!mf.bnd.bndArr[0].IsPointInTurnWorkArea(ytList[j]))
                        {
                            isOutOfBounds = true;
                            break;
                        }

                        for (int i = 1; i < mf.bnd.bndArr.Count; i++)
                        {
                            //make sure not inside a non drivethru boundary
                            if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                            if (mf.bnd.bndArr[i].IsPointInTurnWorkArea(ytList[j]))
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
                        if (mf.Guidance.GuidanceOffset != 0)
                        {
                            double turnOffset = (mf.Guidance.WidthMinusOverlap * rowSkipsWidth) - (isTurnRight ? -mf.Guidance.GuidanceOffset * 2.0 : mf.Guidance.GuidanceOffset * 2.0);
                            if (turnOffset < 0) isTurnRight = !isTurnRight;
                            if (isTurnRight)
                            {
                                double cosHead = Math.Cos(crossingCurvePoint.Heading);
                                double sinHead = Math.Sin(crossingCurvePoint.Heading);

                                for (int i = 0; i < count; i++)
                                {
                                    Vec3 arr2 = ytList[i];
                                    arr2.Northing += cosHead * mf.Guidance.GuidanceOffset;
                                    arr2.Easting += sinHead * mf.Guidance.GuidanceOffset;
                                    ytList[i] = arr2;
                                }
                            }
                        }

                        youTurnPhase = 3;
                        return true;
                    }

                    //keep moving infield till pattern is all inside
                    if (mf.CurveLines.isSameWay)
                    {
                        crossingCurvePoint.Index--;
                        if (crossingCurvePoint.Index < 0) crossingCurvePoint.Index = 0;
                    }
                    else
                    {
                        crossingCurvePoint.Index++;
                        if (crossingCurvePoint.Index >= curListCount)
                            crossingCurvePoint.Index = curListCount - 1;
                    }
                    crossingCurvePoint.Easting = mf.CurveLines.curList[crossingCurvePoint.Index].Easting;
                    crossingCurvePoint.Northing = mf.CurveLines.curList[crossingCurvePoint.Index].Northing;
                    crossingCurvePoint.Heading = mf.CurveLines.curList[crossingCurvePoint.Index].Heading;

                    double tooClose = Glm.Distance(ytList[0], pivotPos);
                    isTurnCreationTooClose = tooClose < 3;
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

            if (SwapYouTurn)
            {
                if (mf.CurveLines.isSameWay)
                {
                    if (isYouTurnRight) mf.CurveLines.HowManyPathsAway += rowSkipsWidth;
                    else mf.CurveLines.HowManyPathsAway -= rowSkipsWidth;
                }
                else
                {
                    if (isYouTurnRight) mf.CurveLines.HowManyPathsAway -= rowSkipsWidth;
                    else mf.CurveLines.HowManyPathsAway += rowSkipsWidth;
                }
                mf.CurveLines.isSameWay = !mf.CurveLines.isSameWay;

                if (mf.ABLines.isSameWay)
                {
                    if (isYouTurnRight) mf.ABLines.HowManyPathsAway += rowSkipsWidth;
                    else mf.ABLines.HowManyPathsAway -= rowSkipsWidth;
                }
                else
                {
                    if (isYouTurnRight) mf.ABLines.HowManyPathsAway -= rowSkipsWidth;
                    else mf.ABLines.HowManyPathsAway += rowSkipsWidth;
                }
                mf.ABLines.isSameWay = !mf.CurveLines.isSameWay;
            }
        }

        //Normal copmpletion of youturn
        public void CompleteYouTurn()
        {
            //just do the opposite of last turn
            if (SwapYouTurn) isYouTurnRight = !isYouTurnRight;
            SwapYouTurn = true;
            isYouTurnTriggered = false;
            ResetCreatedYouTurn();
            mf.seq.ResetSequenceEventTriggers();
            mf.seq.isSequenceTriggered = false;
            mf.isBoundAlarming = false;
        }

        //something went seriously wrong so reset everything
        public void ResetYouTurn()
        {
            //fix you turn
            SwapYouTurn = true;
            isYouTurnTriggered = false;
            ResetCreatedYouTurn();
            mf.isBoundAlarming = false;
            isTurnCreationTooClose = false;
            isTurnCreationNotCrossingError = false;

            //reset sequence
            mf.seq.ResetSequenceEventTriggers();
            mf.seq.isSequenceTriggered = false;
        }

        public void ResetCreatedYouTurn()
        {
            SwapYouTurn = true;
            turnDistanceAdjuster = 0;
            youTurnPhase = -1;
            ytList.Clear();
        }

        //get list of points from txt shape file
        public void LoadYouTurnShapeFromData(string Data)
        {
            try
            {
                string[] Text = Data.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                youFileList.Clear();
                Vec2 coords = new Vec2();
                for (int v = 0; v < Text.Length; v++)
                {
                    string[] words = Text[v].Split(',');
                    coords.Easting = double.Parse(words[0], CultureInfo.InvariantCulture);
                    coords.Northing = double.Parse(words[1], CultureInfo.InvariantCulture);
                    youFileList.Add(coords);
                }
            }
            catch (Exception e)
            {
                mf.TimedMessageBox(2000, "YouTurn File is Corrupt", "But Field is Loaded");
                mf.WriteErrorLog("YouTurn File is Corrupt " + e);
            }
        }

        //Resets the drawn YOuTurn and set diagPhase to 0

        //build the points and path of youturn to be scaled and transformed
        public void BuildManualYouTurn(bool isTurnRight, bool isTurnButtonTriggered)
        {
            isYouTurnTriggered = true;
            bool TurnAround;
            double head;
            //point on AB line closest to pivot axle point from ABLine PurePursuit
            if (mf.ABLines.BtnABLineOn && mf.ABLines.CurrentLine < mf.ABLines.ABLines.Count && mf.ABLines.CurrentLine > -1)
            {
                rEastYT = mf.rEast;
                rNorthYT = mf.rNorth;
                TurnAround = mf.ABLines.isSameWay;
                head = mf.ABLines.ABLines[mf.ABLines.CurrentLine].Heading;

                if (mf.ABLines.isSameWay)
                {
                    if (isTurnRight) mf.ABLines.HowManyPathsAway += rowSkipsWidth;
                    else mf.ABLines.HowManyPathsAway -= rowSkipsWidth;
                }
                else
                {
                    if (isTurnRight) mf.ABLines.HowManyPathsAway -= rowSkipsWidth;
                    else mf.ABLines.HowManyPathsAway += rowSkipsWidth;
                }
                mf.ABLines.isSameWay = !mf.ABLines.isSameWay;
            }
            else
            {
                if (mf.CurveLines.curList.Count < 2) return;
                if (mf.CurveLines.isSameWay)
                {
                    if (isTurnRight) mf.CurveLines.HowManyPathsAway += rowSkipsWidth;
                    else mf.CurveLines.HowManyPathsAway -= rowSkipsWidth;
                }
                else
                {
                    if (isTurnRight) mf.CurveLines.HowManyPathsAway -= rowSkipsWidth;
                    else mf.CurveLines.HowManyPathsAway += rowSkipsWidth;
                }
                TurnAround = mf.CurveLines.isSameWay;
                mf.CurveLines.isSameWay = !mf.CurveLines.isSameWay;


                rEastYT = mf.rEast;
                rNorthYT = mf.rNorth;
                head = mf.CurveLines.curList[mf.currentLocationIndex].Heading;//set to curent line heading ;)
            }

            double toolOffset = mf.Guidance.GuidanceOffset * 2.0;
            double turnOffset;

            //turning right
            if (isTurnRight) turnOffset = mf.Guidance.WidthMinusOverlap * rowSkipsWidth + toolOffset;
            else turnOffset = mf.Guidance.WidthMinusOverlap * rowSkipsWidth - toolOffset;

            CDubins dubYouTurnPath = new CDubins();
            CDubins.turningRadius = mf.vehicle.minTurningRadius;

            //if its straight across it makes 2 loops instead so goal is a little lower then start
            if (!TurnAround) head += 3.14;
            else head -= 0.01;

            //move the start forward 2 meters, this point is critical to formation of uturn
            rEastYT += (Math.Sin(head) * 2);
            rNorthYT += (Math.Cos(head) * 2);

            //now we have our start point
            var start = new Vec3(rNorthYT, rEastYT, head);
            var goal = new Vec3();


            //now we go the other way to turn round
            head -= Math.PI;
            if (head < 0) head += Glm.twoPI;

            //set up the goal point for Dubins
            goal.Heading = head;
            if (isTurnButtonTriggered)
            {
                if (isTurnRight)
                {
                    goal.Easting = rEastYT - (Math.Cos(-head) * turnOffset);
                    goal.Northing = rNorthYT - (Math.Sin(-head) * turnOffset);
                }
                else
                {
                    goal.Easting = rEastYT + (Math.Cos(-head) * turnOffset);
                    goal.Northing = rNorthYT + (Math.Sin(-head) * turnOffset);
                }
            }

            //generate the turn points
            ytList = dubYouTurnPath.GenerateDubins(start, goal);

            Vec3 pt;
            for (int a = 0; a < 3; a++)
            {
                pt.Easting = ytList[0].Easting + (Math.Sin(head));
                pt.Northing = ytList[0].Northing + (Math.Cos(head));
                pt.Heading = ytList[0].Heading;
                ytList.Insert(0, pt);
            }

            int count = ytList.Count;

            for (int i = 1; i <= 7; i++)
            {
                pt.Easting = ytList[count - 1].Easting + (Math.Sin(head) * i);
                pt.Northing = ytList[count - 1].Northing + (Math.Cos(head) * i);
                pt.Heading = head;
                ytList.Add(pt);
            }
        }


        public void DrawYouTurn()
        {
            int ptCount = ytList.Count;
            if (ptCount < 2) return;
            GL.PointSize(mf.ABLines.lineWidth);

            if (isYouTurnTriggered) GL.Color3(0.95f, 0.95f, 0.25f);
            else if (isOutOfBounds) GL.Color3(0.9495f, 0.395f, 0.325f);
            else GL.Color3(0.395f, 0.925f, 0.30f);

            GL.Begin(PrimitiveType.Points);
            for (int i = 0; i < ptCount; i++)
            {
                GL.Vertex3(ytList[i].Easting, ytList[i].Northing, 0);
            }
            GL.End();
        }
    }
}