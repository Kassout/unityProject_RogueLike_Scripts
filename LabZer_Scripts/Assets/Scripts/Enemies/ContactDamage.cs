
using UnityEngine;

/// <summary>
/// Class <c>ContactDamage</c> is a Unity script used to manage the contact damage behavior of a game object.
/// </summary>    
public class ContactDamage : MonoBehaviour
{
    #region Fields / Properties
    
    /// <summary>
    /// Instance field <c>contactAreaRadius</c> represents the radius value of the contact area of the contact damage game object.
    /// </summary>
    [SerializeField]
    private float _contactAreaRadius;

    /// <summary>
    /// Instance field <c>collisionCircleOffsetPosition</c> is a Unity <c>Vector2</c> representing the offset position vector of the collision circle center of the contact damage game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionCircleOffsetPosition;

    /// <summary>
    /// Instance field <c>whatTakeDamage</c> is a Unity <c>LayerMask</c> structure representing layer levels inside which game objects will take contact damage.
    /// </summary>
    [SerializeField]
    private LayerMask _whatTakeDamage;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + _collisionCircleOffsetPosition, _contactAreaRadius, _whatTakeDamage);
        if (hit && hit.CompareTag("Player"))
        {
            PlayerHealthController.Instance.DamagePlayer();
        }
        else if (hit && hit.TryGetComponent<Breakable>(out Breakable breakable))
        {
            breakable.DestroyProp();
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + _collisionCircleOffsetPosition, _contactAreaRadius);
    }

    #endregion
}