using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace AgOpenGPS
{
    public class CTraffic
    {
        public int cntrPGNFromAOG = 0;
        public int cntrPGNToAOG = 0;

        public int cntrUDPOut = 0;
        public int cntrUDPIn = 0;

        public int cntrGPSIn = 0;
        public int cntrGPSOut = 0;

        public int cntrGPS2In = 0;
        public int cntrGPS2Out = 0;

        public int cntrSteerIn = 0;
        public int cntrSteerOut = 0;

        public int cntrMachineIn = 0;
        public int cntrMachineOut = 0;

        public bool isTrafficOn = true;

        public int enableCounter = 0;

        //0x7FD0
        public byte[] looperStatus = new byte[] { 0x80, 0x81, 0x7f, 0xD2, 0x08, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
    }

    public partial class FormLoop
    {
        // Server socket
        private Socket recvFrom_AgOpenGPS_Socket, sendTo_AgOpenGPS_Socket, UDP_sendSocket, UDP_recvSocket;

        CTraffic traffic = new CTraffic();

        EndPoint sendTo_AgOpenGPS_EndPoint, recvFrom_AgOpenGPS_EndPoint;

        // Data stream
        private byte[] buffer = new byte[1024];

        // Status delegate
        private delegate void UpdateStatusDelegate(int port, byte[] msg);
        private UpdateStatusDelegate updateStatusDelegate = null;

        // Send and Recv socket for udp network
        IPEndPoint epAutoSteer = new IPEndPoint(IPAddress.Any, 9998);
        private bool isUDPNetworkConnected;

        //IP address and port of Auto Steer server
        IPAddress epIP = IPAddress.Parse(AgOpenGPS.Properties.Settings.Default.setIP_autoSteerIP);

        // Status delegate
        private delegate void UpdateRecvMessageDelegate(int port, byte[] msg);
        private UpdateRecvMessageDelegate updateRecvMessageDelegate = null;

        //initialize loopback and udp network
        private void LoadUDPNetwork()
        {
            try //udp network
            {
                // Initialise the delegate which updates the message received
                updateRecvMessageDelegate = ReceiveFromUDP;

                // Initialise the socket
                UDP_sendSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                UDP_recvSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

                UDP_sendSocket.EnableBroadcast = true;
                UDP_recvSocket.EnableBroadcast = true;

                // Initialise the IPEndPoint for the server and listen on port 9999
                IPEndPoint recv = new IPEndPoint(IPAddress.Any, AgOpenGPS.Properties.Settings.Default.setIP_thisPort);

                // Associate the socket with this IP address and port
                UDP_recvSocket.Bind(recv);

                // Initialise the IPEndPoint for the server to send on port 9998
                IPEndPoint server = new IPEndPoint(IPAddress.Any, 9998);
                //sendSocket.Bind(server);
                IPEndPoint epAutoSteer = new IPEndPoint(epIP, 8888);

                // Initialise the IPEndPoint for the client - async listner client only!
                EndPoint client = new IPEndPoint(IPAddress.Any, 0);

                // Start listening for incoming data
                UDP_recvSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref client, new AsyncCallback(ReceiveDataUDPAsync), UDP_recvSocket);
                isUDPNetworkConnected = true;
            }
            catch (Exception e)
            {
                //WriteErrorLog("UDP Server" + e);
                MessageBox.Show("Load Error: " + e.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadLoopback()
        {
            try //loopback
            {
                // Initialise the delegate which updates the status
                updateStatusDelegate = new UpdateStatusDelegate(ReceiveFromLoopBack);

                recvFrom_AgOpenGPS_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                recvFrom_AgOpenGPS_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                recvFrom_AgOpenGPS_Socket.Bind(new IPEndPoint(IPAddress.Any, 17777));
                recvFrom_AgOpenGPS_EndPoint = new IPEndPoint(IPAddress.Loopback, 15550);
                recvFrom_AgOpenGPS_Socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref recvFrom_AgOpenGPS_EndPoint, new AsyncCallback(ReceiveDataLoopAsync), recvFrom_AgOpenGPS_Socket);
                //15550 --> 17777

                sendTo_AgOpenGPS_Socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
                sendTo_AgOpenGPS_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                sendTo_AgOpenGPS_Socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.Broadcast, true);
                sendTo_AgOpenGPS_Socket.Bind(new IPEndPoint(IPAddress.Any, 17770));
                sendTo_AgOpenGPS_EndPoint = new IPEndPoint(IPAddress.Loopback, 15555);
                //17770 --> 15555
            }
            catch (Exception ex)
            {
                //lblStatus.Text = "Error";
                MessageBox.Show("Load Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //loopback functions
        #region Send And Receive
        private void SendToLoopBackMessage(string message)
        {
            try
            {
                // Get packet as byte array
                byte[] byteData = Encoding.ASCII.GetBytes(message);

                if (byteData.Length != 0)
                {
                    traffic.cntrPGNToAOG += byteData.Length;

                    // Send packet to the zero
                    sendTo_AgOpenGPS_Socket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, sendTo_AgOpenGPS_EndPoint,
                            new AsyncCallback(SendDataLoopAsync), null);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Send Error: " + ex.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SendToLoopBackMessage(byte[] byteData)
        {
            try
            {
                if (byteData.Length != 0)
                {
                    traffic.cntrPGNToAOG += byteData.Length;

                    int crc = 0;
                    for (int i = 2; i + 1 < byteData.Length; i++)
                    {
                        crc += byteData[i];
                    }
                    byteData[byteData.Length - 1] = (byte)crc;

                    // Send packet to the zero
                    sendTo_AgOpenGPS_Socket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, sendTo_AgOpenGPS_EndPoint,
                        new AsyncCallback(SendDataLoopAsync), null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Send Error: " + ex.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveFromLoopBack(int port, byte[] data)
        {
            if (data.Length != 10)
            {
            }
            //return;

            traffic.cntrPGNFromAOG += data.Length;

            //Send out to udp network
            if (isUDPNetworkConnected) SendUDPMessage(data);

            //lblIncoming.Text = (data[0].ToString() + "," + data[1].ToString()
            //    + "," + data[2].ToString() + "," + data[3].ToString()
            //    + "," + data[4].ToString() + "," + data[5].ToString()
            //    + "," + data[6].ToString() + "," + data[7].ToString()
            //    + "," + data[8].ToString() + "," + data[9].ToString()
            //    + " \r\n");


            if (data[0] == 127)
            {
                switch (data[1])
                {
                    //machine send back F8 248
                    case 0xD0:
                        {
                            currentLat = BitConverter.ToDouble(data, 2);
                            break;
                        }
                    case 0xD1:
                        {
                            currentLon = BitConverter.ToDouble(data, 2);
                            break;
                        }
                }
            }
        }

        public void SendDataLoopAsync(IAsyncResult asyncResult)
        {
            try
            {
                sendTo_AgOpenGPS_Socket.EndSend(asyncResult);
            }
            catch (Exception ex)
            {
                MessageBox.Show("SendData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveDataLoopAsync(IAsyncResult asyncResult)
        {
            try
            {
                // Receive all data
                int msgLen = recvFrom_AgOpenGPS_Socket.EndReceiveFrom(asyncResult, ref recvFrom_AgOpenGPS_EndPoint);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                recvFrom_AgOpenGPS_Socket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref recvFrom_AgOpenGPS_EndPoint, new AsyncCallback(ReceiveDataLoopAsync), recvFrom_AgOpenGPS_Socket);

                //string text = Encoding.ASCII.GetString(localMsg);

                // Update status through a delegate
                int port = ((IPEndPoint)recvFrom_AgOpenGPS_EndPoint).Port;
                Invoke(updateStatusDelegate, new object[] { port, localMsg });
            }
            catch (Exception)
            {
                //MessageBox.Show("ReceiveData Error: " + ex.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        //sends byte array
        public void SendUDPMessage(byte[] byteData)
        {
            if (isUDPNetworkConnected)
            {
                try
                {

                    // Send packet to the zero
                    if (byteData.Length != 0)
                        UDP_sendSocket.BeginSendTo(byteData, 0, byteData.Length, SocketFlags.None, epAutoSteer, new AsyncCallback(SendDataUDPAsync), null);

                    traffic.cntrUDPOut++;
                }
                catch (Exception)
                {
                    //WriteErrorLog("Sending UDP Message" + e.ToString());
                    //MessageBox.Show("Send Error: " + e.Message, "UDP Client", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ReceiveFromUDP(int port, byte[] data)
        {
            //update progress bar for autosteer
            //if (pbarUDP++ > 98) pbarUDP = 0;

            //if it starts with a $, its an nmea sentence
            if (data[0] == 36)
            {
                //pn.rawBuffer += Encoding.ASCII.GetString(data);
                return;
            }

            //if (data[0] == 35 && data[1] == 35)
            //{
            //    string buff = Encoding.ASCII.GetString(data);
            //    return;
            //}

            //quick check
            if (data.Length != 10) return;

            if (data[0] == 127)
            {
                switch (data[1])
                {
                    //autosteer FD - 253
                    case 253:
                        {
                            ////Steer angle actual
                            //double actualSteerAngle = (Int16)((data[2] << 8) + data[3]);

                            ////build string for display
                            //double setSteerAngle = guidanceLineSteerAngle;

                            //if (ahrs.isHeadingCorrectionFromAutoSteer)
                            //{
                            //    ahrs.correctionHeadingX16 = (Int16)((data[4] << 8) + data[5]);
                            //}

                            //if (ahrs.isRollFromAutoSteer)
                            //{
                            //    ahrs.rollX16 = (Int16)((data[6] << 8) + data[7]);
                            //}

                            //mc.steerSwitchValue = data[8];
                            //mc.workSwitchValue = mc.steerSwitchValue & 1;
                            //mc.steerSwitchValue = mc.steerSwitchValue & 2;

                            //byte pwm = data[9];

                            //actualSteerAngleDisp = actualSteerAngle;
                            break;
                        }
                }
            }
        }

        private void SendDataUDPAsync(IAsyncResult asyncResult)
        {
            try
            {
                UDP_sendSocket.EndSend(asyncResult);
            }
            catch (Exception)
            {
                //WriteErrorLog(" UDP Send Data" + e.ToString());
                //MessageBox.Show("SendData Error: " + e.Message, "UDP Server", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReceiveDataUDPAsync(IAsyncResult asyncResult)
        {
            try
            {
                // Initialise the IPEndPoint for the client
                EndPoint epSender = new IPEndPoint(IPAddress.Any, 0);

                // Receive all data
                int msgLen = UDP_recvSocket.EndReceiveFrom(asyncResult, ref epSender);

                byte[] localMsg = new byte[msgLen];
                Array.Copy(buffer, localMsg, msgLen);

                // Listen for more connections again...
                UDP_recvSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref epSender,
                    new AsyncCallback(ReceiveDataUDPAsync), UDP_recvSocket);

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
    }
}
