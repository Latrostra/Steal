using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct EdgeHitInfo {
    public Vector3 PointA;
    public Vector3 PointB;

    public EdgeHitInfo(Vector3 pointA, Vector3 pointB)
    {
        PointA = pointA;
        PointB = pointB;
    }
}
