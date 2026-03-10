using Newtonsoft.Json;

public class StrataLoader
{
    public string[] DeserializeStrata(string json)
    {
       string[] result = JsonConvert.DeserializeObject<string[]>(json);
        return result;
    }
}
