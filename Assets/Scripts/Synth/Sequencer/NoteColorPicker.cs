using System;
using System.Drawing.Imaging;
using Scriptable_Objects;
using Synth_Variables.Native_Types;
using Synth_Variables.Scripts;
using UnityEngine;
using Utils;

namespace Synth.Sequencer
{
    public static class NoteColorPicker 
    {
        private static Color fillRecord = DesignPalette.Red;
        private static Color fillSelected = DesignPalette.LightBlue;
        static Color fillUnselected =  Color.clear;
        
        private static Color strokeDefault = Color.clear;
        private static Color strokePlayTime = Color.white;
        
        
        public static Color GetFillColor(SynthController.SequencerState sequencerState, bool buttonIsSelected)
        {
            if (!buttonIsSelected)return fillUnselected;
            if (sequencerState == SynthController.SequencerState.Recording) return fillRecord;
            return fillSelected;
        }
        
        public static Color GetStrokeColor( bool isPlaying)
        {
            return isPlaying? strokePlayTime : strokeDefault;
        }
        
    }
}