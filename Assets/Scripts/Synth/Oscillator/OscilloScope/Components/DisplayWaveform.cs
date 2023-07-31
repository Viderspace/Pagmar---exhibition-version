using Synth.Oscillator.OscilloScope.Components;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class DisplayWaveform : MonoBehaviour
{
    public OscilloscopeController oscilloscopeController;
    private RawImage rawImage;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        rawImage.texture = oscilloscopeController.GetRenderTexture();
    }

    void Update()
    {
        var tex = oscilloscopeController.GetRenderTexture();
        if (tex!= null)
        {
            rawImage.texture = tex;
        }
    }
}