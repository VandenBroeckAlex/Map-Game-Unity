using MyGame.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static RaycastScript;


[SerializeField]
public class ProvincesManager : MonoBehaviour
{


    [SerializeField] 
    public Dictionary<int, Tile>  provinces_list = new Dictionary<int, Tile>();

    //ux
    public Material MatTileHiglight;
   
    public class SpriteObjJSON
    {
        public int id;
        public string name;
        public string description;
        public int Type;
        public int owner;
        public List<int> neighbors;
        public int lowerX;
        public int higherY;
    }
    public class JSONData
    {
       
        public SpriteObjJSON[] spriteListJSON;
     
    }

    List<Tile> provinces = new List<Tile>();
    Dictionary<int, float[]> jsonData;

    public class ProvinceListData
    {
        public List<Tile> ProvinceList;
    }

    public delegate void OnProvinceUpdated();
    public static OnProvinceUpdated onProvinceUpdated;


    public ProvinceUIController uiController;

    [Obsolete]
    private void Start()
    {
        // initialization should be change later being in Start will cause problem sooner or later
        InitializeHandeler();
        
    }

    [Obsolete]//do not remove this
    void InitializeHandeler()
    {
        RaycastScript.onProvincePlaneHit += GetProvinceId;
        LoadJsonTileInfo();
        Debug.Log($"number of province in json {provinces.Count}");
        //faire une fonction du serializing values in manager
        foreach (var provinceEntry in provinces)
        {
            if(provinceEntry is LandTile Ltile)
            {
                LandTile _newTile = new LandTile(provinceEntry.id);
                _newTile.name = Ltile.name;
                _newTile.ownerId = Ltile.ownerId;
                _newTile.neighbors = Ltile.neighbors;
                provinces_list.Add(Ltile.id, _newTile);
            }
            if (provinceEntry is WaterTile Wtile)
            {
                WaterTile _newTile = new WaterTile(Wtile.id);
                _newTile.name = Wtile.name;
                _newTile.neighbors = Wtile.neighbors;
                provinces_list.Add(Wtile.id, _newTile);
            }



        }
      
        Debug.Log("number of province loaded :" + provinces_list.Count);

        if (uiController == null)
            uiController = FindObjectOfType<ProvinceUIController>();
    }
    void LoadJsonTileInfo()
    {
        string fullPath = FilePath.ColorId;
        string jsonFile = File.ReadAllText(fullPath);

        jsonData = JsonConvert.DeserializeObject<Dictionary<int, float[]>>(jsonFile);

      
        string provincePath = FilePath.TilesInfos;
        string provinceJson = File.ReadAllText(provincePath);
        JArray listTile = JArray.Parse(provinceJson);

        foreach (JObject tile in listTile)
        {
            if (tile == null) continue; // safety
            bool isLand = tile["isLand"]?.Value<bool>() ?? false;

            if (isLand)
            {
                LandTile landTile = tile.ToObject<LandTile>();
                provinces.Add(landTile);
            }
            else
            {
                WaterTile waterTile = tile.ToObject<WaterTile>();
                provinces.Add(waterTile);
            }
        }

        //provinces 
    }

    public void OnProvinceClicked(int id)
    {
      
        uiController.ShowProvinceInfo(provinces_list[id]);
    }

    public int GetProvinceIdByColor(Color color)
    {
        
        ProvincesManager bScript = GetComponent<ProvincesManager>();
        Dictionary<int, Tile> allProvinces = bScript.provinces_list;

        foreach (var kvp in jsonData)
        {
            float[] stored = kvp.Value;

            // Compare exact color values (no tolerance)
            if (color.r == stored[0] && color.g == stored[1] && color.b == stored[2])
            {
                Debug.Log(kvp.Key);
                return kvp.Key; // Found a match
            }
        }

        // No match found
        Debug.Log("No match found");
        return -1;
    }

    // work if no province is deleted !
    Tile GetProvinceInfoById(int id)
    {
        return provinces_list[id];
    }

    public void GetProvinceId(Color color)
    {
        int  id = GetProvinceIdByColor((Color)color);
        Tile recivedProvince = GetProvinceInfoById(id);
        OnProvinceClicked(id);
    }


    

}
