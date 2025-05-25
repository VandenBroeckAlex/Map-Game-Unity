using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using MyGame.Data;
using UnityEngine;


public class RaycastScript : MonoBehaviour
{
    public LayerMask mask;
    public delegate GameObject OnRightClick();
    public delegate GameObject OnLeftClick();
    Camera cam;
    Dictionary<int, float[]> jsonData;

   


    void Start()
    {
        cam = Camera.main;
        MouseClickHandeler.onLeftClick += CallRaycast;
        MouseClickHandeler.onRightClick += CallRaycast;
        LoadJsonDataMapPosition();
    }

    void LoadJsonDataMapPosition()
    {
        string fullPath = Path.Combine(Application.persistentDataPath, "ColorId.json");
        string jsonFile = File.ReadAllText(fullPath);

        jsonData = JsonConvert.DeserializeObject<Dictionary<int, float[]>>(jsonFile);

    }



    private void CallRaycast()

    {
        GetRayCast();

    }


    private GameObject GetRayCast()
    {


        Vector3 mousPos = Input.mousePosition;
        mousPos.z = 100f;

        Debug.DrawRay(transform.position, mousPos-transform.position,UnityEngine.Color.blue);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 10000, mask))
        {
            Debug.Log("hit");
            GetPixelColor(hit);
            
            return GetGameObject(hit);

        }
        else
        {
            return null;
        }
    }

    void GetPixelColor(RaycastHit hit)
    {

        Vector2 pixelUV = hit.textureCoord;
        Renderer renderer = hit.transform.GetComponent<Renderer>();
        Texture2D imageTexture = renderer.material.mainTexture as Texture2D;
        pixelUV.x *= imageTexture.width;
        pixelUV.y *= imageTexture.height;
        Vector2 tiling = renderer.material.mainTextureScale;
        Color color = imageTexture.GetPixel(Mathf.FloorToInt(pixelUV.x * tiling.x), Mathf.FloorToInt(pixelUV.y * tiling.y));
        GetID(color);


    }

    public GameObject GetGameObject(RaycastHit hit)
    {
        
        Debug.Log(hit.transform.position);
        return hit.transform.gameObject;
    }

    // get it in the province handler
    public int GetID(Color color)
    {
        GameObject g = GameObject.Find("ProvinceHandeler");
        ProvinceHandeler1 bScript = g.GetComponent<ProvinceHandeler1>();
        List<Province> allProvinces = bScript.allProvinces;

        foreach (var kvp in jsonData)
        {
            float[] stored = kvp.Value;

            // Compare exact color values (no tolerance)
            if (color.r == stored[0] && color.g == stored[1] && color.b == stored[2])
            {
                Debug.Log(kvp.Key);
                return kvp.Key; // Found a match
            }
        }

        // No match found
        Debug.Log("No match found");
        return -1; 
    }
}
