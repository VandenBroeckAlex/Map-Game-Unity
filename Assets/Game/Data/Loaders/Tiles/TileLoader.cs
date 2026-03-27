using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static DTOTile;

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
                string hex = lTD.spriteColor;
                hex = hex.Replace("#", "");
                uint argb = uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                landTile.spriteColor = (int)argb;
                TileColorIntToId[landTile.spriteColor] = idIterator;
                //landTile.neighbors = lTD.neighbors;
                landTile.superficy = lTD.superficy;
                landTile.isLand = lTD.isLand;
                landTile.isPassable = lTD.isPassable;
                landTile.isCoast = lTD.isCoast;

                int typeId = GetIdByTag(lTD.typeTag, TerrainTypeTag);
                if (typeId == -1)
                {
                    throw new InvalidDataException(
                    $"Unknown terrain tag '{lTD.typeTag}' while creating tile '{lTD.name}'.");
                }

                int countryId = GetIdByTag(lTD.ownerTag, countryTag);
                if (countryId == -1)
                {
                    throw new InvalidDataException(
                         $"Unknown country tag '{lTD.ownerTag}' while creating tile '{lTD.name}'."
                        );
                }
                int occupierId = GetIdByTag(lTD.ownerTag, countryTag);
                if (countryId == -1)
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

                int provinceId = GetIdByTag(lTD.provinceTag, ProvinceTag);
                if (provinceId == -1)
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
                string hex = wTD.spriteColor;
                hex = hex.Replace("#", "");
                uint argb = uint.Parse(hex, System.Globalization.NumberStyles.HexNumber);
                TileColorIntToId[waterTile.spriteColor] = idIterator;
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

                TileList.Add(idIterator,waterTile);
            }
            idIterator++;
        }

        //TileList = ResolveNeighbor(TileList, TileColorIntToId);

        return TileList;
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
 
    //private Dictionary<int,Tile> ResolveNeighbor(Dictionary<int, Tile> tileList, Dictionary<int, int> TileColorIntToId)
    //{
    //    foreach (var kvp in tileList) 
    //    {
    //        foreach(string )
    //    }
    //}
}

