using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test_color : MonoBehaviour
{
    
    public Renderer ren;
    public Material[] mat;

    public Material example;

    void Start()
    {
        ren = GetComponent<Renderer>();
        mat = ren.materials;
        mat[0] = example;
        mat[1] = example;
        mat[8] = example;

        ren.materials = mat;
    }
}
