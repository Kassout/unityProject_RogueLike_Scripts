using BehaviorTree;
using BulletEngine;
using UnityEngine;

/// <summary>
/// Class <c>TaskFireRight</c> is a BehaviorTree <c>Node</c> used to define a fire right direction task behavior.
/// </summary>
public class TaskFireRight : Node
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
    /// Instance field <c>firePoint</c> is a Unity <c>Transform</c> component representing the position, rotation and scale of the fire point game object.
    /// </summary>
    private Transform _firePoint;

    /// <summary>
    /// Instance field <c>bulletSpawner</c> is a BulletEngine <c>BulletSpawner</c> component representing the bullet spawner manager of the game object.
    /// </summary>
    private BulletSpawner _bulletSpawner;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the game object animator.
    /// </summary>
    private Animator _animator;

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
    public TaskFireRight(Transform transform, Transform firePoint, float shotRate, BulletSpawner bulletSpawner, bool switchAnimationAttack)
    {
        _firePoint = firePoint;
        _shotRate = shotRate;
        _bulletSpawner = bulletSpawner;
        _switchAnimationAttack = switchAnimationAttack;

        _animator = transform.GetComponent<Animator>();
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

        _shotTimeCounter -= Time.deltaTime;

        if (_shotTimeCounter <= 0)
        {
            _shotTimeCounter = 1f / _shotRate;

            TriggerAnimationAttack();

            _bulletSpawner.SpawnBullets(_firePoint.rotation.eulerAngles.z, currentTarget.transform);

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