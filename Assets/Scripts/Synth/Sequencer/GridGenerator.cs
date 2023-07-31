using System.Collections.Generic;
using Synth.Sequencer.MidiButtons;
using UnityEngine;

namespace Synth.Sequencer
{
    public class GridGenerator : MonoBehaviour
    {
        public SequencerGrid sequencerGrid;
        public GameObject buttonPrefab;
        public int rows = 12;
        public int columns = 16;
        public float unitWidth = 1f;
        public float unitHeight = 1f;

        public float UnitWidth => bound.width*3f / columns; //sequencerGrid.ButtonWidth;
        public float UnitHeight => sequencerGrid.ButtonHeight;
        public Transform lowerLeftCorner;
        public Rect bound;

        private Vector2 LowerLeftCorner => lowerLeftCorner.position;

 

        private readonly Dictionary<int, string> _noteNames = new Dictionary<int, string>()
        {
            {60, "C"}, {61, "C#"},{62, "D"}, {63, "D#"},{64, "E"}, {65, "F"},
            {66, "F#"}, {67, "G"},{68, "G#"}, {69, "A"},{70, "A#"}, {71, "B"}
        };

        private void Awake()
        {
            bound = GetComponent<RectTransform>().rect;
            // GenerateGrid();
            
     
            SetGrid();
            sequencerGrid.GenerateGrid(gameObject, GameObject.Find("Cursor"),lowerLeftCorner, bound);
        }
        

        private void ShowCursorOnTop()
        {
            GameObject cursor = GameObject.Find("Cursor");
            cursor.transform.SetAsLastSibling();
        }

        private Vector3 GetGridPosition(int x, int y)
        {
            return new Vector3(LowerLeftCorner.x + (x+.5f) * unitWidth, LowerLeftCorner.y + (y+.5f) * unitHeight, 0);
        }

        private void CreateButton(int x, int y)
        {
            var gridPosition = new Vector3(LowerLeftCorner.x + (x+.5f) * unitWidth, LowerLeftCorner.y + (y+.5f) * unitHeight, 10);
            
            GameObject newButton = Instantiate(buttonPrefab, gridPosition, Quaternion.identity, transform);
            newButton.transform.SetAsFirstSibling();
            // newButton.GetComponent<SequencerButton>().Init(x,y);
            newButton.name = $"{_noteNames[60+y]} Button{x}";
        }
        
        private void SetGrid()
        {
            buttonPrefab.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (bound.width/columns));
            buttonPrefab.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bound.height/rows);
            unitWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
            unitHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
        }

        private void GenerateGrid()
        {
            buttonPrefab.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, (bound.width/columns));
            buttonPrefab.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, bound.height/rows);
            unitWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
            unitHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
            
            
            for (int y = 0; y < rows; y++)
            {
                for (int x = 0; x < columns; x++)
                {
                    CreateButton(x,y);
                }
            }
            ShowCursorOnTop();
        }

    }
}