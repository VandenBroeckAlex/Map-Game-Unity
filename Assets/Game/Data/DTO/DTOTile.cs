using System.Collections.Generic;


public class DTOTile
{
    public class TileDTO
    {
        public string tag;
        public string name;
        public string typeTag;
        public string spriteColor;
        public int[] neighbors;
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
