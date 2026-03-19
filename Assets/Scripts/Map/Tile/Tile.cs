
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;


namespace MyGame.Data
{    //base tile

    //make land and water inherit from it
    [SerializeField]
    public class Tile
    {
        [JsonProperty(Order = 0)]
        public int id { get; set; }
        public string name { get; set; } = "";
        public float[] spriteColor { get; set; } = new float[3];
        public List<int> neighbors { get; set; } = new List<int>();

        public int superficy { get; set; }
        public bool isLand { get; set; }
        public bool isPassable { get; set; }

        //public ProvinceStats stats;

        public Tile(int GivenID)
        {
            id = GivenID;

        }

    }
    public class WaterTile : Tile
    {
        [JsonProperty(Order = 1)]
        public int type;

        public WaterTile(int givenID)
            : base(givenID)
        {
            isLand = false;
            isPassable = true;
        }
    }

    public class LandTile : Tile
    {
        [JsonProperty(Order = 7)]
        public int ownerId { get; set; }
        [JsonProperty(Order = 8)]
        public int occupierID { get; set; }
        [JsonProperty(Order = 9)]
        public int rgo { get; set; }
        [JsonProperty(Order = 10)]
        public int type { get; set; }
        [JsonProperty(Order = 11)]
        public bool isCoast { get; set; }

        //arable_resources -> agricultural workspace possible to build in tile or climat rule set
        //resources -> mining resource if present 


        public LandTile(int givenID)
            : base(givenID)
        {
            isLand = true;
            isPassable = true;
        }
    }



    //load it from resource json
    // if type == Raw than good is valid rgo
    public enum Type_rgo
    {
        coal,
        cattle
    }
    //load it from province type json
    public enum Type_province
    {
        plain,
        forest
    }

    //load it from country json. It's not suppose to be here ! bruh
    // check for country in country manager
    public enum Country
    {
        France,
        Germany
    }

}