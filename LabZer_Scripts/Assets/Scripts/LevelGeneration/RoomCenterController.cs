using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class <c>RoomCenterController</c> is a Unity script used to manage the general game room center behavior.
/// </summary>
public class RoomCenterController : MonoBehaviour
{
    #region Fields / Properties

    public static RoomCenterController currentRoom;

    /// <summary>
    /// Instance field <c>openWhenEnemiesCleared</c> represents the open when enemies cleared status of the room center game object.
    /// </summary>
    [SerializeField]
    private bool _openWhenEnemiesCleared;

    /// <summary>
    /// Field <c>enemies</c> is a list of Unity <c>GameObject</c> representing the different enemies present inside the room center.
    /// </summary>
    [SerializeField]
    private List<GameObject> _enemies = new List<GameObject>();

    /// <summary>
    /// Instance field <c>parentRoomController</c> is a Unity <c>RoomController</c> representing the room controller of the room center game object.
    /// </summary>
    public RoomController parentRoomController;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        if (_openWhenEnemiesCleared) {
            parentRoomController.closeWhenEntered = true;
        }

        parentRoomController.activeEvent.AddListener(OnRoomActivated);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (_enemies.Count > 0 && parentRoomController.roomActive && _openWhenEnemiesCleared)
        { 
            for (int i = 0; i < _enemies.Count; i++)
            {
                if (_enemies[i].gameObject == null)
                {
                    _enemies.RemoveAt(i);

                    i--;
                }
            }

            if (_enemies.Count == 0)
            {
                parentRoomController.OpenDoors();
            }
        }          
    }

    /// <summary>
    /// This function is called on room controller activation event.
    /// </summary>
    private void OnRoomActivated()
    {
        foreach(GameObject enemy in _enemies)
        {
            enemy.SetActive(true);
        }

        currentRoom = this;
        parentRoomController.activeEvent.RemoveListener(OnRoomActivated);

        StartCoroutine(ActivateEnemies());
    }

    private IEnumerator ActivateEnemies()
    {
        yield return new WaitForSeconds(0.75f);

        foreach (GameObject enemy in _enemies)
        {
            if (enemy.TryGetComponent<EnemyBT>(out EnemyBT enemyBT))
            {
                enemyBT.enabled = true;
            }

            yield return null;
        }

        yield return null;
    }

    public Transform GetRandomEnemy()
    {
        if (_enemies.Count > 0)
        {
            int enemyIndex = Random.Range(0, _enemies.Count);
            while (!_enemies[Random.Range(0, _enemies.Count)])
            {
                enemyIndex = Random.Range(0, _enemies.Count);
            }

            if (_enemies[enemyIndex].TryGetComponent<GreyDroneSwarmBT>(out GreyDroneSwarmBT droneSwarmBT))
            {
                return droneSwarmBT.swarmUnits[Random.Range(0, droneSwarmBT.swarmUnits.Count)];
            }

            return _enemies[enemyIndex].transform;
        } 
        else
        {
            return null;
        }
    }

    #endregion
}