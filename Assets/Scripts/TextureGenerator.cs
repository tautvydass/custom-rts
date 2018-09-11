using UnityEngine;

public static class TextureGenerator
{
    public static Texture2D GenerateTexture(Color[] colorMap, int width, int height)
    {
        var texture = new Texture2D(width, height);
        texture.SetPixels(colorMap);
        texture.filterMode = FilterMode.Point;
        texture.Apply();

        return texture;
    }
}
