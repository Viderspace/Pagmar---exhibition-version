using System.Collections.Generic;
using System.Linq;
using Timeline_Extentions;
using UnityEditor;
using UnityEditor.Timeline;
using UnityEngine.Timeline;

namespace Editor
{
    [CustomEditor(typeof(JumpMarker))]
    public class JumpMarkerInspector : UnityEditor.Editor
    {
        const string k_TimeLabel = "Timeline will jump at time {0}";
        const string k_NoJumpLabel = "{0} is deactivated.";
        const string k_AddMarker = "No Destination Marker has been found on this track. Add one to use this marker.";
        const string k_JumpTo = "Jump to";
        const string k_None = "None";

        SerializedProperty m_DestinationMarkerList;
        SerializedProperty m_DestinationMarker;
        SerializedProperty m_SecondDestinationMarker;
        SerializedProperty m_EmitOnce;
        SerializedProperty m_EmitInEditor;
        SerializedProperty m_Time;
        SerializedProperty m_NumberOfDestinations;
        SerializedProperty m_TypeOfEvent;
        SerializedProperty m_initSequence;

        void OnEnable()
        {
            m_DestinationMarkerList = serializedObject.FindProperty("destinationMarkerList");
            m_DestinationMarker = serializedObject.FindProperty("destinationMarker");
            m_SecondDestinationMarker = serializedObject.FindProperty("secondDestinationMarker");
            m_EmitOnce = serializedObject.FindProperty("emitOnce");
            m_EmitInEditor = serializedObject.FindProperty("emitInEditor");
            m_Time = serializedObject.FindProperty("m_Time");
            m_NumberOfDestinations = serializedObject.FindProperty("numberOfDestinations");
            m_TypeOfEvent = serializedObject.FindProperty("typeOfEvent");
            m_initSequence = serializedObject.FindProperty("initSequence");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var marker = target as JumpMarker;
            int numberOfDestinations = 1;
            if (marker != null)
            { 
                numberOfDestinations = marker.numberOfDestinations;
            }
        
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                EditorGUILayout.PropertyField(m_Time);
                EditorGUILayout.Space();
                EditorGUILayout.PropertyField(m_NumberOfDestinations);
                EditorGUILayout.PropertyField(m_TypeOfEvent);
            
                var destinationMarkers = DestinationMarkersFor(marker);
                if (!destinationMarkers.Any())
                    DrawNoJump();
                else
                    DrawJumpOptions(destinationMarkers, numberOfDestinations);

                if (changeScope.changed)
                {
                    serializedObject.ApplyModifiedProperties();
                    TimelineEditor.Refresh(RefreshReason.ContentsModified);
                }
            }
        }

        void DrawNoJump()
        {
            EditorGUILayout.HelpBox(k_AddMarker, MessageType.Info);
        
            using (new EditorGUI.DisabledScope(true))
            {
                EditorGUILayout.Popup(k_JumpTo, 0, new[]{ k_None });
                EditorGUILayout.PropertyField(m_EmitOnce);
                EditorGUILayout.PropertyField(m_EmitInEditor);
                EditorGUILayout.PropertyField(m_initSequence);

            }
        }
    
        void DrawJumpOptions(List<DestinationMarker> destinationMarkers, int numberOfDestinations)
        {
            for(int i = 0; i < numberOfDestinations; i++)
            {
                var currentMarker = DrawDestinationPopup(destinationMarkers, i );
                  DrawTimeLabel(currentMarker);
                  // if (destinationMarkers.Contains(currentMarker))
                  // {
                  //     destinationMarkers.Remove(currentMarker);
                  // }
            }
            
            // var destinationMarker = DrawDestinationPopup(destinationMarkers);
            // DrawTimeLabel(destinationMarker);
            //
            // if (destinationMarkers.Contains(destinationMarker))
            // {
            //     destinationMarkers.Remove(destinationMarker);
            // }
            // var secondDestinationMarker = DrawSecondDestinationPopup(destinationMarkers);
            // DrawTimeLabel(secondDestinationMarker);
            EditorGUILayout.PropertyField(m_EmitOnce);
            EditorGUILayout.PropertyField(m_EmitInEditor);
            EditorGUILayout.PropertyField(m_initSequence);
            
        }

        DestinationMarker DrawDestinationPopup(IList<DestinationMarker> destinationMarkers, int elementIndex)
        {
            var popupIndex = 0;
            var ithElementRef = m_DestinationMarkerList.GetArrayElementAtIndex(elementIndex);
            var destinationMarkerIndex = destinationMarkers.IndexOf(ithElementRef.objectReferenceValue as DestinationMarker);
            if (destinationMarkerIndex != -1)
                popupIndex = destinationMarkerIndex + 1;

            DestinationMarker destinationMarker = null;
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                var newIndex = EditorGUILayout.Popup(k_JumpTo, popupIndex, GeneratePopupOptions(destinationMarkers).ToArray());
            
                if (newIndex > 0)
                    destinationMarker = destinationMarkers.ElementAt(newIndex - 1);

                if (changeScope.changed)
                    ithElementRef.objectReferenceValue = destinationMarker;
            }
        
            return destinationMarker;
        }
        
        DestinationMarker DrawSecondDestinationPopup(IList<DestinationMarker> destinationMarkers)
        {
            var popupIndex = 0;
            var destinationMarkerIndex = destinationMarkers.IndexOf(m_SecondDestinationMarker.objectReferenceValue as DestinationMarker);
            if (destinationMarkerIndex != -1)
                popupIndex = destinationMarkerIndex + 1;

            DestinationMarker destinationMarker = null;
            using (var changeScope = new EditorGUI.ChangeCheckScope())
            {
                var newIndex = EditorGUILayout.Popup(k_JumpTo, popupIndex, GeneratePopupOptions(destinationMarkers).ToArray());
            
                if (newIndex > 0)
                    destinationMarker = destinationMarkers.ElementAt(newIndex - 1);

                if (changeScope.changed)
                    m_SecondDestinationMarker.objectReferenceValue = destinationMarker;
            }
        
            return destinationMarker;
        }

        static void DrawTimeLabel(DestinationMarker destinationMarker)
        {
            if (destinationMarker != null)
            {
                if (destinationMarker.active)
                    EditorGUILayout.HelpBox(string.Format(k_TimeLabel, destinationMarker.time.ToString("0.##")), MessageType.Info);
                else
                    EditorGUILayout.HelpBox(string.Format(k_NoJumpLabel, destinationMarker.name), MessageType.Warning);
            }
        } 
     
        
        
        
        
    
        static List<DestinationMarker> DestinationMarkersFor(Marker marker)
        {
            var destinationMarkers = new List<DestinationMarker>();
            var parent = marker.parent;
            if (parent != null)
                destinationMarkers.AddRange(parent.GetMarkers().OfType<DestinationMarker>().ToList());

            return destinationMarkers;
        }

        static IEnumerable<string> GeneratePopupOptions(IEnumerable<DestinationMarker> markers)
        {
            yield return k_None;

            foreach (var marker in markers)
            {
                yield return marker.name;
            }
        }
    }
}