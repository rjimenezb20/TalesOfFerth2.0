using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public EnemyData data;
    public float rotationToEnemySpeed;
    public float timeBetweenAttacks;
    public float attackRange;
    public bool isPatrol = false;
    public bool isShelter = false;

    private Transform target;
    private NavMeshAgent agent;
    private AttackRangeSystem attackRangeSystem;
    private float timer;
    private Animator animator;
    private int attackDistanceOffset = 0;
    private ObjectPool pool;
    private Vector3 townHallPos;
    /*private Vector3 currentWayPoint;
    private int wayPointRuteNumber;
    private int currentWayPointNumber = 0;
    private WavesController WC;*/

    void Start()
    {
        if (!isShelter)
        {
            agent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            attackRangeSystem = GetComponent<AttackRangeSystem>();
            pool = FindAnyObjectByType<ObjectPool>();
            townHallPos = GameObject.FindWithTag("TownHall").transform.position;
            timer = timeBetweenAttacks;


            //WC = FindAnyObjectByType<WavesController>();
            //currentWayPoint = WC.GetWayPoint(wayPointRuteNumber, currentWayPointNumber);
            //agent.SetDestination(currentWayPoint);

            StartCoroutine(EnemyBehavior());
        }
    }

    IEnumerator EnemyBehavior()
    {
        while(true)
        {
            if (target == null)
            {
                target = attackRangeSystem.SetTarget();
            }

            if (target != null)
            {
                if (target.GetComponent<Building>())
                {
                    attackDistanceOffset = target.GetComponent<Building>().dataLvl1.enemyAttackDistanceOffset;
                }

                if (CheckDistanceToEnemy(target) - attackDistanceOffset <= attackRange) //On attack range
                {
                    if (agent.enabled && !agent.isStopped)
                    {
                        agent.isStopped = true;
                    }

                    //Rotation to target
                    Vector3 directionToTarget = target.position - transform.position;
                    directionToTarget.y = 0;
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(directionToTarget), rotationToEnemySpeed / 100);

                    //Attack timer (attack speed)
                    timer += Time.deltaTime % 60;
                    if (timer >= timeBetweenAttacks)
                    {
                        Attack();
                        timer = 0;
                    }
                }
                else //Only in attack range ?
                {
                    agent.isStopped = false;
                    animator.Play("Run");
                    agent.SetDestination(target.transform.position);
                }
            }
            else
            {
                if (!isPatrol)
                {
                    agent.isStopped = false;
                    animator.Play("Run");
                    agent.SetDestination(townHallPos);
                    //WayPointsManage();
                }
            }
            yield return new WaitForEndOfFrame();
        }
    }

    private float CheckDistanceToEnemy(Transform enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        return distance;
    }

    private void Attack()
    {
        animator.CrossFade("Attack", 0.1f);
    }

    public void DoDamage()
    {
        if (target != null) 
            target.GetComponent<Health>().ReceiveDamage(data.attackDamage);
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
    }

    public void Death()
    {
        StopAllCoroutines();

        Component[] components = GetComponents<Component>();

        foreach (Component component in components)
        {
            if (component is Behaviour && !(component is Animator))
            {
                ((Behaviour)component).enabled = false;
            }
        }

        GetComponent<CapsuleCollider>().enabled = false;

        animator.CrossFade("Death", 0.1f);
    }

    public void ReturnToPool()
    {
        pool.ReturnToPool(gameObject);
    }

    /*private void WayPointsManage()
    { 
        if (Vector3.Distance(transform.position, currentWayPoint) < 5)
        {
            currentWayPointNumber += 1;
            currentWayPoint = WC.GetWayPoint(wayPointRuteNumber, currentWayPointNumber);
            agent.SetDestination(currentWayPoint);
        }
    }

    public void SetRouteNumber(int routeNumber)
    {
        wayPointRuteNumber = routeNumber;
    }*/
}  