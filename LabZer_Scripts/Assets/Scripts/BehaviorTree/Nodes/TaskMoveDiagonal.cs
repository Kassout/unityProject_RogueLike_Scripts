using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskMoveDiagonal</c> is a BehaviorTree <c>Node</c> used to define a move diagonal task behavior.
/// </summary>
public class TaskMoveDiagonal : Node
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

    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the sprite renderer of the game object.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskMoveDiagonal(Transform transform, float moveSpeed)
    {
        _transform = transform;
        _moveSpeed = moveSpeed;

        _animator = _transform.GetComponent<Animator>();
        _spriteRenderer = _transform.GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        Vector2 direction = GetData<Vector2>("direction");

        _animator.SetBool("isMoving", true);

        // Switch sprite direction to align with target position
        if (direction.x < 0f)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        _transform.position = Vector2.MoveTowards(_transform.position, (Vector2)_transform.position + direction, _moveSpeed * Time.deltaTime);

        state = NodeState.Running;
        return state;
    }

    #endregion
}