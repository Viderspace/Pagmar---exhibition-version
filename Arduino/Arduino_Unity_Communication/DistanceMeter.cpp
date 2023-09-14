#include "DistanceMeter.h"

DistanceMeter::DistanceMeter(int thresholdCM, int trigpin, int echopin, int ledpin) {
  _thresholdCM = thresholdCM;
  _trigpin = trigpin;
  _echopin = echopin;
  _ledpin = ledpin;
  pinMode(_trigpin, OUTPUT);
  pinMode(_echopin, INPUT);
  if (ledpin != -1) pinMode(ledpin, OUTPUT);
  Serial.begin(9600);  // Starts the serial communication ???
  distance = GetDistance();
}


double DistanceMeter::GetDistance() {
  digitalWrite(_trigpin, LOW);
  delayMicroseconds(2);
  // Sets the trigPin on HIGH state for 10 micro seconds
  digitalWrite(_trigpin, HIGH);
  delayMicroseconds(10);
  digitalWrite(_trigpin, LOW);
  // Reads the echoPin, returns the sound wave travel time in microseconds
  duration = pulseIn(_echopin, HIGH);
  // Calculating the distance
  return duration * 0.034 / 2;
}



void DistanceMeter::Update() {
  distance = 0.995 * distance + 0.005 * GetDistance();

  // // Prints the distance on the Serial Monitor
  //   String msg = (String)(state == HIGH ? "NEAR" : "FAR") + " ";
  // Serial.print(msg);
  // Serial.print((int)distance);
  // Serial.println(" cm");


  int newRead = (distance < _thresholdCM) ? HIGH : LOW;
  if (newRead != state) {
    state = newRead;
    String msg = (String)(newRead == HIGH ? "NEAR" : "FAR") + " ";
    Serial.println("DIST," + msg);

    if (_ledpin != -1) digitalWrite(_ledpin, newRead);
  }
}