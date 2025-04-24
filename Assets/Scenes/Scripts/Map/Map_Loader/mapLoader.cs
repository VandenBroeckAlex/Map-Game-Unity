using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEditor.U2D.Sprites;

public class mapLoader : MonoBehaviour
{
    // Start is called before the first frame update
    public class SpritePositionData
    {
        public int id;
        public float[] spriteColor;
        public float lowerX;
        public float higherY;
    }

    [System.Serializable]
    public class JSONData
    {
        public float canvaWidth;
        public float canvaHeight;
        public SpritePositionData[] spriteListJSON;
    }

    JSONData spriteData;
    Canvas canvas;
    public Material outlineMat;


    void Start()
    {        
        LoadJsonData();              
        CreateCanva();
        LoadSprites(spriteData);      
    }

    void LoadJsonData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("map_info");
        spriteData = JsonConvert.DeserializeObject<JSONData>(jsonFile.text);
    }

    void LoadSprites(JSONData spriteData)
    {
        Debug.Log(spriteData.spriteListJSON);

        foreach (var spriteEntry in spriteData.spriteListJSON)
        {
            string spritePath = $"provinces_split/img_{spriteEntry.id}"; 
            Sprite sprite = Resources.Load<Sprite>(spritePath);

            if (sprite != null)
            {
                CreateSpriteImage(sprite, spriteEntry);
            }
            else
            {
                Debug.LogError($"Sprite with name provinces_split/img_{spriteEntry.id} not found.");
            }
        }
    }


    void CreateSpriteImage(Sprite sprite, SpritePositionData spriteEntry)
    {
        // Create a new GameObject to hold the Image
        GameObject spriteObj = new GameObject("Sprite_" + spriteEntry.id);
        spriteObj.transform.SetParent(canvas.transform); // Set the parent to the Canva

        spriteObj.transform.rotation = Quaternion.Euler(90, 0, 0);
       
        Image image = spriteObj.AddComponent<Image>();
        image.sprite = sprite;
        image.material =  outlineMat;

        Color spriteColor = new Color(spriteEntry.spriteColor[0], spriteEntry.spriteColor[1], spriteEntry.spriteColor[2]);
        image.color = spriteColor;

        // Set the position based on the `lowerX` and `higherY` values
        RectTransform rt = spriteObj.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0, 0);   // Min anchor at top-left
        rt.anchorMax = new Vector2(0, 0);
        rt.pivot = new  Vector2 (0,1);
        rt.anchoredPosition = new Vector2(spriteEntry.lowerX , spriteEntry.higherY + 1); // 1 is added to Y because the map is 1px out of the canvas without it, for some reason.
        rt.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height); // Adjust size based on the sprite
    }

    void CreateCanva() {

        // 1) creat adequate canvas
        GameObject canvasObj = new GameObject("MapCanvas");
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.pixelPerfect = true;
        canvas.transform.position = new Vector3(0, 0, 0);
        canvas.transform.rotation = Quaternion.Euler(90, 0, 0);
        RectTransform rt = canvas.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0, 1);

        // set canvas size
        if (spriteData != null)
        {
            canvas.GetComponent<RectTransform>().sizeDelta = new Vector2(spriteData.canvaWidth, spriteData.canvaHeight);
        }
    }
}
