

using System.Collections.Generic;
using static ColorUtilities;
public class LandTileBuilder
{
    public string id { get; set; }
    public string name { get; set; } = "Default";
    public string tag = "Default";
    public string type = "Default";
    public string spriteColor { get; set; } = "#000000";
    public List<int> neighbors { get; set; } = new List<int>();
    public int superficy { get; set; } = 1;
    public bool isLand { get; set; } = true;
    public bool isPassable { get; set; } = true;
    public string ownerId { get; set; } = "Default";
    public string occupierID { get; set; } = "";
    public string rgo { get; set; } = "Default";
    public bool isCoast { get; set; } = false;
    public string climateId { get; set; } = "Default";

    public string provinceId { get; set; } = "Default";

    public LandTileBuilder WithID(string id)
    {
        this.id = id; return this;
    }
    public LandTileBuilder WithName(string name)
    {
        this.name = name;
        return this;
    }
    public LandTileBuilder WithTag(string tag)
    {
        this.tag = tag; return this;
    }
    public LandTileBuilder WithType(string type)
    {
        this.type = type; return this;
    }
    public LandTileBuilder WithSpriteColor(string spriteColor)
    {
        this.spriteColor = spriteColor; return this;
    }
    public LandTileBuilder WithNeighbor(int neighbors)
    {
        this.neighbors.Add(neighbors); return this;
    }
    public LandTileBuilder WithSuperficy(int superficy)
    {
        this.superficy = superficy; return this;
    }
    public LandTileBuilder WithIsPassble(bool isPassble)
    {
        this.isPassable = isPassble; return this;
    }
    public LandTileBuilder WithOwner(string owner)
    {
        this.ownerId = owner; return this;
    }
    public LandTileBuilder WithOccupier(string occupier)
    {
        this.occupierID = occupier; return this;
    }
    public LandTileBuilder WithRGO(string rgo)
    {
        this.rgo = rgo; return this;
    }
    public LandTileBuilder WithCoast(bool coast)
    {
        this.isCoast = coast; return this;
    }
    public LandTileBuilder WithClimateId(string climateId)
    {
        this.climateId = climateId; return this;
    }
    public LandTileBuilder WithProvince(string province)
    {
        this.provinceId = province; return this;
    }

    public LandTile Build(DataRegistery _regi, IResolutionErrorHandler _errorHandler)
    {
        LandTile landTile = new LandTile(HexToInt(this.spriteColor));
        landTile.name = this.name;
        landTile.tag = this.tag;
        landTile.spriteColor = HexToInt(this.spriteColor);
        
        //landTile.neighbors = lTD.neighbors;
        landTile.superficy = this.superficy;
        landTile.isLand = this.isLand;
        landTile.isPassable = this.isPassable;
        landTile.isCoast = this.isCoast;

        int type = _regi.GetTerrainTypes(this.type);
        //TODO if -1 do not replace value
        if (type != -1)
        {
            landTile.type = type;
        }
        else
        {
            _errorHandler.HandleMissingId(
            $"Unknown terrain tag : '{this.type}' while creating tile : '{this.name}'.");
        }

        int ownerId = _regi.GetCountryTagId(this.ownerId);
        if (ownerId != -1)
        {
            landTile.ownerId= ownerId;
        }
        else
        {
            _errorHandler.HandleMissingId(
               $"Unknown country tag '{this.ownerId}' while creating tile '{this.name}'."
              );
        }

        if (!string.IsNullOrWhiteSpace(this.occupierID))
        {
            int occupierID = _regi.GetCountryTagId(this.occupierID);
            if (occupierID != -1)
            {
                landTile.occupierID = occupierID;

            }
            else
            {
                _errorHandler.HandleMissingId(
                   $"Unknown country tag '{this.occupierID}' while creating tile '{this.name}' occupier."
                  );
            }
        }

        if (!string.IsNullOrWhiteSpace(this.rgo))
        {
            if (_regi.rgoTag.TryGetValue(this.rgo, out int rgoTag))
            {
                landTile.rgo = rgoTag;
            }
            else
            {
                _errorHandler.HandleMissingId(
                    $"Unknown RGO tag '{this.rgo}' while creating tile '{this.name}'('tile name')."
                    );
            }
        }

        int climateId = _regi.GetClimateTagId(this.climateId);
        if (climateId != -1)
        {
            landTile.climateId = climateId;
            
        }
        else
        {
            _errorHandler.HandleMissingId(
                $"Unknown climate tag '{this.climateId}' while creating tile '{this.name}'('tile name')."
                );
        }
            
        int provinceId = _regi.GetProvinceID(this.provinceId);
        if (provinceId != -1)
        {
            landTile.provinceId = provinceId;
        }
        else
        {
            _errorHandler.HandleMissingId(
                            $"Unknown province tag '{this.provinceId}' while creating tile '{this.name}' : ('tile name')."
                            );
        }
            return landTile;
    }

}
