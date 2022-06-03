using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskRushTarget</c> is a BehaviorTree <c>Node</c> used to define a rush target task behavior.
/// </summary>
public class TaskRushTarget : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>dashSpeed</c> represents the dash speed value of the game object.
    /// </summary>
    private float _dashSpeed;

    /// <summary>
    /// Instance field <c>dashDuration</c> represents the dash duration value of the game object.
    /// </summary>
    private float _dashDuration;

    /// <summary>
    /// Instance field <c>dashDeceleration</c> represents the after dash deceleration magnitude of the game object.
    /// </summary>
    private float _dashDeceleration;

    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the game object.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the game object animator.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Instance field <c>rigidbody</c> is a Unity <c>Rigidbody2D</c> component representing the game object link to the Unity's physics engine.
    /// </summary>
    private Rigidbody2D _rigidbody;

    /// <summary>
    /// Instance field <c>activeMoveSpeed</c> represents the active movement speed of the game object.
    /// </summary>
    private float _activeMoveSpeed = 0f;

    /// <summary>
    /// Instance field <c>dashTimeCounter</c> represents the time counter value since the last dash of the game object.
    /// </summary>
    private float _dashTimeCounter = 0f;

    /// <summary>
    /// Instance field <c>dashTrajectory</c> is a Unity <c>Vector3</c> structure representing the trajectory vector of the game object dash.
    /// </summary>
    private Vector3 _dashTrajectory = Vector3.zero;

    #endregion
    
    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskRushTarget(Transform transform, float dashSpeed, float dashDuration, float dashDeceleration)
    {
        _transform = transform;
        _dashSpeed = dashSpeed;
        _dashDuration = dashDuration;
        _dashDeceleration = dashDeceleration;

        _rigidbody = _transform.GetComponent<Rigidbody2D>();
        _animator = _transform.GetComponent<Animator>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        Transform target = GetData<GameObject>("target").transform;

        if (target != null && _dashTimeCounter <= 0)
        {
            _activeMoveSpeed = _dashSpeed;
            _dashTimeCounter = _dashDuration;

            _dashTrajectory = ((target.position + (Vector3)target.GetComponent<Rigidbody2D>().velocity * 2 * Time.deltaTime) - _transform.position);

            _animator.SetBool("isMoving", true);
        }

        if (_dashTimeCounter > 0)
        {
            _dashTimeCounter -= Time.deltaTime;

            if (_dashTimeCounter <= 0)
            {
                _dashTrajectory = ((target.position + (Vector3)target.GetComponent<Rigidbody2D>().velocity * 2 * Time.deltaTime) - _transform.position);
                parent.SetData("waitTrigger", true);
                _animator.SetBool("isMoving", false);
            }
        }

        Debug.DrawLine(_transform.position, (target.position + (Vector3)target.GetComponent<Rigidbody2D>().velocity));
        _rigidbody.velocity = _dashTrajectory.normalized * _activeMoveSpeed;

        _activeMoveSpeed = Mathf.Lerp(_activeMoveSpeed, 0f, _dashDeceleration * Time.deltaTime);

        state = NodeState.Running;
        return state;
    }

    #endregion
}