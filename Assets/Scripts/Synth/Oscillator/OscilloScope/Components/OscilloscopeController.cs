using Lasp;
using UnityEngine;

namespace Synth.Oscillator.OscilloScope.Components
{
    public class OscilloscopeController : MonoBehaviour
    {
        #region Inspector

        [Header("Shader Parameters")] [SerializeField] [Tooltip("Adjust this to increase/decrease the detail")]
        private int sampleSize = 512;
        [SerializeField] Material material;
        [SerializeField] [Range(0f, 10f)] private float lineWidth = 2f;
        [SerializeField] [Range(0, 1)] private float ampRange;
        [SerializeField] private Color lineColor = Color.white;
        [SerializeField] private float shrinkFactor = 1;

        [Header("Components & Monitoring Parameters")] 
        [SerializeField]
        private ComputeShader computeShader;
        // [field: SerializeReference] private ;

        [field: SerializeReference] private float MaxAmp { get; set; }
        // [field: SerializeReference] private float MinAmp { get; set; }

        #endregion


        #region Fields

        private AudioLevelTracker _reaktorStream;
    
        private AudioBuffer _audioBuffer;

        private ComputeBuffer _buffer;
        [SerializeField] private  RenderTexture _renderTexture;

        private int _kernel;

        #endregion

        #region Properties

// Input stream object 
        AudioLevelTracker Stream
            => (_reaktorStream != null) ? _reaktorStream : null;


        #endregion


        private void OnValidate()
        {
            computeShader.SetFloat("lineWidth", lineWidth);
            computeShader.SetFloat("shrinkFactor", shrinkFactor);
            computeShader.SetFloat("ampRange", ampRange);
            computeShader.SetVector("lineColor", lineColor);
        }


        void Awake()
        {
            // Create a new buffer with the same size as the audio samples

            _reaktorStream = GetComponent<AudioLevelTracker>();
            _audioBuffer = new AudioBuffer(sampleSize);
            // sampleSize = (int)transform.GetComponentInParent<RectTransform>().rect.size.x;
            // print("rect size: " + textureDimensions);
            // Create a new RenderTexture to hold the result
            // _renderTexture = new RenderTexture(sampleSize, sampleSize, 24)
            // {
            //     enableRandomWrite = true
            // };
            _renderTexture.enableRandomWrite = true;
            // _renderTexture.Create();
            computeShader.SetInt("width", _renderTexture.width);
            computeShader.SetInt("height", _renderTexture.height);
            computeShader.SetFloat("lineWidth", lineWidth);
            computeShader.SetFloat("shrinkFactor", shrinkFactor);
            computeShader.SetFloat("maxAmp", 0);
            computeShader.SetFloat("minAmp", 0);
            _buffer = new ComputeBuffer(sampleSize, sizeof(float));

            // Get the id of the kernel
            _kernel = computeShader.FindKernel("DrawAudioWaveform");
            computeShader.SetTexture(_kernel, "Result", _renderTexture);
            material.SetTexture( "_MainTex", _renderTexture);

            // InvokeRepeating("RefreshShader", 0.05f, 0.05f);
        }

        void Update()
        {
            // DrawSpectrum();
            Prepare();
            if (_audioBuffer.Full)
            {
                RefreshShader();
            }
        }

        void PrintSlice(float[] slice)
        {
            print("got array of size " + slice.Length);
            for (int i = 0; i < slice.Length; i++)
            {
                print(slice[i]);
            }
        }

        void RefreshShader()
        {
            // Set the buffer data
            _buffer.SetData(_audioBuffer._CleanBuff);

            // Set the shader parameters
            computeShader.SetBuffer(_kernel, "audioData", _buffer);
            // computeShader.SetTexture(_kernel, "Result", _renderTexture);

            // var minmaxAmp = _audioBuffer.GetMinMaxAmp();
            // MinAmp = minmaxAmp.x;
            // MaxAmp = minmaxAmp.y;
            MaxAmp = _audioBuffer.GetMaxAmp();
            computeShader.SetFloat("maxAmp", MaxAmp);
            computeShader.SetFloat("minAmp",  -MaxAmp);

            // Execute the shader
            computeShader.Dispatch(_kernel, sampleSize / 8, sampleSize / 8, 1);
        }
    

        private void Prepare()
        {
            _audioBuffer.Push(Stream.audioDataSlice);
            _audioBuffer.ZSync();
        }


        void OnDestroy()
        {
            // Release the buffer when we are done
            _audioBuffer.Dispose();
            _buffer.Release();
            _renderTexture.Release();
        }

        public RenderTexture GetRenderTexture()
        {
            return _renderTexture;
        }

        // void DrawSpectrum()
        // {
        //     float[] spectrum = new float[256];
        //
        //     AudioListener.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        //
        //     for (int i = 1; i < spectrum.Length - 1; i++)
        //     {
        //         Debug.DrawLine(new Vector3(i - 1, spectrum[i] + 10, 0), new Vector3(i, spectrum[i + 1] + 10, 0), Color.red);
        //         Debug.DrawLine(new Vector3(i - 1, Mathf.Log(spectrum[i - 1]) + 10, 2),
        //             new Vector3(i, Mathf.Log(spectrum[i]) + 10, 2), Color.cyan);
        //         Debug.DrawLine(new Vector3(Mathf.Log(i - 1), spectrum[i - 1] - 10, 1),
        //             new Vector3(Mathf.Log(i), spectrum[i] - 10, 1), Color.green);
        //         Debug.DrawLine(new Vector3(Mathf.Log(i - 1), Mathf.Log(spectrum[i - 1]), 3),
        //             new Vector3(Mathf.Log(i), Mathf.Log(spectrum[i]), 3), Color.blue);
        //     }
        // }
    }
}