using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>LaserBeam</c> is an IFireable component used to manage the different laser beam bullet game object behaviors.
/// </summary>
public class LaserBeam : Bullet, IFireable
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
    /// Instance field <c>whatIsWall</c> is a Unity <c>LayerMask</c> structure representing the different layers considered as wall for the bullet game object.
    /// </summary>
    private LayerMask _whatIsWall;

    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the sprite renderer of the bullet game object.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    private Transform _impactEffectTransform;

    private Transform _instanceOrigin;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        _impactEffectTransform = ObjectPooler.Instance.GetObjectFromPool(impactEffectPrefab, Vector3.zero, Quaternion.identity);
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected override void Update()
    {
        timer -= Time.deltaTime;
        
        if (timer <= 0)
        {
            gameObject.SetActive(false);
            _impactEffectTransform.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void FixedUpdate()
    {

        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right, Mathf.Infinity, _whatIsWall);
        if (hit.collider != null)
        {
            if (_impactEffectTransform != null && !_impactEffectTransform.gameObject.activeInHierarchy)
            {
                _impactEffectTransform.gameObject.SetActive(true);
            }
            else
            {
                _impactEffectTransform.position = hit.point;
                _impactEffectTransform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, hit.normal));

                AudioManager.Instance.PlaySFXLoop(13, true);
            }

            _spriteRenderer.size = new Vector2(hit.distance + 0.5f, 1);
        }

        // Check for collisions
        RaycastHit2D[] hits = Physics2D.BoxCastAll(transform.position + (transform.rotation * _collisionBoxOffsetPosition), _collisionBoxSize, transform.eulerAngles.z, transform.right, Vector2.Distance(transform.position, hit.point), whatIsTarget);
        foreach (RaycastHit2D raycastHit in hits)
        {
            if (raycastHit.collider != null)
            {
                // In case of collision with target, damage him.
                if (raycastHit.collider.CompareTag("Player"))
                {
                    PlayerHealthController.Instance.DamagePlayer();
                }
                else if (raycastHit.collider.TryGetComponent<EnemyHealthController>(out EnemyHealthController enemyHealthController))
                {
                    enemyHealthController.TakeDamage(bulletDamage);
                }
                else if (raycastHit.collider.TryGetComponent<Breakable>(out Breakable breakable))
                {
                    breakable.DestroyProp();
                }
                else if (raycastHit.collider.TryGetComponent<BossController>(out BossController boss))
                {
                    boss.TakeDamage(bulletDamage);
                }
            }
        }

    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube(transform.position + (transform.rotation * _collisionBoxOffsetPosition), _collisionBoxSize);
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        if (_impactEffectTransform)
        {
            _impactEffectTransform.gameObject.SetActive(false);
        }

        if (_instanceOrigin)
        {
            Invoke("ReAttachGameObject", 0.2f);
        }
    }

    private void ReAttachGameObject()
    {
        transform.SetParent(_instanceOrigin);
    }

    #endregion

    #region IFireable

    /// <summary>
    /// This function is responsible for initializing the bullet game object.
    /// </summary>
    /// <param name="bulletData">A BulletEngine <c>BulletData</c> instance representing the information context to setup the current bullet game object with.</param>
    void IFireable.InitialiseBullet(BulletData bulletData)
    {
        _whatIsWall = bulletData.whatIsWall;
        lifeTime = bulletData.lifeTime;
        bulletDamage = bulletData.bulletDamage;
        whatIsTarget = bulletData.whatIsTarget;
        impactEffectPrefab = bulletData.impactEffectPrefab;
        _instanceOrigin = bulletData.instanceOrigin;
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