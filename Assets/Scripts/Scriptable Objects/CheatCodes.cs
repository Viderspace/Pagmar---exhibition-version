using System.Collections.Generic;
using Inputs;
using Runtime.Kernel.System;
using Synth.Sequencer;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu()]
    public class CheatCodes : ScriptableObject
    {
        [SerializeField] private int cheatCodeLength = 4;
        [SerializeField] private string sandboxMode = "2600";
        private readonly List<Keypad> _currentCode = new List<Keypad>();


        public string CurrentCode()
        {
            var code = "";
            foreach (var key in _currentCode)
            {
                code += key.Name;
            }

            return code;
        }

        private bool CompareCode()
        {
            if (_currentCode.Count != cheatCodeLength )
            {
                return false;
            }

            var result = MatchSandboxSkip();
            _currentCode.Clear();
            return result;

        }
     
        
        
        //
        // private bool MatchPreset(IReadOnlyList<Keypad> presetCode)
        // {
        //     for (int i = 0; i < sandboxMode.Length; i++)
        //     {
        //         if (presetCode[i].Name[0] != _currentCode[i].Name[0])
        //             return false;
        //     }
        //
        //     TimelineController.Instance.MoveToEnd();
        //     return true;
        // }
        //  

 

        private bool MatchSandboxSkip()
        {
            Debug.Log("MatchSandboxSkip: " + sandboxMode + " " + CurrentCode());
            for (int i = 0; i < sandboxMode.Length; i++)
            {
                if (sandboxMode[i] != _currentCode[i].Name[0])
                    return false;
            }

            return true;
        }


        public bool ListenForCheatCode(Keypad key)
        {
            _currentCode.Add(key);
            return CompareCode();
        }
        

        public void Reset()
        {
            _currentCode.Clear();
        }
        
    }

    
    [System.Serializable]
    public class CodeActionPair
    {
       public string code; 
        public SynthPreset preset;
        
        
        public List<Keypad> GetCode()
        {
            var codeList = new List<Keypad>();
            foreach (var c in code)
            {
                var key = Keypad.GetKeyFromChar(c);
                if (key == null)
                {
                    Debug.LogError("Invalid code: " + code + " contains invalid character: " + c);
                    return null;
                }
                codeList.Add(key);
            }

            return codeList;
        }
    }

}