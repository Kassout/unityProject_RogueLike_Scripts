using UnityEngine;

/// <summary>
/// Class <c>LevelExitController</c> is a Unity script used to manage the level exit behavior.
/// </summary>
public class LevelExitController : MonoBehaviour
{
    /// <summary>
    /// Sent when another object enters a trigger collider attached to this
    /// object (2D physics only).
    /// </summary>
    /// <param name="other">The other Collider2D involved in this collision.</param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            AudioManager.Instance.StopSFX();
            StartCoroutine(LevelManager.Instance.LevelEnd());
        }
    }
}
