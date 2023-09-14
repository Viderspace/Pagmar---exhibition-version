using System.Collections;
using FX___Animations.Glitch_Effect;
using Inputs.Input_Devices.Arduino;
using Runtime.Kernel.Telephone_State_Machine;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;

namespace Telephone_State_Machine
{
    public class InitState : BaseState
    {
        #region Fields & Properties
        DistanceSensor _distanceSensor;
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
            System.currState = StateMachine.CurrState.IDLE;
            GlitchController.OnGlitchTriggered(false);
            base.Enter();
            _telephoneIdleUi.Value = true;
            if (System.DistanceSensorMode)
            {
                DistanceSensor.DistanceDetected += StartRingingWhenSensorIsTriggered;
                DebugWindow("State: " + this.GetType().Name + " | " + "Distance Sensor Mode");
            }
            else
            {
                _timerTask = new Task(SetTimerForRinging());
            }
        }

        public override void Exit()
        {
            base.Exit();
            _telephoneIdleUi.Value = false;
            if (TimerIsActive) _timerTask.Stop();
            if (System.DistanceSensorMode)
            {
                DistanceSensor.DistanceDetected -= StartRingingWhenSensorIsTriggered;
            }
        }
        
        
        private void StartRingingWhenSensorIsTriggered()
        {
            System.SwitchState(System.RingingState);
            
        }
        
        protected override void OnPhonePickup()
        {

            System.SwitchState(System.DirectUserDialState);
        }

        // protected override void OnPhoneHangup()
        // {
        //     return;
        // }

        #endregion
    }
}