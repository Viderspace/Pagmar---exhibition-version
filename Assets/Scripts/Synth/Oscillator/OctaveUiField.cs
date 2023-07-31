using System.Collections;
using Inputs;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;

namespace Synth.Oscillator
{
    public class OctaveUiField : MonoBehaviour
    {
        [SerializeField] private IntVariable globalOctave;
        [SerializeField] private TMP_InputField _inputField;

        public void InitSettings()
        {
            _inputField.text = TextPattern(globalOctave.Value);
        }

        private void OnEnable()
        {
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onEndEdit.AddListener(UpdateOctave);
            globalOctave.ValueChanged += UpdateOctave;
            // SynthController.OnOctaveChanged += UpdateOctave;
        }

        private void OnDisable()
        {
            globalOctave.ValueChanged -= UpdateOctave;
            // SynthController.OnOctaveChanged -= UpdateOctave;
        }

        private string TextPattern(int octVal)
        {
            return $"OCT {octVal}";
        }

        private IEnumerator ErrMassage()
        {
            _inputField.text = "ERR";
            yield return new WaitForSeconds(1.5f);
            _inputField.text = TextPattern(globalOctave.Value);
        }

        public void UpdateOctave(int octaveValue)
        {
            _inputField.text = TextPattern(octaveValue);
        }

        // Legacy (Debug) method
        private void UpdateOctave(string input)
        {
            if (int.TryParse(input, out int octave))
            {
                octave = (octave < globalOctave.Min) ?globalOctave.Min : octave;
                octave = (octave > globalOctave.Max) ? globalOctave.Max : octave;
                InputManager.OnUpdateOctave(octave);
                _inputField.text = TextPattern(octave);
            }
            else
            {
                StartCoroutine(ErrMassage());
            }
        }
    }
}