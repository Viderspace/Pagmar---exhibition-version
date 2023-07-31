using System.Collections;

namespace Story.Terminal.ContentCreation.TerminalText
{
    public interface ITerminalText
    {
        string GetText();
        
        string AddChar(char letter);
        void Clear();
        
        void ReplaceText(string newText);
        void DeleteChar();
        
    }
}