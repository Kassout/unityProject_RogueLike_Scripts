using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>PiercingBullet</c> is an IFireable component used to manage the different piercing bullet game object behaviors.
/// </summary>
public class PiercingBullet : Bullet, IFireable
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>velocity</c> is a Unity <c>Vector3</c> structure representing the velocity direction vector of the bullet game object.
    /// </summary>
    private Vector2 _velocity;

    /// <summary>
    /// Instance field <c>speed</c> represents the speed movement value of the bullet game object.
    /// </summary>
    private float _speed;

    /// <summary>
    /// Instance field <c>prevPosition</c> is a Unity <c>Vector3</c> structure representing the vector coordinates of the last frame position of the bullet.
    /// </summary>
    private Vector3 _prevPosition;

    private LayerMask _whatIsWall;

    private CapsuleCollider2D _collider;

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
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected override void Update()
    {
        _prevPosition = transform.position;

        transform.Translate(Vector2.right * _velocity * _speed * Time.deltaTime);

        if (!_collider)
        {
            CheckBulletCollision(_prevPosition);
        }
        else
        {
            CheckBulletColliderCollision(_prevPosition, _collider);
        }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region IFirable

    /// <summary>
    /// This function is responsible for initializing the bullet game object.
    /// </summary>
    /// <param name="bulletData">A BulletEngine <c>BulletData</c> instance representing the information context to setup the current bullet game object with.</param>
    void IFireable.InitialiseBullet(BulletData bulletData)
    {
        _velocity = bulletData.velocity;
        _speed = bulletData.speed;
        lifeTime = bulletData.lifeTime;
        bulletDamage = bulletData.bulletDamage;
        whatIsTarget = bulletData.whatIsTarget;
        impactEffectPrefab = bulletData.impactEffectPrefab;
        _whatIsWall = bulletData.whatIsWall;
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

    #region Bullet

    /// <summary>
    /// This function is responsible for checking the collision status of the bullet and process the functions it trigger.
    /// </summary>
    /// <param name="prevPosition">A Unity <c>Vector3</c> structure representing the coordinates of the previous bullet game object position.</param>
    protected override Vector3 CheckBulletCollision(Vector3 prevPosition)
    {
        // Check for collisions
        RaycastHit2D hit = Physics2D.Raycast(prevPosition, (transform.position - prevPosition).normalized, (transform.position - prevPosition).magnitude, whatIsTarget);
        if (hit.collider != null)
        {
            Transform impactEffectTransform = ObjectPooler.Instance.GetObjectFromPool(impactEffectPrefab, hit.point, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, hit.normal)));
            if (impactEffectTransform != null)
            {
                impactEffectTransform.gameObject.SetActive(true);

                AudioManager.Instance.PlaySFX(13);
            }

            // In case of collision with target, damage him.
            if (hit.collider.TryGetComponent<EnemyHealthController>(out EnemyHealthController enemyHealthController))
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

            if (_whatIsWall == (_whatIsWall | (1 << hit.collider.gameObject.layer)))
            {
                gameObject.SetActive(false);
            }
        }

        return hit.point;
    }

    protected override Vector3 CheckBulletColliderCollision(Vector3 prevPosition, CapsuleCollider2D collider)
    {
        RaycastHit2D[] hits = Physics2D.CapsuleCastAll(prevPosition, collider.size, collider.direction, transform.rotation.eulerAngles.z, (transform.position - prevPosition).normalized, (transform.position - prevPosition).magnitude, whatIsTarget);
        if (hits.Length > 0)
        {
            foreach (RaycastHit2D raycastHit in hits)
            {
                if (raycastHit.collider != null)
                {
                    // In case of collision with target, damage him.
                    bool isDamaged = false;
                    if (raycastHit.collider.TryGetComponent<EnemyHealthController>(out EnemyHealthController enemyHealthController))
                    {
                        isDamaged = enemyHealthController.TakeDamage(bulletDamage);
                    }
                    else if (raycastHit.collider.TryGetComponent<Breakable>(out Breakable breakable))
                    {
                        breakable.DestroyProp();
                        isDamaged = true;
                    }
                    else if (raycastHit.collider.TryGetComponent<BossController>(out BossController boss))
                    {
                        isDamaged = boss.TakeDamage(bulletDamage);
                    }
                    else if (_whatIsWall == (_whatIsWall | (1 << raycastHit.collider.gameObject.layer)))
                    {
                        gameObject.SetActive(false);
                        isDamaged = true;
                    }

                    if (isDamaged && impactEffectPrefab)
                    {
                        Transform impactEffectTransform = ObjectPooler.Instance.GetObjectFromPool(impactEffectPrefab, raycastHit.point, Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, raycastHit.normal)));
                        if (impactEffectTransform != null)
                        {
                            impactEffectTransform.gameObject.SetActive(true);
                        }
                        
                        AudioManager.Instance.PlaySFX(13);
                    }
                }
            }

        }

        return default;
    }

    #endregion
}
