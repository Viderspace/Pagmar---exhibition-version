namespace Story.Terminal.ContentCreation.TerminalText
{
    /**
     * This is A Decorator Design pattern Implementation for creating dynamic text editing for the terminal
     */
    public abstract class TerminalTextDecorator : ITerminalText
    {
        protected readonly ITerminalText TerminalText;

        public TerminalTextDecorator(ITerminalText terminalText)
        {
            TerminalText = terminalText;
        }

        public abstract string GetText();
        public abstract string AddChar(char letter);
        public abstract void Clear();
        public abstract void ReplaceText(string newText);
        public abstract void DeleteChar();
    }

}