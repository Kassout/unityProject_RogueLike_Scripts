using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskSwarmGoToTarget</c> is a BehaviorTree <c>Node</c> used to define a swarm go to target task behavior.
/// </summary>
public class TaskSwarmGoToTarget : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the speed value of the swarm movement.
    /// </summary>
    private float _moveSpeed;

    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the swarm.
    /// </summary>
    private Transform _transform;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskSwarmGoToTarget(Transform transform, float moveSpeed)
    {
        _transform = transform;
        _moveSpeed = moveSpeed;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        GameObject target = GetData<GameObject>("target");
        if (Vector2.Distance(_transform.position, target.transform.position) > 0.5f)
        {
            _transform.position = Vector3.MoveTowards(_transform.position, target.transform.position, _moveSpeed * Time.deltaTime);
        }

        state = NodeState.Running;
        return state;
    }

    #endregion
}