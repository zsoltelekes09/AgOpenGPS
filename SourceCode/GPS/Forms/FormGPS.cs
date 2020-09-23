//Please, if you use this, share the improvements

using AgOpenGPS.Properties;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Media;
using System.Net;
using System.Net.Sockets;
using System.Reflection;
using System.Windows.Forms;

namespace AgOpenGPS
{
    //the main form object
    public partial class FormGPS : Form
    {
        #region // Class Props and instances

        //list of vec3 points of Dubins shortest path between 2 points - To be converted to RecPt
        public List<Vec3> flagDubinsList = new List<Vec3>();

        //How many youturn functions
        public const int MAXFUNCTIONS = 8;

        //The base directory where AgOpenGPS will be stored and fields and vehicles branch from
        public string baseDirectory;

        //current directory of vehicle
        public string vehiclesDirectory, vehicleFileName = "";

        //current directory of Tools
        public string toolsDirectory, toolFileName = "";

        //current directory of Environments
        public string envDirectory, envFileName = "";

        //current fields and field directory
        public string fieldsDirectory, currentFieldDirectory;
        public int flagNumberPicked = 0;

        //bool for whether or not a job is active
        public bool isJobStarted = false, isAutoSteerBtnOn, isLidarBtnOn = true;

        //if we are saving a file
        public bool isSavingFile = false, isLogNMEA = false, isLogElevation = false;

        //texture holders
        public uint[] texture = new uint[15];

        //the currentversion of software
        public string currentVersionStr, inoVersionStr;
        public int inoVersionInt;

        //create instance of a stopwatch for timing of frames and NMEA hz determination
        private readonly Stopwatch swFrame = new Stopwatch();

        //create instance of a stopwatch for timing of frames and NMEA hz determination
        private readonly Stopwatch swHz = new Stopwatch();

        //whether or not to use Stanley control
        public bool isStanleyUsed = true;

        public int pbarSteer, pbarMachine, pbarUDP;

        public double BoundarySpacing = 2;

        //used by filePicker Form to return picked file and directory
        public string filePickerFileAndDirectory;

        //the autoManual drive button. Assume in Auto
        public bool isInAutoDrive = true;

        //the trackbar angle for TestAutoSteer
        public bool TestAutoSteer;
        public int TestAutoSteerAngle = 0;
        public int TotalSections = 0;

        public CGuidance Guidance = new CGuidance();

        /// <summary>
        /// create the scene camera
        /// </summary>
        public CCamera camera = new CCamera();

        /// <summary>
        /// create world grid
        /// </summary>
        public CWorldGrid worldGrid;

        /// <summary>
        /// The NMEA class that decodes it
        /// </summary>
        public CNMEA pn;

        /// <summary>
        /// AB Line object
        /// </summary>
        public CABLine ABLines;

        /// <summary>
        /// TramLine class for boundary and settings
        /// </summary>
        public CTram tram;

        /// <summary>
        /// The grid for collision Avoidance
        /// </summary>
        public CMazeGrid mazeGrid;

        /// <summary>
        /// Contour Mode Instance
        /// </summary>
        public CContour ct;

        /// <summary>
        /// ABCurve instance
        /// </summary>
        public CABCurve CurveLines;

        /// <summary>
        /// Auto Headland YouTurn
        /// </summary>
        public CYouTurn yt;

        /// <summary>
        /// Our vehicle only
        /// </summary>
        public CVehicle vehicle;

        /// <summary>
        /// Just the tool attachment that includes the sections
        /// </summary>
        public List<CTool> Tools = new List<CTool>();

        /// <summary>
        /// All the structs for recv and send of information out ports
        /// </summary>
        public CModuleComm mc;

        /// <summary>
        /// The boundary object
        /// </summary>
        public CBoundary bnd;

        /// <summary>
        /// The boundary object
        /// </summary>
        public CTurn turn;

        /// <summary>
        /// The headland created
        /// </summary>
        public CHead hd;

        /// <summary>
        /// The entry and exit sequences, functions, actions
        /// </summary>
        public CSequence seq;

        /// <summary>
        /// The internal simulator
        /// </summary>
        public CSim sim;

        /// <summary>
        /// Heading, Roll, Pitch, GPS, Properties
        /// </summary>
        public CAHRS ahrs;

        /// <summary>
        /// Recorded Path
        /// </summary>
        public CRecordedPath recPath;

        /// <summary>
        /// Most of the displayed field data for GUI
        /// </summary>
        public CFieldData fd;

        /// <summary>
        /// GeoFence around everything you cannot cross
        /// </summary>
        public CGeoFence gf;

        /// <summary>
        /// The font class
        /// </summary>
        public CFont font;
        public bool LeftMouseDownOnOpenGL { get; set; }
        public double FrameTime { get; set; } = 0;
        public double HzTime { get; set; } = 8;
        public SoundPlayer SndBoundaryAlarm { get; set; }
        public int MinuteCounter { get; set; } = 1;
        public int TenMinuteCounter { get; set; } = 1;
        public int NtripCounter { get; set; } = 0;
        public FormTimedMessage Form { get; set; } = new FormTimedMessage();

        #endregion // Class Props and instances

