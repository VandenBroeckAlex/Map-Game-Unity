using System.Collections.Generic;

public class HandleGet
{
    public int GetProvinceIdByColor(int[] givenColor, Dictionary<int, Tile> allProvinces, Dictionary<int, int[]> colorIDList)
    {
        foreach (var kvp in colorIDList)
        {

            int iDListRed = kvp.Value[0];
            int iDListGreen = kvp.Value[1];
            int iDListBlue = kvp.Value[2];

            int givenColorRed = givenColor[0];
            int givenColorGreen = givenColor[1];
            int givenColorBlue = givenColor[2];

            if (givenColorRed == iDListRed && givenColorGreen == iDListGreen && givenColorBlue == iDListBlue)
            {
                return kvp.Key; // Found a match
            }
        }
        return -1;
    }
    Tile GetProvinceById(int id, Dictionary<int, Tile> provinces_list)
    {
        return provinces_list[id];
    }


    public int GetProvinceOwnerByProvinceId(int id, Dictionary<int, Tile> provinces_list)
    {
        Tile province = GetProvinceById(id,provinces_list);

        if (province == null || province.isLand is false)
        {
            return -1;
        }
        else
        {
            LandTile Lprovince = (LandTile)province;
            return Lprovince.ownerId;
        }
    }

}