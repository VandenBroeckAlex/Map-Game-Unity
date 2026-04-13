using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static ColorUtilities;
public class PathindingTest {
    
 
    private string graphTestJsonPath = Path.Combine(
         Application.dataPath,
         "Game",
         "Tests",
         "EditMode",
         "Simulation",
         "PathFinding",
         "mapJson");
    private GraphDataLoder graphLoader;
    private TileLoader tileLoader;
    [SetUp]
    public void Setup()
    {
        graphLoader = new GraphDataLoder();
        tileLoader = new TileLoader();
    }
    public DataRegistery GetGraph01()
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

        
        TerrainType test = _registery.GetTerrainTypeById(_registery.GetTerrainTypes("Default"));
     
        _registery.tiles = tileLoader.DeserializeTiles(tileDataJson, _registery, _errorHandler);
        _registery = graphLoader.Load(tileGraphJson, _registery, _errorHandler);
        return _registery;   
    }
    public DataRegistery GetGraph02()
    {
        IResolutionErrorHandler _errorHandler = new ThrowErrorHandler();
        string jsontest02Path = Path.Combine(graphTestJsonPath, "02");

        string tileGraphJson = GetJsonStringFromFile(jsontest02Path, "tileGraph");
        string tileDataJson = GetJsonStringFromFile(jsontest02Path, "tileData");

        DataRegistery _registery = new DataRegistery();

        TerrainType terrainType = new TerrainType();
        terrainType.name = "Default";
        terrainType.tag = "Default";
        terrainType.isLandType = true;
        terrainType.movementCost = 1;
        TerrainType[] array = new TerrainType[] { terrainType };
        _registery.terrainTypes = array;


        TerrainType test = _registery.GetTerrainTypeById(_registery.GetTerrainTypes("Default"));

        _registery.tiles = tileLoader.DeserializeTiles(tileDataJson, _registery, _errorHandler);
        _registery = graphLoader.Load(tileGraphJson, _registery, _errorHandler);
        return _registery;
    }

    public DataRegistery  GetGraph03()
    {
        IResolutionErrorHandler _errorHandler = new ThrowErrorHandler();
        string jsontest03Path = Path.Combine(graphTestJsonPath, "03");

        string tileGraphJson = GetJsonStringFromFile(jsontest03Path, "tileGraph");
        string tileDataJson = GetJsonStringFromFile(jsontest03Path, "tileData");

        DataRegistery _registery = new DataRegistery();

        TerrainType terrainType = new TerrainType();
        terrainType.name = "Default";
        terrainType.tag = "Default";
        terrainType.isLandType = true;
        terrainType.movementCost = 1;

        TerrainType terrainType2 = new TerrainType();
        terrainType2.name = "Costly";
        terrainType2.tag = "Costly";
        terrainType2.isLandType = true;
        terrainType2.movementCost = 1000; // Is in %

        TerrainType[] array = { terrainType, terrainType2 };
        _registery.terrainTypes = array;

        _registery.terrainTypesTags = new string[] { "Default", "Costly" };

        TerrainType test = _registery.GetTerrainTypeById(_registery.GetTerrainTypes("Default"));

        _registery.tiles = tileLoader.DeserializeTiles(tileDataJson, _registery, _errorHandler);
        _registery = graphLoader.Load(tileGraphJson, _registery, _errorHandler);
        return _registery;
    }
    [Test]
    public void Pathfinder_outputRightAmmountOfNode()
    {
        
       DataRegistery _registery = GetGraph01();
       Pathfinding pathfinder = new Pathfinding(3);
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
    [Test]
    public void Pathfinder_ignore_waterAndUnpassable()
    {

        DataRegistery _registery = GetGraph01();
        Pathfinding pathfinder = new Pathfinding(3);
        MapGraphNode start = _registery.GetMapGraphNode(HexToInt("#0BB235"));
        MapGraphNode end = _registery.GetMapGraphNode(HexToInt("#716cd6"));
        List<MapGraphNode> result = pathfinder.FindPath(start, end, new List<UnitTerrainSpeedModdifier>(), _registery);

        Assert.IsNotNull(result);
        //foreach (MapGraphNode node in result)
        //{
        //    Debug.Log(node.position);
        //}

        
        Assert.AreEqual(7, result.Count);//start node is included

    }

    [Test]
    public void PathFinder_BlokedPath_returnNull()
    {

        DataRegistery _registery = GetGraph02();
        Pathfinding pathfinder = new Pathfinding(3);
        MapGraphNode start = _registery.GetMapGraphNode(HexToInt("#0BB235"));
        MapGraphNode end = _registery.GetMapGraphNode(HexToInt("#716cd6"));
        List<MapGraphNode> result = pathfinder.FindPath(start, end, new List<UnitTerrainSpeedModdifier>(), _registery);

        Assert.IsNull(result);
        //foreach (MapGraphNode node in result)
        //{
        //    Debug.Log(node.position);
        //}
    }
    [Test]
    public void PathFinder_MovementCost_return7()
    {

        DataRegistery _registery = GetGraph03();
        Pathfinding pathfinder = new Pathfinding(3);
        MapGraphNode start = _registery.GetMapGraphNode(HexToInt("#0BB235"));
        MapGraphNode end = _registery.GetMapGraphNode(HexToInt("#716cd6"));
        List<MapGraphNode> result = pathfinder.FindPath(start, end, new List<UnitTerrainSpeedModdifier>(), _registery);

        foreach (MapGraphNode node in result)
        {
            Debug.Log(node.position);
        }

        Assert.IsNotNull(result);
        Assert.AreEqual(7, result.Count);//start node is included
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
