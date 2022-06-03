using UnityEngine;

/// <summary>
/// Class <c>DoorController</c> is a Unity script used to manage the general game door behavior.
/// </summary>
public class DoorController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the door game object animator.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Instance field <c>boxCollider</c> is a Unity <c>BoxCollider2D</c> component representing the door game object 2D box collider.
    /// </summary>
    private BoxCollider2D _boxCollider;

    #endregion
    
    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        _boxCollider.enabled = false;
    }

    #endregion

    #region Private

    /// <summary>
    /// This function is called on open door event.
    /// </summary>
    private void OnOpenDoor()
    {
        _boxCollider.enabled = false;
    }

    /// <summary>
    /// This function is called on close door event.
    /// </summary>
    private void OnCloseDoor()
    {
        _boxCollider.enabled = true;
    }

    #endregion

    #region Public

    /// <summary>
    /// This function is responsible for disabling the door.
    /// </summary>
    public void DisableDoor()
    {
        _animator.SetTrigger("doOpen");
        GetComponent<SpriteRenderer>().sortingLayerName = "Front";
    }

    /// <summary>
    /// This function is responsible for enabling the door.
    /// </summary>
    public void EnableDoor()
    {
        _animator.SetTrigger("doClose");
        GetComponent<SpriteRenderer>().sortingLayerName = "Props";
    }

    #endregion
}
