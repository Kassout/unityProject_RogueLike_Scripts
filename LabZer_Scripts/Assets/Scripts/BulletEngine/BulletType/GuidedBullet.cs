using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>GuidedBullet</c> is an IFireable component used to manage the different guided bullet game object behaviors.
/// </summary>
public class GuidedBullet : Bullet, IFireable
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>speed</c> represents the speed movement value of the bullet game object.
    /// </summary>
    private float _speed;

    /// <summary>
    /// Instance field <c>prevPosition</c> is a Unity <c>Vector3</c> structure representing the vector coordinates of the last frame position of the bullet.
    /// </summary>
    private Vector3 _prevPosition;

    /// <summary>
    /// Instance field <c>moveDirection</c> is a Unity <c>Vector2</c> structure representing the movement direction vector of the bullet game object.
    /// </summary>
    private Vector2 _moveDirection;

    /// <summary>
    /// Instance field <c>target</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the bullet's target game object.
    /// </summary>
    [HideInInspector]
    public Transform target;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    protected override void OnEnable()
    {
        base.OnEnable();
        if (target != null)
        {
            _moveDirection = (target.position - transform.position).normalized;

            transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.right, _moveDirection));
        }
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    protected override void Update()
    {
        _prevPosition = transform.position;

        transform.Translate(Vector2.right * _speed * Time.deltaTime);

        CheckBulletCollision(_prevPosition);

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