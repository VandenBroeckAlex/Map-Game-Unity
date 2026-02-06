using MyGame.Data;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using MyGame.Data;
using static ObjJSON;


public class SpriteCreator_v5 : MonoBehaviour
{
    
    int givenId = 0; // get from json

    public Texture2D BaseImg = null;
    public List<SpriteObj> spriteObjList = new List<SpriteObj>();
    public List<MyGame.Data.Tile> tileInfos = new List<Tile>();
    public List<SpriteInfo> spriteInfos = new List<ObjJSON.SpriteInfo>();
    List<EdgeGraphData.EdgeObj> _edgeData = new List<EdgeGraphData.EdgeObj>();
    Dictionary<int,float[]> color_id = new Dictionary<int, float[]>();



    string pathSave;
    string baseImagePath;
    string jsonSavePathMapInfo;
    string jsonSavePathColorId;
    string jsonSavePathSpritsInfo;
    string jsonSavePathTileInfo;
    string jsonSavePathEdgeInfo;

    //Menu choices
    bool autoNeighbore = true;
    bool topAndBottomNeighbore = false;
    bool leftAndRightNeighbore = false;
    bool keepExistingProvinceData = false;//MainMenuControler.keepExistingProvinceDataChoice;
    bool keepNeighbore = false;
    bool keepsuperficy = false;
    bool keepCenter = false;

    //keepExistingProvince

    
  



    public void CreateSprite()
    {

        pathSave = FilePath.ProvincesSplit;
        baseImagePath = FilePath.ProvinceMapImg;
        jsonSavePathMapInfo = FilePath.MapInfo;
        jsonSavePathColorId = FilePath.ColorId;
        jsonSavePathSpritsInfo = FilePath.SpritesInfos;
        jsonSavePathTileInfo = FilePath.TilesInfos;
        jsonSavePathEdgeInfo = FilePath.MapEdge;
        Directory.CreateDirectory(pathSave);

       
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
                    SetRightAndLeftOfImageAsNeighbore();
                }
                
