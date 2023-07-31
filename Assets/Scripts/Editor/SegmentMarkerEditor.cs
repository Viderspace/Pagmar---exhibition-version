// using Timeline.Markers;
// using UnityEditor;
// using UnityEditor.Timeline;
// using UnityEngine;
// using UnityEngine.Timeline;
//
// namespace Editor
// {
//     [CustomTimelineEditor(typeof(SegmentMarker))]
//     public class SegmentMarkerEditor : MarkerEditor
//     {
//         
//         public override MarkerDrawOptions GetMarkerOptions(IMarker marker)
//         {
//             var segmentMarker = marker as SegmentMarker;
//             if (segmentMarker != null)
//             {
//                 return new MarkerDrawOptions { tooltip = segmentMarker.title };
//             }
//             return base.GetMarkerOptions(marker);
//         }
//         
//         public override void DrawOverlay(IMarker marker, MarkerUIStates uiState, MarkerOverlayRegion region)
//         {
//             var segmentMarker = marker as SegmentMarker;
//             if (segmentMarker == null) return;
//             
//             var markerRegion = region.markerRegion;
//             var timelineRegion = region.timelineRegion;
//             const float k_LineOverlayWidth = 6.0f;
//             float markerRegionCenter = markerRegion.xMin + (markerRegion.width - k_LineOverlayWidth) / 2.0f;
//             Rect lineRect = new Rect(markerRegionCenter,
//                 timelineRegion.y,
//                 k_LineOverlayWidth,
//                 timelineRegion.height);
//            EditorGUI.Toggle(lineRect, segmentMarker.showLineOverlay);
//         }
//     }
// }