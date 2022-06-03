using UnityEngine;

/// <summary>
/// TODO: add comment.
/// </summary>
public class WeaponChest : MonoBehaviour
{
    public WeaponPickup[] potentialWeapon;

    private SpriteRenderer _spriteRenderer;

    public Sprite chestOpen;

    public GameObject notificationPopUp;

    private bool canOpen;

    private bool isOpen;

    public Transform spawnPoint;

    public float scaleSpeed;

    /// <summary>
    /// Instance field <c>collisionBoxOffsetPosition</c> is a Unity <c>Vector2</c> representing the offset position vector of the center of the collision box of the buy item game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxOffsetPosition;

    /// <summary>
    /// Instance field <c>collisionBoxSize</c> is a Unity <c>Vector2</c> representing the size vector of the collision box of the buy item game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxSize;

    /// <summary>
    /// Instance field <c>layerMask</c> is a Unity <c>LayerMask</c> structure representing layer levels the buy item game object can collide with.
    /// </summary>
    [SerializeField]
    private LayerMask _layerMask;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize, 0f, _layerMask);
        if (hit && hit.CompareTag("Player"))
        {
            if (!isOpen)
            {
                notificationPopUp.SetActive(true);

                if (InputManager.Instance.InteractInput)
                {
                    int gunSelect = Random.Range(0, potentialWeapon.Length);

                    Instantiate(potentialWeapon[gunSelect], spawnPoint);

                    _spriteRenderer.sprite = chestOpen;

                    isOpen = true;

                    transform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
                }
            }
        }
        else
        {
            notificationPopUp.SetActive(false);
        }

        if (isOpen)
        {
            transform.localScale = Vector3.MoveTowards(transform.localScale, Vector3.one, scaleSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize);
    }
}
