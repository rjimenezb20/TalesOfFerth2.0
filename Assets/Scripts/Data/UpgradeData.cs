using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeData", menuName = "New Upgrade")]
public class UpgradeData : ScriptableObject
{
    [Header("General")]
    public float upgradeTime;
    public string upgradeName;
    public string upgradeDescription;
    public Sprite upgradeImage;

    [Header("ResourcesCost")]
    public int goldCost;
    public int foodCost;
    public int woodCost;
    public int stoneCost;
    public int metalCost;
    public int populationCost;
}
