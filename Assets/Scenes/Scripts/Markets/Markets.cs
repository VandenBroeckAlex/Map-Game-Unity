using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Goods;

public class Markets 
{
    public class Market
    {
        public Dictionary<Good, float> supply = new();
        public Dictionary<Good, float> demand = new();
        public Dictionary<Good, float> prices = new();
    }
}

/*
1 Production Phase:

    Buildings produce goods.

    Output goes into the global (or local) market.

    Needs Assessment Phase:

    Pops calculate their demand based on size.

    Wealth limits what they can actually buy.

2 Trade Phase:

    Goods are matched with demand.

    Prices adjust.

    Wealth is transferred from consumers to producers.

4 Population Update Phase:

    Unmet needs lower happiness.

    Overconsumption can raise class mobility or birthrate.

    Wealth growth/shrinkage is tracked.
 */