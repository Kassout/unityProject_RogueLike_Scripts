using UnityEngine;

/// <summary>
/// Class <c>CameraController</c> is a Unity script used to manage the general game camera behavior.
/// </summary>
public class CameraController : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static CameraController Instance { get; private set; }

    #endregion

    #region Fields / Properties

    /// <summary>
    /// Instance field <c>moveSpeed</c> represents the speed value of the movement of the camera.
    /// </summary>
    [SerializeField]
    private float _moveSpeed;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private Camera _mainCamera;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private Camera _bigMapCamera;

    /// <summary>
    /// Instance field <c>target</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the camera target.
    /// </summary>
    private Transform _target;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private bool _isBigMapActive;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (_target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(_target.position.x, _target.position.y, transform.position.z), _moveSpeed * Time.deltaTime);
        }

        if (InputManager.Instance.ShowMapInput)
        {
            if (!_isBigMapActive)
            {
                ActivateBigMap();
            }
            else
            {
                DeactivateBigMap();
            }
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// This function is responsible for changing the camera target.
    /// </summary>
    public void ChangeTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private void ActivateBigMap()
    {
        if (!LevelManager.Instance.isPaused)
        {
            _isBigMapActive = true;

            _bigMapCamera.enabled = true;
            _mainCamera.enabled = false;

            PlayerController.Instance.canMove = false;

            Time.timeScale = 0f;

            UIController.Instance.SwitchUnswitchBigMap(_isBigMapActive);
        }
    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private void DeactivateBigMap()
    {
        if (!LevelManager.Instance.isPaused)
        {
            _isBigMapActive = false;

            _bigMapCamera.enabled = false;
            _mainCamera.enabled = true;

            PlayerController.Instance.canMove = true;

            Time.timeScale = 1f;

            UIController.Instance.SwitchUnswitchBigMap(_isBigMapActive);
        }
    }

    #endregion
}