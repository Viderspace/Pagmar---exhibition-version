#include "WString.h"
#ifndef WAVESHAPESELECTOR_H
#define WAVESHAPESELECTOR_H
#include <Arduino.h>
#include "UnityMsgProtocol.h"



#define  SQUAREWAVE_DIG_PIN 25 // Digital input pin for the first position of the rotary switch
#define  SAWTOOTH_DIG_PIN 22 // Digital input pin for the second position of the rotary switch
#define   TRIANGLE_DIG_PIN 23 // Digital input pin for the third position of the rotary switch
#define  SINEWAVE_DIG_PIN 24 // Digital input pin for the fourth position of the rotary switch
#define WAVESHAPE_MSG_CODE "WAV"

typedef enum{ SAWTOOTH,TRIANGLE,SINE, SQUARE, NONE} Shape;


class WaveShapeSelector{
  private:
  // Sawtooth
int sawtooth_state =0;
// Triangle
int triangle_state =0;
// SineWave;
int sinewave_state =0;
// Squarewave
int squarewave_state = 0;

Shape selected_shape = NONE;

public:

WaveShapeSelector();
void Update();
Shape Read();
void Setup();
String shape_names[5] = {"SAWTOOTH","TRIANGLE","SINE", "SQUARE", "NONE"};

};


#endif