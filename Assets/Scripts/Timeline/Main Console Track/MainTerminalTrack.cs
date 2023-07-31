
using UnityEngine.Timeline;

namespace Runtime.Timeline.Main_terminal_Track
// {
//     [TrackColor(1,1,0)]
//     [TrackBindingType(typeof(TMP_Text))]
//     [TrackClipType(typeof(ConsoleTextClip))]
//     public class MainTerminalTrack: TrackAsset
//     {
//         
//     }
// }
{
    [TrackColor(1,1,0)]
    [TrackBindingType(typeof(ConsoleManager))]
    [TrackClipType(typeof(ConsoleTextClip))]
    [TrackClipType(typeof(ConsoleOramClip))]
    public class MainTerminalTrack: TrackAsset
    {
        
    }
}