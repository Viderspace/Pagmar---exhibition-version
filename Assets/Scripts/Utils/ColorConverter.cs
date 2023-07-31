using System;
using UnityEngine;

namespace Utils
{
    public class ColorConverter
    {
        // (about Color Conversion) :
        // I wanted to be able to change the rows colors easily for design purposes, so i made this set of functions
        //  this allows me to insert a hexcode (as a string) and convert it into a Color object.

        
        #region Hex to Color : Color Conversion

        // helper functions: 
        private static int HexToDec(string hex)
        {
            var dec = System.Convert.ToInt32(hex, 16);
            return dec;
        }

        // private static string DecToHex(int value)
        // {
        //     return value.ToString("X2");
        // }

        private static float HexToFloatNormalized(string hex)
        {
            return HexToDec(hex) / 255f;
        }

        // main function:
        public static Color HexToColor(string hexString)
        {
            float red = HexToFloatNormalized(hexString.Substring(0, 2));
            float green = HexToFloatNormalized(hexString.Substring(2, 2));
            float blue = HexToFloatNormalized(hexString.Substring(4, 2));
            return new Color(red, green, blue);
        }

        #endregion

        #region Color to Hex : Color Conversion

        private static string DecToHex(int value)
        {
            return value.ToString("X2");
        }

        private static float FloatNormalizedToHex(float value)
        {
            return (int) Math.Round(value * 255);
        }

        public static string ColorToHex(Color color)
        {
            string red = DecToHex((int) FloatNormalizedToHex(color.r));
            string green = DecToHex((int) FloatNormalizedToHex(color.g));
            string blue = DecToHex((int) FloatNormalizedToHex(color.b));
            return red + green + blue;
        }

        #endregion
    }
}