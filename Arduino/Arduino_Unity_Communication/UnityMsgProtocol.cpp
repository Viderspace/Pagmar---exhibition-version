#include "UnityMsgProtocol.h"

void DebugMassage(String Msg){
  Serial.println(Msg);
}

void SendMassage(String device_kind, String device_name)
{
    if (device_kind.length() <=1 || device_name.length() <= 1){
          Serial.println("-------------------------------------");
      Serial.println("Bad call to SendMassge");
      Serial.println(device_kind + DELIMITER + device_name);
      Serial.println("-------------------------------------");
      return;
  }
      Serial.println(device_kind + DELIMITER + device_name);

}

void SendMassage(String device_kind, String device_name, String value)
{
  // if (device_kind.length() <=1 || device_name.length() <= 1){
  //         Serial.println("-------------------------------------");
      // Serial.println("Bad call to SendMassge");
      // Serial.println(device_kind + DELIMITER + device_name + DELIMITER + value);
      // Serial.println("-------------------------------------");
      // return;
  // }
  Serial.println(device_kind + DELIMITER + device_name + DELIMITER + value);
}
