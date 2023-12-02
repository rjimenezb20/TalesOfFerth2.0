using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicProyectile : MonoBehaviour
{
    public float proyectileSpeed = 1;
    public float destroyTime = 2;
    public int attackDamage = 20;

    private float damageRange = 1.5f;
    private Transform target;
    private bool throwed = false;

    void Update()
    {
        if (throwed)
            if (target == null)
            {
                transform.position = Vector3.Lerp(this.transform.position, this.transform.position - this.transform.up, Time.deltaTime * 40 * proyectileSpeed);
            }
            else
            {
                transform.position = Vector3.MoveTowards(this.transform.position, target.position + new Vector3(0, 1, 0), Time.deltaTime * proyectileSpeed);
            }

        if (target != null)
            if (Vector3.Distance(transform.position, target.transform.position) < damageRange)
            {
                target.GetComponent<Health>().ReceiveDamage(attackDamage);
                Destroy(this.gameObject);
            }
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        throwed = true;
        StartCoroutine(DestroyProyectile());
    }

    IEnumerator DestroyProyectile()
    {
        yield return new WaitUntil(() => target == null);
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }
}
