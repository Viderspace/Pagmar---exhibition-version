using System.Collections;
using Inputs;
using Runtime.Kernel.System;
using Scriptable_Objects;
using Synth_Variables.Adsr;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Utils;

namespace Synth.ADSR
{
    public class BlueADSRGuideLines : MonoBehaviour
    {
        public static BlueADSRGuideLines Instance;
        public AdsrVariables globalAdsr;
        public AdsrVariables blueGuideLinesAdsr;

        private bool sequenceIsActive = false;
        [SerializeField] private UILineRenderer attackLine;
        [SerializeField] private UILineRenderer decayLine;
        [SerializeField] private UILineRenderer sustainLine;
        [SerializeField] private UILineRenderer releaseLine;
        [SerializeField][Range(0,1)] private float errorMargin = 0.02f;

        public bool[] adsrMatch = new bool[4];
        private bool missionAccomplished => adsrMatch[0] && adsrMatch[1] && adsrMatch[2] && adsrMatch[3];
        

        private void OnEnable()
        {
            InputManager.PhoneHangup += ExitSequence;
            Instance = this;
            RenderLine(blueGuideLinesAdsr);
            ShowLine(false);
        }

        private void OnDisable()
        {
            InputManager.PhoneHangup -= ExitSequence;
        }

        public void EnterSequence()
        {
            adsrMatch= new [] {false, false, false, false};
            Singleton.Instance.AudioFx.Play(AudioFx.FX.SmallImpact);
            sequenceIsActive = true;
            RenderLine(blueGuideLinesAdsr);
            RevealAnimation(true);
            globalAdsr.OnAdsrValuesChanged += MatchValuesWithAdsr;
        }
        
        public void ExitSequence()
        {
            if (!sequenceIsActive) return;
            sequenceIsActive = false;
            RevealAnimation(false);
            globalAdsr.OnAdsrValuesChanged -= MatchValuesWithAdsr;
            TimelineController.Instance.SpecialEvents.AdsrIsMatched();
        }

        private void SetAttackLine(Vector2 start, Vector2 end)
        {
            attackLine.Points[0] = start;
            attackLine.Points[1] = end;
            attackLine.SetAllDirty();
        }
        private void SetDecayLine(Vector2 start, Vector2 end)
        {
            decayLine.Points[0] = start;
            decayLine.Points[1] = end;
            decayLine.SetAllDirty();
        }
        private void SetSustainLine(Vector2 start, Vector2 end)
        {
            sustainLine.Points[0] = start;
            sustainLine.Points[1] = end;
            sustainLine.SetAllDirty();
        }
        private void SetReleaseLine(Vector2 start, Vector2 end)
        {
            releaseLine.Points[0] = start;
            releaseLine.Points[1] = end;
            releaseLine.SetAllDirty();
        }

        private void RenderLine(AdsrVariables adsr)
        {
            blueGuideLinesAdsr.ResetToDefault();
            // Set Attack, Decay & Sustain x position
            var normAttack = adsr.Attack * 0.33f;
            var normDecay = adsr.Decay * 0.34f;
            SetAttackLine(Vector2.zero, new Vector2(normAttack, 1));
            SetDecayLine(new Vector2(normAttack, 1), new Vector2(normAttack + normDecay, adsr.Sustain));
            SetSustainLine(new Vector2(normAttack + normDecay, adsr.Sustain), new Vector2(0.67f, adsr.Sustain));
            SetReleaseLine(new Vector2(0.67f, adsr.Sustain), new Vector2(0.67f + adsr.Release * 0.33f, 0));
        }

        private void ShowLine(bool show)
        {
            attackLine.gameObject.SetActive(show);
            decayLine.gameObject.SetActive(show);
            sustainLine.gameObject.SetActive(show);
            releaseLine.gameObject.SetActive(show);
        }

        private void RevealAnimation(bool show)
        {
            StartCoroutine(BlinkReveal(show));
        }

        IEnumerator BlinkReveal(bool show)
        {
            for (int i = 0; i < 3; i++)
            {
                yield return new WaitForSeconds(0.1f);
                ShowLine(show);
                yield return new WaitForSeconds(0.1f);
                ShowLine(!show);
            }
            yield return new WaitForSeconds(0.1f);
            ShowLine(show);
        }

        private void MatchValue(UILineRenderer line,int index, float blueGuide, float value)
        {
            bool isInRange =Mathf.Abs(blueGuide - value) < errorMargin;

            if (!isInRange && adsrMatch[index])
            {
                line.color = DesignPalette.LightBlue;
                adsrMatch[index] = false;
                return;
            }
            
            if (isInRange && !adsrMatch[index])
            {
                line.color = DesignPalette.Green;
                adsrMatch[index] = true;
                Singleton.Instance.AudioFx.Play(AudioFx.FX.SmallImpact);
            }
        }

        private void MatchValuesWithAdsr(AdsrVariables adsr)
        {
            MatchValue(attackLine,0, blueGuideLinesAdsr.attack.Value, adsr.attack.Value);
            MatchValue(decayLine,1, blueGuideLinesAdsr.decay.Value, adsr.decay.Value);
            MatchValue(sustainLine,2, blueGuideLinesAdsr.sustain.Value, adsr.sustain.Value);
            MatchValue(releaseLine,3, blueGuideLinesAdsr.release.Value, adsr.release.Value);

            if (missionAccomplished)
            {
                Singleton.Instance.AudioFx.Play(AudioFx.FX.InsertNewLine);
                ExitSequence();
            }
        }
    }
}