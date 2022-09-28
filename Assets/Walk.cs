using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Walk : StateMachineBehaviour
{
    private NavMeshAgent _nmc;
    private IEnemyAi _enemyAi;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _nmc = animator.gameObject.GetComponent<NavMeshAgent>();
        _enemyAi = animator.gameObject.GetComponent<EnemyAi>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (_nmc.velocity == Vector3.zero) {
            _nmc.SetDestination(_enemyAi.CalculatePosition());
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
    }
}
