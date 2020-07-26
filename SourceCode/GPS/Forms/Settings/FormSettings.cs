//Please, if you use this, share the improvements

using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSettings : Form
    {
        //class variables
        private readonly FormGPS mf;

        private double antennaHeight, antennaOffset, antennaPivot, wheelbase, minTurningRadius;

        private bool isPivotBehindAntenna, isSteerAxleAhead;

        private readonly double metImp2m, m2MetImp;

        private bool isAutoSteerAuto;
        private int snapDistance, vehicleType;

        private int lightbarCmPerPixie;

        //constructor
        public FormSettings(Form callingForm, int page)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
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
            groupBox9.Text = gStr.gsAutoManualAutosteerBtn;
            cboxAutoSteerAuto.Text = gStr.gsManual;
            groupBox1.Text = gStr.gs____SnapDistance;
            label17.Text = gStr.gsMeasurementsIn;
            groupBox4.Text = gStr.gsGuidanceLineWidth;
            Text = gStr.gsVehicleSettings;

            nudMinTurnRadius.Controls[0].Enabled = false;
            nudAntennaHeight.Controls[0].Enabled = false;
            nudAntennaOffset.Controls[0].Enabled = false;
            nudAntennaPivot.Controls[0].Enabled = false;
            nudLightbarCmPerPixel.Controls[0].Enabled = false;
            nudMinTurnRadius.Controls[0].Enabled = false;
            nudSnapDistance.Controls[0].Enabled = false;
            nudWheelbase.Controls[0].Enabled = false;
            nudLineWidth.Controls[0].Enabled = false;
            nudHydLiftSecs.Controls[0].Enabled = false;

            if (mf.isMetric)
            {
                metImp2m = 0.01;
                m2MetImp = 100.0;
                lblInchesCm.Text = gStr.gsCentimeters;
            }
            else
            {
                metImp2m = Glm.in2m;
                m2MetImp = Glm.m2in;
                lblInchesCm.Text = gStr.gsInches;
            }
            //select the page as per calling menu or button from mainGPS form
            tabControl1.SelectedIndex = page;
        }

        //do any field initializing for form here
        private void FormSettings_Load(object sender, EventArgs e)
        {
            nudLightbarCmPerPixel.Value = (Properties.Settings.Default.setDisplay_lightbarCmPerPixel);
            lightbarCmPerPixie = Properties.Settings.Default.setDisplay_lightbarCmPerPixel;

            //Vehicle settings to what it is in the settings page------------------------------------------------
            antennaHeight = Properties.Vehicle.Default.setVehicle_antennaHeight;
            if (nudAntennaHeight.CheckValueCm(ref antennaHeight)) nudAntennaHeight.BackColor = System.Drawing.Color.OrangeRed;

            antennaPivot = Math.Abs(Properties.Vehicle.Default.setVehicle_antennaPivot);
            if (nudAntennaPivot.CheckValueCm(ref antennaPivot)) nudAntennaPivot.BackColor = System.Drawing.Color.OrangeRed;

            wheelbase = Math.Abs(Properties.Vehicle.Default.setVehicle_wheelbase);
            if (nudWheelbase.CheckValueCm(ref wheelbase)) nudWheelbase.BackColor = System.Drawing.Color.OrangeRed;

            minTurningRadius = Properties.Vehicle.Default.setVehicle_minTurningRadius;
            if (nudMinTurnRadius.CheckValueCm(ref minTurningRadius)) nudMinTurnRadius.BackColor = System.Drawing.Color.OrangeRed;

            antennaOffset = Properties.Vehicle.Default.setVehicle_antennaOffset;
            if (nudAntennaOffset.CheckValueCm(ref antennaOffset)) nudAntennaOffset.BackColor = System.Drawing.Color.OrangeRed;

            nudHydLiftSecs.Value = (decimal)Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead;

            isPivotBehindAntenna = Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna;
            isSteerAxleAhead = Properties.Vehicle.Default.setVehicle_isSteerAxleAhead;

            nudSnapDistance.Value = Properties.Settings.Default.setAS_snapDistance;

            cboxAutoSteerAuto.Checked = Properties.Settings.Default.setAS_isAutoSteerAutoOn;
            isAutoSteerAuto = Properties.Settings.Default.setAS_isAutoSteerAutoOn;
            if (isAutoSteerAuto)
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOn;
                cboxAutoSteerAuto.Text = "Remote";
            }
            else
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOff;
                cboxAutoSteerAuto.Text = gStr.gsManual;
            }

            //fix the min max based on inches - they are 2.54 times smaller then cm
            if (!mf.isMetric)
            {
                nudAntennaHeight.Maximum /= 2.54M;
                nudAntennaHeight.Minimum /= 2.54M;

                nudAntennaPivot.Maximum /= 2.54M;
                nudAntennaPivot.Minimum /= 2.54M;

                nudWheelbase.Maximum /= 2.54M;
                nudWheelbase.Minimum /= 2.54M;

                nudAntennaOffset.Maximum /= 2.54M;
                nudAntennaOffset.Minimum /= 2.54M;

                nudMinTurnRadius.Maximum /= 2.54M;
                nudMinTurnRadius.Minimum /= 2.54M;
            }

            nudAntennaHeight.ValueChanged -= NudAntennaHeight_ValueChanged;
            nudAntennaHeight.Value = (decimal)(antennaHeight * m2MetImp);
            nudAntennaHeight.ValueChanged += NudAntennaHeight_ValueChanged;

            nudAntennaOffset.ValueChanged -= NudAntennaOffset_ValueChanged;
            nudAntennaOffset.Value = (decimal)(antennaOffset * m2MetImp);
            nudAntennaOffset.ValueChanged += NudAntennaOffset_ValueChanged;

            nudAntennaPivot.ValueChanged -= NudAntennaPivot_ValueChanged;
            nudAntennaPivot.Value = (decimal)(antennaPivot * m2MetImp);
            nudAntennaPivot.ValueChanged += NudAntennaPivot_ValueChanged;

            nudWheelbase.ValueChanged -= NudWheelbase_ValueChanged;
            nudWheelbase.Value = (decimal)(wheelbase * m2MetImp);
            nudWheelbase.ValueChanged += NudWheelbase_ValueChanged;

            nudMinTurnRadius.ValueChanged -= NudMinTurnRadius_ValueChanged;
            nudMinTurnRadius.Value = (decimal)(minTurningRadius * m2MetImp);
            nudMinTurnRadius.ValueChanged += NudMinTurnRadius_ValueChanged;

            nudLineWidth.Value = Properties.Settings.Default.setDisplay_lineWidth;

            vehicleType = Properties.Vehicle.Default.setVehicle_vehicleType;

            //front page
            if (vehicleType == 0) rbtnTractor.Checked = true;
            else if (vehicleType == 1) rbtnHarvester.Checked = true;
            else rbtn4WD.Checked = true;

            FixRadioButtonsAndImages();

            btnChangeAttachment.Enabled = false;
            btnChangeAttachment.BackColor = System.Drawing.Color.Transparent;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.setDisplay_lightbarCmPerPixel = lightbarCmPerPixie;
            mf.lightbarCmPerPixel = lightbarCmPerPixie;

            //Vehicle settings -------------------------------------------------------------------------------

            mf.vehicle.isPivotBehindAntenna = isPivotBehindAntenna;
            Properties.Vehicle.Default.setVehicle_isPivotBehindAntenna = mf.vehicle.isPivotBehindAntenna;

            mf.vehicle.isSteerAxleAhead = isSteerAxleAhead;
            Properties.Vehicle.Default.setVehicle_isSteerAxleAhead = mf.vehicle.isSteerAxleAhead;

            mf.vehicle.antennaPivot = antennaPivot;
            Properties.Vehicle.Default.setVehicle_antennaPivot = mf.vehicle.antennaPivot;

            mf.vehicle.wheelbase = wheelbase;
            Properties.Vehicle.Default.setVehicle_wheelbase = wheelbase;

            mf.vehicle.minTurningRadius = minTurningRadius;
            Properties.Vehicle.Default.setVehicle_minTurningRadius = minTurningRadius;

            mf.vehicle.antennaHeight = antennaHeight;
            Properties.Vehicle.Default.setVehicle_antennaHeight = mf.vehicle.antennaHeight;

            mf.vehicle.antennaOffset = antennaOffset;
            Properties.Vehicle.Default.setVehicle_antennaOffset = antennaOffset;

            //Guidance

            Properties.Settings.Default.setAS_snapDistance = snapDistance;

            mf.ahrs.isAutoSteerAuto = isAutoSteerAuto;
            Properties.Settings.Default.setAS_isAutoSteerAutoOn = isAutoSteerAuto;

            Properties.Settings.Default.setDisplay_lineWidth = (int)(nudLineWidth.Value);
            mf.ABLine.lineWidth = (int)(nudLineWidth.Value);

            mf.tram.abOffset = (Math.Round((mf.Guidance.GuidanceWidth - mf.Guidance.GuidanceOverlap) / 2.0, 3));

            Properties.Vehicle.Default.setVehicle_vehicleType = vehicleType;
            mf.vehicle.vehicleType = vehicleType;

            mf.vehicle.hydLiftLookAheadTime = (double)nudHydLiftSecs.Value;
            Properties.Vehicle.Default.setVehicle_hydraulicLiftLookAhead = mf.vehicle.hydLiftLookAheadTime;

            Properties.Settings.Default.Save();
            Properties.Vehicle.Default.Save();

            //back to FormGPS
            DialogResult = DialogResult.OK;
            Close();
        }

        private void NudLightbarCmPerPixel_ValueChanged(object sender, EventArgs e)
        {
            lightbarCmPerPixie = (int)nudLightbarCmPerPixel.Value;
        }

        private void NudSnapDistance_ValueChanged(object sender, EventArgs e)
        {
            snapDistance = (int)nudSnapDistance.Value;
        }

        private void NudMinTurnRadius_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }
        
        private void NudWheelbase_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudAntennaPivot_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudAntennaHeight_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudAntennaOffset_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudCutoffSpeed_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudSnapDistanceSmall_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudSnapDistance_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudLightbarCmPerPixel_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void NudLineWidth_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void CboxAutoSteerAuto_CheckedChanged(object sender, EventArgs e)
        {
            isAutoSteerAuto = cboxAutoSteerAuto.Checked;
            if (isAutoSteerAuto)
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOn;
                cboxAutoSteerAuto.Text = "Remote";
            }
            else
            {
                cboxAutoSteerAuto.Image = Properties.Resources.AutoSteerOff;
                cboxAutoSteerAuto.Text = gStr.gsManual;
            }
        }

        //don't save anything, leave the settings as before
        private void BtnCancel_Click(object sender, EventArgs e)
        { DialogResult = DialogResult.Cancel; Close(); }

        private void NudHydLiftSecs_Enter(object sender, EventArgs e)
        {
            mf.KeypadToNUD((NumericUpDown)sender);
            btnCancel.Focus();
        }

        private void BtnChangeAttachment_Click(object sender, EventArgs e)
        {
            btnChangeAttachment.Enabled = false;
            btnChangeAttachment.BackColor = System.Drawing.Color.Transparent;

            FixRadioButtonsAndImages();
            tabControl1.SelectedTab = tabSettings;

            btnNext.Focus();

            if (vehicleType == 0) //2WD tractor
            {
                isPivotBehindAntenna = true;
                isSteerAxleAhead = true;
            }
            if (vehicleType == 1) //harvestor
            {
                isPivotBehindAntenna = true;
                isSteerAxleAhead = false;
            }
            if (vehicleType == 2) //4WD
            {
                isPivotBehindAntenna = false;
                isSteerAxleAhead = true;
            }
        }

        private void Rbtn4WD_CheckedChanged(object sender, EventArgs e)
        {
            var radioButton = sender as RadioButton;

            // Only do something when the event was raised by the radiobutton 
            // being checked, so we don't do this twice.
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

        #region Vehicle //----------------------------------------------------------------

        private void NudAntennaHeight_ValueChanged(object sender, EventArgs e)
        {
            antennaHeight = (double)nudAntennaHeight.Value * metImp2m;
        }

        private void NudAntennaPivot_ValueChanged(object sender, EventArgs e)
        {
            antennaPivot = (double)nudAntennaPivot.Value * metImp2m;
        }

        private void NudAntennaOffset_ValueChanged(object sender, EventArgs e)
        {
            antennaOffset = (double)nudAntennaOffset.Value * metImp2m;
        }

        private void NudMinTurnRadius_ValueChanged(object sender, EventArgs e)
        {
            minTurningRadius = (double)nudMinTurnRadius.Value * metImp2m;
        }

        private void NudWheelbase_ValueChanged(object sender, EventArgs e)
        {
            wheelbase = (double)nudWheelbase.Value * metImp2m;
        }

        #endregion Vehicle //----------------------------------------------------------------
    }
}