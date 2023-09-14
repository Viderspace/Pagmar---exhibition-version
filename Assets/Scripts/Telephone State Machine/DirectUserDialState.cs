using System.Collections;
using Inputs;
using Runtime.Kernel.Telephone_State_Machine;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;

namespace Telephone_State_Machine
{
    /// <summary>
    /// This state is entered when the user has actively picked up the phone (with no ringing prior)
    /// This state enables access to Cheat Codes (Users can 'dial' a code number to skip to the main synth)
    /// 
    /// if the user hangs up the phone, the system will return to the DefaultIdleState
    /// if the user presses wrong code or more than 6 seconds pass, timer will trigger the hacking state
    /// </summary>
    public class DirectUserDialState : BaseState
    {
        #region Fields

        public override string StateName { get => "DirectUserDialState"; }
        private CheatCodes _cheatCodes => Singleton.Instance.CheatCodes;
        private ToggleVariable _telephoneUi;
        private Task _timerTask;
        private float _timer;
        private string _activeKeyName = "None";
        #endregion

        public DirectUserDialState(StateMachine system, TMP_Text debugText, ToggleVariable telephoneUi) : base(system,
            debugText)
        {
            _telephoneUi = telephoneUi;
        }

        #region Timer coroutine
        
        private IEnumerator SetTimerForInteraction()
        {
            _timer = TelephoneSettings.UserDialingTimeUntilInteraction;
            while (_timer > 0)
            {
                DebugWindow("");
                _timer -= Time.deltaTime;
                yield return null;
            }

            System.SwitchState(System.InteractionState);
            yield return null;
        }
        
        #endregion


 
        
        
        #region Override methods
        public override void Enter()
        {
            System.currState = StateMachine.CurrState.DIRECT_USER_DIAL;
            base.Enter();
            _cheatCodes.Reset();
            _telephoneUi.Value = true;
            DebugWindow("DirectUserDialState");
            System.KeypadPlayer.PlayLineTone(true);
            _timerTask = new Task(SetTimerForInteraction());
        }
        
        public override void Exit()
        {
            base.Exit();
            _telephoneUi.Value = false;
            _cheatCodes.Reset();
            System.KeypadPlayer.Mute();
            System.SetRingingSound(false);
            StopTimer();
        }

        private void StopTimer()
        {
            if (_timerTask != null)
            {
                _timerTask.Stop();
                _timerTask = null;
            }
        }

        protected override void OnPhoneHangup()
        {
            base.OnPhoneHangup();
            _telephoneUi.ResetToDefault();
        }

        protected override void OnKeypadPress(Keypad key)
        {
            base.OnKeypadPress(key);
            System.KeypadPlayer.PlayLineTone(false);
            System.KeypadPlayer.OnKeypadDown(key);
            _activeKeyName = key.Name;
            _timer += 1f;
            DirectUserDialInputField.Instance.InsertDigit(key);
        }
        
        protected override void OnKeypadRelease(Keypad key)
        {
            base.OnKeypadRelease(key);
            System.KeypadPlayer.OnKeypadUp(key);

            _activeKeyName = "None";
         
            var cheatCodeDetected = _cheatCodes.ListenForCheatCode(key);
            
            if (cheatCodeDetected)
            {
                System.SwitchState(System.SandboxState);
            }
        }
        
        protected override void DebugWindow(string message )
        {

            base.DebugWindow("DirectUserDialState :" +
                             "\n | Key: " + _activeKeyName +
                             "\n | lineTone : " + System.KeypadPlayer.LineToneIsPlaying +
                             "\n | harshTone : " + System.KeypadPlayer.HarshToneIsPlaying +
                             "\n | code : " + _cheatCodes.CurrentCode() +
                             "\n | time until auto start : " + _timer);
        }
        
        #endregion
    }
}