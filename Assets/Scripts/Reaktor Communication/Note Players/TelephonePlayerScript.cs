using extOSC;
using Inputs;
using UnityEngine;

namespace Reaktor_Communication.Note_Players
{
    [CreateAssetMenu]
    public class TelephonePlayerScript : ScriptableObject
    {
        public void PlayLineTone(bool on, OSCTransmitter transmitter)
        {
            var gateMessage = new OSCMessage("/Telephone/LineTone");
            gateMessage.AddValue(OSCValue.Float(on ? 1 : 0));
            transmitter.Send(gateMessage);
        }
        
        public void PlayHarshTone(bool on, OSCTransmitter transmitter)
        {
            var gateMessage = new OSCMessage("/Telephone/HarshTone");
            gateMessage.AddValue(OSCValue.Float(@on ? 1 : 0));
            transmitter.Send(gateMessage);
        }
        
        public void SendKeypad(Keypad key, bool on, OSCTransmitter transmitter)
        {
            var keyMassgae = new OSCMessage("/Telephone/KeyVal");
            keyMassgae.AddValue(OSCValue.Int(key.ReaktorValue));
            transmitter.Send(keyMassgae);

            var gateMessage = new OSCMessage("/Telephone/Keygate");
            gateMessage.AddValue(OSCValue.Float(on ? 1 : 0));
            transmitter.Send(gateMessage);
        }
    }
}