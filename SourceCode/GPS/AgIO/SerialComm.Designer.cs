//Please, if you use this, share the improvements

using System.IO.Ports;
using System;
using System.Windows.Forms;
using System.Drawing;
using System.Globalization;
using System.IO;

namespace AgOpenGPS
{
    public partial class FormLoop
    {
        public static string portNameGPS = "COM GPS";
        public static int baudRateGPS = 4800;

        public static string portNameGPS2 = "COM GPS2";
        public static int baudRateGPS2 = 4800;

        public static string portNameMachine = "COM Mach";
        public static int baudRateMachine = 38400;

        public static string portNameAutoSteer = "COM AS";
        public static int baudRateAutoSteer = 38400;

        //used to decide to autoconnect section arduino this run
        public bool wasRateMachineConnectedLastRun = false;
        public string recvGPSSentence = "Inital Setting";
        public string recvGPS2Sentence = "Inital Setting";
        public string recvSteerSentence = "Inital Setting";
        public string recvMachineSentence = "Inital Setting";

        public byte checksumSent = 0;
        public byte checksumRecd = 0;

        //used to decide to autoconnect autosteer arduino this run
        public bool wasAutoSteerConnectedLastRun = false;

        //serial port gps is connected to
        public SerialPort spGPS = new SerialPort(portNameGPS, baudRateGPS, Parity.None, 8, StopBits.One);

        //serial port gps2 is connected to
        public SerialPort spGPS2 = new SerialPort(portNameGPS2, baudRateGPS2, Parity.None, 8, StopBits.One);

        //serial port Arduino is connected to
        public SerialPort spMachine = new SerialPort(portNameMachine, baudRateMachine, Parity.None, 8, StopBits.One);

        //serial port AutoSteer is connected to
        public SerialPort spAutoSteer = new SerialPort(portNameAutoSteer, baudRateAutoSteer, Parity.None, 8, StopBits.One);

        #region AutoSteerPort // --------------------------------------------------------------------
        private void ReceivedAutoSteerPort(string sentence)
        {
            int end = sentence.IndexOf("\r");
            if (end == -1) return;
            end = sentence.IndexOf(",", StringComparison.Ordinal);
            if (end == -1) return;

            recvSteerSentence = sentence;
            traffic.cntrSteerIn++;

            //the ArdMachine sentence to be parsed
            //sentence = sentence.Substring(0, end);
            string[] words = sentence.Split(',');
            byte[] data = new byte[10];

            if (words.Length != 10) return; // check lenght: 2 byte header + 8 byte data

            for (int i = 0; i < 10; i++)
            {
                data[i] = Convert.ToByte(words[i]);
            }
            SendToLoopBackMessage(data);
        }

        public void SendAutoSteerPort(byte[] items, int numItems)
        {
            ////add the out of bounds bit to uturn byte bit 7
            //if (mc.isOutOfBounds)
            //    mc.machineData[mc.mdUTurn] |= 0b10000000;
            //else
            //    mc.machineData[mc.mdUTurn] &= 0b01111111;

            //if (spAutoSteer.IsOpen)
            //{
            //    try { spAutoSteer.Write(items, 0, numItems); }
            //    catch (Exception e)
            //    {
            //        WriteErrorLog("Out Data to Steering Port " + e.ToString());
            //        SerialPortAutoSteerClose();
            //    }
            //}
        }

        //open the Arduino serial port
        public void OpenAutoSteerPort()
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
                //WriteErrorLog("Opening Steer Port" + e.ToString());

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

        public void CloseAutoSteerPort()
        {
            if (spAutoSteer.IsOpen)
            {
                spAutoSteer.DataReceived -= sp_DataReceivedAutoSteer;
                try { spAutoSteer.Close(); }
                catch (Exception e)
                {
                    //WriteErrorLog("Closing steer Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                Properties.Settings.Default.setPort_wasAutoSteerConnected = false;
                Properties.Settings.Default.Save();

                spAutoSteer.Dispose();
            }
        }

        //Arduino serial port receive in its own thread
        private void sp_DataReceivedAutoSteer(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spAutoSteer.IsOpen)
            {
                //if (!Properties.Settings.Default.setAS_isJRK)
                {
                    try
                    {
                        //System.Threading.Thread.Sleep(25);
                        string sentence = spAutoSteer.ReadLine();
                        this.BeginInvoke(new LineReceivedEventHandlerAutoSteer(ReceivedAutoSteerPort), sentence);
                    }
                    //this is bad programming, it just ignores errors until its hooked up again.
                    catch (Exception )
                    {
                        //WriteErrorLog("AutoSteer Recv" + ex.ToString());
                    }

                }
                //else   //get 2 byte feedback from pololu

                //{


                //    byte[] buffer = new byte[2];
                //    spAutoSteer.Read(buffer, 0, 2);
                //    int feedback = buffer[0] + 256 * buffer[1];

                //    actualSteerAngleDisp = feedback - 2047;
                //    actualSteerAngleDisp += (Properties.Settings.Default.setAS_steerAngleOffset - 127) * 5;
                //    actualSteerAngleDisp /= Properties.Settings.Default.setAS_countsPerDegree;
                //    actualSteerAngleDisp *= 100;                               
                //}
            }
        }

