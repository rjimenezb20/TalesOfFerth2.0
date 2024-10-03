 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingController : MonoBehaviour
{
    public static BuildingController current;
    public GridLayout gridLayout;
    public GameObject warnings;

    [HideInInspector] public BoundsInt buildingArea;
    [HideInInspector] public bool buildingMode = false;

    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap tempTilemap;
    [SerializeField] private TileBase whitetile;
    [SerializeField] private TileBase whiteBuildedtile;
    [SerializeField] private TileBase greentile;
    [SerializeField] private TileBase redtile;

    private GameObject buildingBackUp;
    private Building buildingToPlace;
    private UIManager UIM;
    private BoundsInt prevArea;
    private static int groundLayer;
    private bool rotated = false;
    private SelectionController SC;
    private ResourcesController RC;
    private FogProjector FP;

    public void Awake()
    {
        current = this;
        groundLayer = LayerMask.GetMask("Ground");
        UIM = FindAnyObjectByType<UIManager>();
        SC = FindAnyObjectByType<SelectionController>();
        RC = FindAnyObjectByType<ResourcesController>();
        FP = FindAnyObjectByType<FogProjector>();
    }

    public void LateUpdate()
    {
        if(buildingMode)
        {
            if (CanBePlaced()) //EFICIENCIA (?)
            {
                if (Input.GetMouseButton(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    if (Physics.Raycast(ray, out RaycastHit raycastHit, 1000f, groundLayer))
                    {
                        if (!FP.IsVisiblePosition(raycastHit.point))
                        {
                            if (RC.CheckIfEnoughResources(buildingToPlace.dataLvl1.goldCost, buildingToPlace.dataLvl1.foodCost, buildingToPlace.dataLvl1.woodCost, buildingToPlace.dataLvl1.stoneCost, buildingToPlace.dataLvl1.metalCost, 0))
                            {
                                buildingToPlace.OnPlace();
                                buildingToPlace.ResourcesLostAndGains();

                                StartCoroutine(buildingToPlace.GetComponent<ProgressBar>().StartBuildingTimer(buildingToPlace.gameObject, false));

                                ClearArea();
                                TakeArea();

                                buildingBackUp.transform.rotation = buildingToPlace.transform.rotation;

                                buildingToPlace = null;

                                InstanciateObjectWithRotation(buildingBackUp, buildingBackUp.transform.rotation);

                                if (rotated)
                                {
                                    buildingToPlace.RotateTiles();
                                    buildingToPlace.RotateBuilding(buildingBackUp.transform.rotation);
                                }
                            }
                            else
                            {
                                warnings.GetComponent<Animation>().Play("ResourcesWarning");
                            }
                        }
                        else
                        {
                            warnings.GetComponent<Animation>().Play("VisionWarning");
                        }
                    }
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                buildingToPlace.RotateTiles();
                buildingToPlace.RotateBuilding(buildingToPlace.transform.rotation * Quaternion.Euler(0, 90, 0));
                
                if (rotated)
                    rotated = false;
                else 
                    rotated = true;
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonUp(1))
            {
                ClearArea();
                Destroy(buildingToPlace.gameObject);
                UIM.ShowHideQuantity(false, 0);
                buildingMode = false;
                gridLayout.gameObject.SetActive(false);
            }
        }
    }

    private void ClearArea()
    {
        TileBase[] toClear = new TileBase[prevArea.size.x * prevArea.size.y * prevArea.size.z];
        FillTiles(toClear, null);
        tempTilemap.SetTilesBlock(prevArea, toClear);
    }

    public void InstanciateObject(GameObject prefab)
    {
        SC.ClearSelection();

        if (!buildingToPlace)
        {
            gridLayout.gameObject.SetActive(true);
            GameObject obj = Instantiate(prefab, GetMouseWorldPositon(groundLayer).point, Quaternion.identity);
            buildingToPlace = obj.GetComponent<Building>();
            buildingBackUp = prefab;
            buildingMode = true;
        }
    }

    public void InstanciateObjectWithRotation(GameObject prefab, Quaternion rotation)
    {
        if (!buildingToPlace)
        {
            gridLayout.gameObject.SetActive(true);
            GameObject obj = Instantiate(prefab, GetMouseWorldPositon(groundLayer).point, rotation);
            buildingToPlace = obj.GetComponent<Building>();
            buildingBackUp = prefab;
            buildingMode = true;
        }
    }

    public void BuildingLevelUp(GameObject button)
    {
        if (SC.selectedBuilding.Updating == false)
        {
            button.gameObject.SetActive(false);
            UIM.ButtonHoverExit();
            SC.selectedBuilding.ResourcesLostAndGains();
            StartCoroutine(SC.selectedBuilding.GetComponent<ProgressBar>().StartBuildingTimer(SC.selectedBuilding.gameObject, true));
            SC.selectedBuilding.ChangeMatToOnConstruction();
            SC.selectedBuilding.Updating = true;
        }
    }

    private bool CanBePlaced()
    {
        ClearArea();

        bool canBePlaced = true;

        buildingToPlace.area.position = gridLayout.WorldToCell(Vector3Int.RoundToInt(GetMouseWorldPositon(groundLayer).point)) - Vector3Int.RoundToInt(new Vector3(buildingToPlace.area.size.x / 2, buildingToPlace.area.size.y / 2, buildingToPlace.area.size.z / 2));
        buildingArea = buildingToPlace.area;
        buildingToPlace.transform.position = gridLayout.CellToWorld(buildingToPlace.area.position) + gridLayout.CellToWorld(new Vector3Int(buildingToPlace.area.size.x, buildingToPlace.area.size.y, 0)) / 2 - new Vector3(0, gridLayout.CellToWorld(new Vector3Int(buildingToPlace.area.size.x, buildingToPlace.area.size.y, 0)).y, 0) / 2;

        TileBase[] baseArray = GetTilesBlock(buildingArea, mainTilemap);

        int size = baseArray.Length;
        TileBase[] tileArray = new TileBase[size];

        for (int i = 0; i < baseArray.Length; i++)
        {
            if (baseArray[i] == whitetile)
            {
                tileArray[i] = greentile;
            }
            else
            {
                tileArray[i] = redtile;
            }
        }

        tempTilemap.SetTilesBlock(buildingArea, tileArray);
        prevArea = buildingArea;

        for (int i = 0; i < tileArray.Length; i++)
        {
            if (tileArray[i] == redtile)
            {
                canBePlaced = false;
            }
        }
        return canBePlaced;
    }

    private void TakeArea()
    {
        BoundsInt buildingArea = buildingToPlace.area;
        TileBase[] tileArray = GetTilesBlock(buildingToPlace.area, mainTilemap);

        FillTiles(tileArray, whiteBuildedtile);
        mainTilemap.SetTilesBlock(buildingArea, tileArray);
    }

    public void TakeAreaBack(Building placedBuilding)
    {
        BoundsInt buildingArea = placedBuilding.area;
        TileBase[] tileArray = GetTilesBlock(placedBuilding.area, mainTilemap);

        FillTiles(tileArray, whitetile);
        mainTilemap.SetTilesBlock(buildingArea, tileArray);
    }

    //Utils ---------------------------------------------------
    public static RaycastHit GetMouseWorldPositon(LayerMask layer)
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 1000f, layer))
        {
            return raycastHit;
        }
        else
        {
            return raycastHit;
        }
    }

    private static TileBase[] GetTilesBlock(BoundsInt area, Tilemap tilemap)
    {
        TileBase[] array = new TileBase[area.size.x * area.size.y * area.size.z];
        int counter = 0;

        foreach (var v in area.allPositionsWithin)
        {
            Vector3Int pos = new Vector3Int(v.x, v.y, v.z);
            array[counter] = tilemap.GetTile(pos);
            counter++;
        }
        return array;
    }
    private static void FillTiles(TileBase[] array, TileBase type)
    {
        for (int i = 0; i < array.Length; i++)
        {
            array[i] = type;
        }
    }

    public Building GetBuildingToPlace()
    {
        return buildingToPlace;
    }
}