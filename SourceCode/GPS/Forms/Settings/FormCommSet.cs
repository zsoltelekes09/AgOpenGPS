using System;
using System.Linq;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormCommSet : Form
    {
        //class variables
        private readonly FormGPS mf;

        //constructor
        public FormCommSet(Form callingForm)
        {
            //get copy of the calling main form
            mf = callingForm as FormGPS;
            InitializeComponent();
            btnOpenSerial.Text = gStr.gsConnect;
            btnOpenSerialArduino.Text = gStr.gsConnect;
            btnOpenSerialAutoSteer.Text = gStr.gsConnect;
            btnCloseSerial.Text = gStr.gsDisconnect;
            btnCloseSerialArduino.Text = gStr.gsDisconnect;
            btnCloseSerialAutoSteer.Text = gStr.gsDisconnect;
            btnRescan.Text = gStr.gsRescanPorts;

            label3.Text = gStr.gsToAutoSteer;
            label6.Text = gStr.gsFromAutoSteer;
            label2.Text = gStr.gsToMachinePort;
            label15.Text = gStr.gsFromMachinePort;

            groupBox1.Text = gStr.gsGPSPort;
            groupBox3.Text = gStr.gsAutoSteerPort;
            groupBox2.Text = gStr.gsMachinePort;

            lblCurrentArduinoPort.Text = gStr.gsPort;
            lblCurrentPort.Text = gStr.gsPort;
            lblCurrentAutoSteerPort.Text = gStr.gsPort;
            lblCurrentBaud.Text = gStr.gsBaud;
            lblCurrentPort2.Text = gStr.gsPort;
            lblCurrentBaud2.Text = gStr.gsBaud;


        }

        private void FormCommSet_Load(object sender, EventArgs e)
        {

            usejrk.Checked = Properties.Settings.Default.setAS_isJRK  ;

            //check if GPS port is open or closed and set buttons accordingly
            if (mf.SerialGPS.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
            }
            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
            }

            if (mf.SerialGPS2.IsOpen)
            {
                cboxBaud2.Enabled = false;
                cboxPort2.Enabled = false;
                btnCloseSerial2.Enabled = true;
                btnOpenSerial2.Enabled = false;
            }
            else
            {
                cboxBaud2.Enabled = true;
                cboxPort2.Enabled = true;
                btnCloseSerial2.Enabled = false;
                btnOpenSerial2.Enabled = true;
            }

            //check if Arduino port is open or closed and set buttons accordingly
            if (mf.spMachine.IsOpen)
            {
                cboxArdPort.Enabled = false;
                btnCloseSerialArduino.Enabled = true;
                btnOpenSerialArduino.Enabled = false;
            }
            else
            {
                cboxArdPort.Enabled = true;
                btnCloseSerialArduino.Enabled = false;
                btnOpenSerialArduino.Enabled = true;
            }

            //check if AutoSteer port is open or closed and set buttons accordingly
            if (mf.spAutoSteer.IsOpen)
            {
                cboxASPort.Enabled = false;
                btnCloseSerialAutoSteer.Enabled = true;
                btnOpenSerialAutoSteer.Enabled = false;
            }
            else
            {
                cboxASPort.Enabled = true;
                btnCloseSerialAutoSteer.Enabled = false;
                btnOpenSerialAutoSteer.Enabled = true;
            }

            //load the port box with valid port names
            cboxPort.Items.Clear();
            cboxPort2.Items.Clear();
            cboxArdPort.Items.Clear();
            cboxASPort.Items.Clear();
            foreach (String s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxPort.Items.Add(s);
                cboxPort2.Items.Add(s);
                cboxArdPort.Items.Add(s);
                cboxASPort.Items.Add(s);
            }

            lblCurrentBaud.Text = mf.SerialGPS.BaudRate.ToString();
            lblCurrentPort.Text = mf.SerialGPS.PortName;
            lblCurrentBaud2.Text = mf.SerialGPS2.BaudRate.ToString();
            lblCurrentPort2.Text = mf.SerialGPS2.PortName;
            lblCurrentArduinoPort.Text = mf.spMachine.PortName;
            lblCurrentAutoSteerPort.Text = mf.spAutoSteer.PortName;
        }

        #region PortSettings //----------------------------------------------------------------

        //AutoSteer
        private void CboxASPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf.spAutoSteer.PortName = cboxASPort.Text;
            FormGPS.portNameAutoSteer = cboxASPort.Text;
            lblCurrentAutoSteerPort.Text = cboxASPort.Text;
        }

        private void BtnOpenSerialAutoSteer_Click(object sender, EventArgs e)
        {
            mf.SerialPortAutoSteerOpen();
            if (mf.spAutoSteer.IsOpen)
            {
                cboxASPort.Enabled = false;
                btnCloseSerialAutoSteer.Enabled = true;
                btnOpenSerialAutoSteer.Enabled = false;
                lblCurrentAutoSteerPort.Text = mf.spAutoSteer.PortName;
            }
            else
            {
                cboxASPort.Enabled = true;
                btnCloseSerialAutoSteer.Enabled = false;
                btnOpenSerialAutoSteer.Enabled = true;
            }
        }

        private void BtnCloseSerialAutoSteer_Click(object sender, EventArgs e)
        {
            mf.SerialPortAutoSteerClose();
            if (mf.spAutoSteer.IsOpen)
            {
                cboxASPort.Enabled = false;
                btnCloseSerialAutoSteer.Enabled = true;
                btnOpenSerialAutoSteer.Enabled = false;
            }
            else
            {
                cboxASPort.Enabled = true;
                btnCloseSerialAutoSteer.Enabled = false;
                btnOpenSerialAutoSteer.Enabled = true;
            }
        }

        // Arduino
        private void BtnOpenSerialArduino_Click(object sender, EventArgs e)
        {
            mf.SerialPortMachineOpen();
            if (mf.spMachine.IsOpen)
            {
                cboxArdPort.Enabled = false;
                btnCloseSerialArduino.Enabled = true;
                btnOpenSerialArduino.Enabled = false;
                lblCurrentArduinoPort.Text = mf.spMachine.PortName;
            }
            else
            {
                cboxArdPort.Enabled = true;
                btnCloseSerialArduino.Enabled = false;
                btnOpenSerialArduino.Enabled = true;
            }
        }

        private void BtnCloseSerialArduino_Click(object sender, EventArgs e)
        {
            mf.SerialPortMachineClose();
            if (mf.spMachine.IsOpen)
            {
                cboxArdPort.Enabled = false;
                btnCloseSerialArduino.Enabled = true;
                btnOpenSerialArduino.Enabled = false;
            }
            else
            {
                cboxArdPort.Enabled = true;
                btnCloseSerialArduino.Enabled = false;
                btnOpenSerialArduino.Enabled = true;
            }
        }

        private void CboxArdPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            mf.spMachine.PortName = cboxArdPort.Text;
            FormGPS.portNameMachine = cboxArdPort.Text;
            lblCurrentArduinoPort.Text = cboxArdPort.Text;
        }

        // GPS Serial Port
        private void CboxBaud_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            mf.SerialGPS.BaudRate = Convert.ToInt32(cboxBaud.Text);
            FormGPS.baudRateGPS = Convert.ToInt32(cboxBaud.Text);
        }

        private void CboxPort_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            mf.SerialGPS.PortName = cboxPort.Text;
            FormGPS.portNameGPS = cboxPort.Text;
        }


        private void BtnOpenSerial_Click(object sender, EventArgs e)
        {
            //else
            {
                mf.SerialPortOpenGPS();
                if (mf.SerialGPS.IsOpen)
                {
                    cboxBaud.Enabled = false;
                    cboxPort.Enabled = false;
                    btnCloseSerial.Enabled = true;
                    btnOpenSerial.Enabled = false;
                    lblCurrentBaud.Text = mf.SerialGPS.BaudRate.ToString();
                    lblCurrentPort.Text = mf.SerialGPS.PortName;
                }
                else
                {
                    cboxBaud.Enabled = true;
                    cboxPort.Enabled = true;
                    btnCloseSerial.Enabled = false;
                    btnOpenSerial.Enabled = true;
                }
            }
        }

        private void BtnCloseSerial_Click(object sender, EventArgs e)
        {
            mf.SerialPortCloseGPS();
            if (mf.SerialGPS.IsOpen)
            {
                cboxBaud.Enabled = false;
                cboxPort.Enabled = false;
                btnCloseSerial.Enabled = true;
                btnOpenSerial.Enabled = false;
            }
            else
            {
                cboxBaud.Enabled = true;
                cboxPort.Enabled = true;
                btnCloseSerial.Enabled = false;
                btnOpenSerial.Enabled = true;
            }
        }

        private void BtnRescan_Click(object sender, EventArgs e)
        {
            cboxASPort.Items.Clear();
            foreach (String s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxASPort.Items.Add(s);
                cboxArdPort.Items.Add(s);
                cboxPort.Items.Add(s);
                cboxPort2.Items.Add(s);
            }
        }

            #endregion PortSettings //----------------------------------------------------------------

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //GPS phrase
            textBoxRcv.Lines = mf.recvSentenceSettings;

            //RateMachine phrases
            txtBoxRecvArduino.Text = mf.mc.serialRecvMachineStr;
            txtBoxSendArduino.Text = "32762, "
                 + mf.mc.machineData[2] + "," + mf.mc.machineData[3] + "," + mf.mc.machineData[4]//machine hi lo and speed x 4
                 + "," + mf.mc.machineData[5] + "," + mf.mc.machineData[6] + "," + mf.mc.machineData[7]; //setpoint hi lo
            //autoSteer phrases
            txtBoxRecvAutoSteer.Text = mf.mc.serialRecvAutoSteerStr;
            txtBoxSendAutoSteer.Text = "32766, " + mf.mc.autoSteerData[mf.mc.sdSpeed]
                                    + ", " + mf.guidanceLineDistanceOff + ", " + mf.guidanceLineSteerAngle + ", " + mf.mc.machineData[mf.mc.mdUTurn];
        }

        private void BtnSerialOK_Click(object sender, EventArgs e)
        {
            //save
            DialogResult = DialogResult.OK;
            Close();
        }

        private void Usejrk_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.setAS_isJRK = usejrk.Checked;
            Properties.Settings.Default.Save();
            mf.isJRK = Properties.Settings.Default.setAS_isJRK;
        }
        private void CboxPort_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            mf.SerialGPS2.PortName = cboxPort2.Text;
            FormGPS.portNameGPS2 = cboxPort2.Text;
        }

        private void CboxBaud_SelectedIndexChanged_2(object sender, EventArgs e)
        {
            mf.SerialGPS2.BaudRate = Convert.ToInt32(cboxBaud2.Text);
            FormGPS.baudRateGPS2 = Convert.ToInt32(cboxBaud2.Text);
        }

        private void BtnOpenSerial_Click2(object sender, EventArgs e)
        {
            {
                mf.SerialPortOpenGPS2();
                if (mf.SerialGPS2.IsOpen)
                {
                    cboxBaud2.Enabled = false;
                    cboxPort2.Enabled = false;
                    btnCloseSerial2.Enabled = true;
                    btnOpenSerial2.Enabled = false;
                    lblCurrentBaud2.Text = mf.SerialGPS2.BaudRate.ToString();
                    lblCurrentPort2.Text = mf.SerialGPS2.PortName;
                }
                else
                {
                    cboxBaud2.Enabled = true;
                    cboxPort2.Enabled = true;
                    btnCloseSerial2.Enabled = false;
                    btnOpenSerial2.Enabled = true;
                }
            }
        }

        private void BtnCloseSerial_Click2(object sender, EventArgs e)
        {
            mf.SerialPortCloseGPS2();
            if (mf.SerialGPS2.IsOpen)
            {
                cboxBaud2.Enabled = false;
                cboxPort2.Enabled = false;
                btnCloseSerial2.Enabled = true;
                btnOpenSerial2.Enabled = false;
            }
            else
            {
                cboxBaud2.Enabled = true;
                cboxPort2.Enabled = true;
                btnCloseSerial2.Enabled = false;
                btnOpenSerial2.Enabled = true;
            }
        }
    } //class
} //namespace