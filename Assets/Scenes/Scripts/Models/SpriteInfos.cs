using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteInfos
{
    public int id;
    public float[] spriteColor;
    public int lowerX;
    public int higherY;
    public float[] center;
    public string name = "";
    public int superficy;
    public int Type = 0;
    public int owner = 0;
    public List<float[]> neighbors;

    public SpriteInfos(float[] color, Vector2Int coord, int id, List<float[]> neighbors, float[] center, int superficy)
    {
        this.id = id;
        this.spriteColor = color;
        this.lowerX = coord.x;
        this.higherY = coord.y;
        this.neighbors = neighbors;
        this.center = center;
        this.superficy = superficy;
    }
}
