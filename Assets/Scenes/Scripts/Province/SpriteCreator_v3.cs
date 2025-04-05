
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Data;
using UnityEditor;





public class SpriteCreator_v3 : MonoBehaviour
{
    
    string pathSave = Application.dataPath;
    

    float[] colorToGet = { 52, 255, 68 }; // to divide by 255
    Color colorToGetFormated;
    public Texture2D BaseImg; //aller la rechercher automatiquement grâce à son nom et la set comme readeable
                    
   
    private List <SpriteObj> spriteList = new List<SpriteObj>();
    


    //the image can not contain black !

    // Start is called before the first frame update
    void Start()
    {

        //supprimer toutes les img présente dans le dossier
        pathSave += "/sprites_terrain/provinces_split";


        if(MainMenuControler.recalculateMapChoice == true)
        {

            //GenerateColorFormat(colorToGet);
            DeleteOldSprite();


            GenerateMapSprite();

            SaveSprites(spriteList);


        }
        else
        {
            Debug.Log("the map have not been recalculated");
        }        
    }


    private void DeleteOldSprite()
    {

        
        string folderPath = "Assets/sprites_terrain/provinces_split";  

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
        

        //loop on each px
        for (int x = 0; x < BaseImg.width; x++)
        {
            for (int y = 0; y < BaseImg.height; y++)
            {


                Color pixelColor = BaseImg.GetPixel(x, y);
                bool ColorHaveBeenFound = false;
       

                // check if obj color already exist
                for (int j =  0; j < spriteList.Count ; j++) {
                                            
                    if (spriteList[j].spriteColor  == pixelColor) {       
                        
                        Vector2Int PxCoord = new Vector2Int(x,y);                  
                        spriteList[j].spritePixels.Add(PxCoord);

                        // check for biggest and lowest X n Y
                        if ( x < spriteList[j].lowerX)
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
                    Debug.Log("new color found !");
                    Vector2Int pxCoord = new Vector2Int(x, y);
                    SpriteObj newSpriteObj = GenerateSpriteObj(pixelColor, pxCoord);
                    spriteList.Add(newSpriteObj);
                }
                   


                
            }
        }

        

        //loop to save all sprite("sprite_01", sprite);
        //SaveSprites(spriteList);
    }

    private void GenerateColorFormat(float[] colorToGet)
    {
        for (int i = 0; i < colorToGet.Length; i++)
        {
            colorToGetFormated[i] = colorToGet[i] / 255;
        }
        colorToGetFormated[3] = 1;
    }

    private int GenerateID()
    {
        int givenId = 0;
        int id = givenId;
        givenId++;
        return(id);
    }


    // sprite object contain list of pixel to be set and value for the size of the sprite
    private SpriteObj GenerateSpriteObj(Color pixelColor, Vector2Int pixelCoord)
    {
        SpriteObj newSprite = new SpriteObj(pixelColor, pixelCoord);
        int id = GenerateID();
        newSprite.setId(id);
        return (newSprite);
    }

    public class SpriteObj
    {
       
        public Color spriteColor;
        public List<Vector2Int> spritePixels;
        public int higherX;
        public int higherY;
        public int lowerX;
        public int lowerY;
        public int id;

        public SpriteObj (Color SPRITECOLOR, Vector2Int pixelCoord)
        {
            spriteColor = SPRITECOLOR;
            spritePixels = new List<Vector2Int>();
            spritePixels.Add(pixelCoord);
            higherX = pixelCoord.x;
            higherY = pixelCoord.y;
            lowerX = pixelCoord.x;
            lowerY = pixelCoord.y;
            
        }
            public void setId(int GivenId)
            {
                id = GivenId;
            }


    }

    private void SaveSprites(List<SpriteObj> spriteList)
    {
        byte[] bytes;

        for (int i = 0; i < spriteList.Count; i++)
        {
            int sizeX = spriteList[i].higherX - spriteList[i].lowerX + 1;
            int sizeY = spriteList[i].higherY - spriteList[i].lowerY + 1;

            Texture2D tex = new Texture2D(sizeX, sizeY);
            Color[] colorArray = new Color[sizeX * sizeY];

            // Resize pixels for new texture size
            for (int j = 0; j < spriteList[i].spritePixels.Count; j++)
            {
                int x = spriteList[i].spritePixels[j].x - spriteList[i].lowerX;
                int y = spriteList[i].spritePixels[j].y - spriteList[i].lowerY;
                int index = x + y * sizeX;

                colorArray[index] = Color.red;
            }

            tex.SetPixels(colorArray);
            bytes = tex.EncodeToPNG();

            if (bytes != null)
            {
                File.WriteAllBytes(pathSave + "/img_" + i + ".png", bytes);
            }
        }
    }

}
