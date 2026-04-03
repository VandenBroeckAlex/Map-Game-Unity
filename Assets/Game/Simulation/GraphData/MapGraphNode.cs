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
        public MapGraphNode Node; // Direct reference (faster)
        public int Distance;
    }

    public MapGraphNode(int id, int terrainType,Vector2 postion)
    {

    }
    
    
    private int _tileId;
    private int _terrainTypeId;
    public Vector2 position;
    private List<Neighbor> _neighbors = new List<Neighbor>();
    

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
    public void setTerrainId(int terrainId)
    {
        _terrainTypeId = terrainId;
    }
    public void AddNeighbor(Neighbor neighbore)
    {
        _neighbors.Add(neighbore);
    }

}