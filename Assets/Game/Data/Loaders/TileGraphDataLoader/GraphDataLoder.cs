using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine.Android;
using static ColorUtilities;

public class GraphDataLoder
{
    List<MapGraphNode> mapGraph = new List<MapGraphNode>();
    public LoaderDataRegistery Load(LoaderDataRegistery _registery, IResolutionErrorHandler _errorHandler)
    {
        string filePath = GamePaths.GetTileData("TileGraph");

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            DTOGraphData[] dtoGraphData = JsonConvert.DeserializeObject<DTOGraphData[]>(jsonText);

            //resolve hex to id
            foreach (DTOGraphData nodeData in dtoGraphData)
            {
                int id = HexToInt(nodeData.id);
                int terrainType = _registery.tiles[id].type;

                MapGraphNode mpg = new MapGraphNode(
                    id,
                    terrainType,
                    new System.Numerics.Vector2(nodeData.pivot[0], nodeData.pivot[1])
                    );
                mapGraph.Add(mpg);
            }

            foreach(DTOGraphData nodeData in dtoGraphData)
            {
                MapGraphNode node = GetNodeByHex(nodeData.id);
                foreach (KeyValuePair<string,int> kvp in nodeData.neighbors)
                {
                    MapGraphNode neighbore = GetNodeByHex(kvp.Key);

                    if (neighbore != null) 
                    {
                        
                        node.AddNeighbor(neighbore,kvp.Value);
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
       //TODO ERROR
       throw new FileNotFoundException();
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
