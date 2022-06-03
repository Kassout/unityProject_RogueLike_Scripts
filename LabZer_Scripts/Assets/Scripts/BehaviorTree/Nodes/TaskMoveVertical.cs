using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskMoveVertical</c> is a BehaviorTree <c>Node</c> used to define a move vertical task behavior.
/// </summary>
public class TaskMoveVertical : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the movement speed value of the game object.
    /// </summary>
    private float _moveSpeed;

    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the game object.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the game object animator.
    /// </summary>
    private Animator _animator;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskMoveVertical(Transform transform, float moveSpeed)
    {
        _transform = transform;
        _moveSpeed = moveSpeed;

        _animator = _transform.GetComponent<Animator>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        GameObject target = GetData<GameObject>("target");

        if (Mathf.Abs(_transform.position.y - target.transform.position.y) > 0.1f)
        {
            _animator.SetBool("isMoving", true);

            _transform.position = Vector2.MoveTowards(_transform.position, new Vector2(_transform.position.x, target.transform.position.y), _moveSpeed * Time.deltaTime);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }

        state = NodeState.Success;
        return state;
    }

    #endregion
}