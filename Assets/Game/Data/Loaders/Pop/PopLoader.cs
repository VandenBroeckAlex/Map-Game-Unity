
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static CultureLoader;

public class PopLoader
{
    public List<Pop> Deserialize_Pop(string json,
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
            PopBuilderTag builder = new PopBuilderTag ();

            builder.WithId(indexer)
            .WithSize(op.size)
            .WithWorkplaces(op.workplace)
            .WithProvince(op.provinceTag)
            .WithCountry(op.countryTag)
            .WithJobTag(op.job)
            .WithCulture(op.culture)
            .WithReligion(op.religion)
            .WithCashAmmount(op.cashAmount);

            //.WithGoodRequirement()
            foreach(DTOGoodRequirement gr in op.GoodRequirement)
            {
                builder.WithGoodRequirement(gr.good, gr.stockpile, gr.MaxNeed);
            }
            indexer++;

            result.Add(builder.Build(_registery,_errorHandler));
        }
        return result;
    }

}
