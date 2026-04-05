
using System.Collections.Generic;
using static ColorUtilities;
public class WaterTileBuilder
{
    public int id { get; set; }
    public string name { get; set; } = "Default";
    public string tag = "Default";
    public string type = "Default";
    public string spriteColor { get; set; } = "#000000";
    public List<int> neighbors { get; set; } = new List<int>();
    public int superficy { get; set; } = 1;
    public bool isLand { get; set; } = false;
    public bool isPassable { get; set; } = true;
    public string ownerId { get; set; } = "Default";
    public string occupierID { get; set; }
    public string rgo { get; set; } = "Default";
    public bool isCoast { get; set; } = false;
    public string climateId { get; set; } = "Default";

    public string provinceId { get; set; } = "Default";

    public WaterTileBuilder WithID(int id)
    {
        this.id = id; return this;
    }
    public WaterTileBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }
    public WaterTileBuilder WithTag(string tag)
    {
        this.tag = tag; return this;
    }
    public WaterTileBuilder WithType(string type)
    {
        this.type = type; return this;
    }
    public WaterTileBuilder WithSpriteColor(string spriteColor)
    {
        this.spriteColor = spriteColor; return this;
    }
    public WaterTileBuilder WithNeighbor(int neighbors)
    {
        this.neighbors.Add(neighbors); return this;
    }
    public WaterTileBuilder WithSuperficy(int superficy)
    {
        this.superficy = superficy; return this;
    }
    public WaterTileBuilder WithIsPassble(bool isPassble)
    {
        this.isPassable = isPassble; return this;
    }

    public WaterTileBuilder WithClimateId(string climateId)
    {
        this.climateId = climateId; return this;
    }
 

    public WaterTile Build(DataRegistery _regi, IResolutionErrorHandler _errorHandler)
    {
        WaterTile waterTile = new WaterTile(HexToInt(this.spriteColor));
        waterTile.name = this.name;
        waterTile.tag = this.tag;
        waterTile.spriteColor = HexToInt(this.spriteColor);
        //waterTile.neighbors = lTD.neighbors;
        waterTile.superficy = this.superficy;
        waterTile.isLand = this.isLand;
        waterTile.isPassable = this.isPassable;

        if (waterTile.type != -1)
        {
            waterTile.type = _regi.GetTerrainTypes(this.type);
        }
        else
        {
            _errorHandler.HandleMissingId(
                       $"Unknown terrain tag '{this.type}' while creating tile '{this.name}'.");
        }


            return waterTile;
    }

}
