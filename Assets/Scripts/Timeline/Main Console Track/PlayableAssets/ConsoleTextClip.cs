using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Playables;
using Utils;

namespace Runtime.Timeline.Main_terminal_Track
{
    public class ConsoleTextClip: PlayableAsset
    {
        public enum InsertionMode
        {
            Replace,
            InsertInNewLine,
            AppendInSameLine
        }
        [Header("Terminal Text Settings")]
        [TextArea(3,10)]
        [SerializeField] private string text;
        [SerializeField] private InsertionMode insertionMode;
        [SerializeField] public DesignPalette.TextColor textColor = DesignPalette.DefaultTextColor;
        [SerializeField] public TerminalTextSettings.TypeSpeed typeSpeed = TerminalTextSettings.TypeSpeed.Normal;
        
        public string MakeText()
        {
            if (textColor == DesignPalette.DefaultTextColor) return text;
            
            var colorTagPrefix = "#" + DesignPalette.TextColors[textColor];
            return $"<color={colorTagPrefix}>{text}</color>";
        }

        public List<bool> GetKeystrokeMap()
        {
            List<bool> map = new List<bool>();
            string debugmsg = "";
            for(int i=0; i< text.Length; i++)
            {
                debugmsg += $"({i})({text[i]}:{(!char.IsSeparator(text[i])?"Y":"N")}), ";
                map.Add(!char.IsSeparator(text[i]));
            }
            map.Add(false);
            map.Add(false);
            map.Add(false);
            Debug.Log(debugmsg);
            return map;
        }


        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            if (Application.isPlaying == false) return Playable.Null;

            var playable = ScriptPlayable<ConsoleTextBehaviour>.Create(graph);
            ConsoleTextBehaviour textBehaviour = playable.GetBehaviour();
            textBehaviour.KeystrokeMap = GetKeystrokeMap();
            textBehaviour.InsertionMode = insertionMode;
            textBehaviour.TextField = MakeText();
            textBehaviour.typeSpeed = typeSpeed;
            return playable;
        }
    }
}