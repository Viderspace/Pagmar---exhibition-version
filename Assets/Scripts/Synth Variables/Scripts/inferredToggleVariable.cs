using System;
using Synth_Variables.Native_Types;

namespace Synth_Variables.Scripts
{
    public class InferredToggleVariabl<T> : ToggleVariable{
        public SynthVariable<T> synthVariable;

        private void OnEnable()
        {
            synthVariable.ValueChanged += CheckAndToggle;
        }

        private void OnDisable()
        {
            synthVariable.ValueChanged -= CheckAndToggle;
        }

        protected virtual void CheckAndToggle(T value)
        {
            throw new NotImplementedException();
        }

#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();
            CheckAndToggle(synthVariable.Value);
        }
#endif
    }
}