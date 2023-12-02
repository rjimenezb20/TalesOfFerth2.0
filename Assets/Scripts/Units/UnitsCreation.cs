using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitsCreation : MonoBehaviour
{
    public Unit soldierPrefab;
    public Unit archerPrefab;
    public Unit magePrefab;

    private SelectionController SC;

    void Start()
    {
        SC = GetComponent<SelectionController>();
    }

    public void StartCreation(Unit unit)
    {
        Barracks currentBarrack = SC.selectedBuilding.GetComponent<Barracks>();
        currentBarrack.AddUnitToQueue(unit);
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
