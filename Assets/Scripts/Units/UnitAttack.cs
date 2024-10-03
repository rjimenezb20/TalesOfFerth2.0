using System.Collections;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    public float attackRange = 5f;
    public float timeBetweenAttacks = 5f;
    public float rotationToEnemySpeed = 1f;

    [HideInInspector] public bool attackOrder = false;
    [HideInInspector] public Transform target;
    [HideInInspector] public int attackDamage;

    private UnitMovement unitMovement;
    private Animator animator;
    private AttackRangeSystem attackRangeSystem;
    private float timer;

    void Start()
    {
        attackDamage = GetComponent<Unit>().data.attackDamage;
        attackRangeSystem = GetComponent<AttackRangeSystem>();
        unitMovement = GetComponent<UnitMovement>();
        animator = GetComponent<Animator>();
        timer = timeBetweenAttacks;

        StartCoroutine(OnTargetBehavior());
    }

    IEnumerator OnTargetBehavior()
    {
        while (true)
        {
            if (!unitMovement.movingOrder)
            {
                if (target != null && target.GetComponent<Health>().enabled)
                {
                    if (CheckDistanceToEnemy(target) <= attackRange) //On attack range
                    {
                        if (unitMovement.agent.enabled && !unitMovement.agent.isStopped)
                            unitMovement.agent.isStopped = true;

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
                    else //Out of range
                    {
                        ResetAttack();
                        if (target != null)
                            unitMovement.MoveToEnemy(target.transform.position);
                    }
                }
                else
                {
                    target = attackRangeSystem.SetTarget();
                }
            }
            yield return null;
        }
    }

    public void AttackOrder(GameObject target)
    {
        unitMovement.StopAllCoroutines();
        this.target = target.transform;
        unitMovement.movingOrder = false;
    }
    public void DoDamage()
    {
        if (target != null)
            target.GetComponent<Health>().ReceiveDamage(attackDamage);
    }
    public void ResetTarget()
    {
        target = null;
    }
    public void ResetAttack()
    {
        timer = timeBetweenAttacks;
    }
    private void Attack()
    {
        animator.CrossFade("Attack", 0.1f);
    }
    private float CheckDistanceToEnemy(Transform enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        return distance;
    }
}