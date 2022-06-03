using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// TODO: add comment.
/// </summary>
public class WeaponBuyItem : MonoBehaviour, IShopItem
{
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
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private GameObject _purchasePopUp;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private Text _itemCostMessage;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    [SerializeField]
    private Weapon[] _potentialWeapons;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private int _itemCost;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private Weapon _weapon;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private SpriteRenderer _weaponSprite;

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    private bool _isActive;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    private void Awake()
    {
        _weaponSprite = GetComponent<SpriteRenderer>();

        int selectedWeapon = Random.Range(0, _potentialWeapons.Length);
        _weapon = _potentialWeapons[selectedWeapon];

        _weaponSprite.sprite = _weapon.weaponSprite;
        _itemCostMessage.text = _weapon.itemCost.ToString();
        _itemCost = _weapon.itemCost;

    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    private void Update()
    {
        Collider2D hit = Physics2D.OverlapBox((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize, 0f, _layerMask);
        if (hit && hit.CompareTag("Player"))
        {
            _purchasePopUp.SetActive(true);
            _isActive = true;

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
                if (LevelManager.Instance.currentCoins >= _itemCost)
                {
                    OnBuyItem();
                }
                else
                {
                    AudioManager.Instance.PlaySFX(16);
                }
            }
        }
        else
        {
            _purchasePopUp.SetActive(false);
            _isActive = false;
        }
    }

    /// <summary>
    /// Callback to draw gizmos only if the object is selected.
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireCube((Vector2)transform.position + _collisionBoxOffsetPosition, _collisionBoxSize);
    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public void OnBuyItem()
    {
        LevelManager.Instance.SpendCoins(_itemCost);

        Weapon weaponClone = Instantiate(_weapon);
        weaponClone.transform.parent = PlayerController.Instance.gunTransform;
        weaponClone.transform.position = PlayerController.Instance.gunTransform.position;
        weaponClone.transform.localRotation = Quaternion.identity;
        weaponClone.transform.localScale = Vector3.one;

        AudioManager.Instance.PlaySFX(19);

        PlayerController.Instance.EquipWeapon(weaponClone);

        Destroy(gameObject);
    }

    /// <summary>
    /// TODO: add comment.
    /// </summary>
    public bool isActive()
    {
        return _isActive;
    }
}
