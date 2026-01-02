using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;


public class SpriteCreator : MonoBehaviour
{
    
    string pathSave = Application.dataPath;
    float[] colorToGet = { 52, 255, 68}; // to divide by 255
    Color colorToGetFormated;
    public Texture2D fullMap;
    private Texture2D sprite;



    // Start is called before the first frame update
    void Start()
    {
        sprite = new Texture2D(80, 80);
        pathSave += "/sprites_terrain/provinces_split";

        GenerateColorFormat(colorToGet);
        Debug.Log("the format is :" + colorToGetFormated);
        GenerateMapSprite();
        Debug.Log("hello cc");
    }

    private void GenerateMapSprite()
    {
      
        // keep higher and lower x and y for img size create the image one time a the end

        for (int x = 0; x < fullMap.width; x++) {
            for(int y = 0; y < fullMap.height; y++)
            {
                Color pixelColor = fullMap.GetPixel(x, y);

                if (pixelColor != null && pixelColor == colorToGetFormated) {
                    
                    sprite.SetPixel(x, y, pixelColor);
                }
                saveFile("sprite_01", sprite);
            }
        }

    }

    private void GenerateColorFormat(float[] colorToGet)
    {
        for(int i = 0; i < colorToGet.Length; i++)
        {
            colorToGetFormated[i] = colorToGet[i] / 255;
        }
        colorToGetFormated[3] = 1;
    }

    public void saveFile(string fileNameSaveInto, Texture2D tex)
    {
        // Encode texture into PNG
        byte[] bytes = tex.EncodeToPNG();
     
        File.WriteAllBytes(pathSave + "/img_01" +".png", bytes);
    }

    // void MarkPixel(int x, int y){}

}
