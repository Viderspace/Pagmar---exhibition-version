using Scriptable_Objects;
using SynthVariables.Scripts;
using UnityEngine;
using Utils;

namespace Synth_Variables.Scripts
{
    [CreateAssetMenu(fileName = "Waveshape Variable", menuName = "Variables/Waveshape")]
    public class WaveShapeVariable : SynthVariable<SynthController.WaveShape>
    {
        
#if UNITY_EDITOR

        private void OnValidate()
        {
            ValidationUtility.SafeOnValidate(() => { Value = ValuePreview; });
        }
#endif


        public float OscValue()
        {
            switch (Value)
            {
                case SynthController.WaveShape.Sine:
                    return 0f;
                case SynthController.WaveShape.Triangle:
                    return 1f/ 3;
                case SynthController.WaveShape.Sawtooth:
                    return 2f/3;
                case SynthController.WaveShape.Square:
                    return 1f;
                default:
                    return 0;
            }
        }
    }
}