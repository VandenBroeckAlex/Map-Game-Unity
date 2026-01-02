using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AcessPoint : MonoBehaviour
{
    Mesh mesh;
    Vector3[] vertices;

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        List<Vector3> topVertices = new List<Vector3>();
        vertices = mesh.vertices;


        Debug.Log("il y a " + vertices.Length + " vertices dans la liste de  départ");

        vertices = PointsToWorldPosition(vertices);

        topVertices = HighestPointsY(vertices);

        int c = topVertices.Count;

        Debug.Log("il y a " + topVertices.Count + " vertices dans la list top Y");

        for (int i = 0; i < c; i++)
        {
            Debug.Log("list top Y pt" + i + " : " + topVertices[i]);
        }

        topVertices = RemoveRepeatedPoints(topVertices);

        Debug.Log("il y a " + topVertices.Count + " vertices dans la list curred  Y");

        for (int i = 0; i < topVertices.Count; i++)
        {
            Debug.Log("list top curred Y pt" + i + " : " + topVertices[i]);

        }

        Vector3[] LinePoints = topVertices.ToArray();
        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();
        
        lineRenderer.positionCount = topVertices.Count;
        lineRenderer.useWorldSpace = true;
        lineRenderer.loop = true;
        lineRenderer.alignment = LineAlignment.TransformZ;
        lineRenderer.SetPositions(LinePoints);


    }


    Vector3[] PointsToWorldPosition(Vector3[] vertices)
    {
        int b = vertices.Length;
        for (int i = 0; i < b; i++)
        {
            vertices[i] = transform.TransformPoint(vertices[i]);

        }
        return vertices;
    }

    List<Vector3> HighestPointsY(Vector3[] vertices)
    {
        // keep only Pt with highest Y
        float flag = 0;        
        List<Vector3> topVertices = new List<Vector3>();

        for (int i = 0; i < vertices.Length; i++)
        {
            if (vertices[i].y >= flag)
            {
                if (vertices[i].y > flag)
                {
                    flag = vertices[i].y;
                    topVertices.Clear();
                }

                topVertices.Add(vertices[i]);
            }

        }
        return topVertices;
    }

    List<Vector3> RemoveRepeatedPoints(List<Vector3> vertices)
    {
        int b = vertices.Count;
        List<Vector3> curredPointList = new List<Vector3>();

        curredPointList.Add(vertices[0]);

        for (int i = 1; i < b; i++)
        {
            bool isThere = false;


            for (int j = 0; j < curredPointList.Count; j++)
            {
                if (vertices[i] == curredPointList[j]) {

                    isThere = true;
                    break; 
                }                

            }

            if (isThere == false)
            {
                curredPointList.Add(vertices[i]);
            }
        }
        return curredPointList;
    }
}
