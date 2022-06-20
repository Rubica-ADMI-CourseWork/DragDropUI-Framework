using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

/// <summary>
/// Detects mouse/finger click on Ground
/// spawns a gear prefab at that position
/// and has the prefab follow the mouse/finger as it moves
/// and sets it at the Tank Turret Position
/// </summary>
public class WorldUIInterface : MonoBehaviour
{
    public GameObject gearPrefab;
    public GameObject gemPrefab;
    public LayerMask virtualDragWorld;
    public LayerMask tankLayer;
    public GameObject greenRingFX;

    public event Action OnGearEquipped;

    Vector3 currentMousePos;
    GameObject resourceInScene;
  

    public static WorldUIInterface Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && resourceInScene != null)
        {
            Ray spawnRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit spawnHit;
            if (!Physics.Raycast(spawnRay, out spawnHit, 100f, virtualDragWorld))
                return;

            currentMousePos = spawnHit.point;

            resourceInScene.transform.rotation = Quaternion.identity;
            resourceInScene.transform.position = currentMousePos;
        }


        else if (resourceInScene != null && Input.GetMouseButton(0))
        {
            Ray moveRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit moveHit;

            if (!Physics.Raycast(moveRay, out moveHit, 100f, virtualDragWorld))
                return;

            currentMousePos = moveHit.point;
            resourceInScene.transform.position = currentMousePos;

            if (Physics.Raycast(moveRay,100f, tankLayer))
            {
                greenRingFX.SetActive(true);
                greenRingFX.GetComponent<ParticleSystem>().Play();
            }
            else
            {
                greenRingFX.SetActive(false);
            }
        }

        else if (resourceInScene != null && Input.GetMouseButtonUp(0))
        {
            Ray equipRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit equipHit;

            if (Physics.Raycast(equipRay, out equipHit, 100f, tankLayer))
            {
                currentMousePos = equipHit.point;
                resourceInScene.GetComponent<Rigidbody>().isKinematic = false;
                greenRingFX.SetActive(false);
                OnGearEquipped?.Invoke();
            }
            else
            {
                Destroy(resourceInScene);
            }
        }
    }

    public void InitInterface(ItemType _itemType)
    {
        switch (_itemType)
        {
            case ItemType.Gear:
                resourceInScene = Instantiate(gearPrefab);
                break;
            case ItemType.Gem:
                resourceInScene = Instantiate(gemPrefab);
                break;
        }
    }
}