        // Constructor, Initializes a new instance of the "FormGPS" class.
        public FormGPS()
        {
            //winform initialization
            InitializeComponent();

            ControlExtension.Draggable(oglZoom, true);
            ControlExtension.Draggable(oglBack, true);

            //build the gesture structures
            SetupStructSizes();

            //create the world grid
            worldGrid = new CWorldGrid(this);

            //our vehicle made with gl object and pointer of mainform
            vehicle = new CVehicle(this);

            //our NMEA parser
            pn = new CNMEA(this);

            //create the ABLine instance
            ABLines = new CABLine(this);

            //new instance of contour mode
            ct = new CContour(this);

            //new instance of contour mode
            CurveLines = new CABCurve(this);

            //instance of tram
            tram = new CTram(this);

            //new instance of auto headland turn
            yt = new CYouTurn(this);

            //module communication
            mc = new CModuleComm(this);

            //boundary object
            bnd = new CBoundary(this);

            //Turn object
            turn = new CTurn(this);

            //GeoFence
            gf = new CGeoFence(this);

            //headland object
            hd = new CHead();

            //headland entry/exit sequences
            seq = new CSequence(this);

            //nmea simulator built in.
            sim = new CSim(this);

            //all the attitude, heading, roll, pitch reference system
            ahrs = new CAHRS(this);

            //A recorded path
            recPath = new CRecordedPath(this);

            //fieldData all in one place
            fd = new CFieldData(this);

            //The grid for obstacle avoidance
            mazeGrid = new CMazeGrid(this);

            // Add Message Event handler for Form decoupling from client socket thread
            updateRTCM_DataEvent = new UpdateRTCM_Data(OnAddMessage);

            //access to font class
            font = new CFont(this);
        }

        private void UpdateGuiText()
        {
            setWorkingDirectoryToolStripMenuItem.Text = gStr.gsDirectories;
            enterSimCoordsToolStripMenuItem.Text = gStr.gsEnterSimCoords;
            topMenuLoadVehicle.Text = gStr.gsLoadVehicle;
            topMenuSaveVehicle.Text = gStr.gsSaveVehicle;
            aboutToolStripMenuItem.Text = gStr.gsAbout;
            shortcutKeysToolStripMenuItem.Text = gStr.gsShortcutKeys;
            menustripLanguage.Text = gStr.gsLanguage;
            topMenuLoadTool.Text = gStr.gsLoadTool;
            topMenuSaveTool.Text = gStr.gsSaveTool;
            topMenuLoadEnvironment.Text = gStr.gsLoadEnvironment;
            topMenuSaveEnvironment.Text = gStr.gsSaveEnvironment;
            gPSInfoToolStripMenuItem.Text = gStr.gsModuleInfo;
            showStartScreenToolStripMenuItem.Text = gStr.gsShowStartScreen;
            //Display Menu

            //settingsToolStripMenuItem.Text = gStr.gsDisplay;
            topMenuFileExplorer.Text = gStr.gsWindowsFileExplorer;
            optionsToolStripMenuItem.Text = gStr.gsOptions;

            simulatorOnToolStripMenuItem.Text = gStr.gsSimulatorOn;

            resetALLToolStripMenuItem.Text = gStr.gsResetAll;
            colorsToolStripMenuItem.Text = gStr.gsColors;
            lightbarToolStripMenuItem.Text = gStr.gsLightbarOn;
            topFieldViewToolStripMenuItem.Text = gStr.gsTopFieldView;
            toolToolStripMenu.Text = gStr.gsTool;

            resetEverythingToolStripMenuItem.Text = gStr.gsResetAllForSure;
            fileExplorerToolStripMenuItem.Text = gStr.gsWindowsFileExplorer;

            //Settings Menu
            toolstripYouTurnConfig.Text = gStr.gsUTurn;
            toolstripAutoSteerConfig.Text = gStr.gsAutoSteer;
            steerChartStripMenu.Text = gStr.gsSteerChart;
            toolstripVehicleConfig.Text = gStr.gsVehicle;
            toolstripDisplayConfig.Text = gStr.gsDataSources;
            toolstripUSBPortsConfig.Text = gStr.gsSerialPorts;
            toolstripUDPConfig.Text = gStr.gsUDP;
            toolStripNTRIPConfig.Text = gStr.gsNTRIP;

            //Tools Menu
            treePlantToolStrip.Text = gStr.gsTreePlanter;
            SmoothABtoolStripMenu.Text = gStr.gsSmoothABCurve;
            toolStripBtnMakeBndContour.Text = gStr.gsMakeBoundaryContours;
            boundariesToolStripMenuItem.Text = gStr.gsBoundary;
            headlandToolStripMenuItem.Text = gStr.gsHeadland;
            deleteContourPathsToolStripMenuItem.Text = gStr.gsDeleteContourPaths;
            deleteAppliedAreaToolStripMenuItem.Text = gStr.gsDeleteAppliedArea;
            deleteForSureToolStripMenuItem.Text = gStr.gsAreYouSure;
            webcamToolStrip.Text = gStr.gsWebCam;
            googleEarthFlagsToolStrip.Text = gStr.gsGoogleEarth;
            offsetFixToolStrip.Text = gStr.gsOffsetFix;
            moduleConfigToolStripMenuItem.Text = gStr.gsModuleConfiguration;

            //Recorded Path
            deletePathMenu.Text = gStr.gsDeletePath;
            recordPathMenu.Text = gStr.gsRecordStop;
            goPathMenu.Text = gStr.gsGoStop;
            pausePathMenu.Text = gStr.gsPauseResume;


            stripSectionColor.Text = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);


