using System;
using System.Collections;
using Scriptable_Objects;
using Synth_Variables.Adsr;
using Synth.ADSR;
using Synth.Sequencer.MidiButtons;
using UnityEngine;

namespace Synth.Modules.ADSR
{
    public class AdsrTrail : MonoBehaviour
    {
        public AdsrVariables globalAdsr;
        [SerializeField] private Transform trailDot;
        public SynthController SynthController => Singleton.Instance.SynthController;
        public Vector2[] points = new Vector2[5];
        private Task _task;

        static int Apoint = 1;
        static int Dpoint = 2;
        static int Spoint = 3;
        static int Rpoint = 4;

        private void OnDrawGizmos()
        {
            if (points == null || points.Length < 5) return;
            Gizmos.color = Color.green;
            Gizmos.DrawLine(points[0], points[1]);
            Gizmos.DrawLine(points[1], points[2]);
            Gizmos.DrawLine(points[2], points[3]);
            Gizmos.DrawLine(points[3], points[4]);


            // drawing lines between each 2 points,  to make the shape of the adsr
            // DrawGizmo.GetCustomAttribute()
        }

        private void Start()
        {
            Init();
        }

        private void OnEnable()
        {
            globalAdsr.OnAdsrValuesChanged += UpdateCurve;
            SequencerButton.NoteLaunch += StartAnimation;

        }
        private void OnDisable()
        {
            globalAdsr.OnAdsrValuesChanged -= UpdateCurve;
            SequencerButton.NoteLaunch -= StartAnimation;
        }

        private void StartAnimation()
        {
            if (_task != null && _task.Running)
            {
                _task.Stop();
            }

            _task = new Task(FollowPath(trailDot));
        }

        private void Init()
        {
            points[0] = new Vector2(0, 0);
            points[1] = new Vector2(0.2f, 1);
            points[2] = new Vector2(0.4f, .5f);
            points[3] = new Vector2(0.67f, .5f);
            points[4] = new Vector2(1f, 0f);
            // _path.bezierPath = new BezierPath(points, false, PathSpace.xy);
            // _path.bezierPath.ControlPointMode = BezierPath.ControlMode.Free;
        }

        IEnumerator FollowPath(Transform trailObject)
        {
            var time = 0f;
            var attackTime = SynthController.GlobalAdsr.AttackSecs();
            var decayTime = SynthController.GlobalAdsr.DecaySecs();
            var releaseTime = SynthController.GlobalAdsr.ReleaseSecs();
            var susTime = Sequencer.SequencerController.Instance.NoteLength - attackTime - decayTime;

            // Moving through the path of the first 4 points, total time = adsTime
            while (time < attackTime)
            {
                trailObject.position = Vector2.Lerp(points[0], points[1], time / attackTime);
                time += Time.deltaTime;
                yield return null;
            }

            trailObject.position = points[1];
            time = 0;
            while (time < decayTime)
            {
                trailObject.position = Vector2.Lerp(points[1], points[2], time / decayTime);
                time += Time.deltaTime;
                yield return null;
            }

            trailObject.position = points[2];
            time = 0;
            while (time < susTime)
            {
                trailObject.position = Vector2.Lerp(points[2], points[3], time / susTime);
                time += Time.deltaTime;
                yield return null;
            }

            trailObject.position = points[3];
            time = 0;
            while (time < releaseTime)
            {
                trailObject.position = Vector2.Lerp(points[3], points[4], time / releaseTime);
                time += Time.deltaTime;
                yield return null;
            }

            trailObject.position = points[4];
            yield return null;
        }


  

        private void UpdateCurve(AdsrVariables adsr)
        {
            // Set Attack, Decay & Sustain x position
            var normAttack = adsr.Attack * 0.33f;
            var normDecay = adsr.Decay * 0.34f; //  MathF.Min( adsr.Decay * 0.34f, SequencerCursor.Instance.NoteLength);
            SetAttack(normAttack);
            SetDecay(normAttack + normDecay, adsr.Sustain);
            SetRelease(0.67f + adsr.Release * 0.33f);
        }

        private void SetAttack(float attack)
        {
            points[Apoint] = new Vector3(attack, 1);
        }

        private void SetDecay(float decay, float sustainLevel)
        {
            points[Dpoint] = new Vector3(decay, sustainLevel);
            points[Spoint] = new Vector3(0.67f, sustainLevel);
        }

        private void SetRelease(float release)
        {
            points[Rpoint] = new Vector3(release, 0);
        }
    }
}