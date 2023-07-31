using Inputs.Scriptable_Objects;
using UnityEngine;

namespace Inputs.Input_Devices.Arduino
{
    public class ArduinoReceiver : MonoBehaviour
    {

        [SerializeField] private ArduinoAddresses ArduinoInterface;

        private void OnEnable()
        {
            ArduinoTransmitter.DataReceived += GetData;
        }

        private void OnDisable()
        {
            ArduinoTransmitter.DataReceived -= GetData;
        }

        public void GetData(string data)
        {
            ArduinoInterface.InterpretMassage(data);
        }

    }
}
