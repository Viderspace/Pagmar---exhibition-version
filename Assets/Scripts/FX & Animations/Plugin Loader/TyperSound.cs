
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyperSound : MonoBehaviour
{
    [SerializeField] private AudioClip singleTypeSoundClip;
    [SerializeField] private AudioClip doubleTypeSoundClip;
    [SerializeField] private AudioClip EnterSoundClip;

    private static TyperSound _instance;
    public static TyperSound Instance => _instance;
    private int typeCount = 0;
    private AudioSource singleTypeSound;
    private AudioSource doubleTypeSound;
    private AudioSource EnterSound;
    

    
    private void GenerateAudioSources(){
        singleTypeSound = gameObject.AddComponent<AudioSource>();
        singleTypeSound.clip = singleTypeSoundClip;
        singleTypeSound.volume = .3f;
        
        EnterSound = gameObject.AddComponent<AudioSource>();
        EnterSound.volume = .3f;
        EnterSound.clip = EnterSoundClip;
        
        doubleTypeSound = gameObject.AddComponent<AudioSource>();
        doubleTypeSound.volume = .3f;
        doubleTypeSound.clip = doubleTypeSoundClip;
    }

    private void Awake()
    {
        // This is a singleton class
        if (_instance == null)
        {
            _instance = this;
            GenerateAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void AdjustPitch(char letter)
    {
        if (letter == ' ' || letter == '\n')
        {
            singleTypeSound.pitch = 1f;
        }
        else
        {
            singleTypeSound.pitch += 0.005f;
        }
    }
    
    private float _sinceCharEntered = 0f;
    private float stopTypeTime = 0.05f;

    private void FixedUpdate()
    {
        _sinceCharEntered += Time.deltaTime;
        if (_sinceCharEntered > stopTypeTime && doubleTypeSound.isPlaying)
        {
            doubleTypeSound.loop = false;
            doubleTypeSound.Stop();
            typeCount = 0;
        }
        
        if (typeCount == 1)
        {
            typeCount--;
            singleTypeSound.Play();
        }
        if (typeCount == 2)
        {
            typeCount -= 2;
            doubleTypeSound.Play();
        }

        if (typeCount > 2 && !doubleTypeSound.isPlaying)
        {
            doubleTypeSound.loop = true;
            doubleTypeSound.Play();
        }


    }

    private void PlayChar()
    {
        typeCount++;
        _sinceCharEntered = 0;
    }


    public void PlayTypeSound(char letter)
    {
        switch (letter)
        {
            case '\n':
                EnterSound.Play();
                return;
            default:
                PlayChar();
                return;
        }
    }

}