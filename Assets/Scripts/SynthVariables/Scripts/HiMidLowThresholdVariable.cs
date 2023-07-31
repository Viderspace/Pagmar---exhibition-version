using System;
using SynthVariables.Scripts;
using UnityEngine;

namespace Synth_Variables.Scripts
{
    public enum Threshold
    {
        High,
        Mid,
        Low
    }
    [CreateAssetMenu(fileName = "ThresholdVariable", menuName = "Variables/Threshold Variable")]
    public class HiMidLowThresholdVariable : SynthVariable<Threshold>
    {
        [SerializeField] public FloatVariable Variable;
        [SerializeField] public float HighThreshold;
        [SerializeField] public float LowThreshold;

        private void OnEnable()
        {
            Variable.ValueChanged += SetThreshold;
        }

        private void SetThreshold(float value)
        {
            if (value > HighThreshold)
            {
                Value = Threshold.High;
            }
            else if (value < LowThreshold)
            {
                Value = Threshold.Low;
            }
            else
            {
                Value = Threshold.Mid;
            }
        }
    }
}