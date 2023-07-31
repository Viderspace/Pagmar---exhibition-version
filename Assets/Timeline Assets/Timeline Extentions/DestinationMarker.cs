using System.ComponentModel;
using UnityEngine;
using UnityEngine.Timeline;

namespace Timeline_Extentions
{
    [DisplayName("Jump/DestinationMarker")]
    [CustomStyle("DestinationMarker")]
    public class DestinationMarker : Marker
    {
        [SerializeField] public bool active;
    
        [SerializeField] public SerialIdResumeEvent typeOfEvent;
        public enum SerialIdResumeEvent
        {
            RespondWhenInputHasEntered,
            RespondToBadInput,
            RespondToGoodInput,
        }
    
        void Reset() 
        {
            active = true;
        }
    }
}
