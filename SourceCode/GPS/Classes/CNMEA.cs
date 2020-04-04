using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace AgOpenGPS
{
    #region NMEA_Sentence_Guide

    //$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M ,  ,*47
    //   0     1      2      3    4      5 6  7  8   9    10 11  12 13  14
    //        Time      Lat       Lon

    /*
    GGA - essential fix data which provide 3D location and accuracy data.

     $GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M,,*47

    Where:
         GGA          Global Positioning System Fix Data
         123519       Fix taken at 12:35:19 UTC
         4807.038,N   Latitude 48 deg 07.038' N
         01131.000,E  Longitude 11 deg 31.000' E
         1            Fix quality: 0 = invalid
                                   1 = GPS fix (SPS)
                                   2 = DGPS fix
                                   3 = PPS fix
                                   4 = Real Time Kinematic
                                   5 = Float RTK
                                   6 = estimated (dead reckoning) (2.3 feature)
                                   7 = Manual input mode
                                   8 = Simulation mode
     (7)    08           Number of satellites being tracked
     (8)    0.9          Horizontal dilution of position
         545.4,M      Altitude, Meters, above mean sea level
         46.9,M       Height of geoid (mean sea level) above WGS84
                          ellipsoid
         (empty field) time in seconds since last DGPS update
         (empty field) DGPS station ID number
         *47          the checksum data, always begins with *
     *
     *
   //$GPRMC,123519,A,4807.038,N,01131.000,E,022.4,084.4,230394,003.1,W*6A
   //  0      1    2   3      4    5      6   7     8     9     10   11
    //        Time      Lat        Lon       knots  Ang   Date  MagV

    Where:
         RMC          Recommended Minimum sentence C
         123519       Fix taken at 12:35:19 UTC
         A            Status A=active or V=Void.
         4807.038,N   Latitude 48 deg 07.038' N
         01131.000,E  Longitude 11 deg 31.000' E
         022.4        Speed over the ground in knots
         084.4        Track angle in degrees True
         230394       Date - 23rd of March 1994
         003.1,W      Magnetic Variation
         *6A          The checksum data, always begins with *
     *
   $GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48
     *
        VTG          Track made good and ground speed
        054.7,T      True track made good (degrees)
        034.4,M      Magnetic track made good
        005.5,N      Ground speed, knots
        010.2,K      Ground speed, Kilometers per hour
        *48          Checksum
        *
$PAOGI
 * * From GGA:
(1 , 2) 123519 Fix taken at 1219 UTC
(3 , 4) 4807.038,N Latitude 48 deg 07.038' N
(5, 6) 01131.000,E Longitude 11 deg 31.000' E
(7) 1 Fix quality: 0 = invalid
1 = GPS fix (SPS)
2 = DGPS fix
3 = PPS fix
4 = Real Time Kinematic
5 = Float RTK
6 = estimated (dead reckoning) (2.3 feature)
7 = Manual input mode
8 = Simulation mode
(8) 08 Number of satellites being tracked
(9) 0.9 Horizontal dilution of position
(10, 11) 545.4,M Altitude, Meters, above mean sea level
(12) 1.2 time in seconds since last DGPS update

From RMC or VTG:
(13) 022.4 Speed over the ground in knots (Or would you prefer KPH)
(14) 084.4 Track angle in degrees True

FROM IMU:
(15) XXX.xx IMU Heading in degrees True
(16) XXX.xx Roll angle in degrees (What is a positive roll, left leaning - left down, right up?)
(17) XXX.xx Pitch angle in degrees (Positive pitch = nose up)
(18) XXX.xx Yaw Rate in Degrees / second
(19) T/F IMU status - Valid IMU Fusion

*CHKSUM
*
*             /*       Time, yaw, tilt, range for moving baseline RTK
An example of the PTNL,AVR message string is:

$PTNL,AVR,181059.6,+149.4688,Yaw,+0.0134,Tilt,,,60.191,3,2.5,6*00

AVR message fields
Field	Meaning
0	Message ID $PTNL,AVR
1	UTC of vector fix
2	Yaw angle in degrees
3	Yaw
4	Tilt angle in degrees
5	Tilt
6	Reserved
7	Reserved
8	Range in meters
9	GPS quality indicator:
0: Fix not available or invalid
1: Autonomous GPS fix
2: Differential carrier phase solution RTK (Float)
3: Differential carrier phase solution RTK (Fix)
4: Differential code-based solution, DGPS
10	PDOP
11	Number of satellites used in solution
12	The checksum data, always begins with *
    */

    #endregion NMEA_Sentence_Guide

    public class CNMEA
    {
        //WGS84 Lat Long
        public double latitude, longitude;

        public double latStart, lonStart;

        public double actualEasting, actualNorthing, zone;
        public double centralMeridian, convergenceAngle;

        public int DualAntennaDistance = 0;
        public double HeadingAngleCorrection = 0;
        public bool UpdatedLatLon, EnableHeadRoll;
        public List<byte> rawBuffer = new List<byte>(), rawBuffer2 = new List<byte>();

        public string fixFrom;
        private string[] words;

        //UTM coordinates
        //public double northing, easting;
        public vec2 fix = new vec2(0, 0);

        //used to offset the antenna position to compensate for drift
        public vec2 fixOffset = new vec2(0, 0);

        //other GIS Info
        public double altitude, speed;

        public double headingTrue, headingHDT, hdop, ageDiff;

        //BaselineData
        public double upProjection, baselineLength, baselineCourse;

        //imu
        public double nRoll, nYaw, nAngularVelocity;

        public bool isValidIMU;
        public int fixQuality;
        public int satellitesTracked;
        public string status = "q";
        public DateTime utcDateTime;
        public char hemisphere = 'N';

        //UTM numbers are huge, these cut them way down.
        public int utmNorth, utmEast;

        public StringBuilder logNMEASentence = new StringBuilder();
        private readonly FormGPS mf;

        public CNMEA(FormGPS f)
        {
            //constructor, grab the main form reference
            mf = f;
            fixFrom = Properties.Settings.Default.setGPS_fixFromWhichSentence;
            latStart = 0;
            lonStart = 0;
        }

        //ParseNMEA
        public void UpdateNorthingEasting()
        {
            #region Convergence

            double[] xy = DecDeg2UTM(latitude, longitude);
            //keep a copy of actual easting and northings
            actualEasting = xy[0];
            actualNorthing = xy[1];

            //if a field is open, the real one is subtracted from the integer
            fix.easting = xy[0] - utmEast + fixOffset.easting;
            fix.northing = xy[1] - utmNorth + fixOffset.northing;

            double east = fix.easting;
            double nort = fix.northing;

            //compensate for the fact the zones lines are a grid and the world is round
            fix.easting = (Math.Cos(-convergenceAngle) * east) - (Math.Sin(-convergenceAngle) * nort);
            fix.northing = (Math.Sin(-convergenceAngle) * east) + (Math.Cos(-convergenceAngle) * nort);

            UpdatedLatLon = true;

            //east = fix.easting;
            //nort = fix.northing;

            //go back again - programming reference only
            //fix.easting = (Math.Cos(convergenceAngle) * east) - (Math.Sin(convergenceAngle) * nort);
            //fix.northing = (Math.Sin(convergenceAngle) * east) + (Math.Cos(convergenceAngle) * nort);

            #endregion

            #region Antenna Offset

            if (mf.vehicle.antennaOffset != 0)
            {
                fix.easting = (Math.Cos(-mf.fixHeading) * mf.vehicle.antennaOffset) + fix.easting;
                fix.northing = (Math.Sin(-mf.fixHeading) * mf.vehicle.antennaOffset) + fix.northing;
            }
            #endregion

            #region Roll

            mf.rollUsed = 0;
            
            if ((mf.ahrs.isRollFromAutoSteer || mf.ahrs.isRollFromGPS) && !mf.ahrs.isRollFromOGI)
            {
                mf.rollUsed = ((double)(mf.ahrs.rollX16 - mf.ahrs.rollZeroX16)) * 0.0625;

                //change for roll to the right is positive times -1
                mf.rollCorrectionDistance = Math.Sin(glm.toRadians((mf.rollUsed))) * -mf.vehicle.antennaHeight;

                // roll to left is positive  **** important!!
                // not any more - April 30, 2019 - roll to right is positive Now! Still Important
                fix.easting = (Math.Cos(-mf.fixHeading) * mf.rollCorrectionDistance) + fix.easting;
                fix.northing = (Math.Sin(-mf.fixHeading) * mf.rollCorrectionDistance) + fix.northing;
            }

            //used only for draft compensation
            else if (mf.ahrs.isRollFromOGI) mf.rollUsed = ((double)(mf.ahrs.rollX16 - mf.ahrs.rollZeroX16)) * 0.0625;

            //pitchDistance = (pitch * vehicle.antennaHeight);
            //pn.fix.easting = (Math.Sin(fixHeading) * pitchDistance) + pn.fix.easting;
            //pn.fix.northing = (Math.Cos(fixHeading) * pitchDistance) + pn.fix.northing;

            #endregion Roll
        }

        public void ParseNMEA()
        {
            for (int i = 0; i < rawBuffer.Count - 5; i++)
            {
                /*if (rawBuffer[i] == 0x24)//$
                {

                    if (i > 0) rawBuffer.RemoveRange(0, i);
                    i = 0;
                    for (int j = 2; j < rawBuffer.Count; j++)
                    {
                        if (rawBuffer[j] == 0x2A)//*
                        {
                            int sum = 0;
                            for (int inx = 1; inx < j; inx++)
                            {
                                sum ^= rawBuffer[inx];// Build checksum
                            }

                            // Calculated checksum converted to a 2 digit hex string
                            if (String.Format("{0:X2}", sum) == Convert.ToChar(rawBuffer[j + 1]).ToString() + Convert.ToChar(rawBuffer[j + 2]).ToString())
                            {
                                string nextNMEASentence = Encoding.ASCII.GetString(rawBuffer.GetRange(0, j).ToArray());

                                mf.recvSentenceSettings[3] = mf.recvSentenceSettings[2];
                                mf.recvSentenceSettings[2] = mf.recvSentenceSettings[1];
                                mf.recvSentenceSettings[1] = mf.recvSentenceSettings[0];
                                mf.recvSentenceSettings[0] = nextNMEASentence;

                                //parse them accordingly
                                words = nextNMEASentence.Split(',');
                                if (words.Length < 3) return;

                                if (words[0] == "$GPGGA" || words[0] == "$GNGGA") ParseGGA();
                                if (words[0] == "$GPVTG" || words[0] == "$GNVTG") ParseVTG();
                                if (words[0] == "$GPRMC" || words[0] == "$GNRMC") ParseRMC();
                                if (words[0] == "$GPHDT" || words[0] == "$GNHDT") ParseHDT();
                                if (words[0] == "$PAOGI") ParseOGI();
                                if (words[0] == "$PTNL") ParseAVR();
                                if (words[0] == "$GNTRA") ParseTRA();
                                if (words[0] == "$GNTRA") ParseTRA();
                                if (words[0] == "$PSTI" && words[1] == "032") ParseSTI032(); //there is also an $PSTI,030,... wich contains different data!

                                //mf.testNMEA1 = mf.testNMEA.ElapsedMilliseconds;
                            }
                            rawBuffer.RemoveRange(0, j + 2);
                        }
                    }
                }
                */
                //else
                if (rawBuffer.Count > 99 + i)//100 bytes
                {
                    if (rawBuffer[i] == 0xB5 && rawBuffer[i + 1] == 0x62 && rawBuffer[i + 2] == 0x01 && rawBuffer[i + 3] == 0x07)//UBX-PVT
                    {
                        if (i > 0) rawBuffer.RemoveRange(0, i);//start with Header

                        int CK_A = 0;
                        int CK_B = 0;

                        for (int j = 2; j < 98; j += 1)// start with Class and end by Checksum
                        {
                            CK_A = (CK_A + rawBuffer[j]) & 0xFF;
                            CK_B = (CK_B + CK_A) & 0xFF;
                        }

                        if (rawBuffer[98] == CK_A && rawBuffer[99] == CK_B)
                        {
                            long itow = rawBuffer[6] | (rawBuffer[7] << 8) | (rawBuffer[8] << 16) | (rawBuffer[9] << 24);

                            if (rawBuffer[84] == 0x00)
                            {
                                if ((rawBuffer[27] & 0x81) == 0x81)
                                {
                                    fixQuality = 4;
                                    EnableHeadRoll = true;
                                }
                                else if ((rawBuffer[27] & 0x41) == 0x41)
                                {
                                    fixQuality = 5;
                                    EnableHeadRoll = true;
                                }
                                else
                                {
                                    fixQuality = 1;
                                    EnableHeadRoll = false;
                                }

                                satellitesTracked = rawBuffer[29];

                                longitude = (rawBuffer[30] | (rawBuffer[31] << 8) | (rawBuffer[32] << 16) | (rawBuffer[33] << 24)) * 0.0000001;//to deg
                                latitude = (rawBuffer[34] | (rawBuffer[35] << 8) | (rawBuffer[36] << 16) | (rawBuffer[37] << 24)) * 0.0000001;//to deg
                                altitude = (rawBuffer[42] | (rawBuffer[43] << 8) | (rawBuffer[44] << 16) | (rawBuffer[45] << 24)) * 0.001;//to meters

                                hdop = (rawBuffer[46] | (rawBuffer[47] << 8) | (rawBuffer[48] << 16) | (rawBuffer[49] << 24)) * 0.01;

                                UpdateNorthingEasting();

                                speed = (rawBuffer[66] | (rawBuffer[67] << 8) | (rawBuffer[68] << 16) | (rawBuffer[69] << 24)) * 0.0036;//to km/h

                                //average the speed
                                AverageTheSpeed();

                                mf.recvSentenceSettings[2] = mf.recvSentenceSettings[0];
                                mf.recvSentenceSettings[0] = "$UBX-PVT, Longitude = " + longitude.ToString("N7", CultureInfo.InvariantCulture) + ", Latitude = " + latitude.ToString("N7", CultureInfo.InvariantCulture) + ", Altitude = " + altitude.ToString("N3", CultureInfo.InvariantCulture) + ", itow = " + itow.ToString();
                            }
                            else
                            {
                                fixQuality = 0;
                                mf.recvSentenceSettings[2] = mf.recvSentenceSettings[0];
                                mf.recvSentenceSettings[0] = "$UBX-PVT, Longitude = ???, Latitude = ???, Altitude = ???, itow = " + itow.ToString();
                            }
                        }
                        //mf.testNMEA1 = mf.testNMEA.ElapsedMilliseconds;
                        rawBuffer.RemoveRange(0, 100);
                    }
                }
            }
            if (rawBuffer.Count > 500)
            {
                rawBuffer.RemoveRange(0, 250);
            }

            for (int i = 0; i < rawBuffer2.Count - 5; i++)
            {
                if (rawBuffer2.Count > 71 + i)//72 bytes
                {
                    if (rawBuffer2[i] == 0xB5 && rawBuffer2[i + 1] == 0x62 && rawBuffer2[i + 2] == 0x01 && rawBuffer2[i + 3] == 0x3C)//UBX-PVT
                    {
                        if (i > 0) rawBuffer2.RemoveRange(0, i);//start with Header

                        int CK_A = 0;
                        int CK_B = 0;

                        for (int j = 2; j < 70; j += 1)// start with Class and end by Checksum
                        {
                            CK_A = (CK_A + rawBuffer2[j]) & 0xFF;
                            CK_B = (CK_B + CK_A) & 0xFF;
                        }

                        if (rawBuffer2[70] == CK_A && rawBuffer2[71] == CK_B)
                        {
                            long itow = rawBuffer2[10] | (rawBuffer2[11] << 8) | (rawBuffer2[12] << 16) | (rawBuffer2[13] << 24);

                            if (EnableHeadRoll && ((rawBuffer2[67] & 0x01) == 0x01) && (((rawBuffer2[66] & 0x2D) == 0x2D) || ((rawBuffer2[66] & 0x35) == 0x35)))
                            {
                                int relposlength = rawBuffer2[26] | (rawBuffer2[27] << 8) | (rawBuffer2[28] << 16) | (rawBuffer2[29] << 24);//in cm!

                                if (DualAntennaDistance - 5 < relposlength && relposlength < DualAntennaDistance + 5)
                                {
                                    //save dist?
                                }

                                headingHDT = (rawBuffer2[30] | (rawBuffer2[31] << 8) | (rawBuffer2[32] << 16) | (rawBuffer2[33] << 24)) * 0.00001;
                                mf.ahrs.rollX16 = (int)(glm.toRadians(Math.Atan2((rawBuffer2[22] | (rawBuffer2[23] << 8) | (rawBuffer2[24] << 16) | (rawBuffer2[25] << 24)) + rawBuffer2[40] * 0.1, DualAntennaDistance)) * 16);

                                //ahrs.rollX16 = (int)(glm.toRadians(Math.Atan2(BitConverter.ToInt32(Data, 22) + BitConverter.ToInt32(Data, 40) / 100, mf.DualAntennaDistance * 10.0)) * 16);

                                mf.recvSentenceSettings[3] = mf.recvSentenceSettings[1];
                                mf.recvSentenceSettings[1] = "$UBX-RELPOSNED, Heading = " + headingHDT.ToString("N4", CultureInfo.InvariantCulture) + ", Roll = " + nRoll.ToString("N4", CultureInfo.InvariantCulture) + ", itow = " + itow.ToString();
                            }
                            else //Bad Quality
                            {
                                mf.ahrs.rollX16 = 9999;
                                headingHDT = 9999;
                                mf.recvSentenceSettings[3] = mf.recvSentenceSettings[1];
                                mf.recvSentenceSettings[1] = "$UBX-RELPOSNED, Heading = 9999, Roll = 9999, itow = " + itow.ToString();
                            }
                        }
                        rawBuffer2.RemoveRange(0, 72);
                    }
                }
            }

            if (rawBuffer2.Count > 72)//message length
            {
                rawBuffer2.RemoveRange(0, rawBuffer2.Count - 72);
            }


        }
        /*
        // Returns a valid NMEA sentence from the pile from portData
        public string Parse()
        {
            string sentence;
            do
            {
                //double check for valid sentence
                // Find start of next sentence
                int start = rawBuffer.IndexOf("$", StringComparison.Ordinal);
                if (start == -1) return null;
                rawBuffer = rawBuffer.Substring(start);

                // Find end of sentence
                int end = rawBuffer.IndexOf("\n", StringComparison.Ordinal);
                if (end == -1) return null;

                //the NMEA sentence to be parsed
                sentence = rawBuffer.Substring(0, end + 1);

                mf.recvSentenceSettings[3] = mf.recvSentenceSettings[2];
                mf.recvSentenceSettings[2] = mf.recvSentenceSettings[1];
                mf.recvSentenceSettings[1] = mf.recvSentenceSettings[0];
                mf.recvSentenceSettings[0] = sentence.Replace('\n', ' ');

                //remove the processed sentence from the rawBuffer
                rawBuffer = rawBuffer.Substring(end + 1);
            }

            //if sentence has valid checksum, its all good
            while (!ValidateChecksum(sentence));

            //do we want to log? Grab before pieces are missing
            if (mf.isLogNMEA && nmeaCntr++ > 3)
            {
                logNMEASentence.Append(sentence);
                nmeaCntr = 0;
            }

            // Remove trailing checksum and \r\n and return
            sentence = sentence.Substring(0, sentence.IndexOf("*", StringComparison.Ordinal));

            return sentence;
        }
        //checks the checksum against the string
        public bool ValidateChecksum(string Sentence)
        {
            int sum = 0;
            try
            {
                char[] sentenceChars = Sentence.ToCharArray();
                // All character xor:ed results in the trailing hex checksum
                // The checksum calc starts after '$' and ends before '*'
                int inx;
                for (inx = 1; ; inx++)
                {
                    if (inx >= sentenceChars.Length) // No checksum found
                        return false;
                    var tmp = sentenceChars[inx];
                    // Indicates end of data and start of checksum
                    if (tmp == '*') break;
                    sum ^= tmp;    // Build checksum
                }
                // Calculated checksum converted to a 2 digit hex string
                string sumStr = String.Format("{0:X2}", sum);

                // Compare to checksum in sentence
                return sumStr.Equals(Sentence.Substring(inx + 1, 2));
            }
            catch (Exception e)
            {
                mf.WriteErrorLog("Validate Checksum" + e);
                return false;
            }
        }
        */





        private double rollK, Pc, G, Xp, Zp, XeRoll;
        private double P = 1.0;
        private readonly double varRoll = 0.1; // variance, smaller, more faster filtering
        private readonly double varProcess = 0.0003;

        private void AverageTheSpeed()
        {
            //average the speed
            mf.avgSpeed = (mf.avgSpeed * 0.9) + (speed * 0.1);
        }

        private void ParseAVR()
        {
            if (!String.IsNullOrEmpty(words[1]))
            {

                //True heading
                // 0 1 2 3 4 5 6 7 8 9
                // $PTNL,AVR,145331.50,+35.9990,Yaw,-7.8209,Tilt,-0.4305,Roll,444.232,3,1.2,17 * 03
                //Field
                // Meaning
                //0 Message ID $PTNL,AVR
                //1 UTC of vector fix
                //2 Yaw angle, in degrees
                //3 Yaw
                //4 Tilt angle, in degrees
                //5 Tilt
                //6 Roll angle, in degrees
                //7 Roll
                //8 Range, in meters
                //9 GPS quality indicator:
                // 0: Fix not available or invalid
                // 1: Autonomous GPS fix
                // 2: Differential carrier phase solution RTK(Float)
                // 3: Differential carrier phase solution RTK(Fix)
                // 4: Differential code-based solution, DGPS
                //10 PDOP
                //11 Number of satellites used in solution
                //12 The checksum data, always begins with *

                if (words[8] == "Roll")
                    double.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out nRoll);
                else
                    double.TryParse(words[5], NumberStyles.Float, CultureInfo.InvariantCulture, out nRoll);

                if (mf.ahrs.isRollFromGPS)

                //input to the kalman filter
                {
                    //added by Andreas Ortner
                    rollK = nRoll;

                    //Kalman filter
                    Pc = P + varProcess;
                    G = Pc / (Pc + varRoll);
                    P = (1 - G) * Pc;
                    Xp = XeRoll;
                    Zp = Xp;
                    XeRoll = (G * (rollK - Zp)) + Xp;

                    mf.ahrs.rollX16 = (int)(XeRoll * 16);
                }
                else mf.ahrs.rollX16 = 0;
            }
        }

        private void ParseGGA()
        {
            //$GPGGA,123519,4807.038,N,01131.000,E,1,08,0.9,545.4,M,46.9,M ,  ,*47
            //   0     1      2      3    4      5 6  7  8   9    10 11  12 13  14
            //        Time      Lat       Lon

            //is the sentence GGA
            if (!String.IsNullOrEmpty(words[2]) && !String.IsNullOrEmpty(words[3])
                && !String.IsNullOrEmpty(words[4]) && !String.IsNullOrEmpty(words[5]))
            {
                if (fixFrom == "GGA")
                {
                    //get latitude and convert to decimal degrees
                    int decim = words[2].IndexOf(".", StringComparison.Ordinal);
                    decim -= 2;
                    double.TryParse(words[2].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out latitude);
                    double.TryParse(words[2].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out double temp);
                    temp *= 0.01666666666667;
                    latitude += temp;
                    if (words[3] == "S")
                    {
                        latitude *= -1;
                        hemisphere = 'S';
                    }
                    else { hemisphere = 'N'; }

                    //get longitude and convert to decimal degrees
                    decim = words[4].IndexOf(".", StringComparison.Ordinal);
                    decim -= 2;
                    double.TryParse(words[4].Substring(0, decim), NumberStyles.Float, CultureInfo.InvariantCulture, out longitude);
                    double.TryParse(words[4].Substring(decim), NumberStyles.Float, CultureInfo.InvariantCulture, out temp);
                    longitude += temp * 0.0166666666667;

                    { if (words[5] == "W") longitude *= -1; }

                    //calculate zone and UTM coords
                    UpdateNorthingEasting();
                }

                //fixQuality
                int.TryParse(words[6], NumberStyles.Float, CultureInfo.InvariantCulture, out fixQuality);

                //satellites tracked
                int.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out satellitesTracked);

                //hdop
                double.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out hdop);

                //altitude
                double.TryParse(words[9], NumberStyles.Float, CultureInfo.InvariantCulture, out altitude);

                //age of differential
                double.TryParse(words[11], NumberStyles.Float, CultureInfo.InvariantCulture, out ageDiff);

            }
        }

        private void ParseOGI()
        {
            //PAOGI parsing of the sentence
            //make sure there aren't missing coords in sentence
            if (!String.IsNullOrEmpty(words[2]) && !String.IsNullOrEmpty(words[3])
                && !String.IsNullOrEmpty(words[4]) && !String.IsNullOrEmpty(words[5]))
            {
                if (fixFrom == "OGI")
                {
                    //get latitude and convert to decimal degrees
                    double.TryParse(words[2].Substring(0, 2), NumberStyles.Float, CultureInfo.InvariantCulture, out latitude);
                    double.TryParse(words[2].Substring(2), NumberStyles.Float, CultureInfo.InvariantCulture, out double temp);
                    temp *= 0.01666666666666666666666666666667;
                    latitude += temp;
                    if (words[3] == "S")
                    {
                        latitude *= -1;
                        hemisphere = 'S';
                    }
                    else { hemisphere = 'N'; }

                    //get longitude and convert to decimal degrees
                    double.TryParse(words[4].Substring(0, 3), NumberStyles.Float, CultureInfo.InvariantCulture, out longitude);
                    double.TryParse(words[4].Substring(3), NumberStyles.Float, CultureInfo.InvariantCulture, out temp);
                    longitude += temp * 0.01666666666666666666666666666667;

                    { if (words[5] == "W") longitude *= -1; }

                }

                //fixQuality
                int.TryParse(words[6], NumberStyles.Float, CultureInfo.InvariantCulture, out fixQuality);

                //satellites tracked
                int.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out satellitesTracked);

                //hdop
                double.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out hdop);

                //altitude
                double.TryParse(words[9], NumberStyles.Float, CultureInfo.InvariantCulture, out altitude);

                //age of differential
                double.TryParse(words[10], NumberStyles.Float, CultureInfo.InvariantCulture, out ageDiff);

                //kph for speed - knots read
                double.TryParse(words[11], NumberStyles.Float, CultureInfo.InvariantCulture, out speed);
                speed = Math.Round(speed * 1.852, 1);

                //Dual antenna derived heading
                double.TryParse(words[12], NumberStyles.Float, CultureInfo.InvariantCulture, out headingHDT);

                //roll
                double.TryParse(words[13], NumberStyles.Float, CultureInfo.InvariantCulture, out nRoll );

                //used only for sidehill correction - position is compensated in Lat/Lon of Dual module
                if (mf.ahrs.isRollFromOGI)
                {
                    rollK = nRoll; //input to the kalman filter
                    Pc = P + varProcess;
                    G = Pc / (Pc + varRoll);
                    P = (1 - G) * Pc;
                    Xp = XeRoll;
                    Zp = Xp;
                    XeRoll = (G * (rollK - Zp)) + Xp;//result

                    mf.ahrs.rollX16 = (int)(XeRoll * 16);
                }

                else mf.ahrs.rollX16 = 0;

                //pitch
                //double.TryParse(words[14], NumberStyles.Float, CultureInfo.InvariantCulture, out nPitch);

                //Angular velocity
                double.TryParse(words[15], NumberStyles.Float, CultureInfo.InvariantCulture, out nAngularVelocity);

                //calculate zone and UTM coords, roll calcs
                UpdateNorthingEasting();

                //Angular velocity
                double.TryParse(words[17], NumberStyles.Float, CultureInfo.InvariantCulture, out nAngularVelocity);

                //is imu valid fusion
                isValidIMU = words[18] == "T";

                AverageTheSpeed();

                /*
                $PAOGI
                (1) 123519 Fix taken at 1219 UTC

                Roll corrected position
                (2,3) 4807.038,N Latitude 48 deg 07.038' N
                (4,5) 01131.000,E Longitude 11 deg 31.000' E

                (6) 1 Fix quality: 
                    0 = invalid
                    1 = GPS fix(SPS)
                    2 = DGPS fix
                    3 = PPS fix
                    4 = Real Time Kinematic
                    5 = Float RTK
                    6 = estimated(dead reckoning)(2.3 feature)
                    7 = Manual input mode
                    8 = Simulation mode
                (7) Number of satellites being tracked
                (8) 0.9 Horizontal dilution of position
                (9) 545.4 Altitude (ALWAYS in Meters, above mean sea level)
                (10) 1.2 time in seconds since last DGPS update

                (11) 022.4 Speed over the ground in knots - can be positive or negative

                FROM AHRS:
                (12) Heading in degrees
                (13) Roll angle in degrees(positive roll = right leaning - right down, left up)
                (14) Pitch angle in degrees(Positive pitch = nose up)
                (15) Yaw Rate in Degrees / second

                * CHKSUM
                */
            }
        }

        private void ParseVTG()
        {
            //$GPVTG,054.7,T,034.4,M,005.5,N,010.2,K*48
            //is the sentence GGA
            if (!String.IsNullOrEmpty(words[1]) && !String.IsNullOrEmpty(words[5]))
            {
                //kph for speed - knots read
                double.TryParse(words[5], NumberStyles.Float, CultureInfo.InvariantCulture, out speed);
                speed = Math.Round(speed * 1.852, 1);

                //True heading
                double.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out headingTrue);

                //average the speed
                AverageTheSpeed();
            }
            else
            {
                speed = 0;
            }
        }

        private void ParseHDT()
        {
            /* $GNHDT,123.456,T * 00

            Field Meaning
            0   Message ID $GNHDT
            1   Heading in degrees
            2   T: Indicates heading relative to True North
            3   The checksum data, always begins with *
                */

            if (!String.IsNullOrEmpty(words[1]))
            {
                //True heading
                double.TryParse(words[1], NumberStyles.Float, CultureInfo.InvariantCulture, out headingHDT);
            }
        }

        private void ParseSTI032() //heading and roll from SkyTraQ receiver
        {
            if (!String.IsNullOrEmpty(words[10]))
            {
                //baselineCourse: angle between baseline vector (from kinematic base to rover) and north direction, degrees
                double.TryParse(words[10], NumberStyles.Float, CultureInfo.InvariantCulture, out baselineCourse);
                headingHDT = (baselineCourse < 270) ? (double)(baselineCourse + 90) : (double)(baselineCourse - 270); //Rover Antenna on the left, kinematic base on the right!!!
            }

            if (!String.IsNullOrEmpty(words[8]) && !String.IsNullOrEmpty(words[9]))
            {
                double.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out upProjection); //difference in hight of both antennas (rover - kinematic base)
                double.TryParse(words[9], NumberStyles.Float, CultureInfo.InvariantCulture, out baselineLength); //distance between kinematic base and rover
                nRoll = Math.Atan(upProjection / baselineLength) * 180 / Math.PI; //roll to the right is positiv (rover left, kinematic base right!)

                if (mf.ahrs.isRollFromGPS)
                //input to the kalman filter
                {
                    rollK = nRoll;

                    //Kalman filter
                    Pc = P + varProcess;
                    G = Pc / (Pc + varRoll);
                    P = (1 - G) * Pc;
                    Xp = XeRoll;
                    Zp = Xp;
                    XeRoll = (G * (rollK - Zp)) + Xp;

                    mf.ahrs.rollX16 = (int)(XeRoll * 16);
                }
                else mf.ahrs.rollX16 = 0;
            }

            /*
            $PSTI,032,033010.000,111219,A,R,‐4.968,‐10.817,‐1.849,12.046,204.67,,,,,*39
            (1) 032 Baseline Data indicator
            (2) UTC time hhmmss.sss
            (3) UTC date ddmmyy
            (4) Status:
                V = Void
                A = Active
            (5) Mode Indicator:
                F = Float RTK
                R = fixed RTK
            (6) East-projection of baseline, meters
            (7) North-projection of baseline, meters
            (8) Up-projection of baseline, meters
            (9) Baseline length, meters
            (10) Baseline course: angle between baseline vector and north direction, degrees
            (11) - (15) Reserved
            (16) * Checksum
            */
        }

        private void ParseTRA()  //tra contains hdt and roll for the ub482 receiver
        {
            if (!String.IsNullOrEmpty(words[1]))
            {

                double.TryParse(words[2], NumberStyles.Float, CultureInfo.InvariantCulture, out headingHDT);
                //  Console.WriteLine(headingHDT);
                double.TryParse(words[3], NumberStyles.Float, CultureInfo.InvariantCulture, out nRoll);
                // Console.WriteLine(nRoll);


                int.TryParse(words[5], NumberStyles.Float, CultureInfo.InvariantCulture, out int trasolution);
                if (trasolution != 4) nRoll = 0;

                mf.ahrs.rollX16 = mf.ahrs.isRollFromGPS ? (int)(nRoll * 16) : 0;
            }
        }

        private void ParseRMC()
        {
            //GPRMC parsing of the sentence
            //make sure there aren't missing coords in sentence
            if (!String.IsNullOrEmpty(words[3]) && !String.IsNullOrEmpty(words[4])
                && !String.IsNullOrEmpty(words[5]) && !String.IsNullOrEmpty(words[6]))
            {
                if (fixFrom == "RMC")
                {
                    //get latitude and convert to decimal degrees
                    double.TryParse(words[3].Substring(0, 2), NumberStyles.Float, CultureInfo.InvariantCulture, out latitude);
                    double.TryParse(words[3].Substring(2), NumberStyles.Float, CultureInfo.InvariantCulture, out double temp);
                    latitude += temp * 0.01666666666666666666666666666667;

                    if (words[4] == "S")
                    {
                        latitude *= -1;
                        hemisphere = 'S';
                    }
                    else { hemisphere = 'N'; }

                    //get longitude and convert to decimal degrees
                    double.TryParse(words[5].Substring(0, 3), NumberStyles.Float, CultureInfo.InvariantCulture, out longitude);
                    double.TryParse(words[5].Substring(3), NumberStyles.Float, CultureInfo.InvariantCulture, out temp);
                    longitude += temp * 0.01666666666666666666666666666667;

                    if (words[6] == "W") longitude *= -1;

                    //calculate zone and UTM coords
                    UpdateNorthingEasting();
                }

                //Convert from knots to kph for speed
                double.TryParse(words[7], NumberStyles.Float, CultureInfo.InvariantCulture, out speed);
                speed = Math.Round(speed * 1.852, 1);

                //True heading
                double.TryParse(words[8], NumberStyles.Float, CultureInfo.InvariantCulture, out headingTrue);

                //Status
                if (String.IsNullOrEmpty(words[2]))
                {
                    status = "z";
                }
                else
                {
                    try { status = words[2]; }
                    catch (Exception e)
                    {
                        mf.WriteErrorLog("Parse RMC" + e);
                    }
                }


                //average the speed
                AverageTheSpeed();
            }
        }


        ////////////////////////////////////////////////////////////////////////////////////////////////////////
        private const double sm_a = 6378137.0;

        private const double sm_b = 6356752.314;
        private const double UTMScaleFactor = 0.9996;
        //private double UTMScaleFactor2 = 1.0004001600640256102440976390556;

        private double ArcLengthOfMeridian(double phi)
        {
            const double n = (sm_a - sm_b) / (sm_a + sm_b);
            double alpha = ((sm_a + sm_b) / 2.0) * (1.0 + (Math.Pow(n, 2.0) / 4.0) + (Math.Pow(n, 4.0) / 64.0));
            double beta = (-3.0 * n / 2.0) + (9.0 * Math.Pow(n, 3.0) * 0.0625) + (-3.0 * Math.Pow(n, 5.0) / 32.0);
            double gamma = (15.0 * Math.Pow(n, 2.0) * 0.0625) + (-15.0 * Math.Pow(n, 4.0) / 32.0);
            double delta = (-35.0 * Math.Pow(n, 3.0) / 48.0) + (105.0 * Math.Pow(n, 5.0) / 256.0);
            double epsilon = (315.0 * Math.Pow(n, 4.0) / 512.0);
            return alpha * (phi + (beta * Math.Sin(2.0 * phi))
                    + (gamma * Math.Sin(4.0 * phi))
                    + (delta * Math.Sin(6.0 * phi))
                    + (epsilon * Math.Sin(8.0 * phi)));
        }

        private double[] MapLatLonToXY(double phi, double lambda, double lambda0)
        {
            double[] xy = new double[2];
            double ep2 = (Math.Pow(sm_a, 2.0) - Math.Pow(sm_b, 2.0)) / Math.Pow(sm_b, 2.0);
            double nu2 = ep2 * Math.Pow(Math.Cos(phi), 2.0);
            double n = Math.Pow(sm_a, 2.0) / (sm_b * Math.Sqrt(1 + nu2));
            double t = Math.Tan(phi);
            double t2 = t * t;
            double l = lambda - lambda0;
            double l3Coef = 1.0 - t2 + nu2;
            double l4Coef = 5.0 - t2 + (9 * nu2) + (4.0 * (nu2 * nu2));
            double l5Coef = 5.0 - (18.0 * t2) + (t2 * t2) + (14.0 * nu2) - (58.0 * t2 * nu2);
            double l6Coef = 61.0 - (58.0 * t2) + (t2 * t2) + (270.0 * nu2) - (330.0 * t2 * nu2);
            double l7Coef = 61.0 - (479.0 * t2) + (179.0 * (t2 * t2)) - (t2 * t2 * t2);
            double l8Coef = 1385.0 - (3111.0 * t2) + (543.0 * (t2 * t2)) - (t2 * t2 * t2);

            /* Calculate easting (x) */
            xy[0] = (n * Math.Cos(phi) * l)
                + (n / 6.0 * Math.Pow(Math.Cos(phi), 3.0) * l3Coef * Math.Pow(l, 3.0))
                + (n / 120.0 * Math.Pow(Math.Cos(phi), 5.0) * l5Coef * Math.Pow(l, 5.0))
                + (n / 5040.0 * Math.Pow(Math.Cos(phi), 7.0) * l7Coef * Math.Pow(l, 7.0));

            /* Calculate northing (y) */
            xy[1] = ArcLengthOfMeridian(phi)
                + (t / 2.0 * n * Math.Pow(Math.Cos(phi), 2.0) * Math.Pow(l, 2.0))
                + (t / 24.0 * n * Math.Pow(Math.Cos(phi), 4.0) * l4Coef * Math.Pow(l, 4.0))
                + (t / 720.0 * n * Math.Pow(Math.Cos(phi), 6.0) * l6Coef * Math.Pow(l, 6.0))
                + (t / 40320.0 * n * Math.Pow(Math.Cos(phi), 8.0) * l8Coef * Math.Pow(l, 8.0));

            return xy;
        }

        public double[] DecDeg2UTM(double latitude, double longitude)
        {
            //only calculate the zone once!
            if (!mf.isGPSPositionInitialized) zone = Math.Floor((longitude + 180.0) * 0.16666666666666666666666666666667) + 1;

            double[] xy = MapLatLonToXY(latitude * 0.01745329251994329576923690766743,
                                        longitude * 0.01745329251994329576923690766743,
                                        (-183.0 + (zone * 6.0)) * 0.01745329251994329576923690766743);

            xy[0] = (xy[0] * UTMScaleFactor) + 500000.0;
            xy[1] *= UTMScaleFactor;
            if (xy[1] < 0.0)
                xy[1] += 10000000.0;
            return xy;
        }
    }
}

