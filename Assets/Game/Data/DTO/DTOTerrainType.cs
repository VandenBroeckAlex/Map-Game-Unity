using UnityEngine;

public class DTOTerrainType
{
    public string tag;
    public string name;
    public bool isLandType;
    public int movementCost;
    //icon/img
    //list moddifiers

    /*
     Eco:
        construction price multi
        pop consumption multy
        
        admin power consumption

     Distance multiplier

     */
}

public class DTOLandTerrainType : DTOTerrainType
{
    //unit and Military
    public int combatWidth;
    public int attrition;

    public int BuildingCostMult;
    //icon/img
    //list moddifiers

    /*
     Eco:
        construction price multi
        pop consumption multy
        
        admin power consumption

     Distance multiplier

     */
}