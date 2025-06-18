using System;
using UnityEngine;

namespace MyGame.Data
{
    [Serializable]
    public class Province
    {
        public string name;
        public string description;
        public int id;
        public Type_province type;
        public bool isLand;
        public bool isPassable;
        public int ownerId;
        public int[] neighbors;
        public int owner;
        public int occupierID;
        public Type_rgo rgo;
        public Province(int id, string name, string description, int owner, int[] neighbors)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.owner = owner;
            this.neighbors = neighbors;

        }
        public Province() { }

        public Province(int givenID, string givenName, string givenDescription, Type_province givenType, int givenOwner, int[] givenneighbors)
        {
            id = givenID;
            description = givenDescription;
            name = givenName;
            type = givenType;
            ownerId = givenOwner;
            neighbors = givenneighbors;
        }
    }
    public class WaterTile
    {
        public string name;
        public string description;
        public int id;
        public int type;
        public bool isLand;
        public bool isPassable;
        public int[] neighbors;
        public WaterTile(int id, string name, string description, int[] neighbors)
        {
            this.id = id;
            this.name = name;
            this.description = description;

            this.neighbors = neighbors;

        }
        public WaterTile() { }

        public WaterTile(int givenID, string givenName, string givenDescription, int givenType, int[] givenneighbors)
        {
            id = givenID;
            description = givenDescription;
            name = givenName;
            type = givenType;

            neighbors = givenneighbors;
        }
    }

    public enum Type_rgo
    {
        coal,
        cattle
    }
    public enum Type_province
    {
        plain,
        forest
    }

    //tochange !
    public enum Country
    {
        France,
        Germany
    }

}