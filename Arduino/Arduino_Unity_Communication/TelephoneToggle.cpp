#include "TelephoneToggle.h"



TelephoneToggle::TelephoneToggle(){}


void TelephoneToggle::Setup(){
    pinMode(PHONE_DIG_PIN, INPUT);
}

void TelephoneToggle::Update(){
     int value_read = digitalRead(PHONE_DIG_PIN);
     if (value_read != current_state)
     {
       current_state = (State)value_read;
       SendMassage(PHONE_CODE, state_names[value_read]);
       delay(100);
     }
}