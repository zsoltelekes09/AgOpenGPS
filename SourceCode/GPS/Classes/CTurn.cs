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

        //constructor
        public CTurn(FormGPS _f)
        {
            mf = _f;
        }

        // the list of possible bounds points
        public List<Vec3> turnClosestList = new List<Vec3>();

        public List<List<Vec3>> BoxList = new List<List<Vec3>>();

        public int turnSelected = 0, closestTurnNum;
        //generated box for finding closest point
        public Vec2 boxA = new Vec2(9000, 9000), boxB = new Vec2(9002, 9000);

        public Vec2 boxC = new Vec2(9001, 9001), boxD = new Vec2(9003, 9002);

        //point at the farthest turn segment from pivotAxle
        public Vec3 closestTurnPt = new Vec3();

        public void FindClosestTurnPoint(bool isYouTurnRight, Vec3 fromPt, double headAB)
        {
            //initial scan is straight ahead of pivot point of vehicle to find the right turnLine/boundary

            int closestTurnNum = int.MinValue;


            double CosHead = Math.Cos(headAB);
            double SinHead = Math.Sin(headAB);

            List<Vec4> Crossings1 = new List<Vec4>();

            Vec3 rayPt = fromPt;
            rayPt.Northing += CosHead * mf.maxCrossFieldLength;
            rayPt.Easting += SinHead * mf.maxCrossFieldLength;

            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                if (mf.bnd.bndArr.Count > i ) Crossings1.FindCrossingPoints(mf.bnd.bndArr[i].turnLine, fromPt, rayPt, i);
            }

            if (Crossings1.Count > 0)
            {
                Crossings1.Sort((x, y) => x.Time.CompareTo(y.Time));

                rayPt.Easting = Crossings1[0].Easting;
                rayPt.Northing = Crossings1[0].Northing;
                closestTurnNum = Crossings1[0].Index;
            }

            //second scan is straight ahead of outside of tool based on turn direction
            double scanWidthL, scanWidthR;
            if (isYouTurnRight)// not sure why you want to do it a second time if none found ?
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

            int ptCount = mf.bnd.bndArr[closestTurnNum].turnLine.Count;
            for (int p = 0; p < ptCount; p++)
            {
                if ((((boxB.Easting - boxA.Easting) * (mf.bnd.bndArr[closestTurnNum].turnLine[p].Northing - boxA.Northing))
                        - ((boxB.Northing - boxA.Northing) * (mf.bnd.bndArr[closestTurnNum].turnLine[p].Easting - boxA.Easting))) < 0) { continue; }

                if ((((boxD.Easting - boxC.Easting) * (mf.bnd.bndArr[closestTurnNum].turnLine[p].Northing - boxC.Northing))
                        - ((boxD.Northing - boxC.Northing) * (mf.bnd.bndArr[closestTurnNum].turnLine[p].Easting - boxC.Easting))) < 0) { continue; }

                if ((((boxC.Easting - boxB.Easting) * (mf.bnd.bndArr[closestTurnNum].turnLine[p].Northing - boxB.Northing))
                        - ((boxC.Northing - boxB.Northing) * (mf.bnd.bndArr[closestTurnNum].turnLine[p].Easting - boxB.Easting))) < 0) { continue; }

                if ((((boxA.Easting - boxD.Easting) * (mf.bnd.bndArr[closestTurnNum].turnLine[p].Northing - boxD.Northing))
                        - ((boxA.Northing - boxD.Northing) * (mf.bnd.bndArr[closestTurnNum].turnLine[p].Easting - boxD.Easting))) < 0) { continue; }

                //it's in the box, so add to list
                inBox.Easting = mf.bnd.bndArr[closestTurnNum].turnLine[p].Easting;
                inBox.Northing = mf.bnd.bndArr[closestTurnNum].turnLine[p].Northing;
                inBox.Heading = mf.bnd.bndArr[closestTurnNum].turnLine[p].Heading;

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

        public bool FindClosestTurnPointinBox(bool isYouTurnRight, Vec3 fromPt)
        {
            double CosHead = Math.Cos(fromPt.Heading);
            double SinHead = Math.Sin(fromPt.Heading);

            double scanWidthL, scanWidthR;

            if (isYouTurnRight)
            {
                scanWidthL = -(mf.Guidance.GuidanceWidth * 0.5 - mf.Guidance.GuidanceOffset);
                scanWidthR = mf.Guidance.GuidanceWidth * (0.5 + mf.yt.rowSkipsWidth) + mf.Guidance.GuidanceOffset;
            }
            else
            {
                scanWidthL = -(mf.Guidance.GuidanceWidth * (0.5 + mf.yt.rowSkipsWidth) - mf.Guidance.GuidanceOffset);
                scanWidthR = (mf.Guidance.GuidanceWidth * 0.5 + mf.Guidance.GuidanceOffset);
            }

            double NorthingMax = boxA.Northing = fromPt.Northing + SinHead * -scanWidthL;
            double EastingMax = boxA.Easting = fromPt.Easting + CosHead * scanWidthL;
            double NorthingMin = NorthingMax;
            double EastingMin = EastingMax;

            boxB.Northing = fromPt.Northing + SinHead * -scanWidthR;


            if (boxB.Northing > NorthingMax) NorthingMax = boxB.Northing;
            if (boxB.Northing < NorthingMin) NorthingMin = boxB.Northing;
            boxB.Easting = fromPt.Easting + CosHead * scanWidthR;
            if (boxB.Easting > EastingMax) EastingMax = boxB.Easting;
            if (boxB.Easting < EastingMin) EastingMin = boxB.Easting;
            boxC.Northing = boxB.Northing + CosHead * mf.maxCrossFieldLength;
            if (boxC.Northing > NorthingMax) NorthingMax = boxC.Northing;
            if (boxC.Northing < NorthingMin) NorthingMin = boxC.Northing;
            boxC.Easting = boxB.Easting + SinHead * mf.maxCrossFieldLength;
            if (boxC.Easting > EastingMax) EastingMax = boxC.Easting;
            if (boxC.Easting < EastingMin) EastingMin = boxC.Easting;

            boxD.Northing = boxA.Northing + CosHead * mf.maxCrossFieldLength;
            if (boxD.Northing > NorthingMax) NorthingMax = boxD.Northing;
            if (boxD.Northing < NorthingMin) NorthingMin = boxD.Northing;
            boxD.Easting = boxA.Easting + SinHead * mf.maxCrossFieldLength;
            if (boxD.Easting > EastingMax) EastingMax = boxD.Easting;
            if (boxD.Easting < EastingMin) EastingMin = boxD.Easting;


            Vec2 BoxBA = boxB - boxA;
            Vec2 BoxDC = boxD - boxC;
            Vec2 BoxCB = boxC - boxB;
            Vec2 BoxAD = boxA - boxD;

            BoxList.Clear();
            Vec3 inBox;
            int lasti = -1;
            for (int i = 0; i < mf.bnd.bndArr.Count; i++)
            {
                if (mf.bnd.bndArr[i].isDriveThru || mf.bnd.bndArr[i].isDriveAround) continue;
                if (mf.bnd.bndArr.Count > i)
                {
                    int ptCount = mf.bnd.bndArr[i].turnLine.Count;
                    for (int p = 0; p < ptCount; p++)
                    {
                        if (mf.bnd.bndArr[i].turnLine[p].Northing > NorthingMax || mf.bnd.bndArr[i].turnLine[p].Northing < NorthingMin) continue;
                        if (mf.bnd.bndArr[i].turnLine[p].Easting > EastingMax || mf.bnd.bndArr[i].turnLine[p].Easting < EastingMin) continue;

                        if (((BoxBA.Easting * (mf.bnd.bndArr[i].turnLine[p].Northing - boxA.Northing))
                                - (BoxBA.Northing * (mf.bnd.bndArr[i].turnLine[p].Easting - boxA.Easting))) < 0) continue;

                        if (((BoxDC.Easting * (mf.bnd.bndArr[i].turnLine[p].Northing - boxC.Northing))
                                - (BoxDC.Northing * (mf.bnd.bndArr[i].turnLine[p].Easting - boxC.Easting))) < 0) continue;

                        if (((BoxCB.Easting * (mf.bnd.bndArr[i].turnLine[p].Northing - boxB.Northing))
                                - (BoxCB.Northing * (mf.bnd.bndArr[i].turnLine[p].Easting - boxB.Easting))) < 0) continue;

                        if (((BoxAD.Easting * (mf.bnd.bndArr[i].turnLine[p].Northing - boxD.Northing))
                                - (BoxAD.Northing * (mf.bnd.bndArr[i].turnLine[p].Easting - boxD.Easting))) < 0) continue;

                        inBox.Northing = mf.bnd.bndArr[i].turnLine[p].Northing;
                        inBox.Easting = mf.bnd.bndArr[i].turnLine[p].Easting;
                        inBox.Heading = p;

                        if (i != lasti)
                        {
                            BoxList.Add(new List<Vec3>());
                            lasti = i;
                        }
                        BoxList[BoxList.Count-1].Add(inBox);
                    }
                }
            }
            return BoxList.Count > 0;
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
                if (mf.bnd.bndArr[i].turnLine.Count < 1) return;

                if (mf.bnd.bndArr[i].Northingmin > mf.worldGrid.NorthingMax || mf.bnd.bndArr[i].Northingmax < mf.worldGrid.NorthingMin) continue;
                if (mf.bnd.bndArr[i].Eastingmin > mf.worldGrid.EastingMax || mf.bnd.bndArr[i].Eastingmax < mf.worldGrid.EastingMin) continue;

                GL.Begin(PrimitiveType.LineLoop);
                for (int h = 0; h < mf.bnd.bndArr[i].turnLine.Count; h++) GL.Vertex3(mf.bnd.bndArr[i].turnLine[h].Easting, mf.bnd.bndArr[i].turnLine[h].Northing, 0);
                GL.End();
            }
        }
    }
}