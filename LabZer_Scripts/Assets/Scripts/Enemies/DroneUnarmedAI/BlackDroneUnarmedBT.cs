using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;
using PathFinder = Pathfinding.PathFinder;

/// <summary>
/// Class <c>BlackDroneUnarmedBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of a black unarmed drone game object.
/// </summary>
public class BlackDroneUnarmedBT : EnemyBT
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the movement speed value of the unarmed black drone game object.
    /// </summary>
    [SerializeField]
    private float _moveSpeed = 2f;

    /// <summary>
    /// Instance field <c>chasingFOVRange</c> represents the chasing range value of the field of view of the unarmed black drone game object.
    /// </summary>
    [SerializeField]
    private float _chasingFOVRange = 6f;

    /// <summary>
    /// Instance field <c>whatIsPlayer</c> is a Unity <c>LayerMask</c> structure representing layer levels the player game object can be find in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsPlayer = 1 << 6;

    /// <summary>
    /// Instance field <c>whatIsWall</c> is a Unity <c>LayerMask</c> structure representing the different layers considered as wall for the unarmed black drone game object.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsWall;

    /// <summary>
    /// Instance field <c>pathFinder</c> is a Pathfinding <c>PathFinder</c> component representing the game object pathfinding behavior manager.
    /// </summary>
    private PathFinder _pathFinder;

    #endregion

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _pathFinder = GetComponentInParent<PathFinder>();
    }

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
                    new TaskChaseTarget(transform, _moveSpeed, _pathFinder)
                }),
                new Sequence(new List<Node>
                {
                    new TaskMoveErratic(transform, _moveSpeed - 2),
                    new CheckWallContact(transform, _whatIsWall)
                })
            })
        });

        return root;
    }

    #endregion
}
