using System;
using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;

namespace Inputs.Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Arduino Address Manager", menuName = "Arduino/Address Manager", order = 0)]
    public class ArduinoAddresses : ScriptableObject
    {
        public const char Delimiter = ',';
        public const string POT_CODE = "POT";
        public const string KEYPAD_CODE = "KEY";
        public const string KEY_DOWN = "DOWN";
        public const string KEY_UP = "UP";

        public const string PHONE_CODE = "PHN";
        public const string PHONE_DOWN = "DOWN";
        public const string PHONE_UP = "UP";

        public const string BUTTON_CODE = "BTN";

        public const string RESET_GRID_MSG = "RESET_GRID";
        public const string SEQUENCER_RECORD_MSG = "SEQRECORD";
        public const string SEQUENCER_PLAY_PAUSE_MSG = "SEQRUN";
        public const string SEQUENCER_MOVE_LEFT_MSG = "SEQLEFT";
        public const string SEQUENCER_MOVE_RIGHT_MSG = "SEQRIGHT";
        public const string FILTER_ACTIVATE_MSG = "FILTER_TOGGLE";

        private const char RE_DIAL_KEY = 'R';

        private static readonly string[] PhoneStates = new string[] {"U", "D"};

        public void InterpretMassage(string msg)
        {
            if (!msg.Contains(Delimiter)) return;
            var split = msg.Split(Delimiter, 2);
            string code = split[0].Trim();
            string command = split[1].Trim();


            switch (code)
            {
                case PHONE_CODE:
                    InvokePhoneEvent(command);
                    break;
                case KEYPAD_CODE:
                    InvokeKeypadEvent(command);
                    break;
                case POT_CODE:
                    SetPotentiometer(command);
                    break;
                case BUTTON_CODE:
                    InvokeButtonEvent(command);
                    break;
                case "WAV":
                    InvokeWaveShape(command);
                    break;
                    
            }
        }
        
        private float lastButtonPressTime = 0;
        private const float debounceTime = 0.2f;

        private void InvokeWaveShape(string shape)
        {
            Debug.Log("RECEIVED WAVE SHAPE: "+ shape);
            switch (shape)
            {
                case "SINE":
                    InputManager.OnUpdateWaveshape(SynthController.WaveShape.Sine);
                    break;
                case "SQUARE":
                    InputManager.OnUpdateWaveshape(SynthController.WaveShape.Square);
                    break;
                case "TRIANGLE":
                    InputManager.OnUpdateWaveshape(SynthController.WaveShape.Triangle);
                    break;
                case "SAWTOOTH":
                    InputManager.OnUpdateWaveshape(SynthController.WaveShape.Sawtooth);
                    break;
            }
        }
        
        
        
        public readonly Dictionary<string, ButtonAction> ButtonNotifiers =
            new Dictionary<string, ButtonAction>()
            {
                {SEQUENCER_RECORD_MSG, InputManager.OnSequencerRecordButtonPressed},
                {SEQUENCER_PLAY_PAUSE_MSG, InputManager.OnSequencerPlayPauseButtonPressed},
                {SEQUENCER_MOVE_LEFT_MSG, InputManager.OnUpdateLeftArrowPressed},
                {SEQUENCER_MOVE_RIGHT_MSG, InputManager.OnUpdateRightArrowPressed},
                {FILTER_ACTIVATE_MSG, InputManager.OnActivateFilter},
                {RESET_GRID_MSG, InputManager.OnUpdateClearButtonPressed}
            };

        private void InvokeButtonEvent(string buttonName)
        {
     
            lastButtonPressTime += Time.deltaTime;
            if (lastButtonPressTime < debounceTime) return;
            lastButtonPressTime = 0;
            buttonName = buttonName.Trim();
            ButtonNotifiers[buttonName]?.Invoke();
        }

        private static void InvokePhoneEvent(string command)
        {
            command = command.Trim();
            if (command == PHONE_UP) InputManager.OnPhonePickup();
            else if (command == PHONE_DOWN) InputManager.OnPhoneHangup();
            else Debug.LogError("Invalid phone command: " + command);
        }

        public delegate void PotentiometerAction(float value);
        public delegate void ButtonAction();

        private readonly Dictionary<string, PotentiometerAction> PotentiometerNotifiers =
            new Dictionary<string, PotentiometerAction>()
            {
                {"CUTOFF", InputManager.OnUpdateCutoffPos},
                {"ADSR.A", InputManager.OnUpdateAttack},
                {"ADSR.D", InputManager.OnUpdateDecay},
                {"ADSR.S", InputManager.OnUpdateSustain},
                {"ADSR.R", InputManager.OnUpdateRelease},
                {"VOLUME", InputManager.OnUpdateMasterVolume},
                {"OCTAVE", InputManager.OnUpdateOctave},
                {"BPM", InputManager.OnUpdateBpm},
                {"REVERB", InputManager.OnUpdateReverb},
                {"DELAY", InputManager.OnUpdateDelay},
            };

        private void SetPotentiometer(string info)
        {
            try
            {
                var split = info.Split(Delimiter, 2);
                string potentiometerName = split[0];
                float value = float.Parse(split[1]);
                PotentiometerNotifiers[potentiometerName](value);
            }
            catch(Exception e)
            {
                Debug.Log("Error parsing potentiometer value: " + info);
            }
  
        }

        private void InvokeKeypadEvent(string info)
        {
            var split = info.Split(Delimiter, 2);
            string key = (split[0]).Trim();
            string actionType = split[1].Trim();
            
            if (key[0] == RE_DIAL_KEY && actionType == KEY_DOWN)
            {
                InputManager.OnKeypadClearCurrentStep();
                return;
            }

            switch (actionType)
            {
                case KEY_UP:
                    InputManager.OnKeypadButtonPressed(Keypad.GetKeyFromChar(key[0]));
                    break;
                case KEY_DOWN:
                    InputManager.OnKeypadButtonReleased(Keypad.GetKeyFromChar(key[0]));
                    break;
            }
        }
    }
}