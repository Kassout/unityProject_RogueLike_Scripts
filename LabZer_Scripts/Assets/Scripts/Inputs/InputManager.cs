using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Class <c>InputManager</c> is a Unity script used to manage the general player inputs behavior.
/// </summary>
public class InputManager : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static InputManager Instance { get; private set; }

    #endregion

    #region Fields / Properties

    /// <summary>
    /// Instance field <c>inputActions</c> is a Unity <c>PlayerInput</c> component object representing the general input bindings of the game.
    /// </summary>
    private InputActions _inputActions;

    /// <summary>
    /// Instance field <c>movementInput</c> is a Unity <c>Vector2</c> component object representing the movement input vector of the player.
    /// </summary>
    private Vector2 _movementInput;

    /// <summary>
    /// Instance property <c>MovemementInput</c> is a Unity <c>Vector2</c> component object representing the movement input vector of the player.
    /// </summary>
    public Vector2 MovemementInput
    {
        get { return _movementInput; }
    }

    /// <summary>
    /// Instance field <c>targetInput</c> is a Unity <c>Vector2</c> component object representing the target position input vector of the player.
    /// </summary>
    private Vector2 _targetInput;

    /// <summary>
    /// Instance property <c>TargetInput</c> is a Unity <c>Vector2</c> component object representing the target position input vector of the player.
    /// </summary>
    public Vector2 TargetInput
    {
        get { return _targetInput; }
    }

    /// <summary>
    /// Instance field <c>shootingInput</c> represents the shooting status of the player.
    /// </summary>
    private bool _shootingInput;

    /// <summary>
    /// Instance property <c>ShootingInput</c> represents the shooting status of the player.
    /// </summary>
    public bool ShootingInput
    {
        get { return _shootingInput; }
    }

    /// <summary>
    /// Instance field <c>dashInput</c> represents the dashing status of the player.
    /// </summary>
    private bool _dashInput;

    /// <summary>
    /// Instance property <c>DashInput</c> represents the dashing status of the player.
    /// </summary>
    public bool DashInput
    {
        get { return _dashInput; }
    }

    /// <summary>
    /// Instance field <c>pauseInput</c> represents the pausing status of the player.
    /// </summary>
    private bool _pauseInput;

    /// <summary>
    /// Instance property <c>PauseInput</c> represents the pausing status of the player.
    /// </summary>
    public bool PauseInput
    {
        get { return _pauseInput; }
    }

    /// <summary>
    /// Instance field <c>isDeviceGamepad</c> represents the gamepad device use status of the player inputs.
    /// </summary>
    private bool _isDeviceGamepad;

    /// <summary>
    /// Instance property <c>IsDeviceGamepad</c> represents the gamepad device use status of the player inputs.
    /// </summary>
    public bool IsDeviceGamepad
    {
        get { return _isDeviceGamepad; }
    }

    /// <summary>
    /// Instance field <c>interactInput</c> represents the interact status of the player.
    /// </summary>
    private bool _interactInput;

    /// <summary>
    /// Instance property <c>InteractInput</c> represents the interact status of the player.
    /// </summary>
    public bool InteractInput
    {
        get { return _interactInput; }
    }

    /// <summary>
    /// Instance field <c>showMapInput</c> represents the show map status of the player.
    /// </summary>
    private bool _showMapInput;

    /// <summary>
    /// Instance property <c>ShowMapInput</c> represents the show map status of the player.
    /// </summary>
    public bool ShowMapInput
    {
        get { return _showMapInput; }
    }

    /// <summary>
    /// Instance field <c>hideMiniMapInput</c> represents the hide mini map status of the player.
    /// </summary>
    private bool _hideMiniMapInput;

    /// <summary>
    /// Instance property <c>HideMiniMapInput</c> represents the hide mini map status of the player.
    /// </summary>
    public bool HideMiniMapInput
    {
        get { return _hideMiniMapInput; }
    }

    /// <summary>
    /// Instance field <c>switchWeaponInput</c> represents the switch weapon status of the player.
    /// </summary>
    private bool _switchWeaponInput;

    /// <summary>
    /// Instance property <c>SwitchWeaponInput</c> represents the switch weapon status of the player.
    /// </summary>
    public bool SwitchWeaponInput
    {
        get { return _switchWeaponInput; }
    }

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Singleton
        Instance = this;
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        if (_inputActions == null)
        {
            _inputActions = new InputActions();

            _inputActions.Player.Move.performed += ctx => _movementInput = ctx.ReadValue<Vector2>();
            _inputActions.Player.Target.performed += ctx => _targetInput = ctx.ReadValue<Vector2>();

            _inputActions.Player.Shoot.performed += _ => _shootingInput = true;
            _inputActions.Player.Shoot.canceled += _ => _shootingInput = false;

            _inputActions.Player.Dash.performed += _ => _dashInput = true;
            _inputActions.Player.Dash.canceled += _ => _dashInput = false;

            _inputActions.Player.Pause.performed += _ => _pauseInput = true;

            _inputActions.Player.Interact.performed += _ => _interactInput = true;

            _inputActions.Player.ShowMap.performed += _ => _showMapInput = true;

            _inputActions.Player.HideMiniMap.performed += _ => _hideMiniMapInput = true;

            _inputActions.Player.SwitchWeapon.performed += _ => _switchWeaponInput = true;
        }

        _inputActions.Enable();
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        _inputActions.Disable();
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    private void LateUpdate()
    {
        _pauseInput = false;
        _hideMiniMapInput = false;
        _showMapInput = false;
        _switchWeaponInput = false;
        _interactInput = false;
    }

    #endregion

    #region PlayerInput

    /// <summary>
    /// This function is called on an input device changed event.
    /// </summary>
    private void OnControlsChanged(PlayerInput playerInput)
    {
        _isDeviceGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
        if (UIController.Instance)
        {
            UIController.Instance.OnControlsChanged(_isDeviceGamepad);
        }
    }

    #endregion
}