using System.Collections;
using Inputs;
// using RtMidi.LowLevel;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using Synth.Sequencer;
using UnityEngine;

namespace Reaktor_Communication.Note_Players
{
    public class MusicalMidiPlayer : BasePlayer
    {
        #region Midi Sending Logic
        public MidiChannel channel = MidiChannel.Ch1;
        public float velocity = 0.9f;

        private Task _sequenceTask;
        

        #endregion
        private SequencerController Sequencer => SequencerController.Instance;
        private bool SequencerEnabled => Sequencer != null;// && Sequencer.IsRunning;
        private SynthController SynthController => Singleton.Instance.SynthController;

        public ToggleVariable NoteOnVariable;
        
        void Start()
        {
            MidiBridge.instance.Warmup();
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

        public override void PlayNoteAtLength(Keypad key, float length)
        {
            // todo: Check if the base method is calling the overridden methods or not
            if (!SequencerEnabled) return;
            _sequenceTask = new Task(PlayNoteAtLengthRoutine(key, length));
        }

        public override void OnKeypadDown(Keypad key)
        {
            base.OnKeypadDown(key);
            NoteOnVariable.Value = true;
            var midiValue = SynthController.GetNoteInCurrentOctave(key);
            MidiOut.SendNoteOn (channel, midiValue, velocity);
        }

        public override void OnKeypadUp(Keypad key)
        {
            base.OnKeypadUp(key);
            NoteOnVariable.Value = false;
            var midiValue =  SynthController.GetNoteInCurrentOctave(key);
            MidiOut.SendNoteOff (channel, midiValue);

        }

        // Ignoring unrelated events
        public override void PlayLineTone(bool on)
        {}

        public override  void PlayHarshTone(bool on)
        {}
        
    }
}