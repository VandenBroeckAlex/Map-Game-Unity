using MyGame.Data;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

using static ObjJSON;


public class SpriteCreator_v5 : MonoBehaviour
{
    
    int givenId = 0; // get from json

    public Texture2D BaseImg = null;
    public List<SpriteObj> spriteObjList = new List<SpriteObj>();
    public ArrayList TileInfos = new ArrayList();
    public List<SpriteInfo> SpriteInfos = new List<ObjJSON.SpriteInfo>();
    EdgeGraphData _edgeGraphData = new EdgeGraphData();
    Dictionary<int,float[]> color_id = new Dictionary<int, float[]>();

    string pathSave;
    string baseImagePath;
    string jsonSavePathMapInfo;
    string jsonSavePathColorId;
    string jsonSavePathSpritsInfo;
    string jsonSavePathTileInfo;
    
    //Menu choices
    bool autoNeighbore = true;
    bool topAndBottomNeighbore = false;
    bool leftAndRightNeighbore = false;
    bool keepExistingProvinceData = MainMenuControler.keepExistingProvinceDataChoice;

    //keepExistingProvince
    CombinedJSON spriteData;
    


    List<Tile> test;



    private void Awake()
    {

        pathSave = FilePath.ProvincesSplit;
        baseImagePath = FilePath.ProvinceMapImg;
        jsonSavePathMapInfo = FilePath.MapInfo;
        jsonSavePathColorId = FilePath.ColorId;
        jsonSavePathSpritsInfo = FilePath.SpritesInfos;
        jsonSavePathTileInfo = FilePath.TilesInfos;
        Directory.CreateDirectory(pathSave);

        if (MainMenuControler.recalculateMapChoice 
            || !File.Exists(jsonSavePathMapInfo) 
            || FileUtils.IsFolderEmpty(pathSave)
            || !File.Exists(jsonSavePathSpritsInfo)
            || !File.Exists(jsonSavePathColorId)
            || !File.Exists(jsonSavePathTileInfo)
            )
        {
            BaseImg = FileUtils.LoadBaseImage(baseImagePath);

            if (BaseImg == null)
            {
                Debug.LogError("Base image not found or failed to load.");
                return;
            }


            FileUtils.DeleteOldSpriteFiles(pathSave);
            GenerateDataFromImage();
            CreateColorIdList();
            SaveSprites(spriteObjList);
            if (autoNeighbore is true)
            {
                Debug.Log("Auto neighbore called");
                AutoNeighbore();

                if (leftAndRightNeighbore is true)
                {
                    Debug.Log("cc  LR !");
                    SetRightAndLeftOfImageAsNeighbore();
                }
                
                if (topAndBottomNeighbore)
                {
                    Debug.Log("cc  TB !");
                    SetRightAndLeftOfImageAsNeighbore();
                }
                
            }
            //_edgeGraphData.CalculateEdge();
            CreateJSON();
          
        }
        else
        {
            Debug.Log("The map has NOT been recalculated.");
        }

        
    }



    private void GenerateDataFromImage()
    {

        //read the whole image
        for (int x = 0; x < BaseImg.width; x++)
        {
            for (int y = 0; y < BaseImg.height; y++)
            {
                Color pixelColor = BaseImg.GetPixel(x, y);

                var _sprite = spriteObjList.Where(s => s.spriteColor == pixelColor).FirstOrDefault();

                if (_sprite is not null)
                {
                    Vector2Int pxCoord = new Vector2Int(x, y);
                    _sprite.spritePixels.Add(pxCoord);

                    _sprite.lowerX = Mathf.Min(_sprite.lowerX, x);
                    _sprite.higherX = Mathf.Max(_sprite.higherX, x);
                    _sprite.lowerY = Mathf.Min(_sprite.lowerY, y);
                    _sprite.higherY = Mathf.Max(_sprite.higherY, y);
                }
                if (_sprite is null)
                {
                    Vector2Int pxCoord = new Vector2Int(x, y);
                    SpriteObj newSpriteObj = GenerateSpriteObj(pixelColor, pxCoord);
                    spriteObjList.Add(newSpriteObj);
                }
            }
        }
    }
    private SpriteObj GenerateSpriteObj(Color pixelColor, Vector2Int pixelCoord )
    {
        SpriteObj newSprite = new SpriteObj(pixelColor, pixelCoord);
        newSprite.SetId(GenerateID());
        return newSprite;
    }

