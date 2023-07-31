using System.Collections;
using System.Collections.Generic;
using Synth_Variables.Scripts;
using TMPro;
using UnityEngine;

public class AdsrLabelBreakpointDisplayer : MonoBehaviour
{
    [SerializeField] private WidthThresholdVariable Threshold;
    [SerializeField] private TMP_Text textComponent;
    
    
    private void OnEnable()
    {
        Threshold.ValueChanged += DisplayText;
    }
    
    private void OnDisable()
    {
        Threshold.ValueChanged -= DisplayText;
    }

    private void DisplayText(WidthThreshold newState)
    {
        if (newState == WidthThreshold.WideEnough)
        {
            textComponent.enabled = true;
        }
        else
        {
            textComponent.enabled = false;
        }
    }
}
