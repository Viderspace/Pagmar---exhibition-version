// using Synth.Modules.Sequencer;
// using Synth.Sequencer.MidiButtons;
// using UnityEngine.UI;
// using Color = UnityEngine.Color;
//
// namespace Synth.Sequencer
// {
//     interface IMidiButton
//     {
//         public void SwitchState(MidiButtonState nextState);
//     }
//
//     public abstract class MidiButtonState : IMidiButton
//     {
//         protected MidiButtonBehaviour Context;
//         public ColorBlock ColorBlock;
//
//
//         public abstract Color GetColor();
//         public abstract float GetFlashDuration();
//
//         public abstract Color FleshColor();
//         
//
//         protected MidiButtonState(MidiButtonBehaviour context)
//         {
//             Context = context;
//         }
//
//         public abstract void SwitchState(MidiButtonState nextState);
//     }
//
//
//     public class MidiSelectedState : MidiButtonState
//     {
//         public MidiSelectedState(MidiButtonBehaviour context) : base(context)
//         {
//             ColorBlock = new ColorBlock
//             {
//                 normalColor = MidiButtonGlobalSettings.UnselectedColor, //MidiButton.unselectedColor;
//                 highlightedColor = MidiButtonGlobalSettings.SelectedColor, // <<<<<-----------
//                 pressedColor = MidiButtonGlobalSettings.PlayingColor, //MidiButton.playingColor;
//                 selectedColor = MidiButtonGlobalSettings.UnselectedColor, //MidiButton.unselectedColor;
//                 disabledColor = MidiButtonGlobalSettings.UnselectedColor //MidiButton.unselectedColor;
//             };
//         }
//
//         public override Color GetColor()
//         {
//             return MidiButtonGlobalSettings.SelectedColor;
//         }
//
//         public override float GetFlashDuration()
//         {
//             return SequencerController.Instance.NoteLength * MidiButtonGlobalSettings.FlashDurationWhenSelected;
//         }
//
//         public override Color FleshColor()
//         {
//             return Color.white;
//         }
//
//         public override void SwitchState(MidiButtonState nextState)
//         {
//             Context.currentState = nextState;
//             Context._button.colors = nextState.ColorBlock;
//         }
//     }
//
//     public class MidiUnselectedState : MidiButtonState
//     {
//         public MidiUnselectedState(MidiButtonBehaviour context) : base(context)
//         {
//             ColorBlock = new ColorBlock
//             {
//                 normalColor = MidiButtonGlobalSettings.UnselectedColor, //MidiButton.unselectedColor;
//                 highlightedColor = MidiButtonGlobalSettings.UnselectedColor, // <<<<<-----------
//                 pressedColor = MidiButtonGlobalSettings.PlayingColor, //MidiButton.playingColor;
//                 selectedColor = MidiButtonGlobalSettings.UnselectedColor, //MidiButton.unselectedColor;
//                 disabledColor = MidiButtonGlobalSettings.UnselectedColor //MidiButton.unselectedColor;
//             };
//         }
//
//         public override Color GetColor()
//         {
//             return MidiButtonGlobalSettings.UnselectedColor;
//         }
//         
//
//         public override float GetFlashDuration()
//         {
//             return SequencerController.Instance.NoteLength * MidiButtonGlobalSettings.FlashDurationWhenUnselected;
//         }
//
//         public override Color FleshColor()
//         {
//             return 0.5f * MidiButtonGlobalSettings.UnselectedColor;
//         }
//
//         public override void SwitchState(MidiButtonState nextState)
//         {
//             Context.currentState = nextState;
//             Context._button.colors = nextState.ColorBlock;
//         }
//     }
// }