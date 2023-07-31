using UnityEngine;

[ExecuteInEditMode]
[RequireComponent(typeof(Camera))]
public class CRT : MonoBehaviour {
     
    public Shader crtShader;
    public Texture image;
    public bool useImage = true;

    [Range(1.0f, 10.0f)]
    public float curvature = 1.0f;

    [Range(1.0f, 100.0f)]
    public float vignetteWidth = 30.0f;

    public float greenOffset = 0.15f;
    public float redBlueOffset =  0.135f;

    private Material crtMat;

    void Start() {
        crtMat ??= new Material(crtShader);
        crtMat.hideFlags = HideFlags.HideAndDontSave;
    }

    void OnRenderImage(RenderTexture source, RenderTexture destination) {
        // float _green_offset;
        // float _red_blue_offset;
                crtMat.SetFloat("_green_offset", greenOffset);
        crtMat.SetFloat("_red_blue_offset", redBlueOffset);
        crtMat.SetFloat("_Curvature", curvature);
        crtMat.SetFloat("_VignetteWidth", vignetteWidth);
        Graphics.Blit(useImage ? image : source, destination, crtMat);
    }
    
}
