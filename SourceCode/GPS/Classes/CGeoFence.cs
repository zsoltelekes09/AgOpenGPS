using System;

namespace AgOpenGPS
{
    public class CGeoFence
    {
        //copy of the mainform address
        private readonly FormGPS mf;

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
                pt.Easting = fromPt.Easting + (sinHead * b);
                pt.Northing = fromPt.Northing + (cosHead * b);

                if (mf.bnd.bndArr[0].IsPointInTurnWorkArea(pt))
                {
                    for (int t = 1; t < mf.bnd.bndArr.Count; t++)
                    {
                        if (mf.bnd.bndArr[t].isDriveThru) continue;

                        if (mf.bnd.bndArr[t].isDriveAround)
                        {
                            if (mf.bnd.bndArr[t].IsPointInGeoFenceArea(pt))
                            {
                                isFound = true;
                                closestTurnNum = t;
                                closestTurnIndex = b;

                                start.Easting = fromPt.Easting + (sinHead * b);
                                start.Northing = fromPt.Northing + (cosHead * b);
                                start.Heading = headAB;
                                break;
                            }
                        }
                        else
                        {
                            //its a uturn obstacle
                            if (mf.bnd.bndArr[0].IsPointInTurnWorkArea(pt))
                            {
                                start.Easting = 88888;
                                return;
                            }
                        }
                    }
                }
                else
                {
                    start.Easting = 88888;
                    return;
                }
                if (isFound) break;
            }
            if (closestTurnNum == 99) return;
            isFound = false;

            for (int b = closestTurnIndex + 200; b > closestTurnIndex; b--)
            {
                pt.Easting = fromPt.Easting + (sinHead * b);
                pt.Northing = fromPt.Northing + (cosHead * b);

                if (mf.bnd.bndArr[closestTurnNum].IsPointInGeoFenceArea(pt))
                {
                    isFound = true;

                    stop.Easting = fromPt.Easting + (sinHead * b);
                    stop.Northing = fromPt.Northing + (cosHead * b);
                    stop.Heading = headAB;
                }

                if (isFound) break;
            }

            for (int i = 0; i < 30; i++)
            {
                start.Easting -= sinHead;
                start.Northing -= cosHead;
                start.Heading = headAB;

                int iStart = (int)((((int)((start.Northing - mf.minFieldY) / mf.mazeGrid.mazeScale)) * mf.mazeGrid.mazeColXDim)
                    + (int)((start.Easting - mf.minFieldX) / mf.mazeGrid.mazeScale));

                if (iStart >= mazeDim)
                {
                    start.Easting = 88888;
                    return;
                }

                if (mf.mazeGrid.mazeArr[iStart] == false) break;
            }

            for (int i = 0; i < 30; i++)
            {
                stop.Easting += sinHead;
                stop.Northing += cosHead;
                stop.Heading = headAB;

                int iStop = (int)((((int)((stop.Northing - mf.minFieldY) / mf.mazeGrid.mazeScale)) * mf.mazeGrid.mazeColXDim)
                    + (int)((stop.Easting - mf.minFieldX) / mf.mazeGrid.mazeScale));

                if (iStop >= mazeDim)
                {
                    start.Easting = 88888;
                    return;
                }

                if (mf.mazeGrid.mazeArr[iStop] == false) break;
            }
        }

        public bool IsPointInsideGeoFences(Vec3 pt)
        {
            //if inside outer boundary, then potentially add
            if (mf.bnd.bndArr.Count > 0 && mf.bnd.bndArr[0].IsPointInGeoFenceArea(pt))
            {
                for (int b = 1; b < mf.bnd.bndArr.Count; b++)
                {
                    if (mf.bnd.bndArr[b].IsPointInGeoFenceArea(pt))
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
            if (mf.bnd.bndArr.Count > 0 && mf.bnd.bndArr[0].IsPointInGeoFenceArea(pt))
            {
                for (int b = 1; b < mf.bnd.bndArr.Count; b++)
                {
                    if (mf.bnd.bndArr[b].IsPointInGeoFenceArea(pt))
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

        public void DrawGeoFenceLines()
        {
            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                if (mf.bnd.bndArr[i].Eastingmin > mf.worldGrid.EastingMax || mf.bnd.bndArr[i].Eastingmax < mf.worldGrid.EastingMin) continue;
                if (mf.bnd.bndArr[i].Northingmin > mf.worldGrid.NorthingMax || mf.bnd.bndArr[i].Northingmax < mf.worldGrid.NorthingMin) continue;

                mf.bnd.bndArr[i].DrawGeoFence();
            }
        }
    }
}