using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class TerrainFromExactImageSize : MonoBehaviour
{
    public string resourcePath = "Province_Map_height";  // Without ".png"
    public float terrainMaxHeight = 5f;                // Vertical scale
    public Material waterMaterial;
    public Material provinceIdMat;

    void Start()
    {
        // Load the heightmap image
        Texture2D heightMap = Resources.Load<Texture2D>(resourcePath);
        if (heightMap == null)
        {
            Debug.LogError("Could not load heightmap from Resources/" + resourcePath);
            return;
        }

        int imgWidth = heightMap.width;
        int imgHeight = heightMap.height;

        // Pick the nearest valid resolution (power of two + 1)
        int heightmapRes = Mathf.NextPowerOfTwo(Mathf.Max(imgWidth, imgHeight)) + 1;

        TerrainData terrainData = new TerrainData();
        terrainData.heightmapResolution = heightmapRes;
        
        // Make terrain physically match the image size
        terrainData.size = new Vector3(imgWidth, terrainMaxHeight, imgHeight);

        float[,] heights = new float[heightmapRes, heightmapRes];

        for (int y = 0; y < heightmapRes; y++)
        {
            for (int x = 0; x < heightmapRes; x++)
            {
                // Map terrain height sample to image space
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

        // create the sea plane
        GameObject plane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        plane.transform.localScale = new Vector3(imgWidth / 10f, 1f, imgHeight / 10f);
        plane.transform.position = new Vector3(imgWidth/2, -0.5f, -imgHeight/2);
        plane.GetComponent<Renderer>().material = waterMaterial;


        CreateProvinceIdMap(imgWidth, imgHeight);
    }

    void CreateProvinceIdMap( int imgWidth, int imgHeight)
    {
        // create the province id plane
        GameObject provinceIDPlane = GameObject.CreatePrimitive(PrimitiveType.Plane);
        provinceIDPlane.name = "provinceIDPlane";
        provinceIDPlane.transform.localScale = new Vector3(imgWidth / 10f, 1f, imgHeight / 10f);
        provinceIDPlane.transform.position = new Vector3(imgWidth / 2, 0, -imgHeight / 2);
        provinceIDPlane.transform.rotation =  Quaternion.Euler(0, 180, 0);
        provinceIDPlane.layer = LayerMask.NameToLayer("Raycast");
        provinceIDPlane.GetComponent<Renderer>().material = provinceIdMat;
        MeshRenderer renderer = provinceIDPlane.GetComponent<MeshRenderer>();
        renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
        renderer.receiveShadows = false;
        renderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
    }
}