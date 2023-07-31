using System;
using SynthVariables.Scripts;
using UnityEngine;

namespace Synth_Variables.Scripts
{
    public enum WidthThreshold
    {
        TooNarrow,
        WideEnough
    }


    [CreateAssetMenu(fileName = "WidthThresholdVariable", menuName = "Variables/Width Threshold Variable")]
    public class WidthThresholdVariable  : SynthVariable<WidthThreshold>
    {
        [SerializeField] public FloatVariable Variable;
        [SerializeField] public float Threshold;

        private void OnEnable()
        {
            Variable.ValueChanged += SetThreshold;
        }

        private void OnDisable()
        {
            Variable.ValueChanged -= SetThreshold;
        }

        private void SetThreshold(float value)
        {
            if (value >= Threshold)
            {
                Value = WidthThreshold.WideEnough;
            }
    
            else
            {
                Value = WidthThreshold.TooNarrow;
            }
        }
        
    }
}