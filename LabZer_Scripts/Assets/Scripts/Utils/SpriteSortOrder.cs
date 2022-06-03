using UnityEngine;

/// <summary>
/// Class <c>SpriteSortOrder</c> is a Unity script used to manager the sprite sorting order behavior.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteSortOrder : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the sprite renderer manager of the game object.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingOrder = Mathf.RoundToInt(transform.position.y * -10f);
    }
}
