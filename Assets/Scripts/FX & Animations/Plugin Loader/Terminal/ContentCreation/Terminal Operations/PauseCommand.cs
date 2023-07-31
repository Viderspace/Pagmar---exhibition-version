using System;
using System.Collections;
using Story.Terminal.System;
using UnityEngine;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    public class PauseCommand : TerminalCommand
    {
        [SerializeField] private float pauseTime;
        
        public PauseCommand(float time=0.5f)
        {
            pauseTime = time;
        }
        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            yield return PauseDuration(pauseTime);
        }
    }
}