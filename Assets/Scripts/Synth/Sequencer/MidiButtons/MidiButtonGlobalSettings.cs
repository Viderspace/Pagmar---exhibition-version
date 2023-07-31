using System;
using UnityEngine;
using UnityEngine.UI;

namespace Synth.Sequencer.MidiButtons
{
    public class MidiButtonGlobalSettings : MonoBehaviour
    {
        private static MidiButtonGlobalSettings _instance;
        public static MidiButtonGlobalSettings Instance => _instance;
        
        [SerializeField] private Color playingColor;
        public static Color PlayingColor;

        [SerializeField] private Color selectedColor;
        public static Color SelectedColor;

        [SerializeField] private Color unselectedColor;
        public static Color UnselectedColor;

        [SerializeField] private Color highlightedColor;
        public static Color HighlightedColor;

        [SerializeField] private bool flashAnimationLerp = true;
        public static bool FlashAccordingToAdsr = true;

        [SerializeField] private bool allowNoteEditingWhileCursorRunning = true;
        public static bool AllowNoteEditingWhileCursorRunning = true;

        [SerializeField] private bool allowNoteEditingFromKeypad;
        public static bool AllowNoteEditingFromKeypad;


        public static Color ResetColor = UnselectedColor * .5f;


        [SerializeField] [Tooltip("1 = the full note length")] [Range(.1f, 2f)]
        float flashDurationWhenSelected = 1f;

        public static float FlashDurationWhenSelected = 1f;

        [SerializeField] [Tooltip("0.5 = half the note length")] [Range(.1f, 2f)]
        float flashDurationWhenUnselected = .4f;

        public static float FlashDurationWhenUnselected = .4f;


        private void Awake()
        {
            if (_instance != null) Destroy(gameObject);
            else _instance = this;
        }

        private void Start()
        {
            Refresh();
        }

        private void Refresh()
        {
            // colors
            SelectedColor = selectedColor;
            UnselectedColor = unselectedColor;
            PlayingColor = playingColor;
            HighlightedColor = highlightedColor;

            // boolean settings
            FlashAccordingToAdsr = flashAnimationLerp;
            AllowNoteEditingWhileCursorRunning = allowNoteEditingWhileCursorRunning;
            AllowNoteEditingFromKeypad = allowNoteEditingFromKeypad;

            // flash duration settings
            FlashDurationWhenSelected = flashDurationWhenSelected;
            FlashDurationWhenUnselected = flashDurationWhenUnselected;
            
            EditGridColorInPlaymode();
        }
        
                
        public void EditGridColorInPlaymode()
        {
            var buttonPool = SequencerButton.ButtonPool;
            foreach (var column in buttonPool.Values)
            {
                foreach (var button in column.Values)
                {
                    // button.ButtonState?.InitColors();
                }
            }
        }

        private void OnValidate()
        { 
            Refresh();
        }
    }
}