using System;
using System.Collections.Generic;
using System.Numerics;

public class MapGraphNode
{
    //add a terain type id
    // unit will give its terrain state
    // multiply distance by modifier

    //passage right
    public struct Neighbor
    {
        public MapGraphNode node; // Direct reference (faster)
        public int distance;
    }

    private int _tileId;
    private int _terrainTypeId;
    public Vector2 position;
    private List<Neighbor> _neighbors = new List<Neighbor>();

    public MapGraphNode(int id, int terrainType, Vector2 postion)
    {
        _tileId = id;
        _terrainTypeId = terrainType;
        this.position = postion;

    }

    public int GetProvinceId()
    {
        return _tileId;
    }
    public List<Neighbor> GetNeighbores()
    {
        return _neighbors;
    }
    public int GetTerrainId()
    {
        return _terrainTypeId;
    }
    public void SetTerrainId(int terrainId)
    {
        _terrainTypeId = terrainId;
    }
    public void AddNeighbor(MapGraphNode node, int distance)
    {

    Neighbor neighbore = new Neighbor();
        neighbore.distance = distance;
        neighbore.node = node;
        _neighbors.Add(neighbore);
    }

  }
