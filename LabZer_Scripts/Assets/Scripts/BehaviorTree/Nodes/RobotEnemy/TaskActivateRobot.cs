using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskActivateRobot</c> is a BehaviorTree <c>Node</c> used to define a activate robot task behavior.
/// </summary>
public class TaskActivateRobot : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the game object animator.
    /// </summary>
    private Animator _animator;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskActivateRobot(Transform transform)
    {
        _animator = transform.GetComponent<Animator>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        parent.SetData("isActivated", true);
        
        _animator.SetTrigger("doActivation");

        state = NodeState.Success;
        return state;
    }

    #endregion
}