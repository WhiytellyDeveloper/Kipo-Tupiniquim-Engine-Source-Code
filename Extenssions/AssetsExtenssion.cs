using PixelInternalAPI.Extensions;
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

        public static void OverlayTexture(this Texture2D baseTex, Texture2D overlayTex)
        {
            for (int x = 0; x < overlayTex.width; x++)
            {
                for (int y = 0; y < overlayTex.height; y++)
                {
                    Color overlayPixel = overlayTex.GetPixel(x, y);
                    if (overlayPixel.a > 0)
                        baseTex.SetPixel(x, y, overlayPixel);
                }
            }

            baseTex.Apply();
        }

        public static Sprite OverlaySprite(this Sprite baseTex, Texture2D overlayTex, bool overlayTransparent = false)
        {
            var texture = baseTex.texture.MakeReadableTexture();

            Rect baseRect = baseTex.rect;
            int baseXOffset = (int)baseRect.x;
            int baseYOffset = (int)baseRect.y;

            for (int x = 0; x < overlayTex.width; x++)
            {
                for (int y = 0; y < overlayTex.height; y++)
                {
                    Color overlayPixel = overlayTex.GetPixel(x, y);

                    int mappedX = baseXOffset + x;
                    int mappedY = baseYOffset + y;

                    if (mappedX >= 0 && mappedX < texture.width && mappedY >= 0 && mappedY < texture.height)
                    {
                        Color basePixel = texture.GetPixel(mappedX, mappedY);

                        if (overlayTransparent && basePixel.a == 0)
                            texture.SetPixel(mappedX, mappedY, overlayPixel);
                        else if (!overlayTransparent && overlayPixel.a > 0)
                            texture.SetPixel(mappedX, mappedY, overlayPixel);        
                    }
                }
            }

            texture.Apply();

            return Sprite.Create(
                texture,
                baseTex.rect,
                baseTex.pivot,
                baseTex.pixelsPerUnit,
                0,
                SpriteMeshType.FullRect
            );
        }

        public static Sprite ConvertTexToSpriteWithBase(this Texture2D texture, Sprite baseSp)
        {
            return Sprite.Create(texture, baseSp.rect, baseSp.pivot);
        }

        public static Texture2D ResizeTexture(this Texture2D original, int newWidth, int newHeight)
        {
            Texture2D resized = new(newWidth, newHeight, original.format, false);

            for (int y = 0; y < newHeight; y++)
            {
                for (int x = 0; x < newWidth; x++)
                {
                    float u = (float)x / newWidth;
                    float v = (float)y / newHeight;
                    Color color = original.GetPixelBilinear(u, v);
                    resized.SetPixel(x, y, color);
                }
            }

            resized.Apply();
            return resized;
        }
    }
}