
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;




public class SpriteCreator_v2 : MonoBehaviour
{
    
    string pathSave = Application.dataPath;
    

    float[] colorToGet = { 52, 255, 68 }; // to divide by 255
    Color colorToGetFormated;
    public Texture2D BaseImg; //aller la rechercher automatiquement gr�ce � son nom et la set comme readeable
                    
   
    private List <SpriteObj> spriteList = new List<SpriteObj>();
    


    //the image can not contain black !

    // Start is called before the first frame update
    void Start()
    {

        //supprimer toutes les img pr�sente dans le dossier
        pathSave += "/sprites_terrain/provinces_split";

        //GenerateColorFormat(colorToGet);
      
        GenerateMapSprite();
     
        SaveSprites(spriteList);
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


    public void SaveSprites(List<SpriteObj> spriteList)
    {
        // Encode texture into PNG
        byte[] bytes;
        

        for (int i = 0; i < spriteList.Count; i++)
        {


            int sizeX = spriteList[i].higherX - spriteList[i].lowerX +1;
            int sizeY= spriteList[i].higherY - spriteList[i].lowerY +1;

            int baseImgX = BaseImg.width;
            int baseImgY = BaseImg.height;

            Texture2D tex = new Texture2D(10  , 10 ) ;

            Color[] colorArray = new Color[10 * 10];

            Debug.Log("the img id : " + i + "X = " + sizeX + "Y = " + sizeY);
            Debug.Log("img width size : " + tex.width);
            Debug.Log("the size of the array is : " + colorArray.Length);

            try
            {
                for (int j = 0; j < spriteList[i].spritePixels.Count; j++)
                {


                    colorArray[spriteList[i].spritePixels[j].x  + (spriteList[i].spritePixels[j].y * tex.width)] = Color.red;

                }
            }
            catch
            {
                Debug.Log("nop");
            }
           

            Debug.Log("colorArray = " + colorArray.Length);


            tex.SetPixels(colorArray);
            bytes = tex.EncodeToPNG();


            if (bytes != null)
            {
                
                File.WriteAllBytes(pathSave + "/img_"+ i + ".png", bytes);
            }

        }


    }
}
