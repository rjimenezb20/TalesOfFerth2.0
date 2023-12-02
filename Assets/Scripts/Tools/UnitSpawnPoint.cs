using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSpawnPoint : MonoBehaviour
{
    public Transform spawnPoint;
    public Transform movePoint;
    private SelectionController SC;

    private LayerMask layerMask;

    void Start()
    {
        SC = FindObjectOfType<SelectionController>();
        layerMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            if (GetComponent<Building>().selected)
            {
                ChangeMovePoint();
            }
        }
    }

    public void ChangeMovePoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, layerMask))
        {
            if (raycastHit.transform.tag == "Environment")
            {
                movePoint.position = raycastHit.point;
            }
        }            
    }
}
