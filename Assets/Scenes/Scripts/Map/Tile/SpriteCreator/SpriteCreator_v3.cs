
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEditor;
using Newtonsoft.Json;




// Vector2 cause problem when serialize to JSON


public class SpriteCreator_v3 : MonoBehaviour
{
    string pathSave = Application.dataPath;
    int givenId = 0;
    public class SpriteObj
    {
        public Color spriteColor;
        public List<Vector2Int> spritePixels;
        public int higherX;
        public int higherY;
        public int lowerX;
        public int lowerY;
        public int id;

        public SpriteObj(Color SPRITECOLOR, Vector2Int pixelCoord)
        {
            spriteColor = SPRITECOLOR;
            spritePixels = new List<Vector2Int>();
            spritePixels.Add(pixelCoord);
            higherX = pixelCoord.x;
            higherY = pixelCoord.y;
            lowerX = pixelCoord.x;
            lowerY = pixelCoord.y;
        }
        public void SetId(int GivenId)
        {
            id = GivenId;
        }
    }

    public class SpriteObjJSON
    {
        public int id;
        public float[] spriteColor;
        public int lowerX;
        public int higherY;
        public string name;
        public string description;
        public int Type;
        public int owner;
        public int[] neighbors;



        public SpriteObjJSON(float[] SPRITECOLOR, Vector2Int pixelCoord, int givenId)
        {
            id = givenId;
            spriteColor = SPRITECOLOR;
            lowerX = pixelCoord.x;
            higherY = pixelCoord.y;
        }
    }
    public List<SpriteObj> spriteList = new List<SpriteObj>();
    public List<SpriteObjJSON> spriteListJSON = new List<SpriteObjJSON>();
    public Texture2D BaseImg = null;
    public class CombinedJSON
    {
        public int canvaWidth;
        public int canvaHeight;
        public List<SpriteObjJSON> spriteListJSON;
        public CombinedJSON(int canvaWidth, int canvaHeight, List<SpriteObjJSON> spriteListJSON)
        {
            this.canvaWidth = canvaWidth;
            this.canvaHeight = canvaHeight;
            this.spriteListJSON = spriteListJSON;
        }
    }

    string folderPath = "Assets/Resources/provinces_split";

    //the image can not contain pure black !
    private void Awake()
    {
        BaseImg = Resources.Load<Texture2D>("Province_Map"); //aller la rechercher automatiquement gr�ce � son nom et la set comme readeable

        pathSave += "/Resources/provinces_split";
        TextAsset mapPositionData = Resources.Load<TextAsset>("map_info");


        //MainMenuControler.recalculateMapChoice == true || mapPositionData == null || IsFolderEmpty("Assets/Resources/provinces_split") == true

        if (MainMenuControler.recalculateMapChoice == true || mapPositionData == null || IsFolderEmpty("Assets/Resources/provinces_split") == true)
        {

            DeleteOldSprite();

            GenerateMapSprite();

            SaveSprites(spriteList);

            ChangeImageTypes();

            CreateJSON();

            AssetDatabase.Refresh();

        }
        else
        {
            Debug.Log("the map have not been recalculated");
        }
    }

    //Delete all sprite in the file
    private void DeleteOldSprite()
    {




        // Check if the directory exists
        if (Directory.Exists(folderPath))
        {

            string[] filePaths = Directory.GetFiles(folderPath);

            // Loop through each file and delete it
            foreach (string filePath in filePaths)
            {

                if (filePath.StartsWith("Assets"))
                {
                    AssetDatabase.DeleteAsset(filePath);
                }
            }

            // Refresh the AssetDatabase to reflect changes
            AssetDatabase.Refresh();

            Debug.Log("All files in the folder have been deleted.");
        }
        else
        {
            Debug.LogError("The folder does not exist.");
        }
    }

    private void GenerateMapSprite()
    {

        Color lastPxColor = Color.black;


        Debug.Log(BaseImg);
        //loop on each px
        for (int x = 0; x < BaseImg.width; x++)
        {
            for (int y = 0; y < BaseImg.height; y++)
            {


                Color pixelColor = BaseImg.GetPixel(x, y);
                bool ColorHaveBeenFound = false;


                // check if obj color already exist
                for (int j = 0; j < spriteList.Count; j++)
                {

                    if (spriteList[j].spriteColor == pixelColor)
                    {

                        Vector2Int PxCoord = new Vector2Int(x, y);
                        spriteList[j].spritePixels.Add(PxCoord);

                        // check for biggest and lowest X n Y
                        if (x < spriteList[j].lowerX)
                        {
                            spriteList[j].lowerX = x;
                        }

                        if (x > spriteList[j].higherX)
                        {
                            spriteList[j].higherX = x;
                        }

                        if (y < spriteList[j].lowerY)
                        {
                            spriteList[j].lowerY = y;
                        }

                        if (y > spriteList[j].higherY)
                        {
                            spriteList[j].higherY = y;
                        }


                        ColorHaveBeenFound = true;
                        continue;
                    }
                }

                //no ?  create it
                if (ColorHaveBeenFound == false)
                {
                    Vector2Int pxCoord = new Vector2Int(x, y);
                    SpriteObj newSpriteObj = GenerateSpriteObj(pixelColor, pxCoord);
                    spriteList.Add(newSpriteObj);
                }




            }
        }



        //loop to save all sprite("sprite_01", sprite);
        //SaveSprites(spriteList);
    }

