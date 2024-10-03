using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mine : MonoBehaviour
{
    public float detectionRadius;

    private List<int> mineList = new List<int>();
    private int mineralNumber = 0;
    private int stoneNumber = 0;
    private SphereCollider detectionTrigger;
    private Building building;

    private void Awake()
    {
        FindObjectOfType<UIManager>().ShowHideQuantity(true, 1);
    }

    void Start()
    {
        detectionTrigger = GetComponent<SphereCollider>();
        building = GetComponent<Building>();

        detectionTrigger.radius = detectionRadius * 1.5f;
    }

    public List<int> OnPlace()
    {
        mineList.Clear();
        mineList.Add(stoneNumber);
        mineList.Add(mineralNumber);
        return mineList;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Stone")
        {
            stoneNumber++;
            building.UpdateQuatityText(stoneNumber);
        }

        if (other.gameObject.tag == "Mineral")
        {
            mineralNumber++;
            building.UpdateQuatityText(mineralNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Stone")
        {
            stoneNumber--;
            building.UpdateQuatityText(stoneNumber);
        }

        if (other.gameObject.tag == "Mineral")
        {
            mineralNumber--;
            building.UpdateQuatityText(mineralNumber);
        }
    }
}