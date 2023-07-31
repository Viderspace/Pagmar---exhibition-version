using System.Collections.Generic;
using Inputs;
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
        [SerializeField][Tooltip("Allows us to enter playmode with the state machine disabled, pressing Enter key to toggle it ")] 
        public bool debugMode;
        #endregion
        
        private const float AvgIdleTimeUntilRinging = 12f;
        
        public const float UserDialingTimeUntilInteraction = 10f;
        
        public float GetRandomIdleTimeUntilRinging()
        {
            return debugMode ? 2 :AvgIdleTimeUntilRinging +Random.Range(-5f, 5f);
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