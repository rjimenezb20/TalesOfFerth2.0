using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mage : MonoBehaviour
{
    [Header("Mage")]
    public GameObject magicProjectile;
    public Transform shootPoint;

    private GameObject projectil;

    public void SpawnProjectile()
    {
        projectil = Instantiate(magicProjectile, shootPoint);
    }

    public void ShootProjectile()
    {
        projectil.transform.parent = null;

        if (GetComponent<UnitAttack>().target != null)
            projectil.GetComponent<MagicProyectile>().SetTarget(GetComponent<UnitAttack>().target);
    }
}