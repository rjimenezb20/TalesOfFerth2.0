using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "New Unit")]
public class UnitData : ScriptableObject
{
    [Header("UI")]
    public Sprite image;
    public string unitName;
    public string description;

    [Header("Stats")]
    public int healthPoints;
    public int attackDamage;
    public int attackRange;
    public float creationTime;

    [Header("ResourcesCost")]
    public int goldCost;
    public int foodCost;
    public int woodCost;
    public int stoneCost;
    public int metalCost;
    public int populationCost;
}