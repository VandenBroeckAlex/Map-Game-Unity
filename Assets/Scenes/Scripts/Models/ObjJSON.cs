using MyGame.Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjJSON 
{

        public int id;
        public float[] spriteColor;
        public int lowerX;
        public int higherY;
        public float[] center;
        public string name = "";
        public int superficy;
        public int type = 0;
        public int ownerId;
        public int occupierID;
        public List<int> neighbors;   
        public bool isLand = true;
        public bool isPassable = true;
        public int owner;
        public int rgo;

    public ObjJSON(float[] color, int x, int y, int id, List<int> neighbors, float[] center, int superficy)
        {
            this.id = id;
            this.spriteColor = color;
            this.lowerX = x;
            this.higherY = y;
            this.neighbors = neighbors;
            this.center = center;
            this.superficy = superficy;
        }
    
}

