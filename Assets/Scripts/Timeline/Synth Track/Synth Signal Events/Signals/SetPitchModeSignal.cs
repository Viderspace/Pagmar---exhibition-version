using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline.Synth_Track.Synth_Signal_Events.Signals
{
    public class SetPitchModeSignal : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField] private SynthController.PitchMode pitchMode;
        public SynthController.PitchMode PitchMode => pitchMode;
        [Space(20)] [SerializeField] private bool retroactive;
        [SerializeField] private bool emitOnce;

        public PropertyName id { get; }

        public NotificationFlags flags =>
            (retroactive? NotificationFlags.Retroactive : default) |
            (emitOnce? NotificationFlags.TriggerOnce : default);
    }
}