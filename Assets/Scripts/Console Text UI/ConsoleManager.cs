using System;
using Inputs;
using Runtime.Timeline.Main_terminal_Track.Memento_SnapShots;
using Scriptable_Objects;
using TMPro;
using UnityEngine;

namespace Runtime.Timeline.Main_terminal_Track
{
    public class ConsoleManager : MonoBehaviour
    {
        [SerializeField] TMP_Text textComponent;
        private CareTaker _careTaker;
        public AudioFx AudioFx => Singleton.Instance.AudioFx;
        [SerializeField] private float meter;
        public float LevelMeter { get; set; }

        public string text
        {
            get => textComponent.text;
            set => textComponent.text = value;
        }

        public int maxVisibleCharacters
        {
            get => textComponent.maxVisibleCharacters;
            set => textComponent.maxVisibleCharacters = value;
        }

        public int BufferKeysCount = 0;

        public int textInfoCharacterCount
        {
            get => textComponent.textInfo.characterCount;
            set => textComponent.textInfo.characterCount = value;
        }

        private void Start()
        {
            _careTaker = new CareTaker(this);
            ClearScreenOnHangup();
        }

        private void OnApplicationQuit()
        {
            ClearScreenOnHangup();
        }

        public void ClearScreenOnHangup()
        {
            textComponent.text = "";
            textComponent.maxVisibleCharacters = 0;
        }

        private void OnEnable()
        {
            InputManager.PhoneHangup += ClearScreenOnHangup;
        }

        private void OnDisable()
        {
            InputManager.PhoneHangup -= ClearScreenOnHangup;
        }

        public ConsoleSnapshot Save(string updatedText, int visibleCharsAtStart)
        {
            return _careTaker.Backup(updatedText,visibleCharsAtStart);
        }
        
        private float lastMeter = 0;

        private void FixedUpdate()
        {
            meter = Mathf.Max(meter, LevelMeter);
            if (lastMeter < 0 && LevelMeter > 0 && BufferKeysCount > 0)
            {
                print("key STROKE pressed now");
                BufferKeysCount--;
                maxVisibleCharacters++;
            }
            
            lastMeter = LevelMeter;
        
            
        }


        /**
     * This method is called by a clip when entered, after the clip updated the textComponent text field
     * if id == -1, it will save the current state of the textComponent,
     * else it will revert the textComponent to the state it was in when it was entered initially,
     *  And return the textComponent to the state it was in when it was exited.
     * @ param snapId - the id of the clip that called this method
     * @ return - the number of characters that were visible when the clip was entered initially
     */
        public ConsoleSnapshot OnEnterClip(ConsoleSnapshot snapshot, int visibleCharsAtStart = -1)
        {
            if (Application.isPlaying == false) return null;

            
            //TODO - Testing new Mechanic
            // AudioFx.Play(AudioFx.FX.DigitalKeystrokesSequence);
            // maxVisibleCharacters = 0;
            
            
            if (snapshot == null)
            {
                // Clip is entered for the first time, save the current state
                return Save(textComponent.text, visibleCharsAtStart);
            }
            else
            {
                // revert to the state the textComponent was in when it was entered initially
                _careTaker.Revert(snapshot.GetId());
                return snapshot;
            }
        }

        public void Restore(ConsoleSnapshot memento)
        {
            textComponent.text = memento.GetUpdatedText();
            textComponent.maxVisibleCharacters = memento.GetVisibleCountOnEnter();
        }
    }
}