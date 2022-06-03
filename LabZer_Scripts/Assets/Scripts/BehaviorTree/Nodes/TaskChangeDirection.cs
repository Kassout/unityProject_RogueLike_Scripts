using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskChangeDirection</c> is a BehaviorTree <c>Node</c> used to define a change direction task behavior.
/// </summary>
public class TaskChangeDirection : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>coordinateChoice</c> represents a list of coordinate values assigned to the game object direction.
    /// </summary>
    private int[] _coordinateChoice;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskChangeDirection(int[] coordinateChoice)
    {
        _coordinateChoice = coordinateChoice;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        Vector2 currentDirection = GetData<Vector2>("direction");
        RaycastHit2D hit = GetData<RaycastHit2D>("hitPoint");

        Vector2 newDirection = currentDirection - 2 * Vector2.Dot(currentDirection, hit.normal) * hit.normal;

        parent.SetData("direction", newDirection);

        state = NodeState.Running;
        return state;
    }

    #endregion
}