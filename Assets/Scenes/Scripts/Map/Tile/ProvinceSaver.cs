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
        string JsonFile = File.ReadAllText(fullPath);
        provincePosition = JsonConvert.DeserializeObject<JSONData>(JsonFile);
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
        var ProvinceHandler = GameObject.Find("/ProvincesManager").GetComponent<ProvincesManager>();
        Dictionary<int, Tile> allProvinces = ProvinceHandler.provinces_list;

    
        List<ObjJSON> ListObjJSON = new List<ObjJSON>(provincePosition.spriteListJSON);

        foreach (var kvp in allProvinces)
        {
            Tile Province = kvp.Value;
            ObjJSON jsonObj = ListObjJSON.FirstOrDefault(obj => obj.Id == Province.id);
            if (jsonObj != null)
            {
                jsonObj.name = Province.name;             
                jsonObj.neighbors = Province.neighbors;
                jsonObj.isLand = Province.isLand;
                jsonObj.isPassable = Province.isPassable;
     
            }
        }
        CombinedJSON CombinedData = new CombinedJSON(provincePosition.canvaWidth, provincePosition.canvaHeight, ListObjJSON);
        string Output = JsonConvert.SerializeObject(CombinedData, Formatting.Indented);
        File.WriteAllText(fullPath, Output);
        Debug.Log("Province data saved!");
    }





}
