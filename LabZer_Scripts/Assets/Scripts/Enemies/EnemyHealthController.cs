using UnityEngine;

/// <summary>
/// Class <c>EnemyHealthController</c> is a Unity script used to manage the general enemies health behavior.
/// </summary>
public class EnemyHealthController : MonoBehaviour
{
    #region Fields / Properties

    /// <summary>
    /// Instance field <c>health</c> represents health quantity of the enemy;
    /// </summary>
    [SerializeField]
    private int _health = 150;

    /// <summary>
    /// Instance property <c>Health</c> represents health quantity of the enemy;
    /// </summary>
    public int Health { get { return _health; } }

    /// <summary>
    /// Instance field <c>enemyBT</c> is a BehaviorTree <c>Tree</c> object representing the behavior tree of the enemy;
    /// </summary>
    private BehaviorTree.Tree _enemyBT;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the game object animator.
    /// </summary>
    private Animator _animator;

    private const float DAMAGE_COOLDOWN = 0.2f;

    private float _damageCoolDownCounter;

    #endregion

    #region MonoBehavior

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _enemyBT = GetComponent<BehaviorTree.Tree>();
        _animator = GetComponent<Animator>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        _damageCoolDownCounter -= Time.deltaTime;
    }

    #endregion

    #region Private

    /// <summary>
    /// This function is called when the enemy death animation start explosion.
    /// </summary>
    private void OnDeathAnimationExplosion()
    {
        AudioManager.Instance.PlaySFX(1);
    }

    #endregion

    #region Public

    /// <summary>
    /// This function is responsible for applying a given damage amount to the current enemy game object.
    /// </summary>
    public bool TakeDamage(int damage, bool doOverrideDamageCooldown = false)
    {
        if (_health > 0 && (_damageCoolDownCounter <= 0 || doOverrideDamageCooldown))
        {
            _health -= damage;

            _animator.SetTrigger("doHurt");

            AudioManager.Instance.PlaySFX(12);

            _damageCoolDownCounter = DAMAGE_COOLDOWN;

            return true;
        }

        return false;
    }

    #endregion
}