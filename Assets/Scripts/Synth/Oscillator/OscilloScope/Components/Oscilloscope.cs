using System;
using Lasp;
using Synth_Variables;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.XR;
using Utils;

// namespace Synth.Oscillator.OscilloScope.Components
// {
//     public class Oscilloscope : MonoBehaviour
//     {
//         #region Inspector
//         
//         [Header("Shader Parameters")] 
//         [Tooltip("Adjust this to increase/decrease the detail")]
//         [SerializeField] private  int sampleSize = 512;
//         [SerializeField] [Range(0f, 10f)] private float lineWidth = 3f;
//         [SerializeField] [Range(0, 1)] private float amplitudeBounds;
//         [SerializeField] private Color lineColor = DesignPalette.Pink;
//
//         
//        [Space(30)] 
//        [Header("Components & Monitoring Parameters")] 
//         [SerializeField] private ComputeShader computeShader;
//         [SerializeField] Material material;
//         [SerializeField] private FloatVariable PeakAmplitude;
//         
//         #endregion
//
//         #region Fields
//
//         private AudioLevelTracker _reaktorStream;
//     
//         private SimplerAudioBuffer _audioBuffer;
//
//         private ComputeBuffer _computeBuffer;
//         
//         private  RenderTexture _renderTex;
//
//         private int _kernel;
//         
//         #endregion
//
//         #region MonoBehaviour
//
//         private void OnValidate()
//         {
//             computeShader.SetFloat("lineWidth", lineWidth);
//             computeShader.SetFloat("ampRange", amplitudeBounds);
//             computeShader.SetVector("lineColor", lineColor);
//         }
//
//         private void SetupComputeShader()
//         {
//             _kernel = computeShader.FindKernel("DrawAudioWaveform");
//             computeShader.SetInt("width", sampleSize);
//             computeShader.SetInt("height", sampleSize);
//             computeShader.SetFloat("lineWidth", lineWidth);
//             computeShader.SetFloat("maxAmp", 0);
//             computeShader.SetFloat("minAmp", 0);
//         }
//         
//         private void SetupRenderTexture()
//         {
//             _renderTex = new RenderTexture(sampleSize, sampleSize, 24)
//             {
//                 enableRandomWrite = true
//             };
//             _renderTex.Create();
//         }
//         
//         
//         void Awake()
//         {
//             _reaktorStream = GetComponent<AudioLevelTracker>();
//             _audioBuffer = new SimplerAudioBuffer(sampleSize, PeakAmplitude);
//             _computeBuffer = new ComputeBuffer(sampleSize, sizeof(float));
//             
//             SetupComputeShader();
//             SetupRenderTexture();
//             computeShader.SetTexture(_kernel, "Result", _renderTex);
//             material.SetTexture( "_MainTex", _renderTex);
//         }
//
//         private void RefreshShader()
//         {
//             _audioBuffer.PushAndProcess(_reaktorStream.audioDataSlice);
//             if (_audioBuffer.IsEmpty) return;
//             
//             _computeBuffer.SetData(_audioBuffer.GetBuffer());
//             computeShader.SetBuffer(_kernel, "audioData", _computeBuffer);
//             computeShader.SetFloat("maxAmp", PeakAmplitude.Value);
//             computeShader.SetFloat("minAmp", -PeakAmplitude.Value);
//             
//             computeShader.Dispatch(_kernel, sampleSize / 8, sampleSize / 8, 1);
//         }
//         
//         void FixedUpdate()
//         {
//             if (_reaktorStream == null) return;
//             RefreshShader();
//         }
//
//         private void OnDestroy()
//         {
//             _computeBuffer.Release();
//             _renderTex.Release();
//         }
//
//         #endregion
//
//     }
//     


namespace Synth.Oscillator.OscilloScope.Components
{
    public class Oscilloscope : MonoBehaviour
    {
        #region Inspector
        public TMP_Text debugWindow;
        [Header("Shader Parameters")] [SerializeField] [Tooltip("Adjust this to increase/decrease the detail")]
        private int sampleSize = 512;
        [SerializeField] Material material;
        [SerializeField] [Range(0f, 10f)] private float lineWidth = 2f;
        [SerializeField] private Color lineColor = Color.white;

