/*
Can loan to bank in last resort (panic) if is solvent
 at a very high interest rate

Can loan to country with interest
 */

public class CentralBank
{
    private int countryId;

    public string name;
    
    private int goldReserve;

    byte localMoneyParity;
    byte reserveRate;

    public int localCurencyVolume; // country pop workplace bank

    /*
     variable for traking confidance in the market
     */

    /*
    classic Bank :
    
    localMoneyReserve
    issuedDebtAmmount
    List<account>

    account :
    ownerType : pop,country,workplace
    ownerId
    reserve
    list<Debt>
    getDebtAmount

    Debt:
    ownerType
    ownerId
    debtorId
    ammountToRepay
    CurrencyType (gold for foreign entity, localCurency)

    */
}
