using AgOpenGPS.Properties;
using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormDisplayOptions : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        //constructor
        public FormDisplayOptions(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            Text = String.Get("gsOptions");

            chkExtraGuides.Text = String.Get("gsExtraGuides");
            chkGrid.Text = String.Get("gsGridOn");
            chkLogNMEA.Text = String.Get("gsLogNMEA");
            chkPolygons.Text = String.Get("gsPolygonsOn");
            chkPursuitLines.Text = String.Get("gsPursuitLine");
            chkSky.Text = String.Get("gsSkyOn");
            chkUTurnOn.Text = String.Get("gsUTurnAlwaysOn");
            chkCompass.Text = String.Get("gsCompassOn");
            chkSpeedo.Text = String.Get("gsSpeedoOn");
            chkStartFullScreen.Text = String.Get("gsStartFullScreen");
            chkDayNight.Text = String.Get("gsAutoDayNightMode");
            chkAutoLoadFields.Text = String.Get("gsAutoLoadFields");
            rbtnMetric.Text = String.Get("gsMetric");
            rbtnImperial.Text = String.Get("gsImperial");
            unitsGroupBox.Text = String.Get("gsUnits");

        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            chkSky.Checked = mf.isSkyOn;
            chkGrid.Checked = mf.isGridOn;
            chkCompass.Checked = mf.isCompassOn;
            chkSpeedo.Checked = mf.isSpeedoOn;
            chkDayNight.Checked = mf.isAutoDayNight;
            chkStartFullScreen.Checked = Properties.Settings.Default.setDisplay_isStartFullScreen;
            chkExtraGuides.Checked = mf.isSideGuideLines;
            chkLogNMEA.Checked = mf.isLogNMEA;
            chkPolygons.Checked = mf.isDrawPolygons;
            chkPursuitLines.Checked = mf.isPureDisplayOn;
            chkUTurnOn.Checked = mf.isUTurnAlwaysOn;
            chkAutoLoadFields.Checked = mf.isAutoLoadFields;
            chkDrawBackBuffer.Checked = mf.DrawBackBuffer;

            if (mf.isMetric) rbtnMetric.Checked = true;
            else rbtnImperial.Checked = true;
        }
        private void BntOK_Click(object sender, EventArgs e)
        {
            mf.isSkyOn = chkSky.Checked;
            mf.isGridOn = chkGrid.Checked;
            mf.isCompassOn = chkCompass.Checked;
            mf.isSpeedoOn = chkSpeedo.Checked;
            mf.isAutoDayNight = chkDayNight.Checked;
            mf.isSideGuideLines = chkExtraGuides.Checked;
            mf.isLogNMEA = chkLogNMEA.Checked;
            mf.isDrawPolygons = chkPolygons.Checked;
            mf.isPureDisplayOn = chkPursuitLines.Checked;
            mf.isUTurnAlwaysOn = chkUTurnOn.Checked;
            mf.isAutoLoadFields = chkAutoLoadFields.Checked;
            mf.DrawBackBuffer = chkDrawBackBuffer.Checked;
            if (mf.isAutoLoadFields) mf.LoadFields();
            else mf.Fields.Clear();

            Settings.Default.setMenu_isSkyOn = mf.isSkyOn;
            Settings.Default.setMenu_isGridOn = mf.isGridOn;
            Settings.Default.setMenu_isCompassOn = mf.isCompassOn;
            Settings.Default.setMenu_isSpeedoOn = mf.isSpeedoOn;
            Settings.Default.setDisplay_isAutoDayNight = mf.isAutoDayNight;
            Settings.Default.setDisplay_isStartFullScreen = chkStartFullScreen.Checked;
            Settings.Default.setMenu_isSideGuideLines = mf.isSideGuideLines;
            Settings.Default.setMenu_isLogNMEA = mf.isLogNMEA;
            mf.isDrawPolygons = chkPolygons.Checked ;
            Settings.Default.setMenu_isPureOn = mf.isPureDisplayOn;
            Settings.Default.setMenu_isUTurnAlwaysOn = mf.isUTurnAlwaysOn;
            Settings.Default.AutoLoadFields = mf.isAutoLoadFields;
            Settings.Default.DrawBackBuffer = mf.DrawBackBuffer;



            if (rbtnMetric.Checked) { Settings.Default.setMenu_isMetric = true; mf.isMetric = true; }
            else { Settings.Default.setMenu_isMetric = false; mf.isMetric = false; }

            //metric settings
            if (mf.isMetric)
            {
                mf.metImp2m = 0.01;
                mf.m2MetImp = 100.0;
                mf.cutoffMetricImperial = 1;
                mf.decimals = 0;
            }
            else
            {
                mf.metImp2m = Glm.in2m;
                mf.m2MetImp = Glm.m2in;
                mf.cutoffMetricImperial = 1.60934;
                mf.decimals = 3;
            }


            Settings.Default.Save();
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}