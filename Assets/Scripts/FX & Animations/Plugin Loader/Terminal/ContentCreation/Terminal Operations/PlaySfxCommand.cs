using System.Collections;
using Scriptable_Objects;
using Story.Terminal.System;
using NotImplementedException = System.NotImplementedException;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    public class PlaySfxCommand : TerminalCommand
    {
        AudioFx.FX _sound;

        public PlaySfxCommand(AudioFx.FX sound)
        {
            _sound = sound;
        }


        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            if (Singleton.Instance != null)
            {
                Singleton.Instance.AudioFx.Play(_sound);
            }
            yield return null;
        }
    }
}