using extOSC;
using Inputs;
using Synth_Variables.Native_Types;
using UnityEngine;

namespace Reaktor_Communication.Note_Players
{
    public class TelephonePlayer : BasePlayer
    {
        private TelephonePlayerScript _telephoneTransmitter;
        private OSCTransmitter _transmitter;
        public ToggleVariable NoteOnVariable;

        private void Start()
        {
            _telephoneTransmitter = ScriptableObject.CreateInstance<TelephonePlayerScript>();
            _transmitter = ReaktorController.Instance.Transmitter;
        }
        
        public override void OnKeypadDown(Keypad key)
        {
            base.OnKeypadDown(key);
            NoteOnVariable.Value = true;
            _telephoneTransmitter.SendKeypad(key, true, _transmitter);
        }

        public override void OnKeypadUp(Keypad key)
        {
            base.OnKeypadUp(key);
            NoteOnVariable.Value = false;
            _telephoneTransmitter.SendKeypad(key, false, _transmitter);

        }

        public override void PlayLineTone(bool on)
        {
            base.PlayLineTone(on);
            _telephoneTransmitter.PlayLineTone(on, _transmitter);
        }
        public override void PlayHarshTone(bool on)
        {
            base.PlayHarshTone(on);
            _telephoneTransmitter.PlayHarshTone(on, _transmitter);
        }
    }
    
}