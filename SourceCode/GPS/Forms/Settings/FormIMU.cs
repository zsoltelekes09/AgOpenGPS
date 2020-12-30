using System;
using System.Linq;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormIMU : Form
    {
        private readonly FormGPS mf;

        private double FixStepDist, DualHeadingCorrection, DualAntennaDistance;

        public FormIMU(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //Languages

            groupBox4.Text = String.Get("gsFixFrom");

            this.headingGroupBox.Text = String.Get("gsGPSHeadingFrom");
            this.label13.Text = String.Get("gsDualAntenna");
            this.label12.Text = String.Get("gsFromVTGorRMC");
            this.label11.Text = String.Get("gsFixToFixCalc");
            this.btnRollZero.Text = String.Get("gsRollZero");
            this.btnRemoveZeroOffset.Text = String.Get("gsRemoveOffset");
            this.label10.Text = String.Get("gsALLSettingsRequireRestart");

            this.groupBox6.Text = String.Get("gsRollSource");
            this.rbtnRollAVR.Text = String.Get("gsFromGPS");
            this.rbtnRollAutoSteer.Text = String.Get("gsFromAutoSteer");
            rbtnRollOGI.Text = "OGI";

            this.groupBoxHeadingCorrection.Text = String.Get("gsHeadingCorrectionSource");
            this.rbtnHeadingCorrAutoSteer.Text = String.Get("gsFromAutoSteer");
            //rbtnHeadingCorrUDP.Text = "UDP";
            rbtnHeadingCorrNone.Text = String.Get("gsNone");
            rbtnRollNone.Text = String.Get("gsNone");

            this.groupBox1.Text = String.Get("gsFixToFixDistance");
            this.label35.Text = String.Get("gsMeters");
            this.lblSimGGA.Text = String.Get("gsUseGGAForSimulator");

            this.Text = String.Get("gsDataSources");

            tabHeading.Text = String.Get("gsHeading");
            tabFix.Text = String.Get("gsFix");
            tabRoll.Text = String.Get("gsRoll");
        }

        #region EntryExit

        private void BntOK_Click(object sender, EventArgs e)
        {
            ////Display ---load the delay slides --------------------------------------------------------------------
            Properties.Vehicle.Default.IMU_UID = tboxTinkerUID.Text.Trim();


            Properties.Vehicle.Default.FixStepDist = mf.FixStepDist = FixStepDist;
            Properties.Vehicle.Default.DualHeadingCorrection = mf.DualHeadingCorrection = DualHeadingCorrection;
            Properties.Vehicle.Default.DualAntennaDistance = mf.DualAntennaDistance = DualAntennaDistance;
            Properties.Vehicle.Default.HeadingCorrectionFromAutoSteer = mf.ahrs.isHeadingCorrectionFromAutoSteer = rbtnHeadingCorrAutoSteer.Checked;
            Properties.Vehicle.Default.HeadingCorrectionFromBrick = mf.ahrs.isHeadingCorrectionFromBrick = rbtnHeadingCorrBrick.Checked;
            Properties.Vehicle.Default.RollFromAutoSteer = mf.ahrs.isRollFromAutoSteer = rbtnRollAutoSteer.Checked;
            Properties.Settings.Default.setIMU_isRollFromAVR = mf.ahrs.isRollFromAVR = rbtnRollAVR.Checked;

            Properties.Settings.Default.setIMU_isRollFromOGI = rbtnRollOGI.Checked;
            mf.ahrs.isRollFromOGI = rbtnRollOGI.Checked;

            Properties.Settings.Default.setGPS_isRTK = cboxIsRTK.Checked;
            mf.isRTK = cboxIsRTK.Checked;

            Properties.Vehicle.Default.FusionWeight = (double)(hsbarFusion.Value) * 0.01;
            mf.ahrs.fusionWeight = (double)(hsbarFusion.Value) * 0.01;

            Properties.Vehicle.Default.Save();

            //back to FormGPS
            Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void FormDisplaySettings_Load(object sender, EventArgs e)
        {
            TboxFixStepDistance.Text = ((FixStepDist = Properties.Vehicle.Default.FixStepDist) * mf.Mtr2Unit).ToString(mf.GuiFix);
            TboxFixStepDistance.CheckValue(ref FixStepDist, 0.1, 5);

            TBoxDualHeadingCorrection.Text = (DualHeadingCorrection = Properties.Vehicle.Default.DualHeadingCorrection).ToString("N3");
            TBoxDualHeadingCorrection.CheckValue(ref DualHeadingCorrection, -180, 180);

            TboxDualAntennaDistance.Text = ((DualAntennaDistance = Properties.Vehicle.Default.DualAntennaDistance) * mf.Mtr2Unit).ToString(mf.GuiFix);
            TboxDualAntennaDistance.CheckValue(ref DualAntennaDistance, 0, 15);

            tboxTinkerUID.Text = Properties.Vehicle.Default.IMU_UID;

            //heading correction
            rbtnHeadingCorrAutoSteer.Checked = Properties.Vehicle.Default.HeadingCorrectionFromAutoSteer;
            rbtnHeadingCorrBrick.Checked = Properties.Vehicle.Default.HeadingCorrectionFromBrick;
            //rbtnHeadingCorrUDP.Checked = Properties.Settings.Default.setIMU_isHeadingCorrectionFromExtUDP;
            if (!rbtnHeadingCorrAutoSteer.Checked && !rbtnHeadingCorrBrick.Checked)
                rbtnHeadingCorrNone.Checked = true;   //&& !rbtnHeadingCorrUDP.Checked

            //Roll
            rbtnRollAutoSteer.Checked = Properties.Vehicle.Default.RollFromAutoSteer;
            rbtnRollAVR.Checked = Properties.Settings.Default.setIMU_isRollFromAVR;
            rbtnRollOGI.Checked = Properties.Settings.Default.setIMU_isRollFromOGI;
            if (!rbtnRollAutoSteer.Checked && !rbtnRollAVR.Checked && !rbtnRollOGI.Checked) rbtnRollNone.Checked = true;

            lblRollZeroOffset.Text = ((double)Properties.Vehicle.Default.RollZeroX16 / 16).ToString("N2");

            //Fix
            if (Properties.Vehicle.Default.FixFromSentence == "GGA") rbtnGGA.Checked = true;
            else if (Properties.Vehicle.Default.FixFromSentence == "RMC") rbtnRMC.Checked = true;
            else if (Properties.Vehicle.Default.FixFromSentence == "OGI") rbtnOGI.Checked = true;

            //heading
            if (Properties.Vehicle.Default.HeadingFromSource == "Fix") rbtnHeadingFix.Checked = true;
            else if (Properties.Vehicle.Default.HeadingFromSource == "GPS") rbtnHeadingGPS.Checked = true;
            else if (Properties.Vehicle.Default.HeadingFromSource == "Dual") rbtnHeadingHDT.Checked = true;

            cboxIsRTK.Checked = Properties.Settings.Default.setGPS_isRTK;

            hsbarFusion.Value = (int)(Properties.Vehicle.Default.FusionWeight * 100);
            lblFusion.Text = (hsbarFusion.Value).ToString();
            lblFusionIMU.Text = (50 - hsbarFusion.Value).ToString();
        }

        #endregion EntryExit

        private void BtnRemoveZeroOffset_Click(object sender, EventArgs e)
        {
            mf.ahrs.rollZeroX16 = 0;
            lblRollZeroOffset.Text = "0.00";
            Properties.Vehicle.Default.RollZeroX16 = 0;
            Properties.Vehicle.Default.Save();
        }

        private void BtnZeroRoll_Click(object sender, EventArgs e)
        {
            if (mf.ahrs.isRollFromAutoSteer || mf.ahrs.isRollFromAVR || mf.ahrs.isRollFromOGI)
            {
                mf.ahrs.rollZeroX16 = mf.ahrs.rollX16;
                lblRollZeroOffset.Text = ((double)mf.ahrs.rollZeroX16 / 16).ToString("N2");
                Properties.Vehicle.Default.RollZeroX16 = mf.ahrs.rollX16;
                Properties.Settings.Default.Save();
            }
            else
            {
                lblRollZeroOffset.Text = "***";
            }
        }

        private void RbtnGGA_CheckedChanged(object sender, EventArgs e)
        {
            var checkedButton = groupBox4.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            Properties.Vehicle.Default.FixFromSentence = checkedButton.Text;
            Properties.Vehicle.Default.Save();
            mf.pn.FixFromSentence = checkedButton.Text;
        }

        private void RbtnHeadingFix_CheckedChanged(object sender, EventArgs e)
        {
            var checkedButton = headingGroupBox.Controls.OfType<RadioButton>().FirstOrDefault(r => r.Checked);
            Properties.Vehicle.Default.HeadingFromSource = checkedButton.Text;
            Properties.Vehicle.Default.Save();
            mf.HeadingFromSource = checkedButton.Text;
        }

        private void TboxTinkerUID_Click(object sender, EventArgs e)
        {
            if (mf.isKeyboardOn)
            {
                mf.KeyboardToText((TextBox)sender, this);
                btnCancel.Focus();
            }
        }

        private void HsbarFusion_ValueChanged(object sender, EventArgs e)
        {
            lblFusion.Text = (hsbarFusion.Value).ToString();
            lblFusionIMU.Text = (50 - hsbarFusion.Value).ToString();
        }

        private void TboxFixStepDistance_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0.1, 5, FixStepDist, this, mf.Decimals, true, mf.Unit2Mtr, mf.Mtr2Unit))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxFixStepDistance.Text = ((FixStepDist = form.ReturnValue) * mf.Mtr2Unit).ToString(mf.GuiFix);
                }
            }
            btnCancel.Focus();
        }

        private void TboxDualAntennaDistance_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 15, DualAntennaDistance, this, mf.Decimals, true, mf.Unit2Mtr, mf.Mtr2Unit))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxDualAntennaDistance.Text = ((DualAntennaDistance = form.ReturnValue) * mf.Mtr2Unit).ToString(mf.GuiFix);
                }
            }
            btnCancel.Focus();
        }

        private void TBoxHeadingCorrection_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(-180, 180, DualHeadingCorrection, this, 2, false))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TBoxDualHeadingCorrection.Text = (DualHeadingCorrection = form.ReturnValue).ToString(mf.GuiFix);
                }
            }
            btnCancel.Focus();
        }
    }
}