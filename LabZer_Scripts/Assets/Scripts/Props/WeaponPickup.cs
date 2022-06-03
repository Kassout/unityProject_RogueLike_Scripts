using UnityEngine;

/// <summary>
/// TODO: add comment.
/// </summary>
public class WeaponPickup : MonoBehaviour
{
    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public GameObject weaponPrefab;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private Weapon _weapon;

    /// <summary>
    /// Instance field <c>waitToBeCollected</c> represents duration value before the game object is available to picks up.
    /// </summary>
    [SerializeField]
    private float _waitToBeCollected = 0.5f;

    /// <summary>
    /// Instance field <c>collisionBoxOffsetPosition</c> is a Unity <c>Vector2</c> representing the offset position vector of the center of the collision box of the pick up game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxOffsetPosition;

    /// <summary>
    /// Instance field <c>collisionBoxSize</c> is a Unity <c>Vector2</c> representing the size vector of the collision box of the pick up game object.
    /// </summary>
    [SerializeField]
    private Vector2 _collisionBoxSize;

    /// <summary>
    /// Instance field <c>layerMask</c> is a Unity <c>LayerMask</c> structure representing layer levels the pick up game object can collide with.
    /// </summary>
    [SerializeField]
    private LayerMask _layerMask;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _weapon = weaponPrefab.GetComponent<Weapon>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        if (_waitToBeCollected > 0)
        {
            _waitToBeCollected -= Time.deltaTime;
        }

        if (_waitToBeCollected <= 0)
        {
            // Check for collisions
            Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize, 0f, _layerMask);
            if (hit && hit.CompareTag("Player"))
            {
                bool hasWeapon = false;
                foreach (Weapon weaponToCheck in PlayerController.Instance.availableWeapons)
                {
                    if (_weapon.weaponName.Equals(weaponToCheck.weaponName))
                    {
                        hasWeapon = true;
                    }
                }

                if (!hasWeapon && InputManager.Instance.InteractInput)
                {
                    Weapon weaponClone = Instantiate(_weapon);
                    weaponClone.transform.parent = PlayerController.Instance.gunTransform;
                    weaponClone.transform.position = PlayerController.Instance.gunTransform.position;
                    weaponClone.transform.localRotation = Quaternion.identity;
                    weaponClone.transform.localScale = Vector3.one;

                    PlayerController.Instance.EquipWeapon(weaponClone);

                    Destroy(gameObject);
                }
            }
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
