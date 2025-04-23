using JetBrains.Annotations;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
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
        public int ownerId;
        public int[] neighbors;
        private int owner;

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
            allProvinces.Add(new Province(provinceEntry.id, provinceEntry.name, provinceEntry.description, provinceEntry.owner, provinceEntry.neighbors));
  
        }
    }

    void LoadJsonDataMapPosition()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("map_position");
        provincePosition = JsonConvert.DeserializeObject<JSONData>(jsonFile.text);
    }

}
