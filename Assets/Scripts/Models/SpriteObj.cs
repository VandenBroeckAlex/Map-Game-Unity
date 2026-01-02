using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteObj 
{
    public Color spriteColor;
    public List<Vector2Int> spritePixels = new List<Vector2Int>();
    public int higherX, higherY, lowerX, lowerY;
    public List<int> neighboreId = new List<int>();
    public int id;

    public SpriteObj(Color color, Vector2Int pixelCoord)
    {
        spriteColor = color;
        spritePixels.Add(pixelCoord);
        higherX = lowerX = pixelCoord.x;
        higherY = lowerY = pixelCoord.y;
       
    }

    public void SetId(int id) => this.id = id;
}