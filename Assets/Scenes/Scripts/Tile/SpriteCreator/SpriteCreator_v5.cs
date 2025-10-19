using MyGame.Data;
using Newtonsoft.Json;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static ObjJSON;
using static UnityEditor.Rendering.CameraUI;
using static UnityEngine.Rendering.DebugUI;

public class SpriteCreator_v5 : MonoBehaviour
{
    
    int givenId = 0; // get from json

    public Texture2D BaseImg = null;
    public List<SpriteObj> spriteObjList = new List<SpriteObj>();
    public List<SpriteInfos> ListObjJSONTemp = new List<SpriteInfos>();
    public ArrayList TileInfos = new ArrayList();
    public List<SpriteInfo> SpriteInfos = new List<ObjJSON.SpriteInfo>();
    EdgeGraphData _edgeGraphData = new EdgeGraphData();

    
    string pathSave = FilePath.ProvincesSplit;
    string baseImagePath = FilePath.ProvinceMapImg;
    string jsonSavePathMapInfo = FilePath.MapInfo;
    string jsonSavePathColorId = FilePath.ColorId;
    string jsonSavePathSpritsInfo = FilePath.SpritesInfos;
    string jsonSavePathTileInfo = FilePath.TilesInfos;
    
    
    //Menu choices
    bool autoNeighbore = true;
    bool topAndBottomNeighbore = false;
    bool leftAndRightNeighbore = true;
    bool keepExistingProvinceData = MainMenuControler.keepExistingProvinceDataChoice;

    //keepExistingProvince
    CombinedJSON spriteData;
    Dictionary<float[], bool> existingProvinceColor = new Dictionary<float[], bool>();


    List<Tile> test;



