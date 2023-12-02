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

    [Header("Level 1")]
    public int healthPoints1;
    public int goldCost1;
    public int foodCost1;
    public int woodCost1;
    public int stoneCost1;
    public int metalCost1;
    public int goldGain1;
    public int foodGain1;
    public int woodGain1;
    public int stoneGain1;
    public int metalGain1;
    public int populationGain1;

    [Header("Level 2")]
    public int healthPoints2;
    public int goldCost2;
    public int foodCost2;
    public int woodCost2;
    public int stoneCost2;
    public int metalCost2;
    public int goldGain2;
    public int foodGain2;
    public int woodGain2;
    public int stoneGain2;
    public int metalGain2;
    public int populationGain2;

    [Header("Level 3")]
    public int healthPoints3;
    public int goldCost3;
    public int foodCost3;
    public int woodCost3;
    public int stoneCost3;
    public int metalCost3;
    public int goldGain3;
    public int foodGain3;
    public int woodGain3;
    public int stoneGain3;
    public int metalGain3;
    public int populationGain3;    
}