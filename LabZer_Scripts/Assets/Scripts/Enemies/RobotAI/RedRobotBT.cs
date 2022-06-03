using System.Collections.Generic;
using BehaviorTree;
using BulletEngine;
using UnityEngine;

/// <summary>
/// TODO: add comment
/// </summary>
public class RedRobotBT : EnemyBT
{
    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the movement speed value of the unarmed black drone game object.
    /// </summary>
    [SerializeField]
    private float _moveSpeed = 2f;

    /// <summary>
    /// TODO: add comment
    /// </summary>
    [SerializeField]
    private RuntimeAnimatorController _activatedAnimator;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private float _fieldOfViewRange;

    /// <summary>
    /// Instance field <c>whatIsPlayer</c> is a Unity <c>LayerMask</c> structure representing layer levels the player game object can be find in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsPlayer = 1 << 6;

    /// <summary>
    /// TODO: add comment
    /// </summary>
    /// <summary>
    /// Instance field <c>firePoint</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the fire point game object.
    /// </summary>
    [SerializeField]
    private Transform _firePoint;

    /// <summary>
    /// TODO: add comment
    /// </summary>
    private BulletSpawner _bulletSpawner;

    /// <summary>
    /// TODO: add comment
    /// </summary>
    [SerializeField]
    private float _shotRate;

    /// <summary>
    /// TODO: add comment
    /// </summary>
    [SerializeField]
    private bool _switchAnimationAttack;

    /// <summary>
    /// TODO: add comment
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// TODO: add comment
    /// </summary>
    private Rigidbody2D _rigidbody;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _animator.SetFloat("moveSpeed", _moveSpeed);
    }

    /// <summary>
    /// TODO: add comment
    /// </summary>
    private void SwitchAnimationLayer()
    {
        _animator.runtimeAnimatorController = _activatedAnimator;
        _rigidbody.constraints = RigidbodyConstraints2D.None;
        _bulletSpawner.enabled = true;
    }

    /// <summary>
    /// This function is responsible for setting up the tree, defining the game object behavior by assembling action & control flow nodes together.
    /// </summary>
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
                new CheckRobotDeactivated(),
                new CheckTargetInFOVRange(transform, _fieldOfViewRange, _whatIsPlayer),
                new TaskActivateRobot(transform)
            }),
            new Sequence(new List<Node>
            {
                new TaskAcquireTargetInFOVRange(transform, _fieldOfViewRange, _whatIsPlayer),
                new TaskMoveVertical(transform, _moveSpeed),
                new TaskFireRight(transform, _firePoint, _shotRate, _bulletSpawner, _switchAnimationAttack)
            })
        });

        return root;
    }
}
