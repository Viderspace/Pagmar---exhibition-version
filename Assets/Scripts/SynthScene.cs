using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using UnityEngine;

public class SynthScene : MonoBehaviour
{
    [SerializeField] private ToggleVariable oscillatorOnOffSwitch;
    [SerializeField] private ToggleVariable sequencerOnOffSwitch;
    [SerializeField] private ToggleVariable filterOnOffSwitch;
    [SerializeField] private ToggleVariable adsrOnOffSwitch;
    [SerializeField] private PitchModeVariable activePitchMode;
    // [SerializeField] private IntVariable octave;
    // [SerializeField] private WaveShapeVariable activeWaveShape;
    // [SerializeField] private SequencerModeVariable sequencerMode;
    // [SerializeField] private IntVariable bpm;
    //

    public void TurnOnSynthModules()
    {
        oscillatorOnOffSwitch.Value = true;
        sequencerOnOffSwitch.Value = true;
        filterOnOffSwitch.Value = true;
        adsrOnOffSwitch.Value = true;
        activePitchMode.Value = SynthController.PitchMode.MusicalNotes;
    }
    IEnumerator Start()
    {
        yield return new WaitForSeconds(3f);
        TurnOnSynthModules();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
