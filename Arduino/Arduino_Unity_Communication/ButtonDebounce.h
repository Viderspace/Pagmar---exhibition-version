
#ifndef ButtonDebounce_h
#define ButtonDebounce_h
#include "WString.h"
#include <Arduino.h>


class ButtonDebounce{
  private:
  int pinId;
  String _name;


  // Variables will change:
  int buttonState;            // the current reading from the input pin
  int lastButtonState = LOW;  // the previous reading from the input pin

// the following variables are unsigned longs because the time, measured in
// milliseconds, will quickly become a bigger number than can be stored in an int.
unsigned long lastDebounceTime = 0;  // the last time the output pin was toggled
unsigned long debounceDelay = 50; 

public:
ButtonDebounce(int pin, String name);
void Update();
};


#endif