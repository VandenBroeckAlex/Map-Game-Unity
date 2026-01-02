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
    public int superficy;


    public SpriteInfos(float[] color, Vector2Int coord, int id,  float[] center, int superficy)
    {
        this.id = id;
        this.spriteColor = color;
        this.lowerX = coord.x;
        this.higherY = coord.y;
        this.center = center;
        this.superficy = superficy;
    }
}
