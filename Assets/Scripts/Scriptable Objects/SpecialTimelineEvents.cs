using System.Collections;
using Inputs;
using Runtime.Kernel.System;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using Synth.ADSR;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Utils;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "SpecialTimelineEvents")]
    public class SpecialTimelineEvents : ScriptableObject
    {
        [SerializeField] private SequencerModeInferredVariable SequencerIsInRecordMode;

        public void HoldUntilKeypadIsPressed()
        {
            TimelineController.Instance.playableDirector.Pause();
            InputManager.KeypadButtonReleased += KeypadPressed;
        }

        private void KeypadPressed(Keypad key)
        {
            InputManager.KeypadButtonReleased -= KeypadPressed;
            TimelineController.Instance.ResumeTimeline();
        }

        public void HoldUntilRedialKeyIsPressed()
        {
            TimelineController.Instance.playableDirector.Pause();
            InputManager.KeypadClearCurrentStep += ReDialKeyWasPressed;
        }

        private void ReDialKeyWasPressed()
        {
            InputManager.KeypadClearCurrentStep -= ReDialKeyWasPressed;
            TimelineController.Instance.ResumeTimeline();
        }

        public void HoldUntilRecordButtonIsPressed()
        {
            TimelineController.Instance.playableDirector.Pause();
            SequencerCodeValidator.Instance.Enter();
            SequencerIsInRecordMode.ValueChanged += RecordButtonPressed;
        }

        private void RecordButtonPressed(bool isInRecordMode)
        {
            if (isInRecordMode)
            {
                SequencerIsInRecordMode.ValueChanged -= RecordButtonPressed;
                TimelineController.Instance.ResumeTimeline();
            }
        }

        public void HoldUntilFilterIsEnabled()
        {
            TimelineController.Instance.playableDirector.Pause();
            Singleton.Instance.SynthController.FilterOnOffSwitch.ValueChanged += FilterIsEnabled;
        }

        private void FilterIsEnabled(bool isEnabled)
        {
            if (isEnabled)
            {
                Singleton.Instance.SynthController.FilterOnOffSwitch.ValueChanged -= FilterIsEnabled;
                TimelineController.Instance.ResumeTimeline();
            }
        }
        
        
        public void HoldUntilFilterIs2600HZ()
        {
            TimelineController.Instance.playableDirector.Pause();
            Cutoff2600HzSequence.Instance.EnterSequence(this);
        }

        public void FilterIs2600Hz()
        {
            TimelineController.Instance.ResumeTimeline();
        }
        
        public void DisableGreenCutoffLine()
        {
            Cutoff2600HzSequence.Instance.DisableGreenLine();
        }

        public void EnableAdsrOn()
        {
            Singleton.Instance.SynthController.AdsrOnOffSwitch.Value = true;
        }

        public void HoldUntilAdsrIsMatched()
        {
            TimelineController.Instance.playableDirector.Pause();
            BlueADSRGuideLines.Instance.EnterSequence();
        }
        
        public void AdsrIsMatched()
        {
            TimelineController.Instance.ResumeTimeline();
        }
    }
}