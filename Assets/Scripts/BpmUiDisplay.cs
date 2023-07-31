using System;
using System.Collections;
using System.Collections.Generic;
using Synth_Variables;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;
using Utils;

public class BpmUiDisplay : MonoBehaviour
{
    [SerializeField] IntVariable BpmVariable;
    [SerializeField] private TMP_Text text;

    [SerializeField] private string linePrefix = "BPM.Value: ";
    [SerializeField] private float highlightTime = 0.5f;
    private Color DefaultColor = DesignPalette.WhiteYellowish;
    private float _timer = 0f;
    private Task _highlightTask;

    private void Start()
    {
        text.color = DesignPalette.WhiteYellowish;
    }

    private void OnEnable()
    {
        BpmVariable.ValueChanged += UpdateBpm;
    }

    private void OnDisable()
    {
        BpmVariable.ValueChanged -= UpdateBpm;
    }

    private void UpdateBpm(int newValue)
    {
        text.text = linePrefix + newValue;
        HighlightOnValueChanged();
    }

    private void HighlightOnValueChanged()
    {
        if (_highlightTask != null && _highlightTask.Running)
        {
            _timer = highlightTime;
            return;
        }
        _highlightTask = new Task(Highlight());
    }

    private IEnumerator Highlight()
    {
        _timer = highlightTime;
        Color textColor = text.color;
        text.color = Color.white;
        while (_timer > 0f)
        {
            _timer -= Time.deltaTime;
            yield return null;
        }
        text.color = textColor;
    }
}