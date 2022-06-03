using System.Collections.Generic;
using BehaviorTree;
using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>BlackRobotStaticBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of a static black robot game object.
/// </summary>
public class BlackRobotStaticBT : EnemyBT
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>detectionFOVRange</c> represents the detection range value of the field of view of the static black robot game object.
    /// </summary>
    [SerializeField]
    private float _detectionFOVRange = 6f;

    /// <summary>
    /// Instance field <c>whatIsPlayer</c> is a Unity <c>LayerMask</c> structure representing layer levels the player game object can be find in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsPlayer = 1 << 6;

    /// <summary>
    /// Instance field <c>firePoint</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the fire point of the static black robot game object.
    /// </summary>
    [SerializeField]
    private Transform _firePoint;

    /// <summary>
    /// Instance field <c>shotRate</c> represents the fire shot frenquency value of the static black robot game object.
    /// </summary>
    [SerializeField]
    private float _shotRate;

    /// <summary>
    /// Instance field <c>switchAnimationAttack</c> represents the switch animation attack status of the static black robot game object.
    /// </summary>
    [SerializeField]
    private bool _switchAnimationAttack;

    /// <summary>
    /// Instance field <c>bulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the static black robot game object.
    /// </summary>
    private BulletSpawner _bulletSpawner;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the static black robot game object animator.
    /// </summary>
    private Animator _animator;

    #endregion
    
    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("shotRate", _shotRate / 2f);
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
                new CheckTargetInFOVRange(transform, _detectionFOVRange, _whatIsPlayer),
                new TaskFireRight(transform, _firePoint, _shotRate, _bulletSpawner, _switchAnimationAttack)
            })
        });

        return root;
    }

    #endregion
}