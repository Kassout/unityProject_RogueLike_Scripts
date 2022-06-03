using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Class <c>UIController</c> is a Unity script used to manage the general game UI behavior.
/// </summary>
public class UIController : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static UIController Instance { get; private set; }

    #endregion

    #region Fields / Properties

    /// <summary>
    /// Field <c>deathScreen</c> is a Unity <c>GameObject</c> instance representing the UI death screen of the game.
    /// </summary>
    public GameObject deathScreen;

    /// <summary>
    /// Field <c>pauseMenu</c> is a Unity <c>GameObject</c> instance representing the UI pause menu screen of the game.
    /// </summary>
    public GameObject pauseScreen;

    /// <summary>
    /// Instance field <c>healthBar</c> is a Unity <c>Image</c> UI component representing the player health bar UI element of the screen.
    /// </summary>
    [SerializeField]
    private Image _healthBar;

    /// <summary>
    /// Instance field <c>healthBarSprites</c> is a list of Unity <c>Sprite</c> component representing the different player health bar with the different level of fillings.
    /// </summary>
    [SerializeField]
    private List<Sprite> _healthBarSprites;

    /// <summary>
    /// Instance field <c>healthText</c> is a Unity <c>Text</c> UI component representing the player health text UI element of the screen.
    /// </summary>
    [SerializeField]
    private Text _healthText;

    /// <summary>
    /// Instance field <c>coinText</c> is a Unity <c>Text</c> UI component representing the player coin text UI element of the screen.
    /// </summary>
    public Text coinText;

    /// <summary>
    /// Instance field <c>shieldsBar</c> is a Unity <c>Image</c> UI component representing the player shields bar UI element of the screen.
    /// </summary>
    [SerializeField]
    private Image _shieldsBar;

    /// <summary>
    /// Instance field <c>shieldsBarSprites</c> is a list of Unity <c>Sprite</c> component representing the different player shields bar with the different level of fillings.
    /// </summary>
    [SerializeField]
    private List<Sprite> _shieldsBarSprites;

    /// <summary>
    /// Instance field <c>fadeScreen</c> is a Unity <c>Image</c> UI component representing the fading screen of the level scene.
    /// </summary>
    [SerializeField]
    private Image _fadeScreen;

    /// <summary>
    /// Instance field <c>fadeSpeed</c> represents the speed value of the screen fading.
    /// </summary>
    [SerializeField]
    private float _fadeSpeed;

    /// <summary>
    /// Instance field <c>newGameScene</c> represents the name of the main game scene.
    /// </summary>
    [SerializeField]
    private string _newGameScene;

    /// <summary>
    /// Instance field <c>mainMenuScene</c> represents the name of the main menu scene.
    /// </summary>
    [SerializeField]
    private string _mainMenuScene;

    /// <summary>
    /// Instance field <c>resumeButton</c> is a Unity <c>GameObject</c> instance representing the resume button game object.
    /// </summary>
    [SerializeField]
    private GameObject _resumeButton;

    /// <summary>
    /// Instance field <c>pauseMainMenuButton</c> is a Unity <c>GameObject</c> instance representing the pause screen main menu button game object.
    /// </summary>
    [SerializeField]
    private GameObject _pauseMainMenuButton;

    /// <summary>
    /// Instance field <c>newGameButton</c> is a Unity <c>GameObject</c> instance representing the new game button game object.
    /// </summary>
    [SerializeField]
    private GameObject _newGameButton;

    /// <summary>
    /// Instance field <c>deathMainMenuButton</c> is a Unity <c>GameObject</c> instance representing the death screen main menu button game object.
    /// </summary>
    [SerializeField]
    private GameObject _deathMainMenuButton;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private GameObject _mapDisplay;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private GameObject _UIRightBorder;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private Image _currentWeaponImageWithMap;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private Image _currentWeaponImageWithoutMap;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private Text _weaponText;

    /// <summary>
    /// Instance field <c>fadeToBlack</c> represents the fade to black status of the fading screen of the level scene.
    /// </summary>
    private bool _fadeToBlack;

    /// <summary>
    /// Instance field <c>fadeOutBlack</c> represents the fade out status of the fading screen of the level scene.
    /// </summary>
    private bool _fadeOutBlack;

    /// <summary>
    /// Instance field <c>pauseScreenEventSystem</c> is a Unity <c>EventSystem</c> component representing the event system of the pause screen game object.
    /// </summary>
    private EventSystem _pauseScreenEventSystem;

    /// <summary>
    /// Instance field <c>deathScreenEventSystem</c> is a Unity <c>EventSystem</c> component representing the event system of the death screen game object.
    /// </summary>
    private EventSystem _deathScreenEventSystem;

    /// <summary>
    /// Instance field <c>resumeButtonEventTrigger</c> is a Unity <c>EventTrigger</c> component representing the event trigger of the resume button game object.
    /// </summary>
    private EventTrigger _resumeButtonEventTrigger;

    /// <summary>
    /// Instance field <c>pauseMainMenuButtonEventTrigger</c> is a Unity <c>EventTrigger</c> component representing the event trigger of the pause screen main menu button game object.
    /// </summary>
    private EventTrigger _pauseMainMenuButtonEventTrigger;

    /// <summary>
    /// Instance field <c>newGameButtonEventTrigger</c> is a Unity <c>EventTrigger</c> component representing the event trigger of the new game button game object.
    /// </summary>
    private EventTrigger _newGameButtonEventTrigger;

    /// <summary>
    /// Instance field <c>deathMainMenuButtonEventTrigger</c> is a Unity <c>EventTrigger</c> component representing the event trigger of the death screen main menu button game object.
    /// </summary>
    private EventTrigger _deathMainMenuButtonEventTrigger;

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

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private bool _isHide;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public Slider bossHealthBar;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public Image bossHealthFiller;

    public Text levelMessage;

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

        _pauseScreenEventSystem = pauseScreen.GetComponentInChildren<EventSystem>();
        _deathScreenEventSystem = deathScreen.GetComponentInChildren<EventSystem>();

        _resumeButtonEventTrigger = _resumeButton.GetComponent<EventTrigger>();
        _pauseMainMenuButtonEventTrigger = _pauseMainMenuButton.GetComponent<EventTrigger>();
        _newGameButtonEventTrigger = _newGameButton.GetComponent<EventTrigger>();
        _deathMainMenuButtonEventTrigger = _deathMainMenuButton.GetComponent<EventTrigger>();

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
        _fadeOutBlack = true;
        _fadeToBlack = false;

        OnControlsChanged(false);
        SetupWeaponUI(PlayerController.Instance.availableWeapons[PlayerController.Instance.currentWeapon]);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (_fadeOutBlack)
        {
            _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, Mathf.MoveTowards(_fadeScreen.color.a, 0f, _fadeSpeed * Time.deltaTime));
            if (_fadeScreen.color.a == 0f)
            {
                _fadeOutBlack = false;
                TimerController.Instance.StartStopwatch();
            }
        }

        if (_fadeToBlack)
        {
            _fadeScreen.color = new Color(_fadeScreen.color.r, _fadeScreen.color.g, _fadeScreen.color.b, Mathf.MoveTowards(_fadeScreen.color.a, 1f, _fadeSpeed * Time.deltaTime));
            if (_fadeScreen.color.a == 1f)
            {
                _fadeToBlack = false;
            }
        }

        if (InputManager.Instance.HideMiniMapInput)
        {
            SwitchUnswitchMiniMap();
        }

    }

    #endregion

    #region Public

    /// <summary>
    /// This function is responsible for updating the health bar UI element.
    /// </summary>
    public void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        int sprite = Mathf.CeilToInt(9f * currentHealth / maxHealth);
        _healthBar.sprite = _healthBarSprites[sprite];

        _healthText.text = currentHealth + " / " + maxHealth;
    }

    /// <summary>
    /// This function is responsible for updating the shield bar UI element.
    /// </summary>
    public void UpdateShieldsBar(int currentShields)
    {
        _shieldsBar.sprite = _shieldsBarSprites[currentShields];
    }

    /// <summary>
    /// This function is responsible for fading out level scene screen to black screen.
    /// </summary>
    public void StartFadeToBlack()
    {
        TimerController.Instance.StopStopwatch();
        _fadeToBlack = true;
        _fadeOutBlack = false;
    }

    /// <summary>
    /// This function is responsible for starting a new game, loading the main game scene.
    /// </summary>
    public void NewGame()
    {
        Time.timeScale = 1f;
        Destroy(GameObject.Find("Player(Clone)"));
        Destroy(GameObject.Find("Timer(Clone)"));
        SceneManager.LoadScene(_newGameScene);
    }

    /// <summary>
    /// This function is responsible for returning to the main menu screen, loading the main menu scene.
    /// </summary>
    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;
        Destroy(GameObject.Find("Player(Clone)"));
        Destroy(GameObject.Find("Timer(Clone)"));
        SceneManager.LoadScene(_mainMenuScene);
    }

    /// <summary>
    /// This function is responsible for resuming the game.
    /// </summary>
    public void Resume()
    {
        LevelManager.Instance.PauseUnpause();
    }

    /// <summary>
    /// This functions is call on select button event triggered.
    /// </summary>
    private void OnSelectButton()
    {
        AudioManager.Instance.PlaySFX(17);
    }

    /// <summary>
    /// This functions is call on submit button event triggered.
    /// </summary>
    private void OnSubmitButton()
    {
        AudioManager.Instance.StopSFX();
        AudioManager.Instance.PlaySFX(18);
    }

    /// <summary>
    /// This function is call on input device controls changed event triggered.
    /// </summary>
    public void OnControlsChanged(bool isDeviceGamepad)
    {
        if (isDeviceGamepad)
        {
            Cursor.visible = false;
            _pauseScreenEventSystem.SetSelectedGameObject(_resumeButton);

            _pauseMainMenuButtonEventTrigger.triggers.Add(_gamepadSelectEntry);
            _pauseMainMenuButtonEventTrigger.triggers.Add(_gamepadSubmitEntry);
            _pauseMainMenuButtonEventTrigger.triggers.Remove(_kbmSelectEntry);
            _pauseMainMenuButtonEventTrigger.triggers.Remove(_kbmSubmitEntry);

            _resumeButtonEventTrigger.triggers.Add(_gamepadSelectEntry);
            _resumeButtonEventTrigger.triggers.Add(_gamepadSubmitEntry);
            _resumeButtonEventTrigger.triggers.Remove(_kbmSelectEntry);
            _resumeButtonEventTrigger.triggers.Remove(_kbmSubmitEntry);

            _deathScreenEventSystem.SetSelectedGameObject(_resumeButton);

            _deathMainMenuButtonEventTrigger.triggers.Add(_gamepadSelectEntry);
            _deathMainMenuButtonEventTrigger.triggers.Add(_gamepadSubmitEntry);
            _deathMainMenuButtonEventTrigger.triggers.Remove(_kbmSelectEntry);
            _deathMainMenuButtonEventTrigger.triggers.Remove(_kbmSubmitEntry);

            _newGameButtonEventTrigger.triggers.Add(_gamepadSelectEntry);
            _newGameButtonEventTrigger.triggers.Add(_gamepadSubmitEntry);
            _newGameButtonEventTrigger.triggers.Remove(_kbmSelectEntry);
            _newGameButtonEventTrigger.triggers.Remove(_kbmSubmitEntry);
        }
        else
        {
            Cursor.visible = true;
            _pauseScreenEventSystem.SetSelectedGameObject(null);

            _pauseMainMenuButtonEventTrigger.triggers.Add(_kbmSelectEntry);
            _pauseMainMenuButtonEventTrigger.triggers.Add(_kbmSubmitEntry);
            _pauseMainMenuButtonEventTrigger.triggers.Remove(_gamepadSelectEntry);
            _pauseMainMenuButtonEventTrigger.triggers.Remove(_gamepadSubmitEntry);

            _resumeButtonEventTrigger.triggers.Add(_kbmSelectEntry);
            _resumeButtonEventTrigger.triggers.Add(_kbmSubmitEntry);
            _resumeButtonEventTrigger.triggers.Remove(_gamepadSelectEntry);
            _resumeButtonEventTrigger.triggers.Remove(_gamepadSubmitEntry);

            _deathScreenEventSystem.SetSelectedGameObject(null);

            _deathMainMenuButtonEventTrigger.triggers.Add(_kbmSelectEntry);
            _deathMainMenuButtonEventTrigger.triggers.Add(_kbmSubmitEntry);
            _deathMainMenuButtonEventTrigger.triggers.Remove(_gamepadSelectEntry);
            _deathMainMenuButtonEventTrigger.triggers.Remove(_gamepadSubmitEntry);

            _newGameButtonEventTrigger.triggers.Add(_kbmSelectEntry);
            _newGameButtonEventTrigger.triggers.Add(_kbmSubmitEntry);
            _newGameButtonEventTrigger.triggers.Remove(_gamepadSelectEntry);
            _newGameButtonEventTrigger.triggers.Remove(_gamepadSubmitEntry);
        }
    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public void SwitchUnswitchBigMap(bool isBigMapActive)
    {
        if (_isHide)
        {
            _mapDisplay.SetActive(false);
            _UIRightBorder.SetActive(true);
        }
        else
        {
            _mapDisplay.SetActive(!isBigMapActive);
            _UIRightBorder.SetActive(isBigMapActive);
        }
    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public void SwitchUnswitchMiniMap()
    {
        if (!_isHide)
        {
            _mapDisplay.SetActive(false);
            _UIRightBorder.SetActive(true);
            _isHide = true;
        }
        else
        {
            _mapDisplay.SetActive(true);
            _UIRightBorder.SetActive(false);
            _isHide = false;
        }

    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public void SetupWeaponUI(Weapon currentWeapon)
    {
        _currentWeaponImageWithMap.sprite = currentWeapon.weaponSprite;
        _currentWeaponImageWithoutMap.sprite = currentWeapon.weaponSprite;
    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public void UpdateUITextOnNewWeapon(Weapon newWeapon)
    {
        StartCoroutine(ShowNewWeaponName(newWeapon.weaponName));
    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private IEnumerator ShowNewWeaponName(string weaponName)
    {
        _weaponText.text = weaponName;

        _weaponText.color = new Color(_weaponText.color.r, _weaponText.color.g, _weaponText.color.b, 0f);
        _weaponText.gameObject.SetActive(true);

        float opacity = _weaponText.color.a;
        while (_weaponText.color.a < 0.9f)
        {
            opacity = Mathf.Lerp(opacity, 1f, 3f * Time.deltaTime);

            _weaponText.color = new Color(_weaponText.color.r, _weaponText.color.g, _weaponText.color.b, opacity);

            yield return null;
        }

        yield return new WaitForSeconds(0.25f);

        opacity = _weaponText.color.a;
        while (_weaponText.color.a > 0.1f)
        {
            opacity = Mathf.Lerp(opacity, 0f, 3f * Time.deltaTime);

            _weaponText.color = new Color(_weaponText.color.r, _weaponText.color.g, _weaponText.color.b, opacity);

            yield return null;
        }

        _weaponText.gameObject.SetActive(false);
        _weaponText.color = new Color(_weaponText.color.r, _weaponText.color.g, _weaponText.color.b, 0f);
    }

    #endregion
}