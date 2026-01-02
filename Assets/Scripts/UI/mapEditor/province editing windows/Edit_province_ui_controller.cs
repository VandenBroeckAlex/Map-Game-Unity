using MyGame.Data;
using UnityEngine;
using UnityEngine.UIElements;

public class ProvinceUIController : MonoBehaviour
{
    private VisualElement root;
    private VisualElement provincePage;
    private TextField provinceNameField;
    private Toggle isLandToggle;
    private Button closeButton;

    private Tile currentProvince;
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        provincePage = root.Q<VisualElement>("provincePage");
        provinceNameField = root.Q<TextField>("ProvinceNameField");
        isLandToggle = root.Q<Toggle>("isLand");
        closeButton = root.Q<Button>("Close");

        closeButton.clicked += () => HideProvincePanel();
        HideProvincePanel(); // Hide at start
    }
    public void ShowProvinceInfo(Tile province)
    {
        currentProvince = province;

        provinceNameField.value = province.name;
        isLandToggle.value = province.isLand;

        provincePage.style.display = DisplayStyle.Flex;

        // Register callbacks (with cleanup, explained next)
        RegisterCallbacks();
    }

    public void HideProvincePanel()
    {
        provincePage.style.display = DisplayStyle.None;
    }
    private EventCallback<ChangeEvent<string>> onNameChanged;
    private EventCallback<ChangeEvent<bool>> onIsLandChanged;

    private void RegisterCallbacks()
    {
        // Unregister previous callbacks if they exist
        if (onNameChanged != null)
            provinceNameField.UnregisterValueChangedCallback(onNameChanged);
        if (onIsLandChanged != null)
            isLandToggle.UnregisterValueChangedCallback(onIsLandChanged);

        // Register new callbacks that update the province object
        onNameChanged = evt =>
        {
            if (currentProvince != null)
                currentProvince.name = evt.newValue;
        };
        provinceNameField.RegisterValueChangedCallback(onNameChanged);

        onIsLandChanged = evt =>
        {
            if (currentProvince != null)
                currentProvince.isLand = evt.newValue;
        };
        isLandToggle.RegisterValueChangedCallback(onIsLandChanged);
    }

}
