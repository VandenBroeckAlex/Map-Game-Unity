
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CultureLoader;

public class PopLoader
{
    public List<Pop> Deserialize_Pop(string json,
        PopJob[] popJobTagId,
        Culture[] culturesTagId,
        Religion[] religionsTagId,
        string[] goodTagID,
        List<Province> listProvince,
        DataRegistery _registery,
        IResolutionErrorHandler _errorHandler
        )
    {
        
        List<Pop> result = new List<Pop> ();
        DTOPopulation[] data = JsonConvert.DeserializeObject<DTOPopulation[]>(json);

        if(data is null || data.Length == 0)
        {
            throw new InvalidDataException("Population.json is empty");
        }

        // check for pop that share religion culture type and living place
        int indexer = 0;
        foreach (DTOPopulation op in data) 
        {
            int jobId = GetIdByTag(popJobTagId, op.job);
            int cultureId = GetIdByTag(culturesTagId, op.culture);
            int religionId = GetIdByTag(religionsTagId, op.religion);
            if (jobId == -1)
            {
                throw new InvalidDataException(
                $"Unknown job tag '{op.job}' while creating pop '{op.job}', culture '{op.culture}', religion '{op.religion}' in {op.provinceTag}.");
            }

           if(cultureId == -1)
           {
                throw new InvalidDataException(
                $"Unknown culture tag '{op.culture}' while creating pop '{op.job}', culture '{op.culture}', religion '{op.religion}' in {op.provinceTag}.");
           }
            
            if (religionId == -1)
            {
                throw new InvalidDataException(
                $"Unknown religion tag '{op.religion}'while creating pop '{op.job}', culture '{op.culture}', religion '{op.religion}' in {op.provinceTag}.");
            }


            IdNum gr = new IdNum(0,10);
            List<IdNum> grList = new List<IdNum>();

            grList.Add( gr );

            Province province= listProvince.Where(p => p.tag == op.provinceTag).FirstOrDefault();

            if (province == null) 
            { 
                throw new InvalidDataException($"Can't find province tag {op.provinceTag} while creating population(province: {op.provinceTag},type: {op.job},culture: {op.culture},religion: {op.religion})");
            }

            int _provinceTag = province.id;


            Pop pop = new Pop(
                indexer,
                op.size,
                _provinceTag,
                jobId,
                cultureId,
                religionId,
                op._cashAmount,
                grList
                );
            indexer++;
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
