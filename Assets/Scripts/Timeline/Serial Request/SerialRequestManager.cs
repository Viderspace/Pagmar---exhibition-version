using System;
using ASCII_Animations;
using Inputs;
using Runtime.Kernel.System;
using Timeline_Extentions;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Timeline.Serial_Input_Routine
{
    public class SerialRequestManager : MonoBehaviour, INotificationReceiver
    {
        #region Inspector
        [SerializeField] public JumpMarker lastJumpMarker;
        [SerializeField] public SerialConsolePrompt consolePrompt;
        [SerializeField] public BlueBoxProgramLoader blueBoxProgramLoader;
        [SerializeField] public AsciiProgressBar asciiAsciiProgressBar;

        private bool _lastAttemptWasCorrect;

        #endregion
        
        public void GetNewMarker(JumpMarker marker)
        {
            lastJumpMarker = marker;
        }

        #region INotificationReceiver
        
        int calls = 0;

        public void OnNotify(Playable origin, INotification notification, object context)
        {
            var jumpMarker = notification as JumpMarker;
            if (jumpMarker == null) return;
            
            if (jumpMarker.initSequence)
            {
                EnterSequence();
            }
            
            lastJumpMarker = jumpMarker;

            switch (jumpMarker.typeOfEvent)
            {
                case JumpMarker.SerialIdPauseEvent.HoldAndWaitForInput:
                    PauseAndWaitForInput();
                    return;

                case JumpMarker.SerialIdPauseEvent.PlayValidationProgram:
                    calls++;
                    print("---------JumpMarker.PlayValidationProgram called Validation "+calls+" times at "+jumpMarker.time+" -----------");
                    PlayValidationProgram();
                    return;
                default:
                    throw new ArgumentOutOfRangeException();
            }

        }

        #endregion

        #region Monobehaviour

        private void Start()
        {
            consolePrompt.ShowPrompt(false);
        }

        private void OnEnable()
        {
            InputManager.PhoneHangup += AbortAndReset;
        }

        private void OnDisable()
        {
            InputManager.PhoneHangup -= AbortAndReset;
        }

        #endregion

        #region Methods
        
        private void AbortAndReset()
        {
            consolePrompt.ShowPrompt(false);
            consolePrompt.Clear();
        }

        public void EnterSequence()
        {
            consolePrompt.UserEnteredAllDigits = OnUserDoneEnteringDigits;
            consolePrompt.gameObject.SetActive(true);
            consolePrompt.Clear(); 
        }
        
        private void ExitSequence()
        {
            consolePrompt.UserEnteredAllDigits = null;
            TimelineController.Instance.SkipToAndPlay(lastJumpMarker.SuccessRoutine);
            consolePrompt.ShowPrompt(false);
            consolePrompt.Clear();
        }


        public void PauseAndWaitForInput()
        {
            TimelineController.Instance.PauseTimeline();
            consolePrompt.Clear();
            consolePrompt.ShowPrompt();
        }
        
        private void OnUserDoneEnteringDigits(bool serialIsCorrect)
        {
            _lastAttemptWasCorrect = serialIsCorrect;
            consolePrompt.Clear();
            consolePrompt.ShowPrompt(false);
            TimelineController.Instance.SkipToAndPlay(lastJumpMarker.NextResponse);
        }
        
        public void ShowProgressBar(bool show=true)
        {
            if (show) asciiAsciiProgressBar.StartProgressBar(15f);
            else asciiAsciiProgressBar.StopAndHideProgressBar();
        }

        public void PlayValidationProgram()
        {
            blueBoxProgramLoader.onValidationFinished = _lastAttemptWasCorrect ? ExitSequence : JumpToFailureResponse;
            blueBoxProgramLoader.RunValidation(_lastAttemptWasCorrect);
        }

        private void JumpToFailureResponse()
        {
            asciiAsciiProgressBar.StopAndHideProgressBar();
            TimelineController.Instance.SkipToAndPlay(lastJumpMarker.FailureRoutine);
        }

        #endregion
    }
}