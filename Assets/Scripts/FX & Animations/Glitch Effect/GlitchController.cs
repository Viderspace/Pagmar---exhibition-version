using System;
using System.Collections;
using Inputs;
using Scriptable_Objects;
using UnityEngine;

namespace FX___Animations.Glitch_Effect
{
    [RequireComponent(typeof(Camera))]
    public class GlitchController : MonoBehaviour
    {
        [SerializeField]private GlitchEffect glitchEffect;
        public KeyCode pressToTestKey = KeyCode.G;
        public float intensity = 1f;
        
        public static GlitchController Instance { get; private set; }

        [SerializeField] private bool debugTest = false;
        
        public static event Action<bool> GlitchTriggered;

        private void Awake()
        {
            if (Instance== null)
            {
                Instance = this;
            }
        }

        private void Start()
        {
            if (glitchEffect == null)
            {
                glitchEffect = GetComponent<GlitchEffect>();
            }
        }
        

        void Update()
        {
        
            if (debugTest && Input.GetKeyDown(pressToTestKey))
            {
                InputManager.OnGlitchEffectStart();
            }
        }
    
        public void TriggerGlitch(float duration)
        {
            print("TriggerGlitch");
            StartCoroutine(GlitchEffect(duration));
        }
    
        public void TriggerGlitch()
        {
            print("TriggerGlitch");
            StartCoroutine(GlitchEffect(Singleton.Instance.AudioFx.GlitchDuration));
        }
    
        private void ActivateGlitch(bool on = true)
        {
            glitchEffect.intensity = on ? intensity : 0;
            glitchEffect.flipIntensity =  on ? intensity : 0;
            glitchEffect.colorIntensity =  on ? intensity : 0;
        }

        private IEnumerator GlitchEffect(float duration)
        {
            ActivateGlitch(true);
            Singleton.Instance.AudioFx.Play(AudioFx.FX.Glitch);
            yield return new WaitForSeconds(duration);
            ActivateGlitch(false);
        }

        public static void OnGlitchTriggered(bool intoTheMatrix)
        {
            if (Instance == null) return;
            Instance.TriggerGlitch();
            GlitchTriggered?.Invoke(intoTheMatrix);
        }
    }
}
