using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager instance { get; private set; }

    private List<Player> players = new List<Player>();

    private Player humanPlayer;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Initialize();
        DontDestroyOnLoad(gameObject);
    }

    public void Initialize()
    {
        humanPlayer = new Player(0, "Alex", 0, true);
        players.Add(humanPlayer);
    }

    public Player GetHumanPlayer()
    {
        return humanPlayer;
    }
}
