using System.Collections.Generic;
using Scriptable_Objects;
using UnityEngine;
using UnityEngine.Playables;
using Utils;

namespace Runtime.Timeline.Main_terminal_Track
{
    public class ConsoleOramClip : PlayableAsset
    {
        [Header("Oram Text Settings")]
        [TextArea(3,10)]
        [SerializeField] private string oramMassage;

        private const ConsoleTextClip.InsertionMode InsertionMode = ConsoleTextClip.InsertionMode.Replace;
        private const string OramPrefixTag = "<Oram>: ";
        [SerializeField] public DesignPalette.TextColor massageColor = DesignPalette.DefaultTextColor;
        [SerializeField] public TerminalTextSettings.TypeSpeed typeSpeed = TerminalTextSettings.TypeSpeed.Normal;

        private string OramPrefix() 
        {
            string oramPrefixColor =  "#" +  DesignPalette.TextColors[Singleton.Instance.TextSettings.oramPrefixColor];
            return $"<color={oramPrefixColor}>{OramPrefixTag}</color>";
        }
        
        
        public string MakeText()
        {
            if (massageColor == DesignPalette.DefaultTextColor) return OramPrefix() + oramMassage;
            
            var colorTagPrefix = "#" + DesignPalette.TextColors[massageColor];
            return OramPrefix() + $"<color={colorTagPrefix}>{oramMassage}</color>";
        }
        
        public List<bool> GetKeystrokeMap()
        {
            var text = OramPrefixTag + oramMassage;
            List<bool> map = new List<bool>();
            for(int i=0; i< text.Length; i++)
            {
                map.Add(!char.IsSeparator(text[i]));
            }
            map.Add(false);
            map.Add(false);
            map.Add(false);
       
            return map;
        }

        
        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            if (Application.isPlaying == false) return Playable.Null;

            var playable = ScriptPlayable<ConsoleOramMsgBehaviour>.Create(graph);
            ConsoleOramMsgBehaviour textBehaviour = playable.GetBehaviour();
            
            textBehaviour.KeystrokeMap = GetKeystrokeMap();
            textBehaviour.InsertionMode = InsertionMode;
            textBehaviour.typeSpeed = typeSpeed;
            textBehaviour.prefixCharacterCount = OramPrefixTag.Length;
            textBehaviour.oramMassage = MakeText();
            return playable;
        }
        
    }
}