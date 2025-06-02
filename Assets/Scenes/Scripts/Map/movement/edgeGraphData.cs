using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class edgeGraphData : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]
    enum TerrainType {plaine, forest, mountain, marsh, desert, jungle }

    
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
    }
    public class JSONData
    {
        public SpriteObjJSON[] spriteListJSON;
    }

    JSONData provincePosition;

    void Start()
    {
        calculateEdge();
    }

    void calculateEdge()
    {
        DeserializeJSON();
        foreach (var provinceEntry in provincePosition.spriteListJSON)
        {
            //get id get neighbore


        }

    }
    void DeserializeJSON()
    {
        string provincePath = Path.Combine(Application.persistentDataPath, "map_info.json");
        string provinceJson = File.ReadAllText(provincePath);
        provincePosition = JsonConvert.DeserializeObject<JSONData>(provinceJson);
    }
}



/* This is the wanted result
"edges": [
    {
      "from": 0,
      "to": 3,
      "baseDistance": 5.6,
      "terrain": "plains",
      "hasRoad": true,
      "roadModifier": 0.8
    },
    {
    "from": 0,
      "to": 4,
      "baseDistance": 8.2,
      "terrain": "forest",
      "hasRoad": false,
      "roadModifier": 1.0
    }
  ]
*/