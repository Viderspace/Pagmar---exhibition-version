using System.Collections;
using System.Text;
using Inputs;
using TMPro;
using UnityEngine;

namespace ASCII_Animations
{
    public class AsciiWaveform : MonoBehaviour
    {
        [SerializeField] TMP_Text textComponent;
        public string hexFrameColor = "#FFFFFF";
        public string hexWaveColor = "#FFFFFF";
        public Color frameColor;
        public Color waveColor;
        public float speed = 1f;
        public float hz = 2;
        public int width = 52;
        public int height = 9;
        private Coroutine animationCoroutine;
        private Task _task;
        private string waveChar = "*";
        // private string coloredWaveChar =   $"<color={hexWaveColor}>" + waveChar + "</color>";


        public float thickness = 1f;


        private void OnValidate()
        {
            hexFrameColor = "#" + ColorUtility.ToHtmlStringRGB(frameColor);
            hexWaveColor = "#" + ColorUtility.ToHtmlStringRGB(waveColor);
        }


        private void Awake()
        {
            textComponent = GetComponent<TMP_Text>();
            textComponent.text = "";
        }

        // private void Start()
        // {
        //     animationCoroutine = StartCoroutine(AnimateSineWave());
        // }
    
        public void ShowAsciiWave()
        {
            textComponent.text = "";
            _task = new Task(AnimateSineWave());
        }
    
        public void HideAsciiWave()
        {
            if (_task!= null)_task.Stop();
            textComponent.text = "";
        }

        private void OnEnable()
        {
            InputManager.PhoneHangup += HideAsciiWave;
        }
    
        private void OnDisable()
        {
            InputManager.PhoneHangup -= HideAsciiWave;
        }


        private string CheckEdges(int x, int y)
        {
            if ((y == -1 || y == height) && (x == -1 || x == width)) // corner frame
            {
                return $"<color={hexFrameColor}>" + "+" + "</color>";
            
            }

            if (y == -1 || y == height) // frame
            {
                return $"<color={hexFrameColor}>" + "-" + "</color>";
            }

            if (x == -1 || x == width)
            {
                return $"<color={hexFrameColor}>" + "|" + "</color>";
            }

            return NullChar;
        }

        private const string NullChar = "\0";

        private IEnumerator AnimateSineWave()
        {
            while (true)
            {
                StringBuilder sb = new StringBuilder((width + 2) * (height + 2)); // to build our ASCII art
                float time = 0f; // time variable to animate the sine wave
                var stop = 10f;
                while (true)
                {
                    sb.Clear(); // clear previous frame

                    for (int y = height; y >= -1; y--)
                    {
                        for (int x = -1; x <= width; x++)
                        {
                            var isFrameChar = CheckEdges(x, y);
                            if (isFrameChar != NullChar)
                            {
                                sb.Append(isFrameChar);
                            }
                            else
                            {
                      

                                float sineValue = Mathf.Sin((x + time) * 2 * Mathf.PI / width * hz); // get the sine value
                                float sineY = Mathf.Lerp(0, height, sineValue * 0.5f + 0.5f); // map it to our grid height
                                var dist = Mathf.Abs(sineY - y);
                                if (dist >= thickness) sb.Append(' '); // if it's outside the thickness, draw a space
                                else
                                    sb.Append($"<color={hexWaveColor}>" + waveChar +
                                              "</color>"); // otherwise draw a wave character
                            }
                        }

                        if (y > -1) sb.Append('\n'); // newline unless it's the last line
                    }

                    textComponent.text = sb.ToString(); // set the text
                    if (time > stop * speed)
                    {
                        break;
                    }

                    time += Time.fixedDeltaTime * speed; // increment the time
                    yield return new WaitForFixedUpdate();
                }
                textComponent.text = "";
            }
       
        }
    }
}