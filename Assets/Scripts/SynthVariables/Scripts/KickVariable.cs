using Synth_Variables.Native_Types;
using UnityEngine;

namespace SynthVariables.Scripts
{
    public class KickVariable : ScriptableObject
    {
        [SerializeField] ToggleVariable kickToggle;
        [SerializeField] Mode currentMode;
        enum Mode
        {
            Kick808,
            KickDeadmau5,
            Off
        }
    }
}