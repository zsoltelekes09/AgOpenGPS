using AgOpenGPS.Properties;
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

            this.BtnFreeDrive.Text = String.Get("gsDrive");
            this.tabGain.Text = String.Get("gsGain");
            this.label29.Text = String.Get("gsSidehillDraftGain");
            //this.label22.Text = gStr.gsOutputGain;
            this.label41.Text = String.Get("gsMinimumPWMDrive");
            this.label7.Text = String.Get("gsProportionalGain");
            label1.Text = String.Get("gsChooseType");

            this.tabSteer.Text = String.Get("gsSteer");
            this.label25.Text = String.Get("gsCountsPerDegree");
            //this.label45.Text = gStr.gsMaxIntegralValue;
            this.label19.Text = String.Get("gsMaxSteerAngleInDegrees");
            //this.label33.Text = gStr.gsIntegralGain;
            this.label10.Text = String.Get("gsWheelAngleSensorZero");

            this.tabLook.Text = "Pure P";
            this.label2.Text = String.Get("gsUTurnLookAheadMultiplier");
            this.label37.Text = String.Get("gsLookAheadInSeconds");
            this.label4.Text = String.Get("gsLookAheadOfflineMultiplier");
            this.label6.Text = String.Get("gsMinLookAheadInMeters");

            this.tabStan.Text = "Stanley";
            this.btnChart.Text = String.Get("gsSteerChart");
            this.label3.Text = String.Get("gsAgressiveness");
            this.label5.Text = String.Get("gsOvershootReduction");
            this.Text = String.Get("gsAutoSteerConfiguration");
        }

        private void FormSteer_Load(object sender, EventArgs e)
        {
            hsbarSteerAngleSensorZero.Value = Vehicle.Default.setAS_steerAngleOffset - 127;
            lblSteerAngleSensorZero.Text = hsbarSteerAngleSensorZero.Value.ToString();

            hsbarCountsPerDegree.Value = Vehicle.Default.setAS_countsPerDegree;
            lblCountsPerDegree.Text = hsbarCountsPerDegree.Value.ToString();

            hsbarMinPWM.Value = Vehicle.Default.setAS_minSteerPWM;
            lblMinPWM.Text = hsbarMinPWM.Value.ToString();

            hsbarProportionalGain.Value = Vehicle.Default.setAS_Kp;
            lblProportionalGain.Text = hsbarProportionalGain.Value.ToString();

            hsbarSidehillDraftGain.Value = Vehicle.Default.setAS_Kd;
            lblSidehillDraftGain.Text = hsbarSidehillDraftGain.Value.ToString();

            hsbarLowSteerPWM.ValueChanged -= HsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged -= HsbarMinPWM_ValueChanged;

            hsbarLowSteerPWM.Value = Vehicle.Default.setAS_lowSteerPWM;
            lblLowSteerPWM.Text = (mf.mc.Config_AutoSteer[mf.mc.ssLowPWM]).ToString();

            hsbarHighSteerPWM.Value = Vehicle.Default.setAS_highSteerPWM;
            lblHighSteerPWM.Text = hsbarHighSteerPWM.Value.ToString();

            hsbarLowSteerPWM.ValueChanged += HsbarLowSteerPWM_ValueChanged;
            hsbarHighSteerPWM.ValueChanged += HsbarMinPWM_ValueChanged;


            mf.vehicle.maxSteerAngle = Vehicle.Default.setVehicle_maxSteerAngle;
            hsbarMaxSteerAngle.Value = (Int16)mf.vehicle.maxSteerAngle;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();

            mf.vehicle.goalPointLookAheadSeconds = Vehicle.Default.setVehicle_goalPointLookAhead;
            hsbarLookAhead.Value = (Int16)(mf.vehicle.goalPointLookAheadSeconds * 10);
            lblLookAhead.Text = mf.vehicle.goalPointLookAheadSeconds.ToString();

            mf.vehicle.goalPointLookAheadMinimumDistance = Vehicle.Default.setVehicle_lookAheadMinimum;
            hsbarLookAheadMin.Value = (Int16)(mf.vehicle.goalPointLookAheadMinimumDistance * 10);
            lblLookAheadMinimum.Text = mf.vehicle.goalPointLookAheadMinimumDistance.ToString();

            mf.vehicle.goalPointDistanceMultiplier = Vehicle.Default.setVehicle_lookAheadDistanceFromLine;
            hsbarDistanceFromLine.Value = (Int16)(mf.vehicle.goalPointDistanceMultiplier * 10);
            lblDistanceFromLine.Text = (mf.vehicle.goalPointDistanceMultiplier + 1).ToString();

            hsbarLookAheadUturnMult.Value = (Int16)(mf.vehicle.goalPointLookAheadUturnMult * 10);
            lblLookAheadUturnMult.Text = mf.vehicle.goalPointLookAheadUturnMult.ToString();

            mf.vehicle.stanleyGain = Vehicle.Default.setVehicle_stanleyGain;
            hsbarStanleyGain.Value = (Int16)(mf.vehicle.stanleyGain * 10);
            lblStanleyGain.Text = mf.vehicle.stanleyGain.ToString();

            mf.vehicle.stanleyHeadingErrorGain = Vehicle.Default.setVehicle_stanleyHeadingErrorGain;
            hsbarHeadingErrorGain.Value = (Int16)(mf.vehicle.stanleyHeadingErrorGain * 10);
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();

            hsbarIntegralGain.Value = (Int16)(mf.vehicle.stanleyIntegralGain * 100);
            lblStanleyIntegralGain.Text = mf.vehicle.stanleyIntegralGain.ToString("N2");

            hsbarAvgXTE.Value = (Int16)(mf.vehicle.avgXTE * 100);
            lblAvgXTE.Text = hsbarAvgXTE.Value.ToString();

            TboxIntHeading.Text = mf.vehicle.integralHeadingLimit.ToString();
            TboxIntDistance.Text = (mf.vehicle.integralDistanceAway * 100).ToString();

            //make sure free drive is off
            BtnFreeDrive.BackColor = Color.Red;
            mf.TestAutoSteer = false;
            //hSBarFreeDrive.Value = 0;
            mf.TestAutoSteerAngle = 0;

            if (mf.isStanleyUsed) btnStanley.Text = "Stanley";
            else btnStanley.Text = "Pure P";

            toSend = false;
        }

        private void FormSteer_FormClosing(object sender, FormClosingEventArgs e)
        {
            mf.TestAutoSteerAngle = 0;
            mf.TestAutoSteer = false;
            Vehicle.Default.Save();
        }

        //Stanley Page tab
        private void HsbarStanleyGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyGain = hsbarStanleyGain.Value * 0.1;
            lblStanleyGain.Text = mf.vehicle.stanleyGain.ToString();
            Vehicle.Default.setVehicle_stanleyGain = mf.vehicle.stanleyGain;
        }

        private void HsbarHeadingErrorGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyHeadingErrorGain = hsbarHeadingErrorGain.Value * 0.1;
            lblHeadingErrorGain.Text = mf.vehicle.stanleyHeadingErrorGain.ToString();
            Vehicle.Default.setVehicle_stanleyHeadingErrorGain = mf.vehicle.stanleyHeadingErrorGain;
        }

        //Scrollbars
        private void HsbarLookAhead_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadSeconds = hsbarLookAhead.Value * 0.1;
            lblLookAhead.Text = mf.vehicle.goalPointLookAheadSeconds.ToString();
            Vehicle.Default.setVehicle_goalPointLookAhead = mf.vehicle.goalPointLookAheadSeconds;
            //mf.AutoSteerSettingsOutToPort();
        }

        private void HsbarLookAheadUturnMult_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadUturnMult = hsbarLookAheadUturnMult.Value * 0.1;
            lblLookAheadUturnMult.Text = mf.vehicle.goalPointLookAheadUturnMult.ToString();
            Vehicle.Default.setVehicle_goalPointLookAheadUturnMult = mf.vehicle.goalPointLookAheadUturnMult;
        }

        private void HsbarDistanceFromLine_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointDistanceMultiplier = hsbarDistanceFromLine.Value * 0.1;
            lblDistanceFromLine.Text = (mf.vehicle.goalPointDistanceMultiplier + 1).ToString();
            Vehicle.Default.setVehicle_lookAheadDistanceFromLine = mf.vehicle.goalPointDistanceMultiplier;
        }

        private void HsbarLookAheadMin_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.goalPointLookAheadMinimumDistance = hsbarLookAheadMin.Value * 0.1;
            lblLookAheadMinimum.Text = mf.vehicle.goalPointLookAheadMinimumDistance.ToString();
            Vehicle.Default.setVehicle_lookAheadMinimum = mf.vehicle.goalPointLookAheadMinimumDistance;
        }

        private void HsbarCountsPerDegree_ValueChanged(object sender, EventArgs e)
        {
            mf.mc.Config_AutoSteer[mf.mc.ssCountsPerDegree] = unchecked((byte)hsbarCountsPerDegree.Value);
            lblCountsPerDegree.Text = (mf.mc.Config_AutoSteer[mf.mc.ssCountsPerDegree]).ToString();
            Vehicle.Default.setAS_countsPerDegree = mf.mc.Config_AutoSteer[mf.mc.ssCountsPerDegree];
            toSend = true;
        }

        private void HsbarMaxSteerAngle_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.maxSteerAngle = hsbarMaxSteerAngle.Value;
            lblMaxSteerAngle.Text = hsbarMaxSteerAngle.Value.ToString();
            Vehicle.Default.setVehicle_maxSteerAngle = mf.vehicle.maxSteerAngle;
        }

        private void HsbarSteerAngleSensorZero_ValueChanged(object sender, EventArgs e)
        {
            lblSteerAngleSensorZero.Text = hsbarSteerAngleSensorZero.Value.ToString();
            mf.mc.Config_AutoSteer[mf.mc.ssSteerOffset] = unchecked((byte)(127 + hsbarSteerAngleSensorZero.Value));
            Vehicle.Default.setAS_steerAngleOffset = mf.mc.Config_AutoSteer[mf.mc.ssSteerOffset];
            toSend = true;
        }

        //Stanley Parameters

        private void HsbarMinPWM_ValueChanged(object sender, EventArgs e)
        {
            mf.mc.Config_AutoSteer[mf.mc.ssMinPWM] = unchecked((byte)hsbarMinPWM.Value);
            lblMinPWM.Text = (mf.mc.Config_AutoSteer[mf.mc.ssMinPWM]).ToString();
            Vehicle.Default.setAS_minSteerPWM = mf.mc.Config_AutoSteer[mf.mc.ssMinPWM];
            toSend = true;
        }

        private void HsbarProportionalGain_ValueChanged(object sender, EventArgs e)
        {
            mf.mc.Config_AutoSteer[mf.mc.ssKp] = unchecked((byte)hsbarProportionalGain.Value);
            lblProportionalGain.Text = (mf.mc.Config_AutoSteer[mf.mc.ssKp]).ToString();
            Vehicle.Default.setAS_Kp = mf.mc.Config_AutoSteer[mf.mc.ssKp];
            toSend = true;
        }

        private void HsbarSidehillDraftGain_ValueChanged(object sender, EventArgs e)
        {
            mf.mc.Config_AutoSteer[mf.mc.ssKd] = unchecked((byte)hsbarSidehillDraftGain.Value);
            lblSidehillDraftGain.Text = (mf.mc.Config_AutoSteer[mf.mc.ssKd]).ToString();
            Vehicle.Default.setAS_Kd = mf.mc.Config_AutoSteer[mf.mc.ssKd];
            toSend = true;
        }

        private void HsbarLowSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarHighSteerPWM.Value = hsbarLowSteerPWM.Value;
            mf.mc.Config_AutoSteer[mf.mc.ssLowPWM] = unchecked((byte)hsbarLowSteerPWM.Value);
            lblLowSteerPWM.Text = (mf.mc.Config_AutoSteer[mf.mc.ssLowPWM]).ToString();
            Vehicle.Default.setAS_lowSteerPWM = mf.mc.Config_AutoSteer[mf.mc.ssLowPWM];
            toSend = true;
        }

        private void HsbarHighSteerPWM_ValueChanged(object sender, EventArgs e)
        {
            if (hsbarLowSteerPWM.Value > hsbarHighSteerPWM.Value) hsbarLowSteerPWM.Value = hsbarHighSteerPWM.Value;
            mf.mc.Config_AutoSteer[mf.mc.ssHighPWM] = unchecked((byte)hsbarHighSteerPWM.Value);
            lblHighSteerPWM.Text = (mf.mc.Config_AutoSteer[mf.mc.ssHighPWM]).ToString();
            Vehicle.Default.setAS_highSteerPWM = mf.mc.Config_AutoSteer[mf.mc.ssHighPWM];
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
                BtnFreeDrive.BackColor = Color.Red;
                mf.TestAutoSteer = false;
                BtnSteerAngleDown.Enabled = false;
                BtnSteerAngleUp.Enabled = false;
                BtnPlus20.Enabled = false;
                BtnMinus20.Enabled = false;
                BtnFreeDriveZero.Enabled = false;
                mf.TestAutoSteerAngle = 0;
                lblSteerAngle.Text = mf.SetSteerAngle;
            }
            else
            {
                //turn ON free drive mode
                BtnFreeDrive.BackColor = Color.LimeGreen;
                mf.TestAutoSteer = true;
                BtnSteerAngleDown.Enabled = true;
                BtnSteerAngleUp.Enabled = true;
                BtnPlus20.Enabled = true;
                BtnMinus20.Enabled = true;
                BtnFreeDriveZero.Enabled = true;
                //hSBarFreeDrive.Value = 0;
                mf.TestAutoSteerAngle = 0;
                lblSteerAngle.Text = mf.SetSteerAngle;
            }
        }

        private void BtnFreeDriveZero_Click(object sender, EventArgs e)
        {
            if (mf.TestAutoSteerAngle == 0)
                mf.TestAutoSteerAngle = 5;
            else mf.TestAutoSteerAngle = 0;
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
            Vehicle.Default.setVehicle_isStanleyUsed = mf.isStanleyUsed;
            Vehicle.Default.Save();
        }

        int counter = 0;
        private void Timer1_Tick(object sender, EventArgs e)
        {
            lblSteerAngle.Text = mf.SetSteerAngle;
            lblSteerAngleActual.Text = (mf.actualSteerAngleDisp*0.01).ToString("N1") + "\u00B0";

            double err = (mf.actualSteerAngleDisp * 0.01 - mf.guidanceLineSteerAngle * 0.01);

            if (mf.TestAutoSteer)
                err = (mf.actualSteerAngleDisp * 0.01 - mf.TestAutoSteerAngle);

            lblError.Text = Math.Abs(err).ToString("N1") + "\u00B0";
            if (err > 0) lblError.ForeColor = Color.DarkRed;
            else lblError.ForeColor = Color.DarkGreen;
            
            lblPWMDisplay.Text = mf.mc.pwmDisplay.ToString();

            int bar = (int)(10 * err);
            if (bar > 50) bar = 50;
            if (bar < -50) bar = -50;

            if (bar > 0)
            {
                pbErrPos.Value = bar;
                pbErrNeg.Value = 0;
            }
            else
            {
                pbErrPos.Value = 0;
                pbErrNeg.Value = -bar;
            }

            if (mf.TestAutoSteer)
                bar = (mf.TestAutoSteerAngle * 10);
            else
                bar = (mf.guidanceLineSteerAngle / 10);
            if (bar > 100) bar = 100;
            if (bar < -100) bar = -100;

            if (bar > 0)
            {
                pbSetPos.Value = bar;
                pbSetNeg.Value = 0;
            }
            else
            {
                pbSetPos.Value = 0;
                pbSetNeg.Value = -bar;
            }

            bar = (int)(mf.actualSteerAngleDisp * 0.1);
            if (bar > 100) bar = 100;
            if (bar < -100) bar = -100;

            if (bar > 0)
            {
                pbActPos.Value = bar;
                pbActNeg.Value = 0;
            }
            else
            {
                pbActPos.Value = 0;
                pbActNeg.Value = -bar;
            }

            counter++;
            if (toSend && counter > 6)
            {
                Vehicle.Default.Save();
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
            byte text1 = mf.checksumSent;
            byte text2 = mf.checksumRecd;
            lblSent.Text = text1.ToString();
            lblRecd.Text = text2.ToString();

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

        private void TboxIntDistance_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 200, mf.vehicle.integralDistanceAway * 100, this, true, 0))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxIntDistance.Text = ((mf.vehicle.integralDistanceAway = Math.Round(form.ReturnValue * 0.01, 2)) * 100).ToString("N0");
                }
            }
            BtnFreeDrive.Focus();
        }

        private void TboxIntHeading_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 20, mf.vehicle.integralHeadingLimit, this, false, 2))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxIntHeading.Text = (mf.vehicle.integralHeadingLimit = Math.Round(form.ReturnValue, 0)).ToString();
                }
            }
            BtnFreeDrive.Focus();
        }
        private void hsbarIntegralGain_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.stanleyIntegralGain = hsbarIntegralGain.Value * 0.01;
            lblStanleyIntegralGain.Text = mf.vehicle.stanleyIntegralGain.ToString("N2");
            Vehicle.Default.setSteer_integralGain = mf.vehicle.stanleyIntegralGain;
        }

        private void hsbarAvgXTE_ValueChanged(object sender, EventArgs e)
        {
            mf.vehicle.avgXTE = hsbarAvgXTE.Value * 0.01;
            lblAvgXTE.Text = hsbarAvgXTE.Value.ToString();
            Vehicle.Default.setSteer_averageXTE = mf.vehicle.avgXTE;
        }

        private void BtnPlus20_Click(object sender, EventArgs e)
        {
            mf.TestAutoSteerAngle += 20;
            if (mf.TestAutoSteerAngle > 40) mf.TestAutoSteerAngle = 40;
        }

        private void BtnMinus20_Click(object sender, EventArgs e)
        {
            mf.TestAutoSteerAngle -= 20;
            if (mf.TestAutoSteerAngle < -40) mf.TestAutoSteerAngle = -40;
        }
    }
}
