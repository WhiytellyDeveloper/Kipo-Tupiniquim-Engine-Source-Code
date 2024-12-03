using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KipoTupiniquimEngine.Extenssions
{
    public static class AssetsExtenssion
    {
        public static Color GetMostGenericFromSprite(this Sprite sprite)
        {
            if (sprite == null)
                return Color.white;

            Texture2D readableTexture = MakeTextureReadable(sprite.texture);
            Color[] pixels = readableTexture.GetPixels();
            Dictionary<Color, int> colorFrequency = new Dictionary<Color, int>();

            foreach (Color color in pixels)
            {
                if (color.a == 1 && !IsGrayScale(color))
                {
                    if (colorFrequency.ContainsKey(color))
                        colorFrequency[color]++;
                    else
                        colorFrequency.Add(color, 1);
                }
            }

            Color mostFrequentColor = new(0.5f, 0.5f, 0.5f, 1f);
            int maxCount = 0;

            foreach (var entry in colorFrequency)
            {
                if (entry.Value > maxCount)
                {
                    maxCount = entry.Value;
                    mostFrequentColor = entry.Key;
                }
            }

            return mostFrequentColor;
        }

        private static bool IsGrayScale(Color color)
        {
            const float tolerance = 0.08f;
            return Mathf.Abs(color.r - color.g) < tolerance && Mathf.Abs(color.g - color.b) < tolerance;
        }

        private static Texture2D MakeTextureReadable(this Texture texture)
        {
            RenderTexture renderTexture = RenderTexture.GetTemporary(
                texture.width,
                texture.height,
                0,
                RenderTextureFormat.Default,
                RenderTextureReadWrite.Linear);

            Graphics.Blit(texture, renderTexture);

            RenderTexture previous = RenderTexture.active;
            RenderTexture.active = renderTexture;

            Texture2D readableTexture = new(texture.width, texture.height);
            readableTexture.ReadPixels(new(0, 0, texture.width, texture.height), 0, 0);
            readableTexture.Apply();

            RenderTexture.active = previous;
            RenderTexture.ReleaseTemporary(renderTexture);

            return readableTexture;
        }

        public static Sprite ChangeColorToDominant(this Sprite originalSprite, Color referenceColor, Color colorToReplace)
        {
            Texture2D readableTexture = MakeTextureReadable(originalSprite.texture);
            readableTexture.filterMode = FilterMode.Point;

            for (int x = 0; x < readableTexture.width; x++)
            {
                for (int y = 0; y < readableTexture.height; y++)
                {
                    Color pixelColor = readableTexture.GetPixel(x, y);
                    if (pixelColor.Equals(referenceColor))
                        readableTexture.SetPixel(x, y, colorToReplace);

                }
            }

            readableTexture.Apply();

            return Sprite.Create(readableTexture, originalSprite.rect, new(0.5f, 0.5f));
        }

        public static Color SetHexaColor(string hexa)
        {
            Color _color = Color.white;
            ColorUtility.TryParseHtmlString(hexa, out _color);
            return _color;
        }

        public static Color GetMiddlePixelColor(this Sprite sprite)
        {
            Texture2D texture = MakeTextureReadable(sprite.texture);

            int midX = texture.width / 2;
            int midY = texture.height / 2;

            return texture.GetPixel(midX, midY);
        }
    }
}