using System.Collections.Generic;


public class DefTile
{
    public class TileDTO
    {
        public string tag;
        public string name;
        public string typeTag;
        public float[] spriteColor;
        public List<int> neighbors;
        public int superficy;
        public bool isLand;
        public bool isPassable;
    }
    public class WaterTileDTO : TileDTO
    {
    }

    public class LandTileDTO : TileDTO
    {
        public string ownerTag;
        public string occupierTag;
        public string rgoTag;
        public bool isCoast;
        public string climatTag;
        public string provinceTag;
    }
}
