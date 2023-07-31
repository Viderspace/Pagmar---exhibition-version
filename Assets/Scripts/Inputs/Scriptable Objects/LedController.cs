using System;
using Synth_Variables.Native_Types;
using UnityEngine;
using Utils;

namespace Inputs.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Arduino LED", menuName = "Arduino/LED", order = 0)]
    public class LedController : ScriptableObject
    {
        [SerializeField] public byte pin;
        [SerializeField] public bool DefaultState;
        [SerializeField] public bool StatePreview;

        [SerializeField] public ToggleVariable OptionalBind;
        private bool _state;
        
        public bool State
        {
            get => _state;
            set
            {
                _state = value;
                OnStateChanged(value);
            }
        }
 
        
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            ValidationUtility.SafeOnValidate(() => { State = StatePreview; });
        }
#endif


        public void Setup()
        {
            ArduinoTransmitter.RegisterLed(pin);
            if (OptionalBind != null)
            {
                OptionalBind.ValueChanged += (b) => { State = b; };
            }
        }

        public void ResetToDefault()
        {
            State = DefaultState;
        }


        
        public event Action<byte, bool> StateChanged;


        protected void OnStateChanged(bool on)
        {
            StatePreview = on;
            StateChanged?.Invoke(pin, on);
        }

    }
}