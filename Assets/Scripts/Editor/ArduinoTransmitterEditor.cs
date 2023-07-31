using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(ArduinoTransmitter))]
    public class ArduinoTransmitterEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            ArduinoTransmitter arduinoTransmitter = (ArduinoTransmitter)target;

            
            // Button to connect to the Arduino
            if (GUILayout.Button("Connect"))
            {
                arduinoTransmitter.Connect();
            }

            // Button to disconnect from the Arduino
            if (GUILayout.Button("Disconnect"))
            {
                arduinoTransmitter.Disconnect();
            }

            // Label showing the connection status
            string statusText = arduinoTransmitter.GetPortStatus() ? "Connected" : "Disconnected";
            EditorGUILayout.LabelField("Connection Status: " + statusText);
            
        }
    }
}