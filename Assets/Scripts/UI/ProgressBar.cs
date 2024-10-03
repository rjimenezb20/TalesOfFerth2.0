using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    public GameObject buildingCanvas;
    [HideInInspector] public bool creating;
    
    private GameObject unitCanvas;
    private GameObject upgradeCanvas;
    private float timer = 0;
    private bool ready = false;
    private UnitsCreation UC;
    private UIManager UIM;

    private void Start()
    { 
        UC = FindAnyObjectByType<UnitsCreation>();
        UIM = FindAnyObjectByType<UIManager>();
        unitCanvas = UIM.unitQueue;
        upgradeCanvas = UIM.UpgradeQueuePanel;
    }

    private void Update()
    {
        if (buildingCanvas != null)
            if (buildingCanvas.activeSelf)
                buildingCanvas.transform.LookAt(buildingCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public IEnumerator StartBuildingTimer(GameObject current, bool isLevelUp)
    {
        buildingCanvas.SetActive(true);

        Building building = current.GetComponent<Building>();

        if (!isLevelUp)
        {
            StartCoroutine(Timer(building.dataLvl1.timeToBuild, buildingCanvas));
        } 
        else
        {
            StartCoroutine(Timer(building.dataLvl1.timeToUpdate, buildingCanvas));
        }
            
        yield return new WaitUntil(() => ready == true);

        if (!isLevelUp)
        {
            building.ConstructionFinish();
        }
        else
        {
            building.LevelUp();
        }
        
        buildingCanvas.SetActive(false);
        ready = false;
        timer = 0;
    }

    public IEnumerator StartUnitTimer(float time, string unit, Building building)
    {
        StartCoroutine(Timer(time, unitCanvas));

        yield return new WaitUntil(() => ready == true);

        if (unit == "Soldier")
        {
            UC.SpawnSoldier(building);
        }
        else if (unit == "Archer")
        {
            UC.SpawnArcher(building);
        }
        else if (unit == "Mage")
        {
           UC.SpawnMage(building);
        }

        GetComponent<Barracks>().RemoveFirstUnitFromQueue();

        ready = false;
        timer = 0;
    }

    public IEnumerator StartUpgradeTimer(float time, Action upgrade)
    {
        yield return new WaitUntil(() => upgradeCanvas);

        StartCoroutine(Timer(time, upgradeCanvas));

        yield return new WaitUntil(() => ready == true);

        upgrade();
        UIM.UpgradeQueuePanel.SetActive(false);

        creating = false;
        ready = false;
    }

    IEnumerator Timer(float totalTime, GameObject canvas)
    {
        Slider slider = canvas.GetComponentInChildren<Slider>();
        slider.value = 0;
        timer = 0;

        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
            slider.value = timer / totalTime;

            if (timer >= totalTime)
                break;
        }

        ready = true;
      
        yield return new WaitForEndOfFrame();
    }
}