            if (isNTRIP_TurnedOn)
            {
                if (NtripCounter > 20) NTRIPStartStopStrip.Text = (isNTRIP_Connecting ? gStr.gsAuthourizing : isNTRIP_Sending ? gStr.gsSendingGGA : (NTRIP_Watchdog > 10 ? gStr.gsWaiting : gStr.gsListening)) + "\n" + string.Format("{0:00}:{1:00}", ((NtripCounter - 21) / 60), (Math.Abs(NtripCounter - 21)) % 60);
                else NTRIPStartStopStrip.Text = gStr.gsConnectingIn + "\n" + (Math.Abs(NtripCounter - 21));
            }
            else NTRIPStartStopStrip.Text = gStr.gsNTRIPOff + "\n";
        }


        private void ZoomByMouseWheel(object sender, MouseEventArgs e)
        {
            if (e.Delta > 0)
            {
                if (camera.zoomValue <= 20) camera.zoomValue += camera.zoomValue * 0.02;
                else camera.zoomValue += camera.zoomValue * 0.01;
                if (camera.zoomValue > 220) camera.zoomValue = 220;
                camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
            }
            else
            {
                if (camera.zoomValue <= 20)
                {
                    if ((camera.zoomValue -= camera.zoomValue * 0.02) < 6.0) camera.zoomValue = 6.0; 
                }
                else
                { 
                    if ((camera.zoomValue -= camera.zoomValue * 0.01) < 6.0) camera.zoomValue = 6.0;
                }

                camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
            }
            SetZoom();
        }

        //Initialize items before the form Loads or is visible
        private void FormGPS_Load(object sender, EventArgs e)
        {
            MouseWheel += ZoomByMouseWheel;

            //remembered window position

            if (Settings.Default.setWindow_Maximized)
            {
                WindowState = FormWindowState.Normal;
                Location = Settings.Default.setWindow_Location;
                Size = Settings.Default.setWindow_Size;
            }
            else
            {
                Location = Settings.Default.setWindow_Location;
                Size = Settings.Default.setWindow_Size;
            }
            if (Settings.Default.setDisplay_isStartFullScreen)
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                btnFullScreen.BackgroundImage = Resources.WindowNormal;
                isFullScreen = true;
            }
            else
            {
                isFullScreen = false;
            }

            vehicleFileName = Vehicle.Default.setVehicle_vehicleName;
            toolFileName = Vehicle.Default.setVehicle_toolName;


