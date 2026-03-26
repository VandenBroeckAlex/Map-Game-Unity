using System.Collections.Generic;



    public class Tile
    {
        public int id { get; set; }
        public string name { get; set; } = "";
        public int type;
        public int spriteColor { get; set; }
        public List<int> neighbors { get; set; } = new List<int>();
        public int superficy { get; set; }
        public bool isLand { get; set; }
        public bool isPassable { get; set; }

        
        public Tile(int GivenID)
        {
            id = GivenID;
        }

    }
   

    


