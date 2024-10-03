using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [Header("Menu")]
    public GameObject inGameMenu;
    public GameObject victoryPanel;
    public GameObject losePanel;

    [Header("Utilities")]
    public GameObject utilities;
    public GameObject WatchTower;
    public GameObject Tower;
    public GameObject House;
    public GameObject Sawmill;
    public GameObject Barracks;
    public GameObject Smith;
    public GameObject Mine;
    public GameObject DestroyBuilding;

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
    public GameObject quantityTreeIcon;
    public GameObject quantityMineIcon;
    public Vector3 quantityPosition;

    [Header("Unit Queue")]
    public List<Image> unitQueueImgs;

    [Header("Creation Info")]
    public GameObject creationInfoPanel;
    public Vector3 creationInfoPanelOffset;
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

    [Header("Building Upgrades")]
    [Space(10)]

    public GameObject UpgradeQueuePanel;

    [Header("House Upgrades")]
    public GameObject LevelUpHouseBt;

    [Header("Smith Upgrades")]
    public GameObject Level2UpgradeBt;
    public GameObject Level3UpgradeBt;

    [Header("Barracks Upgrades")]
    public GameObject LevelUpBarracksBt;

    [Header("Tower Upgrades")]
    public GameObject LevelUpTowerBt;

    private GameObject activeUtility;
    private Unit currentUnit;
    private Building currentBuilding;
    private bool menuOpen = false;
    private UpgradesController UC;
    private SelectionController SC;
    private bool gameEnd = false;

    private void Start()
    {
        UC = GetComponent<UpgradesController>();
        SC = FindAnyObjectByType<SelectionController>();
    }

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

        if (Input.GetKeyDown(KeyCode.Escape) && !gameEnd) {
            OpenMenu();
        }
    }

    public void ShowUtilities(string name, Building building)
    {
        if (activeUtility != null)
        {
            activeUtility.SetActive(false);
            DestroyBuilding.SetActive(false);
        }

        if (name != null)
        {
            DestroyBuilding.SetActive(true);

            if (name == "WatchTower")
            {
                WatchTower.SetActive(true);
                activeUtility = WatchTower;
            }
            else if (name == "Tower")
            {
                Tower.SetActive(true);
                activeUtility = Tower;

                UpgradeButtonManage(building, Tower);
            }
            else if (name == "House")
            {
                House.SetActive(true);
                activeUtility = House;

                UpgradeButtonManage(building, House);
            }
            else if (name == "Barracks")
            {
                Barracks.SetActive(true);
                activeUtility = Barracks;

                if (UC.Level2Upgrade)
                {
                    Barracks.transform.GetChild(3).gameObject.SetActive(true);
                }

                if (UC.Level3Upgrade)
                {
                    Barracks.transform.GetChild(4).gameObject.SetActive(true);
                }

                UpgradeButtonManage(building, Barracks);
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
    }

    private void UpgradeButtonManage(Building building, GameObject activeUtility)
    {
        if (building.buildingLevel == 1 && UC.Level2Upgrade)
        {
            activeUtility.transform.GetChild(0).gameObject.SetActive(true);
        }
        else if (building.buildingLevel == 2 && UC.Level3Upgrade)
        {
            activeUtility.transform.GetChild(0).gameObject.SetActive(false);
            activeUtility.transform.GetChild(1).gameObject.SetActive(true);
        }
        else
        {
            activeUtility.transform.GetChild(0).gameObject.SetActive(false);
            activeUtility.transform.GetChild(1).gameObject.SetActive(false);
        }
    }

    public void ShowUnitInfo(Unit unit)
    {
        currentBuilding = null;
        currentUnit = unit;

        Image.sprite = currentUnit.data.portrait;
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

        Image.sprite = currentBuilding.dataLvl1.image;
        Name.text = currentBuilding.dataLvl1.buildingName;
        Description.text = currentBuilding.dataLvl1.description;
        MaxHp.text = "/ " + health.MaxHP.ToString();
        Hp.text = health.HP.ToString();
        Attack.text = currentBuilding.dataLvl1.attackDamage.ToString();
        Range.text = currentBuilding.dataLvl1.attackRange.ToString();
        slider.value = (float)health.HP / health.MaxHP;
    }

    public void ShowHideQuantity(bool a, int b)
    {
        quantity.SetActive(a);
        quantityTreeIcon.SetActive(false);
        quantityMineIcon.SetActive(false);

        if (b == 0)
            quantityTreeIcon.SetActive(a);
        else if (b == 1)
            quantityMineIcon.SetActive(a);
    }

    public void ShowHideUnitQueue(bool a)
    {
        unitQueue.SetActive(a);
    }

    public void UpdateUpgradeImage(Sprite image)
    {
        UpgradeQueuePanel.transform.Find("Current").GetComponent<Image>().sprite = image;
    }

    public void UpdateUnitQueueImages(List<Unit> li)
    {
        foreach (Image img in unitQueueImgs)
        {
            Color color = img.color;
            color.a = 0;
            img.color = color;
        }

        for (int i = 0; i < li.Count; i++)
        {
            unitQueueImgs[i].sprite = li[i].data.image;

            Color color = unitQueueImgs[i].color;
            color.a = 1;
            unitQueueImgs[i].color = color;
        }
    }

    public void BuildingButtonHover(BuildingData data)
    {
        creationInfoPanel.SetActive(true);
        creationInfoPanel.transform.position = Input.mousePosition + creationInfoPanelOffset;
        creationName.GetComponent<TextMeshProUGUI>().text = data.buildingName;
        creationDescription.GetComponentInChildren<TextMeshProUGUI>().text = data.description;

        if (data.goldCost == 0)
            creationGoldCost.SetActive(false);
        else
            creationGoldCost.GetComponentInChildren<TextMeshProUGUI>().text = data.goldCost.ToString();

        if (data.foodCost == 0)
            creationFoodCost.SetActive(false);
        else
            creationFoodCost.GetComponentInChildren<TextMeshProUGUI>().text = data.foodCost.ToString();

        if (data.woodCost == 0)
            creationWoodCost.SetActive(false);
        else
            creationWoodCost.GetComponentInChildren<TextMeshProUGUI>().text = data.woodCost.ToString();

        if (data.stoneCost == 0)
            creationStoneCost.SetActive(false);
        else
            creationStoneCost.GetComponentInChildren<TextMeshProUGUI>().text = data.stoneCost.ToString();

        if (data.metalCost == 0)
            creationMetalCost.SetActive(false);
        else
            creationMetalCost.GetComponentInChildren<TextMeshProUGUI>().text = data.metalCost.ToString();

        if (data.buildingName == "Tower")
        {
            creationAttack.GetComponentInChildren<TextMeshProUGUI>().text = data.attackDamage.ToString();
            creationRange.GetComponentInChildren<TextMeshProUGUI>().text = data.attackRange.ToString();
            creationAttack.SetActive(true);
            creationRange.SetActive(true);
        } 
        else
        {
            creationAttack.SetActive(false);
            creationRange.SetActive(false);
        }

        creationHealth.SetActive(true);
        creationHealth.GetComponentInChildren<TextMeshProUGUI>().text = data.healthPoints.ToString();
        creationImage.GetComponentInChildren<Image>().sprite = data.image;
    }

    public void UnitButtonHover(Unit unit)
    {
        creationInfoPanel.SetActive(true);
        creationInfoPanel.transform.position = Input.mousePosition + creationInfoPanelOffset;

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

        creationHealth.SetActive(true);
        creationHealth.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.healthPoints.ToString();
        creationAttack.SetActive(true);
        creationAttack.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.attackDamage.ToString();
        creationRange.SetActive(true);
        creationRange.GetComponentInChildren<TextMeshProUGUI>().text = unit.data.attackRange.ToString();
        creationImage.GetComponentInChildren<Image>().sprite = unit.data.image;
    }

    public void UpgradeButtonHover(UpgradeData data)
    {
        creationInfoPanel.SetActive(true);
        creationInfoPanel.transform.position = Input.mousePosition + creationInfoPanelOffset;

        creationName.GetComponent<TextMeshProUGUI>().text = data.upgradeName;
        creationDescription.GetComponentInChildren<TextMeshProUGUI>().text = data.upgradeDescription;

        if (data.goldCost == 0)
            creationGoldCost.SetActive(false);
        else
            creationGoldCost.GetComponentInChildren<TextMeshProUGUI>().text = data.goldCost.ToString();

        if (data.foodCost == 0)
            creationFoodCost.SetActive(false);
        else
            creationFoodCost.GetComponentInChildren<TextMeshProUGUI>().text = data.foodCost.ToString();

        if (data.woodCost == 0)
            creationWoodCost.SetActive(false);
        else
            creationWoodCost.GetComponentInChildren<TextMeshProUGUI>().text = data.woodCost.ToString();

        if (data.stoneCost == 0)
            creationStoneCost.SetActive(false);
        else
            creationStoneCost.GetComponentInChildren<TextMeshProUGUI>().text = data.stoneCost.ToString();

        if (data.metalCost == 0)
            creationMetalCost.SetActive(false);
        else
            creationMetalCost.GetComponentInChildren<TextMeshProUGUI>().text = data.metalCost.ToString();

        creationHealth.SetActive(false);
    }

    public void ButtonHoverExit()
    {
        creationInfoPanel.SetActive(false);
        creationGoldCost.SetActive(true);
        creationFoodCost.SetActive(true);
        creationWoodCost.SetActive(true);
        creationStoneCost.SetActive(true);
        creationMetalCost.SetActive(true);
    }

    public void OpenMenu()
    {
        if (menuOpen)
        {
            ResumeGame();
            inGameMenu.SetActive(false);
            return;
        }

        PauseGame();
        inGameMenu.SetActive(true);
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        menuOpen = true;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        menuOpen = false;
    }

    public void ShowHideUpgradeQueuePanel(bool a)
    {
        UpgradeQueuePanel.SetActive(a);
    }

    public void Level2SmithUpgrade()
    {
        Level2UpgradeBt.SetActive(false);
        Level3UpgradeBt.SetActive(true);
    }

    public void Level3SmithUpgrade()
    {
        Level3UpgradeBt.SetActive(false);
    }

    public void Win()
    {
        gameEnd = true;
        inGameMenu.SetActive(true);
        victoryPanel.SetActive(true);
        inGameMenu.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
        inGameMenu.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);
        PauseGame();
    }

    public void Lose()
    {
        gameEnd = true;
        inGameMenu.SetActive(true);
        losePanel.SetActive(true);
        inGameMenu.transform.GetChild(1).GetChild(0).GetChild(0).gameObject.SetActive(false);
        inGameMenu.transform.GetChild(1).GetChild(0).GetChild(1).gameObject.SetActive(false);
        PauseGame();
    }
}