using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class IntCurentMax
    {
        public int current;
        public int max;
    [SerializeField]
    public IntCurentMax(int current, int max)
        {
            this.current = current;
            this.max = max;
        }
    }

