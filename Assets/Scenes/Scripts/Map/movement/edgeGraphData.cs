using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.IO;
using Unity.VisualScripting;
using static EdgeGraphData;
using static UnityEngine.Rendering.DebugUI;

public class EdgeGraphData : MonoBehaviour
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
}



