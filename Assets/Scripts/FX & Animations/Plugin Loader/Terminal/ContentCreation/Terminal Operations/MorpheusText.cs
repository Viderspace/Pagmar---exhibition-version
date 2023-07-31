using System;
using System.Collections;
using Story.Terminal.System;
using UnityEngine;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    public class MorpheusText : TerminalCommand
    {
        private static string morpheusPrefixTag = "<Oram> : ";
        [Header("Morpheus Text message")] public bool startInNewLine = true;
        [TextArea(3, 10)] public string morpheusMassage;
        public TypeSpeed _speed;
        public TextColor prefixTagColor = TextColor.Green;

        public float GetPlayDuration()
        {
            return Speeds[_speed] * morpheusMassage.Length;
        }


        public MorpheusText(string morpheusMassage = "Wake up neo", TypeSpeed speed = TypeSpeed.Normal,
            bool startInNewLine = true,
            TextColor prefixTagColor = TextColor.Green)
        {
            this.morpheusMassage = morpheusMassage;
            this._speed = speed;
            this.startInNewLine = startInNewLine;
            this.prefixTagColor = prefixTagColor;
        }


        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            yield return new StylizedText(morpheusPrefixTag, TypeSpeed.Instant, prefixTagColor, startInNewLine).Execute(
                terminal, terminal.MainScreen);
            yield return new StylizedText(morpheusMassage, _speed, TextColor.White, false).Execute(terminal,
                terminal.MainScreen);
        }
    }
}