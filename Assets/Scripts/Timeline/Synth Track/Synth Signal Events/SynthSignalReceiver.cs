using System;
using Inputs;
using Scriptable_Objects;
using Timeline.Synth_Track.Synth_Signal_Events.Signals;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Timeline.Synth_Signals
{
    /// <summary>
    /// Notifies the SynthData from the timeline to change the pitch mode.
    /// The massage will then activate the note player and any other objects that need to know about the pitch mode.
    /// </summary>
    public class SynthSignalReceiver: MonoBehaviour
    {
        [SerializeField] private SynthController synthController;

        

        #region Event Methods
        private void ResetSynthToInit()
        {
            if (Singleton.Instance == null) return;
            Singleton.Instance.SynthController.ResetToInit();
        }

 
        public void SetMusicalPitchMode()
        {
            synthController.UpdatePitchMode(SynthController.PitchMode.MusicalNotes);
        }

        public void ActivateOscillatorModule()
        {
            synthController.OscillatorOnOffSwitch.Value = true;
        }
        
        public void ActivateSequencerModule()
        {
            synthController.SequencerOnOffSwitch.Value = true;
        }
        
        
        
        
        
        
        #endregion

        
    }
}