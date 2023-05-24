using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildingData", menuName = "New Building")]
public class BuildingData : ScriptableObject
{
    [Header("Building")]
    public string buildingName;
    public int healthPoints;

    [Header("Level 2")]
    public int level2Health;
    public int level2ResourceIncrease;

    [Header("Level 2")]
    public int level3Health;
    public int level3ResourceIncrease;

    [Header("Resources Cost")]
    public int goldCost;
    public int foodCost;
    public int woodCost;
    public int stoneCost;
    public int metalCost;

    [Header("Resources Gain")]
    public int goldGain;
    public int foodGain;
    public int woodGain;
    public int stoneGain;
    public int metalGain;
    public int populationGain;
}