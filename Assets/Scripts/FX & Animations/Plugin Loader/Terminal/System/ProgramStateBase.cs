// using System.Collections;
// using System.Collections.Generic;
// using Story.Terminal.ContentCreation;
// using Story.Terminal.ContentCreation.Terminal_Operations;
// using UnityEngine;
//
// namespace Story.Terminal.System
// {
//     public class ProgramState
//     {
//         public  TerminalProgramRunner Runner;
//         public  RunningState RunningState;
//         public  IdleState IdleState;
//         public ListeningState ListeningState;
//         public ProgramStateBase CurrentState;
//         public ProgramState(TerminalProgramRunner runner)
//         {
//             Runner = runner;
//             RunningState = new RunningState(this);
//             IdleState = new IdleState(this);
//             ListeningState = null;
//             CurrentState = IdleState;
//         }
//
//         public void Tick(string userInput = "")
//         {
//             CurrentState.Tick(userInput);
//         }
//         
//         public void SetListeningState(string expectedInput, InputValidationRoutine validationRoutine, UserInputCommand inputCommand)
//         {
//             ListeningState = new ListeningState(expectedInput, validationRoutine, inputCommand, this);
//             CurrentState = ListeningState;
//         }
//         
//         public  void SetIdle()
//         {
//             CurrentState = IdleState;
//         }
//         
//         public  void SetRunning()
//         {
//             CurrentState = RunningState;
//         }
//
//         public bool Validating;
//                 
//         public IEnumerator Resume()
//         {
//             Validating = false;
//             yield return null;
//         }
//         
//         
//     }
//     
//     
//     
//     public abstract class ProgramStateBase
//     {
//         public abstract void Tick(string userInput = "");
//     }
//
//     public class RunningState : ProgramStateBase
//     {
//         ProgramState Context;
//         private TerminalProgramRunner Runner => Context.Runner;
//         public RunningState(ProgramState context)
//         {
//             Context = context;
//         }
//         public override void Tick(string userInput = "")
//         {
//             if (Runner.IsRunning) return;
//
//             if (Runner.HasMoreCommands) Runner.MoveToNextCommand();
//
//             else Context.SetIdle();
//         }
//
//     }
//
//     public class ListeningState : ProgramStateBase
//     {
//         ProgramState Context;
//         private TerminalProgramRunner Runner => Context.Runner;
//
//         public string CurrentInput { get; set; }
//         public string ExpectedInput { get; set; }
//         public InputValidationRoutine ValidationRoutine { get; set; }
//         public UserInputCommand InputCommand { get; set; }
//
//         public ListeningState(string expectedInput, InputValidationRoutine validationRoutine, UserInputCommand inputCommand, ProgramState context)
//         {
//             Context = context;
//             ExpectedInput = expectedInput;
//             CurrentInput = string.Empty;
//             ValidationRoutine = validationRoutine;
//             InputCommand = inputCommand;
//         }
//
//
//         private void ListenToUserInput(string input)
//         {
//             foreach (var c in input)
//             {
//                 if (c == '\b' && CurrentInput.Length > 0)
//                 {
//                     Runner.DeleteCharCommand();
//                     CurrentInput = CurrentInput.Substring(0, CurrentInput.Length - 1);
//                 }
//                 else
//                 {
//                     Runner.TypeUserPassword(c);
//                 }
//             }
//
//             CurrentInput += input;
//         }
//         // public IEnumerator TurnOnValidationWindow(bool value)
//         // {
//         //     Context.ValidationScreen.gameObject.SetActive(value);
//         //     yield return null;
//         // }
//  
//
//         private void EvaluateInput()
//         {
//             if (CurrentInput.Length < ExpectedInput.Length) return;
//             
//             
//             // Success / Failure Routine
//             
//             List<TerminalCommand> routine ;
//             if (CurrentInput.Equals(ExpectedInput))
//             {
//                 routine = ValidationRoutine.GetSuccessRoutine();
//             }
//             else
//             {
//                 routine = ValidationRoutine.GetFailureRoutine();
//                 Runner.Queue.PushFirst(InputCommand.Execute(Runner, Runner.MainScreen));
//             }
//   
//             // Pushing to Queue the Validation Routine
//             PushValidationQueue(routine);
//        
//         }
//
//         private void PushValidationQueue(List<TerminalCommand> routine)
//         {
//             Context.Validating = true;
//             Context.SetIdle();
//             Runner.RunSerialValidation(routine, Context.Resume());
//             // Context.Queue.PushFirst(TurnOnValidationWindow(false));
//             // for(int i = routine.Count-1 ; i >= 0 ; i--)
//             // {
//             //     Runner.Queue.PushFirst(routine[i].Execute(Runner, Runner.MainScreen));
//             // }
//             // Context.Queue.PushFirst(TurnOnValidationWindow(true));
//         }
//
//         public override void Tick(string userInput = "")
//         {
//             if (string.IsNullOrEmpty(userInput)) return;
//             ListenToUserInput(userInput);
//             EvaluateInput();
//         }
//     }
//
//     public class IdleState : ProgramStateBase
//     {
//         ProgramState Context;
//         private TerminalProgramRunner Runner => Context.Runner;
//
//         public IdleState(ProgramState context)
//         {
//             Context = context;
//         }
//         public override void Tick(string userInput = "")
//         {
//             if (Runner.HasMoreCommands && !Context.Validating) Context.SetRunning();
//         }
//     }
// }