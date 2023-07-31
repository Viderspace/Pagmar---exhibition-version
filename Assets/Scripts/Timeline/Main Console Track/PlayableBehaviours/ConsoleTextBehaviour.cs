using System.Collections.Generic;
using Runtime.Timeline.Main_terminal_Track.Memento_SnapShots;
using Scriptable_Objects;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

namespace Runtime.Timeline.Main_terminal_Track
{
        public class ConsoleTextBehaviour : PlayableBehaviour
    {
        AudioFx AudioFx => Singleton.Instance.AudioFx;
        public ConsoleSnapshot Snapshot;
        
        #region clip behaviour properties

        public ConsoleTextClip.InsertionMode InsertionMode;
        public TerminalTextSettings.TypeSpeed typeSpeed;
        public List<bool> KeystrokeMap;
        public string TextField;
        
        #endregion

        #region Clip Internal Fields

        // the number of characters that are currently visible
        int _visibleCount = 0;
        // the time that should pass before the next character is displayed
       // float _timer = 0f; todo - remove
        // the time that has passed since the last character was displayed
        float _time = 0f;

        private bool _isSet = false;
        
        private int charIndex = 0;
        
        #endregion

 

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            base.OnBehaviourPause(playable, info);
            _isSet = false;
        }

        public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        {
            if (Application.isPlaying == false) return;

            ConsoleManager textComponent = playerData as ConsoleManager;
            if (textComponent == null) return;
            
            if (_isSet == false)
            {
                Initialize(textComponent);
                return;
            }

            if (_time > 0)
            {
                _time -= Time.deltaTime;
                // _time -= 0.0167f;
                return;
            }
            
            // reset timer
            _time = TerminalTextSettings.VariantSpeed(typeSpeed);
            
            // if text has changed -> update total visible characters

            
            // no need for update
            if (_visibleCount > textComponent.textInfoCharacterCount) return;
            
            textComponent.maxVisibleCharacters = _visibleCount; // Update the text shown by the text component
            _visibleCount += 1;
            charIndex += 1;
            if (charIndex - 1 >= KeystrokeMap.Count)
            {
                return;
            }
            if (AudioFx != null && KeystrokeMap[charIndex-1]) AudioFx.AddKeyStroke();
        }
        
        
        
        
        
        
        
        

        #region Utility methods
        
        public void Initialize(ConsoleManager textComponent)
        {
            if (_isSet) return;
            _isSet = true;
            
            charIndex = 0;

            if (Snapshot != null)
            {
                Snapshot = textComponent.OnEnterClip(Snapshot);
                _visibleCount = Snapshot.GetVisibleCountOnEnter();
                return;
            }

            if (InsertionMode == ConsoleTextClip.InsertionMode.Replace)
            {
                _visibleCount = 0;
            }
            else
            {
                _visibleCount = textComponent.textInfoCharacterCount;
            }
            
           UpdateTextComponent(textComponent, TextField);
           Snapshot = textComponent.OnEnterClip(Snapshot, _visibleCount);
        }

        public void UpdateTextComponent(ConsoleManager textComponent, string textField)
        {
            switch (InsertionMode)
            {
                case ConsoleTextClip.InsertionMode.Replace:
                    textComponent.text= textField;
                    break;
                
                case ConsoleTextClip.InsertionMode.InsertInNewLine:
                    textComponent.text = textComponent.text + "\n" + textField;
                    break;
                
                case ConsoleTextClip.InsertionMode.AppendInSameLine:
                    textComponent.text =  textComponent.text + " " + textField;
                    break;
                
                default:
                    Debug.Log("PROBLEM IN TEXT CLIP BEHAVIOUR");
                    textComponent.text =  textField;
                    break;
            }
        }
        #endregion
    }
    // public class ConsoleTextBehaviour : PlayableBehaviour
    // {
    //     AudioFx AudioFx => Singleton.Instance.AudioFx;
    //     
    //     #region clip behaviour properties
    //
    //     public ConsoleTextClip.InsertionMode InsertionMode;
    //     public TerminalTextSettings.TypeSpeed typeSpeed;
    //     public string TextField;
    //     
    //     // the number of characters that are currently visible
    //     int _visibleCount = 0;
    //     // the time that should pass before the next character is displayed
    //     float _timer = 0f;
    //     // the time that has passed since the last character was displayed
    //     float _time = 0f;
    //
    //     private bool _isSet = false;
    //
    //     #endregion
    //
    //     public override void OnBehaviourPause(Playable playable, FrameData info)
    //     {
    //         base.OnBehaviourPause(playable, info);
    //         _isSet = false;
    //     }
    //
    //     public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    //     {
    //         TMP_Text textComponent = playerData as TMP_Text;
    //         if (textComponent == null) return;
    //         
    //         if (_isSet == false)
    //         {
    //             Initialize(textComponent);
    //             return;
    //         }
    //
    //         if (_time > 0)
    //         {
    //             _time -= Time.deltaTime;
    //             return;
    //         }
    //         
    //         // reset timer
    //         _time = TerminalTextSettings.VariantSpeed(typeSpeed);
    //         
    //         // if text has changed -> update total visible characters
    //
    //         
    //         // no need for update
    //         if (_visibleCount > textComponent.textInfo.characterCount) return;
    //         
    //         textComponent.maxVisibleCharacters = _visibleCount; // Update the text shown by the text component
    //         _visibleCount += 1;
    //         if (AudioFx != null) AudioFx.AddKeyStroke();
    //     }
    //
    //     #region Utility methods
    //     
    //     public void Initialize(TMP_Text textComponent)
    //     {
    //         if (_isSet) return;
    //         _isSet = true;
    //
    //         if (InsertionMode == ConsoleTextClip.InsertionMode.Replace)
    //         {
    //             _visibleCount = 0;
    //         }
    //         else
    //         {
    //             _visibleCount = textComponent.textInfo.characterCount;
    //         }
    //         
    //        UpdateTextComponent(textComponent, TextField);
    //     }
    //
    //     public void UpdateTextComponent(TMP_Text textComponent, string textField)
    //     {
    //         switch (InsertionMode)
    //         {
    //             case ConsoleTextClip.InsertionMode.Replace:
    //                 textComponent.text= textField;
    //                 break;
    //             
    //             case ConsoleTextClip.InsertionMode.InsertInNewLine:
    //                 textComponent.text = textComponent.text + "\n" + textField;
    //                 break;
    //             
    //             case ConsoleTextClip.InsertionMode.AppendInSameLine:
    //                 textComponent.text =  textComponent.text + " " + textField;
    //                 break;
    //             
    //             default:
    //                 Debug.Log("PROBLEM IN TEXT CLIP BEHAVIOUR");
    //                 textComponent.text =  textField;
    //                 break;
    //         }
    //     }
    //     #endregion
    // }
}