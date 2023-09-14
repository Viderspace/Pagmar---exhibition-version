using System;
using Inputs;
using Synth_Variables;
using Synth_Variables.Adsr;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Synth Controller", menuName = "ScriptableObjects", order = 0)]
    public class SynthController : ScriptableObject
    {
        #region Inspector

        /**
         * Oscillator Properties
         */
        [Header("Oscillator Variables")] [SerializeField]
        public ToggleVariable OscillatorOnOffSwitch;
        [SerializeField] public PitchModeVariable ActivePitchMode;
        [SerializeField] public IntVariable Octave;
        [SerializeField] public WaveShapeVariable ActiveWaveShape;

        /**
         * Sequencer Properties
         */
        [Space(20)] [Header("Sequencer Variables")] [SerializeField]
        public ToggleVariable SequencerOnOffSwitch;
        [SerializeField] public SequencerModeVariable SequencerMode;
        [SerializeField] public IntVariable Bpm;

        /**
         * Filter Properties
         */
        [Space(20)] [Header("Filter Variables")] [SerializeField]
        public ToggleVariable FilterOnOffSwitch;
        [SerializeField] public FloatVariable CutoffOffset;

        /**
         * ADSR Properties
         */
        [Space(20)] [Header("Adsr Variables")] [SerializeField]
        public ToggleVariable AdsrOnOffSwitch;
        [SerializeField] public AdsrVariables GlobalAdsr;

        #endregion

        #region Global Settings & Definitions

        private const float QuarterBeat = 0.25f;
        private const float SecsInMinute = 60;

        public enum WaveShape
        {
            Sine = 0,
            Square = 3,
            Triangle = 1,
            Sawtooth = 2
        }

        public enum PitchMode
        {
            Telephone,
            MusicalNotes
        }

        public enum SequencerState
        {
            Running,
            Recording,
            Idle
        }

        #endregion

        #region RealTime Dynamic Properties

        public float EnvelopeFullDuration => NoteLength + GlobalAdsr.ReleaseSecs();
        public float NoteLength => (SecsInMinute * QuarterBeat) / Bpm.Value;

        #endregion

        #region OnEnable & OnDisabled Methods

        private void OnEnable()
        {
            InputManager.PhoneHangup += ResetToInit;

            #region Oscillator

            InputManager.SetPitchMode += UpdatePitchMode;
            InputManager.UpdateOctave += UpdateOctave;
            InputManager.UpdateWaveshape += UpdateWaveShape;

            #endregion

            #region ADSR

            InputManager.UpdateAdsrIsActive += UpdateAdsrEnabled;
            InputManager.UpdateAttack += UpdateAttack;
            InputManager.UpdateDecay += UpdateDecay;
            InputManager.UpdateSustain += UpdateSustain;
            InputManager.UpdateRelease += UpdateRelease;

            #endregion

            #region Filter

            InputManager.UpdateFilterIsActive += UpdateFilterEnabled;
            InputManager.UpdateCutoffPos += UpdateCutoffPos;

            #endregion

            #region Sequencer

            InputManager.UpdateSequencerPlayPauseButtonPressed += ToggleSequencerRunning;
            InputManager.UpdateSequencerRecordButtonPressed += ToggleSequencerRecording;
            InputManager.UpdateBpm += UpdateBpm;

            #endregion

            // if (ControlVoltage == null)
            // {
            //     ControlVoltage = CreateInstance<RealTimeCv>();
            // }

            ResetToInit();
        }

        private void OnDisable()
        {
            InputManager.PhoneHangup -= ResetToInit;

            #region Oscillator

            InputManager.SetPitchMode -= UpdatePitchMode;
            InputManager.UpdateOctave -= UpdateOctave;
            InputManager.UpdateWaveshape -= UpdateWaveShape;

            #endregion

            #region ADSR

            InputManager.UpdateAdsrIsActive -= UpdateAdsrEnabled;
            InputManager.UpdateAttack -= UpdateAttack;
            InputManager.UpdateDecay -= UpdateDecay;
            InputManager.UpdateSustain -= UpdateSustain;
            InputManager.UpdateRelease -= UpdateRelease;

            #endregion

            #region Filter

            InputManager.UpdateFilterIsActive -= UpdateFilterEnabled;
            InputManager.UpdateCutoffPos -= UpdateCutoffPos;

            #endregion

            #region Sequencer

            InputManager.UpdateBpm -= UpdateBpm;

            #endregion
        }

        #endregion

        #region Reset Methods

        private void Reset()
        {
            ResetToInit();
        }

        public void ResetToInit()
        {
            OscillatorOnOffSwitch.ResetToDefault();
            ActiveWaveShape.ResetToDefault();
            ActivePitchMode.ResetToDefault();
            Octave.ResetToDefault();

            SequencerOnOffSwitch.ResetToDefault();
            SequencerMode.ResetToDefault();
            Bpm.ResetToDefault();

            FilterOnOffSwitch.ResetToDefault();
            CutoffOffset.ResetToDefault();

            AdsrOnOffSwitch.ResetToDefault();
            GlobalAdsr.ResetToDefault();
        }

        #endregion

        #region Notification Methods

        #region Oscillator

        public void ToggleOscillatorOnOff(bool isEnabled)
        {
            OscillatorOnOffSwitch.Value = isEnabled;
        }

        public void UpdateOctave(int octaveValue)
        {
            Octave.Value = octaveValue;
        }

        public void UpdateOctave(float raw_value)
        {
            Octave.Value = (int) (Octave.Min + raw_value * (Octave.Max - Octave.Min));
        }

        public void UpdateWaveShape(WaveShape shape)
        {
            ActiveWaveShape.Value = shape;
        }

        public void UpdatePitchMode(PitchMode pitchMode)
        {
            ActivePitchMode.Value = pitchMode;
        }

        #endregion

        #region Filter

        private void ToggleSequencerEnabled(bool isEnabled)
        {
            SequencerOnOffSwitch.Value = isEnabled;
        }

        public void UpdateFilterEnabled()
        {
            FilterOnOffSwitch.Value = !FilterOnOffSwitch.Value;
        }

        public void UpdateCutoffPos(float offsetValue)
        {
            CutoffOffset.Value = offsetValue;
        }

        #endregion

        #region Sequencer

        public void ToggleSequencerRecording()
        {
            switch (SequencerMode.Value)
            {
                case SequencerState.Recording:
                    SequencerMode.Value = SequencerState.Idle;
                    break;
                case SequencerState.Running:
                    SequencerMode.Value = SequencerState.Recording;
                    break;
                case SequencerState.Idle:
                    SequencerMode.Value = SequencerState.Recording;
                    break;
            }
        }

        public void ToggleSequencerRunning()
        {
            switch (SequencerMode.Value)
            {
                case SequencerState.Recording:
                    SequencerMode.Value = SequencerState.Running;
                    break;
                case SequencerState.Running:
                    SequencerMode.Value = SequencerState.Idle;
                    break;
                case SequencerState.Idle:
                    SequencerMode.Value = SequencerState.Running;
                    break;
            }
        }

        public void UpdateBpm(int bpmValue)
        {
            Bpm.Value = bpmValue;
        }

        public void UpdateBpm(float raw_value)
        {
            Bpm.Value = (int) (Bpm.Min + raw_value * (Bpm.Max - Bpm.Min));
        }

        #endregion

        #region ADSR

        public void UpdateAdsrEnabled(bool isEnabled)
        {
            AdsrOnOffSwitch.Value = isEnabled;
        }

        public void UpdateAttack(float attackValue)
        {
            GlobalAdsr.Attack = attackValue;
        }

        public void UpdateDecay(float decayValue)
        {
            GlobalAdsr.Decay = decayValue;
        }

        public void UpdateSustain(float sustainValue)
        {
            GlobalAdsr.Sustain = sustainValue;
        }

        public void UpdateRelease(float releaseValue)
        {
            GlobalAdsr.Release = releaseValue;
        }

        #endregion

        public void SetSynthToSandBoxMode()
        {
            if (!OscillatorOnOffSwitch.Value) OscillatorOnOffSwitch.Value = true;
            if (!SequencerOnOffSwitch.Value) SequencerOnOffSwitch.Value = true;
            if (!FilterOnOffSwitch.Value) FilterOnOffSwitch.Value = true;
            if (!AdsrOnOffSwitch.Value) AdsrOnOffSwitch.Value = true;
            if (ActivePitchMode.Value == PitchMode.Telephone) ActivePitchMode.Value = PitchMode.MusicalNotes;
        }

        public void JumpToSynthToSandBoxMode()
        {
            Debug.Log("Synth Jumped to Sandbox Mode and toggled off and on the modules");
            OscillatorOnOffSwitch.Value = false;
            SequencerOnOffSwitch.Value = false;
            FilterOnOffSwitch.Value = false;
            AdsrOnOffSwitch.Value = false;
            // ActivePitchMode.Value = PitchMode.Telephone;
            OscillatorOnOffSwitch.Value = true;
            SequencerOnOffSwitch.Value = true;
            FilterOnOffSwitch.Value = true;
            AdsrOnOffSwitch.Value = true;

            if (ActivePitchMode.Value == PitchMode.Telephone) ActivePitchMode.Value = PitchMode.MusicalNotes;
        }

        #endregion

        #region Utility Methods

        public int GetNoteInCurrentOctave(Keypad key)
        {
            return (Octave.Value * 12 + 24) + key.MidiValue;
        }

        #endregion
    }
}