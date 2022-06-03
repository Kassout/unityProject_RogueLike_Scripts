using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskApplyEnemySwarmDeath</c> is a BehaviorTree <c>Node</c> used to define an apply enemy swarm death task behavior.
/// </summary>
public class TaskApplyEnemySwarmDeath : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the swarm.
    /// </summary>
    private Transform _transform;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskApplyEnemySwarmDeath(Transform transform)
    {
        _transform = transform;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        Object.Destroy(_transform.gameObject);

        state = NodeState.Success;
        return state;
    }
    
    #endregion
}