        [Header("Components & Monitoring Parameters")] 
        [SerializeField]
        private ComputeShader computeShader;

        [SerializeField] private FloatVariable MaxAmp;

        #endregion


        #region Fields

        private AudioLevelTracker _reaktorStream;
    
        private AudioBuffer _audioBuffer;
        private SimplerAudioBuffer _simplerAudioBuffer;

        private ComputeBuffer _buffer;
        [SerializeField] private  RenderTexture _renderTexture;

        private int _kernel;
        Lasp.InputStream _stream;

        #endregion

        #region Properties

// Input stream object 
        AudioLevelTracker Stream
            => (_reaktorStream != null) ? _reaktorStream : null;


        #endregion

        private string DeviceInfo;

        private InputStream FindInputDevice()
        {
            foreach (var device in Lasp.AudioSystem.InputDevices)
            {
                if (device.Name.Contains("BlackHole"))
                {
                    print("found blackhole device");
                    DeviceInfo ="Device: "+ device.Name +" Channels: "+ device.ChannelCount + " Is Valid: " + device.IsValid +
                                " ID: " + device.ID;
                    return Lasp.AudioSystem.GetInputStream(device);
                }
            }

            return null;
        }
        
        public void ChangeDevice(InputStream stream, DeviceDescriptor device)
        {
            DeviceInfo ="Device: "+ device.Name +" Channels: "+ device.ChannelCount + " Is Valid: " + device.IsValid +
                        " ID: " + device.ID;
            _stream = stream;
        }
 
        

        void Awake()
        {
            _simplerAudioBuffer = new SimplerAudioBuffer(sampleSize, MaxAmp);
            // Create a new buffer with the same size as the audio samples
            _stream = FindInputDevice();

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
            computeShader.SetVector("lineColor", lineColor);
            computeShader.SetInt("size", sampleSize);
            computeShader.SetFloat("lineWidth", lineWidth);
            computeShader.SetFloat("amp", 0);
            _buffer = new ComputeBuffer(sampleSize, sizeof(float));

            // Get the id of the kernel
            _kernel = computeShader.FindKernel("CSMain");
            computeShader.SetTexture(_kernel, "Result", _renderTexture);
            material.SetTexture( "_MainTex", _renderTexture);

            // InvokeRepeating("RefreshShader", 0.05f, 0.05f);
        }

        void Update()
        {
            // DrawSpectrum();
            Prepare();
            // if (_audioBuffer.Full)
            if (!_simplerAudioBuffer.IsEmpty)

            {
                RefreshShader();
            }
        }

        private void DebugLasp(NativeSlice<float> slice)
        {
            if (slice.Length < 32)
            {
                debugWindow.text = DeviceInfo+ "\n\n"+ "Slice length = " + slice.Length;
                return;
            }

            string msg = DeviceInfo + "\n\n";
            msg += "Lasp: NormalizedLevel = " + _stream.GetChannelLevel(0) + "\n";
            msg += "MaxAmp: = " + MaxAmp.Value + "\n";
            msg += "Input slice : ";
            
             for (int i=0; i<32; i++)
            {
                msg += slice[i] + " ";
            }
            
            debugWindow.text = msg;
        }
        
        void RefreshShader()
        {
            // Set the buffer data
            var slice = _simplerAudioBuffer.GetBuffer();
            _buffer.SetData(slice);
            // _buffer.SetData(_audioBuffer._CleanBuff);

            // Set the shader parameters
            computeShader.SetBuffer(_kernel, "audioData", _buffer);
 
            MaxAmp.Value = _audioBuffer.GetMaxAmp();
            // MaxAmp.Value = _simplerAudioBuffer.

            computeShader.SetFloat("amp", MaxAmp.Value);

            // Execute the shader
            computeShader.Dispatch(_kernel, sampleSize / 8, sampleSize / 8, 1);
        }
    

        private void Prepare()
        {
            var slice = _stream.GetChannelDataSlice(0);
            DebugLasp(slice);
            _simplerAudioBuffer.PushAndProcess(slice);
            _audioBuffer.Push(slice);
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
    }
}
