using System;
using System.Collections;
using System.Collections.Generic;
using Scriptable_Objects;
using Synth_Variables.Scripts;
using UnityEngine;
using UnityEngine.UI;

public class SequencerUiSwitchCursorType : MonoBehaviour
{
    [SerializeField] private SequencerModeVariable SequencerMode;
    [SerializeField] private Image DefaultCursor;
    [SerializeField] private Image RecordCursor;

    private void Start()
    {
        SetCursorType(SynthController.SequencerState.Idle);
    }

    private void OnEnable()
    {
        SequencerMode.ValueChanged += SetCursorType;
    }
    
    private void OnDisable()
    {
        SequencerMode.ValueChanged -= SetCursorType;
    }

    private void SetCursorType(SynthController.SequencerState newState)
    {
        switch (newState)
        {
            case SynthController.SequencerState.Idle:
            case SynthController.SequencerState.Running:
                DefaultCursor.enabled = true;
                RecordCursor.enabled = false;
                break;
            case SynthController.SequencerState.Recording:
                DefaultCursor.enabled = false;
                RecordCursor.enabled = true;
                break;
        }
    }

}
