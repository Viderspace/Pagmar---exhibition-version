using System;
using extOSC;
using Inputs;
using Scriptable_Objects;
using Synth_Variables;
using Synth_Variables.Adsr;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Reaktor_Communication
{
    /**
     * API for sending messages to Reaktor (playing midi notes & adjusting synth parameters).
     */
    [RequireComponent(typeof(OSCTransmitter), typeof(OSCReceiver))]
    public class ReaktorController : MonoBehaviour
    {
        private static ReaktorController _instance;
        public static ReaktorController Instance => _instance;

        [SerializeField] private ToggleVariable FilterOnOffSwitch;
        [SerializeField] private FloatVariable CutoffOffset;
        [SerializeField] private FloatVariable CvModListener;
        
        [SerializeField] public WaveShapeVariable waveShapeVariable;

        [SerializeField] private ToggleVariable AdsrOnOffSwitch;
        [SerializeField] private AdsrVariables globalAdsr;
        
        [SerializeField] private NoteManager noteManager;
        
        private OSCTransmitter _transmitter;
        private OSCReceiver _receiver;
        public OSCTransmitter Transmitter => _transmitter;
        public OSCReceiver Receiver => _receiver;
        
        public NoteManager NoteManager  => noteManager;

        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            else _instance = this;

            _transmitter = GetComponent<OSCTransmitter>();
            _receiver = GetComponent<OSCReceiver>();
            
            Receiver.Bind("/ADSR/CVOUT", ReceiveCvMod);
        }
        
        private void ReceiveCvMod(OSCMessage message)
        {
            CvModListener.Value = message.Values[0].FloatValue;
        }
        


        private void OnEnable()
        {
            InputManager.UpdateMasterVolume += SetMasterVolume;
            waveShapeVariable.ValueChanged += SetWaveform;
            AdsrOnOffSwitch.ValueChanged += EnableAdsr;
            globalAdsr.attack.ValueChanged += SetAttack;
            globalAdsr.decay.ValueChanged += SetDecay;
            globalAdsr.sustain.ValueChanged += SetSustain;
            globalAdsr.release.ValueChanged += SetRelease;
            CutoffOffset.ValueChanged += SetCutoffPos;
            FilterOnOffSwitch.ValueChanged += ActivateFilter;


        }

        private void OnDisable()
        {
            InputManager.UpdateMasterVolume -= SetMasterVolume;
            waveShapeVariable.ValueChanged -= SetWaveform;
            AdsrOnOffSwitch.ValueChanged -= EnableAdsr;
            globalAdsr.attack.ValueChanged -= SetAttack;
            globalAdsr.decay.ValueChanged -= SetDecay;
            globalAdsr.sustain.ValueChanged -= SetSustain;
            globalAdsr.release.ValueChanged -= SetRelease;
            CutoffOffset.ValueChanged -= SetCutoffPos;

            FilterOnOffSwitch.ValueChanged  -= ActivateFilter;
        }

        #region API Methods
        
        public void ActivateFilter(bool active)
        {
            SetAdsrTarget(active ? 1 : -1);
        }

        public void SetMasterVolume(float vol)
        {
            SendFloat("/MasterVol", vol);
        }
        
        private void EnableAdsr(bool enable)
        {
            SendFloat("/ADSR/Toggle", enable ? 1 : 0);
        }

        public void SetAdsrTarget(float targetVal)
        {
            SendFloat("/ModTarget", targetVal);
        }

        public void SetCutoffPos(float pos)
        {
            SendFloat("/CutoffPos", pos);
        }

        public void SetAttack(float attack)
        {
            SendFloat("/ADSR/A", attack);
        }

        public void SetDecay(float decay)
        {
            SendFloat("/ADSR/D", decay);
        }

        public void SetSustain(float sustain)
        {
            SendFloat("/ADSR/S", sustain);
        }

        public void SetRelease(float release)
        {
            SendFloat("/ADSR/R", release);
        }

        private void SetWaveform(SynthController.WaveShape ignore)
        {
            var message = new OSCMessage("/waveShape");
            message.AddValue(OSCValue.Float(waveShapeVariable.OscValue()));
            Transmitter.Send(message);
        }


        // Waveshape buttons are creating the massages themselves, so we just need to send them.
        public void SetWaveShape(OSCMessage shape) => Transmitter.Send(shape);

        #endregion

        private void SendFloat(string address, float value)
        {
            var message = new OSCMessage(address);
            message.AddValue(OSCValue.Float(value));
            Transmitter.Send(message);
        }
        
        private void SendInt(string address, int value)
        {
            var message = new OSCMessage(address);
            message.AddValue(OSCValue.Int(value));
            Transmitter.Send(message);
        }

        private void OnApplicationQuit()
        {
            EnableAdsr(false);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) EnableAdsr(false);
            else if (AdsrOnOffSwitch.Value == true) EnableAdsr(true);
        }
    }



}