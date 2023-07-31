using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Timeline_Extentions
{
    [DisplayName("Jump/JumpMarker")]
    [CustomStyle("JumpMarker")]
    public class JumpMarker : Marker, INotification, INotificationOptionProvider
    {
        [SerializeField] public List<DestinationMarker> destinationMarkerList = new List<DestinationMarker>()
        {
            null, null, null
        };
        [SerializeField] public DestinationMarker destinationMarker;
        [SerializeField] public DestinationMarker secondDestinationMarker;
        [SerializeField] public bool emitOnce;
        [SerializeField] public bool emitInEditor;
        private bool retroactive = false;


        [SerializeField]public int numberOfDestinations = 3;
        [SerializeField] public SerialIdPauseEvent typeOfEvent;
        [SerializeField] public bool initSequence;
    
        private const int Success = 1;
        private const int Failure = 0;
        private const int Response = 0;
        public DestinationMarker SuccessRoutine => destinationMarkerList[Success];
        public DestinationMarker FailureRoutine => destinationMarkerList[Failure];
        public DestinationMarker NextResponse => destinationMarkerList[Response];
    
        public enum SerialIdPauseEvent
        {
            HoldAndWaitForInput,
            PlayValidationProgram,
        }
    

    


        public PropertyName id { get; }
        
        NotificationFlags INotificationOptionProvider.flags =>
            (emitOnce ? NotificationFlags.TriggerOnce : default) |
            (retroactive? NotificationFlags.Retroactive: default)|
            (emitInEditor ? NotificationFlags.TriggerInEditMode : default);
    



        private void OnValidate()
        {
            // Debug.Log("JumpMarker - Selected: " + destinationMarker + " and second: " + secondDestinationMarker);
            // destinationMarkerList.Clear();

            if (destinationMarkerList.Count < numberOfDestinations)
            {
                for (int i = 0; i < numberOfDestinations - destinationMarkerList.Count ; i++)
                {
                    destinationMarkerList.Add(null);
                }
            }
        
        
            for(int i = 0; i < numberOfDestinations; i++)
            {
                Debug.Log("JumpMarker destination marker: " + i +" is : "+ destinationMarkerList[i]);

            }
        }
    }
}
