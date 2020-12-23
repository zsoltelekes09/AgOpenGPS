using System;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormCommSet : Form
    {
        //class variables
        private readonly FormGPS mf = null;

        //constructor
        public FormCommSet(Form callingForm)
        {
            //get copy of the calling main form
            Owner = mf = callingForm as FormGPS;
            InitializeComponent();
            btnOpenSerial.Text = String.Get("gsConnect");
            btnOpenSerialArduino.Text = String.Get("gsConnect");
            btnOpenSerialAutoSteer.Text = String.Get("gsConnect");
            btnCloseSerial.Text = String.Get("gsDisconnect");
            btnCloseSerialArduino.Text = String.Get("gsDisconnect");
            btnCloseSerialAutoSteer.Text = String.Get("gsDisconnect");
            btnRescan.Text = String.Get("gsRescanPorts");

            groupBox1.Text = String.Get("gsGPSPort");
            groupBox3.Text = String.Get("gsAutoSteerPort");
            groupBox2.Text = String.Get("gsMachinePort");

            lblCurrentArduinoPort.Text = String.Get("gsPort");
            lblCurrentPort.Text = String.Get("gsPort");
            lblCurrentAutoSteerPort.Text = String.Get("gsPort");
            lblCurrentBaud.Text = String.Get("gsBaud");

        }

        private void FormCommSet_Load(object sender, EventArgs e)
        {
            //check if GPS port is open or closed and set buttons accordingly
            if (mf.spGPS.IsOpen)
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
            cboxArdPort.Items.Clear();
            cboxASPort.Items.Clear();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames())
            {
                cboxPort.Items.Add(s);
                cboxArdPort.Items.Add(s);
                cboxASPort.Items.Add(s);
            }

            lblCurrentBaud.Text = mf.spGPS.BaudRate.ToString();
            lblCurrentPort.Text = mf.spGPS.PortName;
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
            mf.spGPS.BaudRate = Convert.ToInt32(cboxBaud.Text);
            FormGPS.baudRateGPS = Convert.ToInt32(cboxBaud.Text);
        }

        private void CboxPort_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            mf.spGPS.PortName = cboxPort.Text;
            FormGPS.portNameGPS = cboxPort.Text;
        }


        private void BtnOpenSerial_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setMenu_isSimulatorOn)
            {
                MessageBox.Show(String.Get("gsGotoTopMenuDisplayTouchSimulator") + "\n\r" + String.Get("gsApplicationWillRestart"), String.Get("gsSimulatorOnMustbeOFF"), MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                mf.SerialPortOpenGPS();
                if (mf.spGPS.IsOpen)
                {
                    cboxBaud.Enabled = false;
                    cboxPort.Enabled = false;
                    btnCloseSerial.Enabled = true;
                    btnOpenSerial.Enabled = false;
                    lblCurrentBaud.Text = mf.spGPS.BaudRate.ToString();
                    lblCurrentPort.Text = mf.spGPS.PortName;
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
            if (mf.spGPS.IsOpen)
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
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames()) { cboxASPort.Items.Add(s); }

            cboxArdPort.Items.Clear();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames()) { cboxArdPort.Items.Add(s); }

            cboxPort.Items.Clear();
            foreach (string s in System.IO.Ports.SerialPort.GetPortNames()) { cboxPort.Items.Add(s); }
        }

        #endregion PortSettings //----------------------------------------------------------------

        private void Timer1_Tick(object sender, EventArgs e)
        {
            //GPS phrase
            textBoxRcv.Lines = mf.recvSentenceSettings;
        }

        private void BtnSerialOK_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }
    } //class
} //namespace