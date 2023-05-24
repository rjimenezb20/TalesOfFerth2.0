using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour, IBuildingLevel
{
    private int level = 1;
    private Building building;
    private ResourcesController RC;
    
    void Start()
    {
        building = GetComponent<Building>();
        RC = FindObjectOfType<ResourcesController>();
    }

    public void Level1()
    {
        level = 1;
        RC.goldGain += building.data.goldGain;
        RC.AddMaxPopulation(building.data.populationGain);
    }

    public void Level2()
    {
        level = 2;
    }

    public void Level3()
    {
        level = 3;
    }
}