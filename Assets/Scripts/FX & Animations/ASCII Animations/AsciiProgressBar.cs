using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace ASCII_Animations
{
    public class AsciiProgressBar : MonoBehaviour
    {
        [SerializeField] private float timePassed = 0f;
        public int width = 50;
        public string frameColorHex = "#FFFFFF"; // Default to white
        public string fillColorHex = "#FF0000"; // Default to red
        public float defaultDuration = 5f; // Duration of the progress bar animation
        public Color frameColor;
        public Color waveColor;
        public char fillChar = '░';
        public char emptyChar = ' ';
        public char frameCharLeft = '▐';
        public char frameCharRight = '▌';
        private Coroutine _coroutine;

        public List<char> fillChars = new List<char>()
        {
            '▁', '▂', '▃', '▄', '▅', '▆', '▇', '█', '▀', '▔', '▏', '▎', '▍', '▌', '▋', '▊', '▉', '▐',
            '▕', '▖', '▗', '▘', '▙', '▚', '▛', '▜', '▝', '▞', '▟', '░', '▒', '▓', '⎕', '='
        };
        // private string enterserialPrompt = "  ▁ ▂ ▃ ▄ ▅ ▆ ▇ █ ▀ ▔ ▏ ▎ ▍ ▌ ▋ ▊ ▉ ▐ ▕ ▖ ▗ ▘ ▙ ▚ ▛ ▜ ▝ ▞ ▟ ░ ▒ ▓ ⎕ ⍂  ● ○ ◯ ◔ ◕ ◶ ◌ ◉ ◎ ◦ ◆ ◇ 0 0 1 2 3 4 5 6 7 8 9 ₀ ₁ ₂ ₃ ₄ ₅ ₆ ₇ ₈ ₉ ⁰ ¹ ² ³ ⁴ ⁵ ⁶ ⁷ ⁸ ⁹ ⟦ ⌈ ⌊ ⌉ ⌋  __ _  ∎";


        private TextMeshProUGUI textMeshPro;

        private void OnValidate()
        {
            frameColorHex = "#" + ColorUtility.ToHtmlStringRGB(frameColor);
            fillColorHex = "#" + ColorUtility.ToHtmlStringRGB(waveColor);
        }


        private void Awake()
        {
            textMeshPro = GetComponent<TextMeshProUGUI>();
            if (textMeshPro == null)
            {
                Debug.LogError("Missing TextMeshProUGUI component!");
            }
        }

        private void AbortAndResetOnHangup()
        {
            if (_coroutine != null)
            {
                StopCoroutine(_coroutine);
            }
            textMeshPro.text = "";
        }
        
        
        public void StartProgressBar(float duration= 0f)
        {
            if (duration > 0f)
            {
                _coroutine = StartCoroutine(AnimateProgressBar(duration));
            }
            else
            {
                _coroutine = StartCoroutine(AnimateProgressBar(defaultDuration));
            }
        }

        public void StopAndHideProgressBar()
        {
            AbortAndResetOnHangup();
        }


        public void StartProgressBarFromSignal()
        {
            StartProgressBar(5f);
        }

        private IEnumerator AnimateProgressBar(float duration)
        {
            StringBuilder line1 = new StringBuilder(width + 2); // to build our ASCII art
            StringBuilder line2 = new StringBuilder(width + 2); // to build our ASCII art

            float time = 0f; // time variable to animate the progress bar
            while (time < duration)
            {
                line1.Clear(); // clear previous frame
                line2.Clear(); // clear previous frame

                line1.Append($"<color={frameColorHex}>{frameCharLeft}</color>");
                line2.Append($"<color={frameColorHex}>{frameCharLeft}</color>");
                float completionRatio = time / duration;
                int filledLength = (int) (width * completionRatio);
                int halfFilled = filledLength == 0 ? 0 : 1;
                line1.Append($"<color={fillColorHex}>{new string(fillChar, filledLength - halfFilled)}</color>");
                line2.Append($"<color={fillColorHex}>{new string(fillChar, filledLength - halfFilled)}</color>");

                if (halfFilled > 0)
                {
                    line1.Append($"<color={fillColorHex}>{'░'}</color>");
                    line2.Append($"<color={fillColorHex}>{'░'}</color>");
                }

                line1.Append(new string(emptyChar, width - filledLength));
                line2.Append(new string(emptyChar, width - filledLength));

                line1.Append($"<color={frameColorHex}>{frameCharRight}</color>");
                line2.Append($"<color={frameColorHex}>{frameCharRight}</color>");

                textMeshPro.text = line1.ToString() + " " + completionRatio.ToString("0%");
                // + "\n" + line2; // set the text

                time += Time.deltaTime; // increment the time
                timePassed = time;
                yield return null;
            }

            string full =
                $"<color={frameColorHex}>{frameCharLeft}</color><color={fillColorHex}>{new string(fillChar, width)}</color><color={frameColorHex}>{frameCharRight}</color>"+" " + 1.ToString("0%");
                // + "\n" +
                // $"<color={frameColorHex}>{frameCharLeft}</color><color={fillColorHex}>{new string(fillChar, width)}</color><color={frameColorHex}>{frameCharRight}</color>";
            textMeshPro.text = full;
        }
        //
        // public void OnNotify(Playable origin, INotification notification, object context)
        // {
        //     if(notification is ShowProgBarSigEmitter showProgSignal)
        //     {
        //         if (showProgSignal.signalType == ShowProgBarSigEmitter.SignalType.Show)
        //         {
        //             StartProgressBar(showProgSignal.duration);
        //         }
        //         else
        //         {
        //             StopAndHideProgressBar();
        //         }
        //     }
        // }
    }
}