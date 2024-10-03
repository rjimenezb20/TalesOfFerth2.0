using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject proyectile;
    public Transform shootPos;
    public float timeBetweenAttacks = 2f;
    [HideInInspector] public bool isLevel2;

    private int attackDamage;
    private AttackRangeSystem attackRangeSystem;
    private Transform target;
    private float timer;

    void Start()
    {
        attackRangeSystem = GetComponent<AttackRangeSystem>();
        attackDamage = GetComponent<Building>().dataLvl1.attackDamage;
        attackRangeSystem.SetRange(GetComponent<Building>().dataLvl1.attackDamage);

        StartCoroutine(OnTargetBehavior());
    }

    public void SetStats(int attackDamage, int attackRange, int attackSpeed)
    {
        this.attackDamage = attackDamage;
        attackRangeSystem.SetRange(attackRange);
        timeBetweenAttacks = attackSpeed;
    }

    IEnumerator OnTargetBehavior()
    {
        while (true)
        {
            target = attackRangeSystem.SetTarget();

            if (target != null)
            {
                //Atack timer
                timer += Time.deltaTime % 60;
                if (timer >= timeBetweenAttacks)
                {
                    GameObject newProyectile = Instantiate(proyectile, shootPos.position, Quaternion.identity);
                    ArrowProyectile arrow = newProyectile.GetComponent<ArrowProyectile>();
                    arrow.SetTarget(target);
                    arrow.SetDamage(attackDamage);
                    timer = 0;
                }
            }
            yield return null;
        }
    }
}