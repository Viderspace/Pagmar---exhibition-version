using Scriptable_Objects;
using UnityEngine;
using Utils;

namespace Synth_Variables.Scripts
{
    [CreateAssetMenu(fileName =  "Sequencer Mode", menuName ="Variables/Sequencer Mode")]
    public class SequencerModeVariable: SynthVariable<SynthController.SequencerState>
    {
                        
#if UNITY_EDITOR

        private void OnValidate()
        {
            ValidationUtility.SafeOnValidate(() => { Value = ValuePreview; });
        }
#endif

    }
}