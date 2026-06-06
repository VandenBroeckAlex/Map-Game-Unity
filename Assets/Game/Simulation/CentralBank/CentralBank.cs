/*
Can loan to bank in last resort (panic) if is solvent
 at a very high interest rate

Can loan to country with interest
 */

using System.Collections.Generic;

public class CentralBank
{
    private int countryId;

    public string name;
    
    private int goldReserve = 0;

    byte localMoneyParity;
    byte reserveRate;

    public int localCurencyVolume; // country pop workplace bank

    List<Account> accounts;


    public void AddGoldReserve(int ammount)
    {
        this.goldReserve += ammount;
    }

    public int GetGoldReserve()
    {
        return this.goldReserve;
    }
}
