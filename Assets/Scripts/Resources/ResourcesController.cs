
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

    private int gold = 10000;
    private int food = 10000;
    private int wood = 10000;
    private int stone = 10000;
    private int metal = 10000;
    private int population = 0;

    private int goldGain;
    private int foodGain;
    private int woodGain;
    private int stoneGain;
    private int metalGain;
    private int maxPopulation;

    private float goldTimer;
    private float foodTimer;
    private float woodTimer;
    private float stoneTimer;
    private float metalTimer;

    private void Start()
    {
        UpdateText(goldTxt, gold);
        UpdateText(foodTxt, food);
        UpdateText(woodTxt, wood);
        UpdateText(stoneTxt, stone);
        UpdateText(metalTxt, metal);
        UpdateText(populationTxt, population);
        AddMaxPopulation(50);
    }

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

    public void AddResourcesGain(int goldGain, int foodGain, int woodGain, int stoneGain, int metalGain, int maxPopulation)
    {
        this.goldGain += goldGain;
        this.foodGain += foodGain;
        this.woodGain += woodGain;
        this.stoneGain += stoneGain;
        this.metalGain += metalGain;
        AddMaxPopulation(maxPopulation);
    }

    public void SubstractResources(int goldCost, int foodCost, int woodCost, int stoneCost, int metalCost, int populationCost)
    {
        gold -= goldCost;
        UpdateText(goldTxt, gold);

        food -= foodCost;
        UpdateText(foodTxt, food);

        wood -= woodCost;
        UpdateText(woodTxt, wood);

        stone -= stoneCost;
        UpdateText(stoneTxt, stone);

        metal -= metalCost;
        UpdateText(metalTxt, metal);

        population += populationCost;
        UpdateText(populationTxt, population);
    }

    public bool CheckIfEnoughResources(int goldCost, int foodCost, int woodCost, int stoneCost, int metalCost, int populationCost)
    {
        if (gold < goldCost || food < foodCost || wood < woodCost || stone < stoneCost || metal < metalCost || population + populationCost > maxPopulation)
            return false;
        else
            return true;
    }

    public void AddPopulation(int population)
    {
        this.population += population;
        UpdateText(populationTxt, this.population);
    }

    private void AddMaxPopulation(int maxPopulation)
    {
        this.maxPopulation += maxPopulation;
        maxPopulationTxt.text = " / " + this.maxPopulation;
    }

    private void UpdateText(TextMeshProUGUI text, int amount)
    {
        text.text = " " + amount;
    }
}