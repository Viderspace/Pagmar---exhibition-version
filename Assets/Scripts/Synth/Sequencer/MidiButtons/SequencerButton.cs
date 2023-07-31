using System;
using System.Collections;
using System.Collections.Generic;
using Inputs;
using Reaktor_Communication;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using UnityEngine;
using UnityEngine.UI;

namespace Synth.Sequencer.MidiButtons
{
    public class SequencerButton : MonoBehaviour
    {
        #region Fields and Properties
        public string fillDebug = "fill";
        public string strokeDebug = "stroke";

        // private Button buttonComponent;
        public ToggleVariable IsOnVariable;
        private SequencerModeVariable SequencerGlobalState;

        [SerializeField]private Image fillColor;
       [SerializeField] private Image strokeColor;
        private SynthController SynthController => Singleton.Instance.SynthController;


        public static event Action NoteLaunch;

        // private bool IsOn { get; set; }
        public int MidiNote { get; set; }
        public Keypad MyKey { get; set; }
        public int TimeStamp { get; set; }

        #endregion


        private void SetColor()
        {
          
        }
   
        #region MonoBehavior

        private void OnEnable()
        {
            SequencerController.NoteTrigger += PlayMe;
            SequencerController.NoteReset += GlobalReset;
            SequencerController.SelectNoteFromKeypad += ExternalSelectMe;
            SequencerController.UnselectCurrentStep += ExternalUnSelectCurrentStep;
        }


        private void OnDisable()
        {
            SequencerController.NoteTrigger -= PlayMe;
            SequencerController.NoteReset -= GlobalReset;
            SequencerController.SelectNoteFromKeypad -= ExternalSelectMe;
            SequencerController.UnselectCurrentStep -= ExternalUnSelectCurrentStep;
        }
        
        public void SetColor(SynthController.SequencerState mode, bool isSelected)
        {
            fillColor.color = NoteColorPicker.GetFillColor(mode, isSelected);
            strokeColor.color = NoteColorPicker.GetStrokeColor(false);
            fillDebug = "fill: " + fillColor.color.ToString();
            strokeDebug = "stroke: " + strokeColor.color.ToString();
        }
        public void SetColor(SynthController.SequencerState mode)
        {
            fillColor.color = NoteColorPicker.GetFillColor(mode, IsOnVariable.Value);
            strokeColor.color = NoteColorPicker.GetStrokeColor(false);
            fillDebug = "fill: " + fillColor.color.ToString();
            strokeDebug = "stroke: " + strokeColor.color.ToString();
        }
        
        public void SetColor( bool isSelected)
        {
            fillColor.color = NoteColorPicker.GetFillColor(SequencerGlobalState.Value, isSelected);
            strokeColor.color = NoteColorPicker.GetStrokeColor(false);
            fillDebug = "fill: " + fillColor.color.ToString();
            strokeDebug = "stroke: " + strokeColor.color.ToString();
        }
   


        private void Awake()
        {
            // GetComponent<Image>().color = Color.white;
            // buttonComponent = GetComponent<Button>();
            IsOnVariable = ScriptableObject.CreateInstance<ToggleVariable>();
            IsOnVariable.DefaultValue = false;
            IsOnVariable.Value = false;
            // fillColor = transform.GetChild(0).GetComponent<Image>();
            // strokeColor = transform.GetChild(1).GetComponent<Image>();
            // buttonComponent.onClick.AddListener(Select);
            IsOnVariable.ValueChanged += SetColor;
            SetColor(SynthController.SequencerState.Idle, false);
        }


        public void Init(int timeStamp, int midiNote, SequencerModeVariable sequencerMode)
        {
            SequencerGlobalState = sequencerMode;
            SequencerGlobalState.ValueChanged += SetColor;

            TimeStamp = timeStamp;
            MidiNote = midiNote;
            MyKey = Keypad.GetKeyFromMidiNote(midiNote);
            AddToGrid(timeStamp, midiNote);

        }

        #endregion


        #region Methods

        private void GlobalReset()
        {
            IsOnVariable.Value = false;
            // _animator.DoResetAnimation();
        }

        private void PlayMe(int globalTimeStamp)
        {
            if (globalTimeStamp != TimeStamp) return;

            if (IsOnVariable.Value)
            {
                ReaktorController.Instance.NoteManager.PlayNoteAtLength(MyKey, SynthController.NoteLength);
                StartCoroutine(ShowStrokeOnPlay());
                OnNoteLaunch();
            }
            else
            {
                fillColor.color = NoteColorPicker.GetFillColor(SequencerGlobalState.Value, IsOnVariable.Value);
            }
        }

        private IEnumerator ShowStrokeOnPlay()
        {
            strokeColor.color = NoteColorPicker.GetStrokeColor(true);
            yield return new WaitForSeconds(SynthController.NoteLength);
            strokeColor.color = NoteColorPicker.GetStrokeColor(false);
        }


        public void Select()
        {

            IsOnVariable.Value = !IsOnVariable.Value;
            TurnOffParallelButtons();
        }

        private void ExternalUnselectMe()
        {
            IsOnVariable.Value = false;
        }

        private void ExternalSelectMe(int note, int timeStamp)
        {
            if (note != MidiNote || timeStamp != TimeStamp) return;
            Select();
        }
        
        // Re-dial Keypad press
        private void ExternalUnSelectCurrentStep(int timeStamp)
        {
            if ( timeStamp != TimeStamp) return;
            ExternalUnselectMe();
        }

        private void TurnOffParallelButtons()
        {
            foreach (var (otherButtonMidiNote, otherButton) in ButtonPool[TimeStamp])
            {
                if (otherButtonMidiNote == MidiNote) continue;
                otherButton.ExternalUnselectMe();
            }
        }

        #endregion

        private static void OnNoteLaunch()
        {
            NoteLaunch?.Invoke();
        }
        
        
        
        #region Static
        public static readonly Dictionary<int, Dictionary<int, SequencerButton>> ButtonPool = new();
        
        private void AddToGrid(int timeStamp, int midiNote)
        {
            if (!ButtonPool.ContainsKey(timeStamp))
            {
                ButtonPool.Add(timeStamp, new Dictionary<int, SequencerButton>());
            }

            ButtonPool[timeStamp].Add(midiNote, this);
        }

        #endregion

        
    }
}