    private void Awake()
    {
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
            //for id
            if (keepExistingProvinceData == true)
            {
                Debug.Log("loading json");
                LoadJSON();
                if(spriteData is null)
                {
                    keepExistingProvinceData = false;
                }
                givenId = spriteData.spriteListJSON.LastOrDefault().Id + 1;
            }

            FileUtils.DeleteOldSpriteFiles(pathSave);
            GenerateSprites();
            SaveSprites(spriteObjList);
            CreateJSON();
            _edgeGraphData.CalculateEdge();
        }
        else
        {
            Debug.Log("The map has not been recalculated.");
        }

        
    }



    private void GenerateSprites()
    {
        Color lastPxColor = Color.black;
        //read the whole image
        for (int x = 0; x < BaseImg.width; x++)
        {
            for (int y = 0; y < BaseImg.height; y++)
            {
                Color pixelColor = BaseImg.GetPixel(x, y);
                bool colorFoundInList = false;

                foreach (var sprite in spriteObjList)
                {
                    if (sprite.spriteColor == pixelColor)
                    {
                        Vector2Int pxCoord = new Vector2Int(x, y);
                        sprite.spritePixels.Add(pxCoord);

                        sprite.lowerX = Mathf.Min(sprite.lowerX, x);
                        sprite.higherX = Mathf.Max(sprite.higherX, x);
                        sprite.lowerY = Mathf.Min(sprite.lowerY, y);
                        sprite.higherY = Mathf.Max(sprite.higherY, y);

                        if (pixelColor != lastPxColor)
                        {
                            float[] lastColor = ColorUtils.GenerateColorFormat(lastPxColor);

                            if (autoNeighbore == true)
                            {
                                if (y == 0 && topAndBottomNeighbore == true)
                                {
                                    sprite.neighboreColor.Add(lastColor);
                                    continue;
                                }

                                if (y != 0)
                                {
                                    if (!sprite.neighboreColor.Any(c => c.SequenceEqual(lastColor)))
                                    {
                                        sprite.neighboreColor.Add(lastColor);
                                    }
                                }
                            }
                        }
                        colorFoundInList = true;
                        break;
                    }
                }

                if (!colorFoundInList)
                {
                    Vector2Int pxCoord = new Vector2Int(x, y);
                    SpriteObj newSpriteObj = GenerateSpriteObj(pixelColor, pxCoord, ColorUtils.GenerateColorFormat(lastPxColor));
                    spriteObjList.Add(newSpriteObj);
                }
                lastPxColor = pixelColor;
            }
        }

        if (leftAndRightNeighbore == true)
        {
            //  read base image width 0 and base image maxwidth
            //  color list of float
            //  check the id of the color in spriteObjList and maxwidth color in neighbore
            for (int y = 0; y < BaseImg.height; y++)
            {
                Color leftPixelColor = BaseImg.GetPixel(0, y);
                Color rightPixelColor = BaseImg.GetPixel(BaseImg.width - 1, y);

                // get leftPixelColorObj add right
                int _indexLeft = spriteObjList.FindIndex(s => s.spriteColor == leftPixelColor);
                int _indexRight = spriteObjList.FindIndex(s => s.spriteColor == rightPixelColor);

                if (spriteObjList[_indexLeft].neighboreColor.Contains(ColorUtils.GenerateColorFormat(spriteObjList[_indexRight].spriteColor)) || spriteObjList[_indexLeft].spriteColor == spriteObjList[_indexRight].spriteColor)
                {

                    continue;
                }

                spriteObjList[_indexLeft].neighboreColor.Add(ColorUtils.GenerateColorFormat(spriteObjList[_indexRight].spriteColor));
                spriteObjList[_indexRight].neighboreColor.Add(ColorUtils.GenerateColorFormat(spriteObjList[_indexLeft].spriteColor));

            }


        }
    }

    private SpriteObj GenerateSpriteObj(Color pixelColor, Vector2Int pixelCoord, float[] neighboreColor)
    {
        SpriteObj newSprite = new SpriteObj(pixelColor, pixelCoord, neighboreColor);
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
                colorArray[x + y * width] = Color.cyan;
            }

            tex.SetPixels(colorArray);
            tex.Apply();

            byte[] bytes = tex.EncodeToPNG();
            if (bytes != null)
            {
                string filePath = Path.Combine(pathSave, $"img_{sprite.id}.png");
                File.WriteAllBytes(filePath, bytes);
            }




            SpriteInfos jsonObj = new SpriteInfos(
                ColorUtils.GenerateColorFormat(sprite.spriteColor),
                new Vector2Int(sprite.lowerX, sprite.higherY),
                sprite.id,
                sprite.neighboreColor,
                new float[] { ((sprite.higherX - sprite.lowerX) / 2) + sprite.lowerX, ((sprite.higherY - sprite.lowerY) / 2) + sprite.lowerY }, // ((higher - lower)/2) + lower
                sprite.spritePixels.Count
                );

            ListObjJSONTemp.Add(jsonObj);
        }
    }


    // --- Data Classes ---


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

        //make a list of every province color < provincecolor -
        // this should be fix !

        //for (int i = 0; i < spriteData.spriteListJSON.Count; i++)
        //{

        //    existingProvinceColor.Add(spriteData.spriteListJSON[i].SpriteColor, false);
        //    if (spriteData.spriteListJSON[i].Id > givenId)
        //    {
        //        givenId = spriteData.spriteListJSON[i].Id;
        //    }

        //}

        //bool result;
        //bool found = existingProvinceColor.TryGetValue(new float[] { 0.227450982f, 0.227450982f, 0.9882353f }, out result);
        //Debug.Log("cc: " + found + ", value: " + result);


    }


    private void CreateJSON()
    {
        List<ObjJSON> listObjJSON = new List<ObjJSON>();

        Dictionary<int, float[]> idColor = new Dictionary<int, float[]>();

       


        // TODO AddTileInfos

        // TODO AddSpriteInfos

        for (int i = 0; i < ListObjJSONTemp.Count; i++)
        {
            List<int> neighboreID = new List<int>();
            idColor[ListObjJSONTemp[i].id] = ListObjJSONTemp[i].spriteColor;

            if (autoNeighbore == true)
            {
                for (int y = 0; y < ListObjJSONTemp[i].neighbors.Count; y++)
                {

                    for (int x = 0; x < ListObjJSONTemp.Count; x++)
                    {
                        if (Enumerable.SequenceEqual(ListObjJSONTemp[i].neighbors[y], ListObjJSONTemp[x].spriteColor))
                        {
                            neighboreID.Add(ListObjJSONTemp[x].id);
                            continue;
                        }
                    }
                }
            }

        }

        // TODO Remove province if their color doesn't appear

        // TODO check in other province if this province id appear
        SetLeftNeighbor(listObjJSON);
        ExportJson(listObjJSON, idColor);
    }
    private void AddTileInfos(int id,float[] spriteColor,List<int> neighbore, int superficy)
    {
        if (spriteColor[2] == 1f && spriteColor[0] < 0.4999)
        {
            ObjJSON.WaterTile  tile = new ObjJSON.WaterTile(id,spriteColor,neighbore, superficy);
            TileInfos.Add(tile);
        }
        else
        {
            ObjJSON.LandTile tile = new ObjJSON.LandTile(id, spriteColor, neighbore, superficy);
            TileInfos.Add(tile);
        }
    }

    private void AddSpriteInfos(float[] color, int x, int y, float[] center)
    {
        ObjJSON.SpriteInfo spriteInfo = new ObjJSON.SpriteInfo (color,x,y,center);
        SpriteInfos.Add(spriteInfo);
    }


    private void ExportJson(List<ObjJSON> listObjJSON, Dictionary<int, float[]> idColor)
    {
        CombinedJSON combinedData = new CombinedJSON(BaseImg.width, BaseImg.height, listObjJSON);
        string output = JsonConvert.SerializeObject(combinedData, Formatting.Indented);
        File.WriteAllText(jsonSavePathMapInfo, output);

        output = JsonConvert.SerializeObject(idColor, Formatting.Indented);
        File.WriteAllText(jsonSavePathColorId, output);
    }

    private void SetLeftNeighbor(List<ObjJSON> listObjJSON)
    {
        //Because the image is read from bottom to top, the top province will never encounter the bottom province       
        if (autoNeighbore == true)
        {
            for (int i = 0; i < listObjJSON.Count; i++)
            {
                // get all province that have listObjJSON[i].id as neighbore
                List<ObjJSON> neighbores = listObjJSON.Where(n => n.neighbors.Contains(listObjJSON[i].Id)).ToList();

                for (int x = 0; x < neighbores.Count; x++)
                {
                    if (!listObjJSON[i].neighbors.Contains(neighbores[x].Id) || neighbores[x].Id != listObjJSON[i].Id)
                    {
                        listObjJSON[i].neighbors.Add(neighbores[x].Id);
                    }
                }
                listObjJSON[i].neighbors = RemoveDuplicate(listObjJSON[i].neighbors);
            }
        }
    }

    List<int> RemoveDuplicate(List<int> givenList)
    {
        return givenList.Distinct().OrderBy(n => n).ToList();
    }

    string FindKeyByColor(Dictionary<string, float[]> jsonData, Color color)
    {
        foreach (var kvp in jsonData)
        {
            float[] stored = kvp.Value;

            // Compare exact color values (no tolerance)
            if (stored.Length >= 3 &&
                color.r == stored[0] &&
                color.g == stored[1] &&
                color.b == stored[2])
            {
                Debug.Log("Found key: " + kvp.Key);
                return kvp.Key; // Found a match
            }
        }

        Debug.Log("No matching color found.");
        return null; // Not found
    }

    bool ColorComparator(float[] color1, float[] color2)
    {

        for (int i = 0; i < color1.Length; i++)
        {
            if (Mathf.Approximately(color1[i], color2[i]))
            {

            }
            else
            {
                return false;
            }

        }
        return true;
    }
}
