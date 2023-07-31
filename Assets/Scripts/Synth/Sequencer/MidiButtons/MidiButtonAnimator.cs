// using System.Collections;
// using Scriptable_Objects;
// using Synth.ADSR;
// using Synth.Modules.ADSR;
// using UnityEngine;
// using UnityEngine.UI;
//
// namespace Synth.Sequencer.MidiButtons
// {
//     public class MidiButtonAnimator : MonoBehaviour
//     {
//         [SerializeField] private AnimationCurve adsrAnimationCurve = new AnimationCurve();
//         private SynthController SynthController => Singleton.Instance.SynthController;
//
//         private ButtonState _buttonState;
//         private Button _button;
//
//         public void Init(ButtonState buttonState, GridNoteColor manager)
//         {
//             _manager = manager;
//
//             _button = GetComponent<Button>();
//             _buttonState = buttonState;
//             InitColors();
//         }
//
//         private MidiButtonColorManager _manager;
//         
//
//
//         private void InitColors()
//         {
//             ColorBlock colorBlock = _button.colors;
//             colorBlock.normalColor = MidiButtonGlobalSettings.UnselectedColor;
//             // colorBlock.highlightedColor = MidiButtonGlobalSettings.SelectedColor;
//             colorBlock.highlightedColor = _manager.GetFillColor();
//
//             colorBlock.pressedColor = _manager.GetFillColor();
//             colorBlock.selectedColor = MidiButtonGlobalSettings.UnselectedColor;
//             colorBlock.disabledColor = MidiButtonGlobalSettings.UnselectedColor;
//             _button.colors = colorBlock;
//         }
//
//
//         public void DoFleshAnimation()
//         {
//             StartCoroutine(FlashAnimation(_buttonState.FlashColor));
//         }
//
//         public void DoResetAnimation()
//         {
//             StartCoroutine(FlashAnimation(MidiButtonGlobalSettings.ResetColor));
//         }
//
//         // private Keyframe[] SetAnimationCurveWithAdsrParams()
//         // {
//         //     var adsr = AdsrDynamicValues.Instance.Adsr;
//         //     var attTime = adsr.Attack*SequencerCursor.Instance.NoteLength;
//         //     var decTime = adsr.Decay*SequencerCursor.Instance.NoteLength;
//         //     var relTime = adsr.Release*SequencerCursor.Instance.NoteLength;
//         //     return new[]
//         //     {
//         //         new Keyframe(0, 0),
//         //         new Keyframe(adsr.Attack, 1),
//         //         new Keyframe(adsr.Attack + adsr.Decay, adsr.Sustain),
//         //         new Keyframe(SequencerCursor.Instance.NoteLength, adsr.Sustain),
//         //         new Keyframe(SequencerCursor.Instance.NoteLength + adsr.Release, 0)
//         //     };
//         // }
//
//         private IEnumerator FlashAnimation(Color flashColor)
//         {
//             ColorBlock colorBlock = _button.colors;
//             Color currentColor = _buttonState.CurrentColor;
//             float flashDuration = _buttonState.FlashDuration;
//             float endTime = Time.time + flashDuration;
//             if (MidiButtonGlobalSettings.FlashAccordingToAdsr)
//             {
//                 while (Time.time < endTime)
//                 {
//                     // Old Way = Simple Lerp
//                     colorBlock.normalColor = Vector4.Lerp(
//                         flashColor,
//                         currentColor,
//                         (endTime - Time.time) / flashDuration);
//                     _button.colors = colorBlock;
//                     yield return null;
//                 }
//             }
//             else
//             {
//                 colorBlock.normalColor = flashColor;
//                 _button.colors = colorBlock;
//                 yield return new WaitForSeconds(flashDuration);
//             }
//
//             colorBlock.normalColor = currentColor;
//             _button.colors = colorBlock;
//             yield return null;
//         }
//     }
// }