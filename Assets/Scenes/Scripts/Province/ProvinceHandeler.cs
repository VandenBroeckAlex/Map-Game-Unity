using MyGame.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static RaycastScript;



public class ProvinceHandeler : MonoBehaviour
{


    [SerializeField] public Dictionary<int, Province>  allProvinces = new Dictionary<int, Province>();
    //ux
    public Material MatTileHiglight;
   
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
    Dictionary<int, float[]> jsonData;

    public class ProvinceListData
    {
        public List<Province> ProvinceList;
    }

    public delegate void OnProvinceUpdated();
    public static OnProvinceUpdated onProvinceUpdated;


    public ProvinceUIController uiController;

    private void Start()
    {
        // initialization should be change later being in Start will cause problem sooner or later
        InitializeHandeler();
        
    }

    [Obsolete]
    void InitializeHandeler()
    {
        RaycastScript.onProvincePlaneHit += GetProvinceId;
        LoadJsonDataMapPosition();
        //faire une fonction du serializing values in handler
        foreach (var provinceEntry in provincePosition.spriteListJSON)
        {
            allProvinces.Add(provinceEntry.id, new Province(
                                         provinceEntry.id,
                                         provinceEntry.name,
                                         provinceEntry.description,
                                         provinceEntry.owner,
                                         provinceEntry.neighbors
                                         ));

        }
        Debug.Log("number of province loaded :" + allProvinces.Count);

        if (uiController == null)
            uiController = FindObjectOfType<ProvinceUIController>();
    }
    void LoadJsonDataMapPosition()
    {
        string fullPath = FilePath.ColorId;
        string jsonFile = File.ReadAllText(fullPath);

        jsonData = JsonConvert.DeserializeObject<Dictionary<int, float[]>>(jsonFile);

      
        string provincePath = FilePath.MapInfo;
        string provinceJson = File.ReadAllText(provincePath);
        provincePosition = JsonConvert.DeserializeObject<JSONData>(provinceJson);
    }

    public void OnProvinceClicked(int id)
    {
      
        uiController.ShowProvinceInfo(allProvinces[id]);
    }

    public int GetProvinceIdByColor(Color color)
    {
        GameObject g = GameObject.Find("ProvinceHandeler");
        ProvinceHandeler bScript = g.GetComponent<ProvinceHandeler>();
        Dictionary<int, Province> allProvinces = bScript.allProvinces;

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

    Province GetProvinceInfoById(int id)
    {
        return allProvinces[id];
    }

    public void GetProvinceId(Color color)
    {
        int  id = GetProvinceIdByColor((Color)color);
        Province recivedProvince = GetProvinceInfoById(id);
        OnProvinceClicked(id);
    }


    public void UpdateProvince(Province province)
    {

    }
}
