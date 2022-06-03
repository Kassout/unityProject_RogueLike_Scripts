using System.Collections.Generic;
using BehaviorTree;
using BulletEngine;
using UnityEngine;
using PathFinder = Pathfinding.PathFinder;

/// <summary>
/// Class <c>RedDroneBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of a red drone game object.
/// </summary>
public class RedDroneBT : EnemyBT
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the movement speed value of the red drone game object.
    /// </summary>
    [SerializeField]
    private float _moveSpeed = 2f;

    /// <summary>
    /// Instance field <c>chasingFOVRange</c> represents the chasing range value of the field of view of the red drone game object.
    /// </summary>
    [SerializeField]
    private float _chasingFOVRange = 6f;

    /// <summary>
    /// Instance field <c>whatIsPlayer</c> is a Unity <c>LayerMask</c> structure representing layer levels the player game object can be find in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsPlayer = 1 << 6;

    /// <summary>
    /// Instance field <c>gunTransform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the gun of the red drone game object.
    /// </summary>
    [SerializeField]
    private Transform _gunTransform;

    /// <summary>
    /// Instance field <c>shotRate</c> represents the fire shot frenquency value of the red drone game object.
    /// </summary>
    [SerializeField]
    private float _shotRate;

    /// <summary>
    /// Instance field <c>switchAnimationAttack</c> represents the switch animation attack status of the red drone game object.
    /// </summary>
    [SerializeField]
    private bool _switchAnimationAttack;

    /// <summary>
    /// Instance field <c>onDeathBulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the red drone game object for death drone action.
    /// </summary>
    [SerializeField]
    private BulletSpawner _onDeathBulletSpawner;

    /// <summary>
    /// Instance field <c>firingBulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the red drone game object for firing target action.
    /// </summary>
    [SerializeField]
    private BulletSpawner _firingBulletSpawner;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the red drone game object animator.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Instance field <c>pathFinder</c> is a Pathfinding <c>PathFinder</c> component representing the game object pathfinding behavior manager.
    /// </summary>
    private PathFinder _pathFinder;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _pathFinder = GetComponentInParent<PathFinder>();
        _animator = GetComponent<Animator>();
        
        _animator.SetFloat("shotRate", _shotRate / 2f);
        _animator.SetFloat("moveSpeed", _moveSpeed);
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
            new Selector(new List<Node>
            {
                new Sequence(new List<Node>
                {
                    new TaskAcquireTargetInFOVRange(transform, _chasingFOVRange, _whatIsPlayer),
                    new TaskChaseTarget(transform, _moveSpeed, _pathFinder),
                    new TaskFireTarget(transform, _gunTransform, _shotRate, _firingBulletSpawner, _switchAnimationAttack)
                }),
                new Sequence(new List<Node>
                {
                    new TaskMoveErratic(transform, _moveSpeed)
                })
            })
        });

        return root;
    }

    #endregion

    #region Private

    /// <summary>
    /// This function is responsible to triggering the game object explosion event.
    /// </summary>    
    private void TriggerExplosion()
    {
        _onDeathBulletSpawner.SpawnBullets();
    }

    #endregion
}