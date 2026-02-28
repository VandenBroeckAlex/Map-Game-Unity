using System.Collections.Generic;


public class DefTile
{
    public class Tile
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public float[] spriteColor { get; set; } = new float[3];
        public List<int> neighbors { get; set; } = new List<int>();
        public int superficy { get; set; }
        public bool isLand { get; set; }
        public bool isPassable { get; set; }
        public DefModdifierContainer stats;

    }
    public class WaterTile : Tile
    {
        public int type;
    }

    public class LandTile : Tile
    {
        public int ownerId { get; set; }
        public int occupierID { get; set; }
        public int rgo { get; set; }
        public int type { get; set; }
        public bool isCoast { get; set; }

        //arable_resources -> agricultural workspace possible to build in tile or climat rule set
        //resources -> mining resource present 
    }
}
