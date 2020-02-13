using System.IO.Ports;
using System;
using System.Windows.Forms;
using System.Globalization;
using AgOpenGPS.Properties;

namespace AgOpenGPS
{
    public partial class FormGPS
    {
        public static string portNameGPS = "COM GPS";
        public static int baudRateGPS = 4800;

        public static string portNameGPS2 = "COM GPS";
        public static int baudRateGPS2 = 4800;

        public static string portNameRelaySection = "COM Sect";
        public static int baudRateRelaySection = 38400;

        public static string portNameAutoSteer = "COM AS";
        public static int baudRateAutoSteer = 38400;

        public bool isJRK;

        public string NMEASentence = "No Data";

        //used to decide to autoconnect section arduino this run
        public bool wasRateRelayConnectedLastRun = false;
        public string[] recvSentenceSettings = new string[4];

        //used to decide to autoconnect autosteer arduino this run
        public bool wasAutoSteerConnectedLastRun = false;

        //serial port gps is connected to
        public SerialPort SerialGPS = new SerialPort(portNameGPS, baudRateGPS, Parity.None, 8, StopBits.One);
        public SerialPort SerialGPS2 = new SerialPort(portNameGPS2, baudRateGPS2, Parity.None, 8, StopBits.One);

        //serial port Arduino is connected to
        public SerialPort spRelay = new SerialPort(portNameRelaySection, baudRateRelaySection, Parity.None, 8, StopBits.One);

        //serial port AutoSteer is connected to
        public SerialPort spAutoSteer = new SerialPort(portNameAutoSteer, baudRateAutoSteer, Parity.None, 8, StopBits.One);


        public byte[] rawBuffer = new byte[500];
        public byte[] Header = { 0xB5, 0x62 };
        public int BytesRead = 0;
        public int MessageLength = 0;
        int CK_A = 0, CK_B = 0;
        public double HeadDif = 0;
        public double DualGPSDistance = 0;
        public bool EnableLatLon = true;
        public long lastitow = 0;

        #region AutoSteerPort // --------------------------------------------------------------------

        public void AutoSteerDataOutToPort()
        {
            //default to a stop initially
            //mc.machineControlData[mc.cnPedalControl] &= 0b00111111;

            //if (isInAutoDrive) //Is in Auto Drive Mode enabled
            //{
            //    if (!ast.isInFreeDriveMode)
            //    {
            //        //make it go - or with 1
            //        if (recPath.isDrivingRecordedPath)
            //        {
            //            mc.machineControlData[mc.cnPedalControl] |= 0b11000000;
            //        }

            //        if (self.isSelfDriving)
            //        {
            //            mc.machineControlData[mc.cnPedalControl] |= 0b11000000;
            //        }
            //    }
            //    else //in AutoDrive and FreeDrive
            //    {
            //        mc.machineControlData[mc.cnPedalControl] |= 0b11000000;
            //    }

            //    ////Is there something in the way?
            //    //if (isLidarBtnOn && (mc.lidarDistance > 200 && mc.lidarDistance < 1000))
            //    //{
            //    //    mc.machineControlData[mc.cnPedalControl] &= 0b00111111;
            //    //}
            //}
            //else // Auto/Manual is in Manual so release the clutch only
            //{
            //    //release the clutch for manual driving
            //    mc.machineControlData[mc.cnPedalControl] |= 0b01000000;
            //    mc.machineControlData[mc.cnPedalControl] &= 0b01111111;
            //}

            ////pause the thing if paused. Duh.
            //if (recPath.isPausedDrivingRecordedPath)
            //{
            //    mc.machineControlData[mc.cnPedalControl] &= 0b00111111;
            //}

            ////gone out of bounds so full stop.
            //if (mc.isOutOfBounds)
            //{
            //    mc.machineControlData[mc.cnPedalControl] &= 0b00111111;
            //}

            //add the out of bounds bit to uturn byte bit 7
            if (mc.isOutOfBounds) 
                mc.autoSteerData[mc.sdYouTurnByte] |= 0b10000000;            
            else 
                mc.autoSteerData[mc.sdYouTurnByte] &= 0b01111111;

            //send out to network
            if (Properties.Settings.Default.setUDP_isOn)
            {
                //machine control
                //SendUDPMessage(mc.machineControlData);

                //send autosteer since it never is logic controlled
                SendUDPMessage(mc.autoSteerData);

                //rate control
                SendUDPMessage(mc.relayData);
            }

            //Tell Arduino the steering parameter values
            if (spAutoSteer.IsOpen)
            {
                if (isJRK)
                {
                    byte[] command = new byte[2];
                    int target;
                    target = guidanceLineSteerAngle * Properties.Settings.Default.setAS_countsPerDegree;
                    target /= 100;
                    target += ((Properties.Settings.Default.setAS_steerAngleOffset - 127) * 5); //steeroffstet                   
                    target += 2047; //steerangle center
                    
                    if (target > 4075) target = 4075;
                    if (target < 0) target = 0;
                    command[0] = unchecked((byte)(0xC0 + (target & 0x1F)));
                    command[1] = unchecked((byte)((target >> 5) & 0x7F));
                    spAutoSteer.Write(command, 0, 2);

                    ////send get scaledfeedback command
                    byte[] command2 = new byte[1];
                    command2[0] = 0xA7;

                    spAutoSteer.Write(command2, 0, 1);

                }
                else
                {
                    try { spAutoSteer.Write(mc.autoSteerData, 0, CModuleComm.pgnSentenceLength); }
                    catch (Exception e)
                    {
                        WriteErrorLog("Out Data to Steering Port " + e.ToString());
                        SerialPortAutoSteerClose();
                    }
                }                
            } 
        }

