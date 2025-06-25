using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using MyGame.Data;
using UnityEngine.UIElements;

public class ProvinceSaver : MonoBehaviour
{
  
    string fullPath = FilePath.MapInfo;
    public class SpriteObjJSON
    {
        public int id;
        public string name;
        public int superficy;
        public int Type;
        public int owner;
        public int[] neighbors;
        public int lowerX;
        public int higherY;
    }
    public class JSONData
    {
        public int canvaWidth;
        public int canvaHeight;
        public SpriteObjJSON[] spriteListJSON;
    }

    JSONData provincePosition;
    void LoadJsonDataMapPosition()
    {
        string jsonFile = File.ReadAllText(fullPath);
        provincePosition = JsonConvert.DeserializeObject<JSONData>(jsonFile);
    }


    List<Province> ListObjJSON = new List<Province>();

    public class CombinedJSON
    {
        public int canvaWidth;
        public int canvaHeight;

        public List<Province> spriteListJSON;
        public CombinedJSON(int width, int height, List<Province> list)
        {
            canvaWidth = width;
            canvaHeight = height;
            spriteListJSON = list;
        }
    }

    public void SaveProvinceData()
    {
        LoadJsonDataMapPosition();
        var provinceHandler = GameObject.Find("/ProvincesManager").GetComponent<ProvincesManager>();
        Dictionary<int, Province> allProvinces = provinceHandler.allProvinces;
        //replace spriteListJSON with allProvinces from province handeler
        //provincePosition.spriteListJSON = newList
        foreach (var kvp in allProvinces)
        {
            Province province = kvp.Value; // get the actual Province object
            

            Province jsonObj = new Province
            {
                id = province.id,
                name = province.name,           
                type = province.type,
                isLand = province.isLand,
                isPassable = province.isPassable,
                ownerId = province.ownerId,
                neighbors = province.neighbors,
                owner = province.owner,
                occupierID = province.occupierID,
                rgo = province.rgo,
            };

            ListObjJSON.Add(jsonObj);
        }

        CombinedJSON combinedData = new CombinedJSON(provincePosition.canvaWidth, provincePosition.canvaHeight, ListObjJSON);
        string output = JsonConvert.SerializeObject(combinedData, Formatting.Indented);
        File.WriteAllText(fullPath, output);
        Debug.Log("Province data saved!");
    }



}
