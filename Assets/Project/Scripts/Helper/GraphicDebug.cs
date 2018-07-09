using System.Collections;
using UnityEngine;

public class GraphicDebug : MonoBehaviour
{
    public static GraphicDebug _graphicDebug;
    public void DrawQuad(Rect position, Color color)
    {
        Texture2D texture = new Texture2D(1, 1);
        texture.SetPixel(0, 0, color);
        texture.Apply();
        GUI.skin.box.normal.background = texture;
        GUI.Box(position, GUIContent.none);
    }
}