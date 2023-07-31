
namespace Story.Terminal.ContentCreation.TerminalText
{
    public class CoreTerminalText : ITerminalText
    {
        private string _text;
        private float _typeSpeed;
        private bool _isFullLine;
        private bool _typeEnabled;
        

        public CoreTerminalText()
        {
            _text = string.Empty;
        }

        public CoreTerminalText(string text)
        {
            _text = text;
        }

        public string GetText()
        {
            return _text;
        }

        public string AddChar(char letter)
        {
            _text += letter;
            return _text;
        }

        public void Clear()
        {
            _text = "";
        }

        public void ReplaceText(string newText)
        {
            _text = newText;
        }

        public void DeleteChar()
        {
            if (_text.Length > 0)
                _text = _text.Remove(_text.Length - 1);
        }
    }
}