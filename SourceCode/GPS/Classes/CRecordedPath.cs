using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CRecPathPt
    {
        public double Easting { get; set; }
        public double Northing { get; set; }
        public double Heading { get; set; }
        public double Speed { get; set; }
        public bool AutoBtnState { get; set; }

        //constructor
        public CRecPathPt(double _easting, double _northing, double _heading, double _speed,
                            bool _autoBtnState)
        {
            Easting = _easting;
            Northing = _northing;
            Heading = _heading;
            Speed = _speed;
            AutoBtnState = _autoBtnState;
        }
    }

    public class CRecordedPath
    {
        //constructor
        public CRecordedPath(FormGPS _f)
        {
            mf = _f;
        }

        //pointers to mainform controls
        private readonly FormGPS mf;

        //the recorded path from driving around
        public List<CRecPathPt> recList = new List<CRecPathPt>();

        public int recListCount;

        //the dubins path to get there
        public List<CRecPathPt> shuttleDubinsList = new List<CRecPathPt>();

        public int shuttleListCount;

        public List<Vec3> mazeList = new List<Vec3>();

        //list of vec3 points of Dubins shortest path between 2 points - To be converted to RecPt
        public List<Vec3> shortestDubinsList = new List<Vec3>();

        //generated reference line
        public Vec2 refPoint1 = new Vec2(1, 1), refPoint2 = new Vec2(2, 2);

        public double distanceFromRefLine, distanceFromCurrentLine, refLineSide = 1.0;
        private int A, B, C;
        public double abFixHeadingDelta, abHeading;
        public bool isABSameAsVehicleHeading = true, isOnRightSideCurrentLine = true;

        public int lastPointFound = -1, currentPositonIndex;

        //pure pursuit values
        public Vec3 steerAxlePosRP = new Vec3(0, 0, 0);

        public Vec3 homePos = new Vec3();
        public Vec2 goalPointRP = new Vec2(0, 0);
        public double steerAngleRP, rEastRP, rNorthRP;

        public bool isBtnFollowOn, isEndOfTheRecLine, isRecordOn;
        public bool isDrivingRecordedPath, isPausedDrivingRecordedPath, isFollowingDubinsToPath, isFollowingRecPath, isFollowingDubinsHome;

        public bool StartDrivingRecordedPath()
        {
            //create the dubins path based on start and goal to start of recorded path
            A = B = C = 0;
            recListCount = recList.Count;
            if (recListCount < 5) return false;

            //the goal is the first point of path, the start is the current position
            Vec3 goal = new Vec3(recList[0].Easting, recList[0].Northing, recList[0].Heading);

            //save a copy of where we started.
            homePos = mf.pivotAxlePos;

            //get the dubins for approach to recorded path
            GetDubinsPath(goal);
            shuttleListCount = shuttleDubinsList.Count;

            //has a valid dubins path been created?
            if (shuttleListCount == 0) return false;

            //technically all good if we get here so set all the flags
            isFollowingDubinsHome = false;
            isFollowingRecPath = false;
            isFollowingDubinsToPath = true;
            isEndOfTheRecLine = false;
            currentPositonIndex = 0;
            isDrivingRecordedPath = true;

            isPausedDrivingRecordedPath = false;
            return true;
        }

        public bool trig;
        public double north;
        public int pathCount = 0;

        public void UpdatePosition()
        {
            if (isFollowingDubinsToPath)
            {
                //set a speed of 10 kmh
                mf.sim.stepDistance = shuttleDubinsList[C].Speed / 17.86;

                steerAxlePosRP = mf.steerAxlePos;

                StanleyDubinsPath(shuttleListCount);

                //check if close to recorded path
                int cnt = shuttleDubinsList.Count;
                pathCount = cnt - B;
                if (pathCount < 8)
                {
                    double distSqr = Glm.DistanceSquared(steerAxlePosRP.northing, steerAxlePosRP.easting, recList[0].Northing, recList[0].Easting);
                    if (distSqr < 2)
                    {
                        isFollowingRecPath = true;
                        isFollowingDubinsToPath = false;
                        shuttleDubinsList.Clear();
                        shortestDubinsList.Clear();
                    }
                }
            }

            if (isFollowingRecPath)
            {
                steerAxlePosRP = mf.steerAxlePos;

                StanleyRecPath(recListCount);

                //if end of the line then stop
                if (!isEndOfTheRecLine)
                {
                    mf.sim.stepDistance = recList[C].Speed / 17.86;
                    north = recList[C].Northing;

                    pathCount = recList.Count - C;

                    //section control - only if different click the button
                    bool autoBtn = (mf.autoBtnState == FormGPS.btnStates.Auto);
                    trig = autoBtn;
                    if (autoBtn != recList[C].AutoBtnState) mf.btnAutoSection.PerformClick();
                }
                else
                {
                    //create the dubins path based on start and goal to start trip home
                    GetDubinsPath(homePos);
                    shuttleListCount = shuttleDubinsList.Count;

                    //its too small
                    if (shuttleListCount < 3)
                    {
                        StopDrivingRecordedPath();
                        return;
                    }

                    //set all the flags
                    isFollowingDubinsHome = true;
                    A = B = C = 0;
                    isFollowingRecPath = false;
                    isFollowingDubinsToPath = false;
                    isEndOfTheRecLine = false;
                }
            }

            if (isFollowingDubinsHome)
            {
                int cnt = shuttleDubinsList.Count;
                pathCount = cnt - B;
                if (pathCount < 3)
                {
                    StopDrivingRecordedPath();
                    return;
                }

                mf.sim.stepDistance = shuttleDubinsList[C].Speed / 17.86;
                steerAxlePosRP = mf.steerAxlePos;

                StanleyDubinsPath(shuttleListCount);
            }

            //if paused, set the sim to 0
            if (isPausedDrivingRecordedPath) mf.sim.stepDistance = 0;
        }

        public void StopDrivingRecordedPath()
        {
            isFollowingDubinsHome = false;
            isFollowingRecPath = false;
            isFollowingDubinsToPath = false;
            shuttleDubinsList.Clear();
            shortestDubinsList.Clear();
            mf.sim.stepDistance = 0;
            isDrivingRecordedPath = false;
            mf.goPathMenu.Image = Properties.Resources.AutoGo;
            isPausedDrivingRecordedPath = false;
        }

        private void GetDubinsPath(Vec3 goal)
        {
            CDubins.turningRadius = mf.vehicle.minTurningRadius * 1.0;
            CDubins dubPath = new CDubins();

            // current psition
            steerAxlePosRP = mf.pivotAxlePos;

            //bump it forward
            Vec3 pt2 = new Vec3
            {
                easting = steerAxlePosRP.easting + (Math.Sin(steerAxlePosRP.heading) * 3),
                northing = steerAxlePosRP.northing + (Math.Cos(steerAxlePosRP.heading) * 3),
                heading = steerAxlePosRP.heading
            };

            //get the dubins path vec3 point coordinates of turn
            shortestDubinsList.Clear();
            shuttleDubinsList.Clear();

            shortestDubinsList = dubPath.GenerateDubins(pt2, goal, mf.gf);

            //if Dubins returns 0 elements, there is an unavoidable blockage in the way.
            if (shortestDubinsList.Count > 0)
            {
                shortestDubinsList.Insert(0, mf.pivotAxlePos);

                //transfer point list to recPath class point style
                for (int i = 0; i < shortestDubinsList.Count; i++)
                {
                    CRecPathPt pt = new CRecPathPt(shortestDubinsList[i].easting, shortestDubinsList[i].northing, shortestDubinsList[i].heading, 9.0, false);
                    shuttleDubinsList.Add(pt);
                }
                return;
            }

            //find a path from start to goal - diagnostic, but also used later
            mazeList = mf.mazeGrid.SearchForPath(pt2, goal);

            //you can't get anywhere!
            if (mazeList == null) return;

            //start is navigateable - maybe
            int cnt = mazeList.Count;

            if (cnt > 0)
            {
                {
                    int turnRadius = (int)(3 * mf.vehicle.minTurningRadius);
                    if (cnt > 2 * turnRadius)
                    {
                        mazeList.RemoveRange(0, turnRadius);
                        cnt = mazeList.Count;
                        mazeList.RemoveRange(cnt - turnRadius, turnRadius);
                    }
                }

                shortestDubinsList = dubPath.GenerateDubins(pt2, mazeList[0], mf.gf);
                if (shortestDubinsList.Count > 0)
                {
                    for (int i = 0; i < shortestDubinsList.Count; i++)
                    {
                        CRecPathPt pt = new CRecPathPt(shortestDubinsList[i].easting, shortestDubinsList[i].northing, shortestDubinsList[i].heading, 10.0, false);
                        shuttleDubinsList.Add(pt);
                    }
                }
                else
                {
                    return; //unable to generate a dubins to the start
                }

                for (int i = 0; i < mazeList.Count; i++)
                {
                    CRecPathPt pt = new CRecPathPt(mazeList[i].easting, mazeList[i].northing, mazeList[i].heading, 15.0, false);
                    shuttleDubinsList.Add(pt);
                }

                shortestDubinsList = dubPath.GenerateDubins(mazeList[mazeList.Count - 1], goal, mf.gf);

                for (int i = 0; i < shortestDubinsList.Count; i++)
                {
                    CRecPathPt pt = new CRecPathPt(shortestDubinsList[i].easting, shortestDubinsList[i].northing, shortestDubinsList[i].heading, 11.0, false);
                    shuttleDubinsList.Add(pt);
                }
                return;
            }
        }

        private void StanleyRecPath(int ptCount)
        {
            //find the closest 2 points to current fix
            double minDistA = 9999999999;

            //set the search range close to current position
            int top = currentPositonIndex + 5;
            if (top > ptCount) top = ptCount;

            double dist;
            for (int t = currentPositonIndex; t < top; t++)
            {
                dist = ((steerAxlePosRP.easting - recList[t].Easting) * (steerAxlePosRP.easting - recList[t].Easting))
                                + ((steerAxlePosRP.northing - recList[t].Northing) * (steerAxlePosRP.northing - recList[t].Northing));
                if (dist < minDistA)
                {
                    minDistA = dist;
                    A = t;
                }
            }

            //Save the closest point
            C = A;

            //next point is the next in list
            B = A + 1;
            if (B == ptCount)
            {
                //don't go past the end of the list - "end of the line" trigger
                A--;
                B--;
                isEndOfTheRecLine = true;
            }

            //save current position
            currentPositonIndex = A;

            //get the distance from currently active AB line
            double dx = recList[B].Easting - recList[A].Easting;
            double dz = recList[B].Northing - recList[A].Northing;

            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

            abHeading = Math.Atan2(dx, dz);
            //abHeading = recList[A].heading;

            //how far from current AB Line is fix
            distanceFromCurrentLine =
                ((dz * steerAxlePosRP.easting) - (dx * steerAxlePosRP.northing) + (recList[B].Easting
                        * recList[A].Northing) - (recList[B].Northing * recList[A].Easting))
                            / Math.Sqrt((dz * dz) + (dx * dx));

            //are we on the right side or not
            isOnRightSideCurrentLine = distanceFromCurrentLine > 0;

            // calc point on ABLine closest to current position
            double U = (((steerAxlePosRP.easting - recList[A].Easting) * dx)
                        + ((steerAxlePosRP.northing - recList[A].Northing) * dz))
                        / ((dx * dx) + (dz * dz));

            rEastRP = recList[A].Easting + (U * dx);
            rNorthRP = recList[A].Northing + (U * dz);

            //the first part of stanley is to extract heading error
            double abFixHeadingDelta = (steerAxlePosRP.heading - abHeading);

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
            steerAngleRP = Math.Atan((distanceFromCurrentLine * mf.vehicle.stanleyGain) / ((mf.pn.speed * 0.277777) + 1));

            //clamp it to max 42 degrees
            if (steerAngleRP > 0.74) steerAngleRP = 0.74;
            if (steerAngleRP < -0.74) steerAngleRP = -0.74;

            //add them up and clamp to max in vehicle settings
            steerAngleRP = Glm.ToDegrees((steerAngleRP + abFixHeadingDelta) * -1.0);
            if (steerAngleRP < -mf.vehicle.maxSteerAngle) steerAngleRP = -mf.vehicle.maxSteerAngle;
            if (steerAngleRP > mf.vehicle.maxSteerAngle) steerAngleRP = mf.vehicle.maxSteerAngle;

            //Convert to millimeters and round properly to above/below .5
            distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);

            //every guidance method dumps into these that are used and sent everywhere, last one wins
            mf.guidanceLineDistanceOff = mf.distanceDisplay = (Int16)distanceFromCurrentLine;
            mf.guidanceLineSteerAngle = (Int16)(steerAngleRP * 100);
        }

        private void StanleyDubinsPath(int ptCount)
        {
            //distanceFromCurrentLine = 9999;
            //find the closest 2 points to current fix
            double minDistA = 9999999999;
            for (int t = 0; t < ptCount; t++)
            {
                double dist = ((steerAxlePosRP.easting - shuttleDubinsList[t].Easting) * (steerAxlePosRP.easting - shuttleDubinsList[t].Easting))
                                + ((steerAxlePosRP.northing - shuttleDubinsList[t].Northing) * (steerAxlePosRP.northing - shuttleDubinsList[t].Northing));
                if (dist < minDistA)
                {
                    minDistA = dist;
                    A = t;
                }
            }

            //save the closest point
            C = A;
            //next point is the next in list
            B = A + 1;
            if (B == ptCount) { A--; B--; }                //don't go past the end of the list - "end of the line" trigger

            //get the distance from currently active AB line
            //x2-x1
            double dx = shuttleDubinsList[B].Easting - shuttleDubinsList[A].Easting;
            //z2-z1
            double dz = shuttleDubinsList[B].Northing - shuttleDubinsList[A].Northing;

            if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

            //abHeading = Math.Atan2(dz, dx);
            abHeading = shuttleDubinsList[A].Heading;

            //how far from current AB Line is fix
            distanceFromCurrentLine = ((dz * steerAxlePosRP.easting) - (dx * steerAxlePosRP
                .northing) + (shuttleDubinsList[B].Easting
                        * shuttleDubinsList[A].Northing) - (shuttleDubinsList[B].Northing * shuttleDubinsList[A].Easting))
                            / Math.Sqrt((dz * dz) + (dx * dx));

            //are we on the right side or not
            isOnRightSideCurrentLine = distanceFromCurrentLine > 0;

            // calc point on ABLine closest to current position
            double U = (((steerAxlePosRP.easting - shuttleDubinsList[A].Easting) * dx)
                        + ((steerAxlePosRP.northing - shuttleDubinsList[A].Northing) * dz))
                        / ((dx * dx) + (dz * dz));

            rEastRP = shuttleDubinsList[A].Easting + (U * dx);
            rNorthRP = shuttleDubinsList[A].Northing + (U * dz);

            //the first part of stanley is to extract heading error
            double abFixHeadingDelta = (steerAxlePosRP.heading - abHeading);

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
            steerAngleRP = Math.Atan((distanceFromCurrentLine * mf.vehicle.stanleyGain) / ((mf.pn.speed * 0.277777) + 1));

            //clamp it to max 42 degrees
            if (steerAngleRP > 0.74) steerAngleRP = 0.74;
            if (steerAngleRP < -0.74) steerAngleRP = -0.74;

            //add them up and clamp to max in vehicle settings
            steerAngleRP = Glm.ToDegrees((steerAngleRP + abFixHeadingDelta) * -1.0);
            if (steerAngleRP < -mf.vehicle.maxSteerAngle) steerAngleRP = -mf.vehicle.maxSteerAngle;
            if (steerAngleRP > mf.vehicle.maxSteerAngle) steerAngleRP = mf.vehicle.maxSteerAngle;

            //Convert to millimeters and round properly to above/below .5
            distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);

            //every guidance method dumps into these that are used and sent everywhere, last one wins
            mf.guidanceLineDistanceOff = mf.distanceDisplay = (Int16)distanceFromCurrentLine;
            mf.guidanceLineSteerAngle = (Int16)(steerAngleRP * 100);
        }

        public void DrawRecordedLine()
        {
            int ptCount = recList.Count;
            if (ptCount < 1) return;
            GL.LineWidth(1);
            GL.Color3(0.98f, 0.92f, 0.460f);
            GL.Begin(PrimitiveType.LineStrip);
            for (int h = 0; h < ptCount; h++) GL.Vertex3(recList[h].Easting, recList[h].Northing, 0);
            GL.End();

            if (mf.isPureDisplayOn)
            {
                //Draw lookahead Point
                GL.PointSize(8.0f);
                GL.Begin(PrimitiveType.Points);

                //GL.Color(1.0f, 1.0f, 0.25f);
                //GL.Vertex(rEast, rNorth, 0.0);

                GL.Color3(1.0f, 0.5f, 0.95f);
                GL.Vertex3(rEastRP, rNorthRP, 0.0);
                GL.End();
                GL.PointSize(1.0f);
            }
        }

        public void DrawDubins()
        {
            if (shuttleDubinsList.Count > 1)
            {
                //GL.LineWidth(2);
                GL.PointSize(2);
                GL.Color3(0.298f, 0.96f, 0.2960f);
                GL.Begin(PrimitiveType.Points);
                for (int h = 0; h < shuttleDubinsList.Count; h++)
                    GL.Vertex3(shuttleDubinsList[h].Easting, shuttleDubinsList[h].Northing, 0);
                GL.End();
            }
        }
    }
}