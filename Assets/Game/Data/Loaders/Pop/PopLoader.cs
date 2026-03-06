
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

public class PopLoader
{
    public List<Pop> Deserialize_Pop(string json,
        Dictionary<string, int> popJobTagId,
        Dictionary<string, int> culturesTagId,
        Dictionary<string, int> religionsTagId,
        Dictionary<string, int> goodTagID
        )
    {
        List<Pop> result = new List<Pop> ();
        DTOPopulation[] data = JsonConvert.DeserializeObject<DTOPopulation[]>(json);

        foreach (DTOPopulation op in data) 
        {
            if(!popJobTagId.TryGetValue(op.job, out int jobId))
            {
                throw new InvalidDataException(
                $"Unknown job tag '{op.job}' while creating pop '{op.id}'.");
            }
           if(!culturesTagId.TryGetValue(op.culture, out int cultureId))
           {
                throw new InvalidDataException(
                $"Unknown culture tag '{op.culture}' while creating pop '{op.id}'.");
           }
            if (!religionsTagId.TryGetValue(op.religion, out int religionId))
            {
                throw new InvalidDataException(
                $"Unknown culture tag '{op.culture}' while creating pop '{op.id}'.");
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
}
