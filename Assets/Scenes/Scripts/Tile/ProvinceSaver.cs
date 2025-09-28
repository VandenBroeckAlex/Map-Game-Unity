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
        public int type;
        public int owner;
        public int[] neighbors;
        public int lowerX;
        public int higherY;
    }
    public class JSONData
    {
        public int canvaWidth;
        public int canvaHeight;
        public ObjJSON[] spriteListJSON;
    }

    JSONData provincePosition;
    void LoadJsonDataMapPosition()
    {
        string jsonFile = File.ReadAllText(fullPath);
        provincePosition = JsonConvert.DeserializeObject<JSONData>(jsonFile);
    }


    List<ObjJSON> ListObjJSON;

    public class CombinedJSON
    {
        public int canvaWidth;
        public int canvaHeight;

        public List<ObjJSON> spriteListJSON;
        public CombinedJSON(int width, int height, List<ObjJSON> list)
        {
            canvaWidth = width;
            canvaHeight = height;
            spriteListJSON = list;
        }
    }

    public void SaveProvinceData()
    {
        LoadJsonDataMapPosition();  

        // Get all provinces from the ProvincesManager
        var provinceHandler = GameObject.Find("/ProvincesManager").GetComponent<ProvincesManager>();
        Dictionary<int, Tile> allProvinces = provinceHandler.provinces_list;

    
        List<ObjJSON> ListObjJSON = new List<ObjJSON>(provincePosition.spriteListJSON);

        foreach (var kvp in allProvinces)
        {
            Tile province = kvp.Value;
            ObjJSON jsonObj = ListObjJSON.FirstOrDefault(obj => obj.id == province.id);
            if (jsonObj != null)
            {
                jsonObj.name = province.name;             
                jsonObj.neighbors = province.neighbors;
                jsonObj.isLand = province.isLand;
                jsonObj.isPassable = province.isPassable;
     
            }
        }
        CombinedJSON combinedData = new CombinedJSON(provincePosition.canvaWidth, provincePosition.canvaHeight, ListObjJSON);
        string output = JsonConvert.SerializeObject(combinedData, Formatting.Indented);
        File.WriteAllText(fullPath, output);
        Debug.Log("Province data saved!");
    }





}
