// using extOSC;
// using extOSC.UI;
// using Inputs.Input_Devices;
// using Reaktor_Communication;
// using Scriptable_Objects;
// using Synth.ADSR;
// using Synth.Modules.Sequencer;
// using Synth.Oscillator;
// using UnityEngine;
//
// namespace Inputs
// {
//     public class IpadOscReceiver : MonoBehaviour
//     {
//         private OSCReceiver _receiver;
//
//
//         [SerializeField] private OSCRotary adsrA;
//         [SerializeField] private OSCRotary adsrD;
//         [SerializeField] private OSCRotary adsrS;
//         [SerializeField] private OSCRotary adsrR;
//
//         [SerializeField] private SequencerController sequencerController;
//
//         [SerializeField] private WaveShapeButton square;
//         [SerializeField] private WaveShapeButton sine;
//         [SerializeField] private WaveShapeButton triangle;
//         [SerializeField] private WaveShapeButton saw;
//
//         [SerializeField] private FilterWindowAnimator filterWindowAnimator;
//
//     
//
//         #region Control Methods
//
//         private void ActivateFilter(OSCMessage msg)
//         {
//             bool value = msg.Values[0].FloatValue > 0;
//             InputManager.OnActivateFilter(value);
//         }
//
//         private void SetAttack(OSCMessage msg)
//         {
//             adsrA.Value = msg.Values[0].FloatValue;
//             InputManager.OnUpdateAttack( msg.Values[0].FloatValue);
//
//         }
//
//         private void SetDecay(OSCMessage msg)
//         {
//             adsrD.Value = msg.Values[0].FloatValue;
//             InputManager.OnUpdateDecay( msg.Values[0].FloatValue);
//         }
//
//         private void SetSustain(OSCMessage msg)
//         {
//             adsrS.Value = msg.Values[0].FloatValue;
//             InputManager.OnUpdateSustain( msg.Values[0].FloatValue);
//         }
//
//         private void SetRelease(OSCMessage msg)
//         {
//             adsrR.Value = msg.Values[0].FloatValue;
//             InputManager.OnUpdateRelease( msg.Values[0].FloatValue);
//         }
//
//         private void PlayPauseSequencer(OSCMessage msg)
//         {
//             bool value = msg.Values[0].FloatValue > 0;
//             // sequencerCursor.ToggleRun(value);
//             InputManager.OnUpdateSequencerRun(value);
//         }
//
//         private void WaveShapeSelect(OSCMessage msg)
//         {
//             var value = msg.Values[0].FloatValue;
//             SynthData.WaveShape waveshape;
//             switch (value)
//             {
//                 case < 0.25f:
//                     // square.OnButtonPress();
//                     waveshape = SynthData.WaveShape.Square;
//                     break;
//                 case >= .25f and < .5f:
//                     // sine.OnButtonPress();
//                     waveshape = SynthData.WaveShape.Sine;
//                     break;
//                 case >= .5f and < .75f:
//                     // triangle.OnButtonPress();
//                     waveshape = SynthData.WaveShape.Triangle;
//                     break;
//                 case >= .75f:
//                     // saw.OnButtonPress();
//                     waveshape = SynthData.WaveShape.Sawtooth;
//                     break;
//                 default:
//                     waveshape = SynthData.DefaultWaveShape;
//                     break;
//             }
//             InputManager.OnUpdateWaveshape(waveshape);
//         }
//
//         private void SetMasterVol(OSCMessage ipadMsg)
//         {
//             var value = Mathf.Clamp(ipadMsg.Values[0].FloatValue, 0, 1);
//             // ReaktorController.Instance.SetMasterVolume(value);
//             InputManager.OnUpdateMasterVolume(value);
//         }
//
//
//         private void CutoffPos(OSCMessage msg)
//         {
//             // filterWindowAnimator.SetCutoffOffset(msg.Values[0].FloatValue);
//             InputManager.OnUpdateCutoffPos(msg.Values[0].FloatValue);
//         }
//
//         private void KeypadKeyPress(OSCMessage msg)
//         {
//             var value = msg.Values[0].IntValue;
//             // KeypadSequencerController.Instance.OnSelectNoteFromKeypad(KeypadNoteValues[value]);
//             InputManager.OnKeypadButtonPressed(Keypad.GetKeyFromOscValue(value));
//         }
//
//         private void KeypadMoveRight(OSCMessage msg)
//         {
//             // KeypadSequencerController.Instance.MoveRight();
//             InputManager.OnUpdateRightArrowPressed();
//         }
//
//         private void KeypadMoveLeft(OSCMessage msg)
//         {
//             // KeypadSequencerController.Instance.MoveLeft();
//             InputManager.OnUpdateLeftArrowPressed();
//         }
//
//         private void KeypadClearGrid(OSCMessage msg)
//         {
//             // KeypadSequencerController.Instance.ClearGrid();
//             InputManager.OnUpdateClearButtonPressed();
//         }
//     
//         private void UpdateBpm(OSCMessage value)
//         {
//             var normalizedBpm = Mathf.FloorToInt(Mathf.Lerp(SynthData.MinBpm,SynthData.MaxBpm,value.Values[0].FloatValue));
//             // BpmInput.Instance.UpdateBpm(normalizedBpm);
//             InputManager.OnUpdateBpm(normalizedBpm);
//         }
//     
//         private void ShowDialer(OSCMessage msg)
//         {
//             // HideRevealDialerPanel.Instance.Show(msg.Values[0].FloatValue > 0);
//             InputManager.OnUpdateSequencerIsActive(msg.Values[0].FloatValue > 0);
//         }
//     
//         public void UpdateOctave(OSCMessage value)
//         {
//             var normalizedBpm = Mathf.FloorToInt(Mathf.Lerp(SynthData.MinOctave,SynthData.MaxOctave,value.Values[0].FloatValue));
//             // OctaveInput.Instance.UpdateOctave(normalizedBpm);
//             InputManager.OnUpdateOctave(normalizedBpm);
//         }
//
//         #endregion
//
//
//         // Start is called before the first frame update
//         void Start()
//         {
//             _receiver = ReaktorController.Instance.Receiver;
//         
//             _receiver.Bind(IpadOscAddresses.AttackAddress, SetAttack);
//             _receiver.Bind(IpadOscAddresses.DecayAddress, SetDecay);
//             _receiver.Bind(IpadOscAddresses.SustainAddress, SetSustain);
//             _receiver.Bind(IpadOscAddresses.ReleaseAddress, SetRelease);
//         
//             _receiver.Bind(IpadOscAddresses.BpmAddress, UpdateBpm);
//             _receiver.Bind(IpadOscAddresses.OctaveAddress, UpdateOctave);
//             _receiver.Bind(IpadOscAddresses.WaveshapeAddress, WaveShapeSelect);
//         
//             _receiver.Bind(IpadOscAddresses.FilterOnOffAddress, ActivateFilter);
//             _receiver.Bind(IpadOscAddresses.CutoffPosAddress, CutoffPos);
//         
//             _receiver.Bind(IpadOscAddresses.PlayPauseAddress, PlayPauseSequencer);
//             _receiver.Bind(IpadOscAddresses.MasterVolAddress, SetMasterVol);
//             _receiver.Bind(IpadOscAddresses.HideShowDialerAddress, ShowDialer);
//
//             _receiver.Bind(IpadOscAddresses.KeypadAddress, KeypadKeyPress);
//             _receiver.Bind(IpadOscAddresses.KeypadRightAddress, KeypadMoveRight);
//             _receiver.Bind(IpadOscAddresses.KeypadLeftAddress, KeypadMoveLeft);
//             _receiver.Bind(IpadOscAddresses.KeypadClearAddress, KeypadClearGrid);
//         }
//     }
// }