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

    public class WaterTile
    {
        public int Id;
        public float[] SpriteColor;

        public string name = "";
        public List<int> neighbors;
        public int superficy;
        public bool isLand = false;
        public bool isPassable = true;
        public WaterTile(int ID, float[] SPRITECOLOR, int SUPERFICY)
        {
            this.Id=ID;
            this.SpriteColor = SPRITECOLOR;
            this.superficy = SUPERFICY;
        }
    }

    public class LandTile 
    {
        public int Id;
        public float[] SpriteColor;
        public string name = "";
        public List<int> neighbors;
        public int superficy;        
        public string type = ""; //enum 
        public string rgo = ""; //enum 
        public int ownerId = 0; //id in country list 
        public int? occupierID;
        public bool isLand = true;
        public bool isPassable = true;
        public bool isCoast = false;
        public LandTile(int ID, float[] SPRITECOLOR, int SUPERFICY)
        {
            this.Id = ID;
            this.SpriteColor = SPRITECOLOR;
            this.superficy = SUPERFICY;
        }
    }

    public class CoastalTile : LandTile
    {       
        public CoastalTile(int ID, float[] SPRITECOLOR,  int SUPERFICY) : base(ID, SPRITECOLOR,SUPERFICY)
        {
            this.Id = ID;
            this.SpriteColor = SPRITECOLOR;
            this.superficy = SUPERFICY;
            this.isCoast = true;
        }
    }

    public class SpriteInfo
    {
        public float[] SpriteColor;
        public int lowerX;
        public int higherY;
        public float[] center;

        public SpriteInfo(float[] color, int x, int y,  float[] center)
        {
       
            this.SpriteColor = color;
            this.lowerX = x;
            this.higherY = y;
            this.center = center;
        }
    }
}

