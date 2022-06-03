using BehaviorTree;
using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>TaskFireTarget</c> is a BehaviorTree <c>Node</c> used to define a fire target task behavior.
/// </summary>
public class TaskFireTarget : Node
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>shotRate</c> represents the fire shot frenquency value of the game object.
    /// </summary>
    private float _shotRate;

    /// <summary>
    /// Instance field <c>switchAnimationAttack</c> represents the switch animation attack status of the game object.
    /// </summary>
    private bool _switchAnimationAttack;

    /// <summary>
    /// Instance field <c>bulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the game object.
    /// </summary>
    private BulletSpawner _bulletSpawner;

    /// <summary>
    /// Instance field <c>transform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the game object.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Instance field <c>gunTransform</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the game object gun.
    /// </summary>
    private Transform _gunTransform;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the game object animator.
    /// </summary>
    private Animator _animator;

    /// <summary>
    /// Instance field <c>spriteRenderer</c> is a Unity <c>SpriteRenderer</c> component representing the sprite renderer of the game object.
    /// </summary>
    private SpriteRenderer _spriteRenderer;

    /// <summary>
    /// Instance field <c>shotTimeCounter</c> represents the time value since the last fire shot.
    /// </summary>
    private float _shotTimeCounter = 0f;

    /// <summary>
    /// Instance field <c>flip</c> represents the flip status of the game object
    /// </summary>
    private bool _flip = true;

    #endregion

    #region BehaviorTree

    /// <summary>
    /// Default Constructor.
    /// </summary>
    public TaskFireTarget(Transform transform, Transform gunTransform, float shotRate, BulletSpawner bulletSpawner, bool switchAnimationAttack)
    {
        _transform = transform;
        _gunTransform = gunTransform;
        _shotRate = shotRate;
        _bulletSpawner = bulletSpawner;
        _switchAnimationAttack = switchAnimationAttack;

        _animator = _transform.GetComponent<Animator>();
        _spriteRenderer = _transform.GetComponentInChildren<SpriteRenderer>();
    }

    /// <summary>
    /// This function is responsible for evaluating the state of the node, executing the task he is associate with.
    /// </summary>
    /// <returns>A BehaviorTree <c>NodeState</c> instance representing the state of the node.</returns>
    public override NodeState Evaluate()
    {
        GameObject currentTarget = null;
        if (!TryGetData<GameObject>("target", out currentTarget))
        {
            currentTarget = (GameObject)GetData("tempTarget");
        }

        if (currentTarget.transform.position.x < _transform.position.x)
        {
            _spriteRenderer.flipX = true;
        }
        else
        {
            _spriteRenderer.flipX = false;
        }

        _shotTimeCounter -= Time.deltaTime;

        if (_shotTimeCounter <= 0)
        {
            _shotTimeCounter = 1f / _shotRate;

            TriggerAnimationAttack();

            // Rotate enemy crosshair to player
            float angle = 0.0f;
            if (currentTarget.TryGetComponent<Rigidbody2D>(out Rigidbody2D rigidbody))
            {
                Vector2 predictedTargetPosition = (Vector2)currentTarget.transform.position + rigidbody.velocity * 0.5f;
                angle = Mathf.Atan2((predictedTargetPosition.y - _transform.position.y), (predictedTargetPosition.x - _transform.transform.position.x)) * Mathf.Rad2Deg;
            }
            else
            {
                angle = Mathf.Atan2((currentTarget.transform.position.y - _transform.position.y), (currentTarget.transform.position.x - _transform.transform.position.x)) * Mathf.Rad2Deg;
            }

            _gunTransform.rotation = Quaternion.Euler(0, 0, angle);

            _bulletSpawner.StartCoroutine(_bulletSpawner.SpawnBulletBursts(_gunTransform.rotation.eulerAngles.z, currentTarget.transform));

            AudioManager.Instance.PlaySFX(4);
        }

        state = NodeState.Running;
        return state;
    }

    #endregion

    #region Private

    /// <summary>
    /// This function is responsible for triggering the attack animation.
    /// </summary>
    private void TriggerAnimationAttack()
    {
        bool isMoving = GetData<bool>("isMoving");

        if (_switchAnimationAttack)
        {

            if (_flip)
            {
                if (isMoving)
                {
                    _animator.SetTrigger("doShoot");
                }
                else
                {
                    _animator.SetTrigger("doShootStatic");
                }

                _flip = false;
            }
            else
            {
                _flip = true;
            }
        }
        else
        {
            if (isMoving)
            {
                _animator.SetTrigger("doShoot");
            }
            else
            {
                _animator.SetTrigger("doShootStatic");
            }
        }
    }

    #endregion
}