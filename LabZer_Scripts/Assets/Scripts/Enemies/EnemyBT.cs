using UnityEngine;
using Tree = BehaviorTree.Tree;

/// <summary>
/// Class <c>EnemyBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of enemies game object.
/// </summary>
public abstract class EnemyBT : Tree
{
    /// <summary>
    /// Instance field <c>shouldDropItem</c> represents the should drop item status of the breakable object.
    /// </summary>
    [SerializeField]
    protected bool _shouldDropItem;

    /// <summary>
    /// Instance field <c>itemDropChance</c> represents the chance percentage value of item drops on game object destroyed.
    /// </summary>
    [SerializeField]
    protected int _itemDropChance;

    /// <summary>
    /// Field <c>itemsToDrop</c> is an array of Unity <c>GameObject</c> representing the different items the breakable object can drop on destroyed.
    /// </summary>
    [SerializeField]
    protected GameObject[] _itemsToDrop;

    /// <summary>
    /// This function is called when the MonoBehaviour will be destroyed.
    /// </summary>
    protected void OnDestroy()
    {
        if (_shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f);

            if (dropChance < _itemDropChance)
            {
                Instantiate(_itemsToDrop[Random.Range(0, _itemsToDrop.Length)], transform.position, Quaternion.identity);
            }
        }
    }
}