//                #region $GPRMC

//                //GPRMC parsing of the sentence
//                //make sure there aren't missing coords in sentence
//                if (words[0] == "$GPRMC" & words[3] != "" & words[4] != "" & words[5] != "" & words[6] != "")
//                {
//                    //Status
//                    if (words[2] == String.Empty) status = "z";
//                    else
//                    {
//                        try { status = words[2]; }
//                        catch (ArgumentNullException) { }
//                    }

//                    //Date and Time
//                    if (words[1].Length != 0)
//                    {
//                        try
//                        {
//                            if (words[1].Length == 6)
//                            {
//                                // Only HHMMSS
//                                utcDateTime = new DateTime(
//                                    (int.Parse(words[9].Substring(4, 2)) + 2000),
//                                    int.Parse(words[9].Substring(2, 2)),
//                                    int.Parse(words[9].Substring(0, 2)),
//                                    int.Parse(words[1].Substring(0, 2)),
//                                    int.Parse(words[1].Substring(2, 2)),
//                                    int.Parse(words[1].Substring(4, 2)));
//                            }
//                            else
//                            {
//                                // HHMMSS.MS
//                                utcDateTime = new DateTime(
//                                    (int.Parse(words[9].Substring(4, 2)) + 2000),
//                                    int.Parse(words[9].Substring(2, 2)),
//                                    int.Parse(words[9].Substring(0, 2)),
//                                    int.Parse(words[1].Substring(0, 2)),
//                                    int.Parse(words[1].Substring(2, 2)),
//                                    int.Parse(words[1].Substring(4, 2)),
//                                    int.Parse(words[1].Substring(7)));
//                            }
//                        }
//                        catch (ArgumentNullException) { }
//                    }

//                    //update the receive counter that detects loss of communication
//                    //update that RMC data is newly updated
//                    updatedRMC = true;
//                }//end $GPRMC
//#endregion $GPRMC