using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "EnemyData", menuName = "New Enemy")]
public class EnemyData : ScriptableObject
{
    [Header("Enemy")]
    public string unitName;
    public int healthPoints;
}