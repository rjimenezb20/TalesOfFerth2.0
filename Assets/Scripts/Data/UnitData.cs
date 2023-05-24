using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitData", menuName = "New Unit")]
public class UnitData : ScriptableObject
{
    [Header("Unit")]
    public string unitName;
    public int healthPoints;
    public int attackDamage;

}