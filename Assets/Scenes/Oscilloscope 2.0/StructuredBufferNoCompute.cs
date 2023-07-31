using System.Collections;
using System.Collections.Generic;
using Lasp;
using UnityEngine;

public class StructuredBufferNoCompute : MonoBehaviour
{
    [SerializeField]
    private ComputeShader computeShader;
        [SerializeField]  private AudioLevelTracker _audioStream;
        private  const int sampleSize = 512;
        private AudioSamples buffer;
    //The same struct in Shader, an array of sampleSize floats
    struct myObjectStruct
    {
        public float objPosition;
    };

    public Material Mat;
    private ComputeBuffer _computeBuffer;
    private myObjectStruct[] _mos;
    // public Transform[] _myObjects;
    
    [SerializeField] [Range(0f, 10f)] private float lineWidth = 2f;
    [SerializeField] [Range(0, 1)] private float ampRange;
    [SerializeField] private Color lineColor = Color.white;
    [SerializeField] private float shrinkFactor = 1;

    // Use this for initialization
    void Start ()
    {
        Mat.SetFloat("lineWidth", lineWidth);
        Mat.SetFloat("shrinkFactor", shrinkFactor);
        Mat.SetFloat("ampRange", ampRange);
        Mat.SetVector("lineColor", lineColor);
        buffer = new AudioSamples(sampleSize);
        _mos = new myObjectStruct[sampleSize];

        //Initiate buffer
        _computeBuffer = new ComputeBuffer(sampleSize,sizeof(float)); //(3)*4bytes in myObjectStruct
        Mat.SetBuffer("audioData", _computeBuffer);
    }
	
	// Update is called once per frame
	void Update ()
    {
        buffer.Push(_audioStream.audioDataSlice);
        buffer.ZSync();
        if (buffer.IsFull) RunShader();
    }

    public void RunShader()
    {
        var samples = buffer.GetSamples();
        for (int i = 0; i < sampleSize; i++)
        {
            _mos[i].objPosition = samples[i];
           // Debug.Log(i+" "+_myObjects[i].position);
        }

        //Set buffer
        _computeBuffer.SetData(_mos);

        //Assign buffer to unlit shader
        Mat.SetBuffer("audioData", _computeBuffer);
    }

    void OnDestroy()
    {
        //Clean Buffer
        _computeBuffer.Release();
    }
}


