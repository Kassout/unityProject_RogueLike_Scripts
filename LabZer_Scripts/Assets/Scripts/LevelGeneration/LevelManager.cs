using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Class <c>LevelManager</c> is a Unity script used to manage the general levels behavior.
/// </summary>
public class LevelManager : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static LevelManager Instance { get; private set; }

    #endregion

    #region Fields / Properties

    /// <summary>
    /// Instance field <c>waitToLoad</c> represents the time value to wait for level to load.
    /// </summary>
    [SerializeField]
    private float _waitToLoad = 4f;

    /// <summary>
    /// Instance field <c>nextLevel</c> represents the name of the next level to load.
    /// </summary>
    [SerializeField]
    private string _nextLevel;

    /// <summary>
    /// Instance field <c>isPaused</c> represents the is paused status of the game.
    /// </summary>
    [HideInInspector]
    public bool isPaused;

    /// <summary>
    /// Instance field <c>currentCoins</c> represents the number value of coins currently held by the player.
    /// </summary>
    [HideInInspector]
    public int currentCoins;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private Transform _startPoint;

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

        PlayerController.Instance.transform.position = _startPoint.position;
        PlayerController.Instance.canMove = true;

        currentCoins = CharacterTracker.Instance.currentCoins;

        Time.timeScale = 1f;

        UIController.Instance.coinText.text = currentCoins.ToString();
        if (!AudioManager.Instance.isBossLevel)
        {
            UIController.Instance.levelMessage.text = SceneManager.GetActiveScene().name + " / 5";
        }
        else
        {
            UIController.Instance.levelMessage.text = SceneManager.GetActiveScene().name;
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (InputManager.Instance.PauseInput)
        {
            PauseUnpause();
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// This coroutine is responsible for ending the current level and load the next one.
    /// </summary>
    public IEnumerator LevelEnd()
    {
        AudioManager.Instance.PlayLevelWin();

        PlayerController.Instance.canMove = false;

        UIController.Instance.StartFadeToBlack();

        yield return new WaitForSeconds(_waitToLoad);

        CharacterTracker.Instance.currentCoins = currentCoins;
        CharacterTracker.Instance.currentHealth = PlayerHealthController.Instance.currentHealth;
        CharacterTracker.Instance.maxHealth = PlayerHealthController.Instance.maxHealth;
        CharacterTracker.Instance.currentShields = PlayerHealthController.Instance.currentShields;

        SceneManager.LoadScene(_nextLevel);
    }

    /// <summary>
    /// This function is responsible for pausing and unpausing the game.
    /// </summary>
    public void PauseUnpause()
    {
        if (!isPaused)
        {
            UIController.Instance.pauseScreen.SetActive(true);
            if (InputManager.Instance.IsDeviceGamepad)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = true;
            }

            isPaused = true;
            AudioManager.Instance.StopSFX();

            Time.timeScale = 0f;
        }
        else
        {
            UIController.Instance.pauseScreen.SetActive(false);
            if (InputManager.Instance.IsDeviceGamepad)
            {
                Cursor.visible = false;
            }
            else
            {
                Cursor.visible = false;
            }

            isPaused = false;

            Time.timeScale = 1f;
        }
    }

    /// <summary>
    /// This function is responsible for adding a given amount of coin to the player coins count.
    /// </summary>
    public void GetCoins(int amount)
    {
        currentCoins += amount;

        UIController.Instance.coinText.text = currentCoins.ToString();
    }

    /// <summary>
    /// This function is responsible for spending away a given amount of coin of the player coins count.
    /// </summary>
    public void SpendCoins(int amount)
    {
        currentCoins -= amount;

        if (currentCoins < 0)
        {
            currentCoins = 0;
        }

        UIController.Instance.coinText.text = currentCoins.ToString();
    }

    #endregion
}