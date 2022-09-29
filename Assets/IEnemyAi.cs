using UnityEngine;
internal interface IEnemyAi
{
    Vector3 CalculatePosition();
    bool IsRotating {get;}
}