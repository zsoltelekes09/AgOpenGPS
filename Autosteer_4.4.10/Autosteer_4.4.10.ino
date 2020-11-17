////////////// User Settings /////////////////////////////
  /* 
   *  Wheel angle sensor zero point...
   *  
   * 3320 minus 127 = 3193. 1288 counts per 1 volt
   * The 127 (half of 255) is so you can take WAS zero of 3320 
   * from 3193 to 3347. Zero from AOG is sent as a number from 0 to 255. 
   * 
   * Leave at 3193 - Only change if using a factory OEM wheel angle Sensor
   * Put your wheels straight forward, adjust WAS physical position
   * So it puts out 2.5v. Do a good installation!
   * 
   * Factory WAS - Wheels pointing forward, measure the voltage. 
   * Example 2.2v - So, 2.5 minus 2.2 = 0.3v times 
   * 1288 counts per volt = 386 counts. 3320 - 386 - 127 = 2706.
   * So your new WAS_ZERO is 2706.
   */

#define WAS_ZERO 3193
  
//enables the UDP communication
#define Enable_UDP false

//enable serial1 & serial2 for F9p stack [ubx protocol]
#define F9PStack false

//enables hydraulic relays
#define Enable_Hydraulic_Lift false


#define Enable_Section_Control_Output false

#define Enable_Section_Control_Input false



//   ***********  Hydraulic Lift  **************
#define LOWER 18
#define RAISE 19


//{0,0,14} section 3 on pin 14
//{0,14,0} section 2 on pin 14

byte Sections_Output[100] = {14, 15, 16, 17};//Section pins just add pins to enable starts with first section

byte Sections_InputAuto[100] = {14, 15, 0, 0};//Section pins
byte Sections_InputOn[100] =   {0, 0, 16, 17};//Section pins




#define ToolIndex = 0x00;//dont change unless you know what you are doing


  //   ***********  Motor drive connections  **************
  
  //Connect ground only for cytron, Connect Ground and +5v for IBT2

  //PWM1 for Cytron PWM, Left PWM for IBT2
  #define PWM1_LPWM  3  //PD3
  //Dir1 for Cytron Dir, Both L and R enable for IBT2
  #define DIR1_RL_ENABLE  4  //PD4
  //Not Connected for Cytron, Right PWM for IBT2
  #define PWM2_RPWM  9 //D9

  //--------------------------- Switch Input Pins ------------------------
  #define STEERSW_PIN 6 //PD6
  #define WORKSW_PIN 7  //PD7
  #define REMOTE_PIN 8  //PB0

  #include "bNO055_AOG.h"  // BNO055 IMU
  
  #define A 0X28             //I2C address selection pin LOW
  #define B 0x29             //                          HIGH
  BNO055 IMU(A);  // create an instance

  //How many degrees before decreasing Max PWM
  #define LOW_HIGH_DEGREES 5.0

  //value for max step in roll noise 5 to 20 range
  #define ROLL_DSP_STEP 5
  
 //PWM Frequency -> 490hz (default) = 0 and -> 122hz = 1  -> 3921hz = 2
  #define PWM_Frequency 0

  
////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

  // if not in eeprom, overwrite
  #define EEP_Ident 4410
  
  //version in AOG ex. v4.4.10 -> 4+4+10=18
  #define aogVersion 18

  #define RAD2GRAD 57.2957795

  #include <Wire.h>
  #include <EEPROM.h>
  #include "zADS1015.h"
  Adafruit_ADS1115 ads;     // Use this for the 16-bit version ADS1115
  
  //GY-45 (1C)
  //installed Sparkfun, Adafruit MMA8451 (1D)   
  #include "mMA8452_AOG.h"  //MMA inclinometer
  
  MMA8452 MMA1D(0x1D);
  MMA8452 MMA1C(0x1C);


  

uint16_t x_ , y_ , z_;        

//loop time variables in microseconds  
const unsigned int LOOP_TIME = 50;      
unsigned long lastTime = LOOP_TIME;
unsigned long currentTime = LOOP_TIME;
byte watchdogTimer = 20;
byte HydraulicLiftWatchdog = 20;
byte serialResetTimer = 100; //if serial buffer is getting full, empty it

