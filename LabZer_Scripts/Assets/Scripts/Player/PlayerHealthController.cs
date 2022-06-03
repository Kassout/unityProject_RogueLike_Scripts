using UnityEngine;

/// <summary>
/// Class <c>PlayerHealthController</c> is a Unity script used to manage the player's character health behavior.
/// </summary>
public class PlayerHealthController : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static PlayerHealthController Instance { get; private set; }

    #endregion

    #region Fields / Properties

    /// <summary>
    /// Instance field <c>maxHealth</c> represents the player max health value.
    /// </summary>
    [HideInInspector]
    public int maxHealth;

    /// <summary>
    /// Instance field <c>invicibilityDuration</c> represents the player invicibility duration value.
    /// </summary>
    [SerializeField]
    private float _invicibilityDuration = 1f;

    /// <summary>
    /// Instance field <c>invicibilityTimeCounter</c> represents the time counter value since the last invicibility of the player.
    /// </summary>
    private float _invicibilityTimeCounter;

    /// <summary>
    /// Instance field <c>currentHealth</c> represents the player current health value.
    /// </summary>
    [HideInInspector]
    public int currentHealth;

    /// <summary>
    /// Instance field <c>currentShields</c> represents the player current shields number.
    /// </summary>
    [HideInInspector]
    public int currentShields;

    [HideInInspector]
    public bool isInvicible;

    /// <summary>
    /// Constant field <c>MAX_SHIELDS</c> represents the player maximum shields number.
    /// </summary>
    private const int MAX_SHIELDS = 3;

    /// <summary>
    /// Instance field <c>playerAnimator</c> is a Unity <c>Animator</c> component representing the animation manager of the player character.
    /// </summary>
    private Animator _playerAnimator;

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
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        _playerAnimator = PlayerController.Instance.gameObject.GetComponent<Animator>();

        maxHealth = CharacterTracker.Instance.maxHealth;
        currentHealth = CharacterTracker.Instance.currentHealth;
        currentShields = CharacterTracker.Instance.currentShields;

        UIController.Instance.UpdateHealthBar(currentHealth, maxHealth);
        UIController.Instance.UpdateShieldsBar(currentShields);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (_invicibilityTimeCounter > 0)
        {
            _invicibilityTimeCounter -= Time.deltaTime;
        }
    }

    #endregion

    #region Public

    /// <summary>
    /// This function is responsible for damaging the player health.
    /// </summary>
    public void DamagePlayer()
    {
        if (_invicibilityTimeCounter <= 0 && currentHealth > 0)
        {
            // If player has shield, protect the player from damage but lose the shield
            if (currentShields > 0)
            {
                currentShields--;

                _playerAnimator.SetTrigger("doHurtShield");

                AudioManager.Instance.PlaySFX(11);

                _invicibilityTimeCounter = _invicibilityDuration;

                UIController.Instance.UpdateShieldsBar(currentShields);
            }
            // Else, damage the player
            else
            {
                currentHealth--;

                _invicibilityTimeCounter = _invicibilityDuration;

                _playerAnimator.SetTrigger("doHurt");

                AudioManager.Instance.PlaySFX(9);

                if (currentHealth == 0)
                {
                    _playerAnimator.SetTrigger("doDeath");

                    AudioManager.Instance.PlaySFX(8);
                }

                UIController.Instance.UpdateHealthBar(currentHealth, maxHealth);
            }
        }
    }

    /// <summary>
    /// This function is responsible for making the player invicible for a given amount of time.
    /// </summary>
    public void MakeInvicibility(float invicibilityDuration)
    {
        _invicibilityTimeCounter = invicibilityDuration;
    }

    /// <summary>
    /// This function is responsible for healing the player of a given amount of health.
    /// </summary>
    public void HealPlayer(int healAmount)
    {
        currentHealth = Mathf.Clamp(currentHealth + healAmount, 0, maxHealth);

        UIController.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

    /// <summary>
    /// This function is responsible for adding a damage shield to the player.
    /// </summary>
    public void ShieldPlayer()
    {
        currentShields = Mathf.Clamp(currentShields + 1, 0, MAX_SHIELDS);

        UIController.Instance.UpdateShieldsBar(currentShields);
    }

    #endregion
}