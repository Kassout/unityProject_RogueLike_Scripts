using UnityEngine;

/// <summary>
/// Class <c>FadeAway</c> is a Unity script used to manage the fade away behavior of a game object's sprite.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class FadeAway : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the game object's sprite renderer manager.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Instance field <c>fadingGradientDuration</c> represents the sprite fading away duration value.
    /// </summary>
    [SerializeField]
    private float _fadingGradientDuration;

    /// <summary>
    /// Instance field <c>fadingGradient</c> is a Unity <c>Color</c> structure representing the color shift to apply to the game object's sprite.
    /// </summary>
    [SerializeField]
    private Color _fadingGradient;

    /// <summary>
    /// Instance field <c>fadingGradientTimeCounter</c> represents the time counter value since the last fading gradient step occurs.
    /// </summary>
    private float _fadingGradientTimeCounter;

    /// <summary>
    /// Instance field <c>defaultSpriteColor</c> is a Unity <c>Color</c> structure representing the default color of the game object's sprite.
    /// </summary>
    private readonly Color _defaultSpriteColor = new Color(1f, 1f, 1f, 1f);

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _fadingGradientTimeCounter = _fadingGradientDuration;
    }

    // Update is called once per frame
    private void Update()
    {
        if (_fadingGradientTimeCounter <= 0)
        {
            _fadingGradientTimeCounter = _fadingGradientDuration;
            _spriteRenderer.color -= _fadingGradient;
        }

        _fadingGradientTimeCounter -= Time.deltaTime;

        if (_spriteRenderer.color.a <= 0f)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        _fadingGradientTimeCounter = _fadingGradientDuration;
        _spriteRenderer.color = _defaultSpriteColor;
    }

    #endregion
}