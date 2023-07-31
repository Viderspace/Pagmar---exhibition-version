using System.Collections;
using System.Collections.Generic;
using Inputs;
using Scriptable_Objects;
using Story.Terminal.ContentCreation.Terminal_Operations;
using Story.Terminal.System;
using UnityEngine;
using UnityEngine.UI;

namespace Story.Terminal.ContentCreation
{
    public class SerialValidationWindow : MonoBehaviour
    {
        #region Inspector
        [SerializeField] private TerminalScreen screen;
        [SerializeField] TerminalProgramRunner runner;

        #endregion

        #region Monobehaviour
        
        private void OnEnable()
        {
            InputManager.PhoneHangup += AbortAndReset;
        }

        private void OnDisable()
        {
            InputManager.PhoneHangup -= AbortAndReset;
        }


        private void AbortAndReset()
        {
            
            StartCoroutine(CloseWindow());
        }


        private void Awake()
        {
            screen.gameObject.SetActive(false);
            GetComponent<Image>().enabled = false;
        }
        
        #endregion
        
        #region Public Methods

        private int calls = 0;
        public void StartValidationRoutine(List<TerminalCommand> routine, IEnumerator resumeRoutine)
        {
            calls++;
            print("Starting validation routine "+calls+" times");

            runner.Queue.PushLast(OpenWindow());
            foreach (var command in routine)
            {
                runner.Queue.PushLast(command.Execute(runner, screen));
            }
            var pause = new PauseCommand(.5f).Execute(runner, screen);
            runner.Queue.PushLast(pause);
            runner.Queue.PushLast(CloseWindow());
            runner.Queue.PushLast(resumeRoutine);
        }

    
        
        #endregion

        #region Private Methods

        IEnumerator OpenWindow()
        {
            // Singleton.Instance.AudioFx.Play(AudioFx.FX.BackgroundProcess);
            screen.gameObject.SetActive(true);
            GetComponent<Image>().enabled = true;
            yield return null;
        }
    
        IEnumerator CloseWindow()
        {
            // Singleton.Instance.AudioFx.Play(AudioFx.FX.EventNotification);
            screen.gameObject.SetActive(false);
            GetComponent<Image>().enabled = false;
            screen.ClearEverything();
            yield return null;
        }
        

        #endregion
    }
}
