// using System;
// using Scriptable_Objects;
// using Synth.ADSR;
// using UnityEngine;
//
// namespace Synth.Modules.ADSR
// {
//     /**
//      * This Singleton class is responsible for managing ADSR Parameters in real time.
//      * Objects can subscribe to get Notifications from Reaktor when one of the ADSR parameters is changed.
//      * The subscribing classes will have the liberty to visually represent the ADSR parameters in any way they want.
//      */
//     public class AdsrController : MonoBehaviour
//     {
//         SynthController SynthController => Singleton.Instance.SynthController;
//         private void OnEnable()
//         {
//             SynthController.OnAttackChanged += OnAdsrValuesChanged;
//             SynthController.OnDecayChanged += OnAdsrValuesChanged;
//             SynthController.OnSustainChanged += OnAdsrValuesChanged;
//             SynthController.OnReleaseChanged += OnAdsrValuesChanged;
//         }
//
//         private void OnDisable()
//         {
//             SynthController.OnAttackChanged -= OnAdsrValuesChanged;
//             SynthController.OnDecayChanged -= OnAdsrValuesChanged;
//             SynthController.OnSustainChanged -= OnAdsrValuesChanged;
//             SynthController.OnReleaseChanged -= OnAdsrValuesChanged;
//         }
//         
//      
//    
//
//
//         public static event Action<Adsr> NotifyWhenAdsrValuesChanged;
//         private  void OnAdsrValuesChanged(float ignore)
//         {
//             NotifyWhenAdsrValuesChanged?.Invoke(SynthController.GlobalAdsr);
//         }
//         
//
//
//     }
// }