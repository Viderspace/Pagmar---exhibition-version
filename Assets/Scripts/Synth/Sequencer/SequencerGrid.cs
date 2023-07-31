using System.Collections.Generic;
using Synth_Variables.Scripts;
using Synth.Sequencer.MidiButtons;
using UnityEngine;

namespace Synth.Sequencer
{
    [CreateAssetMenu]
    public class SequencerGrid : ScriptableObject
    {
        [SerializeField] public SequencerModeVariable sequencerMode;
        [SerializeField] public GameObject buttonPrefab;
        private GameObject parent;
        private GameObject Cursor;
        private Transform lowerLeftCorner;
        private Rect bound;

        private int rows = 12;
        private int columns = 16;
        public float ButtonWidth = 1f;
        public float ButtonHeight = 1f;

        private Vector2 LowerLeftCorner => lowerLeftCorner.position;

        private readonly Dictionary<int, string> _noteNames = new()
        {
            {60, "C"}, {61, "C#"}, {62, "D"}, {63, "D#"}, {64, "E"}, {65, "F"},
            {66, "F#"}, {67, "G"}, {68, "G#"}, {69, "A"}, {70, "A#"}, {71, "B"}
        };

        private void SetGrid()
        {
            buttonPrefab.GetComponent<RectTransform>()
                .SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (bound.width / columns));
            buttonPrefab.GetComponent<RectTransform>()
                .SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bound.height / rows);
            ButtonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
            ButtonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
        }

        private void ShowCursorOnTop()
        {
            Cursor.transform.SetAsLastSibling();
        }

        public void GenerateGrid(GameObject Parent, GameObject cursor, Transform LowerleftCorner, Rect Bound)
        {
            parent = Parent;
            Cursor = cursor;
            lowerLeftCorner = LowerleftCorner;
            bound = Bound;
            SetGrid();
            for (int note = 0; note < rows; note++)
            {
                for (int timeStamp = 0; timeStamp < columns; timeStamp++)
                {
                    CreateButton(timeStamp, note);
                }
            }

            ShowCursorOnTop();
        }

        private void CreateButton(int x, int y)
        {
            var gridPosition = new Vector3(LowerLeftCorner.x + (x + .5f) * ButtonWidth,
                LowerLeftCorner.y + (y + .5f) * ButtonHeight, 0);

            GameObject newButton = Instantiate(buttonPrefab, gridPosition, Quaternion.identity, parent.transform);
            newButton.transform.SetAsFirstSibling();
            newButton.GetComponent<SequencerButton>().Init(x, y, sequencerMode);
            newButton.name = $"{_noteNames[60 + y]} Button{x}";
        }
    }
}