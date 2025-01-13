using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace KipoTupiniquimEngine.Extenssions
{
    public static class HUDExtenssion
    {
        public static void SetAnchorAndPosition(this RectTransform rectTransform, TextAnchor anchorPreset)
        {
            Vector2 anchorMin, anchorMax, pivot;

            switch (anchorPreset)
            {
                case TextAnchor.UpperLeft:
                    anchorMin = new Vector2(0, 1);
                    anchorMax = new Vector2(0, 1);
                    pivot = new Vector2(0, 1);
                    break;
                case TextAnchor.UpperCenter:
                    anchorMin = new Vector2(0.5f, 1);
                    anchorMax = new Vector2(0.5f, 1);
                    pivot = new Vector2(0.5f, 1);
                    break;
                case TextAnchor.UpperRight:
                    anchorMin = new Vector2(1, 1);
                    anchorMax = new Vector2(1, 1);
                    pivot = new Vector2(1, 1);
                    break;
                case TextAnchor.MiddleLeft:
                    anchorMin = new Vector2(0, 0.5f);
                    anchorMax = new Vector2(0, 0.5f);
                    pivot = new Vector2(0, 0.5f);
                    break;
                case TextAnchor.MiddleCenter:
                    anchorMin = new Vector2(0.5f, 0.5f);
                    anchorMax = new Vector2(0.5f, 0.5f);
                    pivot = new Vector2(0.5f, 0.5f);
                    break;
                case TextAnchor.MiddleRight:
                    anchorMin = new Vector2(1, 0.5f);
                    anchorMax = new Vector2(1, 0.5f);
                    pivot = new Vector2(1, 0.5f);
                    break;
                case TextAnchor.LowerLeft:
                    anchorMin = new Vector2(0, 0);
                    anchorMax = new Vector2(0, 0);
                    pivot = new Vector2(0, 0);
                    break;
                case TextAnchor.LowerCenter:
                    anchorMin = new Vector2(0.5f, 0);
                    anchorMax = new Vector2(0.5f, 0);
                    pivot = new Vector2(0.5f, 0);
                    break;
                case TextAnchor.LowerRight:
                    anchorMin = new Vector2(1, 0);
                    anchorMax = new Vector2(1, 0);
                    pivot = new Vector2(1, 0);
                    break;
                default:
                    Debug.LogError("Unsupported Anchor Preset");
                    return;
            }

            Vector2 originalPosition = rectTransform.localPosition;
            rectTransform.anchorMin = anchorMin;
            rectTransform.anchorMax = anchorMax;
            rectTransform.pivot = pivot;
            rectTransform.localPosition = originalPosition;
        }
    }
}
