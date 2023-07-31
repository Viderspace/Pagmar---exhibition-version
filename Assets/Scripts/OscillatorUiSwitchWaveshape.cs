using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using Synth_Variables.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class OscillatorUiSwitchWaveshape : MonoBehaviour
{
    [SerializeField] private WaveShapeVariable WaveShapeVariable;
    [SerializeField] private Image targetImage;
    [SerializeField] private Sprite SquareSprite;
    [SerializeField] private Sprite SawSprite;
    [SerializeField] private Sprite TriangleSprite;
    [SerializeField] private Sprite SineSprite;

    private void OnEnable()
    {
        WaveShapeVariable.ValueChanged += SetShape;
    }

    private void OnDisable()
    {
        WaveShapeVariable.ValueChanged -= SetShape;
    }

    private void Start()
    {
        SetShape(WaveShapeVariable.DefaultValue);
    }

    private void SetShape(SynthController.WaveShape newShape)
    {
        switch (newShape)
        {
            case SynthController.WaveShape.Sine:
                targetImage.overrideSprite = SineSprite;
                
                break;
            case SynthController.WaveShape.Sawtooth:
                targetImage.overrideSprite = SawSprite;
                break;
            case SynthController.WaveShape.Square:
                targetImage.overrideSprite = SquareSprite;
                break;
            case SynthController.WaveShape.Triangle:
                targetImage.overrideSprite = TriangleSprite;
                break;
            default:
                targetImage.overrideSprite = SineSprite;
                break;
        }
    }
}
