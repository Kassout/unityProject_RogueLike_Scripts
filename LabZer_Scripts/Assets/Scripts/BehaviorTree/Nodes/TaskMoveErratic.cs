using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskMoveErratic</c> is a BehaviorTree <c>Node</c> used to define a move erratic task behavior.
/// </summary>
public class TaskMoveErratic : Node
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

    /// <summary>
    /// Instance field <c>nextPosition</c> is a Unity <c>Vector3</c> structure representing the position coordinates of the next position for the game object to reach.
    /// </summary>
    private Vector3 _nextPosition;

    /// <summary>
    /// Instance field <c>positionOld</c> is a Unity <c>Vector3</c> structure representing the position coordinates of the last game object position.
    /// </summary>
    private Vector3 _positionOld;

    /// <summary>
    /// Instance field <c>isStuck</c> represents the is position stuck status of the game object.
    /// </summary>
    private bool _isStuck;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskMoveErratic(Transform transform, float moveSpeed)
    {
        _transform = transform;
        _moveSpeed = moveSpeed;
        _nextPosition = _transform.position;

        _animator = _transform.GetComponent<Animator>();
        _spriteRenderer = _transform.GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        if (Vector2.Distance(_transform.position, _nextPosition) > 0.25f && !TryGetData<RaycastHit2D>("hitPoint", out RaycastHit2D hit) && !hit)
        {
            _animator.SetBool("isMoving", true);

            // Switch sprite direction to align with target position
            if (_nextPosition.x < _transform.position.x)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
        else
        {
            _animator.SetBool("isMoving", false);

            _nextPosition = _transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0);
        }

        _transform.position = Vector3.MoveTowards(_transform.position, _nextPosition, _moveSpeed * Time.deltaTime);

        parent.SetData("direction", (Vector2)(_nextPosition - _transform.position));

        state = NodeState.Success;
        return state;
    }

    #endregion
}