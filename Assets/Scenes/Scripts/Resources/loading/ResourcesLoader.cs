using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;

public class ResourcesLoader : MonoBehaviour
{
    public static ResourcesLoader Instance;

    public Dictionary<string, ResourceDefinition> resources;

    private void Awake()
    {
        Instance = this;
        LoadResources();
    }

    public void LoadResources()
    {
        /*
        string json = File.ReadAllText(Application.streamingAssetsPath + "/resources.json");
        var list = JsonUtilityWrapper.FromJsonList<ResourceDefinition>(json);
        resources = list.ToDictionary(r => r.id);
        */
    }

    public ResourceDefinition Get(string id) => resources.TryGetValue(id, out var r) ? r : null;
}
