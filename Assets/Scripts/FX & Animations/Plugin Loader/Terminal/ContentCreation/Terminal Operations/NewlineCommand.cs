using System;
using System.Collections;
using Story.Terminal.System;
using UnityEngine;
namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    public class NewlineCommand : TerminalCommand
    {
        [Header("NewLine (aka Enter)")] public int numberOfLines = 1;
        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            for (int i = 0; i < numberOfLines; i++)
            {
                yield return new SimpleText("", TypeSpeed.Normal, true).Execute(terminal, terminal.MainScreen);
            }
        }
    }
}