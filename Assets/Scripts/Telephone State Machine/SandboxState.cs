using FX___Animations.Glitch_Effect;
using Runtime.Kernel.System;
using Runtime.Kernel.Telephone_State_Machine;
using TMPro;

namespace Telephone_State_Machine
{
    public class SandboxState: BaseState
    {
        private StateMachine _system;
        public SandboxState(StateMachine system, TMP_Text debugText) : base(system, debugText)
        {
            _system = system;
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
            System.currState = StateMachine.CurrState.SANDBOX;
            GlitchController.OnGlitchTriggered(true);
            TimelineController.Instance.MoveToEnd();
            Singleton.Instance.SynthController.JumpToSynthToSandBoxMode();
            base.DebugWindow("Sandbox State");
        }
        
        public override void Exit()
        {
            base.Exit();
            // GlitchController.OnGlitchTriggered(false);
            // _system.isInSandboxMode.Value = false;
        }
        
        
    }
}