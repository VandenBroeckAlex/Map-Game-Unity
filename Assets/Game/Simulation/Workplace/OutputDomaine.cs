
public enum OutputDomain
{
        Market,         // Goes to the trade system
        Tile,           //Goes to the tile               
        Province,       // Infrastructure, local buffs
        Country,        // Research points, Admin points, Prestige
        Internal ,      // Goods stored inside the building (Work-in-progress)
}

public struct ProductionOutput
{
    public OutputDomain domain;
    public int id;
    public int marketId;
    public int resourceId; // e.g., Iron ID, or "Research" ID
    public float amount;
}