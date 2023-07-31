using System;
using System.Collections;
using System.Collections.Generic;
using Synth_Variables;
using UnityEngine;
using UnityEngine.Events;

public class UIValueTrigger : MonoBehaviour
{
    [SerializeField] private float triggerOnValue = 0f;
    [SerializeField] private FloatVariable value;
    [SerializeField] private UnityEvent OnValueTrigger;
    // Start is called before the first frame update
    void Start()
    {
        value.ValueChanged += OnValueChanged;
    }

    private void OnDisable()
    {
        value.ValueChanged -= OnValueChanged;
    }

    public void OnValueChanged(float newValue)
    {
        if (Mathf.Abs(newValue - triggerOnValue) <= float.Epsilon)
        {
            OnValueTrigger.Invoke();
        }
    }

   
}
