using System.Collections;
using System.Collections.Generic;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OscillatorUiOctaveDynamicText : MonoBehaviour
{
    [SerializeField] private IntVariable OctaveVariable;
    [SerializeField] private TMP_Text textComponent;
    


    private void OnEnable()
    {
        OctaveVariable.ValueChanged += SetOctaveText;
    }

    private void OnDisable()
    {
        OctaveVariable.ValueChanged -= SetOctaveText;
    }

    private void Start()
    {
        SetOctaveText(OctaveVariable.DefaultValue);
    }

    private void SetOctaveText(int newOctave)
    {
        textComponent.text = $"[Range(C{newOctave},C{newOctave + 1})]";
    }
}
