using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Class <c>RoomController</c> is a Unity script used to manage the general game room behavior.
/// </summary>
public class RoomController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>activeEvent</c> is a Unity <c>UnityEvent</c> representing the function trigger for room controller game object activation event.
    /// </summary>
    public UnityEvent activeEvent = new UnityEvent();

    /// <summary>
    /// Instance field <c>closeWhenEntered</c> represents the close when entered status of the room game object.
    /// </summary>
    [HideInInspector]
    public bool closeWhenEntered = false;

    /// <summary>
    /// Instance field <c>doors</c> is an array of Unity <c>DoorController</c> components representing the different controller of the room's doors game object.
    /// </summary>
    [SerializeField]
    private DoorController[] _doors;

    [SerializeField]
    private GameObject _mapHider;

    /// <summary>
    /// Instance field <c>roomActive</c> represents the active status of the room game object.
    /// </summary>
    [HideInInspector]
    public bool roomActive;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            CameraController.Instance.ChangeTarget(transform);

            if (closeWhenEntered)
            {
                foreach (DoorController door in _doors)
                {
                    door.EnableDoor();
                }
            }

            roomActive = true;
            _mapHider.SetActive(false);
            activeEvent.Invoke();
        }
    }

    /// <summary>
    /// Sent when another object leaves a trigger collider attached to
    /// this object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            roomActive = false;
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// This function is responsible for opening the room game object doors.
    /// </summary>
    public void OpenDoors() {
        foreach (DoorController door in _doors)
        {
            door.DisableDoor();

            closeWhenEntered = false;
        }
    }

    #endregion
}