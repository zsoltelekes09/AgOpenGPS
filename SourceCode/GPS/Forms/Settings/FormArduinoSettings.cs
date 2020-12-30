//Please, if you use this, share the improvements

using System;
using System.Drawing;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormArduinoSettings : Form
    {
        //class variables
        private readonly FormGPS mf;
        private byte ackerman = 0, maxpulsecounts = 0;
        private double minspeed = 0, maxspeed = 0, raisetime = 0, lowertime = 0;
        //constructor
        public FormArduinoSettings(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();

            //Language keys
            this.Text = String.Get("gsModuleConfiguration");

            //Steer Tab
            tabAutoSteer.Text = String.Get("gsAutoSteer");
            label1.Text = String.Get("gsMotorDriver");
            label6.Text = String.Get("gsMMAAxis");
            label3.Text = String.Get("gsA2DConvertor");
            label5.Text = String.Get("gsSteerEnable");
            label8.Text = String.Get("gsMinSpeed");
            label4.Text = String.Get("gsMaxSpeed");
            label2.Text = String.Get("gsInclinometer");
            chkInvertWAS.Text = String.Get("gsInvertWAS");
            chkInvertSteer.Text = String.Get("gsInvertSteerDirection");
            chkInvertRoll.Text = String.Get("gsInvertRoll");
            chkBNOInstalled.Text = String.Get("gsBNOInstalled");
            cboxEncoder.Text = String.Get("gsEncoder");
            label7.Text = String.Get("gsEncoderCounts");
            label9.Text = String.Get("gsSendToModule");
            groupBox1.Text = String.Get("gsHydraulicToolLift");

            chkWorkSwitchManual.Text = String.Get("gsWorkSwitchControlsManual");
            chkWorkSwActiveLow.Text = String.Get("gsActiveLow");
            chkEnableWorkSwitch.Text = String.Get("gsEnableWorkSwitch");
            chkRemoteAutoSteerButton.Text = String.Get("gsAutoManualAutosteerBtn");

            //Machine tab
            label10.Text = String.Get("gsRaiseTime");
            label11.Text = String.Get("gsLowerTime");
            cboxIsHydOn.Text = String.Get("gsEnableHydraulics");
            tabMachine.Text = String.Get("gsMachine");
        }

        //do any field initializing for form here
        private void FormToolSettings_Load(object sender, EventArgs e)
        {
            int sett = Properties.Vehicle.Default.setArdSteer_setting0;

            if ((sett & 1) == 0) chkInvertWAS.Checked = false;
            else chkInvertWAS.Checked = true;

            if ((sett & 2) == 0) chkInvertRoll.Checked = false;
            else chkInvertRoll.Checked = true;

            if ((sett & 4) == 0) chkInvertSteer.Checked = false;
            else chkInvertSteer.Checked = true;

            if ((sett & 8) == 0) cboxConv.Text = "Differential";
            else cboxConv.Text = "Single";

            if ((sett & 16) == 0) cboxMotorDrive.Text = "IBT2";
            else cboxMotorDrive.Text = "Cytron";

            if ((sett & 32) == 0) cboxSteerEnable.Text = "Button";
            else cboxSteerEnable.Text = "Switch";

            if ((sett & 64) == 0) cboxMMAAxis.Text = "Y Axis";
            else cboxMMAAxis.Text = "X Axis";

            if ((sett & 128) == 0) cboxEncoder.Checked = false;
            else cboxEncoder.Checked = true;

            sett = Properties.Vehicle.Default.setArdSteer_setting1;

            if ((sett & 1) == 0) chkBNOInstalled.Checked = false;
            else chkBNOInstalled.Checked = true;

            if ((sett & 2) == 0) cboxSteerInvertRelays.Checked = false;
            else cboxSteerInvertRelays.Checked = true;

            //inclinometer
            byte inc = Properties.Vehicle.Default.setArdSteer_inclinometer;
            switch (inc)
            {
                case 0:
                    cboxInclinometer.Text = "None";
                    break;
                case 1:
                    cboxInclinometer.Text = "DOGS2";
                    break;
                case 2:
                    cboxInclinometer.Text = "MMA (1C)";
                    break;
                case 3:
                    cboxInclinometer.Text = "MMA (1D)";
                    break;

                default:
                    cboxInclinometer.Text = "Error";
                    break;
            }

            TboxMinSpeed.Text = ((minspeed = Properties.Vehicle.Default.setArdSteer_minSpeed / 5) * mf.Kmh2Unit).ToString(mf.GuiFix);
            TboxMaxSpeed.Text = ((maxspeed = Properties.Vehicle.Default.setArdSteer_maxSpeed / 5) * mf.Kmh2Unit).ToString(mf.GuiFix);
            TboxAckerman.Text = (ackerman = Properties.Vehicle.Default.setArdSteer_ackermanFix).ToString();
            TboxMaxSensorCounts.Text = (maxpulsecounts = Properties.Vehicle.Default.setArdSteer_maxPulseCounts).ToString();

            chkWorkSwActiveLow.Checked = Properties.Vehicle.Default.setF_IsWorkSwitchActiveLow;
            chkEnableWorkSwitch.Checked = Properties.Vehicle.Default.setF_IsWorkSwitchEnabled;
            chkWorkSwitchManual.Checked = Properties.Vehicle.Default.setF_IsWorkSwitchManual;
            chkRemoteAutoSteerButton.Checked = Properties.Vehicle.Default.setAS_isAutoSteerAutoOn;

            //Machine --------------------------------------------------------------------------------------------
            sett = Properties.Vehicle.Default.setArdMac_setting0;

            if ((sett & 1) == 0) cboxMachInvertRelays.Checked = false;
            else cboxMachInvertRelays.Checked = true;


            raisetime = Properties.Vehicle.Default.setArdMac_hydRaiseTime;
            TboxRaiseTime.Text = (raisetime /= 10).ToString();
            lowertime = Properties.Vehicle.Default.setArdMac_hydLowerTime;
            TboxLowerTime.Text = (lowertime /= 10).ToString();
            cboxIsHydOn.Checked = Properties.Vehicle.Default.setArdMac_isHydEnabled > 0;
        }

        private void BtnOK_Click(object sender, EventArgs e)
        {
            SaveSettings();
            DialogResult = DialogResult.OK;
            Close();
        }

        private void SaveSettings()
        {
            switch (cboxInclinometer.Text)
            {
                case "None":
                    Properties.Vehicle.Default.setArdSteer_inclinometer = 0;
                    break;

                case "DOGS2":
                    Properties.Vehicle.Default.setArdSteer_inclinometer = 1;
                    break;

                case "MMA (1C)":
                    Properties.Vehicle.Default.setArdSteer_inclinometer = 2;
                    break;

                case "MMA (1D)":
                    Properties.Vehicle.Default.setArdSteer_inclinometer = 3;
                    break;

                default:
                    Properties.Vehicle.Default.setArdSteer_inclinometer = 0;
                    break;
            }

            int sett = 0;

            if (chkInvertWAS.Checked) sett |= 1;
            if (chkInvertRoll.Checked) sett |= 2;
            if (chkInvertSteer.Checked) sett |= 4;
            if (cboxConv.Text == "Single") sett |= 8;
            if (cboxMotorDrive.Text == "Cytron") sett |= 16;
            if (cboxSteerEnable.Text == "Switch") sett |= 32;
            if (cboxMMAAxis.Text == "X Axis") sett |= 64;
            if (cboxEncoder.Checked) sett |= 128;

            Properties.Vehicle.Default.setArdSteer_setting0 = mf.mc.Config_ardSteer[mf.mc.arSet0] = (byte)sett;

            sett = 0;

            if (chkBNOInstalled.Checked) sett |= 1;
            if (cboxSteerInvertRelays.Checked) sett |= 2;

            if (chkWorkSwActiveLow.Checked) sett |= 4;
            if (chkWorkSwitchManual.Checked) sett |= 8;

            Properties.Vehicle.Default.setArdSteer_setting1 = mf.mc.Config_ardSteer[mf.mc.arSet1] = (byte)sett;

            Properties.Vehicle.Default.setArdSteer_minSpeed = mf.mc.Config_ardSteer[mf.mc.arMinSpd] = (byte)(minspeed * 5);
            Properties.Vehicle.Default.setArdSteer_maxSpeed = mf.mc.Config_ardSteer[mf.mc.arMaxSpd] = (byte)(maxspeed * 5);
            
            Properties.Vehicle.Default.setArdSteer_maxPulseCounts = maxpulsecounts;
            Properties.Vehicle.Default.setArdSteer_ackermanFix = mf.mc.Config_ardSteer[mf.mc.arAckermanFix] = ackerman;

            Properties.Vehicle.Default.Save();

            byte inc = (byte)(Properties.Vehicle.Default.setArdSteer_inclinometer << 6);            
            mf.mc.Config_ardSteer[mf.mc.arIncMaxPulse] = (byte)(inc + (byte)Properties.Vehicle.Default.setArdSteer_maxPulseCounts);

            //WorkSwitch settings

            Properties.Vehicle.Default.setF_IsWorkSwitchActiveLow = mf.mc.isWorkSwitchActiveLow = chkWorkSwActiveLow.Checked;
            Properties.Vehicle.Default.setF_IsWorkSwitchEnabled = mf.mc.isWorkSwitchEnabled = chkEnableWorkSwitch.Checked;
            Properties.Vehicle.Default.setF_IsWorkSwitchManual = mf.mc.isWorkSwitchManual = chkWorkSwitchManual.Checked;
            Properties.Vehicle.Default.setAS_isAutoSteerAutoOn = mf.mc.RemoteAutoSteer = chkRemoteAutoSteerButton.Checked;

            //Machine ---------------------------------------------------------------------------------------------------

            sett = 0;
            if (cboxMachInvertRelays.Checked) sett |= 1;

            Properties.Vehicle.Default.setArdMac_setting0 = (byte)sett;
            mf.mc.Config_ardMachine[mf.mc.amSet0] = (byte)sett;

            Properties.Vehicle.Default.setArdMac_hydRaiseTime = (byte)(raisetime * 10);
            mf.mc.Config_ardMachine[mf.mc.amRaiseTime] = (byte)(raisetime * 10);

            Properties.Vehicle.Default.setArdMac_hydLowerTime = (byte)(lowertime * 10);
            mf.mc.Config_ardMachine[mf.mc.amLowerTime] = (byte)(lowertime * 10);

            Properties.Vehicle.Default.setArdMac_isHydEnabled = (byte)(cboxIsHydOn.Checked ? 1 : 0);
            mf.mc.Config_ardMachine[mf.mc.amEnableHyd] = Properties.Vehicle.Default.setArdMac_isHydEnabled;
        }

        private void BtnSendToSteerArduino_Click(object sender, EventArgs e)
        {
            SaveSettings();

            if (tabcArduino.SelectedTab.Name == "tabAutoSteer")
            {
                mf.UpdateSendDataText("Auto Steer: Config Settings");
                mf.SendData(mf.mc.Config_ardSteer, true);
            }
            else if (tabcArduino.SelectedTab.Name == "tabMachine")
            {
                mf.UpdateSendDataText("Auto Steer: Config Hydraulic Lift");
                mf.SendData(mf.mc.Config_ardMachine, false);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
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
        }

        private void TboxMinSpeed_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 50, minspeed, this, 1, true, mf.Unit2Kmh, mf.Kmh2Unit, 0.2M))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMinSpeed.Text = ((minspeed = form.ReturnValue) * mf.Kmh2Unit).ToString(mf.GuiFix);
                }
            }
            btnCancel.Focus();
        }

        private void TboxLowerTime_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 20, lowertime, this, 1, false))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxLowerTime.Text = (lowertime = form.ReturnValue).ToString("0.0");
                }
            }
            btnCancel.Focus();
        }

        private void TboxRaiseTime_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 20, raisetime, this, 1, false))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxRaiseTime.Text = (raisetime = form.ReturnValue).ToString("0.0");
                }
            }
            btnCancel.Focus();
        }

        private void TboxMaxSpeed_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, 50, maxspeed, this, 1, true, mf.Unit2Kmh, mf.Kmh2Unit, 0.2M))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMaxSpeed.Text = ((maxspeed = form.ReturnValue) * mf.Kmh2Unit).ToString(mf.GuiFix);
                }
            }
            btnCancel.Focus();
        }

        private void TboxAckerman_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(50, 200, ackerman, this, 0, false))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxAckerman.Text = (ackerman = (byte)form.ReturnValue).ToString();
                }
            }
            btnCancel.Focus();
        }

        private void TboxMaxSensorCounts_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 60, maxpulsecounts, this, 0, false))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMaxSensorCounts.Text = (maxpulsecounts = (byte)form.ReturnValue).ToString();
                }
            }
            btnCancel.Focus();
        }
    }
}