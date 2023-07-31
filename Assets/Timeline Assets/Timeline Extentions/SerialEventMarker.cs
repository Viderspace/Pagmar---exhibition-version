using UnityEngine;
using UnityEngine.Events;

namespace Timeline_Extentions
{
    public class SerialEventMarker : JumpMarker
    {
        [SerializeField] public UnityEvent onEvent;
    }
}