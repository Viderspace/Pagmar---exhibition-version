
#include "UnityListener.h"

UnityListener::UnityListener() {
  // initialize the serial communication:
  // Serial.begin(9600);
  
  
  // reserve 200 bytes for the inputString:
  inputString.reserve(200);


  stringComplete = false;
}

void UnityListener::Update() {
  // check if data has been sent from the computer:
  if (stringComplete) {
    parseData(inputString);
    inputString = "";
    stringComplete = false;
  }
}

void UnityListener::PrintLastMsg(int cmd, int pin, int toggle){
    String lastCmd = cmd == REGISTER_PIN ? "Register" : (cmd == SET_PIN? "Toggle" : "Print");
    String lastPin = "Pin #"+ (String)pin;
    String lastToggle = (toggle == 1 ? "ON" : "OFF");
    DebugMassage("Last Msg: " + lastCmd + " " + lastPin + " " + lastToggle);
//     Serial.println("Last Msg: " + lastCmd + " " + lastPin + " " + lastToggle);
}

void UnityListener::ExecuteCommand(int cmd, int pin, int toggle){
      PrintLastMsg(cmd, pin, toggle);


    if (cmd == REGISTER_PIN) { // REGISTER_CMD
    pinMode(pin, OUTPUT);
    digitalWrite(pin, LOW);
  } 
  else if (cmd == SET_PIN) { // TOGGLE_CMD
    int ledValue = TOGGLE_VALUE(toggle);
    digitalWrite(pin, ledValue);
  }
  else if(cmd == PRINT_LAST_MSG){
    PrintLastMsg(last_cmd, last_pin, last_toggle);
  }

  last_cmd = cmd;
  last_pin = pin;
  last_toggle = toggle;
}



void UnityListener::parseData(String data) {
  while (data != "") {
    int commandEnd = data.indexOf('\n');
    String msg = data.substring(0, commandEnd);
    data = data.substring(commandEnd+1);

  int commaIndex1 = msg.indexOf(',');
  int commaIndex2 = msg.lastIndexOf(',');
  
  int commandType = msg.substring(0, commaIndex1).toInt();
  int ledPin = msg.substring(commaIndex1+1, commaIndex2).toInt();
  int turnOn = msg.substring(commaIndex2+1).toInt();
  ExecuteCommand(commandType, ledPin, turnOn);

  if (data.length() < 3){
    data ="";
  }
  }
  




}


//============


