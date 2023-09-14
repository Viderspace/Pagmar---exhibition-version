using System;
using System.Collections;
using FX___Animations.Glitch_Effect;
using Reaktor_Communication;
using Runtime.Kernel.System;
using Runtime.Kernel.Telephone_State_Machine;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;

namespace Telephone_State_Machine
{
    public class StateMachine : MonoBehaviour
    {
        public static StateMachine Instance;

        public enum CurrState
        {
            IDLE,
            RINGING,
            DIRECT_USER_DIAL,
            INTERACTION,
            SANDBOX,
            DEBUG
        }
        public CurrState currState = CurrState.IDLE;
        
        public static event Action<CurrState> StateChanged;
        #region Inspector

        [SerializeField] public bool DistanceSensorMode;
        [SerializeField] private string statePreview;
        [SerializeField]  private ToggleVariable IdleTelephoneScreen;
      [SerializeField]  private ToggleVariable DirectUserDialScreen;
      [SerializeField]  private ToggleVariable RingingWarningScreen;
      [SerializeField] private ToggleVariable InteractionScreen;
      [SerializeField] private GameObject BlackGlitchBG;

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
            Instance = this;
            BlackGlitchBG.SetActive(false);
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

        public void EnterSandboxMode()
        {
            StartCoroutine(OramGoodbyeGlitch());
        }
        private IEnumerator OramGoodbyeGlitch()
        {
            GlitchController.OnGlitchTriggered(true);
            yield return new WaitForSeconds(0.4f);
            BlackGlitchBG.SetActive(true);
            CurrentState = SandboxState;
            statePreview = SandboxState.StateName;
            CurrentState.Enter(); 
            yield return new WaitForSeconds(2.5f);
            Singleton.Instance.AudioFx.Play(AudioFx.FX.InsertNewLine);
            BlackGlitchBG.SetActive(false);
        }

        #endregion

        #region Methods
        private void InitializeStateMachine()
        {
            InitState = new InitState(this, statusWindow, IdleTelephoneScreen);
            RingingState = new RingingState(this, statusWindow, RingingWarningScreen);
            DirectUserDialState = new DirectUserDialState(this, statusWindow, DirectUserDialScreen);
            InteractionState = new InteractionState(this, statusWindow, InteractionScreen);
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
            StateChanged?.Invoke(currState);
        }

   

        public void SetRingingSound(bool on)
        {
            if (on) AudioPlayer.Play();
            else AudioPlayer.Stop();
        }
        
        #endregion
    }


}