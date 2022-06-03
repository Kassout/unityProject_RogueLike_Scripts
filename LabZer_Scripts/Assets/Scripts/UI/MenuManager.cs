using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

/// <summary>
/// Class <c>MenuManager</c> is a Unity script used to manage the start menu behavior.
/// </summary>
public class MenuManager : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>levelToLoad</c> represents the name of the level scene to load.
    /// </summary>
    [SerializeField]
    private string _levelToLoad;

    /// <summary>
    /// Instance field <c>buttonSelectSFX</c> is a Unity <c>AudioSource</c> component representing the button select music audio source.
    /// </summary>
    [SerializeField]
    private AudioSource _buttonSelectSFX;

    /// <summary>
    /// Instance field <c>buttonSubmitSFX</c> is a Unity <c>AudioSource</c> component representing the button submit music audio source.
    /// </summary>
    [SerializeField]
    private AudioSource _buttonSubmitSFX;

    /// <summary>
    /// Instance field <c>startButton</c> is a Unity <c>GameObject</c> instance representing the start button game object.
    /// </summary>
    [SerializeField]
    private GameObject _startButton;

    /// <summary>
    /// Instance field <c>exitButton</c> is a Unity <c>GameObject</c> instance representing the exit button game object.
    /// </summary>
    [SerializeField]
    private GameObject _exitButton;

    /// <summary>
    /// Instance field <c>startButtonEventTrigger</c> is a Unity <c>EventTrigger</c> component representing the event trigger of the start button game object.
    /// </summary>
    private EventTrigger _startButtonEventTrigger;

    /// <summary>
    /// Instance field <c>exitButtonEventTrigger</c> is a Unity <c>EventTrigger</c> component representing the event trigger of the exit button game object.
    /// </summary>
    private EventTrigger _exitButtonEventTrigger;

    /// <summary>
    /// Instance field <c>gamepadSelectEntry</c> is a Unity <c>EventTrigger.Entry</c> object representing the event trigger entry for a gamepad button selection.
    /// </summary>
    private EventTrigger.Entry _gamepadSelectEntry;

    /// <summary>
    /// Instance field <c>gamepadSubmitEntry</c> is a Unity <c>EventTrigger.Entry</c> object representing the event trigger entry for a gamepad button submission.
    /// </summary>
    private EventTrigger.Entry _gamepadSubmitEntry;

    /// <summary>
    /// Instance field <c>kbmSelectEntry</c> is a Unity <c>EventTrigger.Entry</c> object representing the event trigger entry for a keyboard mouse button selection.
    /// </summary>
    private EventTrigger.Entry _kbmSelectEntry;

    /// <summary>
    /// Instance field <c>kbmSubmitEntry</c> is a Unity <c>EventTrigger.Entry</c> object representing the event trigger entry for a keyboard mouse button submission.
    /// </summary>
    private EventTrigger.Entry _kbmSubmitEntry;

    private AsyncOperation loadingLevelProgress;


    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _startButtonEventTrigger = _startButton.GetComponent<EventTrigger>();
        _exitButtonEventTrigger = _exitButton.GetComponent<EventTrigger>();

        _gamepadSelectEntry = new EventTrigger.Entry();
        _gamepadSelectEntry.eventID = EventTriggerType.Select;
        _gamepadSelectEntry.callback.AddListener((data) => { OnSelectButton(); });

        _gamepadSubmitEntry = new EventTrigger.Entry();
        _gamepadSubmitEntry.eventID = EventTriggerType.Submit;
        _gamepadSubmitEntry.callback.AddListener((data) => { OnSubmitButton(); });

        _kbmSelectEntry = new EventTrigger.Entry();
        _kbmSelectEntry.eventID = EventTriggerType.PointerEnter;
        _kbmSelectEntry.callback.AddListener((data) => { OnSelectButton(); });

        _kbmSubmitEntry = new EventTrigger.Entry();
        _kbmSubmitEntry.eventID = EventTriggerType.PointerClick;
        _kbmSubmitEntry.callback.AddListener((data) => { OnSubmitButton(); });
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        StartCoroutine(LoadLevelAsync());

        Cursor.visible = true;
        EventSystem.current.SetSelectedGameObject(null);

        _startButtonEventTrigger.triggers.Add(_kbmSelectEntry);
        _startButtonEventTrigger.triggers.Add(_kbmSubmitEntry);
        _startButtonEventTrigger.triggers.Remove(_gamepadSelectEntry);
        _startButtonEventTrigger.triggers.Remove(_gamepadSubmitEntry);

        _exitButtonEventTrigger.triggers.Add(_kbmSelectEntry);
        _exitButtonEventTrigger.triggers.Add(_kbmSubmitEntry);
        _exitButtonEventTrigger.triggers.Remove(_gamepadSelectEntry);
        _exitButtonEventTrigger.triggers.Remove(_gamepadSubmitEntry);
    }

    private IEnumerator LoadLevelAsync()
    {
        loadingLevelProgress = SceneManager.LoadSceneAsync(_levelToLoad, LoadSceneMode.Single);
        loadingLevelProgress.allowSceneActivation = false;

        while (!loadingLevelProgress.isDone)
        {
            yield return null;
        }

        Debug.Log("Level Loaded");
    }

    /// <summary>
    /// This function is responsible for starting the game.
    /// </summary>
    public void StartGame()
    {
        loadingLevelProgress.allowSceneActivation = true;
    }

    /// <summary>
    /// This function is responsible for exiting the game.
    /// </summary>
    public void ExitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// This functions is call on select button event triggered.
    /// </summary>
    private void OnSelectButton()
    {
        _buttonSelectSFX.Play();
    }

    /// <summary>
    /// This functions is call on submit button event triggered.
    /// </summary>
    private void OnSubmitButton()
    {
        _buttonSelectSFX.Stop();
        _buttonSubmitSFX.Play();
    }

    /// <summary>
    /// This function is call on input device controls changed event triggered.
    /// </summary>
    private void OnControlsChanged(PlayerInput playerInput)
    {
        if (_startButtonEventTrigger && _exitButtonEventTrigger)
        {
            if (playerInput.currentControlScheme.Equals("Gamepad"))
            {
                Cursor.visible = false;
                EventSystem.current.SetSelectedGameObject(_startButton);

                _startButtonEventTrigger.triggers.Add(_gamepadSelectEntry);
                _startButtonEventTrigger.triggers.Add(_gamepadSubmitEntry);
                _startButtonEventTrigger.triggers.Remove(_kbmSelectEntry);
                _startButtonEventTrigger.triggers.Remove(_kbmSubmitEntry);

                _exitButtonEventTrigger.triggers.Add(_gamepadSelectEntry);
                _exitButtonEventTrigger.triggers.Add(_gamepadSubmitEntry);
                _exitButtonEventTrigger.triggers.Remove(_kbmSelectEntry);
                _exitButtonEventTrigger.triggers.Remove(_kbmSubmitEntry);
            }
            else
            {
                Cursor.visible = true;
                EventSystem.current.SetSelectedGameObject(null);

                _startButtonEventTrigger.triggers.Add(_kbmSelectEntry);
                _startButtonEventTrigger.triggers.Add(_kbmSubmitEntry);
                _startButtonEventTrigger.triggers.Remove(_gamepadSelectEntry);
                _startButtonEventTrigger.triggers.Remove(_gamepadSubmitEntry);

                _exitButtonEventTrigger.triggers.Add(_kbmSelectEntry);
                _exitButtonEventTrigger.triggers.Add(_kbmSubmitEntry);
                _exitButtonEventTrigger.triggers.Remove(_gamepadSelectEntry);
                _exitButtonEventTrigger.triggers.Remove(_gamepadSubmitEntry);
            }
        }
    }
}
