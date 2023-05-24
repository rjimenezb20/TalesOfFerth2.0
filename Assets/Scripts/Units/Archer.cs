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
        Archer archer = GetComponent<Archer>();
        GameObject instanciatedArrow = Instantiate(archer.arrow, archer.shootPoint.position, Quaternion.identity);

        if (GetComponent<UnitAttack>().target != null)
            instanciatedArrow.GetComponent<Proyectile>().SetTarget(GetComponent<UnitAttack>().target);
    }
}