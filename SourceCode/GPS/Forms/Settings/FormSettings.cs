//Please, if you use this, share the improvements

using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSettings : Form
    {
        //class variables
        private readonly FormGPS mf;

        private double antennaHeight, antennaOffset, antennaPivot, wheelbase, minTurningRadius, hydliftsecs;

        private bool isPivotBehindAntenna, isSteerAxleAhead;
        private int snapDistance, vehicleType, lightbarCmPerPixie, linewidth;

        //constructor
        public FormSettings(Form callingForm, int page)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            tabSettings.Text = gStr.gsAntenna;
            tabVehicle.Text = gStr.gsVehicle;
            gboxAttachment.Text = gStr.gsVehicleType;
            label6.Text = gStr.gsTurnRadius;
            label26.Text = gStr.gsWheelbase;
            label9.Text = gStr.gsLeftIs_;
            label15.Text = gStr.gsHeight;
            label7.Text = gStr.gsOffset;
            label18.Text = gStr.gsDistance;
            groupBox3.Text = gStr.gsHydraulicLiftLookAhead;
            tabConfig.Text = gStr.gsType;
            tabGuidance.Text = gStr.gsGuidance;


            groupBox2.Text = gStr.gsCmPerLightbarPixel;
            groupBox1.Text = gStr.gs____SnapDistance;
            label17.Text = gStr.gsMeasurementsIn;
            groupBox4.Text = gStr.gsGuidanceLineWidth;
            Text = gStr.gsVehicleSettings;

            lblInchesCm.Text = mf.isMetric? gStr.gsCentimeters : gStr.gsInches;

            //select the page as per calling menu or button from mainGPS form
            tabControl1.SelectedIndex = page;
        }

        //do any field initializing for form here
        private void FormSettings_Load(object sender, EventArgs e)
        {
            //TypeTab
            isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna;
            isSteerAxleAhead = Properties.Vehicle.Default.setVehicle_isSteerAxleAhead;


            vehicleType = Properties.Vehicle.Default.setVehicle_vehicleType;

            //front page
            if (vehicleType == 0) rbtnTractor.Checked = true;
            else if (vehicleType == 1) rbtnHarvester.Checked = true;
            else rbtn4WD.Checked = true;

            FixRadioButtonsAndImages();

            //SettingsTab
            TboxAntennaPivot.Text = (antennaPivot = Math.Round(Math.Abs(Properties.Vehicle.Default.setVehicle_antennaPivot) * mf.m2MetImp, mf.decimals)).ToString();
            TboxAntennaPivot.CheckValue(ref antennaPivot, 0, Math.Round(10 * mf.m2MetImp, mf.decimals));
            TboxAntennaHeight.Text = (antennaHeight = Math.Round(Properties.Vehicle.Default.setVehicle_antennaHeight * mf.m2MetImp, mf.decimals)).ToString();
            TboxAntennaHeight.CheckValue(ref antennaHeight, 0, Math.Round(10 * mf.m2MetImp, mf.decimals));
            TboxWheelbase.Text = (wheelbase = Math.Round(Math.Abs(Properties.Vehicle.Default.setVehicle_wheelbase) * mf.m2MetImp, mf.decimals)).ToString();
            TboxWheelbase.CheckValue(ref wheelbase, 0, Math.Round(20 * mf.m2MetImp, mf.decimals));
            TboxAntennaOffset.Text = (antennaOffset = Math.Round(Properties.Vehicle.Default.setVehicle_antennaOffset * mf.m2MetImp, mf.decimals)).ToString();
            TboxAntennaOffset.CheckValue(ref antennaHeight, Math.Round(-10 * mf.m2MetImp, mf.decimals), Math.Round(10 * mf.m2MetImp, mf.decimals));

            //VehicleTab
            TboxMinTurnRadius.Text = (minTurningRadius = Math.Round(Properties.Vehicle.Default.setVehicle_minTurningRadius * mf.m2MetImp, mf.decimals)).ToString();
            TboxMinTurnRadius.CheckValue(ref minTurningRadius, Math.Round(0.5 * mf.m2MetImp, mf.decimals), Math.Round(100 * mf.m2MetImp, mf.decimals));
            TboxHydLiftSecs.Text = (hydliftsecs = Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead).ToString("0.0#");
            TboxHydLiftSecs.CheckValue(ref hydliftsecs, 0, 20);

            //GuidanceTab
            TboxSnapDistance.Text = (snapDistance = Properties.Settings.Default.setAS_snapDistance).ToString();
            TboxSnapDistance.CheckValue(ref snapDistance, 1, 500);
            TboxLightbarCmPerPixel.Text = (lightbarCmPerPixie = Properties.Settings.Default.setDisplay_lightbarCmPerPixel).ToString();
            TboxLightbarCmPerPixel.CheckValue(ref lightbarCmPerPixie, 1, 20);
            TboxLineWidth.Text = (linewidth = Properties.Settings.Default.setDisplay_lineWidth).ToString();
            TboxLineWidth.CheckValue(ref linewidth, 1, 8);

        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            //TypeTab
            Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna = mf.vehicle.isPivotBehindAntenna = isPivotBehindAntenna;
            Properties.Vehicle.Default.setVehicle_isSteerAxleAhead = mf.vehicle.isSteerAxleAhead = isSteerAxleAhead;
            Properties.Vehicle.Default.setVehicle_vehicleType = mf.vehicle.vehicleType = vehicleType;

            //SettingsTab
            Properties.Vehicle.Default.setVehicle_antennaPivot = mf.vehicle.antennaPivot = Math.Round(antennaPivot * mf.metImp2m, 2);
            Properties.Vehicle.Default.setVehicle_antennaHeight = mf.vehicle.antennaHeight = Math.Round(antennaHeight * mf.metImp2m, 2);
            Properties.Vehicle.Default.setVehicle_wheelbase = mf.vehicle.wheelbase = Math.Round(wheelbase * mf.metImp2m, 2);
            Properties.Vehicle.Default.setVehicle_antennaOffset = mf.vehicle.antennaOffset = Math.Round(antennaOffset * mf.metImp2m, 2);

            //VehicleTab
            Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead = mf.vehicle.hydLiftLookAheadTime = hydliftsecs;
            Properties.Vehicle.Default.setVehicle_minTurningRadius = mf.vehicle.minTurningRadius = Math.Round(minTurningRadius * mf.metImp2m, 2);

            //GuidanceTab
            Properties.Settings.Default.setAS_snapDistance = snapDistance;
            Properties.Settings.Default.setDisplay_lightbarCmPerPixel = mf.lightbarCmPerPixel = lightbarCmPerPixie;
            Properties.Settings.Default.setDisplay_lineWidth = mf.ABLines.lineWidth = linewidth;

            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void BtnChangeAttachment_Click(object sender, EventArgs e)
        {
            btnChangeAttachment.Enabled = false;
            btnChangeAttachment.BackColor = System.Drawing.Color.Transparent;

            FixRadioButtonsAndImages();
            tabControl1.SelectedTab = tabSettings;

            btnNext.Focus();

            isPivotBehindAntenna = vehicleType != 2;//4WD
            isSteerAxleAhead = vehicleType != 1;//harvestor
        }


        private void Rbtn4WD_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;

            if (radioButton.Checked)
            {
                btnChangeAttachment.Enabled = true;
                btnChangeAttachment.BackColor = System.Drawing.SystemColors.ActiveCaption;
            }
        }

        private void FixRadioButtonsAndImages()
        {
            if (rbtnTractor.Checked)
            {
                vehicleType = 0;
                tabSettings.BackgroundImage = Properties.Resources.VehicleSettingsTractor;

            }
            else if (rbtnHarvester.Checked)
            {
                vehicleType = 1;
                tabSettings.BackgroundImage = Properties.Resources.VehicleSettingsHarvester;

            }
            else if (rbtn4WD.Checked)
            {
                vehicleType = 2;
                tabSettings.BackgroundImage = Properties.Resources.VehicleSettings4WD;
            }
        }

        #region SettingsTab
        private void TboxAntennaPivot_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(10 * mf.m2MetImp, mf.decimals), antennaPivot, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxAntennaPivot.Text = (antennaPivot = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxAntennaHeight_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(10 * mf.m2MetImp, mf.decimals), antennaHeight, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxAntennaHeight.Text = (antennaHeight = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxWheelbase_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(20 * mf.m2MetImp, mf.decimals), wheelbase, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxWheelbase.Text = (wheelbase = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxAntennaOffset_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(-10 * mf.m2MetImp, mf.decimals), Math.Round(10 * mf.m2MetImp, mf.decimals), antennaOffset, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxAntennaOffset.Text = (antennaOffset = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }
        #endregion SettingsTab
        #region VehicleTab
        private void TboxHydLiftSecs_Enter(object sender, EventArgs e)
        {

            using (var form = new FormNumeric(0, 20, hydliftsecs, this, false,2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxHydLiftSecs.Text = (hydliftsecs = Math.Round(form.ReturnValue, 2)).ToString("0.0#");
                }
            }
            btnCancel.Focus();
        }

        private void TboxMinTurnRadius_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(Math.Round(0.5 * mf.m2MetImp, mf.decimals), Math.Round(100 * mf.m2MetImp, mf.decimals), minTurningRadius, this, mf.isMetric, mf.decimals))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMinTurnRadius.Text = (minTurningRadius = Math.Round(form.ReturnValue, mf.decimals)).ToString();
                }
            }
            btnCancel.Focus();
        }
        #endregion VehicleTab
        #region GuidanceTab
        private void TboxSnapDistance_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 500, snapDistance, this, true, 0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxSnapDistance.Text = (snapDistance = (int)form.ReturnValue).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxLightbarCmPerPixel_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 20, lightbarCmPerPixie, this, true, 0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxLightbarCmPerPixel.Text = (lightbarCmPerPixie = (int)form.ReturnValue).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxLineWidth_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 8, linewidth, this, true, 0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxLineWidth.Text = (linewidth = (int)form.ReturnValue).ToString();
                }
            }
            btnCancel.Focus();
        }
        #endregion GuidanceTab
    }
}