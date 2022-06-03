using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>ClassicBullet</c> is an IFireable component used to manage the different classic bullet game object behaviors.
/// </summary>
public class ClassicBullet : Bullet, IFireable
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