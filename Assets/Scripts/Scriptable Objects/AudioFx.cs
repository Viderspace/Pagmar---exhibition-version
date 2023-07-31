using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Audio;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "AudioFx", menuName = "AudioFx", order = 0)]
    public class AudioFx : ScriptableObject
    {
        [SerializeField] private AudioMixer SfxMixer;
        public AudioMixer SFXMixer => SfxMixer;
        public AudioMixerGroup audioMixerGroup => SfxMixer.FindMatchingGroups("SFX")[0];

        [SerializeField] [Range(0, 1)] public float keystrokeLatency = 0.0f;


        [SerializeField] private AudioClip ringingPhoneSoundClip;
        [SerializeField] private AudioClip singleTypeSoundClip;
        [SerializeField] private AudioClip doubleTypeSoundClip;
        [SerializeField] private AudioClip enterSoundClip;
        [SerializeField] private AudioClip digitalKeystrokesSequence;
        [SerializeField] private AudioClip glitchSoundClip;
        [SerializeField] private AudioClip eventNotificationSoundClip;
        [SerializeField] private AudioClip backgroundProcessSoundClip;
        [SerializeField] private AudioClip smallImpact;
        [SerializeField] private AudioClip pluginArm;


        [SerializeField] [Range(0, 1)] private float fxVolume = 0.3f;
        [SerializeField] [Range(0, 1)] public float keystrokeVolume = 0.3f;

        public float GlitchDuration => glitchSoundClip.length * 0.9f;

        public AudioClip RingingPhone => ringingPhoneSoundClip;
        public AudioClip SingleKeyStroke => singleTypeSoundClip;
        public AudioClip DoubleKeyStroke => doubleTypeSoundClip;
        public AudioClip InsertNewline => enterSoundClip;

        public AudioClip DigitalKeystrokesSequence => digitalKeystrokesSequence;

        private readonly Dictionary<FX, PlayFunction> _playFunctions = new();
        private readonly Dictionary<FX, AudioClip> _audioClips = new();

        public enum FX
        {
            RingingPhone,
            SingleKeystroke,
            DoubleKeystroke,
            InsertNewLine,
            DigitalKeystrokesSequence, // deprecated
            Glitch,
            EventNotification,
            BackgroundProcess,
            SmallImpact,
            PluginArm
        }

        private void OnValidate()
        {
            SetAudioSourceVolume();
        }

        private void SetAudioSourceVolume()
        {
            if (AudioFxPlayer.Instance == null) return;
            AudioFxPlayer.Instance.SetAudioSourceVolume(fxVolume);
        }


        public void AddKeyStroke(int n = 1)
        {
            if (AudioFxPlayer.Instance == null) return;
            AudioFxPlayer.Instance.GetKeystrokeWithLatency(ApplyKeystrokeLatency(n));
            // Play(FX.SingleKeystroke);
        }

        IEnumerator ApplyKeystrokeLatency(int n = 1)
        {
            yield return new WaitForSeconds(keystrokeLatency);
            _insertKeystrokeToBuffer?.Invoke(n);
            yield return null;
        }


        public delegate void PlayFunction(FX fx, bool on = true);

        public delegate void InsertKeystrokeToBuffer(int n = 1);

        private InsertKeystrokeToBuffer _insertKeystrokeToBuffer;


        public AudioClip BindAudioClip(PlayFunction sceneObjPlayFunction, FX clipType)
        {
            if (_audioClips.Count == 0) Init();
            _playFunctions.Add(clipType, sceneObjPlayFunction);
            return _audioClips[clipType];
        }

        public void BindBuffer(InsertKeystrokeToBuffer insertKeystrokeToBuffer)
        {
            _insertKeystrokeToBuffer = insertKeystrokeToBuffer;
        }

        public void Init()
        {
            _audioClips.Add(FX.RingingPhone, RingingPhone);
            _audioClips.Add(FX.SingleKeystroke, SingleKeyStroke);
            _audioClips.Add(FX.DoubleKeystroke, DoubleKeyStroke);
            _audioClips.Add(FX.InsertNewLine, InsertNewline);
            _audioClips.Add(FX.DigitalKeystrokesSequence, digitalKeystrokesSequence);
            _audioClips.Add(FX.Glitch, glitchSoundClip);
            _audioClips.Add(FX.EventNotification, eventNotificationSoundClip);
            _audioClips.Add(FX.BackgroundProcess, backgroundProcessSoundClip);
            _audioClips.Add(FX.SmallImpact, smallImpact);
            _audioClips.Add(FX.PluginArm,pluginArm );

        }


        public void Play(FX fx, bool on = true)
        {
            if (_audioClips == null || _audioClips.Count == 0) return;
            _playFunctions[fx](fx, on);
        }
    }
}