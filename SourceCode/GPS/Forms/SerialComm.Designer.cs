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


        public string[] DataSend = new string[9];
        public string[] DataRecieved = new string[9];

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

        int spUBXHeaderIdx = 0;
        public byte[] spUBXMessage = new byte[3] { 0xB5, 0x62, 0x01 };


        public void SendData(byte[] Data, bool Checksum)
        {
            for (int i = 7; i > 0; i--) DataSend[i] = DataSend[i - 1];
            DataSend[0] = DataSend[8];

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

        //Arduino serial port receive in its own thread
        private void sp_DataReceivedAutoSteer(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spAutoSteer.IsOpen)
            {
                try
                {
                    while (spAutoSteer.BytesToRead > 0)
                    {
                        int Data = spAutoSteer.ReadByte();

                        if (spUBXHeaderIdx < 3)
                        {
                            if (spUBXMessage[spUBXHeaderIdx] == Data) spUBXHeaderIdx++;
                            else spUBXHeaderIdx = 0;
                        }
                        if (spAutoSteerIdx == 0 && spUBXHeaderIdx < 3)
                        {
                            if (Data == spAutoSteerBytes[spAutoSteerIdx]) spAutoSteerIdx++;
                            else spAutoSteerIdx = 0;
                        }
                        else if (spUBXHeaderIdx < 3)
                        {
                            spAutoSteerBytes[spAutoSteerIdx] = (byte)Data;
                            if (spAutoSteerIdx == 2) Array.Resize(ref spAutoSteerBytes, Math.Max((int)spAutoSteerBytes[spAutoSteerIdx], 3));

                            if (++spAutoSteerIdx >= spAutoSteerBytes.Length)
                            {
                                BeginInvoke((MethodInvoker)(() => UpdateRecvMessage(5, spAutoSteerBytes)));
                                spAutoSteerIdx = 0;
                            }
                        }
                        else
                        {
                            spUBXMessage[spUBXHeaderIdx] = (byte)Data;
                            if (spUBXHeaderIdx == 5) Array.Resize(ref spUBXMessage, Math.Max((spUBXMessage[4] + (spUBXMessage[5] << 8)) + 8, 8));

                            if (++spUBXHeaderIdx >= spUBXMessage.Length)
                            {
                                BeginInvoke((MethodInvoker)(() => UpdateRecvMessage(5, spUBXMessage)));
                                spUBXHeaderIdx = 0;
                            }
                        }
                    }
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