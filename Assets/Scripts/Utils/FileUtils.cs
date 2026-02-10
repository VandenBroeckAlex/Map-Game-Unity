using UnityEngine;
using System.IO;


public class FileUtils 
{
    public static Texture2D LoadBaseImage(string baseImagePath)
    {
        Texture2D BaseImg = new Texture2D(0,0);
        if (File.Exists(baseImagePath))
        {
            byte[] imageData = File.ReadAllBytes(baseImagePath);
            
            BaseImg.LoadImage(imageData);
        }
        return BaseImg;
    }

    public static void DeleteOldSpriteFiles(string pathSave)
    {
        if (Directory.Exists(pathSave))
        {
            foreach (string filePath in Directory.GetFiles(pathSave))
            {
                File.Delete(filePath);
            }
        }
    }

    public static bool IsFolderEmpty(string folderPath)
    {
        if (Directory.Exists(folderPath))
        {
            return Directory.GetFiles(folderPath).Length == 0 &&
                   Directory.GetDirectories(folderPath).Length == 0;
        }
        return true;
    }

    public static bool CreateDirectory(string path)
    {
        if (Directory.Exists(path))
        {
            return true;
        }

        Directory.CreateDirectory(path);
        return true;
    }
}
