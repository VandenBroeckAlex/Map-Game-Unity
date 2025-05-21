using System.IO;
using UnityEngine;
using UnityEngine.Rendering;

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
        //create the province id mat 

        // load id map
        string baseImagePath = Path.Combine(Application.persistentDataPath, "Province_Map.png");
        byte[] imageData = File.ReadAllBytes(baseImagePath);
        Texture2D BaseImg = new Texture2D(2, 2);
        BaseImg.LoadImage(imageData);
        provinceIdMat.SetTexture("_BaseMap", BaseImg);

        //create mesh
        provinceIDPlane.GetComponent<Renderer>().material = provinceIdMat; //load it 

        var renderer = provinceIDPlane.GetComponent<Renderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
    }
}

/*
 float GetHeightFromTerrainType(TerrainType type, Vector2 position) {
    switch (type) {
        case TerrainType.Water:
            return 0f;
        case TerrainType.Plains:
            return 0.2f + Mathf.PerlinNoise(position.x * 0.01f, position.y * 0.01f) * 0.05f;
        case TerrainType.Hills:
            return 0.4f + Mathf.PerlinNoise(position.x * 0.02f, position.y * 0.02f) * 0.1f;
        case TerrainType.Mountains:
            return 0.6f + Mathf.PerlinNoise(position.x * 0.03f, position.y * 0.03f) * 0.3f;
        default:
            return 0f;
    }
}
for (int x = 0; x < terrainWidth; x++) {
    for (int y = 0; y < terrainHeight; y++) {
        Vector2 pos = new Vector2(x, y);
        Province province = GetProvinceAtPosition(pos); // Determine based on your map data
        float height = GetHeightFromTerrainType(province.terrainType, pos);
        heightMap[x, y] = height;
    }
}
 */