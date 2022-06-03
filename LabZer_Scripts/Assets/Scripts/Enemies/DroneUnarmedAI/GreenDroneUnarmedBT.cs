using System.Collections.Generic;
using BehaviorTree;
using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>GreenDroneUnarmedBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of a green unarmed drone game object.
/// </summary>
public class GreenDroneUnarmedBT : EnemyBT
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the movement speed value of the unarmed green drone game object.
    /// </summary>
    [SerializeField]
    private float _moveSpeed = 2f;

    /// <summary>
    /// Instance field <c>whatIsWall</c> is a Unity <c>LayerMask</c> structure representing layer levels the wall game objects can be found in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsWall;

    /// <summary>
    /// Instance field <c>bulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the unarmed green drone game object.
    /// </summary>
    private BulletSpawner _bulletSpawner;

    /// <summary>
    /// Instance field <c>coordinateChoice</c> represents a list of coordinate values assigned to the unarmed green drone game object direction.
    /// </summary>
    private readonly int[] _coordinateChoice = { -1, 1 };

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _bulletSpawner = GetComponent<BulletSpawner>();
    }

    #endregion

    #region BehaviorTree

    /// <summary>
    /// This function is responsible for setting up the tree, defining the game object behavior by assembling action & control flow nodes together.
    /// </summary>
    /// <returns>A BehaviorTree <c>Node</c> instance representing the setup node.</returns>
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckEnemyDeath(transform),
                new TaskApplyEnemyDeath(transform)
            }),
            new Sequence(new List<Node>
            {
                new TaskMoveDiagonal(transform, _moveSpeed),
                new CheckWallContact(transform, _whatIsWall),
                new TaskChangeDirection(_coordinateChoice)
            })
        });

        root.SetData("direction", new Vector2(_coordinateChoice[Random.Range(0, _coordinateChoice.Length)], _coordinateChoice[Random.Range(0, _coordinateChoice.Length)]));

        return root;
    }

    #endregion

    #region Private

    /// <summary>
    /// This function is responsible to triggering the game object explosion event.
    /// </summary>    
    private void TriggerExplosion()
    {
        _bulletSpawner.SpawnBullets();
    }
    
    #endregion
}