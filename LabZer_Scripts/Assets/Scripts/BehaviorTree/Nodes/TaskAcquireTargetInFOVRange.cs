using BehaviorTree;
using UnityEngine;

/// <summary>
/// Class <c>TaskAcquireTargetInFOVRange</c> is a BehaviorTree <c>Node</c> used to define an acquire target in field of view range task behavior.
/// </summary>
public class TaskAcquireTargetInFOVRange : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>fieldOfViewRange</c> represents the range value of the field of view of the game object.
    /// </summary>
    private float _fieldOfViewRange;

    /// <summary>
    /// Instance field <c>whatIsPlayer</c> is a Unity <c>LayerMask</c> structure representing layer levels the player game object can be find in.
    /// </summary>
    private LayerMask _whatIsPlayer;

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
    public TaskAcquireTargetInFOVRange(Transform transform, float fieldOfViewRange, LayerMask whatIsPlayer)
    {
        _transform = transform;
        _fieldOfViewRange = fieldOfViewRange;
        _whatIsPlayer = whatIsPlayer;

        _animator = _transform.GetComponent<Animator>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        object target = GetData("target");
        if (target == null)
        {
            Collider2D collider = Physics2D.OverlapCircle(_transform.position, _fieldOfViewRange, _whatIsPlayer);
            if (collider != null)
            {
                parent.SetData("target", collider.gameObject);

                if (_animator && !TryGetData<bool>("doAlert", out bool doAlert) && !doAlert)
                {
                    _animator.SetTrigger("doAlert");
                    parent.SetDataRoot("doAlert", true);
                }

                state = NodeState.Success;
                return state;
            }

            state = NodeState.Failure;
            return state;
        }

        state = NodeState.Success;
        return state;
    }

    #endregion
}