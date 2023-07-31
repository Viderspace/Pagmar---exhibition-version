using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using Synth_Variables.Scripts;
using TMPro;
using UnityEngine;

public class OscillatorUiWaveshapeDynamicText : MonoBehaviour
{
    [SerializeField] private WaveShapeVariable WaveShapeVariable;
    [SerializeField] private TMP_Text textComponent;
    


    private void OnEnable()
    {
        WaveShapeVariable.ValueChanged += SetWaveShapeText;
    }

    private void OnDisable()
    {
        WaveShapeVariable.ValueChanged -= SetWaveShapeText;
    }

    private void Start()
    {
        SetWaveShapeText(WaveShapeVariable.DefaultValue);
    }

    private void SetWaveShapeText(SynthController.WaveShape newWaveshape)
    {
        string waveshapeText = newWaveshape == SynthController.WaveShape.Sine ? "SineWave" :
            newWaveshape == SynthController.WaveShape.Sawtooth ? "Sawtooth" :
            newWaveshape == SynthController.WaveShape.Square ? "SquareWave" : "Triangle";
        textComponent.text = $"[{waveshapeText}]";
    }
}
