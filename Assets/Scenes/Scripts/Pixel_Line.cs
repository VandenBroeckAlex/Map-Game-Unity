using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pixel_Line : MonoBehaviour
{
    public Texture2D MapTex;
    private List<Vector3> pointList = new List<Vector3> {} ;
    private int x = 0;
    private int y = 0;
    private Color lastPx = Color.black;
    


    // Start is called before the first frame update
    private void Start()
    {
         double NbPixels = Math.Pow(MapTex.width, 2);

        lastPx = MapTex.GetPixel(0, 0);

        for (double i = 1; i <= NbPixels ; i++)
        {
            x++;

            if (MapTex.GetPixel(x, y) != lastPx){

               pointList.Add(new Vector2(x, y));
                x = 0;
                y++;
            }

            if (x == MapTex.width)
            {
                x = 0;
                y++;
            }

            if(y == MapTex.height)
            {
                break;


            }
            lastPx = MapTex.GetPixel(x, y,0);
            Debug.Log(MapTex.GetPixel(x,y));

        }
        /*check every pixel, if pixel not equal to previous pixel. store picel position
         
         image size/100 = uv de 1 px         
         */
        Debug.Log("The list have " + pointList.Count + " point in it");
/*
        Vector3[] LinePoints = pointList.ToArray();
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        lineRenderer.positionCount = pointList.Count;
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true;
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.SetPositions(LinePoints);

        */
    }
}
