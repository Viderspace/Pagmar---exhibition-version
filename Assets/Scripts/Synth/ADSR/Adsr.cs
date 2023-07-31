using System;
using UnityEngine;

namespace Synth.ADSR
{
    public class Adsr
    {
        private Vector4 _values;

        public Adsr(Vector4 initValues)
        {
            ResetValues(initValues.x, initValues.y, initValues.z, initValues.w);
        }

        public void ResetValues(float attack, float decay, float sustain, float release)
        {
            _values = new Vector4(attack, decay, sustain, release);
        }

        private static float ParamInSecs(float x)
        {
            return MathF.Pow(10, 4 * x) / 1000; // formula for converting [0,1] to actual seconds in reaktor
        }

        public float AttackSecs() => ParamInSecs(Attack);
        public float DecaySecs() => ParamInSecs(Decay);
        public float ReleaseSecs() => ParamInSecs(Release);

        public float Attack
        {
            get => _values.x;
            set => _values.x = value;
        }
        

        public float Decay
        {
            get => _values.y;
            set => _values.y = value;
        }


        public float Sustain
        {
            get => _values.z;
            set => _values.z = value;
        }


        public float Release
        {
            get => _values.w;
            set => _values.w = value;
        }
    }
}