    private int GenerateID()
    {
        return givenId++;
    }

    private void SaveSprites(List<SpriteObj> spriteList)
    {
        foreach (var sprite in spriteList)
        {
            int width = sprite.higherX - sprite.lowerX + 1;
            int height = sprite.higherY - sprite.lowerY + 1;

            Texture2D tex = new Texture2D(width, height);
            Color[] colorArray = new Color[width * height];

            foreach (var pixel in sprite.spritePixels)
            {
                int x = pixel.x - sprite.lowerX;
                int y = pixel.y - sprite.lowerY;
                colorArray[x + y * width] = sprite.spriteColor;
            }

            tex.SetPixels(colorArray);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            if (bytes != null)
            {
                string filePath = Path.Combine(pathSave, $"img_{sprite.id}.png");
                File.WriteAllBytes(filePath, bytes);
            }   
        }
    }

    private void AutoNeighbore()
    {
        //read image top bottome 
        Color lastPixel = BaseImg.GetPixel(0, 0);
        for (int x = 0; x < BaseImg.width; x++)
        {
            lastPixel = BaseImg.GetPixel(x, 0);
            for (int y = 0; y < BaseImg.height; y++)
            {
                Color pixelColor = BaseImg.GetPixel(x, y);
                //when pixel color change
                if (pixelColor != lastPixel)
                {
                    // get province id of both color
                    int lastProvinceId = GetIdByColor(lastPixel);
                    int curentProvinceId = GetIdByColor(pixelColor);
                    SetNeighbore(lastProvinceId, curentProvinceId);
                }
                lastPixel = pixelColor;
            }
            
        }
        //read image top bottome left to right
        for (int y = 0; y < BaseImg.width; y++)
        {
            lastPixel = BaseImg.GetPixel(0, y);
            for (int x = 0; x < BaseImg.height; x++)
            {
                Color pixelColor = BaseImg.GetPixel(x, y);
                //when pixel color change
                if (pixelColor != lastPixel)
                {
                    // get province id of both color
                    int lastProvinceId = GetIdByColor(lastPixel);
                    int curentProvinceId = GetIdByColor(pixelColor);
                    SetNeighbore(lastProvinceId, curentProvinceId);
                }
                lastPixel = pixelColor;
            }
        }
    }

    private void CreateColorIdList()
    {
        foreach (var _tile in spriteObjList)
        {
            color_id.Add(_tile.id, ColorUtils.GenerateColorFormat(_tile.spriteColor));
        }
    }

