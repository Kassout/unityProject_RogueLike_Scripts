using System.Collections.Generic;
using UnityEngine;
using System;
using Random = UnityEngine.Random;

/// <summary>
/// Class <c>LevelGenerator</c> is a Unity script used to manage the level generation behavior.
/// </summary>
public class LevelGenerator : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>generatorPoint</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the generator point.
    /// </summary>
    [SerializeField]
    private Transform _generatorPoint;

    /// <summary>
    /// Field <c>GameObject</c> is a Unity <c>GameObject</c> representing the layout sprite of the rooms.
    /// </summary>
    [SerializeField]
    private GameObject _layoutRoom;

    /// <summary>
    /// Instance field <c>startColor</c> is a Unity <c>Color</c> structure representing the layout color of the starting room.
    /// </summary>
    [SerializeField]
    private Color _startColor;

    /// <summary>
    /// Instance field <c>endColor</c> is a Unity <c>Color</c> structure representing the layout color of the ending room.
    /// </summary>
    [SerializeField]
    private Color _endColor;

    /// <summary>
    /// Instance field <c>shopColor</c> is a Unity <c>Color</c> structure representing the layout color of the shopping room.
    /// </summary>
    [SerializeField]
    private Color _shopColor;

    /// <summary>
    /// Instance field <c>weaponRoomColor</c> is a Unity <c>Color</c> structure representing the layout color of the weapon room.
    /// </summary>
    [SerializeField]
    private Color _weaponRoomColor;

    /// <summary>
    /// Instance field <c>distanceToEnd</c> represents the distance value to cover before ending the room generation.
    /// </summary>
    [SerializeField]
    private int _distanceToEnd;

    /// <summary>
    /// Instance field <c>xOffset</c> represents the x coordinate position offset of the generator point movement.
    /// </summary>
    [SerializeField]
    private float _xOffset = 18f;

    /// <summary>
    /// Instance field <c>yOffset</c> represents the y coordinate position offset of the generator point movement.
    /// </summary>
    [SerializeField]
    private float _yOffset = 10f;

    /// <summary>
    /// Instance field <c>whatIsRoom</c> is a Unity <c>LayerMask</c> structure representing the layer levels the room outlines can collide with.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsRoom;

    /// <summary>
    /// Field <c>roomsOutline</c> is a list of Unity <c>RoomOutlineData</c> scriptable objects representing the different rooms outline to generate the level with.
    /// </summary>
    [SerializeField]
    private List<RoomOutlineData> _roomsOutline;

    /// <summary>
    /// Instance field <c>centerStart</c> is a Unity <c>RoomCenterController</c> script representing the room center controller of the starting level room.
    /// </summary>
    [SerializeField]
    private RoomCenterController _centerStart;

    /// <summary>
    /// Instance field <c>centerEnd</c> is a Unity <c>RoomCenterController</c> script representing the room center controller of the ending level room.
    /// </summary>
    [SerializeField]
    private RoomCenterController _centerEnd;

    /// <summary>
    /// Instance field <c>includeShop</c> represents the include shop status of the level generation.
    /// </summary>
    [SerializeField]
    private bool _includeShop;

    /// <summary>
    /// Instance field <c>centerShop</c> is a Unity <c>RoomCenterController</c> script representing the room center controller of the shopping level room.
    /// </summary>
    [SerializeField]
    private RoomCenterController _centerShop;

    /// <summary>
    /// Instance field <c>minDistanceToShop</c> represents the minimum distance in rooms before the level generation can spawn a shop room.
    /// </summary>
    [SerializeField]
    private int _minDistanceToShop;

    /// <summary>
    /// Instance field <c>maxDistanceToShop</c> represents the maximum distance in rooms after the level generation can't spawn a shop room.
    /// </summary>
    [SerializeField]
    private int _maxDistanceToShop;

    /// <summary>
    /// Instance field <c>includeWeaponRoom</c> represents the include weapon room status of the level generation.
    /// </summary>
    [SerializeField]
    private bool _includeWeaponRoom;

    /// <summary>
    /// Instance field <c>centerWeaponRoom</c> is a Unity <c>RoomCenterController</c> script representing the room center controller of the weapon level room.
    /// </summary>
    [SerializeField]
    private RoomCenterController _centerWeaponRoom;

    /// <summary>
    /// Instance field <c>minDistanceToWeaponRoom</c> represents the minimum distance in rooms before the level generation can spawn a weapon room.
    /// </summary>
    [SerializeField]
    private int _minDistanceToWeaponRoom;

    /// <summary>
    /// Instance field <c>maxDistanceToWeaponRoom</c> represents the maximum distance in rooms after the level generation can't spawn a weapon room.
    /// </summary>
    [SerializeField]
    private int _maxDistanceToWeaponRoom;

    /// <summary>
    /// Instance field <c>potentialCenters</c> is a list of Unity <c>RoomCenterController</c> scripts representing the different room center controllers of the generated level.
    /// </summary>
    [SerializeField]
    public RoomCenterController[] _potentialCenters;

    /// <summary>
    /// Enum field <c>Direction</c> represents the different generation movement directions of the generator point.
    /// </summary>
    public enum Direction
    {
        up,
        right,
        down,
        left
    };

    /// <summary>
    /// Field <c>selectedDirection</c> is a <c>Direction</c> instance representing the selected movement direction of the generator point.
    /// </summary>
    private Direction selectedDirection;

    /// <summary>
    /// Field <c>endRoom</c> is a Unity <c>GameObject</c> representing the ending level room game object.
    /// </summary>
    private GameObject _endRoom;

    /// <summary>
    /// Field <c>shopRoom</c> is a Unity <c>GameObject</c> representing the shopping level room game object.
    /// </summary>
    private GameObject _shopRoom;

    /// <summary>
    /// Field <c>weaponRoom</c> is a Unity <c>GameObject</c> representing the weapon level room game object.
    /// </summary>
    private GameObject _weaponRoom;

    /// <summary>
    /// Field <c>layoutRoomObjects</c> is a list of Unity <c>GameObject</c> representing the layout rooms of the generated level.
    /// </summary>
    private List<GameObject> _layoutRoomObjects = new List<GameObject>();

    /// <summary>
    /// Field <c>generatedOutlines</c> is a list of Unity <c>GameObject</c> instance representing the rooms outline of the generated level.
    /// </summary>
    private List<GameObject> _generatedOutlines = new List<GameObject>();

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        // Generate level with rooms layout
        GameObject startRoom = Instantiate(_layoutRoom, _generatorPoint.position, _generatorPoint.rotation);
        startRoom.GetComponent<SpriteRenderer>().color = _startColor;

        selectedDirection = (Direction)Random.Range(0, 4);

        MoveGenerationPoint();

        for (int i = 0; i < _distanceToEnd; i++)
        {
            GameObject newRoom = Instantiate(_layoutRoom, _generatorPoint.position, _generatorPoint.rotation);

            if (i + 1 == _distanceToEnd)
            {
                newRoom.GetComponent<SpriteRenderer>().color = _endColor;
                _endRoom = newRoom;
            }
            else
            {
                _layoutRoomObjects.Add(newRoom);
            }

            selectedDirection = (Direction)Random.Range(0, 4);
            MoveGenerationPoint();

            while (Physics2D.OverlapCircle(_generatorPoint.position, 0.2f, _whatIsRoom))
            {
                MoveGenerationPoint();
            }
        }

        if (_includeShop)
        {
            int shopSelector = Random.Range(_minDistanceToShop - 1, _maxDistanceToShop);

            _shopRoom = _layoutRoomObjects[shopSelector];
            _layoutRoomObjects.RemoveAt(shopSelector);

            _shopRoom.GetComponent<SpriteRenderer>().color = _shopColor;
        }

        if (_includeWeaponRoom)
        {
            int weaponRoomSelector = Random.Range(_minDistanceToWeaponRoom - 1, _maxDistanceToWeaponRoom);

            _weaponRoom = _layoutRoomObjects[weaponRoomSelector];
            _layoutRoomObjects.RemoveAt(weaponRoomSelector);

            _weaponRoom.GetComponent<SpriteRenderer>().color = _weaponRoomColor;
        }

        // Create room outlines
        PlaceRoomOutlineData(Vector3.zero, startRoom);
        foreach (GameObject room in _layoutRoomObjects)
        {
            PlaceRoomOutlineData(room.transform.position, room);
        }
        PlaceRoomOutlineData(_endRoom.transform.position, _endRoom);

        if (_includeShop)
        {
            PlaceRoomOutlineData(_shopRoom.transform.position, _shopRoom);
        }

        if (_includeWeaponRoom)
        {
            PlaceRoomOutlineData(_weaponRoom.transform.position, _weaponRoom);
        }

        // Create room centers
        foreach (GameObject outline in _generatedOutlines)
        {
            RoomCenterController roomCenter = null;

            if (outline.transform.position == Vector3.zero)
            {
                roomCenter = Instantiate(_centerStart, outline.transform.position, transform.rotation);
                RoomCenterController.currentRoom = roomCenter;
            }
            else if (outline.transform.position == _endRoom.transform.position)
            {
                roomCenter = Instantiate(_centerEnd, outline.transform.position, transform.rotation);
            }
            else if (_includeShop && outline.transform.position == _shopRoom.transform.position)
            {
                roomCenter = Instantiate(_centerShop, outline.transform.position, transform.rotation);
            }
            else if (_includeWeaponRoom && outline.transform.position == _weaponRoom.transform.position)
            {
                roomCenter = Instantiate(_centerWeaponRoom, outline.transform.position, transform.rotation);
            }
            else
            {
                int centerSelected = Random.Range(0, _potentialCenters.Length);

                roomCenter = Instantiate(_potentialCenters[centerSelected], outline.transform.position, transform.rotation);
            }

            roomCenter.parentRoomController = outline.GetComponent<RoomController>();
            roomCenter.transform.parent = outline.transform.parent;
        }
    }

    #endregion

    #region Private

    /// <summary>
    /// This function is responsible for picking the right room outline for the right exit directions of the current room.
    /// </summary>
    private RoomOutlineData PickRoomOutlineData(RoomOutlineData.Exit exitDirection)
    {
        foreach (RoomOutlineData room in _roomsOutline)
        {
            if (room.exitDirection == exitDirection)
            {
                return room;
            }
            else if (Convert.ToSByte(room.exitDirection) == -1 || Convert.ToByte(room.exitDirection) == 15)
            {
                return room;
            }
        }
        return null;
    }

    /// <summary>
    /// This function is responsible for moving the generation point.
    /// </summary>
    private void MoveGenerationPoint()
    {
        switch (selectedDirection)
        {
            case Direction.up:
                _generatorPoint.position += new Vector3(0f, _yOffset, 0f);
                break;
            case Direction.down:
                _generatorPoint.position -= new Vector3(0f, _yOffset, 0f);
                break;
            case Direction.right:
                _generatorPoint.position += new Vector3(_xOffset, 0f, 0f);
                break;
            case Direction.left:
                _generatorPoint.position -= new Vector3(_xOffset, 0f, 0f);
                break;
        }
    }

    /// <summary>
    /// This function is responsible for placing the room outline at the given room position.
    /// </summary>
    private void PlaceRoomOutlineData(Vector3 roomPosition, GameObject room = null)
    {
        bool roomAbove = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, _yOffset, 0f), 0.2f, _whatIsRoom);
        bool roomBelow = Physics2D.OverlapCircle(roomPosition + new Vector3(0f, -_yOffset, 0f), 0.2f, _whatIsRoom);
        bool roomLeft = Physics2D.OverlapCircle(roomPosition + new Vector3(-_xOffset, 0f, 0f), 0.2f, _whatIsRoom);
        bool roomRight = Physics2D.OverlapCircle(roomPosition + new Vector3(_xOffset, 0f, 0f), 0.2f, _whatIsRoom);

        RoomOutlineData.Exit exitDirection = (RoomOutlineData.Exit)(Convert.ToByte(roomAbove) + Convert.ToByte(roomRight) * 0x02 + Convert.ToByte(roomBelow) * 0x04 + Convert.ToByte(roomLeft) * 0x08);

        RoomOutlineData roomPrefab = PickRoomOutlineData(exitDirection);

        if (roomPrefab != null)
        {
            GameObject roomtOutline = Instantiate(roomPrefab.roomOutlinePrefab, roomPosition, transform.rotation);
            if (room)
            {
                roomtOutline.transform.parent = room.transform;
            }
            _generatedOutlines.Add(roomtOutline);
        }
    }

    #endregion
}