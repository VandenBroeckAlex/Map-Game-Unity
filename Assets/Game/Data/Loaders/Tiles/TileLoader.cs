using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using static DTOTile;
using static ColorUtilities;
public class TileLoader
{
    public Dictionary<int, Tile> DeserializeTiles(
        string json, 
        Dictionary<string,int> RgoTag,
        string[] TerrainTypeTag, // land or water terrain ?
        string[] countryTag,
        string[] ProvinceTag,
        string[] ClimateTag,
        DataRegistery _registery,
        IResolutionErrorHandler _errorHandler
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

                LandTileBuilder tileBuilder = new LandTileBuilder()
                    .WithID(HexToInt(lTD.spriteColor))
                    .WithName(lTD.name)
                    .WithTag(lTD.tag)
                    .WithSpriteColor(lTD.spriteColor)
                    .WithSuperficy(lTD.superficy)
                    .WithIsPassble(lTD.isLand)
                    .WithCoast(lTD.isCoast)
                    .WithType(lTD.typeTag)
                    .WithOwner(lTD.ownerTag)
                    .WithRGO(lTD.rgoTag)
                    .WithClimateId(lTD.climatTag)
                    .WithProvince(lTD.provinceTag);
                

                if(lTD.ownerTag != "")
                {
                    tileBuilder.WithOccupier(lTD.occupierTag);
                }

               
                 LandTile landTile = tileBuilder.Build(_registery, _errorHandler);
                TileDictionnary.Add(HexToInt(lTD.spriteColor), landTile);
            
            }
            else
            {
                WaterTileDTO wTD = tile.ToObject<WaterTileDTO>();
                

                WaterTileBuilder builder = new WaterTileBuilder()
                    .WithName(wTD.name)
                    .WithTag(wTD.tag)
                    .WithSpriteColor(wTD.spriteColor)
                    .WithSuperficy(wTD.superficy)
                    .WithIsPassble(wTD.isPassable)
                    .WithType(wTD.typeTag);
                
                WaterTile waterTile = builder.Build(_registery, _errorHandler);

               // TileColorIntToId[waterTile.spriteColor] = HexToInt(wTD.spriteColor);
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

