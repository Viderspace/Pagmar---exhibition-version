using System;
using System.Collections;
using System.Collections.Generic;
using Inputs;
using Runtime.Kernel.System;
using Scriptable_Objects;
using Synth_Variables.Scripts;
using Synth.Sequencer;
using Synth.Sequencer.MidiButtons;
using Timeline_Extentions;
using UnityEngine;
using UnityEngine.Playables;

public class SequencerCodeValidator : MonoBehaviour , INotificationReceiver
{
    public bool isActiveDebug = false;
    public int ScanNumber = 0;
    public bool passcodeIsAccurate = true;
    public static SequencerCodeValidator Instance;
    [SerializeField] private SequencerModeVariable SequencerMode;
    private List<Keypad> Code = new()
    {
        Keypad.Six, Keypad.Five, Keypad.Three, Keypad.Six,
        Keypad.Silence, Keypad.Six, Keypad.Seven, Keypad.Seven,
        Keypad.Silence, Keypad.Seven, Keypad.Silence, Keypad.Eight,
        Keypad.Silence, Keypad.Eight, Keypad.Zero, Keypad.Silence
    };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        
        if (Code.Count != 16)
        {
            Debug.LogError("Sequencer Code is not 16 digits long (SequencerCodeValidator script)");
            return;
        }
    }

    public void Enter()
    {
        SequencerMode.ValueChanged += StartListeningToCode;
        isActiveDebug = true;
        ScanNumber = 0;
        passcodeIsAccurate = true;
    }

    public void Exit()
    {
        SequencerMode.ValueChanged += StartListeningToCode;
        SequencerController.NoteTrigger -= ListenToSequencerCode;
        isActiveDebug = false;
        ScanNumber = 0;
        passcodeIsAccurate = false;
    }

    private void StartListeningToCode(SynthController.SequencerState sequencerState)
    {
        print("Code Validator received: " + sequencerState);
        if (sequencerState != SynthController.SequencerState.Running) return;

        // User has to press the Play button after inserting the code,
        // Starting to check if the code is correct
        SequencerController.NoteTrigger += ListenToSequencerCode;
    }

    private bool CheckCurrentNote(int index, Keypad keypad)
    {
        return Code[index] == keypad;
    }

    private bool ValidateCurrentStep(int timeStamp)
    {
        var buttons = SequencerButton.ButtonPool[timeStamp];

        foreach (var gridButton in buttons.Values) // Looking for active buttons
        {
            if (gridButton.IsOnVariable.Value) // THIS button is active
            {
                if (CheckCurrentNote(timeStamp, gridButton.MyKey)) // matches the code
                {
                    return true;
                }
            }
        }
        // No active buttons found

        if (Code[timeStamp] == Keypad.Silence) // Check if current code should be Silence Note
        {
            return true;
        }

        return false;
    }

    private void ListenToSequencerCode(int timeStamp)
    {

        if (!ValidateCurrentStep(timeStamp))
        {
            passcodeIsAccurate = false;
        }

        if (timeStamp == 15)
        {
            if (ScanNumber == 0)
            {
                StartCoroutine(passcodeIsAccurate ? nameof(CodeIsCorrect) : nameof(CodeIsIncorrect));
            }
            ScanNumber++;
        }
    }
    
  private  IEnumerator CodeIsIncorrect()
    {
        yield return new WaitWhile(() => ScanNumber < 3);
        // skipping to bad code response
        TimelineController.Instance.SkipToAndPlay(lastJumpMarker.destinationMarkerList[0]);
        // resetting the values
        Exit();
        SequencerMode.Value = SynthController.SequencerState.Recording;
        Enter();
        yield return null;
    }
  private IEnumerator CodeIsCorrect()
    {
        yield return new WaitWhile(() => ScanNumber < 3);
        // skipping to good code response
        TimelineController.Instance.SkipToAndPlay(lastJumpMarker.destinationMarkerList[1]);
        // exiting
        Exit();
        yield return null;
    }
  
  
  
  
  [SerializeField] public JumpMarker lastJumpMarker;
  
  
  public void OnNotify(Playable origin, INotification notification, object context)
  {
      var jumpMarker = notification as JumpMarker;
      if (jumpMarker == null) return;
            
      if (jumpMarker.initSequence)
      {
          // Save the last jump marker to late jump to the section based on the validation result     
          lastJumpMarker = jumpMarker;
          Enter();
      }

      else if (jumpMarker.numberOfDestinations == 1) // Jump to retry again
      {
          TimelineController.Instance.SkipToAndPlay(jumpMarker.destinationMarkerList[0]);
      }
      
  

  }

}

