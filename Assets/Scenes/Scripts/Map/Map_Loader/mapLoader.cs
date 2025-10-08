using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using static UnityEditor.U2D.ScriptablePacker;

public class MapLoader : MonoBehaviour
{
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
        public int canvaWidth;
        public int canvaHeight;
        public SpritePositionData[] SpriteListJSON;
    }

    JSONData SpriteData;
    public static int CanvaWidth;
    public static int CanvaHeight;

    Canvas _Canvas;
    public Material OutlineMat;

    void Start()
    {
        LoadJsonData();
        CreateCanva();
        StartCoroutine(LoadSpritesAsync(SpriteData));
    }

    void LoadJsonData()
    {
        string JsonPath = FilePath.MapInfo;

        if (File.Exists(JsonPath))
        {
            string jsonText = File.ReadAllText(JsonPath);
            SpriteData = JsonConvert.DeserializeObject<JSONData>(jsonText);
        }
        else
        {
            Debug.LogError("map_info.json not found at " + JsonPath);
        }
    }

    IEnumerator LoadSpritesAsync(JSONData spriteData)
    {
        string FolderPath = FilePath.ProvincesSplit;

        int loadedCount = 0;

        foreach (var spriteEntry in spriteData.SpriteListJSON)
        {
            try
            {
                string filePath = Path.Combine(FolderPath, $"img_{spriteEntry.id}.png");

                if (File.Exists(filePath))
                {
                    byte[] fileData = File.ReadAllBytes(filePath);

                    Texture2D tex = new Texture2D(2, 2);
                    if (tex.LoadImage(fileData))
                    {
                        tex.filterMode = FilterMode.Point;
                        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 1));
                        CreateSpriteImage(sprite, spriteEntry);
                        loadedCount++;
                    }
                    else
                    {
                        Debug.LogError($"Failed to load texture from {filePath}");
                    }
                }
                else
                {
                    Debug.LogWarning($"Image not found: {filePath}");
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Exception loading sprite ID {spriteEntry.id}: {ex}");
            }

            // This must be outside the try-catch
            yield return null;
        }

        Debug.Log($"Finished loading {loadedCount}/{spriteData.SpriteListJSON.Length} sprites.");
    }


    void CreateSpriteImage(Sprite sprite, SpritePositionData spriteEntry)
    {
        GameObject spriteObj = new GameObject("Sprite_" + spriteEntry.id);
        spriteObj.transform.SetParent(_Canvas.transform);
        spriteObj.transform.rotation = Quaternion.Euler(90, 0, 0);

        Image image = spriteObj.AddComponent<Image>();
        image.sprite = sprite;
        image.material = OutlineMat;

        Color spriteColor = new Color(
            spriteEntry.spriteColor[0],
            spriteEntry.spriteColor[1],
            spriteEntry.spriteColor[2]
        );
        image.color = spriteColor;

        RectTransform rt = spriteObj.GetComponent<RectTransform>();
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.zero;
        rt.pivot = new Vector2(0, 1);
        rt.anchoredPosition = new Vector2(spriteEntry.lowerX, spriteEntry.higherY + 1);
        rt.sizeDelta = new Vector2(sprite.rect.width, sprite.rect.height);
    }

    void CreateCanva()
    {
        GameObject canvasObj = new GameObject("MapCanvas");
        _Canvas = canvasObj.AddComponent<Canvas>();
        _Canvas.renderMode = RenderMode.WorldSpace;
        _Canvas.pixelPerfect = true;
        _Canvas.transform.position = Vector3.zero;
        _Canvas.transform.rotation = Quaternion.Euler(90, 0, 0);

        RectTransform rt = _Canvas.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0, 1);

        if (SpriteData != null)
        {
            rt.sizeDelta = new Vector2(SpriteData.canvaWidth, SpriteData.canvaHeight);
             CanvaWidth = SpriteData.canvaWidth;
             CanvaHeight = SpriteData.canvaHeight;
}

    }
}
