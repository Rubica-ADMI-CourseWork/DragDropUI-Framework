using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TankUpgrader : MonoBehaviour
{
    public GameObject blueTurret;
    public GameObject redTurret;
    public GameObject upgradeDisplay;
    public GameObject upgradedText;
    public GameObject maxedOutText;
    bool upgraded;

    private WorldUIInterface worldUI;
    private void OnEnable()
    {
        worldUI = FindObjectOfType<WorldUIInterface>(); 
    }
    private void Start()
    {
        upgradeDisplay.SetActive(false);
        WorldUIInterface.Instance.OnGearEquipped += Upgrade;
        blueTurret.SetActive(true);
        upgraded = false;
    }

    private void Upgrade()
    {
        StartCoroutine(HandleUpgrade());
    }

    IEnumerator HandleUpgrade()
    {
        yield return new WaitForSeconds(1f);
        if(upgraded == false)
        {
            maxedOutText.SetActive(false);
            upgradedText.SetActive(true);
            upgradeDisplay.SetActive(true);
            blueTurret.SetActive(false);
            redTurret.SetActive(true);
            upgraded = true;
        }
        else
        {
            upgradedText.SetActive(false);
            maxedOutText.SetActive(true);
            upgradeDisplay.SetActive(true);
        }
    }

    public void InactivateDisplayCanvas()
    {
        upgradeDisplay.SetActive(false);
    }
}
