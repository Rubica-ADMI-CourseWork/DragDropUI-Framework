using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// This class handles the upgrading of the tank. 
/// it listens for upgrade event by subscribing to the 
/// OnUpgrade event of the WorldInterface class
/// </summary>
public class TankUpgrader : MonoBehaviour
{
    [Header("Tank Turrets")]
    public GameObject blueTurret;
    public GameObject redTurret;

    [Header("Upgrade Display Text Fields")]
    public GameObject upgradeDisplay;
    public GameObject upgradedText;
    public GameObject maxedOutText;
    bool upgraded;

    private WorldUIInterface worldUI;//reference to the WorldInterface script in scene
    private void OnEnable()
    {
        worldUI = FindObjectOfType<WorldUIInterface>(); 
    }
    private void Start()
    {
        //Subscription to OnGearEquipped event on WorldInterface script
        WorldUIInterface.Instance.OnGearEquipped += Upgrade;

        //initialization
        upgradeDisplay.SetActive(false);
        blueTurret.SetActive(true);
        upgraded = false;
    }

    /// <summary>
    /// this event listens for gear upgrade and starts a coroutine
    /// </summary>
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

   
}
