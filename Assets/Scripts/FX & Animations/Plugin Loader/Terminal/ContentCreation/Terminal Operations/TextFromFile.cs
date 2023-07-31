using System;
using System.Collections;
using System.Linq;
using Story.Terminal.System;
using UnityEngine;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    public class ReadTextFromFile : TerminalCommand
    {
        [Header("Text File")]
        public TextAsset file;
        
        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            var fileLines = file.text.Split('\n' ).ToList();
            foreach (var textLine in fileLines)
            {
                var text = (textLine ==Environment.NewLine ? string.Empty : textLine);
                yield return new SimpleText(text, TypeSpeed.Instant).Execute(terminal, screen);
            }
        }
    }
}