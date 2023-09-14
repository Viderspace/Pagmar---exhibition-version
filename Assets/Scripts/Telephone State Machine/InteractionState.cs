using System.Collections;
using FX___Animations.Glitch_Effect;
using Runtime.Kernel.Telephone_State_Machine;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;

namespace Telephone_State_Machine
{
    public class InteractionState : BaseState
    {
        Task _showTimelineTask;
        ToggleVariable _interactionUi;
        public InteractionState(StateMachine system, TMP_Text debugText , ToggleVariable interactionUi) : base(system, debugText)
        {
            _interactionUi = interactionUi;
        }

        public override string StateName
        {
            get => "InteractionState";
        }

        private IEnumerator ShowTimelineTime()
        {
            // var timer = 60f;
            // while (timer > 0)
            // {
            //     timer -= Time.deltaTime;
            //     DebugWindow("State: " + this.GetType().Name + " | timeline position: " + System.TimelineController.TimePosition);
            //     yield return null;
            // }
            System.TimelineController.StartTimeline();
            yield return new WaitForSeconds(.4f);
            GlitchController.OnGlitchTriggered(true);
        }
        

        public override void Enter()
        {
            _interactionUi.Value = true;
            System.currState = StateMachine.CurrState.INTERACTION;
            base.Enter();
            _showTimelineTask = new Task(ShowTimelineTime());
        }
        
        
        
        

        public override void Exit()
        {
            base.Exit();
            // System.TimelineController.ResetToInit();
            // GlitchController.OnGlitchTriggered(false);
            _interactionUi.Value = false;
            if (_showTimelineTask != null && _showTimelineTask.Running)
            {
                _showTimelineTask.Stop();
            }
        }
    }
}