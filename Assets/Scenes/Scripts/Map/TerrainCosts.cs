using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainCosts : MonoBehaviour
{
 /*
    public class ProvinceNode
    {
        public int id;
        public Vector2 center;
        public List<Edge> neighbors = new List<Edge>();
    }

    public class Edge
    {
        public ProvinceNode to;
        public float baseDistance;
        public TerrainType terrain;
        public bool hasRoad;
        public float roadModifier;

        public float GetMovementCost()
        {
            float terrainMod = TerrainCosts[terrain];
            return baseDistance * terrainMod * roadModifier;
        }
    }

    public enum TerrainType
    {
        Plains, Forest, Mountain, Desert
    }

    // having one file with all game "rules" could be nice

    /*
    public static Dictionary<TerrainType, float> TerrainCosts = new()
    {
        { TerrainType.Plains, 1.0f },
        { TerrainType.Forest, 1.3f },
        { TerrainType.Mountain, 2.0f },
        { TerrainType.Desert, 1.5f }
    };
    */
}
