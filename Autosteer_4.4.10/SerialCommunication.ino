#if (Enable_UDP)
  #if (F9PStack)
    void udpNtrip(uint16_t dest_port, uint8_t src_ip[IP_LEN], uint16_t src_port, byte *udpData, uint16_t len)
    {
      Serial1.write(udpData, len);
    }
  #endif

  //callback when received packets
  void udpSteerRecv(uint16_t dest_port, uint8_t src_ip[IP_LEN], uint16_t src_port, byte *udpData, uint16_t len)
  {
    ParseData(udpData, len);
  }
#endif

void ReadSerials()
{
  byte Data = 0;
  #if (!Enable_UDP)
    while (Serial.available() > 0)
    {
      Data = Serial.read();
      if (Index == 0)
      {
        if (Data == Recieve[Index]) Index++;
        else Index = 0;
      }
      else
      {
        Recieve[Index] = Data;
        if (Index > 2 && Index + 1 >= Recieve[2])
        {
          ParseData(Recieve, Recieve[2]);
          Index = 0;
        }
        else Index++;
      }
    }
  #elif (F9PStack && Enable_UDP)
    while (Serial1.available() > 0)//PVT
    {
      Data = Serial1.read();
      if (Index1 < 6)
      {
        if (Data == PVTBuffer[Index1]) Index1++;
        else Index1 = 0;
      }
      else
      {
        PVTBuffer[Index1] = Data;
        if (Index1 >= 99)
        {
          SendData(PVTBuffer, 101);
          Index1 = 0;
        }
        else Index1++;
      }
    }
    while (Serial2.available() > 0)//RelPosNED
    {
      Data = Serial2.read();
      if (Index2 < 6)
      {
        if (Data == RelPosNEDBuffer[Index2]) Index2++;
        else Index2 = 0;
      }
      else
      {
        RelPosNEDBuffer[Index2] = Data;
        if (Index2 >= 71)
        {
          SendData(RelPosNEDBuffer, 72);
          Index2 = 0;
        }
        else Index2++;
      }
    }
  #endif
}
