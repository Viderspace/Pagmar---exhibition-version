using System;
using System.Collections;
using Story.Terminal.System;
using UnityEngine;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    public class ClearScreenCommand : TerminalCommand
    {
        [Header("Clear Screen")]
        public float pauseBeforeClearingTheScreen = 2f;
        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            yield return PauseDuration(pauseBeforeClearingTheScreen);
            screen.ClearEverything();
            yield return null;
        }
    }
}