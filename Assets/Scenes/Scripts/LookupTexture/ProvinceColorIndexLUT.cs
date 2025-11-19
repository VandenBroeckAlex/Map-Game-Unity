using MyGame.Data;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ProvinceColorIndexLUT : MonoBehaviour 
{
    public void BuildProvinceIdLookupTexture(Dictionary<int, float[]> colorIDList, Material terrainMaterial,int size)
    {

        //Debug.Log(colorIDList.Count);
        Color[] pixels = new Color[size+1];
        //Debug.Log("Array size =" + pixels.Length);
        // default color 
        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.red;

        foreach (KeyValuePair<int, float[]> entry in colorIDList)
        {
            float[] col = entry.Value;

            Color32 color = new Color32(
           (byte)Mathf.RoundToInt(col[0] * 255f),
           (byte)Mathf.RoundToInt(col[1] * 255f),
           (byte)Mathf.RoundToInt(col[2] * 255f),
           255);
            //Debug.Log("the key is " + entry.Key);
            pixels[entry.Key] = color;
        }
        Texture2D lookupTex = new Texture2D(size, 1, TextureFormat.RGBA32, false);
        lookupTex.filterMode = FilterMode.Point;
        lookupTex.wrapMode = TextureWrapMode.Clamp;
        lookupTex.SetPixels(pixels);
        lookupTex.Apply(false, false);
        //lookupTex.GetPixel();
        terrainMaterial.SetTexture("_ProvinceColorIndexLUT", lookupTex);
    }
}
