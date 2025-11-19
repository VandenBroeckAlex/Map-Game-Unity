using System.Collections.Generic;
using UnityEngine;
using MyGame.Data;

public class PoliticalMapLUT
{
    public void BuildPoliticalLookupTexture(Dictionary<int, Tile> provinces_list,int size, Material terrainMaterial)
    {
        Color[] pixels = new Color[size+1];
       


        for (int i = 0; i < pixels.Length; i++)
            pixels[i] = Color.clear;

        foreach (var kv in provinces_list) 
        {
            Tile province = kv.Value;
            if (!province.isLand) continue;

            LandTile landprovince = (LandTile)province;
            Color32 ownerColor = CountriesManager.instance.GetCountryColorById(landprovince.ownerId);

            pixels[kv.Key] = ownerColor;
        }
        Texture2D lookupTex = new Texture2D(size, 1, TextureFormat.RGBA32, false);
        lookupTex.filterMode = FilterMode.Point;
        lookupTex.wrapMode = TextureWrapMode.Clamp;
        lookupTex.SetPixels(pixels);
        lookupTex.Apply(false, false);
        terrainMaterial.SetTexture("_PoliticalLUT", lookupTex);
    }
}
