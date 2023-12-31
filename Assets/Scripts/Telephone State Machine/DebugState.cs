using Telephone_State_Machine;
using TMPro;

namespace Runtime.Kernel.Telephone_State_Machine
{
    public class DebugState: BaseState
    {
        public DebugState(StateMachine system, TMP_Text debugText) : base(system, debugText) {}
        public override string StateName { get => "DebugState"; }

        public override void Enter()
        {
            System.currState = StateMachine.CurrState.DEBUG;
            base.Enter();
            base.DebugWindow("Debug State (Press Return key to Exit/Enter Debug State)");
        }
    }
}