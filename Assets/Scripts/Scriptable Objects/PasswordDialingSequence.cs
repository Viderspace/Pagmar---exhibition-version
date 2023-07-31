using System.Collections.Generic;
using Inputs;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Password Dialing Sequence")]
    public class PasswordDialingSequence : ScriptableObject
    {
        [Header("Insert chars as they appear on the keypad, or 's' for silence")]
        [SerializeField] public List<char> PasswordSequence = new List<char>();


        public bool IsDialCorrect(Keypad key, int TimeStep)
        {
            if (key == null && PasswordSequence[TimeStep] == 's') return true;
            
            var correctKey = Keypad.GetKeyFromChar(PasswordSequence[TimeStep]);
            return correctKey == key;
        }
    }
}