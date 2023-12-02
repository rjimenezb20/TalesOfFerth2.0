using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ProgressBar : MonoBehaviour
{
    public GameObject buildingCanvas;
    
    private GameObject unitCanvas;
    private float timer = 0;
    private bool ready = false;
    private UnitsCreation UC;
    private UIManager UIM;

    private void Start()
    { 
        UC = FindAnyObjectByType<UnitsCreation>();
        UIM = FindAnyObjectByType<UIManager>();
        unitCanvas = UIM.unitQueue;
    }

    private void Update()
    {
        if (buildingCanvas.activeSelf)
            buildingCanvas.transform.LookAt(buildingCanvas.transform.position + Camera.main.transform.rotation * Vector3.forward, Camera.main.transform.rotation * Vector3.up);
    }

    public IEnumerator StartBuildingTimer(GameObject current, bool isLevelUp)
    {
        buildingCanvas.SetActive(true);

        Building building = current.GetComponent<Building>();

        if (!isLevelUp)
        {
            StartCoroutine(Timer(building.data.timeToBuild, buildingCanvas));
        } 
        else
        {
            StartCoroutine(Timer(building.data.timeToUpdate, buildingCanvas));
        }
            
        yield return new WaitUntil(() => ready == true);

        if (!isLevelUp)
        {
            building.Place();
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

    IEnumerator Timer(float totalTime, GameObject canvas)
    {
        Slider slider = canvas.GetComponentInChildren<Slider>();

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
