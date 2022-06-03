using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicSpriteSortOrder : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the sprite renderer manager of the game object.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    [SerializeField]
    private int _sortOffSet;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();   
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Update()
    {
        _spriteRenderer.sortingOrder = _sortOffSet + Mathf.RoundToInt(PlayerController.Instance.transform.position.y * -10f);
    }
}
