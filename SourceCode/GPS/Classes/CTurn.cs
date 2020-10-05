using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CTurn
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        private readonly double boxLength = 2000;

        /// <summary>
        /// array of turns
        /// </summary>
        public List<CTurnLines> turnArr = new List<CTurnLines>();

        //constructor
        public CTurn(FormGPS _f)
        {
            mf = _f;
        }

        // the list of possible bounds points
        public List<Vec3> turnClosestList = new List<Vec3>();

        public int turnSelected = 0, closestTurnNum;

        //generated box for finding closest point
        public Vec2 boxA = new Vec2(9000, 9000), boxB = new Vec2(9002, 9000);

        public Vec2 boxC = new Vec2(9001, 9001), boxD = new Vec2(9003, 9002);

        //point at the farthest turn segment from pivotAxle
        public Vec3 closestTurnPt = new Vec3();

        public void FindClosestTurnPoint(bool isYouTurnRight, Vec3 fromPt, double headAB)
        {
            //initial scan is straight ahead of pivot point of vehicle to find the right turnLine/boundary

            Vec3 rayPt = new Vec3();

            int closestTurnNum = int.MinValue;


            double CosHead = Math.Cos(headAB);
            double SinHead = Math.Sin(headAB);

            List<Vec4> Crossings1 = new List<Vec4>();

            double s1_x, s1_y;

            s1_x = CosHead * mf.maxCrossFieldLength;
            s1_y = SinHead * mf.maxCrossFieldLength;

            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                if (mf.turn.turnArr.Count > i ) Crossings1.FindCrossingPoints(ref mf.turn.turnArr[i].turnLine, fromPt.Northing, fromPt.Easting, s1_x, s1_y, i);
            }

            if (Crossings1.Count > 0)
            {
                Crossings1.Sort((x, y) => x.Heading.CompareTo(y.Heading));

                rayPt.Easting = Crossings1[0].Easting;
                rayPt.Northing = Crossings1[0].Northing;
                closestTurnNum = Crossings1[0].Index;
            }

            //second scan is straight ahead of outside of tool based on turn direction
            double scanWidthL, scanWidthR;
            if (isYouTurnRight)// not sure why you qwant to do it a second time if none found ?
            {
                scanWidthL = 0;//mf.Guidance.GuidanceWidth * 0.25;
                scanWidthR = mf.Guidance.GuidanceWidth * 0.75;//mf.Guidance.GuidanceWidth * 0.5;
            }
            else
            {
                scanWidthL = -mf.Guidance.GuidanceWidth * 0.75;//-mf.Guidance.GuidanceWidth * 0.5;
                scanWidthR = 0;// -mf.Guidance.GuidanceWidth * 0.25;
            }

            boxA.Northing = fromPt.Northing + SinHead * -scanWidthL;
            boxA.Easting = fromPt.Easting + CosHead * scanWidthL;

            boxB.Northing = fromPt.Northing + SinHead * -scanWidthR;
            boxB.Easting = fromPt.Easting + CosHead * scanWidthR;

            boxC.Northing = boxB.Northing + CosHead * boxLength;
            boxC.Easting = boxB.Easting + SinHead * boxLength;

            boxD.Northing = boxA.Northing + CosHead * boxLength;
            boxD.Easting = boxA.Easting + SinHead * boxLength;

            //determine if point is inside bounding box of the 1 turn chosen above
            turnClosestList.Clear();
            if (closestTurnNum < 0) return;

            mf.turn.closestTurnNum = closestTurnNum;
            Vec3 inBox;

            int ptCount = turnArr[closestTurnNum].turnLine.Count;
            for (int p = 0; p < ptCount; p++)
            {
                if ((((boxB.Easting - boxA.Easting) * (turnArr[closestTurnNum].turnLine[p].Northing - boxA.Northing))
                        - ((boxB.Northing - boxA.Northing) * (turnArr[closestTurnNum].turnLine[p].Easting - boxA.Easting))) < 0) { continue; }

                if ((((boxD.Easting - boxC.Easting) * (turnArr[closestTurnNum].turnLine[p].Northing - boxC.Northing))
                        - ((boxD.Northing - boxC.Northing) * (turnArr[closestTurnNum].turnLine[p].Easting - boxC.Easting))) < 0) { continue; }

                if ((((boxC.Easting - boxB.Easting) * (turnArr[closestTurnNum].turnLine[p].Northing - boxB.Northing))
                        - ((boxC.Northing - boxB.Northing) * (turnArr[closestTurnNum].turnLine[p].Easting - boxB.Easting))) < 0) { continue; }

                if ((((boxA.Easting - boxD.Easting) * (turnArr[closestTurnNum].turnLine[p].Northing - boxD.Northing))
                        - ((boxA.Northing - boxD.Northing) * (turnArr[closestTurnNum].turnLine[p].Easting - boxD.Easting))) < 0) { continue; }

                //it's in the box, so add to list
                inBox.Easting = turnArr[closestTurnNum].turnLine[p].Easting;
                inBox.Northing = turnArr[closestTurnNum].turnLine[p].Northing;
                inBox.Heading = turnArr[closestTurnNum].turnLine[p].Heading;

                //which turn/headland is it from
                turnClosestList.Add(inBox);
            }

            //which of the points is closest
            ptCount = turnClosestList.Count;
            if (ptCount != 0)
            {
                double totalDist = 0.75 * Math.Sqrt(((fromPt.Easting - rayPt.Easting) * (fromPt.Easting - rayPt.Easting))
                + ((fromPt.Northing - rayPt.Northing) * (fromPt.Northing - rayPt.Northing)));

                //determine closest point
                double minDistance = 9999999;
                for (int i = 0; i < ptCount; i++)
                {
                    double dist = Math.Sqrt(((fromPt.Easting - turnClosestList[i].Easting) * (fromPt.Easting - turnClosestList[i].Easting))
                                    + ((fromPt.Northing - turnClosestList[i].Northing) * (fromPt.Northing - turnClosestList[i].Northing)));

                    if (minDistance >= dist && dist > totalDist)
                    {
                        minDistance = dist;
                        closestTurnPt.Easting = turnClosestList[i].Easting;
                        closestTurnPt.Northing = turnClosestList[i].Northing;
                        closestTurnPt.Heading = turnClosestList[i].Heading;
                    }
                }
                if (closestTurnPt.Heading < 0) closestTurnPt.Heading += Glm.twoPI;
            }
        }

        public void BuildTurnLines(int Num)
        {
            if (mf.bnd.bndArr.Count == 0) return;
            //to fill the list of line points
            Vec3 point = new Vec3();

            //inside boundaries
            for (int j = (Num < 0) ? 0 : Num; j < mf.bnd.bndArr.Count; j++)
            {
                turnArr[j].turnLine.Clear();

                int ChangeDirection = j == 0 ? 1 : -1;

                int ptCount = mf.bnd.bndArr[j].bndLine.Count;

                for (int i = 0; i < ptCount; i++)
                {
                    //calculate the point outside the boundary
                    point.Northing = mf.bnd.bndArr[j].bndLine[i].Northing + Math.Sin(mf.bnd.bndArr[j].bndLine[i].Heading) * (-mf.yt.triggerDistanceOffset * ChangeDirection);
                    point.Easting = mf.bnd.bndArr[j].bndLine[i].Easting + Math.Cos(mf.bnd.bndArr[j].bndLine[i].Heading) * (mf.yt.triggerDistanceOffset * ChangeDirection);
                    point.Heading = mf.bnd.bndArr[j].bndLine[i].Heading;
                    turnArr[j].turnLine.Add(point);
                }
                turnArr[j].FixTurnLine(mf.yt.triggerDistanceOffset, ref mf.bnd.bndArr[j].bndLine);

                turnArr[j].PreCalcTurnLines();

                if (Num > -1) break;
            }
        }

        public void DrawTurnLines()
        {
            GL.LineWidth(mf.ABLines.lineWidth);
            GL.Color3(0.3555f, 0.6232f, 0.20f);
            //GL.PointSize(2);

            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                if (mf.bnd.bndArr[i].isDriveAround || mf.bnd.bndArr[i].isDriveThru) continue;

                ////draw the turn line oject
                int ptCount = mf.turn.turnArr[i].turnLine.Count;
                if (ptCount < 1) return;

                if (mf.bnd.bndArr[i].Northingmin > mf.worldGrid.NorthingMax || mf.bnd.bndArr[i].Northingmax < mf.worldGrid.NorthingMin) continue;
                if (mf.bnd.bndArr[i].Eastingmin > mf.worldGrid.EastingMax || mf.bnd.bndArr[i].Eastingmax < mf.worldGrid.EastingMin) continue;

                GL.Begin(PrimitiveType.LineLoop);
                for (int h = 0; h < ptCount; h++) GL.Vertex3(mf.turn.turnArr[i].turnLine[h].Easting, mf.turn.turnArr[i].turnLine[h].Northing, 0);
                GL.End();
            }
        }

        //draws the derived closest point
        public void DrawClosestPoint()
        {
            //GL.PointSize(6.0f);
            //GL.Color3(0.219f, 0.932f, 0.070f);
            //GL.Begin(PrimitiveType.Points);
            //GL.Vertex3(closestTurnPt.Easting, closestTurnPt.Northing, 0);
            //GL.End();

            //GL.LineWidth(1);
            //GL.Color3(0.92f, 0.62f, 0.42f);
            //GL.Begin(PrimitiveType.LineStrip);
            //GL.Vertex3(boxD.Easting, boxD.Northing, 0);
            //GL.Vertex3(boxA.Easting, boxA.Northing, 0);
            //GL.Vertex3(boxB.Easting, boxB.Northing, 0);
            //GL.Vertex3(boxC.Easting, boxC.Northing, 0);
            //GL.End();
        }
    }
}