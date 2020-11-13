//Please, if you use this, share the improvements
using System;
using System.Collections.Generic;
using System.Text;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //very first fix to setup grid etc
        public bool isGPSPositionInitialized = false;

        double xTrackCorrection, abFixHeadingDelta;
        //string to record fixes for elevation maps
        public StringBuilder sbFix = new StringBuilder();

        // autosteer variables for sending serial
        public Int16 guidanceLineDistanceOff, guidanceLineSteerAngle, distanceDisplay;
        public Vec2 GoalPoint = new Vec2(0, 0), radiusPoint = new Vec2(0, 0);
        public double distanceFromCurrentLine, steerAngle, rEast, rNorth, ppRadius;
        public int currentLocationIndex, A, B;

        //how many fix updates per sec
        public int fixUpdateHz = 8;
        public double fixUpdateTime = 0.125;

        //for heading or Atan2 as camera
        public string headingFromSource, headingFromSourceBak;

        public Vec3 pivotAxlePos = new Vec3(0, 0, 0);
        public Vec3 steerAxlePos = new Vec3(0, 0, 0);

        //headings
        public double fixHeading = 0.0, camHeading = 0.0, gpsHeading = 0.0;


        //a distance between previous and current fix
        public double treeSpacingCounter = 0.0;
        public byte treeTrigger = 0x00;

        //how far travelled since last section was added, section points
        double sectionTriggerStepDistance = 0;
        public Vec2 prevSectionPos = new Vec2(0, 0);


        public bool NotLoadedField = true;

        public Vec2 prevBoundaryPos = new Vec2(0, 0);

        //are we still getting valid data from GPS, resets to 0 in NMEA OGI block, watchdog 
        public int recvCounter = 0;

        //Everything is so wonky at the start
        int startCounter = 50;

        //individual points for the flags in a list
        public List<CFlag> flagPts = new List<CFlag>();

        //tally counters for display
        //public double totalSquareMetersWorked = 0, totalUserSquareMeters = 0, userSquareMetersAlarm = 0;

        public double avgSpeed;//for average speed
        public int crossTrackError;

        //youturn
        public double distancePivotToTurnLine = -2222;
        public double distanceToolToTurnLine = -2222;

        //the value to fill in you turn progress bar
        public int youTurnProgressBar = 0, DualAntennaDistance = 140;

        //IMU 
        public double rollCorrectionDistance = 0;
        double gyroCorrection, gyroCorrected;

        //step position - slow speed spinner killer
        private int totalFixSteps = 60, currentStepFix = 0;
        public Vec3[] stepFixPts = new Vec3[60];
        public double distanceCurrentStepFix = 0, fixStepDist, minFixStepDist = 0, HeadingCorrection = 0;

        public double nowHz = 0;

        public bool isRTK;

        //called by timer every 15 ms
        private void ScanForNMEA_Tick(object sender, EventArgs e)
        {
            NMEAWatchdog.Enabled = false;
            pn.ParseNMEA();

            if (pn.UpdatedLatLon)
            {
                //reset  flag
                pn.UpdatedLatLon = false;

                //Measure the frequency of the GPS updates
                swHz.Stop();
                nowHz = ((double)System.Diagnostics.Stopwatch.Frequency) / (double)swHz.ElapsedTicks;

                //simple comp filter
                if (nowHz < 20) HzTime = 0.95 * HzTime + 0.05 * nowHz;
                fixUpdateTime = 1.0 / (double)HzTime;

                swHz.Reset();
                swHz.Start();

                //start the watch and time till it finishes
                swFrame.Reset();
                swFrame.Start();

                //update all data for new frame
                UpdateFixPosition();
                recvCounter = 0;
            }
            else if (recvCounter++ > 133)
            {
                ShowNoGPSWarning();
            }
            NMEAWatchdog.Enabled = true;
        }

        public double rollUsed;
        public double headlandDistanceDelta = 0, boundaryDistanceDelta = 0;

        private void UpdateFixPosition()
        {
            if (startCounter > 0) startCounter--;

            if (!isGPSPositionInitialized)
            {
                InitializeFirstFewGPSPositions();
                return;
            }

            //grab the most current fix and save the distance from the last fix
            distanceCurrentStepFix = Glm.Distance(pn.fix, stepFixPts[0]);

            #region Heading

            if ((pn.HeadingForced != 9999 && (headingFromSource == "GPS" || headingFromSource == "Dual")) || timerSim.Enabled)
            {
                stepFixPts[0].Heading = distanceCurrentStepFix;
                stepFixPts[0].Easting = pn.fix.Easting;
                stepFixPts[0].Northing = pn.fix.Northing;
                if ((fd.distanceUser += distanceCurrentStepFix) > 3000) fd.distanceUser %= 3000; ;//userDistance can be reset

                fixHeading = Glm.ToRadians(pn.HeadingForced);
                camHeading = pn.HeadingForced;
                gpsHeading = Glm.ToRadians(pn.HeadingForced);
            }
            else
            {
                if (distanceCurrentStepFix > minFixStepDist / totalFixSteps)
                {
                    for (int i = totalFixSteps - 1; i > 0; i--) stepFixPts[i] = stepFixPts[i - 1];

                    //**** heading of the vec3 structure is used for distance in Step fix!!!!!
                    stepFixPts[0].Heading = distanceCurrentStepFix;
                    stepFixPts[0].Easting = pn.fix.Easting;
                    stepFixPts[0].Northing = pn.fix.Northing;

                    fd.distanceUser += distanceCurrentStepFix;
                    fd.distanceUser %= 3000;
                }

                fixStepDist = 0;
                for (currentStepFix = 0; currentStepFix < totalFixSteps - 1; currentStepFix++)
                {
                    fixStepDist += stepFixPts[currentStepFix].Heading;
                    if (fixStepDist >= minFixStepDist)//combined points > minFixStepDist, so now we can change heading?//no need to fuse headings of all points?????
                    {
                        gpsHeading = Math.Atan2(pn.fix.Easting - stepFixPts[currentStepFix + 1].Easting, pn.fix.Northing - stepFixPts[currentStepFix + 1].Northing);
                        if (gpsHeading < 0) gpsHeading += Glm.twoPI;
                        fixHeading = gpsHeading;

                        //determine fix positions and heading in degrees for glRotate opengl methods.
                        int camStep = (currentStepFix + 1) * 2;
                        if (camStep > (totalFixSteps - 1)) camStep = (totalFixSteps - 1);
                        camHeading = Math.Atan2(pn.fix.Easting - stepFixPts[camStep].Easting, pn.fix.Northing - stepFixPts[camStep].Northing);
                        if (camHeading < 0) camHeading += Glm.twoPI;

                        camHeading = Glm.ToDegrees(gpsHeading);
                        break;
                    }
                }
            }
            #endregion Heading

            #region Heading Correction
            //an IMU with heading correction, add the correction
            if (ahrs.correctionHeadingX16 != 9999 && (ahrs.isHeadingCorrectionFromBrick || ahrs.isHeadingCorrectionFromAutoSteer)) //| ahrs.isHeadingCorrectionFromExtUDP
            {
                //current gyro angle in radians
                double correctionHeading = Glm.ToRadians((double)ahrs.correctionHeadingX16 * 0.0625);

                //Difference between the IMU heading and the GPS heading
                double gyroDelta = (correctionHeading + gyroCorrection) - gpsHeading;
                if (gyroDelta < 0) gyroDelta += Glm.twoPI;

                //calculate delta based on circular data problem 0 to 360 to 0, clamp to +- 2 Pi
                if (gyroDelta >= -Glm.PIBy2 && gyroDelta <= Glm.PIBy2) gyroDelta *= -1.0;
                else
                {
                    if (gyroDelta > Glm.PIBy2) { gyroDelta = Glm.twoPI - gyroDelta; }
                    else { gyroDelta = (Glm.twoPI + gyroDelta) * -1.0; }
                }
                gyroDelta %= Glm.twoPI;

                //if the gyro and last corrected fix is < 10 degrees, super low pass for gps
                if (Math.Abs(gyroDelta) < 0.18)
                {
                    //a bit of delta and add to correction to current gyro
                    gyroCorrection += (gyroDelta * (0.25 / HzTime)) % Glm.twoPI;
                }
                else
                {
                    //a bit of delta and add to correction to current gyro
                    gyroCorrection += (gyroDelta * (2.0 / HzTime)) % Glm.twoPI;
                }

                //determine the Corrected heading based on gyro and GPS
                gyroCorrected = (correctionHeading + gyroCorrection) % Glm.twoPI;
                if (gyroCorrected < 0) gyroCorrected += Glm.twoPI;

                fixHeading = gyroCorrected;
            }

            #endregion Heading Correction

            #region Antenna Offset
            if (vehicle.antennaOffset != 0)
            {
                pn.fix.Easting = (Math.Cos(-fixHeading) * vehicle.antennaOffset) + pn.fix.Easting;
                pn.fix.Northing = (Math.Sin(-fixHeading) * vehicle.antennaOffset) + pn.fix.Northing;
            }
            #endregion

            #region Roll

            rollUsed = 0;
            //used only for draft compensation in OGI Sentence
            if (ahrs.rollX16 != 9999 && ahrs.isRollFromOGI) rollUsed = ((double)(ahrs.rollX16 - ahrs.rollZeroX16)) * 0.0625;
            else if (ahrs.rollX16 != 9999 && (ahrs.isRollFromAutoSteer || ahrs.isRollFromAVR))
            {
                rollUsed = ((double)(ahrs.rollX16 - ahrs.rollZeroX16)) * 0.0625;

                //change for roll to the right is positive times -1
                rollCorrectionDistance = Math.Sin(Glm.ToRadians(rollUsed)) * -vehicle.antennaHeight;

                // roll to left is positive  **** important!!
                // not any more - April 30, 2019 - roll to right is positive Now! Still Important
                pn.fix.Easting = (Math.Cos(-fixHeading) * rollCorrectionDistance) + pn.fix.Easting;
                pn.fix.Northing = (Math.Sin(-fixHeading) * rollCorrectionDistance) + pn.fix.Northing;
            }

            #endregion Roll

            #region Step Fix

            CalculatePositionHeading();

            //test if travelled far enough for new boundary point
            if (Glm.Distance(pn.fix, prevBoundaryPos) > 1) AddBoundaryPoint();

            //tree spacing
            if (vehicle.treeSpacing != 0 && (Tools[0].Sections[0].IsSectionOn || Tools[0].Sections[Tools[0].numOfSections].IsSectionOn) && (treeSpacingCounter += (distanceCurrentStepFix * 200)) > vehicle.treeSpacing)
            {
                treeSpacingCounter %= vehicle.treeSpacing;//keep the distance below spacing
                mc.Send_Treeplant[3] = (treeTrigger ^= 0x01);
                DataSend[8] = "Tree Plant: State " + ((treeTrigger == 0x01) ? "On" : "Off");
                SendData(mc.Send_Treeplant, false);
            }
            else if (vehicle.treeSpacing == 0 && mc.Send_Treeplant[3] == 0x01)
            {
                mc.Send_Treeplant[3] = 0;
                DataSend[8] = "Tree Plant: State Off";
                SendData(mc.Send_Treeplant, false);
            }

            //test if travelled far enough for new Section point, To prevent drawing high numbers of triangles
            if (isJobStarted && Glm.Distance(pn.fix, prevSectionPos) > sectionTriggerStepDistance)
            {
                //save the north & east as previous
                prevSectionPos.Northing = pn.fix.Northing;
                prevSectionPos.Easting = pn.fix.Easting;

                if (recPath.isRecordOn)
                {
                    //keep minimum speed of 1.0
                    double speed = pn.speed;
                    if (pn.speed < 1.0) speed = 1.0;
                    bool autoBtn = (autoBtnState == btnStates.Auto);

                    CRecPathPt pt = new CRecPathPt(steerAxlePos.Easting, steerAxlePos.Northing, steerAxlePos.Heading, pn.speed, autoBtn);
                    recPath.recList.Add(pt);
                }

                if (CurveLines.isOkToAddPoints && CurveLines.CurrentEditLine < CurveLines.Lines.Count && CurveLines.CurrentEditLine > -1)
                {
                    Vec3 pt = new Vec3(pivotAxlePos.Northing, pivotAxlePos.Easting, pivotAxlePos.Heading);
                    CurveLines.Lines[CurveLines.CurrentEditLine].curvePts.Add(pt);
                }

                // if non zero, at least one section is on.
                int sectionCounter = 0;

                for (int i = 0; i < Tools.Count; i++)
                {
                    //send the current and previous GPS fore/aft corrected fix to each section
                    for (int j = 0; j < Tools[i].Sections.Count; j++)
                    {
                        if (Tools[i].Sections[j].IsMappingOn)
                        {
                            Tools[i].Sections[j].AddMappingPoint();
                            sectionCounter++;
                        }
                    }
                }

                //add point if no autosteer and section enabled?
                if ((!ct.isContourBtnOn && isAutoSteerBtnOn) || sectionCounter == 0)
                {
                    if (ct.isContourOn) ct.StopContourLine(steerAxlePos);
                }
                else if (!isAutoSteerBtnOn)
                {
                    ct.AddPoint(pivotAxlePos, ct.isContourOn);
                }

                //grab fix and elevation
                if (isLogElevation) sbFix.Append(pn.fix.Easting.ToString("N2") + "," + pn.fix.Northing.ToString("N2") + ","
                                                    + pn.altitude.ToString("N2") + ","
                                                    + pn.latitude + "," + pn.longitude + "\r\n");
            }

            #endregion fix

            #region AutoSteer

            //preset the values
            guidanceLineDistanceOff = 32000;

            if (ct.isContourBtnOn)
            {
                ct.DistanceFromContourLine(pivotAxlePos, steerAxlePos);
            }
            else if (ABLines.BtnABLineOn)
            {
                ABLines.GetCurrentABLine(pivotAxlePos, steerAxlePos);
                if (yt.isRecordingCustomYouTurn)
                {
                    //save reference of first point
                    if (yt.youFileList.Count == 0)
                    {
                        Vec2 start = new Vec2(steerAxlePos.Northing, steerAxlePos.Easting);
                        yt.youFileList.Add(start);
                    }
                    else
                    {
                        //keep adding points
                        Vec2 point = new Vec2(steerAxlePos.Northing - yt.youFileList[0].Northing, steerAxlePos.Easting - yt.youFileList[0].Easting);
                        yt.youFileList.Add(point);
                    }
                }
            }
            else if (CurveLines.BtnCurveLineOn)
            {
                CurveLines.GetCurrentCurveLine(pivotAxlePos, steerAxlePos);
            }

            // autosteer at full speed of updates
            if (!isAutoSteerBtnOn) //32020 means auto steer is off
            {
                guidanceLineDistanceOff = 32020;
            }

            //if the whole path driving driving process is green
            if (recPath.isDrivingRecordedPath) recPath.UpdatePosition();


            //sidehill draft compensation
            if (rollUsed != 0)
            {
                guidanceLineSteerAngle = (Int16)(guidanceLineSteerAngle +
                    ((-rollUsed) * ((double)mc.Config_AutoSteer[mc.ssKd] / 50)) * 500);
            }

            pn.speed.LimitToRange(-163, 163);

            mc.Send_AutoSteer[3] = unchecked((byte)((int)(pn.speed * 200.0) >> 8));
            mc.Send_AutoSteer[4] = unchecked((byte)(pn.speed * 200.0));
            mc.Send_AutoSteer[5] = unchecked((byte)(guidanceLineDistanceOff >> 8));
            mc.Send_AutoSteer[6] = unchecked((byte)(guidanceLineDistanceOff));
            mc.Send_AutoSteer[7] = unchecked((byte)(guidanceLineSteerAngle >> 8));
            mc.Send_AutoSteer[8] = unchecked((byte)(guidanceLineSteerAngle));

            if (TestAutoSteer)
            {
                //32030
                mc.Send_AutoSteer[5] = 0x7D;
                mc.Send_AutoSteer[6] = 0x1E;
            }


            DataSend[8] = "Auto Steer: Speed " + ((int)(pn.speed * 200.0)/200.0).ToString("N2") + ", Distance " + guidanceLineDistanceOff.ToString() + ", Angle " + guidanceLineSteerAngle.ToString();
            SendData(mc.Send_AutoSteer, false);

            //for average cross track error
            if (guidanceLineDistanceOff < 29000)
            {
                crossTrackError = (int)((double)crossTrackError * 0.90 + Math.Abs((double)guidanceLineDistanceOff) * 0.1);
            }
            else crossTrackError = 0;

            #endregion

            #region Youturn

            //reset the fault distance to an appropriate weird number
            //-2222 means it fell out of the loop completely
            //-3333 means unable to find a nearest point at all even though inside the work area of field
            // -4444 means cross trac error too high
            distancePivotToTurnLine = -4444;

            //always force out of bounds and change only if in bounds after proven so
            //mc.isOutOfBounds = true;

            //if an outer boundary is set, then apply critical stop logic
            if (bnd.bndArr.Count > 0)
            {
                //Are we inside outer and outside inner all turn boundaries, no turn creation problems
                if (!yt.isTurnCreationTooClose && !yt.isTurnCreationNotCrossingError && IsInsideGeoFence() )
                {
                    NotLoadedField = true;
                    //reset critical stop for bounds violation
                    if (mc.isOutOfBounds)
                    {
                        mc.Send_Uturn[3] |= 0x80;
                        mc.isOutOfBounds = false;
                        DataSend[8] = "Uturn: " + Convert.ToString(mc.Send_Uturn[3], 2).PadLeft(6, '0');
                        SendData(mc.Send_Uturn, false);
                    }

                    //do the auto youturn logic if everything is on.
                    if (yt.isYouTurnBtnOn && isAutoSteerBtnOn)
                    {
                        //if we are too much off track > 1.3m, kill the diagnostic creation, start again
                        if (crossTrackError > 1000 && !yt.isYouTurnTriggered)
                        {
                            yt.ResetCreatedYouTurn();
                        }
                        else
                        {
                            //now check to make sure we are not in an inner turn boundary - drive thru is ok
                            if (yt.youTurnPhase == -1) yt.youTurnPhase++;
                            else if (yt.youTurnPhase != 3)
                            {
                                if (crossTrackError > 1000)
                                {
                                    yt.ResetCreatedYouTurn();
                                }
                                else
                                {
                                    if (yt.YouTurnType == 0)
                                    {
                                        if (ABLines.BtnABLineOn) yt.BuildABLinePatternYouTurn(yt.isYouTurnRight);
                                        else yt.BuildCurvePatternYouTurn(yt.isYouTurnRight, pivotAxlePos);
                                    }
                                    else if (yt.YouTurnType == 2 && ABLines.BtnABLineOn)
                                    {
                                        yt.BuildABLineCurveYouTurn(yt.isYouTurnRight);
                                    }
                                    else
                                    {
                                        if (ABLines.BtnABLineOn) yt.BuildABLineDubinsYouTurn(yt.isYouTurnRight);
                                        else yt.BuildCurveDubinsYouTurn(yt.isYouTurnRight, pivotAxlePos);
                                    }
                                }
                            }
                            else if (yt.ytList.Count > 1)//wait to trigger the actual turn since its made and waiting
                            {
                                //distance from current pivot to first point of youturn pattern
                                distancePivotToTurnLine = Glm.Distance(yt.ytList[0], steerAxlePos);

                                if ((distancePivotToTurnLine <= 20.0) && (distancePivotToTurnLine >= 18.0) && !yt.isYouTurnTriggered)

                                    if (!isBoundAlarming)
                                    {
                                        SndBoundaryAlarm.Play();
                                        isBoundAlarming = true;
                                    }

                                //if we are close enough to pattern, trigger.
                                if ((distancePivotToTurnLine <= 10.0) && (distancePivotToTurnLine >= 0) && !yt.isYouTurnTriggered)
                                {
                                    double dx = yt.ytList[1].Northing - yt.ytList[0].Northing;
                                    double dy = yt.ytList[1].Easting - yt.ytList[0].Easting;
                                    double Time = ((steerAxlePos.Northing - yt.ytList[0].Northing) * dx + (steerAxlePos.Easting - yt.ytList[0].Easting) * dy) / (dx * dx + dy * dy);

                                    if (Time > 0)
                                    {
                                        yt.YouTurnTrigger();
                                        isBoundAlarming = false;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        //now check to make sure we are not in an inner turn boundary - drive thru is ok
                        if (distanceFromCurrentLine > 1300 && !yt.isYouTurnTriggered)
                        {
                            yt.ResetCreatedYouTurn();
                        }
                    }// end of isInWorkingArea

                }
                // here is stop logic for out of bounds - in an inner or out the outer turn border.
                else
                {
                    if (!mc.isOutOfBounds)
                    {
                        mc.Send_Uturn[3] &= 0x7F;
                        mc.isOutOfBounds = true;
                        DataSend[8] = "Uturn: " + Convert.ToString(mc.Send_Uturn[3], 2).PadLeft(6, '0');
                        SendData(mc.Send_Uturn, false);
                    }

                    if (yt.isYouTurnBtnOn)
                    {
                        yt.ResetCreatedYouTurn();
                        sim.stepDistance = 0;
                    }
                    CheckInsideOtherBoundary();
                }
            }
            else
            {
                if (mc.isOutOfBounds)
                {
                    mc.Send_Uturn[3] |= 0x80;
                    mc.isOutOfBounds = false;
                    DataSend[8] = "Uturn: " + Convert.ToString(mc.Send_Uturn[3], 2).PadLeft(6, '0');
                    SendData(mc.Send_Uturn, false);
                }
                CheckInsideOtherBoundary();
            }

            #endregion

            //update Back
            oglBack.Refresh();

            //end of UppdateFixPosition

            swFrame.Stop();
            //stop the timer and calc how long it took to do calcs and draw
            FrameTime = (double)swFrame.ElapsedTicks / (double)System.Diagnostics.Stopwatch.Frequency * 1000;

            //Update Main window
            oglMain.Refresh();
        }

        public void CalculateSteerAngle(ref List<Vec3> Points2, bool isSameWay, bool Loop = false)
        {
            List<Vec3> Points = (yt.isYouTurnTriggered) ? yt.ytList : Points2;

            if (Points.Count > 1)
            {
                bool useSteer = isStanleyUsed;

                if (!vehicle.isSteerAxleAhead) useSteer = !useSteer;
                bool isreversedriving = pn.speed < -0.09;
                if (isreversedriving) useSteer = !useSteer;

                Vec3 point = (useSteer) ? steerAxlePos : pivotAxlePos;


                double minDistA = double.PositiveInfinity;

                for (int t = 0; t < Points.Count; t++)
                {
                    double dist = ((point.Easting - Points[t].Easting) * (point.Easting - Points[t].Easting))
                                    + ((point.Northing - Points[t].Northing) * (point.Northing - Points[t].Northing));
                    if (dist < minDistA)
                    {
                        minDistA = dist;
                        A = t;
                    }
                }

                if (minDistA == double.PositiveInfinity) return;
                double Time = -1;

                if ((!Loop && A+1 < Points.Count) || Loop)
                {
                    B = (A + 1).Clamp(Points.Count);
                    double dx = Points[B].Northing - Points[A].Northing;
                    double dy = Points[B].Easting - Points[A].Easting;
                    Time = ((point.Northing - Points[A].Northing) * dx + (point.Easting - Points[A].Easting) * dy) / (dx * dx + dy * dy);
                }
                if (Time < 0 && ((!Loop && A > 0) || Loop))
                {
                    B = (A - 1).Clamp(Points.Count);
                    double dx = Points[B].Northing - Points[A].Northing;
                    double dy = Points[B].Easting - Points[A].Easting;
                    Time = ((point.Northing - Points[A].Northing) * dx + (point.Easting - Points[A].Easting) * dy) / (dx * dx + dy * dy);
                }

                if (yt.isYouTurnTriggered)
                {
                    isSameWay = true;
                    //used for sequencing to find entry, exit positioning
                    double calc = 100 / yt.ytLength;
                    double ytLength = 0;

                    for (int i = 0; i+1 < yt.ytList.Count && i+1 < (A > B ? A : B); i++)
                    {
                        ytLength += Glm.Distance(yt.ytList[i], yt.ytList[i + 1]);
                    }
                    ytLength += Glm.Distance(yt.ytList[A > B ? B : A], point);
                    
                    yt.onA = ytLength;

                    //return and reset if too far away or end of the line
                    if (A >= Points.Count - 1)
                    {
                        seq.DoSequenceEvent(true);
                        yt.CompleteYouTurn();
                        return;
                    }
                    else seq.DoSequenceEvent(false);
                }

                //just need to make sure the points continue ascending or heading switches all over the place
                if (A > B) { int C = A; A = B; B = C; }

                if (Loop && A == 0 && B == Points.Count - 1) { int C = A; A = B; B = C; }

                currentLocationIndex = A;

                //get the distance from currently active AB line
                double Dx = Points[B].Northing - Points[A].Northing;
                double Dy = Points[B].Easting - Points[A].Easting;

                if (Math.Abs(Dy) < double.Epsilon && Math.Abs(Dx) < double.Epsilon) return;

                //how far from current AB Line is fix
                distanceFromCurrentLine = ((Dx * point.Easting) - (Dy * point.Northing) + (Points[B].Easting
                            * Points[A].Northing) - (Points[B].Northing * Points[A].Easting))
                                / Math.Sqrt((Dx * Dx) + (Dy * Dy));

                // ** Pure pursuit ** - calc point on ABLine closest to current position
                double U = (((point.Easting - Points[A].Easting) * Dy)
                            + ((point.Northing - Points[A].Northing) * Dx))
                            / ((Dy * Dy) + (Dx * Dx));

                rEast = Points[A].Easting + (U * Dy);
                rNorth = Points[A].Northing + (U * Dx);

                if (isStanleyUsed)
                {
                    //Points[A].Heading = Math.Atan2(Dy, Dx);
                    abFixHeadingDelta = fixHeading - Points[A].Heading;

                    if (!isSameWay)
                    {
                        distanceFromCurrentLine *= -1.0;
                        abFixHeadingDelta += Math.PI;
                    }

                    //Fix the circular error
                    while (abFixHeadingDelta > Math.PI) abFixHeadingDelta -= Glm.twoPI;
                    while (abFixHeadingDelta < -Math.PI) abFixHeadingDelta += Glm.twoPI;


                    vehicle.avgDist = (1 - vehicle.avgXTE) * distanceFromCurrentLine + vehicle.avgXTE * vehicle.avgDist;
                    distanceFromCurrentLine = vehicle.avgDist;
                    double calc = ((Math.Abs(pn.speed * 0.277777)) + 2);

                    xTrackCorrection = Math.Cos(abFixHeadingDelta) * Math.Atan((distanceFromCurrentLine * vehicle.stanleyGain) / calc);

                    abFixHeadingDelta *= vehicle.stanleyHeadingErrorGain;
                    if (abFixHeadingDelta > 0.74) abFixHeadingDelta = 0.74;
                    if (abFixHeadingDelta < -0.74) abFixHeadingDelta = -0.74;

                    steerAngle = Glm.ToDegrees((xTrackCorrection + (pn.speed > -0.1 ? abFixHeadingDelta : -abFixHeadingDelta)) * -1.0);

                    if (steerAngle < -vehicle.maxSteerAngle) steerAngle = -vehicle.maxSteerAngle;
                    if (steerAngle > vehicle.maxSteerAngle) steerAngle = vehicle.maxSteerAngle;
                    
                    //Integral
                    double deltaDeg = Math.Abs(Glm.ToDegrees(abFixHeadingDelta));
                    double integralSpeed = (pn.speed) / 10;

                    double distErr = Math.Abs(distanceFromCurrentLine);

                    if (deltaDeg < vehicle.integralHeadingLimit && distErr < vehicle.integralDistanceAway && pn.speed > 0.5 && isAutoSteerBtnOn)
                    {
                        if ((vehicle.inty < 0 && distanceFromCurrentLine < 0) || (vehicle.inty > 0 && distanceFromCurrentLine > 0))
                            vehicle.inty += distanceFromCurrentLine * -vehicle.stanleyIntegralGain * 3 * integralSpeed;
                        else vehicle.inty += distanceFromCurrentLine * -vehicle.stanleyIntegralGain
                                * integralSpeed * ((vehicle.integralHeadingLimit - deltaDeg) / vehicle.integralHeadingLimit);

                        if (vehicle.stanleyIntegralGain > 0) steerAngle += vehicle.inty;
                        else vehicle.inty = 0;
                    }
                    else
                    {
                        vehicle.inty = 0;
                    }
                }
                else
                {
                    //used for accumulating distance to find goal point
                    double distSoFar;

                    //update base on autosteer settings and distance from line
                    double goalPointDistance = vehicle.UpdateGoalPointDistance(distanceFromCurrentLine);

                    // used for calculating the length squared of next segment.
                    double tempDist = 0;

                    if (!isSameWay)
                    {
                        //counting down
                        distSoFar = Glm.Distance(Points[A], rEast, rNorth);
                        //Is this segment long enough to contain the full lookahead distance?
                        if (distSoFar > goalPointDistance)
                        {
                            //treat current segment like an AB Line
                            GoalPoint.Easting = rEast - (Math.Sin(Points[A].Heading) * goalPointDistance);
                            GoalPoint.Northing = rNorth - (Math.Cos(Points[A].Heading) * goalPointDistance);
                        }

                        //multiple segments required
                        else
                        {
                            //cycle thru segments and keep adding lengths. check if start and break if so.
                            while (A > 0 || !yt.isYouTurnTriggered)
                            {
                                B = (B - 1).Clamp(Points.Count);
                                A = (A - 1).Clamp(Points.Count);
                                tempDist = Glm.Distance(Points[B], Points[A]);

                                //will we go too far?
                                if ((tempDist + distSoFar) > goalPointDistance) break; //tempDist contains the full length of next segment
                                distSoFar += tempDist;
                            }

                            double t = (goalPointDistance - distSoFar); // the remainder to yet travel
                            t /= tempDist;

                            GoalPoint.Easting = (((1 - t) * Points[B].Easting) + (t * Points[A].Easting));
                            GoalPoint.Northing = (((1 - t) * Points[B].Northing) + (t * Points[A].Northing));
                        }
                    }
                    else
                    {
                        //counting up
                        distSoFar = Glm.Distance(Points[B], rEast, rNorth);

                        //Is this segment long enough to contain the full lookahead distance?
                        if (distSoFar > goalPointDistance)
                        {
                            //treat current segment like an AB Line
                            GoalPoint.Easting = rEast + (Math.Sin(Points[A].Heading) * goalPointDistance);
                            GoalPoint.Northing = rNorth + (Math.Cos(Points[A].Heading) * goalPointDistance);
                        }

                        //multiple segments required
                        else
                        {
                            //cycle thru segments and keep adding lengths. check if end and break if so.
                            // ReSharper disable once LoopVariableIsNeverChangedInsideLoop
                            while (B < Points.Count - 1 || !yt.isYouTurnTriggered)
                            {
                                B = (B + 1).Clamp(Points.Count);
                                A = (A + 1).Clamp(Points.Count);

                                tempDist = Glm.Distance(Points[B], Points[A]);

                                //will we go too far?
                                if ((tempDist + distSoFar) > goalPointDistance)
                                {
                                    break; //tempDist contains the full length of next segment
                                }
                                distSoFar += tempDist;
                            }

                            double t = (goalPointDistance - distSoFar); // the remainder to yet travel
                            t /= tempDist;

                            GoalPoint.Easting = (((1 - t) * Points[A].Easting) + (t * Points[B].Easting));
                            GoalPoint.Northing = (((1 - t) * Points[A].Northing) + (t * Points[B].Northing));
                        }
                    }

                    //calc "D" the distance from pivot axle to lookahead point
                    double goalPointDistanceSquared = Glm.DistanceSquared(GoalPoint.Northing, GoalPoint.Easting, pivotAxlePos.Northing, pivotAxlePos.Easting);

                    //calculate the the delta x in local coordinates and steering angle degrees based on wheelbase
                    double localHeading = Glm.twoPI - fixHeading;
                    ppRadius = goalPointDistanceSquared / (2 * (((GoalPoint.Easting - pivotAxlePos.Easting) * Math.Cos(localHeading)) + ((GoalPoint.Northing - pivotAxlePos.Northing) * Math.Sin(localHeading))));

                    steerAngle = Glm.ToDegrees(Math.Atan(2 * (((GoalPoint.Easting - pivotAxlePos.Easting) * Math.Cos(localHeading))
                        + ((GoalPoint.Northing - pivotAxlePos.Northing) * Math.Sin(localHeading))) * vehicle.wheelbase / goalPointDistanceSquared));

                    if (steerAngle < -vehicle.maxSteerAngle) steerAngle = -vehicle.maxSteerAngle;
                    if (steerAngle > vehicle.maxSteerAngle) steerAngle = vehicle.maxSteerAngle;

                    if (ppRadius < -500) ppRadius = -500;
                    if (ppRadius > 500) ppRadius = 500;

                    radiusPoint.Easting = pivotAxlePos.Easting + (ppRadius * Math.Cos(localHeading));
                    radiusPoint.Northing = pivotAxlePos.Northing + (ppRadius * Math.Sin(localHeading));

                    //angular velocity in rads/sec  = 2PI * m/sec * radians/meters
                    double angVel = Glm.twoPI * 0.277777 * pn.speed * (Math.Tan(Glm.ToRadians(steerAngle))) / vehicle.wheelbase;

                    //clamp the steering angle to not exceed safe angular velocity
                    if (Math.Abs(angVel) > vehicle.maxAngularVelocity)
                    {
                        steerAngle = Glm.ToDegrees(steerAngle > 0 ?
                            (Math.Atan((vehicle.wheelbase * vehicle.maxAngularVelocity) / (Glm.twoPI * pn.speed * 0.277777)))
                            : (Math.Atan((vehicle.wheelbase * -vehicle.maxAngularVelocity) / (Glm.twoPI * pn.speed * 0.277777))));
                    }

                    //distance is negative if on left, positive if on right
                    //if you're going the opposite direction left is right and right is left
                    //double temp;
                    if (!isSameWay) distanceFromCurrentLine *= -1.0;
                }

                //Convert to centimeters
                distanceFromCurrentLine = Math.Round(distanceFromCurrentLine * 1000.0, MidpointRounding.AwayFromZero);

                guidanceLineDistanceOff = distanceDisplay = (Int16)distanceFromCurrentLine;
                guidanceLineSteerAngle = (Int16)(steerAngle * 100);
            }
            else
            {
                if (yt.isYouTurnTriggered) yt.CompleteYouTurn();
                //invalid distance so tell AS module
                distanceFromCurrentLine = 32000;
                guidanceLineDistanceOff = 32000;
            }
        }

        private void CheckInsideOtherBoundary()
        {
            if (isAutoLoadFields && NotLoadedField)
            {
                double x = pn.actualEasting;
                double y = pn.actualNorthing;

                for (int i = 0; i < Fields.Count; i++)
                {
                    if (Fields[i].Dir != currentFieldDirectory || isJobStarted == false)
                    {
                        int j = Fields[i].Boundary.Count - 1;

                        bool oddNodes = false;

                        for (int k = 0; k < Fields[i].Boundary.Count; k++)
                        {
                            if ((Fields[i].Boundary[k].Northing < y && Fields[i].Boundary[j].Northing >= y
                            || Fields[i].Boundary[j].Northing < y && Fields[i].Boundary[k].Northing >= y)
                            && (Fields[i].Boundary[k].Easting <= x || Fields[i].Boundary[j].Easting <= x))
                            {
                                oddNodes ^= (Fields[i].Boundary[k].Easting + (y - Fields[i].Boundary[k].Northing) /
                                (Fields[i].Boundary[j].Northing - Fields[i].Boundary[k].Northing) * (Fields[i].Boundary[j].Easting - Fields[i].Boundary[k].Easting) < x);
                            }
                            j = k;
                        }

                        if (oddNodes)
                        {
                            NotLoadedField = false;
                            FileSaveEverythingBeforeClosingField();
                            FileOpenField(fieldsDirectory + Fields[i].Dir + "\\Field.txt");
                            break;
                        }
                    }
                }
            }
        }

        public bool isBoundAlarming;

        //all the hitch, pivot, section, trailing hitch, headings and fixes
        private void CalculatePositionHeading()
        {
            #region pivot hitch trail

            //translate from Gps position --> PivotAxle position --> SteerAxle position
            pivotAxlePos.Easting = pn.fix.Easting - (Math.Sin(fixHeading) * vehicle.antennaPivot);
            pivotAxlePos.Northing = pn.fix.Northing - (Math.Cos(fixHeading) * vehicle.antennaPivot);
            pivotAxlePos.Heading = fixHeading;
            steerAxlePos.Easting = pivotAxlePos.Easting + (Math.Sin(fixHeading) * vehicle.wheelbase);
            steerAxlePos.Northing = pivotAxlePos.Northing + (Math.Cos(fixHeading) * vehicle.wheelbase);
            steerAxlePos.Heading = fixHeading;

            sectionTriggerStepDistance = 10000;

            for (int i = 0; i < Tools.Count; i++)
            {
                //determine where the rigid vehicle hitch ends
                Tools[i].HitchPos.Northing = pn.fix.Northing + (Math.Cos(fixHeading) * (Tools[i].HitchLength - vehicle.antennaPivot));
                Tools[i].HitchPos.Easting = pn.fix.Easting + (Math.Sin(fixHeading) * (Tools[i].HitchLength - vehicle.antennaPivot));

                //tool attached via a trailing hitch
                if (Tools[i].isToolTrailing)
                {
                    double over;
                    if (Tools[i].isToolTBT)
                    {
                        //Torriem rules!!!!! Oh yes, this is all his. Thank-you
                        if (distanceCurrentStepFix != 0)
                        {
                            double t = (Tools[i].TankWheelLength) / distanceCurrentStepFix;
                            Vec2 tt = Tools[i].TankWheelPos - Tools[i].HitchPos;
                            Tools[i].TankWheelPos.Heading = Math.Atan2(t * tt.Easting, t * tt.Northing);
                        }
                        over = Math.Abs(Math.PI - Math.Abs(Math.Abs(Tools[i].TankWheelPos.Heading - fixHeading) - Math.PI));

                        if (over > 2.36 || startCounter > 0)//criteria for a forced reset to put tool directly behind vehicle
                        {
                            Tools[i].TankWheelPos.Heading = fixHeading;
                        }

                        double TankCos = Math.Cos(Tools[i].TankWheelPos.Heading);
                        double TankSin = Math.Sin(Tools[i].TankWheelPos.Heading);

                        Tools[i].TankWheelPos.Northing = Tools[i].HitchPos.Northing + TankCos * Tools[i].TankWheelLength;
                        Tools[i].TankWheelPos.Easting = Tools[i].HitchPos.Easting + TankSin * Tools[i].TankWheelLength;

                        Tools[i].TankHitchPos.Northing = Tools[i].TankWheelPos.Northing + TankCos * Tools[i].TankHitchLength;
                        Tools[i].TankHitchPos.Easting = Tools[i].TankWheelPos.Easting + TankSin * Tools[i].TankHitchLength;
                    }
                    else
                    {
                        Tools[i].TankWheelPos.Heading = fixHeading;
                        Tools[i].TankWheelPos.Northing = Tools[i].HitchPos.Northing;
                        Tools[i].TankWheelPos.Easting = Tools[i].HitchPos.Easting;
                        Tools[i].TankHitchPos.Northing = Tools[i].TankWheelPos.Northing;
                        Tools[i].TankHitchPos.Easting = Tools[i].TankWheelPos.Easting;
                    }

                    //Torriem rules!!!!! Oh yes, this is all his. Thank-you
                    if (distanceCurrentStepFix != 0)
                    {
                        double t = (Tools[i].ToolWheelLength) / distanceCurrentStepFix;
                        Vec2 tt = Tools[i].ToolWheelPos - Tools[i].TankHitchPos;
                        Tools[i].ToolWheelPos.Heading = Math.Atan2(t * tt.Easting, t * tt.Northing);
                    }
                    ////the tool is seriously jacknifed or just starting out so just spring it back.
                    over = Math.Abs(Math.PI - Math.Abs(Math.Abs(Tools[i].ToolWheelPos.Heading - Tools[i].TankWheelPos.Heading) - Math.PI));

                    if (over > 2.36 || startCounter > 0)//criteria for a forced reset to put tool directly behind vehicle
                    {
                        Tools[i].ToolWheelPos.Heading = Tools[i].TankWheelPos.Heading;
                    }

                    double ToolCos = Math.Cos(Tools[i].ToolWheelPos.Heading);
                    double ToolSin = Math.Sin(Tools[i].ToolWheelPos.Heading);

                    Tools[i].ToolWheelPos.Northing = Tools[i].TankHitchPos.Northing + ToolCos * Tools[i].ToolWheelLength;
                    Tools[i].ToolWheelPos.Easting = Tools[i].TankHitchPos.Easting + ToolSin * Tools[i].ToolWheelLength;
                    
                    Tools[i].ToolHitchPos.Northing = Tools[i].ToolWheelPos.Northing + ToolCos * Tools[i].ToolHitchLength;
                    Tools[i].ToolHitchPos.Easting = Tools[i].ToolWheelPos.Easting + ToolSin * Tools[i].ToolHitchLength;
                }
                else
                {
                    Tools[i].ToolWheelPos.Heading = fixHeading;
                    Tools[i].ToolHitchPos.Northing = Tools[i].HitchPos.Northing + Math.Cos(fixHeading) * Tools[i].ToolHitchLength;
                    Tools[i].ToolHitchPos.Easting = Tools[i].HitchPos.Easting + Math.Sin(fixHeading) * Tools[i].ToolHitchLength;
                }

                #endregion

                //used to increase triangle count when going around corners, less on straight
                //pick the slow moving side edge of tool
                double distance = Guidance.GuidanceWidth * 0.5;
                if (distance > 3) distance = 3;

                double twist;
                //whichever is less
                if (Tools[i].ToolFarLeftSpeed < Tools[i].ToolFarRightSpeed) twist = Tools[i].ToolFarLeftSpeed / Tools[i].ToolFarRightSpeed;
                else twist = Tools[i].ToolFarRightSpeed / Tools[i].ToolFarLeftSpeed;

                if (twist < 0.2) twist = 0.2;
                sectionTriggerStepDistance = Math.Min(distance * twist * twist, sectionTriggerStepDistance);

                //finally fixed distance for making a curve line
                if (!CurveLines.isOkToAddPoints) sectionTriggerStepDistance += 0.2;
                else sectionTriggerStepDistance = 1.0;

                //precalc the sin and cos of heading * -1
                Tools[i].sinSectionHeading = Math.Sin(-Tools[i].ToolWheelPos.Heading);
                Tools[i].cosSectionHeading = Math.Cos(-Tools[i].ToolWheelPos.Heading);
            }
        }

        //perimeter and boundary point generation
        public void AddBoundaryPoint()
        {
            //save the north & east as previous
            prevBoundaryPos.Easting = pn.fix.Easting;
            prevBoundaryPos.Northing = pn.fix.Northing;

            //build the boundary line

            if (bnd.isOkToAddPoints)
            {
                Vec3 point = new Vec3(
                    pivotAxlePos.Northing + (Math.Sin(pivotAxlePos.Heading) * (bnd.isDrawRightSide ? -bnd.createBndOffset : bnd.createBndOffset)),
                    pivotAxlePos.Easting + (Math.Cos(pivotAxlePos.Heading) * (bnd.isDrawRightSide ? bnd.createBndOffset : -bnd.createBndOffset)),
                    pivotAxlePos.Heading);
                bnd.bndBeingMadePts.Add(point);
            }
        }

        //calculate the extreme tool left, right velocities, each section lookahead, and whether or not its going backwards
        public void CalculateSectionLookAhead()
        {
            //calculate left side of section 1
            Vec2 right, left = new Vec2();

            //speed max for section kmh*0.277 to m/s * 10 cm per pixel * 1.7 max speed
            double meterPerSecPerPixel = Math.Abs(avgSpeed) * 4.5;
            Vec2 lastLeftPoint = new Vec2();
            Vec2 lastRightPoint = new Vec2();

            for (int i = 0; i < Tools.Count; i++)
            {
                double leftSpeed = 0, rightSpeed = 0;
                //now loop all the section rights and the one extreme left

                if (Tools[i].numOfSections > 0)
                {
                    for (int j = 0; j < Tools[i].numOfSections; j++)
                    {
                        lastLeftPoint = Tools[i].Sections[j].leftPoint;

                        if (j > 0 && Tools[i].Sections[j - 1].positionRight == Tools[i].Sections[j].positionLeft && Tools[i].Sections[j - 1].positionForward == Tools[i].Sections[j].positionForward)
                            Tools[i].Sections[j].leftPoint = Tools[i].Sections[j - 1].rightPoint;
                        else
                            Tools[i].Sections[j].leftPoint = new Vec2(Tools[i].sinSectionHeading * Tools[i].Sections[j].positionLeft + Tools[i].ToolHitchPos.Northing + Tools[i].cosSectionHeading * Tools[i].Sections[j].positionForward, Tools[i].cosSectionHeading * Tools[i].Sections[j].positionLeft + Tools[i].ToolHitchPos.Easting - Tools[i].sinSectionHeading * Tools[i].Sections[j].positionForward);

                        left = Tools[i].Sections[j].leftPoint - lastLeftPoint;

                        //get the speed for left side
                        leftSpeed = left.GetLength() / fixUpdateTime * 10;
                        if (leftSpeed > meterPerSecPerPixel) leftSpeed = meterPerSecPerPixel;

                        lastRightPoint = Tools[i].Sections[j].rightPoint;

                        Tools[i].Sections[j].rightPoint = new Vec2(Tools[i].sinSectionHeading * Tools[i].Sections[j].positionRight + Tools[i].ToolHitchPos.Northing + Tools[i].cosSectionHeading * Tools[i].Sections[j].positionForward, Tools[i].cosSectionHeading * Tools[i].Sections[j].positionRight + Tools[i].ToolHitchPos.Easting - Tools[i].sinSectionHeading * Tools[i].Sections[j].positionForward);

                        //now we have left and right for this section
                        right = Tools[i].Sections[j].rightPoint - lastRightPoint;

                        //grab vector length and convert to meters/sec/10 pixels per meter                
                        rightSpeed = right.GetLength() / fixUpdateTime * 10;
                        if (rightSpeed > meterPerSecPerPixel) rightSpeed = meterPerSecPerPixel;

                        //Is section outer going forward or backward
                        double head = Math.Atan2(left.Easting, left.Northing);
                        if (Math.PI - Math.Abs(Math.Abs(head - Tools[i].ToolWheelPos.Heading) - Math.PI) > Glm.PIBy2)
                        {
                            if (leftSpeed > 0) leftSpeed *= -1;
                        }

                        head = Math.Atan2(right.Easting, right.Northing);
                        if (Math.PI - Math.Abs(Math.Abs(head - Tools[i].ToolWheelPos.Heading) - Math.PI) > Glm.PIBy2)
                            if (rightSpeed > 0) rightSpeed *= -1;

                        //save the far left and right speed in m/sec averaged over 30%
                        if (j == 0)
                            Tools[i].ToolFarLeftSpeed = Tools[i].ToolFarLeftSpeed * 0.7 + (leftSpeed * 0.1) * 0.3;
                        if (j == Tools[i].numOfSections-1)
                            Tools[i].ToolFarRightSpeed = Tools[i].ToolFarRightSpeed * 0.7 + (rightSpeed * 0.1) * 0.3;

                        //choose fastest speed
                        Tools[i].Sections[j].speedPixels = ((leftSpeed > rightSpeed) ? leftSpeed : rightSpeed) * 0.36;
                    }
                    //fill in tool positions
                    Tools[i].Sections[Tools[i].numOfSections].leftPoint = Tools[i].Sections[0].leftPoint;
                    Tools[i].Sections[Tools[i].numOfSections].rightPoint = Tools[i].Sections[Tools[i].numOfSections - 1].rightPoint;
                    //set the look ahead for hyd Lift in pixels per second
                    vehicle.hydLiftLookAheadDistanceLeft = Math.Min(Tools[i].ToolFarLeftSpeed * vehicle.hydLiftLookAheadTime * 10, 199);
                    vehicle.hydLiftLookAheadDistanceRight = Math.Min(Tools[i].ToolFarRightSpeed * vehicle.hydLiftLookAheadTime * 10, 199);
                    Tools[i].lookAheadDistanceOnPixelsLeft = Math.Min(Tools[i].ToolFarLeftSpeed * Tools[i].LookAheadOnSetting * 10, 199);
                    Tools[i].lookAheadDistanceOnPixelsRight = Math.Min(Tools[i].ToolFarRightSpeed * Tools[i].LookAheadOnSetting * 10, 199);
                    Tools[i].lookAheadDistanceOffPixelsLeft = Math.Min(Tools[i].ToolFarLeftSpeed * Tools[i].LookAheadOffSetting * 10, 160);
                    Tools[i].lookAheadDistanceOffPixelsRight = Math.Min(Tools[i].ToolFarRightSpeed * Tools[i].LookAheadOffSetting * 10, 160);
                }
            }
        }

        //the start of first few frames to initialize entire program
        private void InitializeFirstFewGPSPositions()
        {
            //reduce the huge utm coordinates
            pn.utmEast = (int)(pn.fix.Easting);
            pn.utmNorth = (int)(pn.fix.Northing);
            pn.fix.Easting = (int)pn.fix.Easting - pn.utmEast;
            pn.fix.Northing = (int)pn.fix.Northing - pn.utmNorth;

            //calculate the central meridian of current zone
            pn.centralMeridian = -177 + ((pn.zone - 1) * 6);

            //Azimuth Error - utm declination
            pn.convergenceAngle = Math.Atan(Math.Sin(Glm.ToRadians(pn.latitude)) * Math.Tan(Glm.ToRadians(pn.longitude - pn.centralMeridian)));

            //Draw a grid once we know where in the world we are.
            worldGrid.CheckWorldGrid(pn.fix.Northing, pn.fix.Easting);

            //in radians
            fixHeading = 0;

            for (int i = 0; i < Tools.Count; i++) Tools[i].ToolWheelPos.Heading = fixHeading;

            //send out initial zero settings
            //set up the modules
            mc.ResetAllModuleCommValues(true);

            //SendSteerSettingsOutAutoSteerPort();
            //SendArduinoSettingsOutToAutoSteerPort();

            IsBetweenSunriseSunset(pn.latitude, pn.longitude);

            //set display accordingly
            isDayTime = (DateTime.Now.Ticks < sunset.Ticks && DateTime.Now.Ticks > sunrise.Ticks);

            if (isAutoDayNight)
            {
                isDay = !isDayTime;
                SwapDayNightMode();
            }
            isGPSPositionInitialized = true;
            return;
        }

        public bool IsInsideGeoFence()
        {
            //first where are we, must be inside outer and outside of inner geofence non drive thru turn borders
            if (bnd.bndArr.Count > 0)
            {
                if (bnd.bndArr[0].IsPointInGeoFenceArea(pivotAxlePos))
                {
                    for (int j = 1; j < bnd.bndArr.Count; j++)
                    {
                        //make sure not inside a non drivethru boundary
                        if (bnd.bndArr[j].isDriveThru) continue;

                        if (bnd.bndArr[j].IsPointInGeoFenceArea(pivotAxlePos))
                        {
                            distancePivotToTurnLine = -3333;
                            return false;
                        }
                    }
                    return true;
                }
                else
                {
                    distancePivotToTurnLine = -3333;
                    return false;
                }
            }
            else
            {
                return true;
            }
        }



// intense math section....   the lat long converted to utm   *********************************************************
#region utm Calculations
private double sm_a = 6378137.0;
        private double sm_b = 6356752.314;
        private double UTMScaleFactor2 = 1.0004001600640256102440976390556;

        //the result is placed in these two
        public double utmLat = 0;
        public double utmLon = 0;

        //public double RollDistance { get => rollCorrectionDistance; set => rollCorrectionDistance = value; }

        public void UTMToLatLon(double X, double Y)
        {
            double cmeridian;

            X -= 500000.0;
            X *= UTMScaleFactor2;

            /* If in southern hemisphere, adjust y accordingly. */
            if (pn.hemisphere == 'S')
                Y -= 10000000.0;
            Y *= UTMScaleFactor2;

            cmeridian = (-183.0 + (pn.zone * 6.0)) * 0.01745329251994329576923690766743;
            double[] latlon = new double[2]; //= MapXYToLatLon(X, Y, cmeridian);          
  
            double phif, Nf, Nfpow, nuf2, ep2, tf, tf2, tf4, cf;
            double x1frac, x2frac, x3frac, x4frac, x5frac, x6frac, x7frac, x8frac;
            double x2poly, x3poly, x4poly, x5poly, x6poly, x7poly, x8poly;

            /* Get the value of phif, the footpoint latitude. */
            phif = FootpointLatitude(Y);

            /* Precalculate ep2 */
            ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0))
                  / Math.Pow(sm_b, 2.0);

            /* Precalculate cos (phif) */
            cf = Math.Cos(phif);

            /* Precalculate nuf2 */
            nuf2 = ep2 * Math.Pow(cf, 2.0);

            /* Precalculate Nf and initialize Nfpow */
            Nf = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nuf2));
            Nfpow = Nf;

            /* Precalculate tf */
            tf = Math.Tan(phif);
            tf2 = tf * tf;
            tf4 = tf2 * tf2;

            /* Precalculate fractional coefficients for x**n in the equations
               below to simplify the expressions for latitude and longitude. */
            x1frac = 1.0 / (Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**2) */
            x2frac = tf / (2.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**3) */
            x3frac = 1.0 / (6.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**4) */
            x4frac = tf / (24.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**5) */
            x5frac = 1.0 / (120.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**6) */
            x6frac = tf / (720.0 * Nfpow);

            Nfpow *= Nf;   /* now equals Nf**7) */
            x7frac = 1.0 / (5040.0 * Nfpow * cf);

            Nfpow *= Nf;   /* now equals Nf**8) */
            x8frac = tf / (40320.0 * Nfpow);

            /* Precalculate polynomial coefficients for x**n.
               -- x**1 does not have a polynomial coefficient. */
            x2poly = -1.0 - nuf2;

            x3poly = -1.0 - 2 * tf2 - nuf2;

            x4poly = 5.0 + 3.0 * tf2 + 6.0 * nuf2 - 6.0 * tf2 * nuf2
                - 3.0 * (nuf2 * nuf2) - 9.0 * tf2 * (nuf2 * nuf2);

            x5poly = 5.0 + 28.0 * tf2 + 24.0 * tf4 + 6.0 * nuf2 + 8.0 * tf2 * nuf2;

            x6poly = -61.0 - 90.0 * tf2 - 45.0 * tf4 - 107.0 * nuf2
                + 162.0 * tf2 * nuf2;

            x7poly = -61.0 - 662.0 * tf2 - 1320.0 * tf4 - 720.0 * (tf4 * tf2);

            x8poly = 1385.0 + 3633.0 * tf2 + 4095.0 * tf4 + 1575 * (tf4 * tf2);

            /* Calculate latitude */
            latlon[0] = phif + x2frac * x2poly * (X * X)
                + x4frac * x4poly * Math.Pow(X, 4.0)
                + x6frac * x6poly * Math.Pow(X, 6.0)
                + x8frac * x8poly * Math.Pow(X, 8.0);

            /* Calculate longitude */
            latlon[1] = cmeridian + x1frac * X
                + x3frac * x3poly * Math.Pow(X, 3.0)
                + x5frac * x5poly * Math.Pow(X, 5.0)
                + x7frac * x7poly * Math.Pow(X, 7.0);

            utmLat = latlon[0] * 57.295779513082325225835265587528;
            utmLon = latlon[1] * 57.295779513082325225835265587528;
        }
 
        private double FootpointLatitude(double y)
        {
            double y_, alpha_, beta_, gamma_, delta_, epsilon_, n;
            double result;

            /* Precalculate n (Eq. 10.18) */
            n = (sm_a - sm_b) / (sm_a + sm_b);

            /* Precalculate alpha_ (Eq. 10.22) */
            /* (Same as alpha in Eq. 10.17) */
            alpha_ = ((sm_a + sm_b) / 2.0)
                * (1 + (Math.Pow(n, 2.0) / 4) + (Math.Pow(n, 4.0) / 64));

            /* Precalculate y_ (Eq. 10.23) */
            y_ = y / alpha_;

            /* Precalculate beta_ (Eq. 10.22) */
            beta_ = (3.0 * n / 2.0) + (-27.0 * Math.Pow(n, 3.0) / 32.0)
                + (269.0 * Math.Pow(n, 5.0) / 512.0);

            /* Precalculate gamma_ (Eq. 10.22) */
            gamma_ = (21.0 * Math.Pow(n, 2.0) * 0.0625)
                + (-55.0 * Math.Pow(n, 4.0) / 32.0);

            /* Precalculate delta_ (Eq. 10.22) */
            delta_ = (151.0 * Math.Pow(n, 3.0) / 96.0)
                + (-417.0 * Math.Pow(n, 5.0) / 128.0);

            /* Precalculate epsilon_ (Eq. 10.22) */
            epsilon_ = (1097.0 * Math.Pow(n, 4.0) / 512.0);

            /* Now calculate the sum of the series (Eq. 10.21) */
            result = y_ + (beta_ * Math.Sin(2.0 * y_))
                + (gamma_ * Math.Sin(4.0 * y_))
                + (delta_ * Math.Sin(6.0 * y_))
                + (epsilon_ * Math.Sin(8.0 * y_));

            return result;
        }

#endregion

    }//end class
}//end namespace

