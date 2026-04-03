using Newtonsoft.Json;
using System.IO;
using static ColorUtilities;

public class GraphDataLoder
{
    public void Load(LoaderDataRegistery _registery, IResolutionErrorHandler _errorHandler)
    {
       string filePath = GamePaths.GetTileData("TilesGraph");

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            DTOGraphData[] dtoGraphData = JsonConvert.DeserializeObject<DTOGraphData[]>(jsonText);
        
            //resolve hex to id
            foreach(DTOGraphData nodeData in dtoGraphData)
            {
                int id = HexToInt(nodeData.id);
                int terrainType = _registery.tiles[id].type;

                MapGraphNode mpg = new MapGraphNode(
                    id,
                    terrainType,
                    new System.Numerics.Vector2(nodeData.pivot[0], nodeData.pivot[1])
                    ); 
            }
            //id resolution pass
        }
    }
}
