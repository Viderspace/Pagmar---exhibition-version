#include "WString.h"
#include <Arduino.h>

#ifndef UNITYMSGPROTOCOL_H
#define UNITYMSGPROTOCOL_H

#define DELIMITER ","

void DebugMassage(String Msg);

void SendMassage(String device_kind, String device_name);
// {
//       Serial.println(device_kind + DELIMITER + device_name);

// }

void SendMassage(String device_kind, String device_name, String value);
// {
//   Serial.println(device_kind + DELIMITER + device_name + DELIMITER + value);
// }


 #endif