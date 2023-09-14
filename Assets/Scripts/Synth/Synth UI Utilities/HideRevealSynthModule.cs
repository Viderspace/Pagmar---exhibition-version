using System;
using System.Collections;
using System.Collections.Generic;
using Synth_Variables.Native_Types;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace Synth.Synth_UI_Utilities
{
    [ExecuteInEditMode]
    public class HideRevealSynthModule : MonoBehaviour
    {
        [SerializeField] public string moduleName;
        [SerializeField] public ToggleVariable OnOffSwitch;
        [SerializeField] public List<UiToggleObject> ComponentsToToggle = new List<UiToggleObject>();

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
            print("ToggleOnOff UI Event: " +(on? "ON " : "OFF ") + moduleName);
            if (Application.isPlaying)
            {
                StartCoroutine(GradualReveal(on));
                return;
            }
            
            
            foreach (var component in ComponentsToToggle)
            {
                bool visible = component.visibleWhenOn ? on : !on;
                component.objectToToggle.SetActive(visible);
            }
        }

        public IEnumerator GradualReveal(bool on)
        {
            foreach (var component in ComponentsToToggle)
            {
                bool visible = component.visibleWhenOn ? on : !on;
                component.objectToToggle.SetActive(visible);
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
    
    
    
    [Serializable]
    public struct UiToggleObject
    {
        public GameObject objectToToggle;
        public bool visibleWhenOn;
    }
}