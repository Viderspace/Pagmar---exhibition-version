using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Utils
{
    [ExecuteAlways]
    public class UiColorAutoUpdate : MonoBehaviour
    {
        public List<GraphicComponent> componentsToColor = new();
        public bool GlobalSettings = false;
        public PaletteColor convertAllTo = PaletteColor.Null;
        public enum PaletteColor
        {
            BGBlue,
            Green,
            WhiteYellowish,
            GreyBlue,
            LightBlue,
            Pink,
            Red,
            Null
        }



        void UpdateColors()
        {
            foreach (var graphicComponent in componentsToColor)
            {
                if (graphicComponent.component == null) continue;

                if (GlobalSettings)
                {
                    graphicComponent.component.color = DesignPalette.Colors[convertAllTo];
                }
                else
                {
                    graphicComponent.component.color = DesignPalette.Colors[graphicComponent.colorPalette];
                }
                
            }
        }

        private void OnValidate()
        {
            UpdateColors();
        }

        private void Awake()
        {
            UpdateColors();
        }
    }

    [Serializable]
    public struct GraphicComponent
    {
        public Graphic component;
        public UiColorAutoUpdate.PaletteColor colorPalette;
    }
}