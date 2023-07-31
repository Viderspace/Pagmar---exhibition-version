using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using Story.Terminal.ContentCreation.Terminal_Operations;
using Unity.Collections;
using UnityEngine;

namespace Story.Terminal.ContentCreation
{
    [CreateAssetMenu(fileName = "Terminal Validation Routine", menuName = "Terminal Validation Routine")]
    public class InputValidationRoutine : TerminalProgram
    {
        [SerializeReference] public TerminalProgram validationRoutine;
        [SerializeReference] public TerminalProgram invalidInputResponse;
        [SerializeReference] public TerminalProgram validInputResponse;
        
        
        public List<TerminalCommand> GetSuccessRoutine()
        {
            List<TerminalCommand> routine = new List<TerminalCommand>();
            routine.Add(new PlaySfxCommand(AudioFx.FX.BackgroundProcess));
            routine.AddRange(validationRoutine.programCommands);
            routine.Add(new PlaySfxCommand(AudioFx.FX.EventNotification));
            routine.AddRange(validInputResponse.programCommands);
            return routine;            
        }
        public List<TerminalCommand> GetFailureRoutine()
        {
            List<TerminalCommand> routine = new List<TerminalCommand>();
            routine.Add(new PlaySfxCommand(AudioFx.FX.BackgroundProcess));
            routine.AddRange(validationRoutine.programCommands);
            routine.Add(new PlaySfxCommand(AudioFx.FX.EventNotification));
            routine.AddRange(invalidInputResponse.programCommands);
            return routine;            
        }
    }
}