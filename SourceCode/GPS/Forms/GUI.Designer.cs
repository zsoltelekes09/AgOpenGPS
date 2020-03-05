using System;
using System.Drawing;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using System.Globalization;

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
        private bool isDrawPolygons;

        //Is it in 2D or 3D, metric or imperial, display lightbar, display grid etc
        public bool isIn3D = true, isMetric = true, isLightbarOn = true, isGridOn, isFullScreen;
        public bool isUTurnAlwaysOn, isCompassOn, isSpeedoOn, isAutoDayNight, isSideGuideLines = true;
        public bool isPureDisplayOn = true, isSkyOn = true, isRollMeterOn = false;
        public bool isDay = true, isDayTime = true;

        //master Manual and Auto, 3 states possible
        public enum btnStates { Off, Auto, On }
        public btnStates manualBtnState = btnStates.Off;
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

        //section button states
        public enum manBtn { Off, Auto, On }

        private void IsBetweenSunriseSunset(double lat, double lon)
        {
            CSunTimes.Instance.CalculateSunRiseSetTimes(pn.latitude, pn.longitude, dateToday, ref sunrise, ref sunset);
            isDay = (DateTime.Now.Ticks < sunset.Ticks && DateTime.Now.Ticks > sunrise.Ticks);
        }

        private void LoadGUI()
        {
            //set the language to last used
            SetLanguage(Settings.Default.setF_culture);
            
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
            metricToolStrip.Checked = isMetric;

            if (isMetric)
            {
                //lblSpeedUnits.Text = gStr.gsKMH;
                metricToolStrip.Checked = true;
                imperialToolStrip.Checked = false;
                //lblFlowLeft.Text = "LPM"; lblFlowRight.Text = "LPM";
            }
            else
            {
                //lblSpeedUnits.Text = gStr.gsMPH;
                metricToolStrip.Checked = false;
                imperialToolStrip.Checked = true;
                //lblFlowLeft.Text = "GPM"; lblFlowRight.Text = "GPM";

            }

            //load up colors
            fieldColorDay = (Settings.Default.setDisplay_colorFieldDay);
            sectionColorDay = (Settings.Default.setDisplay_colorSectionsDay);
            fieldColorNight = (Settings.Default.setDisplay_colorFieldNight);
            sectionColorNight = (Settings.Default.setDisplay_colorSectionsNight);

            DisableYouTurnButtons();

            //set up grid and lightbar
            isGridOn = Settings.Default.setMenu_isGridOn;
            gridOnToolStripMenuItem.Checked = isGridOn;

            //log NMEA 
            isLogNMEA = Settings.Default.setMenu_isLogNMEA;
            logNMEAToolStripMenuItem.Checked = isLogNMEA;

            isLightbarOn = Settings.Default.setMenu_isLightbarOn;
            lightbarToolStripMenuItem.Checked = isLightbarOn;

            isSideGuideLines = Settings.Default.setMenu_isSideGuideLines;
            extraGuidesToolStripMenuItem.Checked = isSideGuideLines;

            isPureDisplayOn = Settings.Default.setMenu_isPureOn;
            pursuitOnToolStripMenuItem.Checked = isPureDisplayOn;

            isSkyOn = Settings.Default.setMenu_isSkyOn;
            skyOnToolStripMenuItem.Checked = isSkyOn;

            isCompassOn = Settings.Default.setMenu_isCompassOn;
            compassOnToolStripMenuItem.Checked = isCompassOn;

            isSpeedoOn = Settings.Default.setMenu_isSpeedoOn;
            speedoOnToolStripMenuItem.Checked = isSpeedoOn;

            isAutoDayNight = Settings.Default.setDisplay_isAutoDayNight;
            autoDayNightModeToolStripMenuItem.Checked = isAutoDayNight;

            isUTurnAlwaysOn = Settings.Default.setMenu_isUTurnAlwaysOn;
            uTurnAlwaysOnToolStripMenuItem.Checked = isUTurnAlwaysOn;

            if (Settings.Default.setMenu_isOGLZoomOn == 1)
                topFieldViewToolStripMenuItem.Checked = true;
            else topFieldViewToolStripMenuItem.Checked = false;

            oglZoom.Width = 400;
            oglZoom.Height = 400;
            oglZoom.Visible = true;
            oglZoom.Left = 300;
            oglZoom.Top = 80;

            oglZoom.SendToBack();

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

            LineUpManualBtns();

            yt.rowSkipsWidth = Properties.Vehicle.Default.set_youSkipWidth;
            cboxpRowWidth.SelectedIndex = yt.rowSkipsWidth - 1;

            //default to come up in mini panel, exit remembers 

            SwapBatmanPanels();

            if (Properties.Settings.Default.setAS_isAutoSteerAutoOn) btnAutoSteer.Text = "A";
            else btnAutoSteer.Text = "M";

            //panelSim.Location = Settings.Default.setDisplay_panelSimLocation;

            FixPanelsAndMenus();

            layoutPanelRight.Enabled = false;
            //boundaryToolStripBtn.Enabled = false;
            toolStripBtnDropDownBoundaryTools.Enabled = false;

            if (!isNTRIP_TurnedOn)
            {
                NTRIPBytesMenu.Visible = false;
                pbarNtripMenu.Visible = false;
            }

            if (Properties.Settings.Default.setUDP_isOn)
            {
                pbarUDPComm.Visible = true;
                toolStripStatusLabel2.Visible = true;
            }
            else
            {
                pbarUDPComm.Visible = false;
                toolStripStatusLabel2.Visible = false;
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
                startFullScreenToolStripMenuItem.Checked = true;
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                btnFullScreen.BackgroundImage = Properties.Resources.WindowNormal;
                isFullScreen = true;
            }
            else
            {
                startFullScreenToolStripMenuItem.Checked = false;
                isFullScreen = false;
            }

            //is rtk on?
            isRTK = Properties.Settings.Default.setGPS_isRTK;


        }

        private void SwapDayNightMode()
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
            if (heading < 0) heading += glm.twoPI;

            heading = glm.toDegrees(heading);

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

        //hide the left panel
        public void SwapBatmanPanels()
        {
            //Properties.Settings.Default.Save();
            if (Properties.Settings.Default.setDisplay_isBatmanOn)
            {
                oglMain.Left = statusStripLeft.Width + panelBatman.Width;

                if (Settings.Default.setDisplay_isSimple)
                {
                    oglMain.Width = Width - 17 - statusStripLeft.Width - panelBatman.Width - layoutPanelRight.Width / 2;
                }
                else
                {
                    oglMain.Width = Width-16 - statusStripLeft.Width - panelBatman.Width - layoutPanelRight.Width;
                }

                if (isFullScreen) oglMain.Width += 16;

                panelBatman.Left = statusStripLeft.Width;
                panelBatman.Visible = true;


                if (panelDrag.Visible) panelDrag.Left = statusStripLeft.Width + panelBatman.Width + 5;

                LineUpManualBtns();
            }
            else
            {
                //no side panel
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

                panelBatman.Visible = false;
                


                if (panelDrag.Visible) panelDrag.Left = statusStripLeft.Width + 10;

                LineUpManualBtns();
            }
        }

        //line up section On Off Auto buttons based on how many there are
        public void LineUpManualBtns()
        {
            int oglCenter = (panelBatman.Visible ? panelBatman.Width : 0) + statusStripLeft.Width + oglMain.Width/2;

            int top = 180;
            if (panelSim.Visible == true) top = 230;

            panelSim.Width = Math.Min(panelSim.MaximumSize.Width, oglMain.Width - 10);
            panelSim.Left = oglCenter - panelSim.Width / 2;

            btnSection1Man.Top  = Height - top;
            btnSection2Man.Top  = Height - top;
            btnSection3Man.Top  = Height - top;
            btnSection4Man.Top  = Height - top;
            btnSection5Man.Top  = Height - top;
            btnSection6Man.Top  = Height - top;
            btnSection7Man.Top  = Height - top;
            btnSection8Man.Top  = Height - top;
            btnSection9Man.Top  = Height - top;
            btnSection10Man.Top = Height - top;
            btnSection11Man.Top = Height - top;
            btnSection12Man.Top = Height - top;
            btnSection13Man.Top = Height - top;
            btnSection14Man.Top = Height - top;
            btnSection15Man.Top = Height - top;
            btnSection16Man.Top = Height - top;

            int oglButtonWidth = oglMain.Width * 3/4;
            //if (tool.numOfSections < 9 )  oglButtonWidth = oglMain.Width * 5/6;

            int buttonMaxWidth = 120, buttonHeight = 30;

            int buttonWidth = oglButtonWidth / tool.numOfSections;
            if (buttonWidth > buttonMaxWidth) buttonWidth = buttonMaxWidth;
            btnSection1Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection2Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection3Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection4Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection5Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection6Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection7Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection8Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection9Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection10Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection11Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection12Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection13Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection14Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection15Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);
            btnSection16Man.Size = new System.Drawing.Size(buttonWidth, buttonHeight);

            switch (tool.numOfSections)
            {
                case 1:
                    btnSection1Man.Left = (oglCenter) - btnSection1Man.Size.Width/2;
                    break;

                case 2:                    
                    btnSection1Man.Left = (oglCenter) - btnSection1Man.Size.Width;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    break;

                case 3:
                    btnSection1Man.Left = (oglCenter) - btnSection1Man.Size.Width/2 - btnSection1Man.Size.Width;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    break;

                case 4:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;

                    break;

                case 5:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    break;

                case 6:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width)/2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    break;

                case 7:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    break;

                case 8:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    break;

                case 9:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                    break;

                case 10:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                    btnSection10Man.Left = btnSection9Man.Left + btnSection1Man.Size.Width;
                    break;

                case 11:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                    btnSection10Man.Left = btnSection9Man.Left + btnSection1Man.Size.Width;
                    btnSection11Man.Left = btnSection10Man.Left + btnSection1Man.Size.Width;
                    break;

                case 12:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                    btnSection10Man.Left = btnSection9Man.Left + btnSection1Man.Size.Width;
                    btnSection11Man.Left = btnSection10Man.Left + btnSection1Man.Size.Width;
                    btnSection12Man.Left = btnSection11Man.Left + btnSection1Man.Size.Width;
                    break;
                case 13:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                    btnSection10Man.Left = btnSection9Man.Left + btnSection1Man.Size.Width;
                    btnSection11Man.Left = btnSection10Man.Left + btnSection1Man.Size.Width;
                    btnSection12Man.Left = btnSection11Man.Left + btnSection1Man.Size.Width;
                    btnSection13Man.Left = btnSection12Man.Left + btnSection1Man.Size.Width;
                    break;
                case 14:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                    btnSection10Man.Left = btnSection9Man.Left + btnSection1Man.Size.Width;
                    btnSection11Man.Left = btnSection10Man.Left + btnSection1Man.Size.Width;
                    btnSection12Man.Left = btnSection11Man.Left + btnSection1Man.Size.Width;
                    btnSection13Man.Left = btnSection12Man.Left + btnSection1Man.Size.Width;
                    btnSection14Man.Left = btnSection13Man.Left + btnSection1Man.Size.Width;
                    break;
                case 15:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                    btnSection10Man.Left = btnSection9Man.Left + btnSection1Man.Size.Width;
                    btnSection11Man.Left = btnSection10Man.Left + btnSection1Man.Size.Width;
                    btnSection12Man.Left = btnSection11Man.Left + btnSection1Man.Size.Width;
                    btnSection13Man.Left = btnSection12Man.Left + btnSection1Man.Size.Width;
                    btnSection14Man.Left = btnSection13Man.Left + btnSection1Man.Size.Width;
                    btnSection15Man.Left = btnSection14Man.Left + btnSection1Man.Size.Width;
                    break;
                case 16:
                    btnSection1Man.Left = (oglCenter) - (tool.numOfSections * btnSection1Man.Size.Width) / 2;
                    btnSection2Man.Left = btnSection1Man.Left + btnSection1Man.Size.Width;
                    btnSection3Man.Left = btnSection2Man.Left + btnSection1Man.Size.Width;
                    btnSection4Man.Left = btnSection3Man.Left + btnSection1Man.Size.Width;
                    btnSection5Man.Left = btnSection4Man.Left + btnSection1Man.Size.Width;
                    btnSection6Man.Left = btnSection5Man.Left + btnSection1Man.Size.Width;
                    btnSection7Man.Left = btnSection6Man.Left + btnSection1Man.Size.Width;
                    btnSection8Man.Left = btnSection7Man.Left + btnSection1Man.Size.Width;
                    btnSection9Man.Left = btnSection8Man.Left + btnSection1Man.Size.Width;
                    btnSection10Man.Left = btnSection9Man.Left + btnSection1Man.Size.Width;
                    btnSection11Man.Left = btnSection10Man.Left + btnSection1Man.Size.Width;
                    btnSection12Man.Left = btnSection11Man.Left + btnSection1Man.Size.Width;
                    btnSection13Man.Left = btnSection12Man.Left + btnSection1Man.Size.Width;
                    btnSection14Man.Left = btnSection13Man.Left + btnSection1Man.Size.Width;
                    btnSection15Man.Left = btnSection14Man.Left + btnSection1Man.Size.Width;
                    btnSection16Man.Left = btnSection15Man.Left + btnSection1Man.Size.Width;
                    break;
            }

            //if (isJobStarted)
            {
                switch (tool.numOfSections)
                {
                    case 1:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = false;
                        btnSection3Man.Visible = false;
                        btnSection4Man.Visible = false;
                        btnSection5Man.Visible = false;
                        btnSection6Man.Visible = false;
                        btnSection7Man.Visible = false;
                        btnSection8Man.Visible = false;
                        btnSection9Man.Visible = false;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 2:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = false;
                        btnSection4Man.Visible = false;
                        btnSection5Man.Visible = false;
                        btnSection6Man.Visible = false;
                        btnSection7Man.Visible = false;
                        btnSection8Man.Visible = false;
                        btnSection9Man.Visible = false;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 3:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = false;
                        btnSection5Man.Visible = false;
                        btnSection6Man.Visible = false;
                        btnSection7Man.Visible = false;
                        btnSection8Man.Visible = false;
                        btnSection9Man.Visible = false;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 4:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = false;
                        btnSection6Man.Visible = false;
                        btnSection7Man.Visible = false;
                        btnSection8Man.Visible = false;
                        btnSection9Man.Visible = false;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 5:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = false;
                        btnSection7Man.Visible = false;
                        btnSection8Man.Visible = false;
                        btnSection9Man.Visible = false;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 6:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = false;
                        btnSection8Man.Visible = false;
                        btnSection9Man.Visible = false;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 7:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = false;
                        btnSection9Man.Visible = false;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 8:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = false;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 9:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = true;
                         btnSection10Man.Visible = false;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 10:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = true;
                         btnSection10Man.Visible = true;
                         btnSection11Man.Visible = false;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 11:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = true;
                         btnSection10Man.Visible = true;
                         btnSection11Man.Visible = true;
                         btnSection12Man.Visible = false;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 12:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = true;
                         btnSection10Man.Visible = true;
                         btnSection11Man.Visible = true;
                         btnSection12Man.Visible = true;
                         btnSection13Man.Visible = false;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 13:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = true;
                         btnSection10Man.Visible = true;
                         btnSection11Man.Visible = true;
                         btnSection12Man.Visible = true;
                         btnSection13Man.Visible = true;
                         btnSection14Man.Visible = false;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 14:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = true;
                         btnSection10Man.Visible = true;
                         btnSection11Man.Visible = true;
                         btnSection12Man.Visible = true;
                         btnSection13Man.Visible = true;
                         btnSection14Man.Visible = true;
                         btnSection15Man.Visible = false;
                         btnSection16Man.Visible = false;
                        break;

                    case 15:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = true;
                         btnSection10Man.Visible = true;
                         btnSection11Man.Visible = true;
                         btnSection12Man.Visible = true;
                         btnSection13Man.Visible = true;
                         btnSection14Man.Visible = true;
                         btnSection15Man.Visible = true;
                         btnSection16Man.Visible = false;
                        break;

                    case 16:
                        btnSection1Man.Visible = true;
                        btnSection2Man.Visible = true;
                        btnSection3Man.Visible = true;
                        btnSection4Man.Visible = true;
                        btnSection5Man.Visible = true;
                        btnSection6Man.Visible = true;
                        btnSection7Man.Visible = true;
                        btnSection8Man.Visible = true;
                        btnSection9Man.Visible = true;
                         btnSection10Man.Visible = true;
                         btnSection11Man.Visible = true;
                         btnSection12Man.Visible = true;
                         btnSection13Man.Visible = true;
                         btnSection14Man.Visible = true;
                         btnSection15Man.Visible = true;
                         btnSection16Man.Visible = true;
                        break;
                }
            }
        }

        //force all the buttons same according to two main buttons
        private void ManualAllBtnsUpdate()
        {
            ManualBtnUpdate(0, btnSection1Man);
            ManualBtnUpdate(1, btnSection2Man);
            ManualBtnUpdate(2, btnSection3Man);
            ManualBtnUpdate(3, btnSection4Man);
            ManualBtnUpdate(4, btnSection5Man);
            ManualBtnUpdate(5, btnSection6Man);
            ManualBtnUpdate(6, btnSection7Man);
            ManualBtnUpdate(7, btnSection8Man);
            ManualBtnUpdate(8, btnSection9Man);
            ManualBtnUpdate(9, btnSection10Man);
            ManualBtnUpdate(10, btnSection11Man);
            ManualBtnUpdate(11, btnSection12Man);
            ManualBtnUpdate(12, btnSection13Man);
            ManualBtnUpdate(13, btnSection14Man);
            ManualBtnUpdate(14, btnSection15Man);
            ManualBtnUpdate(15, btnSection16Man);

        }
        //update individual btn based on state after push

        private void ManualBtnUpdate(int sectNumber, Button btn)
        {
            switch (section[sectNumber].manBtnState)
            {
                case manBtn.Off:
                    section[sectNumber].manBtnState = manBtn.Auto;
                    if (isDay)
                    {
                        btn.BackColor = Color.Lime;
                        btn.ForeColor = Color.Black;
                    }
                    else
                    {
                        btn.BackColor = Color.ForestGreen;
                        btn.ForeColor = Color.White;
                    }
                    break;
            

                case manBtn.Auto:
                    section[sectNumber].manBtnState = manBtn.On;
                    if (isDay)
                    {
                        btn.BackColor = Color.Yellow;
                        btn.ForeColor = Color.Black;
                    }
                    else
                    {
                        btn.BackColor = Color.DarkGoldenrod;
                        btn.ForeColor = Color.White;
                    }
                    break;

                case manBtn.On:
                    section[sectNumber].manBtnState = manBtn.Off;
                    if (isDay)
                    {
                        btn.ForeColor = Color.Black;
                        btn.BackColor = Color.Red;
                    }
                    else
                    {
                        btn.BackColor = Color.Crimson;
                        btn.ForeColor = Color.White;
                    }
                    break;
            }
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
            testHalfSecond.Restart();
            testHalfSecond.Start();


            if (guidanceLineDistanceOff == 32020 | guidanceLineDistanceOff == 32000)
            {
                AutoSteerToolBtn.Text = "Off \r\n" + ActualSteerAngle;
            }
            else
            {
                AutoSteerToolBtn.Text = SetSteerAngle + "\r\n" + ActualSteerAngle;
            }

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

            lblTrigger.Text = sectionTriggerStepDistance.ToString("N2");

            testHalfSecond1 = testHalfSecond.ElapsedMilliseconds;
            lblHz.Text = NMEAHz + ".0 Hz\r\n" + FixQuality + HzTime.ToString("N1") + " Hz";
            lblHz2.Text = (int)(FrameTime) + "\r\n" + testHalfSecond1.ToString() + " " + testOneSecond1.ToString() + " " + testThreeSecond1.ToString() + " " + testNMEA1.ToString();

            HalfSecondUpdate.Enabled = true;
        }

        private void OneSecond_Update(object sender, EventArgs e)
        {
            OneSecondUpdate.Enabled = false;
            testOneSecond.Restart();
            testOneSecond.Start();

            //counter used for saving field in background
            MinuteCounter++;
            TenMinuteCounter++;

            if (isRTK)
            {
                if (pn.fixQuality == 4 || pn.fixQuality == 5)
                {
                    lblHz.BackColor = Color.Transparent;
                    lblHz2.BackColor = Color.Transparent;
                }
                else
                {
                    lblHz.BackColor = Color.Salmon;
                    lblHz2.BackColor = Color.Salmon;
                }
            }
            else
            {
                lblHz.BackColor = Color.Transparent;
                lblHz2.BackColor = Color.Transparent;
            }

            if (panelBatman.Visible)
            {
                //both
                lblLatitude.Text = Latitude;
                lblLongitude.Text = Longitude;


                lblRoll.Text = RollInDegrees;
                lblYawHeading.Text = GyroInDegrees;
                lblGPSHeading.Text = GPSHeading;

                //up in the menu a few pieces of info
                if (isJobStarted)
                {
                    lblEasting.Text = "E:" + (pn.fix.easting).ToString("N1");
                    lblNorthing.Text = "N:" + (pn.fix.northing).ToString("N1");
                }
                else
                {
                    lblEasting.Text = "E:" + (pn.actualEasting).ToString("N1");
                    lblNorthing.Text = "N:" + (pn.actualNorthing).ToString("N1");
                }

                lblUturnByte.Text = Convert.ToString(mc.machineData[mc.mdUTurn], 2).PadLeft(6, '0');
            }

            if (ABLine.isBtnABLineOn && !ct.isContourBtnOn) btnEditHeadingB.Text = ((int)(ABLine.moveDistance * 100)).ToString();
            if (curve.isBtnCurveOn && !ct.isContourBtnOn) btnEditHeadingB.Text = ((int)(curve.moveDistance * 100)).ToString();

            pbarAutoSteerComm.Value = pbarSteer;
            pbarUDPComm.Value = pbarUDP;
            pbarMachineComm.Value = pbarMachine;

            if (mc.steerSwitchValue == 0) btnAutoSteer.BackColor = System.Drawing.Color.SkyBlue;
            else btnAutoSteer.BackColor = System.Drawing.Color.Transparent;

            //AutoSteerAuto button enable - Ray Bear inspired code - Thx Ray!
            if (isJobStarted && ahrs.isAutoSteerAuto && !recPath.isDrivingRecordedPath &&
                (ABLine.isBtnABLineOn || ct.isContourBtnOn || curve.isBtnCurveOn))
            {
                if (mc.steerSwitchValue == 0)
                {
                    if (!isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                }
                else
                {
                    if (isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                }
            }

            //Make sure it is off when it should
            if ((!ABLine.isBtnABLineOn && !ct.isContourBtnOn && !curve.isBtnCurveOn && isAutoSteerBtnOn) || (recPath.isDrivingRecordedPath && isAutoSteerBtnOn)) btnAutoSteer.PerformClick();

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
                if (NtripCounter > 20) lblNTRIPSeconds.Text = string.Format("{0:00}:{1:00}", ((NtripCounter-21) / 60), (Math.Abs(NtripCounter-21)) % 60);
                else lblNTRIPSeconds.Text = gStr.gsConnectingIn + " " + (Math.Abs(NtripCounter - 21));

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
            if (isMetric) distanceToolBtn.Text = fd.DistanceUserMeters + "\r\n" + fd.WorkedUserHectares2;
            else distanceToolBtn.Text = fd.DistanceUserFeet + "\r\n" + fd.WorkedUserAcres2;

            //statusbar flash red undefined headland
            if (mc.isOutOfBounds && statusStripBottom.BackColor == Color.Transparent) statusStripBottom.BackColor = Color.Tomato;
            else if (!mc.isOutOfBounds && statusStripBottom.BackColor == Color.Tomato) statusStripBottom.BackColor = Color.Transparent;

                    lblEast.Text = ((int)(pn.actualEasting)).ToString();
                    lblNorth.Text = ((int)(pn.actualNorthing)).ToString();

            testOneSecond1 = testOneSecond.ElapsedMilliseconds;
            OneSecondUpdate.Enabled = true;
        }

        private void ThreeSecond_Update(object sender, EventArgs e)
        {
            ThreeSecondUpdate.Enabled = false;
            testThreeSecond.Restart();
            testThreeSecond.Start();

            zoomUpdateCounter = true;
            //check to make sure the grid is big enough
            worldGrid.checkZoomWorldGrid(pn.fix.northing, pn.fix.easting);

            if (panelBatman.Visible)
            {
                if (isMetric) lblAltitude.Text = Altitude;
                else lblAltitude.Text = AltitudeFeet;

                lblZone.Text = pn.zone.ToString();
            }

            if (isMetric)
            {
                lblTotalFieldArea.Text = fd.AreaBoundaryLessInnersHectares;
                lblTotalAppliedArea.Text = fd.WorkedHectares;
                lblWorkRemaining.Text = fd.WorkedAreaRemainHectares;
                lblPercentRemaining.Text = fd.WorkedAreaRemainPercentage;
                lblTimeRemaining.Text = fd.TimeTillFinished;

                lblAreaAppliedMinusOverlap.Text = ((fd.actualAreaCovered * glm.m2ha).ToString("N2"));
                lblAreaMinusActualApplied.Text = (((fd.areaBoundaryOuterLessInner - fd.actualAreaCovered) * glm.m2ha).ToString("N2"));
                lblOverlapPercent.Text = (fd.overlapPercent.ToString("N2")) + "%";
                lblAreaOverlapped.Text = (((fd.workedAreaTotal - fd.actualAreaCovered) * glm.m2ha).ToString("N3"));

                lblEqSpec.Text = (Math.Round(tool.toolWidth, 2)).ToString() + " m  " + vehicleFileName + toolFileName;
            }
            else //imperial
            {
                lblTotalFieldArea.Text = fd.AreaBoundaryLessInnersAcres;
                lblTotalAppliedArea.Text = fd.WorkedAcres;
                lblWorkRemaining.Text = fd.WorkedAreaRemainAcres;
                lblPercentRemaining.Text = fd.WorkedAreaRemainPercentage;
                lblTimeRemaining.Text = fd.TimeTillFinished;

                lblAreaAppliedMinusOverlap.Text = ((fd.actualAreaCovered * glm.m2ac).ToString("N2"));
                lblAreaMinusActualApplied.Text = (((fd.areaBoundaryOuterLessInner - fd.actualAreaCovered) * glm.m2ac).ToString("N2"));
                lblOverlapPercent.Text = (fd.overlapPercent.ToString("N2")) + "%";
                lblAreaOverlapped.Text = (((fd.workedAreaTotal - fd.actualAreaCovered) * glm.m2ac).ToString("N3"));

                lblEqSpec.Text = (Math.Round(tool.toolWidth * glm.m2ft, 2)).ToString() + " ft  " + vehicleFileName + toolFileName;
            }

            //not Metric/Standard units sensitive
            if (ABLine.isBtnABLineOn) btnABLine.Text = "# " + PassNumber;
            else btnABLine.Text = "";

            if (curve.isBtnCurveOn) btnCurve.Text = "# " + CurveNumber;
            else btnCurve.Text = "";

            lblDateTime.Text = DateTime.Now.ToString("HH:mm:ss") + "\n\r" + DateTime.Now.ToString("ddd MMM yyyy");


            testThreeSecond1 = testThreeSecond.ElapsedMilliseconds;
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
        public string Heading { get { return Convert.ToString(Math.Round(glm.toDegrees(fixHeading), 1)) + "\u00B0"; } }
        public string GPSHeading { get { return (Math.Round(glm.toDegrees(gpsHeading), 1)) + "\u00B0"; } }
        public string Status { get { if (pn.status == "A") return "Active"; else return "Void"; } }
        public string FixQuality
        {
            get
            {
                //if (timerSim.Enabled)
                //    return "Sim: ";

                if (pn.fixQuality == 0) return "Invalid ";
                else if (pn.fixQuality == 1) return "GPS Single ";
                else if (pn.fixQuality == 2) return "DGPS ";
                else if (pn.fixQuality == 3) return "PPS ";
                else if (pn.fixQuality == 4) return "RTK Fix ";
                else if (pn.fixQuality == 5) return "Float ";
                else if (pn.fixQuality == 6) return "Estimate ";
                else if (pn.fixQuality == 7) return "Man IP ";
                else if (pn.fixQuality == 8) return "Sim ";
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
                if (ahrs.rollX16 != 9999)
                    return Math.Round((ahrs.rollX16 - ahrs.rollZeroX16) * 0.0625, 1) + "\u00B0";
                else return "-";
            }
        }
        public string SetSteerAngle { get { return ((double)(guidanceLineSteerAngle) * 0.01).ToString("N1") + "\u00B0"; } }
        public string ActualSteerAngle { get { return ((double)(actualSteerAngleDisp) * 0.01).ToString("N1") + "\u00B0"; } }

        public string FixHeading { get { return Math.Round(fixHeading, 4).ToString(); } }
        
        //public string LookAhead { get { return ((int)(section[0].lookAheadOn)).ToString(); } }
        public string StepFixNum { get { return (currentStepFix).ToString(); } }
        public string CurrentStepDistance { get { return Math.Round(distanceCurrentStepFix, 3).ToString(); } }
        public string TotalStepDistance { get { return Math.Round(fixStepDist, 3).ToString(); } }

        public string WorkSwitchValue { get { return mc.workSwitchValue.ToString(); } }
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
        public string FixOffsetInch { get { return ((pn.fixOffset.easting*glm.m2in).ToString("N0")+ ", " + (pn.fixOffset.northing*glm.m2in).ToString("N0")); } }

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
                if (distancePivotToTurnLine > 0 ) return (((int)(glm.m2ft * (distancePivotToTurnLine))) + " ft");
                else return "--";
            }
        }

        #endregion properties 
    }//end class
}//end namespace