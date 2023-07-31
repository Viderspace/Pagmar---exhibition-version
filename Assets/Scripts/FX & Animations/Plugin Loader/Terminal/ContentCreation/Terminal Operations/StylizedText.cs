using System;
using System.Collections;
using Story.Terminal.System;
using UnityEngine;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    public class StylizedText : TerminalCommand
    {
        [Header("Stylized Text")] [SerializeReference]
        public bool startInNewLine;

        [TextArea(5, 20)] [SerializeReference] public string text;
        [SerializeReference] public TypeSpeed typeSpeed;
        [SerializeReference] public TextColor textColor;


        public StylizedText(StylizedText otherToCopy)
        {
            text = otherToCopy.text;
            typeSpeed = otherToCopy.typeSpeed;
            textColor = otherToCopy.textColor;
            startInNewLine = otherToCopy.startInNewLine;
        }

        public StylizedText(string content = "enter text here", TypeSpeed speed = TypeSpeed.Fast,
            TextColor color = TextColor.White, bool fullLine = false)
        {
            text = content.Replace(Environment.NewLine, "");
            typeSpeed = speed;
            textColor = color;
            startInNewLine = fullLine;
        }


        public override IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen)
        {
            if (startInNewLine) yield return RevealText(terminal, text, typeSpeed, textColor);
            else yield return RevealLineCharByChar(terminal.GetLastLine()," "+ text, typeSpeed, textColor);
        }
    }
}