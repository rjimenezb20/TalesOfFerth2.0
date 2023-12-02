using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public GameObject proyectile;
    public Transform spawnPoint;
    public float timeBetweenAttacks = 2f;

    private AttackRangeSystem attackRangeSystem;
    private Transform target;
    private float timer;

    void Start()
    {
        attackRangeSystem = GetComponent<AttackRangeSystem>();

        StartCoroutine(OnTargetBehavior());
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
                    GameObject newProyectile = Instantiate(proyectile, spawnPoint.position, Quaternion.identity);
                    newProyectile.GetComponent<ArrowProyectile>().SetTarget(target);
                    timer = 0;
                }
            }
            yield return null;
        }
    }
}