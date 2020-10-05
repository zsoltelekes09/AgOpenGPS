using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormSteer : Form
    {
        private readonly FormGPS mf;

        bool toSend = false;

        //Form stuff
        public FormSteer(Form callingForm)
        {
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            this.btnFreeDrive.Text = gStr.gsDrive;
            this.tabGain.Text = gStr.gsGain;
            this.label29.Text = gStr.gsSidehillDraftGain;
            //this.label22.Text = gStr.gsOutputGain;
            this.label41.Text = gStr.gsMinimumPWMDrive;
            this.label7.Text = gStr.gsProportionalGain;
            label1.Text = gStr.gsChooseType;

            this.tabSteer.Text = gStr.gsSteer;
            this.label25.Text = gStr.gsCountsPerDegree;
            //this.label45.Text = gStr.gsMaxIntegralValue;
            this.label19.Text = gStr.gsMaxSteerAngleInDegrees;
            //this.label33.Text = gStr.gsIntegralGain;
            this.label10.Text = gStr.gsWheelAngleSensorZero;

            this.tabLook.Text = "Pure P";
            this.label2.Text = gStr.gsUTurnLookAheadMultiplier;
            this.label37.Text = gStr.gsLookAheadInSeconds;
            this.label4.Text = gStr.gsLookAheadOfflineMultiplier;
            this.label6.Text = gStr.gsMinLookAheadInMeters;

            this.tabStan.Text = "Stanley";
            this.btnChart.Text = gStr.gsSteerChart;
            this.label3.Text = gStr.gsAgressiveness;
            this.label5.Text = gStr.gsOvershootReduction;
            this.Text = gStr.gsAutoSteerConfiguration;
        }

        private void FormSteer_Load(object sender, EventArgs e)
        {
            hsbarSteerAngleSensorZero.Value = Properties.Vehicle.Default.setAS_steerAngleOffset - 127;
            lblSteerAngleSensorZero.Text = hsbarSteerAngleSensorZero.Value.ToString();

            hsbarCountsPerDegree.Value = Properties.Vehicle.Default.setAS_countsPerDegree;
            lblCountsPerDegree.Text = hsbarCountsPerDegree.Value.ToString();

            hsbarMinPWM.Value = Properties.Vehicle.Default.setAS_minSteerPWM;
            lblMinPWM.Text = hsbarMinPWM.Value.ToString();

            hsbarProportionalGain.Value = Properties.Vehicle.Default.setAS_Kp;
            lblProportionalGain.Text = hsbarProportionalGain.Value.ToString();

            hsbarSidehillDraftGain.Value = Properties.Vehicle.Default.setAS_Kd;
            lblSidehillDraftGain.Text = hsbarSidehillDraftGain.Value.ToString();

            hsbarLowSteerPWM.ValueChanged -= HsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged -= HsbarMinPWM_ValueChanged;

            hsbarLowSteerPWM.Value = Properties.Vehicle.Default.setAS_lowSteerPWM;
            lblLowSteerPWM.Text = (mf.mc.Config_AutoSteer[mf.mc.ssLowPWM]).ToString();

            hsbarHighSteerPWM.Value = Properties.Vehicle.Default.setAS_highSteerPWM;
            lblHighSteerPWM.Text = hsbarHighSteerPWM.Value.ToString();

            hsbarLowSteerPWM.ValueChanged += HsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged += HsbarMinPWM_ValueChanged;


            mf.vehicle.maxSteerAngle = Properties.Vehicle.Default.setVehicle_maxSteerAngle;
            hsbarMaxSteerAngle.Value = (Int16)mf.vehicle.maxSteerAngle;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();

            mf.vehicle.goalPointLookAheadSeconds = Properties.Vehicle.Default.setVehicle_goalPointLookAhead;
            hsbarLookAhead.Value = (Int16)(mf.vehicle.goalPointLookAheadSeconds * 10);
            lblLookAhead.Text = mf.vehicle.goalPointLookAheadSeconds.ToString();

            mf.vehicle.goalPointLookAheadMinimumDistance = Properties.Vehicle.Default.setVehicle_lookAheadMinimum;
            hsbarLookAheadMin.Value = (Int16)(mf.vehicle.goalPointLookAheadMinimumDistance * 10);
            lblLookAheadMinimum.Text = mf.vehicle.goalPointLookAheadMinimumDistance.ToString();

            mf.vehicle.goalPointDistanceMultiplier = Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine;
            hsbarDistanceFromLine.Value = (Int16)(mf.vehicle.goalPointDistanceMultiplier * 10);
            lblDistanceFromLine.Text = (mf.vehicle.goalPointDistanceMultiplier + 1).ToString();

            hsbarLookAheadUturnMult.Value = (Int16)(mf.vehicle.goalPointLookAheadUturnMult * 10);
            lblLookAheadUturnMult.Text = mf.vehicle.goalPointLookAheadUturnMult.ToString();

            mf.vehicle.stanleyGain = Properties.Vehicle.Default.setVehicle_stanleyGain;
            hsbarStanleyGain.Value = (Int16)(mf.vehicle.stanleyGain * 10);
            lblStanleyGain.Text = mf.vehicle.stanleyGain.ToString();

            mf.vehicle.stanleyHeadingErrorGain = Properties.Vehicle.Default.setVehicle_stanleyHeadingErrorGain;
            hsbarHeadingErrorGain.Value = (Int16)(mf.vehicle.stanleyHeadingErrorGain * 10);
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();

            //make sure free drive is off
            btnFreeDrive.BackColor = Color.Red;
            mf.TestAutoSteer = false;
            btnSteerAngleDown.Enabled = false;
            btnSteerAngleUp.Enabled = false;
            //hSBarFreeDrive.Value = 0;
            mf.TestAutoSteerAngle = 0;

            if (mf.isStanleyUsed) btnStanley.Text = "Stanley";
            else btnStanley.Text = "Pure P";

            toSend = false;
        }

        private void FormSteer_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.TestAutoSteer = false;
            Properties.Vehicle.Default.Save();
        }

        //Stanley Page tab
        private void HsbarStanleyGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyGain = hsbarStanleyGain.Value * 0.1;
            lblStanleyGain.Text = mf.vehicle.stanleyGain.ToString();
            Properties.Vehicle.Default.setVehicle_stanleyGain = mf.vehicle.stanleyGain;
        }

        private void HsbarHeadingErrorGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyHeadingErrorGain = hsbarHeadingErrorGain.Value * 0.1;
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();
            Properties.Vehicle.Default.setVehicle_stanleyHeadingErrorGain = mf.vehicle.stanleyHeadingErrorGain;
        }

        //Scrollbars
        private void HsbarLookAhead_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadSeconds = hsbarLookAhead.Value * 0.1;
            lblLookAhead.Text = mf.vehicle.goalPointLookAheadSeconds.ToString();
            Properties.Vehicle.Default.setVehicle_goalPointLookAhead = mf.vehicle.goalPointLookAheadSeconds;
            //mf.AutoSteerSettingsOutToPort();
        }

        private void HsbarLookAheadUturnMult_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadUturnMult = hsbarLookAheadUturnMult.Value * 0.1;
            lblLookAheadUturnMult.Text = mf.vehicle.goalPointLookAheadUturnMult.ToString();
            Properties.Vehicle.Default.setVehicle_goalPointLookAheadUturnMult = mf.vehicle.goalPointLookAheadUturnMult;
        }

        private void HsbarDistanceFromLine_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointDistanceMultiplier = hsbarDistanceFromLine.Value * 0.1;
            lblDistanceFromLine.Text = (mf.vehicle.goalPointDistanceMultiplier + 1).ToString();
            Properties.Vehicle.Default.setVehicle_lookAheadDistanceFromLine = mf.vehicle.goalPointDistanceMultiplier;
        }

        private void HsbarLookAheadMin_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadMinimumDistance = hsbarLookAheadMin.Value * 0.1;
            lblLookAheadMinimum.Text = mf.vehicle.goalPointLookAheadMinimumDistance.ToString();
            Properties.Vehicle.Default.setVehicle_lookAheadMinimum = mf.vehicle.goalPointLookAheadMinimumDistance;
        }

        private void HsbarCountsPerDegree_ValueChanged(object sender, EventArgs e)
        {
            mf.mc.Config_AutoSteer[mf.mc.ssCountsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
            lblCountsPerDegree.Text = (mf.mc.Config_AutoSteer[mf.mc.ssCountsPerDegree]).ToString();
            Properties.Vehicle.Default.setAS_countsPerDegree = mf.mc.Config_AutoSteer[mf.mc.ssCountsPerDegree];
            toSend = true;
        }

        private void HsbarMaxSteerAngle_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.maxSteerAngle = hsbarMaxSteerAngle.Value;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();
            Properties.Vehicle.Default.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle;
        }

        private void HsbarSteerAngleSensorZero_ValueChanged(object sender, EventArgs e)
        {
            lblSteerAngleSensorZero.Text = hsbarSteerAngleSensorZero.Value.ToString();
            mf.mc.Config_AutoSteer[mf.mc.ssSteerOffset] = unchecked((byte)(127 + hsbarSteerAngleSensorZero.Value));
            Properties.Vehicle.Default.setAS_steerAngleOffset = mf.mc.Config_AutoSteer[mf.mc.ssSteerOffset];
            toSend = true;
        }

        //Stanley Parameters

        private void HsbarMinPWM_ValueChanged(object sender, EventArgs e)
        {
            mf.mc.Config_AutoSteer[mf.mc.ssMinPWM] = unchecked((byte)hsbarMinPWM.Value);
            lblMinPWM.Text = (mf.mc.Config_AutoSteer[mf.mc.ssMinPWM]).ToString();
            Properties.Vehicle.Default.setAS_minSteerPWM = mf.mc.Config_AutoSteer[mf.mc.ssMinPWM];
            toSend = true;
        }

        private void HsbarProportionalGain_ValueChanged(object sender, EventArgs e)
        {
            mf.mc.Config_AutoSteer[mf.mc.ssKp] = unchecked((byte)hsbarProportionalGain.Value);
            lblProportionalGain.Text = (mf.mc.Config_AutoSteer[mf.mc.ssKp]).ToString();
            Properties.Vehicle.Default.setAS_Kp = mf.mc.Config_AutoSteer[mf.mc.ssKp];
            toSend = true;
        }

        private void HsbarSidehillDraftGain_ValueChanged(object sender, EventArgs e)
        {
            mf.mc.Config_AutoSteer[mf.mc.ssKd] = unchecked((byte)hsbarSidehillDraftGain.Value);
            lblSidehillDraftGain.Text = (mf.mc.Config_AutoSteer[mf.mc.ssKd]).ToString();
            Properties.Vehicle.Default.setAS_Kd = mf.mc.Config_AutoSteer[mf.mc.ssKd];
            toSend = true;
        }

        private void HsbarLowSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarHighSteerPWM.Value = hsbarLowSteerPWM.Value;
            mf.mc.Config_AutoSteer[mf.mc.ssLowPWM] = unchecked((byte)hsbarLowSteerPWM.Value);
            lblLowSteerPWM.Text = (mf.mc.Config_AutoSteer[mf.mc.ssLowPWM]).ToString();
            Properties.Vehicle.Default.setAS_lowSteerPWM = mf.mc.Config_AutoSteer[mf.mc.ssLowPWM];
            toSend = true;
        }

        private void HsbarHighSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarLowSteerPWM.Value = hsbarHighSteerPWM.Value;
            mf.mc.Config_AutoSteer[mf.mc.ssHighPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
            lblHighSteerPWM.Text = (mf.mc.Config_AutoSteer[mf.mc.ssHighPWM]).ToString();
            Properties.Vehicle.Default.setAS_highSteerPWM = mf.mc.Config_AutoSteer[mf.mc.ssHighPWM];
            toSend = true;
        }

        //FREE DRIVE SECTION

        //private void hSBarFreeDrive_ValueChanged(object sender, EventArgs e)
        //{
        //    mf.ast.driveFreeSteerAngle = (Int16)hSBarFreeDrive.Value;
        //}

        private void BtnFreeDrive_Click(object sender, EventArgs e)
        {
            if (mf.TestAutoSteer)
            {
                //turn OFF free drive mode
                btnFreeDrive.BackColor = Color.Red;
                mf.TestAutoSteer = false;
                btnSteerAngleDown.Enabled = false;
                btnSteerAngleUp.Enabled = false;
                //hSBarFreeDrive.Value = 0;
                mf.TestAutoSteerAngle = 0;
            }
            else
            {
                //turn ON free drive mode
                btnFreeDrive.BackColor = Color.LimeGreen;
                mf.TestAutoSteer = true;
                btnSteerAngleDown.Enabled = true;
                btnSteerAngleUp.Enabled = true;
                //hSBarFreeDrive.Value = 0;
                mf.TestAutoSteerAngle = 0;
                lblSteerAngle.Text = "0";
            }
        }

        private void BtnFreeDriveZero_Click(object sender, EventArgs e)
        {
            if (mf.TestAutoSteerAngle == 0)
                mf.TestAutoSteerAngle = 5;
            else mf.TestAutoSteerAngle = 0;
            //hSBarFreeDrive.Value = mf.ast.driveFreeSteerAngle;
        }


        private void BtnChart_Click(object sender, EventArgs e)
        {
            mf.steerChartStripMenu.PerformClick();
        }

        private void BtnStanley_Click(object sender, EventArgs e)
        {
            mf.isStanleyUsed = !mf.isStanleyUsed;
            if (mf.isStanleyUsed) btnStanley.Text = "Stanley";
            else btnStanley.Text = "Pure P";
            Properties.Vehicle.Default.setVehicle_isStanleyUsed = mf.isStanleyUsed;
            Properties.Vehicle.Default.Save();
        }

        int counter = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblSteerAngle.Text = mf.SetSteerAngle;
            lblSteerAngleActual.Text = (mf.actualSteerAngleDisp*0.01).ToString("N1") + "\u00B0";

            double err = (mf.actualSteerAngleDisp * 0.01 - mf.guidanceLineSteerAngle * 0.01);
            lblError.Text = Math.Abs(err).ToString("N1") + "\u00B0";
            if (err > 0) lblError.ForeColor = Color.DarkRed;
            else lblError.ForeColor = Color.DarkGreen;
            
            lblPWMDisplay.Text = mf.mc.pwmDisplay.ToString();

            counter++;
            if (toSend && counter > 6)
            {
                Properties.Vehicle.Default.Save();
                mf.DataSend[8] = "Auto Steer: Config Steer Settings";
                mf.SendData(mf.mc.Config_AutoSteer, true);
                toSend = false;
                counter = 0;
            }

            if (mf.checksumSent - mf.checksumRecd == 0)
            {
                lblSent.BackColor = Color.LightGreen;
                lblRecd.BackColor = Color.LightGreen;
            }
            else
            {
                lblSent.BackColor = Color.Salmon;
                lblRecd.BackColor = Color.Salmon;
            }

            lblSent.Text = mf.checksumSent.ToString();
            lblRecd.Text = mf.checksumRecd.ToString();

            if (hsbarMinPWM.Value > hsbarLowSteerPWM.Value) lblMinPWM.ForeColor = Color.Red;
            else lblMinPWM.ForeColor = Color.Black;
        }

        private void BtnSteerAngleUp_MouseDown(object sender, MouseEventArgs e)
        {
            mf.TestAutoSteerAngle++;
            if (mf.TestAutoSteerAngle > 40) mf.TestAutoSteerAngle = 40;
        }

        private void BtnSteerAngleDown_MouseDown(object sender, MouseEventArgs e)
        {
            mf.TestAutoSteerAngle--;
            if (mf.TestAutoSteerAngle < -40) mf.TestAutoSteerAngle = -40;
        }
    }
}
