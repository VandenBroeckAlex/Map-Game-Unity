[System.Serializable]
public class PlayerData
{
    public int id;
    public string playerName;
    public int countryID;
    public bool isHuman;

    public PlayerData(int id, string name, int countryID, bool isHuman)
    {
        this.id = id;
        this.playerName = name;
        this.countryID = countryID;
        this.isHuman = isHuman;
    }
}
