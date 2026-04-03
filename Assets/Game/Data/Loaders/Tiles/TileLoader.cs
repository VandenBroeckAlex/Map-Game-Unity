using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DTOTile;
using static ColorUtilities;
public class TileLoader
{
    public Dictionary<int,Tile> DeserializeTiles(
        string json, 
        Dictionary<string,int> RgoTag,
        string[] TerrainTypeTag, // land or water terrain ?
        string[] countryTag,
        string[] ProvinceTag,
        string[] ClimateTag
        )
    {

        // <int,int> <Hex, id> for neighbor
        Dictionary<int,int> TileColorIntToId = new Dictionary<int,int>();

        JArray listTile = JArray.Parse(json);
        Dictionary<int, Tile> TileDictionnary = new Dictionary<int, Tile>();
   
        foreach (JObject tile in listTile)
        {
            if (tile == null) continue; // safety
            bool isLand = tile["isLand"]?.Value<bool>() ?? false;

            if (isLand)
            {
                
                LandTileDTO lTD = tile.ToObject<LandTileDTO>();
                LandTile landTile = new LandTile(HexToInt(lTD.spriteColor));
                landTile.name = lTD.name;
                landTile.tag = lTD.tag;
                landTile.spriteColor = HexToInt(lTD.spriteColor);
                TileColorIntToId[landTile.spriteColor] = HexToInt(lTD.spriteColor);
                //landTile.neighbors = lTD.neighbors;
                landTile.superficy = lTD.superficy;
                landTile.isLand = lTD.isLand;
                landTile.isPassable = lTD.isPassable;
                landTile.isCoast = lTD.isCoast;

                landTile.type = GetIdByTag(lTD.typeTag, TerrainTypeTag);
                if (landTile.type == -1)
                {
                    throw new InvalidDataException(
                    $"Unknown terrain tag '{lTD.typeTag}' while creating tile '{lTD.name}'.");
                }
      
                landTile.ownerId = GetIdByTag(lTD.ownerTag, countryTag);
                if (landTile.ownerId == -1)
                {
                    throw new InvalidDataException(
                         $"Unknown country tag '{lTD.ownerTag}' while creating tile '{lTD.name}'."
                        );
                }
            
                landTile.occupierID = GetIdByTag(lTD.ownerTag, countryTag);
                if (landTile.occupierID == -1)
                {
                    throw new InvalidDataException(
                         $"Unknown country tag '{lTD.ownerTag}' while creating tile '{lTD.name}'."
                        );
                }
              
                if (RgoTag.TryGetValue(lTD.rgoTag, out int rgoTag))
                {
                    landTile.rgo = rgoTag;
                }
                else
                {
                    throw new InvalidDataException(
                        $"Unknown RGO tag '{lTD.rgoTag}' while creating tile '{lTD.name}'('tile name')."
                        );
                }
             
                int climateId = GetIdByTag(lTD.climatTag, ClimateTag);
                if (climateId == -1)
                {
                    throw new InvalidDataException(
                        $"Unknown climate tag '{lTD.climatTag}' while creating tile '{lTD.name}'('tile name')."
                        );
                }
                landTile.climateId = climateId;
                int provinceId = GetIdByTag(lTD.provinceTag, ProvinceTag);
                if (provinceId == -1)
                {
                    throw new InvalidDataException(
                        $"Unknown province tag '{lTD.provinceTag}' while creating tile '{lTD.name}' : ('tile name')."
                        );
                }
                TileDictionnary.Add(HexToInt(lTD.spriteColor), landTile);
            
            }
            else
            {
                WaterTileDTO wTD = tile.ToObject<WaterTileDTO>();
                WaterTile waterTile = new WaterTile(HexToInt(wTD.spriteColor));

                waterTile.name = wTD.name;
                waterTile.tag = wTD.tag;
                waterTile.spriteColor = HexToInt(wTD.spriteColor);
                TileColorIntToId[waterTile.spriteColor] = HexToInt(wTD.spriteColor);
                //waterTile.neighbors = wTD.neighbors;
                waterTile.superficy = wTD.superficy;
                waterTile.isLand = wTD.isLand;
                waterTile.isPassable = wTD.isPassable;
                waterTile.type = GetIdByTag(wTD.typeTag, TerrainTypeTag);
                if (waterTile.type == -1)
                {
                    throw new InvalidDataException(
                    $"Unknown terrain tag '{wTD.typeTag}' while creating tile '{wTD.name}'.");
                }

                TileDictionnary.Add(HexToInt(wTD.spriteColor), waterTile);
            }
        }

        //TileList = ResolveNeighbor(TileList, TileColorIntToId);

        return TileDictionnary;
    }

    private int GetIdByTag(string givenTag, string[] data) 
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == givenTag)
            {
                return i;
            }
        }
        return -1;
    }
 

}

