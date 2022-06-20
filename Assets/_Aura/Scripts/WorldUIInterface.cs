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
    [Header("Prefabs representing inventory items")]
    public GameObject gearPrefab;
    public GameObject gemPrefab;

    [Header("Reference to a virtual world that defines drag height")]
    public LayerMask virtualDragWorld;

    [Header("reference to Tank layer for ray casting")]
    public LayerMask tankLayer;

    [Header("Affordance FX to let us know we are over the tank")]
    public GameObject greenRingFX;

    //public event to broadcast to all listeners the gear equipped event
    public event Action OnGearEquipped;

    //cache of current mouse pos each frame
    Vector3 currentMousePos;

    //cache of currently spawned resource that is being dragged to the tank
    GameObject resourceInScene;


    #region Singleton Setup
    public static WorldUIInterface Instance;

    private void Awake()
    {
        Instance = this;
    } 
    #endregion

    private void Update()
    {

        if (Input.GetMouseButtonDown(0) && resourceInScene != null)
        {
            //attaches spawned resource to finger pos
            ResourceSpawn();
        }


        else if (resourceInScene != null && Input.GetMouseButton(0))
        {
            //keeps resource on finger pos during drag
            ResourceDrag();
        }

        else if (resourceInScene != null && Input.GetMouseButtonUp(0))
        {
            //enables gravity on resources allowing it to fall to tank
            ResourceDrop();
        }
    }

    private void ResourceDrag()
    {
        Ray moveRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit moveHit;

        if (!Physics.Raycast(moveRay, out moveHit, 100f, virtualDragWorld))
            return;

        currentMousePos = moveHit.point;
        resourceInScene.transform.position = currentMousePos;

        if (Physics.Raycast(moveRay, 100f, tankLayer))
        {
            greenRingFX.SetActive(true);
            greenRingFX.GetComponent<ParticleSystem>().Play();
        }
        else
        {
            greenRingFX.SetActive(false);
        }
    }

    private void ResourceSpawn()
    {
        Ray spawnRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit spawnHit;
        if (!Physics.Raycast(spawnRay, out spawnHit, 100f, virtualDragWorld))
            return;

        currentMousePos = spawnHit.point;

        resourceInScene.transform.rotation = Quaternion.identity;
        resourceInScene.transform.position = currentMousePos;
    }

    private void ResourceDrop()
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

    /// <summary>
    /// Tells the World UI Interface which of the item prefabs to instantiate
    /// based on which item icon was clicked over
    /// </summary>
    /// <param name="_itemType"></param>
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
