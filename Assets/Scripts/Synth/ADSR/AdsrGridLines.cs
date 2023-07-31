using System;
using System.Collections;
using Synth_Variables.Adsr;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;

namespace Synth.ADSR
{
    // [ExecuteAlways]
    public class AdsrGridLines : MonoBehaviour
    {
        [SerializeField] private AdsrVariables globalAdsr;
        [SerializeField] private UILineRenderer endAttackLine;
        [SerializeField] private UILineRenderer endDecayLine;
        [SerializeField] private UILineRenderer endSustainLine;
        [SerializeField] private UILineRenderer endReleaseLine;
        [SerializeField] private RectTransform parentRect;
        private float rectWidth => parentRect.rect.width;

        private void SetPosition(RectTransform componentRectTransform, float xValue)
        {
            componentRectTransform.anchoredPosition =
                new Vector2(xValue*rectWidth, componentRectTransform.anchoredPosition.y);
        }

        // Start is called before the first frame update
        void Start()
        {
            MoveLines(globalAdsr);
        }

        private void OnEnable()
        {
            // AdsrController.NotifyWhenAdsrValuesChanged += MoveLines;
            globalAdsr.OnAdsrValuesChanged += MoveLines;
        }

        private void OnDisable()
        {
            globalAdsr.OnAdsrValuesChanged -= MoveLines;
        }

        public void MoveLines(AdsrVariables adsr)
        {
            // Set Attack, Decay & Sustain grid lines x position
            var normAttack = adsr.Attack * 0.33f;
            var normDecay = adsr.Decay * 0.34f;
            SetAttack(normAttack);
            SetDecay(normAttack + normDecay);
            SetSustain(0.67f);
            SetRelease(0.67f + adsr.Release * 0.33f);
        }

        private void SetAttack(float attackPos)
        {
            SetPosition(endAttackLine.GetComponent<RectTransform>(), attackPos);
            // endAttackLine.Points[0].x = attackPos;
            // endAttackLine.Points[1].x = attackPos;
            // endAttackLine.SetAllDirty();
        }

        private void SetDecay(float decayPos)
        {
            SetPosition(endDecayLine.GetComponent<RectTransform>(), decayPos);
            // endDecayLine.Points[0].x = decayPos;
            // endDecayLine.Points[1].x = decayPos;
            // endDecayLine.SetAllDirty();
        }

        private void SetSustain(float sustainPos)
        {
            SetPosition(endSustainLine.GetComponent<RectTransform>(), sustainPos);

            // endSustainLine.Points[0].x = sustainPos;
            // endSustainLine.Points[1].x = sustainPos;
            // endSustainLine.SetAllDirty();
        }

        private void SetRelease(float releasePos)
        {
            SetPosition(endReleaseLine.GetComponent<RectTransform>(), releasePos);
            // endReleaseLine.Points[0].x = releasePos;
            // endReleaseLine.Points[1].x = releasePos;
            // endReleaseLine.SetAllDirty();
        }
    }
}