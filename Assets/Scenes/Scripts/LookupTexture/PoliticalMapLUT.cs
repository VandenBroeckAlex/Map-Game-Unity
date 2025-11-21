using System.Collections.Generic;
using UnityEngine;
using MyGame.Data;
using System;
using System.Linq;

public class PoliticalMapLUT
{
    [Obsolete]
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


    public static Texture2D BuildPoliticalLUT(Dictionary<int, Tile> provinces)
    {
        int count = provinces.Count;

        foreach (var kv in provinces) 
        { 
            if(kv.Value.id > count-1)
            {
                count = kv.Value.id +1;
            }
        }

        Texture2D lut = new Texture2D(count, 1, TextureFormat.RGBA32, false);
        lut.filterMode = FilterMode.Point;
        lut.wrapMode = TextureWrapMode.Clamp;

        Color32[] colors = new Color32[count];

        foreach (KeyValuePair<int, Tile> entry in provinces)
        {
            Color32 countryColor = new Color32(0, 0, 0, 0);

            if (entry.Value.isLand is true)
            {
                LandTile province = (LandTile)entry.Value;
                int countryId = province.ownerId;

                countryColor = CountriesManager.instance.GetCountryColorById(province.ownerId);
                countryColor.a = 255;
            }

            colors[entry.Key] = countryColor;
        }

        lut.SetPixels32(colors);
        lut.Apply();

        return lut;
    }
}
