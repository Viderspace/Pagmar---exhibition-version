using Synth.ADSR;
using UnityEngine;

namespace Scriptable_Objects
{
    public class SynthSnapshot
    {
        private readonly SynthController.PitchMode ActivePitchMode;
        private readonly int Bpm;
        private readonly int Octave;
        private readonly SynthController.WaveShape ActiveShape;
        private readonly float CutoffOffset;
        private readonly bool IsAdsrEnabled;
        private readonly Adsr Adsr;
        private readonly bool IsFilterEnabled;
        private readonly bool IsSequencerRunning;
        private readonly bool IsSequencerRecording;
        private string sequencerMode;

        private string PrintSequencerStatus(SynthController.SequencerState state)
        {
            var color = state == SynthController.SequencerState.Running ? "green" : state == SynthController.SequencerState.Recording ? "red" : "white";
            return $"<color={color}>{state.ToString()}</color>";
        }
    

        public SynthSnapshot(SynthController controller)
        {
            ActivePitchMode = controller.ActivePitchMode.Value;
            Bpm = controller.Bpm.Value;
            Octave = controller.Octave.Value;
            ActiveShape = controller.ActiveWaveShape.Value;
            CutoffOffset = controller.CutoffOffset.Value;
            IsAdsrEnabled = controller.AdsrOnOffSwitch.Value;
            var adsrCopy = new Vector4(controller.GlobalAdsr.Attack, controller.GlobalAdsr.Decay,
                controller.GlobalAdsr.Sustain, controller.GlobalAdsr.Release);
            Adsr = new Adsr(adsrCopy);
            IsFilterEnabled = controller.FilterOnOffSwitch.Value;
            sequencerMode = PrintSequencerStatus(controller.SequencerMode.Value);
        }

        public string Print()
        {
            string s = "Synth Snapshot:\n";
            s += $"Oscillator: Pitch-Mode: {ActivePitchMode}, Shape: {ActiveShape}, Octave: {Octave}\n";
            s += $"Sequencer: Mode:{sequencerMode}, BPM:{Bpm}\n";
            s += $"Filter: Enabled= {IsFilterEnabled},  Cutoff-Offset: {CutoffOffset}\n";
            s += $"ADSR: Enabled= {IsAdsrEnabled}, Values: (A{Adsr.Attack}|D{Adsr.Decay}|S{Adsr.Sustain}|R{Adsr.Release})\n";
            return s;
        }
    }
}