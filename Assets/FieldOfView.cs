using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float viewRadius;
    [Range(0, 360)]
    public float viewAngle;
    public float meshResolution;
    [SerializeField]
    private List<Transform> _visibleTargets = new List<Transform>();
    [SerializeField]
    private float _cutDownDistance = 0.2f;
    [SerializeField]
    private MeshFilter _meshFilter;
    private int _obstacleMask = 1 << 7;
    private int _enemyMask = 1 << 6;
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
        _visibleTargets.Clear();
        Collider[] targetInView = Physics.OverlapSphere(transform.position, viewRadius, _enemyMask);
        foreach(Collider target in targetInView) {
            var dir = (target.transform.position - transform.position).normalized;
            if (Vector3.Angle(transform.forward, dir) < viewAngle / 2) {
                float dist = Vector3.Distance(target.transform.position, transform.position);
                if (!Physics.Raycast(transform.position, dir, dist, _obstacleMask)) {
                    _visibleTargets.Add(target.transform);
                }
            }
        }
    }

    private void DrawFieldOfView() {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();

        for(int i = 0; i <= stepCount; i++) {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewHitInfo viewHitInfo = ViewCast(angle);
            viewPoints.Add(viewHitInfo.Point);
        }

        int vertexCount = viewPoints.Count + 1;
        Vector3[] vertices = new Vector3[vertexCount];
        int[] triangles = new int[(vertexCount - 2) * 3];

        vertices[0] = Vector3.zero;
        for (int i = 0; i < vertexCount - 1; i++) {
            vertices[i + 1] = transform.InverseTransformPoint(viewPoints[i]);
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

    private ViewHitInfo ViewCast(float gloablAngle) {
        Vector3 dir = DirectionAngle(gloablAngle, true);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, dir, out hit, viewRadius, _obstacleMask)) {
            return new ViewHitInfo(hit.point);
        }
        return new ViewHitInfo(transform.position + dir * viewRadius);
    }

    public Vector3 DirectionAngle(float angleInDeg, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDeg += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDeg * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDeg * Mathf.Deg2Rad));
    }
}
