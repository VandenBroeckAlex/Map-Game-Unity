using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using static DefTile;

public class TileLoader
{
    public Dictionary<int,Tile> DeserializeTiles(
        string json, 
        Dictionary<string,int>RgoTagId,
        Dictionary<string, int> TerrainTypeTagId, 
        Dictionary<string, int> countryTagId, 
        Dictionary<string, int> ProvinceTagId,
        Dictionary<string, int> ClimateTag
        )
    {
        JArray listTile = JArray.Parse(json);
        Dictionary<int, Tile> TileList = new Dictionary<int, Tile>();
        int idIterator = 0;
        foreach (JObject tile in listTile)
        {
            if (tile == null) continue; // safety
            bool isLand = tile["isLand"]?.Value<bool>() ?? false;

            if (isLand)
            {
                LandTile landTile = new LandTile(idIterator);
                LandTileDTO lTD = tile.ToObject<LandTileDTO>();
                landTile.name = lTD.name;
                landTile.spriteColor = lTD.spriteColor;
                landTile.neighbors = lTD.neighbors;
                landTile.superficy = lTD.superficy;
                landTile.isLand = lTD.isLand;
                landTile.isPassable = lTD.isPassable;
                landTile.isCoast = lTD.isCoast;
               
                if (TerrainTypeTagId.TryGetValue(lTD.typeTag, out int typeId)){
                    landTile.type = typeId;
                }
                else
                {
                    throw new InvalidDataException(
                    $"Unknown terrain tag '{lTD.typeTag}' while creating tile '{lTD.name}'.");
                }
                if (countryTagId.TryGetValue(lTD.ownerTag, out int countryId)){
                    landTile.ownerId = countryId;
                }
                else
                {
                    throw new InvalidDataException(
                         $"Unknown country tag '{lTD.ownerTag}' while creating tile '{lTD.name}'."
                        );
                }
                if (countryTagId.TryGetValue(lTD.occupierTag, out int occupierId))
                {
                    landTile.ownerId = occupierId;
                }
                else
                {
                    throw new InvalidDataException(
                         $"Unknown occupier tag '{lTD.occupierTag}' while creating tile '{lTD.name}'."
                        );
                }
                if (RgoTagId.TryGetValue(lTD.rgoTag, out int rgoId))
                {
                    landTile.rgo  = rgoId;
                }
                else
                {
                    throw new InvalidDataException(
                         $"Unknown rgo tag '{lTD.rgoTag}' while creating tile '{lTD.name}'."
                        );
                }
                if(ClimateTag.TryGetValue(lTD.climatTag, out int climateId))
                {
                    landTile.climateId = climateId;
                }
                else
                {
                    throw new InvalidDataException(
                        $"Unknown climate tag '{lTD.climatTag}' while creating tile '{lTD.name}'('tile name')."
                        );
                }
                if (ProvinceTagId.TryGetValue(lTD.provinceTag, out int provinceId))
                {
                    landTile.provinceId = provinceId;
                }
                else
                {
                    throw new InvalidDataException(
                        $"Unknown province tag '{lTD.provinceTag}' while creating tile '{lTD.name}'('tile name')."
                        );
                }

                    TileList.Add(idIterator, landTile);
            
            }
            else
            {
                WaterTileDTO wTD = tile.ToObject<WaterTileDTO>();
                WaterTile waterTile = new WaterTile(idIterator);

                waterTile.name = wTD.name;
                waterTile.spriteColor = wTD.spriteColor;
                waterTile.neighbors = wTD.neighbors;
                waterTile.superficy = wTD.superficy;
                waterTile.isLand = wTD.isLand;
                waterTile.isPassable = wTD.isPassable;

                if (TerrainTypeTagId.TryGetValue(wTD.typeTag, out int typeId))
                {
                    waterTile.type = typeId;
                }
                else
                {
                    throw new InvalidDataException(
                    $"Unknown terrain tag '{wTD.typeTag}' while creating tile '{wTD.name}'.");
                }

                TileList.Add(idIterator,waterTile);
            }
            idIterator++;
        }
        return TileList;
    }
}
