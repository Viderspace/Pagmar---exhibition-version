
using System;
using System.Collections.Generic;
using Inputs;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;
using Utils;

[RequireComponent(typeof(TMP_Text))]
public class ConsoleStatusBar : MonoBehaviour
{
    public static ConsoleStatusBar Instance { get; private set; }
    public ToggleVariable isInSandboxMode;
    public TMP_Text textComponent;
    public Color initWhiteColor = Color.white;
    public string afterSerialEnteredColor => DesignPalette.BGBlueHex;
    
        // private string initStatus = "<Remote Desktop>"; 
    public string oramHeader = "<0ram>";

    private void OnEnable()
    {
        InputManager.PhoneHangup += ResetToInit;
    }

    private void OnDisable()
    {
        InputManager.PhoneHangup -= ResetToInit;
    }

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    public void Start()
    {
        ResetToInit();
    }

    private void ResetToInit()
    {
        textComponent.text = RenderText(oramHeader, initWhiteColor);
        AddWarning(" unknown ip", WarningColor.Grey);

    }
    
    public void SetStatus(string status, Color color)
    {
        textComponent.text = RenderText(status, color);
    }

   public void EnterSandboxMode()
    {

            SetStatus("Synth Mode", DesignPalette.BGBlue);
    }
    
    public enum WarningColor
    {
        Red,
        Yellow,
        Grey
    }

    public void DisplayKernelReboot()
    {
        textComponent.text = "";
        AddWarning("Kernal Reboot", WarningColor.Red);
    }
    
    public static Dictionary<WarningColor, Color> warningColorMap = new Dictionary<WarningColor, Color>()
    {
        {WarningColor.Red, Color.red},
        {WarningColor.Yellow, Color.yellow},
        {WarningColor.Grey, Color.grey}
    };

    public void AddWarning(string warning, WarningColor warningColor)
    {
        textComponent.text += " " + RenderText(warning, warningColorMap[warningColor]);
    }
    




    private string RenderText(string header, Color color)
    {
        return $"<color=#{ColorUtility.ToHtmlStringRGB(color)}>{header}</color>";
    }
    
    private string RenderText(string header, string colorHex)
    {
        return $"<color=#{colorHex}>{header}</color>";
    }

    #region TimelineEvents
    
    public void OnSerialIDSequenceEnd()
    {
        textComponent.text = RenderText(oramHeader, afterSerialEnteredColor);
    }
    
    public void SecurityAlert()
    {
       
        AddWarning("Security Alert <Inspection>", WarningColor.Red);
    }

    public void SecurityDisabled()
    {
        textComponent.text = RenderText(oramHeader, afterSerialEnteredColor);
        textComponent.text += " "+ RenderText("Security Disabled", DesignPalette.BGBlueHex);
    }

    

    #endregion


}
