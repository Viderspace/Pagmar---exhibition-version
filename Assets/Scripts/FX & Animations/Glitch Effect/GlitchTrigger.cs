using System.Collections;
using System.Collections.Generic;
using FX___Animations.Glitch_Effect;
using Inputs;
using UnityEngine;
using UnityEngine.UI;

public class GlitchTrigger : MonoBehaviour
{
    public bool displayInTelephoneMode = false;
    public bool displayInOramMode = false;
    public Graphic graphicComponent;
    public AnimationCurve glitchCurve = new AnimationCurve();
    private void OramTakoverGlitchStart(float glitchDuration)
    {
        StartCoroutine(OramTakeoverGlitch(glitchDuration));
    }
    
    private void OramTakoverGlitchStart()
    {
        OramTakoverGlitchStart(Singleton.Instance.AudioFx.GlitchDuration);
    }
    
    private void TelephoneRestoreGlitchStart()
    {
        StartCoroutine(TelephoneRestoreGlitch(Singleton.Instance.AudioFx.GlitchDuration));
    }
    
    private void ListenToGlitch(bool intoTheMatrix)
    {
        if (intoTheMatrix)
        {
            OramTakoverGlitchStart();
        }
        else
        {
            TelephoneRestoreGlitchStart();
        }
    }
    
    
    
    private void OnEnable()
    {
        GlitchController.GlitchTriggered += ListenToGlitch;
    }
    private void OnDisable()
    {
        GlitchController.GlitchTriggered -= ListenToGlitch;
    }
    
    private IEnumerator OramTakeoverGlitch(float glitchDuration)
    {
        Color color = graphicComponent.color;
        float t = 0;
        while (t < glitchDuration)
        {
            float progress = t / glitchDuration;
            color.a = (Random.value > progress) ? glitchCurve.Evaluate(progress) : 0;
            graphicComponent.color = color;
            yield return new WaitForFixedUpdate();
            t += Time.deltaTime;
        }

        color.a = displayInOramMode ? 1 : 0;
        graphicComponent.color = color;
    }
    private IEnumerator TelephoneRestoreGlitch(float glitchDuration)
    { 
        Color color = graphicComponent.color;
        float t = 0;

        while (t < glitchDuration)
        {
            float progress = 1-t / glitchDuration;
            color.a = (Random.value > progress) ? glitchCurve.Evaluate(progress) : 0;
            graphicComponent.color = color;
            yield return new WaitForFixedUpdate();
            t += Time.deltaTime;
        }

        color.a = displayInTelephoneMode ? 1 : 0;
        graphicComponent.color = color;
    }


    private float Jitter(float t, float duration)
    {
        return (Random.value > (t / duration)) ? 1 : 0;
    }

    
}
