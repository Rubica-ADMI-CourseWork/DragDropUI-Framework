using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GearInteractions : MonoBehaviour
{
    public GameObject equipFx;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Tank"))
        {
            Destroy(gameObject);
            Instantiate(equipFx, transform.position, Quaternion.identity);
        }
    }
}
