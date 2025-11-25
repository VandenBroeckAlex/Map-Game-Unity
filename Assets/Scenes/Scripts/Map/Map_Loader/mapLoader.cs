using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using static ObjJSON;


public class MapLoader : MonoBehaviour
{

    //get height and width here
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

    List<SpriteInfo> SpriteData;
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
        string jsonPath = FilePath.SpritesInfos;

        if (File.Exists(jsonPath))
        {
            string jsonText = File.ReadAllText(jsonPath);
            SpriteData = JsonConvert.DeserializeObject<List<SpriteInfo>>(jsonText);
        }
        else
        {
            Debug.LogError("map_info.json not found at " + jsonPath);
        }
    }

    IEnumerator LoadSpritesAsync(List<SpriteInfo> spriteData)
    {
        string FolderPath = FilePath.ProvincesSplit;

        int loadedCount = 0;

        foreach (var spriteEntry in spriteData)
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
            yield return null;
        }

        Debug.Log($"Finished loading {loadedCount}/{spriteData.Count} sprites.");
    }


    void CreateSpriteImage(Sprite sprite, SpriteInfo spriteEntry)
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

        // load id map
        string baseImagePath = Path.Combine(Application.persistentDataPath, "Province_Map.png");
        byte[] imageData = File.ReadAllBytes(baseImagePath);
        Texture2D BaseImg = new Texture2D(2, 2);
        BaseImg.LoadImage(imageData);


        if (SpriteData != null)
        {
            rt.sizeDelta = new Vector2(BaseImg.width, BaseImg.height);
            CanvaWidth = BaseImg.width;
            CanvaHeight = BaseImg.height;
        }

    }
}
