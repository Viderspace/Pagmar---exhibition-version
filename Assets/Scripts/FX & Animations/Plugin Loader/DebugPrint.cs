using UnityEngine;

namespace Story
{
    public class DebugPrint : MonoBehaviour
    {
        [SerializeField] private bool debugMode;
        public static bool DebugMode;

        private void OnValidate()
        {
            DebugMode = debugMode;
        }

        public static void DPrint(string message)
        {
            if (DebugMode) Debug.Log(message);
        }
    }
}