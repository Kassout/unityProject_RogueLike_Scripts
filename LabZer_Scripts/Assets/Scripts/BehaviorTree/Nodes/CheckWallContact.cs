using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>CheckWallContact</c> is a BehaviorTree <c>Node</c> used to define a check for wall contact behavior.
/// </summary>
public class CheckWallContact : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>whatIsWall</c> is a Unity <c>LayerMask</c> structure representing layer levels the wall game objects can be found in.
    /// </summary>
    private LayerMask _whatIsWall;

    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the game object.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Instance field <c>moveDirection</c> is a Unity <c>Vector2</c> structure representing the the current movement direction vector of the game object.
    /// </summary>
    private Vector2 _moveDirection;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public CheckWallContact(Transform transform, LayerMask whatIsWall)
    {
        _transform = transform;
        _whatIsWall = whatIsWall;
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        RaycastHit2D hit = Physics2D.Raycast(_transform.position, GetData<Vector2>("direction"), 1f, _whatIsWall);
        if (hit)
        {
            parent.SetData("hitPoint", hit);

            state = NodeState.Success;
            return state;
        }
        else
        {
            ClearData("hitPoint");
        }

        state = NodeState.Failure;
        return state;
    }

    #endregion
}