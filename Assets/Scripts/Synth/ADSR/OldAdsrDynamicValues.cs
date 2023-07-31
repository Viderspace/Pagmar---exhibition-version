// namespace Synth.Modules.ADSR
// {
//     public class OldAdsrDynamicValues
//     {
//         using System;
// using extOSC;
// using Inputs;
// using Synth.Modules.ADSR;
// using Synth.Reaktor;
// using Synth.Sequencer;
// using UnityEngine;
//
// namespace Synth.ADSR
// {
//     /**
//      * This Singleton class is responsible for frequently Updating and managing ADSR Parameters from reaktor in real time.
//      * Objects can subscribe to get Notifications from Reaktor when one of the ADSR parameters is changed.
//      * The subscribing classes will have the liberty to visually represent the ADSR parameters in any way they want.
//      */
//     public class AdsrController : MonoBehaviour
//     {
//         private static AdsrController _instance;
//         public static AdsrController Instance => _instance;
//         
//
//         // private string reaktorAddress = "/Trig";  -- !  deprecated address
//         private const string AdsrRequestAddress = "/ADSR/GET";
//         private const string AdsrReceiveAddress = "/ADSR/ALL";
//         private readonly RealtimeCv _cv = new();
//         public float CurrentCv => _cv.Value;
//         public float NormalizedCv => _cv.NormalizedValue;
//
//         private Adsr _globalAdsr;
//
//
//         // This Method is called by an OSC FloatEvent, it updates The cv value in real time
//         public void SetRealTimeCv(float sample)
//         {
//             _cv.Sample(sample);
//         }
//         
//         public void SetRealTimeCv(OSCMessage msg)
//         {
//             _cv.Sample(msg.Values[0].FloatValue);
//         }
//
//
//         public static event Action<Adsr> NotifyWhenAdsrValuesChanged;
//
//         public float EnvelopeFullDuration => Sequencer.SequencerModule.Instance.NoteLength + Adsr.Release;
//
//         private void Awake()
//         {
//             if (_instance != null)
//             {
//                 Destroy(gameObject);
//                 return;
//             }
//             _instance = this;
//             _globalAdsr = new Adsr(InputSettings.DefaultAdsrValues);
//         }
//
//         private void OnEnable()
//         {
//             InputManager.UpdateAttack += SetAttack;
//             InputManager.UpdateDecay += SetDecay;
//             InputManager.UpdateSustain += SetSustain;
//             InputManager.UpdateRelease += SetRelease;
//         }
//
//         private void OnDisable()
//         {
//             InputManager.UpdateAttack -= SetAttack;
//             InputManager.UpdateDecay -= SetDecay;
//             InputManager.UpdateSustain -= SetSustain;
//             InputManager.UpdateRelease -= SetRelease;
//         }
//         
//         public void SetAttack(float value)
//         {
//             Adsr.Attack = value;
//             Instance.NotifyAdsrValueChange();
//             ReaktorController.Instance.SetAttack(value);
//         }
//         public void SetDecay(float value)
//         {
//             Adsr.Decay = value;
//             Instance.NotifyAdsrValueChange();
//             ReaktorController.Instance.SetDecay(value);
//         }
//         public void SetSustain(float value)
//         {
//             Adsr.Sustain = value;
//             Instance.NotifyAdsrValueChange();
//             ReaktorController.Instance.SetSustain(value);
//         }
//         public void SetRelease(float value)
//         {
//             Adsr.Release = value;
//             Instance.NotifyAdsrValueChange();
//             ReaktorController.Instance.SetRelease(value);
//         }
//
//         void Start()
//         {
//             BindReceiversToReaktor();
//             SetAdsrInitialValuesInReaktor();
//             // FindObjectOfType<OSCReceiver>().Bind("/ADSR/CVOUT", SetRealTimeCv);
//         }
//
//         private void SetAdsrInitialValuesInReaktor()
//         {
//             Vector4 initVals = InputSettings.DefaultAdsrValues;
//             Adsr.SetValues(initVals.x, initVals.y, initVals.z, initVals.w);
//             ReaktorController.Instance.SetAttack(Adsr.Attack);
//             ReaktorController.Instance.SetDecay(Adsr.Decay);
//             ReaktorController.Instance.SetSustain(Adsr.Sustain);
//             ReaktorController.Instance.SetRelease(Adsr.Release);
//             SendTrigMsg();
//         }
//         
//
//
//         public void SendTrigMsg()
//         {
//             var message = new OSCMessage(AdsrRequestAddress);
//             message.AddValue(OSCValue.Float(1f)); // Trigger value is insignificant
//             ReaktorController.Instance.Transmitter.Send(message);
//             ReaktorController.Instance.Transmitter.Send(message);
//         }
//
//         // #region Binders to Reaktor
//
//         private void BindReceiversToReaktor()
//         {
//             ReaktorController.Instance.Receiver.Bind(AdsrReceiveAddress, ReceiveAdsrValues);
//         }
//
//         private void ReceiveAdsrValues(OSCMessage rawValue)
//         {
//             var attackValue = rawValue.Values[0].FloatValue / 80;
//             var decayValue = rawValue.Values[1].FloatValue / 80;
//             var sustainValue = rawValue.Values[2].FloatValue;
//             var releaseValue = rawValue.Values[3].FloatValue / 80;
//             Adsr.SetValues(attackValue, decayValue, sustainValue, releaseValue);
//             NotifyAdsrValueChange();
//         }
//
//         public void NotifyAdsrValueChange()
//         {
//             NotifyWhenAdsrValuesChanged?.Invoke(Adsr.Instance);
//         }
//         
//     }
//
//     public class RealtimeCv
//     {
//         private float _maxSampled = Single.Epsilon;
//         public float Value { get; private set; }
//         public float NormalizedValue { get; private set; }
//
//         public void Sample(float cv)
//         {
//             _maxSampled = Mathf.Max(cv, _maxSampled);
//             Value = cv;
//             NormalizedValue = cv / _maxSampled;
//         }
//     }
// }
//     }
// }