using System;
using UnityEngine;

namespace Synth_Variables
{
    public abstract class SynthVariable<T> : ScriptableObject 
    {
        public T DefaultValue;
        [Tooltip("Control the value from the inspector, or just watch this value get updated from scene in realtime")]public T ValuePreview;
        protected T _value;
        

        public T Value
        {
            get => _value;
            set
            {
                _value = value;
                OnValueChanged(value);
            }
        }

        public void ResetToDefault()
        {
            Value = DefaultValue;
        }

        public event Action<T> ValueChanged;

        protected void OnValueChanged(T obj)
        {
            ValuePreview = _value;
            ValueChanged?.Invoke(obj);
        }
    }
}