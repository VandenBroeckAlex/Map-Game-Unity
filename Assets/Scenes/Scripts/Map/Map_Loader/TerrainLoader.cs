using System.IO;
using UnityEngine;

public class TerrainFromExactImageSize : MonoBehaviour
{
    public string fileName = "Province_Map_height.png";  // Include the extension
    public float terrainMaxHeight = 4.9f;                  // Vertical scale
    public Material waterMaterial;
    public Material provinceIdMat;

    void Start()
    {
        // Load image from persistent data path
        string fullPath = Path.Combine(Application.persistentDataPath, fileName);

        if (!File.Exists(fullPath))
        {
            Debug.LogError("Heightmap image not found at: " + fullPath);
            return;
        }

        byte[] fileData = File.ReadAllBytes(fullPath);
        Texture2D heightMap = new Texture2D(2, 2); // Size doesn't matter, LoadImage will replace it
        if (!heightMap.LoadImage(fileData))
        {
            Debug.LogError("Failed to load heightmap image from: " + fullPath);
            return;
        }

        int imgWidth = heightMap.width;
        int imgHeight = heightMap.height;

        // Pick the nearest valid resolution (power of two + 1)
        int heightmapRes = Mathf.NextPowerOfTwo(Mathf.Max(imgWidth, imgHeight)) + 1;

        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = heightmapRes;
        terrainData.size = new Vector3(imgWidth, terrainMaxHeight, imgHeight);

        float[,] heights = new float[heightmapRes, heightmapRes];

        for (int y = 0; y < heightmapRes; y++)
        {
            for (int x = 0; x < heightmapRes; x++)
            {
                float u = x / (float)(heightmapRes - 1);
                float v = y / (float)(heightmapRes - 1);

                int px = Mathf.Clamp(Mathf.RoundToInt(u * (imgWidth - 1)), 0, imgWidth - 1);
                int py = Mathf.Clamp(Mathf.RoundToInt(v * (imgHeight - 1)), 0, imgHeight - 1);

                float grayscale = heightMap.GetPixel(px, py).grayscale;
                heights[y, x] = grayscale;
            }
        }

        terrainData.SetHeights(0, 0, heights);

        GameObject terrainGO = Terrain.CreateTerrainGameObject(terrainData);
        terrainGO.transform.position = new Vector3(0, -5.01f, -imgHeight);
        terrainGO.GetComponent<Terrain>().heightmapPixelError = 5f;

        Debug.Log($"Terrain created: {imgWidth}x{imgHeight} units with height resolution {heightmapRes}x{heightmapRes}");

        CreateWaterPlane(imgWidth, imgHeight);
        CreateProvinceIdMap(imgWidth, imgHeight);
    }

    void CreateWaterPlane(int imgWidth, int imgHeight)
    {
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(imgWidth / 10f, 1f, imgHeight / 10f);
        plane.transform.position = new Vector3(imgWidth / 2, -0.5f, -imgHeight / 2);
        plane.GetComponent<Renderer>().material = waterMaterial;
    }

    void CreateProvinceIdMap(int imgWidth, int imgHeight)
    {
        GameObject provinceIDPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        provinceIDPlane.name = "provinceIDPlane";
        provinceIDPlane.transform.localScale = new Vector3(imgWidth / 10f, 1f, imgHeight / 10f);
        provinceIDPlane.transform.position = new Vector3(imgWidth / 2, 0, -imgHeight / 2);
        provinceIDPlane.transform.rotation = Quaternion.Euler(0, 180, 0);
        provinceIDPlane.layer = LayerMask.NameToLayer("Raycast");
        provinceIDPlane.GetComponent<Renderer>().material = provinceIdMat;

        var renderer = provinceIDPlane.GetComponent<Renderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
    }
}
