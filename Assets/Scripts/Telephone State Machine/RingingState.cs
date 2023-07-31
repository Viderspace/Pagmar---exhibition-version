using System.Collections;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;

namespace Runtime.Kernel.Telephone_State_Machine
{
    /**
     * Simulates the phone ringing
     * 
     * This state is triggered automatically by the InitIdleState with a random timer.
     * 
     * If the user picks up the phone:
     *              the state machine switches to Hacking state
     * otherwise:
     *              returns to Idle state
     */
    
    
    public class RingingState : BaseState
    {
        private Task _timerTask;
        private ToggleVariable _ringingUi;
        private bool RingingIsActive => _timerTask != null && _timerTask.Running;
        
        public RingingState(StateMachine system, TMP_Text debugText, ToggleVariable ringingUi) : base(system, debugText)
        {
            _ringingUi = ringingUi;
        }
        
        public override string StateName
        {
            get => "RingingState";
        }

        private IEnumerator RingTime()
        {
            var ringingDuration = System.Settings.ringingSound.length + 0.2f;
            // first 3 rings
            System.SetRingingSound(true);
            while (ringingDuration > 0)
            {
                ringingDuration -= Time.deltaTime;
                DebugWindow("State: " + this.GetType().Name + " | ringing till: " + ringingDuration);
                yield return null;
            }
            System.SetRingingSound(false);
            
            ringingDuration = System.Settings.ringingSound.length + 0.2f;
            // 3 more rings
            System.SetRingingSound(true);
            while (ringingDuration > 0)
            {
                ringingDuration -= Time.deltaTime;
                DebugWindow("State: " + this.GetType().Name + " | ringing again till: " + ringingDuration);
                yield return null;
            }
            System.SetRingingSound(false);
            
            // if the coroutine is still running, revert to idle state
            System.SwitchState(System.InitState);
        }
    
        


        #region Overrides 
        public override void Enter()
        {
            base.Enter();
            _ringingUi.Value = true;
            _timerTask = new Task(RingTime());
        }

        public override void Exit()
        {
            base.Exit();
            _ringingUi.Value = false;
            if (RingingIsActive)
            {
                _timerTask.Stop();
            }
            System.SetRingingSound(false);

        }
        
        #endregion
        
        #region IPayphoneSystem interface

        protected override void OnPhonePickup()
        {
            Debug.Log("State machine picked up the phone from RingingState and switched to InteractionState");
            System.SwitchState(System.InteractionState);
        }



        
        #endregion

    
    }
}