using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>GreyDroneSwarmBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of a swarm of grey drones game object.
/// </summary>
public class GreyDroneSwarmBT : EnemyBT
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>enemyPrefab</c> is a Unity <c>GameObject</c> representing the prefabricated object to instanciate as unit of the drone swarm.
    /// </summary>
    [SerializeField]
    private UnityEngine.GameObject _enemyPrefab;

    /// <summary>
    /// Instance field <c>maxSwarmSize</c> represents the maximum value of the swarm unit game object size.
    /// </summary>
    [SerializeField]
    private int _maxSwarmSize = 10;

    /// <summary>
    /// Instance field <c>minSwarmSize</c> represents the minimum value of the swarm unit game object size.
    /// </summary>
    [SerializeField]
    private int _minSwarmSize = 4;

    /// <summary>
    /// Instance field <c>chasingFOVRange</c> represents the chasing range value of the field of view of the grey drone swarm game object.
    /// </summary>
    [SerializeField]
    private float _chasingFOVRange = 6f;

    /// <summary>
    /// Instance field <c>whatIsPlayer</c> is a Unity <c>LayerMask</c> structure representing layer levels the player game object can be find in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsPlayer = 1 << 6;

    /// <summary>
    /// Instance field <c>target</c> is a Unity <c>Transform</c> component representing the grey drone swarm game object target.
    /// </summary>
    [SerializeField]
    private Transform _target;

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the movement speed value of the unarmed black drone game object.
    /// </summary>
    [SerializeField]
    private float _moveSpeed = 2f;

    /// <summary>
    /// Instance field <c>rotationSpeed</c> represents the rotation speed value of the grey drone swarm game object.
    /// </summary>
    [SerializeField]
    private float _rotationSpeed;

    /// <summary>
    /// Instance field <c>resizeSpeed</c> represents the speed value for the swarm's units to resize to a smaller geometry after one get lost.
    /// </summary>
    [SerializeField]
    private float _resizeSpeed;

    /// <summary>
    /// Instance field <c>swarmSize</c> represents the number of drone unit inside the drone swarm.
    /// </summary>
    private int _swarmSize;

    /// <summary>
    /// Instance field <c>swarmUnits</c> is a list of Unity <c>Trasnform</c> component representing the different position, rotation and scale of the swarm's units.
    /// </summary>
    [HideInInspector]
    public List<Transform> swarmUnits = new List<Transform>();

    #endregion

    #region BehaviorTree

    /// <summary>
    /// This function is responsible for setting up the tree, defining the game object behavior by assembling action & control flow nodes together.
    /// </summary>
    /// <returns>A BehaviorTree <c>Node</c> instance representing the setup node.</returns>
    protected override Node SetupTree()
    {
        _swarmSize = UnityEngine.Random.Range(_minSwarmSize, _maxSwarmSize);

        for (int i = 0; i < _swarmSize; i++)
        {
            GameObject swarmUnit = Instantiate(_enemyPrefab, transform);
            Vector3 position = new Vector3();
            position.x = _swarmSize / 4f * Mathf.Cos(Mathf.Deg2Rad * 360 / _swarmSize * i);
            position.y = _swarmSize / 4f * Mathf.Sin(Mathf.Deg2Rad * 360 / _swarmSize * i);
            position.z = 0f;
            swarmUnit.transform.localPosition = position;
            swarmUnits.Add(swarmUnit.transform);
        }

        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new CheckSwarmUnitDeath(swarmUnits),
                new CheckEnemySwarmDeath(swarmUnits),
                new TaskApplyEnemySwarmDeath(transform),
            }),
            new Sequence(new List<Node>
            {
                new TaskRotateSwarm(transform, _rotationSpeed, swarmUnits),
                new TaskAssertUnitSwarmCirclePosition(transform, _resizeSpeed, swarmUnits),
                new TaskAcquireTargetInFOVRange(transform, _chasingFOVRange, _whatIsPlayer),
                new TaskSwarmGoToTarget(transform, _moveSpeed)
            })
        });

        return root;
    }

    #endregion
}