#include "Arduino.h"
#include "ButtonManager.h"


ButtonManager::ButtonManager(){
//      reset_grid_button = ButtonDebounce(26,RESET_GRID_MSG);
//  record_button = ButtonDebounce(27, SEQUENCER_RECORD_MSG);
//    play_button= ButtonDebounce(28, SEQUENCER_PLAY_PAUSE_MSG);
//    left_button= ButtonDebounce(29, SEQUENCER_MOVE_LEFT_MSG);
//    right_button= ButtonDebounce(30, SEQUENCER_MOVE_RIGHT_MSG);
//  filter_button= ButtonDebounce(31,FILTER_ACTIVATE_MSG);
}


void ButtonManager::Update(){
  reset_grid_button.Update();
  record_button.Update();
  play_button.Update();
  left_button.Update();
  right_button.Update();
  filter_button.Update();
}