    private void LoadJSON()
    {

        //read old json 
        string jsonPath = FilePath.MapInfo;
        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            spriteData = JsonConvert.DeserializeObject<CombinedJSON>(jsonText);
        }
        else
        {
            Debug.LogError("map_info.json not found at " + jsonPath);
            File.WriteAllText(jsonSavePathMapInfo, "");
        }
    }


    private void CreateJSON()
    {
        List<ObjJSON> listObjJSON = new List<ObjJSON>();

        for (int i = 0; i < spriteObjList.Count; i++)
        {
            AddTileInfos(spriteObjList[i].id, ColorUtils.GenerateColorFormat(spriteObjList[i].spriteColor), spriteObjList[i].spritePixels.Count, spriteObjList[i].neighboreId);

            AddSpriteInfos(ColorUtils.GenerateColorFormat(spriteObjList[i].spriteColor), spriteObjList[i].lowerX, spriteObjList[i].higherY, spriteObjList[i].higherX, spriteObjList[i].lowerX, spriteObjList[i].higherY, spriteObjList[i].lowerY);
        }

       
        ExportJson(listObjJSON, color_id, TileInfos, SpriteInfos);
    }
    private void AddTileInfos(int id,float[] spriteColor, int superficy, List<int>?neighbore)
    {
        if (spriteColor[2] == 1f && spriteColor[0] < 0.4999)
        {
            ObjJSON.WaterTile  tile = new ObjJSON.WaterTile(id,spriteColor, superficy);
            if(neighbore is not null)
            {
                neighbore.Sort();
                tile.neighbors = neighbore;
            }          
            TileInfos.Add(tile);
        }
        else
        {
            ObjJSON.LandTile tile = new ObjJSON.LandTile(id, spriteColor,superficy);
            neighbore.Sort();
            tile.neighbors = neighbore;
            TileInfos.Add(tile);
        }
    }

    private void AddSpriteInfos(float[] color, int x, int y, int higherX, int lowerX, int higherY, int lowerY)
    {
        var _center = new float[]
        {
                (higherX - lowerX)/2f,
                (higherY - lowerY)/2f
        };

        ObjJSON.SpriteInfo spriteInfo = new ObjJSON.SpriteInfo (color,x,y,_center);
        SpriteInfos.Add(spriteInfo);
    }


    private void ExportJson(List<ObjJSON> listObjJSON, Dictionary<int, float[]> idColor, ArrayList TileInfos, List<SpriteInfo> SpriteInfos)
    {
        
        string output = JsonConvert.SerializeObject(TileInfos, Formatting.Indented);
        File.WriteAllText(jsonSavePathTileInfo, output);

        output = JsonConvert.SerializeObject(SpriteInfos, Formatting.Indented);
        File.WriteAllText(jsonSavePathSpritsInfo, output);

        CombinedJSON combinedData = new CombinedJSON(BaseImg.width, BaseImg.height, listObjJSON);
        output = JsonConvert.SerializeObject(combinedData, Formatting.Indented);
        File.WriteAllText(jsonSavePathMapInfo, output);

        output = JsonConvert.SerializeObject(idColor, Formatting.Indented);
        File.WriteAllText(jsonSavePathColorId, output);
    }


    List<int> RemoveDuplicate(List<int> givenList)
    {
        return givenList.Distinct().OrderBy(n => n).ToList();
    }

    private int GetIdByColor(Color _color)
    {
        var tile = color_id.Where(c => c.Value.SequenceEqual(ColorUtils.GenerateColorFormat(_color))).FirstOrDefault();
        return tile.Key;
    }

    private void SetNeighbore(int id1, int id2) 
    {
        //spriteObjList
        SpriteObj Tile1 = spriteObjList.Where( s => s.id == id1).FirstOrDefault();
        SpriteObj Tile2 = spriteObjList.Where(s => s.id == id2).FirstOrDefault();
        
        if(Tile1 is not null && Tile2 is not null && Tile1 != Tile2 )
        {
            if (!Tile1.neighboreId.Contains(id2))
            {
                Tile1.neighboreId.Add(id2);
            }
            if (!Tile2.neighboreId.Contains(id1))
            {
                Tile2.neighboreId.Add(id1);
            }
        }
    }


    //neighbore top and bottom
    private void SetTopBottomOfIamgeAsNeighbore()
    {
        for (int x = 0; x < BaseImg.width; x++)
        {
            Color TopPixelColor = BaseImg.GetPixel(x, BaseImg.height);
            Color BottomPixelColor = BaseImg.GetPixel(x, 0);

            int TopPixelId = GetIdByColor(TopPixelColor);
            int BootomPixelId = GetIdByColor(BottomPixelColor);

            SetNeighbore(TopPixelId, BootomPixelId);
                
        }
    }
    //neighbore right and left
    private void SetRightAndLeftOfImageAsNeighbore()
    {
        for (int y = 0; y < BaseImg.height; y++)
        {
            Color leftPixelColor = BaseImg.GetPixel(0, y);
            Color rightPixelColor = BaseImg.GetPixel(BaseImg.width, y);

            int leftPixelId = GetIdByColor(leftPixelColor);
            int rightpixelId = GetIdByColor(rightPixelColor);

            SetNeighbore(leftPixelId, rightpixelId);

        }
    }
}
 