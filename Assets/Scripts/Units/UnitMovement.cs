using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitMovement : MonoBehaviour
{
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public bool moving = false;
    [HideInInspector] public bool standing = false; 
    [HideInInspector] public bool movingOrder = false;

    private Animator animator;
    private UnitAttack unitAttack;
    
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        unitAttack = GetComponent<UnitAttack>();
    }

    public void MoveOrder(Vector3 position)
    {
        StopAllCoroutines();

        agent.SetDestination(position);
        agent.isStopped = false;
        animator.Play("Run");
        movingOrder = true;

        unitAttack.attackOrder = false;
        unitAttack.ResetAttack();
        unitAttack.ResetTarget();

        StartCoroutine(CheckDestination());
        StartCoroutine(UnreachableDestination());
    }

    public void MoveToEnemy (Vector3 position)
    {
        StopAllCoroutines();

        agent.SetDestination(position);
        agent.isStopped = false;
        animator.Play("Run");
    }

    IEnumerator CheckDestination()
    {
        yield return new WaitUntil(() => agent.hasPath);
        yield return new WaitUntil(() => agent.remainingDistance < .1f);
        if (agent.hasPath)
        {
            agent.ResetPath();
            animator.Play("Idle");
            moving = false;
            standing = true;
            movingOrder = false;
        }
    }

    IEnumerator UnreachableDestination()
    {
        yield return new WaitUntil(() => agent.hasPath);
        yield return new WaitUntil(() => agent.remainingDistance < 5);
        yield return new WaitForSeconds(1);
        if (agent.hasPath)
        {
            agent.ResetPath();
            moving = false;
            standing = true;
            movingOrder = false;
            yield return new WaitUntil(() => agent.velocity == Vector3.zero);
            animator.Play("Idle");
        }
    }
}