using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskRotate</c> is a BehaviorTree <c>Node</c> used to define a rotate task behavior.
/// </summary>
public class TaskRotate : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>rotationSpeed</c> represents the rotation speed value of the game object.
    /// </summary>
    private float _rotationSpeed;

    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the game object.
    /// </summary>
    private Transform _transform;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskRotate(Transform transform, float rotationSpeed)
    {
        _transform = transform;
        _rotationSpeed = rotationSpeed;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        _transform.Rotate(0f, 0f, _rotationSpeed * Time.deltaTime);

        state = NodeState.Running;
        return state;
    }

    #endregion
}