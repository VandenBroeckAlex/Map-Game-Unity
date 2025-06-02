using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using MyGame.Data;
using System;
using UnityEngine.U2D;

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

    bool autoNeighbore = true;
    bool TopAndBottomNeighbore = false;
    bool LeftAndRightNeighbore = true;

    private void Awake()
    {
        pathSave = FilePath.ProvincesSplit;
        baseImagePath = FilePath.ProvinceMapImg;
        jsonSavePathMapInfo = FilePath.MapInfo;
        jsonSavePathColorId = FilePath.ColorId;
        Directory.CreateDirectory(pathSave);

        if (MainMenuControler.recalculateMapChoice || !File.Exists(jsonSavePathMapInfo) || IsFolderEmpty(pathSave))
        {
            LoadBaseImage();

            if (BaseImg == null)
            {
                Debug.LogError("Base image not found or failed to load.");
                return;
            }

            DeleteOldSpriteFiles();
            GenerateMap();
            SaveSprites(spriteObjList);
            CreateJSON();
        }
        else
        {
            Debug.Log("The map has not been recalculated.");
        }
    }

    void LoadBaseImage()
    {
        if (File.Exists(baseImagePath))
        {
            byte[] imageData = File.ReadAllBytes(baseImagePath);
            BaseImg = new Texture2D(2, 2);
            BaseImg.LoadImage(imageData);
        }
    }

    void DeleteOldSpriteFiles()
    {
        if (Directory.Exists(pathSave))
        {
            foreach (string filePath in Directory.GetFiles(pathSave))
            {
                File.Delete(filePath);
            }
        }
    }

    private void GenerateMap()
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
                            float[] lastColor = GenerateColorFormat(lastPxColor);
                            if(autoNeighbore == true)
                            {
                                if (y == 0 && TopAndBottomNeighbore == true)
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
                    SpriteObj newSpriteObj = GenerateSpriteObj(pixelColor, pxCoord, GenerateColorFormat(lastPxColor));
                    spriteObjList.Add(newSpriteObj);
                }
                lastPxColor = pixelColor;
            }
        }

        if(LeftAndRightNeighbore == true)
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
                        if (!spriteObjList[x].neighboreColor.Any(c => c.SequenceEqual(GenerateColorFormat(rightPixelColor))))
                        {
                            spriteObjList[x].neighboreColor.Add(GenerateColorFormat(rightPixelColor));
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

    private float[] GenerateColorFormat(Color col)
    {
        return new float[] { col.r, col.g, col.b };
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
                GenerateColorFormat(sprite.spriteColor),
                new Vector2Int(sprite.lowerX, sprite.higherY),
                sprite.id,
                sprite.neighboreColor,
                new float[] { ((sprite.higherX - sprite.lowerX)/2) + sprite.lowerX, ((sprite.higherY - sprite.lowerY)/2) + sprite.lowerY } // ((higher - lower)/2) + lower
            );

            ListObjJSONTemp.Add(jsonObj);
        }
    }

   
    private bool IsFolderEmpty(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            return Directory.GetFiles(folderPath).Length == 0 &&
                   Directory.GetDirectories(folderPath).Length == 0;
        }
        return true;
    }

    // --- Data Classes ---

    public class SpriteObj
    {
        public Color spriteColor;
        public List<Vector2Int> spritePixels = new List<Vector2Int>();
        public int higherX, higherY, lowerX, lowerY;
        public List<float[]> neighboreColor = new List<float[]>();
        public int id;

        public SpriteObj(Color color, Vector2Int pixelCoord, float[] neighbor)
        {
            spriteColor = color;
            spritePixels.Add(pixelCoord);
            higherX = lowerX = pixelCoord.x;
            higherY = lowerY = pixelCoord.y;
            neighboreColor.Add(neighbor);
        }

        public void SetId(int id) => this.id = id;
    }

    public class ObjJSONTemp
    {
        public int id;
        public float[] spriteColor;
        public int lowerX;
        public int higherY;
        public float[] center;
        public string name = "";
        public string description = "";
        public int Type = 0;
        public int owner = 0;
        public List<float[]> neighbors;

        public ObjJSONTemp(float[] color, Vector2Int coord, int id, List<float[]> neighbors, float[] center)
        {
            this.id = id;
            this.spriteColor = color;
            this.lowerX = coord.x;
            this.higherY = coord.y;
            this.neighbors = neighbors;
            this.center = center;
        }
    }


    public class ObjJSON
    {
        public int id;
        public float[] spriteColor;
        public int lowerX;
        public int higherY;
        public float[] center;
        public string name = "";
        public string description = "";
        public int Type = 0;
        public int owner = 0;
        public List<int> neighbors;



        public ObjJSON(float[] color, int x, int y, int id, List<int> neighbors, float[] center)
        {
            this.id = id;
            this.spriteColor = color;
            this.lowerX = x;
            this.higherY = y;
            this.neighbors = neighbors;
            this.center = center;
        }
    }

    public class CombinedJSON
    {
        public int canvaWidth;
        public int canvaHeight;
        
        public List <ObjJSON> spriteListJSON;



        public CombinedJSON(int width, int height, List<ObjJSON> list)
        {
            canvaWidth = width;
            canvaHeight = height;
            spriteListJSON = list;
        }
    }

    private void CreateJSON()
    {
        List<ObjJSON> ListObjJSON = new List<ObjJSON>();
        Dictionary<int, float[]> idColor = new Dictionary<int, float[]>();

        for (int i = 0; i < ListObjJSONTemp.Count; i++)
        {
            List<int> neighboreID = new List<int>();
            
            idColor[ListObjJSONTemp[i].id] = ListObjJSONTemp[i].spriteColor;

            // convert neighbore color to id
            if(autoNeighbore == true)
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
            ObjJSON obj = new ObjJSON(ListObjJSONTemp[i].spriteColor, ListObjJSONTemp[i].lowerX, ListObjJSONTemp[i].higherY, ListObjJSONTemp[i].id, neighboreID, ListObjJSONTemp[i].center);
            ListObjJSON.Add(obj);
        }

        // check in other province if this province id appear
        //Because the image is read from bottom to top, the top province will never encounter the bottom province
        // this is a horrible way to do it :(
        //maybe merge this loop with the top one
        if (autoNeighbore == true)
        {
            for (int i = 0; i < ListObjJSON.Count; i++)
            {
                for (int x = 0; x < ListObjJSON.Count; x++)
                {
                    for (int y = 0; y < ListObjJSON[x].neighbors.Count; y++)
                    {
                        if (ListObjJSON[x].neighbors[y] == ListObjJSON[i].id)
                        {
                            ListObjJSON[i].neighbors.Add(ListObjJSON[x].id);
                        }
                    }
                }
                //check if neighbore id does not repeat
                ListObjJSON[i].neighbors = RemoveDuplicate(ListObjJSON[i].neighbors);
            }
        }
        
        CombinedJSON combinedData = new CombinedJSON(BaseImg.width, BaseImg.height, ListObjJSON);
        string output = JsonConvert.SerializeObject(combinedData, Formatting.Indented);
        File.WriteAllText(jsonSavePathMapInfo, output);

        output = JsonConvert.SerializeObject(idColor, Formatting.Indented);
        File.WriteAllText(jsonSavePathColorId, output);
    }
    List<int> RemoveDuplicate(List<int> givenList)
    {
        return givenList.Distinct().OrderBy(n => n).ToList();
    }
}
