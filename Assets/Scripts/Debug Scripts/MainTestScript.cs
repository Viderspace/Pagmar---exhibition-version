using Inputs;
using Scriptable_Objects;
using Synth_Variables;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Kernel.Tests
{
    public class MainTestScript : MonoBehaviour
    {
        public ToggleVariable debugMode;
        public KeyCode phonePickupKey = KeyCode.UpArrow;
        public KeyCode phoneHangupKey = KeyCode.DownArrow;
        public KeyCode showHideDebugWindowKey = KeyCode.D;
        public KeyCode showHideFilterUiKey = KeyCode.F;
        public KeyCode showHideSequencerUiKey = KeyCode.S;
        public KeyCode showHideAdsrKey = KeyCode.E;
        public KeyCode togglePitchModeKey = KeyCode.P;
        public KeyCode RedialKeyCode = KeyCode.Z;
        public KeyCode EnableADSRKey = KeyCode.A;


       [SerializeField] private FloatVariable MaxAmp;

        private bool _isFilterUiVisible;
        private bool _isSequencerUiVisible;
        private bool _isAdsrVisible;
        private SynthController.PitchMode _pitchMode;
        
        public GameObject debugWindow;
        public TMP_Text synthDataText;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
            Screen.SetResolution(1286,1926, FullScreenMode.Windowed);
            _isAdsrVisible = false;
            _isFilterUiVisible = false;
            _pitchMode = SynthController.PitchMode.Telephone;
        }

        private KeyCode[] numKeyCodes =
        {
            KeyCode.Alpha1,
            KeyCode.Alpha2,
            KeyCode.Alpha3,
            KeyCode.Alpha4,
            KeyCode.Alpha5,
            KeyCode.Alpha6,
            KeyCode.Alpha7,
            KeyCode.Alpha8,
            KeyCode.Alpha9,
            KeyCode.Alpha0
        };

        private void CheckReloadScene()
        {
            if (Input.GetKeyDown(KeyCode.R) && Input.GetKey(KeyCode.LeftShift))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }

        private void CheckQuitApp()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }


        private void Update()
        {
            if(Application.targetFrameRate != 60)
            {
                Application.targetFrameRate = 60;
            }
            
            
            
            if (Input.GetKey(KeyCode.C))
            {
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    var currentValue = Singleton.Instance.SynthController.CutoffOffset.Value;
                    InputManager.OnUpdateCutoffPos(currentValue +0.03f);
                }
                else if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    var currentValue = Singleton.Instance.SynthController.CutoffOffset.Value;
                    InputManager.OnUpdateCutoffPos(currentValue -0.03f);

                }
            }

            if (Input.GetKeyDown(EnableADSRKey))
            {
                bool toggleADSR = !Singleton.Instance.SynthController.AdsrOnOffSwitch.Value;
                InputManager.OnUpdateAdsrIsActive(toggleADSR);
            }
            
            
            if (Input.GetKeyDown(showHideDebugWindowKey))
            {
                debugMode.Value = !debugMode.Value;
                debugWindow.SetActive(debugMode.Value);
            }

            if (Input.GetKeyDown(togglePitchModeKey))
            {
                _pitchMode = (Singleton.Instance.SynthController.ActivePitchMode.Value == SynthController.PitchMode.Telephone)?
                    SynthController.PitchMode.MusicalNotes : SynthController.PitchMode.Telephone;
                InputManager.OnSetPitchMode(_pitchMode);
            }
            
            if (Input.GetKeyDown(showHideFilterUiKey))
            {
                _isFilterUiVisible = !_isFilterUiVisible;
                InputManager.OnActivateFilter();
            }
            
            if (Input.GetKeyDown(showHideSequencerUiKey))
            {
                _isSequencerUiVisible = !_isSequencerUiVisible;
                InputManager.OnUpdateSequencerIsActive(_isSequencerUiVisible);
            }
            
            if (Input.GetKeyDown(showHideAdsrKey))
            {
                _isAdsrVisible = !_isAdsrVisible;
                InputManager.OnUpdateAdsrIsActive(_isAdsrVisible);
            }
            
            if (Input.GetKeyDown(RedialKeyCode))
            {
                InputManager.OnKeypadClearCurrentStep();
            }


            if (Input.GetKeyDown(phonePickupKey))
            {
                InputManager.OnPhonePickup();
            }
            else if (Input.GetKeyDown(phoneHangupKey))
            {
                InputManager.OnPhoneHangup();
            }
            
            CheckReloadScene();
            CheckQuitApp();

        }
        
    

        float time = 0;
        private void FixedUpdate()
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
                return;
            }

            time = .5f;
            SynthSnapshot snapshot = new SynthSnapshot(Singleton.Instance.SynthController);
            
            synthDataText.text = snapshot.Print() + "\n \n" +"Max Amp: " + MaxAmp.Value;
        }
    }
}