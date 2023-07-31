using System.Collections.Generic;
using Inputs;
using Synth.Sequencer;
using UnityEngine;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "New Synth Preset", menuName = "Synth Presets")]
    public class SynthPreset : ScriptableObject
    {
        [Header("Oscillator")] 
        public SynthController.PitchMode pitchMode = SynthController.PitchMode.MusicalNotes;
        public SynthController.WaveShape waveShape = SynthController.WaveShape.Sine; 
        public int octave = 4;
        
        public enum KeyNote
        {
            Silence = -1,
            C = 0,
            CSharp = 1,
            D = 2,
            DSharp = 3,
            E = 4,
            F = 5,
            FSharp = 6,
            G = 7,
            GSharp = 8,
            A = 9,
            ASharp = 10,
            B = 11
        }
        
        public static Dictionary<KeyNote, Keypad> KeyNotes = new Dictionary<KeyNote, Keypad>()
        {
            {KeyNote.C, Keypad.One},
            {KeyNote.CSharp, Keypad.Two},
            {KeyNote.D, Keypad.Three},
            {KeyNote.DSharp, Keypad.Four},
            {KeyNote.E, Keypad.Five},
            {KeyNote.F, Keypad.Six},
            {KeyNote.FSharp, Keypad.Seven},
            {KeyNote.G, Keypad.Eight},
            {KeyNote.GSharp, Keypad.Nine},
            {KeyNote.A, Keypad.Star},
            {KeyNote.ASharp, Keypad.Zero},
            {KeyNote.B, Keypad.Hash}
        };

        [Header("Sequencer")] 
        [SerializeField] public List<KeyNote> Sequence = new List<KeyNote>()
        {
        };


            
        public int bpm= 120;
        
        [Header("Filter")]
        public float filterOffset = 0.5f;
        public bool filterIsActive = true;
        
        [Header("ADSR")]
        [SerializeField] public Vector4 adsrValues = new Vector4();


        public void LoadPreset(SynthController synth, SequencerController sequencer= null)
        {
            synth.UpdatePitchMode(pitchMode);
            synth.UpdateWaveShape(waveShape);
            synth.UpdateOctave(octave);
            synth.UpdateBpm(bpm);
            synth.UpdateCutoffPos(filterOffset);
            synth.UpdateAttack(adsrValues.x);
            synth.UpdateDecay(adsrValues.y);
            synth.UpdateSustain(adsrValues.z);
            synth.UpdateRelease(adsrValues.w);
            
            if (sequencer != null)
            {
                sequencer.ClearGrid();
                for (int i = 0; i < Sequence.Count; i++)
                {
                    if (Sequence[i] == KeyNote.Silence)
                    {
                        sequencer.ClearCurrentStep();
                    } 
                    else sequencer.OnSelectNoteFromKeypad(KeyNotes[Sequence[i]]);
                }
            }
            
            
        }
    }
}
