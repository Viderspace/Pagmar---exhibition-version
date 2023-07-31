using System;
using System.Collections.Generic;
using Story.Terminal.ContentCreation.Terminal_Operations;
using Story.Terminal.ContentCreation.TerminalText;
using TMPro;
using UnityEngine;

namespace Story.Terminal.System
{
    public class ScreenBuffer
    {
        public TMP_Text Screen;
        public string PreviousRenderedText;
        public readonly TerminalLine AllocatedBuffer;
        public int MaxCharsPerLine;

        public ScreenBuffer(TMP_Text screen, string previousRenderedText, TerminalLine allocatedBuffer, int maxChars)
        {
            Screen = screen; // update this screen
            PreviousRenderedText = previousRenderedText; // with this text prior to the update
            AllocatedBuffer = allocatedBuffer; // and this text after the update (will be saved for next update)
            MaxCharsPerLine = maxChars;
        }
    }


    public class TerminalScreen : MonoBehaviour
    {
        public static Color DefaultColor = Color.white;

        public int maxLines = 15;
        public int maxCharsPerLine = 10;

        [SerializeField] private TMP_Text _textMesh;

        // private LinkedList<TextSegment> _lineSegments = new();
        private LinkedList<TerminalLine> _lines = new();

        private void Start()
        {
            _textMesh = GetComponent<TMP_Text>();
            // _textMesh.ForceMeshUpdate();
        }

        public void ClearEverything()
        {
            _lines.Clear();
            _textMesh.text = string.Empty;
        }


        private void Fifo()
        {
            DebugPrint.DPrint("Lines: " + _lines.Count);
            // if (_textMesh.isTextOverflowing)
            // {
            //     _lines.RemoveFirst();
            // }
            if (_lines.Count > maxLines)
            {
                var trim = _textMesh.textInfo.lineCount - maxLines;
                for (int i = 0; i < trim; i++)
                    _lines.RemoveFirst();
            }
        }

        public void InsertCharCommand(char letter)
        {
            if (letter.ToString() == Environment.NewLine) return;
            if (_lines.Count == 0)
            {
                _lines.AddLast(new TerminalLine(new CoreTerminalText(letter.ToString())));
            }
            else
            {
                _lines.Last.Value.AddChar(letter);
            }

            RenderTerminal();
        }

        public void DeleteCharCommand()
        {
            _lines.Last.Value.Clear();
            RenderTerminal();
        }


        public void MakeSegment(TerminalCommand.TextColor color, bool finishWithNewLine = false)
        {
            Fifo();
            ITerminalText newTerminalText = new CoreTerminalText();
            if (color != TerminalCommand.TextColor.White)
                newTerminalText = new ColoredTerminalText(newTerminalText, color);

            if (!finishWithNewLine)
            {
                if (_lines.Count == 0) _lines.AddLast(new TerminalLine());
                _lines.Last.Value.AddText(newTerminalText);
                return;
            }

            var newLine = new TerminalLine(newTerminalText);
            _lines.AddLast(newLine);
            Fifo();
        }
        
        public ScreenBuffer GetLastLine()
        {
            if (_lines.Count == 0) return CreateNewLine();

            Fifo();
            string previousRenderedText = GetRenderedTextWithoutLastLine();
            return new ScreenBuffer(_textMesh, previousRenderedText,_lines.Last.Value , maxCharsPerLine);
        }


        public ScreenBuffer CreateNewLine()
        {
            Fifo();
            var newLine = new TerminalLine();
            _lines.AddLast(newLine);
            string previousRenderedText = GetRenderedText();
            return new ScreenBuffer(_textMesh, previousRenderedText, newLine, maxCharsPerLine);
        }

        public void TypeUserPassword(char letter)
        {
            if (_lines.Count == 0) return;
            var lastSegment = _lines.Last.Value.LineParts.Last.Value;
            var placeholder = lastSegment.GetText();
            for (int i = 0; i < placeholder.Length; i++)
            {
                if (placeholder[i] != '_') continue;
                placeholder = placeholder.Remove(i, 1);
                placeholder = placeholder.Insert(i, letter.ToString());
                break;
            }
            lastSegment.ReplaceText(placeholder);
            RenderTerminal();
            
            // _lines.Last.Value.AddChar(letter);
            //
            // string previousRenderedText = GetRenderedText();
            // return new ScreenBuffer(_textMesh, previousRenderedText, _lines.Last.Value, maxCharsPerLine);
        }

        // {
        //     Fifo();
        //     var newLine = new TerminalLine();
        //     _lines.AddLast(newLine);
        //     string previousRenderedText = GetRenderedText();
        //     return new ScreenBuffer(_textMesh, previousRenderedText, newLine, maxCharsPerLine);
        // }


        private void RenderTerminal()
        {
            _textMesh.text = GetRenderedText();
            // for (var line = _lines.First; line != null; line = line.Next)
            // {
            //     if (line.Next != null)
            //     {
            //         _textMesh.text += line.Value.GetText();
            //         continue;
            //     }
            //
            //     _textMesh.text += line.Value.GetText().Replace(Environment.NewLine, "");
            // }
        }

        private string GetRenderedText()
        {
            var renderedText = string.Empty;
            for (var line = _lines.First; line != null; line = line.Next)
            {
                if (line.Next != null)
                {
                    renderedText += line.Value.GetText();
                    continue;
                }

                renderedText += line.Value.GetText().Replace(Environment.NewLine, "");
            }
            return renderedText;
        }
        
        private string GetRenderedTextWithoutLastLine()
        {
            if (_lines.Count == 0) return string.Empty;
            var renderedText = string.Empty;
            for (var line = _lines.First; line != null; line = line.Next)
            {
                if (line.Next != null)
                {
                    renderedText += line.Value.GetText();
                    continue;
                }
                // renderedText += line.Value.GetText().Replace(Environment.NewLine, "");
            }
            return renderedText;
        }

        // private static string GetRenderedSegment(TextSegment segment)
        // {
        //     string preTag = string.Empty, postTag = string.Empty, text = segment.Text;
        //     if (segment.ColorTag != TextSegment.DefaultColor)
        //     {
        //         string color = "#" +ColorUtility.ToHtmlStringRGBA(segment.ColorTag);
        //         preTag = $"<color={color}>";
        //         postTag = "</color>";
        //     }
        //
        //     if (segment.IsFullLine)
        //     {
        //         text = segment.Text + "\n";
        //     }
        //
        //     if (segment.Blink)
        //     {
        //         text += "_";
        //     }
        //     else
        //     {
        //         text = text.Replace("_", "");
        //     }
        //
        //     return preTag + text + postTag;
        // }

        // public void Blink(bool blink)
        // {
        //     if (_lineSegments.Count == 0)
        //     {
        //         _textMesh.text = blink ? "_" : string.Empty;
        //         return;
        //     }
        //
        //     _textMesh.text = _textMesh.text.Replace("_", "");
        //     var last = _lineSegments.Last.Value;
        //     last.Blink = blink;
        //     _lineSegments.Last.Value = last;
        //     RenderTerminal();
        // }
    }
}