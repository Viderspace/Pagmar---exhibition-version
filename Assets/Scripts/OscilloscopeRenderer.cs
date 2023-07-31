using System;
using Lasp;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class OscilloscopeRenderer : MonoBehaviour
{
    static int Resolution = 512;
    public Material targetMaterial;
    public ComputeShader ComputeShader;
    public AudioLevelTracker audioLevelTracker;
    
    [SerializeField] [Range(0f, 10f)] private float lineWidth = 2f;
    [SerializeField] [Range(0, 1)] private float ampRange;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private float shrinkFactor = 1;
    
    private AudioSamples audioSampler;
    private ComputeBuffer _computeBuffer;
    public RenderTexture _resultTexture;

    private void InitShader()
    {
        _resultTexture = new RenderTexture(Resolution, Resolution, 1, GraphicsFormat.R16G16B16A16_SFloat)
        {
            enableRandomWrite = true
        };
        _resultTexture.Create();
        ComputeShader.SetTexture(0, "Result", _resultTexture);

        _computeBuffer = new ComputeBuffer(Resolution, sizeof(float));
        // ComputeShader.SetBuffer(0, "audioData", _computeBuffer);
        SetShaderProperties();
    }
    
    private void SetShaderProperties()
    {
        ComputeShader.SetFloat("lineWidth", lineWidth);
        ComputeShader.SetFloat("shrinkFactor", shrinkFactor);
        ComputeShader.SetFloat("ampRange", ampRange);
        ComputeShader.SetVector("lineColor", lineColor);
    }

    private void RefreshBuffer()
    {
        audioSampler.Push(audioLevelTracker.audioDataSlice);
        audioSampler.ZSync();
        if (audioSampler.IsFull)
        {
            int kernel = ComputeShader.FindKernel("CSMain");
            _computeBuffer.SetData(audioSampler.GetSamples());
            ComputeShader.SetBuffer(kernel, "audioData", _computeBuffer);
            ComputeShader.SetTexture(kernel, "Result", _resultTexture);
            ComputeShader.Dispatch(kernel, Resolution / 8, Resolution / 8, 1);
            targetMaterial.SetTexture("_MainTex", _resultTexture);

        }
    }

    private void Start()
    {
        audioSampler = new AudioSamples(Resolution);
        InitShader();
    }

    private void Update()
    {
        RefreshBuffer(); 
    }
    
    void OnDestroy()
    {
        //Clean Buffer
        _computeBuffer.Release();
    }
}