using System;
using Scriptable_Objects;
using Synth_Variables.Adsr;
using Synth.ADSR;
using UnityEngine;
using UnityEngine.UI.Extensions;

namespace Synth.Modules.ADSR
{
    // [ExecuteAlways]
    public class AdsrLineVisualizer : MonoBehaviour
    {
        SynthController SynthController => Singleton.Instance.SynthController;
        public AdsrVariables globalAdsr;
        [SerializeField] private UILineRenderer line;


        // Start is called before the first frame update
        void Start()
        {
            RenderLine(SynthController.GlobalAdsr);
        }

        private void OnEnable()
        {
            if (line == null) line = GetComponent<UILineRenderer>();
            globalAdsr.OnAdsrValuesChanged += RenderLine;
            // AdsrController.NotifyWhenAdsrValuesChanged += RenderLine;

        }

        private void OnDisable()
        {
            globalAdsr.OnAdsrValuesChanged -= RenderLine;
            // AdsrController.NotifyWhenAdsrValuesChanged -= RenderLine;
        }


        private void RenderLine(AdsrVariables adsr)
        {
            // Set Attack, Decay & Sustain x position
            var normAttack = adsr.Attack * 0.33f;
            var normDecay = adsr.Decay * 0.34f;
            line.Points[1].x = normAttack; // attack end x
            line.Points[2].x = normAttack + normDecay; // decay end x
            line.Points[3].x = 0.67f; // decay end x

            // set Sustain
            line.Points[2].y = adsr.Sustain;
            line.Points[3].y = adsr.Sustain;

            line.Points[4].x = 0.67f + adsr.Release * 0.33f;

            line.SetAllDirty();
        }
    }
}