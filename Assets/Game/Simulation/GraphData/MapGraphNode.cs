using System;
using System.Collections.Generic;
using System.Numerics;

public class MapGraphNode
{
    public struct Neighbor
    {
        public MapGraphNode Node; // Direct reference (fast!)
        public int Distance;
    }

    
    
    public int ProvinceId;
    public Vector2 Position;
    public List<Neighbor> neighbors = new List<Neighbor>();
    

    public int GetProvinceId()
    {
        return _provinceId;
    }
    public List<ValueTuple<int, int>> GetNeighbores()
    {
        return _neighboreDistance;
    }

    public void AddNeighbor(Neighbor neighbore)
    {
        neighbors.Add(neighbore);
    }

}