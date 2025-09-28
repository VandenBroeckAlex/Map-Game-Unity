using System.Collections.Generic;

public class ObjJSON
{
    public int id;
    public float[] spriteColor;
    public int lowerX;
    public int higherY;
    public float[] center;
    public string name = "";
    public int superficy;
    public List<int> neighbors;   
    public bool isLand;
    public bool isPassable = true;


    public ObjJSON(float[] color, int x, int y, int id, List<int> neighbors, float[] center, int superficy, bool isLand)
    {
        this.id = id;
        this.spriteColor = color;
        this.lowerX = x;
        this.higherY = y;
        this.neighbors = neighbors;
        this.center = center;
        this.superficy = superficy;
        this.isLand = isLand;
    }

    // Default constructor for JSON serialization
    public ObjJSON() { }
}
