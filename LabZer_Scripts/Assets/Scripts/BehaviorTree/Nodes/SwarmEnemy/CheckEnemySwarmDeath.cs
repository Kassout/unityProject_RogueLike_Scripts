using System.Collections.Generic;
using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>CheckEnemySwarmDeath</c> is a BehaviorTree <c>Node</c> used to define a check for a swarm of enemies death behavior.
/// </summary>
public class CheckEnemySwarmDeath : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>swarmUnits</c> is a list of Unity <c>Trasnform</c> component representing the different position, rotation and scale of the swarm's units.
    /// </summary>
    private List<Transform> _swarmUnits;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public CheckEnemySwarmDeath(List<Transform> swarmUnits)
    {
        _swarmUnits = swarmUnits;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        if (_swarmUnits.Count <= 0)
        {
            state = NodeState.Success;
            return state;
        }

        state = NodeState.Failure;
        return state;
    }

    #endregion
}