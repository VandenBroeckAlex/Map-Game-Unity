using Newtonsoft.Json;
using System.Collections.Generic;

public class ReligionLoader
{
  

    public Religion[] DeserializeReligions(string json)
    {

        Religion[] religionsData = JsonConvert.DeserializeObject<Religion[]>(json);

        return religionsData;
    }
}
