using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>SlowedBullet</c> is an IFireable component used to manage the different slowed bullet game object behaviors.
/// </summary>
public class SlowedBullet : Bullet, IFireable
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

    private float _currentSpeed;

    /// <summary>
    /// Instance field <c>prevPosition</c> is a Unity <c>Vector3</c> structure representing the vector coordinates of the last frame position of the bullet.
    /// </summary>
    private Vector3 _prevPosition;

    public bool shouldFadeAway;

    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the sprite renderer of the bullet game object.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    private Color _colorGradient = new Color(1f, 1f, 1f, 0f);

    private CapsuleCollider2D _collider;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        if (shouldFadeAway)
        {
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }

        TryGetComponent<CapsuleCollider2D>(out _collider);
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        _currentSpeed = _speed;
        if (shouldFadeAway)
        {
            _spriteRenderer.material.color = Color.white;
        }
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
            CheckBulletCollision(_prevPosition);
        }
        else
        {
            CheckBulletColliderCollision(_prevPosition, _collider);
        }

        if (shouldFadeAway)
        {
            _spriteRenderer.material.color = Color.Lerp(_spriteRenderer.material.color, _colorGradient, _currentSpeed / (lifeTime * 2) * Time.deltaTime);
        }
        _currentSpeed = Mathf.Lerp(_currentSpeed, 0f, _currentSpeed / lifeTime * Time.deltaTime);

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            gameObject.SetActive(false);
        }
    }

    #endregion

    #region IFireable

    /// <summary>
    /// This function is responsible for initializing the bullet game object.
    /// </summary>
    /// <param name="bulletData">A BulletEngine <c>BulletData</c> instance representing the information context to setup the current bullet game object with.</param>
    void IFireable.InitialiseBullet(BulletData bulletData)
    {
        _velocity = bulletData.velocity;
        _speed = Random.Range(bulletData.speed / 2f, bulletData.speed);
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