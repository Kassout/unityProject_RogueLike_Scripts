using UnityEngine;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
public class CharacterTracker : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static CharacterTracker Instance { get; private set; }

    #endregion

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public int currentHealth;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public int currentShields;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public int maxHealth;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public int currentCoins;

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
}
