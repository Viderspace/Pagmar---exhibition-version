using System.Collections;
using FX___Animations.Glitch_Effect;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Runtime.Kernel.Telephone_State_Machine
{
    public class InitState : BaseState
    {
        #region Fields & Properties
        
        private Task _timerTask;
        private ToggleVariable _telephoneIdleUi;

        public override string StateName
        {
            get => "InitState";
        }

        private bool TimerIsActive => _timerTask != null && _timerTask.Running;

        #endregion

        public InitState(StateMachine system, TMP_Text debugText, ToggleVariable telephoneIdleUi) : base(system,
            debugText)
        {
            _telephoneIdleUi = telephoneIdleUi;
        }
        
        
        #region Timer Coroutine
        private IEnumerator SetTimerForRinging()
        {
            var upcomingRing = System.Settings.GetRandomIdleTimeUntilRinging();
            while (upcomingRing > 0)
            {
                DebugWindow("State: " + this.GetType().Name + " | ringing in: " + upcomingRing);
                upcomingRing -= Time.deltaTime;
                yield return null;
            }

            yield return new WaitForSeconds(upcomingRing);
            System.SwitchState(System.RingingState);
            yield return null;
        }
        #endregion
        #region Override methods
        
        public override void Enter()
        {
            base.Enter();
            _telephoneIdleUi.Value = true;
            _timerTask = new Task(SetTimerForRinging());
        }

        public override void Exit()
        {
            base.Exit();
            _telephoneIdleUi.Value = false;
            if (TimerIsActive) _timerTask.Stop();
        }
        
        protected override void OnPhonePickup()
        {
            System.SwitchState(System.DirectUserDialState);
        }
        #endregion
    }
}