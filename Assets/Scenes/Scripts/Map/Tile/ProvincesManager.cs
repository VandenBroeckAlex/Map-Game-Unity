using MyGame.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static RaycastScript;


[SerializeField]
public class ProvincesManager : MonoBehaviour
{
    public static ProvincesManager instance;

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

    //lookup textures:
 
    private Texture2D lookupTex;
    [SerializeField] private Material terrainMaterial;

    private void Awake()
    {
        if(instance  == null)
        {
            instance = this;
        }
       
     
        else 
        {
            Destroy(gameObject);
            Debug.Log("An instence of Province manager already exist");
        }
    }

    [Obsolete]
    private void Start()
    {
        if (terrainMaterial == null)
            terrainMaterial = Resources.Load<Material>("Materials/PoliticalMap");

        Debug.Log("terrainMaterial: " + terrainMaterial);

        // Load province map
        Texture2D provinceMap = FileUtils.LoadBaseImage(FilePath.ProvinceMapImg);

        // Force exact linear uncompressed format
        Color32[] pixels = provinceMap.GetPixels32();
        Texture2D linearProvinceMap = new Texture2D(provinceMap.width, provinceMap.height, TextureFormat.RGBA32, false, true);
        linearProvinceMap.SetPixels32(pixels);
        linearProvinceMap.Apply(false, true);

        provinceMap = linearProvinceMap;

        provinceMap.filterMode = FilterMode.Point;
        provinceMap.wrapMode = TextureWrapMode.Clamp;



        terrainMaterial.SetTexture("_ProvinceTex", linearProvinceMap);

        InitializeHandeler();

        lookupTex = new Texture2D(4096, 4096, TextureFormat.RGBA32, false);
        lookupTex.filterMode = FilterMode.Point;
        lookupTex.wrapMode = TextureWrapMode.Clamp;

        
        BuildPoliticalLookupTexture();

        Debug.Log("Lookup texture size: " + lookupTex.width + "x" + lookupTex.height);
        terrainMaterial.SetTexture("_LookupTex", lookupTex);
        File.WriteAllBytes(Application.dataPath + "/lookup_debug.png", lookupTex.EncodeToPNG());
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
                _newTile.spriteColor = Ltile.spriteColor;
                provinces_list.Add(Ltile.id, _newTile);
            }
            if (provinceEntry is WaterTile Wtile)
            {
                WaterTile _newTile = new WaterTile(Wtile.id);
                _newTile.name = Wtile.name;
                _newTile.neighbors = Wtile.neighbors;
                _newTile.spriteColor = Wtile.spriteColor;
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


    public void BuildPoliticalLookupTexture()
    {
        int texSize = 4096;
        Color[] pixels = new Color[texSize * texSize];

        // default color 
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.red;

        foreach (var kv in provinces_list)
        {
            Tile province = kv.Value;
            if (!province.isLand) continue;


            Color32 c = new Color32(
            (byte)Mathf.RoundToInt(province.spriteColor[0] * 255f),
            (byte)Mathf.RoundToInt(province.spriteColor[1] * 255f),
            (byte)Mathf.RoundToInt(province.spriteColor[2] * 255f),
            255);

            LandTile landprovince = (LandTile)province;
            Debug.Log($"Province {province.name}: spriteColor=({province.spriteColor[0]}, {province.spriteColor[1]}, {province.spriteColor[2]}");
            Color32 ownerColor = CountriesManager.instance.GetCountryColorById(landprovince.ownerId);
            ownerColor.a = 255;
            int index = (c.r << 16) | (c.g << 8) | c.b;
            Debug.Log($"Province {province.name}: rgb=({c.r},{c.g},{c.b}) index={index}");
          
            int texX = index % texSize;
            int texY = index / texSize;

            // Safety check
            if (texX >= 0 && texX < texSize && texY >= 0 && texY < texSize)
                pixels[texY * texSize + texX] = ownerColor;
        }

        lookupTex = new Texture2D(texSize, texSize, TextureFormat.RGBA32, false);
        lookupTex.filterMode = FilterMode.Point;
        lookupTex.wrapMode = TextureWrapMode.Clamp;      
        lookupTex.SetPixels(pixels);
        lookupTex.Apply(false,false);
        //lookupTex.GetPixel();
        terrainMaterial.SetTexture("_LookupTex", lookupTex);

        int count = pixels.Count(p => p != Color.red);
        Debug.Log($"LookupTex non-white pixels: {count}");

    }

    //public void RefreshProvinceColor(Color provinceColor)
    //{
    //    // called when province ownership changes
    //    int index = provinceManager.GetProvinceIndex(provinceColor);
    //    int x = index % 256;
    //    int y = index / 256;

    //    Color newColor = provinceManager.GetCountryColorByProvinceColor(provinceColor);
    //    lookupTex.SetPixel(x, y, newColor);
    //    lookupTex.Apply();
    //}

}
