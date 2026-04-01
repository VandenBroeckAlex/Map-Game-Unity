using System.Collections.Generic;


public class MapGraph
{
    //make it an array
    private List<MapGraphNode> map;

    public MapGraph (List<MapGraphNode> map)
    {
        this.map = map;
    }

    public IList<MapGraphNode> mapNodes
    {
        get 
        { 
            return map.AsReadOnly ();
        }

    }

}
