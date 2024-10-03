using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradesController : MonoBehaviour
{
    public GameObject warnings;

    [Header("UpgradesDatas")]
    public UpgradeData Level2UpgradeData;
    public UpgradeData Level3UpgradeData;

    [Header("Upgrades")]
    public bool Level2Upgrade = false;
    public bool Level3Upgrade = false;

    private ResourcesController RC;
    private UIManager UIM;

    private void Start()
    {
        RC = GetComponent<ResourcesController>();
        UIM = GetComponent<UIManager>();
    }

    public void StartLevelUpUpgrade()
    {
        ProgressBar progressBar = UIM.UpgradeQueuePanel.GetComponentInParent<ProgressBar>();
        progressBar.creating = true;

        if (!Level2Upgrade)
        {
            if (CheckIfEnoughResources(Level2UpgradeData))
            {
                StartCoroutine(progressBar.StartUpgradeTimer(Level2UpgradeData.upgradeTime, GetLevel2Upgrade));
                UIM.UpdateUpgradeImage(Level2UpgradeData.upgradeImage);
                UIM.Level2UpgradeBt.SetActive(false);
                UIM.ButtonHoverExit();
            }  
        } 
        else
        {
            if (CheckIfEnoughResources(Level3UpgradeData))
            {
                StartCoroutine(progressBar.StartUpgradeTimer(Level3UpgradeData.upgradeTime, GetLevel3Upgrade));
                UIM.UpdateUpgradeImage(Level3UpgradeData.upgradeImage);
                UIM.Level3UpgradeBt.SetActive(false);
                UIM.ButtonHoverExit();
            }
        }
    }

    private void GetLevel2Upgrade()
    {
        Level2Upgrade = true;
        UIM.Level2SmithUpgrade();
    }

    private void GetLevel3Upgrade()
    {
        Level3Upgrade = true;
    }

    private bool CheckIfEnoughResources(UpgradeData data)
    {
        if (RC.CheckIfEnoughResources(data.goldCost, data.foodCost, data.woodCost, data.stoneCost, data.metalCost, 0))
        {
            RC.SubstractResources(data.goldCost, data.foodCost, data.woodCost, data.stoneCost, data.metalCost, 0);
            return true;
        } 
        else
        {
            warnings.GetComponent<Animation>().Play("ResourcesWarning");
            return false;
        }
    }
}