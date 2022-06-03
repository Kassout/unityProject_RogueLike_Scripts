using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>GrenadeBullet</c> is an IFireable component used to manage the different grenade bullet game object behaviors.
/// </summary>
public class GrenadeBullet : Bullet, IFireable
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
    /// Instance field <c>maxSpeed</c> represents the maximum speed movement value of the bullet game object.
    /// </summary>
    private float _maxSpeed;

    /// <summary>
    /// Instance field <c>prevPosition</c> is a Unity <c>Vector3</c> structure representing the vector coordinates of the last frame position of the bullet.
    /// </summary>
    private Vector3 _prevPosition;

    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the sprite renderer of the bullet game object.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Instance field <c>bulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the bullet game object.
    /// </summary>
    private BulletSpawner _bulletSpawner;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the bullet game object animator.
    /// </summary>
    private Animator _animator;

    private CapsuleCollider2D _collider;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _bulletSpawner = GetComponent<BulletSpawner>();
        _animator = GetComponent<Animator>();
        TryGetComponent<CapsuleCollider2D>(out _collider);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        _speed = _maxSpeed;
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

        _speed = Mathf.Lerp(_speed, 0f, 1.5f * Time.deltaTime);

        _animator.SetFloat("frequency", _maxSpeed / _speed);

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    private void OnDisable()
    {
        if (_bulletSpawner.enabled)
        {
            AudioManager.Instance.PlaySFX(30);
            _bulletSpawner.SpawnBullets();
        } else 
        {
            _bulletSpawner.enabled = true;
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
        _maxSpeed = Random.Range(bulletData.speed / 2f, bulletData.speed);
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