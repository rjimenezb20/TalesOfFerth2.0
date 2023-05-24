using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{

    public BuildingData data;
    public bool Placed = false;
    public BoundsInt area;
    public Vector3Int Size { get; private set; }
    public GameObject selectionSquare;

    private IBuildingLevel buildingLevel;

    public void Rotate()
    {
        transform.Rotate(new Vector3(0, 90, 0));
        area = new BoundsInt(new Vector3Int(area.position.y, area.position.x, area.position.z), new Vector3Int(area.size.y, area.size.x, area.size.z));
    }

    public void Place()
    {
        Component[] componentes = GetComponents<Component>();

        foreach (Component componente in componentes)
        {
            if (componente is Behaviour && !(componente as Behaviour).enabled)
            {
                // Activar el componente
                (componente as Behaviour).enabled = true;
            }
        }

        Placed = true;
        GetComponent<IBuildingLevel>().Level1();
    }

    public void SetSelected(bool a)
    {
        selectionSquare.SetActive(a);
    }
}