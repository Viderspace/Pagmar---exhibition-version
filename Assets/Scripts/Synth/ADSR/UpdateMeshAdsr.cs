using System;
using Scriptable_Objects;
using Synth_Variables.Adsr;
using Synth.ADSR;
using Synth.Modules.ADSR;
using UnityEngine;

public class UpdateMeshAdsr : MonoBehaviour
{
    private MeshFilter _meshFilter;
    private AdsrVariables globalAdsr;
    private SynthController SynthController => Singleton.Instance.SynthController;
    
    public void Init(AdsrVariables adsr)
    {
        globalAdsr = adsr;
        globalAdsr.OnAdsrValuesChanged += UpdateMesh;

    }

    private void Start()
    {
        _meshFilter = GetComponent<MeshFilter>();
        UpdateMesh(SynthController.GlobalAdsr);
    }

    private void OnEnable()
    {
        // globalAdsr.OnAdsrValuesChanged += UpdateMesh;
        // AdsrController.NotifyWhenAdsrValuesChanged += UpdateMesh;
    }


    private void OnDisable()
    {
        globalAdsr.OnAdsrValuesChanged -= UpdateMesh;
        // AdsrController.NotifyWhenAdsrValuesChanged -= UpdateMesh;
    }

    private void UpdateMesh(AdsrVariables adsr)
    {
        // Set Attack, Decay & Sustain x position
        var normAttack = adsr.Attack * 0.33f;
        var normDecay = adsr.Decay * 0.34f;
        SetAttack(normAttack);
        SetDecay(normAttack + normDecay);
        SetSustain(adsr.Sustain);
        SetRelease(0.67f + adsr.Release * 0.33f);
    }

    private void SetAttack(float attack)
    {
        var mesh = _meshFilter.mesh;
        var vertices = mesh.vertices;
        vertices[1].x = attack;
        vertices[2].x = attack;
        mesh.vertices = vertices;
    }

    private void SetDecay(float decay)
    {
        var mesh = _meshFilter.mesh;
        var vertices = mesh.vertices;
        vertices[3].x = decay;
        vertices[4].x = decay;
        vertices[5].x = 0.67f;
        mesh.vertices = vertices;
    }

    private void SetSustain(float sustain)
    {
        var mesh = _meshFilter.mesh;
        var vertices = mesh.vertices;
        vertices[3].y = sustain;
        vertices[5].y = sustain;
        mesh.vertices = vertices;
    }

    private void SetRelease(float release)
    {
        var mesh = _meshFilter.mesh;
        var vertices = mesh.vertices;
        vertices[6].x = release;
        mesh.vertices = vertices;
    }
}