using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MyGame.Data;



public class ProvinceHandeler1 : MonoBehaviour
{


    [SerializeField] public List<Province> allProvinces = new();
    //ux
    public Material MatTileHiglight;

 

    [Serializable]
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

    public class ProvinceListData
    {
        public List<Province> ProvinceList;
    }


  
    private void Start()
    {
        LoadJsonDataMapPosition();

        foreach (var provinceEntry in provincePosition.spriteListJSON)
        {
            allProvinces.Add(new Province(
                                         provinceEntry.id,
                                         provinceEntry.name,
                                         provinceEntry.description,
                                         provinceEntry.owner,
                                         provinceEntry.neighbors
                                         ));

        }
    }

    void LoadJsonDataMapPosition()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "map_info.json");
        string jsonFile = File.ReadAllText(fullPath);
        provincePosition = JsonConvert.DeserializeObject<JSONData>(jsonFile);
    }

}
