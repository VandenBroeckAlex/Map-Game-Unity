
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

public class ProvincesLoader
{
    private struct ProvinceData
    {
        public string tag;
        public string ownerTag;
        public string name;
        public bool isOccupied;
        public string occupierTag;
        //Moddifiers
    }


    public List<Province> LoadProvince(string jsonText, Dictionary<string, int> countryTagId)
    {
        int idIterator = 0;
        List<ProvinceData> ProvinceDataList = JsonConvert.DeserializeObject<List<ProvinceData>>(jsonText);
        List<Province> provincesList = new List<Province>();
        Dictionary<string, int> provinceTagId = new Dictionary<string, int>();

        foreach (ProvinceData provinceData in ProvinceDataList)
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
               $"Unknown job tag '{provinceData.ownerTag}' while creating pop '{provinceData.name}'.");
            }
            provincesList.Add(province);
            idIterator++;
        }
        return provincesList;
    }
}
