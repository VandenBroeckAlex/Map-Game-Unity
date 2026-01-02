[System.Serializable]
public class Player
{
    public int id;
    public string playerName;
    public int countryID;
    public bool isHuman;

    public Player(int id, string name, int countryID, bool isHuman)
    {
        this.id = id;
        this.playerName = name;
        this.countryID = countryID;
        this.isHuman = isHuman;
    }
}
