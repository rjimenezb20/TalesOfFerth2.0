using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archer : MonoBehaviour
{
    [Header("Archer")]
    public GameObject arrow;
    public Transform shootPoint;

    public void ShootArrow()
    {
        GameObject instanciatedArrow = Instantiate(arrow, shootPoint.position, Quaternion.identity);

        if (GetComponent<UnitAttack>().target != null)
            instanciatedArrow.GetComponent<ArrowProyectile>().SetTarget(GetComponent<UnitAttack>().target);
    }
}