using System;
using Reaktor_Communication;
using Scriptable_Objects;
using Synth_Variables;
using Synth_Variables.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Synth.ADSR
{
    // [ExecuteAlways]
    public class FilterWindowAnimator : MonoBehaviour
    {
        [SerializeField] private FloatVariable Offset;
        [SerializeField] private FloatVariable envelopeMod;
        [SerializeField] private FloatVariable currentFrequency;
        

        [SerializeField] private float windowHeight;

        private Vector2 Position
        {
            get => gameObject.GetComponent<RectTransform>().anchoredPosition;
            set => gameObject.GetComponent<RectTransform>().anchoredPosition = value;
        }
  
        
        private Vector2 verticalMax;
         private Vector2 verticalMin;

         // private void UpdateOffset(float newOffset)
         // {
         //     _offset = newOffset;
         // }
         //
         //
         // private void UpdateEnvMod(float newEnvMod)
         // {
         //     _envMod = newEnvMod;
         // }
         


        private void Start()
        {
            verticalMax = new Vector2(0, windowHeight/2);
            verticalMin = new Vector2(0, -windowHeight/2);
        }

        private void Update()
        {
            currentFrequency.Value = Offset.Value + envelopeMod.Value;
            Position = Vector2.Lerp(verticalMin, verticalMax, currentFrequency.Value);
        }

        // public void SetPosition(float adsrPosition)
        // {
        //     _latestAdsrPosition = adsrPosition;
        //     transform.localPosition = Vector3.Lerp(minPositionVector, maxPositionVector, _offset+ _latestAdsrPosition);
        // }
        //
        // public void SetVertiacalPosition(float adsrPosition)
        // {
        //    var min = new Vector3(0, verticalMin, 0);
        //       var max = new Vector3(0, verticalMax, 0);
        //     _latestAdsrPosition = adsrPosition;
        //     transform.localPosition = Vector3.Lerp(min, max, _offset+ _latestAdsrPosition);
        // }
        //
        //
        // public void SetOffsetVariable(float offset)
        // {
        //     ReaktorController.Instance.SetCutoffPos(offset);
        //     _offset = offset;
        //     transform.localPosition = Vector3.Lerp(minPositionVector, maxPositionVector, _offset+ _latestAdsrPosition);
        // }
    

    
    }
}
