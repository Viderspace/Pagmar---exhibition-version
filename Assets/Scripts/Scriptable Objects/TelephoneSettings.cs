using System.Collections.Generic;
using Inputs;
using Synth_Variables.Native_Types;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Scriptable_Objects
{
    [CreateAssetMenu]
    public class TelephoneSettings : ScriptableObject
    {
        #region Global Assets

        [SerializeField] private List<char> serialNumber = new List<char>();
        
        [SerializeField]public AudioClip ringingSound;
        [SerializeField]
        [Tooltip("Allows us to enter playmode with the state machine disabled, pressing Enter key to toggle it ")]
        public ToggleVariable debugMode;
        #endregion
        
        private const float AvgIdleTimeUntilRinging = 60*15f;
        
        public const float UserDialingTimeUntilInteraction = 10f;
        
        public float GetRandomIdleTimeUntilRinging()
        {
            return debugMode.Value ? 2 : AvgIdleTimeUntilRinging;
        }
        
        public List<Keypad> MakeSerialNumber()
        {
            var code = new List<Keypad>();
            for (int i = 0; i < serialNumber.Count; i++)
            {
                var key = Keypad.GetKeyFromName(serialNumber[i].ToString());
                if (key == null)
                {
                    Debug.LogError($"Key {serialNumber[i]} is not a valid key");
                    return null;
                }
                code.Add(key);
            }

            return code;
        }


    }
}