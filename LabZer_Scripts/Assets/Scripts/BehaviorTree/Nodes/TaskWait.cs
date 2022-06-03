using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskWait</c> is a BehaviorTree <c>Node</c> used to define a wait task behavior.
/// </summary>
public class TaskWait : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>waitDuration</c> represents the duration value for the game object to wait before performing another behavior task.
    /// </summary>
    private float _waitDuration;

    /// <summary>
    /// Instance field <c>waitCounter</c> represents the time counter value since the last game object wait task.
    /// </summary>
    private float _waitCounter;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskWait(float waitDuration)
    {
        _waitDuration = waitDuration;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        if (TryGetData<bool>("waitTrigger", out bool waitTrigger) && waitTrigger)
        {
            _waitCounter = 0;
            parent.SetData("waitTrigger", false);
        }

        if (_waitCounter < _waitDuration)
        {
            _waitCounter += Time.deltaTime;
        }
        else
        {
            state = NodeState.Success;
            return state;
        }

        state = NodeState.Failure;
        return state;
    }

    #endregion
}