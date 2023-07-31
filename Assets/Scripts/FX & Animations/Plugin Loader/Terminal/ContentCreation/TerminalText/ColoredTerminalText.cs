using Story.Terminal.ContentCreation.Terminal_Operations;
using Utils;
namespace Story.Terminal.ContentCreation.TerminalText
{
    public class ColoredTerminalText : TerminalTextDecorator
    {
        private readonly string _colorTag;

        public ColoredTerminalText(ITerminalText terminalText, TerminalCommand.TextColor colorObj) : base(terminalText)
        {
            _colorTag = "#" + DesignPalette.DepreciatedTextColors[colorObj];
        }

        public override string GetText()
        {
            return $"<color={_colorTag}>{TerminalText.GetText()}</color>";
        }

        public override string AddChar(char letter)
        {
            TerminalText.AddChar(letter);
            return GetText();
        }

        public override void Clear()
        {
        }

        public override void ReplaceText(string newText)
        {
            TerminalText.ReplaceText(newText);
        }

        public override void DeleteChar()
        {
            TerminalText.DeleteChar();
        }
    }
}