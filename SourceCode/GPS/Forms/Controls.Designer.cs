//Please, if you use this, share the improvements
using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using Microsoft.Win32;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        // Buttons //-----------------------------------------------------------------------

        private void btnStartStopNtrip_Click(object sender, EventArgs e)
        {
            isNTRIP_TurnedOn = !isNTRIP_TurnedOn;
            NtripCounter = 15;
            if (!isNTRIP_TurnedOn)
            {
                ShutDownNTRIP();
                lblWatch.Text = gStr.gsStopped;
                NTRIPBytesMenu.Visible = false;
                pbarNtripMenu.Visible = false;
            }
            else
            {
                lblWatch.Text = gStr.gsWaiting;
                NTRIPBytesMenu.Visible = true;
                pbarNtripMenu.Visible = true;
            }
        }

        private void btnManualAutoDrive_Click(object sender, EventArgs e)
        {
            //if (isInAutoDrive)
            //{
            //    isInAutoDrive = false;
            //    btnManualAutoDrive.Image = Properties.Resources.Cancel64;
            //    btnManualAutoDrive.Text = gStr.gsManual;
            //}
            //else
            //{
            //    isInAutoDrive = true;
            //    btnManualAutoDrive.Image = Properties.Resources.OK64;
            //    btnManualAutoDrive.Text = gStr.gsAuto;
            //}
        }

        private void goPathMenu_Click(object sender, EventArgs e)
        {
            if (bnd.bndArr.Count == 0)
            {
                TimedMessageBox(2000, gStr.gsNoBoundary, gStr.gsCreateABoundaryFirst);
                return;
            }

            //if contour is on, turn it off
            if (ct.isContourBtnOn) { if (ct.isContourBtnOn) btnContour.PerformClick(); }
            //btnContourPriority.Enabled = true;

            if (yt.isYouTurnBtnOn) btnAutoYouTurn.PerformClick();
            if (isAutoSteerBtnOn) btnAutoSteer.PerformClick();

            DisableYouTurnButtons();

            //if ABLine isn't set, turn off the YouTurn
            if (ABLine.isABLineSet)
            {
                //ABLine.DeleteAB();
                ABLine.isABLineBeingSet = false;
                ABLine.isABLineSet = false;
                //lblDistanceOffLine.Visible = false;

                //change image to reflect on off
                btnABLine.Image = Properties.Resources.ABLineOff;
                ABLine.isBtnABLineOn = false;
            }

            if (curve.isCurveSet)
            {

                //make sure the other stuff is off
                curve.isOkToAddPoints = false;
                curve.isCurveSet = false;
                //btnContourPriority.Enabled = false;
                curve.isBtnCurveOn = false;
                btnCurve.Image = Properties.Resources.CurveOff;
            }

            if (!recPath.isPausedDrivingRecordedPath)
            {
                //already running?
                if (recPath.isDrivingRecordedPath)
                {
                    recPath.StopDrivingRecordedPath();
                    return;
                }

                //start the recorded path driving process



                if (!recPath.StartDrivingRecordedPath())
                {
                    //Cancel the recPath - something went seriously wrong
                    recPath.StopDrivingRecordedPath();
                    TimedMessageBox(1500, gStr.gsProblemMakingPath, gStr.gsCouldntGenerateValidPath);
                }
                else
                {
                    goPathMenu.Image = Properties.Resources.AutoStop;
                }
            }
            else
            {
                recPath.isPausedDrivingRecordedPath = false;
                pausePathMenu.BackColor = Color.Lime;
            }
        }

        private void PausePathMenu_Click(object sender, EventArgs e)
        {
            if (recPath.isPausedDrivingRecordedPath)
            {
                //btnPauseDrivingPath.BackColor = Color.Lime;
                pausePathMenu.BackColor = Color.Lime;
            }
            else
            {
                //btnPauseDrivingPath.BackColor = Color.OrangeRed;
                pausePathMenu.BackColor = Color.OrangeRed;
            }

            recPath.isPausedDrivingRecordedPath = !recPath.isPausedDrivingRecordedPath;

        }

        private void RecordPathMenu_Click(object sender, EventArgs e)
        {
            if (recPath.isRecordOn)
            {
                FileSaveRecPath();
                recPath.isRecordOn = false;
                recordPathMenu.Image = Properties.Resources.BoundaryRecord;
            }
            else if (isJobStarted)
            {
                recPath.recList.Clear();
                recPath.isRecordOn = true;
                recordPathMenu.Image = Properties.Resources.boundaryStop;
            }
        }

        private void DeletePathMenu_Click(object sender, EventArgs e)
        {
            recPath.recList.Clear();
            recPath.StopDrivingRecordedPath();
            FileSaveRecPath();

        }

        //LIDAR control
        private void btnAutoSteer_Click(object sender, EventArgs e)
        {
            System.Media.SystemSounds.Question.Play();

            //new direction so reset where to put turn diagnostic
            yt.ResetCreatedYouTurn();

            if (isAutoSteerBtnOn)
            {
                isAutoSteerBtnOn = false;
                btnAutoSteer.Image = Properties.Resources.AutoSteerOff;
                if (yt.isYouTurnBtnOn) btnAutoYouTurn.PerformClick();
            }
            else
            {
                if (ABLine.isBtnABLineOn | ct.isContourBtnOn | curve.isBtnCurveOn)
                {
                    isAutoSteerBtnOn = true;
                    btnAutoSteer.Image = Properties.Resources.AutoSteerOn;
                }
                else
                {
                    TimedMessageBox(2000, gStr.gsNoGuidanceLines, gStr.gsTurnOnContourOrMakeABLine);
                }
            }
        }

        private void BtnMakeLinesFromBoundary_Click(object sender, EventArgs e)
        {
            if (ct.isContourBtnOn)
            {
                TimedMessageBox(2000, gStr.gsContourOn, gStr.gsTurnOffContourFirst);
                return;
            }
            if (bnd.bndArr.Count == 0)
            {
                TimedMessageBox(2000, gStr.gsNoBoundary, gStr.gsCreateABoundaryFirst);
                return;
            }

            GetAB();
        }

        private void btnCycleLines_Click(object sender, EventArgs e)
        {
            if (ABLine.isBtnABLineOn && ABLine.numABLines > 0)
            {
                ABLine.moveDistance = 0;

                ABLine.numABLineSelected++;
                if (ABLine.numABLineSelected > ABLine.numABLines) ABLine.numABLineSelected = 1;
                ABLine.refPoint1 = ABLine.lineArr[ABLine.numABLineSelected - 1].origin;
                //ABLine.refPoint2 = ABLine.lineArr[ABLine.numABLineSelected - 1].ref2;
                ABLine.abHeading = ABLine.lineArr[ABLine.numABLineSelected - 1].heading;
                ABLine.SetABLineByHeading();
                ABLine.isABLineSet = true;
                ABLine.isABLineLoaded = true;
                yt.ResetYouTurn();
                btnCycleLines.Text = "AB-" + ABLine.numABLineSelected;
                if (tram.displayMode > 0) ABLine.BuildTram();
            }
            else if (curve.isBtnCurveOn && curve.numCurveLines > 0)
            {
                curve.moveDistance = 0;
                curve.OldhowManyPathsAway = -99999;
                curve.numCurveLineSelected++;
                if (curve.numCurveLineSelected > curve.numCurveLines) curve.numCurveLineSelected = 1;

                int idx = curve.numCurveLineSelected - 1;
                curve.aveLineHeading = curve.curveArr[idx].aveHeading;
                curve.refList?.Clear();
                for (int i = 0; i < curve.curveArr[idx].curvePts.Count; i++)
                {
                    curve.refList.Add(curve.curveArr[idx].curvePts[i]);
                }
                curve.CalculateTurnHeadings();
                curve.isCurveSet = true;
                yt.ResetYouTurn();
                btnCycleLines.Text = "Cur-" + curve.numCurveLineSelected;
                if (tram.displayMode > 0) curve.BuildTram();
            }
        }

        private void btnABLine_Click(object sender, EventArgs e)
        {
            btnCycleLines.Text = "AB-" + ABLine.numABLineSelected;

            //check if window already exists
            Form f = Application.OpenForms["FormABCurve"];

            if (f != null)
            {
                f.Focus();
                return;
            }

            Form af = Application.OpenForms["FormABLine"];

            if (af != null)
            {
                af.Close();
                return;
            }


            //if contour is on, turn it off
            if (ct.isContourBtnOn) { if (ct.isContourBtnOn) btnContour.PerformClick(); }
            //btnContourPriority.Enabled = true;

            //make sure the other stuff is off
            curve.isOkToAddPoints = false;
                
            curve.isBtnCurveOn = false;
            btnCurve.Image = Properties.Resources.CurveOff;

            //if there is a line in memory, just use it.
            if (ABLine.isBtnABLineOn == false && ABLine.isABLineLoaded)
            {                
                ABLine.isABLineSet = true;
                EnableYouTurnButtons();
                btnABLine.Image = Properties.Resources.ABLineOn;
                ABLine.isBtnABLineOn = true;
                if (tram.displayMode > 0 ) ABLine.BuildTram();
                return;
            }
            
            //check if window already exists, return if true
            Form fc = Application.OpenForms["FormABLine"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            //Bring up the form
            ABLine.isBtnABLineOn = true;
            btnABLine.Image = Properties.Resources.ABLineOn;

            //turn off youturn...
            //DisableYouTurnButtons();
            //yt.ResetYouTurn();

            var form = new FormABLine(this);
                form.Show();
        }

        private void btnCurve_Click(object sender, EventArgs e)
        {
            btnCycleLines.Text = "Cur-" + curve.numCurveLineSelected;

            //check if window already exists, return if true

            Form f = Application.OpenForms["FormABLine"];

            if (f != null)
            {
                f.Focus();
                return;
            }

            //check if window already exists
            Form cf = Application.OpenForms["FormABCurve"];

            if (cf != null)
            {
                cf.Close();
                return;
            }


            //if contour is on, turn it off
            if (ct.isContourBtnOn) { if (ct.isContourBtnOn) btnContour.PerformClick(); }

            //turn off ABLine 
            ABLine.isABLineBeingSet = false;
            ABLine.isABLineSet = false;
            //lblDistanceOffLine.Visible = false;

            //change image to reflect on off
            btnABLine.Image = Properties.Resources.ABLineOff;
            ABLine.isBtnABLineOn = false;

            //new direction so reset where to put turn diagnostic
            //yt.ResetCreatedYouTurn();

            if (curve.isBtnCurveOn == false && curve.isCurveSet)
            {
                //display the curve
                curve.isCurveSet = true;
                EnableYouTurnButtons();
                btnCurve.Image = Properties.Resources.CurveOn;
                curve.isBtnCurveOn = true;
                curve.BuildTram();
                return;
            }


            //check if window already exists
            Form fc = Application.OpenForms["FormABCurve"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            curve.isBtnCurveOn = true;
            btnCurve.Image = Properties.Resources.CurveOn;

            EnableYouTurnButtons();
            //btnContourPriority.Enabled = true;

            Form form = new FormABCurve(this);
            form.Show();
        }

        private void btnContour_Click(object sender, EventArgs e)
        {
            ct.isContourBtnOn = !ct.isContourBtnOn;
            btnContour.Image = ct.isContourBtnOn ? Properties.Resources.ContourOn : Properties.Resources.ContourOff;

            if (ct.isContourBtnOn)
            {
                //turn off youturn...
                DisableYouTurnButtons();
                if (ct.isRightPriority)
                {
                    btnContourPriority.Image = Properties.Resources.ContourPriorityRight;
                }
                else
                {
                    btnContourPriority.Image = Properties.Resources.ContourPriorityLeft;
                }
            }

            else
            {
                if (ABLine.isBtnABLineOn | curve.isBtnCurveOn)
                {
                    EnableYouTurnButtons();
                }

                btnContourPriority.Image = Properties.Resources.Snap2;
            }
        }

        private void btnContourPriority_Click(object sender, EventArgs e)
        {
            if (ct.isContourBtnOn)
            {

                ct.isRightPriority = !ct.isRightPriority;

                if (ct.isRightPriority)
                {
                    btnContourPriority.Image = Properties.Resources.ContourPriorityRight;
                }
                else
                {
                    btnContourPriority.Image = Properties.Resources.ContourPriorityLeft;
                }
            }
            else
            {
                if (ABLine.isBtnABLineOn)
                {
                    ABLine.SnapABLine();
                }
                else if (curve.isBtnCurveOn)
                {
                    curve.SnapABCurve();
                }
                else
                {
                    TimedMessageBox(2000, gStr.gsNoGuidanceLines, gStr.gsTurnOnContourOrMakeABLine);
                }
            }
        }

        //Snaps
        private void SnapRight()
        {
            if (!ct.isContourBtnOn)
            {
                if (ABLine.isABLineSet)
                {
                    //snap distance is in cm
                    yt.ResetCreatedYouTurn();
                    double dist = 0.01 * Properties.Settings.Default.setAS_snapDistance;

                    ABLine.MoveABLine(dist);
                }
                else if (curve.isCurveSet)
                {
                    //snap distance is in cm
                    yt.ResetCreatedYouTurn();
                    double dist = 0.01 * Properties.Settings.Default.setAS_snapDistance;
                    curve.MoveABCurve(dist);

                }
                else
                {
                    TimedMessageBox(2000, gStr.gsNoGuidanceLines, gStr.gsTurnOnContourOrMakeABLine);
                }
            }

        }

        private void SnapLeft()
        {
            if (!ct.isContourBtnOn)
            {
                if (ABLine.isABLineSet)
                {
                    //snap distance is in cm
                    yt.ResetCreatedYouTurn();
                    double dist = 0.01 * Properties.Settings.Default.setAS_snapDistance;

                    ABLine.MoveABLine(-dist);

                    //FileSaveABLine();
                }
                else if (curve.isCurveSet)
                {
                    //snap distance is in cm
                    yt.ResetCreatedYouTurn();
                    double dist = 0.01 * Properties.Settings.Default.setAS_snapDistance;

                    curve.MoveABCurve(-dist);

                }
                else
                {
                    TimedMessageBox(2000, gStr.gsNoGuidanceLines, gStr.gsTurnOnContourOrMakeABLine);
                }
            }
        }

        private void btnSnapRight_Click(object sender, EventArgs e)
        {
            SnapRight();
        }

        private void btnSnapLeft_Click(object sender, EventArgs e)
        {
            SnapLeft();
        }

        //Section Manual and Auto
        private void btnSection_Click(object sender, EventArgs e)
        {
            Button sa = sender as Button;
            if (sa.Name == "btnAutoSection")
            {
                System.Media.SystemSounds.Exclamation.Play();
                if (autoBtnState == btnStates.Auto) autoBtnState = btnStates.Off;
                else autoBtnState = btnStates.Auto;
            }
            else
            {
                System.Media.SystemSounds.Asterisk.Play();
                if (autoBtnState == btnStates.On) autoBtnState = btnStates.Off;
                else autoBtnState = btnStates.On;
            }
            btnSection_Update();
        }

        public void btnSection_Update()
        {
            btnManualSection.Image = autoBtnState == btnStates.On ? Properties.Resources.ManualOn : Properties.Resources.ManualOff;
            btnAutoSection.Image = autoBtnState == btnStates.Auto ? Properties.Resources.SectionMasterOn : Properties.Resources.SectionMasterOff;

            for (int i = 0; i < Tools.Count; i++)
            {
                //turn section buttons all On/OFF/Auto
                for (int j = 0; j < Tools[i].numOfSections; j++)
                {
                    Tools[i].Sections[j].IsAllowedOn = (autoBtnState != 0);
                    Tools[i].Sections[j].BtnSectionState = autoBtnState;
                    Tools[i].Sections[j].SectionButton.Enabled = (autoBtnState != 0);
                    Tools[i].SectionButtonColor(j);
                }
            }
        }

        //individual buttons for sections
        public void btnSectionMan_Click(object sender, EventArgs e)
        {
            if (sender is Button btn)
            {
                if (autoBtnState != btnStates.Off)
                {
                    string[] words = btn.Name.Split(',');

                    int toolNumber = Convert.ToInt32(words[0]);
                    int sectNumber = Convert.ToInt32(words[1]);

                    //if auto is off just have on-off for choices of section buttons
                    if (Tools[toolNumber].Sections[sectNumber].BtnSectionState == btnStates.Off)
                    {
                        if (autoBtnState != btnStates.Auto) Tools[toolNumber].Sections[sectNumber].BtnSectionState = btnStates.On;
                        else Tools[toolNumber].Sections[sectNumber].BtnSectionState = btnStates.Auto;
                    }
                    else if (Tools[toolNumber].Sections[sectNumber].BtnSectionState == btnStates.Auto) Tools[toolNumber].Sections[sectNumber].BtnSectionState = btnStates.On;
                    else if (Tools[toolNumber].Sections[sectNumber].BtnSectionState == btnStates.On) Tools[toolNumber].Sections[sectNumber].BtnSectionState = btnStates.Off;

                    Tools[toolNumber].SectionButtonColor(sectNumber);
                }
            }
        }

        //The zoom tilt buttons
        private void btnZoomIn_MouseDown(object sender, MouseEventArgs e)
        {
            if (camera.zoomValue <= 20) camera.zoomValue += camera.zoomValue * 0.1;
            else camera.zoomValue += camera.zoomValue * 0.05;
            if (camera.zoomValue > 220) camera.zoomValue = 220;
            camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
            SetZoom();
        }

        private void btnZoomOut_MouseDown(object sender, MouseEventArgs e)
        {
            if (camera.zoomValue <= 20)
            { if ((camera.zoomValue -= camera.zoomValue * 0.1) < 6.0) camera.zoomValue = 6.0; }
            else { if ((camera.zoomValue -= camera.zoomValue * 0.05) < 6.0) camera.zoomValue = 6.0; }
            camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
            SetZoom();
        }

        private void btnpTiltUp_MouseDown(object sender, MouseEventArgs e)
        {
            camera.camPitch -= ((camera.camPitch * 0.012) - 1);
            if (camera.camPitch > -58) camera.camPitch = 0;
        }

        private void btnpTiltDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (camera.camPitch > -59) camera.camPitch = -60;
            camera.camPitch += ((camera.camPitch * 0.012) - 1);
            if (camera.camPitch < -76) camera.camPitch = -76;
        }

        private void btnMoveDown_MouseDown(object sender, MouseEventArgs e)
        {
            offX += (Math.Sin(fixHeading) * 10);
            offY += (Math.Cos(fixHeading) * 10);
        }

        private void btnMoveUp_MouseDown(object sender, MouseEventArgs e)
        {
            offX -= (Math.Sin(fixHeading) * 10);
            offY -= (Math.Cos(fixHeading) * 10);
        }

        private void btnMoveLeft_MouseDown(object sender, MouseEventArgs e)
        {
            offY += (Math.Sin(-fixHeading) * 10);
            offX += (Math.Cos(-fixHeading) * 10);
        }

        private void btnMoveRight_MouseDown(object sender, MouseEventArgs e)
        {
            offY -= (Math.Sin(-fixHeading) * 10);
            offX -= (Math.Cos(-fixHeading) * 10);
        }

        private void btnMoveHome_Click(object sender, EventArgs e)
        {
            offX = 0;
            offY = 0;
        }

        //
        private void btnSaveAB_Click(object sender, EventArgs e)
        {
            if (ABLine.isBtnABLineOn)
            {
                //index to last one.
                int idx = ABLine.numABLineSelected - 1;

                if (idx >= 0)
                {
                    ABLine.lineArr[idx].heading = ABLine.abHeading;
                    //calculate the new points for the reference line and points
                    ABLine.lineArr[idx].origin.easting = ABLine.refPoint1.easting;
                    ABLine.lineArr[idx].origin.northing = ABLine.refPoint1.northing;

                    //sin x cos z for endpoints, opposite for additional lines
                    ABLine.lineArr[idx].ref1.easting = ABLine.lineArr[idx].origin.easting - (Math.Sin(ABLine.lineArr[idx].heading) * 2000.0);
                    ABLine.lineArr[idx].ref1.northing = ABLine.lineArr[idx].origin.northing - (Math.Cos(ABLine.lineArr[idx].heading) * 2000.0);
                    ABLine.lineArr[idx].ref2.easting = ABLine.lineArr[idx].origin.easting + (Math.Sin(ABLine.lineArr[idx].heading) * 2000.0);
                    ABLine.lineArr[idx].ref2.northing = ABLine.lineArr[idx].origin.northing + (Math.Cos(ABLine.lineArr[idx].heading) * 2000.0);
                }

                FileSaveABLines();
                ABLine.moveDistance = 0;
            }

            if (curve.isBtnCurveOn)
            {
                if (curve.refList.Count > 0)
                {
                    //array number is 1 less since it starts at zero
                    int idx = curve.numCurveLineSelected - 1;

                    //curve.curveArr[idx].Name = textBox1.Text.Trim();
                    if (idx >= 0)
                    {
                        curve.curveArr[idx].aveHeading = curve.aveLineHeading;
                        curve.curveArr[idx].curvePts.Clear();
                        //write out the Curve Points
                        foreach (var item in curve.refList)
                        {
                            curve.curveArr[idx].curvePts.Add(item);
                        }
                    }

                    //save entire list
                    FileSaveCurveLines();
                    curve.moveDistance = 0;
                }
            }
        }

        private void btnEditAB_Click(object sender, EventArgs e)
        {
            //if (isAutoSteerBtnOn) btnAutoSteer.PerformClick();

            Form fc = Application.OpenForms["FormEditAB"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (ABLine.numABLineSelected > 0 && ABLine.isBtnABLineOn)
            {
                Form form = new FormEditAB(this);
                form.Show();
            }
            else if (curve.numCurveLineSelected > 0 && curve.isBtnCurveOn)
            {
                Form form = new FormEditCurve(this);
                form.Show();
            }
            else
            {
                TimedMessageBox(1500, gStr.gsNoABLineActive, gStr.gsPleaseEnterABLine);
                return;
            }
        }

        private void btnTramMenu_Click(object sender, EventArgs e)
        {
            curve.isOkToAddPoints = false;

            if (ct.isContourBtnOn) { if (ct.isContourBtnOn) btnContour.PerformClick(); }

            if (ABLine.numABLineSelected > 0 && ABLine.isBtnABLineOn)
            {
                Form form99 = new FormTram(this);
                form99.Show();
                form99.Left = Width - 275;
                form99.Top = 100;
            }
            else if (curve.numCurveLineSelected > 0 && curve.isBtnCurveOn)
            {
                Form form97 = new FormTramCurve(this);
                form97.Show();
                form97.Left = Width - 275;
                form97.Top = 100;
            }
            else
            {
                TimedMessageBox(1500, gStr.gsNoABLineActive, gStr.gsPleaseEnterABLine);
                layoutPanelRight.Enabled = true;
                ABLine.isEditing = false;
                return;
            }
        }

        private void btnFlag_Click(object sender, EventArgs e)
        {
            int nextflag = flagPts.Count + 1;
            CFlag flagPt = new CFlag(pn.latitude, pn.longitude, pn.fix.easting, pn.fix.northing, fixHeading, flagColor, nextflag, (nextflag).ToString());
            flagPts.Add(flagPt);
            FileSaveFlags();

            Form fc = Application.OpenForms["FormFlags"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (flagPts.Count > 0)
            {
                flagNumberPicked = nextflag;
                Form form = new FormFlags(this);
                form.Show();
            }


        }

        private void btnAutoYouTurn_Click(object sender, EventArgs e)
        {
            yt.isTurnCreationTooClose = false;

            if (bnd.bndArr.Count == 0)
            {
                TimedMessageBox(2000, gStr.gsNoBoundary, gStr.gsCreateABoundaryFirst);
                return;
            }

            if (!yt.isYouTurnBtnOn)
            {
                //new direction so reset where to put turn diagnostic
                yt.ResetCreatedYouTurn();

                if (ABLine.isBtnABLineOn || curve.isBtnCurveOn)
                {
                    if (!isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                }
                else return;

                yt.isYouTurnBtnOn = true;
                yt.isTurnCreationTooClose = false;
                yt.isTurnCreationNotCrossingError = false;
                yt.ResetYouTurn();
                //mc.autoSteerData[mc.sdX] = 0;
                mc.machineData[mc.mdUTurn] = 0;
                btnAutoYouTurn.Image = Properties.Resources.Youturn80;
            }
            else
            {
                yt.isYouTurnBtnOn = false;
                yt.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
                btnAutoYouTurn.Image = Properties.Resources.YouTurnNo;
                yt.ResetYouTurn();

                //new direction so reset where to put turn diagnostic
                yt.ResetCreatedYouTurn();

                //mc.autoSteerData[mc.sdX] = 0;
                mc.machineData[mc.mdUTurn] = 0;
            }
        }

        // the context menu from autosteer button
        private void autoUTurnHeadLiftStrip_Click(object sender, EventArgs e)
        {
            if (hd.headArr.Count > 0)
            {
                hd.isOn = !hd.isOn;
                if (hd.isOn) btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                else btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
            }

            if (!yt.isYouTurnBtnOn)  btnAutoYouTurn.PerformClick();

            if (!vehicle.isHydLiftOn) btnHydLift.PerformClick();
        }

        private void btnHeadlandOnOff_Click(object sender, EventArgs e)
        {
            if (hd.headArr[0].HeadLine.Count > 0)
            {
                hd.isOn = !hd.isOn;
                if (hd.isOn) btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                else btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
            }

            if (!hd.isOn)
            {
                mc.machineData[mc.mdHydLift] = 0;
                vehicle.isHydLiftOn = false;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }

        }

        private void btnHydLift_Click(object sender, EventArgs e)
        {
            if (hd.isOn)
            {
                vehicle.isHydLiftOn = !vehicle.isHydLiftOn;
                if (vehicle.isHydLiftOn)
                {
                    btnHydLift.Image = Properties.Resources.HydraulicLiftOn;
                }
                else
                {
                    btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
                    mc.machineData[mc.mdHydLift] = 0;
                }
            }
            else
            {
                mc.machineData[mc.mdHydLift] = 0;
                vehicle.isHydLiftOn = false;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }
        }

        private void gPSInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormModules"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                return;
            }

            Form form = new FormModules(this);
            form.Show();

        }

        private void lblSpeed_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];

            if (f != null)
            {
                f.Focus();
                f.Close();
                return;
            }

            Form form = new FormGPSData(this);
            form.Show();
        }

        private void showStartScreenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setDisplay_isTermsOn = true;
            Properties.Settings.Default.Save();

        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            isFullScreen = !isFullScreen;
            if (isFullScreen)
            {
                WindowState = FormWindowState.Normal;
                //TopMost = true;
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                btnFullScreen.BackgroundImage = Properties.Resources.WindowNormal;

            }
            else
            {
                TopMost = false;
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                btnFullScreen.BackgroundImage = Properties.Resources.WindowFullScreen;

            }
        }

        private void btnShutdown_Click(object sender, EventArgs e)
        {
            DialogResult result3 = MessageBox.Show(gStr.gsOff, gStr.gsWaiting, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (result3 == DialogResult.Yes) Close();
        }

        private void btnReverseDirection_Click(object sender, EventArgs e)
        {
            curve.isSameWay = !curve.isSameWay;
            sim.headingTrue += Math.PI;
            if (sim.headingTrue > (2.0 * Math.PI)) sim.headingTrue -= (2.0 * Math.PI);
            if (sim.headingTrue < 0) sim.headingTrue += (2.0 * Math.PI);

        }

        private void btnSimSetSpeedToZero_Click(object sender, EventArgs e)
        {
            sim.stepDistance = 0;
            hsbarStepDistance.Value = 0;
        }

        private void btnDayNightMode_Click(object sender, EventArgs e)
        {
            SwapDayNightMode();
        }        

        //Options
        private void cboxpRowWidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            yt.rowSkipsWidth = cboxpRowWidth.SelectedIndex + 1;
            yt.ResetCreatedYouTurn();
            Properties.Vehicle.Default.set_youSkipWidth = yt.rowSkipsWidth;
            Properties.Vehicle.Default.Save();
        }

        // Menu Items ------------------------------------------------------------------
        private void boundariesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                using (var form = new FormBoundary(this))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK)
                    {
                        Form form2 = new FormBoundaryPlayer(this);
                        form2.Show();
                    }
                }
            }
            else { TimedMessageBox(3000, gStr.gsFieldNotOpen, gStr.gsStartNewField); }
        }

        private void toolStripBtnMakeBndContour_Click_1(object sender, EventArgs e)
        {
            //build all the contour guidance lines from boundaries, all of them.
            using (var form = new FormMakeBndCon(this))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) { }
            }
        }

        private void deleteContourPathsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //FileCreateContour();
            ct.stripList?.Clear();
            ct.ptList?.Clear();
            ct.ctList?.Clear();
            ContourSaveList?.Clear();
        }

        private void fileExplorerToolStripItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                FileSaveFieldKML();
            }
            Process.Start(fieldsDirectory + currentFieldDirectory);
        }        

        private void topMenuFileExplorer_Click(object sender, EventArgs e)
        {
            Process.Start(baseDirectory);
        }

        private void toolStripDeleteApplied_Click(object sender, EventArgs e)
        {
            //FileCreateContour();
            ct.stripList?.Clear();
            ct.ptList?.Clear();
            ct.ctList?.Clear();
            ContourSaveList?.Clear();
        }

        private void toolStripAreYouSure_Click_1(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                DialogResult result3 = MessageBox.Show(gStr.gsDeleteAllContoursAndSections,
                    gStr.gsDeleteForSure,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {

                    for (int i = 0; i < Tools.Count; i++)
                    {
                        //clear the section lists
                        for (int j = 0; j <= Tools[i].numOfSections; j++)
                        {
                            if (Tools[i].Sections[j].IsMappingOn)
                            {
                                Tools[i].Sections[j].TurnMappingOff();
                                //clean out the lists
                                Tools[i].Sections[j].triangleList?.Clear();
                                Tools[i].Sections[j].TurnMappingOn();
                            }
                        }
                    }

                    //clear out the contour Lists
                    ct.StopContourLine(pivotAxlePos);
                    ct.ResetContour();
                    fd.workedAreaTotal = 0;
                    PatchSaveList?.Clear();
                    PatchDrawList?.Clear();

                    FileCreateContour();
                    FileCreateSections();
                }
                else
                {
                    TimedMessageBox(1500, gStr.gsNothingDeleted, gStr.gsActionHasBeenCancelled);
                }
            }
        }

        private void toolStripBtnField_Click(object sender, EventArgs e)
        {
            JobNewOpenResume();
        }

        private void SmoothABtoolStripMenu_Click(object sender, EventArgs e)
        {
            if (isJobStarted && curve.isBtnCurveOn)
            {
                using (var form = new FormSmoothAB(this))
                {
                    var result = form.ShowDialog();
                    if (result == DialogResult.OK) { }
                }
            }
            else
            {
                if (!isJobStarted) TimedMessageBox(2000, gStr.gsFieldNotOpen, gStr.gsStartNewField);
                else TimedMessageBox(2000, gStr.gsCurveNotOn, gStr.gsTurnABCurveOn);
            }
        }

        private void toolstripDisplayConfig_Click_1(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }

            using (var form = new FormIMU(this))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) 
                { 
                    if (Properties.Settings.Default.setAS_isAutoSteerAutoOn) btnAutoSteer.Text = "R";
                    else btnAutoSteer.Text = "M";

                    ////MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
                    //Close();
                    Application.Restart();
                    Environment.Exit(0);

                }
            }

        }
        private void toolstripUSBPortsConfig_Click_1(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }

            SettingsCommunications();
        }

        private void toolstripUDPConfig_Click_1(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SettingsUDP();
        }

        private void toolStripNTRIPConfig_Click_1(object sender, EventArgs e)
        {
            SettingsNTRIP();
        }

        private void toolstripVehicleConfig_Click_1(object sender, EventArgs e)
        {
            using (var form = new FormSettings(this, 0))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (Properties.Settings.Default.setAS_isAutoSteerAutoOn) btnAutoSteer.Text = "A";
                    else btnAutoSteer.Text = "M";
                }
            }
        }

        private void toolstripYouTurnConfig_Click_1(object sender, EventArgs e)
        {
            var form = new FormYouTurn(this);
            form.ShowDialog();
            cboxpRowWidth.SelectedIndex = yt.rowSkipsWidth - 1;
        }

        private void toolstripAutoSteerConfig_Click_1(object sender, EventArgs e)
        {
            //check if window already exists
            Form fc = Application.OpenForms["FormSteer"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            //
            Form form = new FormSteer(this);
            form.Show();
        }

        private void toolStripAutoSteerChart_Click_1(object sender, EventArgs e)
        {
            //check if window already exists
            Form fcg = Application.OpenForms["FormSteerGraph"];

            if (fcg != null)
            {
                fcg.Focus();
                return;
            }

            //
            Form formG = new FormSteerGraph(this);
            formG.Show();
        }

        private void twoDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camera.camFollowing = true;
            camera.camPitch = 0;
        }

        private void threeDToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camera.camFollowing = true;
            camera.camPitch = -73;
        }

        private void northToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camera.camFollowing = false;
            camera.camPitch = 0;
        }

        private void dNorthToolStripMenuItem_Click(object sender, EventArgs e)
        {
            camera.camPitch = -73;
            camera.camFollowing = false;
        }

        private void toolStripDropDownButtonDistance_Click(object sender, EventArgs e)
        {
            fd.distanceUser = 0;
            fd.workedAreaTotalUser = 0;
        }
        
        private void googleEarthFlagsToolStrip_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                //save new copy of flags
                FileSaveFieldKML();

                //Process.Start(@"C:\Program Files (x86)\Google\Google Earth\client\googleearth", workingDirectory + currentFieldDirectory + "\\Flags.KML");
                Process.Start(fieldsDirectory + currentFieldDirectory + "\\Field.KML");
            }
            else
            {
                TimedMessageBox(1500, gStr.gsFieldIsOpen, gStr.gsStartNewField);
            }
        }

        private void treePlantToolStrip_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fc = Application.OpenForms["FormTreePlant"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            //
            Form form = new FormTreePlant(this);
            form.Show();
        }

        private void webcamToolStrip_Click(object sender, EventArgs e)
        {
            Form form = new FormWebCam();
            form.Show();
        }

        private void offsetFixToolStrip_Click(object sender, EventArgs e)
        {
            using (var form = new FormShiftPos(this))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) { }
            }
        }

        private void AutoSteerToolBtn_Click(object sender, EventArgs e)
        {
            //check if window already exists
            Form fc = Application.OpenForms["FormSteer"];

            if (fc != null)
            {
                fc.Focus();
                fc.Close();
                return;
            }

            //
            Form form = new FormSteer(this);
            form.Show();
        }

        private void vehicleToolStripBtn_Click(object sender, EventArgs e)
        {
            using (var form = new FormSettings(this, 0))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (Properties.Settings.Default.setAS_isAutoSteerAutoOn) btnAutoSteer.Text = "R";
                    else btnAutoSteer.Text = "M";
                }
            }
        }

        private void youTurnStripBtn_Click(object sender, EventArgs e)
        {
            var form = new FormYouTurn(this);
            form.ShowDialog();
            cboxpRowWidth.SelectedIndex = yt.rowSkipsWidth - 1;
        }

        private void toolStripBtnGPSStength_Click(object sender, EventArgs e)
        {
        }

        private void toolStripBtnDrag_ButtonClick(object sender, EventArgs e)
        {
            if (panelDrag.Visible)
            {
                offX = 0;
                offY = 0;
                panelDrag.Visible = false;
            }
            else
            {
                panelDrag.Top = 80;
                panelDrag.Left = 76;
                panelDrag.Visible = true;
            }
        }

        private void ZoomExtentsStripBtn_Click(object sender, EventArgs e)
        {
            if (camera.camSetDistance < -400) camera.camSetDistance = -75;
            else camera.camSetDistance = -3 * maxFieldDistance;
            if (camera.camSetDistance == 0) camera.camSetDistance = -2000;
            SetZoom();
        }

        private void topFieldViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Settings.Default.setMenu_isOGLZoomOn == 1)
            {
                Settings.Default.setMenu_isOGLZoomOn = 0;
                Settings.Default.Save();
                topFieldViewToolStripMenuItem.Checked = false;
                oglZoom.Width = 400;
                oglZoom.Height = 400;
                oglZoom.SendToBack();
            }
            else
            {
                Settings.Default.setMenu_isOGLZoomOn = 1;
                Settings.Default.Save();
                topFieldViewToolStripMenuItem.Checked = true;
                oglZoom.Visible = true;
                oglZoom.Width = 300;
                oglZoom.Height = 300;
                oglZoom.Left = 80;
                oglZoom.Top = 80;
                if (isJobStarted) oglZoom.BringToFront();
            }
        }

        private void headlandToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (bnd.bndArr.Count == 0)
            {
                TimedMessageBox(2000, gStr.gsNoBoundary, gStr.gsCreateABoundaryFirst);
                return;
            }

            GetHeadland();
        }

        private void toolToolStripMenu_Click(object sender, EventArgs e)
        {
            using (var form = new FormToolSettings(this, 0))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                }
            }
        }
        private void moduleConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }

            using (var form = new FormArduinoSettings(this))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                }
            }
        }
        
        private void simplifyToolStrip_Click(object sender, EventArgs e)
        {
            Settings.Default.setDisplay_isSimple = !Settings.Default.setDisplay_isSimple;
            Settings.Default.Save();

            FixPanelsAndMenus();
        }

        //File drop down items
        private void setWorkingDirectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Currently: " + Settings.Default.setF_workingDirectory;

            if (Settings.Default.setF_workingDirectory == "Default") fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else fbd.SelectedPath = Settings.Default.setF_workingDirectory;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                RegistryKey regKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\AgOpenGPS",true);

                if (fbd.SelectedPath != Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
                {
                    //save the user set directory in Registry
                    regKey.SetValue("Directory", fbd.SelectedPath);
                    regKey.Close();
                    Settings.Default.setF_workingDirectory = fbd.SelectedPath;
                    Settings.Default.Save();
                }
                else
                {
                    regKey.SetValue("Directory", "Default");
                    regKey.Close();
                    Settings.Default.setF_workingDirectory = "Default";
                    Settings.Default.Save();
                }

                //restart program
                MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
                Close();
            }
        }

        private void enterSimCoordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = DialogResult.Cancel;
            using (var form = new FormSimCoords(this))
            {
                result = form.ShowDialog();
            }

            if (result == DialogResult.OK)
            {
                if (isJobStarted)
                {
                    FileOpenField("Resume");
                }
                else
                {

                    pn.latitude = sim.latitude;
                    pn.longitude = sim.longitude;

                    double[] xy = pn.DecDeg2UTM(pn.latitude, pn.longitude);

                    pn.actualEasting = xy[0];
                    pn.actualNorthing = xy[1];

                    //reset the offsets
                    pn.utmEast = (int)pn.actualEasting;
                    pn.utmNorth = (int)pn.actualNorthing;

                    pn.fix.easting = pn.actualEasting - pn.utmEast;
                    pn.fix.northing = pn.actualNorthing - pn.utmNorth;

                    worldGrid.CreateWorldGrid(0, 0);

                    pn.zone = Math.Floor((pn.longitude + 180.0) * 0.16666666666666666666666666666667) + 1;

                    //calculate the central meridian of current zone
                    pn.centralMeridian = -177 + ((pn.zone - 1) * 6);

                    //Azimuth Error - utm declination
                    pn.convergenceAngle = Math.Atan(Math.Sin(Glm.ToRadians(pn.latitude))
                                                * Math.Tan(Glm.ToRadians(pn.longitude - pn.centralMeridian)));

                    //reset so it doesnt jump for program?

                    stepFixPts[0].easting = pn.fix.easting;
                    stepFixPts[0].northing = pn.fix.northing;
                    stepFixPts[0].heading = 0;
                }

                //Reset the sim
                sim.steerAngleScrollBar = 0;
                hsbarSteerAngle.Value = 400;
                btnResetSteerAngle.Text = 0.ToString("N1");
                sim.headingTrue = 0;
                sim.stepDistance = 1.3889;
                sim.speed = 5.0;
                hsbarStepDistance.Value = 5;

                //MessageBox.Show(gStr.gsProgramWillExitPleaseRestart, gStr.gsProgramWillExitPleaseRestart);
                //if (isJobStarted) JobClose();
                //Application.Exit();

                //Application.Restart();
                //Environment.Exit(0);
            }
        }

        private void topMenuLoadVehicle_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }

            using (var form = new FormVehiclePicker(this))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) { }
            }
        }

        private void topMenuSaveVehicle_Click(object sender, EventArgs e)
        {
            using (var form = new FormVehicleSaver(this))
            {
                var result = form.ShowDialog();
            }

        }

        private void topMenuLoadTool_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                //TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                //return;
            }

            using (var form = new FormToolPicker(this))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) { }
            }


            //if (FileOpenTool())
            //{
            //    using (var form = new FormToolSettings(this, 0))
            //    {
            //        var result = form.ShowDialog();
            //        if (result == DialogResult.OK) { }
            //    }

            //    TimedMessageBox(3000, gStr.gsDidYouMakeChanges, gStr.gsBeSureToSaveIfYouDid);
            //}
        }

        private void topMenuSaveTool_Click(object sender, EventArgs e)
        {
            using (var form = new FormToolSaver(this))
            {
                var result = form.ShowDialog();
            }
        }

        private void topMenuLoadEnvironment_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }

            using (var form = new FormEnvPicker(this))
            {
                var result = form.ShowDialog();
                if (result == DialogResult.Cancel)
                {
                    MessageBox.Show(gStr.gsRestartRequired,
                        gStr.gsFileError,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);
                    Application.Restart();
                    Environment.Exit(0);
                }
                else if (result == DialogResult.OK)
                {
                    Application.Restart();
                    Environment.Exit(0);
                }
                else if (result == DialogResult.Ignore)
                {
                    //Close();
                }
            }
        }
        private void topMenuSaveEnvironment_Click(object sender, EventArgs e)
        {
            using (var form = new FormEnvSaver(this))
            {
                var result = form.ShowDialog();
            }
        }

        //Help menu drop down items
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new Form_About())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) { }
            }
        }

        //Shortcut keys
        private void shortcutKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormShortcutKeys())
            {
                var result = form.ShowDialog();
                if (result == DialogResult.OK) { }
            }
        }

        //Options Drop down menu items
        private void resetALLToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                MessageBox.Show(gStr.gsCloseFieldFirst);
            }
            else
            {
                DialogResult result2 = MessageBox.Show(gStr.gsReallyResetEverything, gStr.gsResetAll,
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result2 == DialogResult.Yes)
                {
                    Settings.Default.Reset();
                    Settings.Default.Save();

                    Vehicle.Default.Reset();
                    Vehicle.Default.Save();

                    RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

                    //storing the values
                    key.SetValue("Language", "en");
                    key.SetValue("Directory", "Default");
                    key.Close();

                    Settings.Default.setF_culture = "en";
                    Settings.Default.setF_workingDirectory = "Default";
                    Settings.Default.Save();

                    MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
                    Application.Exit();
                }
            }
        }

        private void logNMEAMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void lightbarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            isLightbarOn = !isLightbarOn;
            lightbarToolStripMenuItem.Checked = isLightbarOn;
            Settings.Default.setMenu_isLightbarOn = isLightbarOn;
            Settings.Default.Save();
        }
        private void simulatorOnToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (spGPS.IsOpen)
            {
                simulatorOnToolStripMenuItem.Checked = false;
                panelSim.Visible = false;
                timerSim.Enabled = false;

                TimedMessageBox(2000, gStr.gsGPSConnected, gStr.gsSimulatorForcedOff);
            }
            else
            {
                if (isJobStarted)
                {
                    TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                    simulatorOnToolStripMenuItem.Checked = !simulatorOnToolStripMenuItem.Checked;
                    return;
                }
                if (simulatorOnToolStripMenuItem.Checked)
                {
                    panelSim.Visible = true;
                    timerSim.Enabled = true;
                    TimedMessageBox(2000, gStr.gsTurningOnSimulator, gStr.gsTurningOnSimulator);
                }
                else
                {
                    panelSim.Visible = false;
                    timerSim.Enabled = false;
                    TimedMessageBox(2000, gStr.gsTurningOffSimulator, gStr.gsTurningOffSimulator);
                }
            }

            Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
            Settings.Default.Save();
            LineUpManualBtns();
        }


        //The flag context menus
        private void toolStripMenuItemFlagRed_Click(object sender, EventArgs e)
        {
            flagColor = 0;
            btnFlag.Image = Properties.Resources.FlagRed;
        }
        private void toolStripMenuGrn_Click(object sender, EventArgs e)
        {
            flagColor = 1;
            btnFlag.Image = Properties.Resources.FlagGrn;
        }
        private void toolStripMenuYel_Click(object sender, EventArgs e)
        {
            flagColor = 2;
            btnFlag.Image = Properties.Resources.FlagYel;
        }
        private void toolStripMenuFlagForm_Click(object sender, EventArgs e)
        {
            Form fc = Application.OpenForms["FormFlags"];

            if (fc != null)
            {
                fc.Focus();
                return;
            }

            if (flagPts.Count > 0)
            {
                flagNumberPicked = 1;
                Form form = new FormFlags(this);
                form.Show();
            }            
        }

        //OpenGL Window context Menu and functions
        private void contextMenuStripOpenGL_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //dont bring up menu if no flag selected
            if (flagNumberPicked == 0) e.Cancel = true;
        }
        private void googleEarthOpenGLContextMenu_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                //save new copy of kml with selected flag and view in GoogleEarth
                FileSaveSingleFlagKML(flagNumberPicked);

                //Process.Start(@"C:\Program Files (x86)\Google\Google Earth\client\googleearth", workingDirectory + currentFieldDirectory + "\\Flags.KML");
                Process.Start(fieldsDirectory + currentFieldDirectory + "\\Flag.KML");
            }
        }

        //taskbar buttons    
        
        private void toolstripVR_Click(object sender, EventArgs e)
        {
            if (!isJobStarted)
            {
                TimedMessageBox(1000, gStr.gsFieldNotOpen, gStr.gsStartNewField);
                return;
            }

            //if (ABLine.isABLineSet | curve.isCurveSet)
            //{
            //    //field too small
            //    //using (var form = new FormHeadland(this))
            //    //{
            //    //    var result = form.ShowDialog();
            //    //    if (result == DialogResult.OK)
            //    //    {

            //    //    }
            //    //}
            //}
            //else { TimedMessageBox(3000, gStr.gsBoundaryNotSet, gStr.gsCreateBoundaryFirst); }
            //TimedMessageBox(1500, "Headlands not Implemented", "Some time soon they will be functional");
        }

        //Sim controls
        private void timerSim_Tick(object sender, EventArgs e)
        {
            //if a GPS is connected disable sim
            if (!spGPS.IsOpen)
            {
                if (isAutoSteerBtnOn && (guidanceLineDistanceOff != 32000)) sim.DoSimTick(guidanceLineSteerAngle * 0.01);
                else if (recPath.isDrivingRecordedPath) sim.DoSimTick(guidanceLineSteerAngle * 0.01);
                //else if (self.isSelfDriving) sim.DoSimTick(guidanceLineSteerAngle * 0.01);
                else sim.DoSimTick(sim.steerAngleScrollBar);
            }
        }

        private void hsbarSteerAngle_Scroll(object sender, ScrollEventArgs e)
        {
            sim.steerAngleScrollBar = (hsbarSteerAngle.Value - 400) * 0.1;
            btnResetSteerAngle.Text = sim.steerAngleScrollBar.ToString("N1");
        }

        private void hsbarStepDistance_Scroll(object sender, ScrollEventArgs e)
        {
            sim.stepDistance = ((double)(hsbarStepDistance.Value / 3.6));
        }

        private void btnResetSteerAngle_Click(object sender, EventArgs e)
        {
            sim.steerAngleScrollBar = 0;
            hsbarSteerAngle.Value = 400;
            btnResetSteerAngle.Text = 0.ToString("N1");
        }

        private void btnResetSim_Click(object sender, EventArgs e)
        {
            sim.steerAngleScrollBar = 0;
            hsbarSteerAngle.Value = 400;
            btnResetSteerAngle.Text = 0.ToString("N1");
            sim.headingTrue = 0;
            sim.stepDistance = 1.3889;
            sim.speed = 5.0;
            hsbarStepDistance.Value = 5;

            sim.latitude = Properties.Settings.Default.setGPS_SimLatitude;
            sim.longitude = Properties.Settings.Default.setGPS_SimLongitude;
        }

        //Languages
        private void menuLanguageEnglish_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("en");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);

        }
        private void menuLanguageDeutsch_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("de");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);

        }
        private void menuLanguageRussian_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("ru");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);
        }
        private void menuLanguageDutch_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("nl");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);
        }
        private void menuLanguageSpanish_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("es");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);
        }
        private void menuLanguageFrench_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("fr");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);
        }
        private void menuLanguageItalian_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("it");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);
        }
        private void menuLanguageUkranian_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("uk");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);
        }
        private void menuLanguageSlovak_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("sk");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);

        }
        private void menuLanguagesPolski_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("pl");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);
        }
        private void menuLanguageTest_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, gStr.gsFieldIsOpen, gStr.gsCloseFieldFirst);
                return;
            }
            SetLanguage("af");
            MessageBox.Show(gStr.gsProgramWillExitPleaseRestart);
            Application.Restart();
            Environment.Exit(0);
        }

        private void SetLanguage(string lang)
        {
            //reset them all to false
            menuLanguageEnglish.Checked = false;
            menuLanguageDeutsch.Checked = false;
            menuLanguageRussian.Checked = false;
            menuLanguageDutch.Checked = false;
            menuLanguageSpanish.Checked = false;
            menuLanguageFrench.Checked = false;
            menuLanguageItalian.Checked = false;
            menuLanguageUkranian.Checked = false;
            menuLanguageSlovak.Checked = false;
            menuLanguagePolish.Checked = false;

            menuLanguageTest.Checked = false;

            switch (lang)
            {
                case "en":
                    menuLanguageEnglish.Checked = true;
                    break;

                case "ru":
                    menuLanguageRussian.Checked = true;
                    break;

                case "de":
                    menuLanguageDeutsch.Checked = true;
                    break;

                case "nl":
                    menuLanguageDutch.Checked = true;
                    break;

                case "it":
                    menuLanguageItalian.Checked = true;
                    break;

                case "es":
                    menuLanguageSpanish.Checked = true;
                    break;

                case "fr":
                    menuLanguageFrench.Checked = true;
                    break;

                case "uk":
                    menuLanguageUkranian.Checked = true;
                    break;

                case "sk":
                    menuLanguageSlovak.Checked = true;
                    break;

                case "pl":
                    menuLanguagePolish.Checked = true;
                    break;

                case "af":
                    menuLanguageTest.Checked = true;
                    break;

                default:
                    menuLanguageEnglish.Checked = true;
                    lang = "en";
                    break;

            }

            //adding or editing "Language" subkey to the "SOFTWARE" subkey  
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

            //storing the values  
            key.SetValue("Language", lang);
            key.Close();
        }
    }//end class
}//end namespace