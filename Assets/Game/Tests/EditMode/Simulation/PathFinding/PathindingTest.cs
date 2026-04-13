using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static ColorUtilities;
public class PathindingTest {
    
    GraphDataLoder graphLoader = new GraphDataLoder();
    TileLoader tileLoader = new TileLoader();
    private string graphTestJsonPath = Path.Combine(
         Application.dataPath,
         "Game",
         "Tests",
         "EditMode",
         "Simulation",
         "PathFinding",
         "mapJson");


    [Test]
    public void PathFindingTest()
    {
        IResolutionErrorHandler _errorHandler = new ThrowErrorHandler();
        string jsontest01Path = Path.Combine(graphTestJsonPath, "01");

        string tileGraphJson = GetJsonStringFromFile(jsontest01Path, "tileGraph");
        string tileDataJson = GetJsonStringFromFile(jsontest01Path, "tileData");
     
        DataRegistery _registery = new DataRegistery();

        TerrainType terrainType = new TerrainType();
        terrainType.name = "Default";
        terrainType.tag = "Default";
        terrainType.isLandType = true;
        terrainType.movementCost = 1;
        TerrainType[] array = new TerrainType[] { terrainType };
        _registery.terrainTypes = array;

        Assert.IsNotNull(_registery.terrainTypes);
        Assert.AreEqual(1, _registery.terrainTypes.Length); 
       TerrainType test = _registery.GetTerrainTypeById(_registery.GetTerrainTypes("Default"));
        Assert.IsNotNull(test);
        Assert.AreEqual(1, test.movementCost);
        _registery.tiles = tileLoader.DeserializeTiles(tileDataJson, _registery, _errorHandler);
        _registery = graphLoader.Load(tileGraphJson, _registery, _errorHandler);
    
       Pathfinding pathfinder = new Pathfinding(4);
       MapGraphNode start = _registery.GetMapGraphNode(HexToInt("#0BB235"));
       MapGraphNode end = _registery.GetMapGraphNode(HexToInt("#77D290"));
       List<MapGraphNode> result = pathfinder.FindPath(start, end, new List<UnitTerrainSpeedModdifier>(),_registery);
        
        foreach (MapGraphNode node in result)
        {
            Debug.Log(node.position);
        }
        
        Assert.IsNotNull(result);
        Assert.AreEqual(2, result.Count);//start node is included

    }
    //MapGraph a = new MapGraph()
    //Pathfinding pathfinder = new Pathfinding(3f);

    //multiple MapGraphNode
    //Tile with graph node id
    //Terrain type
    //one unit

    //Create a json for this

    private string GetJsonStringFromFile(string dataPath, string fileName)
    {
        string filePath = Path.Combine(
         dataPath,
         $"{fileName}.json"
     );

        if (File.Exists(filePath))
        {
            return File.ReadAllText(filePath);
        }

        Debug.LogError("JSON file not found at: " + filePath);
        return null;
    }
}
