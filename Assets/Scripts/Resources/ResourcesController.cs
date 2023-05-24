using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ResourcesController : MonoBehaviour
{
    [Header("Resouces Texts")]
    public TextMeshProUGUI goldTxt;
    public TextMeshProUGUI foodTxt;
    public TextMeshProUGUI woodTxt;
    public TextMeshProUGUI stoneTxt;
    public TextMeshProUGUI metalTxt;
    public TextMeshProUGUI populationTxt;
    public TextMeshProUGUI maxPopulationTxt;

    [Header("Gain Timers")]
    public float goldGainTime;
    public float foodGainTime;
    public float woodGainTime;
    public float stoneGainTime;
    public float metalGainTime;

    [HideInInspector] public int goldGain;
    [HideInInspector] public int foodGain;
    [HideInInspector] public int woodGain;
    [HideInInspector] public int stoneGain;
    [HideInInspector] public int metalGain;
    [HideInInspector] public int maxPopulation;

    private int gold;
    private int food;
    private int wood;
    private int stone;
    private int metal;
    private int population;

    private float goldTimer;
    private float foodTimer;
    private float woodTimer;
    private float stoneTimer;
    private float metalTimer;


    private void Update()
    {
        goldTimer += Time.deltaTime % 60;
        foodTimer += Time.deltaTime % 60;
        woodTimer += Time.deltaTime % 60;
        stoneTimer += Time.deltaTime % 60;
        metalTimer += Time.deltaTime % 60;

        //Gold
        if(goldGain > 0)
        {
            if (goldTimer > goldGainTime)
            {
                gold += goldGain;
                goldTimer = 0;
                UpdateText(goldTxt, gold);
            }
        }

        //Food
        if (foodGain > 0)
        {
            if (foodTimer > foodGainTime)
            {
                food += foodGain;
                foodTimer = 0;
                UpdateText(foodTxt, food);
            }
        }

        //Wood
        if (woodGain > 0)
        {
            if (woodTimer > woodGainTime)
            {
                wood += woodGain;
                woodTimer = 0;
                UpdateText(woodTxt, wood);
            }
        }

        //Stone
        if (stoneGain > 0)
        {
            if (stoneTimer > stoneGainTime)
            {
                stone += stoneGain;
                stoneTimer = 0;
                UpdateText(stoneTxt, stone);
            }
        }

        //Metal
        if (metalGain > 0)
        {
            if (metalTimer > metalGainTime)
            {
                metal += metalGain;
                metalTimer = 0;
                UpdateText(metalTxt, metal);
            }
        }
    }

    public void AddPopulation(int population)
    {
        this.population += population;
        UpdateText(populationTxt, this.population);
    }

    public void AddMaxPopulation(int maxPopulation)
    {
        this.maxPopulation += maxPopulation;
        maxPopulationTxt.text = " / " + this.maxPopulation;
    }

    private void UpdateText(TextMeshProUGUI text, int amount)
    {
        text.text = " " + amount;
    }
}