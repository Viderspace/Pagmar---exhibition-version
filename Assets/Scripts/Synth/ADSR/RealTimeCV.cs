using System;
using extOSC;
using UnityEditor;
using UnityEngine;

namespace Synth.Modules.ADSR
{
    /// <summary>
    /// Deprecated
    /// </summary>
    public class RealTimeCv : ScriptableObject
    {
        private float _maxSampled = Single.Epsilon;
        public float Value { get; private set; }
        public float NormalizedValue { get; private set; }


        
        public static event Action<float> OnUpdateControlVoltage;

        public void Listen(OSCMessage msg)
        {
            Sample(msg.Values[0].FloatValue);
            
        }

        private void Sample(float cv)
        {
            _maxSampled = Mathf.Max(cv, _maxSampled);
            Value = cv;
            NormalizedValue = cv / _maxSampled;
            OnUpdateControlVoltage?.Invoke(NormalizedValue);
        }
    }
}