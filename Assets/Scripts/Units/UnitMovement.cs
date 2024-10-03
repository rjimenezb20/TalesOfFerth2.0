using System.Collections;
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
    private bool enterTowerOrder = false;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        unitAttack = GetComponent<UnitAttack>();
    }

    public void MoveOrder(Vector3 position)
    {
        StopAllCoroutines();

        if (agent != null)
        {
            agent.SetDestination(position);
            agent.isStopped = false;
        }
            
        animator.CrossFade("Run", 0.1f);
        movingOrder = true;
        enterTowerOrder = false;
        unitAttack.attackOrder = false;
        unitAttack.ResetAttack();
        unitAttack.ResetTarget();

        StartCoroutine(CheckDestination());
        //StartCoroutine(UnreachableDestination());
    }

    public void MoveToEnemy (Vector3 position)
    {
        StopAllCoroutines();

        if (agent != null)
        {
            agent.SetDestination(position);
            agent.isStopped = false;
        }
        animator.Play("Run");
        enterTowerOrder = false;
    }

    public void AgresiveMoveOrder(Vector3 position)
    {
        StopAllCoroutines();

        agent.SetDestination(position);
        agent.isStopped = false;
        unitAttack.attackOrder = true;
        enterTowerOrder = false;
        animator.CrossFade("Run", 0.1f);

        StartCoroutine(CheckDestination());
        //StartCoroutine(UnreachableDestination());
    }

    public IEnumerator CheckDestination()
    {
        //yield return new WaitUntil(() => agent.hasPath);
        yield return new WaitUntil(() => agent.hasPath && agent.remainingDistance < .1f);
        if (agent.hasPath)
        {
            agent.ResetPath();
            unitAttack.ResetTarget();
            animator.CrossFade("Idle", 0.1f);
            moving = false;
            standing = true;
            movingOrder = false;
        }
    }

    /*public IEnumerator UnreachableDestination()
    {
        yield return new WaitUntil(() => agent.hasPath && agent.remainingDistance < 5);
        yield return new WaitForSeconds(1);

        agent.ResetPath();
        moving = false;
        standing = true;
        movingOrder = false;
        Debug.Log("Entra");
        yield return new WaitUntil(() => agent.velocity == Vector3.zero);
        animator.CrossFade("Idle", 0.1f);
        movingOrder = false;
    }*/

    IEnumerator EnterTowerOrder(WatchTower tower)
    {
        enterTowerOrder = true;

        while (enterTowerOrder)
        {
            if (Vector3.Distance(this.transform.position, tower.transform.position) < 4)
                if (tower.unitsIn.Count < 3)
                {
                    tower.GetUnitIn(this.gameObject);
                    StopAllCoroutines();
                    enterTowerOrder = false;
                } 
                else
                {
                    enterTowerOrder = false;
                }

            yield return null;
        } 

        if (!enterTowerOrder)
            StopCoroutine(EnterTowerOrder(null));
    }

    public void StartEnterTowerOrder(GameObject tower)
    {
        StartCoroutine(EnterTowerOrder(tower.GetComponent<WatchTower>()));
    }
}