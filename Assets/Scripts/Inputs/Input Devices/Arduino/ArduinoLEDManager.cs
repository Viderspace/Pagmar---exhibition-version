using System.Collections;
using System.Collections.Generic;
using Inputs.Scriptable_Objects;
using UnityEngine;

namespace Inputs.Input_Devices
{
    [CreateAssetMenu(fileName = "LED Manager", menuName = "Arduino/LED Manager", order = 0)]
    public class ArduinoLEDManager : ScriptableObject
    {
        [SerializeField] public List<LedController> LedControllers;
        [Header("Blink led effect Settings")]
        [SerializeField] private int blinks = 5;
        [SerializeField] private float speed = 0.5f;

        private void OnEnable()
        {
            InputManager.PhoneHangup += ResetLEDs;
        }
        
        public void ResetLEDs()
        {
            ToggleLEDs(false);
        }

        public void RegisterLEDs()
        {
            foreach (var led in LedControllers)
            {
                led.Setup();
            }
            ResetLEDs();
        }
    
        public void ToggleLEDs(bool on)
        {
            for (int i = 0; i < LedControllers.Count; i++)
            {
                LedControllers[i].State = on;
            }
        }
        
        public IEnumerator BlinkLEDs()
        {
            for (int i = 0; i < blinks; i++)
            {
                ToggleLEDs(true);
                yield return new WaitForSeconds(speed);
                ToggleLEDs(false);
                yield return new WaitForSeconds(speed);
            }
        }
        
    }
}