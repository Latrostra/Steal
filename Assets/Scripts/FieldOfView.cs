using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public List<Transform> visibleTargets = new List<Transform>();
    [SerializeField]
    private float _meshResolution;
    [SerializeField]
    private int _edgeResolveIteration;
    [SerializeField]
    private float _edgeDistanceThreshold;
    [SerializeField]
    private float _cutDownDistance = 0.2f;
    [SerializeField]
    private MeshFilter _meshFilter;
    [SerializeField]
    private LayerMask _targetMask;
    private int _obstacleMask = 1 << 7;
    private Mesh _mesh;
    private void Start() {
        _mesh = new Mesh();
        _mesh.name = "View Visualization";
        _meshFilter.mesh = _mesh;
    }
    private void Update() {
        FindTargetsInAngle();
    }
    private void LateUpdate() {
        DrawFieldOfView();
    }
    private void FindTargetsInAngle() {
        visibleTargets.Clear();
        Collider[] targetInView = Physics.OverlapSphere(transform.position, viewRadius, _targetMask);
        foreach(Collider target in targetInView) {
            var dir = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dir) < viewAngle / 2) {
                float dist = Vector3.Distance(target.transform.position, transform.position);
                if (!Physics.Raycast(transform.position, dir, dist, _obstacleMask)) {
                    visibleTargets.Add(target.transform);
                }
            }
        }
    }

    private void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * _meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        ViewHitInfo viewHitInfoOld = new ViewHitInfo();

        for(int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewHitInfo viewHitInfo = ViewCast(angle);

            if (i > 0) {
                bool edgeDstThresholdExceeded = Mathf.Abs(viewHitInfoOld.Distance - viewHitInfo.Distance) > _edgeDistanceThreshold;
                if (viewHitInfoOld.Hit != viewHitInfo.Hit || (viewHitInfoOld.Hit && viewHitInfo.Hit && edgeDstThresholdExceeded)) {
                    EdgeHitInfo edgeInfo = FindEdge(viewHitInfoOld, viewHitInfo);
                    if (edgeInfo.PointA != Vector3.zero) {
                        viewPoints.Add(edgeInfo.PointA);
                    }
                    if (edgeInfo.PointB != Vector3.zero) {
                        viewPoints.Add(edgeInfo.PointB);
                    }
                }
            }

            viewPoints.Add(viewHitInfo.Point);
            viewHitInfoOld = viewHitInfo;
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]) + Vector3.forward * _cutDownDistance;
            if (i < vertexCount - 2) {
                triangles[i * 3] = 0;
                triangles[i * 3 + 1] = i + 1;
                triangles[i * 3 + 2] = i + 2;
            }
        }
        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }

    private EdgeHitInfo FindEdge(ViewHitInfo minViewCast, ViewHitInfo maxViewCast) {
        float minAngle = minViewCast.Angle;
        float maxAngle = maxViewCast.Angle;
        Vector3 minPoint = Vector3.zero;
        Vector3 maxPoint = Vector3.zero;

        for (int i = 0; i < _edgeResolveIteration; i++)
        {
            float angle = (minAngle + maxAngle) / 2;
            ViewHitInfo newViewCast = ViewCast(angle);

            bool edgeDstThresholdExceeded = Mathf.Abs(minViewCast.Distance - newViewCast.Distance) > _edgeDistanceThreshold;
            if (newViewCast.Hit == minViewCast.Hit && !edgeDstThresholdExceeded) {
                minAngle = angle;
                minPoint = newViewCast.Point;
            } else {
                maxAngle = angle;
                maxPoint = newViewCast.Point;
            }
        }
        return new EdgeHitInfo(minPoint, maxPoint);
    }

    private ViewHitInfo ViewCast(float globalAngle) {
        Vector3 dir = DirectionAngle(globalAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, _obstacleMask)) {
            return new ViewHitInfo(true, hit.point, hit.distance, globalAngle);
        }
        return new ViewHitInfo(false, transform.position + dir * viewRadius, hit.distance, globalAngle);
    }

    public Vector3 DirectionAngle(float angleInDeg, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDeg += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }
}
