using System;
using System.Collections;
using Scriptable_Objects;
using Synth_Variables.Adsr;
using Synth.Modules.ADSR;
using Synth.Sequencer;
using UnityEngine;

namespace Synth.ADSR
{
    public class CvSimulator : MonoBehaviour
    {
        private void Start()
        {
            curve = new AnimationCurve();
            curve.AddKey(0, 0);
            curve.AddKey(0.1f, 1);
            curve.AddKey(0.2f, 0.5f);
            curve.AddKey(0.3f, 0.5f);
            curve.AddKey(0.4f, 0);
        }

        public AdsrVariables globalAdsr;
        public AnimationCurve curve = new AnimationCurve();
        public static float F(float x)
        {
            return MathF.Pow(2, 4 * x) / 1000;
        }

        public void adjustCurve(AdsrVariables adsr)
        {
            var aTime = F(adsr.Attack);
            var dTime = F(adsr.Decay);
            var rTime = F(adsr.Release);
            // curve.MoveKey(0, new Keyframe(0, 0));
            curve.MoveKey(1,  new Keyframe(aTime, 1));
            curve.MoveKey(2, new Keyframe(aTime+dTime, adsr.Sustain));
            curve.MoveKey(3,  new Keyframe(SequencerController.Instance.NoteLength , adsr.Sustain));
            curve.MoveKey(4,  new Keyframe(SequencerController.Instance.NoteLength + rTime, 0));
        }

        public AnimationCurve GetCv()
        {
            adjustCurve(globalAdsr);
            // StartCoroutine(EnvelopeTrigger(envelope, adsr));
            return curve;
        }

        public IEnumerator EnvelopeTrigger(EnvelopeCurve envelopeCurve, Adsr adsr)
        {

            var attackSecs = MathF.Min(F(adsr.Attack), SequencerController.Instance.NoteLength);
            var decaySecs = MathF.Min(F(adsr.Decay), SequencerController.Instance.NoteLength - attackSecs);
            decaySecs = MathF.Max(0, decaySecs);
            var sustain = adsr.Sustain;
            var sustainSecs = SequencerController.Instance.NoteLength - attackSecs - decaySecs;
            sustainSecs = MathF.Max(0, sustainSecs);
            sustainSecs = MathF.Max(0, sustainSecs);
            var releaseSecs = F(adsr.Release);


            var time = 0f;
            while (time < attackSecs)
            {
                envelopeCurve.Value = time / attackSecs;
                time += Time.deltaTime;
                yield return null;
            }

            envelopeCurve.Value = 1;
            time = 0f;
            while (time < decaySecs)
            {
                envelopeCurve.Value = 1 - (1 - sustain) * (time / decaySecs);
                time += Time.deltaTime;
                yield return null;
            }

            envelopeCurve.Value = sustain;


            if (sustainSecs > 0) yield return new WaitForSeconds(sustainSecs);

            time = 0f;
            while (time < releaseSecs)
            {
                envelopeCurve.Value = sustain - sustain * (time / releaseSecs);
                time += Time.deltaTime;
                yield return null;
            }
            
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                adjustCurve(globalAdsr);
            }
        }
    }

    public class EnvelopeCurve
    {
        public float Value { get; set; } = 0;

        // public EnvelopeCurve(AdsrDynamicValues.AdsrValues adsr = null, bool autoStart = true)
        // {
        // }
    }
}