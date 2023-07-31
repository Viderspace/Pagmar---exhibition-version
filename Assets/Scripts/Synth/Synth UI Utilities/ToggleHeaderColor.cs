using Synth_Variables.Native_Types;
using TMPro;
using UnityEngine;
using Utils;

namespace Synth.Synth_UI_Utilities
{
    [ExecuteInEditMode]
    public class ToggleHeaderColor : MonoBehaviour
    {
        [SerializeField] private ToggleVariable OnOffSwitch;
        [SerializeField] private  TMP_Text TextComponent;
        [SerializeField] private string headerText;
        public Color OnColor;
        public Color OffColor;
        

        private void OnEnable()
        {
            OnOffSwitch.ValueChanged += ToggleOnOff;
        }

        private void OnDisable()
        {
            OnOffSwitch.ValueChanged -= ToggleOnOff;
        }

        private void Start()
        {
            ToggleOnOff(OnOffSwitch.DefaultValue);
        }

        private void ToggleOnOff(bool on)
        {
            string onColor = ColorUtility.ToHtmlStringRGB( OnColor);
            string offColor = DesignPalette.TransparentWhiteHex;
            TextComponent.text = $"<color=#{(on?onColor:offColor)}>{headerText}</color>";
        }
    }
}