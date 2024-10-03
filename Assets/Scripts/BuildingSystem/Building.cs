using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class Building : MonoBehaviour
{ 
    public BuildingData dataLvl1;
    public BuildingData dataLvl2;
    public BuildingData dataLvl3;
    public bool Placed = false;
    public bool Updating = false;
    public BoundsInt area;
    public Vector3Int Size { get; private set; }
    public GameObject selectionSquare;
    public GameObject vision;
    [HideInInspector] public bool selected = false;
    [HideInInspector] public int buildingLevel = 0;

    [Header("Building Models")]
    public GameObject level1;
    public GameObject level2;
    public GameObject level3;

    [Header("Materials")]
    public Material normalMat;
    public Material onConstructionMat;

    [Header("Destroy")]
    public GameObject destroyPartycles;
    public Vector3 particleScale;

    private TextMeshProUGUI quantityText;
    private ResourcesController RC;
    private Renderer rend;
    private FogProjector fogProjector;
    private BuildingController BC;
    private UIManager UIM;

    void Start()
    {
        RC = FindAnyObjectByType<ResourcesController>();
        BC = FindAnyObjectByType<BuildingController>();
        UIM = FindAnyObjectByType<UIManager>();
        rend = GetComponentInChildren<Renderer>();
        fogProjector = FindAnyObjectByType<FogProjector>();

        if (GameObject.FindWithTag("Quantity") != null)
            quantityText = GameObject.FindWithTag("Quantity").GetComponentInChildren<TextMeshProUGUI>();
    }

    public void RotateBuilding(Quaternion rotation)
    {
        transform.rotation = rotation;
    }

    public void RotateTiles()
    {
        area = new BoundsInt(new Vector3Int(area.position.y, area.position.x, area.position.z), new Vector3Int(area.size.y, area.size.x, area.size.z));
    }

    public void ConstructionFinish()
    {
        Component[] componentes = GetComponents<Component>();

        foreach (Component componente in componentes)
            if (componente is Behaviour && !(componente as Behaviour).enabled)
                (componente as Behaviour).enabled = true;

        GetComponent<Health>().enabled = true;

        Placed = true;
        ChangeMatToNormal();

        if (dataLvl1.buildingName == "Sawmill" || dataLvl1.buildingName == "Mine")
            GetComponent<SphereCollider>().enabled = false;

        vision.gameObject.SetActive(true);
        GetComponent<Health>().enabled = true;
    }

    public void SetSelected(bool a)
    {
        selectionSquare.SetActive(a);
        selected = a;

        if (dataLvl1.buildingName == "Barracks")
            GetComponent<UnitSpawnPoint>().movePoint.gameObject.SetActive(a);
    }

    public void Level2()
    {
        level1.SetActive(false);
        level2.SetActive(true);

        RC.AddResourcesGain(dataLvl2.goldGain, dataLvl2.foodGain, 0, 0, 0, dataLvl2.populationGain);

        Health health = GetComponent<Health>();
        health.HP = dataLvl2.healthPoints;
        health.MaxHP = dataLvl2.healthPoints;

        if (dataLvl1.buildingName == "Tower")
            GetComponent<Turret>().SetStats(dataLvl2.attackDamage, dataLvl2.attackRange, 1);

        buildingLevel = 2;
    }

    public void Level3()
    {
        if (level3 != null)
        {
            level2.SetActive(false);
            level3.SetActive(true);
        }

        RC.AddResourcesGain(dataLvl3.goldGain, dataLvl3.foodGain, 0, 0, 0, dataLvl3.populationGain);

        Health health = GetComponent<Health>();
        health.HP = dataLvl3.healthPoints;
        health.MaxHP = dataLvl3.healthPoints;

        buildingLevel = 3;
    }

    public void LevelUp()
    {
        ChangeMatToNormal();

        Updating = false;

        if (buildingLevel == 2)
            Level2();
        else if (buildingLevel == 3) 
            Level3();
    }

    public void ResourcesLostAndGains()
    {
        int woodGain = 0;
        int stoneGain = 0;
        int metalGain = 0;

        switch (buildingLevel)
        {
            case 0:
                if (dataLvl1.buildingName == "Sawmill")
                    woodGain = GetComponent<Sawmill>().OnPlace() * dataLvl1.woodGain;

                if (dataLvl1.buildingName == "Mine")
                {
                    stoneGain = GetComponent<Mine>().OnPlace()[0] * dataLvl1.stoneGain;
                    metalGain = GetComponent<Mine>().OnPlace()[1] * dataLvl1.metalGain;
                }
                    
                RC.AddResourcesGain(dataLvl1.goldGain, dataLvl1.foodGain, woodGain, stoneGain, metalGain, dataLvl1.populationGain);
                RC.SubstractResources(dataLvl1.goldCost, dataLvl1.foodCost, dataLvl1.woodCost, dataLvl1.stoneCost, dataLvl1.metalCost, 0);
                buildingLevel = 1;
                break;

            case 1:
                if (dataLvl1.buildingName == "Sawmill")
                    woodGain = GetComponent<Sawmill>().OnPlace() * dataLvl2.woodGain;

                if (dataLvl1.buildingName == "Mine")
                {
                    stoneGain = GetComponent<Mine>().OnPlace()[0] * dataLvl2.stoneGain;
                    metalGain = GetComponent<Mine>().OnPlace()[1] * dataLvl2.metalGain;
                }

                RC.AddResourcesGain(dataLvl2.goldGain, dataLvl2.foodGain, woodGain, stoneGain, metalGain, dataLvl2.populationGain);
                RC.SubstractResources(dataLvl2.goldCost, dataLvl2.foodCost, dataLvl2.woodCost, dataLvl2.stoneCost, dataLvl2.metalCost, 0);
                buildingLevel = 2;
                break;

            case 2:
                if (dataLvl1.buildingName == "Sawmill")
                    woodGain = GetComponent<Sawmill>().OnPlace() * dataLvl3.woodGain;

                if (dataLvl1.buildingName == "Mine")
                {
                    stoneGain = GetComponent<Mine>().OnPlace()[0] * dataLvl3.stoneGain;
                    metalGain = GetComponent<Mine>().OnPlace()[1] * dataLvl3.metalGain;
                }

                RC.AddResourcesGain(dataLvl3.goldGain, dataLvl3.foodGain, woodGain, stoneGain, metalGain, dataLvl3.populationGain);
                RC.SubstractResources(dataLvl3.goldCost, dataLvl3.foodCost, dataLvl3.woodCost, dataLvl3.stoneCost, dataLvl3.metalCost, 0);
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
        rend.material = normalMat;

        if (dataLvl1.buildingName == "Farm")
        {
            Renderer renderer = transform.GetChild(0).GetComponent<Renderer>();

            if (renderer.gameObject.layer == LayerMask.NameToLayer("Building"))
                renderer.material = normalMat;

            return;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
            if (renderer.gameObject.layer == LayerMask.NameToLayer("Building"))
                renderer.material = normalMat;
    }

    public void ChangeMatToOnConstruction()
    {
        rend.material = onConstructionMat;

        if (dataLvl1.buildingName == "Farm")
        {
            Renderer renderer = transform.GetChild(0).GetComponent<Renderer>();

            if (renderer.gameObject.layer == LayerMask.NameToLayer("Building"))
                renderer.material = onConstructionMat;

            return;
        }

        Renderer[] renderers = GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
            if (renderer.gameObject.layer == LayerMask.NameToLayer("Building"))
                renderer.material = onConstructionMat;
    }

    public void OnPlace()
    {
        BoxCollider collider = GetComponent<BoxCollider>();
        if (collider != null)
            collider.enabled = true;

        NavMeshObstacle obstacle = GetComponent<NavMeshObstacle>();
        if (obstacle != null)
            obstacle.enabled = true;

        if (dataLvl1.buildingName == "Wall")
            StartCoroutine(GetComponent<Wall>().BuildWall());

        if (dataLvl1.buildingName == "WallDoor")
        {
            foreach (Wall walls in GetComponentsInChildren<Wall>())
                StartCoroutine(walls.BuildWall());

            GetComponentInChildren<SphereCollider>().enabled = true;
        }

        ChangeMatToOnConstruction();
    }

    public void Destroy()
    {
        GameObject particle = Instantiate(destroyPartycles, transform.position + new Vector3(0, 1.5f, 0), Quaternion.identity);
        particle.transform.localScale = particleScale;

        BC.TakeAreaBack(this);
        fogProjector.StartUpdateCorroutine();

        Destroy(gameObject);

        if (dataLvl1.buildingName == "TownHall")
            UIM.Lose();
    }
}