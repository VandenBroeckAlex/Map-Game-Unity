using Newtonsoft.Json;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SpriteCreator_v3;



public class ProvinceHandeler1 : MonoBehaviour
{


    [SerializeField] public List<Province> allProvinces = new();
    //ux
    public Material MatTileHiglight;

    [Serializable]
    public class Province
    {
        public string name;
        public string description;
        public int id;
        public int type;
        public bool isLand;
        public int ownerId;
        public int[] neighbors;
        public int owner;    
        public int occupierID;

        public Province(int id, string name, string description, int owner, int[] neighbors)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.owner = owner;
            this.neighbors = neighbors;

        }

        public Province(int givenID, string givenName, string givenDescription, int givenType, int givenOwner, int[] givenneighbors) { 
            id = givenID;
            description = givenDescription;
            name = givenName;
            type = givenType;
            ownerId = givenOwner;
            neighbors = givenneighbors;

        }
    }

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
            Debug.Log(provinceEntry.neighbors.ToString());
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
