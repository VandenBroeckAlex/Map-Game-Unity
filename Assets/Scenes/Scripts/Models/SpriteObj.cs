using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteObj 
{
    public Color spriteColor;
    public List<Vector2Int> spritePixels = new List<Vector2Int>();
    public int higherX, higherY, lowerX, lowerY;
    public List<float[]> neighboreColor = new List<float[]>();
    public int id;

    public SpriteObj(Color color, Vector2Int pixelCoord, float[] neighbor)
    {
        spriteColor = color;
        spritePixels.Add(pixelCoord);
        higherX = lowerX = pixelCoord.x;
        higherY = lowerY = pixelCoord.y;
        neighboreColor.Add(neighbor);
    }

    public void SetId(int id) => this.id = id;
}