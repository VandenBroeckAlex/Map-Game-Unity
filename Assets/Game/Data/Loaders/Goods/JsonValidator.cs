using System;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


public class JsonValidator
{

    HashSet<string> validKeys = new HashSet<string>
    {
        "name",
        "tag",
        "basePrice",
        "baseProductionModdifier",
        "type",
        "color",
        "iconPath",
        "isRGO"
    };

    //Get json path for error message
    public bool ValidateGoods(string jsonText, HashSet<string> validTypes)
    {
        bool jsonIsValid = true;

        JArray root = JArray.Parse(jsonText);

        foreach (JObject item in root)
        {
            foreach (var property in item.Properties())
            {
                if (!validKeys.Contains(property.Name))
                {
                    int line = ((IJsonLineInfo)property).LineNumber;
                    string suggestion = FindClosest(property.Name, validKeys);

                    Debug.LogError(
                        $"Line {line}: Unknown key '{property.Name}'. " +
                        (suggestion != null ? $"Did you mean '{suggestion}'?" : "")
                    );

                    jsonIsValid = false;
                }
            }


            JToken typeToken = item["type"];
            if (typeToken != null)
            {
                string typeValue = typeToken.Value<string>();

                if (!validTypes.Contains(typeValue))
                {
                    int line = ((IJsonLineInfo)typeToken).LineNumber;
                    string suggestion = FindClosest(typeValue, validTypes);

                    Debug.LogError(
                        $"Line {line}: Type '{typeValue}' is invalid. " +
                        (suggestion != null ? $"Did you mean '{suggestion}'?" : "")
                    );

                    jsonIsValid = false;
                }
            }
        }

        return jsonIsValid;
    }



    string FindClosest(string input, IEnumerable<string> options)
    {
        int bestDistance = int.MaxValue;
        string bestMatch = null;

        foreach (var option in options)
        {
            int distance = StringDistance.Levenshtein(input, option);
            if (distance < bestDistance && distance <= 3)
            {
                bestDistance = distance;
                bestMatch = option;
            }
        }
        return bestMatch;
    }
}
