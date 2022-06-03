using System;
using UnityEngine;

/// <summary>
/// Structure <c>ObjectPoolItem</c> is a data container for game object item aiming to be instantiate in an object pooler.
/// </summary>
[Serializable]
public struct ObjectPoolItem
{
    /// <summary>
    /// Instance field <c>amountToPool</c> represents the amount value of object to instantiate in the pool at game awake.
    /// </summary>
    public int amountToPool;

    /// <summary>
    /// Field <c>objectToPool</c> is a Unity <c>GameObject</c> representing the prefabricated object to instantiate on the pool.
    /// </summary>
    public GameObject objectToPool;

    /// <summary>
    /// Instance field <c>shouldExpand</c> represents the expandable pool size status of the object inside the pool.
    /// </summary>
    public bool shouldExpand;
}
