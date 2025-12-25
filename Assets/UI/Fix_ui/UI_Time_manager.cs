using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Good;

public class UI_Time_manager : MonoBehaviour
{
    public static UI_Time_manager instance;

    private TickScript _tickscript; 

    public GameObject UI_time;
    public Transform fixed_ui;

    public Button pause;
    public Button resume;

    public void Initialize()
    {
        instance = this;
        _tickscript = TickScript.instance;
        fixed_ui = GameObject.Find("FixedLayer").transform;
        UI_time = GameObject.Find("fix_right container");
        pause = GameObject.Find("pause").GetComponent<Button>();
        pause.onClick.AddListener(() => PauseGame());
        resume = GameObject.Find("play").GetComponent<Button>();
        resume.onClick.AddListener(() => ResumeGame());
    }
    private void OnEnable()
    {
        DateHandeler.onDateChanged += DisplayStringDate;
    }
    private void OnDisable()
    {
        DateHandeler.onDateChanged -= DisplayStringDate;
    }

     void DisplayStringDate(string date)
    {
        TextMeshProUGUI[] texts = UI_time.GetComponentsInChildren<TextMeshProUGUI>();
        texts[0].text = date;
    }

    private void PauseGame()
    {
        Debug.Log("button pause game");
        _tickscript.PauseGame();
    }
    private void ResumeGame()
    {
        Debug.Log("button resume game");
        _tickscript.ThreeSpeed();
    }
}