using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Android;
using static ColorUtilities;

public class GraphDataLoder
{
    List<MapGraphNode> mapGraph = new List<MapGraphNode>();
    public DataRegistery Load(string json, DataRegistery _registery, IResolutionErrorHandler _errorHandler)
    {
        string filePath = GamePaths.GetTileData("TileGraph");
        string jsonText;
        if (json != "")
        {
            jsonText = json;
        }
        else
        {
            if (File.Exists(filePath))
            {
                jsonText = File.ReadAllText(filePath);
            }
            else
            {
                throw new FileNotFoundException();
            }
        }

            DTOGraphData[] dtoGraphData = JsonConvert.DeserializeObject<DTOGraphData[]>(jsonText);

            //resolve hex to id
            foreach (DTOGraphData nodeData in dtoGraphData)
            {
                int id = HexToInt(nodeData.id);
                int terrainType = 0;
                bool isLand = true;
                if (_registery.tiles.TryGetValue(id, out Tile tile))
                {
                    terrainType = tile.type;
                    isLand = tile.isLand;
                }
                else
                {
                    throw new InvalidDataException($"Cound not find a tile with color : {nodeData.id} while creating Tile graph");
                }


                MapGraphNode mpg = new MapGraphNode(
                    id,
                    terrainType,
                    new System.Numerics.Vector2(nodeData.pivot[0], nodeData.pivot[1]),
                    isLand

                    );
                mapGraph.Add(mpg);
            }

            foreach (DTOGraphData nodeData in dtoGraphData)
            {
                MapGraphNode node = GetNodeByHex(nodeData.id);
                foreach (KeyValuePair<string, int> kvp in nodeData.neighbors)
                {
                    MapGraphNode neighbore = GetNodeByHex(kvp.Key);

                    if (neighbore != null)
                    {

                        node.AddNeighbor(neighbore, kvp.Value);
                    }
                    else
                    {
                        //TODO ERROR
                    }
                }
            }

            _registery.mapGraphNodes = mapGraph;
            return _registery;
        }
       
    

    public MapGraphNode GetNodeByHex(string hex)
    {
        int id = HexToInt(hex);

        foreach (MapGraphNode mpg in mapGraph)
        {
            if (mpg.GetProvinceId().Equals(id))
            {
                return mpg;
            }
        }
        return null;
    }
}

