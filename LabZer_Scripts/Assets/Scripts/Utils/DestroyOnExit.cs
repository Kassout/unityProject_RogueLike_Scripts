using UnityEngine;

/// <summary>
/// Class <c>DestroyOnExit</c> is a Unity State Machine script used to destroy game object on animation exit.
/// </summary>
public class DestroyOnExit : StateMachineBehaviour
{
    /// <summary>
    /// Called on the first Update frame when a state machine evaluate this state.
    /// </summary>
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        Destroy(animator.gameObject, animatorStateInfo.length);
    }
}
