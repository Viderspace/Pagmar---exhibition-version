using System.Collections;
using System.Collections.Generic;
using Inputs.Scriptable_Objects;
using Scriptable_Objects;
using Synth_Variables.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.TextCore.Text;
using Utils;

public class SequencerUiAnimateStateTag : MonoBehaviour
{
    [SerializeField] SequencerModeVariable sequencerMode;
    [SerializeField] private TMP_Text text;
    [SerializeField] private TMP_Text blinkingRedDot;

    [SerializeField] private string idleText = "Ready";
    [SerializeField] private string runningText = "Running";
    [SerializeField] private string recordingText = "Recording";
    
    [SerializeField] private float blinkSpeed = 0.5f;
    
    private bool recordBlinkIsActive = false;
    private Coroutine blinkCoroutine;
    
    //   ● ○ ◯ ◔ ◕ ◶ ◌ ◉ ◎ ◦ ◆ ◇

    private void SwitchState(SynthController.SequencerState newState)
    {
        text.text = SetStylizedText(newState);
        SetRecordBlink(newState);
    }

    private void SetRecordBlink(SynthController.SequencerState state)
    {
        if (recordBlinkIsActive && state != SynthController.SequencerState.Recording)
        {
            // Stop blink animation
            StopCoroutine(blinkCoroutine);
            blinkingRedDot.enabled = false;
            recordBlinkIsActive = false;
            return;
        }
        
        if (!recordBlinkIsActive && state == SynthController.SequencerState.Recording)
        {
            // Start blink animation
            blinkingRedDot.enabled = true;
            blinkCoroutine = StartCoroutine(BlinkRecordDot());
            recordBlinkIsActive = true;
            return;
        }
    }

    private string SetStylizedText(SynthController.SequencerState state)
    {
        switch (state)
        {
            case SynthController.SequencerState.Idle:
                return $"<color=#{DesignPalette.WhiteYellowishHex}>{idleText}</color>";

            case SynthController.SequencerState.Running:
                return  $"<color=#{DesignPalette.GreenHex}>{runningText}</color>";
            
            case SynthController.SequencerState.Recording:
                return  $"<color=#{DesignPalette.RedHex}>{recordingText}</color>";
        }

        return "";
    }

    public LedController recordLed;
    
    private IEnumerator BlinkRecordDot()
    {
        bool state = false;
        while (true)
        {
            yield return new WaitForSeconds(blinkSpeed);
            state = !state;
            blinkingRedDot.enabled = state;
            recordLed.State = state;
        }
        // ReSharper disable once IteratorNeverReturns
    }

    private void OnEnable()
    {
        sequencerMode.ValueChanged += SwitchState;
        blinkingRedDot.enabled = false;
    }

    private void OnDisable()
    {
        sequencerMode.ValueChanged -= SwitchState;
    }
}