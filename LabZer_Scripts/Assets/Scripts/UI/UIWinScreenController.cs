using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class <c>UIWinScreenController</c> is a Unity script used to manage the win screen game UI behavior.
/// </summary>
public class UIWinScreenController : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>waitForAnyKey</c> represents the time to wait before player is allow to press any key to change the screen
    /// </summary>
    [SerializeField]
    private float _waitForAnyKey = 2f;

    /// <summary>
    /// Instance field <c>anyKeyText</c> is a Unity <c>GameObject</c> representing the "any key" text UI game object.
    /// </summary>
    [SerializeField]
    private GameObject _anyKeyText;

    /// <summary>
    /// Instance field <c>mainMenuScene</c> represents the name of the main menu scene.
    /// </summary>
    [SerializeField]
    private string _mainMenuScene;

    [SerializeField]
    private Text _finalTimerText;

    private AsyncOperation _levelLoadingProgress;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (TimerController.Instance)
        {
            _finalTimerText.text = TimerController.Instance.currentTimeText.text;
            Destroy (GameObject.Find("Timer(Clone)"));
        }


        if (PlayerController.Instance)
        {
            Destroy(PlayerController.Instance.gameObject);
            Destroy (GameObject.Find("Player(Clone)"));
        }
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        _levelLoadingProgress = SceneManager.LoadSceneAsync(_mainMenuScene);
        _levelLoadingProgress.allowSceneActivation = false;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (_waitForAnyKey > 0)
        {
            _waitForAnyKey -= Time.deltaTime;

            if (_waitForAnyKey <= 0)
            {
                _anyKeyText.SetActive(true);
            }
        } else {
            InputSystem.onAnyButtonPress.CallOnce(ctx => _levelLoadingProgress.allowSceneActivation = true);
        }        
    }
}