#if (F9PStack)
  byte RelPosNEDBuffer[72]= {0xB5, 0x62, 0x01, 0x3c, 0x40, 0x00};
  byte PVTBuffer[100]= {0xB5, 0x62, 0x01, 0x07, 0x5c, 0x00};
  byte Index1 = 0, Index2 = 0;
#endif

#if (Enable_UDP)
  //ethercard 10,11,12,13
  #if defined(__AVR_ATmega2560__)
    #define CS_Pin 53
  #else
    // Arduino Nano = 10 depending how CS of Ethernet Controller ENC28J60 is Connected
    #define CS_Pin 10
  #endif

  #include "etherCard_AOG.h"
  #include <IPAddress.h>

  // ethernet interface ip address
  static byte myip[] = { 192,168,1,2 };
  // gateway ip address
  static byte gwip[] = { 192,168,1,1 };
  //DNS- you just need one anyway
  static byte myDNS[] = { 8,8,8,8 };
  //mask
  static byte mask[] = { 255,255,255,0 };
  //this is port of this autosteer module
  unsigned int portMy = 5577; 
  
  //sending back to where and which port
  static byte ipDestination[] = {192, 168, 1, 255};
  unsigned int portDestination = 9999; //AOG port that listens

  // ethernet mac address - must be unique on your network
  static byte mymac[] = { 0x70,0x69,0x69,0x2D,0x30,0x31 };
    
  byte Ethernet::buffer[300]; // udp send and receive buffer
  
#else
  byte Index;
  byte Recieve[30] = {127};
#endif

#if (Enable_Hydraulic_Lift)
  byte RaiseTimer = 0, LowerTimer = 0, LastTimer = 0;
#endif
  
#if (Enable_Section_Control_Input)
  byte SectionsState[sizeof(Sections_InputOn)];
#endif


  //Array to send udpData back to AgOpenGPS
  byte toSend[10] = {127,253,3};

  
  //inclinometer variables
  float rollK = 0, Pc = 0.0, G = 0.0, P = 1.0, Xp = 0.0, Zp = 0.0;
  float XeRoll = 0;
  float lastRoll=0, diff=0;
  int roll = 0;

  byte relayLo = 0;
  
  //Kalman control variances
  const float varRoll = 0.2; // variance, larger is more filtering
  const float varProcess = 0.02; //process, smaller is more filtering
  
  //Program flow

  bool MMAinitialized = false;
  byte RemoteSwitch = 0, workSwitch = 0;
  
  //steering variables
  int steeringPosition = 0;
  float steerAngleActual = 0, steerAngleError = 0, steerAngleSetPoint = 0;
  
    //pwm variables
  int pwmDrive = 0, pwmDisplay = 0, steerSwitch = 1;
  float pValue = 0, errorAbs = 0, highLowPerDeg = 0; 
  
  //Steer switch button  ***********************************************************************************************************
  byte reading;
  byte previous = 0;

   byte pulseCount = 0; // Steering Wheel Encoder
   bool encEnable = false; //debounce flag
   byte thisEnc = 0, lastEnc = 0;

   void(* resetFunc) (void) = 0;

     //Variables for settings  
   struct Storage {
      float Ko = 0.0f;  //overall gain
      float Kp = 0.0f;  //proportional gain
      float lowPWM = 0.0f;  //band of no action
      float Kd = 0.0f;  //derivative gain 
      float steeringPositionZero = 3320.0;
      byte minPWM=0;
      byte highPWM=100;//max PWM value
      float steerSensorCounts=10;        
  };  Storage steerSettings;  //26 bytes

    //Variables for settings - 0 is false  
   struct Setup {
      byte InvertWAS = 0;
      byte InvertRoll = 0;
      byte MotorDriveDirection = 0;
      byte SingleInputWAS = 1;
      byte CytronDriver = 1;
      byte SteerSwitch = 0;
      byte UseMMA_X_Axis = 0;
      byte ShaftEncoder = 0;
      byte WorkSwActiveLow = 0;
      byte WorkSwManual = 0;
      
      byte BNOInstalled = 0;
      byte InclinometerInstalled = 0;   // set to 0 for none
                                        // set to 1 if DOGS2 Inclinometer is installed and connected to ADS pin A2
                                        // set to 2 if MMA8452 installed GY-45 (1C)
                                        // set to 3 if MMA8452 installed Sparkfun, Adafruit MMA8451 (1D)      
      byte maxSteerSpeed = 100;//Speed * 5
      byte minSteerSpeed = 5;//Speed * 5
      byte PulseCountMax = 5; 
      byte AckermanFix = 100;     //sent as percent
      byte isRelayActiveHigh = 0; //if zero, active low (default)

  };  Setup aogSettings;          //17 bytes


  //Variables for config - 0 is false  
  struct Config {
  byte RaiseTime = 2;
  byte LowerTime = 4;
  byte Enabled = 0;
  byte IsRelayActiveHigh = 0; //if zero, active low (default)
  
  };  Config HydraulicLift;   //4 bytes

