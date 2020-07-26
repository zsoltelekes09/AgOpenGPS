using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public class CABCurve
    {
        //pointers to mainform controls
        private readonly FormGPS mf;

        public bool SpiralMode = false;
        public bool CircleMode = false;
        public double OldhowManyPathsAway;

        //flag for starting stop adding points
        public bool isBtnCurveOn, isOkToAddPoints, isCurveSet;

        public double distanceFromCurrentLine;
        public bool isABSameAsVehicleHeading = true;
        public bool isOnRightSideCurrentLine = true;

        public double howManyPathsAway, curveNumber;
        public Vec2 refPoint1 = new Vec2(1, 1), refPoint2 = new Vec2(2, 2);

        public bool isSameWay, OldisSameWay;
        public double moveDistance;

        //generated box for finding closest point
        public Vec2 boxA = new Vec2(0, 0), boxB = new Vec2(0, 2);

        public Vec2 boxC = new Vec2(1, 1), boxD = new Vec2(2, 3);
        private int A, B, C;
        public int currentLocationIndex;

        public double aveLineHeading;

        //pure pursuit values
        public Vec2 goalPointCu = new Vec2(0, 0);

        public Vec2 radiusPointCu = new Vec2(0, 0);
        public double steerAngleCu, rEastCu, rNorthCu, ppRadiusCu;

        //the list of points of the ref line.
        public List<Vec3> refList = new List<Vec3>();
        //the list of points of curve to drive on
        public List<Vec3> curList = new List<Vec3>();

        public bool isSmoothWindowOpen;
        public List<Vec3> smooList = new List<Vec3>();

        public List<CCurveLines> curveArr = new List<CCurveLines>();
        public int numCurveLines, numCurveLineSelected;

        public bool isEditing;
        public List<Vec2> tramArr = new List<Vec2>();
        public List<List<Vec2>> tramList = new List<List<Vec2>>();

        public List<List<Vec3>> GuidanceLines = new List<List<Vec3>>();

        public CABCurve(FormGPS _f)
        {
            //constructor
            mf = _f;
        }

        public void DrawCurve()
        {
            if (!isEditing)
            {
                int ptCount = refList.Count;
                if (refList.Count == 0) return;

                GL.LineWidth(mf.ABLine.lineWidth);
                GL.Color3(0.96, 0.2f, 0.2f);
                GL.Begin(PrimitiveType.Lines);
                for (int h = 0; h < ptCount; h++) GL.Vertex3(refList[h].easting, refList[h].northing, 0);
                if (!mf.curve.isCurveSet)
                {
                    GL.Color3(0.930f, 0.0692f, 0.260f);
                    ptCount--;
                    GL.Vertex3(refList[ptCount].easting, refList[ptCount].northing, 0);
                    GL.Vertex3(mf.pivotAxlePos.easting, mf.pivotAxlePos.northing, 0);
                }
                GL.End();

                if (mf.font.isFontOn && refList.Count > 410)
                {
                    GL.Color3(0.40f, 0.90f, 0.95f);
                    mf.font.DrawText3D(refList[201].easting, refList[201].northing, "&A");
                    mf.font.DrawText3D(refList[refList.Count - 200].easting, refList[refList.Count - 200].northing, "&B");
                }

                //just draw ref and smoothed line if smoothing window is open
                if (isSmoothWindowOpen)
                {
                    ptCount = smooList.Count;
                    if (smooList.Count == 0) return;

                    GL.LineWidth(mf.ABLine.lineWidth);
                    GL.Color3(0.930f, 0.92f, 0.260f);
                    GL.Begin(PrimitiveType.Lines);
                    for (int h = 0; h < ptCount; h++) GL.Vertex3(smooList[h].easting, smooList[h].northing, 0);
                    GL.End();
                }
                else //normal. Smoothing window is not open.
                {
                    ptCount = curList.Count;
                    if (ptCount > 0 && isCurveSet)
                    {
                        GL.PointSize(2);

                        GL.Color3(0.95f, 0.2f, 0.95f);
                        GL.Begin(PrimitiveType.LineStrip);
                        for (int h = 0; h < ptCount; h++) GL.Vertex3(curList[h].easting, curList[h].northing, 0);
                        GL.End();

                        if (mf.isPureDisplayOn && !mf.isStanleyUsed)
                        {
                            if (ppRadiusCu < 100 && ppRadiusCu > -100)
                            {
                                const int numSegments = 100;
                                double theta = Glm.twoPI / numSegments;
                                double c = Math.Cos(theta);//precalculate the sine and cosine
                                double s = Math.Sin(theta);
                                double x = ppRadiusCu;//we start at angle = 0
                                double y = 0;

                                GL.LineWidth(1);
                                GL.Color3(0.95f, 0.30f, 0.950f);
                                GL.Begin(PrimitiveType.LineLoop);
                                for (int ii = 0; ii < numSegments; ii++)
                                {
                                    //glVertex2f(x + cx, y + cy);//output vertex
                                    GL.Vertex3(x + radiusPointCu.easting, y + radiusPointCu.northing, 0);//output vertex
                                    double t = x;//apply the rotation matrix
                                    x = (c * x) - (s * y);
                                    y = (s * t) + (c * y);
                                }
                                GL.End();
                            }

                            //Draw lookahead Point
                            GL.PointSize(4.0f);
                            GL.Begin(PrimitiveType.Points);
                            GL.Color3(1.0f, 0.5f, 0.95f);
                            GL.Vertex3(goalPointCu.easting, goalPointCu.northing, 0.0);
                            GL.End();
                        }

                        mf.yt.DrawYouTurn();

                        if (mf.yt.isYouTurnTriggered)
                        {
                            GL.Color3(0.95f, 0.95f, 0.25f);
                            GL.LineWidth(mf.ABLine.lineWidth);
                            ptCount = mf.yt.ytList.Count;
                            if (ptCount > 0)
                            {
                                GL.Begin(PrimitiveType.Points);
                                for (int i = 0; i < ptCount; i++)
                                {
                                    GL.Vertex3(mf.yt.ytList[i].easting, mf.yt.ytList[i].northing, 0);
                                }
                                GL.End();
                            }
                            GL.Color3(0.95f, 0.05f, 0.05f);
                        }

                        if (mf.isSideGuideLines)
                        {
                            GL.Color3(0.56f, 0.650f, 0.650f);
                            GL.Enable(EnableCap.LineStipple);
                            GL.LineStipple(1, 0x0101);

                            GL.LineWidth(mf.ABLine.lineWidth);



                            for (int i = 0; i < GuidanceLines.Count; i++)
                            {
                                if (GuidanceLines[i].Count > 0)
                                {
                                    GL.Begin(PrimitiveType.LineStrip);
                                    for (int h = 0; h < GuidanceLines[i].Count; h++) GL.Vertex3(GuidanceLines[i][h].easting, GuidanceLines[i][h].northing, 0);
                                    GL.End();
                                }
                            }
                            GL.Disable(EnableCap.LineStipple);
                        }
                    }
                }
                GL.PointSize(1.0f);
            }

            if (isEditing)
            {
                int ptCount = refList.Count;
                if (refList.Count == 0) return;

                GL.LineWidth(mf.ABLine.lineWidth);
                GL.Color3(0.930f, 0.2f, 0.260f);
                GL.Begin(PrimitiveType.Lines);
                for (int h = 0; h < ptCount; h++) GL.Vertex3(refList[h].easting, refList[h].northing, 0);
                GL.End();

                //current line
                if (curList.Count > 0 && isCurveSet)
                {
                    ptCount = curList.Count;
                    GL.Color3(0.95f, 0.2f, 0.950f);
                    GL.Begin(PrimitiveType.LineStrip);
                    for (int h = 0; h < ptCount; h++) GL.Vertex3(curList[h].easting, curList[h].northing, 0);
                    GL.End();
                }


                if (mf.camera.camSetDistance > -200)
                {
                    double cosHeading2 = Math.Cos(-mf.curve.aveLineHeading);
                    double sinHeading2 = Math.Sin(-mf.curve.aveLineHeading);

                    GL.Color3(0.8f, 0.3f, 0.2f);
                    GL.PointSize(2);
                    GL.Begin(PrimitiveType.Points);

                    for (int i = 1; i <= 6; i++)
                    {
                        for (int h = 0; h < ptCount; h++)
                            GL.Vertex3((cosHeading2 * mf.Guidance.WidthMinusOverlap * i) + mf.curve.refList[h].easting,
                                          (sinHeading2 * mf.Guidance.WidthMinusOverlap * i) + mf.curve.refList[h].northing, 0);
                    }

                    GL.End();
                }
            }

            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 2) DrawTram();
            if (mf.tram.displayMode == 1 || mf.tram.displayMode == 3) mf.tram.DrawTramBnd();
        }

        public void DrawTram()
        {
            GL.Color4(0.8630f, 0.93692f, 0.3260f, 0.22);

            //int k = (int)(mf.camera.camSetDistance / -400);
            for (int i = 0; i < tramList.Count; i++)
            {
                GL.Begin(PrimitiveType.TriangleStrip);
                for (int h = 0; h < tramList[i].Count; h++) GL.Vertex3(tramList[i][h].easting, tramList[i][h].northing, 0);
                GL.End();
            }

            if (mf.font.isFontOn)
            {
                for (int i = 0; i < tramList.Count; i++)
                {
                    GL.Color4(0.8630f, 0.93692f, 0.8260f, 0.752);
                    if (tramList[i].Count > 0)
                    {
                        int middle = tramList[i].Count - 1;
                        mf.font.DrawText3D(tramList[i][middle].easting, tramList[i][middle].northing, (i + 1).ToString());
                        mf.font.DrawText3D(tramList[i][0].easting, tramList[i][0].northing, (i + 1).ToString());
                    }
                }
            }
        }

        public void BuildTram()
        {
            mf.tram.BuildTramBnd();
            tramList?.Clear();
            tramArr?.Clear();

            Vec2 tramLineP1;

            double pass = 0.5;
            double headingCalc = aveLineHeading + Glm.PIBy2;

            double hsin = Math.Sin(headingCalc);
            double hcos = Math.Cos(headingCalc);

            for (int i = 0; i < mf.tram.passes; i++)
            {
                tramArr = new List<Vec2>();
                tramList.Add(tramArr);
                for (int j = 0; j < refList.Count; j += 4)
                {
                    tramLineP1.easting = (hsin * ((mf.tram.tramWidth * (pass + i)) - mf.tram.halfWheelTrack + mf.tram.abOffset)) + refList[j].easting;
                    tramLineP1.northing = (hcos * ((mf.tram.tramWidth * (pass + i)) - mf.tram.halfWheelTrack + mf.tram.abOffset)) + refList[j].northing;

                    if (mf.bnd.bndArr.Count > 0)
                    {
                        if (mf.bnd.bndArr[0].IsPointInsideBoundary(tramLineP1))
                        {
                            tramArr.Add(tramLineP1);

                            tramLineP1.easting = (hsin * mf.tram.wheelTrack) + tramLineP1.easting;
                            tramLineP1.northing = (hcos * mf.tram.wheelTrack) + tramLineP1.northing;
                            tramArr.Add(tramLineP1);
                        }
                    }
                    else
                    {
                        tramArr.Add(tramLineP1);

                        tramLineP1.easting = (hsin * mf.tram.wheelTrack) + tramLineP1.easting;
                        tramLineP1.northing = (hcos * mf.tram.wheelTrack) + tramLineP1.northing;
                        tramArr.Add(tramLineP1);
                    }
                }
            }
        }

        //for calculating for display the averaged new line
        public void SmoothAB(int smPts)
        {
            //count the reference list of original curve
            int cnt = refList.Count;

            //just go back if not very long
            if (!isCurveSet || cnt < 400) return;

            //the temp array
            Vec3[] arr = new Vec3[cnt];

            //read the points before and after the setpoint
            for (int s = 0; s < smPts / 2; s++)
            {
                arr[s].easting = refList[s].easting;
                arr[s].northing = refList[s].northing;
                arr[s].heading = refList[s].heading;
            }

            for (int s = cnt - (smPts / 2); s < cnt; s++)
            {
                arr[s].easting = refList[s].easting;
                arr[s].northing = refList[s].northing;
                arr[s].heading = refList[s].heading;
            }

            //average them - center weighted average
            for (int i = smPts / 2; i < cnt - (smPts / 2); i++)
            {
                for (int j = -smPts / 2; j < smPts / 2; j++)
                {
                    arr[i].easting += refList[j + i].easting;
                    arr[i].northing += refList[j + i].northing;
                }
                arr[i].easting /= smPts;
                arr[i].northing /= smPts;
                arr[i].heading = refList[i].heading;
            }

            //make a list to draw
            smooList?.Clear();
            for (int i = 0; i < cnt; i++)
            {
                smooList.Add(arr[i]);
            }
        }

        public void CalculateTurnHeadings()
        {
            //to calc heading based on next and previous points to give an average heading.
            int cnt = refList.Count;
            if (cnt > 0)
            {
                Vec3[] arr = new Vec3[cnt];
                cnt--;
                refList.CopyTo(arr);
                refList.Clear();

                //middle points
                for (int i = 1; i < cnt; i++)
                {
                    Vec3 pt3 = arr[i];
                    pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                    if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                    refList.Add(pt3);
                }
            }
        }

        //turning the visual line into the real reference line to use
        public void SaveSmoothAsRefList()
        {
            //oops no smooth list generated
            int cnt = smooList.Count;
            if (cnt == 0) return;

            //eek
            refList?.Clear();

            //copy to an array to calculate all the new headings
            Vec3[] arr = new Vec3[cnt];
            smooList.CopyTo(arr);

            //calculate new headings on smoothed line
            for (int i = 1; i < cnt - 1; i++)
            {
                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i].easting, arr[i + 1].northing - arr[i].northing);
                if (arr[i].heading < 0) arr[i].heading += Glm.twoPI;
                refList.Add(arr[i]);
            }
        }
        bool test = false;
        public void GetCurrentCurveLine(Vec3 pivot, Vec3 steer)
        {

            double minDistance;

            double boundaryTriggerDistance = 1;

            if (SpiralMode == true)
            {

                double dist = ((pivot.easting - refList[0].easting) * (pivot.easting - refList[0].easting)) + ((pivot.northing - refList[0].northing) * (pivot.northing - refList[0].northing));

                minDistance = Math.Sqrt(dist);

                howManyPathsAway = Math.Round(minDistance / mf.Guidance.WidthMinusOverlap, 0, MidpointRounding.AwayFromZero);
                if (OldhowManyPathsAway != howManyPathsAway && howManyPathsAway == 0)
                {
                    OldhowManyPathsAway = howManyPathsAway;
                    curList?.Clear();
                }
                if (OldhowManyPathsAway != howManyPathsAway)
                {
                    OldhowManyPathsAway = howManyPathsAway;
                    if (howManyPathsAway < 2) howManyPathsAway = 2;

                    double s = mf.Guidance.WidthMinusOverlap / 2;

                    curList?.Clear();
                    //double circumference = (glm.twoPI * s) / (boundaryTriggerDistance * 0.1);
                    double circumference;

                    for (double round = Glm.twoPI * (howManyPathsAway - 2); round <= (Glm.twoPI * (howManyPathsAway + 2) + 0.00001); round += (Glm.twoPI / circumference))
                    {
                        double x = s * (Math.Cos(round) + (round / Math.PI) * Math.Sin(round));
                        double y = s * (Math.Sin(round) - (round / Math.PI) * Math.Cos(round));

                        Vec3 pt = new Vec3(refList[0].easting + x, refList[0].northing + y, 0);
                        curList.Add(pt);

                        double radius = Math.Sqrt(x * x + y * y);
                        circumference = (Glm.twoPI * radius) / (boundaryTriggerDistance);

                    }

                    int cnt = curList.Count;

                    if (cnt > 1)
                    {
                        Vec3[] arr = new Vec3[cnt];

                        curList.CopyTo(arr);
                        curList.Clear();

                        //first point needs last, first, second points
                        Vec3 pt3 = arr[0];
                        pt3.heading = Math.Atan2(arr[1].easting - arr[cnt - 1].easting, arr[1].northing - arr[cnt - 1].northing);
                        if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                        curList.Add(pt3);

                        //middle points
                        for (int i = 1; i < (cnt - 1); i++)
                        {
                            pt3 = arr[i];
                            pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                            if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                            curList.Add(pt3);
                        }

                        pt3 = arr[cnt - 1];
                        pt3.heading = Math.Atan2(arr[0].easting - arr[cnt - 2].easting, arr[0].northing - arr[cnt - 2].northing);
                        if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                        curList.Add(pt3);
                    }
                }
                //refList = curList;
            }
            else if (CircleMode == true)
            {
                double dist = ((pivot.easting - refList[0].easting) * (pivot.easting - refList[0].easting)) + ((pivot.northing - refList[0].northing) * (pivot.northing - refList[0].northing));

                minDistance = Math.Sqrt(dist);

                howManyPathsAway = Math.Round(minDistance / mf.Guidance.WidthMinusOverlap, 0, MidpointRounding.AwayFromZero);
                if (OldhowManyPathsAway != howManyPathsAway && howManyPathsAway == 0)
                {
                    OldhowManyPathsAway = howManyPathsAway;
                    curList?.Clear();
                }
                else if (OldhowManyPathsAway != howManyPathsAway)
                {
                    if (howManyPathsAway > 100) return;
                    OldhowManyPathsAway = howManyPathsAway;

                    curList?.Clear();

                    int aa = (int)((Glm.twoPI * mf.Guidance.WidthMinusOverlap * howManyPathsAway) / (boundaryTriggerDistance));

                    for (double round = 0; round <= Glm.twoPI + 0.00001; round += (Glm.twoPI) / aa)
                    {
                        Vec3 pt = new Vec3(refList[0].easting + (Math.Sin(round) * mf.Guidance.WidthMinusOverlap * howManyPathsAway), refList[0].northing + (Math.Cos(round) * mf.Guidance.WidthMinusOverlap * howManyPathsAway), 0);
                        curList.Add(pt);
                    }

                    int cnt = curList.Count;

                    if (cnt > 1)
                    {
                        Vec3[] arr = new Vec3[cnt];

                        curList.CopyTo(arr);
                        curList.Clear();

                        //first point needs last, first, second points
                        Vec3 pt3 = arr[0];
                        pt3.heading = Math.Atan2(arr[1].easting - arr[cnt - 1].easting, arr[1].northing - arr[cnt - 1].northing);
                        if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                        curList.Add(pt3);
                        //middle points
                        for (int i = 1; i < (cnt - 1); i++)
                        {
                            pt3 = arr[i];
                            pt3.heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                            if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                            curList.Add(pt3);
                        }

                        pt3 = arr[cnt - 1];
                        pt3.heading = Math.Atan2(arr[0].easting - arr[cnt - 2].easting, arr[0].northing - arr[cnt - 2].northing);
                        if (pt3.heading < 0) pt3.heading += Glm.twoPI;
                        curList.Add(pt3);
                    }
                }


            }
            else
            {
                if (refList.Count < 5) return;
                double minDistA = double.PositiveInfinity, minDistB = double.PositiveInfinity;


                if (!mf.isAutoSteerBtnOn)
                {

                    //find the closest 2 points to current fix
                    for (int t = 0; t < refList.Count; t++)
                    {
                        double dist = ((pivot.easting - refList[t].easting) * (pivot.easting - refList[t].easting))
                                        + ((pivot.northing - refList[t].northing) * (pivot.northing - refList[t].northing));
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
                    if (A > B) { C = A; A = B; B = C; }

                    if (minDistA == 1000000 || minDistB == 1000000) return;

                    //are we going same direction as stripList was created?
                    isSameWay = Math.PI - Math.Abs(Math.Abs(pivot.heading - refList[A].heading) - Math.PI) < Glm.PIBy2;


                    //double distanceFromRefLine = (pivot.northing - refList[A].northing) * (refList[B].easting - refList[A].easting) - (pivot.easting - refList[A].easting) * (refList[B].northing - refList[A].northing);

                    double distanceFromRefLine = ((refList[B].easting - refList[A].easting) * pivot.northing - (refList[B].northing - refList[A].northing) * pivot.easting + refList[B].northing * refList[A].easting - refList[B].easting * refList[A].northing) /
                            Math.Sqrt((refList[A].easting - refList[B].easting) * (refList[A].easting - refList[B].easting) + (refList[A].northing - refList[B].northing) * (refList[A].northing - refList[B].northing));


                    if (isSameWay) distanceFromRefLine -= mf.Guidance.GuidanceOffset;
                    else distanceFromRefLine += mf.Guidance.GuidanceOffset;

                    howManyPathsAway = Math.Round(Math.Abs(distanceFromRefLine) / mf.Guidance.WidthMinusOverlap, 0, MidpointRounding.AwayFromZero);

                    curveNumber = howManyPathsAway;
                    if (distanceFromRefLine < 0) curveNumber = -curveNumber;
                }

                if (OldisSameWay != isSameWay || howManyPathsAway != OldhowManyPathsAway)
                {
                    OldisSameWay = isSameWay;
                    OldhowManyPathsAway = howManyPathsAway;




                    if (test == true)
                    {
                        for (int i = 0; i < refList.Count - 1; i++)
                        {
                            double Degree_Diff = (refList[i + 1].heading - refList[i].heading + 9.42478) % Glm.twoPI - Math.PI;

                            if (Degree_Diff > 0.174533 || Degree_Diff < -0.174533)
                            {
                                refList.Insert(i + 1, new Vec3(refList[i + 1].easting * 0.5 + refList[i].easting * 0.5, refList[i + 1].northing * 0.5 + refList[i].northing * 0.5, refList[i].heading + Degree_Diff * 0.5));
                                i--;
                            }
                        }
                        test = false;
                    }



                    if (mf.isSideGuideLines)
                    {
                        GuidanceLines.Clear();
                        for (double i = -2.5; i < 3.5; i++)
                        {
                            GuidanceLines.Add(new List<Vec3>());

                            for (int j = 0; j < refList.Count - 1; j++)
                            {

                                double piSide2;

                                //sign of distance determines which side of line we are on
                                if (curveNumber > 0) piSide2 = -Glm.PIBy2;
                                else piSide2 = Glm.PIBy2;

                                double Offset2 = mf.Guidance.WidthMinusOverlap * (i + howManyPathsAway);
                                
                                var point = new Vec3(
                                refList[j].easting + (Math.Sin(piSide2 + refList[j].heading) * Offset2),
                                refList[j].northing + (Math.Cos(piSide2 + refList[j].heading) * Offset2),
                                refList[j].heading);
                                bool Add = true;
                                for (int t = 0; t < refList.Count; t++)
                                {
                                    double dist = ((point.easting - refList[t].easting) * (point.easting - refList[t].easting)) + ((point.northing - refList[t].northing) * (point.northing - refList[t].northing));
                                    if (dist < (Offset2 * Offset2) - 10)
                                    {
                                        Add = false;
                                        break;
                                    }
                                }
                                if (Add) GuidanceLines[GuidanceLines.Count - 1].Add(point);
                            }
                        }
                    }

                    //build the current line
                    curList?.Clear();

                    double piSide;

                    //sign of distance determines which side of line we are on
                    if (curveNumber > 0) piSide = -Glm.PIBy2;
                    else piSide = Glm.PIBy2;



                    double Offset = mf.Guidance.WidthMinusOverlap * howManyPathsAway;
                    if (isSameWay)
                    {
                        if (curveNumber > 0) Offset += mf.Guidance.GuidanceOffset;
                        else Offset -= mf.Guidance.GuidanceOffset;
                    }
                    else
                    {
                        if (curveNumber > 0) Offset -= mf.Guidance.GuidanceOffset;
                        else Offset += mf.Guidance.GuidanceOffset;
                    }



                    for (int i = 0; i < refList.Count - 1; i++)
                    {
                        var point = new Vec3(refList[i].easting + (Math.Sin(piSide + refList[i].heading) * Offset), refList[i].northing + (Math.Cos(piSide + refList[i].heading) * Offset), refList[i].heading);
                        bool Add = true;

                        for (int t = 0; t < refList.Count; t++)
                        {
                            double dist = ((point.easting - refList[t].easting) * (point.easting - refList[t].easting)) + ((point.northing - refList[t].northing) * (point.northing - refList[t].northing));
                            if (dist < (Offset * Offset) - 10)
                            {
                                Add = false;
                                break;
                            }
                        }
                        if (Add)
                        {
                            if (curList.Count > 0)
                            {
                                double dist = ((point.easting - curList[curList.Count - 1].easting) * (point.easting - curList[curList.Count - 1].easting)) + ((point.northing - curList[curList.Count - 1].northing) * (point.northing - curList[curList.Count - 1].northing));
                                if (dist > 1)
                                    curList.Add(point);
                            }
                            else curList.Add(point);
                        }
                    }

                    int cnt = curList.Count;
                    if (cnt < -10)
                    {
                        for (int i = 1; i < (curList.Count - 1); i++)
                        {
                            curList[i] = new Vec3(curList[i].easting, curList[i].northing, Math.Atan2(curList[i + 1].easting - curList[i - 1].easting, curList[i + 1].northing - curList[i - 1].northing));
                        }

                        Vec3[] arr = new Vec3[cnt];
                        curList.CopyTo(arr);
                        curList.Clear();
                        bool Next = false;
                        for (int i = 0; i < arr.Length - 1; i++)
                        {
                            if (arr[i].heading - arr[i + 1].heading > 0.174533)
                            {
                                Next = true;
                                curList.Add(new Vec3(arr[i].easting * 0.75 + arr[i + 1].easting * 0.25, arr[i].northing * 0.75 + arr[i + 1].northing * 0.25, 0));
                                curList.Add(new Vec3(arr[i].easting * 0.25 + arr[i + 1].easting * 0.75, arr[i].northing * 0.25 + arr[i + 1].northing * 0.75, 0));
                            }
                            else if (Next)
                            {
                                Next = false;
                                curList.Add(new Vec3(arr[i].easting * 0.75 + arr[i + 1].easting * 0.25, arr[i].northing * 0.75 + arr[i + 1].northing * 0.25, 0));
                                curList.Add(new Vec3(arr[i].easting * 0.25 + arr[i + 1].easting * 0.75, arr[i].northing * 0.25 + arr[i + 1].northing * 0.75, 0));
                            }
                            else
                            {
                                curList.Add(new Vec3(arr[i].easting * 0.5 + arr[i + 1].easting * 0.5, arr[i].northing * 0.5 + arr[i + 1].northing * 0.5, 0));
                            }
                        }


                        for (int i = 1; i < (curList.Count - 1); i++)
                        {
                            curList[i] = new Vec3(curList[i].easting, curList[i].northing, Math.Atan2(curList[i + 1].easting - curList[i - 1].easting, curList[i + 1].northing - curList[i - 1].northing));
                        }



                        if (mf.Tools[0].isToolTrailing)
                        {
                            double head = 0;
                            if (isSameWay) head = Math.PI;

                            if (mf.Tools[0].isToolTBT && mf.Tools[0].toolTankTrailingHitchLength < 0)
                            {
                                arr = new Vec3[curList.Count];
                                curList.CopyTo(arr);
                                curList.Clear();

                                for (int i = 0; i < arr.Length; i++)
                                {
                                    arr[i].easting += Math.Sin(arr[i].heading + head) * mf.Tools[0].toolTankTrailingHitchLength;
                                    arr[i].northing += Math.Cos(arr[i].heading + head) * mf.Tools[0].toolTankTrailingHitchLength;
                                }

                                for (int i = 1; i < (arr.Length - 1); i++)
                                {
                                    arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                                }

                                Next = false;
                                for (int i = 0; i < arr.Length - 1; i++)
                                {
                                    if (arr[i].heading - arr[i + 1].heading > 0.174533)
                                    {
                                        Next = true;
                                        curList.Add(new Vec3(arr[i].easting * 0.75 + arr[i + 1].easting * 0.25, arr[i].northing * 0.75 + arr[i + 1].northing * 0.25, 0));
                                        curList.Add(new Vec3(arr[i].easting * 0.25 + arr[i + 1].easting * 0.75, arr[i].northing * 0.25 + arr[i + 1].northing * 0.75, 0));
                                    }
                                    else if (Next)
                                    {
                                        Next = false;
                                        curList.Add(new Vec3(arr[i].easting * 0.75 + arr[i + 1].easting * 0.25, arr[i].northing * 0.75 + arr[i + 1].northing * 0.25, 0));
                                        curList.Add(new Vec3(arr[i].easting * 0.25 + arr[i + 1].easting * 0.75, arr[i].northing * 0.25 + arr[i + 1].northing * 0.75, 0));
                                    }
                                    else
                                    {
                                        curList.Add(new Vec3(arr[i].easting * 0.5 + arr[i + 1].easting * 0.5, arr[i].northing * 0.5 + arr[i + 1].northing * 0.5, 0));
                                    }
                                }
                                for (int i = 1; i < (curList.Count - 1); i++)
                                {
                                    curList[i] = new Vec3(curList[i].easting, curList[i].northing, Math.Atan2(curList[i + 1].easting - curList[i - 1].easting, curList[i + 1].northing - curList[i - 1].northing));
                                }
                            }

                            arr = new Vec3[curList.Count];
                            curList.CopyTo(arr);
                            curList.Clear();


                            for (int i = 0; i < arr.Length; i++)
                            {
                                arr[i].easting += Math.Sin(arr[i].heading + head) * mf.Tools[0].toolTrailingHitchLength;
                                arr[i].northing += Math.Cos(arr[i].heading + head) * mf.Tools[0].toolTrailingHitchLength;
                            }
                            for (int i = 1; i < (arr.Length - 1); i++)
                            {
                                arr[i].heading = Math.Atan2(arr[i + 1].easting - arr[i - 1].easting, arr[i + 1].northing - arr[i - 1].northing);
                            }

                            Next = false;
                            for (int i = 0; i < arr.Length - 1; i++)
                            {
                                if (arr[i].heading - arr[i + 1].heading > 0.174533)
                                {
                                    Next = true;
                                    curList.Add(new Vec3(arr[i].easting * 0.75 + arr[i + 1].easting * 0.25, arr[i].northing * 0.75 + arr[i + 1].northing * 0.25, 0));
                                    curList.Add(new Vec3(arr[i].easting * 0.25 + arr[i + 1].easting * 0.75, arr[i].northing * 0.25 + arr[i + 1].northing * 0.75, 0));
                                }
                                else if (Next)
                                {
                                    Next = false;
                                    curList.Add(new Vec3(arr[i].easting * 0.75 + arr[i + 1].easting * 0.25, arr[i].northing * 0.75 + arr[i + 1].northing * 0.25, 0));
                                    curList.Add(new Vec3(arr[i].easting * 0.25 + arr[i + 1].easting * 0.75, arr[i].northing * 0.25 + arr[i + 1].northing * 0.75, 0));
                                }
                                else
                                {
                                    curList.Add(new Vec3(arr[i].easting * 0.5 + arr[i + 1].easting * 0.5, arr[i].northing * 0.5 + arr[i + 1].northing * 0.5, 0));
                                }
                            }
                            for (int i = 1; i < (curList.Count - 1); i++)
                            {
                                curList[i] = new Vec3(curList[i].easting, curList[i].northing, Math.Atan2(curList[i + 1].easting - curList[i - 1].easting, curList[i + 1].northing - curList[i - 1].northing));
                            }
                        }
                    }
                }
            }


            int ptCount = curList.Count;

            if (ptCount > 0)
            {
                double minDistA = 1000000, minDistB = 1000000;
                if (mf.isStanleyUsed)
                {
                    //find the closest 2 points to current fix
                    for (int t = 0; t < ptCount; t++)
                    {
                        double dist = ((steer.easting - curList[t].easting) * (steer.easting - curList[t].easting))
                                        + ((steer.northing - curList[t].northing) * (steer.northing - curList[t].northing));
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

                    currentLocationIndex = A;

                    //get the distance from currently active AB line
                    double dx = curList[B].easting - curList[A].easting;
                    double dz = curList[B].northing - curList[A].northing;

                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                    //abHeading = Math.Atan2(dz, dx);
                    double abHeading = curList[A].heading;

                    //how far from current AB Line is fix
                    distanceFromCurrentLine = ((dz * steer.easting) - (dx * steer.northing) + (curList[B].easting
                                * curList[A].northing) - (curList[B].northing * curList[A].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                    //are we on the right side or not
                    isOnRightSideCurrentLine = distanceFromCurrentLine > 0;

                    //absolute the distance
                    distanceFromCurrentLine = Math.Abs(distanceFromCurrentLine);

                    //Subtract the two headings, if > 1.57 its going the opposite heading as refAB
                    double abFixHeadingDelta = (Math.Abs(mf.fixHeading - abHeading));
                    if (abFixHeadingDelta >= Math.PI) abFixHeadingDelta = Math.Abs(abFixHeadingDelta - Glm.twoPI);
                    isABSameAsVehicleHeading = abFixHeadingDelta < Glm.PIBy2;

                    // calc point on ABLine closest to current position
                    double U = (((steer.easting - curList[A].easting) * dx)
                                + ((steer.northing - curList[A].northing) * dz))
                                / ((dx * dx) + (dz * dz));

                    rEastCu = curList[A].easting + (U * dx);
                    rNorthCu = curList[A].northing + (U * dz);

                    //distance is negative if on left, positive if on right
                    if (isABSameAsVehicleHeading)
                    {
                        if (!isOnRightSideCurrentLine)
                        {
                            distanceFromCurrentLine *= -1.0;
                        }
                        abFixHeadingDelta = (steer.heading - abHeading);
                    }

                    //opposite way so right is left
                    else
                    {
                        if (isOnRightSideCurrentLine)
                        {
                            distanceFromCurrentLine *= -1.0;
                        }
                        abFixHeadingDelta = (steer.heading - abHeading + Math.PI);
                    }

                    //Fix the circular error
                    if (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < Math.PI) abFixHeadingDelta += Math.PI;

                    if (abFixHeadingDelta > Glm.PIBy2) abFixHeadingDelta -= Math.PI;
                    else if (abFixHeadingDelta < -Glm.PIBy2) abFixHeadingDelta += Math.PI;

                    abFixHeadingDelta *= mf.vehicle.stanleyHeadingErrorGain;
                    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
                    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

                    steerAngleCu = Math.Atan((distanceFromCurrentLine * mf.vehicle.stanleyGain)
                        / ((Math.Abs(mf.pn.speed) * 0.277777) + 1));

                    if (steerAngleCu > 0.74) steerAngleCu = 0.74;
                    if (steerAngleCu < -0.74) steerAngleCu = -0.74;

                    if (mf.pn.speed > -0.1)
                        steerAngleCu = Glm.ToDegrees((steerAngleCu + abFixHeadingDelta) * -1.0);
                    else
                        steerAngleCu = Glm.ToDegrees((steerAngleCu - abFixHeadingDelta) * -1.0);

                    if (steerAngleCu < -mf.vehicle.maxSteerAngle) steerAngleCu = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCu > mf.vehicle.maxSteerAngle) steerAngleCu = mf.vehicle.maxSteerAngle;

                    //Convert to millimeters
                    distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);
                }
                else
                {
                    //find the closest 2 points to current fix
                    for (int t = 0; t < ptCount; t++)
                    {
                        double dist = ((pivot.easting - curList[t].easting) * (pivot.easting - curList[t].easting))
                                        + ((pivot.northing - curList[t].northing) * (pivot.northing - curList[t].northing));
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

                    currentLocationIndex = A;

                    //get the distance from currently active AB line
                    double dx = curList[B].easting - curList[A].easting;
                    double dz = curList[B].northing - curList[A].northing;

                    if (Math.Abs(dx) < Double.Epsilon && Math.Abs(dz) < Double.Epsilon) return;

                    //abHeading = Math.Atan2(dz, dx);

                    //how far from current AB Line is fix
                    distanceFromCurrentLine = ((dz * pivot.easting) - (dx * pivot.northing) + (curList[B].easting
                                * curList[A].northing) - (curList[B].northing * curList[A].easting))
                                    / Math.Sqrt((dz * dz) + (dx * dx));

                    //are we on the right side or not
                    isOnRightSideCurrentLine = distanceFromCurrentLine > 0;

                    //absolute the distance
                    distanceFromCurrentLine = Math.Abs(distanceFromCurrentLine);

                    // ** Pure pursuit ** - calc point on ABLine closest to current position
                    double U = (((pivot.easting - curList[A].easting) * dx)
                                + ((pivot.northing - curList[A].northing) * dz))
                                / ((dx * dx) + (dz * dz));

                    rEastCu = curList[A].easting + (U * dx);
                    rNorthCu = curList[A].northing + (U * dz);

                    //double minx, maxx, miny, maxy;

                    //minx = Math.Min(curList[A].northing, curList[B].northing);
                    //maxx = Math.Max(curList[A].northing, curList[B].northing);

                    //miny = Math.Min(curList[A].easting, curList[B].easting);
                    //maxy = Math.Max(curList[A].easting, curList[B].easting);

                    //isValid = rNorthCu >= minx && rNorthCu <= maxx && (rEastCu >= miny && rEastCu <= maxy);

                    //if (!isValid)
                    //{
                    //    //invalid distance so tell AS module
                    //    distanceFromCurrentLine = 32000;
                    //    mf.guidanceLineDistanceOff = 32000;
                    //    return;
                    //}

                    //used for accumulating distance to find goal point
                    double distSoFar;

                    //update base on autosteer settings and distance from line
                    double goalPointDistance = mf.vehicle.UpdateGoalPointDistance(distanceFromCurrentLine);
                    mf.lookaheadActual = goalPointDistance;

                    // used for calculating the length squared of next segment.
                    double tempDist = 0.0;

                    if (!isSameWay)
                    {
                        //counting down
                        isABSameAsVehicleHeading = false;
                        distSoFar = Glm.Distance(curList[A], rEastCu, rNorthCu);
                        //Is this segment long enough to contain the full lookahead distance?
                        if (distSoFar > goalPointDistance)
                        {
                            //treat current segment like an AB Line
                            goalPointCu.easting = rEastCu - (Math.Sin(curList[A].heading) * goalPointDistance);
                            goalPointCu.northing = rNorthCu - (Math.Cos(curList[A].heading) * goalPointDistance);
                        }

                        //multiple segments required
                        else
                        {
                            //cycle thru segments and keep adding lengths. check if start and break if so.
                            while (A > 0)
                            {
                                B--; A--;
                                tempDist = Glm.Distance(curList[B], curList[A]);

                                //will we go too far?
                                if ((tempDist + distSoFar) > goalPointDistance) break; //tempDist contains the full length of next segment
                                else distSoFar += tempDist;
                            }

                            double t = (goalPointDistance - distSoFar); // the remainder to yet travel
                            t /= tempDist;

                            goalPointCu.easting = (((1 - t) * curList[B].easting) + (t * curList[A].easting));
                            goalPointCu.northing = (((1 - t) * curList[B].northing) + (t * curList[A].northing));
                        }
                    }
                    else
                    {
                        //counting up
                        isABSameAsVehicleHeading = true;
                        distSoFar = Glm.Distance(curList[B], rEastCu, rNorthCu);

                        //Is this segment long enough to contain the full lookahead distance?
                        if (distSoFar > goalPointDistance)
                        {
                            //treat current segment like an AB Line
                            goalPointCu.easting = rEastCu + (Math.Sin(curList[A].heading) * goalPointDistance);
                            goalPointCu.northing = rNorthCu + (Math.Cos(curList[A].heading) * goalPointDistance);
                        }

                        //multiple segments required
                        else
                        {
                            //cycle thru segments and keep adding lengths. check if end and break if so.
                            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                            while (B < ptCount - 1)
                            {
                                B++; A++;
                                tempDist = Glm.Distance(curList[B], curList[A]);

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

                            goalPointCu.easting = (((1 - t) * curList[A].easting) + (t * curList[B].easting));
                            goalPointCu.northing = (((1 - t) * curList[A].northing) + (t * curList[B].northing));
                        }
                    }

                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = Glm.DistanceSquared(goalPointCu.northing, goalPointCu.easting, pivot.northing, pivot.easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading = Glm.twoPI - mf.fixHeading;
                    ppRadiusCu = goalPointDistanceSquared / (2 * (((goalPointCu.easting - pivot.easting) * Math.Cos(localHeading)) + ((goalPointCu.northing - pivot.northing) * Math.Sin(localHeading))));

                    steerAngleCu = Glm.ToDegrees(Math.Atan(2 * (((goalPointCu.easting - pivot.easting) * Math.Cos(localHeading))
                        + ((goalPointCu.northing - pivot.northing) * Math.Sin(localHeading))) * mf.vehicle.wheelbase / goalPointDistanceSquared));

                    if (steerAngleCu < -mf.vehicle.maxSteerAngle) steerAngleCu = -mf.vehicle.maxSteerAngle;
                    if (steerAngleCu > mf.vehicle.maxSteerAngle) steerAngleCu = mf.vehicle.maxSteerAngle;

                    if (ppRadiusCu < -500) ppRadiusCu = -500;
                    if (ppRadiusCu > 500) ppRadiusCu = 500;

                    radiusPointCu.easting = pivot.easting + (ppRadiusCu * Math.Cos(localHeading));
                    radiusPointCu.northing = pivot.northing + (ppRadiusCu * Math.Sin(localHeading));

                    //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
                    double angVel = Glm.twoPI * 0.277777 * mf.pn.speed * (Math.Tan(Glm.ToRadians(steerAngleCu))) / mf.vehicle.wheelbase;

                    //clamp the steering angle to not exceed safe angular velocity
                    if (Math.Abs(angVel) > mf.vehicle.maxAngularVelocity)
                    {
                        steerAngleCu = Glm.ToDegrees(steerAngleCu > 0 ?
                                (Math.Atan((mf.vehicle.wheelbase * mf.vehicle.maxAngularVelocity) / (Glm.twoPI * mf.pn.speed * 0.277777)))
                            : (Math.Atan((mf.vehicle.wheelbase * -mf.vehicle.maxAngularVelocity) / (Glm.twoPI * mf.pn.speed * 0.277777))));
                    }
                    //Convert to centimeters
                    distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);

                    //distance is negative if on left, positive if on right
                    //if you're going the opposite direction left is right and right is left
                    //double temp;
                    if (isABSameAsVehicleHeading)
                    {
                        //temp = (abHeading);
                        if (!isOnRightSideCurrentLine) distanceFromCurrentLine *= -1.0;
                    }

                    //opposite way so right is left
                    else
                    {
                        //temp = (abHeading - Math.PI);
                        //if (temp < 0) temp = (temp + glm.twoPI);
                        //temp = glm.toDegrees(temp);
                        if (isOnRightSideCurrentLine) distanceFromCurrentLine *= -1.0;
                    }
                }

                mf.guidanceLineDistanceOff = mf.distanceDisplay = (Int16)distanceFromCurrentLine;
                mf.guidanceLineSteerAngle = (Int16)(steerAngleCu * 100);

                if (mf.yt.isYouTurnTriggered)
                {
                    //do the pure pursuit from youTurn
                    mf.yt.DistanceFromYouTurnLine();
                    mf.seq.DoSequenceEvent();

                    //now substitute what it thinks are AB line values with auto turn values
                    steerAngleCu = mf.yt.steerAngleYT;
                    distanceFromCurrentLine = mf.yt.distanceFromCurrentLine;

                    goalPointCu = mf.yt.goalPointYT;
                    radiusPointCu.easting = mf.yt.radiusPointYT.easting;
                    radiusPointCu.northing = mf.yt.radiusPointYT.northing;
                    ppRadiusCu = mf.yt.ppRadiusYT;
                }
            }
            else
            {
                //invalid distance so tell AS module
                distanceFromCurrentLine = 32000;
                mf.guidanceLineDistanceOff = 32000;
            }
        }

        public void SnapABCurve()
        {
            double headingAt90;

            //calculate the heading 90 degrees to ref ABLine heading
            if (isOnRightSideCurrentLine)
            {
                headingAt90 = Glm.PIBy2;
            }
            else
            {
                headingAt90 = -Glm.PIBy2;
            }

            if (isABSameAsVehicleHeading)
            {
                moveDistance += distanceFromCurrentLine * 0.001;
            }
            else
            {
                moveDistance -= distanceFromCurrentLine * 0.001;
            }


            int cnt = curList.Count;
            Vec3[] arr = new Vec3[cnt];
            curList.CopyTo(arr);
            refList.Clear();

            for (int i = 0; i < cnt; i++)
            {
                arr[i].easting = (Math.Sin(headingAt90 + arr[i].heading) * Math.Abs(distanceFromCurrentLine) * 0.001) + arr[i].easting;
                arr[i].northing = (Math.Cos(headingAt90 + arr[i].heading) * Math.Abs(distanceFromCurrentLine) * 0.001) + arr[i].northing;
                refList.Add(arr[i]);
            }
        }

        public void MoveABCurve(double dist)
        {
            double headingAt90;

            //calculate the heading 90 degrees to ref ABLine heading

            if (isABSameAsVehicleHeading)
            {
                headingAt90 = Glm.PIBy2;
                moveDistance += dist;
            }
            else
            {
                headingAt90 = -Glm.PIBy2;
                moveDistance -= dist;
            }

            int cnt = refList.Count;
            Vec3[] arr = new Vec3[cnt];
            refList.CopyTo(arr);
            refList.Clear();

            for (int i = 0; i < cnt; i++)
            {
                arr[i].easting = (Math.Sin(headingAt90 + arr[i].heading) * dist) + arr[i].easting;
                arr[i].northing = (Math.Cos(headingAt90 + arr[i].heading) * dist) + arr[i].northing;
                refList.Add(arr[i]);
            }
        }

        public bool PointOnLine(Vec3 pt1, Vec3 pt2, Vec3 pt)
        {
            var r = new Vec2(0, 0);
            if (pt1.northing == pt2.northing && pt1.easting == pt2.easting) { pt1.northing -= 0.00001; }

            var U = ((pt.northing - pt1.northing) * (pt2.northing - pt1.northing)) + ((pt.easting - pt1.easting) * (pt2.easting - pt1.easting));

            var Udenom = Math.Pow(pt2.northing - pt1.northing, 2) + Math.Pow(pt2.easting - pt1.easting, 2);

            U /= Udenom;

            r.northing = pt1.northing + (U * (pt2.northing - pt1.northing));
            r.easting = pt1.easting + (U * (pt2.easting - pt1.easting));

            double minx, maxx, miny, maxy;

            minx = Math.Min(pt1.northing, pt2.northing);
            maxx = Math.Max(pt1.northing, pt2.northing);

            miny = Math.Min(pt1.easting, pt2.easting);
            maxy = Math.Max(pt1.easting, pt2.easting);
            return _ = r.northing >= minx && r.northing <= maxx && (r.easting >= miny && r.easting <= maxy);
        }

        //add extensons
        public void AddFirstLastPoints()
        {
            int ptCnt = refList.Count - 1;
            for (int i = 1; i < 200; i++)
            {
                Vec3 pt = new Vec3(refList[ptCnt]);
                pt.easting += (Math.Sin(pt.heading) * i);
                pt.northing += (Math.Cos(pt.heading) * i);
                refList.Add(pt);
            }

            //and the beginning
            Vec3 start = new Vec3(refList[0]);
            for (int i = 1; i < 200; i++)
            {
                Vec3 pt = new Vec3(start);
                pt.easting -= (Math.Sin(pt.heading) * i);
                pt.northing -= (Math.Cos(pt.heading) * i);
                refList.Insert(0, pt);
            }
        }

        public void ResetCurveLine()
        {
            curList?.Clear();
            refList?.Clear();
            isCurveSet = false;
            isOkToAddPoints = false;
        }

        ////draw the guidance line
    }

    public class CCurveLines
    {
        public List<Vec3> curvePts = new List<Vec3>();
        public double aveHeading = 3;
        public string Name = "aa";
        public bool spiralmode = false;
        public bool circlemode = false;
    }

}