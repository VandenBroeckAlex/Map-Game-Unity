using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLinePoints : MonoBehaviour
{
    // Start is called before the first frame update

    Vector3[] linePoints = new Vector3[] { new (0.0f, 0.0f, 0.0f), new(1f,1f,1f)  };
    
    


    void Start()
    {
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        int lineIndex = linePoints.Length;        
        lineRenderer.useWorldSpace = true;
        lineRenderer.SetPositions( linePoints);
        
    }

    
        
    
}
