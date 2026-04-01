using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UnitGraphState
{
    public class Unit
    {
        public int unitId;
        public int currentID;
        public int targetId;
        public int distanceLeft;
    }

    public Dictionary<int, List<Unit>> unitGameState;
}
