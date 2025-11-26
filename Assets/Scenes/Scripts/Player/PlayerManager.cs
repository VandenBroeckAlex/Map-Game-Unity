using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    private List<PlayerData> players = new List<PlayerData>();

    private PlayerData humanPlayer;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        humanPlayer = new PlayerData(0, "Alex", 0, true);
        players.Add(humanPlayer);
    }

    public PlayerData GetHumanPlayer()
    {
        return humanPlayer;
    }
}
