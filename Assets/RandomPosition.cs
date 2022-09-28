using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomPosition
{
    private List<Vector3> verticeList = new List<Vector3>();
    private GameObject ground;
    List<Vector3> Corners = new List<Vector3>();
    List<Vector3> EdgeVectors = new List<Vector3>();

    public RandomPosition(List<Vector3> verticeList, GameObject ground)
    {
        this.verticeList = verticeList;
        this.ground = ground;
    }
    public Vector3 CalculateRandomPoint() {
        CalculateCornerPoints();
        int randomCornerIdx = Random.Range(0, 2) == 0 ? 0 : 2;

        CalculateEdgeVectors(randomCornerIdx);

        float u = Random.Range(0.0f, 1.0f); 
        float v = Random.Range(0.0f, 1.0f);

        if (v + u > 1) 
        {
            v = 1 - v;
            u = 1 - u;
        }

        return Corners[randomCornerIdx] + u * EdgeVectors[0] + v * EdgeVectors[1];
    }
    private void CalculateEdgeVectors(int VectorCorner)
    {
        EdgeVectors.Clear();

        EdgeVectors.Add(Corners[3] - Corners[VectorCorner]);
        EdgeVectors.Add(Corners[1] - Corners[VectorCorner]);
    }

    private void CalculateCornerPoints()
    {
        Corners.Clear();

        Corners.Add(ground.gameObject.transform.TransformPoint(verticeList[0]));
        Corners.Add(ground.gameObject.transform.TransformPoint(verticeList[10]));
        Corners.Add(ground.gameObject.transform.TransformPoint(verticeList[110]));
        Corners.Add(ground.gameObject.transform.TransformPoint(verticeList[120]));
    }
}
