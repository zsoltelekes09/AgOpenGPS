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
            this.Text = gStr.gsModuleConfiguration;

            //Steer Tab
            tabAutoSteer.Text = gStr.gsAutoSteer;
            label1.Text = gStr.gsMotorDriver;
            label6.Text = gStr.gsMMAAxis;
            label3.Text = gStr.gsA2DConvertor;
            label5.Text = gStr.gsSteerEnable;
            label8.Text = gStr.gsMinSpeed;
            label4.Text = gStr.gsMaxSpeed;
            label2.Text = gStr.gsInclinometer;
            chkInvertWAS.Text = gStr.gsInvertWAS;
            chkInvertSteer.Text = gStr.gsInvertSteerDirection;
            chkInvertRoll.Text = gStr.gsInvertRoll;
            chkBNOInstalled.Text = gStr.gsBNOInstalled;
            cboxEncoder.Text = gStr.gsEncoder;
            label7.Text = gStr.gsEncoderCounts;
            label9.Text = gStr.gsSendToModule;
            groupBox5.Text = gStr.gsToAutoSteer;
            cboxIsSendMachineControlToAutoSteer.Text = gStr.gsMachinePGN;
            groupBox1.Text = gStr.gsHydraulicToolLift;

            chkWorkSwitchManual.Text = gStr.gsWorkSwitchControlsManual;
            chkWorkSwActiveLow.Text = gStr.gsActiveLow;
            chkEnableWorkSwitch.Text = gStr.gsEnableWorkSwitch;
            chkRemoteAutoSteerButton.Text = gStr.gsAutoManualAutosteerBtn;

            //Machine tab
            label10.Text = gStr.gsRaiseTime;
            label11.Text = gStr.gsLowerTime;
            cboxIsHydOn.Text = gStr.gsEnableHydraulics;
            tabMachine.Text = gStr.gsMachine;
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

            TboxMinSpeed.Text = Math.Round(minspeed = Properties.Vehicle.Default.setArdSteer_minSpeed / 5 / mf.cutoffMetricImperial,2).ToString("0.0");
            TboxMaxSpeed.Text = Math.Round(maxspeed = Properties.Vehicle.Default.setArdSteer_maxSpeed / 5 / mf.cutoffMetricImperial,2).ToString("0.0");
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

            cboxIsSendMachineControlToAutoSteer.Checked = Properties.Vehicle.Default.setVehicle_isMachineControlToAutoSteer;
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

            Properties.Vehicle.Default.setArdSteer_setting0 = (byte)sett;

            sett = 0;

            if (chkBNOInstalled.Checked) sett |= 1;
            if (cboxSteerInvertRelays.Checked) sett |= 2;

            if (chkWorkSwActiveLow.Checked) sett |= 4;
            if (chkWorkSwitchManual.Checked) sett |= 8;

            Properties.Vehicle.Default.setArdSteer_setting1 = (byte)sett;

            Properties.Vehicle.Default.setArdSteer_minSpeed = (byte)Math.Round(minspeed * 5 * mf.cutoffMetricImperial, 2);
            Properties.Vehicle.Default.setArdSteer_maxSpeed = (byte)Math.Round(maxspeed * 5 * mf.cutoffMetricImperial, 2);
            
            Properties.Vehicle.Default.setArdSteer_maxPulseCounts = maxpulsecounts;
            Properties.Vehicle.Default.setArdSteer_ackermanFix = ackerman;

            mf.mc.isMachineDataSentToAutoSteer = cboxIsSendMachineControlToAutoSteer.Checked;
            Properties.Vehicle.Default.setVehicle_isMachineControlToAutoSteer = mf.mc.isMachineDataSentToAutoSteer;

            Properties.Vehicle.Default.Save();

            mf.mc.Config_ardSteer[mf.mc.arSet0] = Properties.Vehicle.Default.setArdSteer_setting0;
            mf.mc.Config_ardSteer[mf.mc.arSet1] = Properties.Vehicle.Default.setArdSteer_setting1;
            mf.mc.Config_ardSteer[mf.mc.arMaxSpd] = Properties.Vehicle.Default.setArdSteer_maxSpeed;
            mf.mc.Config_ardSteer[mf.mc.arMinSpd] = Properties.Vehicle.Default.setArdSteer_minSpeed;
            mf.mc.Config_ardSteer[mf.mc.arAckermanFix] = Properties.Vehicle.Default.setArdSteer_ackermanFix;

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
                mf.DataSend[8] = "Auto Steer: Config Settings";
                mf.SendData(mf.mc.Config_ardSteer, true);
            }
            else if (tabcArduino.SelectedTab.Name == "tabMachine")
            {
                mf.DataSend[8] = "Auto Steer: Config Hydraulic Lift";
                mf.SendData(mf.mc.Config_ardMachine, false);
            }
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            tboxSerialFromAutoSteer.Text = mf.mc.serialRecvAutoSteerStr;
            tboxSerialFromMachine.Text = mf.mc.serialRecvMachineStr;
            if (Properties.Settings.Default.setUDP_isOn)
            {
                tboxSerialFromAutoSteer.Text = "UDP";
                tboxSerialFromMachine.Text = "UDP";
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
        }

        private void TboxMinSpeed_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(50 / mf.cutoffMetricImperial, mf.decimals), minspeed, this, false, 1, 0.2M))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMinSpeed.Text = (minspeed = Math.Round(form.ReturnValue,2)).ToString("0.0");
                }
            }
            btnCancel.Focus();
        }

        private void TboxLowerTime_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 20, lowertime, this, false, 1))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxLowerTime.Text = (lowertime = Math.Round(form.ReturnValue, 2)).ToString("0.0");
                }
            }
            btnCancel.Focus();
        }

        private void TboxRaiseTime_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(1, 20, raisetime, this, false,1))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxRaiseTime.Text = (raisetime = Math.Round(form.ReturnValue, 2)).ToString("0.0");
                }
            }
            btnCancel.Focus();
        }

        private void TboxMaxSpeed_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(0, Math.Round(50 / mf.cutoffMetricImperial, mf.decimals), maxspeed, this, false, 1, 0.2M))
            {
                var result = form.ShowDialog(this);
                if (result == DialogResult.OK)
                {
                    TboxMaxSpeed.Text = (maxspeed = Math.Round(form.ReturnValue, 2)).ToString("0.0");
                }
            }
            btnCancel.Focus();
        }

        private void TboxAckerman_Enter(object sender, EventArgs e)
        {
            using (var form = new FormNumeric(50, 200, ackerman, this, true, 0))
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
            using (var form = new FormNumeric(1, 60, maxpulsecounts, this, true, 0))
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