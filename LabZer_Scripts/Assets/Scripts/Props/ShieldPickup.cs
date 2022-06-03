using UnityEngine;

/// <summary>
/// Class <c>ShieldPickup</c> is a Unity script used to manage the shield pick up game object behavior.
/// </summary>
public class ShieldPickup : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>waitToBeCollected</c> represents duration value before the game object is available to picks up.
    /// </summary>
    [SerializeField]
    private float _waitToBeCollected = 0.5f;

    /// <summary>
    /// Instance field <c>collisionBoxOffsetPosition</c> is a Unity <c>Vector2</c> representing the offset position vector of the center of the collision box of the pick up game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxOffsetPosition;

    /// <summary>
    /// Instance field <c>collisionBoxSize</c> is a Unity <c>Vector2</c> representing the size vector of the collision box of the pick up game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxSize;

    /// <summary>
    /// Instance field <c>layerMask</c> is a Unity <c>LayerMask</c> structure representing layer levels the pick up game object can collide with.
    /// </summary>
    [SerializeField]
    private LayerMask _layerMask;

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (_waitToBeCollected > 0)
        {
            _waitToBeCollected -= Time.deltaTime;
        }

        if (_waitToBeCollected <= 0)
        {
            // Check for collisions
            Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize, 0f, _layerMask);
            if (hit && hit.CompareTag("Player"))
            {
                PlayerHealthController.Instance.ShieldPlayer();

                AudioManager.Instance.PlaySFX(6);
                
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize);
    }
}
