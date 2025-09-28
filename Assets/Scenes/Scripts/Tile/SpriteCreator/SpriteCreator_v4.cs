using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using MyGame.Data;
using System;
using UnityEngine.Rendering;
using NUnit.Framework;
using static UnityEngine.Rendering.DebugUI;
using static UnityEditor.Rendering.CameraUI;

public class SpriteCreator_v4 : MonoBehaviour
{
    string pathSave;
    int givenId = 0;

    public Texture2D BaseImg = null;
    public List<SpriteObj> spriteObjList = new List<SpriteObj>();
    public List<ObjJSONTemp> ListObjJSONTemp = new List<ObjJSONTemp>();

    string baseImagePath;
    string jsonSavePathMapInfo;
    string jsonSavePathColorId;
    
    //Menu choices
    bool autoNeighbore = true;
    bool topAndBottomNeighbore = false;
    bool leftAndRightNeighbore = true;
    

    //keepExistingProvince
    CombinedJSON spriteData;
    Dictionary<float[], bool> existingProvinceColor = new Dictionary<float[], bool>();

    private void Awake()
    {
        pathSave = FilePath.ProvincesSplit;
        baseImagePath = FilePath.ProvinceMapImg;
        jsonSavePathMapInfo = FilePath.MapInfo;
        jsonSavePathColorId = FilePath.ColorId;
        Directory.CreateDirectory(pathSave);
        Debug.Log(MainMenuControler.keepExistingProvinceDataChoice);
        if (MainMenuControler.recalculateMapChoice || !File.Exists(jsonSavePathMapInfo) || FileUtils.IsFolderEmpty(pathSave))
        {
            BaseImg = FileUtils.LoadBaseImage(baseImagePath);

            if (BaseImg == null)
            {
                Debug.LogError("Base image not found or failed to load.");
                return;
            }
            if (MainMenuControler.keepExistingProvinceDataChoice == true)
            {
                Debug.Log("loading json");
                LoadJSON();
            }
            FileUtils.DeleteOldSpriteFiles(pathSave);
            GenerateSprites();
            SaveSprites(spriteObjList);
            CreateJSON();
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
                            float[] lastColor =  ColorUtils.GenerateColorFormat(lastPxColor);
                            if(autoNeighbore == true)
                            {
                                if (y == 0 && topAndBottomNeighbore == true)
                                {
                                    sprite.neighboreColor.Add(lastColor);
                                    continue;
                                }

                                if (y != 0 )
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

        if(leftAndRightNeighbore == true)
        {
            //  read base image width 0 and base image maxwidth
            //color list of float
            //  check the id of the color in spriteObjList and maxwidth color in neighbore
            for (int y = 0; y < BaseImg.height; y++)
            {
                Color leftPixelColor = BaseImg.GetPixel(0, y);
                Color rightPixelColor = BaseImg.GetPixel(BaseImg.width-1, y);

                // get leftPixelColorObj add right
                for (int x = 0; x < spriteObjList.Count; x++)
                {
                    if(leftPixelColor == spriteObjList[x].spriteColor)
                    {
                        if (!spriteObjList[x].neighboreColor.Any(c => c.SequenceEqual(ColorUtils.GenerateColorFormat(rightPixelColor))))
                        {
                            spriteObjList[x].neighboreColor.Add(ColorUtils.GenerateColorFormat(rightPixelColor));
                        }
                        break;
                    }

                }
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

            

           
            ObjJSONTemp jsonObj = new ObjJSONTemp(
                ColorUtils.GenerateColorFormat(sprite.spriteColor),
                new Vector2Int(sprite.lowerX, sprite.higherY),
                sprite.id,
                sprite.neighboreColor,
                new float[] { ((sprite.higherX - sprite.lowerX)/2) + sprite.lowerX, ((sprite.higherY - sprite.lowerY)/2) + sprite.lowerY }, // ((higher - lower)/2) + lower
                sprite.spritePixels.Count
                );

            ListObjJSONTemp.Add(jsonObj);
        }
    }

   
    // --- Data Classes ---


    private void LoadJSON()
    {
        
        
        //read old json and bool>
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
        if (4 < 1)
        {
            for (int i = 0; i < spriteData.spriteListJSON.Count; i++)
            {

                existingProvinceColor.Add(spriteData.spriteListJSON[i].spriteColor, false);
                if (spriteData.spriteListJSON[i].id > givenId)
                {
                    givenId = spriteData.spriteListJSON[i].id;
                }

            }
        }
        bool result;
        bool found = existingProvinceColor.TryGetValue(new float[] { 0.227450982f, 0.227450982f, 0.9882353f }, out result);
        Debug.Log("cc: " + found + ", value: " + result);


    }


    private void CreateJSON()
    {
        List<ObjJSON> listObjJSON = new List<ObjJSON>();
        Dictionary<int, float[]> idColor = new Dictionary<int, float[]>();    
        for (int i = 0; i < ListObjJSONTemp.Count; i++)
        {
            if(MainMenuControler.keepExistingProvinceDataChoice == true)
            {
                bool flag = false;
                foreach (KeyValuePair<float[], bool> kvp in existingProvinceColor.ToList())
                {
                    if(ColorComparator(kvp.Key, ListObjJSONTemp[i].spriteColor))
                    {
                        flag = true;
                        existingProvinceColor[kvp.Key] = true;
                    }
                }

                
                if(flag == false) //than color is not in previous json add province
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

                    //check if Blue is 255 and red less than 180
                    if (ListObjJSONTemp[i].spriteColor[2] == 1 && ListObjJSONTemp[i].spriteColor[0] < 0.4999)
                    {
                        ObjJSON obj = new ObjJSON(ListObjJSONTemp[i].spriteColor, ListObjJSONTemp[i].lowerX, ListObjJSONTemp[i].higherY, ListObjJSONTemp[i].id, neighboreID, ListObjJSONTemp[i].center, ListObjJSONTemp[i].superficy, false);
                        listObjJSON.Add(obj);
                    }
                    else
                    {
                        ObjJSON obj = new ObjJSON(ListObjJSONTemp[i].spriteColor, ListObjJSONTemp[i].lowerX, ListObjJSONTemp[i].higherY, ListObjJSONTemp[i].id, neighboreID, ListObjJSONTemp[i].center, ListObjJSONTemp[i].superficy,true);
                        listObjJSON.Add(obj);
                    }
                        

                }

            }
            if(MainMenuControler.keepExistingProvinceDataChoice == false)
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
                ObjJSON obj = new ObjJSON(ListObjJSONTemp[i].spriteColor, ListObjJSONTemp[i].lowerX, ListObjJSONTemp[i].higherY, ListObjJSONTemp[i].id, neighboreID, ListObjJSONTemp[i].center, ListObjJSONTemp[i].superficy,true);
                listObjJSON.Add(obj);
            }         
        }

        if (MainMenuControler.keepExistingProvinceDataChoice == true)
        {
            List<ObjJSON> existingProvinceList = spriteData.spriteListJSON;

            // Remove province if their color doesn't appear
            foreach (var provinceEntry in existingProvinceColor)
            {
                if (provinceEntry.Value == false)
                {
                    float[] targetColor = provinceEntry.Key;

                    existingProvinceList.RemoveAll(item =>
                        item.spriteColor.Length >= 3 &&
                        item.spriteColor[0] == targetColor[0] &&
                        item.spriteColor[1] == targetColor[1] &&
                        item.spriteColor[2] == targetColor[2]
                    );
                }
            }
        }

        // check in other province if this province id appear
        //Because the image is read from bottom to top, the top province will never encounter the bottom province
        // this is a horrible way to do it :(       
        if (autoNeighbore == true)
        {
            for (int i = 0; i < listObjJSON.Count; i++)
            {
                for (int x = 0; x < listObjJSON.Count; x++)
                {
                    for (int y = 0; y < listObjJSON[x].neighbors.Count; y++)
                    {
                        if (listObjJSON[x].neighbors[y] == listObjJSON[i].id)
                        {
                            listObjJSON[i].neighbors.Add(listObjJSON[x].id);
                        }
                    }
                }
                //check if neighbore id does not repeat
                listObjJSON[i].neighbors = RemoveDuplicate(listObjJSON[i].neighbors);
            }
        }
        
        if(MainMenuControler.keepExistingProvinceDataChoice == true)
        {
            listObjJSON.AddRange(spriteData.spriteListJSON);
        }


        CombinedJSON combinedData = new CombinedJSON(BaseImg.width, BaseImg.height, listObjJSON);
        string output = JsonConvert.SerializeObject(combinedData, Formatting.Indented);
        File.WriteAllText(jsonSavePathMapInfo, output);

        output = JsonConvert.SerializeObject(idColor, Formatting.Indented);
        File.WriteAllText(jsonSavePathColorId, output);
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
        
        for(int i = 0;i< color1.Length; i++)
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
