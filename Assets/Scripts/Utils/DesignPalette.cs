using System;
using System.Collections.Generic;
using Story.Terminal.ContentCreation.Terminal_Operations;
using UnityEngine;

namespace Utils
{
    [ExecuteAlways]
    public class DesignPalette : MonoBehaviour
    {
        [Header("Main Colors")]
        [SerializeField]private Color  bgBlue;
        [SerializeField]private Color  green;
        [SerializeField]private Color  whiteYellowish;
        [SerializeField] private Color greyBlue;
        [SerializeField] private Color lightBlue;
        [SerializeField]private Color  pink;
        [SerializeField]private Color  red;
        
        
        public static Color BGBlue;
        public static Color Green;
        public static Color WhiteYellowish;
        public static Color GreyBlue;
        public static Color LightBlue;
        public static Color Pink;
        public static Color Red;

        private void Awake()
        {
            BGBlue = bgBlue;
            Colors[UiColorAutoUpdate.PaletteColor.BGBlue] = BGBlue;
               
            Green = green;
            Colors[UiColorAutoUpdate.PaletteColor.Green] = Green;

            WhiteYellowish = whiteYellowish;
            Colors[UiColorAutoUpdate.PaletteColor.WhiteYellowish] = WhiteYellowish;
            
            GreyBlue = greyBlue;
            Colors[UiColorAutoUpdate.PaletteColor.GreyBlue] = GreyBlue;
            
            LightBlue = lightBlue;
            Colors[UiColorAutoUpdate.PaletteColor.LightBlue] = LightBlue;
            
            Pink = pink;
            Colors[UiColorAutoUpdate.PaletteColor.Pink] = Pink;
            
            Red = red;
            Colors[UiColorAutoUpdate.PaletteColor.Red] = Red;
        }

        // [Header("Low Opacity Colors")]
      //  [SerializeField]
      private Color lowOpacityBlue;
     //   [SerializeField]
     private Color lowOpacityBlack;
        public static Color LowOpacityBlue;
        public static Color LowOpacityBlack;

        
        //[Header("Terminal text colors")]

        public static string BGBlueHex => "122939";
        public static string GreenHex = "C2F970";
        public static string WhiteYellowishHex => "EDEFD3";
        public static string GreyBlueHex => "516F6F";
        public static string LightBlueHex => "7AACAC";
        public static string PinkHex = "FF8BA3";

        public static string RedHex = "F06449";
        public static string PureWhiteHex => ColorConverter.ColorToHex(Color.white);
        
        public static string TransparentWhiteHex = "edefd366";



        // public static string GreenHex => ColorConverter.ColorToHex(Green); //ColorConverter.ColorToHex(Green);
        public static string LowOpacityBlueHex => ColorUtility.ToHtmlStringRGBA(LowOpacityBlue);
        public static string LowOpacityBlackHex => ColorUtility.ToHtmlStringRGBA(LowOpacityBlack);


        public static  Dictionary<UiColorAutoUpdate.PaletteColor, Color> Colors = new()
        {
            {UiColorAutoUpdate.PaletteColor.BGBlue, BGBlue},
            {UiColorAutoUpdate.PaletteColor.Green, Green},
            {UiColorAutoUpdate.PaletteColor.WhiteYellowish, WhiteYellowish},
            {UiColorAutoUpdate.PaletteColor.GreyBlue, GreyBlue},
            {UiColorAutoUpdate.PaletteColor.LightBlue, LightBlue},
            {UiColorAutoUpdate.PaletteColor.Pink, Pink},
            {UiColorAutoUpdate.PaletteColor.Red, Red},
            {UiColorAutoUpdate.PaletteColor.Null, new Color(0,0,0,0)},
        };
        

        
        /**
         * @deprecated
         */
        public static readonly Dictionary<TerminalCommand.TextColor, string> DepreciatedTextColors = new()
        {
            {TerminalCommand.TextColor.White, PureWhiteHex},
            {TerminalCommand.TextColor.Red, RedHex},
            {TerminalCommand.TextColor.Green, GreenHex},
            {TerminalCommand.TextColor.BabyBlue,LightBlueHex}
        };
        
        public enum TextColor
        {
            White,
            WhiteYellowish,
            Red,
            Green,
            LightBlue,
            GreyBlue,
        }
        
        public static TextColor DefaultTextColor = TextColor.White;
        
        public static readonly Dictionary<TextColor, string> TextColors = new()
        {
            {TextColor.White, PureWhiteHex},
            {TextColor.Red, RedHex},
            {TextColor.Green, GreenHex},
            {TextColor.LightBlue,LightBlueHex}
        };
        private void OnValidate()
        {
            BGBlue = bgBlue;
            Colors[UiColorAutoUpdate.PaletteColor.BGBlue] = BGBlue;
               
            Green = green;
            Colors[UiColorAutoUpdate.PaletteColor.Green] = Green;

            WhiteYellowish = whiteYellowish;
            Colors[UiColorAutoUpdate.PaletteColor.WhiteYellowish] = WhiteYellowish;
            
            GreyBlue = greyBlue;
            Colors[UiColorAutoUpdate.PaletteColor.GreyBlue] = GreyBlue;
            
            LightBlue = lightBlue;
            Colors[UiColorAutoUpdate.PaletteColor.LightBlue] = LightBlue;
            
            Pink = pink;
            Colors[UiColorAutoUpdate.PaletteColor.Pink] = Pink;
            
            Red = red;
            Colors[UiColorAutoUpdate.PaletteColor.Red] = Red;
        }




    }
}