void setup()
{    
  //PWM rate settings
 if (PWM_Frequency == 1) 
  { 
    TCCR2B = TCCR2B & B11111000 | B00000110;    // set timer 2 to 256 for PWM frequency of   122.55 Hz
    TCCR1B = TCCR1B & B11111000 | B00000100;    // set timer 1 to 256 for PWM frequency of   122.55 Hz
  }
  else if (PWM_Frequency == 2)
  {
    TCCR1B = TCCR1B & B11111000 | B00000010;    // set timer 1 to 8 for PWM frequency of  3921.16 Hz
    TCCR2B = TCCR2B & B11111000 | B00000010;    // set timer 2 to 8 for PWM frequency of  3921.16 Hx
  }
  
  //keep pulled high and drag low to activate, noise free safe    
  pinMode(WORKSW_PIN, INPUT_PULLUP); 
  pinMode(STEERSW_PIN, INPUT_PULLUP); 
  pinMode(REMOTE_PIN, INPUT_PULLUP); 
  pinMode(DIR1_RL_ENABLE, OUTPUT);
  
  if (aogSettings.CytronDriver) pinMode(PWM2_RPWM, OUTPUT); 

#if (Enable_Hydraulic_Lift)
  //set the pins to be outputs (pin numbers)
  pinMode(LOWER, OUTPUT);
  pinMode(RAISE, OUTPUT);
#endif

#if (Enable_Section_Control_Output)
  for (int k = 0; k < sizeof(Sections_Output); k++)
  {
    if (Sections_Output[k]) pinMode(Sections_Output[k], OUTPUT);
  }
#elif (Enable_Section_Control_Input)
  for (int k = 0; k < sizeof(Sections_InputOn); k++)
  {
    if (Sections_InputOn[k]) pinMode(Sections_InputOn[k], INPUT_PULLUP);
  }
  for (int k = 0; k < sizeof(Sections_InputAuto); k++)
  {
    if (Sections_InputAuto[k]) pinMode(Sections_InputAuto[k], INPUT_PULLUP);
  }
#endif

  //set up communication
  Wire.begin();
  Serial.begin(38400);

  #if (Enable_UDP && F9PStack)
    Serial1.begin(19200);
    Serial2.begin(19200);
  #endif
  
  //50Khz I2C
  TWBR = 144;
  
  delay(50);

  int EEread = 0;
  EEPROM.get(0, EEread);              // read identifier
    
  if (EEread != EEP_Ident)   // check on first start and write EEPROM
  {
    EEPROM.put(0, EEP_Ident);
    EEPROM.put(2, WAS_ZERO);
    EEPROM.put(10, steerSettings);   
    EEPROM.put(40, aogSettings);
    EEPROM.put(60, HydraulicLift);
  }
  else
  {
    EEPROM.get(10, steerSettings);     // read the Settings
    EEPROM.get(40, aogSettings);
    EEPROM.get(60, HydraulicLift);
  }

  // for PWM High to Low interpolator
  highLowPerDeg = (steerSettings.highPWM - steerSettings.lowPWM) / LOW_HIGH_DEGREES;

  // BNO055 init
  if (aogSettings.BNOInstalled) 
  { 
    IMU.init();  
    IMU.setExtCrystalUse(true);   //use external 32K crystal
  }  

  if (aogSettings.InclinometerInstalled == 2 )
  { 
      // MMA8452 (1) Inclinometer
      MMAinitialized = MMA1C.init();
      
      if (MMAinitialized)
      {
        MMA1C.setDataRate(MMA_12_5hz);
        MMA1C.setRange(MMA_RANGE_2G);
        MMA1C.setHighPassFilter(false); 
      }
      else Serial.println("MMA init fails!!");
  }
  else if (aogSettings.InclinometerInstalled == 3 )
  { 
      // MMA8452 (1) Inclinometer
      MMAinitialized = MMA1D.init();
      
      if (MMAinitialized)
      {
        MMA1D.setDataRate(MMA_12_5hz);
        MMA1D.setRange(MMA_RANGE_2G);
        MMA1D.setHighPassFilter(false); 
      }
      else Serial.println("MMA init fails!!");
  }


 #if (Enable_UDP)
  if (ether.begin(sizeof Ethernet::buffer, mymac, CS_Pin) == 0)
  Serial.println("Failed to access Ethernet controller");
  
  //set up connection
  ether.staticSetup(myip, gwip, myDNS, mask); 
  
  ether.printIp("IP:  ", ether.myip);
  ether.printIp("GW:  ", ether.gwip);
  ether.printIp("DNS: ", ether.dnsip);

  //register udpSerialPrint() to port 8888
  ether.udpServerListenOnPort(&udpSteerRecv, 8888);
   
  #if (F9PStack)
    ether.udpServerListenOnPort(&udpNtrip, 7777);
  #endif
 #endif
 
  Serial.println("Setup complete, waiting for AgOpenGPS");
}// End of Setup



