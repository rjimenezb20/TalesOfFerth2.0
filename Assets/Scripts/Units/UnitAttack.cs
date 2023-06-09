using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class UnitAttack : MonoBehaviour
{
    [HideInInspector] public bool attacking = false;
    public float attackRange = 5f;
    public float timeBetweenAttacks = 5f;
    public float rotationToEnemySpeed = 1f;
    public bool attackOrder = false;
    [HideInInspector] public Transform target;

    private int attackDamage;
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
            if (!attackOrder)
            {
                target = attackRangeSystem.SetTarget();
            }
            
            if (target != null)
            {
                if (!unitMovement.movingOrder)
                {
                    if (CheckDistanceToEnemy(target) <= attackRange) //On attack range
                    {
                        unitMovement.agent.isStopped = true;

                        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(target.position - transform.position), rotationToEnemySpeed);

                        //Atack timer
                        timer += Time.deltaTime % 60;
                        if (timer >= timeBetweenAttacks)
                        {
                            Attack();
                            timer = 0;
                        }
                    }
                    else //Out of attack range
                    {
                        ResetAttack();
                        unitMovement.MoveToEnemy(target.transform.position);
                    }
                }
            }
            else
            {
                attackOrder = false;
            }
            yield return null;
        }
    }

    public void AttackOrder(GameObject target)
    {
        this.target = target.transform;
        attackOrder = true;
    }

    private void Attack()
    {   
        animator.Play("Attack");
    }

    public void DoDamage()
    {
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

    private float CheckDistanceToEnemy(Transform enemy)
    {
        float distance = Vector3.Distance(transform.position, enemy.transform.position);
        return distance;
    }
}