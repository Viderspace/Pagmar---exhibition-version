using System.Collections;
using Inputs;
using JetBrains.Annotations;
using UnityEngine;

namespace Reaktor_Communication.Note_Players
{
    public abstract class BasePlayer : MonoBehaviour
    {
        private Task _sequenceTask;
        [CanBeNull] protected Keypad ActiveKey { get; set; } = null;
        protected bool lineToneIsPlaying = false;
        protected bool harshToneIsPlaying = false;
        public bool LineToneIsPlaying => lineToneIsPlaying;
        public bool HarshToneIsPlaying => harshToneIsPlaying;
        public bool IsPlaying => ActiveKey != null || lineToneIsPlaying || harshToneIsPlaying;

        // Playing a continuous note sound until the key is manually released

        #region API

        public virtual void OnKeypadDown(Keypad key)
        {
            ActiveKey = key;
        }

        // release the current note from playing
        public virtual void OnKeypadUp(Keypad key)
        {
            ActiveKey = null;
        }

        // Play the note from the sequencer in pre-determent length
        public virtual void PlayNoteAtLength(Keypad key, float length)
        {
            _sequenceTask = new Task(PlayNoteAtLengthRoutine(key, length));
        }

        // public virtual void Mute()
        // {
        //     if (ActiveKey != null)
        //     {
        //         OnKeypadUp(ActiveKey);
        //         _sequenceTask?.Stop();
        //     }
        //     if(lineToneIsPlaying) PlayLineTone(false);
        //     if(harshToneIsPlaying) PlayHarshTone(false);
        // }

        public virtual void Mute()
        {
            if (ActiveKey != null)
            {
                OnKeypadUp(ActiveKey);
                _sequenceTask?.Stop();
            }
            PlayLineTone(false);
            PlayHarshTone(false);
        }


        private IEnumerator PlayNoteAtLengthRoutine(Keypad key, float length)
        {
            OnKeypadDown(key);
            var noteDuration = length;
            while (noteDuration > 0)
            {
                noteDuration -= Time.deltaTime;
                yield return null;
            }

            OnKeypadUp(key);
            yield return null;
        }

        public virtual void PlayLineTone(bool on)
        {
            lineToneIsPlaying = on;
        }

        public virtual void PlayHarshTone(bool on)
        {
            harshToneIsPlaying = on;
        }

        #endregion
    }
}