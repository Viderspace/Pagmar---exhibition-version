using Inputs;
using Runtime.Timeline.Main_terminal_Track;
using Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Timeline.Serial_Input_Routine
{
    /// <summary>
    /// Inside the time line, in the part where the user is asked to enter the serial number, this script is responsible for
    /// listening to the keypad and dialing the serial number.
    /// When the User finishes dialing in all the keys, the script will check if the serial number is correct and will
    /// Invoke the event OnSerialNumberEntered.
    /// </summary>>
    public class SerialConsolePrompt : MonoBehaviour
    {
        [SerializeField] TMP_Text textComponent;

        [SerializeField] [Range(0, 3)] double monoSpacingCode = 1.5;
        // private string enterserialPrompt = "  ▁ ▂ ▃ ▄ ▅ ▆ ▇ █ ▀ ▔ ▏ ▎ ▍ ▌ ▋ ▊ ▉ ▐ ▕ ▖ ▗ ▘ ▙ ▚ ▛ ▜ ▝ ▞ ▟ ░ ▒ ▓ ⎕ ⍂  ● ○ ◯ ◔ ◕ ◶ ◌ ◉ ◎ ◦ ◆ ◇ 0 0 1 2 3 4 5 6 7 8 9 ₀ ₁ ₂ ₃ ₄ ₅ ₆ ₇ ₈ ₉ ⁰ ¹ ² ³ ⁴ ⁵ ⁶ ⁷ ⁸ ⁹ ⟦ ⌈ ⌊ ⌉ ⌋  __ _  ∎";

        
        float _timeBetweenDigits = .3f;
        private float time;

        private readonly string _enterSerialPrompt = "Serial Id: ";
        private SerialInputTextField _serialInputTextField;
        




        public delegate void OnUserEnteredAllDigits(bool serialIsCorrect);
        public OnUserEnteredAllDigits UserEnteredAllDigits;
        
        



        #region MonoBehaviour




        private void OnValidate()
        {
            if (_serialInputTextField != null)
            {
                _serialInputTextField.MonoSpacingCode = monoSpacingCode;
                textComponent.text = _enterSerialPrompt + _serialInputTextField.GetCodeWithLastKey(false);
            }
        }
        
        private SerialRequestManager _serialRequestManager;
        
        private void Start()
        {
            _serialRequestManager = GetComponentInParent<SerialRequestManager>();
            _serialInputTextField = new SerialInputTextField(Singleton.Instance.TelephoneSettings.MakeSerialNumber())
            {
                MonoSpacingCode = monoSpacingCode
            };
            textComponent.text = _enterSerialPrompt + _serialInputTextField.GetCodeWithLastKey(false);
            ShowPrompt(false);

        }

        private void Update()
        {
            if (_serialRequestManager.blueBoxProgramLoader.IsRunning) return;
            if (time > 0)
            {
                time -= Time.deltaTime;
                if (time < 0)
                {
                    textComponent.text = _enterSerialPrompt + _serialInputTextField.GetCodeWithLastKey(false);
                }
            }

            if (_serialInputTextField.HasEnteredAllDigits)
            {
                UserEnteredAllDigits?.Invoke(SerialIsCorrect());
                _serialInputTextField.Clear();
                return;
            }
        }

        #endregion

        public void Clear()
        {
            _serialInputTextField.Clear();
        }
        
        public void ShowPrompt(bool on = true)
        {
           if (on) InputManager.KeypadButtonPressed += DialPassword;
              else InputManager.KeypadButtonPressed -= DialPassword;
            textComponent.gameObject.SetActive(on);
            if (!on && _serialInputTextField != null) _serialInputTextField.Clear();
        }
  

        private void InsertNewDigit(Keypad newKey)
        {
            if (_serialInputTextField.HasEnteredAllDigits) return;
            _serialInputTextField.InsertDigit(newKey);
            Singleton.Instance.AudioFx.Play(AudioFx.FX.DoubleKeystroke);
            textComponent.text = _enterSerialPrompt + _serialInputTextField.GetCodeWithLastKey(true);
            time = _timeBetweenDigits;
        }


        bool SerialIsCorrect()
        {
            bool serialIsCorrect = _serialInputTextField.ValidateSerialNumber();
            Debug.Log(serialIsCorrect ? "Serial number matched" : "Serial number did not match");
            return serialIsCorrect;
        }

        public void DialPassword(Keypad keypad)
        {
            InsertNewDigit(keypad);
            // _listenKeypadQueue.Push(keypad);
        }
    }
}