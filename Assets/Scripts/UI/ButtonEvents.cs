using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonEvents : MonoBehaviour
{

    private SelectionController SC;

    void Start()
    {
        SC = FindObjectOfType<SelectionController>();
    }

    //WatchTower
    public void OutOfTowerEvent()
    {
        SC.selectedBuilding.GetComponent<WatchTower>().UnitOut();
    }
}
