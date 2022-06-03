using BulletEngine;
using UnityEngine;

/// <summary>
/// TODO: add comment.
/// </summary>
public class MissileBullet : Bullet, IFireable
{
    #region Fields / Properties

    [SerializeField]
    private GameObject _explosionEffectPrefab;

    /// <summary>
    /// Instance field <c>velocity</c> is a Unity <c>Vector3</c> structure representing the velocity direction vector of the bullet game object.
    /// </summary>
    private Vector2 _velocity;

    /// <summary>
    /// Instance field <c>speed</c> represents the speed movement value of the bullet game object.
    /// </summary>
    private float _speed;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private float _currentSpeed;

    /// <summary>
    /// Instance field <c>prevPosition</c> is a Unity <c>Vector3</c> structure representing the vector coordinates of the last frame position of the bullet.
    /// </summary>
    private Vector3 _prevPosition;

    private Vector3 _hitPosition;

    private CapsuleCollider2D _collider;

    public float acceleration;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        TryGetComponent<CapsuleCollider2D>(out _collider);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        _currentSpeed = 1f;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected override void Update()
    {
        _prevPosition = transform.position;

        transform.Translate(Vector2.right * _velocity * _currentSpeed * Time.deltaTime);

        if (!_collider)
        {
            _hitPosition = CheckBulletCollision(_prevPosition);
        }
        else
        {
            _hitPosition = CheckBulletColliderCollision(_prevPosition, _collider);
        }

        _currentSpeed += acceleration * Time.deltaTime;

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            if (_hitPosition == Vector3.zero)
            {
                _hitPosition = transform.position;
            }

            Transform explosionEffectTransform = ObjectPooler.Instance.GetObjectFromPool(_explosionEffectPrefab, _hitPosition, Quaternion.identity);
            if (explosionEffectTransform != null)
            {
                explosionEffectTransform.gameObject.SetActive(true);
            }

            AudioManager.Instance.PlaySFX(31);
            
            gameObject.SetActive(false);
        }
    }

    protected override Vector3 CheckBulletColliderCollision(Vector3 prevPosition, CapsuleCollider2D collider)
    {
        // Check for collisions
        RaycastHit2D hit = Physics2D.CapsuleCast(prevPosition, collider.size, collider.direction, transform.rotation.eulerAngles.z, (transform.position - prevPosition).normalized, (transform.position - prevPosition).magnitude, whatIsTarget);
        if (hit.collider != null)
        {
            if (impactEffectPrefab)
            {
                Transform impactEffectTransform = ObjectPooler.Instance.GetObjectFromPool(impactEffectPrefab, hit.point, Quaternion.identity);
                if (impactEffectTransform != null)
                {
                    impactEffectTransform.gameObject.SetActive(true);
                }
            }

            AudioManager.Instance.PlaySFX(31);

            // In case of collision with target, damage him.
            if (hit.collider.CompareTag("Player"))
            {
                PlayerHealthController.Instance.DamagePlayer();
                if (PlayerController.Instance.isDashing)
                {
                    return hit.point;
                }
            }
            else if (hit.collider.TryGetComponent<EnemyHealthController>(out EnemyHealthController enemyHealthController))
            {
                enemyHealthController.TakeDamage(bulletDamage);
            }
            else if (hit.collider.TryGetComponent<Breakable>(out Breakable breakable))
            {
                breakable.DestroyProp();
            }
            else if (hit.collider.TryGetComponent<BossController>(out BossController boss))
            {
                boss.TakeDamage(bulletDamage);
            }

            gameObject.SetActive(false);
        }

        return hit.point;
    }

    #endregion

    #region IFireable

    /// <summary>
    /// This function is responsible for initializing the bullet game object.
    /// </summary>
    /// <param name="bulletData">A BulletEngine <c>BulletData</c> instance representing the information context to setup the current bullet game object with.</param>
    void IFireable.InitialiseBullet(BulletData bulletData)
    {
        _speed = bulletData.speed;
        _velocity = bulletData.velocity;
        lifeTime = bulletData.lifeTime;
        bulletDamage = bulletData.bulletDamage;
        whatIsTarget = bulletData.whatIsTarget;
        impactEffectPrefab = bulletData.impactEffectPrefab;
    }

    /// <summary>
    /// This function is responsible getting the bullet game object.
    /// </summary>
    /// <returns>A Unity <c>GameObject</c> representing the bullet game object.</returns>
    GameObject IFireable.GetGameObject()
    {
        return gameObject;
    }

    #endregion
}
