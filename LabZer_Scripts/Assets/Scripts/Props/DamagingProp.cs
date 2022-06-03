using UnityEngine;

/// <summary>
/// Class <c>DamagingProp</c> is a Unity script used to manage the general damaging prop game object behavior.
/// </summary>
public class DamagingProp : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>collisionBoxOffsetPosition</c> is a Unity <c>Vector2</c> representing the offset position vector of the center of the collision box of the damaging prop game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxOffsetPosition;

    /// <summary>
    /// Instance field <c>collisionBoxSize</c> is a Unity <c>Vector2</c> representing the size vector of the collision box of the damaging prop game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxSize;

    /// <summary>
    /// Instance field <c>layerMask</c> is a Unity <c>LayerMask</c> structure representing layer levels the damaging prop game object can collide with.
    /// </summary>
    [SerializeField]
    private LayerMask _layerMask = (1 << 6);

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        // Check for collisions
        Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize, 0f, _layerMask);
        if (hit && hit.CompareTag("Player"))
        {
            PlayerHealthController.Instance.DamagePlayer();
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize);
    }

    #endregion
}