        public void AutoSteerSettingsOutToPort()
        {
            //send out the settings
            //send out to network
            if (Properties.Settings.Default.setUDP_isOn)
            {
                SendUDPMessage(mc.autoSteerSettings);
            }

            //Tell Arduino autoSteer settings
            if (spAutoSteer.IsOpen)
            {
                try { spAutoSteer.Write(mc.autoSteerSettings, 0, CModuleComm.pgnSentenceLength ); }
                catch (Exception e)
                {
                    WriteErrorLog("Out Settings to Steer Port " + e.ToString());
                    SerialPortAutoSteerClose();
                }
            }
        }




        //called by the AutoSteer module delegate every time a chunk is rec'd
        public double actualSteerAngleDisp = 0;


        //the delegate for thread
        private delegate void LineReceivedEventHandlerAutoSteer(string sentence);

        //Arduino serial port receive in its own thread
        private void sp_DataReceivedAutoSteer(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spAutoSteer.IsOpen)
            {
                if (!Properties.Settings.Default.setAS_isJRK)
                {
                    try
                    {
                        string sentence = spAutoSteer.ReadLine();
                        this.BeginInvoke(new LineReceivedEventHandlerAutoSteer(SerialLineReceivedAutoSteer), sentence);
                    }
                    //this is bad programming, it just ignores errors until its hooked up again.
                    catch (Exception ex)
                    {
                        WriteErrorLog("AutoSteer Recv" + ex.ToString());
                    }

                }
                else   //get 2 byte feedback from pololu
                {
                    byte[] buffer = new byte[2];
                    spAutoSteer.Read(buffer, 0, 2);
                    int feedback = buffer[0] + 256 * buffer[1];

                    actualSteerAngleDisp = feedback - 2047;
                    //andreas ortner changed this one
                    //actualSteerAngleDisp -= (Properties.Settings.Default.setAS_steerAngleOffset - 127) * 5;
                    actualSteerAngleDisp += (Properties.Settings.Default.setAS_steerAngleOffset - 127) * 5;
                    actualSteerAngleDisp /= Properties.Settings.Default.setAS_countsPerDegree;
                    actualSteerAngleDisp *= 100;
                }
            }
        }

