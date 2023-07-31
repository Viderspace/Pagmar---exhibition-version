using UnityEngine;
using Utils;

namespace Synth_Variables.Native_Types
{
    [CreateAssetMenu(menuName = "Variables/Integer Variable", fileName = "Integer Variable")]
    public class IntVariable: SynthVariable<int>
    {
        public int Min;
        public int Max;
        
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            ValidationUtility.SafeOnValidate(() => { Value = ValuePreview; });
        }
#endif
        public new int Value
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