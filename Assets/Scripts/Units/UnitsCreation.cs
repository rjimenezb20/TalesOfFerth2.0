using UnityEngine;

public class UnitsCreation : MonoBehaviour
{
    public GameObject warnings;

    public Unit soldierPrefab;
    public Unit archerPrefab;
    public Unit magePrefab;

    private SelectionController SC;
    private ResourcesController RC;

    void Start()
    {
        SC = GetComponent<SelectionController>();
        RC = FindAnyObjectByType<ResourcesController>();
    }

    public void StartCreation(Unit unit)
    {
        Barracks currentBarrack = SC.selectedBuilding.GetComponent<Barracks>();

        if (RC.CheckIfEnoughResources(unit.data.goldCost, unit.data.foodCost, unit.data.woodCost, unit.data.stoneCost, unit.data.metalCost, unit.data.populationCost))
        {
            RC.SubstractResources(unit.data.goldCost, unit.data.foodCost, unit.data.woodCost, unit.data.stoneCost, unit.data.metalCost, unit.data.populationCost);
            currentBarrack.AddUnitToQueue(unit);
        } 
        else
        {
            warnings.GetComponent<Animation>().Play("ResourcesWarning");
        }
    }

    public void SpawnSoldier(Building building)
    {
        Unit unit = Instantiate(soldierPrefab, building.GetComponent<UnitSpawnPoint>().spawnPoint.position, Quaternion.identity);
        MoveUnitsToPoint(unit, building);
    }

    public void SpawnArcher(Building building)
    {
        Unit unit = Instantiate(archerPrefab, building.GetComponent<UnitSpawnPoint>().spawnPoint.position, Quaternion.identity);
        MoveUnitsToPoint(unit, building);
    }

    public void SpawnMage(Building building)
    {
        Unit unit = Instantiate(magePrefab, building.GetComponent<UnitSpawnPoint>().spawnPoint.position, Quaternion.identity);
        MoveUnitsToPoint(unit, building);
    }

    private void MoveUnitsToPoint(Unit unit, Building building)
    {
        unit.GetComponent<UnitMovement>().MoveOrder(building.GetComponent<UnitSpawnPoint>().movePoint.position);
    }
}
