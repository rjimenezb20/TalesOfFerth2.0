using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.CanvasScaler;

public class WatchTower : MonoBehaviour
{
    [Header("Positions")]
    public Transform unitSpot1;
    public Transform unitSpot2;
    public Transform unitSpot3;
    public Transform outSpot1;
    public Transform outSpot2;
    public Transform outSpot3;

    [Header("Bonus")]
    public int DamageIncrease = 20;
    public int RangeIncrease = 30;

    [HideInInspector] public List<GameObject> unitsIn;

    private SelectionController SC;

    private void Start()
    {
        SC = FindObjectOfType<SelectionController>();
    }

    public void GetUnitIn(GameObject unit)
    {
        if (unitsIn.Count < 3) {

            unitsIn.Add(unit);

            unit.GetComponent<NavMeshAgent>().enabled = false;
            unit.GetComponent<UnitMovement>().enabled = false;
            unit.GetComponent<UnitMovement>().movingOrder = false;
            unit.GetComponent<CapsuleCollider>().enabled = false;
            unit.GetComponent<Unit>().enabled = false;

            int index = unitsIn.IndexOf(unit);

            switch(index)
            {
                case 0:
                   unit.transform.position = unitSpot1.position;
                   break;
                case 1:
                    unit.transform.position = unitSpot2.position;
                    break;
                case 2:
                    unit.transform.position = unitSpot3.position;
                    break;
            }

            SC.DeselectUnit(unit.GetComponent<Unit>());
            unit.GetComponent<Animator>().Play("Idle");

            UnitAttack unitAttack = unit.GetComponent<UnitAttack>();
            unitAttack.attackDamage += Mathf.CeilToInt(unitAttack.attackDamage * DamageIncrease / 100);
            unitAttack.attackRange += Mathf.CeilToInt(unitAttack.attackRange * RangeIncrease / 100);

            AttackRangeSystem attackRange = unit.GetComponent<AttackRangeSystem>();
            attackRange.detectionRadius = unitAttack.attackRange;
        }
    }

    public void UnitOut()
    {
        unitsIn[0].GetComponent<NavMeshAgent>().enabled = true;
        unitsIn[0].GetComponent<UnitMovement>().enabled = true;
        unitsIn[0].GetComponent<UnitMovement>().movingOrder = true;
        unitsIn[0].GetComponent<CapsuleCollider>().enabled = true;
        unitsIn[0].GetComponent<Unit>().enabled = true;

        int index = unitsIn.Count - 1;

        switch (index)
        {
            case 0:
                unitsIn[0].transform.position = outSpot1.position;
                break;
            case 1:
                unitsIn[0].transform.position = outSpot2.position;
                break;
            case 2:
                unitsIn[0].transform.position = outSpot3.position;
                break;
        }

        unitsIn.RemoveAt(0);
    }
}