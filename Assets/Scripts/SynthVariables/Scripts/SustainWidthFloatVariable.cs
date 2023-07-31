using UnityEngine;

namespace Synth_Variables.Scripts
{
    [CreateAssetMenu(fileName = "Sustain Width Float Variable", menuName = "Variables/Sustain Width Float Variable")]
    public class SustainWidthFloatVariable : FloatVariable
    {
        [SerializeField] public FloatVariable AttackVariable;
        [SerializeField] public FloatVariable DecayVariable;
        private void OnEnable()
        {
            AttackVariable.ValueChanged += SetValue;
            DecayVariable.ValueChanged += SetValue;
        }

        private void OnDisable()
        {
            AttackVariable.ValueChanged -= SetValue;
            DecayVariable.ValueChanged -= SetValue;
        }
        private void SetValue(float value)
        {
            Value = 1- (AttackVariable.Value + DecayVariable.Value) / 2;
        }
    }
}