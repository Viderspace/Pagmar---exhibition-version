#ifndef TELEPHONETOGGLE_H
#define TELEPHONETOGGLE_H
#include "UnityMsgProtocol.h"
#include <Arduino.h>


#define PHONE_CODE "PHN"
#define PHONE_DOWN "DOWN"
#define PHONE_UP "UP"
#define PHONE_DIG_PIN 36

typedef enum{DOWN = 0, UP =1} State;


class TelephoneToggle{
  public:
String state_names[2] = {PHONE_DOWN,PHONE_UP};
State current_state;

TelephoneToggle();
void Setup();
void Update();
};


#endif