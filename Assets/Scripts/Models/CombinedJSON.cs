using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CombinedJSON
{
    public int canvaWidth;
    public int canvaHeight;
    public List<ObjJSON> spriteListJSON;


    public CombinedJSON(int width, int height, List<ObjJSON> list)
    {
        canvaWidth = width;
        canvaHeight = height;
        spriteListJSON = list;
    }
}