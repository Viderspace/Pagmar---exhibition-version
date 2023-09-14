#include "HardwareSerial.h"
#include "WString.h"
#include "Arduino.h"
#include "SynthPotentiometer.h"

SynthPotentiometer::SynthPotentiometer(int pin, String name){
   _pin = pin;
    _name = name;
    stable_value = 0;
    value_changed = false;
}



void SynthPotentiometer::Update()
{
  raw_value = CalculateValue(raw_value, analogRead(_pin));
  float new_stable_value = NORMALIZE(raw_value);

  if (IsChanged(stable_value, new_stable_value)){
    // value_changed = true;
    stable_value = new_stable_value;
    value_changed = true;
  }
}



String SynthPotentiometer::Print(){
  String msg = _name + ": " + (String)stable_value;
  return msg;
  }

  float SynthPotentiometer::Value(){
    return REVERSE_VAL(stable_value);
  }

bool SynthPotentiometer::IsChanged(float current_value, float new_value)
{
  return abs(new_value - current_value) > SENSITIVITY;
}

float SynthPotentiometer::CalculateValue(float current_value , float new_read){
  return SMOOTHNESS * current_value + SNAPPYNESS * new_read;
}
