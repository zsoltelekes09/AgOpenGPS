using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public partial class FormLoop : Form
    {
        public FormLoop()
        {
            InitializeComponent();
            // Add Message Event handler for Form decoupling from client socket thread
            updateRTCM_DataEvent = new UpdateRTCM_Data(OnAddMessage);
        }

        public FormTimedMessage Form { get; set; } = new FormTimedMessage();

        public bool isKeyboardOn = true;

        public double defaultLat = 0, defaultLon = 0;
        public double currentLat = 0, currentLon = 0;

        public double secondsSinceStart;

        //The base directory where Drive will be stored and fields and vehicles branch from
        public string baseDirectory;

        //current directory of Comm storage
        public string commDirectory, commFileName = "";



        //First run
        private void FormLoop_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setF_workingDirectory == "Default")
            {
                baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\Drive\\";
            }
            else
            {
                baseDirectory = Properties.Settings.Default.setF_workingDirectory + "\\Drive\\";
            }

            //get the fields directory, if not exist, create
            commDirectory = baseDirectory + "Comm\\";
            string dir = Path.GetDirectoryName(commDirectory);
            if (!string.IsNullOrEmpty(dir) && !Directory.Exists(dir)) { Directory.CreateDirectory(dir); }

            if (Properties.Settings.Default.setUDP_isOn)
            {
                LoadUDPNetwork();
            }

            LoadLoopback();

            //set baud and port from last time run


            baudRateGPS = Properties.Settings.Default.setPort_baudRateGPS;
            portNameGPS = Properties.Settings.Default.setPort_portNameGPS;

            //try and open
            OpenGPSPort();

            //same for SectionMachine port
            portNameMachine = Properties.Settings.Default.setPort_portNameMachine;
            wasRateMachineConnectedLastRun = Properties.Settings.Default.setPort_wasMachineConnected;
            if (wasRateMachineConnectedLastRun)
            {
                OpenMachinePort();
            }

            //same for AutoSteer port
            portNameAutoSteer = Properties.Settings.Default.setPort_portNameAutoSteer;
            wasAutoSteerConnectedLastRun = Properties.Settings.Default.setPort_wasAutoSteerConnected;
            if (wasAutoSteerConnectedLastRun)
            {
                OpenAutoSteerPort();
            }

            lblWatch.Text = "Wait GPS";

            //start NTRIP if required
            isNTRIP_RequiredOn = Properties.Settings.Default.setNTRIP_isOn;

            if (isNTRIP_RequiredOn)
            {
                btnStartStopNtrip.Visible = true;
                btnStartStopNtrip.Visible = true;
                lblWatch.Visible = true;
                lblNTRIPBytes.Visible = true;
                lblBytes.Visible = true;
            }
            else
            {
                btnStartStopNtrip.Visible = false;
                btnStartStopNtrip.Visible = false;
                lblWatch.Visible = false;
                lblNTRIPBytes.Visible = false;
                lblBytes.Visible = false;
            }

            btnStartStopNtrip.Text = "Off";

            timer1.Enabled = true;
        }

        public void TimedMessageBox(int timeout, string s1, string s2)
        {
            if (InvokeRequired)
            {
                // We're not in the UI thread, so we need to call BeginInvoke
                BeginInvoke((MethodInvoker)(() => TimedMessageBox(timeout, s1, s2)));
                return;
            }
            Form.SetTimedMessage(timeout, s1, s2, this);
        }

        private void SettingsNTRIP()
        {
            using (FormNtrip form = new FormNtrip(this))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    if (isNTRIP_Connected)
                    {
                        SettingsShutDownNTRIP();
                    }
                }
            }
        }

        private void StartDrive()
        {
            Process.Start(Application.ExecutablePath);
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            secondsSinceStart = (DateTime.Now - Process.GetCurrentProcess().StartTime).TotalSeconds;

            //pbarCPU.Value = (int)pcCPU.NextValue();
            //pbarRAM.Value = (int)pcRAM.NextValue();

            lblCPU.Text = pbarCPU.Value.ToString();
            lblRAM.Text = pbarRAM.Value.ToString();

            //traffic.enableCounter++;

            if (traffic.isTrafficOn && traffic.enableCounter < 120)
            {
                if ((traffic.enableCounter & 1) != 0)
                {

                    lblToAOG.Text = lblFromAOG.Text = lblFromUDP.Text = lblToUDP.Text =
                        lblFromGPS.Text = lblToGPS.Text = lblFromSteer.Text = lblToSteer.Text =
                        lblFromMachine.Text = lblToMachine.Text = "-";

                    traffic.cntrPGNToAOG = traffic.cntrPGNFromAOG = traffic.cntrUDPIn = traffic.cntrUDPOut =
                        traffic.cntrGPSIn = traffic.cntrGPSOut = traffic.cntrSteerIn = traffic.cntrSteerOut =
                        traffic.cntrMachineIn = traffic.cntrMachineOut = 0;

                }

                else
                {
                    lblToAOG.Text = (traffic.cntrPGNToAOG * 8).ToString();
                    lblFromAOG.Text = (traffic.cntrPGNFromAOG * 8).ToString();

                    lblFromUDP.Text = (traffic.cntrUDPIn * 80).ToString();
                    lblToUDP.Text = (traffic.cntrUDPOut * 80).ToString();

                    lblFromGPS.Text = (traffic.cntrGPSIn * 8).ToString();
                    lblToGPS.Text = (traffic.cntrGPSOut * 8).ToString();

                    lblFromSteer.Text = (traffic.cntrSteerIn * 80).ToString();
                    lblToSteer.Text = (traffic.cntrSteerOut * 80).ToString();

                    lblFromMachine.Text = (traffic.cntrMachineIn * 80).ToString();
                    lblToMachine.Text = (traffic.cntrMachineOut * 80).ToString();

                    traffic.cntrPGNToAOG = traffic.cntrPGNFromAOG = traffic.cntrUDPIn = traffic.cntrUDPOut =
                    traffic.cntrGPSIn = traffic.cntrGPSOut = traffic.cntrSteerIn = traffic.cntrSteerOut =
                    traffic.cntrMachineIn = traffic.cntrMachineOut = 0;
                }

                lblCurentLon.Text = currentLon.ToString("N7");
                lblCurrentLat.Text = currentLat.ToString("N7");

                if (traffic.enableCounter == 118)
                {
                    cboxIsTrafficOn.Checked = false;
                }
            }

            lblCurentLon.Text = currentLon.ToString("N7");
            lblCurrentLat.Text = currentLat.ToString("N7");

            //do all the NTRIP routines
            DoNTRIPSecondRoutine();

            //send back to Drive proof of life
            traffic.looperStatus[5] = (byte)(secondsSinceStart);

            //Proof of life to Drive
            SendToLoopBackMessage(traffic.looperStatus);
        }

        private void CboxIsTrafficOn_CheckedChanged(object sender, EventArgs e)
        {
            traffic.isTrafficOn = cboxIsTrafficOn.Checked;
            if (traffic.isTrafficOn)
            {
                traffic.enableCounter = 0;
            }
        }

        private void DoNTRIPSecondRoutine()
        {
            //count up the ntrip clock only if everything is alive
            if ( isNTRIP_RequiredOn)
            {
                IncrementNTRIPWatchDog();
            }

            //Have we connection
            if (isNTRIP_RequiredOn && !isNTRIP_Connected && !isNTRIP_Connecting)
            {
                if (!isNTRIP_Starting && ntripCounter > 20)
                {
                    StartNTRIP();
                }
            }

            if (isNTRIP_Connecting)
            {
                if (ntripCounter > 28)
                {
                    TimedMessageBox(2000, "Connection Problem", "Not Connecting To Caster");
                    ReconnectRequest();
                }
                if (clientSocket != null && clientSocket.Connected)
                {
                    //TimedMessageBox(2000, "NTRIP Not Connected", " At the StartNTRIP() ");
                    //ReconnectRequest();
                    //return;
                    SendAuthorization();
                }

            }

            if (isNTRIP_RequiredOn)
            {
                //update byte counter and up counter
                if (ntripCounter > 59)
                {
                    btnStartStopNtrip.Text = (ntripCounter / 60) + " Mins";
                }
                else if (ntripCounter < 60 && ntripCounter > 22)
                {
                    btnStartStopNtrip.Text = ntripCounter + " Secs";
                }
                else
                {
                    btnStartStopNtrip.Text = "In " + (Math.Abs(ntripCounter - 22)) + " secs";
                }

                //pbarNtripMenu.Value = unchecked((byte)(tripBytes * 0.02));
                lblNTRIPBytes.Text = ((tripBytes) * 0.001).ToString("###,###,###") + " kb";

                //watchdog for Ntrip
                if (isNTRIP_Connecting)
                {
                    lblWatch.Text = String.Get("gsAuthourizing");
                }
                else
                {
                    if (NTRIP_Watchdog > 10)
                    {
                        lblWatch.Text = String.Get("gsWaiting");
                    }
                    else
                    {
                        lblWatch.Text = String.Get("gsListening");
                    }
                }

                if (sendGGAInterval > 0 && isNTRIP_Sending)
                {
                    lblWatch.Text = "Send GGA";
                    isNTRIP_Sending = false;
                }
            }
        }

        private void BtnStartStopNtrip_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.setNTRIP_isOn)
            {
                if (isNTRIP_RequiredOn)
                {
                    ShutDownNTRIP();
                    lblWatch.Text = "Stopped";
                    btnStartStopNtrip.Text = "OffLine";
                    isNTRIP_RequiredOn = false;
                }
                else
                {
                    isNTRIP_RequiredOn = true;
                    lblWatch.Text = "Waiting";
                }
            }
            else
            {
                TimedMessageBox(2000, "Turn on NTRIP", "NTRIP Client Not Set Up");
            }
        }

        private void StripUDPConfig_Click(object sender, EventArgs e)
        {
            using (FormUDP form = new FormUDP(this))
            {
                DialogResult result = form.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Application.Restart();
                    Environment.Exit(0);
                }
            }
        }

        private void StripSerialPortsConfig_Click(object sender, EventArgs e)
        {
            using (FormCommSet form = new FormCommSet(this))
            {
                form.ShowDialog();
            }
        }

        private void StripRunDrive_Click(object sender, EventArgs e)
        {
            StartDrive();
        }

        private void NTRIPToolStrip_Click(object sender, EventArgs e)
        {
            SettingsNTRIP();
        }

        private void WizardToolStrip_Click(object sender, EventArgs e)
        {

        }

        private void LoadToolStrip_Click(object sender, EventArgs e)
        {
            using (FormCommPicker form = new FormCommPicker(this))
            {
                DialogResult result = form.ShowDialog();
                
                if (result == DialogResult.OK)
                {
                    Application.Exit();
                }
            }
        }

        private void SaveToolStrip_Click(object sender, EventArgs e)
        {
            using (FormCommSaver form = new FormCommSaver(this))
            {
                DialogResult result = form.ShowDialog();
            }
        }

        public void KeyboardToText(TextBox sender, Form callingForm)
        {
            sender.BackColor = Color.Red;
            using (var form = new FormKeyboard(sender.Text, callingForm))
            {
                var result = form.ShowDialog(callingForm);
                if (result == DialogResult.OK)
                {
                    sender.Text = form.ReturnString;
                }
            }
            sender.BackColor = Color.AliceBlue;
        }
    }
}

