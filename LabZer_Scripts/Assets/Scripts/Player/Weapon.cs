using System.Collections;
using BulletEngine;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// TODO: add comment.
/// </summary>
public class Weapon : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>firePoint</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the fire point of the player character.
    /// </summary>
    [SerializeField]
    private Transform _firePoint;

    /// <summary>
    /// Instance field <c>shotRate</c> represents the shot rate value of the player's gun.
    /// </summary>
    [SerializeField]
    private float _shotRate;

    /// <summary>
    /// Instance field <c>shotCounter</c> represents the time counter value since the last shot of the player.
    /// </summary>
    private float _shotTimeCounter;

    /// <summary>
    /// Instance field <c>bulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the player game object.
    /// </summary>
    private BulletSpawner _bulletSpawner;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public string weaponName;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public Sprite weaponSprite;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public int itemCost;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public int weaponShotSFX;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public GameObject associatedPickup;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _bulletSpawner = GetComponentInChildren<BulletSpawner>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (PlayerController.Instance.canMove && !LevelManager.Instance.isPaused)
        {
            _shotTimeCounter -= Time.deltaTime;

            // Capture shooting input
            if (InputManager.Instance.ShootingInput)
            {
                // Control shot rate
                if (_shotTimeCounter <= 0)
                {
                    _bulletSpawner.SpawnBullets(_firePoint.rotation.eulerAngles.z);

                    Shake.ShakeCamera();
                    if (InputManager.Instance.IsDeviceGamepad)
                    {
                        StartCoroutine(DoGamepadVibration());
                    }

                    AudioManager.Instance.PlaySFX(weaponShotSFX);

                    _shotTimeCounter = 1f / _shotRate;
                }
            }
        }
    }


    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private IEnumerator DoGamepadVibration()
    {
        Gamepad.current.SetMotorSpeeds(2, 3);

        yield return new WaitForSeconds(0.05f);

        Gamepad.current.SetMotorSpeeds(0, 0);

        yield return null;
    }
}
