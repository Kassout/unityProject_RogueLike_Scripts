using BehaviorTree;

/// <summary>
/// Class <c>CheckRobotDeactivated</c> is a BehaviorTree <c>Node</c> used to define a check for a deactivated robot behavior.
/// </summary>
public class CheckRobotDeactivated : Node
{
    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public CheckRobotDeactivated() { }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        if (TryGetData<bool>("isActivated", out bool isActivated) && isActivated)
        {

            state = NodeState.Failure;
            return state;
        }

        state = NodeState.Success;
        return state;
    }

    #endregion
}