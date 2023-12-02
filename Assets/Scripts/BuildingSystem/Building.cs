using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.VersionControl;
using UnityEngine;

public class Building : MonoBehaviour
{
    public BuildingData data;
    [HideInInspector] public bool Placed = false;
    public BoundsInt area;
    public Vector3Int Size { get; private set; }
    public GameObject selectionSquare;
    [HideInInspector] public bool selected = false;

    [Header("Building Models")]
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;

    [Header("Materials")]
    public Material normalMat;
    public Material onConstructionMat;

    private int buildingLevel = 0;
    private TextMeshProUGUI quantityText;
    private ResourcesController RC;
    private Renderer rend;

    void Start()
    {
        RC = FindObjectOfType<ResourcesController>();
        rend = GetComponentInChildren<Renderer>();

        if (GameObject.FindWithTag("Quantity") != null)
            quantityText = GameObject.FindWithTag("Quantity").GetComponentInChildren<TextMeshProUGUI>();
    }

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

        GetComponent<Health>().enabled = true;

        Placed = true;
        ChangeMatToNormal();
    }

    public void SetSelected(bool a)
    {
        selectionSquare.SetActive(a);
        selected = a;

        if (data.buildingName == "Barracks")
            GetComponent<UnitSpawnPoint>().movePoint.gameObject.SetActive(a);
    }

    public void Level2()
    {
        level1.SetActive(false);
        level2.SetActive(true);
    }

    public void Level3()
    {
        level2.SetActive(false);
        level3.SetActive(true);
    }

    public void LevelUp()
    {
        ChangeMatToNormal();

        if (buildingLevel == 2)
        {
            Level2();
        } 
        else if (buildingLevel == 3) 
        {
            Level3();
        }
    }

    public void SpendConstructionResources()
    {
        int woodGain = 0;

        switch (buildingLevel)
        {
            case 0:
                if (data.buildingName == "Sawmill")
                {
                    woodGain = GetComponent<Sawmill>().OnPlace() * data.woodGain1;
                }

                RC.AddResourcesGain(data.goldGain1, data.foodGain1, woodGain, data.stoneGain1, data.metalGain1, data.populationGain1);
                RC.SubstractResources(data.goldCost1, data.foodCost1, data.woodCost1, data.stoneCost1, data.metalCost1);
                buildingLevel = 1;
                break;

            case 1:
                if (data.buildingName == "Sawmill")
                {
                    woodGain = GetComponent<Sawmill>().OnPlace() * data.woodGain2;
                }

                RC.AddResourcesGain(data.goldGain2, data.foodGain2, woodGain, data.stoneGain2, data.metalGain2, data.populationGain2);
                RC.SubstractResources(data.goldCost2, data.foodCost2, data.woodCost2, data.stoneCost2, data.metalCost2);
                buildingLevel = 2;
                break;

            case 2:
                if (data.buildingName == "Sawmill")
                {
                    woodGain = GetComponent<Sawmill>().OnPlace() * data.woodGain3;
                }

                RC.AddResourcesGain(data.goldGain3, data.foodGain3, woodGain, data.stoneGain3, data.metalGain3, data.populationGain3);
                RC.SubstractResources(data.goldCost3, data.foodCost3, data.woodCost3, data.stoneCost3, data.metalCost3);
                buildingLevel = 3;
                break;
        }
    }

    public void UpdateQuatityText(int amount)
    {
        quantityText.text = amount.ToString();
    }

    public void ChangeMatToNormal()
    {
        //rend.material = normalMat;
        
        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material = normalMat;
        }
    }

    public void ChangeMatToOnConstruction()
    {
        //rend.material = onConstructionMat;

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            renderer.material = onConstructionMat;
        }
    }
}