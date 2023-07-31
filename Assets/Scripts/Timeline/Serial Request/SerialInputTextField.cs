using System.Collections.Generic;
using Inputs;
using UnityEngine;

namespace Runtime.Timeline.Main_terminal_Track
{
     public class SerialInputTextField
    {
        // ▁ ▂ ▃ ▄ ▅ ▆ ▇ █ ▀ ▔ ▏ ▎ ▍ ▌ ▋ ▊ ▉ ▐ ▕ ▖ ▗ ▘ ▙ ▚ ▛ ▜ ▝ ▞ ▟ ░ ▒ ▓ ⎕ ⍂  ● ○ ◯ ◔ ◕ ◶ ◌ ◉ ◎ ◦ ◆ ◇
        // 0 0 1 2 3 4 5 6 7 8 9 ₀ ₁ ₂ ₃ ₄ ₅ ₆ ₇ ₈ ₉ ⁰ ¹ ² ³ ⁴ ⁵ ⁶ ⁷ ⁸ ⁹ ⟦ ⌈ ⌊ ⌉ ⌋  __ _  ∎
        private string _hideCharachter = "";
        private string _blankCharacter = "▁";
        public double MonoSpacingCode = 1.5;


        private List<Keypad> _serialNumber;
        private List<Keypad> _userCode;


        public bool HasEnteredAllDigits =>
            _userCode.Count == Singleton.Instance.TelephoneSettings.MakeSerialNumber().Count;

        public SerialInputTextField(List<Keypad> serialNumber)
        {
            _serialNumber = serialNumber;
            _userCode = new List<Keypad>();
        }

        void printdebug()
        {
            string msg = "UserCode: [";
            for (var i = 0; i < _userCode.Count; i++)
            {
                msg += _userCode[i].Name;
            }

            msg += "]   ,    SerialCode: [";
            for (var i = 0; i < _serialNumber.Count; i++)
            {
                msg += _serialNumber[i].Name;
            }

            Debug.Log(msg + "]");
        }

        public string GetCodeWithLastKey(bool revealed = true)
        {
            int showLastChar = (revealed ? 1 : 0);
            string returnString = $"[<mspace={MonoSpacingCode}em>";
            int i = 0;
            for (i = 0; i < _userCode.Count - showLastChar; i++)
            {
                returnString += _hideCharachter;
            }

            if (revealed) returnString += _userCode[i].Name[0];

            for (i = 0; i < _serialNumber.Count - _userCode.Count; i++)
            {
                returnString += _blankCharacter;
            }

            return returnString + "</mspace>]";
        }

        public void InsertDigit(Keypad digit)
        {
            _userCode.Add(digit);
            printdebug();
        }

        public void Clear()
        {
            _userCode.Clear();
        }

        public bool ValidateSerialNumber()
        {
            if (_userCode.Count < _serialNumber.Count) return false;
            for (int i = 0; i < _userCode.Count; i++)
            {
                if (_serialNumber[i].Name[0] != _userCode[i].Name[0])
                    return false;
            }

            return true;
        }
    }
}