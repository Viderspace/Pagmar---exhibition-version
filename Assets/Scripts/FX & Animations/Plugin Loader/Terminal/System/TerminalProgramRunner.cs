using System.Collections;
using System.Collections.Generic;
using Inputs;
using Story.Terminal.ContentCreation;
using Story.Terminal.ContentCreation.Terminal_Operations;
using UnityEngine;
using NotImplementedException = System.NotImplementedException;

namespace Story.Terminal.System
{
    public class TerminalProgramRunner : MonoBehaviour
    {
        public Task CurrentTask;
        [SerializeField] private TerminalScreen _screen;
        [SerializeField] private TerminalWindow terminalWindow;
        public bool IsRunning => CurrentTask != null && CurrentTask.Running;
        public bool HasMoreCommands => !Queue.IsEmpty || IsRunning;
        public TerminalScreen MainScreen => _screen;


        public int MaxCharsPerLine => _screen.maxCharsPerLine;


        public readonly ProgramCommandQueue Queue = new();

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
            if (CurrentTask!=null) CurrentTask.Stop();
            Queue.Reset();
            _screen.ClearEverything();
        }


        private void Update()
        {
            if (IsRunning) return;

            if (HasMoreCommands) MoveToNextCommand();
        }


        public void MoveToNextCommand()
        {
            if (CurrentTask != null && CurrentTask.Running)
            {
                CurrentTask.Stop();
            }

            CurrentTask = new Task(Queue.PopFirst());
        }

        /**
         * Main method for executing a program
         */
        public void LoadProgram(TerminalProgram newProgram)
        {
            foreach (var procedure in newProgram.programCommands)
            {
                Queue.PushLast(procedure.Execute(this, _screen));
            }
        }

        public void RunSerialValidation(List<TerminalCommand> validationSequence, IEnumerator resumeRoutine)
        {
            terminalWindow.RunSerialValidationProgram(validationSequence, resumeRoutine);
        }


        #region ITerminalOperations

        public void DeleteCharCommand()
        {
            _screen.DeleteCharCommand();
        }


        public void ClearEverything()
        {
            if (CurrentTask != null) CurrentTask.Stop();

            // ProgramState.SetIdle();
            Queue.Reset();
            _screen.ClearEverything();
        }

        public ScreenBuffer CreateNewLine()
        {
            return _screen.CreateNewLine();
        }

        public ScreenBuffer GetLastLine()
        {
            return _screen.GetLastLine();
        }

        public void TypeUserPassword(char letter)
        {
            _screen.TypeUserPassword(letter);
        }

        #endregion

        public void AddInstruction(TerminalCommand command)
        {
            Queue.PushLast(command.Execute(this, _screen));
        }
    }
}