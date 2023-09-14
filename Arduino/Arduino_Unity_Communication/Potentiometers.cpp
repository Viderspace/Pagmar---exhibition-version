#include "WString.h"
#include "HardwareSerial.h"
#include "Potentiometers.h"




void Potentiometers::Update(){

  for(int i=0; i< NUM_POTENTIOMETERS; i++){
    SynthPotentiometer * pot = &_pots[i];

    pot->Update();

    if (pot->value_changed)
    {
          pot->value_changed = false;
          SendMassage(POT_CODE, pot->_name,  (String)(pot->Value()));
          //Serial.println(ProtocolMsg(pot->_name, pot->Value()));

    }
  }
}