
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

public class ProvincesLoader
{
   

    public List<Province> LoadProvince(string jsonText, Dictionary<string, int> countryTagId)
    {
        int idIterator = 0;
        List<DTOProvince> ProvinceDataList = JsonConvert.DeserializeObject<List<DTOProvince>>(jsonText);
        List<Province> provincesList = new List<Province>();
        Dictionary<string, int> provinceTagId = new Dictionary<string, int>();

        foreach (DTOProvince provinceData in ProvinceDataList)
        {
            Province province = new Province();
            province.id = idIterator;
            province.tag = provinceData.tag;
            provinceTagId[province.tag] = idIterator;

            province.name = provinceData.name;
            province.isOccupied = provinceData.isOccupied;
            
            if (countryTagId.TryGetValue(provinceData.ownerTag, out int value))
            {
                int countryId = value;
            }
            else
            {
                throw new InvalidDataException(
               $"Unknown country tag '{provinceData.ownerTag}' while creating province '{provinceData.name}'.");
            }
            provincesList.Add(province);
            idIterator++;
        }
        return provincesList;
    }
}
