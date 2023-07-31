using Scriptable_Objects;
using UnityEngine;

namespace Synth_Variables.Scripts
{
    [CreateAssetMenu(fileName = "Sequencer Mode Inferred Variable", menuName = "Variables/Sequencer Mode Inferred Variable")]

    public class SequencerModeInferredVariable : InferredToggleVariabl<SynthController.SequencerState>{
    [Tooltip("Specify the sequencer mode that will trigger this inferred variable to true")]
    public SynthController.SequencerState triggerState;

        protected override void CheckAndToggle(SynthController.SequencerState value)
        {
            this.Value = (value == triggerState);
        }
    }
}