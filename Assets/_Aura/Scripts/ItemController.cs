using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;


/// <summary>
/// Detects that finger has touched and dragged over it
/// </summary>
public class ItemController : MonoBehaviour, IPointerDownHandler
{
    public ItemType itemType;

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Dragging UI");
        WorldUIInterface.Instance.InitInterface(itemType);
    }
}
public enum ItemType
{
    Gear,
    Gem
}
