using UnityEngine;
using BulletEngine;
using System.Collections;
using System.Collections.Generic;

public class BossController : MonoBehaviour
{
    #region Singleton

    // Singleton
    public static BossController Instance { get; private set; }

    #endregion

    [SerializeField]
    private int _currentHealth;

    [SerializeField]
    private GameObject _levelExit;

    [SerializeField]
    private BossSequence[] _sequences;

    [SerializeField]
    private SpriteRenderer[] _spriteRenderers;

    private BossAction[] _actions;

    private int _currentAction;

    private float _actionCounter;

    private int _currentSequence;

    private Vector2 _moveDirection;

    private bool _isDirectionChosen;

    private int _pointToMoveIndex;

    private float[] _shotCounters = new float[5];

    private Rigidbody2D _rigidbody;

    private Animator _animator;

    private const float MOVE_SPEED_REF = 2f;

    private int _nextEndSequenceHealthLimit;

    private bool _canMove = true;

    [SerializeField]
    private Transform _roomCenter;

    [SerializeField]
    private float _deathExplosionNumber;

    [SerializeField]
    private GameObject _explosionEffectPrefab;

    [SerializeField]
    private List<Sprite> _healthBarVariant;

    private int _healthBarVariantIndex;

    [SerializeField]
    private Transform _playerStartPosition;

    private const float DAMAGE_COOLDOWN = 0.2f;

