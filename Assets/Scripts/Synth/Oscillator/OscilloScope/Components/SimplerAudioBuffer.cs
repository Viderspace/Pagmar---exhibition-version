using System.Collections.Generic;
using Synth_Variables;
using Unity.Collections;
using UnityEngine;

namespace Synth.Oscillator.OscilloScope.Components
{
    public class SimplerAudioBuffer
    {
        private int buffersize;
        private List<float> data;
        private FloatVariable PeakAmp { get; set; }

        public int MaxCap => buffersize * 3;
        public bool IsFull => data.Count >= MaxCap -1;
        public bool IsEmpty => data.Count < buffersize;

        private void FindPeakAmplitude()
        {
            float max = 0;
            foreach (var sample in data)
            {
                max = Mathf.Abs(sample) > max ? sample : max;
            }
            
            if (PeakAmp.Value == 0) PeakAmp.Value = max;
            PeakAmp.Value = 0.9f*PeakAmp.Value + 0.1f*max;
        }
        
        public SimplerAudioBuffer(int size, FloatVariable peakAmplitudeRef)
        {
            buffersize = size;
            PeakAmp = peakAmplitudeRef;
            data = new List<float>();
        }
        
        public void PushAndProcess(NativeSlice<float> samples)
        {
            Push(samples);
            if (IsEmpty) return;
            ZSync();
            
            FindPeakAmplitude();
        }



        void PrintSamplesDebug(List<float> samples)
        {
            var msg = "";
            foreach (var sample in samples)
            {
                msg += $"{sample},";
            }
            UnityEngine.Debug.Log(msg);
        }
        
        public float[] GetBuffer()
        {
            if (IsEmpty)
            {
                // filling the buffer with zeros
                data.AddRange(new float[buffersize-data.Count]);
            }

            return data.GetRange(0, buffersize).ToArray();
        }

   
        // Finding the First Zero Crossing In the buffer
        public void ZSync()
        {
            int i = 0;
            
            while (i++ < buffersize - 2)
            {
                if (data[i] <= 0 && data[i + 1] >= 0)
                {
                    data.RemoveRange(0, i);
                    break;
                }
            }
        }

        private void Push(NativeSlice<float> samples)
        {
            data.AddRange(samples);
            if (data.Count > MaxCap)
            {
                data.RemoveRange(0, data.Count - MaxCap);
            }
        }
    }
}