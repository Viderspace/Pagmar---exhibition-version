using System;

namespace Utils
{
    /// <summary>
    /// Sometimes, when you use Unity's built-in OnValidate, it will spam you with a very annoying warning message,
    /// even though nothing has gone wrong. To avoid this, you can run your OnValidate code through this utility.
    /// </summary>
    public static class ValidationUtility
    {
        /// <summary>
        /// Call this during OnValidate.
        /// Runs <paramref name="onValidateAction"/> once, after all inspectors have been updated.
        /// </summary>
        public static void SafeOnValidate(Action onValidateAction)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.delayCall += _OnValidate;

            void _OnValidate()
            {
                UnityEditor.EditorApplication.delayCall -= _OnValidate;

                onValidateAction();
            }
#endif
        }
    }
}