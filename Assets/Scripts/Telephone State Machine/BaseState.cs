using Inputs;
using Runtime.Kernel.Telephone_State_Machine;
using TMPro;

namespace Telephone_State_Machine
{
    public class BaseState
    {
        protected readonly StateMachine System;
        private readonly TMP_Text _debugText;
        
        public virtual string StateName { get; }

        protected BaseState(StateMachine system, TMP_Text debugText)
        {
            System = system;
            _debugText = debugText;
        }

        #region Virtual Methods
        protected virtual void DebugWindow(string text)
        {
            if (_debugText == null) return;
            _debugText.text = text;
        }
        protected virtual void OnPhonePickup(){}
        protected virtual void OnPhoneHangup()
        {
            System.SwitchState(System.InitState);
        }
        protected virtual void OnKeypadPress(Keypad key){}
        protected virtual void OnKeypadRelease(Keypad key){}
        public virtual void Enter()
        {
            DebugWindow("State: " + this.GetType().Name);
            InputManager.PhonePickup += OnPhonePickup;
            InputManager.PhoneHangup += OnPhoneHangup;
            InputManager.KeypadButtonPressed += OnKeypadPress;
            InputManager.KeypadButtonReleased += OnKeypadRelease;
        }
        public virtual void Exit()
        {
            InputManager.PhonePickup -= OnPhonePickup;
            InputManager.PhoneHangup -= OnPhoneHangup;
            InputManager.KeypadButtonPressed -= OnKeypadPress;
            InputManager.KeypadButtonReleased -= OnKeypadRelease;
        }
        #endregion
    }
}