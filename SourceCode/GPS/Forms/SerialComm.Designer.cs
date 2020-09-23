using System.IO.Ports;
using System;
using System.Windows.Forms;
using AgOpenGPS.Properties;
using System.Globalization;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        public static string portNameGPS = "COM GPS";
        public static int baudRateGPS = 4800;

        public static string portNameMachine = "COM Sect";
        public static int baudRateMachine = 38400;

        public static string portNameAutoSteer = "COM AS";
        public static int baudRateAutoSteer = 38400;

        public string NMEASentence = "No Data";

        //used to decide to autoconnect section arduino this run
        public bool wasRateMachineConnectedLastRun = false;
        public byte checksumSent = 0;
        public byte checksumRecd = 0;

        //called by the AutoSteer module delegate every time a chunk is rec'd
        public double actualSteerAngleDisp = 0;


        public string[] recvSentenceSettings = new string[4];
        public string lastRecvd = "";
        //used to decide to autoconnect autosteer arduino this run
        public bool wasAutoSteerConnectedLastRun = false;

        //serial port gps is connected to
        public SerialPort spGPS = new SerialPort(portNameGPS, baudRateGPS, Parity.None, 8, StopBits.One);

        //serial port Arduino is connected to
        public SerialPort spMachine = new SerialPort(portNameMachine, baudRateMachine, Parity.None, 8, StopBits.One);
        int spMachineIdx = 0;
        public byte[] spMachineBytes = new byte[3] { 0x7F, 0, 0 };

        //serial port AutoSteer is connected to
        public SerialPort spAutoSteer = new SerialPort(portNameAutoSteer, baudRateAutoSteer, Parity.None, 8, StopBits.One);
        int spAutoSteerIdx = 0;
        public byte[] spAutoSteerBytes = new byte[3] { 0x7F, 0, 0 };


        public void SendData(byte[] Data, bool Checksum)
        {
            if (spAutoSteer.IsOpen)
            {
                try { spAutoSteer.Write(Data, 0, Data.Length); }
                catch (Exception e)
                {
                    WriteErrorLog("Out Steer Port " + e.ToString());
                    SerialPortAutoSteerClose();
                }
            }

            if (spMachine.IsOpen)
            {
                try { spMachine.Write(Data, 0, Data.Length); }
                catch (Exception e)
                {
                    WriteErrorLog("Out Machine Port " + e.ToString());
                    SerialPortMachineClose();
                }
            }

            //send out to udp network
            if (Properties.Settings.Default.setUDP_isOn)
            {
                if (isUDPSendConnected)
                {
                    try
                    {
                        if (Data.Length != 0)
                        {
                            sendSocket.BeginSendTo(Data, 0, Data.Length, 0, epAutoSteer, new AsyncCallback(SendData), null);
                        }
                    }
                    catch (Exception) { }
                }
            }
            if (Checksum)
            {
                int tt = Data[2];
                checksumSent = 0;
                for (int i = 3; i < Data[2]; i++)
                {
                    checksumSent += Data[i];
                }
            }
        }


        #region AutoSteerPort // --------------------------------------------------------------------
        private void SerialLineReceivedAutoSteer(string sentence)
        {
            //spit it out no matter what it says
            mc.serialRecvAutoSteerStr = sentence;
            if (pbarSteer++ > 99) pbarSteer = 0;

            // Find end of sentence and a comma, if not a CR, return
            int end = sentence.IndexOf("\r");
            if (end == -1) return;
            end = sentence.IndexOf(",", StringComparison.Ordinal);
            if (end == -1) return;

            //the sentence to be parsed
            string[] words = sentence.Split(',');

            if (words.Length != 10) return; // check lenght: 2 byte header + 8 byte data

            int incomingInt = 0;
            int.TryParse(words[0], out incomingInt);

            if (incomingInt == 127)
            {
                int.TryParse(words[1], out incomingInt);

                switch (incomingInt)
                {
                    // 127,253, 2 - actual steer angle*100, 3 - setpoint steer angle*100, 4 - heading in degrees * 16, 
                    //5 - roll in degrees * 16, 6 - steerSwitch position,pwmDisplay,,;
                    case 253:  //PGN 127 253: AutoSteer main sentence 

                        double.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out actualSteerAngleDisp);

                        //first 2 used for display mainly in autosteer window chart as strings
                        //parse the values
                        if (ahrs.isHeadingCorrectionFromAutoSteer)
                        {
                            int.TryParse(words[4], NumberStyles.Float, CultureInfo.InvariantCulture, out ahrs.correctionHeadingX16);
                        }

                        if (ahrs.isRollFromAutoSteer) int.TryParse(words[5], NumberStyles.Float, CultureInfo.InvariantCulture, out ahrs.rollX16);


                        byte.TryParse(words[6], out byte Data);


                        if (isJobStarted && mc.isWorkSwitchEnabled)
                        {
                            if ((!mc.isWorkSwitchActiveLow && (Data & 1) == 1) || (mc.isWorkSwitchActiveLow && (Data & 1) == 0))
                            {
                                if (mc.isWorkSwitchManual)
                                {
                                    if (autoBtnState != FormGPS.btnStates.On)
                                    {
                                        autoBtnState = FormGPS.btnStates.On;
                                        btnSection_Update();
                                    }
                                }
                                else
                                {
                                    if (autoBtnState != FormGPS.btnStates.Auto)
                                    {
                                        autoBtnState = FormGPS.btnStates.Auto;
                                        btnSection_Update();
                                    }
                                }
                            }
                            else
                            {
                                if (autoBtnState != FormGPS.btnStates.Off)
                                {
                                    autoBtnState = FormGPS.btnStates.Off;
                                    btnSection_Update();
                                }
                            }
                        }


                        //AutoSteerAuto button enable - Ray Bear inspired code - Thx Ray!
                        if (ahrs.RemoteAutoSteer)
                        {
                            if (isJobStarted && !recPath.isDrivingRecordedPath && (ABLines.BtnABLineOn || ct.isContourBtnOn || CurveLines.BtnCurveLineOn))
                            {
                                if ((Data & 2) == 0)
                                {
                                    if (!isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                                    btnAutoSteer.BackColor = System.Drawing.Color.SkyBlue;
                                }
                                else
                                {
                                    if (isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                                    btnAutoSteer.BackColor = System.Drawing.Color.Transparent;
                                }
                            }
                            else
                            {
                                if (isAutoSteerBtnOn) btnAutoSteer.PerformClick();
                                btnAutoSteer.BackColor = System.Drawing.Color.Transparent;
                            }
                        }

                        int.TryParse(words[7], out mc.pwmDisplay);

                        break;

                    // 127,230, 2=checksum, 8 = ino version
                    case 230:

                        byte.TryParse(words[2], out checksumRecd);

                        if (checksumRecd != checksumSent)
                        {
                            MessageBox.Show(
                                "Sent: " + checksumSent + "\r\n Recieved: " + checksumRecd,
                                    "Checksum Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }

                        if (words[3] != inoVersionStr)
                        {
                            Form af = Application.OpenForms["FormSteer"];

                            if (af != null)
                            {
                                af.Focus();
                                af.Close();
                            }

                            af = Application.OpenForms["FormArduinoSettings"];

                            if (af != null)
                            {
                                af.Focus();
                                af.Close();
                            }

                            //spAutoSteer.Close();
                            MessageBox.Show("Arduino INO Is Wrong Version \r\n Upload AutoSteer_USB_" + currentVersionStr + ".ino", gStr.gsFileError,
                                                MessageBoxButtons.OK, MessageBoxIcon.Question);
                            Close();
                        }

                        break;
                }
            }
        }

        private delegate void LineReceivedEventHandlerAutoSteer(string sentence);

        //Arduino serial port receive in its own thread
        private void sp_DataReceivedAutoSteer(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spAutoSteer.IsOpen)
            {
                try
                {


                    string sentence = spAutoSteer.ReadLine();
                    this.BeginInvoke(new LineReceivedEventHandlerAutoSteer(SerialLineReceivedAutoSteer), sentence);


                    /*
                    while (spAutoSteer.BytesToRead > 0)
                    {
                        int test = spAutoSteer.ReadByte();
                        if (spAutoSteerIdx == 0)
                        {
                            if (test == spAutoSteerBytes[spAutoSteerIdx]) spAutoSteerIdx++;
                            else spAutoSteerIdx = 0;
                        }
                        else
                        {
                            spAutoSteerBytes[spAutoSteerIdx] = (byte)test;
                            if (spAutoSteerIdx == 2) Array.Resize(ref spAutoSteerBytes, Math.Max((int)spAutoSteerBytes[spAutoSteerIdx], 3));
                            spAutoSteerIdx++;

                            if (spAutoSteerIdx == spAutoSteerBytes.Length)
                            {
                                BeginInvoke((MethodInvoker)(() => UpdateRecvMessage(5, spAutoSteerBytes)));
                                spAutoSteerIdx = 0;
                            }
                        }
                    }
                    */
                }
                //this is bad programming, it just ignores errors until its hooked up again.
                catch (Exception ex)
                {
                    WriteErrorLog("AutoSteer Recv" + ex.ToString());
                }
            }
        }

        //open the Arduino serial port
        public void SerialPortAutoSteerOpen()
        {
            if (!spAutoSteer.IsOpen)
            {
                spAutoSteer.PortName = portNameAutoSteer;
                spAutoSteer.BaudRate = baudRateAutoSteer;
                spAutoSteer.DataReceived += sp_DataReceivedAutoSteer;
                spAutoSteer.DtrEnable = true;
                spAutoSteer.RtsEnable = true;
            }
            try { spAutoSteer.Open(); }
            catch (Exception e)
            {
                WriteErrorLog("Opening Steer Port" + e.ToString());

                MessageBox.Show(e.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No AutoSteer Port Active");

                Properties.Settings.Default.setPort_wasAutoSteerConnected = false;
                Properties.Settings.Default.Save();
            }

            if (spAutoSteer.IsOpen)
            {
                spAutoSteer.DiscardOutBuffer();
                spAutoSteer.DiscardInBuffer();

                //update port status label

                Properties.Settings.Default.setPort_portNameAutoSteer = portNameAutoSteer;
                Properties.Settings.Default.setPort_wasAutoSteerConnected = true;
                Properties.Settings.Default.Save();
            }
        }

        public void SerialPortAutoSteerClose()
        {
            if (spAutoSteer.IsOpen)
            {
                spAutoSteer.DataReceived -= sp_DataReceivedAutoSteer;
                try { spAutoSteer.Close(); }
                catch (Exception e)
                {
                    WriteErrorLog("Closing steer Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                Properties.Settings.Default.setPort_wasAutoSteerConnected = false;
                Properties.Settings.Default.Save();

                spAutoSteer.Dispose();
            }
        }

        #endregion

        #region MachineSerialPort //--------------------------------------------------------------------

        //Arduino serial port receive in its own thread
        private void sp_DataReceivedMachine(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spMachine.IsOpen)
            {
                try
                {
                    while (spMachine.BytesToRead > 0)
                    {
                        int test = spMachine.ReadByte();
                        if (spMachineIdx == 0)
                        {
                            if (test == spMachineBytes[spMachineIdx]) spMachineIdx++;
                            else spMachineIdx = 0;
                        }
                        else
                        {
                            spMachineBytes[spMachineIdx] = (byte)test;
                            if (spMachineIdx == 2) Array.Resize(ref spMachineBytes, Math.Max((int)spMachineBytes[spMachineIdx], 3));
                            spMachineIdx++;

                            if (spMachineIdx == spMachineBytes.Length)
                            {
                                BeginInvoke((MethodInvoker)(() => UpdateRecvMessage(5, spMachineBytes)));
                                spMachineIdx = 0;
                            }
                        }
                    }
                }
                //this is bad programming, it just ignores errors until its hooked up again.
                catch (Exception ex)
                {
                    WriteErrorLog("Machine Data Recvd " + ex.ToString());
                }
            }
        }

        //open the Arduino serial port
        public void SerialPortMachineOpen()
        {
            if (!spMachine.IsOpen)
            {
                spMachine.PortName = portNameMachine;
                spMachine.BaudRate = baudRateMachine;
                spMachine.DataReceived += sp_DataReceivedMachine;
                spMachine.DtrEnable = true;
                spMachine.RtsEnable = true;
            }

            try { spMachine.Open(); }
            catch (Exception e)
            {
                WriteErrorLog("Opening Machine Port" + e.ToString());

                MessageBox.Show(e.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No Arduino Port Active");


                Properties.Settings.Default.setPort_wasMachineConnected = false;
                Properties.Settings.Default.Save();
            }

            if (spMachine.IsOpen)
            {
                spMachine.DiscardOutBuffer();
                spMachine.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameMachine = portNameMachine;
                Properties.Settings.Default.setPort_wasMachineConnected = true;
                Properties.Settings.Default.Save();
            }
        }

        //close the machine port
        public void SerialPortMachineClose()
        {
            if (spMachine.IsOpen)
            {
                spMachine.DataReceived -= sp_DataReceivedMachine;
                try { spMachine.Close(); }
                catch (Exception e)
                {
                    WriteErrorLog("Closing Machine Serial Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                Properties.Settings.Default.setPort_wasMachineConnected = false;
                Properties.Settings.Default.Save();

                spMachine.Dispose();
            }
        }
        #endregion

        #region GPS SerialPort //--------------------------------------------------------------------------

        //serial port receive in its own thread
        private void sp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spGPS.IsOpen)
            {
                try
                {
                    //read whatever is in port
                    string sentence = spGPS.ReadExisting();
                    BeginInvoke((MethodInvoker)(() => pn.rawBuffer += sentence));
                }
                catch (Exception ex)
                {
                    WriteErrorLog("GPS Data Recv" + ex.ToString());

                    //MessageBox.Show(ex.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "ComPort Failure!");
                }
            }
        }

        public void SerialPortOpenGPS()
        {
            //close it first
            SerialPortCloseGPS();

            if (spGPS.IsOpen)
            {
                simulatorOnToolStripMenuItem.Checked = false;
                panelSim.Visible = false;
                timerSim.Enabled = false;

                Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
                Settings.Default.Save();
            }


            if (!spGPS.IsOpen)
            {
                spGPS.PortName = portNameGPS;
                spGPS.BaudRate = baudRateGPS;
                spGPS.DataReceived += sp_DataReceived;
                spGPS.WriteTimeout = 1000;
            }

            try { spGPS.Open(); }
            catch (Exception)
            {
                //MessageBox.Show(exc.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No Serial Port Active");
                //WriteErrorLog("Open GPS Port " + e.ToString());

                //update port status labels
                //stripPortGPS.Text = " * * ";
                //stripPortGPS.ForeColor = Color.Red;
                //stripOnlineGPS.Value = 1;

                //SettingsPageOpen(0);
            }

            if (spGPS.IsOpen)
            {
                //btnOpenSerial.Enabled = false;

                //discard any stuff in the buffers
                spGPS.DiscardOutBuffer();
                spGPS.DiscardInBuffer();

                //update port status label
                //stripPortGPS.Text = portNameGPS + " " + baudRateGPS.ToString();
                //stripPortGPS.ForeColor = Color.ForestGreen;

                Properties.Settings.Default.setPort_portNameGPS = portNameGPS;
                Properties.Settings.Default.setPort_baudRate = baudRateGPS;
                Properties.Settings.Default.Save();
            }
        }

        public void SerialPortCloseGPS()
        {
            //if (sp.IsOpen)
            {
                spGPS.DataReceived -= sp_DataReceived;
                try { spGPS.Close(); }
                catch (Exception e)
                {
                    WriteErrorLog("Closing GPS Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }

                //update port status labels
                //stripPortGPS.Text = " * * " + baudRateGPS.ToString();
                //stripPortGPS.ForeColor = Color.ForestGreen;
                //stripOnlineGPS.Value = 1;
                spGPS.Dispose();
            }
        }

        #endregion SerialPortGPS

    }//end class
}//end namespace