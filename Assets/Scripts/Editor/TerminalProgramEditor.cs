using UnityEditor;
using UnityEngine;



namespace Story.Terminal.ContentCreation
{

#if UNITY_EDITOR
    [CustomEditor(typeof(TerminalProgram))]
    public class TerminalProgramEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var script = (TerminalProgram) target;

            if (GUILayout.Button("Add stylized text", GUILayout.Height(40)))
            {
                script.AddStylizedText();
            }
            if (GUILayout.Button("Add simple text", GUILayout.Height(40)))
            {
                script.AddSimpleText();
            }
            if (GUILayout.Button("Add Pause time", GUILayout.Height(30)))
            {
                script.AddPause();
            }
            if (GUILayout.Button("Add new line", GUILayout.Height(30)))
            {
                script.AddNewline();
            }
            if (GUILayout.Button("Add user input", GUILayout.Height(40)))
            {
                script.AddInputRoutine();
            }
            if (GUILayout.Button("Add large Text File", GUILayout.Height(40)))
            {
                script.AddTextFile();
            }
            if (GUILayout.Button("Add Morpheus Massage", GUILayout.Height(40)))
            {
                script.AddMorpheusText();
            }
            if (GUILayout.Button("Add Clear Screen", GUILayout.Height(40)))
            {
                script.AddClearScreen();
            }
        }
    }
#endif
}

