using System;
using System.Collections;
using System.Collections.Generic;
using Inputs;
using Synth_Variables.Native_Types;
using UnityEngine;
using UnityEngine.UI;
using Utils;

public class OramCallsTelephoneScreen : MonoBehaviour
{
    [Header("Components")]
   [SerializeField] private ToggleVariable toggleVariable;
    [SerializeField] private GameObject BG;
    Image bgImage;
    [SerializeField] private GameObject GraphicsWarning;
    private Coroutine _coroutine;

    [Header("Settings")] public float delayOnStart = 0.5f;
    public float delayOnBgDissapear = 1.5f;

    private void OnEnable()
    {
        toggleVariable.ValueChanged += ActivateUI;
        InputManager.PhoneHangup += ResetToInit;
    }
    
    private void OnDisable()
    {
        toggleVariable.ValueChanged -= ActivateUI;
        InputManager.PhoneHangup -= ResetToInit;
    }

    void ResetToInit()
    {
        toggleVariable.ResetToDefault();
        ActivateUI(toggleVariable.DefaultValue);
    }

    private void Start()
    {
        bgImage = BG.GetComponent<Image>();
        ActivateUI(toggleVariable.DefaultValue);
    }

    private IEnumerator ShowWarning()
    {
        bgImage.color = DesignPalette.BGBlue;
        yield return new WaitForSeconds(delayOnStart);
        GraphicsWarning.SetActive(true);
        yield return new WaitForSeconds(delayOnBgDissapear);
        // BG.SetActive(false);
        bgImage.color = Color.black;
        yield return new WaitForSeconds(0.2f);
        bgImage.color = DesignPalette.BGBlue;
        yield return new WaitForSeconds(0.06f);
        bgImage.color = Color.black;
        yield return null;
    }


    private void ActivateUI(bool on)
    {
        BG.SetActive(on);
        if (on)
        {
            _coroutine = StartCoroutine(ShowWarning());
            return;
        }
        else
        { 
            if (_coroutine != null) StopCoroutine(_coroutine);
            GraphicsWarning.SetActive(false);
        }
    }
}
