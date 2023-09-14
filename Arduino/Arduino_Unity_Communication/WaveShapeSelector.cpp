#include "WaveShapeSelector.h"

WaveShapeSelector::WaveShapeSelector(){}

void WaveShapeSelector::Setup(){
  pinMode(SAWTOOTH_DIG_PIN, INPUT); // Set the digital input pin for switchPin1
  pinMode(TRIANGLE_DIG_PIN, INPUT); // Set the digital input pin for switchPin2
  pinMode(SINEWAVE_DIG_PIN, INPUT); // Set the digital input pin for switchPin3
  pinMode(SQUAREWAVE_DIG_PIN, INPUT); // Set the digital input pin for switchPin4
}


void WaveShapeSelector::Update(){
  Shape value_read = Read();
  if (value_read != selected_shape)
  {
    selected_shape = value_read;
    SendMassage(WAVESHAPE_MSG_CODE, shape_names[value_read]);
  }
}

Shape WaveShapeSelector::Read(){
  sawtooth_state = digitalRead(SAWTOOTH_DIG_PIN);
  triangle_state = digitalRead(TRIANGLE_DIG_PIN);
  sinewave_state = digitalRead(SINEWAVE_DIG_PIN);
  squarewave_state = digitalRead(SQUAREWAVE_DIG_PIN);

  int sumDebug = 0;
  Shape value_read = NONE;

  if (sawtooth_state){
    value_read = SAWTOOTH;
    sumDebug++;
  }
  if(triangle_state){
      value_read = TRIANGLE;
      sumDebug++;
  }
  if (sinewave_state){
    value_read = SINE;
    sumDebug++;
  }
  if (squarewave_state){
    value_read = SQUARE;
    sumDebug++;
  }
  // if (sumDebug > 1) {Serial.println("BUG @ WaveShapeSelector.Read(): more than 1 shapes are selected");}
  
  return value_read;;
}