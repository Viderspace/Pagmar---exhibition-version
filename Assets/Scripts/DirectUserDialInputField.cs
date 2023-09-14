using System.Collections;
using System.Collections.Generic;
using Inputs;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;

public class DirectUserDialInputField : MonoBehaviour
{

    public static DirectUserDialInputField Instance { get; private set; }
    [SerializeField] TMP_Text textComponent;
    [SerializeField] private GameObject WarningWindow;
    [SerializeField] private ToggleVariable toggleVariable;
    [SerializeField] private GameObject BG;
    [SerializeField] private GameObject GraphicsInstruction;
        
    [SerializeField] [Range(0, 3)] double monoSpacingCode = 1.5;
    // private string enterserialPrompt = "  ▁ ▂ ▃ ▄ ▅ ▆ ▇ █ ▀ ▔ ▏ ▎ ▍ ▌ ▋ ▊ ▉ ▐ ▕ ▖ ▗ ▘ ▙ ▚ ▛ ▜ ▝ ▞ ▟ ░ ▒ ▓ ⎕ ⍂  ● ○ ◯ ◔ ◕ ◶ ◌ ◉ ◎ ◦ ◆ ◇ 0 0 1 2 3 4 5 6 7 8 9 ₀ ₁ ₂ ₃ ₄ ₅ ₆ ₇ ₈ ₉ ⁰ ¹ ² ³ ⁴ ⁵ ⁶ ⁷ ⁸ ⁹ ⟦ ⌈ ⌊ ⌉ ⌋  __ _  ∎";

   
    // ▁ ▂ ▃ ▄ ▅ ▆ ▇ █ ▀ ▔ ▏ ▎ ▍ ▌ ▋ ▊ ▉ ▐ ▕ ▖ ▗ ▘ ▙ ▚ ▛ ▜ ▝ ▞ ▟ ░ ▒ ▓ ⎕ ⍂  ● ○ ◯ ◔ ◕ ◶ ◌ ◉ ◎ ◦ ◆ ◇
    // 0 0 1 2 3 4 5 6 7 8 9 ₀ ₁ ₂ ₃ ₄ ₅ ₆ ₇ ₈ ₉ ⁰ ¹ ² ³ ⁴ ⁵ ⁶ ⁷ ⁸ ⁹ ⟦ ⌈ ⌊ ⌉ ⌋  __ _  ∎
    private string _hideCharachter = "●";
    private string _blankCharacter = "-";
    public double MonoSpacingCode = 1.5;
    
    
    private readonly List<Keypad> _Passcode = new List<Keypad>()
    {
        Keypad.Two, Keypad.Six, Keypad.Zero, Keypad.Zero
    };
        
    private List<Keypad> _userCode = new List<Keypad>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Debug.LogError("Multiple instances of DirectUserDialInputField");
        }
    }

    private void Start()
    {
        ResetToInit();
        Activate(toggleVariable.DefaultValue);
    }

    private void OnEnable()
    {
        toggleVariable.ValueChanged += Activate;
    }

    private void OnDisable()
    {
        toggleVariable.ValueChanged -= Activate;
    }

    // private void SelfDectivate()
    // {
    //     toggleVariable.Value
    // }
    
    
    
    
    private void Activate(bool on)
    {
        ResetToInit();
        BG.SetActive(on);
        GraphicsInstruction.SetActive(on);
        textComponent.gameObject.SetActive(on);
    } 

    public void ResetToInit()
    {
        ClearDigits();
        WarningWindow.SetActive(false);
    }
    
    

    public void ClearDigits()
    {
        _userCode.Clear();
        textComponent.text = RenderUserInputField(false);
    }




    public bool HasEnteredAllDigits =>
        _userCode.Count == _Passcode.Count;
    
    public string RenderUserInputField(bool revealed = true)
    {
        int showLastChar = (revealed ? 1 : 0);
        string returnString = $"[<mspace={MonoSpacingCode}em>";
        int i = 0;
        for (i = 0; i < _userCode.Count - showLastChar; i++)
        {
            returnString += _hideCharachter;
        }

        if (revealed) returnString += _userCode[i].Name[0];

        for (i = 0; i < _Passcode.Count - _userCode.Count; i++)
        {
            returnString += _blankCharacter;
        }

        return returnString + "</mspace>]";
    }

    public void InsertDigit(Keypad digit)
    {
        _userCode.Add(digit);
        textComponent.text = RenderUserInputField();
        if (!HasEnteredAllDigits) return ;
            
        bool result = ValidateSerialNumber();
        if (result)
        {
            ClearDigits();
            return ;
        }
        else
        {
            // SHOW WARNING
             StartCoroutine(BadInputRespond());
        }
    }

    IEnumerator BadInputRespond()
    {
        yield return new WaitForSeconds(1.5f);
        WarningWindow.SetActive(true);
        ClearDigits();
        yield return null;
    }

    public bool ValidateSerialNumber()
    {
        if (_userCode.Count < _Passcode.Count) return false;
        for (int i = 0; i < _userCode.Count; i++)
        {
            if (_Passcode[i].Name[0] != _userCode[i].Name[0])
                return false;
        }

        return true;
    }
}