using System.Collections.Generic;
using BehaviorTree;
using BulletEngine;
using UnityEngine;
using PathFinder = Pathfinding.PathFinder;

/// <summary>
/// Class <c>GreyGuardianBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of a grey guardian game object.
/// </summary>
public class GreyGuardianBT : EnemyBT
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the movement speed value of the grey guardian game object.
    /// </summary>
    [SerializeField]
    private float _moveSpeed = 2f;

    /// <summary>
    /// Instance field <c>chasingFOVRange</c> represents the chasing range value of the field of view of the grey guardian game object.
    /// </summary>
    [SerializeField]
    private float _chasingFOVRange = 6f;

    /// <summary>
    /// Instance field <c>staticShootFOVRange</c> represents the static shooting range value of the field of view of the grey guardian game object.
    /// </summary>
    [SerializeField]
    private float _staticShootFOVRange;

    /// <summary>
    /// Instance field <c>whatIsPlayer</c> is a Unity <c>LayerMask</c> structure representing layer levels the player game object can be find in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsPlayer = 1 << 6;

    /// <summary>
    /// Instance field <c>gunTransform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the gun of the grey guardian game object.
    /// </summary>
    [SerializeField]
    private Transform _gunTransform;

    /// <summary>
    /// Instance field <c>shotRate</c> represents the fire shot frenquency value of the grey guardian game object.
    /// </summary>
    [SerializeField]
    private float _shotRate;

    /// <summary>
    /// Instance field <c>bulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the grey guardian game object.
    /// </summary>
    private BulletSpawner _bulletSpawner;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the grey guardian game object animator.
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
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
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
            new Sequence(new List<Node>
            {
                new CheckTargetInFOVRange(transform, _chasingFOVRange, _whatIsPlayer),
                new TaskGoToTarget(transform, _moveSpeed, _pathFinder),
                new TaskAcquireTargetInFOVRange(transform, _staticShootFOVRange, _whatIsPlayer),
                new TaskFireTarget(transform, _gunTransform, _shotRate, _bulletSpawner, false)
            }),
            new Sequence(new List<Node>
            {
                new TaskAcquireTargetInFOVRange(transform, _staticShootFOVRange, _whatIsPlayer),
                new TaskFireTarget(transform, _gunTransform, _shotRate, _bulletSpawner, false)
            })
        });

        return root;
    }

    #endregion
}