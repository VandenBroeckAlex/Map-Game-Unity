using System.Collections.Generic;
public class TileRegistery
{
    private List<Tile> allTiles = new List<Tile>();

    private Dictionary<int, List<Tile>> tilesByCountry = new Dictionary<int, List<Tile>>();
    private Dictionary<int, List<Tile>> tilesByProvince = new Dictionary<int, List<Tile>>();


    public List<Tile> GetTileByProvince(int provinceId)
    {
        return tilesByProvince.TryGetValue(provinceId, out var list) ? list : new List<Tile>();
    }
    public List<Tile> GetTileByCountry(int countryId)
    {
        return tilesByCountry.TryGetValue(countryId, out var list) ? list : new List<Tile>();
    }

    public void AddTile(Tile tile, ProvinceRegistery _pr)
    {
        allTiles.Add(tile);
        int countryId = _pr.GetProvinceById(tile.province).ownerId;
        AddToBucket(tilesByCountry, countryId, tile);
    }
    public void RemoveTile(Tile tile, ProvinceRegistery _pr)
    {
        allTiles.Remove(tile);
        int countryId = _pr.GetProvinceById(tile.province).ownerId;
        RemoveFromBucket(tilesByCountry, countryId, tile);
    }

    // ---  ---

    private void AddToBucket(Dictionary<int, List<Tile>> dict, int key, Tile tile)
    {
        if (!dict.ContainsKey(key))
        {
            dict[key] = new List<Tile>();
        }
        dict[key].Add(tile);
    }
    private void RemoveFromBucket(Dictionary<int, List<Tile>> dict, int key, Tile tile)
    {
        if (dict.ContainsKey(key))
        {
            dict[key].Remove(tile);
        }
    }
}
