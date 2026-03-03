public class LandTile : Tile
{
    public int ownerId { get; set; }
    public int occupierID { get; set; }
    public int rgo { get; set; }
    public bool isCoast { get; set; }
    public int climateId { get; set; }

    public int provinceId { get; set; }
    //private Moddifier: List<moddifier>

    //arable_resources -> agricultural workspace possible to build in tile or climat rule set
    //resources -> mining resource present 

    public LandTile(int givenID)
        : base(givenID)
    {
        isLand = true;
        isPassable = true;
    }
}