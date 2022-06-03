using UnityEngine;

/// <summary>
/// TODO: add comment.
/// </summary>
public class ExplosionController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>collisionCircleOffsetPosition</c> is a Unity <c>Vector2</c> representing the offset position vector of the center of the collision circle of the damaging explosion game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionCircleOffsetPosition;

    /// <summary>
    /// Instance field <c>collisionCircleRadius</c> represents the radius size of the collision circle of the damaging explosion game object.
    /// </summary>
    [SerializeField]
    private float _collisionCircleRadius;

    /// <summary>
    /// Instance field <c>layerMask</c> is a Unity <c>LayerMask</c> structure representing layer levels the damaging explosion game object can collide with.
    /// </summary>
    [SerializeField]
    private LayerMask _layerMask = (1 << 6);

    private ParticleSystem _particleSystem;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        _particleSystem.Play();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        // Check for collisions
        Collider2D hit = Physics2D.OverlapCircle((Vector2)transform.position + _collisionCircleOffsetPosition, _collisionCircleRadius, _layerMask);
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
        Gizmos.DrawWireSphere((Vector2)transform.position + _collisionCircleOffsetPosition, _collisionCircleRadius);
    }

    #endregion
}
