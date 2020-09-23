void calcSteeringPID(void) 
 {  
  //Proportional only
  pValue = steerSettings.Kp * steerAngleError;
  pwmDrive = (int)pValue;
  
  errorAbs = abs(steerAngleError);
  float newMax = 0; 
   
  if (errorAbs < LOW_HIGH_DEGREES)
  {
    newMax = (errorAbs * highLowPerDeg) + steerSettings.lowPWM;
  }
  else newMax = steerSettings.highPWM;
    
  //add min throttle factor so no delay from motor resistance.
  if (pwmDrive < 0 ) pwmDrive -= steerSettings.minPWM;
  else if (pwmDrive > 0 ) pwmDrive += steerSettings.minPWM;
  
  //limit the pwm drive
  if (pwmDrive > newMax) pwmDrive = newMax;
  if (pwmDrive < -newMax) pwmDrive = -newMax;

  if (aogSettings.MotorDriveDirection) pwmDrive *= -1;
 }

//#########################################################################################

void motorDrive(void) 
  {
    // Used with Cytron MD30C Driver
    // Steering Motor
    // Dir + PWM Signal
    if (aogSettings.CytronDriver)
    {     
      pwmDisplay = pwmDrive;
  
      //fast set the direction accordingly (this is pin DIR1_RL_ENABLE, port D, 4)
      if (pwmDrive >= 0) bitSet(PORTD, 4);  //set the correct direction
      else   
      {
        bitClear(PORTD, 4); 
        pwmDrive = -1 * pwmDrive;  
      }
  
      //write out the 0 to 255 value 
      analogWrite(PWM1_LPWM, pwmDrive);
    }
    else
    {
      // Used with IBT 2  Driver for Steering Motor
      // Dir1 connected to BOTH enables
      // PWM Left + PWM Right Signal     
      pwmDisplay = pwmDrive; 
    
      if (pwmDrive > 0)
      {
        analogWrite(PWM2_RPWM, 0);//Turn off before other one on
        analogWrite(PWM1_LPWM, pwmDrive);
      }      
      else
      {
        pwmDrive = -1 * pwmDrive;  
        analogWrite(PWM1_LPWM, 0);//Turn off before other one on
        analogWrite(PWM2_RPWM, pwmDrive);
      }
    }
  }
  
/*
 
void motorDrive2(void)
{
  if (watchdogTimer < 15)
  {
      digitalWrite(ENA_PIN,LOW);
      motorDrive();
  }
  else
  {
      digitalWrite(ENA_PIN,HIGH);
  }
  
  if (steerAngleError >= 0) digitalWrite(DIR_PIN, HIGH);
  else digitalWrite(DIR_PIN, LOW);
  
  if (steerAngleError*100 > 1 || steerAngleError*100 < -1){
    currentMicros = micros();
    if (currentMicros - prevmicros >= STEP_DELAY)
    {
      prevmicros = currentMicros;
      
      digitalWrite(PWM_PIN,HIGH);
      //delayMicroseconds(2.5);
      //digitalWrite(PWM1_PIN,LOW);
    }
    else
    {
      digitalWrite(PWM_PIN,LOW);
    }
  }
}


//float SteeringPivotPoint = 1.525;
float SteeringPivotPoint = 1.48242190485;
float TrackRot = 1.60;
float SteeringArm = 0.18;

float wheelBase = 2.41;
float messure = SteeringPivotPoint/wheelBase;

float Alpha = abs(TrackRot-SteeringPivotPoint)/(2*SteeringArm);

float ackermanAngle = asin(Alpha)*180./PI;


float RealisticAngle = ackermanAngle-(asin(2.*Alpha - sin(((ackermanAngle + steerAngleActual)*PI/180.)))*180./PI);
float prefectAngle = (atan(1/(1/tan(steerAngleActual*PI/180.)+messure))*180./PI);
Serial.println(prefectAngle,18);
Serial.println(RealisticAngle,18);


  */

  
