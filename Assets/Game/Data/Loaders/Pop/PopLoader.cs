
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using static CultureLoader;

public class PopLoader
{
    public List<Pop> Deserialize_Pop(string json,
        RunTimePopJob[] popJobTagId,
        RunTimeCulture[] culturesTagId,
        ReligionLoader.RunTimeReligion[] religionsTagId,
        string[] goodTagID
        )
    {
        List<Pop> result = new List<Pop> ();
        DTOPopulation[] data = JsonConvert.DeserializeObject<DTOPopulation[]>(json);

        foreach (DTOPopulation op in data) 
        {
            int jobId = GetIdByTag(popJobTagId, op.job);
            int cultureId = GetIdByTag(culturesTagId, op.job);
            int religionId = GetIdByTag(religionsTagId, op.job);
            if (jobId == -1)
            {
                throw new InvalidDataException(
                $"Unknown job tag '{op.job}' while creating pop '{op.id}'.");
            }

           if(cultureId == -1)
           {
                throw new InvalidDataException(
                $"Unknown culture tag '{op.culture}' while creating pop '{op.id}'.");
           }
            
            if (religionId == -1)
            {
                throw new InvalidDataException(
                $"Unknown religion tag '{op.religion}' while creating pop '{op.id}'.");
            }

          


            GoodRequirement gr = new GoodRequirement(0,0,10);
            List<GoodRequirement> grList = new List<GoodRequirement>();

            grList.Add( gr );
            
            Pop pop = new Pop(
                op.id,
                op.size,
                op.provinceId,
                jobId,
                cultureId,
                religionId,
                op._cashAmount,
                grList
                );
        }

        return result;
    }

    private int GetIdByTag<T>(T[] data, string givenTag) where T : IHaveTag
    {
        for (int i = 0; i < data.Length; i++) 
        { 
            if(data[i].tag == givenTag)
            {
                return i;
            }
        }
        return -1;
    }

    private int GetIdByString(string str, string[] array, int popId)
    {
        for (int i = 0; i < array.Length; i++)
        {
            if (array[i] == str)
            {
                return i;
            }
        }
        throw new InvalidDataException($"Unknown good tag '{str}' while creating pop '{popId}'.");
    }
}
