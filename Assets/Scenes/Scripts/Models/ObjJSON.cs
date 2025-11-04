using System.Collections.Generic;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class ObjJSON
{
    public int Id;
    public float[] SpriteColor;

    public string name = "";    
    public List<int> neighbors;
    public int superficy;
    public bool isLand;
    public bool isPassable = true;

    public int lowerX;
    public int higherY;
    public float[] center;

    public ObjJSON(float[] color, int x, int y, int id, List<int> neighbors, float[] center, int superficy, bool isLand)
    {
        this.Id = id;
        this.SpriteColor = color;
        this.lowerX = x;
        this.higherY = y;
        this.neighbors = neighbors;
        this.center = center;
        this.superficy = superficy;
        this.isLand = isLand;
    }

    // Default constructor for JSON serialization
    public ObjJSON() { }


    public class SpriteInfo
    {
        public float[] spriteColor;
        public int lowerX;
        public int higherY;
        public float[] center;
        public int id;

        public SpriteInfo(float[] color, int x, int y,  float[] center,int id)
        {
            this.id = id;
            this.spriteColor = color;
            this.lowerX = x;
            this.higherY = y;
            this.center = center;
        }
    }
}

