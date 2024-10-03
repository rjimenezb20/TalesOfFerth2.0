using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "BuildingData", menuName = "New Building")]
public class BuildingData : ScriptableObject
{
    [Header("UI")]
    public Sprite image;
    public string buildingName;
    public string description;

    [Header("Building Time")]
    public float timeToBuild;
    public float timeToUpdate;

    [Header("Stats")]
    public int healthPoints;
    public int attackDamage;
    public int attackRange;

    [Header("Costs")]
    public int goldCost;
    public int foodCost;
    public int woodCost;
    public int stoneCost;
    public int metalCost;

    [Header("Gains")]
    public int goldGain;
    public int foodGain;
    public int woodGain;
    public int stoneGain;
    public int metalGain;
    public int populationGain;
   
    [Header("+")]
    public int enemyAttackDistanceOffset;
}