    public float[] GenerateColorFormat(Color col)
    {
        float[] color = { col.r, col.g, col.b };


        return color;
    }

    private int GenerateID()
    {

        int id = givenId;
        givenId++;
        return (id);
    }

    // sprite object contain list of pixel to be set and value for the size of the sprite
    private SpriteObj GenerateSpriteObj(Color pixelColor, Vector2Int pixelCoord)
    {
        SpriteObj newSprite = new SpriteObj(pixelColor, pixelCoord);
        int id = GenerateID();
        newSprite.SetId(id);
        return (newSprite);
    }





    // save all sprite 
    private void SaveSprites(List<SpriteObj> spriteList)
    {
        byte[] bytes;

        for (int i = 0; i < spriteList.Count; i++)
        {
            int sizeX = spriteList[i].higherX - spriteList[i].lowerX + 1;
            int sizeY = spriteList[i].higherY - spriteList[i].lowerY + 1;


            // add sprite obj to JSON
            Color col = spriteList[i].spriteColor;
            SpriteObjJSON JsonObj = new SpriteObjJSON(GenerateColorFormat(spriteList[i].spriteColor), new Vector2Int(spriteList[i].lowerX, spriteList[i].higherY), spriteList[i].id);
            spriteListJSON.Add(JsonObj);

            Texture2D tex = new Texture2D(sizeX, sizeY);
            Color[] colorArray = new Color[sizeX * sizeY];

            // Resize pixels for new texture size
            for (int j = 0; j < spriteList[i].spritePixels.Count; j++)
            {
                int x = spriteList[i].spritePixels[j].x - spriteList[i].lowerX;
                int y = spriteList[i].spritePixels[j].y - spriteList[i].lowerY;
                int index = x + y * sizeX;

                colorArray[index] = Color.cyan;
            }

            tex.SetPixels(colorArray);
            bytes = tex.EncodeToPNG();

            if (bytes != null)
            {
                File.WriteAllBytes(pathSave + "/img_" + i + ".png", bytes);
            }
        }
        AssetDatabase.Refresh();
    }

    //create JSON holding sprite color and positions
    private void CreateJSON()
    {
        // Create a dictionary with width and height as separate integers
        Dictionary<string, int[]> canvaSize = new Dictionary<string, int[]>
        {
            { "canvaSize", new int[] { BaseImg.width, BaseImg.height } }
        };


        int canvaWidth = BaseImg.width;
        int canvaHeight = BaseImg.height;

        CombinedJSON combinedData = new CombinedJSON(canvaWidth, canvaHeight, spriteListJSON);

        string output = JsonConvert.SerializeObject(combinedData, Formatting.Indented);
        File.WriteAllText(Application.dataPath + "/Resources/map_info.json", output);


    }

    // change texture type from Default to Sprite
    //change sprite mode from multiple to single
    private void ChangeImageTypes()
    {


        string[] texturePaths = AssetDatabase.FindAssets("t:texture2D", new[] { folderPath });

        Debug.Log("the length is" + texturePaths.Length);

        foreach (string texturePathGUID in texturePaths)
        {
            string texturePath = AssetDatabase.GUIDToAssetPath(texturePathGUID);

            // Get the texture importer for the texture asset
            TextureImporter textureImporter = AssetImporter.GetAtPath(texturePath) as TextureImporter;

            if (textureImporter != null)
            {
                // Check if the texture is already of type Sprite and adjust settings
                if (textureImporter.textureType != TextureImporterType.Sprite)
                {
                    // Change texture type to Sprite
                    textureImporter.textureType = TextureImporterType.Sprite;
                }

                // Change the sprite mode to Single
                textureImporter.spriteImportMode = SpriteImportMode.Single;
                //textureImporter.filterMode = FilterMode.Point;

                // Re-import the texture to apply changes
                AssetDatabase.ImportAsset(texturePath, ImportAssetOptions.ForceUpdate);
                Debug.Log("Changed settings for texture: " + texturePath);
            }
            else
            {
                Debug.LogWarning("Skipping non-sprite texture: " + texturePath);
            }
        }

        // Refresh the AssetDatabase to reflect changes
        AssetDatabase.Refresh();
    }

    bool IsFolderEmpty(string folderPath)
    {
        // Check if directory exists
        if (Directory.Exists(folderPath))
        {
            // Get all files and subdirectories inside the folder
            string[] files = Directory.GetFiles(folderPath);
            string[] subdirectories = Directory.GetDirectories(folderPath);

            // If both files and subdirectories are empty, the folder is empty
            return files.Length == 0 && subdirectories.Length == 0;
        }
        return false;
    }
}