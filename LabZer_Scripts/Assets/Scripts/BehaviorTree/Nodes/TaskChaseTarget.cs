using BehaviorTree;
using Pathfinding;
using UnityEngine;
using MEC;
using Node = BehaviorTree.Node;
using System.Collections.Generic;

/// <summary>
/// Class <c>TaskChaseTarget</c> is a BehaviorTree <c>Node</c> used to define a chase target task behavior.
/// </summary>
public class TaskChaseTarget : Node
{
    #region Fields / Properties

    /// <summary>
    /// Constant field <c>MinPathUpdateTime</c> represents the minimum time value between two path updates.
    /// </summary>
    private const float MinPathUpdateTime = .2f;

    /// <summary>
    /// Constant field <c>PathUpdateMoveThreshold</c> represents the minimum movement threshold so the game object path require an update.
    /// </summary>
    private const float PathUpdateMoveThreshold = .5f;

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
    /// Instance field <c>path</c> is an array of Unity <c>Vector3</c> structures representing the different position coordinates of the game object travel path nodes.
    /// </summary>
    private Vector3[] _path;

    /// <summary>
    /// Instance field <c>pathNodeIndex</c> represents the index value of the game object current node on path.
    /// </summary>
    private int _pathNodeIndex;

    /// <summary>
    /// Instance field <c>isChasing</c> represents the is chasing status of the game object.
    /// </summary>
    private bool _isChasing;

    /// <summary>
    /// Instance field <c>pathFinder</c> is a Pathfinding <c>PathFinder</c> component representing the game object pathfinding behavior manager.
    /// </summary>
    private PathFinder _pathFinder;

    /// <summary>
    /// Instance field <c>updatePathCoroutine</c> is a MEC <c>CoroutineHandle</c> structure representing the current handle of the game object update path coroutine.
    /// </summary>
    private CoroutineHandle _updatePathCoroutine;

    /// <summary>
    /// Instance field <c>followPathCoroutine</c> is a MEC <c>CoroutineHandle</c> structure representing the current handle of the game object follow path path coroutine.
    /// </summary>
    private CoroutineHandle _followPathCoroutine;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskChaseTarget(Transform transform, float moveSpeed, PathFinder pathFinder)
    {
        _transform = transform;
        _moveSpeed = moveSpeed;
        _pathFinder = pathFinder;

        _animator = _transform.GetComponent<Animator>();
        _spriteRenderer = _transform.GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        Transform target = GetData<GameObject>("target").transform;

        if (!_isChasing)
        {
            _animator.SetBool("isMoving", true);

            _updatePathCoroutine = Timing.RunCoroutine(UpdatePath(target).CancelWith(_transform.gameObject));
            
            SetDataRoot("updatePathCoroutine", _updatePathCoroutine);
            parent.SetData("isMoving", true);
            _isChasing = true;
        }

        state = NodeState.Running;
        return state;
    }

    #endregion

    #region Pathfinding

    /// <summary>
    /// This function is called on path found for game object position target requested.
    /// </summary>
    /// <param name="newPath">An array of Unity <c>Vector3</c> structures representing the different path waypoints coordinate positions.</param>
    /// <param name="pathSuccessful">A boolean value representing the path successful status of the path request.</param>
    private void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            _path = newPath;
            _pathNodeIndex = 0;
            Timing.KillCoroutines(_followPathCoroutine);
            _followPathCoroutine = Timing.RunCoroutine(FollowPath().CancelWith(_transform.gameObject));

            SetDataRoot("followPathCoroutine", _followPathCoroutine);
        }
    }

    /// <summary>
    /// This function is responsible for updating the game object path.
    /// </summary>
    /// <param name="target">A Unity <c>Transform</c> component representing the target game object position, rotation and scale.</param>
    private IEnumerator<float> UpdatePath(Transform target)
    {
        if (Time.timeSinceLevelLoad < .3f)
        {
            yield return Timing.WaitForSeconds(0.3f);
        }
        PathRequestManager.RequestPath(new PathRequest(_transform.position, target.position, OnPathFound, _pathFinder));

        float sqrMoveThreshold = PathUpdateMoveThreshold * PathUpdateMoveThreshold;
        Vector3 targetPosOld = target.position;

        while (true)
        {
            yield return Timing.WaitForSeconds(MinPathUpdateTime);

            if ((target.position - targetPosOld).sqrMagnitude > sqrMoveThreshold)
            {
                PathRequestManager.RequestPath(new PathRequest(_transform.position, target.position, OnPathFound, _pathFinder));
                targetPosOld = target.position;
            }
        }
    }

    /// <summary>
    /// This function is responsible for moving the game object to follow the path found for a requested target position.
    /// </summary>
    private IEnumerator<float> FollowPath()
    {
        Vector3 currentWaypoint = _path[0];

        while (true)
        {
            if (_transform.position == currentWaypoint)
            {
                _pathNodeIndex++;
                if (_pathNodeIndex >= _path.Length)
                {
                    yield break;
                }
                currentWaypoint = _path[_pathNodeIndex];
            }

            // Switch sprite direction to align with target position
            if (_transform.position.x > currentWaypoint.x)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }

            _transform.position = Vector3.MoveTowards(_transform.position, currentWaypoint, _moveSpeed * Time.deltaTime);
            yield return Timing.WaitForOneFrame;
        }
    }

    #endregion
}