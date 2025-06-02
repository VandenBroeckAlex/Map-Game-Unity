using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class mapLoader : MonoBehaviour
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
        StartCoroutine(LoadSpritesAsync(spriteData));
    }

    void LoadJsonData()
    {
        string jsonPath = Path.Combine(Application.persistentDataPath, "map_info.json");

        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            spriteData = JsonConvert.DeserializeObject<JSONData>(jsonText);
        }
        else
        {
            Debug.LogError("map_info.json not found at " + jsonPath);
        }
    }

    IEnumerator LoadSpritesAsync(JSONData spriteData)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Provinces_split");

        int loadedCount = 0;

        foreach (var spriteEntry in spriteData.spriteListJSON)
        {
            try
            {
                string filePath = Path.Combine(folderPath, $"img_{spriteEntry.id}.png");

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

        Debug.Log($"Finished loading {loadedCount}/{spriteData.spriteListJSON.Length} sprites.");
    }


    void CreateSpriteImage(Sprite sprite, SpritePositionData spriteEntry)
    {
        GameObject spriteObj = new GameObject("Sprite_" + spriteEntry.id);
        spriteObj.transform.SetParent(canvas.transform);
        spriteObj.transform.rotation = Quaternion.Euler(90, 0, 0);

        Image image = spriteObj.AddComponent<Image>();
        image.sprite = sprite;
        image.material = outlineMat;

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
        canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.WorldSpace;
        canvas.pixelPerfect = true;
        canvas.transform.position = Vector3.zero;
        canvas.transform.rotation = Quaternion.Euler(90, 0, 0);

        RectTransform rt = canvas.GetComponent<RectTransform>();
        rt.pivot = new Vector2(0, 1);

        if (spriteData != null)
        {
            rt.sizeDelta = new Vector2(spriteData.canvaWidth, spriteData.canvaHeight);
        }
    }
}
