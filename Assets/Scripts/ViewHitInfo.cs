using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ViewHitInfo {
    public bool Hit;
    public Vector3 Point;
    public float Distance;
    public float Angle;
    public ViewHitInfo(bool hit, Vector3 point, float distance, float angle)
    {
        Hit = hit;
        Point = point;
        Distance = distance;
        Angle = angle;
    }
}
