using System.Collections.Generic;
using UnityEngine;

public class PopBuilder
{
    public int id { get; set; }
    public int size = 1;
    public List<IdNum> workplaces = new List<IdNum>();
    public int provinceId { get; set; } = 0;
    public int countryID { get; set; } = 0;
    public int jobId { get; set; } = 0;
    public int cultureId { get; set; } = 0;
    public int religionId { get; set; } = 0;
    private int _cashAmount = 0;
    private int _savings;
    public List<GoodRequirement> goodRequirement = new List<GoodRequirement>();
    public int cashAmount
    {
        get { return _cashAmount; }
        set { _cashAmount = value; }
    }

    public PopBuilder WithId(int id)
    {
        this.id = id;return this;
    }

    public PopBuilder WithSize(int size) {
        this.size = size;return this;
    }

    public PopBuilder WithWorkplace(int workplaceID, int ammount)
    {
        IdNum idNum = new IdNum(workplaceID, ammount);
        workplaces.Add(idNum);
        return this;
    }

    public PopBuilder WithProvince(int provinceID)
    {
        this.provinceId = provinceID;return this;
    }

    public PopBuilder WithCountry(int countryID)
    {
        this.countryID = countryID;return this;
    }

    public PopBuilder WithJobId(int id)
    {
        this.jobId = id;return this;
    }

    public PopBuilder WithCulture(int cultureID)
    {
        this.cultureId = cultureID;return this;
    }

    public PopBuilder WithReligion(int religionID)
    {
        this.religionId = religionID;return this;
    }

    public PopBuilder WithCashAmmount(int cashAmmount)
    {
        this.cashAmount = cashAmmount;return this;
    }

    public PopBuilder WithGoodRequirement(int goodId, int stockpile, int max)
    {
        GoodRequirement gr = new GoodRequirement(goodId,stockpile, max);
        goodRequirement.Add(gr);
        return this;
    }

    public Pop Build()
    {
        Pop pop = new Pop(this.id,this.size,this.provinceId,this.jobId,this.cultureId,this.religionId,this._cashAmount,this.goodRequirement,this.workplaces);
        return pop;
    }
}

public class TempGoodRequirment
{
    public string tag;
    public int stockpile;
    public int max;
    public TempGoodRequirment(string tag, int stockpile, int max)
    {
        this.tag = tag;
        this.stockpile = stockpile;
        this.max = max;
    }
}
public class PopBuilderTag
{
    public int id { get; set; }
    public int size = 1;
    public List<IdNum> workplaces = new List<IdNum>();
    public string provinceTag { get; set; } = "Default";
    public string countryTag { get; set; } = "Default";
    public string jobTag { get; set; } = "Default";
    public string cultureTag { get; set; } = "Default";
    public string religionTag { get; set; } = "Default";
    private int _cashAmount = 0;
    private int _savings;
    public List<TempGoodRequirment> goodRequirement = new List<TempGoodRequirment>();
    public int cashAmount
    {
        get { return _cashAmount; }
        set { _cashAmount = value; }
    }

    public PopBuilderTag WithId(int id)
    {
        this.id = id; return this;
    }

    public PopBuilderTag WithSize(int size)
    {
        this.size = size; return this;
    }

    public PopBuilderTag WithWorkplaces(List<IdNum> workplaces)
    {
        IdNum idNum = new IdNum(workplaceID, ammount);
        workplaces.Add(idNum);
        return this;
    }

    public PopBuilderTag WithProvince(string provinceTag)
    {
        this.provinceTag = provinceTag; return this;
    }

    public PopBuilderTag WithCountry(string countryTag)
    {
        this.countryTag = countryTag; return this;
    }

    public PopBuilderTag WithJobTag(string tag)
    {
        this.jobTag = tag; return this;
    }

    public PopBuilderTag WithCulture(string cultureTag)
    {
        this.cultureTag = cultureTag; return this;
    }

    public PopBuilderTag WithReligion(string religionTag)
    {
        this.religionTag = religionTag; return this;
    }

    public PopBuilderTag WithCashAmmount(int cashAmmount)
    {
        this.cashAmount = cashAmmount; return this;
    }

    public PopBuilderTag WithGoodRequirement(string goodTag, int stockpile, int max)
    {
        TempGoodRequirment gr = new TempGoodRequirment(goodTag, stockpile, max);
        goodRequirement.Add(gr);
        return this;
    }


    public Pop Build(DataRegistery _registery, IResolutionErrorHandler _errorHandler)
    {
        int provinceId = _registery.GetProvinceID(this.provinceTag);
        if (provinceId < 0) _errorHandler.HandleMissingId(
            $"Unknown province tag '{this.provinceTag}' while creating population (province : ${this.provinceTag}, job : ${this.jobTag} , culture : ${this.cultureTag},religion : ${this.religionTag}).");
        int jobId = _registery.GetPopJobId(this.jobTag);
        if (jobId < 0) _errorHandler.HandleMissingId(
            $"Unknown job tag '{this.provinceTag}' while creating population (province : ${this.provinceTag}, job : ${this.jobTag} , culture : ${this.cultureTag},religion : ${this.religionTag}).");
        int cultureId = _registery.GetCultureId(this.cultureTag);
        if (cultureId < 0) _errorHandler.HandleMissingId(
            $"Unknown culture tag '{this.provinceTag}' while creating population (province : ${this.provinceTag}, job : ${this.jobTag} , culture : ${this.cultureTag},religion : ${this.religionTag}).");
        int religionId = _registery.GetReligionId(this.religionTag);
        if (religionId < 0) _errorHandler.HandleMissingId(
            $"Unknown religion tag '{this.provinceTag}' while creating population (province : ${this.provinceTag}, job : ${this.jobTag} , culture : ${this.cultureTag},religion : ${this.religionTag}).");

        List<GoodRequirement> goodreqList = new List<GoodRequirement>();

        foreach (TempGoodRequirment tgr in goodRequirement) 
        {
            int goodId = _registery.GetGoodIdByTagId(tgr.tag);
            if (goodId < 0) _errorHandler.HandleMissingId(
            $"Unknown good tag '{tgr.tag}' while creating population  while creating population (province : ${this.provinceTag}, job : ${this.jobTag} , culture : ${this.cultureTag},religion : ${this.religionTag}).");
            GoodRequirement gr = new GoodRequirement(goodId, tgr.stockpile, tgr.max);
            goodreqList.Add(gr);
        }



        Pop pop = new Pop(this.id, this.size, provinceId, jobId, cultureId, religionId, this._cashAmount, goodreqList, this.workplaces);
        return pop;
    }
}
