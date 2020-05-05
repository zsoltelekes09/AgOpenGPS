using System;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using System.Globalization;
using System.IO;
using System.Collections.Generic;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        //ABLines directory
        public string ablinesDirectory;

        //colors for sections and field background
        public byte flagColor = 0;

        //how many cm off line per big pixel
        public int lightbarCmPerPixel;

        //polygon mode for section drawing
        public bool isDrawPolygons;

        //Is it in 2D or 3D, metric or imperial, display lightbar, display grid etc
        public bool isMetric = true, isLightbarOn = true, isGridOn, isFullScreen;
        public bool isUTurnAlwaysOn, isAutoLoadFields, isCompassOn, isSpeedoOn, isAutoDayNight, isSideGuideLines = true;
        public bool isPureDisplayOn = true, isSkyOn = true, isRollMeterOn = false;
        public bool isDay = true, isDayTime = true;
        public bool isKeyboardOn = true;

        //master Manual and Auto, 3 states possible
        public enum btnStates { Off, Auto, On }
        public btnStates autoBtnState = btnStates.Off;

        public Color dayColor = Properties.Settings.Default.setDisplay_colorDayMode;
        public Color nightColor = Properties.Settings.Default.setDisplay_colorNightMode;
        public Color sectionColorDay = Properties.Settings.Default.setDisplay_colorSectionsDay;
        public Color sectionColorNight = Properties.Settings.Default.setDisplay_colorSectionsNight;
        public Color fieldColorDay = Properties.Settings.Default.setDisplay_colorFieldDay;
        public Color fieldColorNight = Properties.Settings.Default.setDisplay_colorFieldNight;

        public int[] customColorsList = new int[16];

        //sunrise sunset
        public DateTime dateToday = DateTime.Today;
        public DateTime sunrise = DateTime.Now;
        public DateTime sunset = DateTime.Now;

        private void IsBetweenSunriseSunset(double lat, double lon)
        {
            CSunTimes.Instance.CalculateSunRiseSetTimes(pn.latitude, pn.longitude, dateToday, ref sunrise, ref sunset);
            isDay = (DateTime.Now.Ticks < sunset.Ticks && DateTime.Now.Ticks > sunrise.Ticks);
        }

        private void LoadGUI()
        {
            isSkyOn = Settings.Default.setMenu_isSkyOn;
            isGridOn = Settings.Default.setMenu_isGridOn;
            isCompassOn = Settings.Default.setMenu_isCompassOn;
            isSpeedoOn = Settings.Default.setMenu_isSpeedoOn;
            isAutoDayNight = Settings.Default.setDisplay_isAutoDayNight;
            isSideGuideLines = Settings.Default.setMenu_isSideGuideLines;
            isLogNMEA = Settings.Default.setMenu_isLogNMEA;
            isPureDisplayOn = Settings.Default.setMenu_isPureOn;
            isUTurnAlwaysOn = Settings.Default.setMenu_isUTurnAlwaysOn;
            isAutoLoadFields = Settings.Default.AutoLoadFields;
            //set the language to last used
            SetLanguage(Settings.Default.setF_culture);

            currentVersionStr = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);

            string[] fullVers = currentVersionStr.Split('.');
            int inoV = int.Parse(fullVers[0], CultureInfo.InvariantCulture);
            inoV += int.Parse(fullVers[1], CultureInfo.InvariantCulture);
            inoV += int.Parse(fullVers[2], CultureInfo.InvariantCulture);
            inoVersionInt = inoV;
            inoVersionStr = inoV.ToString();

            simulatorOnToolStripMenuItem.Checked = Settings.Default.setMenu_isSimulatorOn;
            if (simulatorOnToolStripMenuItem.Checked)
            {
                panelSim.Visible = true;
                timerSim.Enabled = true;
            }
            else
            {
                panelSim.Visible = false;
                timerSim.Enabled = false;
            }

            fixUpdateHz = Properties.Settings.Default.setPort_NMEAHz;
            if (timerSim.Enabled) fixUpdateHz = 10;
            fixUpdateTime = 1 / (double)fixUpdateHz;

            //set the flag mark button to red dot
            btnFlag.Image = Properties.Resources.FlagRed;

            //night mode
            //isDay = Properties.Settings.Default.setDisplay_isDayMode;
            isDay = !isDay;
            SwapDayNightMode();

            //load the string of custom colors
            string[] words = Properties.Settings.Default.setDisplay_customColors.Split(',');
            for (int i = 0; i < 16; i++)
            {
                customColorsList[i] = int.Parse(words[i], CultureInfo.InvariantCulture);
            }

            //metric settings
            isMetric = Settings.Default.setMenu_isMetric;

            //load up colors
            fieldColorDay = (Settings.Default.setDisplay_colorFieldDay);
            sectionColorDay = (Settings.Default.setDisplay_colorSectionsDay);
            fieldColorNight = (Settings.Default.setDisplay_colorFieldNight);
            sectionColorNight = (Settings.Default.setDisplay_colorSectionsNight);

            DisableYouTurnButtons();

            isLightbarOn = Settings.Default.setMenu_isLightbarOn;
            lightbarToolStripMenuItem.Checked = isLightbarOn;

            //set up grid and lightbar

            isKeyboardOn = Settings.Default.setDisplay_isKeyboardOn;
            keyboardToolStripMenuItem1.Checked = isKeyboardOn;


            if (Settings.Default.setMenu_isOGLZoomOn == 1)
                topFieldViewToolStripMenuItem.Checked = true;
            else topFieldViewToolStripMenuItem.Checked = false;

            oglZoom.Width = 400;
            oglZoom.Height = 400;
            oglZoom.Visible = true;
            oglZoom.Left = 300;
            oglZoom.Top = 80;

            oglZoom.SendToBack();


            LineUpManualBtns();

            yt.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            cboxpRowWidth.SelectedIndex = yt.rowSkipsWidth - 1;

            if (Properties.Settings.Default.setAS_isAutoSteerAutoOn) btnAutoSteer.Text = "R";
            else btnAutoSteer.Text = "M";

            //panelSim.Location = Settings.Default.setDisplay_panelSimLocation;

            FixPanelsAndMenus();

            layoutPanelRight.Enabled = false;
            //boundaryToolStripBtn.Enabled = false;
            toolStripBtnDropDownBoundaryTools.Enabled = false;

            if (isNTRIP_TurnedOn)
            {
                //btnStartStopNtrip.Visible = true;
                NTRIPStartStopStrip.Visible = true;
                lblWatch.Visible = true;
                NTRIPBytesMenu.Visible = true;
                pbarNtripMenu.Visible = true;
            }
            else
            {
                //btnStartStopNtrip.Visible = false;
                NTRIPStartStopStrip.Visible = false;
                lblWatch.Visible = false;
                NTRIPBytesMenu.Visible = false;
                pbarNtripMenu.Visible = false;
            }

            if (hd.isOn) btnHeadlandOnOff.Image = Properties.Resources.HeadlandOn;
            else btnHeadlandOnOff.Image = Properties.Resources.HeadlandOff;

            stripSectionColor.BackColor = sectionColorDay;

            if (Properties.Settings.Default.setDisplay_isTermsOn)
            {
                using (var form = new Form_First())
                {
                    var result = form.ShowDialog();
                    if (result != DialogResult.OK)
                    {
                        Close();
                    }
                }
            }

            if (Properties.Settings.Default.setDisplay_isStartFullScreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                btnFullScreen.BackgroundImage = Properties.Resources.WindowNormal;
                isFullScreen = true;
            }
            else
            {
                isFullScreen = false;
            }

            //is rtk on?
            isRTK = Properties.Settings.Default.setGPS_isRTK;



            if (isAutoLoadFields)
            {


                Fields.Clear();


                string[] dirs = Directory.GetDirectories(fieldsDirectory);
                foreach (string dir in dirs)
                {
                    double northingOffset = 0;
                    double eastingOffset = 0;
                    double convergenceAngle = 0;
                    string fieldDirectory = Path.GetFileName(dir);
                    string filename = dir + "\\Field.txt";
                    string line;

                    //make sure directory has a field.txt in it
                    if (File.Exists(filename))
                    {
                        using (StreamReader reader = new StreamReader(filename))
                        {
                            try
                            {
                                //Date time line
                                for (int i = 0; i < 4; i++)
                                {
                                    line = reader.ReadLine();
                                }

                                //start positions
                                if (!reader.EndOfStream)
                                {
                                    line = reader.ReadLine();
                                    string[] offs = line.Split(',');

                                    eastingOffset = (double.Parse(offs[0], CultureInfo.InvariantCulture));
                                    northingOffset = (double.Parse(offs[1], CultureInfo.InvariantCulture));
                                    line = reader.ReadLine();
                                    if (!reader.EndOfStream)
                                    {
                                        line = reader.ReadLine();
                                        convergenceAngle = double.Parse(line, CultureInfo.InvariantCulture);
                                    }
                                }
                            }
                            catch (Exception)
                            {
                            }
                        }

                        //grab the boundary
                        filename = dir + "\\Boundary.txt";

                        if (File.Exists(filename))
                        {
                            using (StreamReader reader = new StreamReader(filename))
                            {
                                try
                                {
                                    //read header
                                    line = reader.ReadLine();//Boundary

                                    if (!reader.EndOfStream) //empty boundary field
                                    {
                                        //True or False OR points from older boundary files
                                        line = reader.ReadLine();


                                        //Check for older boundary files, then above line string is num of points
                                        if (line == "True" || line == "False")
                                        {
                                            line = reader.ReadLine(); //number of points
                                            line = reader.ReadLine(); //number of points
                                        }

                                        int numPoints = int.Parse(line);
                                        if (numPoints > 0)
                                        {
                                            Fields.Add(new CAutoLoadField());
                                            Fields[Fields.Count - 1].Dir = Path.GetFileName(dir);

                                            //load the line
                                            for (int i = 0; i < numPoints; i++)
                                            {
                                                line = reader.ReadLine();
                                                string[] words2 = line.Split(',');
                                                double easting = double.Parse(words2[0], CultureInfo.InvariantCulture);
                                                double northing = double.Parse(words2[1], CultureInfo.InvariantCulture);
                                                vec2 vecPt = new vec2((Math.Cos(convergenceAngle) * easting) - (Math.Sin(convergenceAngle) * northing) + eastingOffset, (Math.Sin(convergenceAngle) * easting) + (Math.Cos(convergenceAngle) * northing) + northingOffset);
                                                Fields[Fields.Count-1].Boundary.Add(vecPt);
                                            }
                                        }
                                    }
                                }
                                catch (Exception)
                                {
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SwapDayNightMode()
        {
            isDay = !isDay;
            if (isDay)
            {
                btnDayNightMode.Image = Properties.Resources.WindowNightMode;

                this.BackColor = dayColor;
                foreach (Control c in this.Controls)
                {
                    //if (c is Label || c is Button)
                    {
                        c.ForeColor = Color.Black;
                    }
                }
                LineUpManualBtns();
            }
            else //nightmode
            {
                btnDayNightMode.Image = Properties.Resources.WindowDayMode;

                this.BackColor = nightColor;

                foreach (Control c in this.Controls)
                {
                    {
                        c.ForeColor = Color.White;
                    }
                }
                LineUpManualBtns();
            }

            Properties.Settings.Default.setDisplay_isDayMode = isDay;
            Properties.Settings.Default.Save();
        }

        private void FixPanelsAndMenus()
        {
            oglMain.Left = statusStripLeft.Width;
            if (Settings.Default.setDisplay_isSimple)
            {
                oglMain.Width = Width - 17 - statusStripLeft.Width - layoutPanelRight.Width / 2;
            }
            else
            {
                oglMain.Width = Width - 16 - statusStripLeft.Width - layoutPanelRight.Width;
            }

            if (isFullScreen) oglMain.Width += 16;

            if (Settings.Default.setDisplay_isSimple)
            {
                toolToolbottomStripBtn.Visible = false;
                vehicleToolStripBtn.Visible = false;
                AutoSteerToolBtn.Visible = false;

                lblDateTime.Visible = true;
                snapLeftBigStrip.Visible = true;
                snapRightBigStrip.Visible = true;
            }
            else
            {
                toolToolbottomStripBtn.Visible = true;
                vehicleToolStripBtn.Visible = true;
                AutoSteerToolBtn.Visible = true;

                if (Width > 1100)
                {
                    snapLeftBigStrip.Visible = true;
                    snapRightBigStrip.Visible = true;
                }
                else
                {
                    snapLeftBigStrip.Visible = false;
                    snapRightBigStrip.Visible = false;
                }
                if (Width > 1300)
                {
                    lblDateTime.Visible = true;
                }
                else
                {
                    lblDateTime.Visible = false;
                }
            }
        }

        public string FindDirection(double heading)
        {
            if (heading < 0) heading += Glm.twoPI;

            heading = Glm.ToDegrees(heading);

            if (heading > 337.5 || heading < 22.5)
            {
                return (" " +  gStr.gsNorth + " ");
            }
            if (heading > 22.5 && heading < 67.5)
            {
                return (" " +  gStr.gsN_East + " ");
            }
            if (heading > 67.5 && heading < 111.5)
            {
                return (" " +  gStr.gsEast + " ");
            }
            if (heading > 111.5 && heading < 157.5)
            {
                return (" " +  gStr.gsS_East + " ");
            }
            if (heading > 157.5 && heading < 202.5)
            {
                return (" " +  gStr.gsSouth + " ");
            }
            if (heading > 202.5 && heading < 247.5)
            {
                return (" " +  gStr.gsS_West + " ");
            }
            if (heading > 247.5 && heading < 292.5)
            {
                return (" " +  gStr.gsWest + " ");
            }
            if (heading > 292.5 && heading < 337.5)
            {
                return (" " +  gStr.gsN_West + " ");
            }
            return (" " +  gStr.gsLost + " ");
        }

        //line up section On Off Auto buttons based on how many there are
        public void LineUpManualBtns()
        {
            int oglCenter = statusStripLeft.Width + oglMain.Width / 2;

            int top = 180;
            if (panelSim.Visible == true) top = 230;

            panelSim.Width = Math.Min(panelSim.MaximumSize.Width, oglMain.Width - 10);
            panelSim.Left = oglCenter - panelSim.Width / 2;

            Size Size = new System.Drawing.Size(Math.Min((oglMain.Width * 3 / 4) / tool.numOfSections, 120), 30);

            for (int i = 0; i < MAXSECTIONS; i++)
            {
                if (i < tool.numOfSections)
                {
                    section[i].SectionButton.Top = Height - top;
                    section[i].SectionButton.Size = Size;
                    section[i].SectionButton.Left = (oglCenter) - (tool.numOfSections * Size.Width) / 2 + Size.Width * i;
                    section[i].SectionButton.Visible = true;
                }
                else section[i].SectionButton.Visible = false;
            }
        }

        //update individual btn based on state after push
        private void ManualBtnUpdate(int sectNumber)
        {
            if (section[sectNumber].BtnSectionState == btnStates.On)
                section[sectNumber].SectionButton.BackColor = isDay ? Color.Yellow : Color.DarkGoldenrod;
            else if (section[sectNumber].BtnSectionState == btnStates.Auto)
                section[sectNumber].SectionButton.BackColor = isDay ? Color.Lime : Color.ForestGreen;
            else
                section[sectNumber].SectionButton.BackColor = isDay ? Color.Red : Color.Crimson;

            section[sectNumber].SectionButton.ForeColor = isDay ? Color.Black : Color.White;
        }

        //Mouse Clicks 
        private void oglMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                //0 at bottom for opengl, 0 at top for windows, so invert Y value
                Point point = oglMain.PointToClient(Cursor.Position);

                //label3.Text = point.X.ToString();

                if (point.Y < 140 && point.Y > 40)
                {
                    int middle = oglMain.Width / 2 + oglMain.Width / 5;
                    if (point.X > middle - 80 && point.X < middle + 80)
                    {
                        SwapDirection();
                        return;
                    }

                    middle = oglMain.Width / 2 - oglMain.Width / 4;
                    if (point.X > middle - 140 && point.X < middle)
                    {
                        if (yt.isYouTurnTriggered)
                        {
                            yt.ResetYouTurn();
                        }
                        else
                        {
                            if (yt.isYouTurnBtnOn) btnAutoYouTurn.PerformClick();
                            yt.isYouTurnTriggered = true;
                            yt.BuildManualYouTurn(false, true);
                            return;
                        }
                    }

                    if (point.X > middle && point.X < middle + 140)
                    {
                        if (yt.isYouTurnTriggered)
                        {
                            yt.ResetYouTurn();
                        }
                        else
                        {
                            if (yt.isYouTurnBtnOn) btnAutoYouTurn.PerformClick();
                            yt.isYouTurnTriggered = true;
                            yt.BuildManualYouTurn(true, true);
                            return;
                        }
                    }
                }

                //prevent flag selection if flag form is up
                Form fc = Application.OpenForms["FormFlags"];
                if (fc != null)
                {
                    fc.Focus();
                    return;
                }

                mouseX = point.X;
                mouseY = oglMain.Height - point.Y;
                LeftMouseDownOnOpenGL = true;
            }
        }
        private void oglZoom_MouseClick(object sender, MouseEventArgs e)
        {
            if ((sender as Control).IsDragging()) return;

            if (oglZoom.Width == 180)
            {
                oglZoom.Width = 300;
                oglZoom.Height = 300;
            }

            else if (oglZoom.Width == 300)
            {
                oglZoom.Width = 180;
                oglZoom.Height = 180;
            }
        }               

        //Function to delete flag
        public void DeleteSelectedFlag()
        {
            //delete selected flag and set selected to none
            flagPts.RemoveAt(flagNumberPicked - 1);
            flagNumberPicked = 0;

            // re-sort the id's based on how many flags left
            int flagCnt = flagPts.Count;
            if (flagCnt > 0)
            {
                for (int i = 0; i < flagCnt; i++) flagPts[i].ID = i + 1;
            }
        }
        public void EnableYouTurnButtons()
        {
            yt.ResetYouTurn();

            yt.isYouTurnBtnOn = false;
            btnAutoYouTurn.Enabled = true;
            btnAutoYouTurn.Image = Properties.Resources.YouTurnNo;
        }
        public void DisableYouTurnButtons()
        {

            btnAutoYouTurn.Enabled = false;
            yt.isYouTurnBtnOn = false;
            btnAutoYouTurn.Image = Properties.Resources.YouTurnNo;
            yt.ResetYouTurn();
        }

        private void ShowNoGPSWarning()
        {
            //update main window
            oglMain.MakeCurrent();
            oglMain.Refresh();
        }

        private void HalfSecond_Update(object sender, EventArgs e)
        {
            HalfSecondUpdate.Enabled = false;

            AutoSteerToolBtn.Text = SetSteerAngle + "\r\n" + ActualSteerAngle;

            AutoSteerToolBtn.Text = SetSteerAngle + "\r\n" + ActualSteerAngle;
            //the main formgps window
            if (isMetric)  //metric or imperial
            {
                lblSpeed.Text = SpeedKPH;
                btnContour.Text = XTE; //cross track error

            }
            else  //Imperial Measurements
            {
                lblSpeed.Text = SpeedMPH;
                btnContour.Text = InchXTE; //cross track error
            }

            lblHz.Text = NMEAHz + "Hz " + (int)(FrameTime) + "\r\n" +
                FixQuality + Math.Round(HzTime, 1, MidpointRounding.AwayFromZero) + " Hz";

            HalfSecondUpdate.Enabled = true;
        }

        private void OneSecond_Update(object sender, EventArgs e)
        {
            OneSecondUpdate.Enabled = false;

            //counter used for saving field in background
            MinuteCounter++;

            if (ABLine.isBtnABLineOn && !ct.isContourBtnOn)
            {
                btnEditHeadingB.Text = ((int)(ABLine.moveDistance * 100)).ToString();
            }
            if (curve.isBtnCurveOn && !ct.isContourBtnOn)
            {
                btnEditHeadingB.Text = ((int)(curve.moveDistance * 100)).ToString();
            }

            //Make sure it is off when it should
            if ((!ABLine.isBtnABLineOn && !ct.isContourBtnOn && !curve.isBtnCurveOn && isAutoSteerBtnOn)
                || (recPath.isDrivingRecordedPath && isAutoSteerBtnOn)) btnAutoSteer.PerformClick();

            //do all the NTRIP routines
            if (isNTRIP_TurnedOn)
            {
                //increment once every second
                NtripCounter++;

                //Thinks is connected but not receiving anything // 30sec maybe a bit much?
                if (NTRIP_Watchdog++ > 10 && isNTRIP_Connected) ReconnectRequest();

                //Have we connection
                if (!isNTRIP_Connected && !isNTRIP_Connecting)
                {
                    if (NtripCounter > 20) StartNTRIP();
                }

                if (isNTRIP_Connecting)
                {
                    if (NtripCounter > 25)//give it 5 seconds
                    {
                        TimedMessageBox(2000, gStr.gsSocketConnectionProblem, gStr.gsNotConnectingToCaster);
                        ReconnectRequest();
                    }
                    if (clientSocket != null && clientSocket.Connected)
                    {
                        SendAuthorization();
                    }
                }

                //update byte counter and up counter
                if (NtripCounter > 20) NTRIPStartStopStrip.Text = string.Format("{0:00}:{1:00}", ((NtripCounter-21) / 60), (Math.Abs(NtripCounter-21)) % 60);
                else NTRIPStartStopStrip.Text = gStr.gsConnectingIn + " " + (Math.Abs(NtripCounter - 21));

                pbarNtripMenu.Value = unchecked((byte)(tripBytes * 0.02));
                NTRIPBytesMenu.Text = ((tripBytes) * 0.001).ToString("###,###,###") + " kb";

                //watchdog for Ntrip
                if (isNTRIP_Connecting) lblWatch.Text = gStr.gsAuthourizing;
                else
                {
                    if (NTRIP_Watchdog > 10) lblWatch.Text = gStr.gsWaiting;
                    else lblWatch.Text = gStr.gsListening;
                }

                if (sendGGAInterval > 0 && isNTRIP_Sending)
                {
                    lblWatch.Text = gStr.gsSendingGGA;
                    isNTRIP_Sending = false;
                }
            }

            //the main formgps window
            //status strip values
            if (isMetric)
            {
                distanceToolBtn.Text = fd.DistanceUserMeters + "\r\n" + fd.WorkedUserHectares2;
            }
            else
            {
                distanceToolBtn.Text = fd.DistanceUserFeet + "\r\n" + fd.WorkedUserAcres2;
            }

            //statusbar flash red undefined headland
            if (mc.isOutOfBounds && statusStripBottom.BackColor == Color.Transparent) statusStripBottom.BackColor = Color.Tomato;
            else if (!mc.isOutOfBounds && statusStripBottom.BackColor == Color.Tomato) statusStripBottom.BackColor = Color.Transparent;

                    //lblEast.Text = ((int)(pn.actualEasting)).ToString();
                    //lblNorth.Text = ((int)(pn.actualNorthing)).ToString();

            OneSecondUpdate.Enabled = true;
        }

        private void ThreeSecond_Update(object sender, EventArgs e)
        {
            ThreeSecondUpdate.Enabled = false;

            zoomUpdateCounter = true;
            //check to make sure the grid is big enough
            worldGrid.CheckZoomWorldGrid(pn.fix.northing, pn.fix.easting);

            if (isMetric)
            {

                fieldStatusStripText.Text = fd.WorkedAreaRemainHectares + "\r\n" +
                                               fd.WorkedAreaRemainPercentage + "\r\n" +
                                               fd.TimeTillFinished;
            }
            else //imperial
            {

                fieldStatusStripText.Text = fd.WorkedAreaRemainAcres + "\r\n" +
                       fd.WorkedAreaRemainPercentage + "\r\n" +
                       fd.TimeTillFinished;
            }

            //not Metric/Standard units sensitive
            if (ABLine.isBtnABLineOn) btnABLine.Text = "# " + PassNumber;
            else btnABLine.Text = "";

            if (curve.isBtnCurveOn) btnCurve.Text = "# " + CurveNumber;
            else btnCurve.Text = "";

            lblDateTime.Text = DateTime.Now.ToString("HH:mm:ss") + "\n\r" + DateTime.Now.ToString("ddd MMM yyyy");


            ThreeSecondUpdate.Enabled = true;
        }


        #region Properties // ---------------------------------------------------------------------

        public string Zone { get { return Convert.ToString(pn.zone); } }
        public string FixNorthing { get { return Convert.ToString(Math.Round(pn.fix.northing + pn.utmNorth, 2)); } }
        public string FixEasting { get { return Convert.ToString(Math.Round(pn.fix.easting + pn.utmEast, 2)); } }
        public string Latitude { get { return Convert.ToString(Math.Round(pn.latitude, 7)); } }
        public string Longitude { get { return Convert.ToString(Math.Round(pn.longitude, 7)); } }

        public string SatsTracked { get { return Convert.ToString(pn.satellitesTracked); } }
        public string HDOP { get { return Convert.ToString(pn.hdop); } }
        public string NMEAHz { get { return Convert.ToString(fixUpdateHz); } }
        public string PassNumber { get { return Convert.ToString(ABLine.passNumber); } }
        public string CurveNumber { get { return Convert.ToString(curve.curveNumber); } }
        public string Heading { get { return Convert.ToString(Math.Round(Glm.ToDegrees(fixHeading), 1)) + "\u00B0"; } }
        public string GPSHeading { get { return (Math.Round(Glm.ToDegrees(gpsHeading), 1)) + "\u00B0"; } }
        public string Status { get { if (pn.status == "A") return "Active"; else return "Void"; } }
        public string FixQuality
        {
            get
            {
                //if (timerSim.Enabled)
                //    return "Sim: ";

                if (pn.FixQuality == 0) return "Invalid ";
                else if (pn.FixQuality == 1) return "GPS Single ";
                else if (pn.FixQuality == 2) return "DGPS ";
                else if (pn.FixQuality == 3) return "PPS ";
                else if (pn.FixQuality == 4) return "RTK Fix ";
                else if (pn.FixQuality == 5) return "Float ";
                else if (pn.FixQuality == 6) return "Estimate ";
                else if (pn.FixQuality == 7) return "Man IP ";
                else if (pn.FixQuality == 8) return "Sim ";
                else return "Unknown: ";
            }
        }

        public string GyroInDegrees
        {
            get
            {
                if (ahrs.correctionHeadingX16 != 9999)
                    return Math.Round(ahrs.correctionHeadingX16 * 0.0625, 1) + "\u00B0";
                else return "-";
            }
        }
        public string RollInDegrees
        {
            get
            {
                if (ahrs.isRollFromAutoSteer || ahrs.isRollFromAVR)
                    return Math.Round((ahrs.rollX16 - ahrs.rollZeroX16) * 0.0625, 1) + "\u00B0";
                else return "-";
            }
        }
        public string SetSteerAngle { get { return ((double)(guidanceLineSteerAngle) * 0.01).ToString("N2") + "\u00B0"; } }
        public string ActualSteerAngle { get { return ((double)(actualSteerAngleDisp) * 0.01).ToString("N2") + "\u00B0"; } }

        public string FixHeading { get { return Math.Round(fixHeading, 4).ToString(); } }
        
        //public string LookAhead { get { return ((int)(section[0].lookAheadOn)).ToString(); } }
        public string StepFixNum { get { return (currentStepFix).ToString(); } }
        public string CurrentStepDistance { get { return Math.Round(distanceCurrentStepFix, 3).ToString(); } }
        public string TotalStepDistance { get { return Math.Round(fixStepDist, 3).ToString(); } }

        public string AgeDiff { get { return pn.ageDiff.ToString(); } }

        //Metric and Imperial Properties
        public string SpeedMPH
        {
            get
            {
                return Convert.ToString(Math.Round(avgSpeed*0.62137, 1));
            }
        }
        public string SpeedKPH
        {
            get
            {
                return Convert.ToString(Math.Round(avgSpeed, 1));
            }
        }

        public string XTE
        {
            get
            {
                //double spd = 0;
                //for (int c = 0; c < 20; c++) spd += avgXTE[c];
                //spd *= 0.1;
                //return ((int)(spd * 0.05) + " cm");
                return (crossTrackError/10 + gStr.gsCM);
            }
        }
        public string InchXTE
        {
            get
            {
                //double spd = 0;
                //for (int c = 0; c < 20; c++) spd += avgXTE[c];
                //spd *= 0.1;
                //return ((int)(spd * 0.019685) + " in");
                return ((int)(crossTrackError/25.54) + " in");
            }
        }

        public string FixOffset { get { return (pn.fixOffset.easting.ToString("N2") + ", " + pn.fixOffset.northing.ToString("N2")); } }
        public string FixOffsetInch { get { return ((pn.fixOffset.easting*Glm.m2in).ToString("N0")+ ", " + (pn.fixOffset.northing*Glm.m2in).ToString("N0")); } }

        public string Altitude { get { return Convert.ToString(Math.Round(pn.altitude,1)); } }
        public string AltitudeFeet { get { return Convert.ToString((Math.Round((pn.altitude * 3.28084),1))); } }
        public string DistPivotM
        {
            get
            {
                if (distancePivotToTurnLine > 0 )
                    return ((int)(distancePivotToTurnLine)) + " m";
                else return "--";
            }
        }
        public string DistPivotFt
        {
            get
            {
                if (distancePivotToTurnLine > 0 ) return (((int)(Glm.m2ft * (distancePivotToTurnLine))) + " ft");
                else return "--";
            }
        }

        #endregion properties 
    }//end class
}//end namespace