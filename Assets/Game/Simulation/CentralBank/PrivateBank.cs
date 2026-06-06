

using Codice.CM.Common;
using System.Collections.Generic;

public class PrivateBank
{
    int localMoneyReserve;
    int issuedDebtAmmount;
    List<Account> accounts;

    public int GetIssueDebtAmmount()
    {
        issuedDebtAmmount = 0;

        foreach (Account account in accounts)
        {
            issuedDebtAmmount += account.getDebtAmount();
        }
        return issuedDebtAmmount;
    }

}

public class Account 
{
    OwnerType ownerType;
    int ownerId;
    int reserve;
    List<Debt> debts = new List<Debt>();
    public int getDebtAmount()
    {
        int amount = 0;
        foreach (Debt debt in debts)
        {
            amount += debt.ammountToRepay;
        }
        return amount;
    }
}

/*Workplace and bank keep track of the debt*/
public class Debt
{
    public OwnerType ownerType;
    public int ownerId;
    public int debtorId;
    public int ammountToRepay;
    public CurencyType curencyType;
}


public enum OwnerType
{
    pop, 
    country, 
    workplace
}

public enum CurencyType
{
    gold,
    localCurency
}