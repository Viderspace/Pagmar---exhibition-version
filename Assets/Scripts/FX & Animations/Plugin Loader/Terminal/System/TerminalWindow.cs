using System;
using System.Collections;
using System.Collections.Generic;
using Inputs;
using Story.Terminal.ContentCreation;
using Story.Terminal.ContentCreation.Terminal_Operations;
using TMPro;
using UnityEngine;

namespace Story.Terminal.System
{
    public class TerminalWindow : MonoBehaviour
    {
        #region Inspector
       [SerializeField] private TerminalScreen screen;
        [SerializeField] TerminalProgramRunner mainProgramRunner;
        public SerialValidationWindow serialValidationWindow;

        #endregion


        private void OnEnable()
        {
            InputManager.PhoneHangup += ClearScreen;
        }

        private void OnDisable()
        {
            InputManager.PhoneHangup -= ClearScreen;
        }

        #region Public Methods

        
        public void AddInstruction(TerminalCommand command)
        {
            mainProgramRunner.AddInstruction(command);
        }


        public void ClearScreen()
        {
            mainProgramRunner.ClearEverything();
        }
        
        #endregion

        
        public void RunSerialValidationProgram(List<TerminalCommand> validationSequence, IEnumerator resumeRoutine)
        {
            serialValidationWindow.StartValidationRoutine(validationSequence,resumeRoutine);
        }
    }
}