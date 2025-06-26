using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class global_map_creator_ui_manager : MonoBehaviour
{
    private VisualElement root;

    private Button save_button;

    private ProvinceSaver provinceSaver;
    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        save_button = root.Q<Button>("save_button");

        provinceSaver = new ProvinceSaver(); 

        save_button.clicked += () => provinceSaver.SaveProvinceData(); 
    }
}