        //called by the AutoSteer module delegate every time a chunk is rec'd
        public double actualSteerAngleDisp = 0;

        //the delegate for thread
        private delegate void LineReceivedEventHandlerAutoSteer(string sentence);

        #endregion

        #region MachineSerialPort //--------------------------------------------------------------------

        private void ReceiveMachinePort(string sentence)
        {
            //mc.serialRecvMachineStr = sentence;
            //if (pbarMachine++ > 99) pbarMachine = 0;

            // Find end of sentence, if not a CR, return
            int end = sentence.IndexOf("\r");
            if (end == -1) return;
            end = sentence.IndexOf(",", StringComparison.Ordinal);
            if (end == -1) return;
            recvMachineSentence = sentence;

            traffic.cntrMachineIn++;
            //the ArdMachine sentence to be parsed
            //sentence = sentence.Substring(0, end);
            string[] words = sentence.Split(',');
            byte[] data = new byte[10];
            try
            {
                if (words.Length != 10) return; // check lenght: 2 byte header + 8 byte data

                for (int i = 0; i < 10; i++)
                {
                    data[i] = Convert.ToByte(words[i]);
                }

                SendToLoopBackMessage(data);
            }
            catch { }

        }

        //Send machine info out to machine board
        public void SendMachinePort(byte[] items, int numItems)
        {            
            //Tell Arduino to turn section on or off accordingly
            if (spMachine.IsOpen)
            {
                try { spMachine.Write(items, 0, numItems); }
                catch (Exception)
                {
                    CloseMachinePort();
                }
            }
        }

        //open the Arduino serial port
        public void OpenMachinePort()
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
                //WriteErrorLog("Opening Machine Port" + e.ToString());

                MessageBox.Show(e.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No Arduino Port Active");


                Properties.Settings.Default.setPort_wasMachineConnected = false;
                Properties.Settings.Default.Save();
            }

            if (spMachine.IsOpen)
            {
                //short delay for the use of mega2560, it is working in debugmode with breakpoint
                System.Threading.Thread.Sleep(1000); // 500 was not enough

                spMachine.DiscardOutBuffer();
                spMachine.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameMachine = portNameMachine;
                Properties.Settings.Default.setPort_wasMachineConnected = true;
                Properties.Settings.Default.Save();
            }
        }

        //close the machine port
        public void CloseMachinePort()
        {
            if (spMachine.IsOpen)
            {
                spMachine.DataReceived -= sp_DataReceivedMachine;
                try { spMachine.Close(); }
                catch (Exception e)
                {
                    //WriteErrorLog("Closing Machine Serial Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                Properties.Settings.Default.setPort_wasMachineConnected = false;
                Properties.Settings.Default.Save();

                spMachine.Dispose();
            }
        }

        //the delegate for thread
        private delegate void LineReceivedEventHandlerMachine(string sentence);

        //Arduino serial port receive in its own thread
        private void sp_DataReceivedMachine(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spMachine.IsOpen)
            {
                try
                {
                    //System.Threading.Thread.Sleep(25);
                    string sentence = spMachine.ReadLine();
                    this.BeginInvoke(new LineReceivedEventHandlerMachine(ReceiveMachinePort), sentence);                    
                    if (spMachine.BytesToRead > 32) spMachine.DiscardInBuffer();
                }
                //this is bad programming, it just ignores errors until its hooked up again.
                catch (Exception )
                {
                    //WriteErrorLog("Machine Data Recvd " + ex.ToString());
                }

            }
        }

        #endregion

        #region GPS SerialPort //--------------------------------------------------------------------------

        //called by the GPS delegate every time a chunk is rec'd
        private void ReceiveGPSPort(string sentence)
        {
            SendToLoopBackMessage(sentence);
            traffic.cntrGPSIn += sentence.Length;
            recvGPSSentence = sentence;

        }

        public void SendGPSPort(byte[] data)
        {
            try
            {
                if (spGPS.IsOpen)
                {
                    spGPS.Write(data, 0, data.Length);
                    traffic.cntrGPSOut += data.Length;
                }
            }
            catch (Exception)
            {
            }

        }

