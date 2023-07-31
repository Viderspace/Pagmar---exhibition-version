using System;
using System.Collections;
using Synth_Variables.Adsr;
using Synth_Variables.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Synth.ADSR
{
    // [ExecuteAlways]
    public class AdsrDynamicLabels : MonoBehaviour
    {
        [SerializeField] private AdsrVariables globalAdsr;
        [SerializeField] private RectTransform parentRect;
        private float rectWidth => parentRect.rect.width;


        private Adsr _lastAdsrValues;
        private HighlightTask _highlightTask;

        [SerializeField] private TMP_Text attackText;
        [SerializeField] private TMP_Text decayText;
        [SerializeField] private TMP_Text sustainText;
        [SerializeField] private TMP_Text releaseText;
        
        
        
        
        
        [SerializeField] private float highlightTime = 0.5f;

        private void Start()
        {
            _lastAdsrValues = globalAdsr.GetCopy();
            _highlightTask = new HighlightTask(attackText, decayText, sustainText, releaseText, highlightTime);
        }
        
        private void OnEnable()
        {
            globalAdsr.OnAdsrValuesChanged += UpdateLabelWidth;
        }

        private void OnDisable()
        {
            globalAdsr.OnAdsrValuesChanged -= UpdateLabelWidth;
        }

        // _lastAdsrValues.ResetValues(adsr.Attack, adsr.Decay, adsr.Sustain, adsr.Release);

        
        
        
        // private void HighlightChangedParameters(AdsrVariables newValues)
        // {
        //     if (Math.Abs(newValues.Attack - _lastAdsrValues.Attack) > 0)
        //     {
        //         _highlightTask.HighlightAttack();
        //     }
        //
        //     if (Math.Abs(newValues.Decay - _lastAdsrValues.Decay) > 0)
        //     {
        //         _highlightTask.HighlightDecay();
        //     }
        //
        //     if (Math.Abs(newValues.Sustain - _lastAdsrValues.Sustain) > 0)
        //     {
        //         _highlightTask.HighlightSustain();
        //     }
        //
        //     if (Math.Abs(newValues.Release - _lastAdsrValues.Release) > 0)
        //     {
        //         _highlightTask.HighlightRelease();
        //     }
        // }
        
        
        /**
     * Update the width of the attack , decay, sustain and release text labels
     * according to the current ADSR values
     */
        private void UpdateLabelWidth(AdsrVariables adsr)
        {
            var attackWidth = (0.36f * rectWidth) * adsr.Attack;
            attackText.rectTransform.sizeDelta = new Vector2(attackWidth, attackText.rectTransform.sizeDelta.y);
            FadeLabel(attackText, attackWidth);
            
            var decayWidth = (0.37f * rectWidth) * adsr.Decay;
            decayText.rectTransform.sizeDelta = new Vector2(decayWidth, decayText.rectTransform.sizeDelta.y);
            FadeLabel(decayText, decayWidth);
            
            var sustainWidth = rectWidth * 0.7f - (attackWidth + decayWidth);
            sustainText.rectTransform.sizeDelta = new Vector2(sustainWidth, sustainText.rectTransform.sizeDelta.y);
            FadeLabel(sustainText, sustainWidth);
            
            var releaseWidth = (0.33f * rectWidth) * adsr.Release;
            releaseText.rectTransform.sizeDelta = new Vector2(releaseWidth, releaseText.rectTransform.sizeDelta.y);
            FadeLabel(releaseText, releaseWidth);
        }

        private void FadeLabel(Graphic label, float width)
        {
            if (width >= 50 && label.color.a >= 1) return;
            var color = label.color;
            var transparency = width / 50f;
            color.a = transparency;
            label.color = color;
        }
    }
    
    
    public class HighlightTask
    {
        private readonly float _highlightTime;
        private Task _attackHighlightTask;
        private Task _decayHighlightTask;
        private Task _sustainHighlightTask;
        private Task _releaseHighlightTask;
        private readonly TMP_Text _attackText;
        private readonly TMP_Text _decayText;
        private readonly TMP_Text _sustainText;
        private readonly TMP_Text _releaseText;

        public HighlightTask(TMP_Text a, TMP_Text d, TMP_Text s, TMP_Text r, float highlightTime = 0.8f)
        {
            _attackText = a;
            _decayText = d;
            _sustainText = s;
            _releaseText = r;
            _highlightTime = highlightTime;
        }

        IEnumerator HighlightText(Graphic text, float time)
        {
            // text.faceColor = Color.white;
            Color ogColor = text.color;
            text.color = new Color(1, 1, 1, ogColor.a);
            yield return new WaitForSeconds(time);
            text.color = new Color(ogColor.r, ogColor.g, ogColor.b, text.color.a);
        }

        public void HighlightAttack()
        {
            if (_attackHighlightTask != null && _attackHighlightTask.Running) return;
            _attackHighlightTask = new Task(HighlightText(_attackText, _highlightTime));
        }

        public void HighlightDecay()
        {
            if (_decayHighlightTask != null && _decayHighlightTask.Running) return;
            _decayHighlightTask = new Task(HighlightText(_decayText, _highlightTime));
        }

        public void HighlightSustain()
        {
            if (_sustainHighlightTask != null && _sustainHighlightTask.Running) return;
            _sustainHighlightTask = new Task(HighlightText(_sustainText, _highlightTime));
        }

        public void HighlightRelease()
        {
            if (_releaseHighlightTask != null && _releaseHighlightTask.Running) return;
            _releaseHighlightTask = new Task(HighlightText(_releaseText, _highlightTime));
        }
    }
}