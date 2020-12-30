namespace AgOpenGPS
{
    public class CModuleComm
    {
        //copy of the mainform address
        private readonly FormGPS mf;

        //Critical Safety Properties
        public bool isOutOfBounds = true;

        public static int pgnSentenceLength = 10;

        //AutoSteer PGN - 32624 - 127.112 - 0x7F70             Header index Length Speed HiLo,  Dist HiLo, Angle HiLo
        public byte[] Send_AutoSteer = new byte[] { 0x80, 0x81, 0x7F, 0x70, 0x06, 0x00, 0x00, 0x7D, 0x14, 0x7D, 0x14, 0x00 };
        public byte[] Send_Sections = new byte[] { 0x80, 0x81, 0x7F, 0x71, 0x02, 0x00, 0x08, 0x00 };
        public byte[] Send_AutoSteerButton = new byte[] { 0x80, 0x81, 0x7F, 0x72, 0x01, 0x00, 0x00 };
        public byte[] Send_HydraulicLift = new byte[] { 0x80, 0x81, 0x7F, 0x73, 0x01, 0x00, 0x00 };
        public byte[] Send_Uturn = new byte[] { 0x80, 0x81, 0x7F, 0x74, 0x01, 0x80, 0x00 };
        public byte[] Send_Treeplant = new byte[] { 0x80, 0x81, 0x7F, 0x75, 0x01, 0x00, 0x00 };

        //Auto Steer Basic config -------------------------------------------------------------------------------
        public byte[] Config_AutoSteer = new byte[11];
        public int ssKp = 3, ssLowPWM = 4, ssKd = 5, ssKo = 6, ssSteerOffset = 7, ssMinPWM = 8, ssHighPWM = 9, ssCountsPerDegree = 10;

        // ----  Arduino Steer Config ----------------------------------------------------------------------------
        public byte[] Config_ardSteer = new byte[10];
        public int arSet0 = 3, arSet1 = 4, arMaxSpd = 5, arMinSpd = 6, arIncMaxPulse = 7, arAckermanFix = 8, arSet2 = 9;

        // ---- Arduino configuration on machine module  ---------------------------------------------------------
        public byte[] Config_ardMachine = new byte[7];
        public int amRaiseTime = 3, amLowerTime = 4, amEnableHyd = 5, amSet0 = 6;


        //UDP sentence just rec'd
        public string recvUDPSentence = "Inital UDP";

        public int lidarDistance, pwmDisplay = 0;

        //for the workswitch
        public bool isWorkSwitchActiveLow, isWorkSwitchEnabled, isWorkSwitchManual, RemoteAutoSteer;

        //constructor
        public CModuleComm(FormGPS _f)
        {
            mf = _f;

            //WorkSwitch logic
            isWorkSwitchEnabled = false;

            //does a low, grounded out, mean on
            isWorkSwitchActiveLow = true;

            ResetAllModuleCommValues(false);


            Config_AutoSteer[0] = 127;// PGN - 32764 as header
            Config_AutoSteer[1] = 252;
            Config_AutoSteer[2] = 11;
            Config_AutoSteer[ssKp] = Properties.Vehicle.Default.setAS_Kp;
            Config_AutoSteer[ssLowPWM] = Properties.Vehicle.Default.setAS_lowSteerPWM;
            Config_AutoSteer[ssKd] = Properties.Vehicle.Default.setAS_Kd;
            Config_AutoSteer[ssKo] = Properties.Vehicle.Default.setAS_Ko;
            Config_AutoSteer[ssSteerOffset] = Properties.Vehicle.Default.setAS_steerAngleOffset;
            Config_AutoSteer[ssMinPWM] = Properties.Vehicle.Default.setAS_minSteerPWM;
            Config_AutoSteer[ssHighPWM] = Properties.Vehicle.Default.setAS_highSteerPWM;
            Config_AutoSteer[ssCountsPerDegree] = Properties.Vehicle.Default.setAS_countsPerDegree;


            Config_ardSteer[0] = 127; //PGN - 32763
            Config_ardSteer[1] = 251;
            Config_ardSteer[2] = 10;
            Config_ardSteer[arSet0] = Properties.Vehicle.Default.setArdSteer_setting0;
            Config_ardSteer[arSet1] = Properties.Vehicle.Default.setArdSteer_setting1;
            Config_ardSteer[arMaxSpd] = Properties.Vehicle.Default.setArdSteer_maxSpeed;
            Config_ardSteer[arMinSpd] = Properties.Vehicle.Default.setArdSteer_minSpeed;
            byte inc = (byte)(Properties.Vehicle.Default.setArdSteer_inclinometer << 6);
            Config_ardSteer[arIncMaxPulse] = (byte)(inc + Properties.Vehicle.Default.setArdSteer_maxPulseCounts);
            Config_ardSteer[arAckermanFix] = Properties.Vehicle.Default.setArdSteer_ackermanFix;
            Config_ardSteer[arSet2] = Properties.Vehicle.Default.setArdSteer_setting2;

            //arduino machine configuration
            Config_ardMachine[0] = 127; //PGN - 32760
            Config_ardMachine[1] = 248;
            Config_ardMachine[2] = 7;
            Config_ardMachine[amRaiseTime] = Properties.Vehicle.Default.setArdMac_hydRaiseTime;
            Config_ardMachine[amLowerTime] = Properties.Vehicle.Default.setArdMac_hydLowerTime;
            Config_ardMachine[amEnableHyd] = Properties.Vehicle.Default.setArdMac_isHydEnabled;
            Config_ardMachine[amSet0] = Properties.Vehicle.Default.setArdMac_setting0;
        }

        //Reset all the byte arrays from modules
        public void ResetAllModuleCommValues(bool send)
        {
            Send_AutoSteer = new byte[] { 0x80, 0x81, 0x7F, 0x70, 0x06, 0x00, 0x00, 0x7D, 0x14, 0x7D, 0x14, 0x00 };
            Send_Sections = new byte[] { 0x80, 0x81, 0x7F, 0x71, 0x02, 0x00, 0x08, 0x00 };
            Send_AutoSteerButton = new byte[] { 0x80, 0x81, 0x7F, 0x72, 0x01, 0x00, 0x00 };
            Send_HydraulicLift = new byte[] { 0x80, 0x81, 0x7F, 0x73, 0x01, 0x00, 0x00 };
            Send_Uturn = new byte[] { 0x80, 0x81, 0x7F, 0x74, 0x01, 0x80, 0x00 };
            Send_Treeplant = new byte[] { 0x80, 0x81, 0x7F, 0x75, 0x01, 0x00, 0x00 };

            if (send)
            {
                mf.SendData(Send_AutoSteer, false);
                mf.SendData(Send_Sections, false);
                mf.SendData(Send_AutoSteerButton, false);
                mf.SendData(Send_HydraulicLift, false);
                mf.SendData(Send_Uturn, false);
                mf.SendData(Send_Treeplant, false);
            }
        }
    }
}