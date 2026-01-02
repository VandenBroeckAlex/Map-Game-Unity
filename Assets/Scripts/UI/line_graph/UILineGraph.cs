using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

[RequireComponent(typeof(CanvasRenderer))]
public class UILineGraph : Graphic
{
    public List<Vector2> points = new List<Vector2>(); // UI local-space points
    public float thickness = 2f;
    public int segmentsPerCurve = 10;
    public float minY = -100;
    public float maxY = 100;
    public float controlPointOffset = 20f;
    protected override void OnPopulateMesh(VertexHelper vh)
    {
        vh.Clear();
        if (points == null || points.Count < 2)
            return;

        List<Vector2> curvePoints = GenerateCurvePoints(points, segmentsPerCurve);

        for (int i = 0; i < curvePoints.Count - 1; i++)
        {
            Vector2 a = curvePoints[i];
            Vector2 b = curvePoints[i + 1];

            float normalizedA = Mathf.InverseLerp(minY, maxY, a.y);
            float normalizedB = Mathf.InverseLerp(minY, maxY, b.y);

            Color colorA = Color.Lerp(Color.red, Color.green, normalizedA);
            Color colorB = Color.Lerp(Color.red, Color.green, normalizedB);

            DrawLine(vh, a, b, colorA, colorB);
        }
    }
    void DrawLine(VertexHelper vh, Vector2 start, Vector2 end, Color colorStart, Color colorEnd)
    {
        Vector2 dir = (end - start).normalized;
        Vector2 normal = new Vector2(-dir.y, dir.x) * thickness * 0.5f;

        UIVertex[] verts = new UIVertex[4];
        for (int i = 0; i < 4; i++)
            verts[i] = UIVertex.simpleVert;

        verts[0].position = start - normal;
        verts[1].position = start + normal;
        verts[2].position = end + normal;
        verts[3].position = end - normal;

        verts[0].color = colorStart;
        verts[1].color = colorStart;
        verts[2].color = colorEnd;
        verts[3].color = colorEnd;

        int idx = vh.currentVertCount;
        vh.AddVert(verts[0]);
        vh.AddVert(verts[1]);
        vh.AddVert(verts[2]);
        vh.AddVert(verts[3]);

        vh.AddTriangle(idx + 0, idx + 1, idx + 2);
        vh.AddTriangle(idx + 2, idx + 3, idx + 0);
    }

    // --- Bezier smoothing between points ---
    List<Vector2> GenerateCurvePoints(List<Vector2> inputPoints, int segments)
    {
        List<Vector2> result = new List<Vector2>();
        for (int i = 0; i < inputPoints.Count - 1; i++)
        {
            Vector2 p0 = inputPoints[i];
            Vector2 p3 = inputPoints[i + 1];

            Vector2 p1 = p0 + new Vector2(controlPointOffset, 0);  
            Vector2 p2 = p3 - new Vector2(controlPointOffset, 0);

            for (int j = 0; j <= segments; j++)
            {
                float t = j / (float)segments;
                Vector2 point = CalculateCubicBezierPoint(t, p0, p1, p2, p3);
                result.Add(point);
            }
        }

        return result;
    }


    Vector2 CalculateCubicBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;

        return uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
    }
}

// Below, 
//using UnityEngine;
//using UnityEngine.UI;
//using System.Collections.Generic;

//[RequireComponent(typeof(CanvasRenderer))]
//public class UILineGraph : Graphic
//{
//    public List<Vector2> points = new List<Vector2>(); // UI local-space points
//    public float thickness = 2f;
//    public int segmentsPerCurve = 10;

//    protected override void OnPopulateMesh(VertexHelper vh)
//    {
//        vh.Clear();
//        if (points == null || points.Count < 2)
//            return;

//        List<Vector2> curvePoints = GenerateCurvePoints(points, segmentsPerCurve);

//        for (int i = 0; i < curvePoints.Count - 1; i++)
//        {
//            DrawLine(vh, curvePoints[i], curvePoints[i + 1]);
//        }
//    }

//    void DrawLine(VertexHelper vh, Vector2 start, Vector2 end)
//    {
//        Vector2 dir = (end - start).normalized;
//        Vector2 normal = new Vector2(-dir.y, dir.x) * thickness * 0.5f;

//        UIVertex[] verts = new UIVertex[4];
//        for (int i = 0; i < 4; i++)
//            verts[i] = UIVertex.simpleVert;

//        verts[0].position = start - normal;
//        verts[1].position = start + normal;
//        verts[2].position = end + normal;
//        verts[3].position = end - normal;

//        for (int i = 0; i < 4; i++)
//            verts[i].color = color;

//        int idx = vh.currentVertCount;
//        vh.AddVert(verts[0]);
//        vh.AddVert(verts[1]);
//        vh.AddVert(verts[2]);
//        vh.AddVert(verts[3]);

//        vh.AddTriangle(idx + 0, idx + 1, idx + 2);
//        vh.AddTriangle(idx + 2, idx + 3, idx + 0);
//    }

//    // --- Bezier smoothing between points ---
//    List<Vector2> GenerateCurvePoints(List<Vector2> inputPoints, int segments)
//    {
//        List<Vector2> result = new List<Vector2>();
//        for (int i = 0; i < inputPoints.Count - 1; i++)
//        {
//            Vector2 p0 = inputPoints[i];
//            Vector2 p1 = inputPoints[i] + new Vector2(20, 0); // Rightward control
//            Vector2 p2 = inputPoints[i + 1] - new Vector2(20, 0); // Leftward control
//            Vector2 p3 = inputPoints[i + 1];

//            for (int j = 0; j <= segments; j++)
//            {
//                float t = j / (float)segments;
//                Vector2 point = CalculateCubicBezierPoint(t, p0, p1, p2, p3);
//                result.Add(point);
//            }
//        }

//        return result;
//    }

//    Vector2 CalculateCubicBezierPoint(float t, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3)
//    {
//        float u = 1 - t;
//        float tt = t * t;
//        float uu = u * u;
//        float uuu = uu * u;
//        float ttt = tt * t;

//        return uuu * p0 + 3 * uu * t * p1 + 3 * u * tt * p2 + ttt * p3;
//    }
//}