                if (topAndBottomNeighbore)
                {
                    SetRightAndLeftOfImageAsNeighbore();
                }
            }

            for (int i = 0; i < spriteObjList.Count; i++)
            {
                AddTileInfos(spriteObjList[i].id, ColorUtils.GenerateColorFormat(spriteObjList[i].spriteColor), spriteObjList[i].spritePixels.Count, spriteObjList[i].neighboreId);

                AddSpriteInfos(ColorUtils.GenerateColorFormat(spriteObjList[i].spriteColor), spriteObjList[i].lowerX, spriteObjList[i].higherY, spriteObjList[i].higherX, spriteObjList[i].lowerX, spriteObjList[i].higherY, spriteObjList[i].lowerY, spriteObjList[i].id);
            }


             

            // keep existing data
            // load json
            // TODO check json validity

            // check that 2 province do  not have the same color
            if (keepExistingProvinceData)
            {
                if (!File.Exists(jsonSavePathTileInfo))
                {
                    Debug.LogError("tilesInfos.json not found at " + jsonSavePathTileInfo);
                    File.WriteAllText(jsonSavePathMapInfo, "");
                    keepExistingProvinceData = false;
                }


                List<Tile> existingProvinceData = LoadJSON(jsonSavePathTileInfo);

                if (existingProvinceData.Count >= 0) 
                {
                    Debug.LogError("No tiles infos in file");
                    keepExistingProvinceData = false;
                }

                for (int i = existingProvinceData.Count - 1; i >= 0; i--)
                {
                    var existingTile = existingProvinceData[i];
                    var imgTile = tileInfos.Where(_tile => _tile.spriteColor.SequenceEqual(existingTile.spriteColor)).FirstOrDefault();

                    if (imgTile is not null)
                    {
                        tileInfos.Remove(imgTile);
                    }
                    else
                    {                      
                        existingProvinceData.RemoveAt(i);
                    }
                }
                tileInfos = existingProvinceData.Concat( tileInfos).ToList();
            }


            CreateNeighboreEdge();

            AutoCoastalTile();

            ExportJson(color_id, tileInfos, spriteInfos,_edgeData);
        
        

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
    private List<Tile> LoadJSON(string path)
    {
            string jsonText = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<List<Tile>>(jsonText);
    }
    private void AddTileInfos(int id,float[] spriteColor, int superficy, List<int>?neighbore)
    {
        if (spriteColor[2] == 1f && spriteColor[0] < 0.4999)
        {
            MyGame.Data.WaterTile  tile = new MyGame.Data.WaterTile(id);
            
            if(neighbore is not null)
            {
                neighbore.Sort();
                tile.neighbors = neighbore;
                tile.spriteColor = spriteColor;
            }          
            tileInfos.Add(tile);
        }
        else
        {
            MyGame.Data.LandTile tile = new MyGame.Data.LandTile(id);
            neighbore.Sort();
            tile.neighbors = neighbore;
            tile.spriteColor = spriteColor;
            tileInfos.Add(tile);
        }
    }
    private void AddSpriteInfos(float[] color, int x, int y, int higherX, int lowerX, int higherY, int lowerY,int id)
    {
        var _center = new float[]
        {
                (higherX - lowerX)/2f,
                (higherY - lowerY)/2f
        };

        ObjJSON.SpriteInfo spriteInfo = new ObjJSON.SpriteInfo (color,x,y,_center,id);
        spriteInfos.Add(spriteInfo);
    }
    private void ExportJson( Dictionary<int, float[]> idColor, List<Tile> TileInfos, List<SpriteInfo> SpriteInfos, List<EdgeGraphData.EdgeObj> _edgeData)
    {
        var settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ContractResolver = new StrictOrderContractResolver()
        };

        string output = JsonConvert.SerializeObject(TileInfos, settings);
        File.WriteAllText(jsonSavePathTileInfo, output);

        output = JsonConvert.SerializeObject(SpriteInfos, Formatting.Indented);
        File.WriteAllText(jsonSavePathSpritsInfo, output);

        output = JsonConvert.SerializeObject(idColor, Formatting.Indented);
        File.WriteAllText(jsonSavePathColorId, output);

        output = JsonConvert.SerializeObject(_edgeData, Formatting.Indented);
        File.WriteAllText(jsonSavePathEdgeInfo, output);
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
    private void CreateNeighboreEdge()
    {
        List<int> processedIds =  new List<int>();
        //_edgeGraphData
        foreach (var tile in tileInfos)
        {         
            foreach (var neighboreId in tile.neighbors)
            {
                //if neighbore not in idSeen
                if (!processedIds.Contains(neighboreId))
                {
                    //get neighbore color
                    var neighboreColor = tileInfos.Where(t => t.id == neighboreId).Select(t => t.spriteColor).FirstOrDefault();
                    
                    
                    //get tile and neighbore center

                    var tileCenter = spriteInfos.Where(s => s.spriteColor.SequenceEqual(tile.spriteColor)).Select(s => s.center).FirstOrDefault();
                    if (tileCenter is null) 
                    {
                        Debug.Log("tileCenter is null");
                    }
                    //var neighboreCenter
                    var neighboreCenter = spriteInfos.Where(s => s.spriteColor.SequenceEqual(neighboreColor)).Select(s => s.center).FirstOrDefault();
                    if (neighboreCenter is null)
                    {
                        Debug.Log("neighboreCenter is null");
                    }

                    //create edge

                    EdgeGraphData.EdgeObj edge = new EdgeGraphData.EdgeObj();
                    edge.from = tile.id;
                    edge.to = neighboreId;
                    edge.baseDistance = (float)(Math.Pow((tileCenter[0] - neighboreCenter[0]), 2) + Math.Pow((tileCenter[1] - neighboreCenter[1]), 2));
                    _edgeData.Add(edge);
                }
            }
            //add tile id in processedIds
            processedIds.Add(tile.id);
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
    private void AutoCoastalTile() 
    {
        foreach (LandTile landTile in tileInfos.OfType<LandTile>())
        {
            foreach (int neighborId in landTile.neighbors)
            {
                Tile neighbor = tileInfos.FirstOrDefault(t => t.id == neighborId);
                if (neighbor is not null && !neighbor.isLand)
                {
                    landTile.isCoast = true;
                }
            }
        }
    }
}
 