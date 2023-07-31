using System;
using Synth_Variables.Scripts;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class AdsrDecayOnlyLabelBreakpointLowerDisplayer : MonoBehaviour
    {
        [SerializeField] private WidthThresholdVariable AttackLabelThreshold;
        [SerializeField] private WidthThresholdVariable DecayLabelThreshold;
        [SerializeField] private string DecayLabel = "Drop";

        [SerializeField] private TMP_Text textComponent;

        private void Start()
        {
            textComponent.text = SetText(AttackLabelThreshold, DecayLabelThreshold);
        }

        private void OnEnable()
        {
            AttackLabelThreshold.ValueChanged += UpdateTextField;
            DecayLabelThreshold.ValueChanged += UpdateTextField;
        }
    
        private void OnDisable()
        {
            AttackLabelThreshold.ValueChanged -= UpdateTextField;
            DecayLabelThreshold.ValueChanged -= UpdateTextField;
        }

        private string SetText(WidthThresholdVariable attackLabel, WidthThresholdVariable decayLabel)
        {
            if (attackLabel.Value == WidthThreshold.WideEnough && decayLabel.Value == WidthThreshold.TooNarrow)
            {
                return DecayLabel;
            }
            return "";
            
        }
    
        private void UpdateTextField(WidthThreshold newState)
        {
            textComponent.text = SetText(AttackLabelThreshold, DecayLabelThreshold);
        }
    }
}