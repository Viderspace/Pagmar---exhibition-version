using System.Collections.Generic;
using Synth_Variables.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class CutoffLineBreakpoinrDisplayer : MonoBehaviour
{
    [SerializeField] private HiMidLowThresholdVariable Threshold;
    [SerializeField] private Threshold ActiveZone;
    [SerializeField] private bool ExcludeActiveZone;
    [SerializeField] private List<Graphic> uiComponent = new List<Graphic>();

    private void OnEnable()
    {
        Threshold.ValueChanged += DisplayGraphic;
    }
    
    private void OnDisable()
    {
        Threshold.ValueChanged -= DisplayGraphic;
    }

    private void Start()
    {
        DisplayGraphic(Threshold.DefaultValue);
    }
    
    
    private void ToggleComponents(bool on)
    {
        foreach (var component in uiComponent)
        {
            component.enabled = on;
        }
    }

    private void DisplayGraphic(Threshold currentZone)
    {
        if (currentZone == ActiveZone)
        {
            if (ExcludeActiveZone)
            {
                ToggleComponents(false);
            }
            else
            {
                ToggleComponents(true);
            }

        }
        else
        {
            if (ExcludeActiveZone)
            {
                ToggleComponents(true);
            }
            else
            {
                ToggleComponents(false);
            }
        }
    }
}
