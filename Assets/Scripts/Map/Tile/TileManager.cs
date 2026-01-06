using MyGame.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using static RaycastScript;


[SerializeField]
public class TileManager 
{

    private GameContext context;

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
    Dictionary<int, float[]> colorIDList;

    public class ProvinceListData
    {
        public List<Tile> ProvinceList;
    }


    //UI
    public delegate void OnProvinceUpdated();
    public static OnProvinceUpdated onProvinceUpdated;
    public ProvinceUIController uiController;



    //Lookup textures:
    private Texture2D lookupTex;
    [SerializeField] private Material terrainMaterial;

    ProvinceColorIndexLUT _ProvinceColorIndexLUT = new();
    PoliticalMapLUT _PoliticalMapLUT = new ();



    public TileManager(GameContext context)
    {
        this.context = context;
    }

    public void Initialize()
    {
        if (terrainMaterial == null)
            terrainMaterial = Resources.Load<Material>("Materials/province_mat");

        InitializeManager();
        InitializeLookupTex();
    }

   
    public void InitializeManager()
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

                //TODO add RGO workplace
                context.workplaceManager.CreateGrainFarm(Ltile.id);
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
       
    }
    void LoadJsonTileInfo()
    {
        string fullPath = FilePath.ColorId;
        string jsonFile = File.ReadAllText(fullPath);

        colorIDList = JsonConvert.DeserializeObject<Dictionary<int, float[]>>(jsonFile);

      
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
        
    
        Dictionary<int, Tile> allProvinces = provinces_list;

        foreach (var kvp in colorIDList)
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


    Tile GetProvinceById(int id)
    {
        return provinces_list[id];
    }

    public void GetProvinceId(Color color)
    {
        int  id = GetProvinceIdByColor((Color)color);
        Tile recivedProvince = GetProvinceById(id);
        OnProvinceClicked(id);
    }

    public int GetProvinceOwnerByProvinceId(int id)
    {
       Tile province = GetProvinceById(id);

        if( province == null || province.isLand is false)
        {
            return -1;
        }
        else
        {
            LandTile Lprovince = (LandTile)province;
            return Lprovince.ownerId;
        }
    }
    //LUT handeling

    Texture2D LoadProvinceMap(string path)
    {
        byte[] data = File.ReadAllBytes(path);

        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false, true);

        
        tex.LoadImage(data, false);

        tex.filterMode = FilterMode.Point;
        tex.wrapMode = TextureWrapMode.Clamp;

        return tex;
    }

    Dictionary<Color32, int> BuildColorToIDMap(Dictionary<int, float[]> idColor)
    {
        Dictionary<Color32, int> map = new();

        foreach (KeyValuePair<int, float[]> entry in idColor)
        {
            Color _provColor = new(entry.Value[0], entry.Value[1], entry.Value[2], 1f);
            Color32 _provColor32 = _provColor;
            map[_provColor32] = entry.Key;
        }

        return map;
    }
    Texture2D BuildProvinceIDMap(Texture2D provinceMap, Dictionary<Color32, int> colorToID)
    {
        int width = provinceMap.width;
        int height = provinceMap.height;

        Texture2D idMap = new Texture2D(width, height, TextureFormat.RGBA32, false, true);
        idMap.filterMode = FilterMode.Point;
        idMap.wrapMode = TextureWrapMode.Clamp;

        Color32[] src = provinceMap.GetPixels32();
        Color32[] dst = new Color32[src.Length];

        for (int i = 0; i < src.Length; i++)
        {
            Color32 color = src[i];
            color.a = 255;

            int id = 0;
            if (!colorToID.TryGetValue(color, out id))
            {
                Debug.LogError($"Province color {color} not found in dictionary!");
                id = 0;
            }

            // store index as: index = R + G*255
            byte lo = (byte)(id & 0xFF);       // low byte  R
            byte hi = (byte)((id >> 8) & 0xFF); // high byte  G

            dst[i] = new Color32(lo, hi, 0, 255);
        }

        idMap.SetPixels32(dst);
        idMap.Apply();

        return idMap;
    }



    private void InitializeLookupTex()
    {
        // 1. Load province map
        Texture2D provinceMap = LoadProvinceMap(FilePath.ProvinceMapImg);
        
        terrainMaterial.SetTexture("_ProvinceTex", provinceMap);
        
        // 2. Load province definitions

        // 3. Build color ID lookup
        Dictionary<Color32, int> colorToID = BuildColorToIDMap(colorIDList);

        // 4. Build ProvinceIDMap (ID -> stored in RG)
        Texture2D idMap = BuildProvinceIDMap(provinceMap, colorToID);
        terrainMaterial.SetTexture("_ProvinceIDMap", idMap);

        // DEBUG
        File.WriteAllBytes(Application.dataPath + "/ProvinceID_debug.png", idMap.EncodeToPNG());

        // 5. Build the Political LUT (ID -> country color)
        Texture2D politicalLUT = PoliticalMapLUT.BuildPoliticalLUT(provinces_list);
        terrainMaterial.SetTexture("_LookupTex", politicalLUT);
        terrainMaterial.SetInt("_LookupWidth", politicalLUT.width);
        File.WriteAllBytes(Application.dataPath + "/PoliticalLUT_debug.png", politicalLUT.EncodeToPNG());

        
    }
}
