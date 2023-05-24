using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingController : MonoBehaviour
{
    public static BuildingController current;

    public BoundsInt buildingArea;
    public GridLayout gridLayout;
    private Grid grid;

    [SerializeField] private Tilemap mainTilemap;
    [SerializeField] private Tilemap tempTilemap;
    [SerializeField] private TileBase whitetile;
    [SerializeField] private TileBase whiteBuildedtile;
    [SerializeField] private TileBase greentile;
    [SerializeField] private TileBase redtile;

    private Building buildingToPlace;
    private GameObject buildingBackUp;
    private BoundsInt prevArea;

    private static int groundLayer;
    private bool buildingInstanciated = false;
    private bool buildingMode = false;

    public void Awake()
    {
        current = this;
        grid = gridLayout.gameObject.GetComponent<Grid>();
        groundLayer = LayerMask.GetMask("Ground");
    }
    public void FixedUpdate()
    {
        //Avoid null reference error when clicking build button
        if (!buildingToPlace)
        {
            return;
        }

        buildingInstanciated = true;
    }

    public void Update()
    {
        if (!buildingInstanciated)
        {
            return;
        }

        if(buildingMode)
        {
            if (CanBePlaced()) //EFICIENCIA (?)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    buildingToPlace.Place();
                    ClearArea();
                    TakeArea();

                    buildingBackUp.transform.rotation = buildingToPlace.transform.rotation;

                    buildingInstanciated = false;
                    buildingToPlace = null;

                    InstanciateObjectWithRotation(buildingBackUp, buildingBackUp.transform.rotation);
                }
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                buildingToPlace.Rotate();
            }

            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonUp(1))
            {
                ClearArea();
                Destroy(buildingToPlace.gameObject);
                buildingInstanciated = false;
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
        if (!buildingToPlace)
        {
            gridLayout.gameObject.SetActive(true);
            GameObject obj = Instantiate(prefab, GetMouseWorldPositon(), Quaternion.identity);
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
            GameObject obj = Instantiate(prefab, GetMouseWorldPositon(), rotation);
            buildingToPlace = obj.GetComponent<Building>();
            buildingBackUp = prefab;
            buildingMode = true;
        }
    }

    
    private bool CanBePlaced()
    {
        ClearArea();

        bool canBePlaced = true;

        buildingToPlace.area.position = gridLayout.WorldToCell(Vector3Int.RoundToInt(GetMouseWorldPositon())) - Vector3Int.RoundToInt(new Vector3(buildingToPlace.area.size.x / 2, buildingToPlace.area.size.y / 2, buildingToPlace.area.size.z / 2));
        buildingArea = buildingToPlace.area;

        buildingToPlace.gameObject.transform.position = gridLayout.CellToWorld(buildingToPlace.area.position) + gridLayout.CellToWorld(new Vector3Int(buildingToPlace.area.size.x, buildingToPlace.area.size.y, 0)) / 2 - new Vector3(0, gridLayout.CellToWorld(new Vector3Int(buildingToPlace.area.size.x, buildingToPlace.area.size.y, 0)).y, 0) / 2;

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

    //Utils ---------------------------------------------------
    public static Vector3 GetMouseWorldPositon()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit raycastHit, 1000f, groundLayer))
        {
            return raycastHit.point;
        }
        else
        {
            return Vector3.zero;
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
}