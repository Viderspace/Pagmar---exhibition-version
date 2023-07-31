using System.Collections.Generic;

namespace Story.Terminal.ContentCreation.TerminalText
{
    public sealed class TerminalLine : TerminalTextDecorator
    {
        public readonly LinkedList<ITerminalText> LineParts = new();

        public TerminalLine(ITerminalText terminalText) : base(terminalText)
        {
            AddText(terminalText);
        }

        public TerminalLine() : base(new CoreTerminalText())
        {
        }

        public void AddText(ITerminalText terminalText)
        {
            LineParts.AddLast(terminalText);
        }


        public override string GetText()
        {
            DebugPrint.DPrint("LINE PARTS: "+LineParts.Count);
            string line = "";
            foreach (var part in LineParts)
            {
                line += part.GetText();
            }

            return line + "\n";
        }

        public override string AddChar(char letter)
        {
            LineParts.Last.Value.AddChar(letter);
            return GetText();
        }

        public override void Clear()
        {
            LineParts.Last.Value.Clear();
        }

        /**
         * Replaces the last part of the line with the new text
         */
        public override void ReplaceText(string newText)
        {
            LineParts.Last.Value.ReplaceText(newText);
        }

        public override void DeleteChar()
        {
            LineParts.Last.Value.DeleteChar();
        }
    }
}