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
        public int type;
        public bool isLand;
        public bool isPassable;
        public int ownerId;
        public int[] neighbors;
        public int owner;
        public int occupierID;
        public int rgo;
        public Province(int id, string name, string description, int owner, int[] neighbors)
        {
            this.id = id;
            this.name = name;
            this.description = description;
            this.owner = owner;
            this.neighbors = neighbors;

        }
        public Province() { }

        public Province(int givenID, string givenName, string givenDescription, int givenType, int givenOwner, int[] givenneighbors)
        {
            id = givenID;
            description = givenDescription;
            name = givenName;
            type = givenType;
            ownerId = givenOwner;
            neighbors = givenneighbors;
        }
    }
}