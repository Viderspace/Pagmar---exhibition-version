using Inputs;
using Scriptable_Objects;
using Synth_Variables.Scripts;
using UnityEngine;
using Utils;

namespace Synth.Sequencer.MidiButtons
{
    [CreateAssetMenu]
    public class GridNoteColor : ScriptableObject
    {
        public SequencerModeVariable SequencerMode;
       public PasswordDialingSequence PasswordDialingSequence;
        public UiColorAutoUpdate.PaletteColor SelectedFillColorOnRecord;
        public UiColorAutoUpdate.PaletteColor SelectedFillColorOnDefault;
        
        public UiColorAutoUpdate.PaletteColor NowPlayingFrameColor;
        public UiColorAutoUpdate.PaletteColor GoodFrameColor;
        public UiColorAutoUpdate.PaletteColor BadFrameColor;


        public Color GetFillColor()
        {
            return SequencerMode.Value == SynthController.SequencerState.Recording?
                DesignPalette.Colors[SelectedFillColorOnRecord] :
                DesignPalette.Colors[SelectedFillColorOnDefault];
        }
        
        public Color GetFrameColor(Keypad key, int TimeStep)
        {
            if (SequencerMode.Value == SynthController.SequencerState.Recording)
            {
                return PasswordDialingSequence.IsDialCorrect(key, TimeStep) ?
                    DesignPalette.Colors[GoodFrameColor] :
                    DesignPalette.Colors[BadFrameColor];
            }
            else
            {
                return DesignPalette.Colors[NowPlayingFrameColor];
            }
        }
    }
}