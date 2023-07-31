using Inputs;
using Reaktor_Communication;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.UI;

namespace Synth.Oscillator
{
    public class WaveShapeButton : MonoBehaviour
    {
        static float _sharedInvertThreshold = 0.5f;

        [SerializeField] [Range(0, 1)] float invertThreshold = 0.5f;

        public Material sharedMaterialShader;
        public SynthController.WaveShape myWaveShapeType = SynthController.WaveShape.Sine;
        private SynthController SynthController => Singleton.Instance.SynthController;
        private WaveShapeStrategy _waveShapeStrategy;
        private Button _button;
        private Material _material;
        private MaterialPropertyBlock _materialPropertyBlock;
        private static readonly int IsOnProperty = Shader.PropertyToID("IsOn");
        private static readonly int Threshold = Shader.PropertyToID("Threshold");
        private bool IsOn { get; set; }


        #region MonoBehavior

        private void OnValidate()
        {
            _sharedInvertThreshold = invertThreshold;
            if (_material != null) UpdateShaderColor(IsOn);
        }

        private void OnEnable()
        {
            SynthController.ActiveWaveShape.ValueChanged += SelectShape;
        }

        private void OnDisable()
        {
            SynthController.ActiveWaveShape.ValueChanged -= SelectShape;
        }

        private void Awake()
        {
            _waveShapeStrategy = new WaveShapeStrategy(myWaveShapeType, this);
            _button = GetComponent<Button>();
            _material = new Material(sharedMaterialShader);
            GetComponent<Image>().material = _material;

        }

        private void Start()
        {
            _button.onClick.AddListener(AlertOnButtonClick);
            IsOn = false;
            UpdateShaderColor(false);
            InitColors();
            SelectShape(SynthController.ActiveWaveShape.Value);
        }

        private void AlertOnButtonClick()
        {
            InputManager.OnUpdateWaveshape(myWaveShapeType);
        }

        #endregion

        #region Public Methods

        public void TurnButtonOff()
        {
            if (!IsOn) return;
            IsOn = false;
            UpdateShaderColor(false);
        }

        #endregion

        #region Private Methods

        private void InitColors()
        {
            ColorBlock colorBlock = _button.colors;
            colorBlock.normalColor = Color.white;
            colorBlock.highlightedColor = Color.white;
            colorBlock.pressedColor = Color.white;
            colorBlock.selectedColor = Color.white;
            colorBlock.disabledColor = Color.white;
            _button.colors = colorBlock;
        }

        private void UpdateShaderColor(bool isOn)
        {
            _material.SetInt(IsOnProperty, isOn ? 1 : 0);
            _material.SetFloat(Threshold, _sharedInvertThreshold);
        }

        public void OnButtonPress()
        {
            IsOn = true;
            SendWaveShapeToSynth();
            UpdateShaderColor(true);
            TurnOffOtherButtons();
        }

        private void ActivateShape()
        {
            IsOn = true;
            UpdateShaderColor(true);
            SendWaveShapeToSynth();
        }

        // Legacy (debug) method
        public void OnButtonPress(SynthController.WaveShape waveShape)
        {
            if (waveShape != myWaveShapeType) return;
            OnButtonPress();
        }
        
        public void SelectShape(SynthController.WaveShape shape)
        {
            if (shape != myWaveShapeType) TurnButtonOff();
            else ActivateShape();


        }

        private void TurnOffOtherButtons()
        {
            _waveShapeStrategy.TurnOffOtherButtons();
        }

        private void SendWaveShapeToSynth()
        {
            if (ReaktorController.Instance == null) return;
            ReaktorController.Instance.SetWaveShape(_waveShapeStrategy.GetWaveShapeMessage());
        }

        #endregion
    }
}