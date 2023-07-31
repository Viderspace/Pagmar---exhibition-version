using System;
using System.Collections;
using Inputs;
using Runtime.Kernel.System;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using UnityEngine;
using UnityEngine.UI.Extensions;
using Utils;

public class Cutoff2600HzSequence : MonoBehaviour
{
    public static Cutoff2600HzSequence Instance;
    public ToggleVariable CutoffIsAt2600Hz;
    private UILineRenderer CutoffLine;
    private Task _waitUntilFilterIs2600HzForSecs;
    private SpecialTimelineEvents EventManager;
    public bool SequenceIsRunning { get; private set; }

    private void OnEnable()
    {
        InputManager.PhoneHangup += ResetToDefault;
    }

    private void ResetToDefault()
    {
        SequenceIsRunning = false;
        CutoffLine.color = DesignPalette.Pink;
        CutoffIsAt2600Hz.ValueChanged -= FilterIs2600Hz;
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        CutoffLine = GetComponent<UILineRenderer>();
    }


    public void EnterSequence(SpecialTimelineEvents eventManager)
    {
        SequenceIsRunning = true;
        EventManager = eventManager;
        HoldUntilFilterIs2600HZ();
    }
    
    public void ExitSequence()
    {
        CutoffIsAt2600Hz.ValueChanged -= FilterIs2600Hz;
        EventManager.FilterIs2600Hz();
    }
    
    public void DisableGreenLine()
    {
        SequenceIsRunning = false;
        CutoffLine.color = DesignPalette.Pink;
    }

    public void HoldUntilFilterIs2600HZ()
    {
        TimelineController.Instance.playableDirector.Pause();
        CutoffIsAt2600Hz.ValueChanged += FilterIs2600Hz;
    }

    private void FilterIs2600Hz(bool closeEnough)
    {
        if (closeEnough)
        {
            CutoffLine.color = DesignPalette.Green;
            _waitUntilFilterIs2600HzForSecs = new Task(WaitUntilFilterIs2600HzForSecs());
        }
        else
        {
            CutoffLine.color = DesignPalette.Pink;
            if (_waitUntilFilterIs2600HzForSecs != null && _waitUntilFilterIs2600HzForSecs.Running)
            {
                _waitUntilFilterIs2600HzForSecs.Stop();
                _waitUntilFilterIs2600HzForSecs = null;
            }
        }
    }

    private IEnumerator WaitUntilFilterIs2600HzForSecs()
    {
        float time = 0;

        while (CutoffIsAt2600Hz.Value && time < 2f)
        {
            time += Time.deltaTime;
            yield return null;
        }

        ExitSequence();
        yield return null;
    }


}