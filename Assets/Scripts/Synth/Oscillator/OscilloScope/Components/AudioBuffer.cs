using System;
using System.Linq;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;

//
// Burst-optimized variant of the Cooley–Tukey FFT
//
sealed class AudioBuffer : System.IDisposable
{
    #region Public properties

    public bool Full => _CleanBuff.Length >= _N;
    public float[] DataArray => _CleanBuff.ToArray();

    #endregion

    #region IDisposable implementation

    public void Dispose()
    {
        // if (_RawBuff.IsCreated) _RawBuff.Dispose();
        // if (_CleanBuff.IsCreated) _CleanBuff.Dispose();
        _RawBuff.Dispose();
        _CleanBuff.Dispose();
    }

    #endregion

    #region Public methods

    public AudioBuffer(int width)
    {
        _N = width;
        _RawBuff = new NativeArray<float>(_N * 2, Allocator.Persistent,
            NativeArrayOptions.UninitializedMemory);

        _CleanBuff = new NativeArray<float>(_N, Allocator.Persistent,
            NativeArrayOptions.UninitializedMemory);

        // _RawBuff = PersistentMemory.New<float>(_N * 2);
        // _CleanBuff = PersistentMemory.New<float>(_N);
    }

    // Push audio data to the FIFO buffer.
    public void Push(NativeSlice<float> data)
    {
        var length = data.Length;
        var N = _N * 2;

        if (length == 0) return;

        if (length < N)
        {
            // The data is smaller than the buffer: Dequeue and copy
            var part = N - length;
            NativeArray<float>.Copy(_RawBuff, N - part, _RawBuff, 0, part);

            data.CopyTo(_RawBuff.GetSubArray(part, length));
        }
        else
        {
            // The data is larger than the buffer: Simple fill
            data.Slice(length - N).CopyTo(_RawBuff);
        }
    }

    // Analyze the input buffer to find the first zero crossing.
    public void ZSync()
    {
        int i = 0;
        while (i++ < _N - 2)
        {
            // Finding the First Zero Crossing
            if (_RawBuff.ElementAtOrDefault(i) <= 0 && _RawBuff.ElementAtOrDefault(i + 1) >= 0) break;
        }

        _CleanBuff.CopyFrom(_RawBuff.GetSubArray(i, _N));
    }

    public Vector2 GetMinMaxAmp()
    {
        float maxAmp = 0;
        float minAmp = 0;

        for (int i = 0; i < _N - 1; i++)
        {
            maxAmp = Math.Max(_CleanBuff.ElementAtOrDefault(i), maxAmp);
            minAmp = Math.Min(_CleanBuff.ElementAtOrDefault(i), minAmp);
        }

        return new Vector2(minAmp, maxAmp);
    }

    public float GetMaxAmp()
    {
        float maxAmp = 0;
        float minAmp = 0;

        for (int i = 0; i < _N - 1; i++)
        {
            maxAmp = Math.Max(_CleanBuff.ElementAtOrDefault(i), maxAmp);
            minAmp = Math.Min(_CleanBuff.ElementAtOrDefault(i), minAmp);
        }

        return maxAmp - minAmp;
    }

    #endregion

    #region Private members

    readonly int _N;
    public NativeArray<float> _RawBuff;
    public NativeArray<float> _CleanBuff;

    #endregion

    // public class PersistentMemory
    // {
    //     public NativeArray<T> New<T>(int size) where T : unmanaged
    //         => new NativeArray<T>(size, Allocator.Persistent,
    //             NativeArrayOptions.UninitializedMemory);
    // }

// Math utility functions
    static class MathUtils
    {
        // Decibel (full scale) calculation
        // Reference level (full scale sin wave) = 1/sqrt(2)
        public static float dBFS(float p)
            => 20 * math.log10(p / 0.7071f + 1.5849e-13f);

        public static float2 dBFS(float2 p)
            => 20 * math.log10(p / 0.7071f + 1.5849e-13f);
    }
}