            currentVersionStr = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);

            string[] fullVers = currentVersionStr.Split('.');
            int inoV = int.Parse(fullVers[0], CultureInfo.InvariantCulture);
            inoV += int.Parse(fullVers[1], CultureInfo.InvariantCulture);
            inoV += int.Parse(fullVers[2], CultureInfo.InvariantCulture);
            inoVersionInt = inoV;
            inoVersionStr = inoV.ToString();


            if (Settings.Default.setDisplay_isTermsOn)
            {
                Form form = new Form_First(this);
                form.ShowDialog(this);
            }


            // load all the gui settings in gui.designer.cs
            LoadSettings();

            //Calculate total width and each section width
            LoadTools();
        }

        //form is closing so tidy up and save settings
        private void FormGPS_FormClosing(object sender, FormClosingEventArgs e)
        {
            //Save, return, cancel save
            if (isJobStarted)
            {
                bool closing = true;
                int choice = SaveOrNot(closing);

                switch (choice)
                {
                    //OK
                    case 0:
                        isUDPSendConnected = false;
                        Settings.Default.setF_CurrentDir = currentFieldDirectory;
                        Settings.Default.Save();

                        FileSaveEverythingBeforeClosingField();

                        //shutdown and reset all module data
                        mc.ResetAllModuleCommValues(true);
                        break;

                    //Ignore and return
                    case 1:
                        e.Cancel = true;
                        break;
                }
            }

            //save window settings
            if (WindowState == FormWindowState.Maximized)
            {
                Settings.Default.setWindow_Location = RestoreBounds.Location;
                Settings.Default.setWindow_Size = RestoreBounds.Size;
                Settings.Default.setWindow_Maximized = false;
                Settings.Default.setWindow_Minimized = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.setWindow_Location = Location;
                Settings.Default.setWindow_Size = Size;
                Settings.Default.setWindow_Maximized = false;
                Settings.Default.setWindow_Minimized = false;
            }
            else
            {
                Settings.Default.setWindow_Location = RestoreBounds.Location;
                Settings.Default.setWindow_Size = RestoreBounds.Size;
                Settings.Default.setWindow_Maximized = false;
                Settings.Default.setWindow_Minimized = true;
            }

            Settings.Default.setDisplay_camPitch = camera.camPitch;
            Settings.Default.setF_UserTotalArea = fd.workedAreaTotalUser;
            Settings.Default.setF_UserTripAlarm = fd.userSquareMetersAlarm;

            //Settings.Default.setDisplay_panelSnapLocation = panelSnap.Location;
            Settings.Default.setDisplay_panelSimLocation = panelSim.Location;

            Settings.Default.Save();
        }

        //called everytime window is resized, clean up button positions
        private void FormGPS_Resize(object sender, EventArgs e)
        {
            LineUpManualBtns();
            FixPanelsAndMenus();
        }

        // Procedures and Functions ---------------------------------------

        public void LoadGLTextures()
        {
            GL.Enable(EnableCap.Texture2D);
            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "Landscape.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[0]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[0]);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
                        BitmapData bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData.Width, bitmapData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData.Scan0);
                        bitmap.UnlockBits(bitmapData);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File LANDSCAPE.PNG is Missing", ex.Message);
            }

            try
            {
                string text2 = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Dependencies\\Images", "Floor.png");
                if (File.Exists(text2))
                {
                    using (Bitmap bitmap2 = new Bitmap(text2))
                    {
                        GL.GenTextures(1, out texture[1]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[1]);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
                        BitmapData bitmapData2 = bitmap2.LockBits(new Rectangle(0, 0, bitmap2.Width, bitmap2.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmapData2.Width, bitmapData2.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, bitmapData2.Scan0);
                        bitmap2.UnlockBits(bitmapData2);
                    }
                }
            }
            catch (Exception ex2)
            {
                //WriteErrorLog("Loading Floor Texture" + ex2);
                MessageBox.Show("Texture File FLOOR.PNG is Missing", ex2.Message);
            }

            try
            {
                string text2 = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Dependencies\\Images", "Font.png");
                if (File.Exists(text2))
                {
                    using (Bitmap bitmap = new Bitmap(text2))
                    {
                        GL.GenTextures(1, out texture[2]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[2]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);

                        font.textureWidth = bitmap.Width; font.textureHeight = bitmap.Height;
                    }
                }
            }
            catch (Exception ex2)
            {
                //WriteErrorLog("Loading Floor Texture" + ex2);
                MessageBox.Show("Texture File Font.PNG is Missing", ex2.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "Turn.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[3]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[3]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File TURN.PNG is Missing", ex.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "TurnCancel.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[4]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[4]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File TURNCANCEL.PNG is Missing", ex.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "TurnManual.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[5]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[5]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File TURNManual.PNG is Missing", ex.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "Compass.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[6]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[6]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9726);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File Compass.PNG is Missing", ex.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "Speedo.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[7]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[7]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File Speedo.PNG is Missing", ex.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "SpeedoNedle.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[8]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[8]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File SpeedoNeedle.PNG is Missing", ex.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "Lift.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[9]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[9]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Nearest);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File Lift.PNG is Missing", ex.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "LandscapeNight.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[10]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[10]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File LAndscapeNight.PNG is Missing", ex.Message);
            }

            try
            {
                string directoryName = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
                string text = Path.Combine(directoryName, "Dependencies\\images", "Steer.png");
                if (File.Exists(text))
                {
                    using (Bitmap bitmap = new Bitmap(text))
                    {
                        GL.GenTextures(1, out texture[11]);
                        GL.BindTexture(TextureTarget.Texture2D, texture[11]);
                        BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, bitmap.Width, bitmap.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
                        bitmap.UnlockBits(data);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, 9729);
                        GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, 9729);
                    }
                }
            }
            catch (Exception ex)
            {
                //WriteErrorLog("Loading Landscape Textures" + ex);
                MessageBox.Show("Texture File Steer.PNG is Missing", ex.Message);
            }

        }// Load Bitmaps And Convert To Textures

        public void StartLocalUDPServer()
        {
            //inter App sockets
            try
            {
                // Initialise the delegate which updates the status
                updateStatusDelegate = new UpdateStatusDelegate(UpdateAppStatus);

                // Initialise the socket
                sendToAppSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                // Initialise the IPEndPoint for the server to send on port 15554
                IPEndPoint server = new IPEndPoint(IPAddress.Loopback, 15554);
                sendToAppSocket.Bind(server);

                // Initialise the client socket
                recvFromAppSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                // Initialise the IPEndPoint for and listen on port 15555
                IPEndPoint client = new IPEndPoint(IPAddress.Loopback, 15555);
                recvFromAppSocket.Bind(client);

                //Our external app address & port number
                epAppOne = new IPEndPoint(IPAddress.Loopback, 17777);
                //epAppTwo = new IPEndPoint(zeroIP, 16666);

                // Initialise the IPEndPoint for the client
                EndPoint clientEp = new IPEndPoint(IPAddress.Loopback, 0);

                // Start listening for incoming data
                recvFromAppSocket.BeginReceiveFrom(appBuffer, 0, appBuffer.Length, SocketFlags.None,
                                                ref clientEp, new AsyncCallback(ReceiveAppData), recvFromAppSocket);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Load Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SwapDirection()
        {
            if (!yt.isYouTurnTriggered)
            {
                yt.isYouTurnRight = !yt.isYouTurnRight;
                yt.isLastYouTurnRight = !yt.isLastYouTurnRight;
                yt.ResetCreatedYouTurn();
            }
        }

        //dialog for requesting user to save or cancel
        public int SaveOrNot(bool closing)
        {
            using (var form = new FormSaveOrNot(closing, this))
            {
                var result = form.ShowDialog(this);

                if (result == DialogResult.OK) return 0;      //Save and Exit
                if (result == DialogResult.Ignore) return 1;   //Ignore
                if (result == DialogResult.Yes) return 2;      //Save As
                return 3;  // oops something is really busted
            }
        }

        public void KeyboardToText(TextBox sender, Form callingForm)
        {
            sender.BackColor = Color.Red;
            using (var form = new FormKeyboard(sender.Text, callingForm))
            {
                var result = form.ShowDialog(callingForm);
                if (result == DialogResult.OK)
                {
                    sender.Text = form.ReturnString;
                }
            }
            sender.BackColor = Color.AliceBlue;
        }

        //function to set section positions
        //function to calculate the width of each section and update
        public void LoadTools()
        {
            for (int i = Tools.Count - 1; i >= Vehicle.Default.ToolSettings.Count; i--)
            {
                TotalSections -= Tools[i].numOfSections;
                Tools[i].RemoveSections();
                Tools.RemoveAt(i);
            }

            for (int i = 0; i < Vehicle.Default.ToolSettings.Count; i++)
            {
                if (Tools.Count <= i)
                {
                    Tools.Add(new CTool(this, Tools.Count));
                }
                else
                {
                    Tools[i].SetToolSettings(i);
                }
            }

            LineUpManualBtns();
        }

        //request a new job
        public void JobNew()
        {
            if (Settings.Default.setMenu_isOGLZoomOn == 1)
            {
                oglZoom.BringToFront();
                oglZoom.Width = 300;
                oglZoom.Height = 300;
            }
            isJobStarted = true;

            //update the menu
            layoutPanelRight.Enabled = true;
            toolStripBtnDropDownBoundaryTools.Enabled = true;
        }

        //close the current job
        public void JobClose()
        {
            //zoom gone
            oglZoom.SendToBack();

            isJobStarted = false;

            layoutPanelRight.Enabled = false;
            toolStripBtnDropDownBoundaryTools.Enabled = false;

            //CurveLine
            btnCurve.Image = Resources.CurveOff;
            CurveLines.BtnCurveLineOn = false;
            CurveLines.Lines.Clear();
            CurveLines.CurrentLine = -1;
            CurveLines.isOkToAddPoints = false;

            //ABLine
            btnABLine.Image = Resources.ABLineOff;
            ABLines.BtnABLineOn = false;
            ABLines.ABLines.Clear();
            ABLines.CurrentLine = -1;

            //TramLines
            tram.displayMode = 0;
            tram.TramList.Clear();


            //turn off headland
            btnHeadlandOnOff.Image = Resources.HeadlandOff;
            hd.BtnHeadLand = false;
            btnHydLift.Image = Resources.HydraulicLiftOff;
            vehicle.BtnHydLiftOn = false;



            pn.latStart = 0;
            pn.lonStart = 0;

            bnd.bndArr.Clear();
            gf.geoFenceArr.Clear();
            turn.turnArr.Clear();
            hd.headArr.Clear();


            autoBtnState = btnStates.Off;
            btnSection_Update();

            for (int i = 0; i < Tools.Count; i++)
            {
                for (int j = 0; j< Tools[i].Sections.Count; j++)
                {
                    //clear the section lists
                    Tools[i].Sections[j].triangleList.Clear();
                }
            }

            //clear the flags
            flagPts.Clear();



            //clear out contour and Lists
            btnContourPriority.Image = Resources.Snap2;
            ct.ResetContour();

            ct.isContourBtnOn = false;
            btnContour.Image = Resources.ContourOff;
            ct.isContourOn = false;

            //AutoSteer
            isAutoSteerBtnOn = false;
            btnAutoSteer.Image = Resources.AutoSteerOff;

            //auto YouTurn shutdown
            YouTurnButtons(false);

            //reset acre and distance counters
            fd.workedAreaTotal = 0;


            //reset GUI areas
            fd.UpdateFieldBoundaryGUIAreas();

            ////turn off path record
            recPath.recList.Clear();
            if (recPath.isRecordOn)
            {
                recPath.isRecordOn = false;
                recordPathMenu.Image = Resources.BoundaryRecord;
            }

            //reset all Port Module values
            mc.ResetAllModuleCommValues(true);
        }

        //bring up field dialog for new/open/resume
        private void JobNewOpenResume()
        {
            //bring up dialog if no job active, close job if one is
            if (!isJobStarted)
            {
                using (var form = new FormJob(this))
                {
                    var result = form.ShowDialog(this);
                    if (result == DialogResult.Yes)
                    {
                        //ask for a directory name
                        using (var form2 = new FormFieldDir(this))
                        { form2.ShowDialog(this); }
                    }
                }

                Text = "AgOpenGPS - " + currentFieldDirectory;

            }

            //close the current job and ask how to or if to save
            else
            {
                bool closing = false;
                int choice = SaveOrNot(closing);
                switch (choice)
                {
                    //OK
                    case 0:
                        Settings.Default.setF_CurrentDir = currentFieldDirectory;
                        Settings.Default.Save();
                        FileSaveEverythingBeforeClosingField();
                        break;

                    //Ignore and return
                    case 1:
                        break;

                    //Save As
                    case 2:
                        //close current field but remember last used like normal
                        Settings.Default.setF_CurrentDir = currentFieldDirectory;
                        Settings.Default.Save();
                        FileSaveEverythingBeforeClosingField();

                        //ask for a directory name
                        using (var form2 = new FormSaveAs(this))
                        {
                            form2.ShowDialog(this);
                        }

                        break;
                }
            }
        }

        //Does the logic to process section on off requests
        private void ProcessSectionOnOffRequests()
        {
            for (int i = 0; i < Tools.Count; i++)
            {
                for (int j = 0; j< Tools[i].Sections.Count; j++)
                {
                    //SECTIONS - 
                    if (Tools[i].Sections[j].SectionOnRequest)
                    {
                        if (!Tools[i].Sections[j].IsSectionOn)
                        {
                            mc.Send_Sections[3] = (byte)i;
                            mc.Send_Sections[4] = (byte)j;
                            mc.Send_Sections[5] = 0x01;
                            SendData(mc.Send_Sections, false);
                        }
                        Tools[i].Sections[j].IsSectionOn = true;
                        Tools[i].Sections[j].SectionOverlapTimer = (int)(HzTime * Tools[i].TurnOffDelay + 1);

                        if (Tools[i].Sections[j].MappingOnTimer == 0) Tools[i].Sections[j].MappingOnTimer = (int)(HzTime * Tools[i].MappingOnDelay + 1);
                    }
                    else
                    {
                        if (Tools[i].Sections[j].SectionOverlapTimer > 0) Tools[i].Sections[j].SectionOverlapTimer--;
                        if (Tools[i].Sections[j].IsSectionOn && Tools[i].Sections[j].SectionOverlapTimer == 0)
                        {
                            mc.Send_Sections[3] = (byte)i;
                            mc.Send_Sections[4] = (byte)j;
                            mc.Send_Sections[5] = 0x00;
                            SendData(mc.Send_Sections, false);

                            Tools[i].Sections[j].IsSectionOn = false;
                        }
                    }

                    //MAPPING -
                    if (!Tools[i].Sections[j].IsMappingOn && isMapping && Tools[i].Sections[j].MappingOnTimer > 0)
                    {
                        if (Tools[i].Sections[j].MappingOnTimer > 0) Tools[i].Sections[j].MappingOnTimer--;
                        if (Tools[i].Sections[j].MappingOnTimer == 0)
                        {
                            Tools[i].Sections[j].TurnMappingOn();
                        }
                    }

                    if (Tools[i].Sections[j].IsSectionOn)
                    {
                        Tools[i].Sections[j].MappingOffTimer = (int)(Tools[i].MappingOffDelay * HzTime + 1);
                    }
                    else
                    {
                        if (Tools[i].Sections[j].MappingOffTimer > 0) Tools[i].Sections[j].MappingOffTimer--;
                        if (Tools[i].Sections[j].MappingOffTimer == 0)
                        {
                            if (Tools[i].Sections[j].IsMappingOn)
                            {
                                Tools[i].Sections[j].TurnMappingOff();
                                Tools[i].Sections[j].MappingOnTimer = 0;
                            }
                        }
                    }
                }
            }
        }

        //called by you turn class to set control byte, click auto man buttons
        public void DoYouTurnSequenceEvent(int function, int action)
        {
            switch (function)
            {
                case 0: //should not be here - it means no function at all
                    TimedMessageBox(2000, "ID 0 ??????", "YouTurn fucked up");
                    break;

                case 1: //Manual button
                    if (action == 0) autoBtnState = btnStates.Off;
                    else autoBtnState = btnStates.On;
                    btnSection_Update();
                    break;

                case 2: //Auto Button
                    if (action == 0) autoBtnState = btnStates.Off;
                    else autoBtnState = btnStates.Auto;
                    btnSection_Update();
                    break;

                case 3: //Machine 1
                    if (action == 0)
                    {
                        TimedMessageBox(1000, seq.pos3, gStr.gsTurnOff);
                        mc.Send_Uturn[3] &= 0b11111110;
                    }
                    else
                    {
                        TimedMessageBox(1000, seq.pos3, gStr.gsTurnOn);
                        mc.Send_Uturn[3] |= 0b00000001;
                    }
                    break;

                case 4: //Machine 2
                    if (action == 0)
                    {
                        TimedMessageBox(1000, seq.pos4, gStr.gsTurnOff);
                        mc.Send_Uturn[3] &= 0b11111101;
                    }
                    else
                    {
                        TimedMessageBox(1000, seq.pos4, gStr.gsTurnOn);
                        mc.Send_Uturn[3] |= 0b00000010;
                    }
                    break;

                case 5: //Machine 3
                    if (action == 0)
                    {
                        TimedMessageBox(1000, seq.pos5, gStr.gsTurnOff);
                        mc.Send_Uturn[3] &= 0b11111011;
                    }
                    else
                    {
                        TimedMessageBox(1000, seq.pos5, gStr.gsTurnOn);
                        mc.Send_Uturn[3] |= 0b00000100;
                    }
                    break;

                case 6: //Machine 4
                    if (action == 0)
                    {
                        TimedMessageBox(1000, seq.pos6, gStr.gsTurnOff);
                        mc.Send_Uturn[3] &= 0b11110111;
                    }
                    else
                    {
                        TimedMessageBox(1000, seq.pos6, gStr.gsTurnOn);
                        mc.Send_Uturn[3] |= 0b00001000;
                    }
                    break;

                case 7: //Machine 5
                    if (action == 0)
                    {
                        TimedMessageBox(1000, seq.pos7, gStr.gsTurnOff);
                        mc.Send_Uturn[3] &= 0b11101111;
                    }
                    else
                    {
                        TimedMessageBox(1000, seq.pos7, gStr.gsTurnOn);
                        mc.Send_Uturn[3] |= 0b00010000;
                    }
                    break;

                case 8: //Machine 6
                    if (action == 0)
                    {
                        TimedMessageBox(1000, seq.pos8, gStr.gsTurnOff);
                        mc.Send_Uturn[3] &= 0b11011111;
                    }
                    else
                    {
                        TimedMessageBox(1000, seq.pos8, gStr.gsTurnOn);
                        mc.Send_Uturn[3] |= 0b00100000;
                    }
                    break;
            }
        }

        //take the distance from object and convert to camera data
        public void SetZoom()
        {
            //match grid to cam distance and redo perspective
            if (camera.camSetDistance <= -20000) camera.gridZoom = 4000;
            else if (camera.camSetDistance >= -20000 && camera.camSetDistance < -10000) camera.gridZoom = 4000;
            else if (camera.camSetDistance >= -10000 && camera.camSetDistance < -5000) camera.gridZoom = 2000;
            else if (camera.camSetDistance >= -5000 && camera.camSetDistance < -2000) camera.gridZoom = 1000;
            else if (camera.camSetDistance >= -2000 && camera.camSetDistance < -1000) camera.gridZoom = 400;
            else if (camera.camSetDistance >= -1000 && camera.camSetDistance < -500) camera.gridZoom = 200;
            else if (camera.camSetDistance >= -500 && camera.camSetDistance < -250) camera.gridZoom = 100;
            else if (camera.camSetDistance >= -250 && camera.camSetDistance < -150) camera.gridZoom = 50;
            else if (camera.camSetDistance >= -150 && camera.camSetDistance < -50) camera.gridZoom = 30;
            else if (camera.camSetDistance >= -50 && camera.camSetDistance < -1) camera.gridZoom = 10;
            //1.216 2.532

            oglMain.MakeCurrent();
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            Matrix4 mat = Matrix4.CreatePerspectiveFieldOfView((float)fovy, oglMain.AspectRatio, 1f, (float)(camDistanceFactor * camera.camSetDistance));
            GL.LoadMatrix(ref mat);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        //All the files that need to be saved when closing field or app
        public void FileSaveEverythingBeforeClosingField()
        {
            //turn off contour line if on
            if (ct.isContourOn) ct.StopContourLine(pivotAxlePos);

            //turn off all the sections

            for (int i = 0; i < Tools.Count; i++)
            {
                for (int j = 0; j < Tools[i].Sections.Count; j++)
                {
                    if (Tools[i].Sections[j].IsMappingOn) Tools[i].Sections[j].TurnMappingOff();
                    Tools[i].Sections[j].SectionOnRequest = false;
                }
            }

            PatchDrawList.Clear();
            //FileSaveHeadland();

            FileSaveSections();
            FileSaveContour();
            FileSaveFieldKML();

            JobClose();
            Text = "AgOpenGPS";
        }

        public void DrawPatchList(int mipmap)
        {
            //for every new chunk of patch
            foreach (var triList in PatchDrawList)
            {
                bool isDraw = true;
                int count2 = triList.Count;
                for (int k = 1; k < count2; k += 3)
                {
                    //determine if point is in frustum or not, if < 0, its outside so abort, z always is 0                            
                    if (frustum[0] * triList[k].Easting + frustum[1] * triList[k].Northing + frustum[3] <= 0)
                        continue;//right
                    if (frustum[4] * triList[k].Easting + frustum[5] * triList[k].Northing + frustum[7] <= 0)
                        continue;//left
                    if (frustum[16] * triList[k].Easting + frustum[17] * triList[k].Northing + frustum[19] <= 0)
                        continue;//bottom
                    if (frustum[20] * triList[k].Easting + frustum[21] * triList[k].Northing + frustum[23] <= 0)
                        continue;//top
                    if (frustum[8] * triList[k].Easting + frustum[9] * triList[k].Northing + frustum[11] <= 0)
                        continue;//far
                    if (frustum[12] * triList[k].Easting + frustum[13] * triList[k].Northing + frustum[15] <= 0)
                        continue;//near

                    //point is in frustum so draw the entire patch. The downside of triangle strips.
                    isDraw = true;
                    break;
                }

                if (isDraw)
                {
                    count2 = triList.Count;
                    if (count2 < 4) continue;
                    //draw the triangle in each triangle strip
                    GL.Begin(PrimitiveType.TriangleStrip);

                    if (isDay) GL.Color4((byte)triList[0].Northing, (byte)triList[0].Easting, (byte)triList[0].Heading, (byte)152);
                    else GL.Color4((byte)triList[0].Northing, (byte)triList[0].Easting, (byte)triList[0].Heading, (byte)(152 * 0.5));

                    //if large enough patch and camera zoomed out, fake mipmap the patches, skip triangles
                    if (count2 >= (mipmap + 2))
                    {
                        int step = mipmap;
                        for (int k = 1; k < count2; k += step)
                        {
                            GL.Vertex3(triList[k].Easting, triList[k].Northing, 0); k++;
                            GL.Vertex3(triList[k].Easting, triList[k].Northing, 0); k++;
                            if (count2 - k <= (mipmap + 2)) step = 0;//too small to mipmap it
                        }
                    }
                    else { for (int k = 1; k < count2; k++) GL.Vertex3(triList[k].Easting, triList[k].Northing, 0); }
                    GL.End();
                }
            }
        }

        public void DrawSectionsPatchList(bool color)
        {
            // the follow up to sections patches
            int patchCount;

            for (int i = 0; i < Tools.Count; i++)
            {
                for (int j = 0; j < Tools[i].Sections.Count; j++)
                {
                    // the follow up to sections patches
                    if (Tools[i].Sections[j].IsMappingOn && (patchCount = Tools[i].Sections[j].triangleList.Count) > 0)
                    {
                        if (color)
                        {
                            if (isDay) GL.Color4((byte)Tools[i].Sections[j].triangleList[0].Northing, (byte)Tools[i].Sections[j].triangleList[0].Easting, (byte)Tools[i].Sections[j].triangleList[0].Heading, (byte)152);
                            else GL.Color4((byte)Tools[i].Sections[j].triangleList[0].Northing, (byte)Tools[i].Sections[j].triangleList[0].Easting, (byte)Tools[i].Sections[j].triangleList[0].Heading, (byte)(152 * 0.5));
                        }
                        //draw the triangle in each triangle strip
                        GL.Begin(PrimitiveType.TriangleStrip);

                        for (int k = 1; k < patchCount; k++)
                        {
                            GL.Vertex3(Tools[i].Sections[j].triangleList[k].Easting, Tools[i].Sections[j].triangleList[k].Northing, 0);
                        }
                        if (Tools[i].Sections[j].IsMappingOn)
                        {
                            GL.Vertex3(Tools[i].Sections[j].leftPoint.Easting, Tools[i].Sections[j].leftPoint.Northing, 0);
                            GL.Vertex3(Tools[i].Sections[j].rightPoint.Easting, Tools[i].Sections[j].rightPoint.Northing, 0);
                        }
                        GL.End();
                    }
                }
            }
        }

        //an error log called by all try catches
        public void WriteErrorLog(string strErrorText)
        {
            try
            {
                //set up file and folder if it doesn't exist
                const string strFileName = "Error Log.txt";
                //string strPath = Application.StartupPath;

                //Write out the error appending to existing
                File.AppendAllText(baseDirectory + "\\" + strFileName, strErrorText + " - " +
                    DateTime.Now.ToString() + "\r\n\r\n");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error in WriteErrorLog: " + ex.Message, "Error Logging", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }
        //message box pops up with info then goes away
        public void TimedMessageBox(int timeout, string s1, string s2)
        {
            Form.SetTimedMessage(timeout, s1, s2, this);
        }
    }//class FormGPS
}//namespace AgOpenGPS

/*The order is:
 *
 * The watchdog timer times out and runs this function ScanForNMEA_Tick().
 * 50 times per second so statusUpdateCounter counts to 25 and updates strip menu etc at 2 hz
 * it also makes sure there is new sentences showing up otherwise it shows **** No GGA....
 * saveCounter ticks 1 x per second, used at end of draw routine every minute to save a backup of field
 * then ScanForNMEA function checks for a complete sentence if contained in pn.rawbuffer
 * if not it comes right back and waits for next watchdog trigger and starts all over
 * if a new sentence is there, UpdateFix() is called
 * Right away CalculateLookAhead(), no skips, is called to determine lookaheads and trigger distances to save triangles plotted
 * Then UpdateFix() continues.
 * Hitch, pivot, antenna locations etc and directions are figured out if trigDistance is triggered
 * When that is done, DoRender() is called on the visible OpenGL screen and its draw routine _draw is run
 * before triangles are drawn, frustum cull figures out how many of the triangles should be drawn
 * When its all the way thru, it triggers the sectioncontrol Draw, its frustum cull, and determines if sections should be on
 * ProcessSectionOnOffRequests() runs and that does the section on off magic
 * SectionControlToArduino() runs and spits out the port machine control based on sections on or off
 * If field needs saving (1.5 minute since last time) field is saved
 * Now the program is "Done" and waits for the next watchdog trigger, determines if a new sentence is available etc
 * and starts all over from the top.
 */
