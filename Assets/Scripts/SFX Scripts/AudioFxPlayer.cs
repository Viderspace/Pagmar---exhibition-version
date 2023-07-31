using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;
using FX = Scriptable_Objects.AudioFx.FX;

public class AudioFxPlayer : MonoBehaviour
{
    private readonly Dictionary<FX, AudioSource> _audioSources = new();
    private AudioFx AudioFx;
    private KeystrokesBuffer _buffer;

    

    private void Init()
    {
        AudioFx = Singleton.Instance.AudioFx;
        foreach (var fx in Enum.GetValues(typeof(FX)))
        {
            var audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = AudioFx.BindAudioClip(PlayFx, (FX) fx);
            audioSource.outputAudioMixerGroup = AudioFx.audioMixerGroup;
            _audioSources.Add((FX) fx, audioSource);
        }
        _buffer = new KeystrokesBuffer(_audioSources[FX.SingleKeystroke]);
        AudioFx.BindBuffer(_buffer.AddKeystroke);
        // Singleton.Instance.AudioFx.Init(this);
    }
    
    
    public static AudioFxPlayer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("AudioFxPlayer already exists!");
        }
        
        Instance = this;
    }


    private void Start()
    {
        Init();
        SetAudioSourceVolume(AudioFx.keystrokeVolume);
        
    }

    public void PlayFx(FX fx, bool on = true)
    {
        if (on) _audioSources[fx].Play();
        else _audioSources[fx].Stop();
    }

    private void Update()
    {
        _buffer.Tick(Time.deltaTime);
    }

    public void SetAudioSourceVolume(float fxVolume)
    {
        foreach (var audioSource in _audioSources)
        {
            audioSource.Value.volume = fxVolume;
        }

        _audioSources[FX.SingleKeystroke].volume = AudioFx.keystrokeVolume;
    }
    
    public void GetKeystrokeWithLatency(IEnumerator latencyCoroutine)
    {
        StartCoroutine(latencyCoroutine);
    }
}

internal class KeystrokesBuffer
{
    private AudioFx AudioFx => Singleton.Instance.AudioFx;
    public int TypeCount;
    private AudioSource _singleKeystroke;

    

    public KeystrokesBuffer(AudioSource singleKeystroke)
    {
        _singleKeystroke = singleKeystroke;
    }




    public void Tick(float deltaTime)
    {


        if (TypeCount > 0)
        {
            TypeCount--;
            AudioFx.Play(FX.SingleKeystroke);
        }
    }


    #region Keystrokes Methods

    public void RollKeyStrokes(bool on)
    {
        if (on)
        {
            _singleKeystroke.loop = true;
            _singleKeystroke.Play();
        }
        else
        {
            _singleKeystroke.loop = false;
            _singleKeystroke.Stop();  
        }
    }

    public bool IsKeyStrokesRolling()
    {
        return _singleKeystroke.isPlaying;
    }

    #endregion

    public void AddKeystroke(int n = 1)
    {
        TypeCount += n;
    }
}