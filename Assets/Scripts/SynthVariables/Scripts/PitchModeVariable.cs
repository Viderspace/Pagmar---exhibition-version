
using Scriptable_Objects;
using SynthVariables.Scripts;
using UnityEngine;
using Utils;

namespace Synth_Variables.Scripts
{
    [CreateAssetMenu(fileName =  "Pitch Mode Variable", menuName ="Variables/Pitch Mode")]
    public class PitchModeVariable : SynthVariable<SynthController.PitchMode>
    {
        
                
#if UNITY_EDITOR

        private void OnValidate()
        {
            ValidationUtility.SafeOnValidate(() => { Value = ValuePreview; });
        }
#endif
        
    }
}