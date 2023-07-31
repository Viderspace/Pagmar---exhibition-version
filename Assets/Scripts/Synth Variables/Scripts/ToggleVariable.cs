using System;
using UnityEngine;
using Utils;

namespace Synth_Variables.Native_Types
{
    [CreateAssetMenu(fileName =  "Toggle Variable", menuName ="Variables/Toggle ON-OFF Variable")]

    public class ToggleVariable: SynthVariable<bool>
    {
        public new bool Value
        {
            get => _value;
            set
            {
                if (value == _value) return;
                _value = value;
                OnValueChanged(_value);
            }
        }
        
        
#if UNITY_EDITOR

        protected virtual void OnValidate()
        {
            ValidationUtility.SafeOnValidate(() => { Value = ValuePreview; });
        }
#endif

    }
    
}