using Newtonsoft.Json;
using System.IO;


public class GraphDataLoder
{
    public void Load(LoaderDataRegistery _registery, IResolutionErrorHandler _errorHandler)
    {
       string filePath = GamePaths.GetTileData("TilesGraph");

        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            DTOGraphData[] dtoGraphData = JsonConvert.DeserializeObject<DTOGraphData[]>(jsonText);
        
            // resolve hex to id
            // resolve neighbore hex to id 
        }
    }
}
