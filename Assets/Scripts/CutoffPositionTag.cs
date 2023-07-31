using System;
using Synth_Variables;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;
using UnityEngine.UI.Extensions;

public class CutoffPositionTag : MonoBehaviour
{
    [SerializeField] private FloatVariable Position;
    [SerializeField] private ToggleVariable cutoffIs2600HzVariable;
   private static int CloseEnoughTo2600Range = 100;
    [SerializeField] private TMP_Text text;
    private int freqDebug;
    
    [SerializeField] private float visibleUpperBound = 0.9f;
    [SerializeField] private float visibleLowerBound = 0.1f;
    private  int currentFrequency = 0;

    private void Start()
    {
        if (!Cutoff2600HzSequence.Instance) return;
        Position.ValueChanged += UpdateText;
        cutoffIs2600HzVariable.ResetToDefault();

    }

    private void UpdateText(float value)
    {
        if (value <= visibleLowerBound || value >= visibleUpperBound)
        {
            text.text = "";
        }
        else
        {
            double freq = LinearToExponential(value);
            text.text = currentFrequency + " Hz";
            freqDebug = currentFrequency;


            if (Cutoff2600HzSequence.Instance.SequenceIsRunning)
            {
                CheckIfCloseEnoughTo2600Hz( (int)freq);  
            }
     
        }
    }

    private const float A = 20;
    private const float B = 1000;

    public int LinearToExponential(float value)
    {
        // Clamp value to ensure it is within [0, 1]
        value = Math.Max(0, Math.Min(value, 1));

        // Perform the exponential transformation
        var frequency = A * Mathf.Pow(B, value);
        currentFrequency = (int)frequency;
        return (int)frequency;
    }

    public void CheckIfCloseEnoughTo2600Hz(int freq)
    {
        if (Mathf.Abs(freq - 2600) < CloseEnoughTo2600Range)
        {
            cutoffIs2600HzVariable.Value = true;
        }

        else if (cutoffIs2600HzVariable.Value == true)
        {
            cutoffIs2600HzVariable.Value = false;
        }
    }

}
