using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [HideInInspector] public bool inGroup = false;
    public GameObject selectionCircle;
    public UnitData data;

    private SelectionController SC;

    void Start()
    {
        SC = GameObject.FindGameObjectWithTag("GameController").GetComponent<SelectionController>();
    }
    
    void Update()
    {
        CheckIfSelected();
    }

    public void SetSelected(bool a)
    {
        selectionCircle.SetActive(a);
    }

    private void CheckIfSelected()
    {
        //Selection rectangle
        if (Input.GetMouseButtonUp(0))
        {
            Vector3 camPos = Camera.main.WorldToScreenPoint(transform.position);
            camPos.y = SelectionController.InvertMouseY(camPos.y);

            if (SelectionController.selection.Contains(camPos, true))
                SC.AddSelectedUnit(this);
        }
    }
}
