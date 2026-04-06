using System.Collections.Generic;
using UnityEngine;

public class PopBuilder
{
    public int id { get; set; }
    public int size = 1;
    public List<IdNum> workplaces = new List<IdNum>();
    public int provinceId { get; set; } = 0;
    public int countryID { get; set; } = 0;
    public int jobId { get; } = 0;
    public int cultureId { get; set; } = 0;
    public int religionId { get; set; } = 0;
    private int _cashAmount = 0;
    private int _savings;
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

    public Pop Build()
    {
        Pop pop = new Pop(this.id,this.size,this.provinceId,this.jobId,this.cultureId,this.religionId,this._cashAmount,this.workplaces);
        return pop;
    }
}