        public void OpenGPSPort()
        {
            //close it first
            CloseGPSPort();

            //if (spGPS.IsOpen)
            //{
            //    //simulatorOnToolStripMenuItem.Checked = false;
            //    //panelSim.Visible = false;
            //    //timerSim.Enabled = false;

            //    //Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
            //    //Settings.Default.Save();
            //}


            if (!spGPS.IsOpen)
            {
                spGPS.PortName = portNameGPS;
                spGPS.BaudRate = baudRateGPS;
                spGPS.DataReceived += sp_DataReceivedGPS;
                spGPS.WriteTimeout = 1000;
            }

            try { spGPS.Open(); }
            catch (Exception)
            {
            }

            if (spGPS.IsOpen)
            {
                //discard any stuff in the buffers
                spGPS.DiscardOutBuffer();
                spGPS.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameGPS = portNameGPS;
                Properties.Settings.Default.setPort_baudRateGPS = baudRateGPS;
                Properties.Settings.Default.Save();
            }
        }

        public void CloseGPSPort()
        {
            //if (sp.IsOpen)
            {
                spGPS.DataReceived -= sp_DataReceivedGPS;
                try { spGPS.Close(); }
                catch (Exception e)
                {
                    //WriteErrorLog("Closing GPS Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }

                //update port status labels
                //stripPortGPS.Text = " * * " + baudRateGPS.ToString();
                //stripPortGPS.ForeColor = Color.ForestGreen;
                //stripOnlineGPS.Value = 1;
                spGPS.Dispose();
            }
        }

        private delegate void LineReceivedEventHandlerGPS(string sentence);

        //serial port receive in its own thread
        private void sp_DataReceivedGPS(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spGPS.IsOpen)
            {
                try
                {
                    string sentence = spGPS.ReadLine();
                    this.BeginInvoke(new LineReceivedEventHandlerGPS(ReceiveGPSPort), sentence);
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion SerialPortGPS

        #region GPS2 SerialPort //--------------------------------------------------------------------------

        //called by the GPS2 delegate every time a chunk is rec'd
        private void ReceiveGPS2Port(string sentence)
        {
            SendToLoopBackMessage(sentence);
            traffic.cntrGPS2In += sentence.Length;
            recvGPS2Sentence = sentence;

        }

        public void SendGPS2Port(byte[] data)
        {
            try
            {
                if (spGPS2.IsOpen)
                {
                    spGPS2.Write(data, 0, data.Length);
                    traffic.cntrGPS2Out += data.Length;
                }
            }
            catch (Exception)
            {
            }

        }

        public void OpenGPS2Port()
        {
            //close it first
            CloseGPS2Port();

            //if (spGPS2.IsOpen)
            //{
            //    //simulatorOnToolStripMenuItem.Checked = false;
            //    //panelSim.Visible = false;
            //    //timerSim.Enabled = false;

            //    //Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
            //    //Settings.Default.Save();
            //}


            if (!spGPS2.IsOpen)
            {
                spGPS2.PortName = portNameGPS2;
                spGPS2.BaudRate = baudRateGPS2;
                spGPS2.DataReceived += sp_DataReceivedGPS2;
                spGPS2.WriteTimeout = 1000;
            }

            try { spGPS2.Open(); }
            catch (Exception)
            {
            }

            if (spGPS2.IsOpen)
            {
                //discard any stuff in the buffers
                spGPS2.DiscardOutBuffer();
                spGPS2.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameGPS2 = portNameGPS2;
                Properties.Settings.Default.setPort_baudRateGPS2 = baudRateGPS2;
                Properties.Settings.Default.Save();
            }
        }

        public void CloseGPS2Port()
        {
            //if (sp.IsOpen)
            {
                spGPS2.DataReceived -= sp_DataReceivedGPS2;
                try { spGPS2.Close(); }
                catch (Exception e)
                {
                    //WriteErrorLog("Closing GPS2 Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }

                //update port status labels
                //stripPortGPS2.Text = " * * " + baudRateGPS2.ToString();
                //stripPortGPS2.ForeColor = Color.ForestGreen;
                //stripOnlineGPS2.Value = 1;
                spGPS2.Dispose();
            }
        }

        private delegate void LineReceivedEventHandlerGPS2(string sentence);

        //serial port receive in its own thread
        private void sp_DataReceivedGPS2(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spGPS2.IsOpen)
            {
                try
                {
                    string sentence = spGPS2.ReadLine();
                    this.BeginInvoke(new LineReceivedEventHandlerGPS2(ReceiveGPS2Port), sentence);
                }
                catch (Exception)
                {
                }
            }
        }

        #endregion SerialPortGPS2



        public void FileSaveComm(string FileName)
        {
            commFileName = Path.GetFileNameWithoutExtension(FileName);
            Properties.Settings.Default.setComm_commName = commFileName;
            Properties.Settings.Default.Save();

            using (StreamWriter writer = new StreamWriter(FileName))
            {
                writer.WriteLine("Version," + Application.ProductVersion.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("Culture," + Properties.Settings.Default.setF_culture.ToString(CultureInfo.InvariantCulture));
                
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");

                writer.WriteLine("IsNtripCasterIP," + Properties.Settings.Default.setNTRIP_casterIP.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripCasterPort," + Properties.Settings.Default.setNTRIP_casterPort.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripCasterURL," + Properties.Settings.Default.setNTRIP_casterURL.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripGGAManual," + Properties.Settings.Default.setNTRIP_isGGAManual.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripOn," + Properties.Settings.Default.setNTRIP_isOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripTCP," + Properties.Settings.Default.setNTRIP_isTCP.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripManualLat," + Properties.Settings.Default.setNTRIP_manualLat.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripManualLon," + Properties.Settings.Default.setNTRIP_manualLon.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripMount," + Properties.Settings.Default.setNTRIP_mount.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripGGAInterval," + Properties.Settings.Default.setNTRIP_sendGGAInterval.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripSendToUDPPort," + Properties.Settings.Default.setNTRIP_sendToUDPPort.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripUserName," + Properties.Settings.Default.setNTRIP_userName.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("IsNtripUserPassword," + Properties.Settings.Default.setNTRIP_userPassword.ToString(CultureInfo.InvariantCulture));

                writer.WriteLine("IsUDPOn," + Properties.Settings.Default.setUDP_isOn.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("GPSSimLatitude," + Properties.Settings.Default.setGPS_SimLatitude.ToString(CultureInfo.InvariantCulture));
                writer.WriteLine("GPSSimLongitude" + "," + Properties.Settings.Default.setGPS_SimLongitude.ToString(CultureInfo.InvariantCulture));


                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
                writer.WriteLine("Empty," + "10");
            }

            //little show to say saved and where
            TimedMessageBox(3000, "Saved in ", commDirectory);
        }

        public DialogResult FileOpenComm(string fileName)
        {
            //make sure the file if fully valid and vehicle matches sections
            using (StreamReader reader = new StreamReader(fileName))
            {
                try
                {
                    string line;
                    Properties.Settings.Default.setComm_commName = fileName;
                    string[] words;
                    line = reader.ReadLine(); words = line.Split(',');


                    string vers = words[1].Replace('.', '0');
                    int fileVersion = int.Parse(vers, CultureInfo.InvariantCulture);

                    string assemblyVersion = Application.ProductVersion.ToString(CultureInfo.InvariantCulture);
                    assemblyVersion = assemblyVersion.Replace('.', '0');
                    int appVersion = int.Parse(assemblyVersion, CultureInfo.InvariantCulture);

                    appVersion /= 100;
                    fileVersion /= 100;

                    if (fileVersion < appVersion)
                    {
                        TimedMessageBox(5000, "File Error", "Must be Version " + Application.ProductVersion.ToString(CultureInfo.InvariantCulture) + " or higher");
                        return DialogResult.Abort;
                    }

                    else
                    {
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setF_culture = (words[1]);

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_casterIP = words[1];
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_casterPort = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_casterURL = words[1];

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_isGGAManual = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_isOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_isTCP = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_manualLat = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_manualLon = double.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_mount = (words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_sendGGAInterval = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_sendToUDPPort = int.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_userName = (words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setNTRIP_userPassword = (words[1]);

                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setUDP_isOn = bool.Parse(words[1]);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setGPS_SimLatitude = double.Parse(words[1], CultureInfo.InvariantCulture);
                        line = reader.ReadLine(); words = line.Split(',');
                        Properties.Settings.Default.setGPS_SimLongitude = double.Parse(words[1], CultureInfo.InvariantCulture);

                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();
                        line = reader.ReadLine();

                        //fill in the current variables with restored data
                        commFileName = Path.GetFileNameWithoutExtension(fileName);
                        Properties.Settings.Default.setComm_commName = commFileName;

                        Properties.Settings.Default.Save();
                    }

                    return DialogResult.OK;
                }
                catch (Exception e) //FormatException e || IndexOutOfRangeException e2)
                {
                    string test = e.Message;

                    //WriteErrorLog("Open Vehicle" + e.ToString());

                    //comm is corrupt, reload with all default information
                    Properties.Settings.Default.Reset();
                    Properties.Settings.Default.Save();
                    MessageBox.Show("Shit", "Comm load", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    //Application.Exit();
                    return DialogResult.Cancel;
                }
            }
        }//end of open file



    }//end class
}//end namespace