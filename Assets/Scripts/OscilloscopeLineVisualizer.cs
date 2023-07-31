using System;
using System.Collections;
using System.Collections.Generic;
using Lasp;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class OscilloscopeLineVisualizer : MonoBehaviour
{
    [SerializeField] private UILineRenderer line;
    [SerializeField]  private AudioLevelTracker _audioStream;
    [SerializeField] private int sampleSize = 512;
    private AudioSamples buffer;

    private Vector2 SetPointPosition(int sampleId, float amp)
    {
        return new Vector2((float)sampleId / sampleSize,  amp);
    }
    
    private void ReadAudio()
    {
        buffer.Push(_audioStream.audioDataSlice);
        buffer.ZSync();
    }

    private void UpdateLine(List<float> samples)
    {
        for (int i = 0; i < sampleSize; i++)
        {
            line.Points[i] = SetPointPosition(i, samples[i]);
        }
        line.SetAllDirty();
    }


    private void EstablishPoints()
    {
        line.Points = new Vector2[sampleSize];
        for (int i = 0; i < sampleSize; i++)
        {
            line.Points[i] = SetPointPosition(i, 0);
        }
    }

    void Start()
    {
        buffer = new AudioSamples(sampleSize);
        EstablishPoints();   
    }

    private void Update()
    {
        ReadAudio();
        
    }

    void FixedUpdate()
    {
        if (buffer.IsFull)
        {
            UpdateLine(buffer.GetSamples());
        }
    }
}


public class AudioSamples
{
    private List<float> _buffer = new List<float>();
    private int _sampleSize;
    private int maxCapacity => _sampleSize * 2;
    private float _maxAmp = 1;
    private float _minAmp = -1;
    private float peak;
    private bool isSilent => peak < 0.001f;
    
    public bool IsFull => _buffer.Count >= maxCapacity;
    public AudioSamples(int sampleSize)
    {
        _sampleSize = sampleSize;
        peak = 0;
    }

    public void PrintDebug(List<float> sample)
    {
        string s = "";
        for (int i = 0; i<sample.Count; i++)
        {
            s += $"({i}){sample[i]},";
        }
        Debug.Log(s);
    }
    
    
    public void Push(NativeSlice<float> data)
    {
        _buffer.AddRange(data);
        if (_buffer.Count > maxCapacity)
        {
            _buffer.RemoveRange(0, _buffer.Count - maxCapacity);
        }
        PeakDetection(_buffer);
       
        
    }

    public void ZSync()
    {
        int i = 0;
        while (i++ < _buffer.Count - 2)
        {
            // Finding the First Zero Crossing
            if (_buffer[i] <= 0 && _buffer[i+1]>= 0) break;
            _buffer.RemoveAt(0);
        }
    }

    private List<float> Normalize(List<float> samples)
    {

        if (isSilent) return samples;
        for (int i = 0; i < samples.Count; i++)
        {

            var sample = (samples[i] < 0 ? Mathf.Max(samples[i] / peak, -1f) : Mathf.Min(samples[i] / peak, 1f)) / 2;
            samples[i] = sample + 0.5f;
        }

        return samples;
    }

    private void PeakDetection(List<float> data)
    {
        float currentPeak = 0;
        foreach (var sample in data)
        {
            if (currentPeak < sample)
            {
                currentPeak = sample;
            }
        }
        peak = currentPeak > peak ? currentPeak : peak;

    }
    
    public List<float> GetSamples()
    {
        var result =  Normalize(_buffer.GetRange(0, _sampleSize));
        PrintDebug(result.GetRange(0,32));
        return result;
    }
}
