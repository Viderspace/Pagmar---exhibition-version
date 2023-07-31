using System.Collections.Generic;
using Synth_Variables.Native_Types;
using UnityEngine;
using UnityEngine.UI;

public class OscillatorUiSwitchOctave : MonoBehaviour
{
    [SerializeField] private IntVariable OctaveVariable;
    [SerializeField] private Image targetImage;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    


    private void OnEnable()
    {
        OctaveVariable.ValueChanged += SetOctaveSprite;
    }

    private void OnDisable()
    {
        OctaveVariable.ValueChanged -= SetOctaveSprite;
    }

    private void Start()
    {
        SetOctaveSprite(OctaveVariable.DefaultValue);
    }

    private void SetOctaveSprite(int newOctave)
    {
        targetImage.overrideSprite = sprites[newOctave];
    }
}
