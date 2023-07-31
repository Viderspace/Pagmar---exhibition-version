using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using Utils;
using MinAttribute = UnityEngine.MinAttribute;

namespace Synth_Variables
{
    [CreateAssetMenu(menuName = "Variables/Float Variable", fileName = "Float Variable")]
    public class FloatVariable : SynthVariable<float>
    {
        public float Min;
        public float Max;

#if UNITY_EDITOR

        private void OnValidate()
        {
            ValidationUtility.SafeOnValidate(() => { Value = ValuePreview; });
        }
#endif
        public new float Value
        {
            get => _value;
            set
            {
                _value = value < Min ? Min : value > Max ? Max : value;
                OnValueChanged(_value);
            }
        }
    }
}