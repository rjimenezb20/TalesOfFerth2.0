using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.UI.CanvasScaler;

public class Barracks : MonoBehaviour
{
    public Image current;
    public Slider timeBar;
    [HideInInspector] public bool creating = false;
    
    private List<Unit> unitQueue = new List<Unit>();
    private UIManager UIM;

    private void Start()
    {
        StartCoroutine(CheckQueue());
        UIM = FindAnyObjectByType<UIManager>();
    }

    public void AddUnitToQueue(Unit unit)
    {
        if (unitQueue.Count < 6)
        {
            unitQueue.Add(unit);
            UIM.UpdateUnitQueueImages(unitQueue);
        }
    }

    public void RemoveFirstUnitFromQueue()
    {
        unitQueue.Remove(unitQueue[0]);
        creating = false;
        UIM.UpdateUnitQueueImages(unitQueue);
    }

    public void RemoveUnitFromQueue(Unit unit)
    {
        unitQueue.Remove(unit);
        UIM.UpdateUnitQueueImages(unitQueue);
    }

    IEnumerator CheckQueue()
    {
        while (true)
        {
            yield return new WaitUntil(() => !creating);

            if (unitQueue.Count > 0)
            {
                ProgressBar progressBar = GetComponent<ProgressBar>();
                StartCoroutine(progressBar.StartUnitTimer(unitQueue[0].data.creationTime, unitQueue[0].data.unitName, GetComponent<Building>()));
                creating = true;
            }
        }
    }
}