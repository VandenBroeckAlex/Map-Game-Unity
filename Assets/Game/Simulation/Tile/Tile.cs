using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;



    public class Tile
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public int type;
        public float[] spriteColor { get; set; } = new float[3];
        public List<int> neighbors { get; set; } = new List<int>();
        public int superficy { get; set; }
        public bool isLand { get; set; }
        public bool isPassable { get; set; }
        
        public Tile(int GivenID)
        {
            id = GivenID;
        }

    }
   

    


