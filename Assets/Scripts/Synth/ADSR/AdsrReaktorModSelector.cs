
using Reaktor_Communication;
using UnityEngine;
using UnityEngine.UI;

namespace Synth.ADSR
{
    /// <summary>
    ///  TODO-Deprecated
    /// </summary>
    public class AdsrReaktorModSelector : MonoBehaviour
    {
        [SerializeField] private Image cutOffImage;
        [SerializeField] private RawImage spectrumProjector;
        private const float AmpCode = -1;
        private const float FilterCode = 1;
        
        // private void OnEnable()
        // {
        //     SynthData.OnFilterEnabledChanged += ActivateFilter;
        // }
        //
        // private void OnDisable()
        // {
        //     SynthData.OnFilterEnabledChanged -= ActivateFilter;
        // }

        private void ActivateFilter(bool active)
        {
            ReaktorController.Instance.SetAdsrTarget(active ? FilterCode : AmpCode);
            cutOffImage.enabled = active;
            spectrumProjector.enabled = active;
        }
    }
}