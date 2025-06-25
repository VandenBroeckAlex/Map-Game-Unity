using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using MyGame.Data;
using System;
using UnityEngine.Rendering;

public class FileUtils 
{
    Texture2D LoadBaseImage(string baseImagePath)
    {
        Texture2D BaseImg = new Texture2D(0,0);
        if (File.Exists(baseImagePath))
        {
            byte[] imageData = File.ReadAllBytes(baseImagePath);
            
            BaseImg.LoadImage(imageData);
        }
        return BaseImg;
    }

    void DeleteOldSpriteFiles(string pathSave)
    {
        if (Directory.Exists(pathSave))
        {
            foreach (string filePath in Directory.GetFiles(pathSave))
            {
                File.Delete(filePath);
            }
        }
    }

    private bool IsFolderEmpty(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            return Directory.GetFiles(folderPath).Length == 0 &&
                   Directory.GetDirectories(folderPath).Length == 0;
        }
        return true;
    }
}
