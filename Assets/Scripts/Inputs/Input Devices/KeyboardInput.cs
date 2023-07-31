using Scriptable_Objects;
using UnityEngine;

namespace Inputs.Input_Devices
{
    public class KeyboardInput : MonoBehaviour
    {
        [Header("delete = clear, p= PITCH MODE")]
        [SerializeField] private KeyCode activateFilterKey;
        bool pitchModeIsMusical = false;

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

        private void CheckNumKeys()
        {
            if (Input.GetKeyDown(KeyCode.Alpha0))
            {
                InputManager.OnKeypadButtonPressed(Keypad.Zero);
            }
            else if (Input.GetKeyUp(KeyCode.Alpha0))
            {
                InputManager.OnKeypadButtonReleased(Keypad.Zero);
            }

            else
            {
                for (int i = 0; i < numKeyCodes.Length; i++)
                {
                    if (Input.GetKeyDown(numKeyCodes[i]))
                    {
                        InputManager.OnKeypadButtonPressed(Keypad.GetKeyFromMidiNote(i));
                    }

                    if (Input.GetKeyUp(numKeyCodes[i]))
                    {
                        InputManager.OnKeypadButtonReleased(Keypad.GetKeyFromMidiNote(i));
                    }
                }
            }
        }

        private void Update()
        {
            // if (Input.anyKeyDown)
            // {
            //     print(Input.inputString);
            // }
            if (Input.GetKeyDown(KeyCode.C))
            {
                InputManager.OnUpdateClearButtonPressed();
            }


            if (Input.GetKeyDown(KeyCode.Delete))
            {
                InputManager.OnUpdateClearButtonPressed();
            }
            
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                InputManager.OnUpdateRightArrowPressed();
            }
            
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                InputManager.OnUpdateLeftArrowPressed();
            }
            
            if (Input.GetKeyDown(KeyCode.R))
            {
                InputManager.OnSequencerRecordButtonPressed();
            }
            
            if (Input.GetKeyDown(KeyCode.Space))
            {
                InputManager.OnSequencerPlayPauseButtonPressed();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                pitchModeIsMusical = !pitchModeIsMusical;
                if (pitchModeIsMusical) InputManager.OnSetPitchMode(SynthController.PitchMode.MusicalNotes);
                else InputManager.OnSetPitchMode(SynthController.PitchMode.Telephone);
            }
            CheckNumKeys();
            
            
            //
    

            //
            // if (Input.GetKeyDown(deactivateFilterKey))
            // {
            //     InputManager.OnActivateFilter();
            // }
            //
            // if (Input.GetKeyDown(activateFilterKey))
            // {
            //     InputManager.OnActivateFilter();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.T))
            // {
            //     InputManager.OnPhonePickup();
            // }
            //
            // if (Input.GetKeyDown(KeyCode.H))
            // {
            //     InputManager.OnPhoneHangup();
            // }
            //
     
            
            // if (Input.GetKeyDown(KeyCode.W))
            // {
            //     InputManager.OnSetPitchMode(SynthController.PitchMode.Telephone);
            // }
            //
   
        }
    }
}