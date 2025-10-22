using System;
using System.Collections.Generic;
using UnityEngine;

namespace MyGame.Data
{
    //base tile

    //make land and water inherit from it
    [SerializeField]
    public class Tile
    {
        public string name;
        public int id;
        public bool isLand;
        public bool isPassable;
        public List<int> neighbors;     // (?)
        
        public Tile(int GivenID, string GivenName,List<int> Givenneighbors)
        {
            id = GivenID;
            name = GivenName;     
            neighbors = Givenneighbors;
        }

    }
    public class WaterTile : Tile
    {
        public int type;

        public WaterTile(int givenID, string givenName, string givenDescription, int givenType, List<int> givenneighbors)
            : base(givenID, givenName, givenneighbors)
        {
            type = givenType;
            isLand = false;
            isPassable = true;
        }
    }

    public class LandTile : Tile
    {
        public int ownerId;
        public int occupierID;
        public int rgo;
        public int type;
        public bool isCoast;
        public LandTile(int givenID, string givenName,int givenType, List<int> givenneighbors)
            : base(givenID, givenName, givenneighbors)
        {
            isLand = true;
            isPassable = true;
            id = givenID;
            name = givenName;
            type = givenType;
            neighbors = givenneighbors;
        }
    }



    //load it from resource json
    // if type == Raw than good is valid rgo
    public enum Type_rgo
    {
        coal,
        cattle
    }
    //load it from province type json
    public enum Type_province
    {
        plain,
        forest
    }

    //load it from country json. It's not suppose to be here ! bruh
    // check for country in country manager
    public enum Country
    {
        France,
        Germany
    }

}