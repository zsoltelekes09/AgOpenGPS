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
            Settings.Default.setNTRIP_isOn = isNTRIP_TurnedOn = !isNTRIP_TurnedOn;
            Settings.Default.Save();

            UpdateNtripButton();
        }

        private void goPathMenu_Click(object sender, EventArgs e)
        {
            if (bnd.bndArr.Count == 0)
            {
                TimedMessageBox(2000, String.Get("gsNoBoundary"), String.Get("gsCreateABoundaryFirst"));
                return;
            }

            //if contour is on, turn it off
            if (ct.isContourBtnOn) { if (ct.isContourBtnOn) btnContour.PerformClick(); }

            isAutoSteerBtnOn = false;
            btnAutoSteer.Image = Properties.Resources.AutoSteerOff;
            if (yt.isYouTurnBtnOn) btnAutoYouTurn.PerformClick();


            YouTurnButtons(false);

            ABLines.BtnABLineOn = false;
            btnABLine.Image = Properties.Resources.ABLineOff;


            CurveLines.isOkToAddPoints = false;
            CurveLines.BtnCurveLineOn = false;
            btnCurve.Image = Properties.Resources.CurveOff;


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
                    TimedMessageBox(1500, String.Get("gsProblemMakingPath"), String.Get("gsCouldntGenerateValidPath"));
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
            //yt.ResetCreatedYouTurn();

            if (isAutoSteerBtnOn)
            {
                isAutoSteerBtnOn = false;
                btnAutoSteer.Image = Properties.Resources.AutoSteerOff;
                //if (yt.isYouTurnBtnOn) btnAutoYouTurn.PerformClick();
                mc.Send_AutoSteerButton[3] = 0x00;
                DataSend[8] = "Auto Steer Button: State Off";
                SendData(mc.Send_AutoSteerButton, false);//reset pulsecount = true & Autosteer off
            }
            else
            {
                if (ABLines.BtnABLineOn | ct.isContourBtnOn | CurveLines.BtnCurveLineOn)
                {
                    isAutoSteerBtnOn = true;
                    btnAutoSteer.Image = Properties.Resources.AutoSteerOn;
                    mc.Send_AutoSteerButton[3] = 0x01;
                    DataSend[8] = "Auto Steer Button: State On";
                    SendData(mc.Send_AutoSteerButton, false);//reset pulsecount = true & Autosteer on
                }
                else
                {
                    TimedMessageBox(2000, String.Get("gsNoGuidanceLines"), String.Get("gsTurnOnContourOrMakeABLine"));
                }
            }
        }

        private void BtnMakeLinesFromBoundary_Click(object sender, EventArgs e)
        {
            if (ct.isContourBtnOn) { if (ct.isContourBtnOn) btnContour.PerformClick(); }

            if (bnd.bndArr.Count == 0)
            {
                TimedMessageBox(2000, String.Get("gsNoBoundary"), String.Get("gsCreateABoundaryFirst"));
                return;
            }

            CurveLines.isOkToAddPoints = false;

            Form f = Application.OpenForms["FormABDraw"];
            if (f != null)
            {
                f.Focus();
                return;
            }
            Form form = new FormABDraw(this, false);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;

            if (CurveLines.BtnCurveLineOn) btnCycleLines.Text = "Cu-" + CurveLines.CurrentLine + 1;
            if (ABLines.BtnABLineOn) btnCycleLines.Text = "AB-" + ABLines.CurrentLine;
        }

        private void btnCycleLines_Click(object sender, EventArgs e)
        {
            if (ABLines.BtnABLineOn && ABLines.ABLines.Count > 0)
            {
                ABLines.CurrentLine = (ABLines.ABLines.Count > 0) ? (ABLines.CurrentLine + 1) % ABLines.ABLines.Count : -1;

                yt.ResetYouTurn();
                btnCycleLines.Text = "AB-" + ABLines.CurrentLine;
                if (tram.displayMode > 0) ABLines.BuildTram();

            }
            else if (CurveLines.BtnCurveLineOn && CurveLines.Lines.Count > 0)
            {
                CurveLines.OldHowManyPathsAway = double.NegativeInfinity;

                CurveLines.CurrentLine = (CurveLines.Lines.Count > 0) ? (CurveLines.CurrentLine + 1) % CurveLines.Lines.Count : -1;

                yt.ResetYouTurn();
                btnCycleLines.Text = "Cur-" + CurveLines.CurrentLine + 1;
                if (tram.displayMode > 0) CurveLines.BuildTram();
            }
        }

        private void btnCurve_Click(object sender, EventArgs e)
        {
            Button test = (Button)sender;

            btnCycleLines.Text = "";

            //if contour is on, turn it off
            if (ct.isContourBtnOn) { if (ct.isContourBtnOn) btnContour.PerformClick(); }

            //check if window already exists
            Form f = Application.OpenForms["FormABCurve"];

            if (f != null)
            {
                f.Focus();
                return;
            }
            else
            {
                Form form = new FormABCurve(this, test.Name == "btnCurve");
                form.Show(this);
                form.Left = Left + Width / 2 - form.Width / 2;
                form.Top = Top + Height / 2 - form.Height / 2;
            }
            if (test.Name == "btnCurve")
            {
                btnCurve.Image = Properties.Resources.CurveOn;
                CurveLines.BtnCurveLineOn = true;
                btnABLine.Image = Properties.Resources.ABLineOff;
                ABLines.BtnABLineOn = false;
            }
            else
            {
                btnABLine.Image = Properties.Resources.ABLineOn;
                ABLines.BtnABLineOn = true;
                btnCurve.Image = Properties.Resources.CurveOff;
                CurveLines.BtnCurveLineOn = false;
            }
            YouTurnButtons(true);
        }

        private void btnContour_Click(object sender, EventArgs e)
        {
            ct.isContourBtnOn = !ct.isContourBtnOn;
            btnContour.Image = ct.isContourBtnOn ? Properties.Resources.ContourOn : Properties.Resources.ContourOff;

            if (ct.isContourBtnOn)
            {
                //turn off youturn...
                YouTurnButtons(false);
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
                if (ABLines.BtnABLineOn | CurveLines.BtnCurveLineOn)
                {
                    YouTurnButtons(true);
                }
                btnContourPriority.Image = Properties.Resources.Snap2;
            }
        }

        private void btnContourPriority_Click(object sender, EventArgs e)
        {
            if (ct.isContourBtnOn)
            {
                ct.isRightPriority = !ct.isRightPriority;
                btnContourPriority.Image = ct.isRightPriority ? Properties.Resources.ContourPriorityRight : Properties.Resources.ContourPriorityLeft;
            }
            else
            {
                if (ABLines.BtnABLineOn)
                {
                    ABLines.MoveLine(ABLines.distanceFromRefLine);
                    ABLines.distanceFromRefLine = 0;
                }
                else if (CurveLines.BtnCurveLineOn)
                {
                    CurveLines.MoveLine(CurveLines.distanceFromRefLine);
                    CurveLines.distanceFromRefLine = 0;
                }
                else
                {
                    TimedMessageBox(2000, String.Get("gsNoGuidanceLines"), String.Get("gsTurnOnContourOrMakeABLine"));
                }
            }
        }

        //Snaps
        private void SnapRight()
        {
            if (!ct.isContourBtnOn)
            {
                if (ABLines.BtnABLineOn && ABLines.ABLines.Count > 0)
                {
                    //snap distance is in cm
                    yt.ResetCreatedYouTurn();
                    double dist = 0.01 * Properties.Settings.Default.setAS_snapDistance;

                    ABLines.MoveLine(isABSameAsVehicleHeading ? dist : -dist);
                }
                else if (CurveLines.BtnCurveLineOn && CurveLines.Lines.Count > 0)
                {
                    //snap distance is in cm
                    yt.ResetCreatedYouTurn();
                    double dist = 0.01 * Properties.Settings.Default.setAS_snapDistance;

                    CurveLines.MoveLine(isABSameAsVehicleHeading ? dist : -dist);
                }
                else
                {
                    TimedMessageBox(2000, String.Get("gsNoGuidanceLines"), String.Get("gsTurnOnContourOrMakeABLine"));
                }
            }

        }

        private void SnapLeft()
        {
            if (!ct.isContourBtnOn)
            {
                if (ABLines.BtnABLineOn)
                {
                    //snap distance is in cm
                    yt.ResetCreatedYouTurn();
                    double dist = 0.01 * Properties.Settings.Default.setAS_snapDistance;

                    ABLines.MoveLine(isABSameAsVehicleHeading ? -dist : dist);
                }
                else if (CurveLines.BtnCurveLineOn && CurveLines.Lines.Count > 0)
                {
                    //snap distance is in cm
                    yt.ResetCreatedYouTurn();
                    double dist = 0.01 * Properties.Settings.Default.setAS_snapDistance;

                    CurveLines.MoveLine(isABSameAsVehicleHeading ? -dist : dist);

                }
                else
                {
                    TimedMessageBox(2000, String.Get("gsNoGuidanceLines"), String.Get("gsTurnOnContourOrMakeABLine"));
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
                    Tools[i].Sections[j].BtnSectionState = autoBtnState;
                    Tools[i].Sections[j].SectionButton.Enabled = isJobStarted;
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

        private void btnEditAB_Click(object sender, EventArgs e)
        {
            if ((ABLines.CurrentLine > -1 && ABLines.BtnABLineOn) || (CurveLines.CurrentLine > -1 && CurveLines.BtnCurveLineOn))
            {
                Form fc = Application.OpenForms["FormEditAB"];

                if (fc != null)
                {
                    fc.Focus();
                    return;
                }
                Form form = new FormEditAB(this, CurveLines.CurrentLine > -1 && CurveLines.BtnCurveLineOn);
                form.Show(this);
                form.Left = Left + Width / 2 - form.Width / 2;
                form.Top = Top + Height / 2 - form.Height / 2;
            }
            else
            {
                TimedMessageBox(1500, String.Get("gsNoABLineActive"), String.Get("gsPleaseEnterABLine"));
                return;
            }
        }

        private void btnTramMenu_Click(object sender, EventArgs e)
        {
            CurveLines.isOkToAddPoints = false;

            if (ct.isContourBtnOn) { if (ct.isContourBtnOn) btnContour.PerformClick(); }

            if ((ABLines.CurrentLine > -1 && ABLines.BtnABLineOn) || (CurveLines.CurrentLine > -1 && CurveLines.BtnCurveLineOn))
            {
                Form f = Application.OpenForms["FormTram"];
                if (f != null)
                {
                    f.Focus();
                    return;
                }
                Form form = new FormTram(this, CurveLines.CurrentLine > -1 && CurveLines.BtnCurveLineOn);
                form.Show(this);
                form.Left = Left + Width/2 - form.Width/2;
                form.Top = Top + Height/2 - form.Height/2;
            }
            else
            {
                TimedMessageBox(1500, String.Get("gsNoABLineActive"), String.Get("gsPleaseEnterABLine"));
                ABLines.isEditing = false;
                return;
            }
        }

        private void btnFlag_Click(object sender, EventArgs e)
        {
            int nextflag = flagPts.Count + 1;
            CFlag flagPt = new CFlag(pn.latitude, pn.longitude, pn.fix.Easting, pn.fix.Northing, fixHeading, flagColor, nextflag, (nextflag).ToString());
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
                form.Show(this);
                form.Left = Left + Width / 2 - form.Width / 2;
                form.Top = Top + Height / 2 - form.Height / 2;
            }
        }

        private void btnAutoYouTurn_Click(object sender, EventArgs e)
        {
            yt.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            yt.ResetYouTurn();

            if (!yt.isYouTurnBtnOn)
            {
                if (bnd.bndArr.Count == 0)
                {
                    TimedMessageBox(2000, String.Get("gsNoBoundary"), String.Get("gsCreateABoundaryFirst"));
                    return;
                }
                if (ABLines.BtnABLineOn || CurveLines.BtnCurveLineOn)
                {
                    if (!isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                }
                else return;

                yt.isYouTurnBtnOn = true;
                btnAutoYouTurn.Image = Properties.Resources.Youturn80;
            }
            else
            {
                yt.isYouTurnBtnOn = false;
                btnAutoYouTurn.Image = Properties.Resources.YouTurnNo;
            }
        }

        // the context menu from autosteer button
        private void autoUTurnHeadLiftStrip_Click(object sender, EventArgs e)
        {
            if (hd.headArr.Count > 0)
            {
                hd.BtnHeadLand = !hd.BtnHeadLand;
                if (hd.BtnHeadLand) btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                else btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
            }

            if (!yt.isYouTurnBtnOn)  btnAutoYouTurn.PerformClick();

            if (!vehicle.BtnHydLiftOn) btnHydLift.PerformClick();
        }

        private void btnHeadlandOnOff_Click(object sender, EventArgs e)
        {
            if (hd.headArr[0].HeadLine.Count > 0)
            {
                hd.BtnHeadLand = !hd.BtnHeadLand;
                if (hd.BtnHeadLand) btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
                else btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;
            }

            if (!hd.BtnHeadLand)
            {
                vehicle.BtnHydLiftOn = false;
                btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
            }
        }

        private void btnHydLift_Click(object sender, EventArgs e)
        {
            if (hd.BtnHeadLand)
            {
                vehicle.BtnHydLiftOn = !vehicle.BtnHydLiftOn;
                if (vehicle.BtnHydLiftOn)
                {
                    btnHydLift.Image = Properties.Resources.HydraulicLiftOn;
                }
                else
                {
                    btnHydLift.Image = Properties.Resources.HydraulicLiftOff;
                }
            }
            else
            {
                vehicle.BtnHydLiftOn = false;
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
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;

        }

        private void lblSpeed_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormGPSData"];
            if (f != null)
            {
                f.Close();
                return;
            }
            Form form = new FormGPSData(this);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
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
                FormBorderStyle = FormBorderStyle.None;
                WindowState = FormWindowState.Maximized;
                btnFullScreen.BackgroundImage = Properties.Resources.WindowNormal;
            }
            else
            {
                FormBorderStyle = FormBorderStyle.Sizable;
                WindowState = FormWindowState.Normal;
                btnFullScreen.BackgroundImage = Properties.Resources.WindowFullScreen;
            }
        }

        private void BtnShutdown_Click(object sender, EventArgs e)
        {
            var form = new FormShutDown(this);
            form.ShowDialog(this);
        }

        private void BtnNoise_Click(object sender, EventArgs e)
        {
            btnNoise.BackColor = (isSimNoisy = !isSimNoisy) ? Color.LightGreen : Color.LightSalmon;
        }

        private void btnReverseDirection_Click(object sender, EventArgs e)
        {
            CurveLines.OldHowManyPathsAway = double.NegativeInfinity;
            CurveLines.isSameWay = !CurveLines.isSameWay;
            sim.headingTrue += Math.PI;
            if (sim.headingTrue > Glm.twoPI) sim.headingTrue -= Glm.twoPI;
            if (sim.headingTrue < 0) sim.headingTrue += Glm.twoPI;
            yt.ResetYouTurn();
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
                Form f = Application.OpenForms["FormBoundary"];
                if (f != null)
                {
                    f.Focus();
                    return;
                }

                Form form = new FormBoundary(this);
                form.Show(this);
                form.Left = Left + Width / 2 - form.Width / 2;
                form.Top = Top + Height / 2 - form.Height / 2;

            }
            else { TimedMessageBox(3000, String.Get("gsFieldNotOpen"), String.Get("gsStartNewField")); }
        }

        private void toolStripBtnMakeBndContour_Click_1(object sender, EventArgs e)
        {
            //build all the contour guidance lines from boundaries, all of them.
            using (var form = new FormMakeBndCon(this))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK) { }
            }
        }

        private void deleteContourPathsToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            //FileCreateContour();
            ct.stripList.Clear();
            ct.ctList.Clear();
            ContourSaveList.Clear();
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

        private void toolStripAreYouSure_Click_1(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                DialogResult result3 = MessageBox.Show(String.Get("gsDeleteAllContoursAndSections"),
                    String.Get("gsDeleteForSure"),
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button2);
                if (result3 == DialogResult.Yes)
                {

                    for (int i = 0; i < Tools.Count; i++)
                    {
                        //clear the section lists
                        for (int j = 0; j< Tools[i].Sections.Count; j++)
                        {
                            if (Tools[i].Sections[j].IsMappingOn)
                            {
                                Tools[i].Sections[j].TurnMappingOff();
                                //clean out the lists
                                Tools[i].Sections[j].triangleList.Clear();
                                Tools[i].Sections[j].TurnMappingOn();
                            }
                        }
                    }

                    //clear out the contour Lists
                    ct.StopContourLine(pivotAxlePos);
                    ct.ResetContour();
                    fd.workedAreaTotal = 0;
                    PatchSaveList.Clear();
                    PatchDrawList.Clear();

                    FileCreateContour();
                    FileCreateSections();
                }
                else
                {
                    TimedMessageBox(1500, String.Get("gsNothingDeleted"), String.Get("gsActionHasBeenCancelled"));
                }
            }
        }

        private void toolStripBtnField_Click(object sender, EventArgs e)
        {
            JobNewOpenResume();
        }

        private void SmoothABtoolStripMenu_Click(object sender, EventArgs e)
        {
            if (isJobStarted && CurveLines.BtnCurveLineOn)
            {
                using (var form = new FormSmoothAB(this))
                {
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.OK) { }
                }
            }
            else
            {
                if (!isJobStarted) TimedMessageBox(2000, String.Get("gsFieldNotOpen"), String.Get("gsStartNewField"));
                else TimedMessageBox(2000, String.Get("gsCurveNotOn"), String.Get("gsTurnABCurveOn"));
            }
        }

        private void toolstripDisplayConfig_Click_1(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormIMU"];
            if (f != null)
            {
                f.Focus();
                return;
            }
            Form form = new FormIMU(this);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;

        }
        private void toolstripUSBPortsConfig_Click_1(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormCommSet"];
            if (f != null)
            {
                f.Focus();
                return;
            }

            Form form = new FormCommSet(this);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
        }

        private void toolstripUDPConfig_Click_1(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormUDP"];

            if (f != null)
            {
                f.Focus();
                return;
            }

            Form form = new FormUDP(this);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
        }

        private void toolStripNTRIPConfig_Click_1(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormNtrip"];
            if (f != null)
            {
                f.Focus();
                return;
            }

            Form form = new FormNtrip(this);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
        }

        private void toolstripYouTurnConfig_Click_1(object sender, EventArgs e)
        {
            var form = new FormYouTurn(this);
            form.ShowDialog(this);
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
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
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
            Form form = new FormSteerGraph(this);
            form.Show();
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
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
                TimedMessageBox(1500, String.Get("gsFieldIsOpen"), String.Get("gsStartNewField"));
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
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
        }

        private void webcamToolStrip_Click(object sender, EventArgs e)
        {
            Form form = new FormWebCam(this);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
        }

        private void offsetFixToolStrip_Click(object sender, EventArgs e)
        {
            using (var form = new FormShiftPos(this))
            {
                var result = form.ShowDialog(this);
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
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
        }

        private void vehicleToolStripBtn_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormSettings"];
            if (f != null)
            {
                f.Focus();
                return;
            }

            Form form = new FormSettings(this, 0);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
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
                oglZoom.Visible = false;
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
                TimedMessageBox(2000, String.Get("gsNoBoundary"), String.Get("gsCreateABoundaryFirst"));
                return;
            }

            Form f = Application.OpenForms["FormHeadland"];
            if (f != null)
            {
                f.Focus();
                return;
            }
            Form form = new FormABDraw(this, true);
            form.Show(this);
            form.Left = Left + Width / 2 - form.Width / 2;
            form.Top = Top + Height / 2 - form.Height / 2;
        }

        private void toolToolStripMenu_Click(object sender, EventArgs e)
        {
            using (var form = new FormToolSettings(this, 0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                }
            }
        }
        private void moduleConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormArduinoSettings(this)) form.ShowDialog(this);
        }

        private void OptionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form f = Application.OpenForms["FormDisplayOptions"];

            if (f != null) f.Focus();
            else
            {
                Form form = new FormDisplayOptions(this);
                form.Show(this);
                form.Left = Left + Width / 2 - form.Width / 2;
                form.Top = Top + Height / 2 - form.Height / 2;
            }
        }

        private void ColorsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormColor(this))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                }
            }
        }

        private void StripSectionColor_Click(object sender, EventArgs e)
        {
            using (var form = new FormColorPicker(this, sectionColorDay))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    sectionColorDay = form.UseThisColor;
                }
            }

            Settings.Default.setDisplay_colorSectionsDay = sectionColorDay;
            Settings.Default.Save();

            stripSectionColor.BackColor = sectionColorDay;
        }

        private void KeyboardToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            isKeyboardOn = !isKeyboardOn;
            keyboardToolStripMenuItem1.Checked = isKeyboardOn;
            Settings.Default.setDisplay_isKeyboardOn = isKeyboardOn;
            Settings.Default.Save();
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
                TimedMessageBox(2000, String.Get("gsFieldIsOpen"), String.Get("gsCloseFieldFirst"));
                return;
            }

            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.ShowNewFolderButton = true;
            fbd.Description = "Currently: " + Settings.Default.setF_workingDirectory;

            if (Settings.Default.setF_workingDirectory == "Default") fbd.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            else fbd.SelectedPath = Settings.Default.setF_workingDirectory;

            if (fbd.ShowDialog(this) == DialogResult.OK)
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
                MessageBox.Show(String.Get("gsProgramWillExitPleaseRestart"));
                Close();
            }
        }

        private void enterSimCoordsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var result = DialogResult.Cancel;
            using (var form = new FormSimCoords(this))
            {
                result = form.ShowDialog(this);
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

                    pn.fix.Easting = pn.actualEasting - pn.utmEast;
                    pn.fix.Northing = pn.actualNorthing - pn.utmNorth;

                    worldGrid.CheckWorldGrid(pn.fix.Northing, pn.fix.Easting);

                    pn.zone = Math.Floor((pn.longitude + 180.0) * 0.16666666666666666666666666666667) + 1;

                    //calculate the central meridian of current zone
                    pn.centralMeridian = -177 + ((pn.zone - 1) * 6);

                    //Azimuth Error - utm declination
                    pn.convergenceAngle = Math.Atan(Math.Sin(Glm.ToRadians(pn.latitude))
                                                * Math.Tan(Glm.ToRadians(pn.longitude - pn.centralMeridian)));

                    //reset so it doesnt jump for program?

                    stepFixPts[0].Easting = pn.fix.Easting;
                    stepFixPts[0].Northing = pn.fix.Northing;
                    stepFixPts[0].Heading = 0;
                }

                //Reset the sim
                sim.steerAngleScrollBar = 0;
                hsbarSteerAngle.Value = 400;
                btnResetSteerAngle.Text = 0.ToString("N1");
                sim.headingTrue = 0;
                sim.stepDistance = 1.3889;
                sim.speed = 5.0;
                hsbarStepDistance.Value = 5;
            }
        }

        private void topMenuLoadVehicle_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, String.Get("gsFieldIsOpen"), String.Get("gsCloseFieldFirst"));
                return;
            }

            using (var form = new FormVehiclePicker(this))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK) { }
            }
        }

        private void topMenuSaveVehicle_Click(object sender, EventArgs e)
        {
            using (var form = new FormVehicleSaver(this))
            {
                var result = form.ShowDialog(this);
            }

        }

        private void topMenuLoadTool_Click(object sender, EventArgs e)
        {
            using (var form = new FormToolPicker(this))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK) { }
            }
        }

        private void topMenuSaveTool_Click(object sender, EventArgs e)
        {
            using (var form = new FormToolSaver(this))
            {
                var result = form.ShowDialog(this);
            }
        }

        private void topMenuLoadEnvironment_Click(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                TimedMessageBox(2000, String.Get("gsFieldIsOpen"), String.Get("gsCloseFieldFirst"));
                return;
            }

            using (var form = new FormEnvPicker(this))
            {

                form.ShowDialog(this);

                LoadSettings();
            }
        }
        private void topMenuSaveEnvironment_Click(object sender, EventArgs e)
        {
            using (var form = new FormEnvSaver(this))
            {
                var result = form.ShowDialog(this);
            }
        }

        //Help menu drop down items
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new Form_About(this))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK) { }
            }
        }

        //Shortcut keys
        private void shortcutKeysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new FormShortcutKeys(this))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK) { }
            }
        }

        //Options Drop down menu items
        private void resetALLToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (isJobStarted)
            {
                MessageBox.Show(String.Get("gsCloseFieldFirst"));
            }
            else
            {
                DialogResult result2 = MessageBox.Show(String.Get("gsReallyResetEverything"), String.Get("gsResetAll"),
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

                    LoadSettings();
                }
            }
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

                TimedMessageBox(2000, String.Get("gsGPSConnected"), String.Get("gsSimulatorForcedOff"));
            }
            else
            {
                if (isJobStarted)
                {
                    TimedMessageBox(2000, String.Get("gsFieldIsOpen"), String.Get("gsCloseFieldFirst"));
                    simulatorOnToolStripMenuItem.Checked = !simulatorOnToolStripMenuItem.Checked;
                    return;
                }
                if (simulatorOnToolStripMenuItem.Checked)
                {
                    panelSim.Visible = true;
                    timerSim.Enabled = true;
                    TimedMessageBox(2000, String.Get("gsTurningOnSimulator"), String.Get("gsTurningOnSimulator"));
                }
                else
                {
                    panelSim.Visible = false;
                    timerSim.Enabled = false;
                    TimedMessageBox(2000, String.Get("gsTurningOffSimulator"), String.Get("gsTurningOffSimulator"));
                }
            }

            Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
            Settings.Default.Save();
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
                form.Show(this);
                form.Left = Left + Width / 2 - form.Width / 2;
                form.Top = Top + Height / 2 - form.Height / 2;
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

        private void SetLanguage(object sender, EventArgs e)
        {
            //reset them all to false
            en.Checked = false;
            de.Checked = false;
            ru.Checked = false;
            nl.Checked = false;
            es.Checked = false;
            fr.Checked = false;
            it.Checked = false;
            uk.Checked = false;
            sk.Checked = false;
            pl.Checked = false;


            string Language = "en";
            if (sender is ToolStripMenuItem StripItem)
            {
                Language = StripItem.Name;
                StripItem.Checked = true;
            }
            else
            {
                Language = sender.ToString();
                ToolStripMenuItem test = (ToolStripMenuItem)menustripLanguage.DropDownItems[Language];
                test.Checked = true;
            }
            String.Culture = new System.Globalization.CultureInfo(Settings.Default.setF_culture = Language);

            Settings.Default.Save();
            UpdateGuiText();

            //adding or editing "Language" subkey to the "SOFTWARE" subkey  
            RegistryKey key = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\AgOpenGPS");

            //storing the values  
            key.SetValue("Language", Language);
            key.Close();
        }
    }//end class
}//end namespace