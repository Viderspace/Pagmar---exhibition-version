using extOSC.Components.Informers;
using extOSC.UI;
using UnityEngine;

namespace Synth.ADSR
{
    public class AdsrRoteryUpdate : MonoBehaviour
    {
        private OSCRotary _myKnob;
        private OSCTransmitterInformerFloat _myTransmitter;
        // Start is called before the first frame update

        private void Awake()
        {
            _myKnob = GetComponent<OSCRotary>();
            _myTransmitter = GetComponent<OSCTransmitterInformerFloat>();
            _myTransmitter.enabled = false;
        }



        public void GetValueFromReaktor(float newVal)
        {
            _myKnob.Value = newVal;
            _myTransmitter.enabled = true;
        }
    }
}
