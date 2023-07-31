using System;
using System.Collections.Generic;

namespace Inputs
{
    [Serializable]
    public class Keypad
    {
        public int MidiValue { get; private set; }
        public int ReaktorValue { get; private set; }
        public string Name { get; private set; }


        private Keypad(int midiNote, int reaktorValue, string name = "")
        {
            MidiValue = midiNote;
            ReaktorValue = reaktorValue;
            Name = name;
            
        }

        public static Keypad GetKeyFromMidiNote(int midiNote)
        {
            return SortedByMidiNotes[midiNote];
        }
        public static Keypad GetKeyFromOscValue(int midiNote)
        {
            return SortedByOscValues[midiNote];
        }

        public static Keypad GetKeyFromChar(char c)
        {
            return GetKeyFromName(c.ToString());
        }
        
        public static Keypad GetKeyFromName(string name)
        {
            foreach (var keypad in SortedByMidiNotes)
            {
                if (keypad.Name == name)
                {
                    return keypad;
                }
            }
            return null;
        }
        
        public static readonly Keypad One = new Keypad(0, 1, "1");
        public static readonly Keypad Two = new Keypad(1, 2,"2");
        public static readonly Keypad Three = new Keypad(2, 3, "3");
        public static readonly Keypad Four = new Keypad(3, 4, "4");
        public static readonly Keypad Five = new Keypad(4, 5, "5");
        public static readonly Keypad Six = new Keypad(5, 6, "6");
        public static readonly Keypad Seven = new Keypad(6, 7, "7");
        public static readonly Keypad Eight = new Keypad(7, 8, "8");
        public static readonly Keypad Nine = new Keypad(8, 9, "9");
        public static readonly Keypad Star = new Keypad(9, 10, "*");
        public static readonly Keypad Zero = new Keypad(10, 11 , "0");
        public static readonly Keypad Hash = new Keypad(11, 12, "#" );
        public static readonly Keypad Silence = new Keypad(-1, -1, "Silence");
        private static readonly List<Keypad> SortedByMidiNotes = new List<Keypad>()
        {
            One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Star, Zero, Hash
        };
        private static readonly List<Keypad> SortedByOscValues = new List<Keypad>()
        {
            null, One, Two, Three, Four, Five, Six, Seven, Eight, Nine, Star, Zero, Hash
        };
    }

}