using System;

namespace Inputs.Input_Devices
{
    [Serializable]
    public class IpadOscAddresses
    {
        //ADSR
        public static string AttackAddress { get; private set; } = "/AdsrA";
        public static string DecayAddress  { get; private set; } = "/AdsrD";
        public static string SustainAddress  { get; private set; } = "/AdsrS";
        public static string ReleaseAddress  { get; private set; } = "/AdsrR";


        // Dialer CTRLS
        public static string HideShowDialerAddress  { get; private set; } = "/ShowDialer";
        public static string BpmAddress  { get; private set; } = "/BPM";
        public static string PlayPauseAddress  { get; private set; } = "/SequencerRun";

        //oscillator
        public static string OctaveAddress  { get; private set; } = "/Oct";
        public static string WaveshapeAddress  { get; private set; } = "/WaveSel";
        public static string MasterVolAddress  { get; private set; } = "/MasterVol";

        //filter
        public static string FilterOnOffAddress  { get; private set; } = "/CutoffOnOff";
        public static string CutoffPosAddress  { get; private set; } = "/CutoffPos";

        // KEYPAD
        public static string KeypadAddress  { get; private set; } = "/Keypad";
        public static string KeypadClearAddress  { get; private set; } = "/Keypad/Clear";
        public static string KeypadLeftAddress  { get; private set; } = "/Keypad/L";
        public static string KeypadRightAddress  { get; private set; } = "/Keypad/R";
    }
}