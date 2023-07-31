using System;
using System.Collections;
using System.Collections.Generic;
using Kernel;
using Scriptable_Objects;
using Story.Terminal.ContentCreation.TerminalText;
using Story.Terminal.System;
using UnityEngine;

namespace Story.Terminal.ContentCreation.Terminal_Operations
{
    [Serializable]
    public abstract class TerminalCommand
    {
        // private AudioFx AudioFx => Singleton.Instance.AudioFx;
        [SerializeReference] public static float DramaticallySlowSpeed = 0.2f;
        [SerializeReference] public static float NormalSpeed = 0.1f;
        [SerializeReference] public static float FastSpeed = 0.02f;
        [SerializeReference] public static float VeryFastSpeed = 0.005f;
        [SerializeReference] public static float InstantSpeed = 0.0f;
        
        public static Dictionary<TypeSpeed, float> Speeds = new Dictionary<TypeSpeed, float>()
        {
            {TypeSpeed.DramaticallySlow, DramaticallySlowSpeed},
            {TypeSpeed.Normal, NormalSpeed},
            {TypeSpeed.Fast, FastSpeed},
            {TypeSpeed.VeryFast, VeryFastSpeed},
            {TypeSpeed.Instant, InstantSpeed},
        };

        public enum TypeSpeed
        {
            DramaticallySlow,
            Normal,
            Fast,
            VeryFast,
            Instant
        }
    
        
        public enum TextColor
        {
            White,
            Red,
            Green,
            BabyBlue,
        }

        #region Template API Methods
        protected IEnumerator PauseDuration(float time)
        {
            yield return new WaitForSeconds(time);
        }

        protected void PlayTypingSound(char letter)
        {
            // AudioFx.AddKeyStroke();
        }
        
        protected IEnumerator TinyPauseBetweenLetters(TypeSpeed typeSpeed)
        {
            yield return new WaitForSeconds(Speeds[typeSpeed] + UnityEngine.Random.Range(-0.005f, 0.005f));
        }
        
        
        
        protected IEnumerator RevealText(TerminalProgramRunner terminal, string text, TypeSpeed typeSpeed, TextColor textColor)
        {
            // AudioFx.Play(AudioFx.FX.InsertNewLine);
            yield return RevealMultiLinesDivided(terminal, text, typeSpeed, textColor);
        }
        
        #endregion
        
        private IEnumerator RevealLine(ScreenBuffer buffer, string line, TypeSpeed typeSpeed, TextColor textColor)
        {
            // AudioFx.Play(AudioFx.FX.InsertNewLine);
            if (typeSpeed == TypeSpeed.Instant)
            {
                yield return RevealLineAtOnce(buffer, line, textColor);
            }
            else
            {
                yield return RevealLineCharByChar(buffer, line, typeSpeed, textColor);
            }
        }

        private IEnumerator RevealMultiLinesDivided(TerminalProgramRunner terminal, string text, TypeSpeed typeSpeed, TextColor textColor)
        {
            string allText = text;
            while (allText.Length > terminal.MaxCharsPerLine)
            {
                string line = allText.Substring(0, terminal.MaxCharsPerLine);
                allText = allText.Substring(terminal.MaxCharsPerLine);
                yield return RevealLine(terminal.CreateNewLine(), line, typeSpeed, textColor);
            }
            yield return RevealLine(terminal.CreateNewLine(), allText, typeSpeed, textColor);

        }

        private IEnumerator RevealLineAtOnce(ScreenBuffer buffer, string line, TextColor textColor)
        {
            
            buffer.AllocatedBuffer.AddText(new ColoredTerminalText(new CoreTerminalText(line), textColor));
                buffer.Screen.text = buffer.PreviousRenderedText + buffer.AllocatedBuffer.GetText();
                // AudioFx.AddKeyStroke(2);
                yield return TinyPauseBetweenLetters(TypeSpeed.Fast);
        }

        protected IEnumerator RevealLineCharByChar(ScreenBuffer buffer, string line, TypeSpeed typeSpeed, TextColor textColor)
        {
            buffer.AllocatedBuffer.AddText(new ColoredTerminalText(new CoreTerminalText(), textColor));
            foreach (char letter in line)
            {
                buffer.AllocatedBuffer.AddChar(letter);
                buffer.Screen.text = buffer.PreviousRenderedText + buffer.AllocatedBuffer.GetText();
                PlayTypingSound(letter);
                yield return TinyPauseBetweenLetters(typeSpeed);
            }
        }
        

        public abstract IEnumerator Execute(TerminalProgramRunner terminal, TerminalScreen screen);
        
    }
}