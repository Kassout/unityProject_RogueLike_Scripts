using BehaviorTree;
using UnityEngine;
using Node = BehaviorTree.Node;

/// <summary>
/// Class <c>CheckEnemyDeath</c> is a BehaviorTree <c>Node</c> used to define a check for an enemy death behavior.
/// </summary>
public class CheckEnemyDeath : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>enemyHealthController</c> is a Unity <c>EnemyHealthController</c> script component representing the enemy gameobject health controller.
    /// </summary>
    private EnemyHealthController _enemyHealthController;

    #endregion
    
    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public CheckEnemyDeath(Transform transform)
    {
        _enemyHealthController = transform.GetComponent<EnemyHealthController>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        if (_enemyHealthController.Health <= 0)
        {
            state = NodeState.Success;
            return state;
        }

        state = NodeState.Failure;
        return state;
    }

    #endregion
}