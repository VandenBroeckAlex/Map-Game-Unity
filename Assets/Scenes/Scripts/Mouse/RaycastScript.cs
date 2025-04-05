using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

public class RaycastScript : MonoBehaviour
{
    public LayerMask mask;
    public delegate GameObject OnRightClick();
    public delegate GameObject OnLeftClick();
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        MouseClickHandeler.onLeftClick += CallRaycast;
        MouseClickHandeler.onRightClick += CallRaycast;
    }

    // Update is called once per frame
  

    private void CallRaycast()
    {
        GetRayCast();

    }


    private GameObject GetRayCast()
    {


        Vector3 mousPos = Input.mousePosition;
        mousPos.z = 100f;

        //Debug.DrawRay(transform.position, mousPos-transform.position,Color.blue);

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100, mask))
        {

            // GetPixelColor(hit);
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
        Debug.Log(imageTexture.GetPixel(Mathf.FloorToInt(pixelUV.x * tiling.x), Mathf.FloorToInt(pixelUV.y * tiling.y)));

    }

    public GameObject GetGameObject(RaycastHit hit)
    {
        
        Debug.Log(hit.transform.position);
        return hit.transform.gameObject;
    }
}
