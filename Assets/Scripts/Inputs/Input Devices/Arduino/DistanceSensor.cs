using System;
using UnityEngine;

namespace Inputs.Input_Devices.Arduino
{
    public class DistanceSensor : MonoBehaviour
    {
        public float SecondsUntilTrigger;
        private float _timeSinceLastTrigger;
        private bool _isNear;
        public static event Action DistanceDetected;

        private static void OnDistanceDetected()
        {
            DistanceDetected?.Invoke();
        }

        private void OnEnable()
        {
            InputManager.DistanceDetected += RegisterDistanceEvent;
        }

        private void RegisterDistanceEvent(bool isNear)
        {
            _isNear = isNear;
            _timeSinceLastTrigger = 0;
        }

        private void Update()
        {
            if (!_isNear) return;
            _timeSinceLastTrigger += Time.deltaTime;
            if (_timeSinceLastTrigger >= SecondsUntilTrigger)
            {
                OnDistanceDetected();
            }
        }
    }
}