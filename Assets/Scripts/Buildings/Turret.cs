using System.Collections;
using UnityEngine;

public class Turret : MonoBehaviour, IBuildingLevel
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
                    newProyectile.GetComponent<Proyectile>().SetTarget(target);
                    timer = 0;
                }
            }
            yield return null;
        }
    }

    public void Level1()
    {
        
    }

    public void Level2()
    {
        throw new System.NotImplementedException();
    }

    public void Level3()
    {
        throw new System.NotImplementedException();
    }
}