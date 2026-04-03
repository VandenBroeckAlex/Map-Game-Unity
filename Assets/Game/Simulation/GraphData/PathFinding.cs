using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class Pathfinding
{

    float _mapWidth;
    public Pathfinding(float mapWidth) {  this._mapWidth = mapWidth; }

    public List<MapGraphNode> FindPath(MapGraphNode start, MapGraphNode target)
    {
        // Nodes to be evaluated
        List<MapGraphNode> openSet = new List<MapGraphNode> { start };
        // Nodes already evaluated
        HashSet<MapGraphNode> closedSet = new HashSet<MapGraphNode>();

        // Records where we came from to reconstruct the path later
        Dictionary<MapGraphNode, MapGraphNode> cameFrom = new Dictionary<MapGraphNode, MapGraphNode>();

        // Cost from start to the node
        Dictionary<MapGraphNode, float> gScore = new Dictionary<MapGraphNode, float>();
        gScore[start] = 0;

        while (openSet.Count > 0)
        {
            // 1. Get the node in openSet with the lowest fScore (Standard A*)
            MapGraphNode current = GetLowestFScore(openSet, gScore, target,_mapWidth);

            if (current == target)
                return ReconstructPath(cameFrom, current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (var neighborEntry in current.GetNeighbores())
            {
                MapGraphNode neighbor = neighborEntry.Node;
                if (closedSet.Contains(neighbor)) continue;

                // distance between current and neighbor
                float tentativeGScore = gScore[current] + neighborEntry.Distance;

                if (!openSet.Contains(neighbor))
                    openSet.Add(neighbor);
                else if (tentativeGScore >= gScore[neighbor])
                    continue; // Not a better path

                cameFrom[neighbor] = current;
                gScore[neighbor] = tentativeGScore;
            }
        }
        return null; // No path found
    }

    private float GetWrappedHeuristic(System.Numerics.Vector2 a, System.Numerics.Vector2 b, float mapWidth)
    {
        float dx = Mathf.Abs(a.X - b.X);
        float dy = Mathf.Abs(a.Y - b.Y);

        // If the distance across the screen is larger than half the map,
        // it's actually shorter to wrap around.
        if (dx > mapWidth / 2)
        {
            dx = mapWidth - dx;
        }

        // Standard Euclidean distance using the adjusted dx
        return Mathf.Sqrt(dx * dx + dy * dy);
    }
    private MapGraphNode GetLowestFScore(List<MapGraphNode> openSet, Dictionary<MapGraphNode, float> gScore, MapGraphNode target, float mapWidth)
    {
        MapGraphNode bestNode = openSet[0];
        float minF = float.MaxValue;

        foreach (var node in openSet)
        {
            float f = gScore[node] + GetWrappedHeuristic(node.position, target.position, mapWidth);
            if (f < minF)
            {
                minF = f;
                bestNode = node;
            }
        }
        return bestNode;
    }

    private List<MapGraphNode> ReconstructPath(Dictionary<MapGraphNode, MapGraphNode> cameFrom, MapGraphNode current)
    {
        List<MapGraphNode> path = new List<MapGraphNode> { current };
        while (cameFrom.ContainsKey(current))
        {
            current = cameFrom[current];
            path.Insert(0, current);
        }
        return path;
    }
}