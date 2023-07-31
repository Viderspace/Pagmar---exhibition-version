using DefaultNamespace;
using FX___Animations.Glitch_Effect;
using Reaktor_Communication;
using Runtime.Kernel.System;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using Telephone_State_Machine;
using TMPro;
using UnityEngine;

namespace Runtime.Kernel.Telephone_State_Machine
{
    public class StateMachine : MonoBehaviour
    {
        #region Inspector
        [SerializeField] private string statePreview;
      [SerializeField]  private ToggleVariable IdleTelephoneScreen;
      [SerializeField]  private ToggleVariable DirectUserDialScreen;
      [SerializeField]  private ToggleVariable RingingWarningScreen;
        
       // [SerializeField] private GlitchController glitchController;
        [SerializeField] private TelephoneSettings settings;
        [SerializeField] private TimelineController timelineController;
        #endregion

        #region State Machine Tools/services
        
        public TMP_Text statusWindow;
        public AudioSource AudioPlayer { get; private set; }
        public NoteManager KeypadPlayer => ReaktorController.Instance.NoteManager;
        public TimelineController TimelineController => timelineController;
        public TelephoneSettings Settings => settings;
        
        // public GlitchController Glitch => glitchController;
        
        #endregion

        #region State Machine States
        public DebugState DebugState { get; private set; }
        public InitState InitState { get; private set; }
        public RingingState RingingState { get; private set; }
        public DirectUserDialState DirectUserDialState { get; private set; }
        public InteractionState InteractionState { get; private set; }
        
        public SandboxState SandboxState { get; private set; }
        public BaseState CurrentState { get; private set; }
        #endregion

        #region Monobehaviour

        public void Awake()
        {
            AudioPlayer = gameObject.AddComponent<AudioSource>();
            AudioPlayer.clip = Settings.ringingSound;
            InitializeStateMachine();
            statePreview = "None";
        }

        private void Update()
        {
            if (settings.debugMode && Input.GetKeyDown(KeyCode.Return))
            {
                SwitchState(CurrentState == DebugState ? InitState : DebugState);
            }
        }

        #endregion

        #region Methods
        private void InitializeStateMachine()
        {
            InitState = new InitState(this, statusWindow, IdleTelephoneScreen);
            RingingState = new RingingState(this, statusWindow, RingingWarningScreen);
            DirectUserDialState = new DirectUserDialState(this, statusWindow, DirectUserDialScreen);
            InteractionState = new InteractionState(this, statusWindow);
            SandboxState = new SandboxState(this, statusWindow);
            DebugState = new DebugState(this, statusWindow);
            CurrentState = (settings.debugMode? DebugState : InitState);
            CurrentState.Enter();
        }

        public void SwitchState(BaseState newState)
        {
            CurrentState.Exit();
            CurrentState = newState;
            statePreview = newState.StateName;
            CurrentState.Enter();
        }

        public void SetRingingSound(bool on)
        {
            if (on) AudioPlayer.Play();
            else AudioPlayer.Stop();
        }
        
        #endregion
    }


}