#ifndef DISTANCEMETER_H
#define DISTANCEMETER_H

#include <Arduino.h>


class DistanceMeter {
private:
  int _trigpin;
  int _echopin;
  int _ledpin;
  int _thresholdCM;
  long duration;



  public:
  DistanceMeter(int thresholdCM, int trigpin, int echopin, int ledpin = -1);
  void Update();
  double GetDistance();
  double distance;
  int state;

};






#endif