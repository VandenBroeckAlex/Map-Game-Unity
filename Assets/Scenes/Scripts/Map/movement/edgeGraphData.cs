using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using static edgeGraphData;
using static UnityEngine.Rendering.DebugUI;

public class edgeGraphData : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    enum TerrainType {plaine, forest, mountain, marsh, desert, jungle }

    private List<EdgeObj> JSONObj = new List<EdgeObj>();

    public class SpriteObjJSON
    {
        public int id;
        public string name;
        public string description;
        public int Type;
        public int owner;
        public int[] neighbors;
        public int lowerX;
        public int higherY;
        public float[] center;
    }
    public class JSONData
    {
        public SpriteObjJSON[] spriteListJSON;
    }

    public class EdgeObj
    {
        public int from;
        public int to;
        public float baseDistance;

    }

    JSONData provincePosition;

    void Start()
    {
        calculateEdge();
    }

    // next step remove both edges  x -> y  y -> x
    // bidirectional will always be true
    void calculateEdge()
    {
        DeserializeJSON();
        foreach (var provinceEntry in provincePosition.spriteListJSON)
        {
            foreach(var neighbore in  provinceEntry.neighbors)
            {
               
                EdgeObj edgeObj = new EdgeObj();
                edgeObj.from = provinceEntry.id;
                edgeObj.to = neighbore;

              
                float[] neighboreCenter = new float[2];
                bool found = false;

                foreach (var province in provincePosition.spriteListJSON)
                {
                    if (province.id == neighbore)
                    {
                        neighboreCenter = province.center;
                        found = true;
                        break;
                    }
                }

                if (found)
                {
                    double distance = Math.Sqrt(
                        Math.Pow(provinceEntry.center[0] - neighboreCenter[0], 2) +
                        Math.Pow(provinceEntry.center[1] - neighboreCenter[1], 2)
                    );
                   
                    edgeObj.baseDistance = (float)System.Math.Round(distance, 2); ; // Cast double to float and Leave only two decimal places after the dot
                }
                else
                {
                    Console.WriteLine($"Warning: Neighbor with ID {neighbore} not found.");
                }

                JSONObj.Add(edgeObj);
            }            
        }
        RemoveDuplicate();
        CreateJSON(JSONObj);
    }
    void DeserializeJSON()
    {
        string provincePath = FilePath.MapInfo;
        string provinceJson = File.ReadAllText(provincePath);
        provincePosition = JsonConvert.DeserializeObject<JSONData>(provinceJson);
    }

    void CreateJSON(List <EdgeObj> Data) 
    {
        string output = JsonConvert.SerializeObject(Data, Formatting.Indented);
        File.WriteAllText(FilePath.MapEdge, output);
    }

    void RemoveDuplicate()
    {
        HashSet<string> seenEdges = new();

        
        for (int i = JSONObj.Count - 1; i >= 0; i--)
        {
            int a = JSONObj[i].from;
            int b = JSONObj[i].to;

            
            int min = Math.Min(a, b);
            int max = Math.Max(a, b);
            string key = $"{min}-{max}";

            if (seenEdges.Contains(key))
            {
                JSONObj.RemoveAt(i);
            }
            else
            {
                seenEdges.Add(key);
            }
        }
    }
}



