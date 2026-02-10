using NUnit.Framework;
using System.Collections.Generic;

public class PopulationSystem
{
    HandleGet getter = new HandleGet();
    IIntentBuffer _intent;

    private readonly List<Pop> _popList;
    public PopulationSystem(IntentBuffer intent, List<Pop> popList)
    {
        _intent = intent;
        _popList = popList;
    }
}