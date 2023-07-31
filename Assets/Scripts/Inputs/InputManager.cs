using System;
using Scriptable_Objects;
using UnityEngine;

namespace Inputs
{
    /**
     * Routes all Input events to the appropriate notifier functions
     */
    public class InputManager : MonoBehaviour
    {
        private InputManager _instance;
        public InputManager Instance => _instance;

        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            else _instance = this;
        }

        #region Notifiers API

        // Phone ctrls
        public static event Action PhonePickup;
        public static event Action PhoneHangup;

        public static event Action<float> GlitchEffectStart;

        // Adsr ctrls
        public static event Action<bool> UpdateAdsrIsActive;
        public static event Action<float> UpdateAttack;
        public static event Action<float> UpdateDecay;
        public static event Action<float> UpdateSustain;
        public static event Action<float> UpdateRelease;

        // Oscillator ctrls
        public static event Action<SynthController.PitchMode> SetPitchMode;
        public static event Action<float> UpdateMasterVolume;
        // public static event Action<int> UpdateOctave;
        public static event Action<float> UpdateOctave;

        public static event Action<SynthController.WaveShape> UpdateWaveshape;

        // Filter ctrls
        public static event Action<float> UpdateCutoffPos;
        // public static event Action<bool> UpdateFilterIsActive;
        public static event Action UpdateFilterIsActive;


        // Sequencer aka dialer ctrls
        public static event Action<bool> UpdateSequencerIsActive;
        public static event Action UpdateSequencerPlayPauseButtonPressed;
        public static event Action UpdateSequencerRecordButtonPressed;
        // public static event Action<int> UpdateBpm;
        public static event Action<float> UpdateBpm;

        public static event Action UpdateLeftArrowPressed;
        public static event Action UpdateRightArrowPressed;
        public static event Action UpdateClearButtonPressed;

        // Keypad ctrls
        public static event Action<Keypad> KeypadButtonPressed;
        public static event Action<Keypad> KeypadButtonReleased;
        
        public static event Action KeypadClearCurrentStep; // The Redial button
        

        #endregion

        #region Invokers

        #region Oscillator

        public static void OnSetPitchMode(SynthController.PitchMode newMode)
        {
            SetPitchMode?.Invoke(newMode);
        }

        public static void OnUpdateWaveshape(SynthController.WaveShape value)
        {
            UpdateWaveshape?.Invoke(value);
        }

        public static void OnUpdateMasterVolume(float value)
        {
            UpdateMasterVolume?.Invoke(value);
        }

        public static void OnUpdateOctave(int value)
        {
            UpdateOctave?.Invoke(value);
        }
        public static void OnUpdateOctave(float value)
        {
            UpdateOctave?.Invoke(value);
        }

        #endregion

        #region ADSR

        public static void OnUpdateAdsrIsActive(bool obj)
        {
            UpdateAdsrIsActive?.Invoke(obj);
        }

        public static void OnUpdateAttack(float value)
        {
            UpdateAttack?.Invoke(value);
        }

        public static void OnUpdateDecay(float value)
        {
            UpdateDecay?.Invoke(value);
        }

        public static void OnUpdateSustain(float value)
        {
            UpdateSustain?.Invoke(value);
        }

        public static void OnUpdateRelease(float value)
        {
            UpdateRelease?.Invoke(value);
        }

        #endregion

        #region Filter

        // public static void OnActivateFilter(bool value)
        // {
        //     UpdateFilterIsActive?.Invoke(value);
        // }
        public static void OnActivateFilter()
        {
            UpdateFilterIsActive?.Invoke();
        }

        public static void OnUpdateCutoffPos(float value)
        {
            UpdateCutoffPos?.Invoke(value);
        }

        #endregion

        #region Sequencer

        public static void OnUpdateSequencerIsActive(bool isVisible)
        {
            UpdateSequencerIsActive?.Invoke(isVisible);
        }

        public static void OnSequencerPlayPauseButtonPressed()
        {
            UpdateSequencerPlayPauseButtonPressed?.Invoke();
        }
        
        public static void OnSequencerRecordButtonPressed()
        {
            UpdateSequencerRecordButtonPressed?.Invoke();
        }
        
        public static void OnKeypadClearCurrentStep()
        {
            KeypadClearCurrentStep?.Invoke();
        }

        public static void OnUpdateBpm(int value)
        {
            UpdateBpm?.Invoke(value);
        }
        
        public static void OnUpdateBpm(float rawValue)
        {
            UpdateBpm?.Invoke(rawValue);
        }
    

        public static void OnUpdateClearButtonPressed()
        {
            UpdateClearButtonPressed?.Invoke();
        }

        public static void OnUpdateLeftArrowPressed()
        {
            UpdateLeftArrowPressed?.Invoke();
        }

        public static void OnUpdateRightArrowPressed()
        {
            UpdateRightArrowPressed?.Invoke();
        }

        #endregion

        #region Keypad

        public static void OnKeypadButtonPressed(Keypad value)
        {
            KeypadButtonPressed?.Invoke(value);
        }

        public static void OnKeypadButtonReleased(Keypad value)
        {
            KeypadButtonReleased?.Invoke(value);
        }

        #endregion

        #region Phone
        
        public static void OnGlitchEffectStart()
        {
            GlitchEffectStart?.Invoke(Singleton.Instance.AudioFx.GlitchDuration);
        }

        public static void OnPhonePickup()
        {
            PhonePickup?.Invoke();
        }

        public static void OnPhoneHangup()
        {
            PhoneHangup?.Invoke();
        }

        #endregion

        #endregion

        public static void OnUpdateReverb(float value)
        {
        }

        public static void OnUpdateDelay(float value)
        {
        }
    }
}