using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CGeoFence
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        /// <summary>
        /// array of turns
        /// </summary>
        public List<CGeoFenceLines> geoFenceArr = new List<CGeoFenceLines>();

        //constructor
        public CGeoFence(FormGPS _f)
        {
            mf = _f;
        }


        public void FindPointsDriveAround(Vec3 fromPt, double headAB, ref Vec3 start, ref Vec3 stop)
        {
            //initial scan is straight ahead of pivot point of vehicle to find the right turnLine/boundary
            Vec3 pt = new Vec3();

            bool isFound = false;
            int closestTurnNum = 99;
            int closestTurnIndex = 99999;
            int mazeDim = mf.mazeGrid.mazeColXDim * mf.mazeGrid.mazeRowYDim;

            double cosHead = Math.Cos(headAB);
            double sinHead = Math.Sin(headAB);

            for (int b = 1; b < mf.maxCrossFieldLength; b += 2)
            {
                pt.easting = fromPt.easting + (sinHead * b);
                pt.northing = fromPt.northing + (cosHead * b);

                if (mf.turn.turnArr[0].IsPointInTurnWorkArea(pt))
                {
                    for (int t = 1; t < mf.bnd.bndArr.Count; t++)
                    {
                        if (mf.bnd.bndArr[t].isDriveThru) continue;

                        if (mf.bnd.bndArr[t].isDriveAround)
                        {
                            if (mf.gf.geoFenceArr[t].IsPointInGeoFenceArea(pt))
                            {
                                isFound = true;
                                closestTurnNum = t;
                                closestTurnIndex = b;

                                start.easting = fromPt.easting + (sinHead * b);
                                start.northing = fromPt.northing + (cosHead * b);
                                start.heading = headAB;
                                break;
                            }
                        }
                        else
                        {
                            //its a uturn obstacle
                            if (mf.turn.turnArr[t].IsPointInTurnWorkArea(pt))
                            {
                                start.easting = 88888;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    start.easting = 88888;
                    return;
                }
                if (isFound) break;
            }

            isFound = false;

            for (int b = closestTurnIndex + 200; b > closestTurnIndex; b--)
            {
                pt.easting = fromPt.easting + (sinHead * b);
                pt.northing = fromPt.northing + (cosHead * b);

                if (mf.gf.geoFenceArr[closestTurnNum].IsPointInGeoFenceArea(pt))
                {
                    isFound = true;

                    stop.easting = fromPt.easting + (sinHead * b);
                    stop.northing = fromPt.northing + (cosHead * b);
                    stop.heading = headAB;
                }

                if (isFound) break;
            }

            for (int i = 0; i < 30; i++)
            {
                start.easting -= sinHead;
                start.northing -= cosHead;
                start.heading = headAB;

                int iStart = (int)((((int)((start.northing - mf.minFieldY) / mf.mazeGrid.mazeScale)) * mf.mazeGrid.mazeColXDim)
                    + (int)((start.easting - mf.minFieldX) / mf.mazeGrid.mazeScale));

                if (iStart >= mazeDim)
                {
                    start.easting = 88888;
                    return;
                }

                if (mf.mazeGrid.mazeArr[iStart] == 0) break;
            }

            for (int i = 0; i < 30; i++)
            {
                stop.easting += sinHead;
                stop.northing += cosHead;
                stop.heading = headAB;

                int iStop = (int)((((int)((stop.northing - mf.minFieldY) / mf.mazeGrid.mazeScale)) * mf.mazeGrid.mazeColXDim)
                    + (int)((stop.easting - mf.minFieldX) / mf.mazeGrid.mazeScale));

                if (iStop >= mazeDim)
                {
                    start.easting = 88888;
                    return;
                }

                if (mf.mazeGrid.mazeArr[iStop] == 0) break;
            }
        }

        public bool IsPointInsideGeoFences(Vec3 pt)
        {
            //if inside outer boundary, then potentially add
            if (geoFenceArr.Count > 0 && geoFenceArr[0].IsPointInGeoFenceArea(pt))
            {
                for (int b = 1; b < mf.bnd.bndArr.Count; b++)
                {
                    if (geoFenceArr[b].IsPointInGeoFenceArea(pt))
                    {
                        //point is in an inner turn area but inside outer
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool IsPointInsideGeoFences(Vec2 pt)
        {
            //if inside outer boundary, then potentially add
            if (geoFenceArr.Count > 0 && geoFenceArr[0].IsPointInGeoFenceArea(pt))
            {
                for (int b = 1; b < mf.bnd.bndArr.Count; b++)
                {
                    if (geoFenceArr[b].IsPointInGeoFenceArea(pt))
                    {
                        //point is in an inner turn area but inside outer
                        return false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }

        public void BuildGeoFenceLines(int Num)
        {
            if (mf.bnd.bndArr.Count == 0)
            {
                return;
            }

            //to fill the list of line points
            Vec3 point = new Vec3();

            //inside boundaries
            for (int j = (Num < 0) ? 0 : Num; j < mf.bnd.bndArr.Count; j++)
            {
                geoFenceArr[j].geoFenceLine.Clear();

                int ChangeDirection = j == 0 ? 1 : -1;

                int ptCount = mf.bnd.bndArr[j].bndLine.Count;

                for (int i = ptCount - 1; i >= 0; i--)
                {
                    //calculate the point outside the boundary
                    point.easting = mf.bnd.bndArr[j].bndLine[i].easting + (-Math.Sin(Glm.PIBy2 + mf.bnd.bndArr[j].bndLine[i].heading) * mf.yt.geoFenceDistance * ChangeDirection);
                    point.northing = mf.bnd.bndArr[j].bndLine[i].northing + (-Math.Cos(Glm.PIBy2 + mf.bnd.bndArr[j].bndLine[i].heading) * mf.yt.geoFenceDistance * ChangeDirection);
                    point.heading = mf.bnd.bndArr[j].bndLine[i].heading;

                    //only add if outside actual field boundary
                    if ((j == 0 && mf.bnd.bndArr[j].IsPointInsideBoundary(point)) || (j != 0 && !mf.bnd.bndArr[j].IsPointInsideBoundary(point)))
                    {
                        Vec2 tPnt = new Vec2(point.easting, point.northing);
                        geoFenceArr[j].geoFenceLine.Add(tPnt);
                    }
                }
                geoFenceArr[j].FixGeoFenceLine(mf.yt.geoFenceDistance, mf.bnd.bndArr[j].bndLine, mf.Tools[0].ToolWidth * 0.5);
                geoFenceArr[j].PreCalcTurnLines();

                if (Num > -1) break;
            }
        }

        public void DrawGeoFenceLines()
        {
            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                geoFenceArr[i].DrawGeoFenceLine();
            }
        }
    }
}