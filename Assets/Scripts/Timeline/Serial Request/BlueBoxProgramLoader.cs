using System;
using System.Collections;
using ASCII_Animations;
using Inputs;
using Story.Terminal.ContentCreation;
using UnityEngine;

public class BlueBoxProgramLoader : MonoBehaviour
{
    public bool IsRunning { get; private set; }
    [SerializeField] public InputValidationRoutine validationRoutine;
    [SerializeField] private SerialValidationWindow window;
    [SerializeField] private AsciiWaveform asciiWaveform;
   [SerializeField] float asciiShowDelay = 0.5f;
   [SerializeField] float asciiShowDuration = 1f;
   
    
    public delegate void OnValidationFinished();
    public OnValidationFinished onValidationFinished;

    private int calls = 0;


    public void RunValidation(bool isCorrect)
    {
        calls++;
        print("---------RunValidation called "+calls+" times-----------");
        IsRunning = true;
        if (isCorrect)
        {
            window.StartValidationRoutine(validationRoutine.GetSuccessRoutine(), ValidationFinished());
        }
        else
        {
            window.StartValidationRoutine(validationRoutine.GetFailureRoutine(), ValidationFinished());
        }
        
        StartCoroutine(ShowAscii());
    }

    public IEnumerator ShowAscii()
    {
        yield return new WaitForSeconds(asciiShowDelay);
        asciiWaveform.ShowAsciiWave();
        yield return new WaitForSeconds(asciiShowDuration);
        asciiWaveform.HideAsciiWave();
    }
    
    

    
    #region Monobehaviour

    private void OnEnable()
    {
        InputManager.PhoneHangup += AbortAndReset;
    }

    private void OnDisable()
    {
        InputManager.PhoneHangup -= AbortAndReset;
    }


    private void AbortAndReset()
    {
        IsRunning = false;
    }

    #endregion

    
    IEnumerator ValidationFinished()
    {
        IsRunning = false;
        asciiWaveform.HideAsciiWave();
        onValidationFinished?.Invoke();
        print("Validation finished");
        yield return null;
    }
    


    
    
    
}
