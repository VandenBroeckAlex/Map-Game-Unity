using System.Collections.Generic;
using UnityEngine;

public class MapGraphNode
{
    private int _provinceId;
    Dictionary<int, int> _neighboreDistance;

    public MapGraphNode(int provinceId)
    {
        _provinceId = provinceId;
        _neighboreDistance = new Dictionary<int, int>();
    }

    public int GetProvinceId()
    {
        return _provinceId;
    }
    public Dictionary<int, int> GetNeighboresDistance()
    {
        return _neighboreDistance;
    }

    public void AddNeighbor(int provinceId, int distance)
    {
        _neighboreDistance[provinceId] = distance;
    }
}