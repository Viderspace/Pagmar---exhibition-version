#ifndef BUTTONMANAGER_H
#define BUTTONMANAGER_H
#include "ButtonDebounce.h"
#include "UnityMsgProtocol.h"


#define RESET_GRID_PIN 26//41 // 30
#define RECORD_SEQ_PIN  27 //32
#define PLAY_SEQ_PIN 28   //34
#define LEFT_SEQ_PIN 29  // 36
#define RIGHT_SEQ_PIN 30 //38
#define FILTER_ACTIVATE_PIN 31 //40


#define BUTTON_CODE "BTN"
#define RESET_GRID_MSG "RESET_GRID"
#define SEQUENCER_RECORD_MSG "SEQRECORD"
#define SEQUENCER_PLAY_PAUSE_MSG "SEQRUN"
#define SEQUENCER_MOVE_LEFT_MSG "SEQLEFT"
#define SEQUENCER_MOVE_RIGHT_MSG "SEQRIGHT"
#define FILTER_ACTIVATE_MSG "FILTER_TOGGLE"

class ButtonManager{

  public:
  ButtonManager();
  void Setup();
  void Update();

  private:
 ButtonDebounce  reset_grid_button = ButtonDebounce(26,RESET_GRID_MSG);
ButtonDebounce record_button = ButtonDebounce(27, SEQUENCER_RECORD_MSG);
 ButtonDebounce  play_button= ButtonDebounce(28, SEQUENCER_PLAY_PAUSE_MSG);
 ButtonDebounce  left_button= ButtonDebounce(29, SEQUENCER_MOVE_LEFT_MSG);
 ButtonDebounce  right_button= ButtonDebounce(30, SEQUENCER_MOVE_RIGHT_MSG);
ButtonDebounce filter_button= ButtonDebounce(31,FILTER_ACTIVATE_MSG);

};

  



#endif