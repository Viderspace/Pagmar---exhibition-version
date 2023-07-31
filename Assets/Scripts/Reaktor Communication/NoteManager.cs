using System.Collections.Generic;
using Inputs;
using Reaktor_Communication.Note_Players;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using UnityEngine;

namespace Reaktor_Communication
{
    public class NoteManager : BasePlayer
    {
        [SerializeField] private PitchModeVariable globalPitchMode;
        [SerializeField] private TelephonePlayer telephonePlayer;
        [SerializeField] private MusicalMidiPlayer musicalMidiPlayer;
        [SerializeField] private ToggleVariable NoteOnVariable;
        private BasePlayer _currentPlayState;
        


  
        private void Start()
        {
            telephonePlayer = gameObject.GetComponent<TelephonePlayer>();
            musicalMidiPlayer = gameObject.GetComponent<MusicalMidiPlayer>();
            

            _currentPlayState = (globalPitchMode.DefaultValue == SynthController.PitchMode.Telephone)
                ? telephonePlayer
                : musicalMidiPlayer;
        }

        private void OnEnable()
        {
            globalPitchMode.ValueChanged += SetPitchMode;
            InputManager.KeypadButtonPressed += OnKeypadDown;
            InputManager.KeypadButtonReleased += OnKeypadUp;
        }

        private void OnDisable()
        {
            globalPitchMode.ValueChanged -= SetPitchMode;
            InputManager.KeypadButtonPressed -= OnKeypadDown;
            InputManager.KeypadButtonReleased -= OnKeypadUp;
        }

        public void SetPitchModeToMusical(bool on = false)
        {
            SetPitchMode(SynthController.PitchMode.MusicalNotes);
        }

        public void SetPitchMode(SynthController.PitchMode type)
        {
            SetPitchMode(type == SynthController.PitchMode.Telephone ? telephonePlayer : musicalMidiPlayer);
        }

        public void SetPitchMode(BasePlayer playerType)
        {
            if (!_currentPlayState) return;
            _currentPlayState.Mute();
            _currentPlayState = playerType;
        }

        public override void Mute()
        {
            _currentPlayState.Mute();
        }

        public override void OnKeypadUp(Keypad key)
        {
            print("Key up "+ key.Name);
            NoteOnVariable.Value = false;
            _currentPlayState.OnKeypadUp(key);
        }

        public override void OnKeypadDown(Keypad key)
        {
            print("Key down"+ key.Name);
            NoteOnVariable.Value = true;
            _currentPlayState.OnKeypadDown(key);
        }

        public override void PlayLineTone(bool on)
        {
            _currentPlayState.PlayLineTone(on);
        }

        public override void PlayHarshTone(bool on)
        {
            _currentPlayState.PlayHarshTone(on);
        }

        public override void PlayNoteAtLength(Keypad key, float length)
        {
            _currentPlayState.PlayNoteAtLength(key, length);
        }

        private void OnApplicationQuit()
        {
            _currentPlayState.Mute();
        }
    }
}