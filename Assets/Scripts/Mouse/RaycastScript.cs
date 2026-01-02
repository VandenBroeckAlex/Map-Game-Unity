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

    public delegate void OnRayCast(Color color);
    public static OnRayCast onProvincePlaneHit;

    

    void Start()
    {
        cam = Camera.main;
        MouseClickHandeler.onLeftClick += CallRaycast;
        MouseClickHandeler.onRightClick += CallRaycast; 
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

        // if ui class hit => call ui handler
        // if province plane hit => get province color => call province handler
        // if unit class hit => call ui handler

        if (Physics.Raycast(ray, out hit, 10000, mask))
        {
            Debug.Log("hit");
            GetPixelColor(hit);
            
            return GetTransformHit(hit);

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
        onProvincePlaneHit?.Invoke(color);
    }

    public GameObject GetTransformHit(RaycastHit hit)
    {
        Debug.Log(hit.transform.position);
        return hit.transform.gameObject;
    }
    
}
