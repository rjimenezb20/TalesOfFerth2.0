using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using static UnityEngine.UI.CanvasScaler;

public class UnitsController : MonoBehaviour
{
    [HideInInspector] public bool agresiveMove = false;
    
    private SelectionController SC;
    private LayerMask layerMask;
    private CursorChange CC;
    
    void Start()
    {
        SC = GetComponent<SelectionController>();
        CC = FindAnyObjectByType<CursorChange>();
        layerMask = LayerMask.GetMask("Ground", "Enemy", "Building");
    }

    void Update()
    {
        if (SC.GetSelectedUnits().Count > 0)
        {
            if (Input.GetMouseButtonDown(1)) 
            {
                SelectedUnitsOrder();
                agresiveMove = false;
            }

            if (Input.GetKey(KeyCode.A))
            {
                CC.ChangeToAttackCursor();
                agresiveMove = true;
            }

            if (agresiveMove)
                if (Input.GetMouseButtonUp(0)) 
                {
                    Debug.Log("Entra");
                    AgressiveMoveOrder();
                    agresiveMove = false;
                    CC.ChangeToNormalCursor();
                }       
        }
    }

    private void SelectedUnitsOrder()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, layerMask))
        {
            if(!EventSystem.current.IsPointerOverGameObject())
            {
                List<Unit> units = SC.GetSelectedUnits();

                if (raycastHit.transform.CompareTag("Enemy")) //Attack
                {
                    foreach (Unit unit in units)
                    {
                        unit.GetComponent<UnitAttack>().AttackOrder(raycastHit.transform.gameObject);
                    }
                }
                else //Move
                {
                    if (raycastHit.transform.CompareTag("Building"))
                    {
                        if (raycastHit.transform.GetComponent<WatchTower>())
                        {
                            foreach (Unit unit in units)
                            {
                                unit.GetComponent<UnitMovement>().MoveOrder(raycastHit.transform.position);
                                unit.GetComponent<UnitMovement>().StartEnterTowerOrder(raycastHit.transform.gameObject);  
                            }
                        }
                    }
                    else
                    {
                        if (NavMesh.SamplePosition(raycastHit.point, out _, 100f, NavMesh.AllAreas))
                        {
                            if (units.Count == SC.GetSelectedUnits().Count)
                            foreach (Unit unit in units)
                            {
                                unit.GetComponent<UnitMovement>().MoveOrder(raycastHit.point);
                            }
                        }
                    }
                }
            }
        }
    }

    private void AgressiveMoveOrder()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, layerMask))
        {
            List<Unit> units = SC.GetSelectedUnits();
            foreach (Unit unit in units)
            {
                unit.GetComponent<UnitMovement>().AgresiveMoveOrder(raycastHit.point);
            }
        }
    }
}