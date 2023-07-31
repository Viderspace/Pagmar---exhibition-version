using System;
using System.Collections;
using System.Collections.Generic;
using Synth.Sequencer;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class AdsrShaderTrigger : MonoBehaviour
{
    public Material targetMaterial;
    // public Texture2D gradientMap;
    
    private static AdsrShaderTrigger instance;
    public static AdsrShaderTrigger Instance => instance;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            targetMaterial = GetComponent<UILineRenderer>().material;
            // targetMaterial.SetTexture("_GradientMap", gradientMap);
        }
    }

    // private void Update()
    // {
    //     mat.SetFloat("_TrigPosition", Mathf.Abs(Mathf.Sin(Time.time)));
    //     
    // }

    public void StartWaveEffect()
    {
        print("Wave effect started");
        StartCoroutine(TriggerShaderCoroutine());
    }
    
    private IEnumerator TriggerShaderCoroutine()
    {
        var time = 0f;
        var length = SequencerController.Instance.NoteLength;
        while (time < length)
        {
            targetMaterial.SetFloat("_TrigPosition", time/length);
            time += Time.deltaTime;
            yield return null;
        }
        targetMaterial.SetFloat("_TrigPosition", 0);
        yield return null;
    }

}
