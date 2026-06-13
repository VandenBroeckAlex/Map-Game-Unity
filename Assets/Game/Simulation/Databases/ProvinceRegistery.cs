using System.Collections.Generic;
using System.Linq;

public class ProvinceRegistery
{
    private List<Province> allProvince = new List<Province>();
    private Dictionary<int, List<Province>> provinceByCountry = new Dictionary<int, List<Province>>();

    public List<Province> GetProvinceInCountry(int countryId)
    {
        return provinceByCountry.TryGetValue(countryId, out var list) ? list : new List<Province>();
    }
    public void CountryChange(Province province,int countryId)
    {
        RemoveFromBucket(provinceByCountry, province.ownerId, province);
        province.ownerId = countryId;
        AddToBucket(provinceByCountry, countryId, province);
    }

    public Province GetProvinceById(int id)
    {
        Province province = allProvince.Where(p => p.id == id).FirstOrDefault();
        return province;
    }

    // ---  ---
    private void AddToBucket(Dictionary<int, List<Province>> dict, int key, Province province)
    {
        if (!dict.ContainsKey(key))
        {
            dict[key] = new List<Province>();
        }
        dict[key].Add(province);
    }
    private void RemoveFromBucket(Dictionary<int, List<Province>> dict, int key, Province province)
    {
        if (dict.ContainsKey(key))
        {
            dict[key].Remove(province);
        }
    }
}
