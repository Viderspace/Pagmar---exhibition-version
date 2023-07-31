// using Synth;
// using UnityEditor;
//
// namespace Editor
// {
//     [CustomEditor(typeof(SynthControlPanel))]
//     public class SynthControlPanelEditor : UnityEditor.Editor
//     {
//         public override void OnInspectorGUI()
//         {
//             // base.OnInspectorGUI();
//             var synthControlPanel = (SynthControlPanel) target;
//     
//             
//             EditorGUILayout.LabelField("Synth Components", EditorStyles.boldLabel);
//             EditorGUILayout.BeginHorizontal();
//             synthControlPanel.OscillatorToggle = EditorGUILayout.Toggle("Oscillator", synthControlPanel.OscillatorToggle);
//             synthControlPanel.SequencerToggle = EditorGUILayout.Toggle("Sequencer", synthControlPanel.SequencerToggle);
//             synthControlPanel.FilterToggle = EditorGUILayout.Toggle("Filter", synthControlPanel.FilterToggle);
//             synthControlPanel.AdsrToggle = EditorGUILayout.Toggle("Adsr", synthControlPanel.AdsrToggle);
//             EditorGUILayout.EndHorizontal();
//             
//             
//             EditorGUILayout.Space();
//             // base.OnInspectorGUI();
//         }
//         
//     }
// }