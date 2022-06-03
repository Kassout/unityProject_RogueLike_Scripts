using System.IO;
using UnityEngine;
using Grid = Pathfinding.Grid;

/// <summary>
/// Class <c>Breakable</c> is a Unity script used to manage the general breakable game objects behavior.
/// </summary>
public class Breakable : MonoBehaviour
{
    /// <summary>
    /// Instance field <c>shouldDropItem</c> represents the should drop item status of the breakable object.
    /// </summary>
    [SerializeField]
    private bool _shouldDropItem;

    /// <summary>
    /// Instance field <c>itemDropChance</c> represents the chance percentage value of item drops on game object destroyed.
    /// </summary>
    [SerializeField]
    private int _itemDropChance;

    /// <summary>
    /// Field <c>itemsToDrop</c> is an array of Unity <c>GameObject</c> representing the different items the breakable object can drop on destroyed.
    /// </summary>
    [SerializeField]
    private GameObject[] _itemsToDrop;

    [SerializeField]
    private int _breakSound;

    [SerializeField]
    private GameObject _explosionEffectPrefab;

    /// <summary>
    /// Instance field <c>animator</c> is a Unity <c>Animator</c> component representing the breakable object animator.
    /// </summary>
    private Animator _animator;

    private Grid _grid;

    private BoxCollider2D _collider;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    private void Start()
    {
        _animator = GetComponent<Animator>();
        _grid = GetComponentInParent<Grid>();
        _collider = GetComponent<BoxCollider2D>();
    }

    /// <summary>
    /// This function is responsible for destroying the breakable game object.
    /// </summary>
    public void DestroyProp()
    {
        _grid.OnObstacleDestroyed(_collider);
        _animator.SetTrigger("doDestruction");

        AudioManager.Instance.PlaySFX(_breakSound);

        Transform explosionEffectTransform = ObjectPooler.Instance.GetObjectFromPool(_explosionEffectPrefab, transform.position, Quaternion.identity);
        if (explosionEffectTransform != null)
        {
            explosionEffectTransform.gameObject.SetActive(true);
        }

        // Drop items
        if (_shouldDropItem)
        {
            float dropChance = Random.Range(0f, 100f);

            if (dropChance < _itemDropChance)
            {
                Instantiate(_itemsToDrop[Random.Range(0, _itemsToDrop.Length)], transform.position, transform.rotation);
            }
        }

        GetComponent<Collider2D>().enabled = false;
    }

    /// <summary>
    /// OnBecameInvisible is called when the renderer is no longer visible by any camera.
    /// </summary>
    private void OnBecameInvisible()
    {
        _animator.enabled = false;
    }

    /// <summary>
    /// OnBecameVisible is called when the renderer became visible by any camera.
    /// </summary>
    private void OnBecameVisible()
    {
        _animator.enabled = true;
    }
}
