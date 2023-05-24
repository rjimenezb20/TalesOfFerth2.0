using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static UnityEngine.UI.CanvasScaler;

public class UnitsController : MonoBehaviour
{
    private SelectionController SC;
    private LayerMask layerMask;

    void Start()
    {
        SC = GetComponent<SelectionController>();
        layerMask = LayerMask.GetMask("Ground", "Enemy");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            if (SC.GetSelectedUnits().Count > 0)
                SelectedUnitsOrder();
        }
    }

    private void SelectedUnitsOrder()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, layerMask))
        {
            if (raycastHit.transform.CompareTag("Enemy")) //Attack
            {
                foreach (Unit unit in SC.GetSelectedUnits())
                {
                    unit.GetComponent<UnitAttack>().AttackOrder(raycastHit.transform.gameObject);
                }
            } 
            else //Move
            {
                if (NavMesh.SamplePosition(raycastHit.point, out _, 100f, NavMesh.AllAreas))
                {
                    foreach (Unit unit in SC.GetSelectedUnits())
                    {
                        unit.GetComponent<UnitMovement>().MoveOrder(raycastHit.point);
                    }
                }
            }
        }
    }
}