void loop()
{
 #if (Enable_UDP)
  //this must be called for ethercard functions to work. Calls udpSteerRecv() defined in UDPCommunication.
  ether.packetLoop(ether.packetReceive());
 #endif
  // Loop triggers every 50 msec and sends back gyro heading, and roll etc   
  currentTime = millis();
  if (currentTime - lastTime >= LOOP_TIME)
  {
    lastTime = currentTime;
    //reset debounce
    encEnable = true;
    //If connection lost to AgOpenGPS, the watchdog will count up and turn off steering
    if (watchdogTimer++ > 12) watchdogTimer = 12;

    if (aogSettings.BNOInstalled) IMU.readIMU();
    
    //DOGS2 inclinometer
    if (aogSettings.InclinometerInstalled > 0)
    {
      if (aogSettings.InclinometerInstalled == 1)
      {
        roll = ((ads.readADC_SingleEnded(2))); // 24,000 to 2700
        roll = (roll - 13300) >> 5; //-375 to 375 -25 deg to +25 deg
      }
      else
      {
        if (MMAinitialized)
        {
          // MMA8452 Inclinometer (1C)
          if (aogSettings.InclinometerInstalled == 2) MMA1C.getRawData(&x_, &y_, &z_);
          else MMA1D.getRawData(&x_, &y_, &z_);//MMA8452 Inclinometer (1D)

          if (aogSettings.UseMMA_X_Axis) roll= x_; //Conversion uint to int
          else roll = y_;

          //16 counts per degree (good for 0 - +/-18 degrees)
          if (roll > 4200)  roll =  4200;
          if (roll < -4200) roll = -4200;
          roll = roll >> 4;  //divide by 8  +-525
          //divide by 2 -268 to +268    -17 to +17 degrees
        }
      }

      // limit the differential
      diff = roll - lastRoll;
      if (diff > ROLL_DSP_STEP ) roll = lastRoll + ROLL_DSP_STEP;
      else if (diff < -ROLL_DSP_STEP) roll = lastRoll - ROLL_DSP_STEP;
      lastRoll = roll;

      //if not positive when rolling to the right
      if (aogSettings.InvertRoll) roll *= -1.0;

      //for Kalman
      rollK = roll;
    }//-----------------------------------------------------------------------------
                
    //Kalman filter
    Pc = P + varProcess;
    G = Pc / (Pc + varRoll);
    P = (1 - G) * Pc;
    Xp = XeRoll;
    Zp = Xp;
    XeRoll = G * (rollK - Zp) + Xp;


    //read all the switches
    reading = digitalRead(WORKSW_PIN);
    if (reading != workSwitch)
    {
        workSwitch = reading;
        
        byte num = workSwitch;
        
        if (aogSettings.WorkSwActiveLow) num = num ^ 0x01;
        if (aogSettings.WorkSwManual) num += num << 1;
        
        toSend[1] = 0xC5;
        toSend[2] = 0x04;
        toSend[3] = num;
        SendData(toSend, 4);
    }
    
    if (aogSettings.SteerSwitch == 1) //steer switch on - off
    {
      reading = digitalRead(STEERSW_PIN); //read auto steer enable switch open = 0n closed = Off
      if (reading != steerSwitch)
      {
        steerSwitch = reading;
        pulseCount = 0;
        toSend[1] = 0xC4;
        toSend[2] = 0x04;
        toSend[3] = (byte)steerSwitch << 1 ^ 0x02;
        SendData(toSend, 4);
      }
    }
    else   //steer Button momentary
    {
      reading = digitalRead(STEERSW_PIN);
      if (reading == LOW && previous == HIGH)
      {
        pulseCount = 0;
        if (steerSwitch == 1) steerSwitch = 0;
        else steerSwitch = 1;

        toSend[1] = 0xC4;
        toSend[2] = 0x04;
        toSend[3] = (byte)steerSwitch << 1 ^ 0x02;
        SendData(toSend, 4);
      }
      previous = reading;
    }
    
    #if (Enable_Section_Control_Input)
      for (int k = 0; k < sizeof(Sections_InputOn); k++)
      {
        if (Sections_InputOn[k]) reading = (byte)digitalRead(Sections_InputOn[k]) << 1 ^ 0x02;
        else reading = 0;
        if (Sections_InputAuto[k]) reading |= (byte)digitalRead(Sections_InputAuto[k]) ^ 0x01;

        if (reading != SectionsState[k])
        {
          SectionsState[k] = (byte)reading;
          toSend[1] = 0xC1;
          toSend[2] = 0x06;
          toSend[3] = ToolIndex;
          toSend[4] = (byte)k;
          toSend[5] = SectionsState[k];
          SendData(toSend, 6);
        }
      }

    #endif
    
    //get steering position       
    if (aogSettings.SingleInputWAS) steeringPosition = ads.readADC_SingleEnded(0);//ADS1115 Single Mode 
    else steeringPosition = ads.readADC_Differential_0_1(); //ADS1115 Differential Mode

    //DETERMINE ACTUAL STEERING POSITION bit shift by 2  0 to 6640 is 0 to 5v
    steeringPosition = ((steeringPosition >> 2) - steerSettings.steeringPositionZero);//read the steering position sensor
          
      //convert position to steer angle. 32 counts per degree of steer pot position in my case
      //  ***** make sure that negative steer angle makes a left turn and positive value is a right turn *****
    if (aogSettings.InvertWAS)
        steerAngleActual = (float)(steeringPosition) / -steerSettings.steerSensorCounts;
    else
        steerAngleActual = (float)(steeringPosition) / steerSettings.steerSensorCounts; 

     //Ackerman fix
    if (steerAngleActual < 0) steerAngleActual = (steerAngleActual * aogSettings.AckermanFix)/100;

    //if (steerAngleActual < 0) steerAngleActual = ackermanAngle-degrees(asin(2.*Alpha - sin(radians(ackermanAngle + steerAngleActual))));

    //RealisticAngle = ackermanAngle+degrees(asin(2.*Alpha - sin(radians(ackermanAngle + steerAngleSetPoint))));//right wheel sensor
    //RealisticAngle = -ackermanAngle+degrees(asin(2.*Alpha - sin(radians(ackermanAngle + -steerAngleSetPoint))));//left wheel sensor

    #if (Enable_Hydraulic_Lift)
      if (HydrLiftWatchdog++ > 11) HydrLiftWatchdog = 11;
      if (RaiseTimer)
      {
        if (HydrLiftWatchdog > 10) RaiseTimer = 1;
        RaiseTimer--;
        if (!RaiseTimer)
          if (HydraulicLift.IsRelayActiveHigh) digitalWrite(RAISE,HIGH);
          else digitalWrite(RAISE,LOW);
        }
      }
      if (LowerTimer)
      {
        if (HydrLiftWatchdog > 10) LowerTimer = 1;
        LowerTimer--;
        if (!LowerTimer)
        {
          if (HydraulicLift.IsRelayActiveHigh) digitalWrite(LOWER,HIGH);
          else digitalWrite(LOWER,LOW);
        }
      }
    #endif


    if (watchdogTimer < 10)
    {
      //Disable H Bridge for IBT2, hyd aux, etc for cytron
      if (aogSettings.CytronDriver) 
      {
        if (aogSettings.isRelayActiveHigh) digitalWrite(PWM2_RPWM, 0);
        else digitalWrite(PWM2_RPWM, 1);
      }
      else digitalWrite(DIR1_RL_ENABLE, 1);
        
      steerAngleError = steerAngleActual - steerAngleSetPoint;   //calculate the steering error
      //if (abs(steerAngleError)< steerSettings.lowPWM) steerAngleError = 0;

      calcSteeringPID();  //do the pid
    }
    else
    {
      //we've lost the comm to AgOpenGPS, or just stop request
      //Disable H Bridge for IBT2, hyd aux, etc for cytron
      if (aogSettings.CytronDriver) 
      {
        if (aogSettings.isRelayActiveHigh) digitalWrite(PWM2_RPWM, 1);
        else digitalWrite(PWM2_RPWM, 0);
      }
      else digitalWrite(DIR1_RL_ENABLE, 0);

      pwmDrive = 0; //turn off steering motor
    }

    motorDrive();       //out to motors the pwm value  
 }//end of timed loop

 #if (Enable_UDP)
  ether.packetLoop(ether.packetReceive());
 #endif
 

  if (encEnable)
  {
    thisEnc = digitalRead(REMOTE_PIN);
    if (thisEnc != lastEnc)
    {
      lastEnc = thisEnc;
      if ( lastEnc)
      {
        if (encEnable)
        {
          pulseCount++;
          encEnable = false;

          if (aogSettings.ShaftEncoder && pulseCount >= aogSettings.PulseCountMax ) 
          {
            watchdogTimer = 12;
      
            toSend[1] = 0xC4;
            toSend[2] = 0x04;
            toSend[3] = 0x10;
            SendData(toSend, 4);
          }
        }
      }
    }
  }
  
  ReadSerials();
} // end of main loop

  
  //TCCR2B = TCCR2B & B11111000 | B00000001;    // set timer 2 divisor to     1 for PWM frequency of 31372.55 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000010;    // set timer 2 divisor to     8 for PWM frequency of  3921.16 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000011;    // set timer 2 divisor to    32 for PWM frequency of   980.39 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000100;    // set timer 2 divisor to    64 for PWM frequency of   490.20 Hz (The DEFAULT)
  //TCCR2B = TCCR2B & B11111000 | B00000101;    // set timer 2 divisor to   128 for PWM frequency of   245.10 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000110;    // set timer 2 divisor to   256 for PWM frequency of   122.55 Hz
  //TCCR2B = TCCR2B & B11111000 | B00000111;    // set timer 2 divisor to  1024 for PWM frequency of    30.64 Hz

  //TCCR1B = TCCR1B & B11111000 | B00000001;    // set timer 1 divisor to     1 for PWM frequency of 31372.55 Hz
  //TCCR1B = TCCR1B & B11111000 | B00000010;    // set timer 1 divisor to     8 for PWM frequency of  3921.16 Hz
  //TCCR1B = TCCR1B & B11111000 | B00000011;    // set timer 1 divisor to    64 for PWM frequency of   490.20 Hz (The DEFAULT)
  //TCCR1B = TCCR1B & B11111000 | B00000100;    // set timer 1 divisor to   256 for PWM frequency of   122.55 Hz
  //TCCR1B = TCCR1B & B11111000 | B00000101;    // set timer 1 divisor to  1024 for PWM frequency of    30
   
