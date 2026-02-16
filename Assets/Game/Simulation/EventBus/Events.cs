using System.Collections.Generic;
using UnityEngine;


public class Events 
{


    public struct PlayerEvent : IEvent 
    {
        public int marketId;
        public int popId;
        public int goodId;
    }
   
}
