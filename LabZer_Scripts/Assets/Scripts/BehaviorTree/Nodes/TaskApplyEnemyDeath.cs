using BehaviorTree;
using MEC;
using UnityEngine;

/// <summary>
/// Class <c>TaskApplyEnemyDeath</c> is a BehaviorTree <c>Node</c> used to define an apply enemy death task behavior.
/// </summary>
public class TaskApplyEnemyDeath : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>deathTriggered</c> represents the death triggered status of the game object.
    /// </summary>
    private bool _deathTriggered = false;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the game object animator.
    /// </summary>
    private Animator _animator;
    
    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the game object.
    /// </summary>
    private Transform _transform;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskApplyEnemyDeath(Transform transform)
    {
        _transform = transform;
        
        _animator = transform.GetComponent<Animator>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        if (!_deathTriggered)
        {
            _animator.SetTrigger("doDeath");

            AudioManager.Instance.PlaySFX(2);

            if(TryGetData<CoroutineHandle>("updatePathCoroutine", out CoroutineHandle updatePathCoroutine))
            {
                Timing.KillCoroutines(updatePathCoroutine);
            }

            if(TryGetData<CoroutineHandle>("followPathCoroutine", out CoroutineHandle followPathCoroutine))
            {
                Timing.KillCoroutines(followPathCoroutine);
            }

            if(_transform.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
            {
                rigidbody.velocity = Vector2.zero;
            }

            _deathTriggered = true;
        }

        state = NodeState.Success;
        return state;
    }

    #endregion
}