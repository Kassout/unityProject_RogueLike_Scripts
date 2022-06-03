using UnityEngine;

/// <summary>
/// Class <c>DisableOnExit</c> is a Unity State Machine script used to disable game object on animation exit.
/// </summary>
public class DisableOnExit : StateMachineBehaviour
{
    /// <summary>
    /// Called on the last update frame when a state machine evaluate this state.
    /// </summary>
    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        animator.gameObject.SetActive(false);
    }
}
