using FX___Animations.Glitch_Effect;
using Runtime.Kernel.System;
using Runtime.Kernel.Telephone_State_Machine;
using TMPro;

namespace Telephone_State_Machine
{
    public class SandboxState: BaseState
    {
        public SandboxState(StateMachine system, TMP_Text debugText) : base(system, debugText)
        {
        }

        public override string StateName
        {
            get => "SandboxState";
        }

        protected override void DebugWindow(string message )
        {
            base.DebugWindow("Sandbox State");
        }

        
        public override void Enter()
        {
            base.Enter();
            Singleton.Instance.SynthController.SetSynthToSandBoxMode();
            TimelineController.Instance.MoveToEnd();
            base.DebugWindow("Sandbox State");
        }
        
        public override void Exit()
        {
            base.Exit();
            GlitchController.OnGlitchTriggered(false);

        }
    }
}