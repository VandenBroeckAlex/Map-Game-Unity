using Newtonsoft.Json;
using PlasticPipe.PlasticProtocol.Messages;

public class ClimateTypeLoader
{
    public class ClimateType
    {
        public string name;
        public string tag;
    }
    public class ClimateTypeData
    {
        public ClimateType[] climateTypes;
        public string[] climateTypesTags;
    }

    public ClimateTypeData deserializeClimateType(string json)
    {
        DTOClimateDef[] climateDTO  = JsonConvert.DeserializeObject<DTOClimateDef[]>(json);
        string[] tags = new string[climateDTO.Length];
        ClimateType[] climateTypes = new ClimateType[climateDTO.Length];

        int indexer = 0;
        foreach(DTOClimateDef def in climateDTO)
        {
            ClimateType ct = new ClimateType();
            ct.name = def.name;
            ct.tag = def.tag;

            climateTypes[indexer] = ct;
            tags[indexer] = def.tag;
            indexer++;
        }
        ClimateTypeData result = new ClimateTypeData();
        result.climateTypes = climateTypes;
        result.climateTypesTags = tags;
        return result;
    }
}
