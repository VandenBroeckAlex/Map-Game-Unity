public class WaterTile : Tile
{

    public WaterTile(int givenID)
        : base(givenID)
    {
        isLand = false;
        isPassable = true;
    }
}