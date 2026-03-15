
using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;

public class ProvincesLoader
{
    public class ProvinceData
    {
        public List<Province> provincesList;
        public string[] provinceTag;
    }

    public ProvinceData LoadProvince(string jsonText, string[] countryTagId)
    {
        int idIterator = 0;
        List<DTOProvince> ProvinceDataList = JsonConvert.DeserializeObject<List<DTOProvince>>(jsonText);
        
        List<Province> provincesList = new List<Province>();
        string[] provinceTagId = new string[ProvinceDataList.Count];

        foreach (DTOProvince provinceData in ProvinceDataList)
        {
            Province province = new Province();
            province.id = idIterator;
            province.tag = provinceData.tag;
            provinceTagId[idIterator] =  province.tag;

            province.name = provinceData.name;
            province.isOccupied = provinceData.isOccupied;

            int countryId = GetIdByTag(provinceData.ownerTag, countryTagId);
            if(countryId == -1)
            {
                throw new InvalidDataException(
               $"Unknown country tag '{provinceData.ownerTag}' while creating province '{provinceData.name}'.");
            }

            province.ownerId = countryId;
            provincesList.Add(province);
            idIterator++;
        }
        ProvinceData result = new ProvinceData();

        result.provincesList = provincesList;
        result.provinceTag = provinceTagId;
        return result;
    }
    private int GetIdByTag(string givenTag, string[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            if (data[i] == givenTag)
            {
                return i;
            }
        }
        return -1;
    }
}
