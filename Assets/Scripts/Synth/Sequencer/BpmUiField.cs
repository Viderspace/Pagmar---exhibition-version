using System.Collections;
using Inputs;
using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;

namespace Synth.Sequencer
{
    /// <summary>
    /// Deprecated
    /// </summary>
    public class BpmUiField : MonoBehaviour
    { 
        [SerializeField] IntVariable globalBpm;
        
        private TMP_InputField _inputField;

        private void OnEnable()
        {
            globalBpm.ValueChanged += UpdateBpm;
            _inputField = GetComponent<TMP_InputField>();
            _inputField.onEndEdit.AddListener(UpdateBpmFromUi);
        }
        
        private void OnDisable()
        {
            globalBpm.ValueChanged -= UpdateBpm;
        }

        private void Start()
        {
            UpdateBpm(globalBpm.Value);
        }


        private string TextPattern(int bpmVal)
        {
            return $"BPM {bpmVal}";
        }


        private IEnumerator ErrMassage()
        {
            _inputField.text = "ERR";
            yield return new WaitForSeconds(1.5f);
            _inputField.text = TextPattern(globalBpm.Value);
        }


        public void UpdateBpm(int bpm)
        {
            _inputField.text = TextPattern(bpm);
        }


        // Legacy (Debug) method
        private void UpdateBpmFromUi(string input)
        {
            if (int.TryParse(input, out int newBpm))
            {
                newBpm = (newBpm < globalBpm.Min) ?  globalBpm.Min : newBpm;
                newBpm = (newBpm >  globalBpm.Max) ? globalBpm.Max : newBpm;
                InputManager.OnUpdateBpm(newBpm);
                _inputField.text = TextPattern(newBpm);
            }
            else
            {
                StartCoroutine(ErrMassage());
            }
        }
    }
}