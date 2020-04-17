using System;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Drawing;
using System.Globalization;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        // Send and Recv socket
        private Socket sendSocket;
        private Socket recvSocket;
        private bool isUDPSendConnected;

        //IP address and port of Auto Steer server
        IPAddress epIP = IPAddress.Parse(Properties.Settings.Default.setIP_autoSteerIP);

        // Data stream
        private byte[] buffer = new byte[1024];

        // Status delegate
        private delegate void UpdateRecvMessageDelegate(int port, byte[] msg);
        private UpdateRecvMessageDelegate updateRecvMessageDelegate = null;

        // - App Sockets  -----------------------------------------------------
        private Socket sendToAppSocket;
        private Socket recvFromAppSocket;

        //endpoints of modules
        IPEndPoint epAppOne;

        // Data stream
        private byte[] appBuffer = new byte[1024];

        // Status delegate
        private delegate void UpdateStatusDelegate(string status);
        private UpdateStatusDelegate updateStatusDelegate = null;

        public void SendAppData(IAsyncResult asyncResult)
        {
            try
            {
                sendToAppSocket.EndSend(asyncResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveAppData(IAsyncResult asyncResult)
        {
            try
            {
                // Initialise the IPEndPoint for the clients
                EndPoint epSender = new IPEndPoint(IPAddress.Any, 0);

                // Receive all data
                int msgLen = recvFromAppSocket.EndReceiveFrom(asyncResult, ref epSender);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(appBuffer, localMsg, msgLen);

                // Listen for more connections again...
                recvFromAppSocket.BeginReceiveFrom(appBuffer, 0, appBuffer.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveAppData), epSender);

                string text = Encoding.ASCII.GetString(localMsg);

                // Update status through a delegate
                Invoke(updateStatusDelegate, new object[] { text + " IP: " + epSender.ToString() + "\r\n" });
            }
            catch (Exception ex)
            {
                MessageBox.Show("ReceiveData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendToApp()
        {
            try
            {
                // Get packet as byte array
                byte[] byteData = Encoding.ASCII.GetBytes("98,43,26");

                if (byteData.Length != 0)
                    sendToAppSocket.BeginSendTo(byteData, 0, byteData.Length, 
                        SocketFlags.None, epAppOne, new AsyncCallback(SendAppData), null);

                    //sendToAppSocket.BeginSendTo(byteData, 0, byteData.Length, 
                        //SocketFlags.None, epAppTwo, new AsyncCallback(SendAppData), null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send Error: " + ex.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void SendPgnToApp(byte[] byteData)
        {
            if (isUDPSendConnected)
            {
                try
                {
                    if (byteData.Length != 0)
                        sendToAppSocket.BeginSendTo(byteData, 0, byteData.Length,
                            SocketFlags.None, epAppOne, new AsyncCallback(SendAppData), null);
                }
                catch (Exception)
                {
                    //WriteErrorLog("Sending UDP Message" + e.ToString());
                    //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void UpdateAppStatus(string status)
        {
            //rtxtStatus.Text = (status);
        }

        // ------------------------------------------------------------------


        //sends ascii text message

        public void SendUDPMessage(string message)
        {
            if (isUDPSendConnected)
            {
                try
                {
                    IPEndPoint epAutoSteer = new IPEndPoint(epIP, Properties.Settings.Default.setIP_autoSteerPort);

                    // Get packet as byte array to send
                    byte[] byteData = Encoding.ASCII.GetBytes(message);
                    if (byteData.Length != 0)
                        sendSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, epAutoSteer, new AsyncCallback(SendData), null);
                }
                catch (Exception)
                {
                    //WriteErrorLog("Sending UDP Message" + e.ToString());
                    //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //sends byte array
        public void SendUDPMessage(byte[] byteData)
        {
            if (isUDPSendConnected)
            {
                try
                {
                    IPEndPoint epAutoSteer = new IPEndPoint(epIP, Properties.Settings.Default.setIP_autoSteerPort);

                    // Send packet to the zero
                    if (byteData.Length != 0)
                        sendSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, epAutoSteer, new AsyncCallback(SendData), null);
                }
                catch (Exception)
                {
                    //WriteErrorLog("Sending UDP Message" + e.ToString());
                    //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        //sends byte array
        public void SendUDPMessageNTRIP(byte[] byteData, int port)
        {
            if (isUDPSendConnected)
            {
                try
                {
                    IPEndPoint epAutoSteer = new IPEndPoint(epIP, port);

                    // Send packet to the zero
                    if (byteData.Length != 0)
                        sendSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, epAutoSteer, new AsyncCallback(SendData), null);
                }
                catch (Exception)
                {
                    //WriteErrorLog("Sending UDP Message" + e.ToString());
                    //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void SendData(IAsyncResult asyncResult)
        {
            try
            {
                sendSocket.EndSend(asyncResult);
            }
            catch (Exception)
            {
                //WriteErrorLog(" UDP Send Data" + e.ToString());
                //MessageBox.Show("SendData Error: " + e.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveData(IAsyncResult asyncResult)
        {
            try
            {
                // Initialise the IPEndPoint for the client
                EndPoint epSender = new IPEndPoint(IPAddress.Any, 0);

                // Receive all data
                int msgLen = recvSocket.EndReceiveFrom(asyncResult, ref epSender);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                recvSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epSender, new AsyncCallback(ReceiveData), epSender);

                //string text =  Encoding.ASCII.GetString(localMsg);
                
                int port = ((IPEndPoint)epSender).Port;
                // Update status through a delegate
                Invoke(updateRecvMessageDelegate, new object[] { port, localMsg });
            }
            catch (Exception)
            {
                //WriteErrorLog("UDP Recv data " + e.ToString());
                //MessageBox.Show("ReceiveData Error: " + e.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateRecvMessage(int port, byte[] data)
        {
            //update progress bar for autosteer
            if (pbarUDP++ > 99) pbarUDP = 0;

            if (data[0] == 0xB5 && data[1] == 0x62 && data[2] == 0x01)//Daniel P
            {
                if (data[3] == 0x07)//UBX-NAV-PVT
                {
                    if (data.Length == 100)
                    {
                        int CK_A = 0;
                        int CK_B = 0;

                        for (int j = 2; j < 98; j += 1)// start with Class and end by Checksum
                        {
                            CK_A = (CK_A + data[j]) & 0xFF;
                            CK_B = (CK_B + CK_A) & 0xFF;
                        }

                        if (data[98] == CK_A && data[99] == CK_B)
                        {
                            long itow = data[6] | (data[7] << 8) | (data[8] << 16) | (data[9] << 24);

                            if (data[84] == 0x00)
                            {
                                if ((data[27] & 0x81) == 0x81)
                                {
                                    pn.fixQuality = 4;
                                    pn.EnableHeadRoll = true;
                                }
                                else if ((data[27] & 0x41) == 0x41)
                                {
                                    pn.fixQuality = 5;
                                    pn.EnableHeadRoll = true;
                                }
                                else
                                {
                                    pn.fixQuality = 1;
                                    pn.EnableHeadRoll = false;
                                }

                                pn.satellitesTracked = data[29];

                                pn.longitude = (data[30] | (data[31] << 8) | (data[32] << 16) | (data[33] << 24)) * 0.0000001;//to deg
                                pn.latitude = (data[34] | (data[35] << 8) | (data[36] << 16) | (data[37] << 24)) * 0.0000001;//to deg
                                pn.altitude = (data[42] | (data[43] << 8) | (data[44] << 16) | (data[45] << 24)) * 0.001;//to meters

                                pn.hdop = (data[46] | (data[47] << 8) | (data[48] << 16) | (data[49] << 24)) * 0.01;

                                pn.UpdateNorthingEasting();

                                pn.speed = (data[66] | (data[67] << 8) | (data[68] << 16) | (data[69] << 24)) * 0.0036;//to km/h

                                //average the speed
                                pn.AverageTheSpeed();

                                recvSentenceSettings[2] = recvSentenceSettings[0];
                                recvSentenceSettings[0] = "$UBX-PVT, Longitude = " + pn.longitude.ToString("N7", CultureInfo.InvariantCulture) + ", Latitude = " + pn.latitude.ToString("N7", CultureInfo.InvariantCulture) + ", Altitude = " + pn.altitude.ToString("N3", CultureInfo.InvariantCulture) + ", itow = " + itow.ToString();
                            }
                            else
                            {
                                pn.fixQuality = 0;
                                recvSentenceSettings[2] = recvSentenceSettings[0];
                                recvSentenceSettings[0] = "$UBX-PVT, Longitude = ???, Latitude = ???, Altitude = ???, itow = " + itow.ToString();
                            }
                        }
                    }
                    return;
                }
                else if (data[3] == 0x3C)//UBX-NAV-RELPOSNED
                {
                    if (data.Length == 72)
                    {
                        int CK_A = 0;
                        int CK_B = 0;

                        for (int j = 2; j < 70; j += 1)// start with Class and end by Checksum
                        {
                            CK_A = (CK_A + data[j]) & 0xFF;
                            CK_B = (CK_B + CK_A) & 0xFF;
                        }

                        if (data[70] == CK_A && data[71] == CK_B)
                        {
                            long itow = data[10] | (data[11] << 8) | (data[12] << 16) | (data[13] << 24);

                            if (pn.EnableHeadRoll && ((data[67] & 0x01) == 0x01) && (((data[66] & 0x2D) == 0x2D) || ((data[66] & 0x35) == 0x35)))
                            {
                                int relposlength = data[26] | (data[27] << 8) | (data[28] << 16) | (data[29] << 24);//in cm!

                                if (pn.DualAntennaDistance - 5 < relposlength && relposlength < pn.DualAntennaDistance + 5)
                                {
                                    //save dist?
                                }

                                pn.headingHDT = (data[30] | (data[31] << 8) | (data[32] << 16) | (data[33] << 24)) * 0.00001;
                                ahrs.rollX16 = (int)(glm.toRadians(Math.Atan2((data[22] | (data[23] << 8) | (data[24] << 16) | (data[25] << 24)) + data[40] * 0.1, pn.DualAntennaDistance)) * 16);

                                recvSentenceSettings[3] = recvSentenceSettings[1];
                                recvSentenceSettings[1] = "$UBX-RELPOSNED, Heading = " + pn.headingHDT.ToString("N4", CultureInfo.InvariantCulture) + ", Roll = " + ahrs.rollX16.ToString("N4", CultureInfo.InvariantCulture) + ", itow = " + itow.ToString();
                            }
                            else //Bad Quality
                            {
                                ahrs.rollX16 = 9999;
                                pn.headingHDT = 9999;
                                recvSentenceSettings[3] = recvSentenceSettings[1];
                                recvSentenceSettings[1] = "$UBX-RELPOSNED, Heading = 9999, Roll = 9999, itow = " + itow.ToString();
                            }
                        }
                    }
                    return;
                }
            }
            else if (data[0] == 0x24)//if it starts with a $, its an nmea sentence
            {
                pn.rawBuffer += Encoding.ASCII.GetString(data);
                return;
            }

            if (data[0] == 0x23 && data[1] == 0x23)
            {
                string buff = Encoding.ASCII.GetString(data);
                return;
            }

            //quick check
            if (data.Length != 10) return;

            if (data[0] == 0x7F)
            {
                switch (data[1])
                {
                    //autosteer FD - 253
                    case 0xFD:
                        {
                            //Steer angle actual
                            actualSteerAngleDisp = (Int16)((data[2] << 8) + data[3]);

                            if (ahrs.isHeadingCorrectionFromAutoSteer)
                            {
                                ahrs.correctionHeadingX16 = (Int16)((data[4] << 8) + data[5]);
                            }

                            if (ahrs.isRollFromAutoSteer)
                            {
                                ahrs.rollX16 = (Int16)((data[6] << 8) + data[7]);
                            }

                            if (isJobStarted && mc.isWorkSwitchEnabled)
                            {
                                if ((!mc.isWorkSwitchActiveLow && (data[8] & 1) == 1) || (mc.isWorkSwitchActiveLow && (data[8] & 1) == 0))
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
                            if (ahrs.isAutoSteerAuto)
                            {
                                if (isJobStarted && !recPath.isDrivingRecordedPath && (ABLine.isBtnABLineOn || ct.isContourBtnOn || curve.isBtnCurveOn))
                                {
                                    if ((data[8] & 2) == 0)
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


                            mc.pwmDisplay = data[9];


                            break;
                        }

                    //From Machine Data
                    case 0xE0:
                        {
                            mc.recvUDPSentence = DateTime.Now.ToString() + "," + data[2].ToString();
                            break;
                        }

                    case 230:

                        checksumRecd = data[2];

                        if (checksumRecd != checksumSent)
                        {
                            MessageBox.Show(
                                "Sent: " + checksumSent + "\r\n Recieved: " + checksumRecd,
                                    "Checksum Error",
                                            MessageBoxButtons.OK, MessageBoxIcon.Question);
                        }

                        if (data[3] != inoVersionInt)
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
                            MessageBox.Show("Arduino INO Is Wrong Version \r\n Upload AutoSteer_USB_4201.INO ", gStr.gsFileError,
                                                MessageBoxButtons.OK, MessageBoxIcon.Question);
                            Close();
                        }

                        break;

                    //lidar
                    case 0xF1:
                        {
                            mc.lidarDistance = (Int16)((data[2] << 8) + data[3]);
                            //mc.recvUDPSentence = DateTime.Now.ToString() + "," + mc.lidarDistance.ToString();
                            break;
                        }

                    //Ext UDP IMU
                    case 0xEE:
                        {
                            //by Matthias Hammer Jan 2019
                            if (ahrs.isHeadingCorrectionFromExtUDP)
                            {
                                ahrs.correctionHeadingX16 = (Int16)((data[4] << 8) + data[5]);
                            }

                            if (ahrs.isRollFromOGI)
                            {
                                ahrs.rollX16 = (Int16)((data[6] << 8) + data[7]);
                            }

                            break;
                        }

                    case 0xF9://MTZ8302 Feb 2020
                        {
                            /*rate stuff
                            
                            //left or single actual rate
                            //int.TryParse(data[0], out mc.incomingInt);
                            mc.rateActualLeft = (double)data[2] * 0.01;
                            //right actual rate
                            mc.rateActualRight = (double)data[3] * 0.01;
                            //Volume for dual and single
                            mc.dualVolumeActual = data[4];

                            rate stuff  */



                            //header
                            mc.ss[mc.swHeaderLo] = 0xF9;
                            mc.ss[mc.swONHi] = data[5];  //Section On status
                            mc.ss[mc.swONLo] = data[6];  //Section On status
                            mc.ss[mc.swOFFHi] = data[7];//Section Auto status(only when Section On)
                            mc.ss[mc.swOFFLo] = data[8];//Section Auto status(only when Section On)
                            mc.ss[mc.swMain] = data[9];  //read MainSW+RateSW
                            break;
                        }
                }
            }
        }

        #region keystrokes
        //keystrokes for easy and quick startup
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            //reset Sim
            if (keyData == Keys.L)
            {
                btnResetSim.PerformClick();
                return true;
            }

            //speed up
            if (keyData == Keys.Up)
            {
                if (sim.stepDistance < 1) sim.stepDistance += 0.04;
                else sim.stepDistance += 0.055;
                if (sim.stepDistance > 27.77) sim.stepDistance = 27.77;
                hsbarStepDistance.Value = (int)(sim.stepDistance * 3.6);
                return true;
            }

            //slow down
            if (keyData == Keys.Down)
            {
                if (sim.stepDistance < 1) sim.stepDistance -= 0.04;
                else sim.stepDistance -= 0.055;
                if (sim.stepDistance < -6.94) sim.stepDistance = -6.94;
                hsbarStepDistance.Value = (int)(sim.stepDistance * 3.6);
                return true;
            }

            //Stop
            if (keyData == Keys.OemPeriod)
            {
                sim.stepDistance = 0;
                hsbarStepDistance.Value = 0;
                return true;
            }

            //turn right
            if (keyData == Keys.Right)
            {
                sim.steerAngle+=2;
                if (sim.steerAngle > 40) sim.steerAngle = 40;
                if (sim.steerAngle < -40) sim.steerAngle = -40;
                sim.steerAngleScrollBar = sim.steerAngle;
                btnResetSteerAngle.Text = sim.steerAngle.ToString();
                hsbarSteerAngle.Value = (int)(10 * sim.steerAngle) + 400;
                return true;
            }

            //turn left
            if (keyData == Keys.Left)
            {
                sim.steerAngle-=2;
                if (sim.steerAngle > 40) sim.steerAngle = 40;
                if (sim.steerAngle < -40) sim.steerAngle = -40;
                sim.steerAngleScrollBar = sim.steerAngle;
                btnResetSteerAngle.Text = sim.steerAngle.ToString();
                hsbarSteerAngle.Value = (int)(10 * sim.steerAngle) + 400;
                return true;
            }

            //zero steering
            if (keyData == Keys.OemQuestion)
            {
                sim.steerAngle = 0.0;
                sim.steerAngleScrollBar = sim.steerAngle;
                btnResetSteerAngle.Text = sim.steerAngle.ToString();
                hsbarSteerAngle.Value = (int)(10 * sim.steerAngle) + 400;
                return true;
            }

            if (keyData == (Keys.F))
            {
                JobNewOpenResume();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.A)) //autosteer button on off
            {
                btnAutoSteer.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            //if (keyData == (Keys.S)) //open the steer chart
            //{
            //    toolstripAutoSteerConfig.PerformClick();
            //    return true;    // indicate that you handled this keystroke
            //}

            if (keyData == (Keys.S)) //open the steer chart
            {
                btnContourPriority.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.C)) //open the steer chart
            {
                toolStripAutoSteerChart.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.V)) //open the vehicle Settings
            {
                toolstripVehicleConfig.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.U)) //open the UTurn Settings
            {
                toolstripYouTurnConfig.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.NumPad1)) //auto section on off
            {
                btnAutoSection.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.N)) //auto section on off
            {
                btnAutoSection.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.NumPad0)) //auto section on off
            {
                btnManualSection.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.M)) //auto section on off
            {
                btnManualSection.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.G)) // Flag click
            {
                btnFlag.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.P)) // Snap/Prioritu click
            {
                btnContourPriority.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            if (keyData == (Keys.F11)) // Full Screen click
            {
                btnFullScreen.PerformClick();
                return true;    // indicate that you handled this keystroke
            }

            // Call the base class
            return base.ProcessCmdKey(ref msg, keyData);
        }
        #endregion

        #region Gesture

        // Private variables used to maintain the state of gestures
        //private DrawingObject _dwo = new DrawingObject();
        private Point _ptFirst = new Point();

        private Point _ptSecond = new Point();
        private int _iArguments = 0;

        // One of the fields in GESTUREINFO structure is type of Int64 (8 bytes).
        // The relevant gesture information is stored in lower 4 bytes. This
        // bit mask is used to get 4 lower bytes from this argument.
        private const Int64 ULL_ARGUMENTS_BIT_MASK = 0x00000000FFFFFFFF;

        //-----------------------------------------------------------------------
        // Multitouch/Touch glue (from winuser.h file)
        // Since the managed layer between C# and WinAPI functions does not
        // exist at the moment for multi-touch related functions this part of
        // code is required to replicate definitions from winuser.h file.
        //-----------------------------------------------------------------------
        // Touch event window message constants [winuser.h]
        private const int WM_GESTURENOTIFY = 0x011A;

        private const int WM_GESTURE = 0x0119;

        private const int GC_ALLGESTURES = 0x00000001;

        // Gesture IDs
        private const int GID_BEGIN = 1;

        private const int GID_END = 2;
        private const int GID_ZOOM = 3;
        private const int GID_PAN = 4;
        private const int GID_ROTATE = 5;
        private const int GID_TWOFINGERTAP = 6;


        private const int GID_PRESSANDTAP = 7;

        // Gesture flags - GESTUREINFO.dwFlags
        private const int GF_BEGIN = 0x00000001;

        private const int GF_INERTIA = 0x00000002;
        private const int GF_END = 0x00000004;

        //
        // Gesture configuration structure
        //   - Used in SetGestureConfig and GetGestureConfig
        //   - Note that any setting not included in either GESTURECONFIG.dwWant
        //     or GESTURECONFIG.dwBlock will use the parent window's preferences
        //     or system defaults.
        //
        // Touch API defined structures [winuser.h]
        [StructLayout(LayoutKind.Sequential)]
        private struct GESTURECONFIG
        {
            public int dwID;    // gesture ID
            public int dwWant;  // settings related to gesture ID that are to be

            // turned on
            public int dwBlock; // settings related to gesture ID that are to be

            // turned off
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct POINTS
        {
            public short x;
            public short y;
        }

        //
        // Gesture information structure
        //   - Pass the HGESTUREINFO received in the WM_GESTURE message lParam
        //     into the GetGestureInfo function to retrieve this information.
        //   - If cbExtraArgs is non-zero, pass the HGESTUREINFO received in
        //     the WM_GESTURE message lParam into the GetGestureExtraArgs
        //     function to retrieve extended argument information.
        //
        [StructLayout(LayoutKind.Sequential)]
        private struct GESTUREINFO
        {
            public int cbSize;           // size, in bytes, of this structure

            // (including variable length Args
            // field)
            public int dwFlags;          // see GF_* flags

            public int dwID;             // gesture ID, see GID_* defines
            public IntPtr hwndTarget;    // handle to window targeted by this

            // gesture
            [MarshalAs(UnmanagedType.Struct)]
            internal POINTS ptsLocation; // current location of this gesture

            public int dwInstanceID;     // internally used
            public int dwSequenceID;     // internally used
            public Int64 ullArguments;   // arguments for gestures whose

            // arguments fit in 8 BYTES
            public int cbExtraArgs;      // size, in bytes, of extra arguments,

            // if any, that accompany this gesture
        }

        // Currently touch/multitouch access is done through unmanaged code
        // We must p/invoke into user32 [winuser.h]
        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetGestureConfig(IntPtr hWnd, int dwReserved, int cIDs, ref GESTURECONFIG pGestureConfig, int cbSize);

        [DllImport("user32")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GetGestureInfo(IntPtr hGestureInfo, ref GESTUREINFO pGestureInfo);

        // size of GESTURECONFIG structure
        private int _gestureConfigSize;

        // size of GESTUREINFO structure
        private int _gestureInfoSize;

        [SecurityPermission(SecurityAction.Demand)]
        private void SetupStructSizes()
        {
            // Both GetGestureCommandInfo and GetTouchInputInfo need to be
            // passed the size of the structure they will be filling
            // we get the sizes upfront so they can be used later.
            _gestureConfigSize = Marshal.SizeOf(new GESTURECONFIG());
            _gestureInfoSize = Marshal.SizeOf(new GESTUREINFO());
        }

        //-------------------------------------------------------------
        // Since there is no managed layer at the moment that supports
        // event handlers for WM_GESTURENOTIFY and WM_GESTURE
        // messages we have to override WndProc function
        //
        // in
        //   m - Message object
        //-------------------------------------------------------------
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        protected override void WndProc(ref Message m)
        {
            bool handled = false;

            switch (m.Msg)
            {
                case WM_GESTURENOTIFY:
                    {
                        // This is the right place to define the list of gestures
                        // that this application will support. By populating
                        // GESTURECONFIG structure and calling SetGestureConfig
                        // function. We can choose gestures that we want to
                        // handle in our application. In this app we decide to
                        // handle all gestures.
                        GESTURECONFIG gc = new GESTURECONFIG
                        {
                            dwID = 0,                // gesture ID
                            dwWant = GC_ALLGESTURES, // settings related to gesture
                                                     // ID that are to be turned on
                            dwBlock = 0 // settings related to gesture ID that are
                        };
                        // to be

                        // We must p/invoke into user32 [winuser.h]
                        bool bResult = SetGestureConfig(
                            Handle, // window for which configuration is specified
                            0,      // reserved, must be 0
                            1,      // count of GESTURECONFIG structures
                            ref gc, // array of GESTURECONFIG structures, dwIDs
                                    // will be processed in the order specified
                                    // and repeated occurances will overwrite
                                    // previous ones
                            _gestureConfigSize // sizeof(GESTURECONFIG)
                        );

                        if (!bResult)
                        {
                            throw new Exception("Error in execution of SetGestureConfig");
                        }
                    }
                    handled = true;
                    break;

                case WM_GESTURE:
                    // The gesture processing code is implemented in
                    // the DecodeGesture method
                    handled = DecodeGesture(ref m);
                    break;

                default:
                    handled = false;
                    break;
            }

            // Filter message back up to parents.
            base.WndProc(ref m);

            if (handled)
            {
                // Acknowledge event if handled.
                try
                {
                    m.Result = new System.IntPtr(1);
                }
                catch (Exception)
                {
                }
            }
        }

        // Taken from GCI_ROTATE_ANGLE_FROM_ARGUMENT.
        // Converts from "binary radians" to traditional radians.
        static protected double ArgToRadians(Int64 arg)
        {
            return (arg / 65535.0 * 4.0 * 3.14159265) - (2.0 * 3.14159265);
        }

        // Handler of gestures
        //in:
        //  m - Message object
        private bool DecodeGesture(ref Message m)
        {
            GESTUREINFO gi;

            try
            {
                gi = new GESTUREINFO();
            }
            catch (Exception)
            {
                return false;
            }

            gi.cbSize = _gestureInfoSize;

            // Load the gesture information.
            // We must p/invoke into user32 [winuser.h]
            if (!GetGestureInfo(m.LParam, ref gi))
            {
                return false;
            }

            switch (gi.dwID)
            {
                case GID_BEGIN:
                case GID_END:
                    break;

                case GID_ZOOM:
                    switch (gi.dwFlags)
                    {
                        case GF_BEGIN:
                            _iArguments = (int)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK);
                            _ptFirst.X = gi.ptsLocation.x;
                            _ptFirst.Y = gi.ptsLocation.y;
                            _ptFirst = PointToClient(_ptFirst);
                            break;

                        default:
                            // We read here the second point of the gesture. This
                            // is middle point between fingers in this new
                            // position.
                            _ptSecond.X = gi.ptsLocation.x;
                            _ptSecond.Y = gi.ptsLocation.y;
                            _ptSecond = PointToClient(_ptSecond);
                            {
                                // The zoom factor is the ratio of the new
                                // and the old distance. The new distance
                                // between two fingers is stored in
                                // gi.ullArguments (lower 4 bytes) and the old
                                // distance is stored in _iArguments.
                                double k = (double)(_iArguments)
                                            / (double)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK);
                                //lblX.Text = k.ToString();
                                camera.zoomValue *= k;
                                if (camera.zoomValue < 6.0) camera.zoomValue = 6;
                                camera.camSetDistance = camera.zoomValue * camera.zoomValue * -1;
                                SetZoom();
                            }

                            // Now we have to store new information as a starting
                            // information for the next step in this gesture.
                            _ptFirst = _ptSecond;
                            _iArguments = (int)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK);
                            break;
                    }
                    break;

                //case GID_PAN:
                //    switch (gi.dwFlags)
                //    {
                //        case GF_BEGIN:
                //            _ptFirst.X = gi.ptsLocation.x;
                //            _ptFirst.Y = gi.ptsLocation.y;
                //            _ptFirst = PointToClient(_ptFirst);
                //            break;

                //        default:
                //            // We read the second point of this gesture. It is a
                //            // middle point between fingers in this new position
                //            _ptSecond.X = gi.ptsLocation.x;
                //            _ptSecond.Y = gi.ptsLocation.y;
                //            _ptSecond = PointToClient(_ptSecond);

                //            // We apply move operation of the object
                //            _dwo.Move(_ptSecond.X - _ptFirst.X, _ptSecond.Y - _ptFirst.Y);

                //            Invalidate();

                //            // We have to copy second point into first one to
                //            // prepare for the next step of this gesture.
                //            _ptFirst = _ptSecond;
                //            break;
                //    }
                //    break;

                case GID_ROTATE:
                    switch (gi.dwFlags)
                    {
                        case GF_BEGIN:
                            _iArguments = 32768;
                            break;

                        default:
                            // Gesture handler returns cumulative rotation angle. However we
                            // have to pass the delta angle to our function responsible
                            // to process the rotation gesture.
                            double k = ((int)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK) - _iArguments) * 0.01;
                            camera.camPitch -= k;
                            if (camera.camPitch < -74) camera.camPitch = -74;
                            if (camera.camPitch > 0) camera.camPitch = 0;
                            _iArguments = (int)(gi.ullArguments & ULL_ARGUMENTS_BIT_MASK);
                            break;
                    }
                    break;

                    //case GID_TWOFINGERTAP:
                    //    // Toggle drawing of diagonals
                    //    _dwo.ToggleDrawDiagonals();
                    //    Invalidate();
                    //    break;

                    //case GID_PRESSANDTAP:
                    //    if (gi.dwFlags == GF_BEGIN)
                    //    {
                    //        // Shift drawing color
                    //        _dwo.ShiftColor();
                    //        Invalidate();
                    //    }
                    //    break;
            }

            return true;
        }

        #endregion Gesture

    }
}
