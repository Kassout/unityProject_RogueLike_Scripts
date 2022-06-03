using System;
using UnityEngine;

/// <summary>
/// Class <c>RoomOutlineData</c> is a Unity scriptable object containing the different data defining a room outline.
/// </summary>
[CreateAssetMenu(fileName = "RoomOutline", menuName = "Data/Room Outline Data", order = 1)]
public class RoomOutlineData : ScriptableObject
{
    /// <summary>
    /// Enum field <c>Exit</c> represents the different available exit direction of a room outline.
    /// </summary>
    [Flags]
    public enum Exit {
        None = 0,
        Up = 1 << 0,
        Right = 1 << 1,
        Down = 1 << 2,
        Left = 1 << 3
    }

    /// <summary>
    /// Field <c>exitDirection</c> is an <c>Exit</c> instance representing the exits direction of the current room outline.
    /// </summary>
    public Exit exitDirection;

    /// <summary>
    /// Field <c>roomOutlinePrefab</c> is a Unity <c>GameObject</c> representing the room outline prefabricated object.
    /// </summary>
    public GameObject roomOutlinePrefab;
}