        private void SerialLineReceivedAutoSteer(string sentence)
        {
            //spit it out no matter what it says
            mc.serialRecvAutoSteerStr = sentence;

            //0 - actual steer angle*100, 1 - setpoint steer angle*100, 2 - heading in degrees * 16, 3 - roll in degrees * 16, 4 - steerSwitch position

            string[] words = mc.serialRecvAutoSteerStr.Split(',');
            if (words.Length == 5)
            {
                //update the progress bar for autosteer.
                if (pbarSteer++ > 99) pbarSteer=0;

                double.TryParse(words[0], NumberStyles.Float, CultureInfo.InvariantCulture, out actualSteerAngleDisp);
               

                //first 2 used for display mainly in autosteer window chart as strings
                //parse the values
                if (ahrs.isHeadingFromAutoSteer)
                {

                    int.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out ahrs.correctionHeadingX16);
                }

                if (ahrs.isRollFromAutoSteer) int.TryParse(words[3], NumberStyles.Float, CultureInfo.InvariantCulture, out ahrs.rollX16);

                int.TryParse(words[4], out mc.steerSwitchValue);
                mc.workSwitchValue = mc.steerSwitchValue & 1;
                mc.steerSwitchValue = mc.steerSwitchValue & 2;
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

        #region RelaySerialPort //--------------------------------------------------------------------

        //build the byte for individual realy control
        private void BuildRelayByte()
        {
            int set = 1;
            int reset = 2046;
            mc.relayData[mc.rdSectionControlByteHi] = (byte)0;
            mc.relayData[mc.rdSectionControlByteLo] = (byte)0;

            int relay = 0;

            //check if super section is on
            if (section[tool.numOfSections].isSectionOn)
            {
                for (int j = 0; j < tool.numOfSections; j++)
                {
                    //all the sections are on, so set them
                    relay = relay | set;
                    set = (set << 1);
                }
            }

            else
            {
                for (int j = 0; j < MAXSECTIONS; j++)
                {
                    //set if on, reset bit if off
                    if (section[j].isSectionOn) relay = relay | set;
                    else relay = relay & reset;

                    //move set and reset over 1 bit left
                    set = (set << 1);
                    reset = (reset << 1);
                    reset = (reset + 1);
                }
            }

            //rate port gets high and low byte
            mc.relayData[mc.rdSectionControlByteHi] = unchecked((byte)(relay >> 8));
            mc.relayData[mc.rdSectionControlByteLo] = unchecked((byte)relay);


            //autosteer gets only the first 8 sections
            mc.autoSteerData[mc.sdRelayLo] = unchecked((byte)(mc.relayData[mc.rdSectionControlByteLo]));
        }

        //Send relay info out to relay board
        public void RelayOutToPort(byte[] items, int numItems)
        {
            //load the uturn byte with the accumulated spacing
            if (vehicle.treeSpacing != 0) mc.relayData[mc.rdTree] = unchecked((byte)((treeTrigger == true) ? 1 : 0));

            //grab the youturn byte
            mc.relayData[mc.rdUTurn] = mc.machineControlData[mc.cnYouTurn];

            //speed
            mc.relayData[mc.rdSpeedXFour] = unchecked((byte)(pn.speed * 4));

            if (Properties.Settings.Default.setUDP_isOn)
            {
                SendUDPMessage(mc.relayData);
            }
            
            //Tell Arduino to turn section on or off accordingly
            if (spRelay.IsOpen)
            {
                try { spRelay.Write(items, 0, numItems); }
                catch (Exception e)
                {
                    WriteErrorLog("Out to Section relays" + e.ToString());
                    SerialPortRelayClose();
                }
            }
        }

        private void SerialLineReceivedRelay(string sentence)
        {
            mc.serialRecvRelayStr = sentence;
            int end;

            // Find end of sentence, if not a CR, return
            end = sentence.IndexOf("\r");
            if (end == -1) return;

            if (pbarRelay++ > 99) pbarRelay = 0;

            //the ArdRelay sentence to be parsed
            sentence = sentence.Substring(0, end);
            string[] words = sentence.Split(',');
        }

        //the delegate for thread
        private delegate void LineReceivedEventHandlerRelay(string sentence);

        //Arduino serial port receive in its own thread
        private void sp_DataReceivedRelay(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (spRelay.IsOpen)
            {
                try
                {
                    string sentence = spRelay.ReadLine();
                    this.BeginInvoke(new LineReceivedEventHandlerRelay(SerialLineReceivedRelay), sentence);
                    if (spRelay.BytesToRead > 32) spRelay.DiscardInBuffer();
                }
                //this is bad programming, it just ignores errors until its hooked up again.
                catch (Exception ex)
                {
                    WriteErrorLog("Relay Data Recvd " + ex.ToString());
                }

            }
        }

        //open the Arduino serial port
        public void SerialPortRelayOpen()
        {
            if (!spRelay.IsOpen)
            {
                spRelay.PortName = portNameRelaySection;
                spRelay.BaudRate = baudRateRelaySection;
                spRelay.DataReceived += sp_DataReceivedRelay;
            }

            try { spRelay.Open(); }
            catch (Exception e)
            {
                WriteErrorLog("Opening Relay Port" + e.ToString());

                MessageBox.Show(e.Message + "\n\r" + "\n\r" + "Go to Settings -> COM Ports to Fix", "No Arduino Port Active");


                Properties.Settings.Default.setPort_wasRelayConnected = false;
                Properties.Settings.Default.Save();
            }

            if (spRelay.IsOpen)
            {
                spRelay.DiscardOutBuffer();
                spRelay.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameRelay = portNameRelaySection;
                Properties.Settings.Default.setPort_wasRelayConnected = true;
                Properties.Settings.Default.Save();
            }
        }

        //close the relay port
        public void SerialPortRelayClose()
        {
            if (spRelay.IsOpen)
            {
                spRelay.DataReceived -= sp_DataReceivedRelay;
                try { spRelay.Close(); }
                catch (Exception e)
                {
                    WriteErrorLog("Closing Relay Serial Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated??");
                }

                Properties.Settings.Default.setPort_wasRelayConnected = false;
                Properties.Settings.Default.Save();

                spRelay.Dispose();
            }
        }
        #endregion

        #region GPS SerialPort //--------------------------------------------------------------------------

        //serial port receive in its own thread
        private void sp_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (SerialGPS.IsOpen)
            {
                try
                {
                    if (Properties.Settings.Default.setGPS_fixFromWhichSentence == "UBX")
                    {


                        while (SerialGPS.BytesToRead > 0)
                        {


                            SerialGPS.Read(rawBuffer, BytesRead, 1);

                            if (BytesRead < 2)
                            {
                                if (Header[BytesRead] == rawBuffer[BytesRead])
                                {
                                    CK_A = 0;
                                    CK_B = 0;
                                    BytesRead++;
                                }
                                else
                                {
                                    BytesRead = 0;
                                }
                            }
                            else
                            {
                                if (BytesRead == 5)
                                {
                                    MessageLength = BitConverter.ToInt16(rawBuffer, 4);
                                }

                                if (BytesRead < MessageLength + 6)
                                {
                                    CK_A = CK_A + rawBuffer[BytesRead];
                                    CK_A &= 0xFf;
                                    CK_B = CK_B + CK_A;
                                    CK_B &= 0xFf;
                                }
                                BytesRead++;
                                if (BytesRead > (MessageLength + 7))//here ck_B is set
                                {
                                    if (CK_A == rawBuffer[MessageLength + 6] && CK_B == rawBuffer[MessageLength + 7])
                                    {
                                        string sentence = "";

                                        if (0x01 == rawBuffer[2] && 0x3C == rawBuffer[3])//UBX-NAV-RELPOSNED
                                        {
                                            long itow = BitConverter.ToInt32(rawBuffer, 10);
                                            Int16 rr = BitConverter.ToInt16(rawBuffer, 66);


                                            //• UBX - NAV - RELPOSNED: The carrSoln flag will be set to 1 for RTK float and 2 for RTK fixed.//this is from first gps source!
                                                    //1 0000 1101                //1 0001 0101
                                            if (( (rr & 0x10D) == 0x10D) || ((rr & 0x115) == 0x115))//gnssFixOK && relPosValid && float/fix
                                            {


                                                pn.headingHDT = (BitConverter.ToInt32(rawBuffer, 30) * 0.00001) + HeadDif;
                                                ahrs.rollX16 = (int)(glm.toRadians(Math.Atan2(BitConverter.ToInt32(rawBuffer, 22) + BitConverter.ToInt32(rawBuffer, 40) / 100, DualGPSDistance * 10.0)) * 16);



                                                recvSentenceSettings[3] = recvSentenceSettings[2];
                                                recvSentenceSettings[2] = recvSentenceSettings[1];
                                                recvSentenceSettings[1] = recvSentenceSettings[0];
                                                recvSentenceSettings[0] = "UBX-RELPOSNED, Heading = " + pn.headingHDT.ToString("N4", CultureInfo.InvariantCulture) + ", Roll = " + (ahrs.rollX16/16).ToString("N4", CultureInfo.InvariantCulture) + ", itow = " + itow.ToString();





                                                //double heading = (BitConverter.ToInt32(rawBuffer, 30) * 0.00001) + HeadDif;
                                                //int roll = (int)(glm.toRadians(Math.Atan2(BitConverter.ToInt32(rawBuffer, 22) + BitConverter.ToInt32(rawBuffer, 40) / 100, DualGPSDistance * 10.0)) * 16);


                                                //sentence = "$UBX-RELPOSNED,1," + heading.ToString("N4", CultureInfo.InvariantCulture) + "," + roll.ToString("N4", CultureInfo.InvariantCulture) + "," +itow.ToString() + "*";
                                            }
                                            else //Bad Quality
                                            {
                                                //sentence = "$UBX-RELPOSNED,0,0,0," + itow.ToString() + "*";
                                            }
                                        }
                                        else if (0x01 == rawBuffer[2] && 0x14 == rawBuffer[3])//UBX-NAV-HPPOSLLH
                                        {
                                            if ((rawBuffer[9] & (1 << 0)) == 0)
                                            {
                                                long itow = BitConverter.ToInt32(rawBuffer, 10);
                                                double longitude = (rawBuffer[30] * 0.01 + BitConverter.ToInt32(rawBuffer, 14)) * 0.0000001;
                                                double latitude = (rawBuffer[31] * 0.01 + BitConverter.ToInt32(rawBuffer, 18)) * 0.0000001;
                                                double altitude = (rawBuffer[33] * 0.1 + BitConverter.ToInt32(rawBuffer, 26)) * 1000.0;
                                                sentence = "$UBX-HPPOSLLH,1," + longitude.ToString("N7", CultureInfo.InvariantCulture) + "," + latitude.ToString("N7", CultureInfo.InvariantCulture) + "," + altitude.ToString("N7", CultureInfo.InvariantCulture) + "," + itow.ToString() + "*";
                                            }
                                        }
                                        else if (0x01 == rawBuffer[2] && 0x07 == rawBuffer[3])//UBX-NAV-PVT
                                        {
                                            //if ((rawBuffer[9] & (1 << 0)) == 0)
                                            {
                                                long itow = BitConverter.ToInt32(rawBuffer, 6);

                                                byte numSV = rawBuffer[29];
                                                double longitude = BitConverter.ToInt32(rawBuffer, 30) * 0.0000001;//to deg
                                                double latitude = BitConverter.ToInt32(rawBuffer, 34) * 0.0000001;//to deg
                                                double altitude = BitConverter.ToInt32(rawBuffer, 42) * 1000.0;//to meters


                                                double gSpeed = BitConverter.ToInt32(rawBuffer, 66) * 0.0036;//to km/h
                                                double headMot = BitConverter.ToInt32(rawBuffer, 70) * 0.00001;//to deg
                                                double gSpeedAcc = BitConverter.ToInt32(rawBuffer, 74) * 0.0036;//to km/h
                                                double headMotAcc = BitConverter.ToInt32(rawBuffer, 78) * 0.00001;//to deg
                                                double pDOP = BitConverter.ToInt32(rawBuffer, 78) * 0.01;


                                                sentence = "$UBX-pvt,1," + longitude.ToString("N7", CultureInfo.InvariantCulture) + "," + latitude.ToString("N7", CultureInfo.InvariantCulture) + "," + altitude.ToString("N7", CultureInfo.InvariantCulture) + "," + itow.ToString() + "*";
                                            }
                                        }



                                        if (sentence != "")
                                        {
                                            int sum = 0, inx;
                                            char[] sentence_chars = sentence.ToCharArray();
                                            char tmp;

                                            for (inx = 1; ; inx++)
                                            {
                                                tmp = sentence_chars[inx];
                                                if (tmp == '*') break;
                                                sum ^= tmp;
                                            }
                                            this.BeginInvoke((MethodInvoker)(() => pn.rawBuffer += (sentence + String.Format("{0:X2}", sum) + "\r\n")));
                                        }
                                    }
                                    MessageLength = 0;
                                    BytesRead = 0;

                                }
                            }
                        }
                    }
                    else
                    {
                        string sentence = SerialGPS.ReadExisting();
                        //this.BeginInvoke(new LineReceivedEventHandler(SerialLineReceived), sentence);

                        this.BeginInvoke((MethodInvoker)(() => pn.rawBuffer += sentence));
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorLog("GPS Data Recv" + ex.ToString());
                }
            }
        }

        //called by the GPS delegate every time a chunk is rec'd
        private delegate void LineReceivedEventHandler(string sentence);
        public void SerialLineReceived(string sentence)
        {
            //spit it out no matter what it says
            pn.rawBuffer += sentence;
        }

        //serial port receive in its own thread
        private void sp_DataReceived2(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (SerialGPS2.IsOpen)
            {
                try
                {
                    if (SerialGPS2.BytesToRead > 0)
                    {
                        int intBuffer = SerialGPS2.BytesToRead;
                        byte[] rawBuffer2 = new byte[intBuffer];
                        SerialGPS2.Read(rawBuffer2, 0, intBuffer);
                        if (SerialGPS.IsOpen)
                        {
                            SerialGPS.Write(rawBuffer2, 0, intBuffer);
                        }
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorLog("GPS Data Recv" + ex.ToString());
                }
            }
        }

        public void ReadGPSData()
        {
            if (SerialGPS.IsOpen)
            {
                try
                {
                    if (Properties.Settings.Default.setGPS_fixFromWhichSentence == "UBX")
                    {
                        while (SerialGPS.BytesToRead > 0)
                        {
                            SerialGPS.Read(rawBuffer, BytesRead, 1);

                            if (BytesRead < 2)
                            {
                                if (Header[BytesRead] == rawBuffer[BytesRead])
                                {
                                    CK_A = 0;
                                    CK_B = 0;
                                    BytesRead++;
                                }
                                else
                                {
                                    BytesRead = 0;
                                }
                            }
                            else
                            {
                                if (BytesRead == 5)
                                {
                                    MessageLength = BitConverter.ToInt16(rawBuffer, 4);
                                }

                                if (BytesRead < MessageLength + 6)
                                {
                                    CK_A = CK_A + rawBuffer[BytesRead];
                                    CK_A &= 0xFf;
                                    CK_B = CK_B + CK_A;
                                    CK_B &= 0xFf;
                                }
                                BytesRead++;
                                if (BytesRead > (MessageLength + 7))//here ck_B is set
                                {
                                    if (CK_A == rawBuffer[MessageLength + 6] && CK_B == rawBuffer[MessageLength + 7])
                                    {
                                        if (0x01 == rawBuffer[2] && 0x3C == rawBuffer[3])//UBX-NAV-RELPOSNED
                                        {
                                            Int16 rr = BitConverter.ToInt16(rawBuffer, 66);
                                            //• UBX - NAV - RELPOSNED: The carrSoln flag will be set to 1 for RTK float and 2 for RTK fixed.//this is from first gps source!
                                            //1 0000 1101                //1 0001 0101
                                            long itow = BitConverter.ToInt32(rawBuffer, 10);
                                            if (((rr & 0x10D) == 0x10D) || ((rr & 0x115) == 0x115))//gnssFixOK && relPosValid && carrSoln float/fix && relPosHeadingValid
                                            {
                                                EnableLatLon = true;
                                                pn.headingHDT = (BitConverter.ToInt32(rawBuffer, 30) * 0.00001) + HeadDif;
                                                ahrs.rollX16 = (int)(glm.toRadians(Math.Atan2(BitConverter.ToInt32(rawBuffer, 22) + BitConverter.ToInt32(rawBuffer, 40) / 100, DualGPSDistance * 10.0)) * 16);




                                                recvSentenceSettings[3] = recvSentenceSettings[1];
                                                //recvSentenceSettings[2] = recvSentenceSettings[1];
                                                //recvSentenceSettings[1] = recvSentenceSettings[0];

                                                if (lastitow + 200 == itow) recvSentenceSettings[1] = "$UBX-RELPOSNED, Heading = " + pn.headingHDT.ToString("N4", CultureInfo.InvariantCulture) + ", Roll = " + pn.nRoll.ToString("N4", CultureInfo.InvariantCulture) + ", itow = " + itow.ToString();
                                                else recvSentenceSettings[1] = "$UBX-RELPOSNED, Heading = " + pn.headingHDT.ToString("N4", CultureInfo.InvariantCulture) + ", Roll = " + pn.nRoll.ToString("N4", CultureInfo.InvariantCulture) + ", itow = False";
                                            }
                                            else //Bad Quality
                                            {
                                                if ((rr & 0x10D) == 0x10D) EnableLatLon = false;

                                                ahrs.rollX16 = 9999;
                                                pn.headingHDT = 9999;
                                                recvSentenceSettings[3] = recvSentenceSettings[1];
                                                recvSentenceSettings[1] = "$UBX-RELPOSNED, Heading = 9999, Roll = 9999, itow = " + itow.ToString();
                                            }
                                        }
                                        else if (0x01 == rawBuffer[2] && 0x14 == rawBuffer[3])//UBX-NAV-HPPOSLLH
                                        {
                                            long itow = BitConverter.ToInt32(rawBuffer, 10);
                                            if (EnableLatLon && rawBuffer[9] == 0x00)
                                            {
                                                pn.longitude = (rawBuffer[30] * 0.01 + BitConverter.ToInt32(rawBuffer, 14)) * 0.0000001;
                                                pn.latitude = (rawBuffer[31] * 0.01 + BitConverter.ToInt32(rawBuffer, 18)) * 0.0000001;
                                                pn.altitude = (rawBuffer[33] * 0.1 + BitConverter.ToInt32(rawBuffer, 26)) * 0.001;

                                                pn.UpdateNorthingEasting();

                                                //recvSentenceSettings[3] = recvSentenceSettings[2];
                                                recvSentenceSettings[2] = recvSentenceSettings[0];
                                                //recvSentenceSettings[1] = recvSentenceSettings[0];
                                                recvSentenceSettings[0] = "$UBX-HPPOSLLH, Longitude = " + pn.longitude.ToString("N7", CultureInfo.InvariantCulture) + ", Latitude = " + pn.latitude.ToString("N7", CultureInfo.InvariantCulture) + ", Altitude = " + pn.altitude.ToString("N3", CultureInfo.InvariantCulture) + ", itow = " + itow.ToString();
                                            }
                                            else
                                            {
                                                recvSentenceSettings[2] = recvSentenceSettings[0];
                                                recvSentenceSettings[0] = "$UBX-HPPOSLLH, Longitude = ???, Latitude = ???, Altitude = ???, itow = " + itow.ToString();
                                            }
                                        }
                                        else if (0x01 == rawBuffer[2] && 0x07 == rawBuffer[3])//UBX-NAV-PVT
                                        {
                                            long itow = BitConverter.ToInt32(rawBuffer, 6);
                                            if (rawBuffer[84] == 0x00)
                                            {
                                                pn.longitude = BitConverter.ToInt32(rawBuffer, 30) * 0.0000001;//to deg
                                                pn.latitude = BitConverter.ToInt32(rawBuffer, 34) * 0.0000001;//to deg
                                                pn.altitude = BitConverter.ToInt32(rawBuffer, 42) * 0.001;//to meters

                                                pn.UpdateNorthingEasting();

                                                pn.speed = BitConverter.ToInt32(rawBuffer, 66) * 0.0036;//to km/h
                                                pn.satellitesTracked = rawBuffer[29];
                                                pn.hdop = BitConverter.ToInt32(rawBuffer, 46) * 0.01;
                                                pn.ageDiff = itow;


                                                //average the speeds for display, not calcs
                                                avgSpeed[ringCounter] = pn.speed;
                                                if (ringCounter++ > 8) ringCounter = 0;

                                                //EnableLatLon = true if relative position components and accuracies are valid and in moving base mode if baseline is valid
                                                byte rr = rawBuffer[27];
                                                if (EnableLatLon && ((rr & 0x81) == 0x81)) pn.fixQuality = 4;
                                                else if (EnableLatLon && ((rr & 0x41) == 0x41)) pn.fixQuality = 5;
                                                else pn.fixQuality = 1;

                                                //recvSentenceSettings[3] = recvSentenceSettings[2];
                                                //recvSentenceSettings[2] = recvSentenceSettings[1];
                                                recvSentenceSettings[2] = recvSentenceSettings[0];
                                                //recvSentenceSettings[1] = recvSentenceSettings[0];
                                                recvSentenceSettings[0] = "$UBX-PVT, Longitude = " + pn.longitude.ToString("N7", CultureInfo.InvariantCulture) + ", Latitude = " + pn.latitude.ToString("N7", CultureInfo.InvariantCulture) + ", Altitude = " + pn.altitude.ToString("N3", CultureInfo.InvariantCulture) + ", itow = " + itow.ToString();
                                            }
                                            else
                                            {
                                                recvSentenceSettings[2] = recvSentenceSettings[0];
                                                recvSentenceSettings[0] = "$UBX-PVT, Longitude = ???, Latitude = ???, Altitude = ???, itow = " + itow.ToString();
                                            }
                                        }
                                    }
                                    MessageLength = 0;
                                    BytesRead = 0;
                                }
                            }
                        }
                    }
                    else
                    {
                        //string sentence = SerialGPS.ReadExisting();
                        //this.BeginInvoke(new LineReceivedEventHandler(SerialLineReceived), sentence);

                        pn.rawBuffer += SerialGPS.ReadExisting();
                    }
                }
                catch (Exception ex)
                {
                    WriteErrorLog("GPS Data Recv" + ex.ToString());
                }
            }
        }

        public void SerialPortOpenGPS()
        {
            //close it first
            SerialPortCloseGPS();

            if (Properties.Settings.Default.setMenu_isSimulatorOn)
            {
                simulatorOnToolStripMenuItem.Checked = false;
                panelSim.Visible = false;
                timerSim.Enabled = false;
                Settings.Default.setMenu_isSimulatorOn = simulatorOnToolStripMenuItem.Checked;
                Settings.Default.Save();
            }

            if (!SerialGPS.IsOpen)
            {
                SerialGPS.PortName = portNameGPS;
                SerialGPS.BaudRate = baudRateGPS;
                //SerialGPS.DataReceived += sp_DataReceived;
                SerialGPS.WriteTimeout = 1000;
            }

            try { SerialGPS.Open(); }
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

            if (SerialGPS.IsOpen)
            {
                //btnOpenSerial.Enabled = false;

                //discard any stuff in the buffers
                SerialGPS.DiscardOutBuffer();
                SerialGPS.DiscardInBuffer();

                //update port status label
                //stripPortGPS.Text = portNameGPS + " " + baudRateGPS.ToString();
                //stripPortGPS.ForeColor = Color.ForestGreen;

                Properties.Settings.Default.setPort_portNameGPS = portNameGPS;
                Properties.Settings.Default.setPort_baudRate = baudRateGPS;
                Properties.Settings.Default.Save();
            }
        }

        public void SerialPortOpenGPS2()
        {
            //close it first
            SerialPortCloseGPS2();



            if (!SerialGPS2.IsOpen)
            {
                SerialGPS2.PortName = portNameGPS2;
                SerialGPS2.BaudRate = baudRateGPS2;
                SerialGPS2.DataReceived += sp_DataReceived2;
                SerialGPS2.WriteTimeout = 1000;
            }

            try { SerialGPS2.Open(); }
            catch (Exception)
            {
            }

            if (SerialGPS2.IsOpen)
            {
                SerialGPS2.DiscardOutBuffer();
                SerialGPS2.DiscardInBuffer();

                Properties.Settings.Default.setPort_portNameGPS2 = portNameGPS2;
                Properties.Settings.Default.setPort_baudRate2 = baudRateGPS2;
                Properties.Settings.Default.Save();

            }
        }
        public void SerialPortCloseGPS()
        {
            //if (sp.IsOpen)
            {
                //SerialGPS.DataReceived -= sp_DataReceived;
                try { SerialGPS.Close(); }
                catch (Exception e)
                {
                    WriteErrorLog("Closing GPS Port" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                }

                //update port status labels
                //stripPortGPS.Text = " * * " + baudRateGPS.ToString();
                //stripPortGPS.ForeColor = Color.ForestGreen;
                //stripOnlineGPS.Value = 1;
                SerialGPS.Dispose();
            }
        }
        public void SerialPortCloseGPS2()
        {
            //if (sp.IsOpen)
            {
                SerialGPS2.DataReceived -= sp_DataReceived2;
                try { SerialGPS2.Close(); }
                catch (Exception e)
                {
                    WriteErrorLog("Closing GPS Port 2" + e.ToString());
                    MessageBox.Show(e.Message, "Connection already terminated?");
                };
                SerialGPS2.Dispose();
            }
        }

        #endregion SerialPortGPS

    }//end class
}//end namespace