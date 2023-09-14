#include <sys/stat.h>
#include "HardwareSerial.h"
#include "WString.h"
#include "UnityMsgProtocol.h"
#include <Arduino.h>

#ifndef UNITYLISTENER_H
#define UNITYLISTENER_H

#define MAX_LED_PINS 20
#define REGISTER_PIN 0
#define SET_PIN 1
#define PRINT_LAST_MSG 2
#define TOGGLE_VALUE(toggle)  ((toggle) == (0) ? (LOW) : (HIGH))


class UnityListener {
  public:
    UnityListener();
    void Update();
    void PrintLastMsg();
    void ExecuteCommand(int cmd, int pin, int toggle);
    void PrintLastMsg(int cmd, int pin, int toggle);
    String inputString;         // a string to hold incoming data
    bool stringComplete; 
    int last_cmd;
    int last_pin;
    int last_toggle;   

    void parseData(String data);
  private:
    bool ledStatus[MAX_LED_PINS];

   

};

#endif