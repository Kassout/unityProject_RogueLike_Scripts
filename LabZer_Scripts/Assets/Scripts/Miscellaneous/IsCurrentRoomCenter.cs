using UnityEngine;

public class IsCurrentRoomCenter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        RoomCenterController.currentRoom = GetComponent<RoomCenterController>();
    }
}
