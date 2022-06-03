using UnityEngine;

/// <summary>
/// Class <c>CoinPickup</c> is a Unity script used to manage the coin pick up game object behavior.
/// </summary>
public class CoinPickup : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>coinAmount</c> represents amount value of coin given by the pick up game object.
    /// </summary>
    [SerializeField]
    private int _coinAmount = 2;

    /// <summary>
    /// Instance field <c>waitToBeCollected</c> represents duration value before the game object is available to picks up.
    /// </summary>
    [SerializeField]
    private float _waitToBeCollected = 0.25f;

    /// <summary>
    /// Instance field <c>collisionCircleOffsetPosition</c> is a Unity <c>Vector2</c> representing the offset position vector of the center of the collision circle of the pick up game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionCircleOffsetPosition;

    /// <summary>
    /// Instance field <c>collisionCircleRadius</c> is a Unity <c>Vector2</c> representing the size vector of the collision circle of the pick up game object.
    /// </summary>
    [SerializeField]
    private float _collisionCircleRadius;

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
            Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + _collisionCircleOffsetPosition, _collisionCircleRadius, _layerMask);
            if (hit && hit.CompareTag("Player"))
            {
                LevelManager.Instance.GetCoins(_coinAmount);

                AudioManager.Instance.PlaySFX(15);
                
                Destroy(gameObject);
            }
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + _collisionCircleOffsetPosition, _collisionCircleRadius);
    }
}
