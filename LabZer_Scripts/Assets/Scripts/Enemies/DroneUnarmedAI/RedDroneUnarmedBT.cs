using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>RedDroneUnarmedBT</c> is a BehaviorTree <c>Tree</c> instance representing the general behavior of a red unarmed drone game object.
/// </summary>
public class RedDroneUnarmedBT : EnemyBT
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>dashSpeed</c> represents the dash speed value of the unarmed red drone game object.
    /// </summary>
    [SerializeField]
    private float _dashSpeed = 30f;

    /// <summary>
    /// Instance field <c>dashDuration</c> represents the dash duration value of the unarmed red drone game object.
    /// </summary>
    [SerializeField]
    private float _dashDuration = 0.75f;

    /// <summary>
    /// Instance field <c>dashDeceleration</c> represents the after dash deceleration magnitude of the unarmed red drone game object.
    /// </summary>
    [SerializeField]
    private float _dashDeceleration = 3f;

    /// <summary>
    /// Instance field <c>chasingFOVRange</c> represents the chasing range value of the field of view of the unarmed red drone game object.
    /// </summary>
    [SerializeField]
    private float _chasingFOVRange = 6f;

    /// <summary>
    /// Instance field <c>whatIsPlayer</c> is a Unity <c>LayerMask</c> structure representing layer levels the player game object can be find in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsPlayer = 1 << 6;

    /// <summary>
    /// Instance field <c>waitDuration</c> represents the duration value for the unarmed red drone game object to wait before performing another behavior task.
    /// </summary>
    [SerializeField]
    private float _waitDuration = 2f;

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
                new TaskAcquireTargetInFOVRange(transform, _chasingFOVRange, _whatIsPlayer),
                new TaskWait(_waitDuration),
                new TaskRushTarget(transform, _dashSpeed, _dashDuration, _dashDeceleration)
            })
        });

        return root;
    }

    #endregion
}