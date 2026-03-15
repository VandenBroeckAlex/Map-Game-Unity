using Newtonsoft.Json;


public class TerrainTypeLoader
{
    public class TerrainType
    {
        public string name;
        public string tag;
        public bool isLandType;
        //moddifier
    }
    public class TerrainTypesData
    {
        public TerrainType[] terrainTypes;
        public string[] tags;
    }

    public TerrainTypesData DeserializeTerrainTypeDef(string json)
    {
        DTOTerrainType[] terrainTypeDTO = JsonConvert.DeserializeObject<DTOTerrainType[]>(json);

        TerrainType[] terrainTypes = new TerrainType[terrainTypeDTO.Length];
        string[] tags = new string[terrainTypeDTO.Length];

        int indexer = 0;
        foreach(DTOTerrainType dto in terrainTypeDTO)
        {
            TerrainType tt = new TerrainType();
            tt.name = dto.name;
            tt.tag = dto.tag;
            tt.isLandType = dto.isLandType;

            terrainTypes[indexer] = tt;
            tags[indexer] = tt.tag;
            indexer++;
        }
        TerrainTypesData result = new TerrainTypesData();

        result.terrainTypes = terrainTypes;
        result.tags = tags;
        return result;
    }
}
