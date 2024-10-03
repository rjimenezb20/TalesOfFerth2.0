using System.Collections;
using UnityEngine;

public class ArrowProyectile : MonoBehaviour
{
    public float proyectileSpeed = 1;
    public float destroyTime = 2;
    public int attackDamage = 20;
    
    private float damageRange = 1.5f;
    private Transform target;
    private Vector3 targetPos;
    private Vector3 lastTargetPos;
    private Vector3 initialPos;
    private Vector3 control;
    private Vector3 previousPos;
    private Vector3 height;

    private float time;

    private void Start()
    {
        initialPos = transform.position;
    }

    void Update()
    {
        time += Time.deltaTime * proyectileSpeed;

        if (target != null)
        {
            targetPos = target.position + new Vector3(0, 1, 0);
            lastTargetPos = targetPos;
        } 
        else
        {
            targetPos = lastTargetPos;
        }

        height = new Vector3(0, 1, 0) * Vector3.Distance(targetPos, initialPos) / 2f;

        control = (initialPos + targetPos) / 2f + height;
        Vector3 ac = Vector3.Lerp(initialPos, control, time);
        Vector3 cb = Vector3.Lerp(control, targetPos, time);
        transform.position = Vector3.Lerp(ac, cb, time);

        Vector3 direction = transform.position - previousPos;
        if (direction.magnitude > 0.01f)
        {
            Quaternion rotation = Quaternion.LookRotation(direction, Vector3.up);
            transform.rotation = rotation;
        }

        previousPos = transform.position;

        if (Vector3.Distance(transform.position, targetPos) < damageRange)
        {
            if (target != null && target.GetComponent<Health>().enabled)
                target.GetComponent<Health>().ReceiveDamage(attackDamage);

            Destroy(gameObject);
        }        
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        StartCoroutine(DestroyProyectile());
    }

    public void SetDamage(int damage)
    {
        attackDamage = damage;
    }

    IEnumerator DestroyProyectile()
    {
        yield return new WaitUntil(() => target == null);
        yield return new WaitForSeconds(destroyTime);
        Destroy(gameObject);
    }
}