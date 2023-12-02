using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class UIManager : MonoBehaviour
{
    [Header("Utilities")]
    public GameObject utilities;
    public GameObject WatchTower;
    public GameObject Tower;
    public GameObject House;
    public GameObject Sawmill;
    public GameObject Barracks;
    public GameObject Smith;
    public GameObject Mine;

    [Header("Unit Info")]
    public Image Image;
    public TextMeshProUGUI Name;
    public TextMeshProUGUI Description;
    public TextMeshProUGUI Hp;
    public TextMeshProUGUI MaxHp;
    public TextMeshProUGUI Attack;
    public TextMeshProUGUI Range;
    public Slider slider;
    public GameObject unitQueue;

    [Header("Quantity")]
    public GameObject quantity;
    public Vector3 quantityPosition;

    [Header("Unit Queue")]
    public List<Image> unitQueueImgs;

    [Header("Creation Info")]
    public GameObject creationInfo;
    public Vector3 creationInfoOffset;
    public GameObject creationName;
    public GameObject creationDescription;
    public GameObject creationGoldCost;
    public GameObject creationFoodCost;
    public GameObject creationWoodCost;
    public GameObject creationStoneCost;
    public GameObject creationMetalCost;
    public GameObject creationPopulationCost;
    public GameObject creationHealth;
    public GameObject creationAttack;
    public GameObject creationRange;
    public Image creationImage;

    private GameObject activeUtility;
    private Unit currentUnit;
    private Building currentBuilding;

    private void Update()
    {
        if (currentUnit != null)
        {
            Health health = currentUnit.GetComponent<Health>();
            if (Hp.text != health.HP.ToString())
            {
                Hp.text = health.HP.ToString();
                slider.value = (float)health.HP / currentUnit.data.healthPoints;
            }
        }
            
        if (currentBuilding != null)
        {
            Health health = currentBuilding.GetComponent<Health>();
            if (Hp.text != health.HP.ToString())
            {
                Hp.text = health.HP.ToString();
                slider.value = (float)health.HP / health.MaxHP;
            }
        }
            
        if (quantity != null)
            quantity.transform.position = Input.mousePosition + quantityPosition;
    }

    public void ShowUtilities(string name)
    {
        if (activeUtility != null)
            activeUtility.SetActive(false); 

        if (name != null)
            if (name == "WatchTower")
            {
                WatchTower.SetActive(true);
                activeUtility = WatchTower;
            }
            else if (name == "Tower")
            {
                Tower.SetActive(true);
                activeUtility = Tower;
            }
            else if (name == "House")
            {
                House.SetActive(true);
                activeUtility = House;
            }
            else if (name == "Barracks")
            {
                Barracks.SetActive(true);
                activeUtility = Barracks;
            }
            else if (name == "Mine")
            {
                Mine.SetActive(true);
                activeUtility = Mine;
            }
            else if (name == "Sawmill")
            {
                Sawmill.SetActive(true);
                activeUtility = Sawmill;
            }
            else if (name == "Smith")
            {
                Smith.SetActive(true);
                activeUtility = Smith;
            }
    }

    public void ShowUnitInfo(Unit unit)
    {
        currentBuilding = null;
        currentUnit = unit;

        Image.sprite = currentUnit.data.image;
        Name.text = currentUnit.data.unitName;
        Description.text = currentUnit.data.description;
        MaxHp.text = "/ " + currentUnit.data.healthPoints.ToString();
        Hp.text = currentUnit.GetComponent<Health>().HP.ToString();
        Attack.text = currentUnit.data.attackDamage.ToString();
        Range.text = currentUnit.data.attackRange.ToString();
        slider.value = (float)currentUnit.GetComponent<Health>().HP / currentUnit.data.healthPoints;
    }

    public void ShowBuildingInfo(Building building)
    {
        currentUnit = null;
        currentBuilding = building;
        Health health = currentBuilding.GetComponent<Health>();

        Image.sprite = currentBuilding.data.image;
        Name.text = currentBuilding.data.buildingName;
        Description.text = currentBuilding.data.description;
        MaxHp.text = health.MaxHP.ToString();
        Hp.text = health.HP.ToString();
        Attack.text = "0";
        Range.text = "0";
        slider.value = (float)health.HP / health.MaxHP;
    }

    public void ShowHideQuantity(bool a)
    {
        quantity.SetActive(a);
    }

    public void ShowHideUnitQueue(bool a)
    {
        unitQueue.SetActive(a);
    }

    public void UpdateUnitQueueImages(List<Unit> li)
    {
        foreach (Image img in unitQueueImgs)
        {
            img.sprite = null;
        }

        for (int i = 0; i < li.Count; i++)
        {
            unitQueueImgs[i].sprite = li[i].data.image;
        }
    }

    public void BuildingButtonHover(Building building)
    {
        creationInfo.SetActive(true);
        creationInfo.transform.position = Input.mousePosition + creationInfoOffset;
        creationName.GetComponent<TextMeshProUGUI>().text = building.data.buildingName;
        creationDescription.GetComponentInChildren<TextMeshProUGUI>().text = building.data.description;

        if (building.data.goldCost1 == 0)
            creationGoldCost.SetActive(false);
        else
            creationGoldCost.GetComponentInChildren<TextMeshProUGUI>().text = building.data.goldCost1.ToString();

        if (building.data.foodCost1 == 0)
            creationFoodCost.SetActive(false);
        else
            creationFoodCost.GetComponentInChildren<TextMeshProUGUI>().text = building.data.foodCost1.ToString();

        if (building.data.woodCost1 == 0)
            creationWoodCost.SetActive(false);
        else
            creationWoodCost.GetComponentInChildren<TextMeshProUGUI>().text = building.data.woodCost1.ToString();

        if (building.data.stoneCost1 == 0)
            creationStoneCost.SetActive(false);
        else
            creationStoneCost.GetComponentInChildren<TextMeshProUGUI>().text = building.data.stoneCost1.ToString();

        if (building.data.metalCost1 == 0)
            creationMetalCost.SetActive(false);
        else
            creationMetalCost.GetComponentInChildren<TextMeshProUGUI>().text = building.data.metalCost1.ToString();

        creationHealth.GetComponentInChildren<TextMeshProUGUI>().text = building.data.healthPoints1.ToString();
        creationAttack.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.attackDamage.ToString();
        creationRange.GetComponentInChildren<TextMeshProUGUI>().text = building.data.buildingName;
        creationImage.GetComponentInChildren<Image>().sprite = building.data.image;
    }

    public void UnitButtonHover(Unit unit)
    {
        creationInfo.SetActive(true);
        creationInfo.transform.position = Input.mousePosition + creationInfoOffset;

        creationName.GetComponent<TextMeshProUGUI>().text = unit.data.unitName;
        creationDescription.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.description;

        if (unit.data.goldCost == 0)
            creationGoldCost.SetActive(false);
        else
            creationGoldCost.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.goldCost.ToString();

        if (unit.data.foodCost == 0)
            creationFoodCost.SetActive(false);
        else
            creationFoodCost.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.foodCost.ToString();

        if (unit.data.woodCost == 0)
            creationWoodCost.SetActive(false);
        else
            creationWoodCost.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.woodCost.ToString();

        if (unit.data.stoneCost == 0)
            creationStoneCost.SetActive(false);
        else
            creationStoneCost.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.stoneCost.ToString();

        if (unit.data.metalCost == 0)
            creationMetalCost.SetActive(false);
        else
            creationMetalCost.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.metalCost.ToString();

        creationHealth.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.healthPoints.ToString();
        creationAttack.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.attackDamage.ToString();
        creationRange.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.attackRange.ToString();
        creationImage.GetComponentInChildren<Image>().sprite = unit.data.image;
    }

    public void ButtonHoverExit()
    {
        creationInfo.SetActive(false);
        creationGoldCost.SetActive(true);
        creationFoodCost.SetActive(true);
        creationWoodCost.SetActive(true);
        creationStoneCost.SetActive(true);
        creationMetalCost.SetActive(true);
    }
}