
#ifndef POTENTIOMETERS_H
#define POTENTIOMETERS_H
#include "WString.h"
#include "SynthPotentiometer.h"
#include "UnityMsgProtocol.h"


#define NUM_POTENTIOMETERS 10

// #define PROTOCOL_MSG(name, value) (POT_CODE + SEPERATOR + name + SEPERATOR + value)

#define CUTOFF_PIN A1
#define ADSR_A_PIN A2 // START
#define ADSR_D_PIN A3 // DROP
#define ADSR_S_PIN A4 // HOLD
#define ADSR_R_PIN A5 // FADE
#define MASTER_VOL_PIN A6
#define OCTAVE_PIN A7 // SCALE
#define BPM_PIN A8
#define REVERB_PIN A9 // SPACE
#define DELAY_PIN A10 // ECHO


class Potentiometers {
public:
	SynthPotentiometer _pots[NUM_POTENTIOMETERS] = {
  SynthPotentiometer(CUTOFF_PIN, "CUTOFF"),
  SynthPotentiometer(ADSR_A_PIN, "ADSR.A"),
  SynthPotentiometer(ADSR_D_PIN, "ADSR.D"),
  SynthPotentiometer(ADSR_S_PIN, "ADSR.S"),
  SynthPotentiometer(ADSR_R_PIN, "ADSR.R"),
  SynthPotentiometer(MASTER_VOL_PIN, "VOLUME"),
  SynthPotentiometer(OCTAVE_PIN, "OCTAVE"),
  SynthPotentiometer(BPM_PIN, "BPM"),
  SynthPotentiometer(REVERB_PIN, "REVERB"),
  SynthPotentiometer(DELAY_PIN, "DELAY")
};

const String POT_CODE = "POT";
    
    // ctor
  // SynthPotentiometer();

    	// methods
  // String PrintStatus();
  void Update();





};



//POTENTIOMETER MAP (Analog inputs)

// CUTOFF  = A0
// Start = A1
// DROP = A2
// HOLD = A3
// FADE = A4
// MASTERVOL = A5
// SCALE = A6
// BPM = A7
// SPACE = A8
// ECHO = A9



#endif
