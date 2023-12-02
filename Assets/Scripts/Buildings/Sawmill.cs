using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Sawmill : MonoBehaviour
{
    public float detectionRadius;

    private int treesNumber = 0;
    private bool placed = false;
    private SphereCollider detectionTrigger;
    private Building building;

    private void Awake()
    {
        FindObjectOfType<UIManager>().ShowHideQuantity(true);
    }

    void Start()
    {
        detectionTrigger = GetComponent<SphereCollider>();
        building = GetComponent<Building>();

        detectionTrigger.radius = detectionRadius * 1.5f;
    }

    public int OnPlace()
    {
        return treesNumber;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tree")
        {
            treesNumber++;
            building.UpdateQuatityText(treesNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tree")
        {
            treesNumber--;
            building.UpdateQuatityText(treesNumber);
        }
    }
} 