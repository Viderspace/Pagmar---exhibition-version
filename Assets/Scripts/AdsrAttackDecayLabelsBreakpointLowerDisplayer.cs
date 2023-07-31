using Synth_Variables.Scripts;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class AdsrAttackDecayLabelsBreakpointLowerDisplayer : MonoBehaviour
    {
        [SerializeField] private WidthThresholdVariable FirstLabelThreshold;
        [SerializeField] private WidthThresholdVariable SecondLabelThreshold;
        [SerializeField] private string FirstLabel = "Attack";
        [SerializeField] private string SecondLabel = "Decay";

        [SerializeField] private TMP_Text textComponent;
    
    
        private void OnEnable()
        {
            FirstLabelThreshold.ValueChanged += UpdateTextField;
            SecondLabelThreshold.ValueChanged += UpdateTextField;
        }
    
        private void OnDisable()
        {
            FirstLabelThreshold.ValueChanged -= UpdateTextField;
            SecondLabelThreshold.ValueChanged -= UpdateTextField;
        }

        private string SetText(WidthThresholdVariable firstLabel, WidthThresholdVariable secondLabel)
        {
            if (firstLabel.Value == WidthThreshold.WideEnough && secondLabel.Value == WidthThreshold.WideEnough)
            {
                return "";
            }
        
            if (firstLabel.Value == WidthThreshold.TooNarrow && secondLabel.Value == WidthThreshold.WideEnough)
            {
                return FirstLabel;
            }
        
            if (firstLabel.Value == WidthThreshold.WideEnough && secondLabel.Value == WidthThreshold.TooNarrow)
            {
                return "";
            }   
 
            else
            {
                return FirstLabel + " | "+ SecondLabel;
            }
        }
    
        private void UpdateTextField(WidthThreshold newState)
        {
            textComponent.text = SetText(FirstLabelThreshold, SecondLabelThreshold);
        }
    }
}