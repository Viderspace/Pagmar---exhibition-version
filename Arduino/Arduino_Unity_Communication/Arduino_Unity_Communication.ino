#include <Keypad.h>
#include "Potentiometers.h"
#include "WaveShapeSelector.h"
#include "TelephoneToggle.h"
#include "ButtonManager.h"
#include "UnityMsgProtocol.h"
#include "DistanceMeter.h"

// ======================= DEFINITIONS =================================== 
const String KEYPAD_CODE = "KEY";
const String KEY_DOWN = "DOWN";
const String KEY_UP = "UP";

// NOTICE: Delay time must match the delay time in unity!
const int delayUnity = 25;

// ======================= KEYPAD DEFINITIONS ===================================
const byte ROWS = 5;  // Number of rows in the keypad
const byte COLS = 3;  // Number of columns in the keypad
// Define the key map based on your wiring
char keys[ROWS][COLS] = {
  { '1', '2', '3' },
  { '4', '5', '6' },
  { '7', '8', '9' },
  { '*', '0', '#' },
  { 'R', 'R', 'R' }
};

// Define the pin connections
byte rowPins[ROWS] = { 48,47,  50, 49, 51 };  // Rows 1 to 5
byte colPins[COLS] = { 44, 45, 46 }; 


// ======================= CV PORTS SETUP ===================================


int cvSeq96_pin =  33;
int cvSeq97_pin = 34;
int cvSeq98_pin = 35;
int cvSeq96_state = LOW;
int cvSeq97_state = LOW;
int cvSeq98_state = LOW;



void CvSetup(){
  pinMode(cvSeq96_pin, INPUT);  // Set pin as input

}


void CvUpdate(){
  int cv96_read = digitalRead(cvSeq96_pin);


  if (cv96_read != cvSeq96_state){
    SendMassage("CV","96", (String)cv96_read);   
     cvSeq96_state = cv96_read;
  }

}


// ======================= DEVICE OBJECTS SETUP ===================================
Keypad keypad = Keypad(makeKeymap(keys), rowPins, colPins, ROWS, COLS);
WaveShapeSelector waveshape_selector = WaveShapeSelector();
Potentiometers potentiometers_manager = Potentiometers();
TelephoneToggle telephone_pickup_toggle = TelephoneToggle();
ButtonManager button_manager = ButtonManager();
DistanceMeter destanceMeter = DistanceMeter(100, 45,46,47); // Placeholder pin values



// ======================= Keypad Listen METHODS ===================================


String lastState = "IDLE";
char lastKey = 't';
bool isDirty = false;


void UpdateKey() {
    char key = keypad.getKey();  // Read the key pressed
    if (key) {
      lastKey = key;
      // isDirty = true;
    }
      if(isDirty){
        SendMassage(KEYPAD_CODE, (String)lastKey, lastState);
      isDirty = false;
    }
  
}


// Taking care of some special events.
void keypadEvent(KeypadEvent key){
    if (key == '\0') return;
    switch (keypad.getState()){
    case PRESSED:
  
        lastState = KEY_DOWN;
        isDirty = true;
        break;

    case RELEASED:
        lastState =  KEY_UP;
        isDirty = true;
        break;

  default:
        break;
    }
}



// ======================= ARDUINO MAIN METHODS ===================================

void global_update(){
          UpdateKey();
          waveshape_selector.Update();
          potentiometers_manager.Update();
          telephone_pickup_toggle.Update();
          button_manager.Update();
          CvUpdate();
          destanceMeter.Update();
}



void setup() {
  Serial.begin(115200);
  CvSetup();
  keypad.addEventListener(keypadEvent);
  waveshape_selector.Setup();
  telephone_pickup_toggle.Setup();
  button_manager.Setup();

}



void loop() {
  global_update();
  delay(delayUnity);
  }




