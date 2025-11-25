using UnityEngine;
using UnityEngine.UIElements;

public class MarketUIInitializer : MonoBehaviour
{
    public VisualTreeAsset listEntryTemplate;

    private CharacterListController controller;

    void Awake()
    {
        Debug.Log("MarketUIInitializer: Awake");

        var doc = GetComponent<UIDocument>();
        var root = doc.rootVisualElement;

        controller = new CharacterListController();

        Debug.Log($"Calling InitializeCharacterList...{root}");
        Debug.Log(listEntryTemplate);
        controller.InitializeCharacterList(root, listEntryTemplate);
    }
}
