using UnityEngine;

/// <summary>
/// Class <c>Shake</c> is a Unity script used to manage the camera shaking behavior.
/// </summary>
public class Shake : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>_animator</c> is a Unity <c>Animator</c> component representing the animation manager of the camera.
    /// </summary>
    private static Animator _animator;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// This function is responsible for triggering the shaking animation of the camera.
    /// </summary>
    public static void ShakeCamera()
    {
        _animator.SetTrigger("doShake");
    }
}
