#include "WString.h"
#include <Arduino.h>
#ifndef SYNTHPOTENTIOMETER_H
#define SYNTHPOTENTIOMETER_H

#define SENSITIVITY 0.01
#define READ_SIZE 1023
#define SMOOTHNESS 0.6 // Response Behaviour, smooth or snappy?
#define SNAPPYNESS (1 - SMOOTHNESS)
#define NORMALIZE(raw_val) (raw_val/READ_SIZE)
#define REVERSE_VAL(val) (1-val)


class SynthPotentiometer {
public:
	// members
    bool value_changed;
    String  _name;



    // ctor
  SynthPotentiometer(int pin, String name);

    	// methods
  String Print();
  float Value();
   void Update();




private:
   static float CalculateValue(float current_value , float new_read);
   bool IsChanged(float current_value, float new_value);


   int _pin;
  float stable_value;
  float raw_value;



};
#endif