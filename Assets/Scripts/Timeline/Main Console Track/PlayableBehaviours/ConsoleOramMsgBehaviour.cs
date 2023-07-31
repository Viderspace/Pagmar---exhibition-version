using System.Collections.Generic;
using Runtime.Timeline.Main_terminal_Track.Memento_SnapShots;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Playables;
using Utils;

namespace Runtime.Timeline.Main_terminal_Track
{
    public class ConsoleOramMsgBehaviour : PlayableBehaviour
    {
        public ConsoleTextClip.InsertionMode InsertionMode;
        public TerminalTextSettings.TypeSpeed typeSpeed;
        public int prefixCharacterCount;
        public string oramMassage;
        public List<bool> KeystrokeMap;


        #region Clip Internal Fields
        
        AudioFx AudioFx => Singleton.Instance.AudioFx;
        public ConsoleSnapshot Snapshot;

        // the number of characters that are currently visible
        private int _visibleCount = 0;
        // the time that should pass before the next character is displayed
        // private float _timer = 0f; todo - remove
        // the time that has passed since the last character was displayed
        private float _time = 0f;

        private bool _isSet = false;
        
     private   int charIndex = 0;
        
        #endregion


        #region PlayableBehaviour overrides
        


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
                return;
            }
            
            // reset timer
            _time = TerminalTextSettings.VariantSpeed(typeSpeed);
            
            // if text has changed -> update total visible characters

            
            // no need for update
            if (_visibleCount > textComponent.textInfoCharacterCount) return;
            
            textComponent.maxVisibleCharacters = _visibleCount; // Update the text shown by the text component
    
            if ( charIndex  < KeystrokeMap.Count &&KeystrokeMap[charIndex]) AudioFx.AddKeyStroke();
            
            _visibleCount += 1;
            charIndex += 1;

        }

        
        
        
        
        #endregion
        
        #region Utility methods
        
        public void Initialize(ConsoleManager textComponent)
        {
            if (_isSet) return;
            _isSet = true;
            
            charIndex = prefixCharacterCount;

            
            if (Snapshot != null)
            {
                Snapshot = textComponent.OnEnterClip(Snapshot);
                // Add prefix character count to visible count to make sure that the prefix is visible on enter
                _visibleCount = Snapshot.GetVisibleCountOnEnter() + prefixCharacterCount;
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
            
            UpdateTextComponent(textComponent, oramMassage);
            Snapshot = textComponent.OnEnterClip(Snapshot, _visibleCount);

            // Add prefix character count to visible count to make sure that the prefix is visible on enter
            _visibleCount += prefixCharacterCount;
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
}