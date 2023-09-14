using System;
using System.Collections.Generic;
using Inputs.Scriptable_Objects;
using JetBrains.Annotations;
using Scriptable_Objects;
using Synth_Variables;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using UnityEngine;
using Utils;

namespace Synth
{
    [ExecuteAlways]
    public class SynthControlPanel : MonoBehaviour
    {
        [Space(40)]  [Header("Control Panel")] [Space(20)] 
        
        [Header("Synth Components")]
        public bool OscillatorToggle;
        public bool SequencerToggle;
        public bool FilterToggle;
        public bool AdsrToggle;
        
        [Space(10)]   [Header("Oscillator Controls")] 
        [SerializeField][Range(0,1)] private float MasterVolCtrl;
        [SerializeField][Range(0,8)] private int OctaveCtrl;
        [SerializeField] private SynthController.PitchMode pitchModeCtrl;
        [SerializeField] private SynthController.WaveShape WaveShapeCtrl;
        
        [Space(10)]   [Header("Sequencer Controls")]
        [SerializeField] private SynthController.SequencerState SequencerModeCtrl;
        [SerializeField][Range(0,300)] private int BpmCtrl;
        
        [Space(10)]   [Header("Filter Controls")]
        [SerializeField][Range(0,1)] private float CutoffOffsetCtrl;
        
        [Space(10)]   [Header("Adsr Controls")]
        [SerializeField][Range(0,1)] private float AttackCtrl;
        [SerializeField][Range(0,1)] private float DecayCtrl;
        [SerializeField][Range(0,1)] private float SustainCtrl;
        [SerializeField][Range(0,1)] private float ReleaseCtrl;

        private void Start()
        {
            ValidationUtility.SafeOnValidate(() => { OscillatorOnOff.Value = OscillatorToggle; });
            ValidationUtility.SafeOnValidate(() => { OscillatorOnOff.Value = OscillatorToggle; });
            ValidationUtility.SafeOnValidate(() => { SequencerOnOff.Value = SequencerToggle; });
            ValidationUtility.SafeOnValidate(() => { FilterOnOff.Value = FilterToggle; });
            ValidationUtility.SafeOnValidate(() => { AdsrOnOff.Value = AdsrToggle; });
            ValidationUtility.SafeOnValidate(() => { MasterVolume.Value = MasterVolCtrl; });
            ValidationUtility.SafeOnValidate(() => { Octave.Value = OctaveCtrl; });
            ValidationUtility.SafeOnValidate(() => { pitchModeVariable.Value = pitchModeCtrl; });
            ValidationUtility.SafeOnValidate(() => { ActiveWaveShape.Value = WaveShapeCtrl; });
            ValidationUtility.SafeOnValidate(() => { SequencerMode.Value = SequencerModeCtrl; });
            ValidationUtility.SafeOnValidate(() => { Bpm.Value = BpmCtrl; });
            ValidationUtility.SafeOnValidate(() => { CutoffOffset.Value = CutoffOffsetCtrl; });
            ValidationUtility.SafeOnValidate(() => { Attack.Value = AttackCtrl; });
            ValidationUtility.SafeOnValidate(() => { Decay.Value = DecayCtrl; });
            ValidationUtility.SafeOnValidate(() => { Sustain.Value = SustainCtrl; });
            ValidationUtility.SafeOnValidate(() => { Release.Value = ReleaseCtrl; });
            
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            ValidationUtility.SafeOnValidate(() => { OscillatorOnOff.Value = OscillatorToggle; });
            ValidationUtility.SafeOnValidate(() => { OscillatorOnOff.Value = OscillatorToggle; });
            ValidationUtility.SafeOnValidate(() => { SequencerOnOff.Value = SequencerToggle; });
            ValidationUtility.SafeOnValidate(() => { FilterOnOff.Value = FilterToggle; });
            ValidationUtility.SafeOnValidate(() => { AdsrOnOff.Value = AdsrToggle; });
            ValidationUtility.SafeOnValidate(() => { MasterVolume.Value = MasterVolCtrl; });
            ValidationUtility.SafeOnValidate(() => { Octave.Value = OctaveCtrl; });
            ValidationUtility.SafeOnValidate(() => { pitchModeVariable.Value = pitchModeCtrl; });
            ValidationUtility.SafeOnValidate(() => { ActiveWaveShape.Value = WaveShapeCtrl; });
            ValidationUtility.SafeOnValidate(() => { SequencerMode.Value = SequencerModeCtrl; });
            ValidationUtility.SafeOnValidate(() => { Bpm.Value = BpmCtrl; });
            ValidationUtility.SafeOnValidate(() => { CutoffOffset.Value = CutoffOffsetCtrl; });
            ValidationUtility.SafeOnValidate(() => { Attack.Value = AttackCtrl; });
            ValidationUtility.SafeOnValidate(() => { Decay.Value = DecayCtrl; });
            ValidationUtility.SafeOnValidate(() => { Sustain.Value = SustainCtrl; });
            ValidationUtility.SafeOnValidate(() => { Release.Value = ReleaseCtrl; });
        }
#endif
 

        [Space(40)]  [Header("Component References")] [Space(20)] 
        
        [Header("Oscillator Components")] [Space(10)] 
        [SerializeReference] private ToggleVariable OscillatorOnOff;
        [SerializeField] private FloatVariable MasterVolume;
        [SerializeField] private IntVariable Octave;
        [SerializeField] private PitchModeVariable pitchModeVariable;
        [SerializeField] private WaveShapeVariable ActiveWaveShape;

        [Header("Oscillator Components")] [Space(10)] 
        [SerializeField] private ToggleVariable SequencerOnOff;
        [SerializeField] private SequencerModeVariable SequencerMode;
        [SerializeField] private IntVariable Bpm;

        [Header("Filter Components")] [Space(10)]
        [SerializeField] private ToggleVariable FilterOnOff;
        [SerializeField] private FloatVariable CutoffOffset;
   
        [Header("Adsr Components")] [Space(10)]
        [SerializeField] private ToggleVariable AdsrOnOff;
        [SerializeField] private FloatVariable Attack;
        [SerializeField] private FloatVariable Decay;
        [SerializeField] private FloatVariable Sustain;
        [SerializeField] private FloatVariable Release;
        
        [Header("Led Components")] [Space(10)]
        [SerializeField] private LedController AdsrOnOffLed;
        [SerializeField] private LedController FilterOnOffLed;
        [SerializeField] private LedController NoteOnOffLed;
        [SerializeField] private LedController SeqRunLed;
        [SerializeField] private LedController SeqPauseLed;
        [SerializeField] private LedController SeqRecordLed;
    }
}