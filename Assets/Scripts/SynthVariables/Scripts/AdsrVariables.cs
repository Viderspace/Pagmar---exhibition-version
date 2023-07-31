using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Synth_Variables.Adsr
{
    [CreateAssetMenu(fileName =  "Adsr Variables", menuName ="Variables/Adsr")]
    public class AdsrVariables : ScriptableObject
    {
        public event Action<AdsrVariables> OnAdsrValuesChanged;
        public FloatVariable attack;
        public FloatVariable decay;
        public FloatVariable sustain;
        public FloatVariable release;

        private void OnEnable()
        {
            attack.ValueChanged += UpdateChange;
            decay.ValueChanged += UpdateChange;
            sustain.ValueChanged += UpdateChange;
            release.ValueChanged += UpdateChange;
        }
        
        private void OnDisable()
        {
            attack.ValueChanged -= UpdateChange;
            decay.ValueChanged -= UpdateChange;
            sustain.ValueChanged -= UpdateChange;
            release.ValueChanged -= UpdateChange;
        }

        public void UpdateChange(float ignore)
        {
            OnAdsrValuesChanged?.Invoke(this);

        }
        
        
        #region Properties
        
        public float Attack
        {
            set
            {
                attack.Value = value;
                // OnAdsrValuesChanged?.Invoke(this);
            }
            get { return attack.Value; }
        }
        
        public float Decay
        {
            set
            {
                decay.Value = value;
                // OnAdsrValuesChanged?.Invoke(this);
            }
            get { return decay.Value; }
        }


        public float Sustain
        {
            set
            {
                sustain.Value = value;
                // OnAdsrValuesChanged?.Invoke(this);
            }
            get { return sustain.Value; }
        }

        public float Release
        {
            set
            {
                release.Value = value;
                // OnAdsrValuesChanged?.Invoke(this);
            }
            get { return release.Value; }
        }
        
        
        
        public float AttackSecs() => ParamInSecs(Attack);
        public float DecaySecs() => ParamInSecs(Decay);
        public float ReleaseSecs() => ParamInSecs(Release);
        #endregion

        #region Setters

        public void ResetToDefault()
        {
            attack.ResetToDefault();
            decay.ResetToDefault();
            sustain.ResetToDefault();
            release.ResetToDefault();
        }
        
        public void Set(float a, float d, float s, float r)
        {
            Attack= a;
            Decay= d;
            Sustain = s;
            Release = r;
            OnAdsrValuesChanged?.Invoke(this);
        }

        public void Set(Vector4 values)
        {
            Set(values.x, values.y, values.z, values.w);
        }
        
        public void Set(AdsrVariables adsr)
        {
            Set(adsr.Attack, adsr.Decay, adsr.Sustain, adsr.Release);
        }
        
        #endregion
        
        
        private static float ParamInSecs(float x)
        {
            return MathF.Pow(10, 4 * x) / 1000; // formula for converting [0,1] to actual seconds in reaktor
        }
        
        
        public  Synth.ADSR.Adsr GetCopy()
        {
            var copy = new Vector4(Attack, Decay, Sustain, Release);
            return new Synth.ADSR.Adsr(copy);

        }
        
        

    }
}