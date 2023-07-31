using System;
using System.Collections.Generic;
using Story.Terminal.ContentCreation.Terminal_Operations;
using UnityEngine;
using Utils;
using static Utils.DesignPalette;

namespace Story.Terminal.ContentCreation
{
    
    [CreateAssetMenu(fileName = "Terminal Program", menuName = "Terminal")]
    
    public class TerminalProgram : ScriptableObject
    {
        [SerializeReference]
        public List<TerminalCommand>  programCommands;
 

        public void AddStylizedText()
        {
            programCommands.Add(new StylizedText());
        }
        public void AddSimpleText()
        {
            programCommands.Add(new SimpleText());
        }
        
        public void AddPause()
        {
            programCommands.Add(new PauseCommand());
        }
        public void AddNewline()
        {
            programCommands.Add(new NewlineCommand());
        }
        public void AddInputRoutine()
        {
            programCommands.Add(new UserInputCommand());
        }

        public void AddTextFile()
        {
            programCommands.Add(new ReadTextFromFile());
        }

        public void AddMorpheusText()
        {
            programCommands.Add(new MorpheusText());
        }
        public void AddClearScreen()
        {
            programCommands.Add(new ClearScreenCommand());
        }
    }


}