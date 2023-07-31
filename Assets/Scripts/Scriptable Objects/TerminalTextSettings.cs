using System;
using System.Collections.Generic;
using Story.Terminal.ContentCreation.Terminal_Operations;
using Synth_Variables;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;

namespace Scriptable_Objects
{
    [CreateAssetMenu(fileName = "Terminal Text Settings")]
    public class TerminalTextSettings : ScriptableObject
    {
        [SerializeField] private FloatVariable dramaticallySlowSpeed;
        // [SerializeField] private float dramaticallySlowSpeed = 0.2f;
        [SerializeField] private FloatVariable normalSpeed;// = 0.15f;
        [SerializeField] private FloatVariable fastSpeed ;//= 0.12f;
        [SerializeField] private FloatVariable veryFastSpeed;// = 0.5f;
        [SerializeField] private float instantSpeed = 0.0f;
        [SerializeField] [Range(0, 1)] private float blinkingSpeed;
        [SerializeField] private char blinkingChar = '█';
        [SerializeField] private float pauseWhenInsertingNewLine = .3f;
        [SerializeField] private float pauseWhenReplacingTheText = .5f;
        [SerializeField] public DesignPalette.TextColor oramPrefixColor = DesignPalette.TextColor.LightBlue;

        private void UpdateDramaticallySlowSpeed(float val)=> _dramaticallySlowSpeed = val;
        private void UpdateNormalSpeed(float val)=> _normalSpeed = val;
        private void UpdateFastSpeed(float val)=> _fastSpeed = val;
        private void UpdateVeryFastSpeed(float val)=> _veryFastSpeed = val;

        private static float _dramaticallySlowSpeed = 0.2f;
        private static float _normalSpeed = 0.1f;
        private static float _fastSpeed = 0.05f;
        private static float _veryFastSpeed = 0.01f;
        private static float _instantSpeed = 0.0f;

        private void OnEnable()
        {
            dramaticallySlowSpeed.ValueChanged += UpdateDramaticallySlowSpeed;
            normalSpeed.ValueChanged += UpdateNormalSpeed;
            fastSpeed.ValueChanged += UpdateFastSpeed;
            veryFastSpeed.ValueChanged += UpdateVeryFastSpeed;
        }

        public void Setup()
        {
            dramaticallySlowSpeed.ValueChanged += UpdateDramaticallySlowSpeed;
            normalSpeed.ValueChanged += UpdateNormalSpeed;
            fastSpeed.ValueChanged += UpdateFastSpeed;
            veryFastSpeed.ValueChanged += UpdateVeryFastSpeed;
            dramaticallySlowSpeed.ResetToDefault();
            normalSpeed.ResetToDefault();
            fastSpeed.ResetToDefault();
            veryFastSpeed.ResetToDefault();
        }

        public float PauseWhenInsertingNewLine => pauseWhenInsertingNewLine;
        public float PauseWhenReplacingTheText => pauseWhenReplacingTheText;
        
        public float BlinkingSpeed => blinkingSpeed;
        public char BlinkingChar => blinkingChar;

        // private void OnValidate()
        // {
        //     _dramaticallySlowSpeed = dramaticallySlowSpeed;
        //     _normalSpeed = normalSpeed;
        //     _fastSpeed = fastSpeed;
        //     _veryFastSpeed = veryFastSpeed;
        //     _instantSpeed = instantSpeed;
        // }
        //
   
        

        public Dictionary<TypeSpeed, float> Speeds = new Dictionary<TypeSpeed, float>()
        {
            {TypeSpeed.DramaticallySlow, _dramaticallySlowSpeed},
            {TypeSpeed.Normal, _normalSpeed},
            {TypeSpeed.Fast, _fastSpeed},
            {TypeSpeed.VeryFast, _veryFastSpeed},
            {TypeSpeed.Instant, _instantSpeed},
        };
        
        public static float VariantSpeed(TypeSpeed originalSpeed)
        {
            if (originalSpeed == TypeSpeed.Instant) return 0.0f;
            var ogSpeed = Speed(originalSpeed);
            return ogSpeed;//+ Random.Range(-ogSpeed, ogSpeed)*0.05f;
         
        }

        public enum TypeSpeed
        {
            DramaticallySlow,
            Normal,
            Fast,
            VeryFast,
            Instant
        }

        public float GetSpeed(TypeSpeed typeSpeed)
        {
            return Speeds[typeSpeed];
        }
        
        public static float Speed(TypeSpeed typeSpeed)
        {
            switch (typeSpeed)
            {
                case TypeSpeed.Instant:
                    return _instantSpeed;
                case TypeSpeed.VeryFast:
                    return _veryFastSpeed;
                case TypeSpeed.Fast:
                    return _fastSpeed;
                case TypeSpeed.Normal:
                    return _normalSpeed;
                case TypeSpeed.DramaticallySlow:
                    return _dramaticallySlowSpeed;
                default:
                    return 0f;
            }
        }


    }
}