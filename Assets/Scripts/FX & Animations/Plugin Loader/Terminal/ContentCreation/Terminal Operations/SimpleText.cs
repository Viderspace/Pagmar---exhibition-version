using System;
using System.Collections;
using Story.Terminal.System;
using UnityEngine;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    class SimpleText : TerminalCommand
    {
        [Header("SimpleText")]
        [SerializeReference] public bool startInNewLine;
        [TextArea(5, 20)] [SerializeReference]
        public string text;
        [SerializeReference] public TypeSpeed typeSpeed;
        
        public SimpleText(string content = "text goes here", 
            TypeSpeed speed = TypeSpeed.VeryFast, bool fullLine = true)
        {
            startInNewLine = fullLine;
            text = content.Replace(Environment.NewLine, "");
            typeSpeed = speed;
        }

        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            if (startInNewLine) yield return RevealText(terminal, text, typeSpeed, TextColor.White);
            else yield return RevealLineCharByChar(terminal.GetLastLine()," "+ text, typeSpeed, TextColor.White);
            // string allText = text;
            // while (allText.Length > terminal.MaxCharsPerLine)
            // {
            //     string line = allText.Substring(0, terminal.MaxCharsPerLine);
            //     allText = allText.Substring(terminal.MaxCharsPerLine);
            //     yield return RevealLine(terminal.CreateNewLine(), line, typeSpeed, TextColor.White);
            // }
            // yield return RevealLine(terminal.CreateNewLine(), allText, typeSpeed, TextColor.White);
        }
    }
}