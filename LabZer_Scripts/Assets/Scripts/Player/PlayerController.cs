using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class <c>PlayerController</c> is a Unity script used to manage the general player's character behavior.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(InputManager))]
public class PlayerController : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static PlayerController Instance { get; private set; }

    #endregion

    #region Fields / Properties

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the player's character movement speed value.
    /// </summary>
    [SerializeField]
    private float _moveSpeed = 5f;

    /// <summary>
    /// Instance field <c>gunTransform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the gun pivot of the player character.
    /// </summary>
    public Transform gunTransform;

    /// <summary>
    /// TODO: add comment
    /// </summary>
    private Dictionary<string, string> _bulletModifier;

    /// <summary>
    /// Instance field <c>dashSpeed</c> represents the player's character dash speed value.
    /// </summary>
    [SerializeField]
    private float _dashSpeed;

    /// <summary>
    /// Instance field <c>dashDuration</c> represents the player dash duration value.
    /// </summary>
    [SerializeField]
    private float _dashDuration;

    /// <summary>
    /// Instance field <c>dashDuration</c> represents the player dash cooldown value.
    /// </summary>
    [SerializeField]
    private float _dashCooldownDuration;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private GameObject _fadeAwayPlayerCloneEffectPrefab;

    /// <summary>
    /// Instance field <c>whatIsEnemy</c> is a Unity <c>LayerMask</c> structure representing layer levels the enemies game object can be find in.
    /// </summary>
    [SerializeField]
    private LayerMask _whatIsEnemy;

    /// <summary>
    /// Instance field <c>aimSensibility</c> represents the rotation sensibility value of the aim direction using gamepad. 
    /// </summary>
    [SerializeField]
    private float _aimSensibility;

    /// <summary>
    /// Instance field <c>stickyAimSensibility</c> represents the rotation sensibility value of the aim direction using gamepad in a sticky aim mode. 
    /// </summary>
    [SerializeField]
    private float _stickyAimSensibility;

    /// <summary>
    /// Instance field <c>crossHairPoint</c> is a Unity <c>GameObject</c> instance representing the player crosshair game object.
    /// </summary>
    [SerializeField]
    private GameObject _crossHairPoint;

    /// <summary>
    /// Instance field <c>dashTimeCounter</c> represents the time counter value since the player dash started.
    /// </summary>
    [HideInInspector]
    public float dashTimeCounter;

    /// <summary>
    /// Instance field <c>canMove</c> represents the can move status of the player character.
    /// </summary>
    [HideInInspector]
    public bool canMove = true;

    /// <summary>
    /// Instance field <c>isDashing</c> represents the is dahsing status of the player character.
    /// </summary>
    [HideInInspector]
    public bool isDashing = false;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private Quaternion _lastAimRotation;

    /// <summary>
    /// Instance field <c>dashCooldownTimeCounter</c> represents the time counter value since the player dash cooldown started.
    /// </summary>
    private float _dashCooldownTimeCounter;

    /// <summary>
    /// Instance field <c>move</c> is a Unity <c>Vector2</c> structure representing the movement vector of the player's character.
    /// </summary>
    private Vector2 _move;

    /// <summary>
    /// Instance field <c>activeMoveSpeed</c> represents the player's character currently active movement speed value.
    /// </summary>
    private float _activeMoveSpeed;

    /// <summary>
    /// Instance field <c>rigidbody</c> is a Unity <c>Rigidbody2D</c> component representing the player character game object link to the Unity's physics engine.
    /// </summary>
    private Rigidbody2D _rigidbody;

    /// <summary>
    /// Instance field <c>camera</c> is a Unity <c>Camera</c> component representing the main camera manager of the game.
    /// </summary>
    private Camera _camera;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the animation manager of the player character.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the sprite renderer of the player character.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public List<Weapon> availableWeapons = new List<Weapon>();

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [HideInInspector]
    public int currentWeapon;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Singleton
        Instance = this;
        DontDestroyOnLoad(gameObject);

        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        _camera = Camera.main;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        _activeMoveSpeed = _moveSpeed;
        canMove = true;

        UIController.Instance.SetupWeaponUI(availableWeapons[currentWeapon]);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (canMove && !LevelManager.Instance.isPaused)
        {
            // Capture target input
            Vector3 target = InputManager.Instance.TargetInput;

            // Change player offset hand angle computation depending input device
            if (InputManager.Instance.IsDeviceGamepad)
            {
                ComputeAimRotationGamepad(target);
            }
            else
            {
                ComputeAimDirectionKBM(target);
            }

            // Capture move input
            if (dashTimeCounter <= 0)
            {
                _move = InputManager.Instance.MovemementInput;
                _move.Normalize();

                // Animate player character
                if (_move != Vector2.zero)
                {
                    animator.SetBool("isMoving", true);

                    AudioManager.Instance.PlaySFXLoop(10, true);

                    if (target == Vector3.zero)
                    {
                        if (_move.x < 0)
                        {
                            _spriteRenderer.flipX = true;
                        }
                        else
                        {
                            _spriteRenderer.flipX = false;
                        }
                    }
                }
                else
                {
                    animator.SetBool("isMoving", false);

                    AudioManager.Instance.PlaySFXLoop(10, false);
                }
            }
            else if (_move.Equals(Vector2.zero))
            {
                _move = Vector2.right * transform.localScale.x;
            }

            // Player dash
            if (InputManager.Instance.DashInput)
            {
                if (_dashCooldownTimeCounter <= 0 && dashTimeCounter <= 0)
                {
                    _activeMoveSpeed = _dashSpeed;
                    dashTimeCounter = _dashDuration;
                    isDashing = true;

                    animator.SetTrigger("doDash");

                    AudioManager.Instance.PlaySFX(7);

                    PlayerHealthController.Instance.MakeInvicibility(_dashDuration);
                }
            }

            // Compute dash cooldown
            if (dashTimeCounter > 0)
            {
                dashTimeCounter -= Time.deltaTime;

                if (dashTimeCounter <= 0)
                {
                    _activeMoveSpeed = _moveSpeed;
                    _dashCooldownTimeCounter = _dashCooldownDuration;
                    isDashing = false;
                }
            }

            if (_dashCooldownTimeCounter > 0)
            {
                _dashCooldownTimeCounter -= Time.deltaTime;
            }

            if (InputManager.Instance.SwitchWeaponInput)
            {
                if (availableWeapons.Count > 0)
                {
                    currentWeapon++;
                    if (currentWeapon >= availableWeapons.Count)
                    {
                        currentWeapon = 0;
                    }

                    SwitchWeapon();
                }
                else
                {
                    Debug.LogError("Player has no guns!");
                }
            }
        }
        else
        {
            _move = Vector2.zero;

            animator.SetBool("isMoving", false);

            AudioManager.Instance.PlaySFXLoop(10, false);
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {
        _rigidbody.velocity = _move * _activeMoveSpeed;
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        _rigidbody.velocity = Vector2.zero;

        if (AudioManager.Instance)
        {
            AudioManager.Instance.PlaySFXLoop(10, false);
        }
    }

    #endregion

    #region Private

    /// <summary>
    /// This function is responsible for computing the player aim direction when using a gamepad device.
    /// </summary>
    private void ComputeAimRotationGamepad(Vector2 targetPosition)
    {
        Vector3 aimDirection = Vector3.right * targetPosition.x + Vector3.up * targetPosition.y;
        if (aimDirection.sqrMagnitude > 0.0f)
        {
            // The aim won't snap into position, but so will the crosshair.
            Vector3 crossHairDirection = aimDirection;
            _crossHairPoint.transform.localPosition = Vector2.MoveTowards(_crossHairPoint.transform.localPosition, crossHairDirection.normalized * 2f, 50 * Time.deltaTime);
            _crossHairPoint.transform.localScale = Vector2.MoveTowards(_crossHairPoint.transform.localScale, Vector2.one * 1.5f, 50 * Time.deltaTime);

            // On aim direction meet an enemy on his trajectory, lower the aim rotation speed so the player can aim track his enemy.
            float activeAimSpeed = 0.0f;
            RaycastHit2D hit = Physics2D.Raycast(gunTransform.position, aimDirection, Mathf.Infinity, _whatIsEnemy);
            if (hit)
            {
                activeAimSpeed = _stickyAimSensibility;
                aimDirection = hit.collider.transform.position - gunTransform.position;
            }
            else
            {
                activeAimSpeed = _aimSensibility;
            }

            // Flip player character sprite according to aiming direction.
            if (aimDirection.x < 0f)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }


            Vector3 rotatedVectorToTarget = Quaternion.Euler(0f, 0f, 90f) * aimDirection;
            Quaternion newRotation = Quaternion.LookRotation(Vector3.forward, rotatedVectorToTarget);
            // If aim rotation angle is large, snap the rotation instantly.
            if (Mathf.Abs(newRotation.eulerAngles.z - _lastAimRotation.eulerAngles.z) >= 90)
            {
                gunTransform.rotation = newRotation;
            }
            else
            {
                gunTransform.rotation = Quaternion.RotateTowards(gunTransform.rotation, newRotation, Time.deltaTime * activeAimSpeed);
            }
            _lastAimRotation = newRotation;
        }
        else
        {
            _crossHairPoint.transform.localPosition = Vector2.MoveTowards(_crossHairPoint.transform.localPosition, Vector2.zero, 50 * Time.deltaTime);
            _crossHairPoint.transform.localScale = Vector2.MoveTowards(_crossHairPoint.transform.localScale, Vector2.zero, 50 * Time.deltaTime);
        }
    }

    /// <summary>
    /// This function is responsible for computing the player aim direction when using a keyboard & mouse devices.
    /// </summary>
    private void ComputeAimDirectionKBM(Vector2 targetPosition)
    {
        Ray ray = Camera.main.ScreenPointToRay(targetPosition);
        Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
        float rayDistance;

        if (groundPlane.Raycast(ray, out rayDistance))
        {
            Vector3 point = ray.GetPoint(rayDistance);
            LookAt(point);

            if (point.x < transform.position.x)
            {
                _spriteRenderer.flipX = true;
            }
            else
            {
                _spriteRenderer.flipX = false;
            }
        }
    }

    private void LookAt(Vector3 lookPoint)
    {
        Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, lookPoint.y, transform.position.z);
        gunTransform.right = heightCorrectedPoint - gunTransform.position;
        _crossHairPoint.transform.position = heightCorrectedPoint;
        _crossHairPoint.transform.localScale = Vector3.one * 1.5f;
    }

    /// <summary>
    /// This function is called on death animation ends.
    /// </summary>
    private void OnDeathAnimationEnds()
    {
        Time.timeScale = 0f;
        UIController.Instance.deathScreen.SetActive(true);
        AudioManager.Instance.PlayGameOver();
        if (InputManager.Instance.IsDeviceGamepad)
        {
            Cursor.visible = false;
        }
        else
        {
            Cursor.visible = true;
        }
    }

    /// <summary>
    /// This function is responsible for spawing a fading away sprite clone of the player.
    /// </summary>
    private void SpawnFadingAwayPlayerCone()
    {
        Transform _fadeAwayPlayerCloneEffectTransform = ObjectPooler.Instance.GetObjectFromPool(_fadeAwayPlayerCloneEffectPrefab, transform.position, transform.rotation);
        if (_fadeAwayPlayerCloneEffectTransform != null)
        {
            if (_spriteRenderer.flipX)
            {
                _fadeAwayPlayerCloneEffectTransform.localScale = new Vector3(1f, 1f, -1f);
            }
            else
            {
                _fadeAwayPlayerCloneEffectTransform.localScale = new Vector3(1f, 1f, 1f);
            }

            _fadeAwayPlayerCloneEffectTransform.gameObject.SetActive(true);
        }
    }

    public void SwitchWeapon()
    {
        foreach (Weapon weapon in availableWeapons)
        {
            weapon.gameObject.SetActive(false);
        }

        availableWeapons[currentWeapon].gameObject.SetActive(true);

        AudioManager.Instance.PlaySFX(20);
        UIController.Instance.SetupWeaponUI(availableWeapons[currentWeapon]);
    }

    public void EquipWeapon(Weapon weapon)
    {
        if (availableWeapons.Count < 2)
        {
            availableWeapons.Add(weapon);
            currentWeapon = PlayerController.Instance.availableWeapons.Count - 1;
            SwitchWeapon();
        }
        else
        {
            Instantiate(availableWeapons[currentWeapon].associatedPickup, transform.position, transform.rotation);
            Destroy(availableWeapons[currentWeapon].gameObject);

            availableWeapons[currentWeapon] = weapon;
            SwitchWeapon();
        }

        AudioManager.Instance.PlaySFX(29);

        UIController.Instance.UpdateUITextOnNewWeapon(weapon);
    }

    #endregion
}