    private float _damageCoolDownCounter;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        // Singleton
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _rigidbody = GetComponent<Rigidbody2D>();
        _animator = GetComponent<Animator>();
        _nextEndSequenceHealthLimit = FindNextEndSequenceHealthLimit();
    }

    /// <summary>
    /// This function is called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {
        StartCoroutine(IntroductionSequence());
    }

    private IEnumerator IntroductionSequence()
    {
        _canMove = false;
        PlayerController.Instance.canMove = false;

        AudioManager.Instance.PlaySFX(37);

        while (Vector3.Distance(PlayerController.Instance.transform.position, _playerStartPosition.position) > 0.5f)
        {
            PlayerController.Instance.transform.position = Vector3.MoveTowards(PlayerController.Instance.transform.position, _playerStartPosition.position, 2f * Time.deltaTime);
            PlayerController.Instance.animator.SetBool("isMoving", true);

            yield return null;
        }

        PlayerController.Instance.animator.SetBool("isMoving", false);
        _animator.SetTrigger("doTalk");

        yield return new WaitForSeconds(1.5f);

        AudioManager.Instance.PlayBossMusic();

        UIController.Instance.bossHealthBar.gameObject.SetActive(true);

        _canMove = true;
        PlayerController.Instance.canMove = true;
    }

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        UIController.Instance.bossHealthBar.maxValue = _currentHealth;
        UIController.Instance.bossHealthBar.value = _currentHealth;

        _actions = SelectRandomNextActionSequence(_currentSequence);

        _actionCounter = _actions[_currentAction].actionLength;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        _damageCoolDownCounter -= Time.deltaTime;

        if (_canMove)
        {
            if (_actionCounter > 0)
            {
                _actionCounter -= Time.deltaTime;

                // Handle Invicibility
                if (_actions[_currentAction].isInvicible)
                {
                    _animator.SetBool("isInvicible", true);
                }

                // Handle movement
                ComputeMovement();

                // Handle shooting
                ComputeShooting();
            }
            else
            {
                ComputeNextAction();
            }
        }
    }

    public bool TakeDamage(int damageAmount, bool doOverrideDamageCooldown = false)
    {
        if (!_actions[_currentAction].isInvicible && _canMove && (_damageCoolDownCounter <= 0 || doOverrideDamageCooldown))
        {
            _currentHealth -= damageAmount;

            if (_currentHealth <= 0)
            {
                _canMove = false;
                _animator.Rebind();
                _animator.Update(0f);

                StartCoroutine(BossDeathSequence());
            }
            else
            {
                _animator.SetTrigger("doHurt");

                if (_currentHealth <= _nextEndSequenceHealthLimit)
                {
                    _nextEndSequenceHealthLimit = FindNextEndSequenceHealthLimit();

                    _actions = SelectRandomNextActionSequence(_currentSequence);

                    _currentAction = 0;

                    _actionCounter = _actions[_currentAction].actionLength;
                }
            }

            UIController.Instance.bossHealthBar.value = _currentHealth;

            _damageCoolDownCounter = DAMAGE_COOLDOWN;

            return true;
        }

        return false;
    }

    private IEnumerator BossDeathSequence()
    {
        AudioManager.Instance.StopMusic();
        AudioManager.Instance.PlaySFX(36);

        for (int i = 0; i < _deathExplosionNumber; i++)
        {
            Transform explosionEffectTransform = ObjectPooler.Instance.GetObjectFromPool(_explosionEffectPrefab, _roomCenter.position + new Vector3(Random.Range(-7f, 7f), Random.Range(-3f, 3f), 0f), Quaternion.identity);
            if (explosionEffectTransform != null)
            {
                explosionEffectTransform.gameObject.SetActive(true);
            }

            AudioManager.Instance.PlaySFX(31);

            yield return new WaitForSeconds(0.1f);
        }

        _animator.SetTrigger("doDeath");
        UIController.Instance.bossHealthBar.gameObject.SetActive(false);

        yield return null;
    }

    private void OnBossDeath()
    {
        if (Vector3.Distance(PlayerController.Instance.transform.position, _levelExit.transform.position) < 2f)
        {
            _levelExit.transform.position += new Vector3(4f, 0f, 0f);
        }

        Instantiate(_levelExit, _roomCenter.position + _levelExit.transform.position, Quaternion.identity);

        _levelExit.SetActive(true);
    }

    private int FindNextEndSequenceHealthLimit()
    {
        int result = 0;
        foreach (BossSequence sequence in _sequences)
        {
            if (result < sequence.endSequenceHealth && sequence.endSequenceHealth < _currentHealth)
            {
                result = sequence.endSequenceHealth;
            }
        }

        UIController.Instance.bossHealthFiller.sprite = _healthBarVariant[_healthBarVariantIndex];
        _healthBarVariantIndex++;

        return result;
    }

    private BossAction[] SelectRandomNextActionSequence(int currentSequenceIndex)
    {
        do
        {
            _currentSequence = Random.Range(0, _sequences.Length);
        }
        while (_currentSequence == currentSequenceIndex || _sequences[_currentSequence].endSequenceHealth != _nextEndSequenceHealthLimit);

        return _sequences[_currentSequence].actions;
    }

    private void ComputeMovement()
    {
        _moveDirection = Vector2.zero;

        if (_actions[_currentAction].shouldMove)
        {
            if (_actions[_currentAction].shouldChasePlayer)
            {
                _moveDirection = PlayerController.Instance.transform.position - transform.position;
                _moveDirection.Normalize();
            }

            if (_actions[_currentAction].moveToPoints)
            {
                if (_actions[_currentAction].pointToMoveTo.Length > 1 && !_isDirectionChosen)
                {
                    _pointToMoveIndex = Random.Range(0, _actions[_currentAction].pointToMoveTo.Length);
                    _isDirectionChosen = true;
                    _moveDirection = _actions[_currentAction].pointToMoveTo[_pointToMoveIndex].position - transform.position;
                    _moveDirection.Normalize();
                }
                else if (Vector2.Distance(transform.position, _actions[_currentAction].pointToMoveTo[0].position) > 0.25f)
                {
                    _moveDirection = _actions[_currentAction].pointToMoveTo[0].position - transform.position;
                    _moveDirection.Normalize();
                }
            }

            if (_moveDirection.x < 0)
            {
                transform.GetChild(0).localScale = new Vector3(-1f, 1f, 1f);
            }
            else
            {
                transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
            }
        }
        else
        {
            if (transform.position.x < PlayerController.Instance.transform.position.x)
            {
                transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f);
            }
            else
            {
                transform.GetChild(0).localScale = new Vector3(-1f, 1f, 1f);
            }
        }

        _rigidbody.velocity = _moveDirection * _actions[_currentAction].moveSpeed;

        if (_rigidbody.velocity.magnitude > 0.1f)
        {
            _animator.SetFloat("moveSpeed", _actions[_currentAction].moveSpeed / MOVE_SPEED_REF);
            _animator.SetBool("isMoving", true);
        }
        else
        {
            _animator.SetBool("isMoving", false);
        }
    }

    private void ComputeShooting()
    {
        if (_actions[_currentAction].shouldShoot)
        {
            for (int i = 0; i < _actions[_currentAction].shootingSystems.Length; i++)
            {
                _shotCounters[i] -= Time.deltaTime;

                if (_shotCounters[i] <= 0)
                {
                    _shotCounters[i] = 1f / _actions[_currentAction].shootingSystems[i].shotRate;

                    BulletSpawner bulletSpawner = _actions[_currentAction].shootingSystems[i].shotSource;
                    bulletSpawner.gameObject.SetActive(true);

                    if (_actions[_currentAction].shootingSystems[i].shouldRotateTowardPlayer)
                    {
                        // Rotate enemy crosshair to player
                        float angle = Mathf.Atan2((PlayerController.Instance.transform.position.y - transform.position.y), (PlayerController.Instance.transform.position.x - transform.transform.position.x)) * Mathf.Rad2Deg;

                        bulletSpawner.transform.rotation = Quaternion.Euler(0, 0, angle);
                    }

                    bulletSpawner.SpawnBullets(bulletSpawner.transform.rotation.eulerAngles.z, PlayerController.Instance.transform);
                    AudioManager.Instance.PlaySFX(_actions[_currentAction].shootingSystems[i].SFXIndex);

                    if (_actions[_currentAction].shootingSystems[i].animationTriggerTag != "")
                    {
                        _animator.SetTrigger(_actions[_currentAction].shootingSystems[i].animationTriggerTag);
                    }
                }

                if (_actions[_currentAction].shootingSystems[i].shouldWeaponRotate)
                {
                    BulletSpawner bulletSpawner = _actions[_currentAction].shootingSystems[i].shotSource;
                    bulletSpawner.transform.Rotate(0f, 0f, _actions[_currentAction].shootingSystems[i].rotationSpeed * Time.deltaTime);
                }
            }
        }
    }

    private void ComputeNextAction()
    {
        _animator.SetBool("isInvicible", false);

        for (int i = 0; i < _actions[_currentAction].shootingSystems.Length; i++)
        {
            BulletSpawner bulletSpawner = _actions[_currentAction].shootingSystems[i].shotSource;
            bulletSpawner.gameObject.SetActive(false);
        }

        _currentAction++;

        if (_currentAction >= _actions.Length)
        {
            _actions = SelectRandomNextActionSequence(_currentSequence);
            _currentAction = 0;
        }

        _actionCounter = _actions[_currentAction].actionLength;
    }
}

[System.Serializable]
public class BossAction
{
    [Header("Action")]
    public float actionLength;

    public bool isInvicible;

    [Header("Movement")]
    public bool shouldMove;

    public bool shouldChasePlayer;

    public float moveSpeed;

    public bool moveToPoints;

    public Transform[] pointToMoveTo;

    [Header("Shooting")]
    public bool shouldShoot;

    public ShootingSystem[] shootingSystems;
}

[System.Serializable]
public class BossSequence
{
    [Header("Sequence")]
    public BossAction[] actions;

    public int endSequenceHealth;
}

[System.Serializable]
public class ShootingSystem
{
    public float shotRate;

    public BulletSpawner shotSource;

    public string animationTriggerTag;

    public bool shouldRotateTowardPlayer;

    public bool shouldWeaponRotate;

    public float rotationSpeed;

    public int SFXIndex;
}
