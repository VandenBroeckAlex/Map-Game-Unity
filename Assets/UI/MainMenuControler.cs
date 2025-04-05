using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuControler : MonoBehaviour
{
    public static bool recalculateMapChoice = false;
    public VisualElement ui;
    public Button playButton;
    public Button mapEditorButton;
    public Toggle recalculateMap;
    public Button quitButton;



    private void Awake()
    {
        ui = GetComponent<UIDocument>().rootVisualElement;
    }

    private void OnEnable()
    {
        playButton = ui.Q<Button>("PlayButton");
        playButton.clicked += OnPlayButtonClicked;

        mapEditorButton = ui.Q<Button>("MapEditorButton");
        mapEditorButton.clicked += OnMapEditorButtonClicked;

        recalculateMap = ui.Q<Toggle>("ReCalculateMap");
       

        quitButton = ui.Q<Button>("QuitButton");
        quitButton.clicked += OnQuit;
    }

    public void OnPlayButtonClicked()
    {
        recalculateMapChoice = recalculateMap.value;
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OnMapEditorButtonClicked()
    {
        recalculateMapChoice = recalculateMap.value;
        SceneManager.LoadScene("MapEditor", LoadSceneMode.Single);

    }

    public void OnQuit()
    {
        Application.Quit();

       EditorApplication.isPlaying = false;
    }
}
