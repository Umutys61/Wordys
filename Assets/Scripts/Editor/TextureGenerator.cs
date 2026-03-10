using UnityEngine;
using UnityEditor;
using System.IO;

public class TextureGenerator
{
    public static void Execute()
    {
        GenerateRoundedSquare();
    }

    public static void GenerateRoundedSquare()
    {
        int size = 128;
        int radius = 20;
        Texture2D texture = new Texture2D(size, size, TextureFormat.ARGB32, false);
        Color[] colors = new Color[size * size];

        for (int y = 0; y < size; y++)
        {
            for (int x = 0; x < size; x++)
            {
                if (IsInRoundedBox(x, y, size, size, radius))
                {
                    colors[y * size + x] = Color.white;
                }
                else
                {
                    colors[y * size + x] = Color.clear;
                }
            }
        }

        texture.SetPixels(colors);
        texture.Apply();

        byte[] bytes = texture.EncodeToPNG();
        string path = "Assets/Textures/RoundedSquare.png";
        
        if (!Directory.Exists("Assets/Textures"))
        {
            Directory.CreateDirectory("Assets/Textures");
        }

        File.WriteAllBytes(path, bytes);
        AssetDatabase.Refresh();
        
        TextureImporter importer = AssetImporter.GetAtPath(path) as TextureImporter;
        if (importer != null)
        {
            importer.textureType = TextureImporterType.Sprite;
            importer.spritePixelsPerUnit = 100;
            importer.spriteBorder = new Vector4(radius, radius, radius, radius);
            importer.SaveAndReimport();
        }
        
        Debug.Log("Rounded Square Sprite Created at " + path);
    }

    private static bool IsInRoundedBox(int x, int y, int width, int height, int radius)
    {
        if (x < radius && y < radius) return IsInCircle(x, y, radius, radius, radius);
        if (x > width - radius && y < radius) return IsInCircle(x, y, width - radius, radius, radius);
        if (x < radius && y > height - radius) return IsInCircle(x, y, radius, height - radius, radius);
        if (x > width - radius && y > height - radius) return IsInCircle(x, y, width - radius, height - radius, radius);

        return true;
    }

    private static bool IsInCircle(int x, int y, int cx, int cy, int r)
    {
        int dx = x - cx;
        int dy = y - cy;
        return dx * dx + dy * dy <= r * r;
    }
}
